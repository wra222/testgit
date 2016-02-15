using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
namespace IMES.WS.PGIStatus
{
    /// <summary>
    /// WSPGIStatus 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSPGIStatus : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public List<PGIStatusResponse> PGIStatus(PGIStatus[] PGIItems)
        {
            string connectionDB_BK = "SD_DBServer_BK";
            
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {            
                //1.檢查必要的input parameter
                Execute.ValidateParameter(connectionDB_BK, PGIItems);

                //3.執行DB insert
                Execute.Process(connectionDB_BK, PGIItems);

                //4.Build Response Message
                List<PGIStatusResponse> pgiresponse = Execute.BuildResponseMsg(connectionDB_BK, PGIItems);

                return pgiresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<PGIStatusResponse> ResponseList = new List<PGIStatusResponse>();
                foreach (PGIStatus item in PGIItems)
                {
                    PGIStatusResponse response = new PGIStatusResponse();
                    string SerialNumber = "";
                    string State = "F";
                    response.SerialNumber = SerialNumber;
                    response.Result = State;
                    ResponseList.Add(response);
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
