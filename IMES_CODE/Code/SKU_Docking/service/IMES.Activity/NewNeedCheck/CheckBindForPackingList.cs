/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckBindForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
5. Doc_Type 选择 Pack List- Addition时，检查是否绑定unit；且对于前2位=”PO”的model需要重新抓取数据
异常情况：
A.	当没有绑定unit时，报错误信息”not combine Serial Number of this DN: ”+@DN ，焦点置于DN输入框
B.	当没有全部绑定时，报错误信息”機器還未完成或數量不對” ，焦点置于DN输入框
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
    public partial class CheckBindForPackingList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckBindForPackingList()
        {
            InitializeComponent();
        }

        // mantis 2073
        private void ChkQty(IList<string> dnList)
        {
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            int qtyPackComn = deliveryRep.GetPackComnSNCount(dnList);
            int qtyPackingData = deliveryRep.GetPackingDataSNCount(dnList);
            if (qtyPackingData != qtyPackComn)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(qtyPackComn.ToString());
                erpara.Add(qtyPackingData.ToString());
                ex = new FisException("CHK1032", erpara); // 机器数量不稳合! Comn数量=%1 , Packing数量=%2
                //ex.stopWF = false;
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            IList<string> internalIdList = new List<string>();
            string type = (string)CurrentSession.GetValue("Doc_type");
            string data = (string)CurrentSession.GetValue("Data");
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            //ITC-1360-1103
            //ITC-1360-1129
            if (type == "Pack List- Addition")
            {
                //not exists (select * from PAK_PackkingData nolock where InternalID in @deliverynolist)
                //其中@deliverynolist参考[4]中“依据刷入的code获取记录集”，得到的InternalID List
                IList<VShipmentPakComnInfo> comnList = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
                bool bExist = false;
                foreach (VShipmentPakComnInfo temp in comnList)
                    internalIdList.Add(temp.internalID);
                bExist = repPizza.CheckExistPakPackkingDataByDnList(internalIdList);
                if (bExist == false)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(data);
                    ex = new FisException("CHK821", erpara);
                    //ex.stopWF = false;
                    throw ex;
                }
                //5. 对于前2位=”PO”的model需要重新抓取数据
                //string data = (string)CurrentSession.GetValue("Data");
                bExist = false;
                //if exists(select top 1 * from [PAK_PAKComn] nolock where left(InternalID,10)=@DN and Model like 'PO%')
                if(data.Length >= 10)
                    bExist = repPizza.CheckExistPakDashPakComnByLikeInternalIDAndModel(data.Substring(0, 10), "PO");
                else
                    bExist = repPizza.CheckExistPakDashPakComnByLikeInternalIDAndModel(data, "PO");
                if (bExist == true)
                {
                    //insert  #DockingPackkingData
                    //select *  from [10.99.183.29].HP_EDI.dbo.[PAK_PackkingData] where InternalID like @DN+'%'
                    //if exists (select top 1 * from #DockingPackkingData)
                    IList<PakPackkingDataInfo> packingList = new List<PakPackkingDataInfo>();
                    packingList = repPizza.GetPakPackkingDataListByLikeInternalID(data);
                    if (packingList != null && packingList.Count != 0)
                    {
                        // insert PAK_PackkingData_Del
                        // select *,getdate() from PAK_PackkingData where InternalID in (select InternalID from #DockingPackkingData)        
                        IList<string> internalID1 = new List<string>();
                        foreach (PakPackkingDataInfo temp in packingList)
                        {
                            internalID1.Add(temp.internalID);
                        }
                        repPizza.InsertForBackupPakPackkingData(internalID1);
                        //delete from PAK_PackkingData where InternalID in (select InternalID from #DockingPackkingData)
                        repPizza.DeletePakPackkingData(internalID1);
                        //insert PAK_PackkingData select * from #DockingPackkingData
                        repPizza.InsertPakPackkingData(packingList);
                    }
                }

                IList<string> dnList = new List<string>();

                switch (InputType)
                {
                    case InputTypeEnum.DN:                                        
                        //5B. 没有全部绑定
                        decimal qty = 0;
                        int cqty = 0;
                        // 只针对刷入的是DN这种情况作此检查：
                        // select  @qty=sum(convert( NUMERIC(18,0),PACK_ID_UNIT_QTY)) from PAK_PAKComn where left(InternalID,10) = left(@deliveryno,10)        
                        //异常??????????
                        qty = repPizza.GetCountOfPakDashPakComnByLikeInternalID(data.Substring(0, 10));
                        //select @cqty=count(SERIAL_NUM) from PAK_PackkingData where left(InternalID,10) = left(@deliveryno,10)
                        cqty = repPizza.GetCountOfPakPackkingDataByLikeInternalID(data.Substring(0, 10));
                        if (cqty < qty)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            ex = new FisException("CHK820", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                        dnList.Add(data.Substring(0, 10));
                        break;
                    case InputTypeEnum.Shipment:
                        dnList = deliveryRep.GetEdiDnListByShipment(data);
                        break;
                    case InputTypeEnum.Waybill:
                        dnList = deliveryRep.GetEdiDnListByWayBill(data);
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

                ChkQty(dnList);
            }
            else if (type == "Pack List- Summary")
            {
                IList<string> dnList;
                string waybill = CurrentSession.GetValue("WayBill") as string;
                if (null == waybill || "" == waybill)
                {
                    FisException ex1;
                    List<string> erpara1 = new List<string>();
                    erpara1.Add("");
                    ex1 = new FisException("CHK1033", erpara1); // 無效的 Waybill %1
                    //ex1.stopWF = false;
                    throw ex1;
                }
                dnList = deliveryRep.GetEdiDnListByWayBill(waybill);

                ChkQty(dnList);
            }
            return base.DoExecute(executionContext);
        }
        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(CheckBindForPackingList));

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of CheckBindForPackingList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(CheckBindForPackingList.InputTypeProperty)));
            }
            set
            {
                base.SetValue(CheckBindForPackingList.InputTypeProperty, value);
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
        }
    }
}
