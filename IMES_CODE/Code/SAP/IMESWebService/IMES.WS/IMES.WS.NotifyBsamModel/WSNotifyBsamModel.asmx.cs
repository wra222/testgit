using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
namespace IMES.WS.NotifyBsamModel
{
    /// <summary>
    /// WSNotifyBsamModel 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSNotifyBsamModel : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public List<NotifyBsamModelResponse> NotifyBsamModel(NotifyBsamModel[] BsamModelItems)
        {
            string connectionDB_BK = "SD_DBServer";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {            
                //1.檢查必要的input parameter
                Execute.ValidateParameter(connectionDB_BK, BsamModelItems);

                //3.執行DB insert
                Execute.Process(connectionDB_BK, BsamModelItems);

                //4.Build Response Message
                List<NotifyBsamModelResponse> pgiresponse = Execute.BuildResponseMsg(connectionDB_BK, BsamModelItems);

                return pgiresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<NotifyBsamModelResponse> ResponseList = new List<NotifyBsamModelResponse>();
                foreach (NotifyBsamModel item in BsamModelItems)
                {
                    NotifyBsamModelResponse response = new NotifyBsamModelResponse();
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
