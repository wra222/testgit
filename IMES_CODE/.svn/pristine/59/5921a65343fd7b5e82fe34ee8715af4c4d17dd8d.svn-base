using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Remoting;
using System.Configuration;
using System.Threading;
using log4net;

namespace IMES.BartenderPrinter
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

                 logger.InfoFormat("Service Starting ........");
               
                 string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                 #region .net remoting
                 ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["OnlineDBServer"];
                 logger.DebugFormat("DB Connect:{0}", ConnectionString);
                 string btwFolder = ConfigurationManager.AppSettings["BartenderTemplateFolder"];
                 if (string.IsNullOrEmpty(btwFolder))
                 {
                     throw new Exception("no setting BartenderTemplateFolder value in app.confg!!");
                 }

                 string tempFolder = ConfigurationManager.AppSettings["TempFolder"];
                 if (string.IsNullOrEmpty(tempFolder))
                 {
                     throw new Exception("no setting TempFolder value in app.confg!!");
                 }

                 string iMESdb = ConfigurationManager.AppSettings["DB_IMES"];

                 if (string.IsNullOrEmpty(iMESdb))
                 {
                     throw new Exception("no setting DB_IMES value in app.confg!!");
                 }

                 int btAppCount = int.Parse(ConfigurationManager.AppSettings["BartenderAplicationCount"] ?? "4");
                 logger.InfoFormat("BartenderTemplateFolder:{0} TempFolder:{1} DB_IMES:{2} BtApp Count:{3}", btwFolder, tempFolder, iMESdb, btAppCount);

                 int port = int.Parse(ConfigurationManager.AppSettings["RemotingServicePort"] ?? "8588");
                 //RemotingConfiguration.Configure(currentPath + "IMES.BartenderPrinter.Service.exe.config", false);
                 //RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                 //RemotingConfiguration.CustomErrorsEnabled(false);
                 RegisterRemoting.RegistRemotingObj("BartnderService", 
                                                                            port, 
                                                                        "IMES.BartenderPrinter.BartenderLabelMgt", 
                                                                        "IMES.BartenderPrinter", 
                                                                        "BartenderLabel");
                 #endregion

                 logger.InfoFormat(".Net Remoting Started !!");
                 
                 string usingTaskManger = ConfigurationManager.AppSettings["UsingTaskManger"] ?? "N";
                 logger.InfoFormat("UsingTaskManger:{0} ", usingTaskManger);

                 #region start Timer
                 string delayTime = ConfigurationManager.AppSettings["StartServiceDelayTime"] ?? "10";

                 base.RequestAdditionalTime(int.Parse(delayTime) * 60000);
                 if (usingTaskManger == "N")
                 {
                     BartenderPool.Init(btAppCount);
                 }
                 else
                 {
                     BtTaskManager.Start(btAppCount);
                 }
                 //int timerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
                 //int idleTime = int.Parse(ConfigurationManager.AppSettings["BatenderAppIdleTime"]);
                 //BatenderPool.StartTimer(timerInterval, idleTime);
                 //logger.InfoFormat("BatenderPool.Timer started Interval:{0} IdleTime:{1}......", timerInterval.ToString(), idleTime.ToString());
                 #endregion
               
             }
             catch (Exception e)
             {
                 logger.Error("Start Service Fail", e);
                 throw;
             }      
        }

        protected override void OnStop()
        {
              ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
              try
              {
                  logger.InfoFormat("Service Stopping ........");
                  int btAppCount = int.Parse(ConfigurationManager.AppSettings["BartenderAplicationCount"] ?? "6");
                  base.RequestAdditionalTime(btAppCount * 60 * 1000);

                  string usingTaskManger = ConfigurationManager.AppSettings["UsingTaskManger"] ?? "N";
                  logger.InfoFormat("UsingTaskManger:{0} ", usingTaskManger);
                  if (usingTaskManger == "N")
                  {
                      BartenderPool.Shutdown();
                  }
                  else
                  {
                      BtTaskManager.Stop();
                  }
                  logger.InfoFormat("Service Stop");  
              }
              catch (Exception e)
              {
                  logger.Error("Stop Service Fail", e);
                  throw;
              }
        }
    }
}
