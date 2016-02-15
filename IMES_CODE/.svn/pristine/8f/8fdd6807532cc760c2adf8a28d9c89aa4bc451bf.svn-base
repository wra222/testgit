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
    public class InOutInfoIPCInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public SumInputOutputQty GetInOutInfoIPCInfo()
        {

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {

                //1.检查传进来的参数                

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select top 1 'D' as Period, Period_Date, Site, BU, Plant,TargetInput,InputQty, NonInput, TargetOutput, OutputQty, NonOutput, Exec_Time
                                             from dbo.PSG_IMES_IPC_InOutQty_Sum
                                             order by Exec_Time desc";
                IList<SumInputOutputQty> SumInputOutputQtyList = SqlHelper.ExecuteReader<SumInputOutputQty>(dbConnectStr, CommandType.Text, sqlText);

                if (SumInputOutputQtyList.Count > 0)
                {
                    return SumInputOutputQtyList[0];
                }
                else
                {
                    return new SumInputOutputQty { InputQty = 0, NonInput = 0, NonOutput = 0, OutputQty = 0, TargetInput = 0, TargetOutput = 0, Exec_Time = DateTime.Now };
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);

                return new SumInputOutputQty { InputQty = 0, NonInput = 0, NonOutput = 0, OutputQty = 0, TargetInput = 0, TargetOutput = 0, Exec_Time = DateTime.Now };
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
