using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security.Permissions;
using System.IO;
using System.Net;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Reflection;
using UTL;
using UTL.SQL;

namespace IMES.WatchFolder.Service
{
    class HostConsole
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.Info("=======================================================================================");
            logger.InfoFormat("BEGIN: {0}()", methodName);        
            //AppConfig config = new AppConfig();
            //string msg = "";
            try
            {

                DateTime now = DateTime.Now;
                //WatchFolder.Init(config);
                WatchFolder.Init();

                Console.ReadKey();
                WatchFolder.Stop();
            }
            catch (Exception e)
            {
                #region exception handle
                logger.Error(methodName, e);
                //SendMail.Send(config.FromAddress,
                //                          config.ToAddress,
                //                          config.CcAddress,
                //                          config.MailSubject + "程式報錯",
                //                          e.StackTrace + " " + e.Message,
                //                          config.EmailServer);
                #endregion
            }
            finally
            {
                #region error send email
                //if (!string.IsNullOrEmpty(msg))
                //{
                //    SendMail.Send(config.FromAddress,
                //                                      config.ToAddress,
                //                                      config.CcAddress,
                //                                      config.MailSubject + " 資料有誤!!",
                //                                      msg,
                //                                      config.EmailServer);
                //}

                #endregion
                
                logger.InfoFormat("END: {0}()", methodName);
            }

        }
    }   
}
