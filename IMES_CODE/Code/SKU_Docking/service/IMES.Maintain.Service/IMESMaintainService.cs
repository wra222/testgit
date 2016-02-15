using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
namespace IMES.Maintain.Service
{
    public partial class IMESMaintainService : ServiceBase
    {
        public IMESMaintainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;

            RemotingConfiguration.Configure(currentPath + "IMES.Maintain.Service.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
        }

        protected override void OnStop()
        {
        }
    }
}
