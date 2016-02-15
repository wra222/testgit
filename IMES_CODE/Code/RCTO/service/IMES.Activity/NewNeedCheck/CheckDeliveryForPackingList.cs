/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDeliveryForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-20   zhanghe               (Reference Ebook SourceCode) Create
D.	当对应的DN还没有完成时，报错误信息” 此筆船務尚未完成” ，焦点置于sn输入框
E.	当对应的DN已打印时，报错误信息” 此筆船務已經打印過packing list” ，焦点置于sn输入框
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckDeliveryForPackingList : BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public CheckDeliveryForPackingList()
        {
            InitializeComponent();
        }

        /// <summary>        
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {            
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            int currentStationCombineQty = (int)CurrentSession.GetValue("CurrentStationCombineQty");
            bool bFlag = false;
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            //3D.
            bFlag = currentDelivery.IsDNFull(currentStationCombineQty);
            if (bFlag != true)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHK824", erpara);
                throw ex;
            }
            //3E此筆船務已經打印過packing list
            string print_flag = (string)CurrentSession.GetValue("Print_Flag");
            IList<string> processList = new List<string>();
            processList = repPizza.GetDocCatFromPakDotParRt("Pack List", "Pack List- Transportation");
            CurrentSession.AddValue("Doc_Type_List", processList);

            if (!String.IsNullOrEmpty(print_flag) && print_flag == "print")
            {
                string doc_type = (string)CurrentSession.GetValue("Doc_type");
                if (!String.IsNullOrEmpty(doc_type))
                {
                    if (doc_type == "ALL")
                    {
                        if (repPizza.CheckExistDnPrintList(currentDelivery.DeliveryNo))
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            ex = new FisException("CHK825", erpara);
                            throw ex;
                        }
                    }
                    else
                    {
                        if (repPizza.CheckExistDnPrintList(currentDelivery.DeliveryNo, doc_type))
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            ex = new FisException("CHK825", erpara);
                            throw ex;
                        }
                    }
                }
            }
            else if (!String.IsNullOrEmpty(print_flag) && print_flag == "reprint")
            {
                string doc_type = (string)CurrentSession.GetValue("Doc_type");
                if (!String.IsNullOrEmpty(doc_type))
                {
                    if (doc_type == "ALL")
                    {
                        foreach (string temp in processList)
                        {
                            if (!repPizza.CheckExistDnPrintList(currentDelivery.DeliveryNo, temp))
                            {
                                FisException ex;
                                List<string> erpara = new List<string>();
                                ex = new FisException("CHK846", erpara);
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        if (!repPizza.CheckExistDnPrintList(currentDelivery.DeliveryNo, doc_type))
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            ex = new FisException("CHK846", erpara);
                            throw ex;
                        }
                    }
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
