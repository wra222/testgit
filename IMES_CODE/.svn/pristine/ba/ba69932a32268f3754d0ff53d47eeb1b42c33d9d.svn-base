using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using UTL.SQL;
using UTL;
using log4net;
using System.Reflection;
using System.Collections;
using System.IO;



namespace UTL.SQL
{
    public class SQLStatement
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static public  DataTable executeSP(string connectionString,string SPName,params SqlParameter[] cmdParms)
        {
            DataTable Ret = new DataTable();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                return SQLHelper.ExecuteDataFill(connectionString, CommandType.StoredProcedure, SPName, cmdParms);
                            
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                return Ret;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
