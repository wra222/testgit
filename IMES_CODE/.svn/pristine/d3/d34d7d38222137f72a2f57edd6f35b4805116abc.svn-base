/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 删除CartonSSCC表中对应的记录
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-05-05   Lucy Liu            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

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
using IMES.FisObject.Common.Misc;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PAK.CartonSSCC;
using IMES.FisObject.FA.Product;


namespace IMES.Activity
{
    /// <summary>
    /// 删除CartonSSCC表中对应的记录
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
    ///         Session.SessionKeys.CartonSNList
    ///        
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        FRUCarton
    /// </para> 
    /// </remarks>
    public partial class DeleteCartonSSCCForPOData: BaseActivity
	{

        public DeleteCartonSSCCForPOData()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            ICartonSSCCRepository cartonSSCCRepository = RepositoryFactory.GetInstance().GetRepository<ICartonSSCCRepository>();
	        IList<string> CartonSNList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.CartonSNList);

            cartonSSCCRepository.DeleteCartonSCC(CartonSNList);
            return base.DoExecute(executionContext);
        }
	}
}
