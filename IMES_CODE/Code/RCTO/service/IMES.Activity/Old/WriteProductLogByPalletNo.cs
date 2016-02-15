// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于根据Pallet号码记录ProuctLog
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-11   Lucy Liu                     create
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
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据Pallet号码记录ProuctLog(070UnpackPallet.xoml)
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CartonWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取PalletNo，调用WriteProductLogByPalletNoDefered方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PalletNo
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
    ///              ProductLog
    /// </para> 
    /// </remarks>
	public partial class WriteProductLogByPalletNo: BaseActivity
	{
        public WriteProductLogByPalletNo()
		{
			InitializeComponent();
		}


        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteProductLogByPalletNo));

        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WriteProductLogByPalletNo.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteProductLogByPalletNo.StatusProperty, value);
            }
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string palletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            ProductLog newLog = new ProductLog();
            newLog.Editor = Editor;
            newLog.Line = Line;
            newLog.Station = Station;
            newLog.Status = Status;

           
            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            currentProductRepository.WriteProductLogByPalletNoDefered(CurrentSession.UnitOfWork, palletNo, newLog);
            return base.DoExecute(executionContext);
        }
	}
}
