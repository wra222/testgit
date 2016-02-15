/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 2009-01-08   207013     Modify: ITC-1103-0074 、ITC-1103-0011
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;

namespace IMES.Activity
{

    public partial class CheckCUSTSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCUSTSN()
        {
            InitializeComponent();
        }
        /// <summary>
        /// check CUST SN是否为空或未产生
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            IProduct product =(IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //Customer SN为空或未产生(product   %1)!
            if (string.IsNullOrEmpty(product.CUSTSN))
            {
                erpara.Add(product.ProId);
                ex = new FisException("CHK073", erpara);
                throw ex;
            }
            //ITC-1122-0119
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.Model);
            return base.DoExecute(executionContext);
        }
    }
}
