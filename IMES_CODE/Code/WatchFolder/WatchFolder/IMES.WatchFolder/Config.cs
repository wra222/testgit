using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Globalization;
using System.IO;
using log4net;
using System.Reflection;

namespace UTL
{
    public class AppConfig
    {
        public readonly int ThreadCount;
        public readonly int SessionTimeOut;

        public readonly string LoginRemoteServerDomain;
        public readonly string LoginRemoteServerUser;
        public readonly string LoginRemoteServerPassword;

        public readonly string FolderPath;
        public readonly string FileFilter;
        public readonly bool IncludeSubdirectories = false;
        public readonly int InternalBufferSize = 64;
        public readonly int MonitorIntervalTime = 200;
        

        public readonly bool IsNeesBackUpAfterCompleted=false;
        public readonly string BckupFolder;
        public readonly string BadXmlFolder;
        public readonly string NoRootTagNameFolder;
        public readonly string ErrorFolder;


        public readonly string FileFormat="TXT";
        public readonly string XMLPath;
        public readonly string TXTDelimiter = ",";
        public readonly string[] TXTColumnName;

        public readonly string SQL2008;
        public readonly string IMESSPName;
        

        public readonly string EmailServer;
        public readonly string FromAddress;
        public readonly string[] ToAddress;
        public readonly string[] CcAddress;
        public readonly string MailSubject;

        public readonly string DBConnectStr;
        public readonly string IsFileNameMonitor;
       

        public AppConfig()
        {

            //ThreadCount = int.Parse(string.IsNullOrEmpty(get_config_value("ThreadCount")) ? "5" : get_config_value("ThreadCount"));
            //SessionTimeOut = int.Parse(string.IsNullOrEmpty(get_config_value("SessionTimeOut")) ? "5" : get_config_value("SessionTimeOut"));
            ThreadCount = get_config_value("ThreadCount", 5);
            SessionTimeOut = get_config_value("SessionTimeOut", 5);

            LoginRemoteServerDomain = get_config_value("LoginRemoteServerDomain");
            LoginRemoteServerUser = get_config_value("LoginRemoteServerUser");
            LoginRemoteServerPassword = get_config_value("LoginRemoteServerPassword");

            FolderPath = get_config_value("FolderPath");
            FileFilter = get_config_value("FileFilter");
            //IncludeSubdirectories = string.IsNullOrEmpty(get_config_value("IncludeSubdirectories")) ? false :
            //    get_config_value("IncludeSubdirectories").ToUpper() == "Y" ? true : false;
            IncludeSubdirectories = get_config_value<string>("IncludeSubdirectories", "N") == "Y" ? true : false;

            //InternalBufferSize = string.IsNullOrEmpty(get_config_value("InternalBufferSize")) ? 64 :int.Parse(get_config_value("InternalBufferSize"));
            //MonitorIntervalTime = string.IsNullOrEmpty(get_config_value("MonitorIntervalTime")) ? 200 : int.Parse(get_config_value("MonitorIntervalTime")); ;
            InternalBufferSize = get_config_value("InternalBufferSize", 64);
            MonitorIntervalTime = get_config_value("MonitorIntervalTime", 200);

            //IsNeesBackUpAfterCompleted = string.IsNullOrEmpty(get_config_value("IsNeesBackUpAfterCompleted")) ? false :
            //    get_config_value("IsNeesBackUpAfterCompleted").ToUpper() == "Y" ? true : false;
            IsNeesBackUpAfterCompleted = (get_config_value<string>("IsNeesBackUpAfterCompleted", "N") == "Y" ? true : false);
                         
            BadXmlFolder = get_config_value("BadXmlFolder");
            BckupFolder = get_config_value("BckupFolder");
            NoRootTagNameFolder = get_config_value("NoRootTagNameFolder");
            ErrorFolder = get_config_value("ErrorFolder");


            //FileFormat = string.IsNullOrEmpty(get_config_value("FileFormat")) ? "TXT" : 
            //                      get_config_value("FileFormat").ToUpper()=="XML" ?"XML":"TXT";

            FileFormat = get_config_value("FileFormat", "TXT");

            XMLPath = get_config_value("XMLPath");
            SQL2008 = (get_config_value("SQL2008")?? "Y").ToUpper();
            IMESSPName = get_config_value("IMESSPName");
            IsFileNameMonitor = get_config_value("IsFileNameMonitor");
            //TXTDelimiter = string.IsNullOrEmpty(get_config_value("TXTDelimiter")) ? "," : get_config_value("TXTDelimiter");
            TXTDelimiter = get_config_value("TXTDelimiter", ",");
           // TXTColumnName = string.IsNullOrEmpty(get_config_value("TXTColumnName")) ? new string[0] : get_config_value("TXTColumnName").Split(new char[] { ',', ';' });
            TXTColumnName = get_config_value<string>("TXTColumnName", "").Split(new char[] { ',', ';' });
            DBConnectStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                      
            EmailServer = get_config_value("MailServer");
            FromAddress = get_config_value("FromAddress");
            ToAddress = get_config_value("ToAddress").Split(new char[] { ';', ',' });
            CcAddress = get_config_value("CcAddress").Split(new char[] { ';', ',' });
            MailSubject = get_config_value("MailSubject");


        }

        private string get_config_value(string _name)
        {

            try
            {
                string s = ConfigurationManager.AppSettings[_name];
                if (string.IsNullOrEmpty(s))
                {
                    return "";
                }
                else
                {
                    return s;
                }
            }
            catch
            {
                return "";
            }
        }



        private T get_config_value<T>(string name, T defaultValue) where T : IConvertible
        {
            try
            {
                string s = ConfigurationManager.AppSettings[name];
                if (string.IsNullOrEmpty(s))
                {
                    return defaultValue;
                }
                else
                {
                    return (T)Convert.ChangeType(s, typeof(T));
                }
            }catch(Exception e)
            {
                throw e;
               //return defaultValue;
            }
        }
        public static Dictionary<string, BUItem> readSection(string sectionName)
        {
            BUSection section = new BUSection();
            return section.get_section(sectionName);
        }

    }

    public class BUSection : IConfigurationSectionHandler
    {
        public BUSection() { }

        public object Create(object parent,
                                         object configContext,
                                         XmlNode section)
        {
            Dictionary<string, BUItem> items = new Dictionary<string, BUItem>();
            System.Xml.XmlNodeList Nodes = section.ChildNodes;
            foreach (XmlElement node in Nodes)
            {
                BUItem item = new BUItem();
                item.Name = getXmAttributeValue(node, "Name");
                item.SPName = getXmAttributeValue(node, "SPName");
                item.ErrorEmail = getXmAttributeValue(node, "ErrorEmail").Split(new char[] { ';', ',' }); ;
                item.ConnectionString = getXmAttributeValue(node, "ConnectionString");
                items.Add(item.Name, item);
            }
            return items;
        }


        public Dictionary<string, BUItem> get_section(string _sectionName)
        {
            return (Dictionary<string, BUItem>)ConfigurationManager.GetSection(_sectionName);
        }

        private string getXmAttributeValue(XmlElement element_, string name_)
        {
            try
            {
                if (element_ == null) { return ""; }
                if (element_.HasAttribute(name_))
                { return element_.Attributes[name_].Value; }
                else
                { return ""; }
            }
            catch
            {
                return "";
            }


        }
        private string getXmAttributeValue(XmlNode node_, string name_)
        {
            return getXmAttributeValue((XmlElement)node_, name_);
        }



    }

    public struct BUItem
    {
        public string Name;
        public string SPName;
        public string[] ErrorEmail;
        public string ConnectionString;
    }


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
