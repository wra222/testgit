using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Configuration;
using System.Threading;
using log4net;


namespace IMES.BartenderPrinter
{
    class Program
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
        static void Main(string[] args)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
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
                //RemotingConfiguration.Configure("IMES.BartenderPrinter.HostConsole.exe.config", false);
                //RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                //RemotingConfiguration.CustomErrorsEnabled(false);
                 RegisterRemoting.RegistRemotingObj("BartnderService", 
                                                                            port, 
                                                                            "IMES.BartenderPrinter.BartenderLabelMgt", 
                                                                            "IMES.BartenderPrinter", 
                                                                            "BartenderLabel");
                logger.Debug("Remoting started ......");
                #endregion

                string usingTaskManger = ConfigurationManager.AppSettings["UsingTaskManger"] ?? "N";
                logger.InfoFormat("UsingTaskManger:{0} ", usingTaskManger);

                #region start Timer
                if (usingTaskManger == "N")
                {
                    BartenderPool.Init(btAppCount);
                }
                else
                {
                    BtTaskManager.Start(btAppCount);
                }
                //int timerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
                //int idleTime =int.Parse(ConfigurationManager.AppSettings["BatenderAppIdleTime"]);
                //BatenderPool.StartTimer(timerInterval, idleTime);
                //logger.DebugFormat("BatenderPool.Timer started Interval:{0} IdleTime:{1}......", timerInterval.ToString(), idleTime.ToString());
                #endregion

                logger.Debug("Start Completed ......");

                Console.ReadLine();

                try
                {
                    if (usingTaskManger == "N")
                    {
                        BartenderPool.Shutdown();
                    }
                    else
                    {
                        BtTaskManager.Stop();
                    }
                }
                catch (Exception e1)
                {
                    logger.Error(e1.Message, e1);  
                }
                logger.Debug("BatenderPool Shut down and press any key stop console ......");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);        
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
           
                        
        }
    }
}
