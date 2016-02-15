// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data(For Docking)模块保存上传的Delivery、Pallet、Delivery_Pallet数据
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-06   itc202017                     Create
// Known issues:
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
using System.Data.SqlClient;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块保存上传的Delivery、Pallet、Delivery_Pallet数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Upload Po Data for PL user(173/Normal/Domestic)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         将DeliveryList保存到Delivery表；
    ///         将PalletList保存到Pallet表；
    ///         将DeliveryPalletList保存到Delivery_Pallet表
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         DeliveryList, PalletList, DeliveryPalletList
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class UploadPoDataForDocking : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadPoDataForDocking()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<Delivery> deliveryList = CurrentSession.GetValue("DeliveryList") as IList<Delivery>;
            IList<Pallet> palletList = CurrentSession.GetValue("PalletList") as IList<Pallet>;
            IList<DeliveryPalletInfo> dpList = CurrentSession.GetValue("DeliveryPalletList") as IList<DeliveryPalletInfo>;

            foreach (Delivery ele in deliveryList)
            {
                Delivery item = deliveryRepository.Find(ele.DeliveryNo);
                if (null != item)
                {
                    item.Editor = Editor;
                    foreach (DeliveryInfo info in ele.DeliveryInfoes)
                    {
                        item.SetExtendedProperty(info.InfoType, info.InfoValue, info.Editor);
                    }
                    deliveryRepository.Update(item, CurrentSession.UnitOfWork);
                }
                else
                {
                    deliveryRepository.Add(ele, CurrentSession.UnitOfWork);
                }
            }

            //<UC:2.18>若导入的Pallet存在于数据库中，则删除这个Pallet后再次导入
            foreach (Pallet ele in palletList)
            {
                Pallet item = palletRepository.Find(ele.PalletNo);
                if (null != item)
                {
                    item.Editor = Editor;
                    palletRepository.Update(item, CurrentSession.UnitOfWork);
                }
                else
                {
                    palletRepository.Add(ele, CurrentSession.UnitOfWork);
                }
            }

            foreach (DeliveryPalletInfo ele in dpList)
            {
                if (deliveryRepository.GetDeliveryPalletListByDN(ele.deliveryNo, ele.palletNo).Count > 0)
                {
                    DeliveryPalletInfo cond = new DeliveryPalletInfo();
                    cond.deliveryNo = ele.deliveryNo;
                    cond.palletNo = ele.palletNo;
                    DeliveryPalletInfo item = new DeliveryPalletInfo();
                    item.editor = Editor;
                    item.udt = DateTime.Now;
                    deliveryRepository.UpdateDeliveryPalletInfoDefered(CurrentSession.UnitOfWork, item, cond);
                }
                else
                {
                    deliveryRepository.InsertDeliveryPalletDefered(CurrentSession.UnitOfWork, ele);
                }
            }

            //<UC:2.20>本次导入的Model 以'PO'和'PF'(2012-09-27)开头的Delivery，需要同步上传信息到整机数据库，如果在整机的数据库中的Delivery/DeliveryInfo/Delivery_Pallet/Pallet 表中存在，则需要将整机的数据库中的Delivery/DeliveryInfo/Delivery_Pallet/Pallet 表中的对应记录删除后再上传
            IList<Delivery> PODeliveryList = new List<Delivery>();
            IList<Pallet> POPltList = new List<Pallet>();
            IList<DeliveryPalletInfo> PODPList = new List<DeliveryPalletInfo>();
            IList<string> PODNList = new List<string>();
            IList<string> POPNList = new List<string>();
            //Find all DN records whose Model property starts with "PO"
            foreach (Delivery ele in deliveryList)
            {
                //Mantis0001695: Final Scan需增加check前一站是否有刷入,新增172及60 開頭機型
                if (ele.ModelName.StartsWith("PO") || 
                    ele.ModelName.StartsWith("PF") || 
                    ele.ModelName.StartsWith("172") || 
                    ele.ModelName.StartsWith("60"))
                {
                    PODeliveryList.Add(ele);
                    PODNList.Add(ele.DeliveryNo);
                }
            }
            //Find their related Delivery_Pallet records and PalletNo
            foreach (DeliveryPalletInfo ele in dpList)
            {
                if (PODNList.Contains(ele.deliveryNo))
                {
                    PODPList.Add(ele);
                    POPNList.Add(ele.palletNo);
                }
            }
            //Find their related Pallet records
            foreach (Pallet ele in palletList)
            {
                if (POPNList.Contains(ele.PalletNo))
                {
                    POPltList.Add(ele);
                }
            }

            /*
            * Answer to: ITC-1414-0176
            * Description: 导入的Model 以'PO'开头的Delivery，需要同步上传信息到整机数据库
            */
            if (PODNList.Count > 0)
            {
                //Call SP:Docking_ConnectPCDB to connect to PC database.
                //SP in-args:
                //  none.
                //productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                //                                           "Docking_ConnectPCDB");

                foreach (Delivery ele in PODeliveryList)
                {
                    //Call SP:Docking_DeleteDeliveryOfPC to do following:
                    //delete from Delivery_Pallet where DeliveryNo=...
                    //delete from DeliveryInfo where DeliveryNo=...
                    //delete from Delivery where DeliveryNo=...
                    //SP in-args:
                    //  @DeliveryNo varchar(20)
                    //productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                    //                                           "Docking_DeleteDeliveryOfPC",
                    //                                           new SqlParameter("DeliveryNo", ele.DeliveryNo));

                    //Call SP:Docking_AddDeliveryOfPC to insert a Delivery record.
                    //SP in-args:
                    //  @DeliveryNo char(20)
                    //  @ShipmentNo char(20)
                    //  @PoNo char(20)
                    //  @Model varchar(20)
                    //  @ShipDate datetime
                    //  @Qty int
                    //  @Status char(2)
                    //  @Editor varchar(30)
                    productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                                                               "Docking_AddDeliveryOfPC",
                                                               new SqlParameter("DeliveryNo", ele.DeliveryNo),
                                                               new SqlParameter("ShipmentNo", ele.ShipmentNo),
                                                               new SqlParameter("PoNo", ele.PoNo),
                                                               new SqlParameter("Model", ele.ModelName),
                                                               new SqlParameter("ShipDate", ele.ShipDate),
                                                               new SqlParameter("Qty", ele.Qty),
                                                               new SqlParameter("Status", ele.Status),
                                                               new SqlParameter("Editor", ele.Editor));

                    foreach (DeliveryInfo info in ele.DeliveryInfoes)
                    {
                        //Call SP:Docking_AddDeliveryInfoOfPC to insert a DeliveryInfo record.
                        //SP in-args:
                        //  @DeliveryNo char(20)
                        //  @InfoType char(20)
                        //  @InfoValue nvarchar(200)
                        //  @Editor varchar(30)
                        productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                                                                   "Docking_AddDeliveryInfoOfPC",
                                                                   new SqlParameter("DeliveryNo", ele.DeliveryNo),
                                                                   new SqlParameter("InfoType", info.InfoType),
                                                                   new SqlParameter("InfoValue", info.InfoValue),
                                                                   new SqlParameter("Editor", info.Editor));
                    }
                }
                foreach (Pallet ele in POPltList)
                {
                    //Call SP:Docking_DeletePalletOfPC to do following:
                    //delete from Delivery_Pallet where PalletNo=...
                    //delete from Pallet where PalletNo=...
                    //SP in-args:
                    //  @PalletNo varchar(20)
                    //productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                    //                                           "Docking_DeletePalletOfPC",
                    //                                           new SqlParameter("PalletNo", ele.PalletNo));

                    //Call SP:Docking_AddPalletOfPC to insert a Pallet record.
                    //SP in-args:
                    //  @PalletNo char(20)
                    //  @UCC char(30)
                    //  @Station char(10)
                    //  @Weight decimal(10,2)
                    //  @PalletModel char(10)
                    //  @Length decimal(10,2)
                    //  @Width decimal(10,2)
                    //  @Height decimal(10,2)
                    //  @Editor varchar(30)
                    //  @Weight_L decimal(10,2)
                    productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                                                               "Docking_AddPalletOfPC",
                                                               new SqlParameter("PalletNo", ele.PalletNo),
                                                               new SqlParameter("UCC", ele.UCC),
                                                               new SqlParameter("Station", ele.Station),
                                                               new SqlParameter("Weight", ele.Weight),
                        //Mantis0001695: Final Scan需增加check前一站是否有刷入,新增區別Docking 使用棧板
                                                               new SqlParameter("PalletModel", "Docking"),
                                                               //new SqlParameter("PalletModel", ele.PalletModel),
                                                               new SqlParameter("Length", ele.Length),
                                                               new SqlParameter("Width", ele.Width),
                                                               new SqlParameter("Height", ele.Height),
                                                               new SqlParameter("Editor", ele.Editor),
                                                               new SqlParameter("Weight_L", ele.Weight_L));
                }
                foreach (DeliveryPalletInfo ele in PODPList)
                {
                    //Call SP:Docking_AddDeliveryPalletOfPC to insert a Delivery_Pallet record.
                    //SP in-args:
                    //  @DeliveryNo char(20)
                    //  @PalletNo char(20)
                    //  @ShipmentNo char(20)
                    //  @DeliveryQty smallint
                    //  @Status char(1)
                    //  @Editor varchar(30)
                    //  @PalletType varchar(8)
                    //  @DeviceQty int
                    productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                                                               "Docking_AddDeliveryPalletOfPC",
                                                               new SqlParameter("DeliveryNo", ele.deliveryNo),
                                                               new SqlParameter("PalletNo", ele.palletNo),
                                                               new SqlParameter("ShipmentNo", ele.shipmentNo),
                                                               new SqlParameter("DeliveryQty", ele.deliveryQty),
                                                               new SqlParameter("Status", ele.status),
                                                               new SqlParameter("Editor", ele.editor),
                                                               new SqlParameter("PalletType", ele.palletType),
                                                               new SqlParameter("DeviceQty", ele.deviceQty));
                }

                //Call SP:Docking_DisconnectPCDB to disconnect from PC database.
                //SP in-args:
                //  none.
                //productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                //                                           "Docking_DisconnectPCDB");
            }

            return base.DoExecute(executionContext);
        }
    }
}
