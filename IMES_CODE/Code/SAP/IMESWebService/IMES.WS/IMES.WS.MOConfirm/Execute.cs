using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using log4net;
using System.Reflection;
using System.Transactions;
using IMES.WS.Common;
using System.Web.Configuration;
using IMES.Query.DB;
using System.Data;
using IMES.WS.SAPMOChangeStatusWS;

namespace IMES.WS.MOConfirm
{
    public enum SAPMOChangeStatusCmd
    {
        TECO,
        REVOKETECO,
        QUERY
    }

    public enum SAPMOStatus
    {
        REL,
        CNF,
        PCNF,
        TECO
    }

    public class Execute
    {

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(MoConfirmResult[] results)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"MoNumber",
                                                                                              "SerialNumber",
                                                                                               "Result"};

                foreach (MoConfirmResult item in results)
                {
                    logger.DebugFormat("MoConfirmResult: \r\n{0}", ObjectTool.ObjectTostring(item));
                }

                foreach (MoConfirmResult item in results)
                {
                    ObjectTool.CheckNullData(NotNullItemList, item);
                }

                //if (string.IsNullOrEmpty( result.MoNumber) )
                //{
                //    throw new Exception("This MoNumber is no data");
                //}

                //if (string.IsNullOrEmpty( result.SerialNumber))
                //{
                //    throw new Exception("This SerialNumber  is no data");
                //}

                //if (string.IsNullOrEmpty( result.Result))
                //{
                //    throw new Exception("This Result  is no data");
                //}


                        
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw e;
            }
            finally
            {
               BaseLog.LoggingEnd(logger, methodName);
            }                        
        }

        //2.檢查資料內容
        public static void CheckData(MoConfirmResult[] CFResults)
        {            
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                if (Common.SQL.CheckSentData(CFResults[0].SerialNumber, CFResults[0].MoNumber) == "N")
                {
                    { throw new Exception("SerialNumber :" + CFResults[0].SerialNumber + " or " + " MoNumber : " + CFResults[0].MoNumber + "  is not exists in IMES SendData table"); }
                }             
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        //3.執行DB insert 
        public static void Process(MoConfirmResult[] CFResults)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                MoConfirmResult CFResult = new MoConfirmResult();
                CFResult.MoNumber = CFResults[0].MoNumber;
                CFResult.SerialNumber= CFResults[0].SerialNumber;
                CFResult.ItemNumber = CFResults[0].ItemNumber;
                CFResult.Result = "T";

                foreach (MoConfirmResult item in CFResults)
                {
                    if (item.Result == "F")
                    {
                        CFResult.Result = "F";
                        break;
                    }
                }
 
                using (TransactionScope txn = UTL.CreateDbTxn())
                {
                    //0.Lock SystemSetting Name='ConfirmMOResultLock'
                    int timespan = IMES.WS.Common.SQL.ConCurrentLock("ConfirmMOResultLock");
                    BaseLog.LoggingInfo(logger, "ConfirmMOResult Lock time span:" + timespan.ToString());  
                    //1 UpdateStatus
                    string Message=SQL.UpdateStatus(CFResult);
                    BaseLog.LoggingInfo(logger,Message);
                    // 最後一行
                    txn.Complete();
                }                

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }          
        }

        //4.Build Response message structure
        /*public static MoReleaseResponse BuildResponseMsg(MoHeader moheader, bool isOK, string errMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                return null;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }           

            LoggingEnd(MethodBase.GetCurrentMethod());
            return null;
        }*/        

    }

    public class SAPClient
    {
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void QuerySAPMOStatus(string moId)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                // Get MO SAPStatus & MOStatus

                DataTable dt = SQL.QueryMOStatus(moId);
                if (dt.Rows.Count > 0)
                {
                    string SAPStatus = (string)dt.Rows[0]["SAPStatus"];
                    string MOStatus = (string)dt.Rows[0]["Status"];
                    logger.DebugFormat(" SAPMOStatus: {0} && MOStatus: {1}", SAPStatus.Trim(), MOStatus.Trim());
                    //Send Query MO Change Status Web Service then update MO Status
                    //不管任何狀態都要在query SAP status & DeliveredQty
                    //if (SAPStatus.Trim() == "REL" || 
                    //    SAPStatus.Trim() == "TECO" ||
                    //    MOStatus.Trim() == "Close" || 
                    //    MOStatus.Trim() == "Release")
                    //{
                        string error = "";
                        SentChangeMOStatus(moId, out error);
                    //}
                    //else
                    //{
                    //    logger.Debug("No need Query SAP MO Status!!");
                    //}
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        //Query SAP MO Status & Withdraw QTY
        public static bool SentChangeMOStatus(string moId, out string errorMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            errorMsg = "";
            try
            {
                //request SAP web service                                                  
                SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WSClient SAPClient = new SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WSClient("SAPMOChangeStatus");
                SAPClient.ClientCredentials.UserName.UserName = WebConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = WebConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                SAPMOChangeStatusWS.ZMO_MES_TECO_I input = new SAPMOChangeStatusWS.ZMO_MES_TECO_I();

                input.MONUMBER = moId;
                input.DELIVEREDQTY = "0";
                input.STATUS = "QUERY";
                string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
                logger.DebugFormat("MOStatusChange: \r\n{0}", ObjectTool.ObjectPropertyToString(input));
                IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                        "ChangeMOStatus",
                                                        moId,
                                                         "QUERY",
                                                      TxnId,
                                                        "",
                                                        "",
                                                        EnumMsgState.Sending,
                                                        "");

                SAPMOChangeStatusWS.ZMO_MES_TECO_E resp = SAPClient.Z_SET_PRODORD_TO_IMES_TECO(input);

                IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                        "ChangeMOStatus",
                                                        moId,
                                                         "QUERY",
                                                         TxnId,
                                                         (resp.RESULT==null ? "":resp.RESULT.Trim()),
                                                        (resp.ERRORMESSAGE == null ? "" : resp.ERRORMESSAGE.Trim()),
                                                        EnumMsgState.Received,
                                                        string.Format("SAPMOStatus:{0} WithdrawQty:{1}",resp.STATUS.Trim(),resp.DELIVEREDQTY.Trim()) );
                //Log Response Data
                logger.DebugFormat("MOStatusChangeResponse: \r\n{0}", ObjectTool.ObjectPropertyToString(resp));

               

                if (resp.RESULT.Trim() == "T")                     
                {
                    // update MO Status
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


        public static bool SentChangeMOStatus(string moId, 
                                                                          SAPMOChangeStatusCmd cmd,
                                                                           int deliveryQty, 
                                                                           out string errorMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            errorMsg = "";
            try
            {
                //request SAP web service                                                  
                SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WSClient SAPClient = new SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WSClient("SAPMOChangeStatus");
                SAPClient.ClientCredentials.UserName.UserName = WebConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = WebConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                SAPMOChangeStatusWS.ZMO_MES_TECO_I input = new SAPMOChangeStatusWS.ZMO_MES_TECO_I();

                input.MONUMBER = moId;
                input.DELIVEREDQTY = deliveryQty.ToString();
                input.STATUS = cmd.ToString();
                string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
                logger.DebugFormat("MOStatusChange: \r\n{0}", ObjectTool.ObjectPropertyToString(input));
                IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                        "ChangeMOStatus",
                                                        moId,
                                                        cmd.ToString(),
                                                       TxnId,
                                                         "",
                                                         "",
                                                        EnumMsgState.Sending,
                                                         "");

                SAPMOChangeStatusWS.ZMO_MES_TECO_E resp = SAPClient.Z_SET_PRODORD_TO_IMES_TECO(input);

                IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                        "ChangeMOStatus",
                                                        moId,
                                                        cmd.ToString(),
                                                         TxnId,
                                                         (resp.RESULT == null ? "" : resp.RESULT.Trim()),
                                                        (resp.ERRORMESSAGE == null ? "" : resp.ERRORMESSAGE.Trim()),
                                                        EnumMsgState.Received,
                                                        string.Format("SAPMOStatus:{0} WithdrawQty:{1}", resp.STATUS.Trim(), resp.DELIVEREDQTY.Trim()));
                //Log Response Data
                logger.DebugFormat("MOStatusChangeResponse: \r\n{0}", ObjectTool.ObjectPropertyToString(resp));

               

                if (resp.RESULT.Trim() == "T")
                {
                    // update SAP MO Status

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
    }
}