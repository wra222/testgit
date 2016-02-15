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
using IMES.FisObject.PAK.Pallet;
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

            Session session = CurrentSession;
            string DN = (string)session.GetValue(Session.SessionKeys.DeliveryNo);

            IList<IProduct> productList = (IList<IProduct>)session.GetValue(Session.SessionKeys.ProdNoList);

            IProduct currentProduct = null;
            foreach (IProduct ele in productList)
            {
                currentProduct = ele;
                productList.Remove(ele);
                break;
            }
            if (productList.Count == 0)
            {
                session.AddValue(Session.SessionKeys.IsComplete, true);
            }
            session.AddValue(Session.SessionKeys.ProdNoList, productList);
            session.AddValue(Session.SessionKeys.Product, currentProduct);
            IList<string> itemTypes = new List<string>();

            itemTypes.Add("CKK");

            productRepository.BackUpProductInfoDefered(session.UnitOfWork, currentProduct.ProId, this.Editor, "CKK");

            productRepository.RemoveProductInfosByTypeDefered(session.UnitOfWork, currentProduct.ProId, itemTypes);



            //// Delete CartonInfo

            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();

            CartonInfoInfo infoCond = new CartonInfoInfo();

            infoCond.cartonNo = currentProduct.CartonSN;

            cartRep.DeleteCartonInfoDefered(session.UnitOfWork, infoCond);



            //Delete Product_Part 

           
            string[] prodList = new string[1];

            prodList[0] = currentProduct.ProId;

            ProductPart tmp = new ProductPart();

            tmp.Station = "8C";
            productRepository.BackUpProductPartDefered(session.UnitOfWork, prodList, tmp, this.Editor);
            productRepository.DeleteProductPartsDefered(session.UnitOfWork, prodList, tmp);

            
            //Update Product       

            productRepository.BackUpProductStatusDefered(session.UnitOfWork, currentProduct.ProId, this.Editor);

            productRepository.BackUpProductDefered(session.UnitOfWork, currentProduct.ProId, this.Editor);
            currentProduct.CartonSN = string.Empty;
            currentProduct.PalletNo = string.Empty;
            currentProduct.DeliveryNo = string.Empty;
            currentProduct.Udt = DateTime.Now;
            productRepository.Update(currentProduct, session.UnitOfWork);


            Delivery oldDelivery = currentRepository.Find(DN);
            if (oldDelivery != null)
            {
                oldDelivery.Status = "00";
                currentRepository.Update(oldDelivery, session.UnitOfWork);
                #region 清空Pallet weight
                IPalletRepository currentPltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<string> palletNoList = currentRepository.GetPalletNoListByDeliveryNo(DN);
                foreach (string pn in palletNoList)
                {
                    //mantis1666: Unpack DN by DN，清除棧板庫位時若unpack 的 DN為棧板唯一的DN才能清庫位
                    //在Pallet 結合DN最後一筆時，才能清空Pallet Location 
                    Pallet pallet = currentPltRepository.Find(pn);
                    IList<string> dnList = productRepository.GetDeliveryNoListByPalletNo(pn);
                    if (dnList.Count < 2)
                    {
                        PakLocMasInfo setVal = new PakLocMasInfo();
                        PakLocMasInfo cond = new PakLocMasInfo();
                        setVal.editor = Editor;
                        setVal.pno = "";
                        cond.pno = pn;
                        cond.tp = "PakLoc";
                        currentPltRepository.UpdatePakLocMasInfoDefered(session.UnitOfWork, setVal, cond);
                        //Clear Floor in Pallet                    
                        pallet.Floor = "";
                        //Clear Floor in Pallet                    
                    }

                    //Clear  weight in Pallet 
                    pallet.Weight = 0;
                    pallet.Weight_L = 0;

                    PalletLog palletLog = new PalletLog { PalletNo = pallet.PalletNo, Station = "RETURN", Line = "Weight:0", Cdt = DateTime.Now, Editor = this.Editor };
                    pallet.AddLog(palletLog);
                    currentPltRepository.Update(pallet, session.UnitOfWork);

                }

                #endregion
            }
            return base.DoExecute(executionContext);
        }
    }
}
