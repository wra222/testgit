/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Generate Customer SN For RCTO Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2012/2/6 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2012/5/10           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-10   Jessica Liu           Create
* Known issues:
* TODO：
*/

//using System;
//using System.ComponentModel;
//using System.ComponentModel.Design;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Workflow.ComponentModel;
//using System.Workflow.ComponentModel.Design;
//using System.Workflow.ComponentModel.Compiler;
//using System.Workflow.ComponentModel.Serialization;
//using System.Workflow.Runtime;
//using System.Workflow.Activities;
//using System.Workflow.Activities.Rules;
//using IMES.FisObject.FA.Product;
using System;
using System.Workflow.ComponentModel;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;


namespace IMES.Activity
{

    /// <summary>
    /// Check Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Generate Customer SN(Generate CPQ SNO RCTO)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.若Product的当前状态为Fail（ProductStatus.Status=0），则报错：“请先修护后，再刷本站”
    ///         2.若Product.CUSTSN不为空且不为Null，则报错：“已经产生过Customer SN，不需要重新产生”
    ///         3.若Left(Product.Model,3)=’173’ or ’146’，且Upper(Model.Family)!= ’ JOURNEY 1.0’，则报错：“该RCTO机型，不需要产生Customer SN”
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckProductForGenerateCustomerSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductForGenerateCustomerSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            //从Session里取得Product对象
            IProduct curProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            //若Product的当前状态为Fail（ProductStatus.Status=0），则报错：“请先修护后，再刷本站”
            if (curProduct.Status.Status == 0)
            {
                erpara.Add(this.Key);
                ex = new FisException("CHK283", erpara);
                throw ex;
            }

            //若Product.CUSTSN不为空且不为Null，则报错：“已经产生过Customer SN，不需要重新产生”
            if (!string.IsNullOrEmpty(curProduct.CUSTSN))
            {
                erpara.Add(this.Key);
                ex = new FisException("CHK910", erpara); 
                throw ex;
            }

            //若left(Product.Model,3)=’173’ or ‘146’，且Upper(Model.Family)<>’JOURNEY 1.0’，则报错：“该RCTO机型，不需要产生Customer SN”
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model curModel = modelRep.Find(curProduct.Model);
            string tempStr = curProduct.Model.Substring(0, 3);
            if (((tempStr == "173") || (tempStr == "146")) && (curModel.Family.FamilyName.ToString().ToUpper().Trim() != "JOURNEY 1.0"))
            {
                erpara.Add(this.Key);
                ex = new FisException("CHK911", erpara); 
                throw ex;
            }

            return base.DoExecute(executionContext);
        }

    }
}

