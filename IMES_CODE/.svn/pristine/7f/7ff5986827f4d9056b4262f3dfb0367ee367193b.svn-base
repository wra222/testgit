using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using Seagull.BarTender.Print;
using System.Threading;
using System.Configuration;
using System.Reflection;

namespace IMES.BartenderPrinter
{
    
    public class PoolItem
    {
        public AutoResetEvent producerEvent = new AutoResetEvent(false);

        public bool HasError=false;
        public string ErrorText=string.Empty;
        public int ThreadId = 0;
        public int DPI=0;
        public string ThreadFolder =null;
        public string BtwSrcFileName=null;
        public string BtwFile=null;
        public IList<NameValue> TextFileValueList=null;
        public IList<NameValue> NameValueList=null;
        public IList<NameValue> ODBCValueList = null;
        public IList<NameValue> OLEDBValueList = null;
        //public List<string> ImgStrList=null;
        public string BmpFolder = null;
        public bool hasTimeout = false;        
    }

    public sealed class BartenderPool
    {
         class PoolMsg
        {
             public int id = 0;
            public bool HasError = false;
            public string ErrorText = string.Empty;
            public AutoResetEvent WaitEvent = new AutoResetEvent(false);
        }

        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static object _syncObj = new object();
        private static Dictionary<int, PoolItem> btEngineList = new Dictionary<int, PoolItem>();
        private static volatile bool _shouldStop = false;
      
        private static AutoResetEvent workerEvent = new AutoResetEvent(false);
        private static Queue<PoolItem> queue = new Queue<PoolItem>();
        private static long workCount =0;
        public static int Timeout = 60000;

        private static string TaskMangerTimeOut = "TaskMangerTimeOut";
        private static string LicenseServerAdress = "LicenseServerAdress";
        private static string LicenseServerPort = "LicenseServerPort";
        private static string LicenseServerRetries = "LicenseServerRetries";
        private static string LicenseServerTimeout = "LicenseServerTimeout";

        private static ConfigurationSectionAdapter configAdapter = null;
        
        public static void Init(int capacity)
        { 
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
			try
            {

                    if (configAdapter == null)
                    {
                        configAdapter = new ConfigurationSectionAdapter(new List<string> { "appSettings", "connectionStrings" });
                        configAdapter.ConfigSectionChanged += new ConfigurationSectionAdapter.ConfigChangedHandler(configAdapter_ConfigSectionChanged);
                    }

                    string timeout = ConfigurationManager.AppSettings[TaskMangerTimeOut] ?? "60000";
                    Timeout = int.Parse(timeout);
                     IList<PoolMsg> msgList = new List<PoolMsg>();                      
					for(int i=0;i<capacity; i++)
                    {
                        PoolMsg msg = new PoolMsg();
                        msg.id = i+1;
                        msgList.Add(msg);
                        //ThreadPool.QueueUserWorkItem(BatenderPool.worker,null);
                        Thread td = new Thread(new ParameterizedThreadStart(BartenderPool.worker));
                        td.Priority = ThreadPriority.AboveNormal;
                        td.Start(msg);                  
                                        
                        logger.InfoFormat("Waiting for stating BtEngine instance:{0}", (i + 1).ToString());
                        //msg.WaitEvent.WaitOne();
                        //logger.DebugFormat("Started BtEngine instance:{0} completed", (i + 1).ToString());
                        //if (msg.HasError)
                        //{
                        //    throw new Exception(msg.ErrorText);
                        //}
                    }
                    var waiteventList = msgList.Select(x=>x.WaitEvent).ToArray();
                    WaitHandle.WaitAll(waiteventList);
                    foreach (PoolMsg item in msgList)
                    {
                        if (item.HasError)
                        {
                            logger.ErrorFormat("Started BtEngine instance:{0} fail, error text :{1}", item.id.ToString(),item.ErrorText);
                        }
                        else
                        {
                            logger.InfoFormat("Started BtEngine instance:{0} completed", item.id.ToString());
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
                logger.InfoFormat("END: {0}()", methodName);
			}
            
        }

        private static PoolItem getItem()
        {
            //string methodName = "getItem";
            //logger.DebugFormat("BEGIN: {0}()", methodName);
			try	 
            {
					 lock(_syncObj)
                    {
                        if (queue.Count>0)
                        {
                            return queue.Dequeue();
                        }
                        else
                        {
                            return null;
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
                //logger.DebugFormat("END: {0}()", methodName);
			}           
        }

        public static void addItem(PoolItem item)
        {
            //string methodName = "addItem";
            //logger.DebugFormat("BEGIN: {0}()", methodName);
			try	 
            {
			    lock(_syncObj)
                {
                    queue.Enqueue(item);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
				throw;					
            }
			finally
			{
                //logger.DebugFormat("END: {0}()", methodName);
			}
        }

        public static void SendWorkerEvent()
        {
            workerEvent.Set();
        }

        private static void worker(object state)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
               
            PoolMsg msg = (PoolMsg)state;
            logger.InfoFormat("BEGIN: {0}({1})", methodName, msg.id.ToString());
        
			try	 
            {
                logger.InfoFormat("Worker{0} Start ...." , msg.id.ToString());
                Interlocked.Increment(ref workCount);
              
                using (Engine _instance = new Engine(true))
                {
                    _instance.Start();
                   
                    //Connect license Server
                    string serverIP = ConfigurationManager.AppSettings[LicenseServerAdress];
                    string serverPort = ConfigurationManager.AppSettings[LicenseServerPort];
                    string serverRetries = ConfigurationManager.AppSettings[LicenseServerRetries];
                    string serverTimeout = ConfigurationManager.AppSettings[LicenseServerTimeout];

                    if (!string.IsNullOrEmpty(serverIP) &&
                        !string.IsNullOrEmpty(serverPort))
                    {
                        _instance.LicenseServer.PreferredConnection.Address = serverIP;
                        _instance.LicenseServer.PreferredConnection.Port = int.Parse(serverPort);
                        if (!string.IsNullOrEmpty(serverRetries))
                        {
                            _instance.LicenseServer.PreferredConnection.Retries = int.Parse(serverRetries);
                        }

                        if (!string.IsNullOrEmpty(serverTimeout))
                        {
                            _instance.LicenseServer.PreferredConnection.Timeout = int.Parse(serverTimeout);
                        }

                       
                        if (_instance.LicenseServer.Connect())
                        {
                            logger.InfoFormat("Worker{0} Bartender connected to license server:{1} port:{2} succes! ", msg.id.ToString(), serverIP, serverPort);
                        }

                        else
                        {
                            string errorText = string.Format("Worker{0} Bartender connect to license server:{1} port:{2} fail! ", msg.id.ToString(), serverIP, serverPort);
                            throw new Exception(errorText);
                        }
                       
                    }
                    else
                    {
                        string errorText = string.Format("Worker{0} No setup license server address & Port! ", msg.id.ToString());
                        logger.Error(errorText);
                        //throw new Exception("No setup license server address & Port");
                    }
                    msg.WaitEvent.Set();
                                     
                    while (!_shouldStop &&
                            (queue.Count > 0 || 
                            workerEvent.WaitOne()))
                    {
                        PoolItem item = getItem();
                        if (item != null && 
                            !item.hasTimeout)
                        { 
                            try
                            {
                                logger.DebugFormat("Worker{0} is starting for producer thread Id :{1}  Source Btw File :{2}",msg.id.ToString(), item.ThreadId.ToString(), item.BtwSrcFileName);
                                BartenderUTL.GenBitmapFile(_instance, item);                                
                                logger.DebugFormat("Worker{0} has finished producer thread Id :{1}  Source Btw File :{2}", msg.id.ToString(), item.ThreadId.ToString(), item.BtwSrcFileName);
                            }
                            catch (Exception e1)
                            {
                                item.HasError = true;
                                item.ErrorText = e1.Message;
                                logger.Error(e1.Message, e1);                               
                            }
                            finally
                            {
                                item.producerEvent.Set();
                            }
                        }
                    }

                    //stop engine
                    _instance.Stop();
                    logger.InfoFormat("Worker{0} Stop" , msg.id.ToString());
                }
            }
            catch (Exception e)
            {
                msg.HasError = true;
                msg.ErrorText = "Worker" + msg.id.ToString() +" "  + e.Message;
                logger.Error(e.Message, e);
			    				
            }
			finally
			{
                Interlocked.Decrement(ref workCount);
                logger.InfoFormat("END: {0}{1}( Instance count:{2})", methodName, msg.id.ToString(), workCount.ToString());
			}
        }

        public static void Shutdown()
        {
          
            _shouldStop = true;
            while (Interlocked.Read(ref workCount) > 0)
            {
                workerEvent.Set();
                Thread.Sleep(1000);
            }
        }

        static void configAdapter_ConfigSectionChanged()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                setTaskTimeOut();
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

        static void  setTaskTimeOut()
        {
            string timeout = ConfigurationManager.AppSettings[TaskMangerTimeOut] ?? "60000";
            Timeout = int.Parse(timeout);
        }
        
    }
    
}
