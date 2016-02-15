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

namespace IMES.EoLA
{
    /// <summary>
    /// 接收EoLA(天津)传进来的参数（CustSN）,传出对象参数（ProductID,CustSN,Model,Family,Customer,ErrorCode,ErrorMessage）
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GetModel : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// ErrorCode=0 => ok
        /// ErrorCode =-1 => can't find data
        /// ErrorCode =-2 => error 
        /// </summary>
        /// <param name="CustSN"></param>
        /// <returns></returns>
        [WebMethod]
        public ModelResponse GetModelResponse(string CustSN)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(CustSN:{1})", methodName, CustSN);
            try
            {
                //1.检查传进来的参数
                Execute.ValidateParameter(CustSN);

                //2.获取DB中的数据
                ModelResponse modelReponse = Execute.modelResponseMsg(CustSN);
                logger.DebugFormat("Reponse data:{0}", modelReponse.ToString());
                return modelReponse;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                ModelResponse modelReponse = new ModelResponse();

                modelReponse.CustSN = CustSN;
                modelReponse.ProductID = "";
                modelReponse.Model = "";
                modelReponse.Family = "";
                modelReponse.Customer = "";
                modelReponse.ErrorCode = -2;
                modelReponse.Message = e.Message;
                logger.DebugFormat("Reponse data:{0}", modelReponse.ToString());
                return modelReponse;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
