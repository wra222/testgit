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
using IMES.DataModel;
using System.Collections.Generic;
using System.Linq;
using IMES.FisObject.Common.Part;
using IMES.Common;


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
    ///         1. Delete IMES_PAK..Pizza / IMES_PAK..PizzaStatus / IMES_PAK..Pizza_Part – 解绑1st  and 2nd Pizza 盒相关资料
    ///         2.1st Pizza 盒的Pizza ID 保存在IMES_FA..Product.PizzaID
    ///          3.2nd Pizza 盒的Pizza ID 保存在IMES_FA..ProductInfo.InfoValue (Condition: InfoType = 'KIT2'
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
    public partial class UnPackDelPiazzPart : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackDelPiazzPart()
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
            Session session = CurrentSession;
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            Product newProduct = (Product)session.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Pizza pizzaPartOwner = newProduct.PizzaObj;
            Pizza pizzaPartOwner1 = newProduct.SecondPizzaObj;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            string customer = newProduct.Customer;
            IList<ConstValueInfo> conditionInfoList = utl.GetConstValueListByType(customer + "_UnpackSnByPartType", null);


            if (pizzaPartOwner != null)
            {
                IList<IProductPart> productPartList = pizzaPartOwner.PizzaParts;
                if (conditionInfoList != null && conditionInfoList.Count > 0)
                {
                    foreach (IProductPart item in productPartList)
                    {
                        UnPackPart.UnPackProduct(session, newProduct, conditionInfoList, item, this.Editor);
                    }
                }

                //Vincent add remove office CD log
                //ActivityCommonImpl commImp = ActivityCommonImpl.Instance;
                utl.UnPack.unPackPizzaOfficeCDLog(session.UnitOfWork, newProduct.PizzaID,newProduct.CUSTSN, this.Editor);

                repPizza.BackUpPizzaStatus(newProduct.PizzaID,this.Editor);
                repPizza.BackUpPizzaPart(newProduct.PizzaID ,this.Editor );
              
                pizzaPartOwner.RemoveAllParts();
               //repPizza.Update(pizzaPartOwner, CurrentSession.UnitOfWork);
                repPizza.Remove(pizzaPartOwner, session.UnitOfWork);               
                productRepository.UpdatePizzaIdForProduct(newProduct.ProId, newProduct.PizzaID);

            }

            if (pizzaPartOwner1 != null)
            {
                IList<IProductPart> productPartList = pizzaPartOwner1.PizzaParts;
                if (conditionInfoList != null && conditionInfoList.Count > 0)
                {
                    foreach (IProductPart item in productPartList)
                    {
                        UnPackPart.UnPackProduct(session, newProduct, conditionInfoList, item, this.Editor);
                    }
                }

                pizzaPartOwner1.RemoveAllParts();
              //  repPizza.Update(pizzaPartOwner1, CurrentSession.UnitOfWork);
                repPizza.Remove(pizzaPartOwner1, session.UnitOfWork);
                            
                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                string[] prodids = new string[1];
                prodids[0] = newProduct.ProId;

                currentProductRepository.DeleteProductInfo(prodids, "KIT2");
                
            }
      
            return base.DoExecute(executionContext);
        }
	}
}
