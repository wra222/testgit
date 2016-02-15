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
using IMES.WS.SAPCancelBindDNWS;

namespace IMES.WS.CancelBindDN
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(CancelBindDN[] results)
        {
            string connectionDB_BK = "SD_DBServer_BK";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"SerialNumber",
                                                                 "Plant",
                                                                 "DN",
                                                                 "Remark1"};
                foreach (CancelBindDN item in results)
                {
                    logger.DebugFormat("CancelBindDN: \r\n{0}", ObjectTool.ObjectTostring(item));
                }

                int errcnt = 0;
                foreach (CancelBindDN item in results)
                {

                    SQL.InsertCancelBindDNLog(connectionDB_BK, dbIndex, 
                                              item.SerialNumber, item.Plant, item.DN, item.Remark1, item.Remark2,
                                              EnumMsgCategory.Receive, EnumMsgState.Success, "", "FIS");

                    //Check null data
                    string className = item.GetType().BaseType.Name;
                    if (className == "Object")
                    { className = item.GetType().Name; }
                    string title = "These columns of " + className + " are null or no data : ";
                    string error = "";
                    foreach (string itemcolumn in NotNullItemList)
                    {
                        if (string.IsNullOrEmpty(GetValueByType(itemcolumn, item).Trim()))
                        { error = error + itemcolumn + ","; }
                    }

                    if (error != "")
                    {
                        error = title + error;
                        //Log error message to CancelBindDNLog.
                        SQL.InsertCancelBindDNLog(connectionDB_BK, dbIndex,
                                                  item.SerialNumber, item.Plant, item.DN, item.Remark1, item.Remark2,
                                                  EnumMsgCategory.Response, EnumMsgState.Fail, error, "IMES");
                        errcnt++;
                        
                    }
                    else {
                        SQL.InsertCancelBindDNLog(connectionDB_BK, dbIndex,
                                                  item.SerialNumber, item.Plant, item.DN, item.Remark1, item.Remark2,
                                                  EnumMsgCategory.Response, EnumMsgState.Success, "Check column ok!", "IMES");
                    }              
                    //ObjectTool.CheckNullData(NotNullItemList, item);
                }
                if (errcnt != 0) throw new Exception("");

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

        //4.Build Response message structure
        public static List<CancelBindDNResponse> BuildResponseMsg(CancelBindDN[] results, bool isOK, string errMsg)
        {
            string connectionDB = "SD_DBServer";
            string connectionDB_BK = "SD_DBServer_BK";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<CancelBindDNResponse> ResponseList = new List<CancelBindDNResponse>();
            try
            {
                if (isOK)
                {
                    //request SAP web service
                    SAPCancelBindDNWS.ZWS_CANCEL_SERIALClient SAPDNClient = new SAPCancelBindDNWS.ZWS_CANCEL_SERIALClient("SAPCancelBindDN");
                    SAPDNClient.ClientCredentials.UserName.UserName = WebConfigurationManager.AppSettings["SAPUserName"].ToString();
                    SAPDNClient.ClientCredentials.UserName.Password = WebConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                    List<SAPCancelBindDNWS.ZwsCancelSnInLine> SnInlist = new List<SAPCancelBindDNWS.ZwsCancelSnInLine>();

                    foreach (CancelBindDN item in results)
                    {
                        BaseLog.LoggingInfo(logger, "SerialNumber: \r\n{0}", item.SerialNumber);
                        SAPCancelBindDNWS.ZwsCancelSnInLine item1 = new SAPCancelBindDNWS.ZwsCancelSnInLine();
                        item1.Serialnumber = item.SerialNumber;
                        item1.Plant = item.Plant;
                        item1.Id = item.DN;
                        item1.Remark1 = item.Remark1;
                        item1.Remark2 = item.Remark2;
                        SnInlist.Add(item1);
                        IMES.WS.Common.SQL.InsertSendData_DB(connectionDB, dbIndex,
                                                            "SendCancelBindDN",
                                                            item.DN,
                                                            item.Plant,
                                                            item1.Serialnumber.Trim(),
                                                            item1.Remark1+","+item1.Remark2,
                                                            EnumMsgState.Sending,
                                                            DateTime.Now);

                    }

                    SAPCancelBindDNWS.ZwsCancelSnOutLine[] SnOutLine = SAPDNClient.ZwsCancelSerial(SnInlist.ToArray());

                    foreach (SAPCancelBindDNWS.ZwsCancelSnOutLine item in SnOutLine)
                    {
                        if (item.Result == "0")
                        {
                            IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                                    EnumMsgCategory.Receive,
                                                                    "ReceiveCancelBindDN",
                                                                    item.Id,
                                                                    item.Plant.Trim(),
                                                                    item.Serialnumber.Trim(),
                                                                    item.Result,
                                                                    item.Errortext,
                                                                    EnumMsgState.Success,
                                                                    "");

                            SQL.InsertCancelBindDNLog(connectionDB_BK, dbIndex,
                                                      item.Serialnumber, item.Plant, item.Id, "", "",
                                                      EnumMsgCategory.Response, EnumMsgState.Success, "", "SAP");

                        }
                        else
                        {
                            IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                                  EnumMsgCategory.Receive,
                                                                  "ReceiveCancelBindDN",
                                                                  item.Id,
                                                                  item.Plant.Trim(),
                                                                  item.Serialnumber.Trim(),
                                                                  item.Result,
                                                                  item.Errortext,
                                                                  EnumMsgState.Fail,
                                                                  "");

                            SQL.InsertCancelBindDNLog(connectionDB_BK, dbIndex,
                                                      item.Serialnumber, item.Plant, item.Id, "", "",
                                                      EnumMsgCategory.Response, EnumMsgState.Fail, item.Errortext, "SAP");
                        }
                        CancelBindDNResponse response = new CancelBindDNResponse();
                        response.SerialNumber = item.Serialnumber;
                        response.DN = item.Id;
                        response.Plant = item.Plant;
                        response.Result = item.Result;
                        response.ErrorText = item.Errortext;
                        ResponseList.Add(response);
                        BaseLog.LoggingInfo(logger, "ResponseMsg: \r\n{0}", ObjectTool.ObjectTostring(response));

                    }
                }
                else {
                    foreach (CancelBindDN item in results)
                    {

                        BaseLog.LoggingInfo(logger, "SerialNumber: \r\n{0}", item.SerialNumber);
                        CancelBindDNResponse responseFail = new CancelBindDNResponse();
                        string SerialNumber = item.SerialNumber;
                        responseFail.SerialNumber = item.SerialNumber;
                        responseFail.Result = "F";
                        responseFail.ErrorText = SQL.GetCancelDNState(connectionDB_BK, dbIndex, item.SerialNumber);
                        ResponseList.Add(responseFail);
                        /*
                        SQL.InsertCancelBindDNLog(connectionDB, dbIndex,
                                                  item.SerialNumber, item.Plant, item.DN, item.Remark1, item.Remark2,
                                                  EnumMsgCategory.Response, EnumMsgState.Fail, responseFail.ErrorText, "IMES");
                        */
                    }
                }
                return ResponseList;
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