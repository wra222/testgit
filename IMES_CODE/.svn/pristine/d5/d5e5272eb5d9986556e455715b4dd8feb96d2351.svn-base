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
using IMES.Common;

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
    public partial class MasterLabel_CheckJamestown : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public MasterLabel_CheckJamestown()
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
			
			CurrentSession.AddValue("isJamestown", "N");
			//if ("JC".Equals(curProduct.Model.Substring(0, 2)))
            if (ActivityCommonImpl.Instance.CheckModelByProcReg(curProduct.Model, "ThinClient"))
			{
				CurrentSession.AddValue("isJamestown", "Y");
				CurrentSession.AddValue(Session.SessionKeys.CN, "T");
			}

            return base.DoExecute(executionContext);
        }

	}
}
