using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using log4net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
namespace AlarmMail
{
    class Program
    {
        private static Config config=null;
        private static string timeStamp = "";
        private static string connStr = "";
        private static string path = "";
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        static void Main(string[] args)
        {
           
            config = new Config();
            timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            path = System.Environment.CurrentDirectory + "\\TmpExcel\\";
            if (!Directory.Exists(path))
            { Directory.CreateDirectory(path); }
            connStr = config.GetConnectionString("DBServer");
            try
            {

                SendMail("GetFAINotApproval");
                SendMail("GetFAINotToUnitWeight");
                DeleteOldFile(path);
            
            }
            catch (Exception e)
            {

                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
         
        }
        static void SendMail(string key)
        {
            string spName = config.GetValue(key+"SpName");
            DataTable dt = SQLHelp.SQLHelper.ExecuteDataFill(connStr, CommandType.StoredProcedure, spName);
            if (dt.Rows.Count == 0)
            { return; }
            string notApprovalFile = path + key+"-" + timeStamp + ".xls";
            MemoryStream ms = ExcelTool.DataTableToExcel(dt, key);
            using (FileStream file = new FileStream(notApprovalFile, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }
            ms.Close();
            ms.Dispose();
            ArrayList arr = GetMailList(key);
            List<string> mailTo =(List<string>) arr[0];
            List<string> mailCC = (List<string>)arr[1];
            string notApprovalMailTitle = config.GetValue(key+"MailTitle");
            string notApprovalMailBody = config.GetValue(key+"MailBody");
            string error = "";
            bool b = MailTool.SendMail(mailTo,mailCC, notApprovalMailTitle, notApprovalMailBody, notApprovalFile, out error);
        
        }

        static ArrayList GetMailList(string key)
        {
            ArrayList arr = new ArrayList();
            string mailCCsql = config.GetValue(key);
            string spName = config.GetValue(key + "MailListSpName");
            DataTable dtC = SQLHelp.SQLHelper.ExecuteDataFill(connStr, CommandType.StoredProcedure, spName);
            List<string> mailTolist = new List<String>();
            List<string> mailCClist = new List<String>();
            if (dtC.Rows.Count > 0)
            {
                mailTolist.AddRange(dtC.Rows[0]["ToEmail"].ToString().Split(',').ToList<string>());
                mailCClist.AddRange(dtC.Rows[0]["CCEmail"].ToString().Split(',').ToList<string>());

            }
            arr.Add(mailTolist.Distinct().ToList());
            arr.Add(mailCClist.Distinct().ToList());
            return arr;
        }

        static void DeleteOldFile(string path)
        {
            try
            {
                int keepCount = 0;
                Config config = new Config();
                string t = config.GetValue("keepExcelAmount");
                int.TryParse(t, out keepCount);
                //keepExcelAmount
                DirectoryInfo d = new DirectoryInfo(path);
                List<FileInfo> lstFi = d.GetFiles().ToList();
                int delTotal = lstFi.Count - keepCount;
                if (delTotal <= 0 || keepCount==0)
                { return; }
                var tmp = lstFi.OrderBy(x => x.CreationTime).Take(delTotal);
                foreach (FileInfo f in tmp)
                {
                   f.Delete();
                }
            }
            catch
            { 
                return;
            }
           
        
        }
    }
}
