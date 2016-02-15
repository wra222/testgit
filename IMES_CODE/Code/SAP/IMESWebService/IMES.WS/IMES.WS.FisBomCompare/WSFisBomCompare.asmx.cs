﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using IMES.WS.Common;
using System.Web.Services.Protocols;
using System.Data;

namespace IMES.WS.FisBomCompare
{
    /// <summary>
    /// WSCancelBindDN 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSFisBomCompare : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public FisBomCompareResponse FisBomCompare(FisBomCompare ModelItem)
        {
            string connectionDB = "TSB_DBServer";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {            
                //1.檢查必要的input parameter
                Execute.ValidateParameter(connectionDB, ModelItem);

                //3.執行DB insert
                Execute.Process(connectionDB, ModelItem);

                //4.Build Response Message
                FisBomCompareResponse dnresponse = Execute.BuildResponseMsg(connectionDB, ModelItem, true, "");

                return dnresponse;
            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod(), e);
                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                FisBomCompareResponse pgiresponse = Execute.BuildResponseMsg(connectionDB, ModelItem, false, e.Message);
                
                return pgiresponse;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        
        }
    }
}
