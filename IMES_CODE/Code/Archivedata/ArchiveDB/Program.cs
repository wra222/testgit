// The latest updated time at :2011/10/06 10:00 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Transactions;
using System.Data;
using System.Threading;
//using GenDeleteSQL;
using ArchiveInterface;
using System.Reflection;

namespace ArchiveDB
{
    class Program
    {
     
        static Config config;
        static DBAcess dbacess;
        static Log logInfo;
        static Log logError;
        static Mail mailObj;
        static string[] paramArr = { "P", "M","PM","MAIL","FK","?"}; //P:Prepare data ; M:Move data ; PM: P+M ; Mail : Test mail
        static void Main(string[] args)
        {
       

            //TEST
            //DBAcess d = new DBAcess();
            //d.MoveSubData();
      
            //TEST  
         
            //Check the ap is aleardy running...
      
           bool is_createdNew;
           Mutex mu = null;
           string mutexName1 = Process.GetCurrentProcess().MainModule.FileName
               .Replace(Path.DirectorySeparatorChar, '_');
           mu = new Mutex(true, "Global\\" + mutexName1, out is_createdNew);
            if (!is_createdNew)
            {
                Console.WriteLine("Archive is running, can not run again.........");
                Console.WriteLine("Application will exit after 5 seconds.........");
                Thread.Sleep(5000);
                return;
            }
           //Check the ap is aleardy running...

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            sw.Start();//開始計時
            mailObj = new Mail();
            config = new Config();
            DateTime startTime = DateTime.Now;
            string param="";
         // param = "P";
          //IF TEST MARKED THESE

            if (args.Length == 0)
            {
                Console.Write("Wrong parameter!!"); Console.Read(); return;
            }
            else
            { param = args[0].ToUpper().Replace("/", "").Replace("-", ""); }
            if (Array.IndexOf(paramArr, param) == -1)
            { Console.Write("Wrong parameter!!"); Console.Read(); return; }
            //IF TEST MARKED THESE
            if (param.Replace("/", "").Replace("-", "") == "?")
            {
                Console.WriteLine(ShowHelp());
                return;
            }

     
            if (param == "MAIL")
            {
                try
                {
                    Console.WriteLine("Begin test mail function.... ");
                    mailObj.IsTest = true;
                //    string testBody="";
                   // string errMsg = errMsg + "Retry :" + i.ToString() + " ; Error Message :" + exp.Message + "<BR>";
                    mailObj.Send("Archive HB DB Error:" + "[DeleteDataRetry]  Delete data " + "PizzaTEST" + " Error:",
                        "Retry :" + "1" + " <BR>Error Message:<BR>" + "Error Message" + "<BR> Delete SQL :" + "<BR>" + "deleteSql"); 
                    Console.WriteLine("Mail test successfully");
                  
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Mail fail,error :" + exp.Message);
                 }
                finally
                {
                    Console.WriteLine("Application will exit after 5 seconds.........");
                    Thread.Sleep(5000);
                   
                 }
                return;
            }

            bool IsTest = config.GetValue("IsTest") == "Y" ? true : false;
            string logPath = config.GetValue("LogPath");
            string xmlPath = config.GetValue("XMLPath");
            string dbName = config.GetValue("DBName");
            if (IsTest) Console.WriteLine("Test Mode ");
            Console.WriteLine("The delete source data setting is " + config.GetValue("IsDeleteSrcData"));
            Console.WriteLine("Application will start after 10 seconds.........");
            Thread.Sleep(1000); // prompt user the Delete DB  setting, the ap will run after 10 seconds....

            int intCdtDay = int.Parse(config.GetValue("CdtDay"));
            int intDay = int.Parse(config.GetValue("Day"));
            int intInterval = int.Parse(config.GetValue("DeleteTimeStampInterval"));
            bool IsDeleteSrcData = config.GetValue("IsDeleteSrcData") == "Y" ? true : false;
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
            if (!Directory.Exists(xmlPath)) Directory.CreateDirectory(xmlPath);
            logInfo = new Log(logPath, LogType.Info);
            logError = new Log(logPath, LogType.Error); //DBName
            if (config.GetValue("DBName") == "HPEDI")
            {
                if (IsDeleteSrcData && (intDay <15 || intCdtDay < 15)) // If the delete source data setting is Y, day must >90 (不可刪除2個月內data,防止user設定錯誤..)
                {
                    Console.WriteLine("The delete source data setting is " + config.GetValue("IsDeleteSrcData") + ", and the day can not less then 15 ");
                    Console.WriteLine("Press any key to exit!! ");
                    Console.Read();
                    return;
                }
            }
            else
            {
                if (IsDeleteSrcData && (intDay < 7 || intCdtDay < 7)) // If the delete source data setting is Y, day must >90 (不可刪除2個月內data,防止user設定錯誤..)
                {
                    Console.WriteLine("The delete source data setting is " + config.GetValue("IsDeleteSrcData") + ", and the day can not less then 30 ");
                    Console.WriteLine("Press any key to exit!! ");
                    Console.Read();
                    return;
                }
            }
          
            string strStartTime = startTime.ToString("yyyyMMddHHmm");
            logInfo.WriteLog(" *************** Begin to Archive DB *************** ");
      
            try
            {
                dbacess = new DBAcess();
                IArchiveInterface ia = dbacess.GetDBSettingObj();
                if (ia == null)
                {
                    string err = "[Inital DBAcess error]  Error : Build IArchiveInterface object error";
                    Console.WriteLine(err);
                    logError.WriteLog(err);
                    mailObj.Send("Warning!! Archive HP DB fail,Inital DBAcess error ", "Inital DBAcess error " + "<BR>" + err);
                    return;
                }
            }
            catch(Exception exp)
            {
                Console.WriteLine(exp.Message);
                logError.WriteLog("[Inital DBAcess error]  Error :" + exp.Message);
                string title = string.Format("Warning!! Archive {0} DB fail,Inital DBAcess error", dbName);
                mailObj.Send(title, "Inital DBAcess error " + "\r\n" + exp.Message);
              return;
            }
            if (param == "FK")//获取数据库所有表的外键关系 ，并存入 ArchiveAllFKConstraint
            {
                Console.WriteLine("Begin to get all FK table list and insert data to ArchiveAllFKConstraint.....");
                dbacess.IniArchiveAllFKConstraint();
                Console.WriteLine("End get all FK table successfully.....");
                Console.WriteLine("Application will exit after 5 seconds.....");
                Thread.Sleep(5000);
                return;

            }
            Console.WriteLine("Begin to Archive....");

            if (param == "P")
            {
                dbacess.InitiaIDListTable();
               
                Console.WriteLine("Application will exit after 5 seconds.........");
                Thread.Sleep(5000);
                return;
            }
            if (param == "M" || param=="PM")
            {
         //       dbacess.TestBulkCopy();
                try
                {
                    if (param == "PM") 
                    {
                        if (!dbacess.InitiaIDListTable())// insert ArchiveIDList
                        {
                           return;
                        }
              
                    }
                   if (param == "M" && !dbacess.SetNewstTimeStamp(intInterval)) // if param=M, it means the InitiaIDListTable had been excuted before, so need not run it agian
                    {
                        Console.WriteLine("Error :Can not get newst Time Stamp!!");
                        mailObj.Send("Error :Can not get newst Time Stamp!!", "Error :Can not get newst Time Stamp!! Move Data can not run");
                        return;
                    }

                       dbacess.BeforeArchiveTableRowCount();//ArchiveTableLog     ArchiveLog
                        dbacess.MoveOtherData();
                        dbacess.MoveSubData();
                        dbacess.MoveCdtData();
                        dbacess.MoveMainData();
                        Console.WriteLine("Finish to archive..");
                        Console.WriteLine("Application will exit after 5 seconds.........");
                        Thread.Sleep(5000);
                     
                }
                catch (Exception exp)
                {
                    mailObj.Send("Archive HB DB Error:MoveData",exp.Message);
                    Console.WriteLine(exp.Message);
                    dbacess.InsertArchiveLog("MoveData", exp.Message, LogType.Error, null);
                    dbacess.IsHaveError = true;
                 }

            }
            TimeSpan copyTime = DateTime.Now - startTime;
         //   int cost = copyTime.Minutes * 60 + copyTime.Seconds;
            sw.Stop();
            string cost = sw.Elapsed.TotalMinutes.ToString("f2") + "min";
            dbacess.EndArchive(cost);
            return;

        }
        static string ShowHelp()
        {
            string msg = @"/Mail    Test Mail function" + "\r\n" +
                                   "/FK      Get all FK table list and insert data to ArchiveAllFKConstraint" + "\r\n" +
                                   "/P       Prepare move data\r\n" +
                                   "/M       Move data without prepare\r\n" +
                                   "/PM      Prepare and move data";
            return msg;
                                
        }
        static void TestMSDTC(string srcConstring,string destConstring)
        {
            SqlConnection srcCon = new SqlConnection(srcConstring);
            SqlConnection destCon = new SqlConnection(destConstring);
            try
            {
                using (TransactionScope transScope = new TransactionScope())
                {
                    srcCon.Open(); 
                    destCon.Open();
                }
                Console.WriteLine("Test MSTDC successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine("Test MSTDC fail!!");
                Console.WriteLine("Error " +exp.Message);
            }
            finally
            { 
                if (srcCon.State !=ConnectionState.Closed)
                {srcCon.Close();}
                if (destCon.State != ConnectionState.Closed)
                { srcCon.Close(); }


            }
         
        }
    }
}
