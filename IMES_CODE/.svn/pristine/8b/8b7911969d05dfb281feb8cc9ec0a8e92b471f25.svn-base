using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Configuration;
using IMES.SAP.Implementation;
using System.Threading;

namespace IMES.SAP.HostConsole
{
    class Program
    {
        static SAPWorker workerObject = new SAPWorker();
        private static Thread sapDeliveryWeightThread = null;
        private static Thread sapPalletWeightThread = null;
        private static Thread sapMasterWeightThread = null;

        static void Main(string[] args)
        {
            #region .net remoting
            ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["OnlineDBServer"];
            Console.WriteLine(ConnectionString.ConnectionString);
            ConnectionString = ConfigurationManager.ConnectionStrings["HistoryDBServer"];
            Console.WriteLine(ConnectionString.ConnectionString);
            RemotingConfiguration.Configure("IMES.SAP.HostConsole.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            #endregion

            #region start backgroud two Thread
            
            // Create the thread object. This does not start the thread.
            sapDeliveryWeightThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            sapDeliveryWeightThread.Start(enumMsgType.Delivery);
            

            sapPalletWeightThread = new Thread(workerObject.DoWork);
            sapMasterWeightThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            sapPalletWeightThread.Start(enumMsgType.Pallet);
            sapMasterWeightThread.Start(enumMsgType.Standard);         

            while (!sapDeliveryWeightThread.IsAlive) ;

            while (!sapPalletWeightThread.IsAlive) ;
            while (!sapMasterWeightThread.IsAlive) ;

            #endregion

            Console.WriteLine("Start Completed ......");

            Console.ReadLine();
            //workerObject.RequestStop(enumMsgType.Delivery);
            //workerObject.RequestStop(enumMsgType.Pallet);
            /*
            workerObject.RequestStop();
            sapDeliveryWeightThread.Join();
            sapPalletWeightThread.Join();
            */
                        
        }
    }
}
