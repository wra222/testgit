/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:  UC:CI-MES12-SPEC-FA-UI Generate Customer SN
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-12-22   207013            Create 
 * ITC-1360-1211 修正ReprintLog中Name
 * 
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

    public partial class CheckCUSTSNForDocking : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCUSTSNForDocking()
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
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            PrintLog condition = new PrintLog();
            condition.Name = curProduct.Customer + "SNO";
            condition.BeginNo = curProduct.ProId;

            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            IList<PrintLog> printLogList = repository.GetPrintLogListByCondition(condition);

            FisException ex;
            List<string> erpara = new List<string>();
            if (printLogList.Count == 0)
            {
                erpara.Add(curProduct.ProId);
                ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                throw ex;
            }

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, curProduct.Customer + "SNO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, curProduct.CUSTSN);

            return base.DoExecute(executionContext);
        }
    }
}
