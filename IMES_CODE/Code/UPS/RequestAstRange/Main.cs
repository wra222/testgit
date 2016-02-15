using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security.Permissions;
using System.IO;
using System.Net;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Reflection;
using System.Data.Common;

using UTL;
using UTL.SQL;
using UTL.Reflection;
using UTL.Config;
using UTL.MetaData;
using UTL.Protocol;
using UTL.Serialize;
using UTL.Translate;
using UPS.UPSPrimaryService;
using UPS.UPSATRP;
using UTL.Mail;


namespace UPS
{
    class Program
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
                IList<SendBOMState> alreadySendVerifyStateList = new List<SendBOMState> { 
                                                                                                SendBOMState.SendAstRange,
                                                                                                SendBOMState.AlreadyGotRange
                                                                                                //SendBOMState.GetAstRangeOK,
                                                                                                //SendBOMState.NoAstRange,
                                                                                                //SendBOMState.GetAstRangeResultFail
                                                                                                };

               
               //1.Get UPS Support AV
                List<string> avPartNoList = UPSWS.GetAvailableAV(config);
               //1.1 insert Support AV PartNo
                if (string.IsNullOrEmpty(config.AVPartNo))
                {
                    SQLStatement.InsertUPSSupportAV(config.DBConnectStr, avPartNoList);
                }

               //2.Get SAP UPSPO
                List<UPSPOBOM> SapPoBomList = POBOM.Parse(config, avPartNoList);
                if (SapPoBomList.Count > 0)
                {
                    //3.BTO 展BOM
                    SapPoBomList = POBOM.InsertBTOAvPartNo(config, SapPoBomList, avPartNoList);
                    //4.過濾需送AV Part PO BOM
                    List<UPSPOBOM> sendBomList = POBOM.GetNeedSendUPSBOM(SapPoBomList, avPartNoList);
                    if (sendBomList.Count > 0)
                    {
                        DateTime now = DateTime.Now;
                        DateTime expireTime = now.AddMinutes(config.WaitingUPSVerifyPOExpiredTime);
                        bool hasNeedRequestAst=true;
                        bool firstRound = true;
                        while (DateTime.Now <= expireTime)
                        {
                            #region Check BOM Status and SendBOM/SendOSI, VerifyPO,Fet AssetRange
                            foreach (UPSPOBOM poBom in sendBomList)
                            {
                                //if (poBom.State == SendBOMState.SendAstRange)  // 送過AstRange 後不在送
                                if (alreadySendVerifyStateList.Contains(poBom.State))
                                {
                                    continue;
                                }

                                //for debug error
                                //UPSWS.patchRangeError(config.DBConnectStr, poBom, poBom.PartNoList[0], "8017004", "8017004", "J%8d");

                                //4.1 先送UPSVerifyPoReady
                                BomStatus validateFirstPoStatus = UPSWS.RequestUPSVerifyPOReady(poBom);
                                //7.UPSGetRange
                                UPSWS.ValidatePoStatus(validateFirstPoStatus, poBom, config);

                                if (poBom.State == SendBOMState.VerifyOK)
                                {
                                    poBom.State = SendBOMState.SendAstRange;
                                    foreach (var av in poBom.PartNoList)
                                    {
                                        //8.Insert AssetRange
                                        ATRPStruct AstRangeStatus = UPSWS.RequestUPSGetRange(poBom.HPPO, av);
                                        UPSWS.ValidateAstRangeStatus(config.DBConnectStr, AstRangeStatus, poBom, av);
                                    }
                                }
                                else if (validateFirstPoStatus.message.Contains("not found") || validateFirstPoStatus.retcode==-1)
                                {
                                    logger.InfoFormat("Start SendBOM PO:{0}", poBom.HPPO);
                                    //5.UPSSendBOM/UPSSendOSI
                                    BomStatus SendBomstatus = UPSWS.RequestUPSSendBOM(poBom, poBom.PartNoList);
                                    UPSWS.ValidateSendBomStatus(SendBomstatus, poBom, config);

                                    if (poBom.State == SendBOMState.SendBOMHolding)
                                    {
                                        logger.InfoFormat("Start SendBOM PO:{0} Waiting {1} seconds ...", poBom.HPPO, config.DelayVerifyPo.ToString());
                                        //Thread.Sleep(config.DelayVerifyPo * 1000);
                                        continue;
                                    }

                                    if (poBom.State == SendBOMState.SendBOMOK ||
                                        poBom.State == SendBOMState.SendBOMHolding ||
                                        poBom.State == SendBOMState.SendOSIOK )
                                    {
                                        //6.UPSVerifyPO
                                        BomStatus validatePoStatus = UPSWS.RequestUPSVerifyPOReady(poBom);
                                        //7.UPSGetRange
                                        UPSWS.ValidatePoStatus(validatePoStatus, poBom, config);

                                        if (poBom.State == SendBOMState.VerifyOK)
                                        {
                                            poBom.State = SendBOMState.SendAstRange;
                                            foreach (var av in poBom.PartNoList)
                                            {
                                                //8.Insert AssetRange
                                                ATRPStruct AstRangeStatus = UPSWS.RequestUPSGetRange(poBom.HPPO, av);
                                                UPSWS.ValidateAstRangeStatus(config.DBConnectStr, AstRangeStatus, poBom, av);
                                            }
                                        }
                                        else
                                        {
                                            logger.ErrorFormat("Start UPSVerifyPOReady PO:{0} State:{1} !!", poBom.HPPO, poBom.State.ToString());
                                        }
                                    }
                                    else   // Send email notify 
                                    {
                                        logger.ErrorFormat("Start SendBOM PO:{0} State:{1} !!", poBom.HPPO, poBom.State.ToString());
                                    }
                                }
                                else
                                {
                                    if (firstRound)
                                    {
                                        logger.InfoFormat("Check Po:{0} already got Asset Range or not", poBom.HPPO);
                                        //Check Po 是否已抓過Asset Range
                                        bool hasSendPo = false;
                                        hasSendPo = SQLStatement.CheckAssetRangeByHpPo(config.DBConnectStr, poBom.HPPO);
                                        if (hasSendPo)
                                        {
                                            logger.InfoFormat("The Po:{0} already got Asset Range", poBom.HPPO);
                                            poBom.State = SendBOMState.AlreadyGotRange;
                                        }
                                        else
                                        {
                                            logger.WarnFormat("After Check DB AssetRange, the PO:{0} need ValidatePoStatus again Status:{1} fail and waiting UPS ......", poBom.HPPO, poBom.State.ToString());
                                        }
                                    }
                                    else
                                    {
                                        logger.WarnFormat("The PO:{0} ValidatePoStatus Status:{1} Fail, need waiting for UPS Server ......", poBom.HPPO, poBom.State.ToString());
                                    }
                                }

                            }
                            #endregion

                            firstRound = false;
                            //hasNeedRequestAst = sendBomList.Any(x => x.State != SendBOMState.SendAstRange && x.State != SendBOMState.AlreadySendBOM);
                            hasNeedRequestAst = sendBomList.Any(x =>!alreadySendVerifyStateList.Contains( x.State));
                            if (hasNeedRequestAst)
                            {
                                logger.InfoFormat("##########after waiting UPS server {0} second then go to retry UPSVerifyPO again........", config.DelayVerifyPo);
                                Thread.Sleep(config.DelayVerifyPo * 1000);
                            }
                            else
                            {                               
                                break;
                            }
                        }

                        logger.InfoFormat("==========(AlreadySendBOM count:{0} SendAstRange count :{1}) Get UPSAstRange Completed==========",
                                                              sendBomList.Where(x => x.State == SendBOMState.AlreadySendBOM).Count(), sendBomList.Where(x => x.State == SendBOMState.SendAstRange).Count());
                        //Check Error item then Send email 
                        //var erroPoList=sendBomList.Where(x=>x.State!= SendBOMState.SendAstRange ||  
                        //                                    (x.State== SendBOMState.SendAstRange && 
                        //                                     x.AstRangeList.Any(y=>y.State != SendBOMState.GetAstRangeOK)));
                        var erroPoList = sendBomList.Where(x => !alreadySendVerifyStateList.Contains(x.State) ||  
                                                            (x.State== SendBOMState.SendAstRange && 
                                                             x.AstRangeList.Any(y=>y.State != SendBOMState.GetAstRangeOK)));
                        if (erroPoList.Count() > 0)
                        {
                            sendMail("UPS資產標籤 報錯[" + erroPoList.Count().ToString()+ "]", genErrorEmailBody(erroPoList));
                        }
                        else
                        {
                            sendMail("UPS資產標籤 成功", "UPS資產標籤 成功");
                        }
                    }
                    else
                    {
                        //Send email                        
                        logger.InfoFormat("No AV Part No Request Asset Range");
                        sendMail("SAP收單中無資產標籤料號", "SAP收單中無資產標籤料號");
                    }
                }
                else
                {
                    logger.InfoFormat("No SAP PO Data");
                    sendMail("無SAP 收單資料", "無SAP 收單資料");
                }                   
               
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                sendMail("程式報錯", e.StackTrace);
                //throw e;
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
            string  newLine ="<br/>"; 
            string ret = newLine;
            string errorFormat ="HPPO:{0} Type:{1}  QTY:{2} AVPartNo:{3} State:{4}  AstRangeState:{5} Message:{6} AstRangeMessage:{7}";
            foreach (var poBom in poBomList)
            {
                ret = ret + string.Format(errorFormat,
                                                                poBom.HPPO,
                                                                poBom.POType,
                                                                poBom.Qty,
                                                                string.Join(",", poBom.PartNoList.ToArray()),
                                                                poBom.State.ToString(),
                                                                string.Join(",",poBom.AstRangeList.Select(x=>x.State.ToString()).ToArray()),
                                                                poBom.ErrorText,
                                                                string.Join(",", poBom.AstRangeList.Select(x => x.ErrorText).ToArray())) + newLine;

            }
                return ret;
        }

    }
}
        