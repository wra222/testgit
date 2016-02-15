/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:CI-MES12-SPEC-SA-UC MB Label Print.docx
 *             获取SMTMO
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-03   zhu lei           Create 
 * 
 * Known issues:
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
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 获得SMTMO对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于MBLabelPrint
    ///         
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
    ///         Session.MBMONO
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.MBMO
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBMORepository
    /// </para> 
    /// </remarks>
    public partial class GetSMTMO: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public GetSMTMO()
		{
			InitializeComponent();
		}
        /// <summary>
        /// GetSMTMO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbmoSn=CurrentSession.GetValue(Session.SessionKeys.MBMONO).ToString();
           
            IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();

            var mbmo = mbmoRepository.Find(mbmoSn);

            //if (mbmo == null)
            //{
            //    var ex = new FisException("CHK011", new string[] { mbmoSn });
            //    throw ex;
            //}

            CurrentSession.AddValue(Session.SessionKeys.MBMO , mbmo);
            return base.DoExecute(executionContext);
        }

	}
}
