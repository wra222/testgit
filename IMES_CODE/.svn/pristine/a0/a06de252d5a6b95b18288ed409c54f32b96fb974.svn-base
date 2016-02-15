using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Principal;
using UTL.SQL;
using System.Threading;
using System.Data.SqlClient;
using UTL.IO;
using log4net;
using UTL.Account;


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
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
        #region write CDSI command file
        static public void LinkPO_Cmd1(string path,   // need string trail include "\"
                                                             string custSN,
                                                             string order,
                                                             string csdiPath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string fileName = custSN + ".OMS";
                string filePath = path + fileName;

                FileOperation.CreateDirectory(path);
                FileStream fs = FileOperation.CreateFile(filePath);
                string cmdStr = "ORD \"" + order + "\"";
                FileOperation.write(fs, cmdStr, true);
                fs.Close();

                FileOperation.CreateDirectory(csdiPath);
                File.Copy(filePath, csdiPath + fileName, true);
                //FileOperation.FileMove(filePath, csdiPath + fileName, true);
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

        static public void SignUp_Cmd2(string path,   // need string trail include "\"
                                                             string custSN,
                                                             string MAC,
                                                             string csdiPath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
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
                File.Copy(filePath, csdiPath + fileName, true);
                //FileOperation.FileMove(filePath, csdiPath + fileName, true);
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

        static public void OMSUPDATE_Cmd3(string path,   // need string trail include "\"
                                                                        string custSN,
                                                                        string MAC,
                                                                        string ATSNAV,
                                                                        string MN1,
                                                                        string SYSID,
                                                                        string csdiPath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
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

        static public void RETRIEVE_Cmd4(string path,   // need string trail include "\"
                                                             string custSN,
                                                             string MAC,
                                                                        string csdiPath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
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

        static public void SIGNOFF_Cmd5(string path,   // need string trail include "\"
                                                                 string custSN,
                                                                  string MAC,
                                                                  string csdiPath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
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

        static public string GetCDSIResult(string cdsiPath,
                                                                 string custSN,
                                                                 string MAC)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string searchPattern = custSN + "-" + MAC + ".*";

                foreach (string FilePath in Directory.GetFiles(cdsiPath, searchPattern))
                {
                    return Path.GetExtension(FilePath);
                }
                return null;
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

        static public void CopyCDSIResultFolder(string cdsiPath,
                                                                           string destPath,
                                                                           string custSN,
                                                                            string MAC)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string srcPath = cdsiPath + custSN + "-" + MAC + "\\";
                destPath = destPath + custSN + "-" + MAC + "\\";
                FileOperation.CreateDirectory(destPath);
                FileOperation.CopyDirectory(srcPath, destPath, true);
                logger.Debug("CopyCDSIResultFolder  srcPath =" + srcPath + " destPath=" + destPath);
                //File.Copy(srcPath, destPath, true);
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

        static public enumCDSIResult CheckCDSIResult(string localPath,
                                                                                        string cdsiPath,
                                                                                         string custSN,
                                                                                         string MAC)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string AckName = custSN + "-" + MAC + ".ACK";
                string ErrName = custSN + "-" + MAC + ".ERR";
                string srcAckPath = cdsiPath + AckName;
                string srcErrPath = cdsiPath + ErrName;
                //FileOperation.CopyDirectory(srcPath, destPath, true);
                //File.Copy(srcErrPath, @"F:\\Fail.txt", true);
                //File.Copy(srcAckPath, @"F:\\test.txt", true);

                logger.Debug("CheckCDSIResult  srcAckPath =" + srcAckPath + " srcErrPath=" + srcErrPath);

                if (File.Exists(srcAckPath.Trim()))
                {

                    File.Copy(srcAckPath, localPath + AckName, true);
                    logger.Debug("CheckCDSIResult  find " + srcAckPath + " Ack file!!");

                    return enumCDSIResult.Ack;
                }
                else if (File.Exists(srcErrPath.Trim()))
                {
                    File.Copy(srcErrPath, localPath + ErrName, true);
                    logger.Debug("CheckCDSIResult  find " + srcErrPath + " Err file !!");
                    return enumCDSIResult.Err;
                }
                else
                {
                    return enumCDSIResult.NotFound;
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

        static public enumCDSIResult CheckCDSIResultByOMS(string resultPath,
                                                                                                     string custSN,
                                                                                                     string MAC,
                                                                                                     string ATSNAV)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string AckName = custSN + "-" + MAC + "\\" + ATSNAV + ".OMS";

                string srcAckPath = resultPath + AckName;
                if (File.Exists(srcAckPath))
                {
                    string[] data = File.ReadAllLines(srcAckPath);
                    foreach (string item in data)
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

        #endregion

        public static  void SendCDSIPOLinkCmd(SqlConnection dbconnect,                                                
                                                  AppConfig config,
                                                   ProductInfo productInfo,
                                                   string po,
                                                  ref string msg)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                WindowsImpersonationContext wicCdsi = null;
                if (!string.IsNullOrEmpty(config.CDSIServerUser))
                    wicCdsi = Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                  config.CDSIDomain,
                                                                                  config.CDSIServerPassword );

                CDSIFile.LinkPO_Cmd1(config.LocalSN2POFolder,
                                                                productInfo.CUSTSN,
                                                                po, config.CDSISN2POFolder);
                logger.Debug("CDSIFile.LinkPO_Cmd1");
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

        public static  void SendCDSICmd(SqlConnection dbconnect,                                                              
                                                                AppConfig config,
                                                                ProductInfo productInfo,
                                                                string po,
                                                                bool isLinkPO,
                                                                ref string msg)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string localmsg = "";
                // write link PO cmd in LocalSN2POFolder
                #region send CDSI Cmd
                WindowsImpersonationContext wicCdsi = null;
                if (isLinkPO)
                {
                    if (!string.IsNullOrEmpty(config.CDSIServerUser))
                        wicCdsi = Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                      config.CDSIDomain,
                                                                                      config.CDSIServerPassword);

                    CDSIFile.LinkPO_Cmd1(config.LocalSN2POFolder,
                                                                    productInfo.CUSTSN,
                                                                    po, config.CDSISN2POFolder);
                    logger.DebugFormat( "CDSIFile.LinkPO_Cmd1");

                }

                CDSIFile.SignUp_Cmd2(config.LocalMAILBOXFolder,
                                                                productInfo.CUSTSN,
                                                                productInfo.MAC,
                                                                config.CDSIMAILBOXFolder);
                logger.DebugFormat("CDSIFile.SignUp_Cmd2");
                //Thread.Sleep(config.CDSICmdInterval);

                CDSIFile.OMSUPDATE_Cmd3(config.LocalMAILBOXFolder,
                                                                             productInfo.CUSTSN,
                                                                             productInfo.MAC,
                                                                             productInfo.ATSNAV,
                                                                             productInfo.MN1,
                                                                             productInfo.SYSID,
                                                                             config.CDSIMAILBOXFolder);
                logger.DebugFormat(" CDSIFile.OMSUPDATE_Cmd3");
                // don't sleep
                Thread.Sleep(config.CDSICmdInterval);
                //Vincent add check result
                enumCDSIResult result1 = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                         config.CDSIMAILBOXFolder,
                                                                                       productInfo.CUSTSN,
                                                                                        productInfo.MAC);
                if (result1 == enumCDSIResult.NotFound)
                {
                    #region retry again and sleep 5 sec
                    System.Threading.Thread.Sleep(config.CDSICmdInterval);
                    result1 = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                  config.CDSIMAILBOXFolder,
                                                                                  productInfo.CUSTSN,
                                                                                  productInfo.MAC);
                    #endregion
                }

                if (result1 == enumCDSIResult.Err || result1 == enumCDSIResult.NotFound)
                {
                    #region write error message
                    //send  error email
                    localmsg = "<br/> OMSUPDATE_Cmd3 CDSIResultFile -- ProductID:" + productInfo.ProductID + "  CUSTSN:" + productInfo.CUSTSN + " PO:" + po + "<br/>" +
                                (result1 == enumCDSIResult.Err ? "Error Result" : "Not Found Result file") +
                               " Please Check CDSI Server, Error Message:";

                    msg = msg + localmsg;

                    if (result1 == enumCDSIResult.Err)
                    {
                        string[] data = File.ReadAllLines(config.LocalMAILBOXFolder + productInfo.CUSTSN + "-" + productInfo.MAC + ".ERR");
                        msg = msg + "<br/>" + string.Join("<br/>", data);
                        localmsg = localmsg + "<br/>" + string.Join("<br/>", data);
                    }

                    logger.DebugFormat(  "OMSUPDATE_CDSIResultFile {0}",
                                   localmsg);

                    #endregion

                    #region SIGNOFF Cmd

                    if (!string.IsNullOrEmpty(config.CDSIServerUser))
                        wicCdsi = Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                      config.CDSIDomain,
                                                                                      config.CDSIServerPassword);

                    CDSIFile.SIGNOFF_Cmd5(config.LocalMAILBOXFolder,
                                                                   productInfo.CUSTSN,
                                                                   productInfo.MAC,
                                                                   config.CDSIMAILBOXFolder);
                    logger.DebugFormat("CDSIFile.SIGNOFF_Cmd5");

                    if (wicCdsi != null)
                    {
                        Logon.Log_off(wicCdsi);

                    }
                    //Disable sleep  Vincent
                    //Thread.Sleep(config.CDSICmdInterval);
                    #endregion
                    return;
                }

                CDSIFile.RETRIEVE_Cmd4(config.LocalMAILBOXFolder,
                                                                productInfo.CUSTSN,
                                                                productInfo.MAC,
                                                                config.CDSIMAILBOXFolder);
                logger.DebugFormat("CDSIFile.RETRIEVE_Cmd4");


                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);

                }

                Thread.Sleep(config.CDSICmdInterval);

                #endregion

                //Get ACK/ERR file
                enumCDSIResult result = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                                          config.CDSIMAILBOXFolder,
                                                                                                        productInfo.CUSTSN,
                                                                                                         productInfo.MAC);
                logger.DebugFormat("CDSIFile.CheckCDSIResult");
                if (result == enumCDSIResult.NotFound)
                {
                    #region retry again and sleep 5 sec
                    Thread.Sleep(config.CDSICmdInterval);
                    result = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                    config.CDSIMAILBOXFolder,
                                                                                       productInfo.CUSTSN,
                                                                                        productInfo.MAC);
                    #endregion
                }

                if (result == enumCDSIResult.Ack)
                {
                    logger.DebugFormat(  "CheckCDSIResult OK");
                    #region  Copy CDSI Result XML file to local Result folder & Read CDSI XML file
                    CDSIFile.CopyCDSIResultFolder(config.CDSIResultFolder,
                                                                                      config.LocalResultFolder,
                                                                                      productInfo.CUSTSN,
                                                                                      productInfo.MAC);
                    logger.DebugFormat("CopyCDSIResultFolder Copy to Result File CDSIFile.CopyCDSIResultFolder");

                    enumCDSIResult OMSresult = CDSIFile.CheckCDSIResultByOMS(config.LocalResultFolder,
                                                                                                                                     productInfo.CUSTSN,
                                                                                                                                     productInfo.MAC,
                                                                                                                                     productInfo.ATSNAV);
                    logger.DebugFormat("CheckCDSIResultByOMS Check Result CDSIFile.CheckCDSIResultByOMS");

                    if (OMSresult == enumCDSIResult.Ack)
                    {
                        string xmlName = productInfo.CUSTSN + "-" + productInfo.MAC + "\\" + productInfo.ATSNAV + ".DAT";

                        string srcXmlPath = config.LocalResultFolder + xmlName;
                        SQLStatement.ReadXMLCDSIAST(dbconnect,                                                                                   
                                                                                   productInfo.ProductID,
                                                                                   srcXmlPath);
                        logger.DebugFormat( "ReadXMLCDSIAST Parse XML ReadXMLCDSIAST");

                    }
                    else
                    {

                        //send  error email
                        localmsg = "<br/>CDSI " + " ProductID:" + productInfo.ProductID + "  PO:" + po + "  " + productInfo.ATSNAV +
                                    ".OMS File -- " +
                                    (OMSresult == enumCDSIResult.Err ? "Error Result" : "Not Found Result file") +
                                   " Please Check CDSI Server!!" + "<br/>";

                        msg = msg + localmsg;


                    }


                    #endregion


                    #region Copy CDSI file to Image download server

                    if (OMSresult == enumCDSIResult.Ack)
                    {
                        if (!string.IsNullOrEmpty(config.IMGServerUser))
                        {
                            WindowsImpersonationContext wic = Logon.ImpersinateUser(config.IMGServerUser, config.IMGDomain, config.IMGServerPassword);
                          logger.DebugFormat( " CopyCDSIResultFolder Copy Image Server start CopyCDSIResultFolder");
                            CDSIFile.CopyCDSIResultFolder(config.LocalResultFolder,
                                                                                          config.IMGFolder,
                                                                                          productInfo.CUSTSN,
                                                                                          productInfo.MAC);
                            logger.DebugFormat( "CopyCDSIResultFolder Copy Image Server end CopyCDSIResultFolder");
                            Logon.Log_off(wic);
                        }
                        else
                        {
                           logger.DebugFormat(  "CopyCDSIResultFolder Copy Image Server start (none login) CopyCDSIResultFolder");

                            CDSIFile.CopyCDSIResultFolder(config.LocalResultFolder,
                                                                                    config.IMGFolder,
                                                                                    productInfo.CUSTSN,
                                                                                    productInfo.MAC);

                            logger.DebugFormat("CopyCDSIResultFolder Copy Image Server end(none login) CopyCDSIResultFolder");
                        }
                    }

                    #endregion
                }
                else
                {
                    #region CDSI Result Error & not found then send email
                    //send  error email
                    localmsg = "<br/> RETRIEVE_Cmd4 CDSIResultFile -- ProductID:" + productInfo.ProductID + "  CUSTSN:" + productInfo.CUSTSN + " PO:" + po + "<br/>" +
                                (result == enumCDSIResult.Err ? "Error Result" : "Not Found Result file") +
                               " Please Check CDSI Server, Error Message:";

                    msg = msg + localmsg;


                    string[] data = File.ReadAllLines(config.LocalMAILBOXFolder + productInfo.CUSTSN + "-" + productInfo.MAC + ".ERR");
                    msg = msg + "<br/>" + string.Join("<br/>", data);
                    localmsg = localmsg + "<br/>" + string.Join("<br/>", data);
                    logger.DebugFormat( "RETRIEVE_CDSIResultFile {0}",
                                   localmsg);

                    #endregion
                }

                #region SIGNOFF Cmd

                if (!string.IsNullOrEmpty(config.CDSIServerUser))
                    wicCdsi = Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                  config.CDSIDomain,
                                                                                  config.CDSIServerPassword);

                CDSIFile.SIGNOFF_Cmd5(config.LocalMAILBOXFolder,
                                                               productInfo.CUSTSN,
                                                               productInfo.MAC,
                                                               config.CDSIMAILBOXFolder);
              logger.DebugFormat( "CDSIFile.SIGNOFF_Cmd5");

                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);

                }
                //Disable sleep  Vincent
                //Thread.Sleep(config.CDSICmdInterval);
                #endregion
                //Delete Local File

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

        public  static  void SendCDSIUpdateCmd(SqlConnection dbconnect,                                                                        
                                                                           AppConfig config,
                                                                           ProductInfo productInfo,
                                                                           string po,
                                                                           bool isLinkPO,
                                                                           ref string msg)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string localmsg = "";
                // write link PO cmd in LocalSN2POFolder
                #region send CDSI Cmd
                WindowsImpersonationContext wicCdsi = null;
                if (isLinkPO)
                {
                    if (!string.IsNullOrEmpty(config.CDSIServerUser))
                        wicCdsi = Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                      config.CDSIDomain,
                                                                                      config.CDSIServerPassword);

                    CDSIFile.LinkPO_Cmd1(config.LocalSN2POFolder,
                                                                    productInfo.CUSTSN,
                                                                    po, config.CDSISN2POFolder);
                    logger.DebugFormat("SendCDSIUpdateCmd {0}", "CDSIFile.LinkPO_Cmd1");

                }

                CDSIFile.SignUp_Cmd2(config.LocalMAILBOXFolder,
                                                                productInfo.CUSTSN,
                                                                productInfo.MAC,
                                                                config.CDSIMAILBOXFolder);
                logger.DebugFormat( "SendCDSIUpdateCmd {0}", "CDSIFile.SignUp_Cmd2");
                //Thread.Sleep(config.CDSICmdInterval);

                CDSIFile.OMSUPDATE_Cmd3(config.LocalMAILBOXFolder,
                                                                             productInfo.CUSTSN,
                                                                             productInfo.MAC,
                                                                             productInfo.ATSNAV,
                                                                             productInfo.MN1,
                                                                             productInfo.SYSID,
                                                                             config.CDSIMAILBOXFolder);
                logger.DebugFormat("SendCDSIUpdateCmd {0}", " CDSIFile.OMSUPDATE_Cmd3");
                // don't sleep          
                //Vincent add check result
                enumCDSIResult result1 = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                         config.CDSIMAILBOXFolder,
                                                                                       productInfo.CUSTSN,
                                                                                        productInfo.MAC);
                if (result1 == enumCDSIResult.NotFound)
                {
                    #region retry again and sleep 5 sec
                    result1 = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                  config.CDSIMAILBOXFolder,
                                                                                  productInfo.CUSTSN,
                                                                                  productInfo.MAC);
                    #endregion
                }

                if (result1 == enumCDSIResult.Err || result1 == enumCDSIResult.NotFound)
                {
                    #region write error message
                    //send  error email
                    localmsg = "<br/> OMSUPDATE_Cmd3 CDSIResultFile -- ProductID:" + productInfo.ProductID + "  CUSTSN:" + productInfo.CUSTSN + " PO:" + po + "<br/>" +
                                (result1 == enumCDSIResult.Err ? "Error Result" : "Not Found Result file") +
                               " Please Check CDSI Server, Error Message:";

                    msg = msg + localmsg;

                    if (result1 == enumCDSIResult.Err)
                    {
                        string[] data = File.ReadAllLines(config.LocalMAILBOXFolder + productInfo.CUSTSN + "-" + productInfo.MAC + ".ERR");
                        msg = msg + "<br/>" + string.Join("<br/>", data);
                        localmsg = localmsg + "<br/>" + string.Join("<br/>", data);
                    }

                    logger.DebugFormat("SendCDSIUpdateCmd {0} {1}",
                                   "OMSUPDATE_CDSIResultFile",
                                   localmsg);

                    #endregion
                }
                else
                {
                    logger.DebugFormat("SendCDSIUpdateCmd {0} {1}",
                                  "OMSUPDATE_CDSIResultFile",
                                  "Get Ack file");
                }



                #endregion



                #region SIGNOFF Cmd

                if (!string.IsNullOrEmpty(config.CDSIServerUser))
                    wicCdsi = Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                  config.CDSIDomain,
                                                                                  config.CDSIServerPassword);

                CDSIFile.SIGNOFF_Cmd5(config.LocalMAILBOXFolder,
                                                               productInfo.CUSTSN,
                                                               productInfo.MAC,
                                                               config.CDSIMAILBOXFolder);
                logger.DebugFormat("SendCDSIUpdateCmd {0}" , "CDSIFile.SIGNOFF_Cmd5");

                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);

                }
                //Disable sleep  Vincent
                //Thread.Sleep(config.CDSICmdInterval);
                #endregion
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
    }
}
