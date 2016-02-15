using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using IMES.LD;
using log4net;
using System.Text;
using System.Xml;

namespace IMESCommonWS.IMES.SARFID
{
    /// <summary>
    /// Summary description for RFIDWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RFIDWebService : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        [WebMethod]
        public MBInfoResponse GetMBInfo(string MessageID, string PCBNO, string Remark)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(SN:{1})", methodName, PCBNO);
            string txnId = DateTime.Now.ToString("yyyyMMddhhmmss.fff");//YT135AV
            try
            {
                //1.检查传进来的参数
                Execute.ValidateParameter(PCBNO);

                //2.获取DB中的数据
                MBInfoResponse mbinfo = Execute.GetMBInfoRFID(PCBNO);
                mbinfo.MessageID = txnId;
                logger.DebugFormat("Reponse data:{0}", mbinfo.ToString());
                return mbinfo;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                MBInfoResponse mbinfo = new MBInfoResponse();
                mbinfo.MessageID = txnId;
                mbinfo.MBSN = PCBNO;
                mbinfo.Status = "F";
                mbinfo.ErrorText = e.Message;
                mbinfo.Remark = "";
                logger.DebugFormat("Reponse data:{0}", mbinfo.ToString());

                return mbinfo;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

    }
}
