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
    public class DailyOutInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        [WebMethod]
        public DailyOutputQty[] GetDailyOutInfo()
        {
           
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            IList<DailyOutputQty> ret = new List<DailyOutputQty>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
               
                //1.检查传进来的参数                

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select Right(convert(varchar(10),Period_Date,111),5) as NPeriod_Date,sum(TotalQty) as TotalQty,AttendanceQty
                                   from dbo.PSG_IMES_CQ_OutQty_D
                                   where convert(varchar(6),Period_Date,112)=convert(varchar(6),getdate(),112)
                                   Group by Period_Date,AttendanceQty                                   
                                   order by Period_Date,AttendanceQty";
                IList<DailyOutputQty> DailyOutputQtyList =SqlHelper.ExecuteReader<DailyOutputQty>(dbConnectStr, CommandType.Text, sqlText);
              
                for (int i = 0; i <= DailyOutputQtyList.Count-1; ++i)
                {
                    ret.Add(new DailyOutputQty
                    {
                        NPeriod_Date = DailyOutputQtyList[i].NPeriod_Date,
                        TotalQty = DailyOutputQtyList[i].TotalQty,
                        AttendanceQty = DailyOutputQtyList[i].AttendanceQty

                    });
                }

 //               string sqlTextNow = @"select Right(convert(varchar(10),Period_Date,111),5) as NPeriod_Date,sum(TotalQty) as TotalQty,AttendanceQty
 //                                  from dbo.PSG_IMES_InOutQtyNow
 //                                  where convert(varchar(6),Period_Date,112)=convert(varchar(6),getdate(),112)
 //                                  Group by Period_Date,AttendanceQty                                   
 //                                  order by Period_Date,AttendanceQty";
 //               IList<DailyOutputQty> DailyOutputQtyList2 = SqlHelper.ExecuteReader<DailyOutputQty>(dbConnectStr, CommandType.Text, sqlTextNow);
//
//                for (int j = 0; j <= DailyOutputQtyList2.Count - 1; ++j)
//                {
//                    ret.Add(new DailyOutputQty
 //                   {
 //                       NPeriod_Date = DailyOutputQtyList2[j].NPeriod_Date,
 //                       TotalQty = DailyOutputQtyList2[j].TotalQty,
 //                       AttendanceQty = DailyOutputQtyList2[j].AttendanceQty
//
 //                   });
 //               }


                return ret.ToArray();              
                
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);

              //  return new DailyOutputQty { Period = "", Period_Date = DateTime.Now, Site = "ICC", BU = "HP", Plant = "CP60", ProdType = "", TotalQty = 0, Exec_Time = DateTime.Now };
                return ret.ToArray();
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }  
        }
    }
}
