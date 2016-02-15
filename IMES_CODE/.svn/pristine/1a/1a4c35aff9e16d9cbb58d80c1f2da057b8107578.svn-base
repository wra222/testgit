using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.ServiceProcess;
using System.Text;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Utility.RuleSets;
using log4net;

namespace IMES.Docking.Service
{
    partial class IMESDockingService : ServiceBase
    {
        public IMESDockingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            ConfigurationManager.AppSettings.Set("RulePath", currentPath + ConfigurationManager.AppSettings.Get("RulePath"));
            LoadProactiveCache();
            RemotingConfiguration.Configure(currentPath + "IMES.Docking.Service.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("IMES Docking Service started.");
        }

        protected override void OnStop()
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("IMES Docking Service stopped.");
        }

        private void LoadProactiveCache()
        {
            DeserializedRuleSetsManager.getInstance.LoadAll();
            DataChangeMediator.Start();
        }
    }
}
