using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;
using log4net;
using System.Configuration;
using System.Reflection;

namespace IMES.BartenderPrinter
{   
			
    public class BtTaskManager
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region singlton 
        private BtTaskManager()
        {
        }
        private static TaskManager _instance = null;
        public static int Timeout = 60000;
        private static ConfigurationSectionAdapter configAdapter = null;

        private static string LicenseServerAdress = "LicenseServerAdress";
        private static string LicenseServerPort = "LicenseServerPort";
        private static string LicenseServerRetries = "LicenseServerRetries";
        private static string LicenseServerTimeout = "LicenseServerTimeout";
        private static string TaskMangerTimeOut = "TaskMangerTimeOut";


        /// <summary>
        /// Instance used for singleton
        /// </summary>
        public static void Start(int capacity)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (configAdapter==null)
                {
                    configAdapter =new ConfigurationSectionAdapter(new List<string> { "appSettings", "connectionStrings" });
                    configAdapter.ConfigSectionChanged += new ConfigurationSectionAdapter.ConfigChangedHandler(configAdapter_ConfigSectionChanged);
                }

                if (_instance == null)
                {
                    _instance = new TaskManager();
                    _instance.Start(capacity);
                    logger.InfoFormat("Task Manager instance {0} success! ", capacity.ToString());
                    foreach (TaskEngine engine in _instance.TaskEngines)
                    {                       
                        engine.VisibleWindows = Seagull.BarTender.Print.VisibleWindows.None;
                    }
                    string serverIP = ConfigurationManager.AppSettings[LicenseServerAdress];
                    string serverPort = ConfigurationManager.AppSettings[LicenseServerPort];
                    string serverRetries = ConfigurationManager.AppSettings[LicenseServerRetries];
                    string serverTimeout = ConfigurationManager.AppSettings[LicenseServerTimeout];

                    if (!string.IsNullOrEmpty(serverIP) &&
                        !string.IsNullOrEmpty(serverPort))
                    {
                        // Create a new LicenseServerTask.
                        LicenseServerTask task = new LicenseServerTask();
                        // Specify the license server connection.
                        task.PreferredConnection.Address = serverIP;
                        task.PreferredConnection.Port = int.Parse(serverPort);


                        if (!string.IsNullOrEmpty(serverRetries))
                        {
                            task.PreferredConnection.Retries = int.Parse(serverRetries);
                        }

                        if (!string.IsNullOrEmpty(serverTimeout))
                        {
                            task.PreferredConnection.Timeout = int.Parse(serverTimeout);
                        }
                        string timeout = ConfigurationManager.AppSettings[TaskMangerTimeOut] ?? "60000";
                        Timeout = int.Parse(timeout);
                        // Execute the task and wait for completion.
                        _instance.TaskQueue.QueueTaskAndWait(task, int.Parse(timeout));

                        // Report the results. 
                        if (task.IsConnected)
                        {
                            logger.InfoFormat("Bartender connected to license server:{0} port:{1} succes! ", serverIP, serverPort);
                        }
                        else
                        {
                            string errorText = string.Format("Bartender connect to license server:{0} port:{1} fail! ", serverIP, serverPort);
                            throw new Exception(errorText);
                        }
                    }
                    else
                    {
                        string errorText = string.Format("No setup license server address & Port! ");
                        logger.Error(errorText);
                        //throw new Exception("No setup license server address & Port");
                    }

                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
            
        }
        #endregion

        public static TaskQueue GetTaskQueue()
        {
            if (_instance != null)
            {
                return _instance.TaskQueue;
            }
            else
            {
                return null;
            }
        }
        
        public static void Stop( )
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (_instance != null)
                {
                    //string timeout = ConfigurationManager.AppSettings["TaskMangerTimeOut"]??"60000";
                    //_instance.Stop(int.Parse(timeout), true); 
                    _instance.Terminate();
                    _instance.Dispose();                   
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }           
        }

        static void configAdapter_ConfigSectionChanged()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.Info("Configure File Changed!");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);

            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

       
    }
}
