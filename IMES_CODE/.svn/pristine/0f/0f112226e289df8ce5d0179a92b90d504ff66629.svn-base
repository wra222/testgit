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

namespace IMES.WS.NotifyStdWeight
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(string connectionDB, NotifyStdWeight[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"SerialNumber",
                                                                 "Plant",
                                                                 "Model",
                                                                 "GrossWeight",
                                                                 "Unit"};
                foreach (NotifyStdWeight item in results)
                {
                    logger.DebugFormat("NotifyStdWeight: \r\n{0}", ObjectTool.ObjectTostring(item));
                    //log receive record to TxnDataLog
                    SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                            EnumMsgCategory.Receive,
                                            "NotifyStdWeight",
                                            string.IsNullOrEmpty(item.Model) ? "" : item.Model,
                                            string.IsNullOrEmpty(item.GrossWeight) ? "" : item.GrossWeight,
                                            string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                            "",
                                            "",
                                            EnumMsgState.Received,
                                            string.IsNullOrEmpty(item.Plant) ? "" : item.Plant);
                }

                foreach (NotifyStdWeight item in results)
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
                                                "NotifyStdWeightResponse",
                                                string.IsNullOrEmpty(item.Model) ? "" : item.Model,
                                                string.IsNullOrEmpty(item.GrossWeight) ? "" : item.GrossWeight,
                                                string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                                "",
                                                "",
                                                EnumMsgState.Fail,
                                                string.IsNullOrEmpty(item.Plant) ? "" : item.Plant);
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
        public static void Process(string connectionDB, string connectionDB_EDI, NotifyStdWeight[] results)
        {
            int dbIndex = 0;
            string HPPlantCode = WebConfigurationManager.AppSettings["HPPlantCode"];
            string SyncEDIStdWeight = WebConfigurationManager.AppSettings["SyncEDIStdWeight"];
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                foreach (NotifyStdWeight item in results)
                {
                    string SerialNumber = item.SerialNumber;
                    if (item.Plant == HPPlantCode)
                    {
                        if (SyncEDIStdWeight == "Y")
                        {
                            //insert or update Grossweight to HPEDI.table for connection
                            SQL.UpdateStdWeight(connectionDB_EDI, dbIndex, item);

                            //insert or update Grossweight to HPEDI.table for connection 2
                            SQL.UpdateStdWeight(connectionDB, dbIndex, item);
                        }
                        else {
                            //insert or update Grossweight to HPEDI.table for connection
                            SQL.UpdateStdWeight(connectionDB_EDI, dbIndex, item);

                        }


                        //log success response record to TxnDataLog
                        SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                EnumMsgCategory.Response,
                                                "NotifyStdWeightResponse",
                                                string.IsNullOrEmpty(item.Model) ? "" : item.Model,
                                                string.IsNullOrEmpty(item.GrossWeight) ? "" : item.GrossWeight,
                                                string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                                "",
                                                "",
                                                EnumMsgState.Success,
                                                string.IsNullOrEmpty(item.Plant) ? "" : item.Plant);
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
        public static List<NotifyStdWeightResponse> BuildResponseMsg(string connectionDB, NotifyStdWeight[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<NotifyStdWeightResponse> ResponseList = new List<NotifyStdWeightResponse>();
            try
            {
                foreach (NotifyStdWeight item in results)
                {
                    BaseLog.LoggingInfo(logger, "SerialNumber: \r\n{0}", ObjectTool.ObjectTostring(item.SerialNumber));

                    NotifyStdWeightResponse response = new NotifyStdWeightResponse();
                    string SerialNumber = item.SerialNumber;
                    string State = SQL.GetStdWeightState(connectionDB, dbIndex, SerialNumber);
                    if (State == "N") State = "F";
                    response.SerialNumber = item.SerialNumber;
                    response.Plant = item.Plant;
                    response.Model = item.Model; 
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