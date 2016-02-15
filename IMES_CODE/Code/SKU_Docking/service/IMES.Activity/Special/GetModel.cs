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
    /// 检查Model是否存在
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
    public partial class GetModel : BaseActivity
	{
        /// <summary>
        /// GetModel
        /// </summary>
        public GetModel()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 检查Model的合法性
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string currentModel = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName).ToString();
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model modelObj = modelRepository.Find(currentModel);

            if (modelObj == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentModel);

                FisException ex = new FisException("CHK164", errpara);
                CurrentSession.RemoveValue(Session.SessionKeys.ModelName);
                throw ex;
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.ModelObj, modelObj);
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
