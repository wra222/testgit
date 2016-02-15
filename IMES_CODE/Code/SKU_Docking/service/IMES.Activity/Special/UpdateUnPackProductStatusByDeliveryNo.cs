// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，更新属于改DeliveryNo的所有Product的状态
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-18   Yuan XiaoWei                 create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Data.SqlClient;
using System;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据DeliveryNo号码更新Product的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的UpdateUnPackProductStatusByDeliveryNoDefered方法
    ///     update ProductStatus set Status =@Status,Line=@Line,Station=@Station,Editor=@Editor,Udt=GETDATE()
    ///     from ProductStatus as S inner join Product as P ON S.ProductID =P.ProductID
    ///     WHERE P.DeliveryNo =@DeliveryNo and (Model LIKE 'PC%' or Model LIKE 'QC%')
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    public partial class UpdateUnPackProductStatusByDeliveryNo : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateUnPackProductStatusByDeliveryNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateUnPackProductStatusByDeliveryNo));

        /// <summary>
        /// 要修改成的Product的状态
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("InArguments Of UpdateUnPackProductStatusByDeliveryNo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateUnPackProductStatusByDeliveryNo.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateUnPackProductStatusByDeliveryNo.StatusProperty, value);
            }
        }

        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentDeliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

            ProductStatus newStatus = new ProductStatus();
            newStatus.Editor = Editor;
            newStatus.Line = Line;
            newStatus.StationId = Station;
            newStatus.Status =  Status;
            newStatus.ReworkCode = "";
            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            currentProductRepository.ExecSpForNonQueryDefered(CurrentSession.UnitOfWork,
                                                                                          IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_FA,
                                                                                          "IMES_DeliveryProductStatus",
                                                                                         new SqlParameter("deliveryNo",CurrentDeliveryNo),
                                                                                         new SqlParameter("station", Station),
                                                                                         new SqlParameter("status", Status == StationStatus.Pass ? 1 : 0),
                                                                                         new SqlParameter("line", Line),
                                                                                         new SqlParameter("editor", Editor),
                                                                                         new SqlParameter("udt", DateTime.Now)
                                                                                         );

            currentProductRepository.UpdateUnPackProductStatusByDeliveryNoDefered(CurrentSession.UnitOfWork, newStatus, CurrentDeliveryNo);
            return base.DoExecute(executionContext);
        }
	}
}
