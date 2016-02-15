// Description: 根据Pallet号码，更新属于该Pallet的所有Product的状态
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-14   Shao.Rong-hua                create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
//using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据Pallet号码更新Product的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PalletWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Pallet，调用Product的GetProductListByPalletNo、UpdateProductListStatus方法
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
    public partial class UpdateProductStatusByPallet : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateProductStatusByPallet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateProductStatusByPallet));

        /// <summary>
        /// 要修改成的Product的状态
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateProductStatusByPallet.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusByPallet.StatusProperty, value);
            }
        }

        /// <summary>
        /// 执行根据Carton修改所有属于该Carton的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pallet currentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);

            String PalletNo = currentPallet.PalletNo;

            ProductStatus newStatus = new ProductStatus();
            newStatus.Editor = Editor;
            newStatus.Line = Line;
            newStatus.StationId = Station;
            newStatus.Status = Status;
            newStatus.ReworkCode = "";


            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            List<string> productIDList = new List<string>();

            productIDList = productRepository.GetProductIDListByPalletNo(PalletNo);



//            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);


//            IList<string> ProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            productRepository.UpdateProductListStatusDefered(CurrentSession.UnitOfWork, newStatus, productIDList);


            
            return base.DoExecute(executionContext);
        }
    }
}
