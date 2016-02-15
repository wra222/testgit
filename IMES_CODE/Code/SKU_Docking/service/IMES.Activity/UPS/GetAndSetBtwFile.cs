﻿// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Change PO Product Condition
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2015-01-21 Vincent             create
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.UPS;
using IMES.UPS;
using IMES.UPS.WS;
using IMES.FisObject.PAK.DN;
using System.Data;
using System.IO;
using log4net;
using IMES.Infrastructure.Extend;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// GetAndSetBtwFile Product
    /// </summary>
    public partial class GetAndSetBtwFile : BaseActivity
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
        ///<summary>
        ///</summary>
        public GetAndSetBtwFile()
        {
            InitializeComponent();
        }
        /// <summary>
        /// bat方式
        /// </summary>
        private const int Bat = 0;


        /// <summary>
        /// 模板方式
        /// </summary>
        private const int Template = 1;


        /// <summary>
        /// bartender方式
        /// </summary>
        private const int Bartender = 3;

        /// <summary>
        /// bartenderSrv方式
        /// </summary>
        private const int BartenderSrv = 4;

        /// <summary>
        /// GetAndSetBtwPrintInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            var currenProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            IUPSRepository upsRep = RepositoryFactory.GetInstance().GetRepository<IUPSRepository>(); 

            IList<AstDefineInfo> needCombineAstList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedCombineAstDefineList);
            IList<IPart> astPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedCombineAstPartList);
            if (needCombineAstList.Count == 0)
            {
                return base.DoExecute(executionContext);  // 此机器不需要print asset Label !
            }

            //CDSI 機器不走UPS Print 
            DateTime now = DateTime.Now;
           // if (!currenProduct.IsBindedUPS)
            if (!needCombineAstList.Where(x=>x.HasUPSAst=="Y").Any())
               // (currenProduct.IsCDSI && !currenProduct.IsCNRS))
            {
                filterPrintTemplate(session, new List<int> { Bat, Template });
                return base.DoExecute(executionContext);  //throw no assign UPS Asset number
            }

            if( !needCombineAstList.Any(x=>x.NeedPrint=="Y") ||
                needCombineAstList.All(x=>x.HasCDSIAst=="Y"))
            {
                return base.DoExecute(executionContext);  // 此机器不需要print asset Label !
            }
            //bool isCDSIDevice = false;
            //if (currenProduct.IsCDSI && !currenProduct.IsCNRS)
            //{
            //    isCDSIDevice = true;              
            //}

            bool hasOnlyUPSTag = needCombineAstList.All(x => x.HasUPSAst == "Y");

            UPSCombinePO combinePO = (UPSCombinePO)session.GetValue(Session.SessionKeys.UPSCombinePO);
            if (combinePO == null)
            {
                combinePO = upsRep.Find(currenProduct.ProId);
            }
           
            if (combinePO == null)
            {
                    //throw no assign UPS Asset number
                    throw new FisException("CQCHK1120", new string[] { currenProduct.CUSTSN, currenProduct.Model });
            }

            var upsAvPart = combinePO.UPSAVPart;
            if (upsAvPart == null || upsAvPart.Count == 0)
            {
                    //throw error no UPS AV PartNo
                   throw new FisException("CQCHK1115", new string[] { currenProduct.CUSTSN, currenProduct.Model });
            }
        
            #region 檢查BOM AV Part 是否都是UPS AV Part
            if (hasOnlyUPSTag)
            {                
                foreach (AstDefineInfo define in needCombineAstList)
                {
                    var matchPartList = astPartList.Where(x => x.BOMNodeType == define.AstType && x.Descr == define.AstCode).ToList();                  
                    foreach (IPart part in matchPartList)
                    {
                        hasOnlyUPSTag = part.Attributes.Any(x => x.InfoType == "AV" && upsAvPart.Any(y => y.AVPartNo == x.InfoValue));
                        if (!hasOnlyUPSTag)
                        {
                            break;
                        }                     
                    }
                    if (!hasOnlyUPSTag)
                    {
                        break;
                    }   
                }
            }
            #endregion

            UPSWS ws = UPSWS.Instance;
            string sn = currenProduct.CUSTSN;
            ATRPStruct  ret= ws.GetATRP(sn, needCombineAstList[0].AstLocation);
            if (ret.retcode < 0)
            {
                //throw error UPS error message
                throw new FisException("CQCHK1118", new string[] { "UPSATRP", currenProduct.CUSTSN,  "", "", ret.message });
            }

            if (ret.tagdata == null || 
                ret.tagdata.Tables.Count==0 || 
                ret.tagdata.Tables[0].Rows.Count==0)
            {
                logger.InfoFormat("UPSARTP SN:{0} Location:{1} no bartender file", sn, needCombineAstList[0].AstLocation);
                filterPrintTemplate(session, new List<int> { Bat, Template });
                return base.DoExecute(executionContext);
            }

            string btwFolder = utl.GetSysSetting("UPSBtwFolder", null);
            string strMN2 = currenProduct.GetModelProperty("MN2") as string;
            string jpgFolder = utl.GetSysSetting("UPSJPGFolder", null);
            IList<string> jpgFileNameList = new List<string>();

            int dataCount = 0;
            DataRowCollection  drc = ret.tagdata.Tables[0].Rows;
            foreach (DataRow dr in drc)
            {
                string btwFileName = null;
                string btwFileInfoName = "BtwFile";
                string btwDataInfoName = "BtwLabelData";
                string printerStockInfoName = "UPSPrinterStock";
                string jpgFileName = null;
                string seqNo = dataCount.ToString("D2");
                if (dataCount == 0)
                {
                    btwFileName = sn + ".btw";
                    jpgFileName = sn + ".jpg";
                }
                else
                {
                    btwFileName = sn + "_" + seqNo + ".btw";
                    jpgFileName = sn + "_" + seqNo + ".jpg";
                    btwFileInfoName = btwFileInfoName + "_" + seqNo;
                    btwDataInfoName = btwDataInfoName + "_" + seqNo;
                    printerStockInfoName = printerStockInfoName + "_" + seqNo;
                }

                #region write Bartender file
                byte[] btwBytes = (byte[])dr[0];
                if (btwBytes.Length == 0)
                {
                    //throw error
                    throw new FisException("CQCHK1118", new string[] { "UPSATRP", currenProduct.CUSTSN, "", "", "bartender file length is zero" });
                }
                writeFileAllBytes(btwFolder, btwFileName, btwBytes);
                #endregion

                string btwLabelData = (string)dr[1];
                currenProduct.SetExtendedProperty(btwFileInfoName, btwFileName, this.Editor);
                currenProduct.SetExtendedProperty(btwDataInfoName, btwLabelData, this.Editor);
                if (dr[2] != null)
                {
                    byte[] jpgBytes = (byte[])dr[2];

                    #region write JPG SOP File
                    if (jpgBytes.Length > 0)
                    {
                        jpgFileNameList.Add(writeFileAllBytes(jpgFolder, jpgFileName, jpgBytes));
                        //foreach (IPart part in astPartList)
                        //{
                        //    if (part.Attributes.Any(x => x.InfoType == "AV" && upsAvPart.Any(y => y.AVPartNo == x.InfoValue)))
                        //    {
                        //        string jpgFileName = null;
                        //        if (dataCount == 0)
                        //        {
                        //            jpgFileName = (strMN2 ?? "") + astPartList[0].PN + ".jpg";
                        //        }
                        //        else
                        //        {
                        //            jpgFileName = (strMN2 ?? "") + astPartList[0].PN + "_" + seqNo + ".jpg";
                        //        }

                        //        jpgFileNameList.Add(writeFileAllBytes(jpgFolder, jpgFileName, jpgBytes));
                        //    }
                        //}
                    }
                    #endregion
                }

                #region UPS PrinterStock
                string printerStock = (string)dr[3];
                if (!string.IsNullOrEmpty(printerStock))
                {
                    currenProduct.SetExtendedProperty(printerStockInfoName, printerStock, this.Editor);
                }
                #endregion

                dataCount++;
            }

            session.AddValue(ExtendSession.SessionKeys.UPSJPGFiles, string.Join("~", jpgFileNameList.ToArray()));
            //若是只有UPS Tag,過濾標籤(支援一站可打印Bat/Bartender 方式)
            if (hasOnlyUPSTag)
            {
                filterPrintTemplate(session, new List<int> { Bartender, BartenderSrv });
            }
            
            prodRep.Update(currenProduct, session.UnitOfWork);
            return base.DoExecute(executionContext);
        }

        private string writeFileAllBytes(string folder,string fileName, byte[] data)
        {
            string file = null;
            if (folder.EndsWith("\\"))
            {
                file = folder + fileName;
            }
            else
            {
                file = folder + "\\" + fileName;              
            }
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            File.WriteAllBytes(file, data);
            return file;
        }

        private void filterPrintTemplate(Session session, IList<int> AllowprintModeList)
        {
            
            var printItemList = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
             IList<PrintItem> resultPrintItemList = new List<PrintItem>();
            if (printItemList == null)
            {
                return;
            }
           
            //需要Remove Bat file
            foreach (var item in printItemList)
            {
                if (AllowprintModeList.Contains(item.PrintMode))
                {
                    resultPrintItemList.Add(item);
                }
            }           

            session.AddValue(Session.SessionKeys.PrintItems, resultPrintItemList);
        }

    }

    
}
