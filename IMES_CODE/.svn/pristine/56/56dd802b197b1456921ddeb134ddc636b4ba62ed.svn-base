/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: GenerateCustomerSN
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-12-15   Tong.Zhi-Yong                implement DoExecute method
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using System.Collections.Generic;
using IMES.FisObject.Common.PrintLog;
namespace IMES.Activity
{
    /// <summary>
    /// 產生CustomerSN號相关逻辑
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
    ///         更新Product.CUSTSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
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
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateCustomerSnForBN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateCustomerSnForBN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 產生CustomerSN號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository< IProductRepository, IProduct>();

            
        
            string custSn = string.Empty;
   
            custSn = CurrentSession.GetValue(Session.SessionKeys.CustSN).ToString();
            ////Check new CustSN is existing or not
            //var testProduct = ipr.GetProductByCustomSn(custSn);
            //if (testProduct != null)
            //{
            //    List<string> param = new List<string>();
            //    param.Add(custSn);
            //    throw new FisException("GEN046", param);
            //}
             
            ////Check new CustSN is existing or not



            product.CUSTSN = custSn;
         
            ipr.Update(product, CurrentSession.UnitOfWork);
            // For Write Print Log........
          
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName,this.Customer + "SNO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo,custSn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo,custSn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr,CurrentSession.GetValue(Session.SessionKeys.ModelName));


            var item = new  PrintLog
            {
                Name = CurrentSession.GetValue(Session.SessionKeys.PrintLogName).ToString(),
                BeginNo = CurrentSession.GetValue(Session.SessionKeys.PrintLogBegNo).ToString(),
                EndNo = CurrentSession.GetValue(Session.SessionKeys.PrintLogEndNo).ToString(),
                Descr = CurrentSession.GetValue(Session.SessionKeys.PrintLogDescr).ToString(),
                Editor = this.Editor
            };
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            repository.Add(item, CurrentSession.UnitOfWork);
          


            return base.DoExecute(executionContext);
        }

        //private void InitializeComponent()
        //{
        //    // 
        //    // GenerateCustomerSnForBN
        //    // 
        //    this.Customer = null;
        //    this.Editor = null;
        //    this.Key = null;
        //    this.Line = null;
        //    this.Name = "GenerateCustomerSnForBN";
        //    this.SessionType = 0;
        //    this.Station = null;

        //}
    }
}
