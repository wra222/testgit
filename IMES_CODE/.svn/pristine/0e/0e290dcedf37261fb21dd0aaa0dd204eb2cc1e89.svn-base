using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using IMES.Query.DB;
using System.Configuration;
using IMES.Service.Common;

namespace IMES.SAP.Implementation
{
    public class SAPWeight

    {
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void ProcessDeliveryWeight(List<SAPWeightDef> defList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            //BaseLog.LoggingBegin(logger, methodName);
            try
            {
                foreach (SAPWeightDef item in defList)
                {
                    List<SAPDeliveryWeight> weights= SQL.GetSAPDeliveryWeight(item.ConnectionStr, item.DBName);
                    //Send data to SAP by web service
                    if (weights.Count>0) SendSAPDeliveryWeight(item, weights);
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);            
            }
            finally
            {

            }
        }

        private static void SendSAPDeliveryWeight(SAPWeightDef def,
                                                                       List<SAPDeliveryWeight> weightList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                SAPDeliveryWeightWS.ZWS_DELIVERY_WEIGHT_UPLOADClient SAPWeightClient = new SAPDeliveryWeightWS.ZWS_DELIVERY_WEIGHT_UPLOADClient("SAPDeliveryWeight");


                SAPWeightClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPWeightClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();


                List<SAPDeliveryWeightWS.ZwsDeliveryWeightLine> weightlist = new List<SAPDeliveryWeightWS.ZwsDeliveryWeightLine>();

                //string IDList=""; 
                string TxnId = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now);
                foreach (SAPDeliveryWeight item in weightList)
                {
                    SAPDeliveryWeightWS.ZwsDeliveryWeightLine item1 = new SAPDeliveryWeightWS.ZwsDeliveryWeightLine();
                    item1.Serialnumber = (item.ID.ToString().Trim() + "-" + TxnId).PadRight(25).Substring(0, 25);
                    item1.Plant = def.PlantCode;
                    item1.Id = item.Shippment;
                    item1.Type = item.Type;
                    item1.Grossweight = item.Weight;
                    item1.Unit = def.WeightUnit;
                    item1.Remark1 = "";
                    weightlist.Add(item1);
                    SQL.InsertSendData("SendDeliveryWeight",
                                                        item.ID.ToString().Trim(),
                                                       def.PlantCode,
                                                        item1.Serialnumber.Trim(),
                                                        item.Shippment + ";" + item.Type + ";" + item.Weight + ";" + def.WeightUnit + ";" + def.DBName,
                                                       IMES.Service.Common.EnumMsgState.Sending,
                                                                DateTime.Now);
                }

                SAPDeliveryWeightWS.ZwsDeliveryWeightOutLine[] weightOutList = SAPWeightClient.ZwsDeliveryWeightUpload(weightlist.ToArray());
                List<string> IdOKList = new List<string>();
                List<string> IdFailList = new List<string>();

                foreach (SAPDeliveryWeightWS.ZwsDeliveryWeightOutLine item in weightOutList)
                {
                    string dataId = (item.Serialnumber.Split(new char[] { '-' }))[0].Trim();
                    if (item.Result == "I" || item.Result == "U")
                    {
                        IdOKList.Add(dataId);
                        SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                "ReceiveDeliveryWeight",
                                                                dataId,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Message,
                                                                EnumMsgState.Success,
                                                                item.Id + ";" + item.Type);
                    }
                    else
                    {
                        IdFailList.Add(dataId);
                        SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                "ReceiveDeliveryWeight",
                                                                dataId,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Message,
                                                                IMES.Service.Common.EnumMsgState.Fail,
                                                                item.Id + ";" + item.Type);
                    }
                }

                //Update FISTOSAP_WEIGHT status
                if (IdOKList.Count > 0) SQL.UpdateSAPWeightStatus(def.ConnectionStr, def.DBName, IdOKList, "OK");
                if (IdFailList.Count > 0) SQL.UpdateSAPWeightStatus(def.ConnectionStr, def.DBName, IdFailList, "Fail");

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }


         public static void ProcessPalletWeight(List<SAPWeightDef> defList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            //BaseLog.LoggingBegin(logger, methodName);
            try
            {
                foreach (SAPWeightDef item in defList)
                {
                    List<SAPPalletWeight> weights= SQL.GetSAPPalletWeight(item.ConnectionStr, item.DBName);
                    //Send data to SAP by web service
                   if (weights.Count>0) SendSAPPalletWeight(item, weights);
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);            
            }
            finally
            {

            }
        }

     
       private static void SendSAPPalletWeight(SAPWeightDef def,
                                                                       List<SAPPalletWeight> weightList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                SAPPalletWeightWS.ZWS_PALLET_WEIGHT_UPLOADClient SAPWeightClient = new SAPPalletWeightWS.ZWS_PALLET_WEIGHT_UPLOADClient("SAPPalletWeight");


                SAPWeightClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPWeightClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();


                List<SAPPalletWeightWS.ZwsPalletWeightInLine> weightlist = new List<SAPPalletWeightWS.ZwsPalletWeightInLine>();

                //string IDList=""; 
                string TxnId = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now);
                foreach (SAPPalletWeight item in weightList)
                {
                    SAPPalletWeightWS.ZwsPalletWeightInLine item1 = new SAPPalletWeightWS.ZwsPalletWeightInLine();
                    item1.Serialnumber =  TxnId.PadRight(25);
                    item1.Plant = def.PlantCode;
                    item1.Id = item.ID;
                    item1.Type = item.Type;
                    item1.Palletid = item.PalletNo;
                    item1.Grossweight = item.Weight;
                    item1.Weightunit =def.WeightUnit;
                    item1.Length =item.Length;
                    item1.Width = item.Width;
                    item1.High =item.Height;
                    item1.Volunit = def.VolumnUnit;
                    item1.Remark1 = "";                   
                    weightlist.Add(item1);
                    SQL.InsertSendData("SendPalletWeight",
                                                        item.PalletNo,
                                                       def.PlantCode,
                                                        item1.Serialnumber.Trim(),
                                                        item.ID + ";" + item.Type + ";" + item.Weight + ";" + def.WeightUnit + ";" + item.Length + ";" + item.Width + ";" + item.Height + ";" + def.VolumnUnit + ";" + def.DBName,
                                                        IMES.Service.Common.EnumMsgState.Sending,
                                                        DateTime.Now);
                }
                
                SAPPalletWeightWS.ZwsPalletWeightOutLine[] weightOutList=  SAPWeightClient.ZwsPalletWeightUpload(weightlist.ToArray());
                List<string> IdOKList = new List<string>();
                List<string> IdFailList = new List<string>();
 
                foreach (SAPPalletWeightWS.ZwsPalletWeightOutLine item in weightOutList)
                {
                    string dataId = (item.Serialnumber.Split(new char[] { '-' }))[0].Trim();
                    if (item.Result == "I" || item.Result == "U")
                    {
                           SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                "ReceivePalletWeight",
                                                                item.Palletid.Trim(),
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Message,
                                                                EnumMsgState.Success,
                                                                item.Id.Trim() + ";" + item.Type.Trim() + ";" + item.Palletid.Trim() + ";" + def.DBName);

                        SQL.UpdateSAPPalletStatus(def.ConnectionStr, def.DBName, 
                                                                        item.Palletid.Trim(), item.Id.Trim(), 
                                                                        item.Type.Trim(), "OK");


                    }
                    else
                    {
                        IdFailList.Add(dataId);
                        SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                                "ReceivePalletWeight",
                                                                item.Palletid.Trim(),
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Message,
                                                                IMES.Service.Common.EnumMsgState.Fail ,
                                                                item.Id.Trim() + ";" + item.Type.Trim() + ";" + item.Palletid.Trim() + ";" + def.DBName);
                         SQL.UpdateSAPPalletStatus(def.ConnectionStr, def.DBName, 
                                                                        item.Palletid.Trim(), item.Id.Trim(), 
                                                                        item.Type.Trim(), "FAIL");
                    }
                }               
                
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);               
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }


       public static void ProcessMasterWeight(List<SAPWeightDef> defList)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;

           //BaseLog.LoggingBegin(logger, methodName);
           try
           {
               foreach (SAPWeightDef item in defList)
               {
                   List<SAPMasterWeight> weights = SQL.GetSAPMasterWeight(item.ConnectionStr, item.DBName);
                   //Send data to SAP by web service
                   if (weights.Count > 0) SendSAPMasterWeight(item, weights);
               }
           }
           catch (Exception e)
           {
               BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
           }
           finally
           {

           }
       }

       private static void SendSAPMasterWeight(SAPWeightDef def,
                                                                 List<SAPMasterWeight> weightList)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;

           BaseLog.LoggingBegin(logger, methodName);
           try
           {
               SAPMasterWeightWS.ZWS_MASTER_WEIGHT_UPLOADClient SAPWeightClient = new SAPMasterWeightWS.ZWS_MASTER_WEIGHT_UPLOADClient("SAPMasterWeight");

               SAPWeightClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
               SAPWeightClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();

               List<SAPMasterWeightWS.ZwsMasterWeightInLine> weightlist = new List<SAPMasterWeightWS.ZwsMasterWeightInLine>();

               //string IDList=""; 
               string TxnId = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now);
               foreach (SAPMasterWeight item in weightList)
               {
                   SAPMasterWeightWS.ZwsMasterWeightInLine item1 = new SAPMasterWeightWS.ZwsMasterWeightInLine();
                   item1.Serialnumber = (item.ID.ToString().Trim() + "-" + TxnId).PadRight(33).Substring(0, 33);
                   //item1.Serialnumber = TxnId.PadRight(25);
                   item1.Plant = def.PlantCode;
                   item1.Id = item.ID;
                   item1.Grossweight = item.GrossWeight;
                   item1.Netweight = item.NetWeight;
                   item1.Unit = def.WeightUnit;
                   item1.Remark1 = "";
                   weightlist.Add(item1);
                   SQL.InsertSendData("SendMasterWeight",
                                                       item.ID,
                                                       def.PlantCode,
                                                       item1.Serialnumber.Trim(),
                                                       item.ID + ";" + item.GrossWeight + ";" + item.NetWeight + ";" + def.WeightUnit + ";" + def.DBName,
                                                       IMES.Service.Common.EnumMsgState.Sending,
                                                       DateTime.Now);
               }
               logger.DebugFormat("*** Send MasterWeight SAP ~ Start ~***");
               SAPMasterWeightWS.ZwsMasterWeightOutLine[] weightOutList = SAPWeightClient.ZwsMasterWeightUpload(weightlist.ToArray());
               logger.DebugFormat("*** Send MasterWeight SAP ~ End ~***");
               
               List<string> IdOKList = new List<string>();
               List<string> IdFailList = new List<string>();

               foreach (SAPMasterWeightWS.ZwsMasterWeightOutLine item in weightOutList)
               {
                   logger.DebugFormat("SAP Return Result :[" + item.Result+ "]");
                   logger.DebugFormat("SAP Return Message :[" + item.Message + "]");

                   string dataId = (item.Serialnumber.Split(new char[] { '-' }))[0].Trim();
                   if (item.Result == "S")
                   {
                       //IdOKList.Add(dataId);
                       SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                            "ReceiveMasterWeight",
                                                            item.Id.Trim(),
                                                            item.Plant.Trim(),
                                                            item.Serialnumber.Trim(),
                                                            item.Result,
                                                            item.Message,
                                                            EnumMsgState.Success,
                                                            item.Id.Trim() + ";" + item.Id.Trim() + ";" + def.DBName);

                       SQL.UpdateSAPMasterWeightStatus(def.ConnectionStr, def.DBName, item.Id.Trim(), "OK");
                   }
                   else
                   {
                       //IdFailList.Add(dataId);
                       SQL.InsertTxnDataLog(EnumMsgCategory.Receive,
                                                               "ReceiveMasterWeight",
                                                               item.Id.Trim(),
                                                               item.Plant.Trim(),
                                                               item.Serialnumber.Trim(),
                                                               item.Result,
                                                               item.Message,
                                                               IMES.Service.Common.EnumMsgState.Fail,
                                                               item.Id.Trim() + ";" + item.Id.Trim() + ";" + def.DBName);

                       SQL.UpdateSAPMasterWeightStatus(def.ConnectionStr, def.DBName, item.Id.Trim(), "Fail");

                   }
               }
                //Update FISTOSAP_WEIGHT status
                //if (IdOKList.Count > 0) SQL.UpdateSAPMasterWeightStatus(def.ConnectionStr, def.DBName, IdOKList, "OK");
                //if (IdFailList.Count > 0) SQL.UpdateSAPMasterWeightStatus(def.ConnectionStr, def.DBName, IdFailList, "Fail");

           }
           catch (Exception e)
           {
               BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
           }
           finally
           {
               BaseLog.LoggingEnd(logger, methodName);
           }
       }

        
    }
}
