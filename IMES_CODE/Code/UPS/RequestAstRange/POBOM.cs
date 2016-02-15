using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTL.Config;
using log4net;
using System.Reflection;
using System.Security.Principal;
using UTL.Account;
using System.IO;
using UTL.IO;
using UTL.SQL;

namespace UPS
{
    public class POBOM
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static List<SAPPO> Parse(AppConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;          
            logger.DebugFormat("BEGIN: {0}()", methodName);
            List<SAPPO> ret = new List<SAPPO>();
            WindowsImpersonationContext wicCdsi = null;
            try
            {
                DateTime now = DateTime.Now;
                if (!string.IsNullOrEmpty(config.SAPServerUser))
                    wicCdsi = Logon.ImpersinateUser(config.SAPServerUser,
                                                                            config.SAPServerDomain,
                                                                           config.SAPServerPassword);
                FileInfo[] fileInfos = FileOperation.GetFilesByWildCard(config.SAPServerFolder,
                                                                                                       config.SAPPOFile);
                foreach (FileInfo file in fileInfos)
                {
                    string SrcFile = config.SAPServerFolder + file.Name;

                    string DestFile = config.SAPServerBckupFolder + file.Name + "." + now.ToString(config.TimeFormat);

                    //2.Rename SAP weight  file
                    logger.InfoFormat("Move SAP file: {0}", DestFile);
                    FileOperation.FileMove(SrcFile, DestFile, true);
                    logger.InfoFormat("Move file Completed");
                    List<SAPPO> poList = ReadFile(DestFile, config);
                    if (ret.Count > 0)
                    {
                        ret =ret.Concat(poList).ToList();
                    }
                    else
                    {
                        ret = poList;
                    }
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
                if (wicCdsi != null)
                {
                    Logon.Log_off(wicCdsi);
                }
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static List<UPSPOBOM> Parse(AppConfig config, List<string> supportAVList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            List<UPSPOBOM> ret = new List<UPSPOBOM>();
            WindowsImpersonationContext wicCdsi = null;
            try
            {
                DateTime now = DateTime.Now;
                if (!string.IsNullOrEmpty(config.SAPServerUser))
                    wicCdsi = Logon.ImpersinateUser(config.SAPServerUser,
                                                                            config.SAPServerDomain,
                                                                           config.SAPServerPassword);
                FileInfo[] fileInfos = FileOperation.GetFilesByWildCard(config.SAPServerFolder,
                                                                                                       config.SAPPOFile);
                foreach (FileInfo file in fileInfos)
                {
                    string SrcFile = config.SAPServerFolder + file.Name;

                    string DestFile = config.SAPServerBckupFolder + file.Name + "." + now.ToString(config.TimeFormat);

                    //2.Rename SAP weight  file
                    logger.InfoFormat("Move SAP file: {0}", DestFile);
                    FileOperation.FileMove(SrcFile, DestFile, true);
                    logger.InfoFormat("Move file Completed");
                    List<UPSPOBOM> poList = ReadFile(DestFile, config, supportAVList);
                    if (ret.Count > 0)
                    {
                        ret = ret.Concat(poList).ToList();
                    }
                    else
                    {
                        ret = poList;
                    }
                }

                return filterPoNo(config, ret);
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
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

        private static List<UPSPOBOM> filterPoNo(AppConfig config, List<UPSPOBOM> sapPoList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            
            try
            {
                logger.InfoFormat("Configure  Po No: {0}", config.PONo);
                if (string.IsNullOrEmpty( config.PONo))
                {
                    return sapPoList;
                }

                List<string> poList = config.PONo.Split(new char[] { ',', '~' }).ToList();
                var retPoList = (from p in sapPoList
                                 where poList.Contains(p.HPPO)
                                 select p).ToList();

                var filteredPoList = retPoList.Select(x => x.HPPO).ToArray();
                logger.InfoFormat("After filtered , need send  Po count: {0} Po List:{1}", retPoList.Count.ToString(), string.Join("~",filteredPoList));
                return retPoList;
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

        public static List<UPSPOBOM> InsertBTOAvPartNo(AppConfig config, List<UPSPOBOM> poBOMList, List<string> supportAvList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                var BTOModelList = (from p in poBOMList
                                    where p.POType == "BTO"
                                    select p.IECSku).Distinct().ToList();

                if (BTOModelList.Count > 0)
                {
                    List<ModelBom> modelAvPartList = SQLStatement.GetModelAVBom(config.DBConnectStr, BTOModelList).OrderBy(x => x.Model).ToList();

                    var btoPoList = (from bom in poBOMList
                                     where bom.POType == "BTO"
                                     select bom);

                    foreach (var po in btoPoList)
                    {
                        po.PartNoList  = (from av in modelAvPartList
                                                  where av.Model == po.IECSku &&
                                                             supportAvList.Contains(av.AVPartNo)
                                                 select av.AVPartNo).ToList();
                    }


                    //var poList = from bom in poBOMList
                    //             join modelPart in modelAvPartList on bom.IECSku equals modelPart.Model
                    //             where bom.POType == "BTO" && string.IsNullOrEmpty(bom.AVPartNo)
                    //             select new UPSPOBOM
                    //             {
                    //                 CustPO = bom.CustPO,
                    //                 POType = bom.POType,
                    //                 IECSku = bom.IECSku,
                    //                 HPPO = bom.HPPO,
                    //                 HPSku = bom.HPSku,
                    //                 Plant = bom.Plant,
                    //                 Qty = bom.Qty,
                    //                 AVPartNo = modelPart.AVPartNo,
                    //                 State = SendBOMState.CreatedPoBOM
                    //             };

                    //poBOMList = poBOMList.Concat(poList).ToList();

                }

                return poBOMList;
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

        public static List<UPSPOBOM> GetNeedSendUPSBOM(List<UPSPOBOM> poBOMList, List<string> avPartNoList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                var bomList = poBOMList.Where(x =>x.PartNoList.Count>0 && x.PartNoList.All(y => avPartNoList.Contains(y)));
                foreach (var item in bomList)
                {
                    item.State = SendBOMState.ReadySendBOM;                  
                }
                return bomList.ToList();

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
        private static List<SAPPO> ReadFile(string file, AppConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);

            List<SAPPO> ret = new List<SAPPO>();
            try
            {
                string[] data = File.ReadAllLines(file);
                foreach (string s in data)
                {
                    try
                    {

                        string[] column = s.Split(new char[] { '~' });
                        if (column.Length < 8)
                        {
                            throw new Exception(string.Format("字串格式錯誤 '~' 字元只有幾:{0}", column.Length));
                        }

                         string avStr = column[7].Trim();
                        
                         IList<string> partList= avStr.Split(new char[] { ',' }).Select(x=>x.Trim()).ToList();
                        foreach(string partNo in partList)
                        {
                            SAPPO item = new SAPPO(); 
                            item.Plant= column[0].Trim();
                            item.POType = column[1].Trim();
                            item.HPPO= column[2].Trim();
                            item.CustPO = column[3].Trim();
                            item.HPSku = column[4].Trim();
                            item.IECSku = column[5].Trim();
                            item.Qty = int.Parse(column[6].Trim());
                            item.AVPartNo = partNo;

                            ret.Add(item);
                        }                            
                        
                    }
                    catch (Exception e)
                    {
                        logger.Info("Raw data =>" + s);
                        logger.Error(methodName, e);
                        throw e;
                    }
                }

                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

        private static List<UPSPOBOM> ReadFile(string file, AppConfig config, List<string> supportAVList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);

            List<UPSPOBOM> ret = new List<UPSPOBOM>();
            try
            {
                string[] data = File.ReadAllLines(file);
                foreach (string s in data)
                {
                    try
                    {

                        string[] column = s.Split(new char[] { '~' });
                        if (column.Length < 10)
                        {
                            throw new Exception(string.Format("字串格式錯誤 '~' 字元只有幾:{0}", column.Length));
                        }

                        UPSPOBOM item = new UPSPOBOM();
                        item.Plant = column[0].Trim();
                        item.POType = column[1].Trim();
                        item.HPPO = column[2].Trim();
                        item.CustPO = column[3].Trim();
                        item.IECPO = column[4].Trim();
                        item.CustPOItem = column[5].Trim();
                        item.HPSku = column[6].Trim();
                        item.IECSku = column[7].Trim();
                        item.Qty = (int)double.Parse(column[8].Trim());

                        string avStr = column[9].Trim();
                        if (!string.IsNullOrEmpty(avStr))
                        {
                            IList<string> partList = avStr.Split(new char[] { ',' }).Select(x => x.Trim()).ToList();

                            item.PartNoList = partList.Where(x => supportAVList.Contains(x)).ToList();
                        }
                        ret.Add(item);                      

                    }
                    catch (Exception e)
                    {
                        logger.Info("Raw data =>" + s);
                        logger.Error(methodName, e);
                        throw e;
                    }
                }

                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

        

    }

    public class UPSPOBOM
    {
        public string Plant;
        public string POType;
        public string HPPO;
        public string CustPO;
        public string CustPOItem;
        public string IECPO;
        public string HPSku;
        public string IECSku;
        public int Qty;
        public List<string> PartNoList =new List<string>();
        public SendBOMState State = SendBOMState.CreatedPoBOM;
        public string ErrorText = "";
        public List<AstRange> AstRangeList = new List<AstRange>();        
    }


    public class AstRange
    {
        public string avPartNo = "";
        public SendBOMState State = SendBOMState.GetAstRangeFail;
        public string ErrorText = "";
        public string AstPreFix = "";
        public string AstDigitCount = "0";
        public string AstPostFix = "";
        public string StartAst = "";
        public string EndAst = "";
    }

    public class SAPPO
    {
        public string Plant;
        public string POType;
        public string HPPO;
        public string CustPO;
        public string CustPOItem;
        public string IECPO;
        public string HPSku;
        public string IECSku;
        public int Qty;
        public string AVPartNo;
        public SendBOMState State = SendBOMState.CreatedPoBOM;
        public string ErrorText = "";
        public string AstPreFix = "";
        public string AstDigitCount = "0";
        public string AstPostFix = "";
        public string StartAst = "";
        public string EndAst = "";
    }

    public enum SendBOMState
    {
        AlreadySendBOM=-7,
        GetAstRangeResultFail = -6,
        GetAstRangeFail=-5,
        VerifyFail=-4,
        SendOSIFail=-3,
        SendBOMFail = -2,
        SendBOMUnSupportedAV= -1,
        CreatedPoBOM=0,
        ReadySendBOM=1,        
        SendBOMOK=2,
        SendBOMHolding,
        NeedSendOSI,
        SendOSIOK,       
        VerifyOK,
        SendAstRange,
        GetAstRangeOK,
        NoAstRange,
        AlreadyGotRange
    }
}
