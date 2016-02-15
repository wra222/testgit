// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Carton号码，更新属于改Carton的所有Product的状态
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
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
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据Carton号码更新Product的状态
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
    ///         1.从Session中获取Carton，调用Carton的UpdateProductStatus方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Carton
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
    ///              ICartonRepository
    ///              Carton
    ///              ProductStatus
    /// </para> 
    /// </remarks>
    public partial class UpdateProductStatusByCarton : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateProductStatusByCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateProductStatusByCarton));

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
                return ((StationStatus)(base.GetValue(UpdateProductStatusByCarton.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusByCarton.StatusProperty, value);
            }
        }

        /// <summary>
        /// 执行根据Carton修改所有属于该Carton的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string currentCarton = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);
            if (currentCarton == null||currentCarton=="")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK109", errpara);
            }
            ProductStatus newStatus = new ProductStatus();
            newStatus.Editor = Editor;
            newStatus.Line = Line;
            newStatus.StationId = Station;
            newStatus.Status =  Status;
            newStatus.ReworkCode = "";
            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            //currentProductRepository.UpdateProductStatusByCartonDefered(CurrentSession.UnitOfWork, currentCarton, newStatus);
            currentProductRepository.UpdateProductListReworkDefered(CurrentSession.UnitOfWork, newStatus, null, currentCarton);
            return base.DoExecute(executionContext);
        }
	}
}
