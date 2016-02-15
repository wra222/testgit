using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
namespace IMES.WS.NotifyStdWeight
{
    /// <summary>
    /// WSNotifyStdWeight 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSNotifyStdWeight : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public List<NotifyStdWeightResponse> NotifyStdWeight(NotifyStdWeight[] StdWeightItems)
        {
            string connectionDB = "SD_DBServer";
            string connectionDB_EDI = "SD_DBServer_EDI";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {            
                //1.檢查必要的input parameter
                Execute.ValidateParameter(connectionDB, StdWeightItems);

                //3.執行DB insert
                Execute.Process(connectionDB, connectionDB_EDI, StdWeightItems);

                //4.Build Response Message
                List<NotifyStdWeightResponse> pgiresponse = Execute.BuildResponseMsg(connectionDB, StdWeightItems);

                return pgiresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<NotifyStdWeightResponse> ResponseList = new List<NotifyStdWeightResponse>();
                foreach (NotifyStdWeight item in StdWeightItems)
                {
                    NotifyStdWeightResponse response = new NotifyStdWeightResponse();
                    string SerialNumber = "";
                    string State = "F";
                    response.SerialNumber = SerialNumber;
                    response.Plant = item.Plant;
                    response.Model = item.Model;
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
