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

namespace IMES.WS.PGIStatus
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(string connectionDB, PGIStatus[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"SerialNumber",
                                                                 "Plant",
                                                                 "EventType",
                                                                 "ID",
                                                                 "Type",
                                                                 "PGI_Date"};
                foreach (PGIStatus item in results)
                {
                    logger.DebugFormat("PGIStatus: \r\n{0}", ObjectTool.ObjectTostring(item));
                }

                foreach (PGIStatus item in results)
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
                        //Log error message to PGILog.
                        string State = "Fail";
                        SQL.InsertPGILog(connectionDB, dbIndex, item, State, error);
                    }
                    
                    //ObjectTool.CheckNullData(NotNullItemList, item);
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
        public static void Process(string connectionDB_BK, PGIStatus[] results)
        {
            string connectionDB = "SD_DBServer";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                //Log success message to PGILog.
                string State = "Success";
                string ErrorDescr = "";

                string PGIDnStatus = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["PGIDeliveryStatus"]) ? "" : WebConfigurationManager.AppSettings["PGIDeliveryStatus"];
                string UpdateDeliveryByPlant = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["UpdateDeliveryByPlant"]) ? "" : WebConfigurationManager.AppSettings["UpdateDeliveryByPlant"];
                string UpdateDnShipDateByPlant = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["UpdateDnShipDateByPlant"]) ? "" : WebConfigurationManager.AppSettings["UpdateDnShipDateByPlant"];
                //string[] PlantList = UpdateDeliveryByPlant.Split(new char[] { ',', ';' });

                BaseLog.LoggingInfo(logger, "The Setting of Plant need to update Delivery.Status! Plant: \r\n{0}", UpdateDeliveryByPlant);

                foreach (PGIStatus item in results)
                {
                    string SerialNumber = item.SerialNumber;
                    string LogState = SQL.GetPGIState(connectionDB_BK, dbIndex, SerialNumber);
                    if (LogState != "F")
                    {
                        SQL.UpdatePGIStatus(connectionDB_BK, dbIndex, item);
                        SQL.InsertPGILog(connectionDB_BK, dbIndex, item, State, ErrorDescr);

                        if (UpdateDeliveryByPlant.Contains(item.Plant)) {
                            // if plant setting is exists, update Delivery.Status by Plant setting
                            if (item.EventType == "PGI")
                            {
                                // if plant setting is exists, update Delivery.ShipDate = PGI_Date
                                if (UpdateDnShipDateByPlant.Contains(item.Plant))
                                {
                                    SQL.UpdatePGIDeliveryStatusIncludeShipDate(connectionDB, 1, item, PGIDnStatus);
                                }
                                else {
                                    SQL.UpdatePGIDeliveryStatus(connectionDB, 1, item, PGIDnStatus);
                                }

                            }
                            else
                                BaseLog.LoggingInfo(logger, "The PGI Status no need to update Delivery.Status ! Plant: \r\n{0}", item.Plant + ", EventType :" + item.EventType);
                        }
                        else
                            BaseLog.LoggingInfo(logger, "The Plant no need to update Delivery.Status ! Plant: \r\n{0}", item.Plant + ", EventType :" + item.EventType);
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
        public static List<PGIStatusResponse> BuildResponseMsg(string connectionDB, PGIStatus[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<PGIStatusResponse> ResponseList = new List<PGIStatusResponse>();
            try
            {
                foreach (PGIStatus item in results)
                {
                    BaseLog.LoggingInfo(logger, "SerialNumber: \r\n{0}", ObjectTool.ObjectTostring(item.SerialNumber));
                }

                foreach (PGIStatus item in results)
                {
                    PGIStatusResponse response = new PGIStatusResponse();
                    string SerialNumber = item.SerialNumber;
                    string State = SQL.GetPGIState(connectionDB, dbIndex, SerialNumber);
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