/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:GetASTwithoutATSNAV activity
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{
    /// <summary>
    /// 获取要卡的资产列表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC AFT MVS.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     详见UC
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class GetASTwithoutATSNAV : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetASTwithoutATSNAV()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取要卡的资产列表
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(CurrentProduct.Model);
            IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;

            List<string> errpara = new List<string>();
            errpara.Add(CurrentProduct.ProId);

            //IList<string> pnsOfATSN1AndATSN3 = new List<string>();
            IList<string> pnsOfATSN1 = new List<string>();
            IList<string> pnsOfATSN3 = new List<string>();
            IList<string> pnsOfAT_any_5 = new List<string>();
            
            //IList<string> pnsOfATSN = new List<string>();
            Hashtable hQtyPNofSC = new Hashtable();
            List<BomItemInfo> sessionBOM = new List<BomItemInfo>();
            //bool bExistATSN1 = false;
            //bool bExistATSN2 = false;
            //bool bExistATSN3 = false;
            //bool bExistATSN5 = false;
            //bool bExistATSN8 = false;
            bool bExistAT_any_5 = false;
            //bool bExistAT_any_7 = false;
            bool bNeedPrintICASALabel = false;
            bool bNeedPrintAnatelLabelCond1 = false;
            bool bNeedPrintAnatelLabelCond2 = false;
            bool bNeedPrintAsstage1 = false;
            bool bNeedPrintAsstage2 = false;
            string LabelAsstage2 = "Asstage-2";

            foreach (IBOMNode bomNode in bomNodeList)
            {
                //ICASA Label('6060BICASA01')
                if (bomNode.Part.PN == "6060BICASA01")
                {
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = "ICASA Label";
                    tmp.type = "ICASA Label";
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = bomNode.Part.PN;
                    sessionBOM.Add(tmp);
                    bNeedPrintICASALabel = true;
                }

                //Smart card reader('6042B0035801','1310A2099701','1310A2172201','1310A2383501','1310A2383502','1310A2423301','6050A2404601')
                /*if (bomNode.Part.PN == "6042B0035801"
                    || bomNode.Part.PN == "1310A2099701"
                    || bomNode.Part.PN == "1310A2172201"
                    || bomNode.Part.PN == "1310A2383501"
                    || bomNode.Part.PN == "1310A2383502"
                    || bomNode.Part.PN == "1310A2423301"
                    || bomNode.Part.PN == "6050A2404601")
                {
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = "Smart card reader";
                    tmp.type = "Smart card reader";
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = bomNode.Part.PN;
                    sessionBOM.Add(tmp);
                }*/
                //(2012-06-07)Smart card reader获取方式变更，详见UC
                foreach (PartInfo tmp in bomNode.Part.Attributes)
                {
                    if (tmp.InfoType == "Descr" && tmp.InfoValue.ToUpper().Contains("SMART CARD"))
                    {
                        if (hQtyPNofSC.Contains(bomNode.Qty))
                        {
                            hQtyPNofSC[bomNode.Qty] = (string)hQtyPNofSC[bomNode.Qty] + "," + bomNode.Part.PN;
                        }
                        else
                        {
                            hQtyPNofSC.Add(bomNode.Qty, bomNode.Part.PN);
                        }
                        break;
                    }
                }

                //Master Label('60NOMSTLBL01')
                if (bomNode.Part.PN == "60NOMSTLBL01")
                {
                    IList<IMES.FisObject.Common.Model.ModelInfo> lst = iModelRepository.GetModelInfoByModelAndName(CurrentProduct.Model, "PCID");
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = "Master Label";
                    tmp.type = "Master Label";
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = lst[0].Value;
                    sessionBOM.Add(tmp);
                }

                //Anatel Label(BomNodeType='PL' and Descr='Anatel label')
                if (bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr == "Anatel label")
                {
                    bNeedPrintAnatelLabelCond1 = true;
                }

                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN1") bExistATSN1 = true;
                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN2") bExistATSN2 = true;
                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN3") bExistATSN3 = true;
                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN5") bExistATSN5 = true;
                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN7") bExistATSN7 = true;
                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN8") bExistATSN8 = true;
                if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr.StartsWith("AT") && bomNode.Part.Descr.EndsWith("5"))
                {
                    bExistAT_any_5 = true;
                    pnsOfAT_any_5.Add(bomNode.Part.PN);
                }

                //if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "PP")
                if (bomNode.Part.BOMNodeType == "PP" && bomNode.Part.Descr == "Asstage-1")
                {
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = bomNode.Part.Descr;
                    tmp.tp = "PP";
                    tmp.type = bomNode.Part.Type;
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = bomNode.Part.PN;
                    sessionBOM.Add(tmp);

                    bNeedPrintAsstage1 = true;
                }

                if (bomNode.Part.BOMNodeType == "PP" && bomNode.Part.Descr == "Asstage-2")
                {
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = bomNode.Part.Descr;
                    tmp.tp = "PP";
                    tmp.type = bomNode.Part.Type;
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = bomNode.Part.PN;
                    sessionBOM.Add(tmp);

                    bNeedPrintAsstage2 = true;
                }

                //ATSN2
                if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN2")
                {
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = bomNode.Part.Descr;
                    tmp.tp = "ATSN2";
                    tmp.type = bomNode.Part.Type;
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = bomNode.Part.PN;
                    sessionBOM.Add(tmp);
                }
                /*
                if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr.StartsWith("ATSN"))
                {
                    pnsOfATSN.Add(bomNode.Part.PN);
                }
                */
                if (bomNode.Part.BOMNodeType == "AT" && (bomNode.Part.Descr == "ATSN1" || bomNode.Part.Descr == "ATSN3"))
                {
                    /*
                     * Answer to: ITC-1360-1061
                     * Description: Get Qty of bom element.
                     */
                    BomItemInfo tmp = new BomItemInfo();
                    tmp.description = bomNode.Part.Descr;
                    tmp.type = bomNode.Part.Type;
                    tmp.qty = bomNode.Qty;
                    tmp.scannedQty = 0;
                    tmp.PartNoItem = bomNode.Part.PN;
                    tmp.parts = new List<PartNoInfo>();
                    sessionBOM.Add(tmp);

                    if (bomNode.Part.Descr == "ATSN1") pnsOfATSN1.Add(bomNode.Part.PN);
                    else pnsOfATSN3.Add(bomNode.Part.PN);
                }
            }

            //(2012-06-07)Smart card reader:若取得数据为多行，且Qty一致，则取得所有数据为共用料
            foreach (DictionaryEntry de in hQtyPNofSC)
            {
                BomItemInfo tmp = new BomItemInfo();
                tmp.description = "Smart card reader";
                tmp.type = "Smart card reader";
                tmp.qty = (int)de.Key;
                tmp.scannedQty = 0;
                tmp.PartNoItem = (string)de.Value;
                sessionBOM.Add(tmp);
            }

            //1）	检查是否需要检查 通过Model 检查 bom中BomNodeType=’AT’  （Descr=’ATSN1’ Or Descr=’ATSN3’） 判断是否有存在ATSN1, ATSN3
            //2）	判断是否有存在,如不存在，则不需要卡资产标签
            //2012-03-17 UC updated:2）	判断是否有存在,如不存在，则不需要卡ATSN1 或ATSN3资产标签
            /*
             * Answer to: ITC-1360-1473
             * Description: 2012-03-17 UC updated
             */
            /*
            if (pnsOfATSN1AndATSN3.Count == 0)
            {
                sessionBOM.Clear();
                CurrentSession.AddValue(Session.SessionKeys.SessionBom, sessionBOM);
                return base.DoExecute(executionContext);
            }
            */

            IList<IProductPart> productParts = CurrentProduct.ProductParts;
            //bool bExistPart = false;
            bool bExistAT_any_5Part = false;
            bool bExistATSN1Part = false;
            bool bExistATSN3Part = false;
            foreach (IProductPart partNode in productParts)
            {
                if (pnsOfATSN1.Contains(partNode.PartID) || pnsOfATSN3.Contains(partNode.PartID))
                {
                    if (pnsOfATSN1.Contains(partNode.PartID)) bExistATSN1Part = true;
                    else bExistATSN3Part = true;
                    //如果有任一存在资产标签但相应的PartSn =‘’则提示“这台机器结合的AST不完全”
                    if (partNode.PartSn == "")
                    {
                        throw new FisException("CHK195", errpara);
                    }
                    else
                    {
                        foreach (BomItemInfo ele in sessionBOM)
                        {
                            /*
                             * Answer to: ITC-1360-1078
                             * Description: When getting bom data, should use PartID(not partSn) to do compare.
                             */
                            if (ele.PartNoItem != partNode.PartID) continue;
                            PartNoInfo tmp = new PartNoInfo();
                            tmp.id = partNode.PartSn;
                            ele.parts.Add(tmp);
                        }
                    }
                }
                //if (pnsOfATSN.Contains(partNode.PartID)) bExistPart = true;
                if (pnsOfAT_any_5.Contains(partNode.PartID)) bExistAT_any_5Part = true;
                if (partNode.PartSn.StartsWith("G")) bNeedPrintAnatelLabelCond2 = true;
                
            }

            //(4/13)如果bom的下阶存在BomNodeType=’AT’ and Descr =’ATSN1’，则检查Product_Part是否已经结合ATSN1的AST。
            //(4/13)若Product_Part不存在ATSN1的结合记录，则提示：“这台机器没有结合ATSN1”
            if (pnsOfATSN1.Count > 0 && !bExistATSN1Part)
            {
                throw new FisException("CHK520", errpara);
            }

            //(4/13)如果bom的下阶存在BomNodeType=’AT’ and Descr =’ATSN3’，则检查Product_Part是否已经结合ATSN3的AST。
            //(4/13)若Product_Part不存在ATSN3的结合记录，则提示：“这台机器没有结合ATSN3”
            if (pnsOfATSN3.Count > 0 && !bExistATSN3Part)
            {
                throw new FisException("CHK521", errpara);
            }

            //4) 如果Product_Part. PartSn where ProductID=@prdid and PartNo in (bom中BomNodeType=’AT’  and Descr like ’ATSN%’) 不存在记录，且通过Model 检查 bom中BomNodeType=’AT’  (Descr= ATSN1，ATSN2，ATSN3，ATSN3，ATSN5，ATSN7)判断不存在（ATSN1，ATSN2，ATSN3，ATSN5，ATSN7），则提示“这个Product需要资产标签号，请去Combine AST”
            //if (!bExistPart && (!bExistATSN1 && !bExistATSN2 && !bExistATSN3 && !bExistATSN5 && !bExistATSN7))
            //if (pnsOfATSN.Count > 0 && !bExistPart)
            /*
             * Answer to: ITC-1360-1671
             * Description: Wrong logic in condition.
             */
            if (bExistAT_any_5 && !bExistAT_any_5Part)
            {
                throw new FisException("CHK198", errpara);
            }

            IPrintLogRepository printLogRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository>();
            //如果需要打印ICASA Label，如果未打印,则提示”请先去打印ICASA Label”
            if (bNeedPrintICASALabel)
            {
                if (!printLogRep.CheckExistPrintLogByLabelNameAndDescr("ICASA label", CurrentProduct.ProId))
                {
                    throw new FisException("CHK518", new string[] { });
                }
            }

            //如果需要打印Anatel label，且未打印，则提示：“请去打印Anatel label”
            if (bNeedPrintAnatelLabelCond1 && bNeedPrintAnatelLabelCond2)
            {
                if (!printLogRep.CheckExistPrintLogByLabelNameAndDescr("Anatel label", CurrentProduct.ProId))
                {
                    throw new FisException("CHK519", new string[] { });
                }
            }

            //(4/23)如果bom的下阶存在BomNodeType=’PP’ and Descr =’Asstage-1’，则检查PrintLog（Name=AT and BegNo=ProductID and EndNo=’’ and Descr=’ATSN’）是否已经打印PP类的资产标签；若不存在打印记录，则报错：“请去Online Generate AST打印PP类标签”
            if (bNeedPrintAsstage1)
            {
            }

            if (bNeedPrintAsstage2)
            {
                PrintLog cond = new PrintLog();
                cond.BeginNo = CurrentProduct.CUSTSN;
                //cond.EndNo = "";
                cond.Name = LabelAsstage2;
                //cond.Descr = "ATSN";

                IList<PrintLog> printLogList = printLogRep.GetPrintLogListByCondition(cond);
                if (printLogList.Count <= 0)
                {
                    throw new FisException("CQCHK0009", new string[] { LabelAsstage2 });
                }
            }

            sessionBOM.Sort(new IcpBomItemInfo());

            CurrentSession.AddValue(Session.SessionKeys.SessionBom, sessionBOM);
            CurrentSession.AddValue("bMatchSN", true);
            CurrentSession.AddValue("bMatchID", true);
            CurrentSession.AddValue("CRCOfProductID", getCRC(CurrentProduct.ProId));
            CurrentSession.AddValue(Session.SessionKeys.MatchedParts, ",");
            //CurrentSession.AddValue("bNeedATSN7", (bNeedPrintASTN7Label && !bExistATSN7Part));
            //CurrentSession.AddValue("PnListOfATSN7", pnsOfATSN7);
            //CurrentSession.AddValue("bCDSI", bCDSICond1);

            return base.DoExecute(executionContext);
        }

        private string getCRC(string orig)
        {
            string sequence = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int sum = 0;
            foreach (char c in orig.ToUpper())
            {
                int pos = sequence.IndexOf(c);
                sum += pos >= 0 ? pos : 36;
            }
            sum %= 16;
            return sequence[sum].ToString();
        }

        private class IcpBomItemInfo : IComparer<BomItemInfo>
        {
            public int Compare(BomItemInfo x, BomItemInfo y)
            {
                return x.type.CompareTo(y.type);
            }
        }
    }
}
