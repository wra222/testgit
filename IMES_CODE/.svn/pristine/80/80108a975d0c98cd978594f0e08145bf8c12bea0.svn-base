// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 备份Product / ProductStatus / Product_Part / ProductInfo 表中将被解绑的记录
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-03-02    itc202017                   create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;
namespace IMES.Activity
{
    /// <summary>
    /// 备份Product / ProductStatus / Product_Part / ProductInfo 表中将被解绑的记录
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
    ///         需要将Product / ProductStatus / Product_Part / ProductInfo 表中的记录备份到UnpackProduct / UnpackProductStatus / UnpackProduct_Part / UnpackProductInfo
    ///         UnpackProduct / UnpackProductStatus / UnpackProduct_Part / UnpackProductInfo 相对于源表增加了两个字段UEditor 和UPdt 分别记录解资料的Operator和时间
    /// </para> 
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
    public partial class UnpackByDN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnpackByDN()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 备份Product / ProductStatus / Product_Part / ProductInfo 表中将被解绑的记录
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            string DN = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

            IList<IProduct> productList = (IList < IProduct > )CurrentSession.GetValue(Session.SessionKeys.ProdNoList);

            IProduct currentProduct = null;
            foreach (IProduct ele in productList)
            {
                currentProduct = ele;
                productList.Remove(ele);
                break;
            }
            if (productList.Count == 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
            }
            CurrentSession.AddValue(Session.SessionKeys.ProdNoList, productList);
            CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            IList<string> itemTypes = new List<string>();

            itemTypes.Add("CKK");

            productRepository.BackUpProductInfoDefered(CurrentSession.UnitOfWork,currentProduct.ProId, this.Editor, "CKK");

            productRepository.RemoveProductInfosByTypeDefered(CurrentSession.UnitOfWork, currentProduct.ProId, itemTypes);



            //// Delete CartonInfo

            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();

            CartonInfoInfo infoCond = new CartonInfoInfo();

            infoCond.cartonNo = currentProduct.CartonSN;

            cartRep.DeleteCartonInfoDefered(CurrentSession.UnitOfWork, infoCond);



            //Delete Product_Part 

           
            string[] prodList = new string[1];

            prodList[0] = currentProduct.ProId;

            ProductPart tmp = new ProductPart();

            tmp.Station = "8C";
            productRepository.BackUpProductPartDefered(CurrentSession.UnitOfWork, prodList, tmp,this.Editor);
            productRepository.DeleteProductPartsDefered(CurrentSession.UnitOfWork, prodList, tmp);

            
            //Update Product       

            productRepository.BackUpProductStatusDefered(CurrentSession.UnitOfWork, currentProduct.ProId, this.Editor);

            productRepository.BackUpProductDefered(CurrentSession.UnitOfWork, currentProduct.ProId, this.Editor);
            currentProduct.CartonSN = string.Empty;
            currentProduct.PalletNo = string.Empty;
            currentProduct.DeliveryNo = string.Empty;
            currentProduct.Udt = DateTime.Now;
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);


            Delivery oldDelivery = currentRepository.Find(DN);
            if (oldDelivery != null)
            {
                oldDelivery.Status = "00";
                currentRepository.Update(oldDelivery, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }
}
