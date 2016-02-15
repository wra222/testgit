using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using log4net;
using UTL;
using UTL.SQL;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;

namespace IMES.WatchFolder
{
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

                    //1.Logon SAP File server                
                    if (!string.IsNullOrEmpty(_config.LoginRemoteServerUser) &&
                        !string.IsNullOrEmpty(_config.LoginRemoteServerDomain) &&
                        !string.IsNullOrEmpty(_config.LoginRemoteServerPassword))
                        wicCdsi = Logon.ImpersinateUser(_config.LoginRemoteServerUser,
                                                                                _config.LoginRemoteServerDomain,
                                                                               _config.LoginRemoteServerPassword);
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
                        //watcher.Created += new FileSystemEventHandler(OnChanged);
                        watcher.Created += OnChanged;
                    }
                    else
                    {
                        //watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess;
                        //watcher.Changed += new FileSystemEventHandler(OnChanged);
                        watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName|NotifyFilters.LastWrite;
                        //watcher.Created += new FileSystemEventHandler(OnChanged);
                        //watcher.Changed += new FileSystemEventHandler(OnChanged);
                        watcher.Changed += OnChanged;
                    }                  
                    
                    //watcher.Deleted += new FileSystemEventHandler(OnDeleted);
                    //watcher.Renamed += new RenamedEventHandler(OnRenamed);
                    watcher.Deleted += OnDeleted;
                    watcher.Renamed += OnRenamed;
                    Thread dealWithData = new Thread(OnDealWithData);

                    if (_config.IsNeesBackUpAfterCompleted)
                    {
                        if (!Directory.Exists(_config.BckupFolder))
                        {
                            Directory.CreateDirectory(_config.BckupFolder);
                        }

                        if (!Directory.Exists(_config.BadXmlFolder))
                        {
                            Directory.CreateDirectory(_config.BadXmlFolder);
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
                    
                    DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
                    //if (lastWriteTime != lastRead)
                    if (!cacheManager.Contains(e.FullPath) ||
                         (DateTime)cacheManager[e.FullPath] != lastWriteTime)
                    {
                        logger.Info(handle.ToString());
                        queue.Enqueue(handle);
                        addCache(e.FullPath, lastWriteTime);
                    }
                    else    //discard the (duplicated) OnChanged event
                    {
                        logger.Info(" discard the (duplicated) OnChanged event  => " + e.FullPath);
                    }
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

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);          
            try
            {
                logger.Info("Deleted file =>" + e.FullPath);
                if (cacheManager.Contains(e.FullPath))
                {
                    cacheManager.Remove(e.FullPath);
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

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            //DataHandle handle = new DataHandle();
            //handle.fullPath = e.FullPath;
            //handle.Name = e.Name;
            //handle.eventType = e.ChangeType;
            try
            {
                //logger.Info(handle.ToString());
                //queue.Enqueue(handle);
                logger.Info("Rename file =>" + e.FullPath  + " old file=>" + e.OldFullPath);
                if (cacheManager.Contains(e.OldFullPath))
                {
                    addCache(e.FullPath, cacheManager[e.OldFullPath]);
                    cacheManager.Remove(e.OldFullPath);
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

                                            if (_config.FileFormat == "XML")
                                            {
                                                XmlDocument xml = CheckWellFormedXML(item.fullPath);
                                                if (xml != null)
                                                {
                                                    item.Xml = xml;
                                                    logger.Debug("Start ThreadPool " + item.fullPath);
                                                    //logger.Debug("XML Content:" + xml.InnerXml);
                                                    ThreadPool.QueueUserWorkItem(Worker, item);
                                                }
                                                else
                                                {
                                                    Thread.Sleep(1000);
                                                    logger.Debug("Retry Check Well-Form XML  " + item.fullPath);
                                                    xml = CheckWellFormedXML(item.fullPath);
                                                    if (xml != null)
                                                    {
                                                        item.Xml = xml;
                                                        logger.Debug("Start ThreadPool " + item.fullPath);
                                                        //logger.Debug("XML Content:" + xml.InnerXml);
                                                        ThreadPool.QueueUserWorkItem(Worker, item);
                                                    }
                                                    else  // bad xml file
                                                    {
                                                        if (_config.IsNeesBackUpAfterCompleted)
                                                        {
                                                            File.Move(item.fullPath, _config.BadXmlFolder + item.Name + "." + DateTime.Now.ToString(TimeFormat));
                                                        }
                                                        logger.Error("Not Well-Form XML file :" + item.fullPath);
                                                    }
                                                }
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
                            Monitor.Wait(padlock,10);
                        }
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

        private static void CheckFileOnStart()
        { 
             string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
             
                 FileInfo[] fileInfos = FileOperation.GetFilesByWildCard(_config.FolderPath,
                                                                                                          _config.FileFilter);
                foreach (FileInfo file in fileInfos)
                {
                    DataHandle handle = new DataHandle();

                    XmlDocument  xml= CheckWellFormedXML(file.FullName);
                     handle.fullPath = file.FullName;
                     handle.Name = file.Name;
                     handle.eventType = WatcherChangeTypes.Created;
                     handle.content =System.IO.File.ReadAllText(file.FullName);
                    if (xml!=null)
                    {
                        handle.Xml = xml;
                        logger.Info(handle.ToString());
                        queue.Enqueue(handle);
                    }
                    else 
                    {
                        string badfolder = _config.BadXmlFolder + DateTime.Now.ToString(dateFormat) + "\\";
                        if (!Directory.Exists(badfolder))
                        {
                            Directory.CreateDirectory(badfolder);
                        }
                        File.Move(file.FullName, badfolder + file.Name + "." + DateTime.Now.ToString(TimeFormat));
                        logger.Error(handle.ToString());
                    }
                    
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
            WindowsImpersonationContext wic=null;
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
                        if(Directory.Exists(_config.FolderPath))
                        {
                            //logger.Debug("Monitor Folder Thread,  lock (timerlock) start...");
                            WriteDebugLog(isWritelog, "Monitor Folder Thread,  lock (timerlock) start...");
                            lock (timerlock)
                            {
                                _happenError = false;
                                watcher.EnableRaisingEvents = false;
                                //logger.Debug("Monitor Folder Thread,  watcher.EnableRaisingEvents = false");
                                if (CheckHappenError(isWritelog,"watcher.EnableRaisingEvents = false")) continue;

                                _happenError = false;
                                watcher.InternalBufferSize = _config.InternalBufferSize * 1024; // 64K
                                //logger.Debug("Monitor Folder Thread,   watcher.InternalBufferSize =  " + _config.InternalBufferSize.ToString() + "K");
                                if (CheckHappenError(isWritelog,"watcher.InternalBufferSize =  " + _config.InternalBufferSize.ToString() + "K")) continue;

                                _happenError = false;
                                watcher.EnableRaisingEvents = true;
                                //logger.Debug("Monitor Folder Thread,    watcher.EnableRaisingEvents = true");
                                if (CheckHappenError(isWritelog,"watcher.EnableRaisingEvents = true")) continue;

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

                if (_config.FileFormat == "XML")
                {
                    //1.check xml data                   
                    XmlNodeList nodeList = item.Xml.SelectNodes(_config.XMLPath);
                    if (nodeList.Count == 0)
                    {
                        if (_config.IsNeesBackUpAfterCompleted)
                        {
                            string NoTagNamefolder = _config.NoRootTagNameFolder + DateTime.Now.ToString(dateFormat) + "\\";
                            if (!Directory.Exists(NoTagNamefolder))
                            {
                                Directory.CreateDirectory(NoTagNamefolder);
                            }

                            File.Move(item.fullPath, NoTagNamefolder + item.Name + "." + DateTime.Now.ToString(TimeFormat));
                        }

                        logger.Error("Can not find xml element  XMLPath : " + _config.XMLPath);
                        return;
                    }

                    string msgTxt = "";
                    foreach (XmlNode node in nodeList)
                    {
                        TbAttribute attr = new TbAttribute();
                        attr.Name = node.Name.Trim();
                        attr.Value = node.InnerText.Trim();
                        msgTxt = msgTxt + attr.ToString() + "\n";
                        attributes.Add(attr);
                    }

                    logger.Info("XPath =>" + msgTxt);
                }
                else
                {
                    string[] data= System.IO.File.ReadAllLines(item.fullPath);
                    if (data.Length == 0)
                    {
                        if (_config.IsNeesBackUpAfterCompleted)
                        {
                            string NoTagNamefolder = _config.NoRootTagNameFolder + DateTime.Now.ToString(dateFormat) + "\\";
                            if (!Directory.Exists(NoTagNamefolder))
                            {
                                Directory.CreateDirectory(NoTagNamefolder);
                            }

                            File.Move(item.fullPath, NoTagNamefolder + item.Name + "." + DateTime.Now.ToString(TimeFormat));
                        }

                        logger.Error("File content is empty  " + item.fullPath);
                        return;
                    }

                    //get last data string
                    string[] lastRowData = data[data.Length - 1].Split(new string[] {_config.TXTDelimiter }, StringSplitOptions.None );
                    string msgTxt = "";
                    
                    foreach (string row in lastRowData)
                    {
                        TbAttribute attr = new TbAttribute();
                        if (_config.TXTColumnName.Length > attributes.Count)
                        {
                            attr.Name = _config.TXTColumnName[attributes.Count].Trim();
                        }
                        else
                        {
                            attr.Name = "item_" + attributes.Count;
                        }
                        attr.Value = string.IsNullOrEmpty(row)? "": row.Trim();
                        msgTxt = msgTxt + attr.ToString() + "\n";
                        attributes.Add(attr);
                       
                    }

                    logger.Info("TXT File  =>" + msgTxt);

                }
                //2.Prepare TVP 

                if (!string.IsNullOrEmpty(_config.IMESSPName))
                {

                    SqlParameter para = null;
                    if (_config.SQL2008 == "N")
                    {
                        if (_config.IsFileNameMonitor == "Y")
                        {
                            para = new SqlParameter("@Attributes", SqlDbType.VarChar);
                            para.Direction = ParameterDirection.Input;
                            para.Value = item.Name;
                        }
                        else
                        {
                            para = new SqlParameter("@Attributes", SqlDbType.VarChar);
                            para.Direction = ParameterDirection.Input;
                            string nameValue = "";
                            foreach (TbAttribute attr in attributes)
                            {
                                nameValue = nameValue + (string.IsNullOrEmpty(nameValue) ? "" : "~") + attr.Name + "=" + attr.Value;
                            }
                            para.Value = nameValue;
                        }
                    }
                    else
                    {
                        if (_config.IsFileNameMonitor == "Y")
                        {
                           
                            TbAttribute attr = new TbAttribute();
                            attr.Name ="FileName";
                            attr.Value = item.Name.Trim();

                            attributes.Add(attr);
                        }
                       
                            para = new SqlParameter("@Attributes", SqlDbType.Structured);
                            para.TypeName = "TbAttribute";
                            para.Direction = ParameterDirection.Input;
                            para.Value = SQLHelper.ToDataTable(attributes);
                        
                    }
                    SqlParameter ErrorTex = new SqlParameter("@ErrorText", SqlDbType.VarChar,255);
                    ErrorTex.Direction = ParameterDirection.Output;
                

                    //3.call iMES sp
                    object retValue = SQLStatement.executeSP(_config.DBConnectStr, _config.IMESSPName, para, ErrorTex);
                    if (retValue != null)
                    {
                        string retCode = (string)retValue;
                        logger.InfoFormat("SP Return Code =>{0}", retCode);
                    }
                }
                else
                {
                    logger.Error("Call iMES SP Name is empty, check configure file!!");
                }

                //4.Move file
                if (_config.IsNeesBackUpAfterCompleted)
                {
                    string bkfolder = _config.BckupFolder + DateTime.Now.ToString(dateFormat) + "\\";
                    if (!Directory.Exists(bkfolder))
                    {
                        Directory.CreateDirectory(bkfolder);
                    }

                    File.Move(item.fullPath, bkfolder + item.Name + "." + DateTime.Now.ToString(TimeFormat));
                }

            }
            catch (Exception e)
            {

                if (_config.IsNeesBackUpAfterCompleted)
                {
                    try
                    {
                        DataHandle item = (DataHandle)obj;
                        string Errorfolder = _config.ErrorFolder + DateTime.Now.ToString(dateFormat) + "\\";

                        if (!Directory.Exists(Errorfolder))
                        {
                            Directory.CreateDirectory(Errorfolder);
                        }

                        File.Move(item.fullPath,
                                        Errorfolder + Path.GetFileName(item.Name) + "." + DateTime.Now.ToString(TimeFormat) + "." + Path.GetExtension(item.Name)
                                        );
                    }
                    catch { }
                }
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

                watcher.Deleted -=OnDeleted;
                watcher.Renamed -= OnRenamed;
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
