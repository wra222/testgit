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
using System.Collections.Generic;

namespace IMES.Sample
{
    /// <summary>
    /// 
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class GetModel : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        [WebMethod]
        public ModelQty[] GetModelQty()
        {
           
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            IList<ModelQty> ret = new List<ModelQty>();
            DateTime now = new DateTime(2014, 1, 1);
            Random rnd = new Random();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
               
                //1.检查传进来的参数
                //Execute.ValidateParameter(CustSN);

                //2.获取DB中的数据
                //ModelResponse modelReponse = Execute.modelResponseMsg(CustSN);

                //logger.DebugFormat("Reponse data:{0}", modelReponse.ToString());
                for (int i = 0; i <= 100; ++i)
                {
                    ret.Add(new ModelQty { Qty = rnd.Next(0, 200), Date = now.AddDays(i) });
                }
                return ret.ToArray();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);

                ret.Add(new ModelQty { Qty = 0, Date = now });
                return ret.ToArray();
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }  
        }
    }
}
