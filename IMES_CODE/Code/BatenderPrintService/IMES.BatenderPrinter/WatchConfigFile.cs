using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;
using log4net;

namespace IMES.BartenderPrinter
{
    public class ConfigurationSectionAdapter : IDisposable
    {
        private DateTime _lastWriteTime = DateTime.MinValue;
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IList<string> _configSectionName = new List<string>();
        private FileSystemWatcher _fileWatcher;

        public ConfigurationSectionAdapter(IList<string> configSectionName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                _configSectionName = configSectionName;
                StartFileWatcher();
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

        private void StartFileWatcher()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                var configurationFileDirectory = new FileInfo(Configuration.FilePath).Directory;
                string fileName = Path.GetFileName(Configuration.FilePath);

                logger.InfoFormat("Monitor Configuration path:{0} file:{1}", configurationFileDirectory.FullName, fileName);
                _fileWatcher = new FileSystemWatcher(configurationFileDirectory.FullName);
                _fileWatcher.Filter = fileName;
                _fileWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;
                _fileWatcher.Changed += ConfigFileWatcherOnChanged;
                _fileWatcher.EnableRaisingEvents = true;
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

        private void ConfigFileWatcherOnChanged(object sender, FileSystemEventArgs args)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                DateTime lastWriteTime = File.GetLastWriteTime(args.FullPath);
                if (_lastWriteTime.Equals(lastWriteTime))
                    return;

                _lastWriteTime = lastWriteTime;


                ClearCache();
                OnConfigSectionChanged();
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

        private void ClearCache()
        {
            foreach (string section in _configSectionName)
            {
                ConfigurationManager.RefreshSection(section);
            }
        }



        private System.Configuration.Configuration Configuration
        {
            get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); }
        }

        public delegate void ConfigChangedHandler();
        public event ConfigChangedHandler ConfigSectionChanged;

        protected void OnConfigSectionChanged()
        {
            if (ConfigSectionChanged != null)
                ConfigSectionChanged();
        }

        public void Dispose()
        {
            _fileWatcher.Changed -= ConfigFileWatcherOnChanged;
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Dispose();
        }
    }
}
