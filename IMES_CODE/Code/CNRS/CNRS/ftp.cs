using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml;

namespace UTL
{
    public class ftp
    {
        public static void ftpfile(string ftpHost, 
                                             string ftpPath, 
                                             string ftpFile,
                                             string localPath, 
                                             string localFile,
                                             string userName, 
                                             string password)
        {

            //here correct hostname or IP of the ftp server to be given
            try
            {
                string ftpfullpath = "ftp://" + ftpHost + ftpPath + ftpFile;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                Console.WriteLine(" ftp Path =>" + ftpfullpath);
                ftp.Credentials = new NetworkCredential(userName, password);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.ConnectionGroupName = "FtpUploadGroup";
                //ftp.ServicePoint.ConnectionLimit = 1;
                //ftp.Timeout = 60000;
                string inputfilepath = localPath + localFile;
                FileStream fs = File.OpenRead(inputfilepath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Console.WriteLine("close read file");
                Console.WriteLine(" Ftp URL=>" + ftp.RequestUri.ToString());
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
                Console.WriteLine(" ftp close");
            }
            catch (Exception e)
            {
                Exception e1 = new Exception(e.StackTrace + " " + e.Message);
                throw e1;
            }

        }

        public static int psftp(string executeName,
                                            string argument,
                                            Log log)
        {
            return ExecShell.Run(executeName, argument, log,"quit\r\n");
        }

        public static void GenFtpBatchFile(string batchFile,
                                                                 List<string> cmdList)
        {
            FileStream fs=  FileOperation.CreateFile(batchFile);
            foreach( string cmdtxt in cmdList)
            {
                FileOperation.write(fs, cmdtxt, true);
            }
            fs.Close();
        }

        public static int RunWinSCP(string executeName,
                                                       string ftpBatchCmd,
                                                        Log log)
        {
            //const string logname = "WinScpLog.xml";
            Process winscp = new Process();
            winscp.StartInfo.FileName = executeName;
            //winscp.StartInfo.Arguments = "/log=\"" + logname + "\"";
            winscp.StartInfo.UseShellExecute = false;
            winscp.StartInfo.RedirectStandardInput = true;
            winscp.StartInfo.RedirectStandardOutput = true;
            winscp.StartInfo.CreateNoWindow = true;
            winscp.Start();
            winscp.StandardInput.WriteLine(ftpBatchCmd);
            log.write(LogType.Info, 0, "RunWinSCP", "BatchFile", ftpBatchCmd);
            winscp.StandardInput.Close();
            string output = winscp.StandardOutput.ReadToEnd();
            log.write(LogType.Info, 0, "RunWinSCP", "Output", ftpBatchCmd);
            winscp.WaitForExit();
            //XPathDocument scpLog = new XPathDocument(logname);
            //XmlNamespaceManager ns = new XmlNamespaceManager(new NameTable());
            //ns.AddNamespace("w", "http://winscp.net/schema/session/1.0");
            //XPathNavigator nav = scpLog.CreateNavigator();

            if (winscp.ExitCode != 0)
            {
                log.write(LogType.Info, 0, "RunWinSCP", "Result", "error code " + winscp.ExitCode.ToString());
                return winscp.ExitCode;
            }
            else
            {
                //XPathNodeIterator files = nav.Select("//w:filename", ns);
                //Console.WriteLine(string.Format("There are {0} files and subdirectories:", files.Count));
                //foreach (XPathNavigator file in files)
                //{
                //    Console.WriteLine(file.SelectSingleNode("w:filename/@value", ns).Value);
                //}
                log.write(LogType.Info, 0, "RunWinSCP", "Result", "OK");
                return winscp.ExitCode;
            }

        }

    }
}
