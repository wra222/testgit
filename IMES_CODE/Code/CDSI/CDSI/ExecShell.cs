using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace UTL
{
    class ExecShell
    {
        public static int Run(string cmdName,
                                         string cmdArgu,
                                         Log log)
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
                log.write(LogType.Info, 0, "ExecShell.Run", "Start Command", "Start " + cmdName + " " + cmdArgu);
                log.write(LogType.Info, 0, "ExecShell.Run", "Output", process1.StandardOutput.ReadToEnd());
                process1.WaitForExit();
                retValue = process1.ExitCode;
                process1.Close();
                log.write(LogType.Info, 0, "ExecShell.Run", "End Command", "return value=" + retValue.ToString());
                return retValue;
            }
            catch (Exception e)
            {
                log.write(LogType.error, 0, "ExecShell.Run", "StackTrace", e.StackTrace);
                log.write(LogType.error, 0, "ExecShell.Run", "Message", e.Message);
                return -99;
            }

        }

        // if can't find checkStr in output string then reutrn -98
        // return -99 internal process error
        public static int Run(string cmdName,
                                         string cmdArgu,
                                         Log log,
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
                log.write(LogType.Info, 0, "ExecShell.Run", "Start Command", "Start " + cmdName + " " + cmdArgu);
                string outStr=process1.StandardOutput.ReadToEnd();
                log.write(LogType.Info, 0, "ExecShell.Run", "Output", outStr);
                process1.WaitForExit();
                retValue = process1.ExitCode;
                process1.Close();
                if (retValue == 0 && !outStr.EndsWith(checkStr))
                    retValue = -98;
                log.write(LogType.Info, 0, "ExecShell.Run", "End Command", "return value=" + retValue.ToString());
                return retValue;
            }
            catch (Exception e)
            {
                log.write(LogType.error, 0, "ExecShell.Run", "StackTrace", e.StackTrace);
                log.write(LogType.error, 0, "ExecShell.Run", "Message", e.Message);
                return -99;
            }


        }
    }
}
