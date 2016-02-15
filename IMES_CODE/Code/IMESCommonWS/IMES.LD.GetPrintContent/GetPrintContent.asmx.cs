using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using log4net;

namespace IMES.LD
{
    /// <summary>
    /// Summary description for GetPrintContent
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class GetPrintContent : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
        /// <summary>
        /// 获得DB查询的SN、PN、Warranty、MN信息
        /// Stautus>0 =>success and Status<0 fail
        /// Status=-1 => can't find data
        /// Status=-2 => error
        /// Status=1 => success
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        [WebMethod]
        public PrintResponse GetPrintResponse(string SN)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(SN:{1})", methodName, SN);
            try
            {
                //1.检查传进来的参数
                Execute.ValidateParameter(SN);

                //2.获取DB中的数据
                PrintResponse printResponse = Execute.GetPrintContentInfo(SN);
                logger.DebugFormat("Reponse data:{0}", printResponse.ToString());
                return printResponse;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                PrintResponse printResponse = new PrintResponse();
                printResponse.SN = SN;
                printResponse.PN = "";
                printResponse.Warranty = "";
                printResponse.MN = "";
                printResponse.Status = -2;
                printResponse.ErrorMsg = e.Message;

                logger.DebugFormat("Reponse data:{0}", printResponse.ToString());

                return printResponse;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
        }

        /// <summary>
        /// 记录ProductLog
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="Status">1:Pass/0:Fail</param>
        [WebMethod]
        public void WritePrintLog(string SN, int Status)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(SN:{1} Status:{2})", methodName, SN, Status.ToString());
            try
            {
                //1.检查传进来的参数
                Execute.ValidateParameter(SN, Status);

                //2写入DB数据
                Execute.WriteProductLogBySN(SN, Status);

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);                
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
        }
    }
}
