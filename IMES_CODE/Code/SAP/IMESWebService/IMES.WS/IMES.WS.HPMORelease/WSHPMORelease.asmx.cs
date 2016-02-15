using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.WS.Common;
using IMES.Query.DB;


namespace IMES.WS.HPMORelease
{
    /// <summary>
    /// IMoRelease 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSHPMORelease :System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        [WebMethod]
        public MoReleaseResponse MoRelease(MoHeader moheader, MoItemDetail[] moitems)
        {
            string connectionDB = "PP_DBServer";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string msgName = "MORelease";
            string responseMsg = "MOReleaseResponse";

            if (string.IsNullOrEmpty(moheader.TCode) || 
                moheader.TCode == "CO02" || 
                moheader.TCode == "BADI")
            {
                msgName = "MOChange";
                responseMsg = "MOChangeResponse";
            }
           
            DBMoHeader dbheader = new DBMoHeader(moheader);
            try
            {
                string BuildOutMtl = "";
                string MaterialGroup = "";
                string TotalQty = "";
                string Status = "";
                BuildOutMtl = string.IsNullOrEmpty(moheader.BuildOutMtl) ? "" : moheader.BuildOutMtl;
                MaterialGroup = string.IsNullOrEmpty(moheader.MaterialGroup) ? "" : moheader.MaterialGroup;
                TotalQty = string.IsNullOrEmpty(moheader.TotalQty) ? "" : moheader.TotalQty;
                Status = string.IsNullOrEmpty(moheader.Status) ? "" : moheader.Status;

                WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, dbIndex, 
                                                        EnumMsgCategory.Receive,
                                                        msgName,
                                                        string.IsNullOrEmpty(moheader.MoNumber) ? "" : moheader.MoNumber,
                                                        string.IsNullOrEmpty(moheader.Status) ? "" : moheader.Status,                                                                              
                                                        string.IsNullOrEmpty(moheader.SerialNumber) ? "" : moheader.SerialNumber,
                                                        "",
                                                        "",
                                                        EnumMsgState.Received,
                                                        BuildOutMtl + ";" + MaterialGroup + ";" + TotalQty + ";" + Status);
               
                List<DBMoItemDetail> lstdbItemdetail = new List<DBMoItemDetail>();
                foreach (MoItemDetail item in moitems)
                {
                    lstdbItemdetail.Add(new DBMoItemDetail(item));
                }
                //1.檢查必要的input parameter
                DBMoItemDetail[] dbitemsArr = lstdbItemdetail.ToArray();
                Execute.ValidateParameter(dbheader, lstdbItemdetail.ToArray());

                //2.檢查資料內容 
                Execute.CheckData(dbheader, dbitemsArr);

                //3.執行DB insert 
                Execute.Process(dbheader, dbitemsArr, connectionDB);

                //4.Build Response Message
                MoReleaseResponse mrealease = Execute.BuildResponseMsg(moheader, true, "");
                WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                    EnumMsgCategory.Send,
                                                    responseMsg,
                                                    string.IsNullOrEmpty(moheader.MoNumber) ? "" : moheader.MoNumber,
                                                    string.IsNullOrEmpty(moheader.Status) ? "" : moheader.Status,
                                                    string.IsNullOrEmpty(moheader.SerialNumber) ? "" : moheader.SerialNumber,
                                                    "",
                                                    "",
                                                    EnumMsgState.Success,
                                                    "");
                return mrealease;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                
                MoReleaseResponse mrealease = Execute.BuildResponseMsg(moheader, false, e.Message);
                WS.Common.SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                                        EnumMsgCategory.Receive,
                                                        responseMsg,
                                                        string.IsNullOrEmpty(moheader.MoNumber) ? "" : moheader.MoNumber,
                                                        string.IsNullOrEmpty(moheader.Status) ? "" : moheader.Status,
                                                        string.IsNullOrEmpty(moheader.SerialNumber) ? "" : moheader.SerialNumber,
                                                        "",
                                                        e.Message,
                                                        EnumMsgState.Fail,
                                                        "");
                return mrealease;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

           
        }
      
    }
}
