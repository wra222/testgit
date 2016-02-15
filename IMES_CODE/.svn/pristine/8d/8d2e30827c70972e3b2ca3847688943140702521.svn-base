using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common;
using UTL;
using UTL.SQL;

namespace IMES.WatchHDCPFolder
{
    [Serializable]
    public class HDCPInfo
    {
        public string FileName { get; set; }
        public string MAC { get; set; }
        public string Status { get; set; }
        public byte[] HDCPKey { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
    public class WatchFolder
    {
        private static AppConfig _config;

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Semaphore threadCount; 
        private static string TimeFormat = "yyyyMMddHHmmss";
        private static string dateFormat = "yyyyMMdd";
        private static FileSystemWatcher watcher = new FileSystemWatcher();
        private static Queue queue = new Queue();
        private static Queue mySyncdQ = Queue.Synchronized(queue);
        private static WindowsImpersonationContext wicCdsi = null;
        private static volatile bool _shouldStop = false;
        private static volatile bool _timerEnable = false;
        private static volatile bool _happenError = false;
        private static DateTime lastRead = DateTime.MinValue;
        //private static Hashtable fileWriteTime = new Hashtable();
        private static readonly object padlock = new object();
        private static readonly object timerlock = new object();
        private static CacheManager cacheManager = CacheFactory.GetCacheManager("SessionManager");

        private static ConfigurationSectionAdapter configAdapter = new ConfigurationSectionAdapter(new List<string> { "appSettings","connectionStrings" });

        
        //public static void Init( AppConfig config)
        public static void Init()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {        
                    _config = new AppConfig();
                    queue.Clear();
                    cacheManager.Flush();
                    configAdapter.ConfigSectionChanged += new ConfigurationSectionAdapter.ConfigChangedHandler(configAdapter_ConfigSectionChanged);
                    watcher.EnableRaisingEvents = false;

                    
                    if (_config.IsNeesBackUpAfterCompleted)
                    {
                        CheckFileOnStart();
                    }

                    threadCount = new Semaphore(_config.ThreadCount, _config.ThreadCount);
                    watcher.Filter = _config.FileFilter;
                    logger.Debug(" watcher.Filter:" + _config.FileFilter);
                    watcher.Path = _config.FolderPath;
                    logger.Debug(" watcher.Path:" + _config.FolderPath);
                    watcher.IncludeSubdirectories =_config.IncludeSubdirectories;
                    logger.Debug("watcher.IncludeSubdirectories:" + _config.IncludeSubdirectories);
                    logger.Debug("_config.IsNeesBackUpAfterCompleted:" + _config.IsNeesBackUpAfterCompleted);

                    if (_config.IsNeesBackUpAfterCompleted)
                    {
                       
                        watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName;
                        watcher.Created += OnChanged;
                    }
                    else
                    {
                        watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName|NotifyFilters.LastWrite;
                        watcher.Changed += OnChanged;
                    }                  
                    
                    //watcher.Deleted += OnDeleted;
                    //watcher.Renamed += OnRenamed;
                    Thread dealWithData = new Thread(OnDealWithData);

                    if (_config.IsNeesBackUpAfterCompleted)
                    {
                        if (!Directory.Exists(_config.FailFileFolder))
                        {
                            Directory.CreateDirectory(_config.FailFileFolder);
                        }
                        if (!Directory.Exists(_config.SuccessFileFolder))
                        {
                            Directory.CreateDirectory(_config.SuccessFileFolder);
                        }
                    }

                    dealWithData.Start();
                    watcher.Disposed += OnDisposed;
                    watcher.Error += OnError;
                   
                    watcher.InternalBufferSize = _config.InternalBufferSize * 1024; // 64K
                    logger.Debug("Set watcher.InternalBufferSize = " + _config.InternalBufferSize.ToString() + "K");
                    watcher.EnableRaisingEvents = true;
                    logger.Debug("Set watcher.EnableRaisingEvents = true" );
            }
            catch(Exception e)
            {
                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);
                }
                logger.Error(methodName, e);
                
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }

        }

        

        static void configAdapter_ConfigSectionChanged()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
             try
            {
                AppConfig config = new AppConfig();
                
                _config = config;
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

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            DataHandle handle = new DataHandle();
            handle.fullPath = e.FullPath;
            handle.Name = e.Name;
            handle.eventType = e.ChangeType;
            try
            {
                if (File.Exists(e.FullPath))
                {
                    CheckHDCPFile();
                }
                else
                {
                    logger.Error(" File not exists => " + e.FullPath);
                }
             }
            catch(Exception ex)
            {               
                logger.Error(methodName, ex);             
                
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

        private static void OnDealWithData()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.Info("OnDealWithData Thread Starting ....");
                while (!_shouldStop)
                {
                    if (mySyncdQ.IsSynchronized && queue.Count > 0)
                    {
                        try
                        {
                            #region handle Queue data
                            DataHandle item = (DataHandle)queue.Peek();

                            logger.Debug(item.ToString());
                            if (item.eventType != WatcherChangeTypes.Deleted)
                            {

                                if (File.Exists(item.fullPath))
                                {
                                    //item.content = System.IO.File.ReadAllText(item.fullPath);

                                    if (_config.FileFormat == "bin")
                                    {
                                        logger.Debug("Start ThreadPool " + item.fullPath);
                                        ThreadPool.QueueUserWorkItem(Worker, item);
                                    }
                                    else // Check Txt
                                    {
                                        logger.Debug("Start ThreadPool " + item.fullPath);
                                        ThreadPool.QueueUserWorkItem(Worker, item);
                                    }
                                }
                                else
                                {
                                    logger.Error("Can not find file name:" + item.fullPath);
                                }
                                queue.Dequeue();
                            }
                            //else if (item.eventType == WatcherChangeTypes.Changed)
                            //{
                            //    queue.Dequeue();
                            //}
                            else //no implementation for another event
                            {
                                queue.Dequeue();
                            }
                            #endregion
                        }
                        catch (Exception ex1)
                        {
                            logger.Error(methodName, ex1);
                        }
                    }

                    if (_shouldStop) break;

                    lock (padlock)
                    {
                        Monitor.Wait(padlock, 10);
                    }
                }
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

        private static void CheckHDCPFile()
        {
            string filename = "";
            HDCPInfo hdcpInfoItem = new HDCPInfo();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            
            try
            {
                string[] files = Directory.GetFiles(_config.FileFolder);
                foreach (string path in files)
                {
                    #region 讀取檔案
                    FileStream myFile = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
                    string[] fileNameList = path.Split('\\');
                    filename = fileNameList[fileNameList.Length - 1].ToString();
                    #endregion

                    #region 檢查檔案長度是否為308倍數
                    long fileSize = myFile.Length;
                    if ((fileSize - 4) % 308 != 0)
                    {
                        File.Move(_config.FileFolder + "\\" + filename, _config.FailFileFolder + "\\" + filename);
                        //throw new Exception("檔案長度不為308的倍數...");
                        logger.Error(filename + " 檔案長度不為308的倍數...");
                        SendMail.Send(_config.FromAddress,
                            _config.ToAddress,
                            _config.CcAddress,
                            _config.MailSubject,
                            filename + " 檔案長度不為308的倍數... ",
                            _config.EmailServer);

                        continue;
                    }
                    #endregion

                    #region 檢查header是否為1,2
                    byte[] header = new byte[4];
                    myFile.Read(header, 0, 4);
                    if (header[0] != 1 && header[0] != 2)
                    {
                        File.Move(_config.FileFolder + "\\" + filename, _config.FailFileFolder + "\\" + filename);
                        //throw new Exception("header不為1或 2...");
                        logger.Error(filename + " header不為1或 2...");
                        SendMail.Send(_config.FromAddress,
                            _config.ToAddress,
                            _config.CcAddress,
                            _config.MailSubject,
                            filename + " header不為1或 2...",
                            _config.EmailServer);
                        continue;
                    }
                    #endregion

                    #region 切割檔案
                    
                    int recordCount = Convert.ToInt32((fileSize - 4) / 308);
                    IList<HDCPInfo> addList = new List<HDCPInfo>();
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] key = new byte[288];
                        byte[] shdCheckSum = new byte[20];
                        byte[] hdcpKey = new byte[308];

                        myFile.Position = 308 * i + 4;
                        myFile.Read(key, 0, 288);

                        myFile.Position = 308 * i + 4;
                        myFile.Read(hdcpKey, 0, 308);

                        myFile.Position = 308 * i + 292;
                        myFile.Read(shdCheckSum, 0, 20);

                        //compare check sum
                        //IOPMVideoOutput
                        //OPM_CONNECTED_HDCP_DEVICE_INFORMATION

                        SHA1 sha = new SHA1CryptoServiceProvider();
                        byte[] calculateCheckSum = sha.ComputeHash(key);
                        if (!shdCheckSum.SequenceEqual(calculateCheckSum))//比對DeviceKeys SHA 與 後20byte 是否相等
                        {
                            //IsFile = false;
                            logger.Error(filename + " DeviceKeys SHA 與 後20byte 不相等...");
                            logger.Error("[" + hdcpKey + "]");
                            SendMail.Send(_config.FromAddress,
                                _config.ToAddress,
                                _config.CcAddress,
                                _config.MailSubject ,
                                filename + " 比對錯誤: DeviceKeys SHA 與 後20byte 不相等...",
                                _config.EmailServer);
                            continue;
                        }
                        hdcpInfoItem.FileName = filename;
                        hdcpInfoItem.HDCPKey = hdcpKey;


                        SqlParameter parakey = new SqlParameter("@HDCPKey", SqlDbType.VarBinary);
                        parakey.Direction = ParameterDirection.Input;
                        parakey.Value = hdcpInfoItem.HDCPKey;

                        SqlParameter paraFileNme = new SqlParameter("@FileName", SqlDbType.VarChar);
                        paraFileNme.Direction = ParameterDirection.Input;
                        paraFileNme.Value = hdcpInfoItem.FileName;


                        SqlParameter paraErrorText = new SqlParameter("@ErrorText", SqlDbType.VarChar, 255);
                        paraErrorText.Direction = ParameterDirection.Output;
                        paraErrorText.Value = "";


                        SqlParameter paraRet = new SqlParameter("@Ret", SqlDbType.Int);
                        paraRet.Direction = ParameterDirection.ReturnValue;

                        #region 將308byte寫入資料庫
                        SQLHelper.ExecuteNonQuery(_config.DBConnectStr, CommandType.StoredProcedure, _config.SPName, parakey, paraFileNme, paraErrorText, paraRet);
                        int Ret = int.Parse(string.Format("{0}", paraRet.Value));
                        if (Ret != 1)
                        {
                            logger.Error(filename + " Insert錯誤:");
                            logger.Error(paraErrorText.Value);
                            SendMail.Send(_config.FromAddress,
                                _config.ToAddress,
                                _config.CcAddress,
                                _config.MailSubject,
                                filename + " Insert錯誤: " + paraErrorText.Value,
                                _config.EmailServer);
                        }

                        #endregion
                    }
                    #endregion

                    myFile.Close();
                    if (File.Exists(_config.SuccessFileFolder + "\\" + filename))
                    {
                        File.Delete(_config.SuccessFileFolder + "\\" + filename);
                    }
                    File.Move(_config.FileFolder + "\\" + filename, _config.SuccessFileFolder + "\\" + filename);
                }

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

        private static void CheckFileOnStart()
        { 
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);

            try
            {
                CheckHDCPFile();
            }
            catch(Exception ex)
            {
                
                logger.Error(methodName, ex);
               
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

        static void OnError(object sender, ErrorEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                
                logger.Error(e.GetException());
                if (e.GetException().GetType() == typeof(InternalBufferOverflowException))
                {
                   
                    watcher.EnableRaisingEvents = false;
                    logger.Debug("Set watcher.EnableRaisingEvents = false");
                    watcher.InternalBufferSize = _config.InternalBufferSize * 1024;
                    logger.Debug("Set watcher.InternalBufferSize = " +_config.InternalBufferSize.ToString()+ "K");
                    watcher.EnableRaisingEvents = true;
                    logger.Debug("Set watcher.EnableRaisingEvents = true");
                }
                else
                {
                   // logger.Debug("OnError Thread,  lock (timerlock) start...");
                    lock (timerlock)
                    {
                        _happenError = true;
                        if (!_timerEnable)
                        {
                            _timerEnable = true;
                            ThreadPool.QueueUserWorkItem(OnCheckDirWorker, e.GetException());
                            logger.Debug("Raise monitor Folder Thread!!");
                        }
                    }
                   // logger.Debug("OnError Thread,  lock (timerlock) end...");
                }
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

        static void OnDisposed(object sender, EventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {

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

        static void OnCheckDirWorker(object state)
        {
            threadCount.WaitOne();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            WindowsImpersonationContext wic = null;
            DateTime st = DateTime.Now;
            bool isWritelog = true;
            try
            {

                if (!string.IsNullOrEmpty(_config.LoginRemoteServerUser) &&
                      !string.IsNullOrEmpty(_config.LoginRemoteServerDomain) &&
                      !string.IsNullOrEmpty(_config.LoginRemoteServerPassword))
                {
                    wic = Logon.ImpersinateUser(_config.LoginRemoteServerUser,
                                                                            _config.LoginRemoteServerDomain,
                                                                           _config.LoginRemoteServerPassword);
                    logger.Debug(" Logon.ImpersinateUser");
                }
                //DirectoryInfo checkFolder = new DirectoryInfo(_config.FolderPath);
                logger.Debug("Monitor Folder :" + _config.FolderPath + "Starting ...");
                while (_timerEnable)
                {
                    try
                    {
                        isWritelog = ((DateTime.Now - st).TotalMinutes > 10);
                        if (isWritelog)
                        {
                            st = DateTime.Now;
                        }
                        //DirectoryInfo checkFolder = new DirectoryInfo(_config.FolderPath);
                        //if (checkFolder.Exists)
                        if (Directory.Exists(_config.FolderPath))
                        {
                            //logger.Debug("Monitor Folder Thread,  lock (timerlock) start...");
                            WriteDebugLog(isWritelog, "Monitor Folder Thread,  lock (timerlock) start...");
                            lock (timerlock)
                            {
                                _happenError = false;
                                watcher.EnableRaisingEvents = false;
                                //logger.Debug("Monitor Folder Thread,  watcher.EnableRaisingEvents = false");
                                if (CheckHappenError(isWritelog, "watcher.EnableRaisingEvents = false")) continue;

                                _happenError = false;
                                watcher.InternalBufferSize = _config.InternalBufferSize * 1024; // 64K
                                //logger.Debug("Monitor Folder Thread,   watcher.InternalBufferSize =  " + _config.InternalBufferSize.ToString() + "K");
                                if (CheckHappenError(isWritelog, "watcher.InternalBufferSize =  " + _config.InternalBufferSize.ToString() + "K")) continue;

                                _happenError = false;
                                watcher.EnableRaisingEvents = true;
                                //logger.Debug("Monitor Folder Thread,    watcher.EnableRaisingEvents = true");
                                if (CheckHappenError(isWritelog, "watcher.EnableRaisingEvents = true")) continue;

                                _timerEnable = false;
                                logger.Debug("Monitor Folder Thread, alive Diectory:" + _config.FolderPath);
                            }
                            //logger.Debug("Monitor Folder Thread,  lock (timerlock) end...");
                            WriteDebugLog(isWritelog, "Monitor Folder Thread,  lock (timerlock) end...");
                            break;
                        }
                        WriteDebugLog(isWritelog, "Monitor Folder Thread,  Directory disconnection :" + _config.FolderPath);
                        //if ((DateTime.Now - st).TotalMinutes > 10)
                        //{
                        //    st = DateTime.Now;
                        //    logger.Debug("Monitor Folder Thread,  Directory disconnection :" + _config.FolderPath);
                        //}
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }

                    lock (padlock)
                    {
                        Monitor.Wait(padlock, _config.MonitorIntervalTime);
                    }
                }

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
            }
            finally
            {
                if (wic != null)
                {
                    Logon.Log_off(wicCdsi);
                }
                logger.InfoFormat("alive thread count:" + threadCount.Release());
                logger.InfoFormat("END: {0}()", methodName);
            }

        }

        private static bool CheckHappenError(bool isLog, string text)
        {
            if (_happenError)
            {
                if (isLog)
                {
                    logger.Debug("Monitor Folder Thread, " + text + "  _happenError = true , contine next time");
                }
                lock (padlock)
                {
                    Monitor.Wait(padlock, _config.MonitorIntervalTime);
                }
                return true;
            }
            return false;
        }

        private static void WriteDebugLog(bool isLog, string text)
        {
            if (isLog)
            {
                logger.Debug(text);
            }
        }

        private static XmlDocument CheckWellFormedXML(string fullPath)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(fullPath);
                return xml;
            }
            catch(Exception e)
            {
                
                logger.Error(methodName, e);
                return null;
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }

        }

        private static void Worker(object obj)
        {
            threadCount.WaitOne();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                IList<TbAttribute> attributes = new List<TbAttribute>();
                DataHandle item = (DataHandle)obj;
                logger.Info("Worker Deal with " + item.fullPath+" ...");
                logger.Debug(item.ToString());
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
            }
            finally 
            {
                logger.InfoFormat("alive thread count:" + threadCount.Release());
                logger.InfoFormat("END: {0}()", methodName);
            }

        }

        private static void addCache(string key, object value)
        {
            if (_config.SessionTimeOut > 0)
            {
                SlidingTime _objSlidingTime = new SlidingTime(TimeSpan.FromMinutes(_config.SessionTimeOut));
                cacheManager.Add(key, value, CacheItemPriority.None, null, _objSlidingTime);
            }
            else
            {
                cacheManager.Add(key, value);
            }
        }

        public  static void Stop()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {

                watcher.EnableRaisingEvents = false;
                if (_config.IsNeesBackUpAfterCompleted)
                {                   
                    watcher.Created -= OnChanged;
                }
                else
                {                
                   watcher.Changed -= OnChanged;
                }
                watcher.Dispose();
                cacheManager.Flush();
                while (queue.Count!=0)
                {
                    Thread.Sleep(50);
                }
                queue.Clear();
                 _shouldStop = true;
                 _timerEnable = false;

                lock (padlock)
                {
                    Monitor.PulseAll(padlock);
                }

                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);
                }


            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
            }
            finally
            {
                
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

    }

    public class DataHandle
    {
        public string fullPath;
        public string Name;
        public WatcherChangeTypes eventType;
        public string content;
        public XmlDocument Xml;
        public string ToString()
        {
            string str = "";
            str = str + checkNullEmpty("fullPath",fullPath);
            str = str + checkNullEmpty("fileName",Name);
            str = str + checkNullEmpty("eventType", eventType.ToString());
            str = str + checkNullEmpty("content",content);
            //str = str +"XML=>"  +(Xml == null ? "" : Xml.OuterXml); 
            return str;

        }
        private string checkNullEmpty( string name, string data)
        {
            return name + " =>" + (string.IsNullOrEmpty(data) ? "" : data) + "\n";
        }
    }

    public class TbAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ToString() { return "Name:" + Name + " Value:" + Value; }
    }

    
}
