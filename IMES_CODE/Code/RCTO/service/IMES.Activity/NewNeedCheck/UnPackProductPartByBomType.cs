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
using IMES.FisObject.Common.Part;
using System.Collections.Generic;

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
    public partial class UnPackProductPartByBomType : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackProductPartByBomType()
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
            Product productPartOwner = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string ProId = productPartOwner.ProId;

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            IList<ProductPart> part_P1 = currentProductRepository.GetProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, "P1", "Royalty");
            if (part_P1.Count > 0)
            {
                currentProductRepository.BackUpProductPartByBomNodeTypeAndDescrLike(ProId, this.Editor, "P1", "Royalty%");
                currentProductRepository.RemoveProductPartByBomNodeTypeAndDescrLike(ProId, "P1", "Royalty%");
                currentProductRepository.Update(productPartOwner, CurrentSession.UnitOfWork);
            }
          
            IList<ProductPart> part_HomeCard = currentProductRepository.GetProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, "P1", "Home Card");
            if (part_HomeCard.Count > 0)
            {
                currentProductRepository.BackUpProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, this.Editor, "P1", "Home Card");
                currentProductRepository.RemoveProductPartByBomNodeTypeAndDescrLike(ProId,"P1", "Home Card");
                currentProductRepository.Update(productPartOwner, CurrentSession.UnitOfWork);
            }
            IList<ProductPart> part_NYLON = currentProductRepository.GetProductPartByDescrLike(productPartOwner.ProId, "NYLON");
            if (part_NYLON.Count > 0)
            {
                currentProductRepository.BackUpProductPartByDescrLike(productPartOwner.ProId, this.Editor, "NYLON%");
                currentProductRepository.RemoveProductPartByDescrLike(ProId, "NYLON%");
                currentProductRepository.Update(productPartOwner, CurrentSession.UnitOfWork);
            }
            IList<ProductPart> part_Poster = currentProductRepository.GetProductPartByDescrLike(productPartOwner.ProId, "Poster");
            if (part_Poster.Count > 0)
            {
                currentProductRepository.BackUpProductPartByDescrLike(productPartOwner.ProId, this.Editor, "Poster%");
                currentProductRepository.RemoveProductPartByDescrLike(ProId, "Poster%");
                currentProductRepository.Update(productPartOwner, CurrentSession.UnitOfWork);
            }

            if (currentProductRepository.CheckExistProductPart(productPartOwner.ProId, "PS"))
            {
                currentProductRepository.BackUpProductPartByBomNodeType(productPartOwner.ProId, this.Editor, "PS");
                currentProductRepository.RemoveProductPartByBomNodeType(productPartOwner.ProId, "PS");
                currentProductRepository.Update(productPartOwner, CurrentSession.UnitOfWork);
            }
            
            return base.DoExecute(executionContext);
        }
	}
}
