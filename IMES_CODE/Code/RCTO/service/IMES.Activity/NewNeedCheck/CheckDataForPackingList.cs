/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDataForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
* A. 刷入的是DN
*  exists (select * from [PAK_PAKComn] nolock where left(InternalID,10)=@DN)
* B.刷入的是Shipment
* exists(select * from v_Shipment_PAKComn nolock where CONSOL_INVOICE=@DN or SHIPMENT = @DN)
* C.输入的是WayBill Number
* exists(select * from v_Shipment_PAKComn nolock where WAYBILL_NUMBER=@DN )
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// Check Data: DN  or   Shipment   or  Waybill
    /// </summary>
    public partial class CheckDataForPackingList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDataForPackingList()
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
            string data = (string)CurrentSession.GetValue("Data");
            //ITC-1360-1104
            
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            bool flag = false;
            //ITC-1360-1104
            //ITC-1360-1148
            //ITC-1360-1147
            if (!String.IsNullOrEmpty(data) && data.Length >= 10)
            {
                flag = repPizza.CheckExistPakDashPakComnByInternalID(data.Substring(0, 10));
                if (flag == true)
                {
                    CurrentSession.AddValue(Session.SessionKeys.VCode, "DN");
                    return base.DoExecute(executionContext);
                }
            }
            flag = repPizza.CheckExistVShipmentPakComnByConsolInvoiceOrShipment(data);
            if (flag == true)
            {
                CurrentSession.AddValue(Session.SessionKeys.VCode, "Shipment");
                return base.DoExecute(executionContext);                
            }
            flag = repPizza.CheckExistVShipmentPakComnByWaybillNo(data);
            if (flag == true)
            {
                CurrentSession.AddValue(Session.SessionKeys.VCode, "Waybill");
                return base.DoExecute(executionContext);                
            }
            //6.18 BOL
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            DeliveryInfo cond = new DeliveryInfo();
            cond.InfoType = "BOL";
            cond.InfoValue = data;
            IList<DeliveryInfo> infoList = new List<DeliveryInfo>();
            infoList = DeliveryRepository.GetDeliveryInfoList(cond);
            if (infoList != null && infoList.Count > 0)
            {
                IList<string> dnList = new List<string>();
                dnList = DeliveryRepository.GetDeliveryNoPrefixByValueAndType(data, "BOL");

                CurrentSession.AddValue(Session.SessionKeys.VCode, "BOL");
                CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnList);
                CurrentSession.AddValue(Session.SessionKeys.DnCount, dnList.Count);
                CurrentSession.AddValue(Session.SessionKeys.DnIndex, 0);

                ArrayList BOLpdfList = new ArrayList();
                CurrentSession.AddValue("BOLPdfList", BOLpdfList);
                return base.DoExecute(executionContext); 
            }
            //6.18 BOL

            CurrentSession.AddValue(Session.SessionKeys.VCode, "Unknown");
            List<string> errpara = new List<string>();
            errpara.Add(data);
            FisException ex = new FisException("CHK816", errpara);
            //ex.stopWF = false;
            throw ex;
            //CurrentSession.AddValue(Session.SessionKeys.VCode, "Unknown");
            //return base.DoExecute(executionContext);
        }        
    }
}

