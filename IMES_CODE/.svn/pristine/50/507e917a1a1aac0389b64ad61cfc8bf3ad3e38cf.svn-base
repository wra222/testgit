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
using System.Configuration;
namespace IMES.Query.Implementation
{
    public class RBPC_Tool: MarshalByRefObject,IRBPC_Tool
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string RbpcConnectionString = ConfigurationManager.ConnectionStrings["RBPCDBServer"].ToString().Trim();

        public DataTable GetPermissionListByAccountId(string AccountId, string AppName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"sp_Query_GetPermissionListByAccountId";


                return SQLHelper.ExecuteDataFill(RbpcConnectionString,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@accountId", 64, AccountId),
                                                 SQLHelper.CreateSqlParameter("@appName", 64, AppName));
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

        public DataTable GetGroupList(string AccountId, string AppName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @" select '' ,a.name as [User Group],
                                                    a.id as [id],
                                                    CONVERT(VARCHAR(10), a.cdt, 111) as [Create Time],
                                                    CONVERT(VARCHAR(10), a.udt, 111) as [Update Time],
                                                    b.login as Author,
                                                    b.id as AuthorId,  a.descr as Comment, a.type
                                                    from Role a,Account b
                                                   
                                              where a.application=@AppName and a.editorId=@AccountId
                                                    and a.editorId=b.id";


                return SQLHelper.ExecuteDataFill(RbpcConnectionString,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@AccountId", 64,AccountId),
                                                 SQLHelper.CreateSqlParameter("@AppName",64, AppName));
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
