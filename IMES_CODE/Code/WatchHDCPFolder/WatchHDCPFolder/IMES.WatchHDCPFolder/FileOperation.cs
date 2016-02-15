using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UTL
{

    public class FileOperation
    {


        #region comon read& write file function
        public static void DeleteFileByWildCard(string LocalPatth,
                                                             string SearchPattern)
        {
            FileInfo[] fileInfo = new DirectoryInfo(LocalPatth).GetFiles(SearchPattern);
            foreach (FileInfo f in fileInfo)
            {
                f.Delete();
            }
        }

        public static FileInfo[] GetFilesByWildCard(string LocalPatth,
                                                             string SearchPattern)
        {
            return new DirectoryInfo(LocalPatth).GetFiles(SearchPattern);
           
        }
        public static void FileDelete(string LocalPath,
                                                      string LocalFile)
        {
            try
            {
                string deleFail = LocalPath + LocalFile;

                File.Delete(deleFail);
            }

            catch (Exception e)
            {
                Exception e1 = new Exception(e.StackTrace + " " + e.Message);
                throw e1;
            }
        }
        public static void CreateDirectory(string LocalPath)
        {
            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }

        }
        public static void DirectoryDelete(string LocalPath,
                                                              bool recursive)
        {
            try
            {

                Directory.Delete(LocalPath, recursive);

            }
            catch (Exception e)
            {
                Exception e1 = new Exception(e.StackTrace + " " + e.Message);
                throw e1;
            }
        }

        public static void FileCopy(string LocalBckPath,
                                                   string BckFile,
                                                   string LocalPath,
                                                   string LocalFile)
        {
            try
            {

                CreateDirectory(LocalBckPath);
                CreateDirectory(LocalPath);
                string CopyFile = LocalPath + LocalFile;
                string ToFile = LocalBckPath + BckFile;

                File.Copy(CopyFile, ToFile, true);
            }
            catch (Exception e)
            {
                Exception e1 = new Exception(e.StackTrace + " " + e.Message);
                throw e1;
            }
        }

        public static void FileMove(string srcFileName,
                                                     string destFileName,
                                                     bool isOverwrite)
        {
            try
            {
                if (isOverwrite) File.Delete(destFileName);
                File.Move(srcFileName, destFileName);
            }
            catch
            {
                throw;
            }
        }
        //public static string FileMove(string srcFilePath,
        //                                            string destFilePath,
        //                                            string searchPattern,
        //                                            bool isOverwrite,
        //                                            out string moveFiles)
        //{

        //    try
        //    {
        //        foreach (string FilePath in Directory.GetFiles(srcFilePath, searchPattern))
        //        {
        //            string FileName = Path.GetFileName(FilePath);
        //            string desFilePath = destFilePath + FileName;
        //            if (isOverwrite) File.Delete(desFilePath);
        //            File.Move(FilePath, desFilePath);
        //            moveFiles = moveFiles + ";" + desFilePath;
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return (ex.StackTrace + " " + ex.Message);
        //    }
        //}
        public static void FileMove(string srcFilePath,
                                                   string destFilePath,
                                                   string searchPattern,
                                                   bool isOverwrite,
                                                  out string moveFiles)
        {
            //string moveFiles = "";
            moveFiles = "";
            try
            {
                foreach (string FilePath in Directory.GetFiles(srcFilePath, searchPattern))
                {
                    string FileName = Path.GetFileName(FilePath);
                    string desFilePath = destFilePath + FileName;
                    if (isOverwrite) File.Delete(desFilePath);
                    File.Move(FilePath, desFilePath);
                    moveFiles = moveFiles + ";" + desFilePath;
                }

            }
            catch
            {
                throw;
                //return (ex.StackTrace + " " + ex.Message);
            }
        }

        public static void FileMove(string srcFilePath,
                                                string destFilePath,
                                                string searchPattern,
                                                bool isOverwrite
                                              )
        {
            string moveFiles = "";
            try
            {
                foreach (string FilePath in Directory.GetFiles(srcFilePath, searchPattern))
                {
                    string FileName = Path.GetFileName(FilePath);
                    string desFilePath = destFilePath + FileName;
                    if (isOverwrite) File.Delete(desFilePath);
                    File.Move(FilePath, desFilePath);
                    moveFiles = moveFiles + ";" + desFilePath;
                }
                // return "";
            }
            catch //(Exception ex)
            {
                //return (ex.StackTrace + " " + ex.Message);
                throw;
            }
        }

        public static string CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            string ret = "";
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        ret = CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting);
                        if (ret != "")
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.StackTrace + " " + ex.Message;
            }
            return ret;
        }

        static public void CreateEmptyFile(string fileName)
        {
            File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite).Close();
        }

        static public FileStream CreateFile(string fileName)
        {
            return File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        static public FileStream CreateAppendFile(string fileName)
        {
            return File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        }

        static public void write(FileStream fs, string txt, bool isNewLine)
        {
            if (isNewLine)
                txt = txt + "\r\n";
            byte[] info = new UTF8Encoding(true).GetBytes(txt);
            fs.Write(info, 0, info.Length);
        }

        static public void writeBinaryFromBase64String(FileStream fs, string txt)
        {

            BinaryWriter bw = new BinaryWriter(fs);
            byte[] Bytes = System.Convert.FromBase64String(txt);
            bw.Write(Bytes);
        }

        static public string[] ReadAllLines(string path, string fileName)
        {
            string pathName = (path.EndsWith("\\") ? path : path + "\\") + fileName;

            return File.ReadAllLines(pathName);

        }
        #endregion




    }
}
