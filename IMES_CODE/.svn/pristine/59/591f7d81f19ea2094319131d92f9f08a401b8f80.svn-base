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
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// Check MBCT2 Label logical 
    /// </summary>
    public partial class CheckMBCT2LabelType : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckMBCT2LabelType()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Check MBCT2 Label logical
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //IList<PrintItem> resultPrintItemList = new List<PrintItem>();

            var printItemList = (IList<PrintItem>)CurrentSession.GetValue(Session.SessionKeys.PrintItems);

            //針對檢查MBCT2  LabelType 寫業務邏輯
            IList<string> matchedLabelTypeList = new List<string>();
            var CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (CurrentProduct == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            string CheckFamily = CurrentProduct.Family;

            IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<ConstValueTypeInfo> list = iPartRepository.GetConstValueTypeList("RCTOGenMBCT2RuleFamily");
            IList<ConstValueTypeInfo> query = (from q in list
                                               where q.value == CheckFamily
                                               select q).ToList<ConstValueTypeInfo>();
            string labelTypePrefix = ""; 
            if (query.Count > 0)
            {
                //matchedLabelTypeList.Add("CT2_LABEL");
                labelTypePrefix = "CT2";
            }
            else
            {
                //matchedLabelTypeList.Add("RCTO_Label");
                labelTypePrefix = "RCTO";
            }
            var resultPrintItemList = (from item in printItemList
                                       where item.LabelType.StartsWith(labelTypePrefix)
                                       select item).ToList();
            CurrentSession.AddValue(Session.SessionKeys.PrintItems, resultPrintItemList);                           
            return base.DoExecute(executionContext);
        }
	}
}
