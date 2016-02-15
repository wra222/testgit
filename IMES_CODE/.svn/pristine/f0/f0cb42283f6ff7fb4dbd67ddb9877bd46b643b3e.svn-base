using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using UPH.Entity.Infrastructure.Interface;
using System.Reflection;
using log4net;
using System.Data.Linq;
using System.Configuration;

namespace UPH.Entity.Infrastructure.Framework
{
    public class UnitOfWork:IDisposable 
    {
        protected static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        [ThreadStatic]
        private static Dictionary<string, DataContext> contextList = null;
        // 
        //private static object syncRoot = new object();
        //[ThreadStatic]       
        //private static DataContext _dc = null; 

        public static DataContext Context(string dbName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                DataContext ret = null;
                if (contextList == null)
                {
                    //contextList = new Dictionary<string, DataContext>();
                    throw new Exception("need Construct UnitOfWork class first!!");
                }
                if (!contextList.ContainsKey(dbName))
                {

                    if (ConfigurationManager.ConnectionStrings[dbName] == null)
                    {
                        throw new Exception("Missing configure connectionString DBName:" + dbName + " in app.conig");

                    }
                    ret = new DataContext(ConfigurationManager.ConnectionStrings[dbName].ConnectionString);
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
            string methodName = MethodBase.GetCurrentMethod().Name;
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
        public  void Commit()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {               
                using (TransactionScope scope = new TransactionScope())
                {
               
                   foreach (DataContext dc in contextList.Values)
                   {
                        dc.SubmitChanges();
                    }
                   scope.Complete();
                }
                contextList.Clear();
             
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

        public void Dispose()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
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
        //public static DataContext Context { get {return _dc;} }
 
    }
}
