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

namespace IMES.WS.MOPullRelease
{
    /// <summary>
    /// Summary description for WSMOPullRelease
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSMOPullRelease : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public MoPullReleaseResponse[] MoPullRelease(MoPullHeader[] moheaders)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string msgName = "MOPullRelease";
            string responseMsg = "MOPullReleaseResponse";
            List<MoPullReleaseResponse> responseList = new List<MoPullReleaseResponse>();
           
           foreach (MoPullHeader moheader in moheaders)
           {
                    try
                    {
                        WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                                        msgName,
                                                                                        string.IsNullOrEmpty(moheader.MoNumber) ? "" : moheader.MoNumber,
                                                                                        string.IsNullOrEmpty(moheader.IssuedQty) ? "" : moheader.IssuedQty,
                                                                                        string.IsNullOrEmpty(moheader.CurrentyIssueQty) ? "" : moheader.CurrentyIssueQty,
                                                                                        "",
                                                                                        "",
                                                                                        EnumMsgState.Received,
                                                                                        "");


                        //1.檢查必要的input parameter

                        Execute.ValidateParameter(moheader);

                        //2.檢查資料內容


                        //3.執行DB insert 
                        string ret = Execute.Process(moheader);

                        //4.Build Response Message
                        string errMsg = "";
                        MoPullReleaseResponse mrealease = Execute.BuildResponseMsg(moheader, ret, ref errMsg);
                        WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                                                       responseMsg,
                                                                                        string.IsNullOrEmpty(moheader.MoNumber) ? "" : moheader.MoNumber,
                                                                                       ret,
                                                                                       string.IsNullOrEmpty(moheader.SerialNumber) ? "" : moheader.SerialNumber,
                                                                                       "",
                                                                                       "",
                                                                                       EnumMsgState.Success,
                                                                                       errMsg);
                        responseList.Add(mrealease);
                    }
                    catch (Exception e)
                    {
                        BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                        string errMsg = e.Message;
                        MoPullReleaseResponse mrealease = Execute.BuildResponseMsg(moheader, "F", ref errMsg);
                        WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Send,
                                                                                       responseMsg,
                                                                                         string.IsNullOrEmpty(moheader.MoNumber) ? "" : moheader.MoNumber,
                                                                                        "F",
                                                                                       string.IsNullOrEmpty(moheader.SerialNumber) ? "" : moheader.SerialNumber,
                                                                                       "",
                                                                                       e.Message,
                                                                                       EnumMsgState.Fail,
                                                                                       "");
                        responseList.Add(mrealease);

                    }
                    finally
                    {
                        BaseLog.LoggingEnd(logger, methodName);
                    }

           }
           return responseList.ToArray();
          
        }
    }
}
