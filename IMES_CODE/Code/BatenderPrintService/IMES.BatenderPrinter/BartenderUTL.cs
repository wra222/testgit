using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using Seagull.BarTender.Print;
using com.inventec.template.structure;
using Seagull.BarTender.Print.Database;
using Seagull.BarTender.PrintServer.Tasks;
using Seagull.BarTender.PrintServer;
using System.Configuration;
using Microsoft.Win32;
using System.Threading;

namespace IMES.BartenderPrinter
{
    public class NameValue
    {
        public string Name{get;set;}
        public string Value{get;set;}  
        public string DataType { get; set; } //1:Named, 2:TextFile 3:ODBC, 4:OLEDB
    }

    public sealed class BartenderUTL
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string  MissLableName ="Input name is missing name:{0}  dataType:{1} in  Batender file {2} !";
        private static string PrinterName = "DefaultPrinterName";
        private static string UsingTaskManger = "UsingTaskManger";
        private static string IsPreviewBitmap = "IsPreviewBitmap";

        private static object synPrinter = new object();

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        public static List<string> Producer(int dpi,
                                                          string btwFileDirectory, string btwFileName,
                                                          string apTempFolder, IList<NameValue> nameValueList)
        {
            string methodName = "Producer";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<string> imgeInfoList = new List<string>();
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                string id = threadId.ToString("D6");
                logger.DebugFormat("ThreadId {0} DPI:{1}  Bartender file Name:{2} start BtEngine", id, dpi.ToString(), btwFileName);
                string threadFolder = apTempFolder.EndsWith("\\") ? apTempFolder + id + "\\" : apTempFolder + "\\" + id + "\\";
                if (!Directory.Exists(threadFolder))
                {
                    Directory.CreateDirectory(threadFolder);
                }
                else  // 清空檔案in Thread Id Folder
                {
                    string[] fileNameList = Directory.GetFiles(threadFolder);
                    foreach (string s in fileNameList)
                    {
                        File.Delete(s);
                    }
                }

                string btwTempFile = threadFolder + btwFileName; //threadId.ToString() + ".btw";
                string btwSrcFile = (btwFileDirectory.EndsWith("\\") ? btwFileDirectory : btwFileDirectory + "\\") + btwFileName;
                if (!File.Exists(btwSrcFile))
                {
                    logger.DebugFormat("BtLabel has no Bartender  file Name {0}", btwSrcFile);
                    throw new Exception("No btw file : " + btwSrcFile);
                }

                logger.DebugFormat("BtLabel Bartender file Name {0}", btwSrcFile);

                File.Copy(btwSrcFile, btwTempFile, true);
               
                #region case datafile, check and write dataFile or not  (DataType ='2')
              
                var textFileValueList = nameValueList.Where(x => x.DataType=="2").ToList();
                if (textFileValueList.Count>0)
                {
                    foreach (NameValue data in textFileValueList)
                    {
                        string dataFileName = threadFolder + data.Name + ".txt";
                        System.IO.File.WriteAllText(dataFileName, data.Value);

                        logger.DebugFormat("BtLabel has Text file {0}", dataFileName);
                    }
                    
                }
                #endregion

                #region delete  files in bmp folder
                string bmpFolder = threadFolder + "bmp\\";
                if (Directory.Exists(bmpFolder))
                {
                    string[] fileNameList = Directory.GetFiles(bmpFolder);
                    foreach (string s in fileNameList)
                    {
                        File.Delete(s);
                    }
                }
                else
                {
                    Directory.CreateDirectory(bmpFolder);
                }
                #endregion

                PoolItem item = new PoolItem
                {
                    ThreadFolder = threadFolder,
                    BtwSrcFileName = btwFileName,
                    BtwFile = btwTempFile,
                    TextFileValueList = textFileValueList,
                    ODBCValueList = nameValueList.Where(x=>x.DataType=="3").ToList(),
                    OLEDBValueList = nameValueList.Where(x => x.DataType == "4").ToList(),
                    NameValueList =nameValueList.Where(x=>x.DataType=="1").ToList() ,
                    ThreadId =threadId,
                    DPI = dpi,                       
                    BmpFolder = bmpFolder,
                    HasError=false,
                    ErrorText=string.Empty,
                    producerEvent = new System.Threading.AutoResetEvent(false)
                };
                string usingTaskManger = ConfigurationManager.AppSettings[UsingTaskManger] ?? "N";
                if (usingTaskManger == "N")
                {                   
                    BartenderPool.addItem(item);
                    BartenderPool.SendWorkerEvent();
                    logger.DebugFormat("Producer Thread {0} waiting for BtEngine doing ...", threadId.ToString());
                    bool hasTimeout= item.producerEvent.WaitOne(BartenderPool.Timeout);
                    if (!hasTimeout)
                    {
                        item.hasTimeout = true;
                        item.HasError = true;
                        item.ErrorText = string.Format("Producer thread id {0} BtEngine print timeout {1}", threadId.ToString(), BartenderPool.Timeout.ToString());
                    }
                    else
                    {
                        logger.DebugFormat("Producer thread id {0} BtEngine print done", threadId.ToString());
                    }
                }
                else
                {

                    GenBitmapFile(BtTaskManager.GetTaskQueue(), item);
                }

                if (item.HasError || item.hasTimeout)
                {
                    throw new Exception(item.ErrorText);
                }
                else
                {
                    string[] bmpfiles = Directory.GetFiles(bmpFolder, "*.bmp");
                    foreach (string fileName in bmpfiles)
                    {
                        logger.DebugFormat("read bmp file in {0} ", fileName);
                        byte[] bytes = System.IO.File.ReadAllBytes(fileName);
                        string bmpText = ByteArrayToString(bytes);
                        imgeInfoList.Add(bmpText);
                       // imgeInfoList.Add(bytes);
                    }
                }
                return imgeInfoList;             
                
            }
            catch (Exception e1)
            {
                logger.Error(e1.Message, e1);
                throw;
            }
            finally
            {
               logger.DebugFormat("END: {0}()", methodName);   
            }
        }

        #region disable code
        //public static void GenBitmapFile(LabelFormatDocument btFormat, PoolItem item)
        //{
        //    string methodName = "GenBitmapFile";
        //    logger.DebugFormat("BEGIN: {0}()", methodName);            
               
        //    try
        //    {
        //        setAndCheckLabelContent(btFormat, item);
                
        //        #region generate Label Bitmap File
        //        string bmpFileName = "%PageNumber%.bmp";
        //        Resolution res = new Resolution(item.DPI);
        //        System.Drawing.Color bkcolor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

        //        Seagull.BarTender.Print.Messages message = null;
        //        Result r = btFormat.ExportPrintPreviewToFile(item.BmpFolder,
        //                                    bmpFileName, ImageType.BMP, ColorDepth.Mono
        //                                      , res, bkcolor, OverwriteOptions.Overwrite,
        //                                     false, false, out message);
        //        if (r != Result.Success)
        //        {
        //            item.HasError = true;
        //            throw new Exception("Generate label bitmap file " + r.ToString());
        //        }
        //        else
        //        {
        //            logger.DebugFormat("Generate bitmap file :{0} " , btFormat.LatestSaveNumber.ToString());
        //        }
        //        #endregion
        //    }
        //    catch (Exception e1)
        //    {
        //        item.HasError = true;
        //        item.ErrorText= e1.Message;
        //        logger.Error(e1.Message, e1);                
        //    }
        //    finally
        //    {
        //        btFormat.Close(SaveOptions.DoNotSaveChanges);
        //    }          
        //}
        #endregion

        public static void GenBitmapFile(Engine btEngine, PoolItem item)
        {
            string methodName = "GenBitmapFile";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {    
                Seagull.BarTender.Print.Messages message = null;
                string printerName = ConfigurationManager.AppSettings[PrinterName] ?? "";
                logger.DebugFormat("set Default Printer Name : {0}", printerName); 
                LabelFormatDocument btFormat = btEngine.Documents.Open(item.BtwFile, printerName);
                try
                {
                    //Setup Label content
                    setAndCheckLabelContent(btFormat, item);

                    string isPreviewBitmap = ConfigurationManager.AppSettings[IsPreviewBitmap] ?? "Y";
                    if (isPreviewBitmap == "Y")
                    {
                        preview2BitmapFile(btFormat, item);
                    }
                    else
                    {
                        print2BitmapFile(btFormat, item);
                    }

                    #region generate Label Bitmap File (disable code)
                    //string bmpFileName = "%PageNumber%.bmp";
                    //Resolution res = new Resolution(item.DPI);
                    //System.Drawing.Color bkcolor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

                    
                    //Result result = btFormat.ExportPrintPreviewToFile(item.BmpFolder,
                    //                            bmpFileName, ImageType.BMP, ColorDepth.Mono
                    //                              , res, bkcolor, OverwriteOptions.Overwrite,
                    //                             false, false, out message);
                    //if (result != Result.Success)
                    //{
                    //    item.HasError = true;
                    //    string str = "";
                    //    foreach (Seagull.BarTender.Print.Message m in message)
                    //    {
                    //        str += string.Format("{0} Message: {1}\r\n", m.Severity, m.Text);
                    //    }
                    //    item.ErrorText = string.Format("Thread Id: {0} generate label {1} bitmap file {2}  ErrorText: {3}", item.ThreadId.ToString(), item.BtwFile, result.ToString(), str);
                    //    throw new Exception(item.ErrorText);
                    //}
                    //else
                    //{
                       
                    //    logger.DebugFormat("Thread Id: {0} generate label {1} bitmap file ok! ", item.ThreadId.ToString(), item.BtwFile);
                    //}
                    #endregion                    

                }
                catch (Exception e1)
                {
                    item.HasError = true;
                    item.ErrorText = e1.Message;
                    logger.Error(e1.Message, e1);
                    //throw;
                }
                finally
                {
                    btFormat.Close(SaveOptions.DoNotSaveChanges);
                }

            }
            catch (Exception e)
            {
                item.HasError = true;
                item.ErrorText = e.Message;
                logger.Error(e.Message, e);
                //throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static void GenBitmapFile(TaskQueue queue,PoolItem item )
        {
            string methodName = "GenBitmapFile";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                 LabelFormat btFormat = null;
                 // Create a new task to retrieve format information.
                 GetLabelFormatTask task = new GetLabelFormatTask(item.BtwFile);                
                 logger.DebugFormat("GetLabelFormatTask starting ..."); 
                 TaskStatus status = queue.QueueTaskAndWait(task, BtTaskManager.Timeout);                
                 if (status == TaskStatus.Success)
                 {
                     // Get the LabelFormat object from the task // (this is cached format information; not an open document).
                     btFormat = task.LabelFormat;                     
                     logger.DebugFormat("GetLabelFormatTask completed"); 
                 }
                 else
                 {
                     string str = "";
                     foreach (Seagull.BarTender.Print.Message m in task.Messages)
                     {
                         str += string.Format("{0} Message: {1}\r\n", m.Severity, m.Text);
                     }

                     item.HasError = true;
                     item.ErrorText = "Generate labelFormat file " + status.ToString() + " ErrorText:" + str;
                     throw new Exception(item.ErrorText);
                 }

                 string printerName = ConfigurationManager.AppSettings[PrinterName] ?? "";
                 logger.DebugFormat("set Default Printer Name : {0}", printerName); 
                 btFormat.PrintSetup.PrinterName = printerName;

                 //setup label content
                 setAndCheckLabelContent(btFormat, item);
                 string isPreviewBitmap = ConfigurationManager.AppSettings[IsPreviewBitmap] ?? "Y";
                 if (isPreviewBitmap == "Y")
                 {
                     preview2BitmapFile(btFormat, queue, item);
                 }
                 else
                 {
                     print2BitmapFile(btFormat, queue, item);
                 }

                #region generate Label Bitmap File(disbale code)
                //string bmpFileName = "%PageNumber%.bmp";
                //Resolution res = new Resolution(item.DPI);
                              
                //System.Drawing.Color bkcolor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

                //ExportPrintPreviewToFileTask bitmapTask = new ExportPrintPreviewToFileTask(btFormat, item.BmpFolder, bmpFileName);
                
                //bitmapTask.Directory = item.BmpFolder; 
                //bitmapTask.FileNameTemplate = bmpFileName;
                //bitmapTask.BackgroundColor = bkcolor;
                //bitmapTask.Colors = ColorDepth.Mono;
                //bitmapTask.ImageType = ImageType.BMP;
                //bitmapTask.IncludeBorder = false;
                //bitmapTask.IncludeMargins = false;
                //bitmapTask.OverwriteOptions = OverwriteOptions.Overwrite;
                //bitmapTask.Resolution = res;
                //logger.DebugFormat("ExportPrintPreviewToFileTask bitmap file starting ..."); 
                //TaskStatus taskStatus = queue.QueueTaskAndWait(bitmapTask, BtTaskManager.Timeout);
               
                //if (taskStatus != TaskStatus.Success)
                //{
                //    string str = "";
                //    foreach (Seagull.BarTender.Print.Message m in bitmapTask.Messages)
                //    {
                //        str += string.Format("{0} Message: {1}\r\n", m.Severity, m.Text);
                //    }
                  
                //    item.HasError = true;
                //    item.ErrorText = string.Format("Thread Id: {0} generate label {1} bitmap file {2}  ErrorText: {3}", item.ThreadId.ToString(), item.BtwFile, taskStatus.ToString(), str);                   
                //    throw new Exception(item.ErrorText);
                //}
                //else
                //{
                //    logger.DebugFormat("Thread Id: {0} generate label {1} bitmap files count :{1} ok! ", item.ThreadId.ToString(), item.BtwFile,bitmapTask.NumberOfPreviews.ToString());                    
                //}
                #endregion  
              
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                item.HasError = true;
                item.ErrorText = e.Message;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
        }

        private static void setAndCheckLabelContent(LabelFormat btFormat, PoolItem item)
        {

             #region 產生LabelFormat 打印參數數據

                DatabaseConnections bcList = btFormat.DatabaseConnections;
                #region Check and Set Database connection
                if (bcList.Count > 0)
                {

                    #region Check and Set TextFile Name
                    var textFileList = bcList.Where(x => x.Type == InputDataFile.TextFile).ToList();
                    if (textFileList.Count > 0)
                    {
                        var nameList = textFileList.Select(x => x.Name).ToList();
                        if (item.TextFileValueList.Count == 0)
                        {
                            item.HasError = true;
                            string errorMsg = string.Format(MissLableName, string.Join(",", nameList.ToArray()), "TextFile", item.BtwSrcFileName);
                            throw new Exception(errorMsg);
                        }
                        else if (textFileList.Count == 1) // 單筆
                        {
                            var textFileName = item.TextFileValueList.Where(x => x.Name == textFileList[0].Name).FirstOrDefault();
                            if (textFileName == null)
                            {
                                textFileName = item.TextFileValueList[0];
                            }
                            TextFile file = (TextFile)textFileList[0];
                            file.FileName = item.ThreadFolder + textFileName.Name + ".txt";                       
                        }
                        else
                        {
                            var textFileName = item.TextFileValueList.Select(x => x.Name).ToList();
                            var missingNameList = nameList.Except(textFileName).ToList();
                            if (missingNameList.Count > 0)
                            {
                                item.HasError = true;
                                string errorMsg = string.Format(MissLableName, string.Join(",", missingNameList.ToArray()), "TextFile", item.BtwSrcFileName);
                                throw new Exception(errorMsg);
                            }
                            foreach (TextFile file in textFileList)
                            {
                                file.FileName = item.TextFileValueList
                                                        .Where(x => x.Name == file.Name)
                                                        .Select(y => item.ThreadFolder + y.Name + ".txt")
                                                        .FirstOrDefault();
                            }
                        }
                    }
                    #endregion

                    #region check and set SQLStatement in DB
                    var OLEDBList = bcList.Where(x => x.Type == InputDataFile.OLEDB).ToList();
                    if (OLEDBList.Count > 0)
                    {
                        if (item.OLEDBValueList.Count > 0)
                        {
                            foreach (OLEDB db in OLEDBList)
                            {
                                string sentence = item.OLEDBValueList
                                                                    .Where(x => x.Name == db.Name)
                                                                    .Select(y => y.Value).FirstOrDefault();
                                if (!string.IsNullOrEmpty(sentence))
                                {
                                    db.SQLStatement = sentence;
                                }
                            }
                        }
                    }

                    var ODBCList = bcList.Where(x => x.Type == InputDataFile.OLEDB).ToList();
                    if (ODBCList.Count > 0)
                    {
                        if (item.ODBCValueList.Count > 0)
                        {
                            foreach (ODBC db in ODBCList)
                            {
                                string sentence = item.ODBCValueList
                                                                    .Where(x => x.Name == db.Name)
                                                                    .Select(y => y.Value).FirstOrDefault();
                                if (!string.IsNullOrEmpty(sentence))
                                {
                                    db.SQLStatement = sentence;
                                }
                            }
                        }
                    }
                    #endregion

                }
                #endregion

                SubStrings substringList = btFormat.SubStrings;
                #region Check and Set Named DataSource Name/Value
                if (substringList.Count > 0)
                {
                    var lbNameList = substringList.Select(x => x.Name).ToList();
                    var dataValueList = item.NameValueList;
                    if (dataValueList.Count > 0)
                    {
                        var dataNameList = dataValueList.Select(x => x.Name).ToList();

                        var missingNameList = lbNameList.Except(dataNameList).ToList();
                        if (missingNameList.Count > 0)
                        {
                            item.HasError = true;
                            string errorMsg = string.Format(MissLableName, string.Join(",", missingNameList.ToArray()), "NamedDataSource", item.BtwSrcFileName);
                            throw new Exception(errorMsg);
                        }
                        //Set Name & Value in label template
                        foreach (SubString substring in substringList)
                        {
                            substring.Value = dataValueList.Where(x => x.Name == substring.Name).Select(y => y.Value).FirstOrDefault() ?? "";
                        }
                    }
                    else
                    {
                        item.HasError = true;
                        string errorMsg = string.Format(MissLableName, string.Join(",", lbNameList.ToArray()), "NamedDataSource", item.BtwSrcFileName);
                        throw new Exception(errorMsg);
                    }
                }
                #endregion
            #endregion

            //btFormat.PrintSetup.PrinterName = "";
            btFormat.PrintSetup.EnablePrompting = false;          
           
        }

        private static void preview2BitmapFile(LabelFormat btFormat, TaskQueue queue, PoolItem item)
        {
            string methodName = "preview2BitmapFile";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string bmpFileName = "%PageNumber%.bmp";
                Resolution res = new Resolution(item.DPI);

                System.Drawing.Color bkcolor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

                ExportPrintPreviewToFileTask bitmapTask = new ExportPrintPreviewToFileTask(btFormat, item.BmpFolder, bmpFileName);

                bitmapTask.Directory = item.BmpFolder;
                bitmapTask.FileNameTemplate = bmpFileName;
                bitmapTask.BackgroundColor = bkcolor;
                bitmapTask.Colors = ColorDepth.Mono;
                bitmapTask.ImageType = ImageType.BMP;
                bitmapTask.IncludeBorder = false;
                bitmapTask.IncludeMargins = false;
                bitmapTask.OverwriteOptions = OverwriteOptions.Overwrite;
                bitmapTask.Resolution = res;
                logger.DebugFormat("ExportPrintPreviewToFileTask bitmap file starting ...");
                TaskStatus taskStatus = queue.QueueTaskAndWait(bitmapTask, BtTaskManager.Timeout);

                if (taskStatus != TaskStatus.Success)
                {
                    string str = "";
                    foreach (Seagull.BarTender.Print.Message m in bitmapTask.Messages)
                    {
                        str += string.Format("{0} Message: {1}\r\n", m.Severity, m.Text);
                    }

                    item.HasError = true;
                    item.ErrorText = string.Format("Thread Id: {0} generate label {1} bitmap file {2}  ErrorText: {3}", item.ThreadId.ToString(), item.BtwFile, taskStatus.ToString(), str);
                    throw new Exception(item.ErrorText);
                }
                else
                {
                    logger.DebugFormat("Thread Id: {0} generate label {1} bitmap files count :{1} ok! ", item.ThreadId.ToString(), item.BtwFile, bitmapTask.NumberOfPreviews.ToString());
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private static void preview2BitmapFile(LabelFormatDocument btFormat, PoolItem item)
        {
            string methodName = "preview2BitmapFile";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                Seagull.BarTender.Print.Messages message = null;
                string bmpFileName = "%PageNumber%.bmp";
                Resolution res = new Resolution(item.DPI);
                System.Drawing.Color bkcolor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");


                Result result = btFormat.ExportPrintPreviewToFile(item.BmpFolder,
                                            bmpFileName, ImageType.BMP, ColorDepth.Mono
                                              , res, bkcolor, OverwriteOptions.Overwrite,
                                             false, false, out message);
                if (result != Result.Success)
                {
                    item.HasError = true;
                    string str = "";
                    foreach (Seagull.BarTender.Print.Message m in message)
                    {
                        str += string.Format("{0} Message: {1}\r\n", m.Severity, m.Text);
                    }
                    item.ErrorText = string.Format("Thread Id: {0} generate label {1} bitmap file {2}  ErrorText: {3}", item.ThreadId.ToString(), item.BtwFile, result.ToString(), str);
                    throw new Exception(item.ErrorText);
                }
                else
                {

                    logger.DebugFormat("Thread Id: {0} generate label {1} bitmap file ok! ", item.ThreadId.ToString(), item.BtwFile);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }

        }


        private static void print2BitmapFile(LabelFormatDocument btFormat, PoolItem item)
        {
            string methodName = "print2BitmapFile";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                lock (synPrinter)
                {
                    setupBitmapPrinter(item);
                    btFormat.PrintSetup.EnablePrompting = false;
                    
                    Thread.Sleep(20);
                    Result result = btFormat.Print();
                    if (result == Result.Success)
                    {
                        waitingBitmapFile(item.BmpFolder);
                        logger.DebugFormat("Thread Id: {0} generate label {1} bitmap files count ok! ", item.ThreadId.ToString(), item.ThreadId.ToString("D5"));
                    }
                    else
                    {
                        item.HasError = true;
                        item.ErrorText = string.Format("Thread Id: {0} generate label {1} bitmap file {2}  ErrorText: {3}", item.ThreadId.ToString(), item.BtwFile, item.ThreadId.ToString("D5"), result.ToString());
                        throw new Exception(item.ErrorText);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private static void print2BitmapFile(LabelFormat btFormat, TaskQueue queue, PoolItem item)
        {
            string methodName = "print2BitmapFile";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                
                PrintLabelFormatTask bitmapTask = new PrintLabelFormatTask(btFormat);
                bitmapTask.PrintJobName = item.BtwSrcFileName;
               
                logger.DebugFormat("PrintLabelFormatTask bitmap file starting ...");
                lock (synPrinter)
                {
                    setupBitmapPrinter(item);
                    Thread.Sleep(20);
                    TaskStatus taskStatus = queue.QueueTaskAndWait(bitmapTask, BtTaskManager.Timeout);
                    if (taskStatus != TaskStatus.Success)
                    {
                        string str = "";
                        foreach (Seagull.BarTender.Print.Message m in bitmapTask.Messages)
                        {
                            str += string.Format("{0} Message: {1}\r\n", m.Severity, m.Text);
                        }

                        item.HasError = true;
                        item.ErrorText = string.Format("Thread Id: {0} generate label {1} bitmap file {2}  ErrorText: {3}", item.ThreadId.ToString(), item.BtwFile, taskStatus.ToString(), str);
                        throw new Exception(item.ErrorText);
                    }
                    else
                    {
                        waitingBitmapFile(item.BmpFolder);
                        logger.DebugFormat("Thread Id: {0} generate label {1} bitmap files  ok! ", item.ThreadId.ToString(), item.BtwFile);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private static void setupBitmapPrinter(PoolItem item)
        {
             //setup regist HKEY_CURRENT_USER\Software\PlotSoft\Writer\OutputOption
            //DEFAULT_FOLDER_PATH  REG_SZ Path
           
            RegistryKey pathKey = Registry.CurrentUser.OpenSubKey("Software\\PlotSoft\\Writer\\OutputOption", true);
            pathKey.SetValue("DEFAULT_FOLDER_PATH", item.BmpFolder, RegistryValueKind.String);
            pathKey.SetValue("DEFAULT_FILENAME", item.ThreadId.ToString("D5"), RegistryValueKind.String);
            pathKey.SetValue("LASTPDFORIMAGE", 1, RegistryValueKind.DWord);
            pathKey.SetValue("HIDE_DIALOG", 1, RegistryValueKind.DWord);
            pathKey.SetValue("USE_DEFAULT_FILENAME", 1, RegistryValueKind.DWord);
            pathKey.SetValue("USE_DEFAULT_FOLDER", 1, RegistryValueKind.DWord);
            pathKey.SetValue("USE_PRINT_JOBNAME", 0, RegistryValueKind.DWord);
            

            //HKEY_CURRENT_USER\Software\PlotSoft\Writer\IMAGE
            //DPI DWORD
            RegistryKey dpiKey = Registry.CurrentUser.OpenSubKey("Software\\PlotSoft\\Writer\\IMAGE", true);
            dpiKey.SetValue("DPI", item.DPI, RegistryValueKind.DWord);
            dpiKey.SetValue("Digits", 2, RegistryValueKind.DWord);
            dpiKey.SetValue("IMAGE_TYPE", "BMP", RegistryValueKind.String);
            dpiKey.SetValue("Option_BMP", "BMPMONO",RegistryValueKind.String);
            
        }

        private static void waitingBitmapFile(string path)
        {
            while (Directory.GetFiles(path, "*.bmp").Length <= 0)
            {
                Thread.Sleep(5);
            }
        }
    }
}
