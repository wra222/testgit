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

namespace IMES.WS.NotifyECOModel
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(string connectionDB, NotifyECOModel[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"TxnId",
                                                                 "Plant",
                                                                 "ECRNo",
                                                                 "Model"};
                foreach (NotifyECOModel item in results)
                {
                    logger.DebugFormat("NotifyECOModel: \r\n{0}", ObjectTool.ObjectTostring(item));
                    //log receive record to TxnDataLog
                    SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                            EnumMsgCategory.Receive,
                                            "NotifyECOModel",
                                            string.IsNullOrEmpty(item.ECRNo) ? "" : item.ECRNo,
                                            string.IsNullOrEmpty(item.Model) ? "" : item.Model,
                                            string.IsNullOrEmpty(item.TxnId) ? "" : item.TxnId,
                                            "",
                                            "",
                                            EnumMsgState.Received,
                                            item.Plant  + ";" + item.ECONo);
                }

                foreach (NotifyECOModel item in results)
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
                        throw new Exception(error);
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
        public static void Process(string connectionDB, NotifyECOModel[] results)
        {
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                foreach (NotifyECOModel item in results)
                {
                    string ECRNo = item.ECRNo;
                    string Model = item.Model;

                    string[] PlantCode = item.Plant.Split(new char[] { ',', ';' });
                    string dataType = "ECOModel";
                    for (int i = 0; i < PlantCode.Length; i++)
                    {
                        string Plant = PlantCode[i].ToUpper();
                        if (PlantCode[i].Trim() != "")
                        {
                            //根據PlantCode獲取連線定義
                            List<SAPMAWBDef> DefList = SQL.GetSAPMawbDefByPlantCode(connectionDB, dbIndex, dataType, Plant);

                            //若有獲取連線定義，進行資料處理；否則By Pass Data
                            if (DefList.Count != 0)
                            {
                                foreach (SAPMAWBDef DefItem in DefList)
                                {
                                    string IsExists = SQL.CheckECOModelExists(DefItem.ConnectionStr, DefItem.DBName, item, DefItem.PlantCode);

                                    if (IsExists == "Y")
                                    {
                                        BaseLog.LoggingInfo(logger, "The Data is exists, no need process ! ECRNo: \r\n{0}", item.ECRNo + ", Model:" + item.Model + ", Plant:" + DefItem.PlantCode);
                                    }
                                    else {
                                        //Check WIP
                                        string HoldState = SQL.CheckECOModelInWIP(DefItem.ConnectionStr, DefItem.DBName, item);

                                        if (HoldState == "Hold")
                                            BaseLog.LoggingInfo(logger, "The Model is Hold ! ECRNo: \r\n{0}", item.ECRNo + ", Model:" + item.Model + ", Plant:" + Plant);
                                        else if (HoldState == "IsHold")
                                            BaseLog.LoggingInfo(logger, "The Model is already Hold ! ECRNo: \r\n{0}", item.ECRNo + ", Model:" + item.Model + ", Plant:" + Plant);
                                        else
                                            BaseLog.LoggingInfo(logger, "The Model is not in WIP ! ECRNo: \r\n{0}", item.ECRNo + ", Model:" + item.Model + ", Plant:" + Plant);


                                        //檢查ECRNo + Model + Plant 是否已存在ECOModel表，不存在則新增，存在則不處理
                                        SQL.InsertECOModel(DefItem.ConnectionStr, DefItem.DBName, item, DefItem.PlantCode, "HoldTravelCard");
                                    }
                                }
                            }
                            else
                                BaseLog.LoggingInfo(logger, "The data of this PlantCode no need process ! ECRNo: \r\n{0}", item.ECRNo + ", Model:" + item.Model + ", Plant:" + Plant);

                        }
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
        public static List<NotifyECOModelResponse> BuildResponseMsg(string connectionDB, NotifyECOModel[] results, bool IsOK, string Message)
        {
            int dbIndex = 0; 
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<NotifyECOModelResponse> ResponseList = new List<NotifyECOModelResponse>();
            try
            {
                if (IsOK)
                {
                    foreach (NotifyECOModel item in results)
                    {
                        //BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", ObjectTool.ObjectTostring(item.TxnId));

                        NotifyECOModelResponse response = new NotifyECOModelResponse();
                        response.TxnId = item.TxnId;
                        response.ECRNo = item.ECRNo;
                        response.Model = item.Model;
                        response.Result = "T";
                        response.Message = Message;
                        ResponseList.Add(response);

                        logger.DebugFormat("NotifyECOModelResponse: \r\n{0}", ObjectTool.ObjectTostring(response));

                        //log success response record to TxnDataLog
                        SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                EnumMsgCategory.Response,
                                                "NotifyECOModelResponse",
                                                string.IsNullOrEmpty(item.ECRNo) ? "" : item.ECRNo,
                                                string.IsNullOrEmpty(item.Model) ? "" : item.Model,
                                                string.IsNullOrEmpty(item.TxnId) ? "" : item.TxnId,
                                                "",
                                                "",
                                                EnumMsgState.Success,
                                                item.Plant +";" + item.ECONo);
                    }
                }
                else {
                    foreach (NotifyECOModel item in results)
                    {
                        //BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", ObjectTool.ObjectTostring(item.TxnId));

                        NotifyECOModelResponse response = new NotifyECOModelResponse();
                        response.TxnId = item.TxnId;
                        response.ECRNo = item.ECRNo;
                        response.Model = item.Model;
                        response.Result = "F";
                        response.Message = Message;
                        ResponseList.Add(response);

                        logger.DebugFormat("NotifyECOModelResponse: \r\n{0}", ObjectTool.ObjectTostring(response));

                        //log fail response record to TxnDataLog
                        SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                EnumMsgCategory.Response,
                                                "NotifyECOModelResponse",
                                                string.IsNullOrEmpty(item.ECRNo) ? "" : item.ECRNo,
                                                string.IsNullOrEmpty(item.Model) ? "" : item.Model,
                                                string.IsNullOrEmpty(item.TxnId) ? "" : item.TxnId,
                                                "F",
                                                Message,
                                                EnumMsgState.Fail,
                                                item.Plant + ";" + item.ECONo);
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