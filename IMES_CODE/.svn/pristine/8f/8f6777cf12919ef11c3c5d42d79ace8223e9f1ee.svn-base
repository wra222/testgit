// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo,InfoType,delete属于DeliveryNo的InfoType=@InfoType的所有ProductInfo
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                  create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.DataModel;
using System.Collections.Generic;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据DeliveryNo,InfoType,delete属于DeliveryNo的InfoType=@InfoType的所有ProductInfo
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
    ///         1.从Session中获取DeliveryNo,调用ProductRepository的UnPackProductInfoByDeliveryNoDefered方法
    ///           delete ProductInfo from ProductInfo as I inner join Product as P ON I.ProductID = P.ProductID
    ///           where I.InfoType=@InfoType and P.DeliveryNo=@DeliveryNo 
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
    ///         InfoType
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
    ///         delete ProductInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnPackProducForDocking : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackProducForDocking()
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
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            IList<string> itemTypes = new List<string>();
            itemTypes.Add("CKK");

            productRepository.BackUpProductInfo(currentProduct.ProId, this.Editor, "CKK");
            productRepository.RemoveProductInfosByType(currentProduct.ProId, itemTypes);

            ////	Delete CartonInfo
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
            CartonInfoInfo infoCond = new CartonInfoInfo();
            infoCond.cartonNo = currentProduct.CartonSN;
            cartRep.DeleteCartonInfoDefered(CurrentSession.UnitOfWork,infoCond);
            
            //Delete Product_Part 
            if (Station == "SP")
            {
                Delivery CurrentDelivery = DeliveryRepository.Find(currentProduct.DeliveryNo);
               
                CurrentSession.AddValue(Session.SessionKeys.Delivery, CurrentDelivery);
                string[] prodList = new string[1];
                prodList[0] = currentProduct.ProId;
                ProductPart tmp = new ProductPart();
                tmp.Station = "8C";
                productRepository.BackUpProductPartDefered(CurrentSession.UnitOfWork, prodList, tmp, Editor);

                productRepository.DeleteProductPartsDefered(CurrentSession.UnitOfWork, prodList, tmp);
            }

      //Update Product       
            currentProduct.CartonSN = string.Empty; 
            currentProduct.PalletNo = string.Empty;
            if (Station == "SP")
            {
                currentProduct.DeliveryNo = string.Empty;
            }
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            productRepository.BackUpProduct(currentProduct.ProId, this.Editor);
            productRepository.BackUpProductStatus(currentProduct.ProId, this.Editor);


            return base.DoExecute(executionContext);
        }
	}
}
