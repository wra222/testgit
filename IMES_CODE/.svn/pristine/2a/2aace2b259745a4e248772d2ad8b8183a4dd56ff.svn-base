/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: change part
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2009-11-24   Tong.Zhi-Yong                implement DoExecute method
 * 2010-03-01   Tong.Zhi-Yong                Modify ITC-1122-0162
 * 2012-01-11   Yang.Wei-Hua                 For IMES2012 SA Repair
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Station;
//using IMES.Infrastructure.Repository.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 更新维修的Part绑定记录, 对于Cause = ‘Key Part Fail’ 时，如果Faulty Part Sno and New Part Sno 均输入，则需要更新[PCB_Part] 中的相关记录，如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!”
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCB维修站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查是否Cause = ‘Key Part Fail’且 Faulty Part Sno and New Part Sno 均输入;
    ///         2.如果是, 更新通过IMBRepository更新Part绑定数据;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!” 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.PCAReapirDefectInfo
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
    ///         PCB_Part
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         PCARepair
    ///         PCARepairDefectInfo
    /// </para> 
    /// </remarks>
    public partial class UpdatePartsForDocking : BaseActivity
    {
        //private const string PCA_REPAIR_STATION = "23";
        //private const string OQC_REPAIR_STATION = "";
        //private const string FA_REPAIR_STATION = "45";
        //private const string PAK_REPAIR_STATION = "";

        ///<summary>
        /// 处理那个类型的维修(SA, FA, PAK)
        ///</summary>
        public static DependencyProperty RepairStationTypeProperty = DependencyProperty.Register("RepairStationType", typeof(string), typeof(UpdatePartsForDocking));

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
                return ((string)(base.GetValue(UpdatePartsForDocking.RepairStationTypeProperty)));
            }
            set
            {
                base.SetValue(UpdatePartsForDocking.RepairStationTypeProperty, value);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdatePartsForDocking()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新维修的Part绑定记录
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            IProductPart part = null;
            IRepairTarget repairTarget = GetRepairTarget();
            RepairDefect defect = null;
            defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);

            if (string.Compare(RepairStationType, "SA", true) == 0)
            {
                part = (IProductPart)CurrentSession.GetValue(Session.SessionKeys.PartID);

                if (part != null)
                {
                    part = getUpdatedPart(part, true, defect.NewPartSno, defect.PartType, "MB");
                }
            }
            //else if (station.StationType == StationType.FARepair || station.StationType == StationType.FAOperation)
            else if (string.Compare(RepairStationType, "FA", true) == 0 || string.Compare(RepairStationType, "OQC", true) == 0)
            {
                if (string.Compare(defect.PartType, "KP/ME", true) == 0)
                {
                    part = findPartBySn(defect.OldPartSno, ((IProduct)repairTarget).ProductParts);
                    //if old part exist, update Product_Parts table
                    if (part != null)
                    {
                        part = getUpdatedPart(part, true, defect.NewPartSno, defect.PartType, "Product");
                    }
                }
                else if (string.Compare(defect.PartType, "MB", true) == 0)
                {
                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    string type = (string)CurrentSession.GetValue("MBorVGA");
                    if (type == "DockingMB")
                    {
                        IMBRepository imr = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                        IProduct tempProduct = ((IProduct)repairTarget);
                        IMB newMB = imr.Find(defect.NewPartSno);

                        //update Product table
                        tempProduct.PCBID = defect.NewPartSno;
                        //PCBModel, MAC, UUID, MBECR, CVSN 2010-03
                        tempProduct.PCBModel = newMB.Model;
                        tempProduct.MAC = newMB.MAC;
                        tempProduct.UUID = newMB.UUID;
                        tempProduct.MBECR = newMB.ECR;
                        //0000873 issue 记录 FA FA Wang, Jenny W.H. (王紅 IES) 已分派 (Gao, Guan-Wei (高貫偉 ITC)) 2012-05-02 有些CPU号码没有在Product表里保存
                        if (!String.IsNullOrEmpty(newMB.CVSN))
                        {
                            tempProduct.CVSN = newMB.CVSN;
                        }

                        UpdateRepairTarget(repairTarget);
                    }
                    else if (type == "VGA")
                    {
                        IProduct tempProduct = ((IProduct)repairTarget);
                        foreach (ProductInfo temp in tempProduct.ProductInfoes)
                        {
                            if (temp.InfoType == "VGA" && temp.InfoValue == defect.OldPartSno.Trim())
                            {
                                temp.InfoValue = defect.NewPartSno.Trim();
                                break;
                            }
                        }
                        UpdateRepairTarget(repairTarget);
                    }
                    ProductPart setValue = new ProductPart();
                    setValue.PartSn = defect.NewPartSno.Trim();
                    ProductPart cond = new ProductPart();
                    cond.ProductID = ((IProduct)repairTarget).ProId;
                    cond.PartSn = defect.OldPartSno.Trim();
                    productRepository.UpdateProductPartDefered(CurrentSession.UnitOfWork, setValue, cond);
                }
            }
            else if (string.Compare(RepairStationType, "PAQC", true) == 0)
            {
                part = (IProductPart)CurrentSession.GetValue(Session.SessionKeys.PartID);

                if (part != null)
                {
                    part = getUpdatedPart(part, true, defect.NewPartSno, defect.PartType, "Product");
                }
            }

            if (part != null)
            {
                repairTarget.ChangePart(part.ID, part);
                UpdateRepairTarget(repairTarget);
            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 根據sn檢索old part是否在Product_Part中存在
        /// </summary>
        /// <param name="sno">old part sno</param>
        /// <param name="parts">list of ProductPart</param>
        /// <returns>IProductPart</returns>
        private IProductPart findPartBySn(string sno, IList<IProductPart> parts)
        {
            if (parts != null && parts.Count != 0)
            {
                foreach (IProductPart item in parts)
                {
                    if (string.Compare(item.PartSn, sno, true) == 0)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 根據pn檢索old part在Product_Part中存在, 並且還要滿足value=''的紀錄
        /// </summary>
        /// <param name="pn">old pn</param>
        /// <param name="parts">list of ProductPart</param>
        /// <returns>IProductPart</returns>
        private IProductPart findPartByPn(string pn, IList<IProductPart> parts)
        {
            if (parts != null && parts.Count != 0)
            {
                foreach (IProductPart item in parts)
                {
                    if (string.Compare(item.PartID, pn, true) == 0 && string.IsNullOrEmpty(item.Value))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        private IProductPart getUpdatedPart(IProductPart part, bool isSn, string val, string type, String objType)
        {
            IProductPart ret = null;

            if (string.Compare(objType, "MB", true) == 0)
            {
                IProductPart tempPart = part;

                if (isSn)
                {
                    //ret = new ProductPart(tempPart.ID, tempPart.PartID, tempPart.ProductID, val, string.Empty, tempPart.Station, tempPart.Editor, tempPart.Cdt, DateTime.Now);
                    ret = new ProductPart(tempPart.ID, tempPart.PartID, tempPart.ProductID, tempPart.Station, type, tempPart.CustomerPn, tempPart.Iecpn, val, tempPart.Editor, tempPart.Cdt, DateTime.Now);
                }
            }
            else
            {
                IProductPart tempPart = part;

                if (isSn)
                {
                    //ret = new ProductPart(tempPart.ID, tempPart.PartID, tempPart.ProductID, val, string.Empty, tempPart.Station, tempPart.Editor, tempPart.Cdt, DateTime.Now);
                    ret = new ProductPart(tempPart.ID, tempPart.PartID, tempPart.ProductID, tempPart.Station, tempPart.PartType, tempPart.CustomerPn, tempPart.Iecpn, val, tempPart.Editor, tempPart.Cdt, DateTime.Now);
                }
                else
                {
                    //ret = new ProductPart(tempPart.ID, tempPart.PartID, val, tempPart.Value, string.Empty, tempPart.Station, tempPart.Editor, tempPart.Cdt, DateTime.Now);
                    ret = new ProductPart(tempPart.ID, tempPart.PartID, val, tempPart.Station, type, tempPart.CustomerPn, tempPart.Iecpn, tempPart.PartSn, this.Editor, tempPart.Cdt, DateTime.Now);
                }
            }

            return ret;
        }
    }
}
