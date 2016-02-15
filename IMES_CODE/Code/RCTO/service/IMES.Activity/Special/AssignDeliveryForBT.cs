// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// CI-MES12-SPEC-PAK-Combine DN & Pallet for BT.docx                     
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-1-6   207003         create
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    ///
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
    ///         Delivery 分配
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
    public partial class AssignDeliveryForBT : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignDeliveryForBT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delivery 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            List<string> errpara = new List<string>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string DN = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string custSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            if (DN != "")
            {
                Delivery reDelivery = deliveryRep.Find(DN);
                if (reDelivery == null)
                {
                    errpara = new List<string>();
                    errpara.Add(DN);
                    throw new FisException("CHK107", errpara);
                }
                
                IList<IProduct> reIList = productRep.GetProductListByDeliveryNo(DN);
                //IList<IProduct> reIList = productRep.GetProductListByDeliveryNo_OnTrans(DN);

                if (reIList.Count > reDelivery.Qty || reIList.Count == reDelivery.Qty)
                {
                    List<string> err = new List<string>();
                    err.Add(DN);
                    err.Add(custSN);
                    throw new FisException("CHK188", err);
                }
                if (curProduct.DeliveryNo == null || curProduct.DeliveryNo == "")
                {
                    IList<string> productList = new List<string>();
                    productList.Add(curProduct.ProId);
                    //productRep.BindDN(DN, productList, reDelivery.Qty);
                    productRep.BindDNDefered(CurrentSession.UnitOfWork, DN, productList, reDelivery.Qty);
                    curProduct.DeliveryNo = DN;
                }

                reIList = productRep.GetProductListByDeliveryNo(DN);

                if (reIList.Count + 1 == reDelivery.Qty) //(reIList.Count + 1 == reDelivery.Qty) //
                {
                    reDelivery.Status = "88";
                    deliveryRep.Update(reDelivery, CurrentSession.UnitOfWork);
                }
                else if (reIList.Count >= reDelivery.Qty) //(reIList.Count + 1 > reDelivery.Qty) //
                {
                    FisException fe = new FisException("CHK515", new string[] { });   //Delivery has over qty,请将此机器的船务Unpack后，再重新刷入!
                    throw fe;
                }

                reDelivery = deliveryRep.Find(DN);

                CurrentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);

                //var product = productRep.GetProductByCustomSn(custSN);
                CurrentSession.AddValue(Session.SessionKeys.Product, curProduct);
            }
            return base.DoExecute(executionContext);
        }
    }

}
