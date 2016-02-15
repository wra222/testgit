using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
using System.Web.Services.Protocols;
using System.Data;

namespace IMES.WS.DefectComponentDetail
{
    /// <summary>
    /// WSDefectComponentDetail 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSDefectComponentDetail : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public ResponseDC DefectComponentDetail(DCItem dcItem)
        {
            string connectionDB = "SD_DBServer";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string BatchID = "";
                string Family = "";
                string IECPn = "";
                string Vendor = "";
                string DefectCode = "";
                BatchID = string.IsNullOrEmpty(dcItem.BatchID) ? "" : dcItem.BatchID.Trim();
                Family = string.IsNullOrEmpty(dcItem.Family) ? "" : dcItem.Family.Trim();
                IECPn = string.IsNullOrEmpty(dcItem.IECPn) ? "" : dcItem.IECPn.Trim();
                Vendor = string.IsNullOrEmpty(dcItem.Vendor) ? "" : dcItem.Vendor.Trim();
                DefectCode = string.IsNullOrEmpty(dcItem.DefectCode) ? "" : dcItem.DefectCode.Trim();


                WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                        EnumMsgCategory.Receive,
                                                        "DefectComponentDetail",
                                                        BatchID,
                                                        string.IsNullOrEmpty(dcItem.Type) ? "" : dcItem.Type.Trim(),
                                                        string.IsNullOrEmpty(dcItem.TxnId) ? "" : dcItem.TxnId.Trim(),
                                                        "",
                                                        "",
                                                        EnumMsgState.Received,
                                                        Family + ";" + IECPn + ";" + Vendor + ";" + DefectCode);

                //1.檢查必要的input parameter
                Execute.ValidateParameter(dcItem);

                string Site = "";
                string CustomerCode = "";
                Site = BatchID.Substring(0, 3);
                CustomerCode = BatchID.Substring(3, 1);

                List<SAPWeightDef> DefList = null;
                DefList = SQL.GetSAPWeightDef(connectionDB, "DefectComponent");
                logger.Info("SQL.GetSAPWeightDef OK!!");

                string ConnectionStr = "";
                string DBName = "";
                string ErrMsg = "";

                //Get BU database connetion by Site & CustomerCode of BatchID
                foreach (SAPWeightDef item in DefList)
                {
                    if ((item.WeightUnit.Equals(Site)) && (item.VolumnUnit.Equals(CustomerCode)))
                    {
                        ConnectionStr = item.ConnectionStr;
                        DBName = item.DBName;
                    }
                }

                if ((ConnectionStr == "") || (DBName == ""))
                {
                    ErrMsg = "The BU (Site:" + Site+", Customer:" + CustomerCode + ") connection setting is not exists ! ";
                    throw new Exception(ErrMsg);
                }

                //3.執行DB Data Check
                Execute.Process(ConnectionStr, DBName, BatchID);

                //4.Build Response Message
                ResponseDC dcresponse = Execute.BuildResponseMsg(ConnectionStr, DBName, dcItem, true, "");

                return dcresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                ResponseDC dcresponse = Execute.BuildResponseMsg("", "", dcItem, false, e.Message);

                return dcresponse;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        
        }
    }
}
