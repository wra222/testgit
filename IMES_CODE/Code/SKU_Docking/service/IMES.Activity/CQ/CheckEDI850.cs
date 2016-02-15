
using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
namespace IMES.Activity
{
    /// <summary>
    ///CheckIsAOILine
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
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckEDI850 : BaseActivity
	{
		///<summary>
		///</summary>
        public CheckEDI850()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            IProduct  prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (string.IsNullOrEmpty(prod.DeliveryNo))
            {
                return base.DoExecute(executionContext);
            }
            Delivery dn = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            if (dn == null)
            {
                dn=dnRep.Find(prod.DeliveryNo);
            }

            if (dn == null)
            {
                throw new FisException("PAK129", new string[] { prod.DeliveryNo });
            }
            if ((string)dn.GetExtendedProperty("Flag") == "N")
            {
                IList<string> docnumList  = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(dn.DeliveryNo.Substring(0, 10));
                if (docnumList == null || docnumList.Count==0)
                {
                    throw new FisException("CQCHK1011", new string[] { prod.DeliveryNo });
                }

                if (!dnRep.CheckEDI850ByHPPoNum(dn.PoNo))
                {
                    throw new FisException("CQCHK1010", new string[] { prod.DeliveryNo });
                }
            }          
            return base.DoExecute(executionContext);
        }
	}
}
