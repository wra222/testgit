// Description: 
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
    ///         1.从Session中获取Pallet，调用Pallet的UpdateProductStatus方法
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
    ///         塞入ProductLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              ICartonRepository
    ///              Carton
    ///              ProductLog
    /// </para> 
    /// </remarks>
    public partial class WriteProductLogByPallet : BaseActivity
    {
        public WriteProductLogByPallet()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteProductLogByPallet));

        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WriteProductLogByPallet.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteProductLogByPallet.StatusProperty, value);
            }
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            String currentPallet = (String)CurrentSession.GetValue(Session.SessionKeys.Pallet);


            IList<ProductLog> logList = new List<ProductLog>();

            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();


            IList<IMES.DataModel.ProductModel> productModelList = ProductRepository.GetProductListByPalletNo(currentPallet);


            foreach (IMES.DataModel.ProductModel model in productModelList)
            {
                ProductLog newLog = new ProductLog();

                newLog.Editor = Editor;
                newLog.Line = Line;
                newLog.Station = Station;
                newLog.Status = Status;
                newLog.ProductID = model.ProductID;
                newLog.Model = model.Model;

                logList.Add(newLog);

            }


            ProductRepository.WriteProductListLogDefered(CurrentSession.UnitOfWork, logList);

            return base.DoExecute(executionContext);

        }
    }
}
