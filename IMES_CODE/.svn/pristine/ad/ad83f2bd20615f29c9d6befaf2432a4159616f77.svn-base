using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using UPS.CNRS.UPSWS;
using UTL.SQL;
using UPS.UTL.SQL;
using UTL;
using UTL.MetaData;
using System.IO;
using System.Security.Principal;
using UTL.Account;
using System.Data.SqlClient;

namespace UPS.CNRS
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

       

        public static bool AssignCNRSPO(AppConfig config,
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
                //Get AVPart Number
                string hpPo = combinePo.HPPO;
                IList<string> avPartNoList = db.UPSPOAVPartEntity.Where(x => x.HPPO == hpPo &&  x.IECPartNo!=null && x.IECPartNo!="" )
                                                            .Select(x=>x.AVPartNo).Distinct().ToList();
                if (avPartNoList == null || avPartNoList.Count == 0)
                {
                    errorText = string.Format("HPPO: {0} Not Find AV PartNo in UPS System", hpPo);
                    return false;
                }

                productInfo.ATSNAV = avPartNoList[0];
                logger.InfoFormat("HPPO: {0}  AVPartNo:{1}", hpPo, avPartNoList[0]);

                ATMStruct ret = GetATM(productInfo.CUSTSN, productInfo.ATSNAV, combinePo.HPPO);
                if (ret.retcode < 0 || 
                    string.IsNullOrEmpty(ret.assetTagNum))
                {
                    errorText = string.Format("UPSATM Return:{0} AssetTag:{1}  Message:{2}", ret.retcode.ToString(),ret.assetTagNum??"", ret.message);
                    logger.ErrorFormat(errorText);
                    return false;
                }
                else
                {
                    logger.InfoFormat(" UPSATM Return:{0} Message:{1} Asset Tag:{2}", ret.retcode.ToString(), ret.message, ret.assetTagNum);
                }

                string astNum = ret.assetTagNum;    

                #region write db data
                DateTime now = DateTime.Now;
                SqlConnection dbconnect = (SqlConnection)db.Connection;
                if (dbconnect.State == System.Data.ConnectionState.Closed)
                {
                    dbconnect.Open();
                }
                SqlTransaction dbTxn = dbconnect.BeginTransaction();
                db.Transaction = dbTxn;

                //PoMO
                SQLStatement.InsertCNRSPoMo(dbconnect, dbTxn, poInfo);
               
                //int assignQty = poInfo.DeliveryQty - poInfo.RemainQty;
                //int specialRemainQty = demianOrder.Qty - assignQty;
                //if (specialRemainQty == 1)
                //{
                //    SQLStatement.UpdateSpecialOrderStatus(dbconnect, poInfo.PO, "Closed");
                //}
                //else if (demianOrder.Status.Equals("Created"))
                //{
                //    SQLStatement.UpdateSpecialOrderStatus(dbconnect, poInfo.PO, "Active");
                //}

                //CSIAST table
                SQLStatement.DeleteCDSIAST(dbconnect, dbTxn, productInfo.ProductID);
                SQLStatement.WriteCDSIAST(dbconnect, dbTxn, productInfo.ProductID, "DID", "");
                SQLStatement.WriteCDSIAST(dbconnect, dbTxn, productInfo.ProductID, "ASSET_TAG", astNum);
                SQLStatement.WriteCDSIAST(dbconnect, dbTxn, productInfo.ProductID, "HPOrder", combinePo.HPPO);
                SQLStatement.WriteCDSIAST(dbconnect, dbTxn, productInfo.ProductID, "PurchaseOrder", "");
                SQLStatement.WriteCDSIAST(dbconnect, dbTxn, productInfo.ProductID, "FactoryPO", combinePo.IECPO);
                SQLStatement.WriteProductAttr(dbconnect, dbTxn, productInfo.ProductID, "CNRSState", "OK", "CNRS", now); 
              
                //Update UPSCombinePO                        
                combinePo.ProductID = productInfo.ProductID;
                combinePo.CUSTSN = productInfo.CUSTSN;
                combinePo.Status = EnumUPSCombinePOStatus.Used.ToString();
                combinePo.IsShipPO = "CNRS";
                combinePo.Udt = now;
                combinePo.Editor = "UPS.CNRS";
                db.SubmitChanges();

                dbTxn.Commit();
                dbconnect.Close();
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
