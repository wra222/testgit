/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Packing Pizza Page
 * UI:CI-MES12-SPEC-PAK-UI Packing Pizza.docx –2011/11/07
 * UC:CI-MES12-SPEC-PAK-UC Packing Pizza.docx –2011/11/07            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-07   zhu lei               Create
* Known issues:
* TODO：
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{
    /// <summary>
    /// 检查SessionBom是否为空
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Packing Pizza
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
    ///         Model 
    ///         
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
    ///         IModelRepository
    ///         IModel
    /// </para> 
    /// </remarks>
    public partial class CheckBomNULL : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBomNULL()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 检查SessionBom是否为空
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentBom = CurrentSession.GetValue(Session.SessionKeys.SessionBom);

            if (currentBom == null)
            {
                List<string> errpara = new List<string>();

                FisException ex = new FisException("CHK165", errpara);
                throw ex;
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
