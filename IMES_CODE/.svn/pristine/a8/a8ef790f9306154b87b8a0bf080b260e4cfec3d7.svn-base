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
    /// Summary description for HRAttendQtyInfo
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HRAttendQtyInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public HRAttendQty[] GetHRAttendQtyInfo(string Site)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            //string TmpPeriodDate = null;
            IList<HRAttendQty> ret = new List<HRAttendQty>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数                

                //2.获取DB中的数据,第一筆固定是當天的
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select Period_Date, sum(AttendanceQty) as AttendanceQty,WorkCategory,Site
                                            from dbo.PSG_IMES_HRAttend
                                            where convert(varchar(8),Period_Date,112)=convert(varchar(8),GETDATE(),112)
                                            and Site='" + Site + "'" +
                                           " Group by Period_Date,Site,WorkCategory  " +                                         
                                           " order by Period_Date,Site,WorkCategory";
                IList<HRAttendQty> HRAttendQtyList = SqlHelper.ExecuteReader<HRAttendQty>(dbConnectStr, CommandType.Text, sqlText);

                for (int i = 0; i <= HRAttendQtyList.Count - 1; ++i)
                {
                    ret.Add(new HRAttendQty
                    {
                        Period_Date = HRAttendQtyList[i].Period_Date,
                        AttendanceQty = HRAttendQtyList[i].AttendanceQty,
                        WorkCategory = HRAttendQtyList[i].WorkCategory,
                        Site=HRAttendQtyList[i].Site
                    });
                }

                //2.第2筆逐一開始放整個月份
                string sqlTextDaily = @"select Period_Date, sum(AttendanceQty) as AttendanceQty,'D' as WorkCategory,Site
                                            from dbo.PSG_IMES_HRAttend
                                            where convert(varchar(6),Period_Date,112)=convert(varchar(6),GETDATE(),112)
                                            and Site='" + Site + "'" +
                                            " and convert(date,Period_Date) <> convert(date,GETDATE())" +
                                           "  Group by Period_Date,Site " +                                      
                                           "  order by Period_Date,Site";
                IList<HRAttendQty> HRAttendQtyList2 = SqlHelper.ExecuteReader<HRAttendQty>(dbConnectStr, CommandType.Text, sqlTextDaily);
                for (int j = 0; j <= HRAttendQtyList2.Count - 1; ++j)
                {
                    ret.Add(new HRAttendQty
                    {
                        Period_Date = HRAttendQtyList2[j].Period_Date,
                        AttendanceQty = HRAttendQtyList2[j].AttendanceQty,
                        WorkCategory = HRAttendQtyList2[j].WorkCategory,
                        Site = HRAttendQtyList2[j].Site
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

