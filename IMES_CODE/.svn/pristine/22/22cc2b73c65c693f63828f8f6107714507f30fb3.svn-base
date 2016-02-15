using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using log4net;
using System.Reflection;
using System.Threading;
using System.IO;

namespace UTL
{
    public class ExecShell
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int Run(string cmdName,
                                         string cmdArgu)
        {

            int retValue=-1;
            try
            {
                Process process1 = new Process();
                //Do not receive an event when the process exits.
                
                process1.EnableRaisingEvents = false;

                //The "/C" Tells Windows to Run The Command then Terminate 
                //string strCmdLine;
                //strCmdLine = "/C " + cmdArgu;
                process1.StartInfo.FileName = cmdName;
                process1.StartInfo.Arguments = cmdArgu;
                process1.StartInfo.CreateNoWindow = true;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.RedirectStandardOutput = true;
                
                process1.Start();
                logger.Info("ExecShell.Run Start Command Start " + cmdName + " " + cmdArgu);
                logger.Info("ExecShell.Run  Output " +process1.StandardOutput.ReadToEnd());
                process1.WaitForExit();
                retValue = process1.ExitCode;
                process1.Close();
                logger.Info("ExecShell.Run End Command   return value=" + retValue.ToString());
                return retValue;
            }
            catch (Exception e)
            {
                logger.Error("ExecShell.Run StackTrace "+ e.StackTrace);
                logger.Error("ExecShell.Run Message "+ e.Message);
                return -99;
            }

        }

        // if can't find checkStr in output string then reutrn -98
        // return -99 internal process error
        public static int Run(string cmdName,
                                         string cmdArgu,                                         
                                         string checkStr )
        {

            int retValue = -1;
            
            try
            {
                Process process1 = new Process();
                //Do not receive an event when the process exits.

                process1.EnableRaisingEvents = false;

                //The "/C" Tells Windows to Run The Command then Terminate 
                //string strCmdLine;
                //strCmdLine = "/C " + cmdArgu;
                process1.StartInfo.FileName = cmdName;
                process1.StartInfo.Arguments = cmdArgu;
                process1.StartInfo.CreateNoWindow = true;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.RedirectStandardOutput = true;

                process1.Start();
                logger.Info("ExecShell.Run Start Command Start " + cmdName + " " + cmdArgu);
                string outStr=process1.StandardOutput.ReadToEnd();
                logger.Info("ExecShell.Run  Output " + outStr);
                process1.WaitForExit();
                retValue = process1.ExitCode;
                process1.Close();
                if (retValue == 0 && !outStr.EndsWith(checkStr))
                    retValue = -98;
                logger.Info("ExecShell.Run  End Command return value=" + retValue.ToString());
                return retValue;
            }
            catch (Exception e)
            {
                logger.Error("ExecShell.Run StackTrace " + e.StackTrace);
                logger.Error("ExecShell.Run Message " + e.Message);
                return -99;
            }


        }

        public static bool CheckOneProcessRun()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            bool ret = true;
            try
            {

                bool is_createdNew;
                Mutex mu = null;
                string mutexName1 = Process.GetCurrentProcess().MainModule.FileName
                    .Replace(Path.DirectorySeparatorChar, '_');
                mu = new Mutex(true, "Global\\" + mutexName1, out is_createdNew);
                if (!is_createdNew)
                {
                    logger.Debug("Process is running, can not  run more than one  process in same time, Please waiting process completed.........");
                    ret = false;
                }

                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }

        }
    }
}
