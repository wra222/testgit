// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Check MAC 
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   207006                       create
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.FRU;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// check fru carton表存在该号,且FRUCarton_Part存在
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
    ///         Session.SessionKeys.Carton 
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
    ///        frucarton
    /// </para> 
    /// </remarks>
    public partial class CheckFRUCarton : BaseActivity
	{
        public CheckFRUCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// check fru carton表存在该号,且FRUCarton_Part存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string cartonNo =(String) this.CurrentSession.GetValue(Session.SessionKeys.Carton);
            IFRUCartonRepository fruCartonRep = RepositoryFactory.GetInstance().GetRepository<IFRUCartonRepository>();

            FRUCarton fr = fruCartonRep.Find(cartonNo);
            if (fr == null)
            {
                throw new FisException("CHK105", new List<string>());
            }
            if (fr.Parts == null || fr.Parts.Count == 0)
            {
                throw new FisException("CHK105", new List<string>());
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
