using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using System.Data.Linq;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace IMES.Entity.Infrastructure.Framework
{
    public sealed class UnitOfWork:IDisposable 
    {
        protected static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        [ThreadStatic]
        private static Dictionary<string, DataContext> contextList = null;
        [ThreadStatic]
        private static DbConnection _conn = null;
        [ThreadStatic]
        private static DbTransaction _trans = null;      
        //private static object syncRoot = new object();
        private static string CatalogRE =@"Initial Catalog=(?<Catalog>\s*\w+\s*);\w+";

        private static DbConnection GetSqlConnection(string connStr)
        {
            if (_conn == null)
            {
                _conn = new SqlConnection(connStr);              
            }
            return _conn;
           
        }

        public static string GetCataLog(string connectionString)
        {
            Match match = Regex.Match(connectionString, CatalogRE, RegexOptions.Compiled);
            if (match.Success)
            {
                return match.Groups["Catalog"].Value.Trim();
            }
            else
            {
                return "";
            }
        }       

        private static DbTransaction GetTransaction()
        {           
            if (_trans == null)
            {
                _trans = _conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            }
            return _trans;           
        }

        public static DataContext Context(string dbName)
        {
            string methodName = "UnitOfWork.Context";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                DataContext ret = null;
                
                if (contextList == null)
                {
                    contextList = new Dictionary<string, DataContext>();
                    //throw new Exception("need Construct UnitOfWork class first!!");
                }
                if (!contextList.ContainsKey(dbName))
                {

                    if (ConfigurationManager.ConnectionStrings[dbName] == null)
                    {
                        throw new Exception("Missing configure connectionString DBName:" + dbName + " in app.conig");
                    }
                    string conStr = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
                    // ret = new DataContext(ConfigurationManager.ConnectionStrings[dbName].ConnectionString);
                    ret = new DataContext(GetSqlConnection(conStr));      
                   
                    contextList.Add(dbName, ret);
                }
                else
                {
                    ret = contextList[dbName];
                }
                
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
        //public static void Begin()
        //{ string methodName = MethodBase.GetCurrentMethod().Name;
        //    logger.DebugFormat("BEGIN: {0}()", methodName);
        //    try
        //    {
        //        if (contextList == null)
        //        {
        //            contextList = new Dictionary<string, DataContext>();
        //        }
        //        else
        //        {
        //            contextList.Clear();
        //        }
        //    }  
        //    catch (Exception e)
        //    {
        //        logger.Error(methodName, e);
        //        throw;
        //    }
        //    finally
        //    {
        //        logger.DebugFormat("END: {0}()", methodName);
        //    }

        //}

        public UnitOfWork()
        {
            string methodName = "UnitOfWork.UnitOfWork()";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (contextList == null)
                {
                    contextList = new Dictionary<string, DataContext>();
                }
                else
                {
                    contextList.Clear();
                }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public  static void ThreadCommit()
        {
            string methodName = "UnitOfWork.Commit";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {               
                //using (TransactionScope scope = new TransactionScope())
                //{                  
                       
               //foreach (DataContext dc in contextList.Values)
                foreach (KeyValuePair<string,DataContext> pair in contextList) 
               {
                   string dbName = pair.Key;
                   DataContext dc = pair.Value;
                   if (dc.Connection.State != System.Data.ConnectionState.Open)
                   {
                       dc.Connection.Open();
                   }
                   string catalog = GetCataLog(ConfigurationManager.ConnectionStrings[dbName].ConnectionString);
                   if (dc.Connection.Database != catalog)
                   {
                       dc.Connection.ChangeDatabase(catalog);
                   }
                   dc.Transaction = GetTransaction();                   
                   dc.SubmitChanges();                   
                }
                _trans.Commit();
                _trans = null;
                _conn = null;
                   //scope.Complete();
                //}
                contextList.Clear();
             
            }
            catch (Exception e)
            {
                _trans.Rollback();
                _trans = null;
                _conn = null;
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public void Commit()
        {
            UnitOfWork.ThreadCommit();
        }
        public void Dispose()
        {
            string methodName ="UnitOfWork.Dispose";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (contextList == null)
                {
                    contextList = new Dictionary<string, DataContext>();
                }               
                contextList.Clear();
                _trans = null;
                _conn = null;              
                
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }   
        //public static DataContext Context { get {return _dc;} }
 
    }

}
