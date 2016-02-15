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
    /// Summary description for OutFAOutLineInfo
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OutFAOutLineInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public OutFAOutLine[] GetOutFAOutLineInfo()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            string TmpPeriodDate = null;
            IList<OutFAOutLine> ret = new List<OutFAOutLine>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数                

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select top 1 convert(varchar,Period_Date) as Period_Date , Period_Date as SortPeriod_Date
                                             from PSG_IMES_InputOutput_Station
                                             where Station='67'
                                             order by SortPeriod_Date desc";
                IList<TmpPeriod_Date> TmpPeriod_DateList = SqlHelper.ExecuteReader<TmpPeriod_Date>(dbConnectStr, CommandType.Text, sqlText);

                if (TmpPeriod_DateList.Count > 0)
                {
                    TmpPeriodDate = TmpPeriod_DateList[0].Period_Date;
                }

                string sqlText1 = @"select Period_Date,Station,PdLine,OutQty,WIPQty
                                   from PSG_IMES_InputOutput_Station
                                   WHERE Station='67'
                                   AND convert(varchar,Period_Date)=" + "'" + TmpPeriodDate + "'";
                IList<OutFAOutLine> OutFAOutLineList = SqlHelper.ExecuteReader<OutFAOutLine>(dbConnectStr, CommandType.Text, sqlText1);

                for (int i = 0; i <= OutFAOutLineList.Count - 1; ++i)
                {
                    ret.Add(new OutFAOutLine
                    {
                        Period_Date = OutFAOutLineList[i].Period_Date,
                        Station = OutFAOutLineList[i].Station,
                        PdLine = OutFAOutLineList[i].PdLine,
                        OutQty = OutFAOutLineList[i].OutQty,
                        WIPQty = OutFAOutLineList[i].WIPQty
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
