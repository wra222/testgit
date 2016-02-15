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
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.DN;
using DM =IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using System;

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
    public partial class UnPackDelivery : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackDelivery()
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
            string Delivery = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            Delivery CurrentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);

            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ////
           

            IList<string> nameList = new List<string>();
            nameList.Add("UCC");
            nameList.Add("BoxId");
            repProduct.BackUpProductByDnPure( Delivery, Editor);
            repProduct.BackUpProductInfoByDn( Delivery, Editor, nameList);
            repProduct.BackUpProductStatusByDnPure( Delivery, Editor);
           
            ////
            ProductStatus newStatus = new ProductStatus();
            newStatus.Editor = Editor;
            newStatus.Line = Line;
            newStatus.StationId = Station;
            newStatus.Status = StationStatus.Pass;
            newStatus.ReworkCode = "";

            repProduct.UpdateUnPackProductStatusByDn(newStatus, Delivery);
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            ////
            ProductLog newLog = new ProductLog();
            newLog.Editor = Editor;
            newLog.Line = Line;
            newLog.Station = Station;
            newLog.Status = StationStatus.Pass;

            repProduct.WriteUnPackProductLogByDn(Delivery,newLog);

            //////
            repProduct.UnPackProductInfoByDeliveryNoAndInfoType("UCC",Delivery);
            repProduct.UnPackProductInfoByDeliveryNoAndInfoType("BoxId", Delivery);
            
            ////////
            if (Station != "8U")
            {
                IList<IProduct> lstProduct = repProduct.GetProductObjListByDn(Delivery);
                if (lstProduct != null && lstProduct.Count > 0)
                {
                    foreach(IProduct tmp in lstProduct)
                    {
                        string carton = tmp.CartonSN;
                        DM.CartonInfoInfo condition = new DM.CartonInfoInfo();
                        condition.cartonNo = carton;
                        cartRep.DeleteCartonInfo(condition);

                        //mantis 1395
                        DM.CartonStatusInfo cond = new DM.CartonStatusInfo();
                        DM.CartonStatusInfo sv = new DM.CartonStatusInfo();
                        cond.cartonNo = tmp.CartonSN;
                        sv.station = Station;
                        sv.status = 1;
                        sv.editor = Editor;
                        sv.udt = DateTime.Now;
                        cartRep.UpdateCartonStatusDefered(CurrentSession.UnitOfWork, sv, cond);

                        DM.CartonLogInfo item = new DM.CartonLogInfo();
                        item.cartonNo = tmp.CartonSN;
                        item.cdt = DateTime.Now;
                        item.editor = Editor;
                        item.line = tmp.Status.Line;
                        item.station = Station;
                        item.status = 1;
                        cartRep.AddCartonLogInfoDefered(CurrentSession.UnitOfWork, item);
                        //mantis 1395


                        string palletNo = tmp.PalletNo;
                        DM.PakLocMasInfo setValue = new DM.PakLocMasInfo();
                        DM.PakLocMasInfo pakLoc_condition = new DM.PakLocMasInfo();
                        setValue.pno = "";
                        setValue.editor = Editor;
                        pakLoc_condition.pno = palletNo;
                        pakLoc_condition.tp = "PakLoc";
                        palletRep.UpdatePakLocMasInfo(setValue, pakLoc_condition);
                    }
                }
            }

            if (Station != "8U")
                repProduct.UnPackProductByDn(Delivery);
            else
                repProduct.UnPackProductByDnWithoutCartonSN(Delivery);

    /////////////////////
            CurrentDelivery.Status = "00";
            CurrentDelivery.Editor = this.Editor;
            CurrentDelivery.Udt = DateTime.Now;
            DeliveryRepository.Update(CurrentDelivery, CurrentSession.UnitOfWork);
    /////////////////////


            return base.DoExecute(executionContext);
        }
	}
}
