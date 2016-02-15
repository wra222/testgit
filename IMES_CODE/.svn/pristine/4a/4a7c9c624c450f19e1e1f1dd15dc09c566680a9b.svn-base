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
using IMES.SAP.Implementation;
using System.Threading;
using log4net;

namespace IMES.SAP.Service
{
    public partial class Service1 : ServiceBase
    {

        static SAPWorker workerObject = new SAPWorker();
        static Thread sapDeliveryWeightThread = null;
        static Thread sapPalletWeightThread = null;
        static Thread sapMasterWeightThread = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
             ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
             try
             {

                 logger.Info("Service Starting ........");
                 string SendSAPWeight = ConfigurationManager.AppSettings["SendSAPWeight"].ToString();
                 logger.Info("SendSAPWeight :" + SendSAPWeight);
                 #region start .Net Remoting
                 string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                 RemotingConfiguration.Configure(currentPath + "IMES.SAP.Service.exe.config", false);

                 RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                 RemotingConfiguration.CustomErrorsEnabled(false);

                 logger.Info(".Net Remoting Started !!");
                 #endregion

                 //SendSAPWeight為 ON，才會new thread.
                 if (SendSAPWeight == "ON")
                 {
                      #region start backgroud two Thread

                     // Create the thread object. This does not start the thread.
                     sapDeliveryWeightThread = new Thread(workerObject.DoWork);

                     //// Start the worker thread.
                     sapDeliveryWeightThread.Start(enumMsgType.Delivery);
                     logger.Info("sapDeliveryWeightThread Started ");

                     sapPalletWeightThread = new Thread(workerObject.DoWork);

                     // // Start the worker thread.
                     sapPalletWeightThread.Start(enumMsgType.Pallet);
                     logger.Info("sapPalletWeightThread Started ");

                     sapMasterWeightThread = new Thread(workerObject.DoWork);

                     // // Start the worker thread.
                     sapMasterWeightThread.Start(enumMsgType.Standard);
                     logger.Info("sapMasterWeightThread Started ");

                     while (!sapDeliveryWeightThread.IsAlive) ;

                     while (!sapPalletWeightThread.IsAlive) ;

                     while (!sapMasterWeightThread.IsAlive) ;

                     logger.Info("Service Started");

                     #endregion
                 }
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
                  logger.Info("Service Stopping ........");
                  // Request that the worker thread stop itself:
                 // workerObject.RequestStop(enumMsgType.Delivery);
                  workerObject.RequestStop();
                  logger.Info("Request Stop ");
                  if (sapDeliveryWeightThread != null) sapDeliveryWeightThread.Join();
                  logger.Info("Stop sapDeliveryWeightThread");

                  //workerObject.RequestStop(enumMsgType.Pallet);
                  logger.Info("Request Stop sapPalletWeightThread");
                  if (sapPalletWeightThread != null) sapPalletWeightThread.Join();
                  logger.Info("Stop sapPalletWeightThread");

                  //workerObject.RequestStop(enumMsgType.Standard);
                  logger.Info("Request Stop sapMasterWeightThread");
                  if (sapMasterWeightThread != null) sapMasterWeightThread.Join();
                  logger.Info("Stop sapMasterWeightThread"); 
                  
                  logger.Info("Service Stop");  
              }
              catch (Exception e)
              {
                  logger.Error("Stop Service Fail", e);
                  throw;
              }
        }
    }
}
