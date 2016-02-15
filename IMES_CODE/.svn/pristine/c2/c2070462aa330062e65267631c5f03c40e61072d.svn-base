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
using IMES.WS.DefectComponentUpdate;

namespace IMES.WS.DefectComponentUpdate
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(RequestDC dcItem)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                List<string> NotNullHeaderItemList = new List<string> {"TxnId",
                                                                 "BatchID"};

                List<string> NotNullItemList = new List<string> {"TxnId",
                                                                 "BatchID",
                                                                 //"PartSerialNo",
                                                                 "PartSn",
                                                                 "Status"};

                //Check null data
                string className = dcItem.dcheader.GetType().BaseType.Name;
                if (className == "Object")
                { className = dcItem.dcheader.GetType().Name; }
                string title = "These Header columns of " + className + " are null or no data ! ";
                string error = "";
                string headerError = "";
                string detailError = "";

                foreach (string itemcolumn in NotNullHeaderItemList)
                {
                    if (string.IsNullOrEmpty(GetValueByType(itemcolumn, dcItem.dcheader).Trim()))
                    { headerError = headerError + itemcolumn + ","; }
                }

                if (headerError != "")
                {
                    error = title + headerError;
                }

                //Check null data
                foreach (DCDetail item in dcItem.dcDetail)
                {
                    className = item.GetType().BaseType.Name;
                    if (className == "Object")
                    { className = item.GetType().Name; }
                    title = "These Detail columns of " + className + " are null or no data ! ";

                    foreach (string detailcolumn in NotNullItemList)
                    {
                        if (string.IsNullOrEmpty(GetValueByType(detailcolumn, item).Trim()))
                        { detailError = detailError + detailcolumn + ","; }
                    }
                }
                if (detailError != "")
                {
                    error = error + title + detailError;
                }

                if (error != "")
                {
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
        public static void Process(string ConnectionStr, string DBName, RequestDC dcItem)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            string ErrMsg;
            string BatchID = dcItem.dcheader.BatchID;

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

            ErrMsg = "";
            //Check PartSn
            try {
                foreach (DCDetail item in dcItem.dcDetail)
                {
                    //Check header's BatchID need equal to detail's BatchID
                    if (!BatchID.Equals(item.BatchID))
                    {
                        //ErrMsg = ErrMsg + "," + item.PartSerialNo;
                        ErrMsg = ErrMsg + "," + item.PartSn;
                    }
                    else
                    {
                        //Status = SQL.CheckDCPartSerialNo(ConnectionStr, DBName, BatchID, item.PartSerialNo);
                        Status = SQL.CheckDCPartSerialNo(ConnectionStr, DBName, BatchID, item.PartSn);
                        if (Status == "")
                        {
                            //ErrMsg = ErrMsg + "," + item.PartSerialNo;
                            ErrMsg = ErrMsg + "," + item.PartSn;
                        }
                    }
                }

                if (ErrMsg != "")
                {
                    ErrMsg = "Thease PartSerialNo :" + ErrMsg + " is not exists !";
                    throw new Exception(ErrMsg);
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
        //3.Query Result and Build Response message structure
        public static ResponseDC BuildResponseMsg(string ConnectionStr, string DBName, RequestDC dcItem, bool isOK, string errMsg)
        {
            string connectionDB = "SD_DBServer";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            string fTxnId = string.IsNullOrEmpty(dcItem.dcheader.TxnId) ? "" : dcItem.dcheader.TxnId;
            string fBatchID = string.IsNullOrEmpty(dcItem.dcheader.BatchID) ? "" : dcItem.dcheader.BatchID;
            string fStatus ="";

            ResponseDC responseDC = new ResponseDC();
            
            BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", fTxnId);                 
            try
            {
                if (isOK)
                {

                    foreach (DCDetail item in dcItem.dcDetail)
                    {

                        // update DefectComponent Status & insert DefectComponentLog 
                        //SQL.UpdateDCStatusByPartSerialNo(ConnectionStr, DBName, item.BatchID, item.PartSerialNo, item.Status, "IQS");
                        SQL.UpdateDCStatusByPartSerialNo(ConnectionStr, DBName, item.BatchID, item.PartSn, item.Status, "IQS");

                        // update DefectComponentBatchStatus & insert DefectComponentBatchStatusLog 
                        SQL.UpdateDCBatchStatusByBatchID(ConnectionStr, DBName, item.BatchID, item.Status, "IQS");
                        
                        //log IQS request data
                        IMES.WS.Common.SQL.InsertSendData_DB(connectionDB, 0,
                                                            "UpdateDefectComponentDetailByIQS",
                                                            item.BatchID,
                                                            //item.PartSerialNo,
                                                            item.PartSn,
                                                            fTxnId,
                                                            item.Status+","+item.Remark1+","+item.Remark2,
                                                            EnumMsgState.Sending,
                                                            DateTime.Now);

                    }

                    //Get BatchID Status
                    if (ConnectionStr != "") {
                        fStatus = SQL.CheckDCBatchID(ConnectionStr, DBName, fBatchID);
                    }

                    IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                          EnumMsgCategory.Response,
                                                          "ResponseDefectComponentUpdate",
                                                          fBatchID,
                                                          fStatus,
                                                          fTxnId,
                                                          "T",
                                                          "",
                                                          EnumMsgState.Success,
                                                          "");

                    responseDC.TxnId = fTxnId;
                    responseDC.BatchID = fBatchID;
                    responseDC.Status = fStatus;
                    responseDC.Result = "T";
                    responseDC.ErrorText = "";

                    BaseLog.LoggingInfo(logger, "responseDC: \r\n{0}", ObjectTool.ObjectTostring(responseDC));

                }
                else {
                    BaseLog.LoggingInfo(logger, "TxnId: \r\n{0}", fTxnId);

                    ////Get BatchID Status
                    //if (ConnectionStr != "")
                    //{
                    //    fStatus = SQL.CheckDCBatchID(ConnectionStr, DBName, fBatchID);
                    //}

                    IMES.WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                          EnumMsgCategory.Response,
                                                          "ResponseDefectComponentUpdate",
                                                          fBatchID,
                                                          fStatus,
                                                          fTxnId,
                                                          "F",
                                                          errMsg,
                                                          EnumMsgState.Fail,
                                                          "");

                    responseDC.TxnId = fTxnId;
                    responseDC.BatchID = fBatchID;
                    responseDC.Status = fStatus;
                    responseDC.Result = "F";
                    responseDC.ErrorText = errMsg;

                    BaseLog.LoggingInfo(logger, "responseDC: \r\n{0}", ObjectTool.ObjectTostring(responseDC));
                }

                return responseDC;
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