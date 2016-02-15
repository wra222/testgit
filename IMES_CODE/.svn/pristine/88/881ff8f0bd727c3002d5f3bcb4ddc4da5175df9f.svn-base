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
    /// Summary description for PAKWIPStageInfo
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PAKWIPStageInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public PAKWIPStage[] GetPAKWIPStageInfo()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            IList<PAKWIPStage> ret = new List<PAKWIPStage>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数      select Right(convert(varchar(10),getdate(),120),5)           

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"Select * from
                                   (Select TOP 20 Period_Date, Right(convert(varchar(19),Period_Date,120),8) as NPeriod_Date,
                                   Stage,sum(WIPQty) as WIPQty
                                   from dbo.PSG_IMES_WIP_Station
                                   where convert(varchar(8),Period_Date,112)=convert(varchar(8),getdate(),112)
                                   AND Stage='PAK'
                                   group by Period_Date,Stage
                                   order by Period_Date DESC) As TempStage Order by TempStage.Period_Date";
                IList<PAKWIPStage> PAKWIPStageList = SqlHelper.ExecuteReader<PAKWIPStage>(dbConnectStr, CommandType.Text, sqlText);

                for (int i = 0; i <= PAKWIPStageList.Count - 1; ++i)
                {
                    ret.Add(new PAKWIPStage
                    {
                        Period_Date = PAKWIPStageList[i].Period_Date,
                        NPeriod_Date = PAKWIPStageList[i].NPeriod_Date,
                        Stage = PAKWIPStageList[i].Stage,
                        WIPQty = PAKWIPStageList[i].WIPQty
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
