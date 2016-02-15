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
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.BOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
//using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.Common.Station;

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
    ///             1.当某个Location (Component + Site)已经维修了2次的时候，则报告错误：“此MB 该Location 已经维修了2次！！禁止key 入！！“
    ///             2.当某个Location (Component + Site)已经维修了1次的时候，需要提示用户：“此MB 该Location 已经维修了1次！“
    ///             3.对于Cause = ‘Key Part Fail’ 时，如果Faulty Part Sno and New Part Sno 均输入，则需要更新[PCB_Part] 中的相关记录，如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!”
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
                return ((string)(base.GetValue(CheckRepairDefect.RepairStationTypeProperty)));
            }
            set
            {
                base.SetValue(CheckRepairDefect.RepairStationTypeProperty, value);
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
                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
                IStation station = stationRepository.Find(Station);

                if (repairTarget == null)
                {
                    throw new FisException();
                }

                defect = (RepairDefect) CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);

                if (defect == null)
                {
                    throw new FisException();
                }

                //if (station.StationType == StationType.SARepair)
                if (string.Compare(RepairStationType, "SA", true) == 0)
                {
                    IList<RepairDefect> defectList = null;

                    defectList = repairTarget.GetRepairDefectBySiteComponent(defect.Site, defect.Component);

                    if (defectList != null && defectList.Count != 0)
                    {
                        //ITC-1103-0096 Tong.Zhi-Yong 2010-01-18
                        int tempCount = 0;

                        foreach (RepairDefect t in defectList)
                        {
                            if (t.ID != defect.ID)
                            {
                                tempCount++;
                            }
                        }

                        if (tempCount == 0 || tempCount == 1)
                        {
                            CurrentSession.AddValue(Session.SessionKeys.RepairTimes, tempCount);
                        }
                        else if (tempCount == 2)
                        {
                            //ITC-1103-0053 Tong.Zhi-Yong 2010-01-07
                            List<string> errpara = new List<string>();
                            errpara.Add(Key);

                            FisException ex26 = new FisException("CHK026", errpara);
                            
                            ex26.stopWF = false;
                            throw ex26;
                        }
                    }
                    else
                    {
                        CurrentSession.AddValue(Session.SessionKeys.RepairTimes, 0);
                    }

                    //ITC-1103-0076 Tong.Zhi-Yong 2010-01-08
                    CurrentSession.RemoveValue(Session.SessionKeys.PartID);

                    if (isKeyPartFail(defect))
                    {
                        //check faulty part sn
                        imbTarget = (IMB)repairTarget;

                        mbParts = imbTarget.MBParts;

                        //if (mbParts == null || mbParts.Count == 0)
                        //{
                        //    throw new FisException();
                        //}

                        foreach (ProductPart temp in mbParts)
                        {
                            if (temp.Value != null && temp.Value.Equals(defect.OldPartSno))
                            {
                                isSnoExist = true;

                                //put partId into Session
                                CurrentSession.AddValue(Session.SessionKeys.PartID, temp);
                                break;
                            }
                        }

                        if (!isSnoExist)
                        {
                            //ITC-1103-0047 Tong.Zhi-Yong 2010-01-07
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
                        BOM bom = (BOM)CurrentSession.GetValue(BOMSessionKey);
                        IPart oldPart = null;
                        IPart newPart = null;
                        IList<IProductPart> partList = null;

                        if (!string.IsNullOrEmpty(defect.OldPartSno.Trim()))
                        {
                            iProductTarget = (IProduct)repairTarget;
                            partList = iProductTarget.ProductParts;

                            //ITC-1122-0114 Tong.Zhi-Yong 2010-02-22
                            if (!defect.PartType.Trim().Equals("MB"))
                            {
                                oldPart = checkOldSN(partList, defect, defect.PartType);
                            }
                        }

                        if (defect.PartType.Trim().Equals("MB"))
                        {
                            if (string.Compare(((IProduct)repairTarget).PCBID, defect.OldPartSno) != 0)
                            {
                                List<string> errpara = new List<string>();
                                FisException ex = new FisException("CHK054", errpara);
                                ex.stopWF = false;
                                throw ex;
                            }

                            CurrentSession.AddValue(Session.SessionKeys.OldMB, defect.OldPartSno);
                            CurrentSession.AddValue(Session.SessionKeys.NewMB, defect.NewPartSno);
                        }

                        if (!string.IsNullOrEmpty(defect.NewPartSno.Trim()))
                        {
                            string tempNewSn = defect.NewPartSno.Trim();

                            newPart = bom.Match(tempNewSn, this.Station);

                            checkNewSN(newPart, defect.PartType, tempNewSn);

                            //ITC-1122-0164 Tong.Zhi-Yong 2010-03-02
                            defect.NewPartSno = ((IBOMPart)newPart).MatchedSn;

                            if (!defect.PartType.Trim().Equals("MB"))
                            {
                                checkBindToThisProduct(iProductTarget, newPart);
                                checkBindToAnotherProduct(iProductTarget, newPart);
                                checkPN(oldPart, newPart);
                            }
                            else
                            {
                                //Need A New Method(Find Whether MB Part Bind To Another Product)
                                checkBindToAnotherProductForMB(iProductTarget, newPart);
                                checkBindToThisProductForMB(iProductTarget, newPart);

                                string oldMBSn = defect.OldPartSno;
                                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                                IMB imb = mbRepository.Find(oldMBSn);

                                if (imb == null || string.Compare(imb.Model, newPart.PN, true) != 0)
                                {
                                    List<string> errpara = new List<string>();
                                    FisException ex = new FisException("CHK055", errpara);
                                    ex.stopWF = false;
                                    throw ex;
                                }
                            }

                            checkPartForbidden((IBOMPart)newPart, iProductTarget);
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
                            CurrentSession.AddValue(Session.SessionKeys.PartID, temp);
                            break;
                        }
                    }

                    if (!isSnoExist)
                    {
                        CurrentSession.RemoveValue(Session.SessionKeys.PartID);
                    }
                }

                return base.DoExecute(executionContext);
            }
            catch (FisException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private bool isKeyPartFail(RepairDefect defect)
        {
            IDefectInfoRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
            IList<CauseInfo> causeList = defectRepository.GetCauseList(Customer);
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
            if (string.Compare(type, "MB", true) == 0 || string.Compare(type, "ME", true) == 0 || string.Compare(type, "KP", true) == 0)
            {
                return true;
            }

            return false;
        }

        private IPart checkOldSN(IList<IProductPart> partList, RepairDefect defect, string type)
        {
            if (type.Equals("ME"))
            {
                return checkOldSNForME(partList, defect, type);
            }
            else
            {
                return checkOldSNForKP(partList, defect, type);
            }
        }

        private IPart checkOldSNForME(IList<IProductPart> partList, RepairDefect defect, string type)
        {
            bool typeFlag = false;
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            PartType partType = null;
            IPart part = null;

            //old sn check
            foreach (IProductPart temp in partList)
            {
                if (temp.Value.Equals(defect.OldPartSno))
                {
                    part = ipr.Find(temp.PartID);

                    if (part != null)
                    {
                        partType = ipr.GetPartType(part.Type);

                        if (partType == null)
                        {
                            break;
                        }
                        else if (string.Compare(partType.PartTypeGroup, defect.PartType, true) == 0)
                        {
                            typeFlag = true;
                            break;
                        }
                    }  
                }
            }

            if (!typeFlag)
            {
                BOM bom = (BOM)CurrentSession.GetValue(BOMSessionKey);
                part = bom.Match(defect.OldPartSno, this.Station);

                if (part != null)
                {
                    partType = ipr.GetPartType(part.Type);

                    if (partType == null)
                    {
                    }
                    else if (string.Compare(partType.PartTypeGroup, defect.PartType, true) == 0)
                    {
                        typeFlag = true;
                    }
                }

                if (!typeFlag)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(defect.OldPartSno);
                    FisException ex = new FisException("CHK053", errpara);
                    ex.stopWF = false;
                    throw ex;                    
                }
            }

            return part;
        }

        private IPart checkOldSNForKP(IList<IProductPart> partList, RepairDefect defect, string type)
        {
            bool typeFlag = false;
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            PartType partType = null;
            IPart part = null;
            string oldSN = defect.OldPartSno;
            bool isHaveCheckBit = defect.OldPartSno.Trim().Length >= 14;
            bool isInProductPart = false;

            //old sn check
            foreach (IProductPart temp in partList)
            {
                if (isHaveCheckBit)
                {
                    isInProductPart = (temp.Value.Equals(oldSN.Substring(0, 13)) || temp.Value.Equals(oldSN));
                }
                else
                {
                    isInProductPart = temp.Value.Equals(oldSN);
                }

                if (isInProductPart)
                {
                    part = ipr.Find(temp.PartID);

                    if (part != null)
                    {
                        partType = ipr.GetPartType(part.Type);

                        if (partType == null)
                        {
                            break;
                        }
                        else if (string.Compare(partType.PartTypeGroup, defect.PartType, true) == 0)
                        {
                            typeFlag = true;
                            break;
                        }
                    }
                }
            }

            if (!typeFlag)
            {

                BOM bom = (BOM)CurrentSession.GetValue(BOMSessionKey);
                part = bom.Match(defect.OldPartSno, this.Station);

                if (part != null)
                {
                    partType = ipr.GetPartType(part.Type);

                    if (partType == null)
                    {
                    }
                    else if (string.Compare(partType.PartTypeGroup, defect.PartType, true) == 0)
                    {
                        typeFlag = true;
                    }
                }

                if (!typeFlag)
                {   //ITC-1122-0202 Tong.Zhi-Yong 2010-03-08
                    List<string> errpara = new List<string>();
                    errpara.Add(defect.OldPartSno);
                    FisException ex = new FisException("CHK053", errpara);
                    ex.stopWF = false;
                    throw ex;
                }
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
            IProductRepository PrdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            try
            {
                //((Product)thisProduct).PartUniqueCheck(newPart.PN, ((IBOMPart)newPart).MatchedSn);
                var parts = PrdctRepository.GetProductPartsByPartNoAndValue(newPart.PN, ((IBOMPart)newPart).MatchedSn);
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
                }
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
            string newPartSn = bomPart.MatchedSn;
            string newPartPn = bomPart.PN;

            if (parts != null)
            {
                foreach (var productPart in parts)
                {
                    if (string.Compare(productPart.Value, newPartSn, true) == 0 && string.Compare(productPart.PartID, newPartPn, true) == 0)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("SN");
                        erpara.Add(newPartSn);
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
            }
        }

        private void checkBindToThisProductForMB(IProduct thisProduct, IPart newPart)
        {
            IBOMPart bomPart = (IBOMPart)newPart;
            string newPartSn = bomPart.MatchedSn;

            if (string.Compare(thisProduct.PCBID, newPartSn, true) == 0)
            {
                List<string> erpara = new List<string>();
                erpara.Add("SN");
                erpara.Add(newPartSn);
                var ex = new FisException("CHK083", erpara);
                ex.stopWF = false;
                throw ex;
            }
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
