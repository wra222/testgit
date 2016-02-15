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
using System.Data.SqlClient;
using IMES.Entity.Repository.Meta.IMESSKU;
using IMES.Entity.Infrastructure.Framework;
using IMES.Entity.Infrastructure.Interface;

namespace IMES.Query.Implementation
{
    public class SARepairDashboard : MarshalByRefObject, ISARepairDashboard
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<DataTable> GetQueryResult(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            List<DataTable> dts = new List<DataTable>();
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    DataTable dt1 = SQLHelper.ExecuteDataFill(Connection, System.Data.CommandType.StoredProcedure,
                                                              "sp_Query_SARepairDashboard",
                                                              new SqlParameter("@type", "1"));

                    DataTable dt2 = SQLHelper.ExecuteDataFill(Connection, System.Data.CommandType.StoredProcedure,
                                                 "sp_Query_SARepairDashboard",
                                                 new SqlParameter("@type", "2"));
                    DataTable dt3 = SQLHelper.ExecuteDataFill(Connection, System.Data.CommandType.StoredProcedure,
                                          "sp_Query_SARepairDashboard",
                                          new SqlParameter("@type", "3"));
                    dts.Add(dt1);
                    dts.Add(dt2);
                    dts.Add(dt3);
                    return dts;
                }
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