using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using IMES.FisObject.Common.BOM;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.RuleSets;
using IMES.Infrastructure.Utility.Cache;

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
            RemotingConfiguration.Configure(currentPath + "IMES.Service.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
        }

        protected override void OnStop()
        {

        }

        private void LoadProactiveCache()
        {
            DeserializedRuleSetsManager.getInstance.LoadAll();
            DataChangeMediator.Start();
        }
    }
}
