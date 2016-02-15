// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的UpdateProductList中的ProductID列表，
//              更新列表中所有Product的状态
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
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
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 用于更新一组Product的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PAK Pallet Data Collection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取NewScanedProductIDList，调用ProductRepository.UpdateProductListStatus
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.NewScanedProductIDList
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
    ///         更新ProductStatus  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              ProductStatus
    /// </para> 
    /// </remarks>
    public partial class UpdateProductListStatus : BaseActivity
    {
        public UpdateProductListStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateProductListStatus));

        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateProductListStatus.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateProductListStatus.StatusProperty, value);
            }
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            ProductStatus newStatus = new ProductStatus();
            newStatus.Editor = Editor;
            newStatus.Line = Line;
            newStatus.StationId = Station;
            newStatus.Status = Status;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IList<string> ProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            //productRepository.UpdateProductListStatusDefered(CurrentSession.UnitOfWork, newStatus, ProductIDList);
            productRepository.UpdateProductListReworkDefered(CurrentSession.UnitOfWork, newStatus, ProductIDList, "");

            return base.DoExecute(executionContext);
        }
    }
}
