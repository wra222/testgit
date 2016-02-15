/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CheckRepairDefect。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-13   Tong.Zhi-Yong                implement DoExecute method
 * 2010-01-07   Tong.Zhi-Yong                Modify ITC-1103-0047
 * 2010-01-07   Tong.Zhi-Yong                Modify ITC-1103-0053
 * 2010-01-08   Tong.Zhi-Yong                Modify ITC-1103-0076
 * 2010-01-18   Tong.Zhi-Yong                Modify ITC-1103-0096
 * 2010-02-22   Tong.Zhi-Yong                Modify ITC-1122-0114
 * 2010-03-02   Tong.Zhi-Yong                Modify ITC-1122-0164
 * 2010-03-03   Tong.Zhi-Yong                Modify ITC-1122-0165
 * 2010-03-08   Tong.Zhi-Yong                Modify ITC-1122-0202 
 * 2010-03-23   Tong.Zhi-Yong                Add Part Forbidden For FA&OQC Repair
 * 2012-01-11   Yang.Wei-Hua                 For IMES2012 SA Repair
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part.PartPolicy;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Defect;
//using IMES.FisObject.Common.BOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
//using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{
    /// <summary>
    /// 检查当前处理的RepairDefectInfo是否合法
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCA, FA, PAQC维修站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         PCA REPAIR站：
    ///             1.检查MBSno的维修次数，若大于等于1，则提示“此MB有@Count次不良”
    ///             2.检查[Site]是否由以下字符组成’ 01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ’，若不是则报错：“错误的Location 代码”
    ///             3.如果[Defect] 并非系统支持的Defect ，则报告错误：“Wrong Defect Code!!”,系统支持的Defect – GetData..DefectCode表（Tp=‘PRD’）
    ///             4.对于Cause = ‘Key Part Fail’ 时，如果Faulty Part Sno and New Part Sno 均输入，则需要更新[PCB_Part] 中的相关记录，如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!”
    ///         FA Repair站：
    ///             1.若Part Type=’KP/ME/MB’时，需要检查输入的sn对应的part type与所选择的type一致
    ///             2.MB Part在Product表存在
    ///             3.与Faulty Part Sno匹配到相同的part no作part check(對於new sno是否已經使用還沒有做出定義)
    ///         PAQC Repair站：
    ///             1.检查old sn是否在product_part表中存在
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.Product
    ///         Session.PCAReapirDefect
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IMB
    ///         PCARepair
    ///         PCARepairInfo
    /// </para> 
    /// </remarks>
    public partial class CheckRepairDefect : BaseActivity
	{
        private const string BOMSessionKey = Session.SessionKeys.SessionBom;

        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckRepairDefect()
		{
			InitializeComponent();
		}

        ///<summary>
        /// 处理那个类型的维修(SA, FA, PAK)
        ///</summary>
        public static DependencyProperty RepairStationTypeProperty = DependencyProperty.Register("RepairStationType", typeof(string), typeof(CheckRepairDefect));

        ///<summary>
        /// 处理那个类型的维修(SA, FA, PAK)
        ///</summary>
        [DescriptionAttribute("RepairStationType")]
        [CategoryAttribute("RepairStationType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string RepairStationType
        {
            get
            {
                return ((string)(GetValue(RepairStationTypeProperty)));
            }
            set
            {
                SetValue(RepairStationTypeProperty, value);
            }
        }

        /// <summary>
        /// 检查当前处理的RepairDefectInfo是否合法
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IRepairTarget repairTarget = GetRepairTarget();
                RepairDefect defect = null;
                IMB imbTarget = null;
                IProduct iProductTarget = null;
                IList<IProductPart> mbParts = null;
                bool isSnoExist = false;
                Session session = CurrentSession;
                if (repairTarget == null)
                {
                    throw new FisException();
                }

                defect = (RepairDefect)session.GetValue(Session.SessionKeys.CurrentRepairdefect);
                if (defect == null)
                {
                    throw new FisException();
                }

                if (string.Compare(RepairStationType, "SA", true) == 0)
                {
                    //1. get repair times, add it to session
                    var repairs = repairTarget.GetRepair();
                    if (repairs != null && repairs.Count > 1)
                    {
                        session.AddValue(Session.SessionKeys.RepairTimes, repairs.Count - 1);
                    }
                    else
                    {
                        session.AddValue(Session.SessionKeys.RepairTimes, 0);
                    }

                    //2.检查[Site]是否由以下字符组成’ 01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ’，若不是则报错：“错误的Location 代码”
                    if (!string.IsNullOrEmpty(defect.Site))
                    {
                        for (int i = 0; i < defect.Site.Length; i++)
                        {
                            char c = defect.Site[i];
                            if (
                                !((c >= '0' && c <= '9')
                                || (c >= 'A' && c <= 'Z')) 
                                )
                            {
                                var ex215 = new FisException("CHK215", new List<string>());
                                ex215.stopWF = false;
                                throw ex215;
                            }
                        }
                    }

                    //3.如果[Defect] 并非系统支持的Defect ，则报告错误：“Wrong Defect Code!!”,系统支持的Defect – GetData..DefectCode表（Tp=‘PRD’）
                    IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                    var defects = defectRepository.GetDefectCodeList("PRD");
                    if (defects != null)
                    {
                        bool defectSupported = false;
                        foreach (var defectCodeInfo in defects)
                        {
                            if (defectCodeInfo.Defect.CompareTo(defect.DefectCodeID) == 0)
                            {
                                defectSupported = true;
                            }
                        }
                        if (!defectSupported)
                        {
                            var ex216 = new FisException("CHK216", new List<string>());
                            ex216.stopWF = false;
                            throw ex216;
                        }
                    }

                    //4.对于Cause = ‘Key Part Fail’ 时，Faulty Part Sno and New Part Sno 均输入，如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!”
                    session.RemoveValue(Session.SessionKeys.PartID);
                    if (!string.IsNullOrEmpty(defect.OldPartSno) && !string.IsNullOrEmpty(defect.NewPartSno))
                    {
                        //check faulty part sn
                        imbTarget = (IMB)repairTarget;

                        mbParts = imbTarget.MBParts;

                        if (mbParts != null || mbParts.Count > 0)
                        {
                            foreach (ProductPart temp in mbParts)
                            {
                                if (temp.PartSn != null && temp.PartSn.Equals(defect.OldPartSno))
                                {
                                    isSnoExist = true;

                                    //put partId into Session
                                    session.AddValue(Session.SessionKeys.PartID, temp);
                                    break;
                                }
                            }
                        }
                        if (!isSnoExist)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(defect.OldPartSno);
                            FisException ex27 = new FisException("CHK027", errpara);
                            ex27.stopWF = false;
                            throw ex27;
                        }
                    }
                }
                //else if (station.StationType == StationType.FARepair || station.StationType == StationType.FAOperation)
                else if (string.Compare(RepairStationType, "FA", true) == 0 )
                {                    
                    if (isNeedCheckPartType(defect.PartType))
                    {                        
                        IPart oldPart = null;                     
                        IList<IProductPart> partList = null;

                        if (!string.IsNullOrEmpty(defect.OldPartSno.Trim()))
                        {
                            iProductTarget = (IProduct)repairTarget;
                            partList = iProductTarget.ProductParts;
                            
                            if (!defect.PartType.Trim().Equals("MB"))
                            {
                                oldPart = checkOldSNForKP(partList, defect, defect.PartType);
                            }
                        }
                        //若Part Type=’ MB’时，需要检查输入的sn对应的part type与所选择的type一致(对于MB与Product.PCBID匹配)；
                        if (defect.PartType.Trim().Equals("MB"))
                        {
                            /*if (string.Compare(((IProduct)repairTarget).PCBID, defect.OldPartSno) != 0)
                            {
                                List<string> errpara = new List<string>();
                                FisException ex = new FisException("CHK828", errpara);
                                ex.stopWF = false;
                                throw ex;
                            }*/

                            session.AddValue(Session.SessionKeys.OldMB, defect.OldPartSno);
                            session.AddValue(Session.SessionKeys.NewMB, defect.NewPartSno);
                        }

                        if (!string.IsNullOrEmpty(defect.NewPartSno.Trim()) &&
                            !string.IsNullOrEmpty(defect.OldPartSno.Trim()))
                        {
                            string checkItemType = String.Empty;
                            if (!defect.PartType.Trim().Equals("MB"))
                            {
                                if (partList != null && partList.Count != 0)
                                {
                                    foreach (IProductPart t in partList)
                                    {
                                        if (t.PartSn == defect.OldPartSno)
                                        {
                                            checkItemType = t.CheckItemType;
                                            break;
                                        }
                                    }
                                }
                                if (!String.IsNullOrEmpty(checkItemType))
                                {
                                    IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();                            //UC未确认 //1.20
                                    HierarchicalBOM bom = (HierarchicalBOM)bomRepository.GetHierarchicalBOMByModel(iProductTarget.Model);
                                    if (bom == null)
                                    {
                                        throw new NullReferenceException("HierarchicalBOM is null");
                                    }
                                    IPartPolicyRepository prtPcyRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.PartPolicy.IPartPolicyRepository>();                            //UC未确认 //1.20
                                    IPartPolicy policy = prtPcyRepository.GetPolicy(checkItemType);
                                    if (policy == null)
                                    {
                                        throw new NullReferenceException("IPartPolicy is null");
                                    }
                                    IFlatBOM fBom = policy.FilterBOM(bom, Station, iProductTarget);
                                    if (fBom == null)
                                    {
                                        throw new NullReferenceException("IFlatBOM is null");
                                    }
                                    PartUnit oldUnit = fBom.Match(defect.OldPartSno, Station);
                                    if (oldUnit == null)
                                    {
                                        throw new NullReferenceException("oldUnit is null");
                                    }
                                    PartUnit newUnit = fBom.CurrentMatchedBomItem.Match(defect.NewPartSno, Station);
                                    if (newUnit == null)
                                    {
                                        throw new NullReferenceException("newUnit is null");
                                    }
                                    newUnit.CurrentSession = (Object)session;
                                    newUnit.ProductId = iProductTarget.ProId;
                                    fBom.CurrentMatchedBomItem.Check(newUnit, iProductTarget, Station,fBom);
                                    defect.NewPart = newUnit.Pn;
                                }
                                else
                                {
                                    throw new NullReferenceException("CheckItemType is null");
                                }
                            }//if (!defect.PartType.Trim().Equals("MB"))
                            else
                            {                               
                                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                                HierarchicalBOM bom = (HierarchicalBOM)bomRepository.GetHierarchicalBOMByModel(iProductTarget.Model);
                                if (bom == null)
                                {
                                    throw new NullReferenceException("HierarchicalBOM is null");
                                }
                                
                                IPartPolicyRepository prtPcyRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.PartPolicy.IPartPolicyRepository>();
                                checkItemType = (string)session.GetValue("MBorVGA");                               
                                //for mantis 503: RCTO 修护界面异常
                                if (checkItemType == "MB")
                                {
                                    checkItemType = "RCTOMB";
                                }
                                IPartPolicy policy = prtPcyRepository.GetPolicy(checkItemType);
                                if (policy == null)
                                {
                                    throw new NullReferenceException("IPartPolicy is null");
                                }
                                IFlatBOM fBom = policy.FilterBOM(bom, Station, iProductTarget);
                                if (fBom == null)
                                {
                                    throw new NullReferenceException("IFlatBOM is null");
                                }
                                PartUnit oldUnit = fBom.Match(defect.OldPartSno, Station);
                                if (oldUnit == null)
                                {
                                    throw new NullReferenceException("oldUnit is null");
                                }
                                PartUnit newUnit = fBom.CurrentMatchedBomItem.Match(defect.NewPartSno, Station);
                                if (newUnit == null)
                                {
                                    throw new NullReferenceException("newUnit is null");
                                }
                                newUnit.ProductId = iProductTarget.ProId;

                                fBom.CurrentMatchedBomItem.Check(newUnit, iProductTarget, Station, fBom);

                                #region Vincent disable below code
                                //if (iProductTarget.Model.Substring(0, 3) != "173")
                                //{
                                //    fBom.CurrentMatchedBomItem.Check(newUnit, iProductTarget, Station, fBom);
                                //}
                                //else
                                //{
                                //    fBom.CurrentMatchedBomItem.Check(newUnit, iProductTarget, "45A", fBom);
                                //}
                                #endregion
                            }                            
                        }
                    }
                }
                else if (string.Compare(RepairStationType, "PAQC", true) == 0)
                {
                    IList<IProductPart> partList = null;

                    iProductTarget = (IProduct)repairTarget;
                    partList = iProductTarget.ProductParts;

                    foreach (ProductPart temp in partList)
                    {
                        if (temp.Value != null && temp.Value.Equals(defect.OldPartSno))
                        {
                            isSnoExist = true;

                            //put partId into Session
                            session.AddValue(Session.SessionKeys.PartID, temp);
                            break;
                        }
                    }

                    if (!isSnoExist)
                    {
                        session.RemoveValue(Session.SessionKeys.PartID);
                    }
                }

                return base.DoExecute(executionContext);
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool isKeyPartFail(RepairDefect defect)
        {
            IDefectInfoRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
            IList<CauseInfo> causeList = defectRepository.GetCauseList(Customer, this.RepairStationType);
            string keyPartFail = "Key Part Fail";

            if (causeList != null && causeList.Count != 0)
            {
                foreach (CauseInfo item in causeList)
                {
                    if (item.id.Equals(defect.Cause) && string.Compare(item.friendlyName, keyPartFail, true) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool isNeedCheckPartType(string type)
        {
            //check whether the type is 'KP/ME/MB'
            if (string.Compare(type, "MB", true) == 0 || string.Compare(type, "KP/ME", true) == 0)
            {
                return true;
            }

            return false;
        }

        private IPart checkOldSNForKP(IList<IProductPart> partList, RepairDefect defect, string type)
        {
            bool typeFlag = false;
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //PartType partType = null;
            IPart part = null;
            string oldSN = defect.OldPartSno;
            bool isHaveCheckBit = defect.OldPartSno.Trim().Length >= 14;
            bool isInProductPart = false;

            //old sn check
            foreach (IProductPart temp in partList)
            {
                if (isHaveCheckBit)
                {
                    isInProductPart = (temp.PartSn.Equals(oldSN.Substring(0, 13)) || temp.PartSn.Equals(oldSN));
                }
                else
                {
                    isInProductPart = temp.PartSn.Equals(oldSN);
                }

                if (isInProductPart)
                {
                    part = ipr.Find(temp.PartID);
                    if (part != null)
                    {
                        typeFlag = true;
                        break;
                    }
                }
            }
            if (!typeFlag)
            {             
                List<string> errpara = new List<string>();                
                FisException ex = new FisException("CHK829", errpara);
                ex.stopWF = false;
                throw ex;             
            }

            return part;
        }

        private void checkNewSN(IPart part, string type, string sn)
        {
            bool typeFlag = false;
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            PartType partType = null;

            if (part != null)
            {
                if (string.Compare(type, "MB", true) == 0)
                {
                    if (string.Compare(part.Type, type, true) == 0)
                    {
                        typeFlag = true;
                    }
                }
                else
                {
                    partType = ipr.GetPartType(part.Type);

                    if (partType != null && string.Compare(partType.PartTypeGroup, type, true) == 0)
                    {
                        typeFlag = true;
                    }
                }
            }

            if (!typeFlag)
            {
                List<string> errpara = new List<string>();
                errpara.Add(sn);
                FisException ex = new FisException("CHK053", errpara);
                ex.stopWF = false;
                throw ex;
            }
        }

        private void checkPN(IPart oldPart, IPart newPart)
        {
            if (string.Compare(oldPart.PN, newPart.PN, true) != 0)
            {
                List<string> errpara = new List<string>();
                FisException ex = new FisException("CHK055", errpara);
                ex.stopWF = false;
                throw ex;
            }
        }

        //ITC-1122-0165 Tong.Zhi-Yong 2010-03-02
        //check whether bind to another product or this product
        private void checkBindToAnotherProduct(IProduct thisProduct, IPart newPart)
        {

            try
            {
                //IProductRepository PrdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //((Product)thisProduct).PartUniqueCheck(newPart.PN, ((IBOMPart)newPart).MatchedSn);
                /*var parts = PrdctRepository.GetProductPartsByPartNoAndValue(newPart.PN, ((IBOMPart)newPart).MatchedSn);
                if (parts != null)
                {
                    foreach (var productPart in parts)
                    {
                        if (productPart.PartID != thisProduct.ProId)
                        {
                            List<string> erpara = new List<string>();
                            erpara.Add("SN");
                            erpara.Add(((IBOMPart)newPart).MatchedSn);
                            erpara.Add(productPart.ProductID);
                            var ex = new FisException("CHK009", erpara);
                            ex.stopWF = false;
                            throw ex;
                        }
                    }
                }*/
            }
            catch (FisException ex)
            {
                ex.stopWF = false;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ITC-1122-0165 Tong.Zhi-Yong 2010-03-02
        private void checkBindToThisProduct(IProduct thisProduct, IPart newPart)
        {
            IList<IProductPart> parts = thisProduct.ProductParts;
            IBOMPart bomPart = (IBOMPart)newPart;
            //string newPartSn;// = bomPart.MatchedSn;
            string newPartPn = bomPart.PN;

            if (parts != null)
            {
                foreach (var productPart in parts)
                {
                    //if (string.Compare(productPart.Value, newPartSn, true) == 0 && string.Compare(productPart.PartID, newPartPn, true) == 0)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("SN");
                        //erpara.Add(newPartSn);
                        var ex = new FisException("CHK083", erpara);
                        ex.stopWF = false;
                        throw ex;
                    }
                }
            }
        }

        private void checkBindToAnotherProductForMB(IProduct thisProduct, IPart newPart)
        {
            IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IBOMPart bomPart = (IBOMPart)newPart;
            /*
            IList<IProduct> lstProduct = ipr.GetProductListByPCBID(bomPart.MatchedSn);
            
            if (lstProduct != null)
            {
                foreach (var product in lstProduct)
                {
                    if (product.ProId != thisProduct.ProId)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("SN");
                        erpara.Add(bomPart.MatchedSn);
                        erpara.Add(product.ProId);
                        var ex = new FisException("CHK009", erpara);
                        ex.stopWF = false;
                        throw ex;
                    }
                }
            }*/
        }

        private void checkBindToThisProductForMB(IProduct thisProduct, IPart newPart)
        {
            IBOMPart bomPart = (IBOMPart)newPart;
            /*
            string newPartSn = bomPart.MatchedSn;

            if (string.Compare(thisProduct.PCBID, newPartSn, true) == 0)
            {
                List<string> erpara = new List<string>();
                erpara.Add("SN");
                erpara.Add(newPartSn);
                var ex = new FisException("CHK083", erpara);
                ex.stopWF = false;
                throw ex;
            }*/
        }

        //2010-03-23
        private void checkPartForbidden(IBOMPart part, IProduct thisProduct)
        {
            try
            {
                part.CheckHold(thisProduct.Family, thisProduct.Model);
            }
            catch (FisException ex)
            {
                ex.stopWF = false;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}
