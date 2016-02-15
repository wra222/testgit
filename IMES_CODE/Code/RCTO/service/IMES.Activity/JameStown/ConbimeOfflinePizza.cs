// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UC: mantis 1945
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    /// JameStown新机型; Conbime Offline Pizza
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
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
    ///         Session.Product
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
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class ConbimeOfflinePizza : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public ConbimeOfflinePizza()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Conbime Offline Pizza
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> errpara = new List<string>();

            Product curProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
            if (curProduct == null)
            {
                errpara.Add(this.Key);
                throw new FisException("SFC002", errpara);
            }

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            string PizzaID = (string)CurrentSession.GetValue(Session.SessionKeys.PizzaID);

            Pizza findPizza = pizzaRepository.Find(PizzaID);
            if (findPizza == null)
            {
                throw new FisException("CHK852", errpara); // 請刷正確的PizzaID
            }

            IProduct productByPizzaID = productRepository.GetProductByPizzaID(PizzaID);
            if (null != productByPizzaID)
            {
                throw new FisException("CHK1020", errpara); // 此PizzaID已结合SN
            }

            if (!curProduct.Model.Trim().Equals(findPizza.Model.Trim()))
            {
                throw new FisException("CHK1019", errpara); // 此PizzaID与SN 不匹配
            }

            curProduct.PizzaID = PizzaID;
            productRepository.Update(curProduct, CurrentSession.UnitOfWork);

            CurrentSession.AddValue(Session.SessionKeys.Pizza, findPizza);

            return base.DoExecute(executionContext);
        }

	}
}
