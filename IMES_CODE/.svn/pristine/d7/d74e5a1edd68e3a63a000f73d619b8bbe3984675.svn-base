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
    /// Summary description for OutSAOutInfo
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OutSAOutInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public OutSAOut[] GetOutSAOutInfo()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            IList<OutSAOut> ret = new List<OutSAOut>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数                

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select top 1 Period_Date,Station,OutQty,WIPQty,OKRate
                                   from dbo.PSG_IMES_InputOutput_StationTTL
                                   WHERE Station='15'
                                   ORDER BY Period_Date DESC";
                IList<OutSAOut> OutSAOutList = SqlHelper.ExecuteReader<OutSAOut>(dbConnectStr, CommandType.Text, sqlText);

                for (int i = 0; i <= OutSAOutList.Count - 1; ++i)
                {
                    ret.Add(new OutSAOut
                    {
                        Period_Date = OutSAOutList[i].Period_Date,
                        Station = OutSAOutList[i].Station,
                        OutQty = OutSAOutList[i].OutQty,
                        WIPQty = OutSAOutList[i].WIPQty,
                        OKRate = OutSAOutList[i].OKRate
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
