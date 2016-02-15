// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Write ProductLog by Dn
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21   Kerwin                       create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据DeliveryNo号码Insert ProductLog 
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
    ///         1.从Session中获取DeliveryNo，调用WriteUnPackProductLogByDeliveryNoDefered方法
    ///         insert into ProductLog(Line,Model,ProductID,Station,Status,Editor,Cdt)
    ///         select @Line,Model,ProductID,@Station,@Status,@Editor,GETDATE()
    ///         from Product 
    ///         where DeliveryNo =@DeliveryNo and (Model LIKE 'PC%' or Model LIKE 'QC%')
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
    ///         Insert ProductLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              ProductLog
    /// </para> 
    /// </remarks>
    public partial class WriteUnPackProductLogByDeliveryNo : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public WriteUnPackProductLogByDeliveryNo()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Status of productLog
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteUnPackProductLogByDeliveryNo));

        /// <summary>
        /// Status of productLog
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("InArguments of WriteUnPackProductLogByDeliveryNo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WriteUnPackProductLogByDeliveryNo.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteUnPackProductLogByDeliveryNo.StatusProperty, value);
            }
        }

        /// <summary>
        /// insert into productlog by dn
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentDeliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            ProductLog newLog = new ProductLog();
            newLog.Editor = Editor;
            newLog.Line = Line;
            newLog.Station = Station;
            newLog.Status = Status;

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            currentProductRepository.WriteUnPackProductLogByDeliveryNoDefered(CurrentSession.UnitOfWork, CurrentDeliveryNo, newLog);
            return base.DoExecute(executionContext);
        }
    }
}
