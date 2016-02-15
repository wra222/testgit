using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.DB;
using IMES.Service.Common;
using System.Reflection;
using log4net;
using System.Transactions;
using System.Data;
using IMES.Service.MO.SAPMOConfirmWS;
using IMES.Service.MO.SAPMOConfirmChangeWS;
using System.Configuration;

namespace IMES.Service.MO
{
    public class Execute
    {

        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static string editor = "FIS";
         
         public static void Process(string isSync, string isParent)
        {
            bool running = false;
            const string TxnName="ConfirmMO";
            const string ProgramName="MO-CONFIRM-RUN";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
             try
             {
                
                 BaseLog.LoggingInfo(logger, "SYNC Mode: {0}, isParentMO:{1}", isSync,isParent);
                 using (TransactionScope txn = UTL.CreateDbTxn())
                 {
                     bool bRun = SQL.CheckConfirmMORunning(ProgramName);
                     if (bRun)
                     {
                         txn.Complete();
                         throw new Exception("The ConfirmMO Program is running");
                     }
                     SQL.UpdateConfirmMORunning(ProgramName, "T");
                     txn.Complete();
                     running = true;
                 }

                 #region 1. get MO List by 'Complete' status and rework flag =N in ProductMO
                 List<CompleteMO> completeMOList = SQL.GetCompleteProductMO(EnumProductMOState.Complete, "N");
                 #endregion

                 foreach (CompleteMO mo in completeMOList)
                 {
                     if (mo.IsParentMO == isParent || string.IsNullOrEmpty(isParent))
                     {
                         #region by MO 傳送給SAP Confirm data

                         List<string> productIdList = new List<string>();
                         string errorDescr = "";
                         List<string> errorMsg = new List<string>();
                         string txnId = UTL.GenTxnId();
                         string moId = mo.MOId;
                         BaseLog.LoggingInfo(logger, "Caculate MO:{0} TxnId:{1}  assembly componet quantity start....", moId, txnId);

                         #region   2.統計&產生SAP confirm MO  資料格式
                         int ret = SQL.CaculateConfirmMO(moId, txnId, mo.IsParentMO, out productIdList, out errorDescr);
                         #endregion

                         if (ret < 0) // 統計失敗
                         {
                             #region   2.1 統計失敗
                             BaseLog.LoggingError(logger, "Caculate MO:{0} ParentMO:{1} TxnId:{2} Error Code:{3} Error Description:{4}",
                                                                moId, mo.IsParentMO, txnId, ret.ToString(), errorDescr);

                             SQL.InsertSendDataAndLog(EnumMsgCategory.Send,
                                                                TxnName,
                                                                moId,
                                                                "",
                                                                txnId,
                                                                "",
                                                                errorDescr,
                                                                EnumMsgState.Fail,
                                                                DateTime.Now);
                             //Send e-mail
                             UTL.SendMail("Confirm MO 統計扣帳失敗",
                                                     string.Format("Caculate MO:{0} ParentMO:{1} TxnId:{2} Error Code:{3} Error Description:{4}",
                                                                moId, mo.IsParentMO, txnId, ret.ToString(), errorDescr));
                             #endregion
                         }
                         else // 統計成功
                         {
                             if (ret == 0)
                             {
                                 #region  2.5 Check return confirm Qty == 0 then don't send confirm Data to SAP

                                 using (TransactionScope txn = UTL.CreateDbTxn())
                                 {
                                     DateTime now = DateTime.Now;

                                     //For state is complete
                                     SQL.UpdateProductMOStatusAndInsertLogByComplete(TxnName,
                                                                                     "ChangeStatus",
                                                                                     productIdList,
                                                                                     EnumProductMOState.Confirm,
                                                                                     txnId,
                                                                                     "NA",
                                                                                     now,
                                                                                     editor,
                                                                                     EnumProductMOState.Complete,
                                                                                     moId);

                                     //For state is close
                                     SQL.UpdateProductMOStatusAndInsertLogByComplete(TxnName,
                                                                                     "ChangeStatus",
                                                                                     productIdList,
                                                                                     EnumProductMOState.Close,
                                                                                     txnId,
                                                                                     "NA",
                                                                                     now,
                                                                                     editor,
                                                                                     EnumProductMOState.Confirm,
                                                                                     moId);

                                     //for state is not Confirm State
                                     SQL.UpdateProductMOReworkStatusAndInsertLog(TxnName,
                                                                                 "Rework",
                                                                                 productIdList,
                                                                                 txnId,
                                                                                 "NA",
                                                                                 now,
                                                                                 editor,
                                                                                 EnumProductMOState.Close,
                                                                                 moId);

                                     // 10.最後一行 commit 
                                     txn.Complete();
                                 }



                                 #endregion
                             }
                             else
                             {
                                 #region  3. get ConfirmMO & ConfirmMaterial table by moId, txnId
                                 int confirmMOID = 0;
                                 ZMO_MES_CF_H moHeader = BuildSAPConfirmMOHeader(moId, txnId, isSync, out confirmMOID);
                                 List<ZMO_MES_CF_I> moItemList = BuildSAPConfirmMOItem(moId, txnId, confirmMOID);

                                 #endregion

                                 #region 4.Insert SendData  &TxnDataLog  table
                                 using (TransactionScope txn = UTL.CreateDbTxn())
                                 {
                                     SQL.InsertSendDataAndLog(EnumMsgCategory.Send,
                                                              TxnName,
                                                              moId,
                                                              "",
                                                              txnId,
                                                              EnumMsgState.Sending,
                                                              DateTime.Now);
                                     txn.Complete();
                                 }
                                 #endregion

                                 # region  4.1 Check SAP MO Status is TECO , if yes,  need revoke TECO
                                 if (CheckSAPClose(moId))
                                 {
                                     string action = "ChangeMOStatus";
                                     string errorText = "";
                                     if (SAPClient.SentChangeMOStatus(moId, out errorText))
                                     {
                                         SQL.InsertSendDataAndLog(EnumMsgCategory.Receive,
                                                                                        action,
                                                                                        moId,
                                                                                        "",
                                                                                        txnId,
                                                                                        "",
                                                                                        "",
                                                                                        EnumMsgState.Success,
                                                                                        DateTime.Now);
                                     }
                                     else
                                     {
                                         SQL.InsertSendDataAndLog(EnumMsgCategory.Receive,
                                                                                        action,
                                                                                        moId,
                                                                                        "",
                                                                                        txnId,
                                                                                        "",
                                                                                        errorText,
                                                                                        EnumMsgState.Fail,
                                                                                        DateTime.Now);
                                     }
                                 }

                                 #endregion

                                 #region  5.Request SAP MOConfirm Web service

                                 if (SAPClient.SentConfirmMO(moHeader, moItemList.ToArray(), out errorMsg))
                                 {
                                     #region request SAP webservice Success & update SendData/ProductMO Confirm Status & insert Log
                                     using (TransactionScope txn = UTL.CreateDbTxn())
                                     {
                                         DateTime now = DateTime.Now;

                                         //6. update SendData Success state 
                                         //7.insert TxnDataLog
                                         SQL.UpdateSendDataAndInsertLog(EnumMsgCategory.Receive,
                                                                        TxnName,
                                                                        moId,
                                                                        "",
                                                                        txnId,
                                                                        "",
                                                                        "",
                                                                        EnumMsgState.Success,
                                                                        now);

                                         //8. update productMO Confirm Status & LastTxnId
                                         //9. insert productMOLog
                                         //For state is complete
                                         SQL.UpdateProductMOStatusAndInsertLogByComplete(TxnName,
                                                                                         "ChangeStatus",
                                                                                         productIdList,
                                                                                         EnumProductMOState.Confirm,
                                                                                         txnId,
                                                                                         "NA",
                                                                                         now,
                                                                                         editor,
                                                                                         EnumProductMOState.Complete,
                                                                                         moId);
                                         //for state is not Confirm State
                                         SQL.UpdateProductMOReworkStatusAndInsertLog(TxnName,
                                                                                     "Rework",
                                                                                     productIdList,
                                                                                     txnId,
                                                                                     "NA",
                                                                                     now,
                                                                                     editor,
                                                                                     EnumProductMOState.Confirm,
                                                                                     moId);
                                         //if (isSync == "X")
                                         //{
                                         //    MoConfirmChange MCC = new MoConfirmChange();
                                         //    MCC.SerialNumber = txnId;
                                         //    MCC.MoNumber = moId;
                                         //    MCC.Result = "T"; // T/F                                
                                         //    string Message = SQL.UpdateStatus(MCC);
                                         //    BaseLog.LoggingInfo(logger, Message);
                                         //}
                                         // 10.最後一行 commit 
                                         txn.Complete();
                                     }
                                     #endregion
                                 }
                                 else  //Send SAP fail Case
                                 {
                                     #region request SAP Web service Fail
                                     errorDescr = string.Join(";", errorMsg.ToArray());

                                     BaseLog.LoggingInfo(logger, errorDescr);
                                     //update SendData Fail state & insert TxnDataLog
                                     SQL.UpdateSendDataAndInsertLog(EnumMsgCategory.Send,
                                                                    TxnName,
                                                                    moId,
                                                                    "",
                                                                    txnId,
                                                                    "",
                                                                    errorDescr,
                                                                    EnumMsgState.Fail,
                                                                    DateTime.Now);
                                     //send e-mail ....
                                     UTL.SendMail("Request SAP MOConfirm Fail ",
                                                 string.Format("MO:{0} TxnId:{1} Error Description:{2}",
                                                                    moId, txnId, errorDescr));
                                     #endregion
                                 }
                                 #endregion
                             }

                         }
                         #endregion by MO 傳送給SAP Confirm data
                     }
                 }
             } 
             catch(Exception e)
             {
                 BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                 //send e-mail
                 UTL.SendMail("Confirm MO 異常",e.Message);
                 throw e;
             } 
             finally 
             {
                if (running)
                    SQL.UpdateConfirmMORunning(ProgramName, "F");

                #region  Request SAP SubmitConfirm Web service

                string action = "SubmitConfirm";
                string TxnId = UTL.GenTxnId();
                string errorText = "";
                SAPClient.SentSubmitConfirm(TxnId, out errorText);
                #endregion

                BaseLog.LoggingEnd(logger, methodName);
             }
             
        }

         public static void ProcessRewok()
         {
             bool running =false;
             const string TxnName = "ConfirmMOChange";
             const string ProgramName = "MO-CONFIRM-REWORK-RUN";
             string methodName = MethodBase.GetCurrentMethod().Name;
             BaseLog.LoggingBegin(logger, methodName);
             try
             {
                 using (TransactionScope txn = UTL.CreateDbTxn())
                 {
                     bool bRun = SQL.CheckConfirmMORunning(ProgramName);
                     if (bRun)
                     {
                         txn.Complete();
                         throw new Exception("The Adjust ConfirmMO Program is running");
                     }
                     SQL.UpdateConfirmMORunning(ProgramName, "T");
                     txn.Complete();
                     running = true;
                 }

                 #region 1. get MO List by 'Complete' status and rework flag =Y in ProductMO
                 List<CompleteMO> completeMOList = SQL.GetCompleteProductMO(EnumProductMOState.Complete, "Y");
                 #endregion

                 foreach (CompleteMO mo in completeMOList)
                 {
                     #region by MO 傳送給SAP Confirm data

                     List<string> productIdList = new List<string>();
                     string errorDescr = "";
                     List<string> errorMsg=new List<string>();                      
                     string txnId = UTL.GenTxnId();
                     string moId = mo.MOId;
                     BaseLog.LoggingInfo(logger, "Caculate Rework MO:{0} TxnId:{1}  assembly componet quantity start....", moId, txnId);

                     #region   2.統計&產生SAP Reworkconfirm MO  資料格式
                     int ret = SQL.CaculateConfirmChangeMO(moId, txnId, mo.IsParentMO, out productIdList, out errorDescr);
                     #endregion

                     if (ret < 0) // 統計失敗
                     {
                         BaseLog.LoggingError(logger, "Caculate Rework MO:{0} ParentMO:{1} TxnId:{2} Error Code:{3} Error Description:{4}",
                                                            moId, mo.IsParentMO, txnId, ret.ToString(), errorDescr);
                         //Send e-mail
                         UTL.SendMail("ConfirmRework MO 統計扣帳失敗",
                                                 string.Format("Caculate Rework MO:{0} ParentMO:{1} TxnId:{2} Error Code:{3} Error Description:{4}",
                                                            moId, mo.IsParentMO, txnId, ret.ToString(), errorDescr));
                     }
                     else if (ret == 0)
                     {
                         BaseLog.LoggingInfo(logger, "MO:{0} have no Change Component", mo);
                         #region ProductMO Confirm Status & insert Log
                         using (TransactionScope txn = UTL.CreateDbTxn())
                         {
                             DateTime now = DateTime.Now;                            

                             //8. update productMO Confirm Status & LastTxnId
                             //9. insert productMOLog
                             //For state is complete
                             SQL.UpdateProductMOStatusAndInsertLogByComplete(TxnName,
                                                                             "ChangeStatus",
                                                                             productIdList,
                                                                             EnumProductMOState.Confirm,
                                                                             txnId,
                                                                             "NA",
                                                                             now,
                                                                             editor,
                                                                             EnumProductMOState.Complete,
                                                                             moId);
                             //for state is not confirm state
                             SQL.UpdateProductMOReworkStatusAndInsertLog(TxnName,
                                                                         "Rework",
                                                                         productIdList,
                                                                         txnId,
                                                                         "NA",
                                                                         now,
                                                                         editor,
                                                                         EnumProductMOState.Confirm,
                                                                         moId);

                             MoConfirmChange MCC = new MoConfirmChange();
                             MCC.SerialNumber = txnId;
                             MCC.MoNumber = moId;
                             MCC.Result = "T"; // T/F                                
                             string Message = SQL.UpdateStatus(MCC);
                             BaseLog.LoggingInfo(logger, Message);

                             // 10.最後一行 commit 
                             txn.Complete();
                         }
                         #endregion
                     }
                     else // 統計成功
                     {

                         #region  3. get ConfirmReworkMO & ConfirmReworkMaterial table by moId, txnId
                         int ConfirmReworkMOID = 0;

                         ZMO_MES_261_H moHeader = BuildSAPConfirmReworkMOHeader(moId, txnId, out ConfirmReworkMOID);
                         List<ZMO_MES_261_I> moItemList = BuildSAPConfirmReworkMOItem(moId, txnId, ConfirmReworkMOID);

                         #endregion

                         #region 4.Insert SendData  &TxnDataLog  table
                         using (TransactionScope txn = UTL.CreateDbTxn())
                         {
                             SQL.InsertSendDataAndLog(EnumMsgCategory.Send,
                                                      TxnName,
                                                      moId,
                                                      "",
                                                      txnId,
                                                      EnumMsgState.Sending,
                                                      DateTime.Now);
                             txn.Complete();
                         }
                         #endregion

                         #region  5.Request SAP MOReworkConfirm Web service
                         if (SAPClient.SentConfirmChangeMO(moHeader, moItemList.ToArray(), out errorMsg))
                         {
                             #region request SAP webservice Success & update SendData/ProductMO Confirm Status & insert Log
                             using (TransactionScope txn = UTL.CreateDbTxn())
                             {
                                 DateTime now = DateTime.Now;

                                 //6. update SendData Success state 
                                 //7.insert TxnDataLog
                                 SQL.UpdateSendDataAndInsertLog(EnumMsgCategory.Receive,
                                                                TxnName,
                                                                moId,
                                                                "",
                                                                txnId,
                                                                "",
                                                                "",
                                                                EnumMsgState.Success,
                                                                now);

                                 //8. update productMO Confirm Status & LastTxnId
                                 //9. insert productMOLog
                                 //For state is complete                                     
                                 SQL.UpdateProductMOStatusAndInsertLogByComplete(TxnName,
                                                                                 "ChangeStatus",
                                                                                 productIdList,
                                                                                 EnumProductMOState.Confirm,
                                                                                 txnId,
                                                                                 "NA",
                                                                                 now,
                                                                                 editor,
                                                                                 EnumProductMOState.Complete,
                                                                                 moId);
                                 //for state is not Confirm State
                                 SQL.UpdateProductMOReworkStatusAndInsertLog(TxnName,
                                                                             "Rework",
                                                                             productIdList,
                                                                             txnId,
                                                                             "NA",
                                                                             now,
                                                                             editor,
                                                                             EnumProductMOState.Confirm,
                                                                             moId);

                                 MoConfirmChange MCC = new MoConfirmChange();
                                 MCC.SerialNumber = txnId;
                                 MCC.MoNumber = moId;
                                 MCC.Result = "T"; // T/F                                
                                 string Message = SQL.UpdateStatus(MCC);
                                 BaseLog.LoggingInfo(logger, Message);

                                 // 10.最後一行 commit 
                                 txn.Complete();
                             }
                             #endregion
                         }
                         else  //Send SAP fail Case
                         {
                             #region request SAP Web service Fail

                             errorDescr=string.Join(";", errorMsg.ToArray());
                             BaseLog.LoggingInfo(logger, errorDescr);
                             //update SendData Fail state & insert TxnDataLog
                             SQL.UpdateSendDataAndInsertLog(EnumMsgCategory.Receive,
                                                            TxnName,
                                                            moId,
                                                            "",
                                                            txnId,
                                                            "",
                                                            errorDescr,
                                                            EnumMsgState.Fail,
                                                            DateTime.Now);
                             //send e-mail ....
                             UTL.SendMail("Request SAP MOConfirmReowrk Fail ",
                                         string.Format("MO:{0} TxnId:{1} Error Description:{2}",
                                                            moId, txnId, errorDescr));
                             #endregion
                         }
                         #endregion


                     }
                     #endregion by MO 傳送給SAP Confirm data
                 }
             }
             catch (Exception e)
             {
                 BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                 //throw ;
                 //send e-mail
                 UTL.SendMail("ConfirmRework MO 異常", e.Message);
                 throw e;
             }
             finally
             {
                 if (running)
                    SQL.UpdateConfirmMORunning(ProgramName, "F");
                 BaseLog.LoggingEnd(logger, methodName);
             }
         }


        private static  ZMO_MES_CF_H BuildSAPConfirmMOHeader(string moId, string txnId, string isSync, out int confirmMoId)
        {
            ZMO_MES_CF_H header = new ZMO_MES_CF_H();
            confirmMoId = -1;
            DataTable dt = SQL.GetConfirmMO(moId, txnId);
            confirmMoId = (int)dt.Rows[0]["ID"];
            
            //mapping DataTable to SAP MoConfirmHeader Data structure            
            header.MONUMBER = dt.Rows[0]["MO"].ToString().Trim();//MoNumber
            header.SERIALNUMBER = dt.Rows[0]["TxnId"].ToString().Trim();//SerialNumber
            header.MOTYPE=dt.Rows[0]["MOType"].ToString().Trim();//MoType
            header.BUILDOUTMTL = dt.Rows[0]["Model"].ToString().Trim();//BuildOutMtl
            header.DELIVEREDQTY=dt.Rows[0]["DeliveredQty"].ToString().Trim();//DeliveredQty
            header.UNIT=dt.Rows[0]["Unit"].ToString().Trim();//Unit            
            header.CONFIRMDATE=string.Format("{0:yyyyMMdd}",(DateTime)dt.Rows[0]["ConfirmDate"]);//ConfirmDate
            header.CONFIRMTIME = string.Format("{0:HHmmss}", (DateTime)dt.Rows[0]["ConfirmDate"]);//ConfirmTime
            header.REMARK1 = "";//Remark1
            header.REMARK2 = "";//Remark2
            header.SYN = isSync;

            return header;
        }

        private static List<ZMO_MES_CF_I> BuildSAPConfirmMOItem(string moId,string txnId,int confirmMoId)
        {
            List<ZMO_MES_CF_I> ItemList = new List<ZMO_MES_CF_I>();
            DataTable dt = SQL.GetConfirmMOMaterial(confirmMoId);

            //mapping DataTable to SAP MoConfirmItem Data structure            
            foreach (DataRow dr in dt.Rows)
            {
                ZMO_MES_CF_I Item =new ZMO_MES_CF_I();
                Item.MONUMBER = moId;//MoNumber
                Item.SERIALNUMBER = txnId;//SerialNumber
                Item.MOITEM = UTL.GetDtString(dr, "MOItem");//MoItem
                Item.RESERVATION = UTL.GetDtString(dr, "Reservation");//Reservation
                Item.RESVITEM = UTL.GetDtString(dr, "ResvItem");//ResvItem
                Item.COMPONENT = UTL.GetDtString(dr, "PartNo");//Component
                Item.WITHDRAWQTY = UTL.GetDtString(dr, "WithdrawQty");//WithdrawQty
                Item.UNIT = UTL.GetDtString(dr, "Unit");//Unit
                Item.ALTGROUP = UTL.GetDtString(dr, "AltGroup");//AltGroup
                Item.PARENTMATERIAL = UTL.GetDtString(dr, "ParentMaterial");//ParentMaterial
                Item.REMARK1 = "";//Remark1
                Item.REMARK2 = "";//Remark2
                
                ItemList.Add(Item);
            }
            
            return ItemList;
        }


        private static ZMO_MES_261_H BuildSAPConfirmReworkMOHeader(string moId, string txnId, out int ConfirmReworkMoId)
        {
            ZMO_MES_261_H header = new ZMO_MES_261_H();
            ConfirmReworkMoId = -1;
            DataTable dt = SQL.GetConfirmReworkMO(moId, txnId);
            ConfirmReworkMoId = (int)dt.Rows[0]["ID"];

            //mapping DataTable to SAP MoConfirmReworkHeader Data structure            
            header.MONUMBER = dt.Rows[0]["MO"].ToString().Trim();//MoNumber
            header.SERIALNUMBER = dt.Rows[0]["TxnId"].ToString().Trim();//SerialNumber            
            header.PLANT = dt.Rows[0]["Plant"].ToString().Trim();//Plant            
            header.PRDVER = dt.Rows[0]["ProductVer"].ToString().Trim();//ProductionVer

            
            return header;
        }

        private static List<ZMO_MES_261_I> BuildSAPConfirmReworkMOItem(string moId, string txnId, int ConfirmReworkMoId)
        {
            List<ZMO_MES_261_I> ItemList = new List<ZMO_MES_261_I>();
            DataTable dt = SQL.GetConfirmReworkMaterial(ConfirmReworkMoId);

            //mapping DataTable to SAP MoConfirmReworkItem Data structure            
            foreach (DataRow dr in dt.Rows)
            {
                ZMO_MES_261_I Item = new ZMO_MES_261_I();
                Item.MONUMBER = moId;//MoNumber
                Item.SERIALNUMBER = txnId;//SerialNumber                
                Item.COMPONENT = UTL.GetDtString(dr, "PartNo");//Component
                Item.WITHDRAWQTY = UTL.GetDtString(dr, "WithdrawQty");//WithdrawQty
                Item.UNIT = UTL.GetDtString(dr, "Unit");//Unit
                Item.MVT = UTL.GetDtString(dr, "Mvt");//mvt
                Item.REMARK1 = "";//REMARK1
                Item.REMARK2 = "";//REMARK2                                                
                ItemList.Add(Item);
            }

            return ItemList;
        }

        private static bool CheckSAPClose(string moId)
        {
            try
            {
                DataTable dt = SQL.GetMOStatus(moId);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SAPStatus"].ToString().Trim() == SAPMOStatus.Close)
                        return true;
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;                
            }
            
        }
    }

    public class SAPClient
    {
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static bool SentConfirmMO(ZMO_MES_CF_H header, ZMO_MES_CF_I[] itemList, out List<string> errorMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            errorMsg = new List<string>();
            try
            {               
                //request SAP web service                
                SAPMOConfirmWS.Z_PRODORDCONF_IMES_WSClient SAPClient = new SAPMOConfirmWS.Z_PRODORDCONF_IMES_WSClient("SAP_ConfirmWS");
                SAPClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();      
                //SAPClient.Z_PRODORDCONF_IMES(header,itemList,                
                ZMO_MES_CF_R[]  ItemResult = new ZMO_MES_CF_R[0];
                SAPClient.Z_PRODORDCONF_IMES(header,ref itemList,ref ItemResult);


                //IMES.Service.MO.SAPMOConfirmWS.ZMO_IME_CNF_R response = SAPClient.Z_PRODORDCONF_IMES((header, ref itemList);
                //check SAP web service response result & return true/false

                foreach (ZMO_MES_CF_R item in ItemResult)
                {
                    if (item.RESULTCNF.Trim() == "F")
                    {
                        errorMsg.Add("MO:" + item.MONUMBER + " Confirm fail");
                    }
                }

                //if (response.RESULTCNF == "T")
                //{
                  //  return true;
               // }

                //errorMsg = "ConfirmMO : SAP Response  Fail....";
                if (errorMsg.Count > 0)
                {
                    return false;
                }
                return true;  
                
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                //errorMsg = e.Message;               
                
                return false;
                //throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public static bool SentConfirmChangeMO(ZMO_MES_261_H header, ZMO_MES_261_I[] itemList,out List<string> errorMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            errorMsg = new List<string>();
            try
            {                
                //request SAP web service                                                  
                //SAPMOConfirmChangeWS.ZMO_IME_261_H SAPClient =new ZMO_IME_261_H();
                SAPMOConfirmChangeWS.Z_PRODORD261_IMES_BAPI_WSClient SAPClient = new SAPMOConfirmChangeWS.Z_PRODORD261_IMES_BAPI_WSClient("SAP_ConfirmChangeWS");
                SAPClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();
                ZMO_MES_261_R[] ItemResult = new ZMO_MES_261_R[0];
                SAPClient.Z_PRODORD261_IMES_BAPI(header, ref itemList, ref ItemResult);
                
                //check SAP web service response result & return true/false
                foreach (ZMO_MES_261_R item in ItemResult)
                {
                    if (item.RESULT261.Trim()== "F")
                    {
                        errorMsg.Add(item.ERRMSG);
                    }                   
                }

                if (errorMsg.Count>0)
                {
                    return false;
                }
                return true;                                
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
               // errorMsg = e.Message;
                return false;
                //throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public static bool SentChangeMOStatus(string moId, out string errorMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            errorMsg = "";
            try
            {
                //request SAP web service                                                  
                 SAPChangeMOStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WSClient SAPClient = new SAPChangeMOStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WSClient("SAP_ChangeMOStatusWS");
                SAPClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();
                
                SAPChangeMOStatusWS.ZMO_MES_TECO_I input = new IMES.Service.MO.SAPChangeMOStatusWS.ZMO_MES_TECO_I();

                input.MONUMBER = moId;
                input.DELIVEREDQTY = "0";
                input.STATUS = "REVOKETECO";
                string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
                logger.DebugFormat("MOStatusChange: \r\n{0}", ObjectTool.ObjectPropertyToString(input));
               SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                       "ChangeMOStatus",
                                                       moId,
                                                        "REVOKETECO",
                                                     TxnId,
                                                       "",
                                                       "",
                                                       EnumMsgState.Sending,
                                                       "");

                SAPChangeMOStatusWS.ZMO_MES_TECO_E resp=  SAPClient.Z_SET_PRODORD_TO_IMES_TECO(input);

                SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                        "ChangeMOStatus",
                                                        moId,
                                                         "REVOKETECO",
                                                         TxnId,
                                                         (resp.RESULT == null ? "" : resp.RESULT.Trim()),
                                                        (resp.ERRORMESSAGE == null ? "" : resp.ERRORMESSAGE.Trim()),
                                                        EnumMsgState.Received,
                                                        string.Format("SAPMOStatus:{0} WithdrawQty:{1}", resp.STATUS.Trim(), resp.DELIVEREDQTY.Trim()));
                logger.DebugFormat("MOStatusChangeResponse: \r\n{0}", ObjectTool.ObjectPropertyToString(resp));
                
               if (resp.RESULT.Trim() == "T")
               {
                   // update SAPMO Status
                   SQL.UpdateSAPMOStatus(moId, resp.STATUS.Trim(), Convert.ToInt32(float.Parse(resp.DELIVEREDQTY.Trim())));
                   return true;
               }
               else
               {
                   errorMsg = resp.ERRORMESSAGE.Trim();
                   return false;
               }
              
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorMsg = e.Message;
                return false;
                
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
        public static bool SentSubmitConfirm(string TxnId, out string errorMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            errorMsg = "";
            try
            {
                //request SAP web service
                SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WSClient SAPClient = new SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WSClient("SAP_SubmitConfirmWS");                                  
                SAPClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                //string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                //logger.DebugFormat("SubmitConfim: \r\n{0}", ObjectTool.ObjectPropertyToString(TxnId));
                SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                        "SubmitConfirm",
                                                        "",
                                                        "",
                                                      TxnId,
                                                        "",
                                                        "",
                                                        EnumMsgState.Sending,
                                                        "");

                SAPSubmitConfirmWS.ZMO_MES_CF_R1 resp = SAPClient.Z_PRODORDCONF_IMES_SUBMIT(TxnId);

                
                SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        "SubmitConfirm",
                                                        "",
                                                        "",
                                                        TxnId,
                                                        (resp.RESULTCNF == null ? "" : resp.RESULTCNF.Trim()),
                                                        (resp.ERRMSG == null ? "" : resp.ERRMSG.Trim()),
                                                        EnumMsgState.Received,
                                                        "");
                 
                logger.DebugFormat("SubmitConfirmResponse: \r\n{0}", ObjectTool.ObjectPropertyToString(resp));

                if (resp.RESULTCNF.Trim() == "T")
                {
                    return true;
                }
                else
                {
                    errorMsg = resp.ERRMSG.Trim();
                    return false;
                }

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorMsg = e.Message;
                return false;

            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
    }    
}
