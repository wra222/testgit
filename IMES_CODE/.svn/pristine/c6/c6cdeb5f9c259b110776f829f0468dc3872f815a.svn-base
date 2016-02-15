/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckTrackForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
6. Doc_Type 选择 Pack List- Shipment时，检查是否绑定Tracking No，若没有则报错误信息”not combine Tracking No of this DN: ”+@DN，焦点置于DN输入框
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
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckTrackForPackingList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckTrackForPackingList()
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
            string type = (string)CurrentSession.GetValue("Doc_type");
            //ITC-1360-1090  "Pack List-Shipment"->"Pack List- Shipment"
            //ITC-1360-1136  "Pack List-Shipment"->"Pack List- Shipment"
            if (type == "Pack List- Shipment" || type == "ALL")
            {
                IList<VShipmentPakComnInfo> comnList = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
                foreach (VShipmentPakComnInfo temp in comnList)
                {
                    //if @Doc_Type = 'Pack List- Shipment' and @region = 'NA' and @sales_chan = 'Consumer' 
                    //and @order_type = 'CTO' and @intl_carrier = 'FDE'
                    if (temp.region == "NA" && temp.sales_chan == "Consumer"
                        && temp.order_type == "CTO" && temp.intl_carrier == "FDE")
                    {
                        string data = (string)CurrentSession.GetValue("Data");
                        bool bExist = false;
                        //exists (select * from dbo.PAK_PackkingData nolock where InternalID = @deliveryno and TRACK_NO_PARCEL <> '')
                        bExist = repPizza.CheckExistPakPackkingData(data);
                        if (bExist == false)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(data);
                            ex = new FisException("CHK826", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}

