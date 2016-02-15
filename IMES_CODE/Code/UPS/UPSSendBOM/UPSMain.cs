﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using UTL.Config;
using System.Reflection;
using UTL.SQL;
using UPS.UPSATRP;
using UPS.UPSPrimaryService;
using System.Threading;
using UTL.Mail;
using UTL.MetaData;
using UPS.UTL.SQL;
using UTL;

namespace UPS
{
    class UPSMain
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<string, BUItem> items = new Dictionary<string, BUItem>();

        private static AppConfig config = new AppConfig();        

        static void Main(string[] args)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("*************************************************************************************************************");
            logger.InfoFormat("BEGIN: {0}()", methodName);

            try
            {
                if (!ExecShell.CheckOneProcessRun())
                {
                    return;
                }
               
                 IList<SendBOMState> ReadySendBOMStateList = new List<SendBOMState> { 
                                                                                                SendBOMState.CreatedPoBOM,
                                                                                                SendBOMState.CreatedWithdrawPoBOM,
                                                                                                SendBOMState.ReadySendBOM,
                                                                                                SendBOMState.SendBOMUnSupportedAV,
                                                                                                SendBOMState.SendBOMFail
                                                                                                };

                 IList<SendBOMState> ReadySendOSIStateList = new List<SendBOMState> { 
                                                                                                SendBOMState.GetOSIFail,
                                                                                                SendBOMState.SendOSIFail
                                                                                                };

                  IList<SendBOMState> ReadyVerifyStateList = new List<SendBOMState> { 
                                                                                                SendBOMState.SendOSIOK,
                                                                                                SendBOMState.SendBOMOK,
                                                                                                SendBOMState.SendBOMHolding,
                                                                                                SendBOMState.VerifyFail,
                                                                                                SendBOMState.AlreadySendBOM
                                                                                                };

                  
                 IList<SendBOMState> DelayVerifyStateList= new List<SendBOMState> {                                                                                                
                                                                                                SendBOMState.SendBOMHolding,
                                                                                                 SendBOMState.AlreadySendBOM
                                                                                                };

                List<HPPO> hpPoList = new List<HPPO>();
                UPSDatabase db = new UPSDatabase(config.DBConnectStr);
                //MESData.TestDB(config, db);

                //正常case readfile-> CheckData -> SendBOM -> SendOSI -> VerifyPo
                List<SAPPO> SAPPOList = UPSPoData.GetSAPPO(config);
                if (SAPPOList.Count > 0)
                {
                    #region 處理 SAP UPSPO.txt 與 UPSHPPO
                    //1.Get UPS Support AV
                    List<string> avPartList = UPSWS.GetAvailableAV(config);
                    if (string.IsNullOrEmpty(config.AVPartNo))
                    {
                        SQLStatement.InsertUPSSupportAV(config.DBConnectStr, avPartList);
                    }

                    //2.Check AV PartNo
                    List<ModelBom> modelBomList = null;
                    var needSendSapPoList = UPSPoData.GetAstSapPo(config, SAPPOList, avPartList, out modelBomList);

                    var warnMsgs = needSendSapPoList.Where(x => !string.IsNullOrEmpty(x.ErrorText)).Select(x => x.ErrorText).ToArray();
                    if (warnMsgs.Length > 0)
                    {
                        string warmStr = string.Join("<br/>", warnMsgs);
                        sendMail("UPS 料號有誤,請SIE確認!!", warmStr);

                    }
                    //3.Get UPSHPPO by HPPO
                    hpPoList =MESData.AddUPSHPPO(config, db,needSendSapPoList, modelBomList,"UPS");
                    #endregion
                }                
                else
                {
                    #region 處理UPSHPPO by ReceiveData
                    //讀取UPSHPPO 資料檢查Fail Case,重送資料
                    DateTime now = DateTime.Now;
                    DateTime receiveDate = new DateTime(now.Year, now.Month, now.Day);
                    receiveDate = receiveDate.AddDays(config.ReceiveDateOffSetDay);
                    logger.InfoFormat("Check UPSHPPO Receive Data:{0}", receiveDate.ToString("yyyyMMdd"));
                    hpPoList = MESData.GetUPSHPPO(config, db, receiveDate);
                    #endregion                   
                }                   
                List<HPPO> sendBomHoldingList = new List<HPPO>();
                if (hpPoList != null && hpPoList.Count > 0)
                {
                    var needSendPoList = hpPoList.Where(x => x.PO.Status != SendBOMState.VerifyOK.ToString() || x.isWithdraw).ToList();
                    foreach (var hpPo in needSendPoList)
                    {
                        UPSPOBOM poBom = UPSPoData.GenUPSPoBom(hpPo);
                        MESData.UpdateUPSModel(config, db, hpPo, poBom, "UPS");//无论sendbom 是否ok,都需要记录UPSModel,否则UPS 机型产线已经过完50站。
                        #region Check Status and requset UPS Web service 
                        if (ReadySendBOMStateList.Contains(poBom.State))
                        {
                            UPSPoData.SendCreatePOStatus(config, poBom);                           
                        }
                        else if (ReadySendOSIStateList.Contains(poBom.State))
                        {
                            UPSPoData.SendOSIStatus(config, poBom);
                        }
                        else if (ReadyVerifyStateList.Contains(poBom.State))
                        {
                            UPSPoData.SendVerifyPOStatus(config, poBom);
                        }
                        else
                        {
                            logger.InfoFormat("No Handle UPSHPPO HPPO:{0} CustPO:{1} Status:{2}", poBom.HPPO, poBom.CustPO, poBom.State.ToString());
                            continue;
                        }
                        #endregion

                        MESData.UpdateUPSHPPO(config, db, hpPo, poBom,"UPS");

                        if (DelayVerifyStateList.Contains(poBom.State))
                        {
                            sendBomHoldingList.Add(hpPo);
                        }

                    }

                    #region Check SendBomHolding status, verify again
                    //var sendBomHoldingList = needSendPoList.Where(x => DelayVerifyStateList.Contains( x.PO.Status)));
                    if (sendBomHoldingList.Count() > 0)
                    {
                        //waiting UPS Generate AST Number
                        logger.InfoFormat("Waiting {0} minutes then verify po again", config.DelayVerifyPo.ToString());
                        Thread.Sleep(config.DelayVerifyPo*60 *1000);
                        foreach (var hpPo in sendBomHoldingList)
                        {
                            UPSPOBOM poBom = UPSPoData.GenUPSPoBom(hpPo);
                            UPSPoData.SendVerifyPOStatus(config, poBom);
                            MESData.UpdateUPSHPPO(config, db, hpPo, poBom, "UPS");
                        }
                    }
                    #endregion

                    //Send Warning message
                    string errorFormat="POType:{0} HPPO:{1} CustPO:{2} Qty:{3} Status:{4} ErrorText:{5}";
                    var wanMsgList = needSendPoList.Where(x => !string.IsNullOrEmpty(x.PO.ErrorText))
                                                                       .Select(x => string.Format(errorFormat,
                                                                                                              x.PO.POType,      
                                                                                                               x.PO.HPPO,
                                                                                                                x.PO.EndCustomerPO, 
                                                                                                                x.PO.Qty,
                                                                                                              x.PO.Status, x.PO.ErrorText)).ToArray();
                    if (wanMsgList.Length > 0)
                    {
                        string warmStr = string.Join("<br/>", wanMsgList);
                        sendMail("UPS AV Part 資料有誤,請確認!!", warmStr);
                    }
                }
                else
                {
                    logger.InfoFormat("No SAP PO Data");
                    sendMail("無SAP 收單資料", "無SAP 收單資料");
                    return;                   
                }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                sendMail("程式報錯", e.Message + Environment.NewLine+e.StackTrace);
                
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }

        }

        private static void sendMail(string subject, string body)
        {
            subject = config.MailSubject + subject;
            SendMail.Send(config.FromAddress, config.ToAddress, config.CcAddress, subject, body, config.EmailServer);
        }


        private static string genErrorEmailBody(IEnumerable<UPSPOBOM> poBomList)
        {
            string newLine = "<br/>";
            string ret = newLine;
            string errorFormat = "HPPO:{0} Type:{1}  QTY:{2} AVPartNo:{3} State:{4}  AstRangeState:{5} Message:{6} AstRangeMessage:{7}";
            foreach (var poBom in poBomList)
            {
                ret = ret + string.Format(errorFormat,
                                                                poBom.HPPO,
                                                                poBom.POType,
                                                                poBom.Qty,
                                                                string.Join(",", poBom.PartNoList.ToArray()),
                                                                poBom.State.ToString(),
                                                                string.Join(",", poBom.AstRangeList.Select(x => x.State.ToString()).ToArray()),
                                                                poBom.ErrorText,
                                                                string.Join(",", poBom.AstRangeList.Select(x => x.ErrorText).ToArray())) + newLine;

            }
            return ret;
        }

       
    }
}
