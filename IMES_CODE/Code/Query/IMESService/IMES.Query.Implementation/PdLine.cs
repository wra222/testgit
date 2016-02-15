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
    public class PdLine : MarshalByRefObject,IPdLine
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DataTable GetPdLine(string customer, IList<string> lstProcess, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @" SELECT Line,Descr FROM Line (NOLOCK) WHERE CustomerID=@customer "; // AND Stage IN (@process) ORDER BY 1";                
                SQLText += string.Format(" AND Stage IN ('{0}')", string.Join("','", lstProcess.ToArray()));
                SQLText += " ORDER BY 1";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@customer", 32, customer, ParameterDirection.Input));
                                                 //SQLHelper.CreateSqlParameter("@process", 32, process, ParameterDirection.Input));
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
