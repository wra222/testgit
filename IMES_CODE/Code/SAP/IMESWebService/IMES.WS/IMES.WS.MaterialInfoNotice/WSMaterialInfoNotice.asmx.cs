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
using System.Web.Services.Protocols;
namespace IMES.WS.MaterialInfoNotice
{
    /// <summary>
    /// WSMaterialInfoNotice 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSMaterialInfoNotice : System.Web.Services.WebService
    { 
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        [SoapDocumentMethod(OneWay = true)]
        [WebMethod]
        public void MaterialInfoNotice(MaterialInfoNoticeMsg materialNotice)
        {
                string methodName = MethodBase.GetCurrentMethod().Name;
                string msgName = "MaterialInfoNotice";
                BaseLog.LoggingBegin(logger, methodName);
                try
                {
                    WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                            msgName,
                                                                            string.IsNullOrEmpty(materialNotice.SerialNumber) ? "" : materialNotice.SerialNumber,
                                                                            "",
                                                                            "",
                                                                            "",
                                                                            "",
                                                                            EnumMsgState.Received,
                                                                            "");
                    Execute.ValidateParameter(materialNotice);
                    Execute.Process(materialNotice);
                    WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                            msgName,
                                                                            string.IsNullOrEmpty(materialNotice.SerialNumber) ? "" : materialNotice.SerialNumber,
                                                                            "",
                                                                            "",
                                                                            "",
                                                                            "",
                                                                            EnumMsgState.Success,
                                                                            "");
                }
                catch (Exception e)
                {
                    BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                    WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                          msgName,
                                                                          string.IsNullOrEmpty(materialNotice.SerialNumber) ? "" : materialNotice.SerialNumber,
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          e.Message,
                                                                          EnumMsgState.Fail,
                                                                          "");
                    
                }
                finally
                {
                    BaseLog.LoggingEnd(logger, methodName);
                }
        }
    }
}
