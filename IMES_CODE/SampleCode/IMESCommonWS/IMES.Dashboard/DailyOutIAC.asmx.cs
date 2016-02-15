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
    /// 
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DailyOutIACInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DailyOutIACQty[] GetDailyOutIACInfo(string Site)
        {

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            IList<DailyOutIACQty> ret = new List<DailyOutIACQty>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {

                //1.检查传进来的参数
                //Execute.ValidateParameter(Site);

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["IACDBConnect"].ConnectionString;
                string sqlText = @"select left(convert(nvarchar,Period_Date,101),5) as NPeriod_Date,sum(OutputQty) as TotalQty
                                   from dbo.IAC_IMES_InOutQty
                                   where convert(varchar(6),Period_Date,112)=convert(varchar(6),getdate(),112)
                                   and  convert(date,Period_Date) < convert(date,getdate())
                                   and Site='" + Site + "'" +
                                   "Group by Period_Date " +
                                   "order by Period_Date";
                IList<DailyOutIACQty> DailyOutIACQtyList = SqlHelper.ExecuteReader<DailyOutIACQty>(dbConnectStr, CommandType.Text, sqlText);

                for (int i = 0; i <= DailyOutIACQtyList.Count - 1; ++i)
                {
                    ret.Add(new DailyOutIACQty
                    {
                        NPeriod_Date = DailyOutIACQtyList[i].NPeriod_Date,
                        TotalQty = DailyOutIACQtyList[i].TotalQty

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


        [WebMethod]
        public IACInOutQty[] GetIACInOutQtyInfo(string Site)
        {

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            string TmpPeriodDate = DateTime.Now.ToString("yyyy/MM/dd");
            IList<IACInOutQty> ret = new List<IACInOutQty>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {

                //1.检查传进来的参数
                //Execute.ValidateParameter(Site);

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["IACDBConnect"].ConnectionString;

                string sqlText = @"select top 1 Period_Date
                                   from dbo.IAC_IMES_InOutQty
                                   where Site='" + Site + "'" +
                                   " order by Period_Date Desc";
                IList<IACPeriod_Date> IACPeriod_DateList = SqlHelper.ExecuteReader<IACPeriod_Date>(dbConnectStr, CommandType.Text, sqlText);

                if (IACPeriod_DateList.Count > 0)
                {
                    TmpPeriodDate = IACPeriod_DateList[0].Period_Date.ToString("yyyy/MM/dd");
                }


                string sqlText1 = @"select left(convert(nvarchar,Period_Date,101),5) as Period_Date, sum(InputQty) as InputQty, sum(OutputQty) as OutputQty
                                   from dbo.IAC_IMES_InOutQty
                                   where Period_Date = " + "'" + TmpPeriodDate + "'" +
                                "  AND Site='" + Site + "'" +
                                   " Group by Period_Date " +
                                   " order by Period_Date ";

                IList<IACInOutQty> IACInOutQtyList = SqlHelper.ExecuteReader<IACInOutQty>(dbConnectStr, CommandType.Text, sqlText1);

                for (int i = 0; i <= IACInOutQtyList.Count - 1; ++i)
                {
                    ret.Add(new IACInOutQty
                    {
                        Period_Date = IACInOutQtyList[i].Period_Date,
                        InputQty = IACInOutQtyList[i].InputQty,
                        OutputQty = IACInOutQtyList[i].OutputQty

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
