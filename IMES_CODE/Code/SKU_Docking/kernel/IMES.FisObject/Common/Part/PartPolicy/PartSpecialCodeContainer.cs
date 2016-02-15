using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Threading;
using IMES.CheckItemModule.Interface;
using log4net;
using System.ComponentModel.Composition.Primitives;
using IMES.Infrastructure.Util;

namespace IMES.FisObject.Common.Part.PartPolicy
{
    public class PartSpecialCodeContainer
    {
        
        #region Singleton
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "PartPolicyModule";
        private static readonly string ProgramName = "ProgramName";
      
        private static readonly PartSpecialCodeContainer Instance = new PartSpecialCodeContainer();
        public static PartSpecialCodeContainer GetInstance
        {
            get
            {
                return Instance;
            }
        }

        private PartSpecialCodeContainer()
        {
            Compose();
        }
 
	#endregion

        private DirectoryCatalog _directoryCatalog = null;

        [ImportMany(typeof(IFilterModule), AllowRecomposition = true)]
        private Lazy<IFilterModule, IDictionary<String, Object>>[] _bomFilterImplementations ;

        [ImportMany(typeof(IFilterModuleEx), AllowRecomposition = true)]
        private Lazy<IFilterModuleEx, IDictionary<String, Object>>[] _bomFilterExImplementations;

        [ImportMany(typeof(IMatchModule), AllowRecomposition = true)]
        private Lazy<IMatchModule, IDictionary<String, Object>>[] _matchImplementations;

        [ImportMany(typeof(ICheckModule), AllowRecomposition = true)]
        private Lazy<ICheckModule, IDictionary<String, Object>>[] _checkImplementations;

        [ImportMany(typeof(ISaveModule), AllowRecomposition = true)]
        private Lazy<ISaveModule, IDictionary<String, Object>>[] _saveImplementations;

        public IFilterModule GetFilterModule(string name)
        {
            IFilterModule ret = null;
            foreach (Lazy<IFilterModule, IDictionary<String, Object>> module in _bomFilterImplementations)
            {
                if (module.Metadata.ContainsKey(ProgramName) && string.Compare(module.Metadata[ProgramName].ToString(), name) == 0)
                {
                    ret = module.Value;
                }
            }
            return ret;
        }

        public IFilterModuleEx GetFilterModuleEx(string name)
        {
            IFilterModuleEx ret = null;
            foreach (Lazy<IFilterModuleEx, IDictionary<String, Object>> module in _bomFilterExImplementations)
            {
                if (module.Metadata.ContainsKey(ProgramName) && string.Compare(module.Metadata[ProgramName].ToString(), name) == 0)
                {
                    ret = module.Value;
                }
            }
            return ret;
        }

        public IMatchModule GetMatchModule(string name)
        {
            IMatchModule ret = null;
            foreach (Lazy<IMatchModule, IDictionary<String, Object>> module in _matchImplementations)
            {
                if (module.Metadata.ContainsKey(ProgramName) && string.Compare(module.Metadata[ProgramName].ToString(), name) == 0)
                {
                    ret = module.Value;
                }
            }
            return ret;
        }

        public ICheckModule GetCheckModule(string name)
        {
            ICheckModule ret = null;
            foreach (Lazy<ICheckModule, IDictionary<String, Object>> module in _checkImplementations)
            {
                if (module.Metadata.ContainsKey(ProgramName) && string.Compare(module.Metadata[ProgramName].ToString(), name) == 0)
                {
                    ret = module.Value;
                }
            }
            return ret;
        }

        public ISaveModule GetSaveModule(string name)
        {
            ISaveModule ret = null;
            foreach (Lazy<ISaveModule, IDictionary<String, Object>> module in _saveImplementations)
            {
                if (module.Metadata.ContainsKey(ProgramName) && string.Compare(module.Metadata[ProgramName].ToString(), name) == 0)
                {
                    ret = module.Value;
                }
            }
            return ret;
        }

        private void Compose()
        {
            string methodName = "Compose";
            logger.DebugFormat(GlobalConstName.LogFormat.Begin, methodName);
            try
            {
                AggregateCatalog catalog = new AggregateCatalog();
                catalog.Changed += new EventHandler<ComposablePartCatalogChangeEventArgs>(catalog_Changed);
                _directoryCatalog = new DirectoryCatalog(Path);
                //catalog.Catalogs.Add(new DirectoryCatalog(Path));
                catalog.Catalogs.Add(_directoryCatalog);
                CompositionContainer container = new CompositionContainer(catalog);
                AttributedModelServices.ComposeParts(container, this);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat(GlobalConstName.LogFormat.End, methodName);
            }

        }

        #region disable watch dll file and change manual mode
//        public void Watcher()
//        {
//            FileSystemWatcher fsw = new FileSystemWatcher(Path);
//            while (true)
//            {
//                WaitForChangedResult cr = fsw.WaitForChanged(WatcherChangeTypes.All);
//                Thread.Sleep(1000);
//                Logger.Info("PartPolicy updated: " + cr.ToString());
//                Refresh();
//            }
//// ReSharper disable FunctionNeverReturns
//        }
//// ReSharper restore FunctionNeverReturns

//        public void StartWatcher()
//        {
//            Thread threadwatcher = new Thread(Watcher);
//            threadwatcher.Start();
//        }

//        private void Refresh()
//        {
//            Compose();
//        }
        #endregion

        public void RefreshParts()
        {
            string methodName ="RefreshParts";
            logger.DebugFormat(GlobalConstName.LogFormat.Begin, methodName);
            try
            {
                _directoryCatalog.Refresh();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat(GlobalConstName.LogFormat.End, methodName);
            }

        }

        void catalog_Changed(object sender, ComposablePartCatalogChangeEventArgs e)
        {
            string methodName = "catalog_Changed";
            logger.DebugFormat(GlobalConstName.LogFormat.Begin, methodName);
            try
            {
                if (logger.IsInfoEnabled)
                {
                    foreach (ComposablePartDefinition item in e.AddedDefinitions)
                    {
                        foreach (ExportDefinition ed in item.ExportDefinitions)
                        {
                            logger.Info("Add " + string.Join(" , ", ed.Metadata.Select(x => x.Key + "," + x.Value.ToString()).ToArray()));
                        }
                    }

                    foreach (ComposablePartDefinition item in e.RemovedDefinitions)
                    {
                        foreach (ExportDefinition ed in item.ExportDefinitions)
                        {
                            logger.Info("Remove " + string.Join(" , ", ed.Metadata.Select(x => x.Key + "," + x.Value.ToString()).ToArray()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                logger.DebugFormat(GlobalConstName.LogFormat.End, methodName);
            }

        }
    }
}