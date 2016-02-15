/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDataForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
1)写入Packinglist_RePrint
依据刷入的code获取记录集：
刷入的是DN：
select * from v_Shipment_PAKComn nolock where left(InternalID,10)=@DN
刷入的是Shipment：
select distinct InternalID,Model,PO_NUM,ACTUAL_SHIPDATE into #3 from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN
输入的是WayBill Number
select distinct InternalID,Model,PO_NUM ,ACTUAL_SHIPDATE  into #1 from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN

根据以上得到的集合，若其model在FA..LocalMaintain 没有定义weight，若在PAK_SkuMasterWeight_FIS能找到model前4位一样的数据(left(PAK_SkuMasterWeight_FIS.Model,4)=left(@model,4))时，将写入Packinglist_RePrint：
FA..LocalMaintain where Tp='SW'  and a.Code=@Model     
insert  into  Packinglist_RePrint  
  values (@DN--刷入的code，ACTUAL_SHIPDATE, @model)

2)写入PAK_SkuMasterWeight_FIS
按照model在PAK_SkuMasterWeight_FIS里找不到记录的，需要得到weight写入
insert into PAK_SkuMasterWeight_FIS values(@model,@weight,getdate())
其中
@weight = convert(decimal(10,3), sum(Weight)/count(*)) from PAK_SkuMasterWeight_FIS where Left(Model,4) = left(@model,4) and substring(Model,10,2)<>'25'
报错：
if @weight is null 
 begin
  select '0','['+@DN+']'+'the weight of this '+@model+' is not found!'
  return
 end   
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK.Paking;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.StandardWeight;

namespace IMES.Activity.Special
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SaveForPackingList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public SaveForPackingList()
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
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            IModelWeightRepository ModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, ModelWeight>();
            IList<PakSkuMasterWeightFisInfo> weightList = new List<PakSkuMasterWeightFisInfo>();
            
            switch (InputType)
            {
                case InputTypeEnum.DN:
                case InputTypeEnum.Shipment:
                case InputTypeEnum.Waybill:
                    IList<VShipmentPakComnInfo> comnList1 = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
                    string data = (string)CurrentSession.GetValue("Data");

                    foreach (VShipmentPakComnInfo temp in comnList1)
                    {
                        //若其model在IMES_PAK..ModelWeight.UnitWeight 没有定义weight
                        //若在PAK_SkuMasterWeight_FIS能找到model前4位一样的数据(left(PAK_SkuMasterWeight_FIS.Model,4)=left(@model,4))时
                        ModelWeight mw = new ModelWeight();
                        mw = ModelWeightRepository.Find(temp.model);                                                 
                        weightList = repPizza.GetPakSkuMasterWeightFisListByLikeModel(temp.model.Substring(0, 4));
                        if ((mw == null || mw.UnitWeight == 0) && (weightList != null && weightList.Count != 0))
                        {                            
                            //将写入Packinglist_RePrint：insert  into  Packinglist_RePrint values (@DN--刷入的code，ACTUAL_SHIPDATE, @model)
                            PackinglistRePrintInfo item = new PackinglistRePrintInfo();
                            item.model = temp.model;
                            item.dn = data;                            
                            item.shipDate = temp.actual_shipdate;
                            repPizza.InsertPackinglistRePrint(item);                            
                        }
                        //按照model在PAK_SkuMasterWeight_FIS里找不到记录的，需要得到weight写入
                        //insert into PAK_SkuMasterWeight_FIS values(@model,@weight,getdate())                        
                        if(!repPizza.CheckExistModelInPakSkuMasterWeightFis(temp.model))                        
                        {
                            decimal weight = 0;
                            PakSkuMasterWeightFisInfo newinfo = new PakSkuMasterWeightFisInfo();
                            //@weight = convert(decimal(10,3), sum(Weight)/count(*)) from PAK_SkuMasterWeight_FIS where Left(Model,4) = left(@model,4) and substring(Model,10,2)<>'25'
                            //UC疑问??????????
                            
                            weight = repPizza.GetAverageWeightFromPakSkuMasterWeightFisByModel(temp.model.Substring(0,4));
                            if (weight < 0)
                            {
                                FisException ex;
                                List<string> erpara = new List<string>();
                                //ITC-1360-1096
                                erpara.Add(data);
                                erpara.Add(temp.model);
                                ex = new FisException("CHK819", erpara);
                                //ex.stopWF = false;
                                throw ex;
                            }
                            else
                            {                                
                                newinfo.weight = weight;
                                newinfo.model = temp.model;
                                newinfo.cdt = DateTime.Now;
                                repPizza.InsertPakSkuMasterWeightFis(newinfo);                        
                            }
                        }
                    }
                    break;
                case InputTypeEnum.DNForPL:
                    //4.保存数据
                    IList<VShipmentPakComnInfo> listShip = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
                    string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

                    foreach (VShipmentPakComnInfo temp in listShip)
                    {
                        //按照model在PAK_SkuMasterWeight_FIS里找不到记录的，需要得到weight写入
                        //insert into PAK_SkuMasterWeight_FIS values(@model,@weight,getdate())                    
                        if (!repPizza.CheckExistModelInPakSkuMasterWeightFis(temp.model))
                        {
                            //@weight = convert(decimal(10,3), sum(Weight)/count(*)) from PAK_SkuMasterWeight_FIS where Left(Model,4) = left(@model,4) 
                            decimal weight;
                            //ITC-1360-1042  底层接口返回值修改为-1
                            weight = repPizza.GetAverageWeightFromPakSkuMasterWeightFis(temp.model.Substring(0, 4));
                            if (weight < 0)
                            {
                                FisException ex;
                                List<string> erpara = new List<string>();
                                erpara.Add(deliveryNo);
                                erpara.Add(temp.model);
                                ex = new FisException("CHK819", erpara);
                                throw ex;
                            }
                            PakSkuMasterWeightFisInfo item = new PakSkuMasterWeightFisInfo();
                            item.weight = weight;
                            item.model = temp.model;
                            item.cdt = DateTime.Now;
                            repPizza.InsertPakSkuMasterWeightFis(item);
                        }
                        IList<decimal> wList = new List<decimal>();
                        //select @weight = Weight from PAK_SkuMasterWeight_FIS where Model = @model if @shipment = ''
                        wList = repPizza.GetWeightFromPakSkuMasterWeightFis(temp.model);
                        foreach (decimal y in wList)
                        {
                            //if @shipment = '
                            if (string.IsNullOrEmpty(temp.consol_invoice.TrimEnd()))
                            {
                                // if not exists(select * from PAK_ShipmentWeight_FIS where Shipment = left(@DN,10))
                                if (!repPizza.CheckExistPakShipmentWeightFis(deliveryNo.Substring(0, 10)))
                                {
                                    IList<decimal> qtyList = new List<decimal>();
                                    //select @qty = PACK_ID_LINE_ITEM_BOX_QTY from v_Shipment_PAKComn where InternalID = @deliveryno


                                    //ITC-1360-1049  底层接口抛异常 返回值修改为<decimal>
                                    qtyList = repPizza.GetPackIdLineItemBoxQtyFromPakShipmentWeightFis(deliveryNo);

                                    foreach (decimal t in qtyList)
                                    {
                                        PakShipmentWeightFisInfo item1 = new PakShipmentWeightFisInfo();
                                        item1.weight = y * t;
                                        item1.cdt = DateTime.Now;
                                        item1.type = "D";
                                        item1.shipment = deliveryNo;
                                        //insert into PAK_ShipmentWeight_FIS values(@DN,'D',convert(decimal(10,3),@qty)*@weight,getdate())
                                        repPizza.InsertPakShipmentWeightFis(item1);
                                    }
                                }
                            }
                            else
                            {
                                //if not exists (select * from PAK_ShipmentWeight_FIS where Shipment = @shipment)
                                if (!repPizza.CheckExistPakShipmentWeightFis(temp.consol_invoice.TrimEnd()))
                                {
                                    decimal qty;
                                    //select @qty = sum(PACK_ID_LINE_ITEM_BOX_QTY) from [v_Shipment_PAKComn] where CONSOL_INVOICE = @shipment
                                    qty = repPizza.GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(temp.consol_invoice.TrimEnd());
                                    PakShipmentWeightFisInfo item1 = new PakShipmentWeightFisInfo();
                                    item1.weight = y * qty;
                                    item1.cdt = DateTime.Now;
                                    item1.type = "S";
                                    item1.shipment = temp.consol_invoice.TrimEnd();
                                    //insert into PAK_ShipmentWeight_FIS values(@shipment,'S',convert(decimal(10,3),@qty)*@weight,getdate())
                                    repPizza.InsertPakShipmentWeightFis(item1);
                                }
                            }
                        }
                    }                    
                    break;
                default:
                    FisException ex1;
                    List<string> erpara1 = new List<string>();
                    erpara1.Add((string)CurrentSession.GetValue("Data"));
                    ex1 = new FisException("CHK816", erpara1);
                    //ex1.stopWF = false;
                    throw ex1;
                //break;
            }

            return base.DoExecute(executionContext);
        }
        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(SaveForPackingList));

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of SaveForPackingList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(SaveForPackingList.InputTypeProperty)));
            }
            set
            {
                base.SetValue(SaveForPackingList.InputTypeProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 
            /// </summary>
            DN = 0,
            /// <summary>
            /// 
            /// </summary>
            Shipment = 1,
            /// <summary>
            /// 
            /// </summary>
            Waybill = 2,
            /// <summary>
            /// 
            /// </summary>
            DNForPL = 3,
        }
    }
}
