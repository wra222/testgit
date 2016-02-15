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
using System.Data;

namespace IMES.WS.MOConfirm
{
    /// <summary>
    /// Summary description for MoConfirmResult
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSMOConfirm : System.Web.Services.WebService
    {

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static object objLock =new object();
        [SoapDocumentMethod(OneWay = true)]
        [WebMethod]
        public void MoConfirmResult(MoConfirmResult[] Result)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string msgName="MOConfirmResult";
            try
            {
                string errorMsg = "";
                string returnMsg = "";
                foreach (MoConfirmResult item in Result)
                {
                    returnMsg = "";
                    if (item.Result == "T" && item.ErrorMessage.Trim() != "")
                    {
                        returnMsg = item.ErrorMessage.Trim().Substring(18, (item.ErrorMessage.Trim().Length - 18));
                    }
                    if (item.Result == "T" && returnMsg != "") 
                    {
                        errorMsg = errorMsg + item.ErrorMessage + "(Success)" + "\n";
                    }
                    else if (item.Result != "T")
                    {
                        errorMsg = errorMsg + item.ErrorMessage + "(Fail)" + "\n";
                    }

                }
                if (Result != null && Result.Length > 0)
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                        String.IsNullOrEmpty(Result[0].MoNumber) ? "" : Result[0].MoNumber,
                                                        String.IsNullOrEmpty(Result[0].Result) ? "" : Result[0].Result,
                                                        String.IsNullOrEmpty(Result[0].SerialNumber) ? "" : Result[0].SerialNumber,
                                                        "",
                                                        //Result = "T" also has errorMsg!
                                                        //Result[0].Result == "T" ? Result[0].ErrorMessage.Trim().Substring(18, (Result[0].ErrorMessage.Trim().Length - 18)) : Result[0].ErrorMessage,
                                                        errorMsg,
                                                        EnumMsgState.Received,
                                                        "");
                else
                {
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                         "",
                                                         "",
                                                       "",
                                                        "",
                                                        "",
                                                        EnumMsgState.Received,
                                                        "");
                    throw new Exception("MoConfirmResult[] lenght is 0");
                }
                //1.檢查必要的input parameter
                Execute.ValidateParameter(Result);

                //2.檢查資料內容
                Execute.CheckData(Result);

                lock (objLock)
                {
                    //3.執行DB insert 
                    Execute.Process(Result);

                    //4. Query SAP MO Status by SAP WebService
                    SAPClient.QuerySAPMOStatus(Result[0].MoNumber);
                }

                //4.Build Response Message
                //MoReleaseResponse mrealease = Execute.BuildResponseMsg(moheader, true, "");
                //return mrealease;
                
                IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive, 
                                                   msgName,
                                                   String.IsNullOrEmpty( Result[0].MoNumber) ? "":Result[0].MoNumber, 
                                                   String.IsNullOrEmpty( Result[0].Result)?"":Result[0].Result, 
                                                   String.IsNullOrEmpty( Result[0].SerialNumber)?"":Result[0].SerialNumber,
                                                   "",
                                                   //Result = "T" also has errorMsg!
                                                   //Result[0].Result == "T" ? "" : errorMsg,
                                                   errorMsg,
                                                   Result[0].Result == "T" ? EnumMsgState.Success : EnumMsgState.Fail, 
                                                   "");
               try
               {
                   DataTable dt = IMES.WS.Common.SQL.GetConfirmMoData(String.IsNullOrEmpty(Result[0].SerialNumber) ? "" : Result[0].SerialNumber,
                                                       String.IsNullOrEmpty(Result[0].MoNumber) ? "" : Result[0].MoNumber);
                   if (dt.Rows.Count > 0)
                   {
                       int DeliveredQty = (int)dt.Rows[0]["DeliveredQty"];
                       string Model = (string)dt.Rows[0]["Model"];
                       //Result = "T" also has errorMsg, need to send mail #2012/07/02
                       if (errorMsg != "")
                       {
                           BaseLog.LoggingInfo(logger, "ebook result of MOConfirm (MO:{0}, Model:{1}, Confirm Qty:{2})", Result[0].MoNumber, Model, DeliveredQty);
                           BaseLog.LoggingInfo(logger, "ebook result of MOConfirm Content:\nMO:{0},\nModel:{1},\nConfirm Qty:{2},\nResult:\n{3}", Result[0].MoNumber, Model, DeliveredQty, errorMsg);

                           UTL.SendMail("ebook result of MOConfirm (MO:" + Result[0].MoNumber + ", Model:" + Model + ", Confirm Qty:" + DeliveredQty + ")",
                                        "MO:" + Result[0].MoNumber + ",\nModel:" + Model + ",\nConfirm Qty:" + DeliveredQty + ",\nResult:\n" + errorMsg);
                       }                     
                   }
               }
               catch (Exception e)
               {
                   BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
               }
                finally
                {
                    BaseLog.LoggingEnd(logger, methodName);
                }
            }
            catch (Exception e)
            {
                if (Result != null && Result.Length > 0)
                    IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                        String.IsNullOrEmpty(Result[0].MoNumber) ? "" : Result[0].MoNumber,
                                                        String.IsNullOrEmpty(Result[0].Result) ? "" : Result[0].Result,
                                                        String.IsNullOrEmpty(Result[0].SerialNumber) ? "" : Result[0].SerialNumber,
                                                        "",
                                                       e.Message,
                                                        EnumMsgState.Fail,
                                                        "");
                else
                     IMES.WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                        msgName,
                                                         "",
                                                         "",
                                                       "",
                                                        "",
                                                       e.Message,
                                                        EnumMsgState.Fail,
                                                        "");

               
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);

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
