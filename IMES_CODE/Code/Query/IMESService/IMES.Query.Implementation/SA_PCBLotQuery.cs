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
    public class SA_PCBLotQuery : MarshalByRefObject, ISA_PCBLotQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetPCBLotQueryResult(string Connection, String LotNo, String PCBNo, Boolean NumType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select LotNo,PCBNo,Status,Editor,Cdt,Udt from PCBLot (nolock)");
                if (NumType)
                {
                    if (LotNo.Length < 16)
                    {
                        sb.AppendLine(string.Format("where LotNo = {0}", LotNo));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("where LotNo in(select value from dbo.fn_split('{0}',','))", LotNo));
                    }
                }
                else
                {
                    if (PCBNo.Length<16)
                    {
                        sb.AppendLine(string.Format("where PCBNo = '{0}'", PCBNo));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("where PCBNo in(select value from dbo.fn_split('{0}',','))", PCBNo));
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
