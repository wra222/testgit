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
    /// Summary description for SAInOutStageLineInfo
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SAInOutStageLineInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public SAInOutStageLine[] GetSAInOutStageLineInfo()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            string TmpPeriodDate = null;
            IList<SAInOutStageLine> ret = new List<SAInOutStageLine>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数      select Right(convert(varchar(10),getdate(),120),5)           

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select top 1 convert(varchar,Period_Date) as Period_Date , Period_Date as SortPeriod_Date
                                             from PSG_IMES_InOut_StageLine
                                             where Stage='SA'
                                             order by SortPeriod_Date desc";
                IList<TmpPeriod_Date> TmpPeriod_DateList = SqlHelper.ExecuteReader<TmpPeriod_Date>(dbConnectStr, CommandType.Text, sqlText);

                if (TmpPeriod_DateList.Count > 0)
                {
                    TmpPeriodDate = TmpPeriod_DateList[0].Period_Date;
                }

                string sqlText1 = @"select Period_Date, Right(convert(varchar(19),Period_Date,120),19)  as NPeriod_Date,
                                   Stage,PdLine,InQty,OutQty
                                   from dbo.PSG_IMES_InOut_StageLine
                                   where convert(varchar,Period_Date)=" + "'" + TmpPeriodDate + "'";
                string sqlText2 = sqlText1 + @"AND Stage='SA'
                                              order by Period_Date,PdLine";
                IList<SAInOutStageLine> SAInOutStageLineList = SqlHelper.ExecuteReader<SAInOutStageLine>(dbConnectStr, CommandType.Text, sqlText2);

                for (int i = 0; i <= SAInOutStageLineList.Count - 1; ++i)
                {
                    ret.Add(new SAInOutStageLine
                    {
                        Period_Date = SAInOutStageLineList[i].Period_Date,
                        NPeriod_Date = SAInOutStageLineList[i].NPeriod_Date,
                        Stage = SAInOutStageLineList[i].Stage,
                        PdLine = SAInOutStageLineList[i].PdLine,
                        InQty = SAInOutStageLineList[i].InQty,
                        OutQty = SAInOutStageLineList[i].OutQty
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
