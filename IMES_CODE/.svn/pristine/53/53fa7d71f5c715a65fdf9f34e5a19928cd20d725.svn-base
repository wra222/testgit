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
    public class Model:MarshalByRefObject, IModel
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DataTable GetModel(string Family)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = "SELECT DISTINCT Model FROM Model (NOLOCK) WHERE Family=@Family  AND Model Like 'PC%'";                
                return SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                    System.Data.CommandType.Text,
                                                    SQLText,
                                                    SQLHelper.CreateSqlParameter("@Family", 32, Family, ParameterDirection.Input));
                                                 
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
