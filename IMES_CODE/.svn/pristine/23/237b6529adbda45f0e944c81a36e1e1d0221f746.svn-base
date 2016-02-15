// 2010-01-29 Liu Dong(eB1-4)         Modify ITC-1122-0025 
// 2010-02-10 Liu Dong(eB1-4)         Modify ITC-1122-0084 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Reflection;
using IMES.DataModel;
using System.Net;
using IMES.Infrastructure.Utility.Tools;
using log4net;

namespace IMES.Infrastructure.Utility.Cache
{
    /// <summary>
    /// Cache数据更新协调管理器
    /// </summary>
    public class DataChangeMediator
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Const Name define
        internal static class CacheConstName
        {
            //Cache 
            public const string CacheName = "CST_{0}";
            public const string CacheOn = "1";
            public const string DefaultRefreshCaheTime = "1"; //one minute

            //Configure file
            public const string RepositoryImplAssembly = "RepositoryImplAssembly";
            public const string CacheStaleCheckInterval="CacheStaleCheckInterval"; 

            //Repository method
            public const string IsCached = "IsCached";

            //IMiscRepository method
            public const string GetAllCacheUpdate = "GetAllCacheUpdate";
            public const string DeleteCacheUpdate = "DeleteCacheUpdate";
            public const string DeleteCacheUpdateForSoloTypes = "DeleteCacheUpdateForSoloTypes";
            public const string AddCacheUpdate = "AddCacheUpdate";
            public const string DeleteCacheUpdateByIPAddressAndAppName = "DeleteCacheUpdateByIPAddressAndAppName";

            //Repostory object name List
            public const string IPartRepository = "IPartRepository";
            public const string IFamilyRepository = "IFamilyRepository";
            public const string IModelRepository = "IModelRepository";
            public const string IBOMRepository = "IBOMRepository";
            public const string IDefectRepository = "IDefectRepository";
            public const string IDefectInfoRepository = "IDefectInfoRepository";

            public const string IProcessRepository = "IProcessRepository";
            public const string IStationRepository = "IStationRepository";
            public const string ILineRepository = "ILineRepository";
            public const string IModelWeightRepository = "IModelWeightRepository";
            public const string IModelToleranceRepository = "IModelToleranceRepository";           

            //Error Message
            public const string MsgCacheLoadError = "Cache Proactive Load Error. InterfaceName: {0}, MethodName:{1}, Error:{2}.";
            public const string MsgCacheRefreshTimeError = "The CacheStaleCheckInterval property {0} in AppSettings area of app.config file should be a positive integer. using default value is 1 minute";
            public const string MsgSwitchCacheNotFound = "Switch cannot be found in config file: {0}. ";
            public const string MsgGetIPError = "Cannot get local IP address.";
            public const string MsgHostLocalIP = "Host local IP address: {0}";

            //Cache process once Error Mesage
            public const string MsgCacheUpdated = "Solo-Cache Updated. ID: {0}, Type:{1}, Item:{2}.";
            public const string MsgCacheUpdateAbnormal = "Solo-Cache Abnormal. ID: {0}, Type:{1}, Item:{2}. Cannot find the Assembly or Switch Off.";
            public const string MsgCacheChangeError = "Solo-Cache Change Error. ID: {0}, Type:{1}, Key:{2}, Error:{3}.";
            public const string MsgCacheUpdateStatistic = "Solo-Cache Updating Statistic: Total:{0}, Succeded:{1}, Error:{2}.";
            public const string MsgDBUpdateStatistic = "Solo-Cache DB Updated Statistic: Rows affected:{0}.";

            public const string MsgNoneCacheUpdated = "NonSolo-Cache Updated. ID: {0}, Type:{1}, Item:{2}.";
            public const string MsgNoneCacheUpdateAbnormal = "NonSolo-Cache Abnormal. ID: {0}, Type:{1}, Item:{2}. Cannot find the Assembly or Switch Off.";
            public const string MsgNoneCacheChangeError = "NonSolo-Cache Change Error. ID: {0}, Type:{1}, Key:{2}, Error:{3}.";
            public const string MsgNoneCacheUpdateStatistic = "NonSolo-Cache Updating Statistic: Total:{0}, Succeded:{1}, Error:{2}.";
            public const string MsgNoneDBUpdateStatistic = "NonSolo-Cache DB Updated Statistic: Rows affected:{0}.";
        }
        #endregion
        #region . LocalChangeDemand .
        //private static Queue<CacheUpdateInfo> _localChangeDemands = new Queue<CacheUpdateInfo>();
        //private static object _syncObj_localChangeDemands = new object();
        /// <summary>
        /// 增加一个Cache变更请求
        /// </summary>
        /// <param name="item"></param>
        public static void AddChangeDemand(CacheUpdateInfo item)
        {
            #region OLD
            ////lock (_syncObj_localChangeDemands)
            ////{
            ////    _localChangeDemands.Enqueue(item);   
            ////}
            //string err = "Bad Cache Subscribing IP List In Config File!";
            //// 2010-02-10 Liu Dong(eB1-4)         Modify ITC-1122-0084 
            //string IPAppslst = System.Configuration.ConfigurationManager.AppSettings["CSIPL"];
            //if (!string.IsNullOrEmpty(IPAppslst))
            //{
            //    string[] IPAppses = IPAppslst.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            //    if (IPAppses != null && IPAppses.Length > 0)
            //    {
            //        foreach (string IPApps in IPAppses)
            //        {
            //            string[] strs = IPApps.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            //            if (strs != null && strs.Length > 1)
            //            {
            //                item.CacheServerIP = strs[0].Trim();
            //                string[] strs_i = strs[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            //                if (strs_i != null && strs_i.Length > 0)
            //                {
            //                    foreach (string appName in strs_i)
            //                    {
            //                        item.AppName = appName.Trim();
            //                        AddCacheUpdate(item);
            //                    }
            //                }
            //                else
            //                    throw new Exception(err);
            //            }
            //            else
            //                throw new Exception(err);
            //        }
            //    }
            //    else
            //        throw new Exception(err);
            //}
            //else
            //    throw new Exception(err);
            #endregion

            IList<string[]> ipps = GetCSIPLFromCacheUpdateServer();
            if (ipps != null && ipps.Count > 0)
            {
                foreach(string[] entry in ipps)
                {
                    item.CacheServerIP = entry[0].Trim();
                    item.AppName = entry[1].Trim();
                    AddCacheUpdate(item);
                }
            }
        }
        #endregion

        #region . Tools .
        private static Type GetImplementedRepository(string interfaceName)
        {
            Type RTp = null;
            //string path = ConfigurationManager.AppSettings.Get("RepositoryImplAssembly").ToString();
            // Go with Vincent's idea to support out of box repository implementation
            string[] paths = ConfigurationManager.AppSettings.Get(CacheConstName.RepositoryImplAssembly).ToString().Split(new char[] {',',';'});
            foreach (string path in paths)
            {
                Type[] tps = Assembly.Load(path).GetTypes();
                foreach (Type tp in tps)
                {
                    if (tp.GetInterface(interfaceName) != null)
                    {
                        RTp = tp;
                        break;
                    }
                }
                if (RTp != null)
                {
                    break;
                }
            }
            return RTp;
        }

        private static object EmbodyAnInstance(Type RTp)
        {
            return Activator.CreateInstance(RTp);
        }

        private static ICache GetRAsCache(string interfaceName)
        {
            return (ICache)EmbodyAnInstance(GetImplementedRepository(interfaceName));
        }

        //private static ICache GetRAsCacheWithSwitchCheck(string interfaceName)
        //{
        //    string key = interfaceName.Replace("Repository","").TrimStart('I');
        //    if (CheckCacheSwitchOpen(key))
        //        return GetRAsCache(interfaceName);
        //    else
        //        return null;
        //}
        //private static void ExcecuteAMethod(string interfaceName, string methodName)
        //{
        //    Type tp = GetImplementedRepository(interfaceName);
        //    object obj = EmbodyAnInstance(tp);
        //    MethodInfo mi = tp.GetMethod(methodName);
        //    return mi.Invoke(obj, null);
        //}

        private static void ExcecuteAMethodWithSwitchCheck(string interfaceName, string methodName)
        {
            Type tp = GetImplementedRepository(interfaceName);
            object obj = EmbodyAnInstance(tp);
            MethodInfo mi = tp.GetMethod(methodName,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            MethodInfo mis = tp.GetMethod(CacheConstName.IsCached,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            try
            {
                if ((bool)mis.Invoke(obj, null))
                    mi.Invoke(obj, null);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(CacheConstName.MsgCacheLoadError, interfaceName, methodName, ex.InnerException.Message);
                throw ex.InnerException;
            }
        }

        private static int GetSleepMilliseconds()
        {
            int iCacheStaleCheckInterval;
            string CacheStaleCheckInterval = System.Configuration.ConfigurationManager.AppSettings[CacheConstName.CacheStaleCheckInterval]??CacheConstName.DefaultRefreshCaheTime; //default one minute
            if (int.TryParse(CacheStaleCheckInterval, out iCacheStaleCheckInterval))
            {
                iCacheStaleCheckInterval *= 60000;
            }
            else
            {
                iCacheStaleCheckInterval = 60000;
                logger.ErrorFormat(CacheConstName.MsgCacheRefreshTimeError,CacheStaleCheckInterval);
            }
            
            //if (string.IsNullOrEmpty(CacheStaleCheckInterval))
            //    //throw new Exception("Please maintain the CacheStaleCheckInterval property in AppSettings area of app.config file.");
            //else if (int.TryParse(CacheStaleCheckInterval, out iCacheStaleCheckInterval) && iCacheStaleCheckInterval > 0)
            //    iCacheStaleCheckInterval *= 60000;
            //else
            //    throw new Exception("The CacheStaleCheckInterval property in AppSettings area of app.config file should be a positive integer.");
            return iCacheStaleCheckInterval;
        }

        private readonly static string LocalHostIPAddress = GetMainIPAddress();
        private static string GetMainIPAddress()
        {
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
            foreach (IPAddress ip in ipEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    logger.InfoFormat(CacheConstName.MsgHostLocalIP, ip.ToString());
                    return ip.ToString();
                }
            }
            //throw new Exception("Cannot get local IP address.");
            throw new Exception(CacheConstName.MsgGetIPError);            
        }
        public static string GetLocalHostIPAddress
        {
            get
            {
                return LocalHostIPAddress;
            }
            //string strHostName = Dns.GetHostName(); //得到本机的主机名
            //IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
            //foreach (IPAddress ip in ipEntry.AddressList)
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            ////throw new Exception("Cannot get local IP address.");
            //logger.ErrorFormat(CacheConstName.MsgGetIPError);
            //return string.Empty;
        }

        private static string GetProcessName()
        {
            string ret = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            return ret;
        }
        #endregion

        #region . MiscRepository .
        private static object _miscRepository = null;

        private static MethodInfo _mthInfo_getAllCacheUpdate = null;
        private static CacheUpdateInfo[] GetAllCacheUpdate(string IP, string appName, string[] types)
        {
            if (null == _miscRepository)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _miscRepository = EmbodyAnInstance(miscRTp);
            }
            if (null == _mthInfo_getAllCacheUpdate)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _mthInfo_getAllCacheUpdate = miscRTp.GetMethod(CacheConstName.GetAllCacheUpdate);
            }
            try
            {
                return (CacheUpdateInfo[])_mthInfo_getAllCacheUpdate.Invoke(_miscRepository, new object[] { IP, appName, types });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static MethodInfo _mthInfo_deleteCacheUpdate = null;
        private static int DeleteCacheUpdate(CacheUpdateInfo cacheUpdateInfo)
        {
            if (null == _miscRepository)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _miscRepository = EmbodyAnInstance(miscRTp);
            }
            if (null == _mthInfo_deleteCacheUpdate)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _mthInfo_deleteCacheUpdate = miscRTp.GetMethod(CacheConstName.DeleteCacheUpdate);
            }
            try
            {
                return (int)_mthInfo_deleteCacheUpdate.Invoke(_miscRepository, new object[] { cacheUpdateInfo });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static MethodInfo _mthInfo_deleteCacheUpdateForSoloTypes = null;
        private static int DeleteCacheUpdateForSoloTypes(CacheUpdateInfo cacheUpdateInfo, string[] types)
        {
            if (null == _miscRepository)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _miscRepository = EmbodyAnInstance(miscRTp);
            }
            if (null == _mthInfo_deleteCacheUpdateForSoloTypes)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _mthInfo_deleteCacheUpdateForSoloTypes = miscRTp.GetMethod(CacheConstName.DeleteCacheUpdateForSoloTypes);
            }
            try
            {
                return (int)_mthInfo_deleteCacheUpdateForSoloTypes.Invoke(_miscRepository, new object[] { cacheUpdateInfo, types });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static MethodInfo _mthInfo_addCacheUpdate = null;
        private static void AddCacheUpdate(CacheUpdateInfo cacheUpdateInfo)
        {
            if (null == _miscRepository)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _miscRepository = EmbodyAnInstance(miscRTp);
            }
            if (null == _mthInfo_addCacheUpdate)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _mthInfo_addCacheUpdate = miscRTp.GetMethod(CacheConstName.AddCacheUpdate);
            }
            try
            {
                _mthInfo_addCacheUpdate.Invoke(_miscRepository, new object[] { cacheUpdateInfo });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static MethodInfo _mthInfo_deleteCacheUpdateByIPAddressAndAppName = null;
        private static void DeleteCacheUpdateByIPAddressAndAppName(string IP, string appName)
        {
            if (null == _miscRepository )
            {
                Type miscRTp = GetImplementedMiscRepository();
                _miscRepository = EmbodyAnInstance(miscRTp);
            }
            if (null == _mthInfo_deleteCacheUpdateByIPAddressAndAppName)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _mthInfo_deleteCacheUpdateByIPAddressAndAppName = miscRTp.GetMethod(CacheConstName.DeleteCacheUpdateByIPAddressAndAppName);
            }
            try
            {
                _mthInfo_deleteCacheUpdateByIPAddressAndAppName.Invoke(_miscRepository, new object[] { IP, appName });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static MethodInfo _mthInfo_getCSIPLFromCacheUpdateServer = null;
        private static IList<string[]> GetCSIPLFromCacheUpdateServer()
        {
            if (null == _miscRepository)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _miscRepository = EmbodyAnInstance(miscRTp);
            }
            if (null == _mthInfo_getCSIPLFromCacheUpdateServer)
            {
                Type miscRTp = GetImplementedMiscRepository();
                _mthInfo_getCSIPLFromCacheUpdateServer = miscRTp.GetMethod("GetCSIPLFromCacheUpdateServer");
            }
            try
            {
                return (IList<string[]>)_mthInfo_getCSIPLFromCacheUpdateServer.Invoke(_miscRepository, null);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static Type _miscRTp = null;
        private static Type GetImplementedMiscRepository()
        {
            if (_miscRTp == null)
            {
                _miscRTp = GetImplementedRepository("IMiscRepository");
            }
            return _miscRTp;
        }
        #endregion

        private static object _syncObj = new object();
        //private int SleepMilliseconds = -1;
        

        private static void LoadProactiveData()
        {
            ExcecuteAMethodWithSwitchCheck("IDefectRepository", "LoadAllCache");
            ExcecuteAMethodWithSwitchCheck("IDefectInfoRepository", "LoadAllCache"); //Slower
            ExcecuteAMethodWithSwitchCheck("IStationRepository", "LoadAllCache"); //StationType 不符合規範, 會報錯!
            ExcecuteAMethodWithSwitchCheck("ILineRepository", "LoadAllCache");
            ExcecuteAMethodWithSwitchCheck("IProcessRepository", "LoadAllCache");
            ExcecuteAMethodWithSwitchCheck("IFamilyRepository", "LoadAllCacheQCRatio");
        }

        private static void ClearSoloCachedData()
        {
            ExcecuteAMethodWithSwitchCheck("IPartRepository", "ClearCache");
            ExcecuteAMethodWithSwitchCheck("IFamilyRepository", "ClearCache");
            ExcecuteAMethodWithSwitchCheck("IModelRepository", "ClearCache");
            ExcecuteAMethodWithSwitchCheck("IBOMRepository", "ClearCache");
            ExcecuteAMethodWithSwitchCheck("IModelWeightRepository", "ClearCache");
            ExcecuteAMethodWithSwitchCheck("IModelToleranceRepository", "ClearCache");
        }

        /// <summary>
        /// 开始处理线程
        /// </summary>
        public static void Start()
        {
            logger.Error(GetProcessName());
            lock (_syncObj)
            {
                ClearBygoneUpdateRequest();

                LoadProactiveData();
            }
            //DataChangeMediator mediator = new DataChangeMediator();
            Thread threadMediator = new Thread(new ThreadStart(Process));
            threadMediator.Start();
        }

        private static void ClearBygoneUpdateRequest()
        {
            DeleteCacheUpdateByIPAddressAndAppName(LocalHostIPAddress, GetProcessName());
        }

        /// <summary>
        /// 处理线程的过程
        /// </summary>
        private static void Process()
        {
            bool serviceStopped = false;
            while (!serviceStopped)
            {
                try
                {
                    int interval = GetSleepMilliseconds();
                    //Thread.Sleep(interval);
                    serviceStopped = BackgroundThreadNotifier.ServiceStopNotifier.WaitOne(interval);
                    if (!serviceStopped)
                    {
                        ProcessOnce();
                    }

                    // 2010-01-29 Liu Dong(eB1-4)         Modify ITC-1122-0025 
                    #region
                    //For local Change
                    //lock (_syncObj_localChangeDemands)
                    //{
                    //    while (_localChangeDemands.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            CacheUpdateInfo cui = _localChangeDemands.Dequeue();
                    //            ProcessItem(cui);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Console.Write("Local Change: " + ex.Message);
                    //        }
                    //    }
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    logger.Error(ex);
                }
            }
        }

        private static IList<string> _soloTypes = new List<string>(new string[] { CacheType.BOM, CacheType.Family, CacheType.Model, CacheType.ModelTolerance, CacheType.ModelWeight, CacheType.Part });
        private static IList<string> _nonSoloTypes = new List<string>(new string[] { CacheType.DefectCode, CacheType.DefectInfo, CacheType.Process, CacheType.Station, CacheType.Line, CacheType.QCRatio });

        private static void ProcessOnce()
        {
            lock (_syncObj)
            {
                CacheUpdateInfo[] updateInfos_Solo = GetAllCacheUpdate(LocalHostIPAddress, GetProcessName(), _soloTypes.ToArray());
                if (updateInfos_Solo != null && updateInfos_Solo.Length > 0)
                {
                    int succCnt = 0;
                    int errrCnt = 0;
                    foreach (CacheUpdateInfo item in updateInfos_Solo)
                    {
                        try
                        {
                            if (ProcessItem(item))
                            {
                                succCnt++;
                                logger.DebugFormat(CacheConstName.MsgCacheUpdated, item.ID, item.Type, item.Item);
                            }
                            else
                                logger.InfoFormat(CacheConstName.MsgCacheUpdateAbnormal, item.ID, item.Type, item.Item);
                        }
                        catch (Exception ex)
                        {
                            errrCnt++;
                            logger.ErrorFormat(CacheConstName.MsgCacheChangeError, item.ID, item.Type, item.Item, ex.Message);
                        }
                    }
                    logger.DebugFormat(CacheConstName.MsgCacheUpdateStatistic, updateInfos_Solo.Length, succCnt.ToString(), errrCnt.ToString());

                    CacheUpdateInfo condition = updateInfos_Solo.Last();
                    int afctRCnt = DeleteCacheUpdateForSoloTypes(condition, _soloTypes.ToArray());

                    logger.DebugFormat(CacheConstName.MsgDBUpdateStatistic, afctRCnt.ToString());
                }

                CacheUpdateInfo[] updateInfos_NonSolo = GetAllCacheUpdate(LocalHostIPAddress, GetProcessName(), _nonSoloTypes.ToArray());// 2010-02-10 Liu Dong(eB1-4)         Modify ITC-1122-0084 
                if (updateInfos_NonSolo != null && updateInfos_NonSolo.Length > 0)
                {
                    int succCnt = 0;
                    int errrCnt = 0;
                    int afctRCnt = 0;
                    foreach (CacheUpdateInfo item in updateInfos_NonSolo)
                    {
                        try
                        {
                            if (ProcessItem(item))
                            {
                                succCnt++;
                                afctRCnt += DeleteCacheUpdate(item);
                                logger.DebugFormat(CacheConstName.MsgNoneCacheUpdated, item.ID, item.Type, item.Item);
                            }
                            else
                                logger.InfoFormat(CacheConstName.MsgNoneCacheUpdateAbnormal, item.ID, item.Type, item.Item);
                        }
                        catch (Exception ex)
                        {
                            errrCnt++;
                            logger.ErrorFormat(CacheConstName.MsgNoneCacheChangeError, item.ID, item.Type, item.Item, ex.Message);
                        }
                    }
                    logger.DebugFormat(CacheConstName.MsgNoneCacheUpdateStatistic , updateInfos_NonSolo.Length, succCnt.ToString(), errrCnt.ToString());
                    logger.DebugFormat(CacheConstName.MsgNoneDBUpdateStatistic, afctRCnt.ToString());
                }
            }
        }

        private static bool ProcessItem(CacheUpdateInfo item)
        {
            ICache worker = null;
            switch (item.Type)
            {
                case CacheType.Part:
                    worker = GetRAsCache(CacheConstName.IPartRepository);
                    break;

                case CacheType.Family:
                case CacheType.QCRatio:
                    worker = GetRAsCache(CacheConstName.IFamilyRepository);
                    break;

                case CacheType.Model:
                    worker = GetRAsCache(CacheConstName.IModelRepository);
                    break;

                case CacheType.BOM:
                    worker = GetRAsCache(CacheConstName.IBOMRepository);
                    break;

                case CacheType.DefectCode:
                    worker = GetRAsCache(CacheConstName.IDefectRepository);
                    break;

                case CacheType.DefectInfo:
                    worker = GetRAsCache(CacheConstName.IDefectInfoRepository);
                    break;

                case CacheType.Process:
                    worker = GetRAsCache(CacheConstName.IProcessRepository);
                    break;

                case CacheType.Station:
                    worker = GetRAsCache(CacheConstName.IStationRepository);
                    break;

                case CacheType.Line:
                    worker = GetRAsCache(CacheConstName.ILineRepository);
                    break;

                case CacheType.ModelWeight:
                    worker = GetRAsCache(CacheConstName.IModelWeightRepository);
                    break;

                case CacheType.ModelTolerance:
                    worker = GetRAsCache(CacheConstName.IModelToleranceRepository);
                    break;
            }
            //item.CacheServerIP = GetIPAddress();// 2010-02-10 Liu Dong(eB1-4)         Modify ITC-1122-0084 
            item.Updated = true;

            if (worker != null)
            {
                if (worker.IsCached())
                {
                    worker.ProcessItem(item);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        
        /// <summary>
        /// 判断一种Cache的开关是否是打开状态
        /// </summary>
        /// <param name="cst"></param>
        /// <returns></returns>
        public static bool CheckCacheSwitchOpen(string cst)
        {
            string key = string.Format(CacheConstName.CacheName,cst);
            string value = string.Empty;
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings[key];
                return value.Equals(CacheConstName.CacheOn);
            }
            catch(Exception ex)
            {
                logger.DebugFormat(CacheConstName.MsgSwitchCacheNotFound, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 做一次所有Cache的刷新操作
        /// </summary>
        public static void RefreshAllCache()
        {
            lock (_syncObj)
            {
                ClearBygoneUpdateRequest();

                ClearSoloCachedData();

                LoadProactiveData();
            }
        }

        /// <summary>
        /// 处理一个Cache变更请求
        /// </summary>
        public static void UpdateCacheNow()
        {
            ProcessOnce();
        }

        /// <summary>
        /// Cache的种类
        /// </summary>
        public static class CacheSwitchType
        {
            public const string Station = "Station";
            public const string Process = "Process";
            public const string Part = "Part";
            public const string Model = "Model";
            public const string Line = "Line";
            public const string Defect = "Defect";
            public const string DefectInfo = "DefectInfo";

            public const string BOM = "BOM";
            public const string ModelWeight = "ModelWeight";
            public const string ModelTolerance = "ModelTolerance";
            public const string Family = "Family";
        }
    }
}
