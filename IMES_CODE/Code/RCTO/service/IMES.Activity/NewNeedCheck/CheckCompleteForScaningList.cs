/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDataForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16                  (Reference Ebook SourceCode) Create
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
    public partial class CheckCompleteForScaningList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCompleteForScaningList()
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
            string type = (string)CurrentSession.GetValue("VCode");
            string data = (string)CurrentSession.GetValue("DeliveryNo");
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            decimal qty = 0, cqty = 0;


            if (type == "DN")
            {
                IList<string> ConsoleId = repPizza.GetConsolInvoiceFromVShipmentPakComn(data);
                IList<string> InvoId=repPizza.GetInternalIdsFromVShipmentPakComnByConsolInvoice(ConsoleId[0]);

                qty = repPizza.GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(InvoId);
                cqty = repPizza.GetCountOfPakPackkingData(InvoId);

            }

            if (type == "Shipment")
            {
                IList<string> ConsoleId = repPizza.GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipment(data);
                qty = repPizza.GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(ConsoleId);

                cqty = repPizza.GetCountOfPakPackkingDataWithDistinctSerialNum(ConsoleId);
            }

            if (qty > cqty)
            {
                List<string> errpara = new List<string>();
                errpara.Add("機器還未完成或數量不對!");
                FisException ex = new FisException("CHK274", errpara);
                //ex.stopWF = false;
                throw ex;

            }

            return base.DoExecute(executionContext);
                      
        }        
    }
}

