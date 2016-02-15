// INVENTEC corporation (c)2010 all rights reserved. 
// Description: DN刷满时，更新Delivery的Status
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-12   Yuan XiaoWei                 create
// 2010-05-12   Yuan XiaoWei                 ITC-1155-0095
// Known issues:
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
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{


    /// <summary>
    /// DN刷满时，更新Deliveryt的Status
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于更新Deliveryt的Status
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Delivery对象
    ///         2.更新状态
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Delivery
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
    ///         更新Deliveryt的Status
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              Delivery
    /// </para> 
    /// </remarks>
    public partial class UpdateDNStatusWhenDNPalletFull : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateDNStatusWhenDNPalletFull()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行修改Delivery的Status，Editor，Udt的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Delivery CurrentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            DeliveryRepository.UpdateAllDNStatusWhenPalletFullDefered(CurrentSession.UnitOfWork, CurrentDelivery.DeliveryNo, CurrentPallet.PalletNo, this.Status, this.Editor);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(UpdateDNStatusWhenDNPalletFull));

        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(UpdateDNStatusWhenDNPalletFull.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateDNStatusWhenDNPalletFull.StatusProperty, value);
            }
        }
    }
}
