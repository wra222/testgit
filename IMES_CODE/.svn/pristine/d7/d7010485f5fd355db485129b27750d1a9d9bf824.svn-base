using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.ServiceProcess;
using System.Text;

namespace IMES.DockingMaintain.Service
{
    public partial class IMESDockingMaintainService : ServiceBase
    {
        public IMESDockingMaintainService()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;

            RemotingConfiguration.Configure(currentPath + "IMES.DockingMaintain.Service.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
