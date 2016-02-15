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

            /*
            bool printFlag = false;
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            IProduct product =(IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
          
            string name = product.Customer + "SNO";
            repository.GetProductLogs(currentProduct.ProId, "PKOK");
            printFlag = repository.CheckExistPrintLogByLabelNameAndDescr(name, product.ProId);
            */
            var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<ProductLog> logList = repository.GetProductLogs(curProduct.ProId, this.Station);
            
            if (logList.Count == 0)
            {
                erpara.Add(curProduct.ProId);
                ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                throw ex;
            }
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, curProduct.Customer + "SNO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, curProduct.ProId);

            return base.DoExecute(executionContext);
        }
    }
}
