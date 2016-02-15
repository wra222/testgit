/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/WriteLogForPackingList
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WriteLogForPackingList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WriteLogForPackingList()
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
            string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

            string doc_type = (string)CurrentSession.GetValue("Doc_type");
            if (doc_type == "ALL")
            {
                IList<string> processList = new List<string>();
                processList = (IList<string>)CurrentSession.GetValue("Doc_Type_List");
                foreach (string temp in processList)
                {
                    DnPrintListInfo item = new DnPrintListInfo();
                    item.dn = deliveryNo;
                    //ITC-1360-1092
                    item.editor = this.Editor;
                    item.doc_cat = temp;
                    item.cdt = DateTime.Now;
                    repPizza.InsertDnPrintListDefered(CurrentSession.UnitOfWork, item);
                }
            }
            else
            {
                DnPrintListInfo item = new DnPrintListInfo();

                item.dn = deliveryNo;
                //ITC-1360-1092
                item.editor = this.Editor;
                item.doc_cat = doc_type;
                item.cdt = DateTime.Now;
                repPizza.InsertDnPrintListDefered(CurrentSession.UnitOfWork, item);
            }
            
            return base.DoExecute(executionContext);
        }
    }
}

