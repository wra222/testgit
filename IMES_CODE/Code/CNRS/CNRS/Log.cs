using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UTL
{   
    public enum LogType
    { 
        Info = 1,
        warning = 2,
        error = 3,
    }

    public class Log
    {
        private const string gMsTimeFormat_ = "yyyy-MM-dd HH:mm:ss.fff";
        private const string gHourFormat_ = "yyMMddHH";

        private const string gLogPrefixStr_ = @"<Log>";
        private const string gLogPostfixStr_ = @"</Log>";
        private const string gLogStr_ = @"<LogInfo time=""{0}"" type=""{1}"" handle={2} action=""{3}"" direction=""{4}"">{5}</LogInfo>";

        private string prefixName_;
        private string path_;
        FileStream logFile;
    
        public Log(string _path,
                        string _prefixName)
        {
            prefixName_ = _prefixName;
            path_ = _path;
        }

        public string fileName;
   
        public bool write(LogType _logType,
                                    int        _handle,
                                    string        _actionName,
                                    string        _direction,
                                    string        _logMsg)
        {
            logFile = null;
            try
            {
#if  CONSOLE
                Console.WriteLine(DateTime.Now.ToString(gMsTimeFormat_) + "|" + _logType.ToString() +"|" +_actionName +"|"+_direction+ "|" + _logMsg);
#endif

                if (!Directory.Exists(path_))
                {
                    Directory.CreateDirectory(path_);
                }

                fileName = path_ + prefixName_ + "_" + DateTime.Now.ToString(gHourFormat_) + ".log";

                //string fileName = path_ + prefixName_ + "_" + DateTime.Now.ToString(gHourFormat_) + ".log";
                string logStr = string.Format(gLogStr_,
                                             DateTime.Now.ToString(gMsTimeFormat_),
                                                                  _logType,
                                                                   _handle,
                                                                  _actionName,
                                                                  _direction,
                                                                  _logMsg);
                
                if (File.Exists(fileName))
                {
                    logStr = "\r\n" + logStr + "\r\n" + gLogPostfixStr_;
                    logFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    logFile.Seek(-8, SeekOrigin.End);
                }
                else
                {
                    logStr = gLogPrefixStr_ + "\r\n" + logStr + "\r\n" + gLogPostfixStr_;
                    logFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                }

                byte[] info = new UTF8Encoding(true).GetBytes(logStr);
                logFile.Write(info, 0, info.Length);
                logFile.Close();
                return true;
                
            }
            finally
            {
                if (logFile != null)
                {
                    logFile.Close();
                }
            }


        }

    }
}
