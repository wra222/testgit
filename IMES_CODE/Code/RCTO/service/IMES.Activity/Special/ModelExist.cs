// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Check MAC 
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-08   210003                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.Common.Line;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{
    /// <summary>
    /// 检查刷入的model，在Model表里是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         刷入的model
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       Session.SessionKeys.ModelExist（true：存在，false：不存在）
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        Product
    /// </para> 
    /// </remarks>
    public partial class ModelExist : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public ModelExist()
		{
			InitializeComponent();
		}
         /// <summary>
        /// 刷入Model，string型。
        /// </summary>
        public static DependencyProperty modelProperty = DependencyProperty.Register("model", typeof(string), typeof(ModelExist));
        /// <summary>
        /// model
        /// </summary>
        [DescriptionAttribute("model")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string model
        {
            get
            {
                return ((string)(base.GetValue(ModelExist.modelProperty)));
            }
            set
            {
                base.SetValue(ModelExist.modelProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            bool ret = false;
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model objModel = modelRepository.Find(model);
            if (objModel != null)
                ret = true;
            CurrentSession.AddValue(Session.SessionKeys.ModelName, ret);
            return base.DoExecute(executionContext);
        }
	}
}
