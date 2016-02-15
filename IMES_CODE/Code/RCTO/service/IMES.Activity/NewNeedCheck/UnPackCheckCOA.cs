// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                   create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的Update方法
    ///           update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnPackCheckCOA : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackCheckCOA()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            Pizza pizzaPartOwner = newProduct.PizzaObj;
            Pizza pizzaPartOwner1 = newProduct.SecondPizzaObj;
            List<string> pnList = new List<string>() ;
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            bool isfund = false;
            string pizzdTwo = null;
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            if (pizzaPartOwner1 != null)
            {
                pizzdTwo =(string) newProduct.GetExtendedProperty("KIT2");                
            }
            if (pizzaPartOwner != null)
            {
                string[] pizzIds;
               // IList<string> GetPartNoListFromPizzaPart(string[] pizzaIds, string bomNodeType, string infoType, string infoValue);
                if(pizzdTwo==null || pizzdTwo==""){
                     pizzIds = new string[]{pizzaPartOwner.PizzaID};
                }
                else if (pizzaPartOwner.PizzaID == null || pizzaPartOwner.PizzaID == "")
                {
                    pizzIds = new string[] { pizzdTwo };
                }
                else{
                    pizzIds = new string[] { pizzaPartOwner.PizzaID, pizzdTwo };
                }

                IList<string> partsn = repPizza.GetPartNoListFromPizzaPart(pizzIds, "P1", "DESC", "OOA");

                if (partsn.Count>0)
                {
                    CurrentSession.AddValue(Session.SessionKeys.COASN, partsn[0]);
                    isfund = true;
                }
               
            }
            CurrentSession.AddValue(Session.SessionKeys.HasDefect, isfund);
            CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, newProduct.DeliveryNo);
            return base.DoExecute(executionContext);
        }
	}
}
