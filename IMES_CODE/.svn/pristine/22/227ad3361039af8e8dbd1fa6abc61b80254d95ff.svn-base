using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace testWS4
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
     [ServiceBehavior(Namespace = "http://inventec.com/WS/")]
    [AspNetCompatibilityRequirements(RequirementsMode =
                       AspNetCompatibilityRequirementsMode.Required)]
    public class GetModel : IGetModel
    {
        public ModelQty[] GetModelQty()
        {

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            IList<ModelQty> ret = new List<ModelQty>();
            DateTime now = new DateTime(2014, 1, 1);
            Random rnd = new Random();            
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

                ret.Add(new ModelQty { Qty = 0, Date = now });
                return ret.ToArray();
            }
            finally
            {
               
            }
        }

       
    }
}
