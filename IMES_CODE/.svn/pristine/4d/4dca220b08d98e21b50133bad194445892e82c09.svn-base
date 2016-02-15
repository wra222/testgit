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
    public class Family :MarshalByRefObject, IFamily
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IFamily Members

        public DataTable GetFamily(string family, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = "select * from Family where Family=@family";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                                    System.Data.CommandType.Text,
                                                                    SQLText,
                                                                    SQLHelper.CreateSqlParameter("@family", 32, family, ParameterDirection.Input));
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

        public DataTable GetFamily(string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = "select * from Family ";
                return SQLHelper.ExecuteDataFill(DBConnection,
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

        public DataTable GetPCBFamily(string DBConnection)
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendLine("SELECT DISTINCT ");
            sb.AppendLine("Replace(UPPER(InfoValue),'B SIDE','') AS Family , ");
            sb.AppendLine("Replace(UPPER(InfoValue),'B SIDE','') AS Family ");
            sb.AppendLine("FROM PartInfo(nolock) ");
            sb.AppendLine("WHERE InfoType = 'MDL' AND CHARINDEX('B SIDE', UPPER(InfoValue)) > 0  ");
            sb.AppendLine("ORDER BY Replace(UPPER(InfoValue),'B SIDE','') ");
           
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     sb.ToString());
            return dt;
        
        
        }

        public DataTable GetPCBFamily(string DBConnection,string TableName)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (TableName == "Part")
                {
                    sb.AppendLine("SELECT DISTINCT ");
                    sb.AppendLine("Descr AS Family , ");
                    sb.AppendLine("Descr AS Family ");
                    sb.AppendLine("FROM Part(nolock) ");
                    sb.AppendLine("WHERE BomNodeType='MB'  ");
                    sb.AppendLine("ORDER BY Descr ");
                }
                else if (TableName == "PartInfo")
                {
                    sb.AppendLine("SELECT DISTINCT ");
                    sb.AppendLine("Replace(UPPER(InfoValue),'B SIDE','') AS Family , ");
                    sb.AppendLine("Replace(UPPER(InfoValue),'B SIDE','') AS Family ");
                    sb.AppendLine("FROM PartInfo(nolock) ");
                    sb.AppendLine("WHERE InfoType = 'MDL' AND CHARINDEX('B SIDE', UPPER(InfoValue)) > 0  ");
                    sb.AppendLine("ORDER BY Replace(UPPER(InfoValue),'B SIDE','') ");
                }

                DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                         System.Data.CommandType.Text,
                                                         sb.ToString());
                return dt;
            }
            catch (Exception e)
            { throw; }

        }
      
        #endregion

        
    }
}
