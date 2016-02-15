/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDataForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
依据刷入不同的code判断方式不同：
A. 刷入的是DN
select @shipdate=ACTUAL_SHIPDATE,@PO_NUM = PO_NUM, @deliveryno = InternalID,@shipment = rtrim(CONSOL_INVOICE),@model = Model,@region = REGION,@sales_chan = SALES_CHAN,@order_type = ORDER_TYPE,@intl_carrier = INTL_CARRIER
			from dbo.v_Shipment_PAKComn nolock	where left(InternalID,10)=@DN
if not exists(select * from [PAK.PAKEdi850raw] where PO_NUM = @PO_NUM)
	begin
		select '0','EDI850資料沒有上傳: '+@DN
		return
	end
B.刷入的是Shipment
select distinct InternalID,Model,PO_NUM,ACTUAL_SHIPDATE into #3 from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN
if  exists(select * from #3 where PO_NUM not in (select PO_NUM from [PAK.PAKEdi850raw]))
	begin
		select '0','EDI850資料沒有上傳: '+@DN
		return
	end
C.输入的是WayBill Number
select distinct InternalID,Model,PO_NUM ,ACTUAL_SHIPDATE  into #1 from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN 
if  exists(select * from #1 where PO_NUM not in (select PO_NUM from [PAK.PAKEdi850raw]))
	begin
		select '0','EDI850資料沒有上傳: '+@DN
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

namespace IMES.Activity.Special
{   
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckEDIForPackingList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckEDIForPackingList()
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
            IList<VShipmentPakComnInfo> comnList = new List<VShipmentPakComnInfo>();
            
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            bool bEDI = false;

            switch (InputType)
            {
                case InputTypeEnum.DN:
                    string dn = (string)CurrentSession.GetValue("Data");
                    //3C. 没有上传EDI资料
                    comnList = repPizza.GetVShipmentPakComnListByLikeInternalID(dn.Substring(0, 10));
                    foreach (VShipmentPakComnInfo temp in comnList)
                    {                        
                        bEDI = repPizza.CheckExistPAKDotPAKEdi850RawByPoNum(temp.po_num);
                        if (bEDI == true)
                            break;
                    }
                    if (bEDI == false)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(dn);
                        ex = new FisException("CHK817", erpara);
                        //ex.stopWF = false;
                        throw ex;
                    }
                    CurrentSession.AddValue("ComnList", comnList);
                    break;
                case InputTypeEnum.DNForPL:                    
                    string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                    //3F. 没有上传EDI资料
                    comnList = repPizza.GetVShipmentPakComnListByInternalID(deliveryNo);
                    foreach (VShipmentPakComnInfo temp in comnList)
                    {
                        bEDI = repPizza.CheckExistPAKDotPAKEdi850RawByPoNum(temp.po_num);
                        if (bEDI == true)
                            break;
                    }
                    if (bEDI == false)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(deliveryNo);
                        ex = new FisException("CHK817", erpara);
                        throw ex;
                    }
                    CurrentSession.AddValue("ComnList", comnList);
                    break;
                case InputTypeEnum.Shipment:
                    string shipment = (string)CurrentSession.GetValue("Data");
                    
                    //3C. 没有上传EDI资料
                    comnList = repPizza.GetVShipmentPakComnByConsolInvoiceOrShipment(shipment);
                    foreach (VShipmentPakComnInfo temp in comnList)
                    {
                        IList<string> poList = new List<string>();
                        poList.Add(temp.po_num);

                        //接口疑问????
                        bEDI = repPizza.CheckExistPoNumNotInPAKDotPAKEdi850Raw(poList);
                        if (bEDI == true)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(shipment);
                            ex = new FisException("CHK817", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                    }
                    CurrentSession.AddValue("ComnList", comnList);
                    break;
                case InputTypeEnum.Waybill:
                    string waybill = (string)CurrentSession.GetValue("Data");
                    
                    //3C. 没有上传EDI资料
                    comnList = repPizza.GetVShipmentPakComnListByWaybillNo(waybill);
                    foreach (VShipmentPakComnInfo temp in comnList)
                    {
                        IList<string> poList1 = new List<string>();
                        poList1.Add(temp.po_num);

                        bEDI = repPizza.CheckExistPoNumNotInPAKDotPAKEdi850Raw(poList1);
                        //接口疑问????
                        if (bEDI == true)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(waybill);
                            ex = new FisException("CHK817", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                    }
                    CurrentSession.AddValue("ComnList", comnList);
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
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(CheckEDIForPackingList));

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of CheckEDIForPackingList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(CheckEDIForPackingList.InputTypeProperty)));
            }
            set
            {
                base.SetValue(CheckEDIForPackingList.InputTypeProperty, value);
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
