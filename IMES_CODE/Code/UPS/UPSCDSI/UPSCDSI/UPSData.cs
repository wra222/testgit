using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using UPS.CDSI.UPSWS;
using UTL.SQL;
using UPS.UTL.SQL;
using UTL;
using UTL.MetaData;
using System.IO;
using System.Security.Principal;
using UTL.Account;
using System.Data.SqlClient;
using System.Xml;
using UTL.IO;

namespace UPS.CDSI
{
    public class UPSData
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ATMStruct GetATM(string sn, string avPartNo, string hpPo)
        {
            
            ATRPSoapClient soapClient = new ATRPSoapClient("UPS");
            return soapClient.UPSGetATM(sn, avPartNo, avPartNo, hpPo, null);

        }

        private static USIStruct GetUSI(string sn,  string avPartNo, string hppo,string hpSKU)
        {
            ATRPSoapClient soapClient = new ATRPSoapClient("UPS");
            return soapClient.UPSGetUSI(sn, hppo, avPartNo, hpSKU);
        }
        
        public static bool DecideUPSCDSI(UPSDatabase db, ProductInfo productInfo,CDSIPO poInfo, out UPSCombinePO combinePo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
			logger.DebugFormat("BEGIN: {0}()", methodName);
			try	 
            {
                combinePo = null;
                
                if (db.UPSModelEntity.Any(x => x.Model== productInfo.Model && 
                                                                x.Status==EnumUPSModelStatus.Enable.ToString()))
                {
                    combinePo = db.UPSCombinePOEntity.Where(x => x.Model == productInfo.Model &&
                                                                             x.IECPO == poInfo.PO &&
                                                                           (x.Status == EnumUPSCombinePOStatus.Free.ToString() ||
                                                                            x.Status == EnumUPSCombinePOStatus.Release.ToString()))
                                                                            .OrderBy(x=>x.ID).FirstOrDefault();
                    return true;

                }
                else
                {

                    return false;
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

        public static bool AssignUPSPO(AppConfig config, 
                                                        UPSDatabase db, 
                                                        ProductInfo productInfo,
                                                        CDSIPO poInfo,    
                                                        UPSCombinePO combinePo,
                                                        out string errorText)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                errorText = string.Empty;
                //ATMStruct ret = GetATM(productInfo.CUSTSN, productInfo.ATSNAV, combinePo.HPPO);
                //if (ret.retcode < 0 || string.IsNullOrEmpty(ret.assetTagNum))
                //{
                //    errorText = string.Format(" UPSATM Return:{0} Asset Tag:{1} Message:{2}", ret.retcode.ToString(),ret.assetTagNum??"", ret.message);
                //    logger.ErrorFormat(errorText);
                //    return false;
                //}
                //else
                //{
                //    logger.InfoFormat(" UPSATM Return:{0} Message:{1} Asset Tag:{2}", ret.retcode.ToString(), ret.message, ret.assetTagNum);
                //}
               
                var hppoData=db.UPSHPPOEntity.Where(x=>x.HPPO== combinePo.HPPO).FirstOrDefault();
                string hpSku =hppoData==null?null: hppoData.HPSKU;
                USIStruct returnUSI= GetUSI(productInfo.CUSTSN, productInfo.ATSNAV, combinePo.HPPO,hpSku);
                if (returnUSI.retcode < 0 || string.IsNullOrEmpty(returnUSI.unattend))
                {
                    errorText = string.Format(" UPSUSI Return:{0}  unattend:{1} Message:{2}", returnUSI.retcode.ToString(),returnUSI.unattend ??"", returnUSI.message);
                    logger.ErrorFormat(errorText);                    
                    return false;
                }
                else
                {
                    logger.InfoFormat(" UPSUSI Return:{0} Message:{1} unattend:{2}", returnUSI.retcode.ToString(), returnUSI.message, returnUSI.unattend);
                }

                string unattend = returnUSI.unattend;
                string astNum = null;
                XmlDocument doc =  new XmlDocument();
                doc.LoadXml(unattend);
                XmlNodeList assetTagNodList = doc.GetElementsByTagName("ComputerName");
                XmlNodeList userNodeList = doc.GetElementsByTagName("Credentials");
                XmlNodeList admNodeList = doc.GetElementsByTagName("AdministratorPassword");
                foreach (XmlNode node in assetTagNodList)
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                    {
                        astNum = node.InnerText;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(astNum))
                {
                    errorText = string.Format("Asset Tag is empty!");
                    logger.ErrorFormat(errorText);
                    return false;
                }
               
                copyUnattend2ImgSrv(config, unattend, productInfo);

                #region write db data
                DateTime now = DateTime.Now;     
               
                if (db.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Connection.Open();
                }
                db.Transaction = db.Connection.BeginTransaction();
                //SqlTransaction dbTxn= dbconnect.BeginTransaction();
                //db.Transaction = dbTxn;
                SqlConnection dbconnect = (SqlConnection)db.Connection;

                SqlTransaction transaction = (SqlTransaction)db.Transaction;
                //PoMO
                SQLStatement.InsertCDSIPoMo(dbconnect, transaction, poInfo);

                //CSIAST table
                SQLStatement.DeleteCDSIAST(dbconnect, transaction, productInfo.ProductID);
                SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, "DID", "");
                SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, "ASSET_TAG", astNum);
                SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, "HPOrder", combinePo.HPPO);
                SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, "PurchaseOrder", hppoData.EndCustomerPO ?? "");
                SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, "FactoryPO", combinePo.IECPO);
                SQLStatement.WriteProductAttr(dbconnect, transaction, productInfo.ProductID, "CDSIState", "OK", "CDSI", now);

                foreach (XmlNode node in userNodeList)
                {
                    foreach (XmlNode cnode in node.ChildNodes)
                    {
                        SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, cnode.Name, cnode.InnerText);                      
                    }          
                }

                foreach (XmlNode node in admNodeList)
                {
                    foreach (XmlNode cnode in node.ChildNodes)
                    {
                        if (cnode.Name.ToLower() == "value")
                        {
                            SQLStatement.WriteCDSIAST(dbconnect, transaction, productInfo.ProductID, "AdministratorPassword", cnode.InnerText);
                        }
                    }
                }


                //Update UPSCombinePO                        
                combinePo.ProductID = productInfo.ProductID;
                combinePo.CUSTSN = productInfo.CUSTSN;
                combinePo.Status = EnumUPSCombinePOStatus.Used.ToString();
                combinePo.IsShipPO = "CDSI";
                combinePo.Udt = now;               
                combinePo.Editor = "UPS.CDSI";
                db.SubmitChanges();

                db.Transaction.Commit();
                db.Connection.Close(); 
               
                #endregion
                return true;
            }
            catch (Exception e)
            {
                errorText = e.Message;
                logger.Error(e.Message, e);
                return false;
            }
            finally
            {
               if (db.Connection.State == System.Data.ConnectionState.Open)
                {
                    db.Connection.Close();
                }
                logger.DebugFormat("END: {0}()", methodName);
            }
           

        }

        private static void copyUnattend2ImgSrv(AppConfig config, string unattend, ProductInfo productInfo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
			logger.DebugFormat("BEGIN: {0}()", methodName);
            WindowsImpersonationContext wicCdsi = null;

			try	 
            {

                if (!string.IsNullOrEmpty(config.IMGServerUser))
                {
                    wicCdsi = Logon.ImpersinateUser(config.IMGServerUser,
                                                                                  config.IMGDomain,
                                                                                  config.IMGServerPassword);
                }
                string srcPath = config.IMGFolder + productInfo.CUSTSN + "-" + productInfo.MAC + "\\";
                string destPathFile = srcPath + "unattend.xml";
                FileOperation.CreateDirectory(srcPath);
                logger.InfoFormat(" Copy imgServer File : {0}", destPathFile);
                File.WriteAllText(destPathFile, unattend);
             }
            catch (Exception e)
            {

                logger.Error(e.Message, e);
				throw;					
            }
			finally
			{
                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);

                }
				logger.DebugFormat("END: {0}()", methodName);
			}
           

        }

    }

    public enum EnumUPSCombinePOStatus{
        Free=1,
        Used,
        Release
    }

    public enum EnumUPSModelStatus
    {
        Enable=1,
        Disable
    }
}
