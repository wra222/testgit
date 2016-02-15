// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块保存上传的Delivery、Pallet、Delivery_Pallet数据
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块保存上传的Delivery、Pallet、Delivery_Pallet数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Upload Po Data for PL user(173/Normal/Domestic)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         将DeliveryList保存到Delivery表；
    ///         将PalletList保存到Pallet表；
    ///         将DeliveryPalletList保存到Delivery_Pallet表
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         DeliveryList, PalletList, DeliveryPalletList
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class UploadPoData : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadPoData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<Delivery> deliveryList = CurrentSession.GetValue("DeliveryList") as IList<Delivery>;
            IList<Pallet> palletList = CurrentSession.GetValue("PalletList") as IList<Pallet>;
            IList<DeliveryPalletInfo> dpList = CurrentSession.GetValue("DeliveryPalletList") as IList<DeliveryPalletInfo>;
                        
            foreach (Delivery ele in deliveryList)
            {
                deliveryRepository.Add(ele, CurrentSession.UnitOfWork);
            }

            foreach (Pallet ele in palletList)
            {
                palletRepository.Add(ele, CurrentSession.UnitOfWork);
            }

            foreach (DeliveryPalletInfo ele in dpList)
            {
                deliveryRepository.InsertDeliveryPalletDefered(CurrentSession.UnitOfWork, ele);
            }

            return base.DoExecute(executionContext);
        }
    }
}
