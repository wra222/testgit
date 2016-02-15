// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  Update WH_PltWeight
// UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-24   Du Xuan (itc98066)          create
// ITC-1360-0662 按照UC修改相关赋值
// ITC-1360-1569 Insert WH_PLTLog
// Known issues:
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     CI-MES12-SPEC-PAK-UC Pallet Weight.docx  
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     4.	Update WH_PltWeight
    ///         ActualPltWeight – 实际重量 (From UI)
    ///         PltWeightInaccuracy – ABS((实际重量 – 标准重量（ForecasetPltWeight））/ 实际重量)
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///		     IPalletRepository       
    ///             
    /// </para> 
    /// </remarks>
    public partial class SavePalletWeightForDocking : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SavePalletWeightForDocking()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            decimal standWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.StandardWeight);
            decimal acturalWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
            decimal palletWeight = (decimal)CurrentSession.GetValue("PalletWeight");

            //Update Pallet
            //Weight – 实际重量
            //Weight_L – （实际重量 – 栈板重量）
            //Station – Pallet Weight 站号
            if (NeedUpdatePltWeight(CurrentPallet, acturalWeight))
            {
                CurrentPallet.Weight = acturalWeight;
                CurrentPallet.Weight_L = acturalWeight - palletWeight;
                CurrentPallet.Station = this.Station;
                CurrentPallet.Editor = this.Editor;
                CurrentPallet.Udt = DateTime.Now;

                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                PalletRepository.Update(CurrentPallet, CurrentSession.UnitOfWork);

                //Vincent: Add or Update PalletAttr.Name="SendStatus", PalletAttr.Value=""
                PalletRepository.UpdateAttrDefered(CurrentSession.UnitOfWork, CurrentPallet.PalletNo, "SendStatus", "", "", this.Editor);
            }
            //7.Insert WH_PLTLog
            WhPltLogInfo newLog = new WhPltLogInfo();
            newLog.plt = CurrentPallet.PalletNo;
            newLog.editor = this.Editor;
            newLog.wc = "99";
            newLog.cdt = DateTime.Now;

            palletRep.InsertWhPltLogDefered(CurrentSession.UnitOfWork, newLog);

            return base.DoExecute(executionContext);
        }
        private bool NeedUpdatePltWeight(Pallet plt, decimal actualPltWeight)
        {
            bool needupdateweight = true;
            if (NeddCheckWeight)
            {
                IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                decimal lastpalletweight = plt.Weight;
                IList<string> ToleranceLst = new List<string>();
                string sysSettingUnitTolerance = "";
                decimal tolerancePercent;
                ToleranceLst = ipartRepository.GetValueFromSysSettingByName("PLTWeightTolerance");
                if (ToleranceLst != null && ToleranceLst.Count > 0)
                {
                    sysSettingUnitTolerance = ToleranceLst[0].ToString();
                }
                else
                {
                    sysSettingUnitTolerance = "3";
                }

                if (!decimal.TryParse(sysSettingUnitTolerance.Replace("%", ""), out tolerancePercent))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sysSettingUnitTolerance);
                    ex = new FisException("CHK017", erpara);
                    throw ex;
                }
                //mantis:001172 如果栈板重复称重，如果误差大于3%，需要报错，小于3%，不需要更新重量
                if (lastpalletweight > 0)//2次称重
                {
                    if (tolerancePercent < 100 * System.Math.Abs((actualPltWeight - lastpalletweight) / lastpalletweight))
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add(actualPltWeight.ToString());
                        erpara.Add(lastpalletweight.ToString());
                        FisException e = new FisException("CHK1100", erpara);
                        throw e;
                    }
                    else
                    {
                        needupdateweight = false;
                    }
                }
                else//首次称重
                {
                    needupdateweight = true;
                }
            }
            return needupdateweight;
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NeddCheckWeightProperty = DependencyProperty.Register("NeddCheckWeight", typeof(bool), typeof(SavePalletWeightForDocking), new PropertyMetadata(false));

        /// <summary>
        /// 是否检查重量误差
        /// </summary>
        [DescriptionAttribute("NeddCheckWeight")]
        [CategoryAttribute("NeddCheckWeight Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NeddCheckWeight
        {
            get
            {
                return ((bool)(base.GetValue(SavePalletWeightForDocking.NeddCheckWeightProperty)));
            }
            set
            {
                base.SetValue(SavePalletWeightForDocking.NeddCheckWeightProperty, value);
            }
        }
    }
}
