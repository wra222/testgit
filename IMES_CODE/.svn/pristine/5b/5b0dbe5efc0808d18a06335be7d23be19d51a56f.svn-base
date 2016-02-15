/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Set Product Model Object to Session for generating CustermerSN
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-01-19   Tong.Zhi-Yong     Create 
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 将Product Model对象放到Session中, 以便产生Customer SN
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Generate customer SN
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         检查Model是否存在，如果存在放到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.ModelName
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.ModelObj
    ///         Session.SessionKeys.ModelForRule
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IModelRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
	public partial class SetProductModel: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public SetProductModel()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 将Product Model对象放到Session中, 以便产生Customer SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string model = string.Empty;
            IProduct product = null;
            IModelRepository imr = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model modelObj = null;

            if (CurrentSession.GetValue(Session.SessionKeys.ECR) == null)
            {
                model = CurrentSession.GetValue(Session.SessionKeys.ModelName).ToString();
                modelObj = imr.Find(model);
                checkModelObj(modelObj, model, false);
                CurrentSession.AddValue(Session.SessionKeys.ModelObj, modelObj);
            }
            else
            {
                if (CurrentSession.GetValue(Session.SessionKeys.ModelObj) == null)
                {
                    product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                    checkModelObj(product.ModelObj, product.Model, true);
                    CurrentSession.AddValue(Session.SessionKeys.ModelObj, product.ModelObj);
                }

                CurrentSession.AddValue(Session.SessionKeys.ModelForRule, ((Model)CurrentSession.GetValue(Session.SessionKeys.ModelObj)).ModelName);
            }

            CurrentSession.AddValue(Session.SessionKeys.Qty, 1);

            return base.DoExecute(executionContext);
        }

        private void checkModelObj(Model modelObj, string model, bool stopWF)
        {
            if (modelObj == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(model);

                FisException ex = new FisException("CHK043", errpara);
                ex.stopWF = stopWF;
                CurrentSession.RemoveValue(Session.SessionKeys.ModelName);
                throw ex;
            }
        }
	}
}
