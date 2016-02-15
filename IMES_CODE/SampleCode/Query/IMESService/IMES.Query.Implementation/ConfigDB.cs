using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using log4net;
using System.Reflection;
using System.Configuration;

namespace IMES.Query.Implementation
{
    public class ConfigDB : MarshalByRefObject,IConfigDB
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        #region IConfigDB Members

        public List<string> GetHistoryDBList()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                return SQLHelper.DB_HISTROY.ToList();
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
         
        public string GetOnlineDB()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                return SQLHelper.DB_CFG;
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


        public List<string> GetOnlineDBList()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                return SQLHelper.DB_CFG_LIST.ToList();
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
       
        public string GetSelectedDB(string DBType, int DBIndex)
         {
            string Connection = "";
            if (DBType == "HistoryDB")
            {
                Connection = SQLHelper.ConnectionString_HISTORY(DBIndex);
            }
            else
            {
                Connection = SQLHelper.ConnectionString_ONLINE(DBIndex);
            }
            return Connection;

        }
       public DBInfo GetDBInfo()
       {
           DBInfo dbInfo = new DBInfo();
           dbInfo.OnLineConnectionString = SQLHelper.OnLineConnectionString;
           dbInfo.HistoryConnectionString = SQLHelper.HistoryConnectionString;
           dbInfo.OnLineDBList = SQLHelper.DB_CFG_LIST;
           dbInfo.HistoryDBList = SQLHelper.DB_HISTROY;
           return dbInfo;

       }

       public string GetOnlineDefaultDBName()
       {
           return SQLHelper.DB_CFG_LIST[0];
       }

       public bool CheckDockingDB(string DBName)
       {
           string name =ConfigurationManager.AppSettings["DockingDBKeyWord"]==null ? "DOCKING": ConfigurationManager.AppSettings["DockingDBKeyWord"].ToString().Trim();
           return DBName.Contains(name);
       }
       
        #endregion
    }
}
