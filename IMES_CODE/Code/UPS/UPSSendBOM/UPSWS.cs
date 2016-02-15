using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTL.Config;
using log4net;
using System.Reflection;
using UPS.UPSPrimaryService;
using UPS.UPSATRP;
using System.Data;
using System.Text.RegularExpressions;
using UTL.SQL;

namespace UPS
{
    public class UPSWS
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static List<string> GetAvailableAV(AppConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            List<string> ret = new List<string>();
            //Get UPS available AV partNo
            try
            {
                if (string.IsNullOrEmpty(config.AVPartNo))
                {
                    logger.InfoFormat("Get UPS Support AV PartNo");

                    ret = RequestUPSGetSupportedAV();
                }
                else
                {
                    logger.InfoFormat("Manul assign AV PartNo: {0}", config.AVPartNo);
                   List<ConstValueTypeInfo> consttype=SQLStatement.GetConstValueType(config.DBConnectStr, "UPSAVPartNo");
                   ret = consttype.Select(x => x.Value).ToList();
                   // ret = config.AVPartNo.Split(new char[] { ',', '~' }).Select(x => x.Trim()).ToList();
                }
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {               
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static List<string> RequestUPSGetSupportedAV()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            List<string> ret = new List<string>();
            //Get UPS available AV partNo
            try
            {
                UPSPrimaryServiceSoapClient soapClient = new UPSPrimaryServiceSoapClient("UPS");
                DataSet ds = soapClient.UPSGetSupportedAV(new DateTime(2000, 01, 01));
                DataRowCollection drs = ds.Tables[0].Rows;
                foreach (DataRow dr in drs)
                {
                    ret.Add(dr["TemplateName"].ToString().Trim());
                }
               
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static BomStatus RequestUPSSendBOM(UPSPOBOM po,List<string> avPartList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                UPSPrimaryServiceSoapClient soapClient = new UPSPrimaryServiceSoapClient("UPS");
                string txnId = DateTime.Now.ToString("yyyyMMddhhmmss.fff");
                string bom = "";
                
                foreach (string av in avPartList)
                {
                    bom = bom + string.Format("<BomItem><PartNumber>{0}</PartNumber></BomItem>", av);
                }           
                
                bom =string.Format("<BOM>{0}</BOM>",bom);
                logger.InfoFormat("UPSSendBOM txnId:{0} HPPO:{1} CustPO:{2} IECPO:{3} Qty:{4} Bom:{5}", 
                                                txnId, po.HPPO, po.CustPO,po.IECPO??"",  po.Qty.ToString(), bom);
                return   soapClient.UPSSendBOM(txnId, po.HPPO, po.CustPO, po.Qty, bom);               
                
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }

        }

        public static BomStatus RequestUPSSendOSI(UPSPOBOM po,string osiString)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                UPSPrimaryServiceSoapClient soapClient = new UPSPrimaryServiceSoapClient("UPS");
                string txnId = DateTime.Now.ToString("yyyyMMddhhmmss.fff");
                logger.InfoFormat("UPSSendOSI txnId:{0} HPPO:{1} :OSI:{2}", txnId, po.HPPO, osiString);   
                return soapClient.UPSSendOSI(txnId, po.HPPO, osiString);               

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static BomStatus RequestUPSVerifyPOReady(UPSPOBOM po)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            List<string> ret = new List<string>();           
            try
            {

                UPSPrimaryServiceSoapClient soapClient = new UPSPrimaryServiceSoapClient("UPS");
                string txnId = DateTime.Now.ToString("yyyyMMddhhmmss.fff");
                logger.InfoFormat("UPSVerifyPOReady HPPO:{0}",po.HPPO);
                return soapClient.UPSVerifyPOReady(po.HPPO);               


            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static BomStatus RequestUPSGetOSI(UPSPOBOM po)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            List<string> ret = new List<string>();
            try
            {

                UPSPrimaryServiceSoapClient soapClient = new UPSPrimaryServiceSoapClient("UPS");
                string txnId = DateTime.Now.ToString("yyyyMMddhhmmss.fff");
                logger.InfoFormat("UPSGetOSI HPPO:{0}", po.HPPO);
                return soapClient.UPSGetOSI(po.HPPO);


            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static ATRPStruct RequestUPSGetRange(string hpPo, string avPartNo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {                
                ATRPSoapClient soapClient = new ATRPSoapClient("UPS");
                string txnId = DateTime.Now.ToString("yyyyMMddhhmmss.fff");
                logger.InfoFormat("UPSGetRange HPPO:{0} AVPartNo:{1}", hpPo, avPartNo);
                return soapClient.UPSGetRange(avPartNo, avPartNo, hpPo);   

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        public static void ValidateSendBomStatus(BomStatus status, UPSPOBOM poBom, AppConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("Reply SendBomStatus PO:{0} retCode:{1} message:{2} datastring:{3}", poBom.HPPO, status.retcode.ToString(), status.message, status.datastring);
                if (status.retcode == 0) //Holding process need waiting time
                {
                    poBom.State = SendBOMState.SendBOMHolding;
                    
                }
                else if (status.retcode == 1) // Send OSI 
                {
                    
                    poBom.State = SendBOMState.NeedSendOSI;
                  

                    //產生OSI String ??????
                    //HP說目前OS只有CustomerPO欄位
                    //string osiString = "<CustomerPO>{0}</CustomerPO>";
                    //osiString = string.Format(osiString, poBom.CustPO);
                   string osiString = GenOSIString(poBom, status.datastring, config);

                    ValidateSendOSIStatus(RequestUPSSendOSI(poBom, osiString), poBom);
                }
                else if (status.retcode == 2)
                {                  
                   poBom.State = SendBOMState.SendBOMOK;                  
                }
                else if (status.retcode == 4)
                {                    
                    poBom.State = SendBOMState.SendBOMUnSupportedAV;
                    poBom.ErrorText = string.Format("Reply SendBomStatus  PO:{0} Code:{1} DataString:{2} Message:{3}", poBom.HPPO, status.retcode, status.datastring, status.message);
                }
                else if (status.retcode == 3)
                {
                    poBom.State = SendBOMState.AlreadySendBOM;
                    poBom.ErrorText = string.Format("Reply SendBomStatus PO:{0} Code:{1} DataString:{2} Message:{3}", poBom.HPPO, status.retcode, status.datastring, status.message);
                }
                else
                {
                    poBom.State = SendBOMState.SendBOMFail;
                    poBom.ErrorText = string.Format("Reply SendBomStatus  PO:{0} Code:{1} DataString:{2} Message:{3}", poBom.HPPO, status.retcode, status.datastring, status.message);
                }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
        public static string ValidateGetOSIStatus(BomStatus status, UPSPOBOM poBom)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("Reply GetOSIStatus retCode:{0} message:{1} datastring:{2}", status.retcode.ToString(), status.message, status.datastring);
                if (status.retcode == 0) //Holding process need waiting time
                {

                    poBom.State = SendBOMState.GetOSIOK;

                }
                else
                {
                    poBom.State = SendBOMState.GetOSIFail;
                    poBom.ErrorText = string.Format("Return Code:{0} DataString:{1} Message:{2}", status.retcode, status.datastring, status.message);
                }
                return status.datastring;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static void ValidateSendOSIStatus(BomStatus status, UPSPOBOM poBom)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("Reply SendOSIStatus retCode:{0} message:{1} datastring:{2}", status.retcode.ToString(), status.message, status.datastring);
                if (status.retcode == 0) //Holding process need waiting time
                {
                    
                     poBom.State = SendBOMState.SendOSIOK;
                  
                }               
                else
                {
                   poBom.State = SendBOMState.SendOSIFail;
                   poBom.ErrorText = string.Format("Return Code:{0} DataString:{1} Message:{2}", status.retcode, status.datastring, status.message);
                 }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static void ValidatePoStatus(BomStatus status, UPSPOBOM poBom, AppConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("Reply ValidatePoStatus retCode:{0} message:{1} datastring:{2}", status.retcode.ToString(), status.message, status.datastring);
                if (status.retcode == 0) //Holding process need waiting time
                {                    
                     poBom.State = SendBOMState.VerifyOK;                 
                }
                else if (status.retcode == 1) //need send OSI
                {
                    if (!string.IsNullOrEmpty(status.datastring))
                    {
                        poBom.State = SendBOMState.NeedSendOSI;


                        //產生OSI String ??????
                        //HP說目前OS只有CustomerPO欄位
                        //string osiString = "<CustomerPO>{0}</CustomerPO>";
                        //osiString = string.Format(osiString, poBom.CustPO);
                        string osiString = GenOSIString(poBom, status.datastring, config);

                        ValidateSendOSIStatus(RequestUPSSendOSI(poBom, osiString), poBom);
                        if (poBom.State == SendBOMState.SendOSIOK)
                        {
                            //6.UPSVerifyPO
                            BomStatus validatePoStatus = UPSWS.RequestUPSVerifyPOReady(poBom);
                            //7.UPSGetRange
                            UPSWS.ValidatePoStatus(validatePoStatus, poBom, config);
                        }
                    }
                    else
                    {
                        poBom.State = SendBOMState.VerifyFail;
                         poBom.ErrorText = string.Format("Return Code:{0} DataString:{1} Message:{2}", status.retcode, status.datastring, status.message);
                    }
                }
                else
                {
                    poBom.State = SendBOMState.VerifyFail;
                    poBom.ErrorText = string.Format("Return Code:{0} DataString:{1} Message:{2}", status.retcode, status.datastring, status.message);

                }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        public static void ValidateAstRangeStatus(string strConnect, ATRPStruct status, UPSPOBOM poBom,string avPartNo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            string msgFormat="HPPO:{0} AV:{1} Ast Range RangeStart:{2} RangeEnd:{3} Mask:{4} Error:{5}";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            logger.InfoFormat("HPPO:{0} AV:{1} Reply ValidateAstRangeStatus Return Code:{2} Version:{3} Message:{4} tagdata:{5}",
                                                  poBom.HPPO, avPartNo, status.retcode, status.version, status.message, status.tagdata.GetXml());
            try
            {
                
                if (status.retcode == 0) //Holding process need waiting time
                {
                    //poBom.State = SendBOMState.GetAstRangeOK;
                    if (status.tagdata.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = status.tagdata.Tables[0].Rows[0];
                        string startAst = dr["RangeStart"].ToString();
                        string endAst = dr["RangeEnd"].ToString();
                        string mask = dr["Mask"].ToString();
                        logger.InfoFormat(msgFormat, poBom.HPPO, avPartNo, startAst, endAst, mask, "");

                        string pattern = @"^(?<prefix>\w*)%(?<digit>\d+)d(?<postfix>\w*)$";
                        Match m = Regex.Match(mask, pattern);
                        if (m.Success)
                        {
                            AstRange astRange = new AstRange();
                            astRange.avPartNo = avPartNo;
                            astRange.AstDigitCount = m.Groups["digit"].Value.Trim();
                            astRange.AstPostFix = m.Groups["postfix"].Value.Trim();
                            astRange.AstPreFix = m.Groups["prefix"].Value.Trim();
                            astRange.StartAst = startAst;
                            astRange.EndAst = endAst;
                            astRange.State = SendBOMState.GetAstRangeOK;
                            logger.InfoFormat("HPPO:{0} IECPO:{1} AV:{2} Ast Range prefix:{3} digit:{4} postfix:{5}",
                                                          poBom.HPPO, poBom.IECPO, avPartNo, astRange.AstPreFix, astRange.AstDigitCount, astRange.AstPostFix);
                            poBom.AstRangeList.Add(astRange);
                            InsertAssetRange(strConnect, poBom.HPPO,  poBom.IECPO,  astRange);
                        }
                        else
                        {
                            AstRange astRange = new AstRange();
                            astRange.avPartNo = avPartNo;
                            astRange.State = SendBOMState.GetAstRangeResultFail;
                            astRange.ErrorText = string.Format(msgFormat, poBom.HPPO, avPartNo, startAst, endAst, mask, "Mask Pattern is wrong");
                            poBom.AstRangeList.Add(astRange);
                            logger.ErrorFormat(msgFormat, poBom.HPPO, avPartNo, startAst, endAst, mask, "Mask Pattern is wrong");
                        }
                    }
                    else  // No Asset Range data
                    {
                        AstRange astRange = new AstRange();
                        astRange.avPartNo = avPartNo;
                        astRange.State = SendBOMState.NoAstRange;
                        astRange.ErrorText = string.Format("HPPO:{0} AV:{1} Reply ValidateAstRangeStatus (No Asset Range) Return Code:{2} Version:{3} Message:{4}",
                                                                                 poBom.HPPO, avPartNo, status.retcode, status.version, status.message);
                        logger.ErrorFormat("HPPO:{0} AV:{1} Reply ValidateAstRangeStatus (No Asset Range) Return Code:{2} Version:{3} Message:{4}",
                                                   poBom.HPPO, avPartNo, status.retcode, status.version, status.message); 
                    }
                }
                else
                {
                    logger.ErrorFormat("HPPO:{0} AV:{1} Reply ValidateAstRangeStatus Fail Return Code:{2} Version:{3} Message:{4}",
                                                  poBom.HPPO, avPartNo,status.retcode, status.version, status.message);
                    AstRange astRange = new AstRange();
                    astRange.avPartNo = avPartNo;
                    astRange.State = SendBOMState.GetAstRangeFail;
                    astRange.ErrorText = string.Format("HPPO:{0} AV:{1}  Reply ValidateAstRangeStatus  Fail Return Code:{2} Version:{3} Message:{4}",
                                                                             poBom.HPPO, avPartNo, status.retcode, status.version, status.message);
                    poBom.AstRangeList.Add(astRange);
                }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        public static void InsertAssetRange(string strConnect, string hpPo, string iecPo, AstRange astRange)
        {
           if ( astRange.State== SendBOMState.GetAstRangeOK)
           {
               string zeroFormat = "D" + astRange.AstDigitCount;
               astRange.StartAst = astRange.AstPreFix + long.Parse(astRange.StartAst).ToString(zeroFormat);
               astRange.EndAst = astRange.AstPreFix + long.Parse(astRange.EndAst).ToString(zeroFormat);

               SQLStatement.InsertAssetRange(strConnect, astRange.avPartNo, astRange.StartAst, astRange.EndAst, "PO:" + hpPo + "~IECPO:" + iecPo+ "~PostFix:" + astRange.AstPostFix, "UPS");
               if (!string.IsNullOrEmpty(astRange.AstPostFix))
               {
                   SQLStatement.InsertAssetRangePostFixed(strConnect, astRange.avPartNo,astRange.AstPostFix, "PO:" + hpPo +"~IECPO:" + iecPo, "UPS");
               }
            }
        }



        public static string GenOSIString(UPSPOBOM poBOM, string osiSpec, AppConfig config)
        {
            string osiString = "";
            string osiFormat = "<{0}>{1}</{0}>";
            int index = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("UPS OSI Spec:{0}, Config OSI Tag Name:{1} Config OSI Map Value:{2}",
                                            osiSpec, 
                                            string.Join (",",config.OSITagName),
                                            string.Join(",",config.OSITagNameMapValue));
                foreach (string name in config.OSITagName)
                {
                    if (!string.IsNullOrEmpty(name) && osiSpec.Contains(name))
                    {
                        string value="";
                        if (config.OSITagNameMapValue.Length > index)
                        {
                            if (config.OSITagNameMapValue[index].ToLower() == "customerpo")
                            {
                                value = poBOM.CustPO;
                            }
                            else
                            {
                                value = config.OSITagNameMapValue[index];
                            }
                        }
                        osiString = osiString + string.Format(osiFormat, name, value);

                    }

                    index++;
                }
                logger.InfoFormat("OSI String:{0}", osiString);
                return osiString;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        public static void patchRangeError(string strConnect,
                                                              UPSPOBOM poBom,
                                                              string avPartNo,
                                                              string startAst,
                                                             string endAst,
                                                            string mask)
        {
            string pattern = @"^(?<prefix>\w*)%(?<digit>\d+)d(?<postfix>\w*)$";
            Match m = Regex.Match(mask, pattern);
            if (m.Success)
            {
                AstRange astRange = new AstRange();
                astRange.avPartNo = avPartNo;
                astRange.AstDigitCount = m.Groups["digit"].Value.Trim();
                astRange.AstPostFix = m.Groups["postfix"].Value.Trim();
                astRange.AstPreFix = m.Groups["prefix"].Value.Trim();
                astRange.StartAst = startAst;
                astRange.EndAst = endAst;
                astRange.State = SendBOMState.GetAstRangeOK;
                logger.InfoFormat("Ast Range prefix:{0} digit:{1} postfix:{2}", astRange.AstPreFix, astRange.AstDigitCount, astRange.AstPostFix);
                InsertAssetRange(strConnect, poBom.HPPO, poBom.IECPO, astRange);
            }
        }
    }
} 
