// INVENTEC corporation (c)2009 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC PCA Shipping Label Print.docx
//              Check MBCT Exist    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-08   zhu lei                      create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// MBCT是否存在
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
    ///         Session.SessionKeys.MB 
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
    ///        Product
    /// </para> 
    /// </remarks>
    public partial class CheckMBCT2Exist : BaseActivity
    {
        /// <summary>
        /// CheckMBCTExist
        /// </summary>
        public CheckMBCT2Exist()
        {
            InitializeComponent();
        }

        /// <summary>
        /// MBCT是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (CurrentProduct == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            string PCBNO = CurrentProduct.PCBID.ToString();
           
            //MBCT2是否存在
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            string MBCT2 = mbRepository.GetPCBInfoValue(PCBNO, "MBCT2");
            //DEBUG for boardinput （kaisheng 2012/03/14）
            if (MBCT2 == "" || MBCT2 == null)
            {
                //CurrentSession.AddValue(Session.SessionKeys.ifElseBranch, true);
                throw new NullReferenceException("该机器没有MBCT2");
            }
            CurrentSession.AddValue("MBCT2", MBCT2);
            return base.DoExecute(executionContext);
        }

    }
}
