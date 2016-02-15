using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
namespace IMES.WS.NotifyECOModel
{
    /// <summary>
    /// WSNotifyECOModel 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSNotifyECOModel : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public List<NotifyECOModelResponse> NotifyECOModel(NotifyECOModel[] ECOModelItems)
        {
            string connectionDB = "PP_DBServer";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {            
                //1.檢查必要的input parameter
                Execute.ValidateParameter(connectionDB, ECOModelItems);

                //3.執行DB insert
                Execute.Process(connectionDB, ECOModelItems);

                //4.Build Response Message
                List<NotifyECOModelResponse> ecoresponse = Execute.BuildResponseMsg(connectionDB, ECOModelItems, true, "");

                return ecoresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<NotifyECOModelResponse> ecoresponse = Execute.BuildResponseMsg(connectionDB, ECOModelItems, false , e.Message);

                /*
                List<NotifyECOModelResponse> ResponseList = new List<NotifyECOModelResponse>();
                foreach (NotifyECOModel item in ECOModelItems)
                {
                    NotifyECOModelResponse response = new NotifyECOModelResponse();
                    response.TxnId = (item.TxnId == null ? "" : item.TxnId);
                    response.ECONo = (item.ECONo == null ? "" : item.ECONo);
                    response.Model = (item.Model == null ? "" : item.Model);
                    response.Result = "F";
                    response.Message = e.Message;
                    ResponseList.Add(response);
                }
                 */
                return ecoresponse;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        
        }
    }
}
