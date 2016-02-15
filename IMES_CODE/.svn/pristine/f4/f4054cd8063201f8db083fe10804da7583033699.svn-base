// INVENTEC corporation (c)201all rights reserved. 
// Description: 将用户的输入与BOM中的Part进行匹配
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-06   YangWeihua              create
// Known issues:
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 将用户的输入与BOM中的Part进行匹配
    ///匹配成功的料进行check, hold check
    ///匹配,check成功的料进行加入SessonBom
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于FA各个站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取用户输入的字符串Session.ValueToCheck;
    ///         2.将目标字符串在BOM中进行匹配；
    ///         3.Check matched part;
    ///         4.将匹配到的Part加入Session.BOM
    ///         5.将匹配到的Part放入Session.MatchedPart
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///            ArgumentException：系统无法读取存放RuleSet的rules文件
    ///            RuleSetValidationException：从rules读取的RuleSet无法通过语法校验
    ///         2.业务异常：
    ///            CHK005 check失败
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionBom
    ///         Session.ValueToCheck（string类型， 需要match的字符串）
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionBOM(记录已刷入part)
    ///         Session.MatchedPart(IPart类型，匹配到的part)
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         BOM
    ///         Session.SessionBOM
    /// </para> 
    /// </remarks>
    public partial class PartMatch : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public PartMatch()
		{
			InitializeComponent();
		}

        /// <summary>
        /// do part match and put matched part into Session.SessionKeys.MatchedParts
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            CurrentSession.AddValue(Session.SessionKeys.MatchedParts, null);
            string subject = CurrentSession.GetValue(Session.SessionKeys.ValueToCheck).ToString();
            var bom = (IFlatBOM)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            PartUnit matchedPart = bom.Match(subject, Station, AllowReplaceMatch);

            if (matchedPart != null)
            {
                try
                {
                    matchedPart.CurrentSession = CurrentSession;
                    IPartOwner owner = GetPartOwner(BindTo);
                    if (owner != null)
                    {
                        matchedPart.ProductId = owner.OwnerId;
                    }
                    if (!this.SkipPartCheck)
                    {
                        bom.Check(matchedPart, owner, Station);
                    }
                    bom.AddCheckedPart(matchedPart, AllowReplaceMatch);
                }
                catch (FisException ex)
                {
                    ex.stopWF = false;
                    throw;
                }
            }
            //else if (Station.Equals("ME"))
            //{
            //    //特殊情况：当灯管在无尘室与灯上灯下结合时，在online会刷灯管的CT，
            //    //此时需要带出灯上灯下的SN来。(从LCMBind表根据LCM CT#带出与之绑定的BTDL/TPDL sn来)
            //    ILCMRepository lcmRepository =
            //        RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();
            //    LCM lcm = lcmRepository.Find(subject);
            //    if (lcm != null && lcm.MEParts.Count > 0)
            //    {
            //        //                   var dicPartBomItem = new Dictionary<IBOMPart, BOMItem>();
            //        foreach (var me in lcm.MEParts)
            //        {
            //            IBOMPart matchedME = new BOMPart();
            //            matchedME = bom.Match(me.MESn, this.Station);
            //            if (matchedME != null)
            //            {
            //                try
            //                {
            //                    matchedME.Check(owner, this.Station);
            //                    matchedME.CheckHold(owner.Family, owner.Model);
            //                    bom.Check(matchedME);
            //                    //                                dicPartBomItem.Add(matchedME, bom.CurrentMatchedBOMItem);

            //                }
            //                catch (FisException ex)
            //                {
            //                    ex.stopWF = false;
            //                    throw;
            //                }
            //            }
            //            else
            //            {
            //                var erpara = new List<string> { subject, me.MESn };
            //                var ex = new FisException("CHK085", erpara) { stopWF = false };
            //                throw ex;
            //            }
            //        }

            //        foreach (var me in lcm.MEParts)
            //        {
            //            IBOMPart matchedME = new BOMPart();
            //            matchedME = bom.Match(me.MESn, this.Station);
            //            if (matchedME != null)
            //            {
            //                //bom.AddCheckedPart(matchedME);
            //                //IBOMPart tempPart = new BOMPart(matchedME.Part, matchedME.Customer, bom.CurrentMatchedBOMItem.Model);
            //                //tempPart.MatchedSn = matchedME.MatchedSn;
            //                //tempPart.ValueType = matchedME.ValueType;
            //                //matchedParts.Add(tempPart);
            //            }
            //            else
            //            {
            //                var erpara = new List<string> { subject, me.MESn };
            //                var ex = new FisException("CHK085", erpara) { stopWF = false };
            //                throw ex;
            //            }
            //        }

            //        foreach (var item in dicPartBomItem)
            //        {
            //            item.Value.AddCheckedPart(item.Key);
            //            matchedParts.Add(item.Key);
            //        }
            //    }
            //

            CurrentSession.AddValue(Session.SessionKeys.MatchedParts, matchedPart);
            
            return base.DoExecute(executionContext);
        }

        private IPartOwner GetPartOwner(PartMatch.BindToEnum bindTo)
        {
            switch (bindTo)
            {
                case PartMatch.BindToEnum.PCB:
                    IMB CurrentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
                    return CurrentMB;
                case PartMatch.BindToEnum.Pizza:
                    Pizza pizzaPartOwner = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
                    if (pizzaPartOwner == null)
                    {
                        Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                        if (newProduct != null)
                        {
                            pizzaPartOwner = newProduct.PizzaObj;
                        }
                    }
                    return pizzaPartOwner;
                case BindToEnum.Nothing:
                    return null;
                default:
                    Product productPartOwner = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                    return productPartOwner;
            }

        }

        /// <summary>
        /// whether need part check 
        /// </summary>
        public static DependencyProperty SkipPartCheckProperty = DependencyProperty.Register("SkipPartCheck", typeof(bool), typeof(PartMatch));

        /// <summary>
        /// SkipPartCheck
        /// </summary>
        [DescriptionAttribute("SkipPartCheck")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool SkipPartCheck
        {
            get
            {
                return ((bool)(base.GetValue(PartMatch.SkipPartCheckProperty)));
            }
            set
            {
                base.SetValue(PartMatch.SkipPartCheckProperty, value);
            }
        }

        /// <summary>
        ///  有重量异常的时候，是否停止工作流，共有两种，True不停止，false 停止
        /// </summary>
        public static DependencyProperty AllowReplaceMatchProperty = DependencyProperty.Register("AllowReplaceMatch", typeof(bool), typeof(PartMatch));

        /// <summary>
        /// NotStopWF:True Or False
        /// </summary>
        [DescriptionAttribute("AllowReplaceMatch")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool AllowReplaceMatch
        {
            get
            {
                return ((bool)(base.GetValue(PartMatch.AllowReplaceMatchProperty)));
            }
            set
            {
                base.SetValue(PartMatch.AllowReplaceMatchProperty, value);
            }
        }

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        public static DependencyProperty BindToProperty = DependencyProperty.Register("BindTo", typeof(PartMatch.BindToEnum), typeof(PartMatch));

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        [Description("BindTo")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public PartMatch.BindToEnum BindTo
        {
            get
            {
                return ((PartMatch.BindToEnum)(base.GetValue(PartMatch.BindToProperty)));
            }
            set
            {
                base.SetValue(PartMatch.BindToProperty, value);
            }
        }

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        public enum BindToEnum
        {
            /// <summary>
            /// 绑定到ProductPart
            /// </summary>
            Product = 1,

            /// <summary>
            /// 绑定到PCBPart
            /// </summary>
            PCB = 2,

            /// <summary>
            /// 绑定到PizzaPart
            /// </summary>
            Pizza = 4,

            /// <summary>
            /// for packing pizza
            /// </summary>
            Nothing = 5
        }
	}
}
