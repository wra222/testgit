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
using UTL.Agent;
using IMES.Station.Interface.StationIntf;
using UTL.SQL;
using log4net;
using UTL.Mail;
using UPS.UTL.SQL;
using UTL.MetaData;
using UPS.CNRS;


namespace CNRS
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
                if (!ExecShell.CheckOneProcessRun())
                {
                    return;
                }
                AppConfig config = new AppConfig();
                string msg = "";
                string localmsg = "";

                //Log log = new Log(config.logPath, config.logPrefixName);
               // log.write(LogType.Info, 0, "Main", "", "Start Program...");
                try
                {
                    SqlConnection dbconnect = new SqlConnection(config.DBConnectStr);
                    dbconnect.Open();
                    logger.DebugFormat("connected with HPIMES DB", "OK");



                    List<string> SnoIdList = SQLStatement.GetCNRSSNList(dbconnect, config.OffsetDay);
                    logger.DebugFormat("Send Link CNRS PO.........................");

                    //List<ProductInfo> AssignedPOList = new List<ProductInfo>();

                    #region send PO Link to CNRS Table
                    foreach (string snoId in SnoIdList)
                    {
                        #region //0.Check ProductID info
                        ProductInfo productInfo = SQLStatement.GetProductInfo(dbconnect, snoId);
                        if (string.IsNullOrEmpty(productInfo.CUSTSN) ||
                               string.IsNullOrEmpty(productInfo.MAC) ||
                               productInfo.IsCNRS != "Y" ||
                               productInfo.IsPO != "Y")
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

                            if (productInfo.IsCNRS != "Y")
                                emptyDataMsg = emptyDataMsg + " ModelInfo.Name='CNRS'  ModelInfo.Value!=Y ";

                            if (productInfo.IsPO != "Y")
                                emptyDataMsg = emptyDataMsg + " ModelInfo.Name='PO'  ModelInfo.Value!=Y ";

                            //localmsg = "<br/> CDSI ProductID:" + snoId + " CustomSN:" + (string.IsNullOrEmpty(productInfo.CUSTSN) ? "" : productInfo.CUSTSN) + " " +
                            //    " MAC:" + (string.IsNullOrEmpty(productInfo.MAC) ? "" : productInfo.MAC) + " " +
                            //    " ATSNAV:" + (string.IsNullOrEmpty(productInfo.ATSNAV) ? "" : productInfo.ATSNAV) + " " + " 為空!!! 請檢查設定!!" + "<br/>";

                            localmsg = "<br/> CNRS ProductID:" + snoId + DataMsg + ";" + emptyDataMsg + "為空!!! 請檢查ModelInfo設定或等過站分配" + emptyDataMsg + "值!!" + "<br/>";

                            msg = msg + localmsg;

                            logger.DebugFormat("Data is null or empty {0}", localmsg);


                            #endregion
                            continue;
                        }
                        #endregion

                        #region //1.Get PO
                        string po = SQLStatement.GetSnoPoMo(dbconnect, snoId);
                        if (string.IsNullOrEmpty(po))
                        {
                            #region assign PO

                            CDSIPO poInfo = new CDSIPO();
                            poInfo.ProductID = snoId;
                            poInfo.MOId = productInfo.MOId;
                            poInfo.PO = "";
                            poInfo.DeliveryNo = "";
                            poInfo.DeliveryQty = 0;
                            poInfo.RemainQty = 0;

                            #region assign PO
                            if (SQLStatement.AssignPO(dbconnect, config, productInfo, poInfo))
                            {
                                logger.DebugFormat("AssignPO {0} PO:{1} DeliveryNo:{2}", poInfo.ProductID, poInfo.PO , poInfo.DeliveryNo);

                                //檢查UPS Device
                                UPSDatabase  db= new UPSDatabase(config.DBConnectStr);
                                UPSCombinePO combinePO =null;
                                if (UPSData.DecideUPSCDSI(db, productInfo, poInfo, out combinePO))
                                {
                                    #region UPS Device
                                    logger.DebugFormat("ProductID:{0} Model:{1} PO:{2} is UPS device!!", productInfo.ProductID, productInfo.Model, productInfo.PO);
                                    string errorText = null;
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
                                        if (!UPSData.AssignCNRSPO(config, db, productInfo, poInfo, combinePO, out errorText))
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
                                    #region none-UPS 機器
                                    SpecialOrder demianOrder = SQLStatement.GetSpecialOrder(dbconnect, poInfo.PO);
                                    if (demianOrder != null && demianOrder.Category.Equals("CNRS"))
                                    {
                                        if (demianOrder.Qty != poInfo.DeliveryQty && config.bCheckQty)
                                        {
                                            #region send  email Delivery Qty is not equal SpecialOrder Qty
                                            localmsg = "<br/>" + "CNRS ProductID:" + snoId +
                                                                             " CustomSN:" + productInfo.CUSTSN +
                                                                              " MAC:" + productInfo.MAC +
                                                                             " Model:" + productInfo.Model +
                                                                             "FactoryPO:" + demianOrder.FactoryPO +
                                                                             " SpecialOrder Qty: " + demianOrder.Qty + " Delivery Qty:" + poInfo.DeliveryQty + "，請檢查SpecialOrder, Delivery資料!!" + "<br/>";
                                            msg = msg + "<br/>" + localmsg;

                                            logger.DebugFormat("Qty is not same {0}", localmsg);

                                            #endregion
                                            continue;
                                        }

                                        if (!demianOrder.Status.Equals("Active") && !demianOrder.Status.Equals("Created"))
                                        {
                                            #region send  email Delivery Qty is not equal SpecialOrder Qty
                                            localmsg = "<br/>" + "CNRS ProductID:" + snoId +
                                                                             " CustomSN:" + productInfo.CUSTSN +
                                                                              " MAC:" + productInfo.MAC +
                                                                             " Model:" + productInfo.Model +
                                                                             "FactoryPO:" + demianOrder.FactoryPO +
                                                                             " SpecialOrder Status: " + demianOrder.Status + "不是Active or Created Status，請檢查SpecialOrder 資料!!" + "<br/>";
                                            msg = msg + "<br/>" + localmsg;

                                            logger.DebugFormat("Wrong SpecialOrder Status {0}", localmsg);


                                            #endregion
                                            continue;
                                        }
                                        string assetTag = demianOrder.AssetTag;

                                        #region 若 SpecialOrder Qty>1 or AssetTag="" 則分配重AssetRange 分配Asset Tag
                                        if (demianOrder.Qty > 1 || string.IsNullOrEmpty(demianOrder.AssetTag))
                                        {
                                            IOnlineGenerateAST onlineAst = ServiceAgent.getInstance().GetObjectByName<IOnlineGenerateAST>("IMESService.OnlineGenerateAST");
                                            if (onlineAst == null)
                                            {
                                                throw new Exception("Remoting call FA  Generate AST Number Service Fail, Please check FA Remoting Port and IP setting value in app.config file !!!");
                                            }
                                            assetTag = onlineAst.GenAstNumberByAstHPPo(snoId, demianOrder.FactoryPO, "CNRS", "CNRS", "CNRS", "HP");
                                            if (string.IsNullOrEmpty(assetTag))
                                            {
                                                throw new Exception("Generate AST Number is null !!!");
                                            }
                                        }
                                        #endregion

                                        SQLStatement.InsertCNRSPoMo(dbconnect, poInfo);
                                        int assignQty = poInfo.DeliveryQty - poInfo.RemainQty;
                                        int specialRemainQty = demianOrder.Qty - assignQty;
                                        if (specialRemainQty == 1)
                                        {
                                            SQLStatement.UpdateSpecialOrderStatus(dbconnect, poInfo.PO, "Closed");
                                        }
                                        else if (demianOrder.Status.Equals("Created"))
                                        {
                                            SQLStatement.UpdateSpecialOrderStatus(dbconnect, poInfo.PO, "Active");
                                        }

                                        SQLStatement.WriteCDSIAST(dbconnect, poInfo.ProductID, "ASSET_TAG", assetTag);

                                        SQLStatement.WriteCDSIAST(dbconnect, poInfo.ProductID, "HPOrder", poInfo.CustPo);
                                        SQLStatement.WriteCDSIAST(dbconnect, poInfo.ProductID, "FactoryPO", poInfo.HpPo);
                                        //SQLStatement.WriteCDSIAST(dbconnect, log, poInfo.ProductID, "FactoryPO", demianOrder.FactoryPO);

                                        SQLStatement.WriteProductAttr(dbconnect, poInfo.ProductID, "CNRSState", "OK", "CNRS", DateTime.Now);
                                        //productInfo.PO = po;
                                        //AssignedPOList.Add(productInfo);
                                    }
                                    else
                                    {
                                        #region send  email not found SpecialPO need upload PO data
                                        localmsg = "<br/>" + "CNRS ProductID:" + snoId +
                                                                         " CustomSN:" + productInfo.CUSTSN +
                                                                          " MAC:" + productInfo.MAC +
                                                                         " Model:" + productInfo.Model +
                                                                         " 找不到SpecialOrder 資料，請上傳FactoryPO資料!!" + "<br/>";
                                        msg = msg + "<br/>" + localmsg;

                                        logger.DebugFormat("Not Special order {0}", localmsg);


                                        #endregion
                                        continue;
                                    }
                                    #endregion
                                }                                
                            }
                            else
                            {
                                #region send  email not found PO need upload PO data
                                localmsg = "<br/>" + "CNRS ProductID:" + snoId +
                                                                 " CustomSN:" + productInfo.CUSTSN +
                                                                  " MAC:" + productInfo.MAC +
                                                                 " Model:" + productInfo.Model +
                                                                 " 找不到船務的HP PO 對應SpecialOrder FactoryPO 資料，請上傳船務資料 或是上傳FactoryPO資料!!" + "<br/>";
                                msg = msg + "<br/>" + localmsg;

                                logger.DebugFormat("Not Found DN {0}", localmsg);
                                #endregion
                                continue;
                            }
                            #endregion

                            #endregion
                        }
                        else //Manual assign PO
                        {
                            #region send  email Not Allow Manual Assigned PO
                            localmsg = "<br/>" + "CNRS ProductID:" + snoId +
                                                             " CustomSN:" + productInfo.CUSTSN +
                                                              " MAC:" + productInfo.MAC +
                                                             " Model:" + productInfo.Model +
                                                             "FactoryPO:" + po +
                                                             " 不允許收動分配PO，請檢查SnoDet_PoMo資料!!" + "<br/>";
                            msg = msg + "<br/>" + localmsg;

                            logger.DebugFormat("Not Allow Manual Assigned PO {0}", localmsg);
                         

                            #endregion
                            continue;
                        }
                        #endregion
                    }
                    #endregion
                    Thread.Sleep(config.AssignedPOCmdInterval);

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
