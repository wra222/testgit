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

namespace IMES.WS.NotifyBsamModel
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(string connectionDB, NotifyBsamModel[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"SerialNumber",
                                                                 "IEC_A_Part",
                                                                 "HP_C_SKU"};
                foreach (NotifyBsamModel item in results)
                {
                    logger.DebugFormat("NotifyBsamModel: \r\n{0}", ObjectTool.ObjectTostring(item));
                    //log receive record to TxnDataLog
                    SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                            EnumMsgCategory.Receive,
                                            "NotifyBsamModel",
                                            string.IsNullOrEmpty(item.IEC_A_Part) ? "" : item.IEC_A_Part,
                                            string.IsNullOrEmpty(item.HP_C_SKU) ? "" : item.HP_C_SKU,
                                            string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                            "",
                                            "",
                                            EnumMsgState.Received,
                                            "");
                }

                foreach (NotifyBsamModel item in results)
                {
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

                        //log fail response record to TxnDataLog
                        SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                EnumMsgCategory.Response,
                                                "NotifyBsamModelResponse",
                                                string.IsNullOrEmpty(item.IEC_A_Part) ? "" : item.IEC_A_Part,
                                                string.IsNullOrEmpty(item.HP_C_SKU) ? "" : item.HP_C_SKU,
                                                string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                                "",
                                                "",
                                                EnumMsgState.Fail,
                                                "");
                    }            
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
        public static void Process(string connectionDB, NotifyBsamModel[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                foreach (NotifyBsamModel item in results)
                {
                    string SerialNumber = item.SerialNumber;
                    string LogState = SQL.GetBsamModelState(connectionDB, dbIndex, SerialNumber);
                    if (LogState != "F")
                    {
                        SQL.UpdateBsamModel(connectionDB, 1, item);

                        //log success response record to TxnDataLog
                        SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                EnumMsgCategory.Response,
                                                "NotifyBsamModelResponse",
                                                string.IsNullOrEmpty(item.IEC_A_Part) ? "" : item.IEC_A_Part,
                                                string.IsNullOrEmpty(item.HP_C_SKU) ? "" : item.HP_C_SKU,
                                                string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                                "",
                                                "",
                                                EnumMsgState.Success,
                                                "");
                    }
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
        public static List<NotifyBsamModelResponse> BuildResponseMsg(string connectionDB, NotifyBsamModel[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<NotifyBsamModelResponse> ResponseList = new List<NotifyBsamModelResponse>();
            try
            {
                foreach (NotifyBsamModel item in results)
                {
                    BaseLog.LoggingInfo(logger, "SerialNumber: \r\n{0}", ObjectTool.ObjectTostring(item.SerialNumber));

                    NotifyBsamModelResponse response = new NotifyBsamModelResponse();
                    string SerialNumber = item.SerialNumber;
                    string State = SQL.GetBsamModelState(connectionDB, dbIndex, SerialNumber);
                    if (State == "N") State = "F";
                    response.SerialNumber = item.SerialNumber;
                    response.Result = State;
                    ResponseList.Add(response);
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