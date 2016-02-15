using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security.Permissions;
using System.IO;
using UTL;
using System.Net ;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;



namespace eBook
{
    class Program
    {   
               
        static void Main(string[] args)
        {
            //if (File.Exists(@"\\10.99.183.50\fsm_com\Mailbox\CNU23090R7-28924AD3C1D7.ACK"))
            //{
            //    Console.WriteLine(@"CheckCDSIResult  find \\10.99.183.50\fsm_com\Mailbox\CNU23090R7-28924AD3C1D7.ACK Ack file!!");
               
            //}
            
            AppConfig config = new AppConfig();
            string msg="";
            string localmsg = "";
                     
            Log log = new Log(config.logPath, config.logPrefixName);
            log.write(LogType.Info, 0, "Main" , "", "Start Program...");
            try 
            {
                SqlConnection dbconnect = new SqlConnection(config.DBConnectStr);
                dbconnect.Open();               
                log.write(LogType.Info, 0, "main", "connected with HPIMES DB", "OK");

                SqlConnection CdsiDbconnect = new SqlConnection(config.CDSIDBConnectStr);
                CdsiDbconnect.Open();
                log.write(LogType.Info, 0, "main", "connected with CDSI DB", "OK");

                List<string> SnoIdList = SQLStatement.GetCDSISNList(dbconnect, log, config.OffsetDay);
                log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main", "Start", "Send Link CDSI PO.........................");
                List<ProductInfo> AssignedPOList = new List<ProductInfo>();

                #region send PO Link to CDSI Server
                foreach (string snoId in SnoIdList)
                {
                    #region //0.Check ProductID info
                    ProductInfo productInfo = SQLStatement.GetProductInfo(dbconnect, log, snoId);
                     if (string.IsNullOrEmpty(productInfo.CUSTSN) ||
                            string.IsNullOrEmpty(productInfo.MAC) ||
                            string.IsNullOrEmpty(productInfo.ATSNAV))
                     {
                            #region send  email not found PO need mantain PO data
                            string emptyDataMsg = "";
                            string DataMsg = "";

                            if (string.IsNullOrEmpty(productInfo.CUSTSN))
                                emptyDataMsg = emptyDataMsg + " CustomSN: ";
                            else
                                DataMsg = DataMsg + " CustomSN:" + productInfo.CUSTSN;

                           if (string.IsNullOrEmpty(productInfo.MAC))
                               emptyDataMsg = emptyDataMsg + " MAC: ";
                           else
                               DataMsg = DataMsg + " MAC:" + productInfo.MAC;

                           if (string.IsNullOrEmpty(productInfo.ATSNAV))
                               emptyDataMsg = emptyDataMsg + " ATSNAV: ";
                           else
                               DataMsg = DataMsg + " ATSNAV:" + productInfo.ATSNAV;

                            //localmsg = "<br/> CDSI ProductID:" + snoId + " CustomSN:" + (string.IsNullOrEmpty(productInfo.CUSTSN) ? "" : productInfo.CUSTSN) + " " +
                            //    " MAC:" + (string.IsNullOrEmpty(productInfo.MAC) ? "" : productInfo.MAC) + " " +
                            //    " ATSNAV:" + (string.IsNullOrEmpty(productInfo.ATSNAV) ? "" : productInfo.ATSNAV) + " " + " 為空!!! 請檢查設定!!" + "<br/>";

                            localmsg = "<br/> CDSI ProductID:" + snoId + DataMsg +";" +emptyDataMsg + "為空!!! 請檢查設定或等過站分配" +emptyDataMsg +"值!!" + "<br/>";

                            msg = msg + localmsg;

                            log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main",
                                           "Data is null or empty",
                                           localmsg);

                           
                            #endregion
                            continue;
                     }
                    #endregion


                     #region //1.Get PO
                     string po = SQLStatement.GetSnoPoMo(dbconnect, log, snoId);
                    if (string.IsNullOrEmpty(po))
                    {
                        #region assign PO
                        if (config.AutoAssignPO == 1)
                        {
                            CDSIPO poInfo = new CDSIPO();
                            poInfo.ProductID = snoId;
                            poInfo.MOId = productInfo.MOId;
                            poInfo.PO = "";
                            poInfo.DeliveryNo = "";

                            #region assign PO
                            if (SQLStatement.AssignPO(dbconnect, log, config, productInfo, poInfo))
                            {
                                log.write(LogType.Info, 0, "AssignPO", poInfo.ProductID, "PO:" + poInfo.PO + " DeliveryNo:" + poInfo.DeliveryNo);

                                SQLStatement.InsertCDSIPoMo(dbconnect, log, poInfo);

                                po = SQLStatement.GetSnoPoMo(dbconnect, log, snoId);

                                //Thread.Sleep(config.CDSICmdInterval);

                                //SendCDSIPOLinkCmd(dbconnect,
                                //                                       log,
                                //                                       config,
                                //                                       productInfo,
                                //                                       po,
                                //                                        ref msg);
                                string msgData = "";
                                SendCDSIUpdateCmd(dbconnect,
                                                                    log,
                                                                    config,
                                                                    productInfo,
                                                                    po,
                                                                    true,
                                                                    ref msgData);
                                productInfo.PO =po;
                                AssignedPOList.Add(productInfo);
                            }
                            else
                            {
                                #region send  email not found PO need upload PO data
                                localmsg = "<br/>" + "CDSI ProductID:" + snoId + 
                                                                 " CustomSN:" + productInfo.CUSTSN +
                                                                  " MAC:" + productInfo.MAC +
                                                                 " ATSNAV:" + productInfo.ATSNAV +
                                                                 " Model:" + productInfo.Model + 
                                                                 " 找不到船務資料，請上傳船務資料!!" + "<br/>";
                                msg = msg + "<br/>" + localmsg;

                                log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main",
                                               "Not Found DN",
                                               localmsg);

                                #endregion

                                continue;
                            }
                            #endregion
                        }
                        else
                        {
                            #region send  email not found PO need upload PO data
                            localmsg = "<br/>" + "CDSI ProductID:" + snoId +
                                                                " MAC:" + productInfo.MAC +
                                                               " ATSNAV:" + productInfo.ATSNAV + 
                                                               " CustomSN:" +  productInfo.CUSTSN  + 
                                                               " Model:" + productInfo.Model + 
                                                               " 未設定PO, 請至iMES維護頁面CDSI PO" + "<br/>";
                            msg = msg + "<br/>" + localmsg;

                            log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main",
                                           "Not Found PO",
                                           localmsg);

                            #endregion
                            continue;
                        }
                        #endregion
                    }
                    else
                    {
                        SendCDSIPOLinkCmd(dbconnect,
                                                                       log,
                                                                       config,
                                                                       productInfo,
                                                                       po,
                                                                        ref msg);
                        productInfo.PO = po;
                        AssignedPOList.Add(productInfo);
                    }
                     #endregion
                }
                #endregion
                Thread.Sleep(config.CDSICmdInterval);

                log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main", "Start", "Send CDSI Update & Retrive Asset Tag.........................");
                #region Send OMSUpdate & Retrive ; used assigned PO for each send comand to CDSI Server
                //foreach (string snoId in SnoIdList)
                foreach (ProductInfo productInfo in AssignedPOList)
                {                   
                        #region //0. Check CDSI Server FactoryPO
                        int cdsiCategory = SQLStatement.CheckCDSIFactoryPO(CdsiDbconnect,
                                                                                                                       log,
                                                                                                                       productInfo.PO);
                        if (cdsiCategory == 0)   // not found CDSI FactoryPO
                        {
                            #region not found CDSI FactoryPO
                            localmsg = "<br/> CDSI ProductID:" + productInfo.ProductID +
                                " CustomSN:" + productInfo.CUSTSN + 
                                " MAC:" + productInfo.MAC + 
                                " ATSNAV:" +productInfo.ATSNAV +
                                 " Model:" + productInfo.Model +
                                " PO:" + productInfo.PO + " 在CDSI DB 找不到FactoryPO!!!" + "<br/>";

                            msg = msg + localmsg;

                            log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main",
                                           "not found CDSI FactoryPO",
                                           localmsg);

                            #endregion

                            continue;

                        }
                        else
                        {
                            localmsg = "CDSI FactoryPO Category: " + (cdsiCategory== 1? "Shell PO" : " Nornal PO");
                            log.write(LogType.Info, AppDomain.CurrentDomain.Id, "Main",
                                          "CheckCDSIFactoryPO",
                                          localmsg);
                        }
                        #endregion
                        
                        SendCDSICmd(dbconnect,
                                                   log,
                                                   config,
                                                   productInfo,
                                                   productInfo.PO,
                                                   false,
                                                   ref msg);

                  
                }
                
                #endregion
            }
            catch (Exception e)
            {
                #region exception handle
                msg = e.StackTrace + " " +  e.Message;
                 log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main", "StackTrace", e.StackTrace);
                 log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main", "Message", e.Message);
                         
               
                SendMail.Send(config.FromAddress,
                                                      config.ToAddress,
                                                      config.CcAddress,
                                                      config.MailSubject + "program error",
                                                      msg,
                                                      config.EmailServer,
                                                      log);
                #endregion
            }
            finally
            {
                #region error send email
                if (!string.IsNullOrEmpty(msg))
                {
                    SendMail.Send(config.FromAddress,
                                                      config.ToAddress,
                                                      config.CcAddress,
                                                      config.MailSubject + " 資料有誤!!",
                                                      msg,
                                                      config.EmailServer,
                                                      log);
                }
                
                #endregion
               
            }
               
        }

        static private void SendCDSIPOLinkCmd(SqlConnection dbconnect,
                                                   Log log,
                                                   AppConfig config,
                                                    ProductInfo productInfo,
                                                    string po,
                                                   ref string msg)
        {
            WindowsImpersonationContext wicCdsi = null;
            if (!string.IsNullOrEmpty(config.CDSIServerUser))
                wicCdsi = UTL.Logon.ImpersinateUser(config.CDSIServerUser,
                                                                              config.CDSIDomain,
                                                                              config.CDSIServerPassword,
                                                                              log);

            CDSIFile.LinkPO_Cmd1(config.LocalSN2POFolder,
                                                            productInfo.CUSTSN,
                                                            po, config.CDSISN2POFolder);
            log.write(LogType.Info, 0, "Write Cmd", "", "CDSIFile.LinkPO_Cmd1");

        }

        static private void SendCDSICmd(SqlConnection dbconnect,
                                                                Log log,
                                                                AppConfig config,
                                                                ProductInfo productInfo,
                                                                string po,
                                                                bool isLinkPO,
                                                                ref string msg)
        {
            string localmsg="";
             // write link PO cmd in LocalSN2POFolder
             #region send CDSI Cmd
                            WindowsImpersonationContext wicCdsi = null;
                            if (isLinkPO)
                            {
                                if (!string.IsNullOrEmpty(config.CDSIServerUser))
                                    wicCdsi = UTL.Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                                  config.CDSIDomain,
                                                                                                  config.CDSIServerPassword,
                                                                                                  log);

                                CDSIFile.LinkPO_Cmd1(config.LocalSN2POFolder,
                                                                                productInfo.CUSTSN,
                                                                                po, config.CDSISN2POFolder);
                                log.write(LogType.Info, 0, "Write Cmd", "", "CDSIFile.LinkPO_Cmd1");

                            }

                            CDSIFile.SignUp_Cmd2(config.LocalMAILBOXFolder,
                                                                            productInfo.CUSTSN,
                                                                            productInfo.MAC,
                                                                            config.CDSIMAILBOXFolder);
                            log.write(LogType.Info, 0, "Write Cmd", "", "CDSIFile.SignUp_Cmd2");
                            //Thread.Sleep(config.CDSICmdInterval);

                            CDSIFile.OMSUPDATE_Cmd3(config.LocalMAILBOXFolder,
                                                                                         productInfo.CUSTSN,
                                                                                         productInfo.MAC,
                                                                                         productInfo.ATSNAV,
                                                                                         productInfo.MN1,
                                                                                         productInfo.SYSID,
                                                                                         config.CDSIMAILBOXFolder);
                            log.write(LogType.Info, 0, "Write Cmd", "", " CDSIFile.OMSUPDATE_Cmd3");
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
                               
                                log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main",
                                               "OMSUPDATE_CDSIResultFile",
                                               localmsg);

                                #endregion

                                #region SIGNOFF Cmd

                                if (!string.IsNullOrEmpty(config.CDSIServerUser))
                                    wicCdsi = UTL.Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                                  config.CDSIDomain,
                                                                                                  config.CDSIServerPassword,
                                                                                                  log);

                                CDSIFile.SIGNOFF_Cmd5(config.LocalMAILBOXFolder,
                                                                               productInfo.CUSTSN,
                                                                               productInfo.MAC,
                                                                               config.CDSIMAILBOXFolder);
                                log.write(LogType.Info, 0, "Write Cmd", "", "CDSIFile.SIGNOFF_Cmd5");

                                if (wicCdsi != null)
                                {
                                    UTL.Logon.Log_off(wicCdsi);

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
                            log.write(LogType.Info, 0, "Write Cmd", "", "CDSIFile.RETRIEVE_Cmd4");
                            

                            if (wicCdsi != null)
                            {
                                UTL.Logon.Log_off(wicCdsi);

                            }

                            Thread.Sleep(config.CDSICmdInterval);

                            #endregion

            //Get ACK/ERR file
            enumCDSIResult result = CDSIFile.CheckCDSIResult(config.LocalMAILBOXFolder,
                                                                                                      config.CDSIMAILBOXFolder,
                                                                                                    productInfo.CUSTSN,
                                                                                                     productInfo.MAC);
            log.write(LogType.Info, 0, "CheckCDSIResult", "", "CDSIFile.CheckCDSIResult");
           if (result == enumCDSIResult.NotFound)
           {
                #region retry again and sleep 5 sec
                                Thread.Sleep(config.CDSICmdInterval);
                                result = CDSIFile.CheckCDSIResult(   config.LocalMAILBOXFolder,
                                                                                                config.CDSIMAILBOXFolder,
                                                                                                   productInfo.CUSTSN,
                                                                                                    productInfo.MAC);
                                #endregion
           }

           if (result == enumCDSIResult.Ack)
           {
               log.write(LogType.Info, AppDomain.CurrentDomain.Id, "Main",
                                           "CheckCDSIResult",
                                           "OK");
                #region  Copy CDSI Result XML file to local Result folder & Read CDSI XML file
                                CDSIFile.CopyCDSIResultFolder(config.CDSIResultFolder,
                                                                                                  config.LocalResultFolder,
                                                                                                  productInfo.CUSTSN,
                                                                                                  productInfo.MAC);
                                log.write(LogType.Info, 0, "CopyCDSIResultFolder", " Copy to Result File", "CDSIFile.CopyCDSIResultFolder");

                                enumCDSIResult OMSresult = CDSIFile.CheckCDSIResultByOMS(config.LocalResultFolder,
                                                                                                                                                 productInfo.CUSTSN,
                                                                                                                                                 productInfo.MAC,
                                                                                                                                                 productInfo.ATSNAV);
                                log.write(LogType.Info, 0, "CheckCDSIResultByOMS", "Check Result", "CDSIFile.CheckCDSIResultByOMS");
                                                               
                                if (OMSresult == enumCDSIResult.Ack)
                                {
                                    string xmlName = productInfo.CUSTSN + "-" + productInfo.MAC + "\\" + productInfo.ATSNAV + ".DAT";

                                    string srcXmlPath = config.LocalResultFolder + xmlName;
                                    SQLStatement.ReadXMLCDSIAST(dbconnect,
                                                                                               log,
                                                                                               productInfo.ProductID,
                                                                                               srcXmlPath);
                                    log.write(LogType.Info, AppDomain.CurrentDomain.Id, "ReadXMLCDSIAST",
                                               "Parse XML",
                                              "ReadXMLCDSIAST");

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
                                        WindowsImpersonationContext wic = UTL.Logon.ImpersinateUser(config.IMGServerUser, config.IMGDomain, config.IMGServerPassword, log);
                                        log.write(LogType.Info, AppDomain.CurrentDomain.Id, "CopyCDSIResultFolder",
                                             "Copy Image Server start",
                                            "CopyCDSIResultFolder");
                                        CDSIFile.CopyCDSIResultFolder(config.LocalResultFolder,
                                                                                                      config.IMGFolder,
                                                                                                      productInfo.CUSTSN,
                                                                                                      productInfo.MAC);
                                        log.write(LogType.Info, AppDomain.CurrentDomain.Id, "CopyCDSIResultFolder",
                                             "Copy Image Server end",
                                            "CopyCDSIResultFolder");
                                        UTL.Logon.Log_off(wic);
                                    }
                                    else
                                    {
                                        log.write(LogType.Info, AppDomain.CurrentDomain.Id, "CopyCDSIResultFolder",
                                            "Copy Image Server start (none login)",
                                           "CopyCDSIResultFolder");

                                        CDSIFile.CopyCDSIResultFolder(config.LocalResultFolder, 
                                                                                                config.IMGFolder, 
                                                                                                productInfo.CUSTSN, 
                                                                                                productInfo.MAC);
                                        
                                        log.write(LogType.Info, AppDomain.CurrentDomain.Id, "CopyCDSIResultFolder",
                                            "Copy Image Server end(none login)",
                                           "CopyCDSIResultFolder");
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
                                log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main",
                                               "RETRIEVE_CDSIResultFile",
                                               localmsg);
                               
                                #endregion
             }

             #region SIGNOFF Cmd

                            if (!string.IsNullOrEmpty(config.CDSIServerUser))
                                wicCdsi = UTL.Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                              config.CDSIDomain,
                                                                                              config.CDSIServerPassword,
                                                                                              log);

                            CDSIFile.SIGNOFF_Cmd5(config.LocalMAILBOXFolder,
                                                                           productInfo.CUSTSN,
                                                                           productInfo.MAC,
                                                                           config.CDSIMAILBOXFolder);
                            log.write(LogType.Info, 0, "Write Cmd", "", "CDSIFile.SIGNOFF_Cmd5");

                            if (wicCdsi != null)
                            {
                                UTL.Logon.Log_off(wicCdsi);

                            }
                            //Disable sleep  Vincent
                            //Thread.Sleep(config.CDSICmdInterval);
                            #endregion
             //Delete Local File
                       


        }

        static private void SendCDSIUpdateCmd(SqlConnection dbconnect,
                                                                           Log log,
                                                                           AppConfig config,
                                                                           ProductInfo productInfo,
                                                                           string po,
                                                                           bool isLinkPO,
                                                                           ref string msg)
        {
            string localmsg = "";
            // write link PO cmd in LocalSN2POFolder
            #region send CDSI Cmd
            WindowsImpersonationContext wicCdsi = null;
            if (isLinkPO)
            {
                if (!string.IsNullOrEmpty(config.CDSIServerUser))
                    wicCdsi = UTL.Logon.ImpersinateUser(config.CDSIServerUser,
                                                                                  config.CDSIDomain,
                                                                                  config.CDSIServerPassword,
                                                                                  log);

                CDSIFile.LinkPO_Cmd1(config.LocalSN2POFolder,
                                                                productInfo.CUSTSN,
                                                                po, config.CDSISN2POFolder);
                log.write(LogType.Info, 0, "Write Cmd", "SendCDSIUpdateCmd", "CDSIFile.LinkPO_Cmd1");

            }

            CDSIFile.SignUp_Cmd2(config.LocalMAILBOXFolder,
                                                            productInfo.CUSTSN,
                                                            productInfo.MAC,
                                                            config.CDSIMAILBOXFolder);
            log.write(LogType.Info, 0, "Write Cmd", "SendCDSIUpdateCmd", "CDSIFile.SignUp_Cmd2");
            //Thread.Sleep(config.CDSICmdInterval);

            CDSIFile.OMSUPDATE_Cmd3(config.LocalMAILBOXFolder,
                                                                         productInfo.CUSTSN,
                                                                         productInfo.MAC,
                                                                         productInfo.ATSNAV,
                                                                         productInfo.MN1,
                                                                         productInfo.SYSID,
                                                                         config.CDSIMAILBOXFolder);
            log.write(LogType.Info, 0, "Write Cmd", "SendCDSIUpdateCmd", " CDSIFile.OMSUPDATE_Cmd3");
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

                log.write(LogType.error, AppDomain.CurrentDomain.Id, "SendCDSIUpdateCmd",
                               "OMSUPDATE_CDSIResultFile",
                               localmsg);

                #endregion
            }
            else
            {
                log.write(LogType.Info, AppDomain.CurrentDomain.Id, "SendCDSIUpdateCmd",
                              "OMSUPDATE_CDSIResultFile",
                              "Get Ack file");
            }

          

            #endregion

           

            #region SIGNOFF Cmd

            if (!string.IsNullOrEmpty(config.CDSIServerUser))
                wicCdsi = UTL.Logon.ImpersinateUser(config.CDSIServerUser,
                                                                              config.CDSIDomain,
                                                                              config.CDSIServerPassword,
                                                                              log);

            CDSIFile.SIGNOFF_Cmd5(config.LocalMAILBOXFolder,
                                                           productInfo.CUSTSN,
                                                           productInfo.MAC,
                                                           config.CDSIMAILBOXFolder);
            log.write(LogType.Info, 0, "Write Cmd", "SendCDSIUpdateCmd", "CDSIFile.SIGNOFF_Cmd5");

            if (wicCdsi != null)
            {
                UTL.Logon.Log_off(wicCdsi);

            }
            //Disable sleep  Vincent
            //Thread.Sleep(config.CDSICmdInterval);
            #endregion
            



        }
                                                    
    }  
}
