using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ArchiveDB
{  
     public enum LogType{Info,Error,Warning,Cost,Event}
        
    class Log
    {
        private string _path;
        private LogType _logType;
        public Log(string path, LogType logType)
        {
            _path = path;
            _logType = logType;
            string FilePath = _path + _logType + ".log";
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
        public void WriteLog(string message)
        {
            string FilePath = _path + _logType + ".log";
            StreamWriter sw;

            if (!File.Exists(FilePath))
            {
                sw = File.CreateText(FilePath);
            }
            else
            {
                sw = File.AppendText(FilePath);
            }
            message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + message;
            // sw = File.CreateText(FilePath);
            sw.WriteLine(message);
            sw.Close();

        }

     
    }
}
