// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判定Model是否有效
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-08   Kerwin                       create
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA;
using IMES.FisObject.Common.Model;
namespace IMES.Activity
{

    /// <summary>
    /// 判定Model是否有效 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-BT_CHANGE
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.所输Model长度不等于12或者其不是一个已存在的Model，提示Wrong Code
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK011
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ModelName
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Model
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckModelValid : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckModelValid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 所输Model长度不等于12或者其不是一个已存在的Model，提示Wrong Code
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得ModelName
            string currentModelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);

            if(string.IsNullOrEmpty(currentModelName) || currentModelName.Length!=12){
                throw new FisException("PAK011", new string[] { currentModelName });
            }

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model currentModel = currentRepository.Find(currentModelName);
            if (currentModel == null)
            {
                throw new FisException("PAK011", new string[] { currentModelName });
            }                
            return base.DoExecute(executionContext);
        }

    }
}

