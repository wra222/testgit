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
using System.Data;
using IMES.Query.DB;
using IMES.WS.SAPFisBomCompareWS;

namespace IMES.WS.FisBomCompare
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(string connectionDB, FisBomCompare result)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"TxnId",
                                                                 "Plant",
                                                                 "Model"};

                logger.DebugFormat("Receive FisBom from FIS: \r\n{0}", ObjectTool.ObjectTostring(result));

                //receive FIS BOM
                //headerRT : IDENTITY of insert to BOM_Compare_Header
                string headerRT = SQL.InsertBomCompareHeader(connectionDB, 0, 
                                           result.TxnId , result.Plant, result.Model, "", "FIS");
                //Check null data
                string className = result.GetType().BaseType.Name;
                if (className == "Object")
                { className = result.GetType().Name; }
                string title = "These columns of " + className + " are null or no data : ";
                string error = "";
                foreach (string itemcolumn in NotNullItemList)
                {
                    if (string.IsNullOrEmpty(GetValueByType(itemcolumn, result).Trim()))
                    { error = error + itemcolumn + ","; }
                }

                if (error != "")
                {
                    error = title + error;
                    throw new Exception("97"+headerRT +"|"+ error);
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
        public static void Process(string connectionDB, FisBomCompare result)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string connectionDB_Fis = "TSB_FisDBServer";
                string Model = result.Model;
                string spName = WebConfigurationManager.AppSettings["FisBomSpName"];

                logger.DebugFormat("*** Get FIS sp data ~ Start ~***");
                List<FisBomItem> FisBomData = SQL.GetFisBomData(connectionDB_Fis, 0, spName, Model, result.TxnId);
                logger.DebugFormat("*** Get FIS sp data ~ End ~***");               

                if (FisBomData.Count == 0) throw new Exception("FIS BOM has no data! Model : " + Model);

                //Bulk insert into BOM_Compare_Item
                logger.DebugFormat("=== Bulk insert BOM_Compare_Item ~ Start ~===");
                DataTable dt = FisBomData.ToDataTable();
                SQL.BulkInsertBomCompareItem(connectionDB, 0, dt);
                logger.DebugFormat("=== Bulk insert BOM_Compare_Item ~ End ~===");
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                string error = "98||"+ e.Message;
                throw new Exception(error);
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        //4.Build Response message structure
        public static FisBomCompareResponse BuildResponseMsg(string connectionDB, FisBomCompare result, bool isOK, string errMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<FisBomCompareResponse> ResponseList = new List<FisBomCompareResponse>();
            try
            {
                if (isOK)
                {
                    string TxnId = result.TxnId;
                    
                    //request SAP web service
                    Z_RFC_FIS_BOM_WSClient SAPBomClient = new Z_RFC_FIS_BOM_WSClient("SAPFisBomCompare");
                    SAPBomClient.ClientCredentials.UserName.UserName = WebConfigurationManager.AppSettings["SAPUserName_8010"].ToString();
                    SAPBomClient.ClientCredentials.UserName.Password = WebConfigurationManager.AppSettings["SAPUserPwd_8010"].ToString();

                    ZIMES_FIS_BOM_H BomHeader = new ZIMES_FIS_BOM_H();
                    List<ZIMES_FIS_BOM_I> BomItem = new List<ZIMES_FIS_BOM_I>();

                    //get FIS BOM ItemData
                    List<FisBomItem> FisBomItem = SQL.GetFisBomItem(connectionDB, 0, TxnId);

                    BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", TxnId);
                    //set SAP Item
                    int i = 0;
                    List<SendData> SendDataList = new List<SendData>();
                    foreach (FisBomItem item in FisBomItem)
                    {                        
                        ZIMES_FIS_BOM_I SAPItem = new ZIMES_FIS_BOM_I();
                        SendData SendData = new SendData();

                        SAPItem.SERIALNUMBER = item.TxnId;
                        SAPItem.ITEMNO = item.ItemNo;
                        SAPItem.ALPGR = item.AltGroup;
                        SAPItem.IDNRK = item.Component;
                        SAPItem.MENGE = item.Qty;
                        SAPItem.MEINS = item.Unit;
                        BomItem.Add(SAPItem);

                        SendData.Action = "SendFisBomCompare";
                        SendData.KeyValue1 = item.ItemNo;
                        SendData.KeyValue2 = item.Component;
                        SendData.TxnId = item.TxnId.Trim();
                        SendData.ErrorCode = "";
                        SendData.ErrorDescr = "";
                        SendData.State = "Sending";
                        SendData.ResendCount = 0;
                        SendData.Comment = item.AltGroup + "," + item.Qty + "," + item.Unit;
                        SendData.Cdt = DateTime.Now;
                        SendData.Udt = DateTime.Now;
                        SendDataList.Add(SendData);

                        i++;
                    }

                    //Bulk insert into SendData
                    logger.DebugFormat("=== Bulk insert SendData ~ Start ~===");
                    DataTable dt = SendDataList.ToDataTable();
                    SQL.BulkInsertSendData(connectionDB, 0, dt);
                    logger.DebugFormat("=== Bulk insert SendData ~ End ~===");
                    
                    string count = i.ToString().PadLeft(4, '0');
                    //set SAP Header
                    BomHeader.SERIALNUMBER = result.TxnId;
                    BomHeader.WERKS = result.Plant;
                    BomHeader.MATNR = result.Model;
                    BomHeader.CNT = count;

                    logger.DebugFormat("*** Send FIS Bom to SAP ~ Start ~***");                   
                    ZIMES_FIS_BOM_I[] BomItem_Req = BomItem.ToArray();
                    ZIMES_FIS_BOM_I_R[] BomItem_Resp = new ZIMES_FIS_BOM_I_R[0];
                    ZIMES_FIS_BOM_H_R FIS_BOM_H_R = SAPBomClient.Z_RFC_FIS_BOM(BomHeader, ref BomItem_Req, ref BomItem_Resp);
                    logger.DebugFormat("*** Send FIS Bom to SAP ~ End ~***");

                    string ErrorMsg = "";
                    // Status=02, 有回傳 item, 非02, 則只有header
                    if (FIS_BOM_H_R.STATUS == "02")
                    {
                        IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                EnumMsgCategory.Receive,
                                                                "ReceiveFisBomCompare",
                                                                FIS_BOM_H_R.MATNR.Trim(),
                                                                FIS_BOM_H_R.WERKS.Trim(),
                                                                FIS_BOM_H_R.SERIALNUMBER.Trim(),
                                                                FIS_BOM_H_R.STATUS.Trim(),
                                                                "",
                                                                EnumMsgState.Success,
                                                                "");

                        //update BOM_Compare_Item
                        foreach (ZIMES_FIS_BOM_I_R itemR in BomItem_Resp)
                        {
                            SQL.UpdateBomCompareItem(connectionDB, 0,
                                                      itemR.SERIALNUMBER, itemR.ITEMNO, itemR.ERRORLOG, "SAP");

                            ErrorMsg = ErrorMsg + "[" + itemR.ITEMNO + "]" + itemR.ERRORLOG;
                        }

                        SQL.UpdateBomCompareHeader(connectionDB, 0,
                                                  FIS_BOM_H_R.SERIALNUMBER.Trim(), FIS_BOM_H_R.STATUS.Trim(), ErrorMsg, "SAP");

                    }
                    else {
                        IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                EnumMsgCategory.Receive,
                                                                "ReceiveFisBomCompare",
                                                                FIS_BOM_H_R.MATNR.Trim(),
                                                                FIS_BOM_H_R.WERKS.Trim(),
                                                                FIS_BOM_H_R.SERIALNUMBER.Trim(),
                                                                FIS_BOM_H_R.STATUS.Trim(),
                                                                "",
                                                                EnumMsgState.Success,
                                                                "");

                        SQL.UpdateBomCompareHeader(connectionDB, 0,
                                                  FIS_BOM_H_R.SERIALNUMBER.Trim(), FIS_BOM_H_R.STATUS.Trim(), "", "SAP");
                    }

                    FisBomCompareResponse response = new FisBomCompareResponse();
                    response.TxnId = FIS_BOM_H_R.SERIALNUMBER;
                    response.Status = FIS_BOM_H_R.STATUS;
                    response.ErrorText = ErrorMsg;

                    BaseLog.LoggingInfo(logger, "ResponseMsg: \r\n{0}", ObjectTool.ObjectTostring(response));
                    return response;
                }
                else {
                    BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", (result.TxnId == null ? "" : result.TxnId));
                    FisBomCompareResponse responseFail = new FisBomCompareResponse();

                    string[] errText = errMsg.Split(new char[] { '|' });
                    if (errText.Length > 1)
                    {
                        if (errText[1].Length == 0)
                        {
                            SQL.UpdateBomCompareHeader(connectionDB, 0,
                                                      result.TxnId, errText[0], errText[2], "IMES");
                        }
                        else
                        {
                            SQL.UpdateBomCompareHeaderByID(connectionDB, 0,
                                                      errText[1], errText[0], errText[2], "IMES");
                        }

                        responseFail.TxnId = (result.TxnId == null ? "" : result.TxnId);
                        responseFail.Status = errText[0];
                        responseFail.ErrorText = errText[2];
                    }
                    else
                    {

                        SQL.UpdateBomCompareHeader(connectionDB, 0,
                                                  result.TxnId, "99", errMsg.ToString(), "IMES");

                        responseFail.TxnId = result.TxnId;
                        responseFail.Status = "99";
                        responseFail.ErrorText = errMsg;
                    }
                    return responseFail;                    
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

        private static string GetValueByType(string type, object dataObj)
        {

            FieldInfo fi = dataObj.GetType().GetField(type);
            if (fi == null)
                return "";
            else
                if (fi.GetValue(dataObj) == null)
                { return ""; }
                else
                { return fi.GetValue(dataObj).ToString(); }

        }

    }
}