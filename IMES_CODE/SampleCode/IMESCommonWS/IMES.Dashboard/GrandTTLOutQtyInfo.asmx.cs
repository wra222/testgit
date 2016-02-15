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
using System.Configuration;

namespace IMES.Dashboard
{
    /// <summary>
    /// Summary description for GrandTTLOutQty
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GrandTTLOutQtyInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public GrandTTLOutQty[] GetGrandTTLOutQtyInfo(string Site)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            int lastQty = 0;
            IList<GrandTTLOutQty> ret = new List<GrandTTLOutQty>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数      select Right(convert(varchar(10),getdate(),120),5)           

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select Right(convert(varchar(10),GPeriod_Date,111),5) as GPeriod_Date,GTotalQty
                                   from dbo.PSG_IMES_InOutGTTLQty
                                   where convert(varchar(6),GPeriod_Date,112)=convert(varchar(6),getdate(),112)
                                   and Site='" + Site + "'" +
                                   " ORDER by GPeriod_Date ";
                IList<GrandTTLOutQty> GrandTTLOutQtyList = SqlHelper.ExecuteReader<GrandTTLOutQty>(dbConnectStr, CommandType.Text, sqlText);

                for (int i = 0; i <= GrandTTLOutQtyList.Count - 1; ++i)
                {
                    ret.Add(new GrandTTLOutQty
                    {
                            
                            GPeriod_Date = GrandTTLOutQtyList[i].GPeriod_Date,
                            GTotalQty = GrandTTLOutQtyList[i].GTotalQty
  
                    });
                }
                return ret.ToArray();

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);

                return ret.ToArray();
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
