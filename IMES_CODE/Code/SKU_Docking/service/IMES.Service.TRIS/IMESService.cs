using System;
using System.Configuration;
using System.Runtime.Remoting;
using System.ServiceProcess;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Utility.RuleSets;
using IMES.Infrastructure.Utility.Tools;
using log4net;

namespace IMES.Service
{
    public partial class IMESService : ServiceBase
    {

        public IMESService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            ConfigurationManager.AppSettings.Set("RulePath", currentPath + ConfigurationManager.AppSettings.Get("RulePath"));
            LoadProactiveCache();
            RemotingConfiguration.Configure(currentPath + "IMES.Service.TRIS.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("IMES TRIS Service started.");
        }

        protected override void OnStop()
        {
            BackgroundThreadNotifier.ServiceStopNotifier.Set();
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("IMES TRIS Service stopped.");
        }

        private void LoadProactiveCache()
        {
            DeserializedRuleSetsManager.getInstance.LoadAll();
            DataChangeMediator.Start();
        }
    }
}
