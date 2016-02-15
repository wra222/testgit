using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security.Permissions;
using System.IO;
using System.Net ;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using UTL.SQL;
using UTL.MetaData;
using UTL;
using UTL.Mail;
using log4net;
using UPS.UTL.SQL;


namespace UPS.CDSI
{
    class Program
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			       
        static void Main(string[] args)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //if (File.Exists(@"\\10.99.183.50\fsm_com\Mailbox\CNU23090R7-28924AD3C1D7.ACK"))
                //{
                //    Console.WriteLine(@"CheckCDSIResult  find \\10.99.183.50\fsm_com\Mailbox\CNU23090R7-28924AD3C1D7.ACK Ack file!!");

                //}
                if (!ExecShell.CheckOneProcessRun())
                {
                    return;
                }

                AppConfig config = new AppConfig();
                string msg = "";
                string localmsg = "";

                //Log log = new Log(config.logPath, config.logPrefixName);
                //log.write(LogType.Info, 0, "Main" , "", "Start Program...");
                try
                {
                    SqlConnection dbconnect = new SqlConnection(config.DBConnectStr);
                    dbconnect.Open();
                    logger.DebugFormat("connected with HPIMES DB", "OK");

                    SqlConnection CdsiDbconnect = new SqlConnection(config.CDSIDBConnectStr);
                    CdsiDbconnect.Open();
                    logger.DebugFormat("connected with CDSI DB", "OK");

                    List<string> SnoIdList = SQLStatement.GetCDSISNList(dbconnect,  config.OffsetDay);
                    logger.DebugFormat("Send Link CDSI PO.........................");
                    List<ProductInfo> AssignedPOList = new List<ProductInfo>();
                    List<ProductInfo> UPSAssignedPOList = new List<ProductInfo>();

                    #region send PO Link to CDSI Server
                    foreach (string snoId in SnoIdList)
                    {
                        #region //0.Check ProductID info
                        ProductInfo productInfo = SQLStatement.GetProductInfo(dbconnect,  snoId);
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

                            localmsg = "<br/> CDSI ProductID:" + snoId + DataMsg + ";" + emptyDataMsg + "為空!!! 請檢查設定或等過站分配" + emptyDataMsg + "值!!" + "<br/>";

                            msg = msg + localmsg;

                            logger.DebugFormat("Data is null or empty {0}",  localmsg);


                            #endregion
                            continue;
                        }
                        #endregion


                        #region //1.Get PO
                        string po = SQLStatement.GetSnoPoMo(dbconnect,  snoId);
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
                                if (SQLStatement.AssignPO(dbconnect,  config, productInfo, poInfo))
                                {
                                    logger.DebugFormat( "AssignPO " + poInfo.ProductID + " PO:" + poInfo.PO + " DeliveryNo:" + poInfo.DeliveryNo);


                                    //檢查UPS Device
                                    UPSDatabase  db= new UPSDatabase(config.DBConnectStr);
                                    UPSCombinePO combinePO =null;
                                    if (UPSData.DecideUPSCDSI(db, productInfo, poInfo, out combinePO))
                                    {
                                        #region UPS Device
                                        logger.DebugFormat("ProductID:{0} Model:{1} PO:{2} is UPS device!!", productInfo.ProductID, productInfo.Model, productInfo.PO); 
                                        string errorText=null;
                                        if (combinePO == null)
                                        {
                                            logger.ErrorFormat("ProductID:{0} Model:{1} PO:{2} is UPS device, no upload UPS PO!!", productInfo.ProductID, productInfo.Model, productInfo.PO); 

                                            localmsg = "<br/> CDSI ProductID:" + productInfo.ProductID +
                                                                           " CustomSN:" + productInfo.CUSTSN +
                                                                           " MAC:" + productInfo.MAC +
                                                                           " ATSNAV:" + productInfo.ATSNAV +
                                                                            " Model:" + productInfo.Model +
                                                                           " PO:" + productInfo.PO + " 未上傳 至UPS Server ,請上傳此PO至UPS Server <br/>";

                                            msg = msg + localmsg;
                                        }
                                        else
                                        {
                                            if (!UPSData.AssignUPSPO(config, db, productInfo, poInfo, combinePO, out errorText))
                                            {
                                                localmsg = "<br/> CDSI ProductID:" + productInfo.ProductID +
                                                                           " CustomSN:" + productInfo.CUSTSN +
                                                                           " MAC:" + productInfo.MAC +
                                                                           " ATSNAV:" + productInfo.ATSNAV +
                                                                            " Model:" + productInfo.Model +
                                                                           " PO:" + productInfo.PO + " 為 UPS Server 報錯 " + errorText + "<br/>";

                                                msg = msg + localmsg;
                                            }
                                        }
                                        #endregion
                                        continue;
                                    }
                                    else
                                    {
                                        logger.DebugFormat("ProductID:{0} Model:{1} PO:{2} is not UPS device!!", productInfo.ProductID, productInfo.Model, productInfo.PO); 
                                        #region CDSI SRV
                                        SQLStatement.InsertCDSIPoMo(dbconnect, poInfo);

                                        po = SQLStatement.GetSnoPoMo(dbconnect, snoId);

                                        //Thread.Sleep(config.CDSICmdInterval);

                                        //SendCDSIPOLinkCmd(dbconnect,
                                        //                                       log,
                                        //                                       config,
                                        //                                       productInfo,
                                        //                                       po,
                                        //                                        ref msg);
                                        string msgData = "";
                                        CDSIFile.SendCDSIUpdateCmd(dbconnect,
                                                                            config,
                                                                            productInfo,
                                                                            po,
                                                                            true,
                                                                            ref msgData);
                                        productInfo.PO = po;
                                        AssignedPOList.Add(productInfo);
                                        #endregion
                                    }
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

                                   logger.DebugFormat( "Not Found DN {0}" ,
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
                                                                   " CustomSN:" + productInfo.CUSTSN +
                                                                   " Model:" + productInfo.Model +
                                                                   " 未設定PO, 請至iMES維護頁面CDSI PO" + "<br/>";
                                msg = msg + "<br/>" + localmsg;

                                logger.DebugFormat( "Not Found PO {0}",
                                               localmsg);

                                #endregion
                                continue;
                            }
                            #endregion
                        }
                        else
                        {
                            CDSIFile.SendCDSIPOLinkCmd(dbconnect,
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

                    logger.DebugFormat("Send CDSI Update & Retrive Asset Tag.........................");
                    #region Send OMSUpdate & Retrive ; used assigned PO for each send comand to CDSI Server
                    //foreach (string snoId in SnoIdList)
                    foreach (ProductInfo productInfo in AssignedPOList)
                    {
                        #region //0. Check CDSI Server FactoryPO
                        int cdsiCategory = SQLStatement.CheckCDSIFactoryPO(CdsiDbconnect,                                                                                                                   
                                                                                                                       productInfo.PO);
                        if (cdsiCategory == 0)   // not found CDSI FactoryPO
                        {
                            #region not found CDSI FactoryPO
                            localmsg = "<br/> CDSI ProductID:" + productInfo.ProductID +
                                " CustomSN:" + productInfo.CUSTSN +
                                " MAC:" + productInfo.MAC +
                                " ATSNAV:" + productInfo.ATSNAV +
                                 " Model:" + productInfo.Model +
                                " PO:" + productInfo.PO + " 在CDSI DB 找不到FactoryPO!!!" + "<br/>";

                            msg = msg + localmsg;

                            logger.DebugFormat( "not found CDSI FactoryPO {0}",
                                           localmsg);

                            #endregion

                            continue;

                        }
                        else
                        {
                            localmsg = "CDSI FactoryPO Category: " + (cdsiCategory == 1 ? "Shell PO" : " Nornal PO");
                            logger.DebugFormat( "CheckCDSIFactoryPO {0}",
                                          localmsg);
                        }
                        #endregion

                        CDSIFile.SendCDSICmd(dbconnect,                                                
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
                    msg = e.StackTrace + " " + e.Message;
                    logger.Error(msg, e);
                    //log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main", "StackTrace", e.StackTrace);
                    //log.write(LogType.error, AppDomain.CurrentDomain.Id, "Main", "Message", e.Message);


                    SendMail.Send(config.FromAddress,
                                                          config.ToAddress,
                                                          config.CcAddress,
                                                          config.MailSubject + "program error",
                                                          msg,
                                                          config.EmailServer);
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
                                                          config.EmailServer);
                    }

                    #endregion

                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);                
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }          
               
        }     
                                                    
    }  
}
