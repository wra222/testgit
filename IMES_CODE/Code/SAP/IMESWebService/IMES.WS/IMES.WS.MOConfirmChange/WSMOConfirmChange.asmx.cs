using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using System.Reflection;
using IMES.WS.Common;
using IMES.Query.DB;
using System.Web.Services.Protocols;

namespace IMES.WS.MOConfirmChange
{
    /// <summary>
    /// WSMOConfirmChange 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSMOConfirmChange : System.Web.Services.WebService
    {

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [SoapDocumentMethod(OneWay = true)]
        [WebMethod]
        public void MoConfirmChangeResult(MoConfirmChangeResult Result)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string msgName="MOConfirmChangeResult";
            try
            {
                if (Result == null)
                {
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                        "" ,
                                                         "" ,
                                                         "",
                                                        "",
                                                        "",
                                                        EnumMsgState.Received,
                                                        "");
                    throw new Exception("Result is Null");
                }
                else
                {
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                        String.IsNullOrEmpty(Result.MoNumber) ? "" : Result.MoNumber,
                                                        String.IsNullOrEmpty(Result.Result) ? "" : Result.Result,
                                                        String.IsNullOrEmpty(Result.SerialNumber) ? "" : Result.SerialNumber,
                                                        "",
                                                        "",
                                                        EnumMsgState.Received,
                                                        "");
                }
                //1.檢查必要的input parameter
                Execute.ValidateParameter(Result);

                //2.檢查資料內容
                Execute.CheckData(Result);

                //3.執行DB insert 
                Execute.Process(Result);

                //4.Build Response Message
                //MoReleaseResponse mrealease = Execute.BuildResponseMsg(moheader, true, "");
                //return mrealease;
                IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                    msgName,
                                                    String.IsNullOrEmpty(Result.MoNumber) ? "" : Result.MoNumber,
                                                    String.IsNullOrEmpty(Result.Result) ? "" : Result.Result,
                                                    String.IsNullOrEmpty(Result.SerialNumber) ? "" : Result.SerialNumber,
                                                    "",
                                                    Result.Result == "T" ? "" : Result.ErrorMessage,
                                                    Result.Result == "T" ? EnumMsgState.Success : EnumMsgState.Fail,
                                                    "");
                if (Result.Result != "T")
                {
                    UTL.SendMail("ebook result of MOConfirmChange fail (MO: " + Result.MoNumber + ", Result: " +Result.ErrorMessage + ")", "MO: " + Result.MoNumber + ", Result: " +Result.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                if (Result == null)
                {
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                      msgName,
                                                       "" ,
                                                      "",
                                                      "" ,
                                                      "",
                                                      e.Message,
                                                      EnumMsgState.Fail,
                                                      "");
                }
                else
                {
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                        String.IsNullOrEmpty(Result.MoNumber) ? "" : Result.MoNumber,
                                                        String.IsNullOrEmpty(Result.Result) ? "" : Result.Result,
                                                        String.IsNullOrEmpty(Result.SerialNumber) ? "" : Result.SerialNumber,
                                                        "",
                                                        e.Message,
                                                        EnumMsgState.Fail,
                                                        "");
                }
                UTL.SendMail(msgName + " fail", e.Message);
                //4.Build Response Error Message
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
    }
}
