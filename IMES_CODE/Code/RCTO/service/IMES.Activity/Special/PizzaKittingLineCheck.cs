using System;
using System.Collections.Generic;
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

namespace IMES.Activity.Special
{
    /// <summary>
    ///如果用户选择的[Line] 的第一码与刷入的Product 在PAK Cosmetic 选择的线别的第一码不同，则弹出对话框，报告错误：“此Product 不应在此线生产！ 该Product 在PAK Cosmetic 时选择的线别是” + @Line
    ///Remark:
    ///@Line – PAK Cosmetic 时选择的线别，可以访问ProductLog.Line 得到
    ///注意ProductLog 中可能存在多条PAK Cosmetic 的记录，取Cdt 最新的
    /// </summary>
    public partial class PizzaKittingLineCheck : BaseActivity
    {
        private const string PakCosmetic = "69";
        /// <summary>
        /// constructor
        /// </summary>
        public PizzaKittingLineCheck()
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
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (currentProduct.ProductLogs != null)
            {
                foreach (var log in currentProduct.ProductLogs)
                {
                    if (string.Compare(log.Station, PakCosmetic) == 0)
                    {
                        if (!string.IsNullOrEmpty(log.Line)
                            && !string.IsNullOrEmpty(this.Line)
                            && string.Compare(this.Line.Substring(0, 1), log.Line.Substring(0, 1)) != 0)
                        {
                            //报告错误：“此Product 不应在此线生产！ 该Product 在PAK Cosmetic 时选择的线别是” + @Line
                            FisException ex;
                            var erpara = new List<string>();
                            erpara.Add(log.Line);
                            ex = new FisException("CHK868", erpara);   
                            throw ex;
                        }
                        else
                        {
                            return base.DoExecute(executionContext);
                        }
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}
