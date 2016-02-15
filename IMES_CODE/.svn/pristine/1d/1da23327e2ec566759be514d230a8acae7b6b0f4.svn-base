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
    /// JameStown新机型; OfflinePizzaFamily 區別是否需要Pizza Id及 PizzaID Label(不需要打印)
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
    public partial class CheckOfflinePizzaFamily : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckOfflinePizzaFamily()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (curProduct == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("SFC002", errpara);
            }

            // OfflinePizzaFamily : 是否需要Pizza Id及 PizzaID Label(不需要打印)
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string keyOfflinePizzaFamily = "OfflinePizzaFamily";

            string isOfflinePizzaFamily = "N";
            IList<ConstValueTypeInfo> lstCnst = partRepository.GetConstValueTypeList(keyOfflinePizzaFamily);
            foreach (ConstValueTypeInfo cv in lstCnst)
            {
                if (cv.value.Equals(curProduct.Family))
                {
                    isOfflinePizzaFamily = "Y";
                    break;
                }
            }

            CurrentSession.AddValue(keyOfflinePizzaFamily, isOfflinePizzaFamily);

            return base.DoExecute(executionContext);
        }

	}
}
