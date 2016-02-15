using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
namespace IMES.WS.MoQuery
{
    /// <summary>
    /// WSMoQuery 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSMoQuery : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public MoQueryResponse MoQuery(MoQuery moquery)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string msgName = "MOQuery";
            string responseMsg = "MOQueryResponse";
            try
            {
                WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                               msgName,
                                                                               string.IsNullOrEmpty(moquery.MoNumber) ? "" : moquery.MoNumber,
                                                                               "",
                                                                               "",
                                                                               "",
                                                                               "",
                                                                               EnumMsgState.Received,
                                                                               "");
                Execute.ValidateParameter(moquery);

                //Execute.BuildResponseMsg(moquery.MoNumber);

                //4.Build Response Message
                MoQueryResponse moresponse = Execute.BuildResponseMsg(moquery.MoNumber);
                WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                                               responseMsg,  
                                                                               string.IsNullOrEmpty( moquery.MoNumber)? "":moquery.MoNumber , 
                                                                               moresponse.Result,
                                                                               "", 
                                                                               "",
                                                                               "",
                                                                               EnumMsgState.Success,  
                                                                               moresponse.HoldResult);
                return moresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                MoQueryResponse moresponse = new MoQueryResponse() { MoNumber = moquery.MoNumber, HoldResult = "N", Result = "NotFound" };
                WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send, 
                                                                            responseMsg, 
                                                                            string.IsNullOrEmpty( moquery.MoNumber)? "":moquery.MoNumber , 
                                                                            "NotFound", 
                                                                            "",
                                                                            "", 
                                                                            e.Message, 
                                                                             EnumMsgState.Fail, 
                                                                             "N");
                
                return moresponse;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        
        }
    }
}
