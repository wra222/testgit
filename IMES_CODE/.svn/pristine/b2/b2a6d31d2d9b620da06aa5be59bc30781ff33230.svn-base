// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-26   Du Xuan (itc98066)          create
// ITC-1360-0839 改为查找Delivery 出货时间
// ITC-1413-0009 新增CDSI 机器Assign Delivery的特殊要求
// Known issues:
using System;
using System.Data;
using System.Linq;
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
using IMES.FisObject.Common.MO;
using IMES.Common;

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
    public partial class AssignDelivery : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignDelivery()
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

            List<string> errpara = new List<string>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();

            IList<ProductBTInfo> btList = productRep.GetProductBT(curProduct.ProId);
            if (btList.Count > 0 && Station.Trim() == "92")
            {
                return base.DoExecute(executionContext);
            }

            Delivery assignDelivery = null;
            CurrentSession.AddValue("HasDN", "N");
            //CDSI 机器Assign Delivery的特殊要求
            //IList<IMES.FisObject.Common.Model.ModelInfo> GetModelInfoByModelAndName(string model, string name);
            IList<IMES.FisObject.Common.Model.ModelInfo> infoList = modelRep.GetModelInfoByModelAndName(curProduct.Model, "PO");
            Model model = modelRep.Find(curProduct.Model);
            string cdsi = "";
            cdsi= model.GetAttribute("PO");
            if (cdsi != "Y")
            {
                cdsi = "";
                cdsi = model.GetAttribute("ATSNAV");
                if (!string.IsNullOrEmpty(cdsi))
                {
                    cdsi = "cdsi";
                }
            }
            else
            {
                cdsi = "cdsi";
            }

            

            if (cdsi == "cdsi")
            {
                #region CDSI case
                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    assignDelivery = deliveryRep.Find(curProduct.DeliveryNo);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);
                    CurrentSession.AddValue("HasDN", "Y");
                    return base.DoExecute(executionContext);
                }

                string factoryPo = "";
                //获取Product 结合的Factory Po，对于CDSI 机器如果获取不到结合的Factory Po，
                //需要报告错误：“CDSI AST MISSING DATA!”
                //SELECT @FactoryPo = Sno FROM CDSIAST NOLOCK WHERE SnoId = @ProductId AND Tp = 'FactoryPO'

                CdsiastInfo conf = new CdsiastInfo();
                conf.snoId = curProduct.ProId;
                conf.tp = "FactoryPO";
                IList<CdsiastInfo> cdsiList = productRep.GetCdsiastInfoList(conf);
                if (cdsiList.Count == 0)
                {
                    errpara.Add(this.Key);
                    throw new FisException("PAK140", errpara);//“CDSI AST MISSING DATA!”
                }
                factoryPo = cdsiList[0].sno;
                IList<Delivery> dnList = deliveryRep.GetDeliveryListByModelPrefix(factoryPo, "PC", 12, "00");

                //IF @Delivery = ''	SELECT 'CDSI 机器，无此PoNo: ' + @FactoryPo + ' 的Delivery!'
                if (dnList.Count == 0)
                {
                    errpara.Add(factoryPo);
                    throw new FisException("PAK141", errpara);//'CDSI 机器，无此PoNo: ' + @FactoryPo + ' 的Delivery!'
                }
                assignDelivery = dnList[0];
                #endregion
            }
            else if (curProduct.IsBindedPo)  //Mo binded PONo case
            {
                #region bind Mo PoNo

                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    assignDelivery = deliveryRep.Find(curProduct.DeliveryNo);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);
                    CurrentSession.AddValue("HasDN", "Y");
                    return base.DoExecute(executionContext);
                }

                string factoryPo = curProduct.BindPoNo;
                IList<Delivery> dnList = deliveryRep.GetDeliveryListByModelPrefix(factoryPo,
                                                                                     curProduct.Model.Substring(0, 2),
                                                                                    curProduct.Model.Length,
                                                                                    "00").Where(x => x.Model == curProduct.Model).ToList();

                //IList<Delivery> dnList = deliveryRep.GetDeliveryListByModel(curProduct.Model,
                //                                                              curProduct.Model.Substring(0, 2),
                //                                                              curProduct.Model.Length,
                //                                                              "00").Where(x => x.PoNo == factoryPo).ToList();
                if (dnList.Count == 0)
                {
                  
                    throw new FisException("CQCHK1101",new string[]{curProduct.MO, factoryPo});//MO Binded 机器，无此PoNo: ' + @FactoryPo + ' 的Delivery!'
                }
                assignDelivery = dnList[0];
                #endregion

            }
            else
            {
                #region normal assign DN case 
                //a.	查找Delivery 出货时间（IMES_PAK..Delivery.ShipDate）大于两天前的，
                //状态为（IMES_PAK..Delivery.Status）'00' 的，Model（IMES_PAK..Delivery.Model）
                //与当前Product Model 相同的Deliveries，如果不存在，则报告错误：“无此机型Delivery!”
                DateTime beginCdt = DateTime.Now.AddDays(-2);
                String[] statusArray = { "00" };

                int assignQty = 0;

                //如果此时Product 已经结合DN (IMES_FA..Product.DeliveryNo)，则跳过Assign Delivery步骤，直接开始Assign Pallet
                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    assignDelivery = deliveryRep.Find(curProduct.DeliveryNo);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);
                    CurrentSession.AddValue("HasDN", "Y");
                    return base.DoExecute(executionContext);
                }

                //IList<Delivery> deliveryList = deliveryRep.GetDeliveryListByCdtAboveAndStatusesAndModel(beginCdt, statusArray, curProduct.Model);
                //SELECT DeliveryNo, ShipDate, Qty, '', 0 FROM Delivery NOLOCK
		        //WHERE Model = @Model AND LEFT(Model, 2) = 'PC'
			    //AND CONVERT(char(10), ShipDate, 111)>=CONVERT(char(10),GETDATE()-3,111)
			    //AND LEN(Model) = 12 AND Status = '00'

                // modify for other model, ex: Jamestown in mantis 1945
                /*IList<Delivery> deliveryList = deliveryRep.GetDeliveryListByModel(curProduct.Model,"PC",12,"00");
                if (deliveryList.Count == 0)
                {
                    errpara.Add(this.Key);
                    throw new FisException("PAK101", errpara);//无此机型Delivery!
                }*/
                IList<Delivery> deliveryList = new List<Delivery>();
                if ("PC" == curProduct.Model.Substring(0, 2))
                    deliveryList = deliveryRep.GetDeliveryListByModel(curProduct.Model, "PC", 12, "00");
                else if (ActivityCommonImpl.Instance.CheckModelByProcReg(curProduct.Model, true))
                    deliveryList = deliveryRep.GetDeliveryListByModel(curProduct.Model, curProduct.Model.Substring(0, 2), curProduct.Model.Length, "00");
                if (null == deliveryList || deliveryList.Count == 0)
                {
                    errpara.Add(this.Key);
                    throw new FisException("PAK101", errpara);//无此机型Delivery!
                }

                //Vincent 2015-02-27 過濾綁訂PoDN
                IList<string> bindPoNoList = moRep.GetBindPoNoByModel(curProduct.Model);
                if (bindPoNoList != null && bindPoNoList.Count > 0)
                {
                    deliveryList = deliveryList.Where(x => !bindPoNoList.Contains(x.PoNo)).ToList();
                }

                //1.	按照Delivery.Model = @Model 选取Delivery
                //2.	选取的Delivery 还要求Delivery.Model 12位长，以'PC' 开头，CONVERT(char(10), ShipDate, 111)>=CONVERT(char(10),GETDATE()-3,111)，Status = '00'
                //3.	满足上面条件的Delivery，按照如下顺序的排序规则，选取第一条；如果没有满足条件的Delivery，则需要报告错误。
                //a)	ShipDate 越早，越优先
                //b)	散装优先于非散装
                //c)	剩余未包装Product数量越少的越优先

                bool assignNA = false;
                foreach (Delivery dvNode in deliveryList)
                {
                    int curqty = productRep.GetCombinedQtyByDN(dvNode.DeliveryNo);
                    int tmpqty = dvNode.Qty - curqty;
                    //string tmpstr = (string)dvNode.GetExtendedProperty("Consolidated");
                    //int curQty = productRep.GetCombinedQtyByDN(dvNode.DeliveryNo);
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
                #endregion
            }
            curProduct.DeliveryNo = assignDelivery.DeliveryNo;
            //f.分配了Delivery 后，需要更新IMES_FA..Product.DeliveryNo，记录该Product 分配了的Delivery；
            //如果该Delivery 此时结合的Product 数量与Delivery定义的数量相等，则需要更新Delivery 的状态为'87'；
            //如果该Delivery 此时结合的Product 数量大于Delivery定义的数量，
            //则报告错误：“Delivery has over qty,请将此机器的船务Unpack后，再重新刷入!”


            //Lock The XXX: 2012.04.20 LiuDong
            //if (assignDelivery != null && !string.IsNullOrEmpty(assignDelivery.DeliveryNo))
            //{
            //    Guid gUiD = Guid.Empty;
            //    var identity = new ConcurrentLocksInfo();
            //    identity.clientAddr = "N/A";
            //    identity.customer = CurrentSession.Customer;
            //    identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            //    identity.editor = CurrentSession.Editor;
            //    identity.line = CurrentSession.Line;
            //    identity.station = CurrentSession.Station;
            //    identity.timeoutSpan4Hold = new TimeSpan(0, 0, 3).Ticks;
            //    identity.timeoutSpan4Wait = new TimeSpan(0, 0, 5).Ticks;
            //    gUiD = productRep.GrabLockByTransThread("Delivery", assignDelivery.DeliveryNo, identity);
            //    CurrentSession.AddValue(Session.SessionKeys.lockToken_DN, gUiD);
            //}
            //Lock The XXX: 2012.04.20 LiuDong


            //assignDelivery.IsDNFull(int currentStationCombineQty);

            //2012.05.11 LD
            ////2012.05.09 LD
            ////int dvQty = productRep.GetCombinedQtyByDN(assignDelivery.DeliveryNo)+1;
            //int dvQty = productRep.GetCombinedQtyByDN_OnTrans(assignDelivery.DeliveryNo);
            int dvQty = productRep.GetCombinedQtyByDN(assignDelivery.DeliveryNo);
            ////2012.05.09 LD
            //2012.05.11 LD

            if (dvQty + 1 > assignDelivery.Qty)
            {
                FisException fe = new FisException("PAK104", new string[] { });   //Delivery has over qty,请将此机器的船务Unpack后，再重新刷入!
                throw fe;
            }
        
            IList<string> ProductIDList = new List<string>();
            ProductIDList.Add(curProduct.ProId);
            productRep.BindDNDefered(CurrentSession.UnitOfWork, curProduct.DeliveryNo, ProductIDList, assignDelivery.Qty);

            if (dvQty + 1 == assignDelivery.Qty)
            {
                if (Station == "96")
                {
                    assignDelivery.Status = "88";
                }
                else
                {
                    assignDelivery.Status = "87";
                }
                deliveryRep.Update(assignDelivery, CurrentSession.UnitOfWork);
            }
            
            CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);

            return base.DoExecute(executionContext);
        }
    }
          
}
