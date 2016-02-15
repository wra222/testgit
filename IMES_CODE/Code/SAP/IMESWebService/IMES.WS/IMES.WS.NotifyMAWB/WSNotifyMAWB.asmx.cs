using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
using System.Web.Configuration;
namespace IMES.WS.NotifyMAWB
{
    /// <summary>
    /// WSPGIStatus 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSNotifyMAWB : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public List<NotifyMAWBResponse> NotifyMAWB(NotifyMAWB[] MAWBItems)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //切換 MAWB temp data
                string SwitchMAWBTempDataToCQ = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["SwitchMAWBTempDataToCQ"]) ? "" : WebConfigurationManager.AppSettings["SwitchMAWBTempDataToCQ"];
                string connectionDB_BK = "";
                if (SwitchMAWBTempDataToCQ == "Y")
                {
                    connectionDB_BK = "SD_DBServer_BK_CQ";
                }
                else {
                    connectionDB_BK = "SD_DBServer_BK";
                }
                string BatchId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                //1.檢查必要的input parameter
                Execute.ValidateParameter(connectionDB_BK, MAWBItems, BatchId);

                //3.執行DB insert
                Execute.Process(connectionDB_BK, MAWBItems, BatchId);

                //4.Build Response Message
                List<NotifyMAWBResponse> pgiresponse = Execute.BuildResponseMsg(connectionDB_BK, BatchId);

                return pgiresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<NotifyMAWBResponse> ResponseList = new List<NotifyMAWBResponse>();
                foreach (NotifyMAWB item in MAWBItems)
                {
                    string connectionDB = "SD_DBServer";
                    int dbIndex = 0;
                    NotifyMAWBResponse response = new NotifyMAWBResponse();
                    string SerialNumber = item.SerialNumber;
                    string MAWB = item.MAWB;
                    string State = "F";
                    response.SerialNumber = SerialNumber;
                    response.MAWB = MAWB;
                    response.Result = State;
                    response.ErrorText = e.ToString().Substring(0,255);
                    ResponseList.Add(response);

                    SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                            EnumMsgCategory.Response,
                                            "NotifyMAWBResponse",
                                            string.IsNullOrEmpty(item.MAWB) ? "" : item.MAWB,
                                            "",
                                            string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                            "",
                                            "",
                                            EnumMsgState.Fail,
                                            e.ToString().Substring(0, 255));
                }
                return ResponseList;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        
        }
    }
}
