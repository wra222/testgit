using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace UTL
{
    public enum enumCDSIResult
    {
        Ack = 0,
        Err = -1,
        NotFound = 1
    }

    public class CDSIFile
    {
        #region write CDSI command file
        static public void LinkPO_Cmd1(string path,   // need string trail include "\"
                                                             string custSN,
                                                             string order,
                                                             string csdiPath)
        {
            string fileName = custSN + ".OMS";
            string filePath = path + fileName;

            FileOperation.CreateDirectory(path);
            FileStream fs = FileOperation.CreateFile(filePath);
            string cmdStr = "ORD \"" + order + "\"";
            FileOperation.write(fs, cmdStr, true);
            fs.Close();

            FileOperation.CreateDirectory(csdiPath);
            File.Copy(filePath, csdiPath + fileName,true);
            //FileOperation.FileMove(filePath, csdiPath + fileName, true);

        }

        static public void SignUp_Cmd2(string path,   // need string trail include "\"
                                                             string custSN,
                                                             string MAC,
                                                             string csdiPath)
        {
            string fileName = custSN + "-" + MAC + ".QRY";
            string filePath = path + fileName;
            FileOperation.CreateDirectory(path);
            FileStream fs = FileOperation.CreateFile(filePath);
            FileOperation.write(fs, "[HEADER]", true);
            FileOperation.write(fs, "RQ=CDSI_OMSUPDATE", true);

            FileOperation.write(fs, "PAR=", true);
            fs.Close();

            FileOperation.CreateDirectory(csdiPath);
            File.Copy(filePath, csdiPath + fileName,true);
            //FileOperation.FileMove(filePath, csdiPath + fileName, true);
        }

        static public void OMSUPDATE_Cmd3(string path,   // need string trail include "\"
                                                                        string custSN,
                                                                        string MAC,
                                                                        string ATSNAV,
                                                                        string MN1,
                                                                        string SYSID,
                                                                        string csdiPath)
        {
            string fileName = custSN + "-" + MAC + ".QRY";
            string filePath = path + fileName;

            FileOperation.CreateDirectory(path);
            FileStream fs = FileOperation.CreateFile(filePath);
            FileOperation.write(fs, "[HEADER]", true);
            FileOperation.write(fs, "RQ=CDSI_OMSUPDATE", true);
            // ”<SERIAL> <DID> <ProductName> <SYSID> <BIOSVer> <BIOSDate> <MACAddress> <UUID> <OMS>”
            string cmd = "\"" + custSN + "\""; //SERIAL
            cmd = cmd + " \"" + ATSNAV + "\"";//DID
            cmd = cmd + " \"" + MN1 + "\"";//ProductName
            cmd = cmd + " \"" + SYSID + "\""; //SYSID
            cmd = cmd + " \"\"";//BIOSVer
            cmd = cmd + " \"\"";//BIOSDate
            cmd = cmd + " \"" + MAC.Substring(0, 2) + ":" +
                                             MAC.Substring(2, 2) + ":" +
                                              MAC.Substring(4, 2) + ":" +
                                              MAC.Substring(6, 2) + ":" +
                                              MAC.Substring(8, 2) + ":" +
                                               MAC.Substring(10, 2) + "\""; //MACAddress
            cmd = cmd + " \"\"";  //UUID
            cmd = cmd + " \"\""; //OMS

            FileOperation.write(fs, "PAR=\"" + cmd + "\"", true);
            fs.Close();

            FileOperation.CreateDirectory(csdiPath);
            //FileOperation.FileMove(filePath, csdiPath + fileName, true);
            File.Copy(filePath, csdiPath + fileName, true);
        }

        static public void RETRIEVE_Cmd4(string path,   // need string trail include "\"
                                                             string custSN,
                                                             string MAC,
                                                                        string csdiPath)
        {
            string fileName = custSN + "-" + MAC + ".QRY";
            string filePath = path + fileName;
            FileOperation.CreateDirectory(path);
            FileStream fs = FileOperation.CreateFile(filePath);
            FileOperation.write(fs, "[HEADER]", true);
            FileOperation.write(fs, "RQ=CDSI_RETRIEVE", true);

            FileOperation.write(fs, "PAR=" + custSN, true);
            fs.Close();

            FileOperation.CreateDirectory(csdiPath);
            //FileOperation.FileMove(filePath, csdiPath + fileName, true);
            File.Copy(filePath, csdiPath + fileName, true);
        }

        static public void SIGNOFF_Cmd5(string path,   // need string trail include "\"
                                                                 string custSN,
                                                                  string MAC,
                                                                  string csdiPath)
        {
            string fileName = custSN + "-" + MAC + ".QRY";
            string filePath = path + fileName;

            FileOperation.CreateDirectory(path);
            FileStream fs = FileOperation.CreateFile(filePath);
            FileOperation.write(fs, "[HEADER]", true);
            FileOperation.write(fs, "RQ=SIGNUP", true);

            FileOperation.write(fs, "PAR=", true);
            fs.Close();

            FileOperation.CreateDirectory(csdiPath);
            //FileOperation.FileMove(filePath, csdiPath + fileName, true);
            File.Copy(filePath, csdiPath + fileName, true);
        }

        static public string GetCDSIResult(string cdsiPath,
                                                                 string custSN,
                                                                 string MAC)
        {
            string searchPattern = custSN + "-" + MAC + ".*";

            foreach (string FilePath in Directory.GetFiles(cdsiPath, searchPattern))
            {
                return Path.GetExtension(FilePath);
            }
            return null;
        }

        static public void CopyCDSIResultFolder(string cdsiPath,
                                                                           string destPath,
                                                                           string custSN,
                                                                            string MAC)
        {
            string srcPath = cdsiPath + custSN + "-" + MAC + "\\";
            destPath = destPath + custSN + "-" + MAC + "\\";
            FileOperation.CreateDirectory(destPath);
            FileOperation.CopyDirectory(srcPath, destPath, true);
          Console.WriteLine( "CopyCDSIResultFolder  srcPath =" + srcPath +  " destPath=" +destPath);
                                        
            //File.Copy(srcPath, destPath, true);

        }

        static public enumCDSIResult CheckCDSIResult(string localPath,
                                                                                        string cdsiPath,
                                                                                         string custSN,
                                                                                         string MAC)
        {
            string AckName = custSN + "-" + MAC + ".ACK";
            string ErrName = custSN + "-" + MAC + ".ERR";
            string srcAckPath = cdsiPath + AckName;
            string srcErrPath = cdsiPath + ErrName;
            //FileOperation.CopyDirectory(srcPath, destPath, true);
            //File.Copy(srcErrPath, @"F:\\Fail.txt", true);
            //File.Copy(srcAckPath, @"F:\\test.txt", true);
         
            Console.WriteLine("CheckCDSIResult  srcAckPath =" + srcAckPath + " srcErrPath=" + srcErrPath);

            if (File.Exists(srcAckPath.Trim()))
            {

                File.Copy(srcAckPath, localPath + AckName, true);
                Console.WriteLine("CheckCDSIResult  find " + srcAckPath  + " Ack file!!");
                 
                return enumCDSIResult.Ack;
            }
            else if (File.Exists(srcErrPath.Trim()))
            {
                File.Copy(srcErrPath, localPath + ErrName, true);
                Console.WriteLine("CheckCDSIResult  find " + srcErrPath + " Err file !!");
                return enumCDSIResult.Err;
            }
            else
            {
                return enumCDSIResult.NotFound;
            }
        }

        static public enumCDSIResult CheckCDSIResultByOMS(string resultPath,
                                                                                                     string custSN,
                                                                                                     string MAC,
                                                                                                     string ATSNAV)
        {
            string AckName = custSN + "-" + MAC + "\\" + ATSNAV + ".OMS";

            string srcAckPath = resultPath + AckName;
            if (File.Exists(srcAckPath))
            {
                string[]  data=File.ReadAllLines(srcAckPath);
                foreach(string item in data)
                {
                    if (item.Trim().Contains("Status") && item.Trim().Contains("OK"))
                    {
                        return enumCDSIResult.Ack;
                    } 
                }

                return enumCDSIResult.Err;
              
            }
            else
            {
                return enumCDSIResult.NotFound;
            }
        }

        #endregion
    }
}
