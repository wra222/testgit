/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * UI:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13 
 * UC:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13   
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-2-1   Dorothy            Create 
  * ITC-1360-795 修改productlog查询条件
 *  ITC-1360-799 修改productlog查询条件
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    public partial class CheckReprintLabel : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckReprintLabel()
        {
            InitializeComponent();
        }
        /// <summary>
        /// check label是否需要reprint
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            
            var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct product =(IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string curLine = (string)CurrentSession.GetValue(Session.SessionKeys.LineCode);
            
            IList<ProductLog> logList = repository.GetProductLogs("8D", 1, curLine);

            bool findflag = false;
            foreach (ProductLog node in logList)
            {
                if (node.ProductID == product.ProId)
                {
                    findflag = true;
                    break;
                }
            }

            if (!findflag)
            {
                erpara.Add(product.ProId);
                ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                throw ex;
            }

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, product.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.ProId);
            return base.DoExecute(executionContext);
        }
    }
}
