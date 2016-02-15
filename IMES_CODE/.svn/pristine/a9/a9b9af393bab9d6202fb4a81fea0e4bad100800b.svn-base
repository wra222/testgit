/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0070  修正carton status&line的赋值
* ITC-1414-0098  增加snoid的排序
* ITC-1414-0116  palletList拣选错误
* ITC-1414-0120  更新carton station为95
* Known issues:
* TODO：
* 
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;

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
    ///      Carton NO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         更新Product.CUSTSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
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
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class AssignForCarton : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssignForCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        { 
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();

            try
            {
                //2.	Assign Pallet
                //Assign Pallet 的方法：
                //1.	找到与Product.DeliveryNo 结合的Pallets  
                //2.	取其中尚未完成Combine PO in Carton 的Pallets
                //3.	按照Pallet No 正序，和Pallet 已经完成Combine PO in Carton 的数量逆序排序，取第一个Pallet
                //DECLARE @dn char(16)
                //DECLARE @pea float
                //DECLARE @pea2 int
                //SELECT @pea = InfoValue FROM DeliveryInfo (NOLOCK) WHERE DeliveryNo=@dn AND InfoType = ‘CQty’
                string peastr;
                Delivery dev = deliveryRep.Find(product.DeliveryNo);
                peastr = (string)dev.GetExtendedProperty("CQty");

                int pea = 0;
                if (string.IsNullOrEmpty(peastr))
                {
                    pea = 0;
                }
                else
                {
                    decimal tmp = Convert.ToDecimal(peastr);
                    pea = Convert.ToInt32(tmp);
                }
                

                //SET @pea2=CONVERT(int, @pea)
                //CREATE TABLE #plt (Plt char(14), qty int, tot int)
                //CREATE TABLE #Pltamount (Plt char(14), qty int)

                //INSERT #plt 
	            //SELECT PalletNo, DeliveryQty, 0 
		        //FROM Delivery_Pallet (NOLOCK) 
		        //WHERE DeliveryNo=@dn 
                IList<DeliveryPalletInfo> palletList = deliveryRep.GetDeliveryPalletListByDN(product.DeliveryNo);

                //INSERT #Pltamount
	            //SELECT PalletNo, COUNT(ProductID) as Qty
		        //FROM Product (NOLOCK)
		        //WHERE DeliveryNo = @dn 
                IList<IProduct> mountList = productRep.GetProductListByDeliveryNo(product.DeliveryNo);

                //UPDATE #plt SET tot = b.qty FROM #plt a,#Pltamount b
                //WHERE a.Plt=b.Plt
                //SELECT * FROM #plt

                //DELETE FROM #plt WHERE @pea2 * CONVERT(int, qty) – CONVERT(int, tot) < 1
                //SELECT * FROM #plt
                 for (int i = palletList.Count-1; i >= 0; i--)
                {
                    DeliveryPalletInfo node = palletList[i];
                    IList <ProductModel> proList = productRep.GetProductByDnPallet(product.DeliveryNo,node.palletNo);

                    if ((proList != null) && (proList.Count > 0))
                    {
                        int tot = proList.Count;
                        int qty = node.deliveryQty;
                        node.id = tot;
                        if (pea*qty - tot <1)
                        {
                            //ITC-1414-0116
                            palletList.RemoveAt(i);
                        }
                    }
                }
                //SELECT TOP 1 Plt as [Pallet No]
	            //FROM #plt 
	            //ORDER BY Plt, tot DESC
                 var tmpList = from item in palletList orderby item.palletNo, item.id descending select item;
                 IList<DeliveryPalletInfo> orderList = tmpList.ToList<DeliveryPalletInfo>();

                 string palletNo = orderList[0].palletNo;

                //3.	Assign Location by Pallet
                //IF EXISTS(SELECT SnoId FROM PAK_LocMas (NOLOCK)WHERE Tp='PakLoc' AND Pno=@PalletNo) 
                //BEGIN
	            //    SELECT @loc=SnoId FROM PAK_LocMas(NOLOCK) WHERE Tp='PakLoc' AND Pno=@PalletNo
                //END
                //ELSE
                //BEGIN
	            //IF EXISTS(SELECT SnoId FROM PAK_LocMas(NOLOCK) WHERE Tp='PakLoc' AND Pno='' )
	            //BEGIN
		        //SELECT @loc=SnoId FROM PAK_LocMas(NOLOCK) WHERE Tp='PakLoc' AND Pno='' ORDER BY CONVERT(int, SnoId)
		        //UPDATE PAK_LocMas SET Pno=@PalletNo,Udt=GETDATE() WHERE Tp='PakLoc' AND SnoId=@loc
	            //END
	            //ELSE	
	            //BEGIN
		        //SELECT @loc='Others'
	            //END	
                //END

                //@PalletNo – 上文分配的Pallet No
                //@loc – 系统分配的库位
                 string loc = "";
                 IList<PakLocMasInfo> macList = palletRep.GetPakLocMasList(palletNo, "PakLoc");
                 if (macList.Count > 0)
                 {
                     loc = macList[0].snoId;
                 }
                 else
                 {
                     macList = palletRep.GetPakLocMasList("", "PakLoc");
                     if (macList.Count > 0)
                     {
                         PakLocMasInfo locInfo = macList[0];
                         foreach (var item in macList)
                         {
                             if (Convert.ToInt64(locInfo.snoId) > Convert.ToInt64(item.snoId))
                             {
                                 locInfo = item;
                             }
                         }
                         loc = locInfo.snoId;

                         PakLocMasInfo sitem= new PakLocMasInfo();
                         PakLocMasInfo cond= new PakLocMasInfo();
                         sitem.pno= palletNo;
                         sitem.udt = DateTime.Now;
                         cond.tp = "PakLoc";
                         cond.snoId = loc;
                         palletRep.UpdatePakLocMasInfoDefered(CurrentSession.UnitOfWork,sitem,cond );

                     }
                     else
                     {
                         loc = "Others";
                     }
                 }
                 CurrentSession.AddValue("Location",loc);

                //4.	Product结合Pallet and Carton    
                //将页面上[Products in Carton] 中的每一个Product和上文系统分配的Pallet 以及上文生成的Carton No 进行结合 – Update Product
                //Product.PalletNo – Pallet No
                //Product.CartonSN – Carton No

                 IList<IProduct> productList = (List<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);
                 foreach (var item in productList)
                 {
                     item.PalletNo = palletNo;
                     item.CartonSN = product.CartonSN;
                     //productRep.Update(item,CurrentSession.UnitOfWork);
                     productRep.UpdateForBindDNAndPalletDefered(CurrentSession.UnitOfWork, (Product)item);
                 }

                //ITC-1414-0070
                //ITC-1414-0120
                //5.	更新CartonStatus 的状态为95 （Station = ‘95’ ，Status= ‘1’），记录CartonLog
                CartonStatusInfo sinfo = new CartonStatusInfo();
                CartonStatusInfo sconf = new CartonStatusInfo();
                sconf.cartonNo = product.CartonSN;

                sinfo.editor = Editor;
                sinfo.line = Line;
                sinfo.station ="95";//Station;
                sinfo.status = 1;//pass
                sinfo.udt = DateTime.Now;
                cartRep.UpdateCartonStatusDefered(CurrentSession.UnitOfWork,sinfo,sconf);

                CartonLogInfo linfo = new CartonLogInfo();
                linfo.cartonNo = product.CartonSN;
                linfo.editor = Editor;
                linfo.line = Line;
                linfo.station = "95";
                linfo.status = 1;//pass
                linfo.cdt = DateTime.Now;
                cartRep.AddCartonLogInfoDefered(CurrentSession.UnitOfWork, linfo);

                 //6.更新Carton上所有Product 的ProductStatus 的状态为Combine Po In Carton for Docking 的站号（Station = Combine Po In Carton for Docking 的站号，Status= ‘1’），并记录ProductLog
                 string line = string.IsNullOrEmpty(this.Line) ? product.Status.Line : this.Line;

                 var newStatus = new IMES.FisObject.FA.Product.ProductStatus();
                 newStatus.Editor = Editor;
                 newStatus.Line = line;
                 newStatus.StationId = Station;
                 newStatus.Status = IMES.FisObject.Common.Station.StationStatus.Pass;

                 IList<string> ProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);

                 #region  record multi-productId product status
                 //System.Data.DataTable productTb = CreateDataTable.CreateStringListTb();
                 //foreach (string id in ProductIDList)
                 //{
                 //    productTb.Rows.Add(id);
                 //}
                 //SqlParameter para1 = new SqlParameter("ProductIDList", System.Data.SqlDbType.Structured);
                 //para1.Direction = System.Data.ParameterDirection.Input;
                 //para1.Value = productTb;

                 //productRep.ExecSpForNonQueryDefered(CurrentSession.UnitOfWork,
                 //                                                                                IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_FA,
                 //                                                                                "IMES_MultiUpdateProductStatus",
                 //                                                                               para1,
                 //                                                                               new SqlParameter("station", Station),
                 //                                                                               new SqlParameter("status",  1),
                 //                                                                               new SqlParameter("line", Line),
                 //                                                                               new SqlParameter("editor", Editor),
                 //                                                                               new SqlParameter("udt", DateTime.Now)
                 //                                                                               );
                 IList<IMES.DataModel.TbProductStatus> stationList = productRep.GetProductStatus(ProductIDList);
                 productRep.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, stationList);
                 #endregion

                 productRep.UpdateProductListStatusDefered(CurrentSession.UnitOfWork, newStatus, ProductIDList);

                 foreach (var item in productList)
                 {
                     var productLog = new ProductLog
                     {
                         Model = item.Model,
                         Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                         Editor = Editor,
                         Line = line,
                         Station = Station,
                         Cdt = DateTime.Now
                     };

                     item.AddLog(productLog);
                     productRep.Update(item, CurrentSession.UnitOfWork);
                 }
            }
            catch (Exception)
            {
                throw;
            }
            
            return base.DoExecute(executionContext);
        }

    }
}
