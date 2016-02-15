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
    public partial class UnPackCOA : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackCOA()
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
           // Pizza pizzaPartOwner1 = newProduct.SecondPizzaObj;

            IList<PizzaPart> currentPizzaPart = (IList<PizzaPart>)pizzaPartOwner.PizzaParts;
           // IList<PizzaPart> currentPizzaPart1 = (IList<PizzaPart>)pizzaPartOwner1.PizzaParts;

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(newProduct.Model);
            IList<IBOMNode> bomNodeList =  bom.FirstLevelNodes;

            foreach (IBOMNode bomNode in bomNodeList)
            {
               if ((bomNode.Part.BOMNodeType == "P1") && ((bomNode.Part.GetProperty("DESC")== "OOA")))
               {
                   FisException ex;
                   List<string> erpara = new List<string>();
                   erpara.Add("该Unit 已经结合了Office COA，不能Unpack! 请先将Office COA Unpack.");
                   ex = new FisException("CHK020", erpara);
                   throw ex;
               }
            }

            //currentProductRepository.UpdateProductsForUnboundDefered(CurrentSession.UnitOfWork, CurrentDeliveryNo);
            return base.DoExecute(executionContext);
        }
	}
}
