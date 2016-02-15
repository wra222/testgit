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
using IMES.WS.DefectComponentDetail;

namespace IMES.WS.DefectComponentDetail
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(DCItem dcItem)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                //Check the condition column first!
                if (dcItem.Type == "")
                {
                    throw new Exception("These columns of Type are null or no data ! ");
                }
                

                List<string> NotNullItemList = new List<string> {"TxnId",
                                                                 "BatchID",
                                                                 "Type"};

                List<string> NotNullItemList2 = new List<string> {"TxnId",
                                                                 "BatchID",
                                                                 "Type",
                                                                 "Family",
                                                                 "IECPn",
                                                                 "Vendor",
                                                                 "DefectCode"};

                //Check null data
                string className = dcItem.GetType().BaseType.Name;
                if (className == "Object")
                { className = dcItem.GetType().Name; }
                string title = "These columns of " + className + " are null or no data : ";
                string error = "";

                if (dcItem.Type == "")
                    { error = "Type"; }
                else if (dcItem.Type == "0")
                {
                    foreach (string itemcolumn in NotNullItemList)
                    {
                        if (string.IsNullOrEmpty(GetValueByType(itemcolumn, dcItem).Trim()))
                        { error = error + itemcolumn + ","; }
                    }
                }
                else if (dcItem.Type == "1")
                {
                    foreach (string itemcolumn in NotNullItemList2)
                    {
                        if (string.IsNullOrEmpty(GetValueByType(itemcolumn, dcItem).Trim()))
                        { error = error + itemcolumn + ","; }
                    }
                }

                if (error != "")
                {
                    error = title + error;
                    throw new Exception(error);
                }
                //ObjectTool.CheckNullData(NotNullItemList, item);


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

        //2.執行DB Data Check 
        public static void Process(string ConnectionStr, string DBName, string BatchID)
        {
            string ErrMsg;
            string Status = SQL.CheckDCBatchID(ConnectionStr, DBName, BatchID);

            if (Status == "")
            {
                ErrMsg = "The BatchID : " + BatchID + " is not exists! ";
                throw new Exception(ErrMsg);
            }
            //BatchID Status: IQS Completed
            else if (Status == "30")
            {
                ErrMsg = "The BatchID : " + BatchID + " is completed! ";
                throw new Exception(ErrMsg);
            }

        }
        //3.Query Result and Build Response message structure
        public static ResponseDC BuildResponseMsg(string ConnectionStr, string DBName, DCItem dcItem, bool isOK, string errMsg)
        {
            string ErrMsg;
            string connectionDB = "SD_DBServer";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            string fTxnId = string.IsNullOrEmpty(dcItem.TxnId) ? "" : dcItem.TxnId;
            string fBatchID = string.IsNullOrEmpty(dcItem.BatchID) ? "" : dcItem.BatchID;
            string fType = string.IsNullOrEmpty(dcItem.Type) ? "" : dcItem.Type;
            string fFamily = string.IsNullOrEmpty(dcItem.Family) ? "" : dcItem.Family;
            string fIECPn = string.IsNullOrEmpty(dcItem.IECPn) ? "" : dcItem.IECPn;
            string fVendor = string.IsNullOrEmpty(dcItem.Vendor) ? "" : dcItem.Vendor;
            string fDefectCode = string.IsNullOrEmpty(dcItem.DefectCode) ? "" : dcItem.DefectCode;
            string fStatus ="";

            DCHeader dcHeader = new DCHeader();
            List<DCDetail> dcDetail = new List<DCDetail>();
            BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", fTxnId);                 
            try
            {
                if (isOK)
                {
                    //Update BatchID Status:10 (Request by IQS) when BatchID status is '00'(Print)
                    // SQL.UpdateDCStatusByBatchID(ConnectionStr, DBName, fBatchID, Status, Editor);
                    SQL.UpdateDCBstchStatusByBatchID(ConnectionStr, DBName, fBatchID, "10", "IQS");

                    //Get BatchID Status
                    fStatus = SQL.CheckDCBatchID(ConnectionStr, DBName, fBatchID);

                    List<DCDetail> QueryResult = new List<DCDetail>();

                    if (fType == "0")
                        QueryResult = SQL.QueryDCDetailByBatchID(ConnectionStr, DBName, fBatchID);
                    else
                        QueryResult = SQL.QueryDCDetailByCondition(ConnectionStr, DBName, fBatchID, fFamily, fIECPn, fVendor, fDefectCode);

                    if (QueryResult.Count == 0)
                    {
                        ErrMsg = "The BatchID : " + dcItem.BatchID + " no data! ";
                        throw new Exception(ErrMsg);
                    }

                    foreach (DCDetail item in QueryResult)
                    {                
                        DCDetail DetailItem = new DCDetail
                        {
                            TxnId = fTxnId,
                            BatchID = item.BatchID,
                            Family = item.Family,
                            IECPn = item.IECPn,
                            PartType = item.PartType,
                            Vendor = item.Vendor,
                            DefectCode = item.DefectCode,
                            DefectDescr = item.DefectDescr,
                            PartSn = item.PartSn,
                            PartSerialNo = item.PartSerialNo
                        };

                        dcDetail.Add(DetailItem);

                        //log Send data
                        IMES.WS.Common.SQL.InsertSendData_DB(connectionDB, 0,
                                                            "SendDefectComponentDetail",
                                                            item.BatchID,
                                                            //item.PartSerialNo,
                                                            item.PartSn,
                                                            fTxnId,
                                                            //item.Family+","+item.IECPn+","+item.PartType+","+item.Vendor+","+item.DefectCode+","+item.PartSn,
                                                            item.Family + "," + item.IECPn + "," + item.PartType + "," + item.Vendor + "," + item.DefectCode + "," + item.PartSerialNo,
                                                            EnumMsgState.Sending,
                                                            DateTime.Now);
                    }


                    IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                          EnumMsgCategory.Response,
                                                          "ResponseDefectComponentDetail",
                                                          fBatchID,
                                                          fType,
                                                          fTxnId,
                                                          "T",
                                                          "",
                                                          EnumMsgState.Success,
                                                          "");

                    dcHeader.TxnId = fTxnId;
                    dcHeader.BatchID = fBatchID;
                    dcHeader.Status = fStatus;
                    dcHeader.Result = "T";
                    dcHeader.ErrorText = "";

                    BaseLog.LoggingInfo(logger, "dcHeader: \r\n{0}", ObjectTool.ObjectTostring(dcHeader));

                }
                else {
                    BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", fTxnId);

                    IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                          EnumMsgCategory.Response,
                                                          "ResponseDefectComponentDetail",
                                                          fBatchID,
                                                          fType,
                                                          fTxnId,
                                                          "F",
                                                          errMsg,
                                                          EnumMsgState.Fail,
                                                          "");
                    //if (fBatchID != "")
                    //{
                    //    //Get BatchID Status
                    //    fStatus = SQL.CheckDCBatchID(ConnectionStr, DBName, fBatchID);
                    //}

                    dcHeader.TxnId = fTxnId;
                    dcHeader.BatchID = fBatchID;
                    dcHeader.Status = fStatus;
                    dcHeader.Result = "F";
                    dcHeader.ErrorText = errMsg;

                    BaseLog.LoggingInfo(logger, "dcHeader: \r\n{0}", ObjectTool.ObjectTostring(dcHeader));
                }

                ResponseDC response = new ResponseDC();
                response.dcHeader = dcHeader;
                response.dcDetail = dcDetail.ToArray();

                return response;
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