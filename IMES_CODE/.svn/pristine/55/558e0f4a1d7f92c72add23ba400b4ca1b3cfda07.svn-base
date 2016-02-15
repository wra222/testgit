using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;

namespace IMES.Query.Implementation
{
    public class PAK_COARMAQuery : MarshalByRefObject, IPAK_COARMAQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetCOARMAQueryResult(string Connection, String RMA, String COA, Boolean NumType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                if (NumType)
                {
                    sb.AppendLine("SELECT COASN,IECPN,RMA,Cdt FROM HPIMES..COARMA (nolock)");
                    if (RMA.Length < 16)
                    {
                        sb.AppendLine(string.Format("where RMA = {0}", RMA));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("where RMA in(select value from dbo.fn_split('{0}',','))", RMA));
                    }
                }
                else
                {
                    if (COA.Length < 16)
                    {
                        sb.AppendLine(string.Format("SELECT COASN,IECPN,RMA,Cdt FROM HPIMES..COARMA (nolock) where COASN like '{0}'", COA));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("select convert(varchar,convert(numeric,value)) as COASN into #1 from dbo.fn_split('{0}',',')", COA));
                        sb.AppendLine("update #1  set COASN='00'+COASN WHERE LEN(COASN)=12");
                        sb.AppendLine("update #1  set COASN='000'+COASN WHERE LEN(COASN)=11");
                        sb.AppendLine("SELECT COASN,IECPN,RMA,Cdt FROM HPIMES..COARMA (nolock)");
                        sb.AppendLine("where COASN in(select COASN from #1)");
                    }
                }


                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText);
            }
            catch (Exception e)
            {

                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
    }
}
