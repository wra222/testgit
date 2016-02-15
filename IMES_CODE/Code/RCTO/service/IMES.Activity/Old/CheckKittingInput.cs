/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.TestLog;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{
    /// <summary>
    /// 检查亮灯系统数据合法性
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
    ///         Session.SessionKeys.Product 
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
    public partial class CheckKittingInput : BaseActivity
    {
        public CheckKittingInput()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model model = modelRep.Find(prod.Model);

            if (model == null)
            {
                List<string> erpara = new List<string>();
                erpara.Add(prod.Model);
                FisException ex = new FisException("CHK038", erpara);
                ex.stopWF = true;
                throw ex;
            }

            //若有空格，只取空格前字串
            string family = model.Family.FamilyName;
            int emptyString = family.IndexOf(" ");
            if(emptyString > 0 )
            {
                family = family.Substring(0, emptyString);
            }

            if (!productRepository.CheckKitting(family, prod.Model, CurrentSession.GetValue(Session.SessionKeys.Floor).ToString()))
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("CHK010", erpara);
                ex.stopWF = true;
                throw ex;
            }

            return base.DoExecute(executionContext);
        }


    }
}
