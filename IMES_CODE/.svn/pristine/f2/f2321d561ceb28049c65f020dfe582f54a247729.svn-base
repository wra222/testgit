// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// CI-MES12-SPEC-PAK-Combine COA and DN.docx             
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




using log4net;






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
    public partial class AssignDeliveryForNonBT : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignDeliveryForNonBT()
        {
            InitializeComponent();
        }
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            string custSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            string DN = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string DNTemp = DN;
            IList<string> productList = new List<string>();
            productList.Add(curProduct.ProId);

            Delivery reDelivery = deliveryRep.Find(DN);
            if (reDelivery == null)
            {
                throw new FisException("CHK190", new string[] { DN });//DN不存在
            }

            int qty = 0;
            int packedQty = 0;
            qty = reDelivery.Qty;
            IList<IProduct> getList = new List<IProduct>();
            getList = productRep.GetProductListByDeliveryNo(DN);
            if (null != getList)
            {
                packedQty = getList.Count;
            }
            string dn1 = "";
            string dn2 = "";
            bool full = false;



            if (packedQty > qty || packedQty == qty)
            {
                string poNo = (string)CurrentSession.GetValue(Session.SessionKeys.Pno);
                if (null == poNo)
                {
                    poNo = "";
                }
                if (poNo != "")
                {
                    full = true;
                    DNQueryCondition condition = new DNQueryCondition();
                    DateTime temp = DateTime.Now;
                    temp = temp.AddDays(-3);
                    condition.Model = reDelivery.ModelName;
                    condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0, 0);
                    IList<DNForUI> dnList = deliveryRep.GetDNListByConditionWithSorting(condition);
                    foreach (DNForUI tmp in dnList)
                    {
                        if (tmp.Status != "00")
                        {
                            continue;
                        }
                        if (!(tmp.ModelName.Length == 12 && tmp.ModelName.Substring(0, 2) == "PC"))
                        {
                            continue;
                        }
                        int qtyTemp = 0;
                        int packedQtyTemp = 0;
                        qtyTemp = tmp.Qty;
                        IList<IProduct> proList = new List<IProduct>();
                        proList = productRep.GetProductListByDeliveryNo(tmp.DeliveryNo);
                        if (null != productList)
                        {
                            packedQtyTemp = proList.Count;
                        }
                        if (packedQtyTemp > qtyTemp || packedQtyTemp == qtyTemp)
                        {
                            continue;
                        }
                        if (poNo != "")
                        {
                            if (tmp.PoNo != poNo)
                            {
                                continue;
                            }
                        }
                        if (dn1 == "")
                        {
                            dn1 = tmp.DeliveryNo;
                        }
                        else if (dn2 == "")
                        {
                            dn2 = tmp.DeliveryNo;
                        }

                        if (dn1 != "" && dn2 != "")
                        {
                            break;
                        }
                    }
                }
                else
                {
                    full = true;
                    Delivery assignDelivery = null;
                    int assignQty = 0;

                    IList<Delivery> deliveryList = deliveryRep.GetDeliveryListByModel(curProduct.Model, "PC", 12, "00");
                    if (deliveryList.Count == 0)
                    {
                        List<string> errp = new List<string>();
                        errp.Add(curProduct.CUSTSN);
                        throw new FisException("PAK101", errpara);//无此机型Delivery!
                    }

                    //a)	ShipDate 越早，越优先
                    //b)	散装优先于非散装
                    //c)	剩余未包装Product数量越少的越优先

                    bool assignNA = false;
                    foreach (Delivery dvNode in deliveryList)
                    {
                        int curqty = productRep.GetCombinedQtyByDN(dvNode.DeliveryNo);
                        int tmpqty = dvNode.Qty - curqty;
                        int curQty = productRep.GetCombinedQtyByDN(dvNode.DeliveryNo);
                        if (tmpqty > 0)
                        {
                            bool naflag = false;
                            foreach (DeliveryPallet dpNode in dvNode.DnPalletList)
                            {
                                if (dpNode.PalletID.Substring(0, 2) == "NA")
                                {
                                    naflag = true;
                                    break;
                                }
                            }
                            if (assignDelivery == null)
                            {
                                assignDelivery = dvNode;
                                assignQty = tmpqty;
                                assignNA = naflag;
                                continue;
                            }
                            if (DateTime.Compare(assignDelivery.ShipDate, dvNode.ShipDate) < 0)
                            {
                                continue;
                            }
                            if (!assignNA && naflag)
                            {
                                assignDelivery = dvNode;
                                assignQty = tmpqty;
                                assignNA = naflag;
                            }
                            else if (assignNA == naflag)
                            {
                                if (tmpqty < assignQty)
                                {
                                    assignDelivery = dvNode;
                                    assignQty = tmpqty;
                                    assignNA = naflag;
                                }
                            }
                        }
                    }
                    if (assignDelivery == null)
                    {
                        FisException fe = new FisException("PAK103", new string[] { });   //没找到可分配的delivery
                        throw fe;
                    }
                    dn1 = assignDelivery.DeliveryNo;

                    assignDelivery = null;
                    assignNA = false;
                    assignQty = 0;
                    foreach (Delivery dvNode in deliveryList)
                    {
                        if (dvNode.DeliveryNo == dn1)
                        {
                            continue;
                        }
                        int curqty = productRep.GetCombinedQtyByDN(dvNode.DeliveryNo);
                        int tmpqty = dvNode.Qty - curqty;
                        int curQty = productRep.GetCombinedQtyByDN(dvNode.DeliveryNo);
                        if (tmpqty > 0)
                        {
                            bool naflag = false;
                            foreach (DeliveryPallet dpNode in dvNode.DnPalletList)
                            {
                                if (dpNode.PalletID.Substring(0, 2) == "NA")
                                {
                                    naflag = true;
                                    break;
                                }
                            }
                            if (assignDelivery == null)
                            {
                                assignDelivery = dvNode;
                                assignQty = tmpqty;
                                assignNA = naflag;
                                continue;
                            }
                            if (DateTime.Compare(assignDelivery.ShipDate, dvNode.ShipDate) < 0)
                            {
                                continue;
                            }
                            if (!assignNA && naflag)
                            {
                                assignDelivery = dvNode;
                                assignQty = tmpqty;
                                assignNA = naflag;
                            }
                            else if (assignNA == naflag)
                            {
                                if (tmpqty < assignQty)
                                {
                                    assignDelivery = dvNode;
                                    assignQty = tmpqty;
                                    assignNA = naflag;
                                }
                            }
                        }
                    }
                    if (assignDelivery != null)
                    {
                        dn2 = assignDelivery.DeliveryNo;
                    }
                }
            }


            if (full == true)
            {
                if (dn1 != "")
                {
                    logger.Debug("(INdn1)" + dn1);
                    reDelivery = deliveryRep.Find(dn1);
                    DN = dn1;
                    productRep.BindDN(DN, productList, reDelivery.Qty);
                    CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);
                }
                var currentProduct = productRep.GetProductByCustomSn(custSN);
                if (currentProduct.DeliveryNo != DN)
                {
                    if (dn2 != "")
                    {
                        logger.Debug("(INdn2)" + dn2);
                        reDelivery = deliveryRep.Find(dn2);
                        DN = dn2;
                        productRep.BindDN(DN, productList, reDelivery.Qty);
                        CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                        CurrentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);
                    }
                }
            }
            else
            {
                logger.Debug("(NoFull)" + DN );
                productRep.BindDN(DN, productList, reDelivery.Qty);
                CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                CurrentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);
            }

            var currentProduct2 = productRep.GetProductByCustomSn(custSN);
            logger.Debug("(DNProduct)" + currentProduct2.DeliveryNo + "$");
            if (currentProduct2.DeliveryNo.Trim() != DN.Trim())
            {
                if (DNTemp != DN)
                {
                    DNTemp = DNTemp + "&" + DN;
                }
                List<string> err = new List<string>();
                err.Add(DNTemp);
                err.Add(custSN);
                throw new FisException("CHK188", err);
            }

            CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
            CurrentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);
            CurrentSession.AddValue("doDelivery", "true");
            IList<IProduct> reIList = productRep.GetProductListByDeliveryNo(DN);
            if (reIList.Count == reDelivery.Qty) 
            {
                reDelivery.Status = "87";
            }
            if (reDelivery.Status == "87")
            {
                CurrentSession.AddValue(Session.SessionKeys.IsBT, false);
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.IsBT, true);
            }
            return base.DoExecute(executionContext);
        }
    }

}
