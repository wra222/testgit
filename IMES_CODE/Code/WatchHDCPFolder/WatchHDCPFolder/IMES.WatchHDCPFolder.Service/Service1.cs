using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading;
using log4net;
using IMES.WatchHDCPFolder;
using UTL;

namespace IMES.WatchHDCPFolder.Service
{
    public partial class Service1 : ServiceBase
    {
       
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
             ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
             try
             {
                 logger.Info("===========================================================================================================================================");
                 string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                 logger.Info("WatchFolder Service Service Starting ......");

                 //AppConfig config = new AppConfig();
                 //WatchFolder.Init(config);
                 WatchFolder.Init();

                 logger.Info("WatchFolder Service Service Started");



             }
             catch (Exception e)
             {
                 logger.Error("Start Service Fail", e);
                 throw;
             }
             finally
             {

             }
        }

        protected override void OnStop()
        {
              ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
              try
              {
                   logger.Info("WatchFolder Service Stop .....");  
                  WatchFolder.Stop();
                  logger.Info("WatchFolder Service Stopped");  

              }
              catch (Exception e)
              {
                  logger.Error("WatchFolder Stop Service Fail", e);
                  throw;
              }
        }
    }
}
