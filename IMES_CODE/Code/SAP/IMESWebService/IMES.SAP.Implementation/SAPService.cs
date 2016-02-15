using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Data.Sql;
using log4net;
using IMES.Query.DB;
using IMES.SAP.Interface;
using IMES.WS.MORelease;
using IMES.WS.MaterialInfoNotice;
using IMES.WS.MOConfirm;
using IMES.WS.Common;
using System.Data;


namespace IMES.SAP.Implementation
{
    public class SAPService:MarshalByRefObject,ISAPService
    {
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region ISAPService Members

        
        public bool UpdateMO(string moId, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                // send MO to SAP
                //Query MO
                SAPWSQueryMO.Z_GET_PRODORD_TO_IMES_WSClient sapQueryMOClient = new SAPWSQueryMO.Z_GET_PRODORD_TO_IMES_WSClient("SAPQueryMO");

                sapQueryMOClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                sapQueryMOClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                SAPWSQueryMO.ZMO_IMES_H QueryHeader = new SAPWSQueryMO.ZMO_IMES_H();

                QueryHeader.MONUMBER = moId;
                QueryHeader.SERIALNUMBER = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                SAPWSQueryMO.ZE1RESBL[] items = new SAPWSQueryMO.ZE1RESBL[0];

                SAPWSQueryMO.ZE1AFKOL header = sapQueryMOClient.Z_GET_PRODORD_TO_IMES(QueryHeader, ref items);
                //regenerate WS moHeader  
                WSMORelease MoRelease = new WSMORelease();
                MoHeader moHeader = new MoHeader();
                List<MoItemDetail> moItems =new List<MoItemDetail>();
                #region map moHedaer
                moHeader.BasicFinishDate = header.BASICFINISHDATE;
                moHeader.BasicStartDate = header.BASICSTARTDATE;
                moHeader.BOMExplDate = header.BOMEXPLDATE;
                moHeader.BOMStatus = header.BOMSTATUS;
                moHeader.BuildOutMtl = header.BUILDOUTMTL;
                moHeader.CreateDate = header.CREATEDATE;
                moHeader.DeliveredQty = header.DELIVEREDQTY;
                moHeader.MaterialGroup = header.MATERIALGROUP;
                moHeader.MaterialType = header.MATERIALTYPE;
                moHeader.MoNumber = header.MONUMBER;
                moHeader.MoType = header.MOTYPE;
                moHeader.Plant = header.PLANT;
                moHeader.Priority = header.PRIORITY;
                moHeader.ProductionVer = header.PRODUCTIONVER;
                moHeader.Remark1 = header.REMARK1;
                moHeader.Remark2 = header.REMARK2;
                moHeader.Remark3 = header.REMARK3;
                moHeader.Remark4 = header.REMARK4;
                moHeader.Remark5 = header.REMARK5;
                moHeader.SalesOrder = header.SALESORDER;
                moHeader.SerialNumber = header.SERIALNUMBER;
                moHeader.SOItem = header.SOITEM;
                moHeader.Status = header.STATUS;
                moHeader.TCode = header.TCODE ;
                moHeader.TotalQty = header.TOTALQTY;
                moHeader.Unit = header.UNIT;
                   
                #endregion

                #region map moItem
                foreach (SAPWSQueryMO.ZE1RESBL item in items)
                {
                    MoItemDetail detail = new MoItemDetail();
                    detail.AltGroup = item.ALTGROUP;
                    detail.Bulk = item.BULK_IND;
                    detail.Component = item.COMPONENT;
                    detail.Delete = item.DELETE_IND;
                    detail.FinalIssue = item.FINAL_ISSUE;
                    detail.MN = item.MN;
                    detail.MoItem = item.MOITEM;
                    detail.MoNumber = item.MONUMBER;
                    detail.Pantom = item.PANTOM;
                    detail.ParentMaterial = item.PARENT_MATERIAL;
                    detail.ReqQty = item.REQQTY;
                    detail.Reservation = item.RESERVATION;
                    detail.ResvItem = item.RESVITEM;
                    detail.SerialNumber =item.SERIALNUMBER;
                    detail.SpecialStock = item.SPECIALSTOCK;
                    detail.Unit =item.UNIT;
                    detail.UnitReqQty = item.UNITREQQTY; 
                    detail.WithdrawQty = item.WITHDRAWQTY;
                    moItems.Add(detail);
                }
                #endregion

                MoReleaseResponse moReleaseResp=  MoRelease.MoRelease(moHeader, moItems.ToArray());
                if (moReleaseResp.Result == "T")
                {
                    errorText = "";
                    return true;
                }
                else
                {
                    errorText = moReleaseResp.FailDescr;
                    return false;
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public bool UpdateMOHeader(string moId, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                // send MO to SAP
                //Query MO
                SAPWSQueryMO.Z_GET_PRODORD_TO_IMES_WSClient sapQueryMOClient = new SAPWSQueryMO.Z_GET_PRODORD_TO_IMES_WSClient("SAPQueryMO");

                sapQueryMOClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                sapQueryMOClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                SAPWSQueryMO.ZMO_IMES_H QueryHeader = new SAPWSQueryMO.ZMO_IMES_H();

                QueryHeader.MONUMBER = moId;
                QueryHeader.SERIALNUMBER = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                SAPWSQueryMO.ZE1RESBL[] items = new SAPWSQueryMO.ZE1RESBL[0];

                SAPWSQueryMO.ZE1AFKOL header = sapQueryMOClient.Z_GET_PRODORD_TO_IMES(QueryHeader, ref items);
                //regenerate WS moHeader  
                WSMORelease MoRelease = new WSMORelease();
                MoHeader moHeader = new MoHeader();
                List<MoItemDetail> moItems = new List<MoItemDetail>();
                #region map moHedaer
                moHeader.BasicFinishDate = header.BASICFINISHDATE;
                moHeader.BasicStartDate = header.BASICSTARTDATE;
                moHeader.BOMExplDate = header.BOMEXPLDATE;
                moHeader.BOMStatus = header.BOMSTATUS;
                moHeader.BuildOutMtl = header.BUILDOUTMTL;
                moHeader.CreateDate = header.CREATEDATE;
                moHeader.DeliveredQty = header.DELIVEREDQTY;
                moHeader.MaterialGroup = header.MATERIALGROUP;
                moHeader.MaterialType = header.MATERIALTYPE;
                moHeader.MoNumber = header.MONUMBER;
                moHeader.MoType = header.MOTYPE;
                moHeader.Plant = header.PLANT;
                moHeader.Priority = header.PRIORITY;
                moHeader.ProductionVer = header.PRODUCTIONVER;
                moHeader.Remark1 = header.REMARK1;
                moHeader.Remark2 = header.REMARK2;
                moHeader.Remark3 = header.REMARK3;
                moHeader.Remark4 = header.REMARK4;
                moHeader.Remark5 = header.REMARK5;
                moHeader.SalesOrder = header.SALESORDER;
                moHeader.SerialNumber = header.SERIALNUMBER;
                moHeader.SOItem = header.SOITEM;
                moHeader.Status = header.STATUS;
                moHeader.TCode = header.TCODE;
                moHeader.TotalQty = header.TOTALQTY;
                moHeader.Unit = header.UNIT;

                #endregion

                #region map moItem
                //foreach (SAPWSQueryMO.ZE1RESBL item in items)
                //{
                //    MoItemDetail detail = new MoItemDetail();
                //    detail.AltGroup = item.ALTGROUP;
                //    detail.Bulk = item.BULK_IND;
                //    detail.Component = item.COMPONENT;
                //    detail.Delete = item.DELETE_IND;
                //    detail.FinalIssue = item.FINAL_ISSUE;
                //    detail.MN = item.MN;
                //    detail.MoItem = item.MOITEM;
                //    detail.MoNumber = item.MONUMBER;
                //    detail.Pantom = item.PANTOM;
                //    detail.ParentMaterial = item.PARENT_MATERIAL;
                //    detail.ReqQty = item.REQQTY;
                //    detail.Reservation = item.RESERVATION;
                //    detail.ResvItem = item.RESVITEM;
                //    detail.SerialNumber = item.SERIALNUMBER;
                //    detail.SpecialStock = item.SPECIALSTOCK;
                //    detail.Unit = item.UNIT;
                //    detail.UnitReqQty = item.UNITREQQTY;
                //    detail.WithdrawQty = item.WITHDRAWQTY;
                //    moItems.Add(detail);
                //}
                #endregion

                MoReleaseResponse moReleaseResp = MoRelease.MoRelease(moHeader, moItems.ToArray());
                if (moReleaseResp.Result == "T")
                {
                    errorText = "";
                    return true;
                }
                else
                {
                    errorText = moReleaseResp.FailDescr;
                    return false;
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public bool UpdateMaterial(string component, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                SAPWSQueryMaterial.Z_RFC_GET_MATMASClient sapQueryMaterialClient = new SAPWSQueryMaterial.Z_RFC_GET_MATMASClient("SAPQueryMaterial");

                sapQueryMaterialClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                sapQueryMaterialClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();
                SAPWSQueryMaterial.ZmatInfo materialInfo = sapQueryMaterialClient.ZRfcGetMatmas(component, ConfigurationManager.AppSettings["SAPPlant"].ToString());

                IMES.WS.MaterialInfoNotice.SQL.InsertSAPMaterialInfo(materialInfo.Material,
                                                                                                           materialInfo.Serilnumber,
                                                                                                           materialInfo.Plant,
                                                                                                           materialInfo.Materialgroup,
                                                                                                           materialInfo.Materialtype,
                                                                                                           materialInfo.Materialstatus,
                                                                                                           materialInfo.Oldmaterialnumber,
                                                                                                           materialInfo.Externalmaterialgroup,
                                                                                                           materialInfo.Materialdescription);
                WSMaterialInfoNotice wsMaterialInfo = new WSMaterialInfoNotice();
                MaterialInfoNoticeMsg materialMsg = new MaterialInfoNoticeMsg();
                materialMsg.SerialNumber =  materialInfo.Serilnumber;
                wsMaterialInfo.MaterialInfoNotice(materialMsg);               
                return true;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }


        public MaterialInfo QueryMaterialInfo(string component, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                SAPWSQueryMaterial.Z_RFC_GET_MATMASClient sapQueryMaterialClient = new SAPWSQueryMaterial.Z_RFC_GET_MATMASClient("SAPQueryMaterial");

                sapQueryMaterialClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                sapQueryMaterialClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();
                SAPWSQueryMaterial.ZmatInfo materialInfo = sapQueryMaterialClient.ZRfcGetMatmas(component, ConfigurationManager.AppSettings["SAPPlant"].ToString());
                MaterialInfo info = new MaterialInfo();


                info.Material=  materialInfo.Material;
                info.Serilnumber= materialInfo.Serilnumber;
                info.Plant=materialInfo.Plant;
                info.Materialgroup = materialInfo.Materialgroup;
                info.Materialtype = materialInfo.Materialtype;
                info.Materialstatus = materialInfo.Materialstatus;
                info.CustomerMaterial =materialInfo.Oldmaterialnumber;
                info.Cutsomer =     materialInfo.Externalmaterialgroup;
                info.Materialdescription = materialInfo.Materialdescription;
                
                return info;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return null;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public bool QuerySAPMOStatus(string moId, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                return SAPClient.SentChangeMOStatus(moId, out errorText);

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;

            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }

        //Change SAP MO Status to TECO
        public bool CloseSAPMO(string moId, string comment,  string editor, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                //Check MO Status is not Close or Release status
                DataTable  MoInfo = IMES.WS.MOConfirm.SQL.QueryMOStatus(moId);
                string status = "";
                int TransferQty = 0;     


                if (MoInfo.Rows.Count > 0)
                {
                    status = MoInfo.Rows[0]["Status"].ToString();
                    TransferQty = (int)MoInfo.Rows[0]["Transfer_Qty"];
                    if (status != "Run" &&
                       status != "Start")
                    {
                        errorText = "MO Status [" + status + "] does not allow close MO [" + moId + "]";
                        throw new Exception(errorText);
                    }
                }
                else
                {
                    errorText = "Can't find MO  [" + moId + "]";
                    throw new Exception(errorText);
                }

                //Change MOStatus to Close
                IMES.WS.MOConfirm.SQL.UpdateMOStatus(moId, "ChangeMOStatus", "CloseMO", "Close", comment, "", editor);
                if (SAPClient.SentChangeMOStatus(moId, SAPMOChangeStatusCmd.TECO,TransferQty, out errorText))
                {
                    return true;
                }
                else
                {
                    IMES.WS.MOConfirm.SQL.UpdateMOStatus(moId, "ChangeMOStatus", "RollbackCloseMO", status, errorText, "", editor);
                    return false;
                }

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;

            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
        public bool OpenSAPMO(string moId, string editor, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                return SAPClient.SentChangeMOStatus(moId, SAPMOChangeStatusCmd.REVOKETECO,0, out errorText);

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;

            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public bool ChangeIMESMOStatus(string moId, enumMOStatus status, string comment, string editor, out string errorText)
        {
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
              

                //Change MOStatus to Close
                IMES.WS.MOConfirm.SQL.UpdateMOStatus(moId, "ChangeIMESMOStatus", "ChangeStatus", status.ToString(), comment, "", editor);
                return true;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;

            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }

        public bool ConfirmMO(string isParent,out string errorText)
        {
            //isParent:="Y" ==>Parent MO
            //                 "N" ==>ChildMO

            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                IMES.Service.MO.Execute.Process("", isParent);
                return true;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
 
        }

        public bool AdjustConfirmMO(out string errorText)
        {
           
            errorText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                IMES.Service.MO.Execute.ProcessRewok();
                return true;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorText = e.Message;
                return false;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        //Phase I of SAP Check SN 
        //Cancel bind DN
        //Phase I of SAP Check SN 
        //Cancel bind DN
        public string CancelBindDN(string Plant, string DN, out string errorText)
        {
            BaseLog.LoggingInfo(logger, "Plant: \r{0}", Plant);
            BaseLog.LoggingInfo(logger, "DN: \r{0}", DN);

            string errorCode = "0";
            errorText = "";
            string connectionDB = "SD_DBServer";
            string connectionDB_BK = "SD_DBServer_BK";
            string methodName = MethodBase.GetCurrentMethod().Name;
            string Remark1 = "";
            string Remark2 = "";

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                SAPCancelBindDNWS.ZWS_CANCEL_SERIALClient SAPDNClient = new SAPCancelBindDNWS.ZWS_CANCEL_SERIALClient("SAPCancelBindDN");

                SAPDNClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPDNClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                List<SAPCancelBindDNWS.ZwsCancelSnInLine> SnInlist = new List<SAPCancelBindDNWS.ZwsCancelSnInLine>();

                string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                SQL.InsertCancelBindDNLog(connectionDB_BK, 0,
                                          TxnId, Plant, DN, Remark1, Remark2,
                                          "Receive", "Success", "", "FIS");

                SAPCancelBindDNWS.ZwsCancelSnInLine item1 = new SAPCancelBindDNWS.ZwsCancelSnInLine();
                item1.Serialnumber = TxnId;
                item1.Plant = Plant;
                item1.Id = DN;
                item1.Remark1 = Remark1;
                item1.Remark2 = Remark2;
                SnInlist.Add(item1);
                SQL.InsertSendData_DB(connectionDB, 0,
                                        "SendCancelBindDN",
                                        item1.Id.Trim(),
                                        item1.Plant.Trim(),
                                        item1.Serialnumber.Trim(),
                                        item1.Remark1 + "," + item1.Remark2,
                                        IMES.Service.Common.EnumMsgState.Sending,
                                        DateTime.Now);

                SAPCancelBindDNWS.ZwsCancelSnOutLine[] SnOutLine = SAPDNClient.ZwsCancelSerial(SnInlist.ToArray());

                //bool rt;
                //rt = true;
                foreach (SAPCancelBindDNWS.ZwsCancelSnOutLine item in SnOutLine)
                {
                    if (item.Result == "0")
                    {
                        SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                IMES.Service.Common.EnumMsgCategory.Receive,
                                                                "ReceiveCancelBindDN",
                                                                item.Id,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Errortext,
                                                                IMES.Service.Common.EnumMsgState.Success,
                                                                "");

                        SQL.InsertCancelBindDNLog(connectionDB_BK, 0,
                                                  item.Serialnumber.Trim(), item.Plant.Trim(), item.Id, "", "",
                                                  "Response", "Success", "", "SAP");
                        errorCode = item.Result;
                    }
                    else
                    {
                        SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                IMES.Service.Common.EnumMsgCategory.Receive,
                                                                "ReceiveCancelBindDN",
                                                                item.Id,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Errortext,
                                                                IMES.Service.Common.EnumMsgState.Fail,
                                                                "");

                        SQL.InsertCancelBindDNLog(connectionDB_BK, 0,
                                                  item.Serialnumber.Trim(), item.Plant.Trim(), item.Id, "", "",
                                                  "Response", "Fail", item.Errortext, "SAP");

                        errorCode = item.Result;
                    }

                    BaseLog.LoggingInfo(logger, "Serialnumber: \r{0}", item.Serialnumber);
                    BaseLog.LoggingInfo(logger, "DN: \r{0}", DN);
                    BaseLog.LoggingInfo(logger, "Plant: \r{0}", Plant);
                    BaseLog.LoggingInfo(logger, "Result: \r{0}", item.Result);
                    BaseLog.LoggingInfo(logger, "Errortext: \r{0}", item.Errortext);

                    errorText = item.Errortext;
                }
                return errorCode;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorCode = "9";
                errorText = e.Message;
                return errorCode;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public string CancelBindDNbyItem(string Plant, string DN, string dnItem, string dnType, out string errorText)
        {
            BaseLog.LoggingInfo(logger, "Plant: \r{0}", Plant);
            BaseLog.LoggingInfo(logger, "DN: \r{0}", DN);

            string errorCode = "0";
            errorText = "";
            string connectionDB = "SD_DBServer";
            string connectionDB_BK = "SD_DBServer_BK";
            string methodName = MethodBase.GetCurrentMethod().Name;
            string iItem = (dnItem == null) ? "" : dnItem;
            string iType = (dnType == null) ? "" : dnType;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                SAPCancelBindDNWS.ZWS_CANCEL_SERIALClient SAPDNClient = new SAPCancelBindDNWS.ZWS_CANCEL_SERIALClient("SAPCancelBindDN");

                SAPDNClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPDNClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();

                List<SAPCancelBindDNWS.ZwsCancelSnInLine> SnInlist = new List<SAPCancelBindDNWS.ZwsCancelSnInLine>();

                string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                SQL.InsertCancelBindDNLog(connectionDB_BK, 0,
                                          TxnId, Plant, DN, iItem, iType,
                                          "Receive", "Success", "", "FIS");

                SAPCancelBindDNWS.ZwsCancelSnInLine item1 = new SAPCancelBindDNWS.ZwsCancelSnInLine();
                item1.Serialnumber = TxnId;
                item1.Plant = Plant;
                item1.Id = DN;
                item1.Remark1 = iItem;
                item1.Remark2 = iType; //機型12碼，區分SKU or Docking
                SnInlist.Add(item1);
                SQL.InsertSendData_DB(connectionDB, 0,
                                        "SendCancelBindDN",
                                        item1.Id.Trim(),
                                        item1.Plant.Trim(),
                                        item1.Serialnumber.Trim(),
                                        item1.Remark1 + "," + item1.Remark2,
                                        IMES.Service.Common.EnumMsgState.Sending,
                                        DateTime.Now);

                SAPCancelBindDNWS.ZwsCancelSnOutLine[] SnOutLine = SAPDNClient.ZwsCancelSerial(SnInlist.ToArray());

                //bool rt;
                //rt = true;
                foreach (SAPCancelBindDNWS.ZwsCancelSnOutLine item in SnOutLine)
                {
                    if (item.Result == "0")
                    {
                        SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                IMES.Service.Common.EnumMsgCategory.Receive,
                                                                "ReceiveCancelBindDN",
                                                                item.Id,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Errortext,
                                                                IMES.Service.Common.EnumMsgState.Success,
                                                                "");

                        SQL.InsertCancelBindDNLog(connectionDB_BK, 0,
                                                  item.Serialnumber.Trim(), item.Plant.Trim(), item.Id, "", "",
                                                  "Response", "Success", "", "SAP");
                        errorCode = item.Result;
                    }
                    else
                    {
                        SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                IMES.Service.Common.EnumMsgCategory.Receive,
                                                                "ReceiveCancelBindDN",
                                                                item.Id,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Errortext,
                                                                IMES.Service.Common.EnumMsgState.Fail,
                                                                "");

                        SQL.InsertCancelBindDNLog(connectionDB_BK, 0,
                                                  item.Serialnumber.Trim(), item.Plant.Trim(), item.Id, "", "",
                                                  "Response", "Fail", item.Errortext, "SAP");

                        errorCode = item.Result;
                    }

                    BaseLog.LoggingInfo(logger, "Serialnumber: \r{0}", item.Serialnumber);
                    BaseLog.LoggingInfo(logger, "DN: \r{0}", DN);
                    BaseLog.LoggingInfo(logger, "Plant: \r{0}", Plant);
                    BaseLog.LoggingInfo(logger, "Result: \r{0}", item.Result);
                    BaseLog.LoggingInfo(logger, "Errortext: \r{0}", item.Errortext);

                    errorText = item.Errortext;
                }
                return errorCode;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorCode = "9";
                errorText = e.Message;
                return errorCode;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public string UploadMasterWeight(string Plant, string Model, string GrossWeight, string NetWeight, string Unit, out string errorText)
        {
            BaseLog.LoggingInfo(logger, "Plant: \r{0}", Plant);
            BaseLog.LoggingInfo(logger, "Model: \r{0}", Model);
            BaseLog.LoggingInfo(logger, "GrossWeight: \r{0}", GrossWeight);
            BaseLog.LoggingInfo(logger, "NetWeight: \r{0}", NetWeight);
            BaseLog.LoggingInfo(logger, "Unit: \r{0}", Unit);

            string errorCode = "0";
            errorText = "";
            string connectionDB = "SD_DBServer";
            string connectionDB_BK = "SD_DBServer_BK";
            string methodName = MethodBase.GetCurrentMethod().Name;
            string Remark1 = "";
            NetWeight = (NetWeight == null ? "" : NetWeight);
            NetWeight = (NetWeight == "0" ? "0.00" : NetWeight);

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                SAPMasterWeightWS.ZWS_MASTER_WEIGHT_UPLOADClient SAPMasterWeightClient = new SAPMasterWeightWS.ZWS_MASTER_WEIGHT_UPLOADClient("SAPMasterWeight");
                SAPMasterWeightClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                SAPMasterWeightClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPUserPwd"].ToString();
                
                List<SAPMasterWeightWS.ZwsMasterWeightInLine> MasterWeightInlist = new List<SAPMasterWeightWS.ZwsMasterWeightInLine>();

                string TxnId = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);

                SQL.InsertMasterWeightLog(connectionDB_BK, 0,
                                          TxnId, Plant, Model, GrossWeight, NetWeight, Unit, Remark1,
                                          "Receive", "Success", "", "FIS");

                SAPMasterWeightWS.ZwsMasterWeightInLine item1 = new SAPMasterWeightWS.ZwsMasterWeightInLine();
                item1.Serialnumber = Model.Trim()+"-"+TxnId;
                item1.Plant = Plant;
                item1.Id = Model;
                item1.Grossweight = GrossWeight;
                item1.Netweight = NetWeight;
                item1.Unit = Unit;
                item1.Remark1 = Remark1;
                MasterWeightInlist.Add(item1);
                SQL.InsertSendData_DB(connectionDB, 0,
                                        "SendMasterWeight",
                                        item1.Id.Trim(),
                                        item1.Plant.Trim(),
                                        item1.Serialnumber.Trim(),
                                        item1.Grossweight + "," + item1.Netweight + "," +
                                        item1.Unit + "," + item1.Remark1,
                                        IMES.Service.Common.EnumMsgState.Sending,
                                        DateTime.Now);
                SAPMasterWeightWS.ZwsMasterWeightOutLine[] MasterWeightOutLine = SAPMasterWeightClient.ZwsMasterWeightUpload(MasterWeightInlist.ToArray());
                
                //bool rt;
                //rt = true;
                foreach (SAPMasterWeightWS.ZwsMasterWeightOutLine item in MasterWeightOutLine)
                {
                    if (item.Result == "I" || item.Result == "U")
                    {
                        SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                IMES.Service.Common.EnumMsgCategory.Receive,
                                                                "ReceiveMasterWeight",
                                                                item.Id,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Message,
                                                                IMES.Service.Common.EnumMsgState.Success,
                                                                "");

                        SQL.InsertMasterWeightLog(connectionDB_BK, 0,
                                                  item.Serialnumber.Trim(), item.Plant.Trim(), item.Id, "", "", "", "",
                                                  "Response", "Success", "", "SAP");
                        errorCode = item.Result;
                    }
                    else
                    {
                        SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                                IMES.Service.Common.EnumMsgCategory.Receive,
                                                                "ReceiveMasterWeight",
                                                                item.Id,
                                                                item.Plant.Trim(),
                                                                item.Serialnumber.Trim(),
                                                                item.Result,
                                                                item.Message,
                                                                IMES.Service.Common.EnumMsgState.Fail,
                                                                "");

                        SQL.InsertMasterWeightLog(connectionDB_BK, 0,
                                                  item.Serialnumber.Trim(), item.Plant.Trim(), item.Id, "", "", "", "",
                                                  "Response", "Fail", item.Message, "SAP");

                        errorCode = item.Result;
                    }

                    BaseLog.LoggingInfo(logger, "Serialnumber: \r{0}", item.Serialnumber);
                    BaseLog.LoggingInfo(logger, "Model: \r{0}", Model);
                    BaseLog.LoggingInfo(logger, "Plant: \r{0}", Plant);
                    BaseLog.LoggingInfo(logger, "Result: \r{0}", item.Result);
                    BaseLog.LoggingInfo(logger, "Errortext: \r{0}", item.Message);

                    errorText = item.Message;
                }
                return errorCode;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                errorCode = "9";
                errorText = e.Message;
                return errorCode;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public ReplyUploadSAPSN UploadSapCustSN(IList<UploadSAPSN> uploadSAPSNList)
        {
            string connectionDB = "SD_DBServer";
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                var deliveryList=uploadSAPSNList.GroupBy(y=> new {DeliveryNo=y.DN+y.DLV_ITM}).Select(p=> new {Delivery=p.First(), Qty =p.Count()}).ToList();
                string txnId  =uploadSAPSNList[0].SERIALNUMBER;
                logger.InfoFormat("[Upload CustSN - Input] TxnId:{0}", txnId);

                foreach(var p in deliveryList)
                {
                    string deliveryNo = p.Delivery.DN + p.Delivery.DLV_ITM;
                    logger.InfoFormat("[Upload CustSN - Input] DeliveryNo:{0} Qty:{1}", deliveryNo, p.Qty.ToString());

                    SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                            IMES.Service.Common.EnumMsgCategory.Send,
                                                            "SendUpdateCustSN",
                                                            deliveryNo,
                                                            p.Qty.ToString(),
                                                            txnId.Trim(),
                                                            "",
                                                            "",
                                                            IMES.Service.Common.EnumMsgState.Success,
                                                            "");

                }
               
                ReplyUploadSAPSN ret = new ReplyUploadSAPSN() { HeaderList = new List<ReplyUploadSAPSNHeader>(), ItemList = new List<ReplyUploadSAPSNItem>() };
                //to do for send SAP Webservice: 
                #region 1.prepare web service structure
                SAPUploadSN.ZWS_CO_SN_UPLOADClient SAPClient = new SAPUploadSN.ZWS_CO_SN_UPLOADClient("SAPUploadSN");
                SAPClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["ECCUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["ECCUserPwd"].ToString();
                //Request format 

                SAPUploadSN.ZWS_SN_UPLOAD request = new IMES.SAP.Implementation.SAPUploadSN.ZWS_SN_UPLOAD();
                IList<SAPUploadSN.ZWS_ZSDSN_IN_LINE> input = new List<SAPUploadSN.ZWS_ZSDSN_IN_LINE>();
                foreach(UploadSAPSN sn in  uploadSAPSNList)
                {
                    input.Add(copyUploadSAPSN(sn));

                    logger.DebugFormat("\r\n[Upload CustSN - Input]==== \r\n{0}", ObjectTool.ObjectTostring(sn));
                    
                }
                request.IN = input.ToArray(); 
                #endregion

                #region 2.Send Data  and get reply data to iMES
                SAPUploadSN.ZWS_SN_UPLOADResponse resp=  SAPClient.ZWS_SN_UPLOAD(request);
                ReplyUploadSAPSN reply =new ReplyUploadSAPSN();
                foreach(var p in resp.OUT_HEADER)
                {
                    logger.InfoFormat("[Upload CustSN - Output] TxnId:{0} DeliveryNo:{1} Result:{2} Message:{3} ", p.SERIALNUMBER, p.DLV_NO+p.DLV_ITM, p.RESULT, p.MESSAGE);

                    SQL.InsertTxnDataLog_DB(connectionDB, 0,
                                                            IMES.Service.Common.EnumMsgCategory.Receive,
                                                            "ReceiveUploadCustSN",
                                                            p.DLV_NO + p.DLV_ITM,
                                                            "",
                                                            p.SERIALNUMBER,
                                                            p.RESULT,
                                                            p.MESSAGE,
                                                            IMES.Service.Common.EnumMsgState.Fail,
                                                            "");
                }
              
                foreach(var x in resp.OUT_ITEM)
                {
                    logger.InfoFormat("[Upload CustSN - Output] TxnId:{0} DeliveryNo:{1} SN:{2} Result:{3} Message:{4}  ", x.SERIALNUMBER, x.DLV_NO + x.DLV_ITM, x.SERNO, x.RESULT, x.MESSAGE);
                }

                return copyUploadSNReply(resp);
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


        public ResponseSAPPo GetSAPPOInfo(RequestSAPPo poInfo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat(@"Request PO Info TxnId:{0} 
                                                                          PoItem:{1}  
                                                                          PoNo:{2} 
                                                                          Type:{3} 
                                                                           Remark:{4}", 
                                                poInfo.TxnId, poInfo.PoItem,poInfo.PoNo,poInfo.Type, poInfo.Remark );
              
                SAPPO.ZWS_CO_POINFO_QUERYClient SAPClient = new SAPPO.ZWS_CO_POINFO_QUERYClient("SAPPOInfo");
                SAPClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["ECCUserName"].ToString();
                SAPClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["ECCUserPwd"].ToString();
                SAPPO.ZWS_POINFO_QUERY_LINE[] poItems = new SAPPO.ZWS_POINFO_QUERY_LINE[]
                {
                    new  SAPPO.ZWS_POINFO_QUERY_LINE{ ID=poInfo.PoNo, 
                                                                                        ITEM=poInfo.PoItem, 
                                                                                        REMARK=poInfo.Remark}  
                };

                SAPPO.ZWS_POINFO_QUERY request = new SAPPO.ZWS_POINFO_QUERY();

                request.I_SERIAL = poInfo.TxnId;
                request.FLAG = poInfo.Type;
                request.IN = poItems;
                SAPPO.ZWS_POINFO_QUERYResponse response = SAPClient.ZWS_POINFO_QUERY(request);
                logger.InfoFormat(@"Response Info TxnId:{0} 
                                                Result:{1}  
                                                ErroText:{2} 
                                                Data:{3}",
                                               response.O_SERIAL, response.RESULT, response.ERRORTEXT, response.DATA);
                return new ResponseSAPPo {
                                                            TxnId = poInfo.TxnId,
                                                           Result = response.RESULT,
                                                           Data = response.DATA, 
                                                           ErrorText = response.ERRORTEXT };

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return new ResponseSAPPo
                {
                    TxnId = poInfo.TxnId, 
                                                            Data = string.Empty, 
                                                            ErrorText = e.Message, 
                                                            Result = "F" };
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
            

        }
        #endregion

        #region private function
        SAPUploadSN.ZWS_ZSDSN_IN_LINE copyUploadSAPSN(UploadSAPSN sn)
        {
            SAPUploadSN.ZWS_ZSDSN_IN_LINE ret = new IMES.SAP.Implementation.SAPUploadSN.ZWS_ZSDSN_IN_LINE()
            {
                 SERIALNUMBER =sn.SERIALNUMBER,
                  ACNUM=sn.ACNUM,
                  ASSET_NO=sn.ASSET_NO,
                  BMAC=sn.BMAC,
                  BOX_ID=sn.BOX_ID,
                  BUILDER_VER=sn.BUILDER_VER,
                  CARTON_NO =sn.CARTON_NO,
                  CUST_PALLET=sn.CUST_PALLET,
                  DLV_ITM=sn.DLV_ITM,
                  DLV_NO=sn.DN,
                  ERAC=sn.ERAC,
                  FCCID=sn.FCCID,
                  FDA=sn.FDA,
                  HEX_MEID= sn.HEX_MEID,
                   ICCID=sn.ICCID,
                   IMEI_CODE =sn.IMEI_CODE,
                   MACADDRESS=sn.MACADDRESS,
                   MACADDRESS2= sn.MACADDRESS2,
                   MANUFACTURE_DATE =sn.MANUFACTURE_DATE,
                    ODMAT=sn.ODMAT,
                   PALLET_NO=sn.PALLET_NO,
                    PALLET_ORDINAL =sn.PALLET_ORDINAL,
                     PBOX_ID= sn.PBOX_ID,
                     PO_NO=sn.PO_NO,
                      QTY =sn.QTY,
                       SERNO=sn.SERNO,
                       SERNO2=sn.SERNO2,
                       SIMLOCK_CODE=sn.SIMLOCK_CODE,
                       SOFTWARE_PN=sn.SOFTWARE_PN,
                       SOFTWARE_VER=sn.SOFTWARE_VER,
                       TYPE=sn.TYPE,
                       UUID=sn.UUID,
                       WARRANTY_DT=sn.WARRANTY_DT,
                       WARRANTY_ID=sn.WARRANTY_ID,
                       WLMAT=sn.WLMAT,
                       WMAC=sn.WMAC
            };
            return ret;
        }
        ReplyUploadSAPSN copyUploadSNReply(SAPUploadSN.ZWS_SN_UPLOADResponse resp)
        {
            ReplyUploadSAPSN ret = new ReplyUploadSAPSN();
            ret.HeaderList = new List<ReplyUploadSAPSNHeader>();
            ret.ItemList = new List<ReplyUploadSAPSNItem>();

            foreach (SAPUploadSN.ZWS_ZSDSN_OUT_LINE line in resp.OUT_HEADER)
            {
                ret.HeaderList.Add(new ReplyUploadSAPSNHeader()
                {
                     DLV_ITM=line.DLV_ITM,
                     DLV_NO =line.DLV_NO,
                     MESSAGE = line.MESSAGE,
                     RESULT = line.RESULT,
                     SERIALNUMBER = line.SERIALNUMBER                        
                });   
            }

            foreach(SAPUploadSN.ZWS_ZSDSN_OUT_ITEM_LINE item in resp.OUT_ITEM)
            {
                  ret.ItemList.Add(new ReplyUploadSAPSNItem()
                {
                     DLV_ITM = item.DLV_ITM,
                     DLV_NO =item.DLV_NO,
                     MESSAGE = item.MESSAGE,
                     RESULT = item.RESULT,
                     SERIALNUMBER= item.SERIALNUMBER,
                     SERNO=item.SERNO
                });
            }

            return ret;
        }

        #endregion
    }
}
