/*
 * INVENTEC corporation (c)2011 all rights reserved. 
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2012-2-28
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2012-2-28     
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-2-28   Dorothy            Create 
 * ITC-1360-0466 增加特殊卡站判断
 * ITC-1360-0581 同466增加特殊卡站判断
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    public partial class BlockForCenerateCustSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BlockForCenerateCustSN()
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
            IProduct curProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model curModel = modelRep.Find(curProduct.Model);

            //2012-7-10, Jesscia Liu, For RCTO
            //若Left(Product.Model,3)<>’173’ 且<>’146’，则进行如下检查
            string tempStr = curProduct.Model.Substring(0, 3);
            if ((tempStr != "173") && (tempStr != "146"))
            {
                string stage = curModel.GetAttribute("STAG");
                if (stage == "S")
                {
                    erpara.Add(this.Key);
                    ex = new FisException("CHK865", erpara);//此Product设置为在Master Label Print中打印Customer SN,打印请求无效，请Check
                    throw ex;
                }
                else if (stage == "T")
                {   //如果STAG=T 检查 Master Label 是否打印（查找WC=59的记录）
                    IList<ProductLog> logList = repository.GetProductLogs(curProduct.ProId, "59");
                    if (logList.Count == 0)
                    {
                        erpara.Add(this.Key);
                        ex = new FisException("CHK866", erpara);//机器需要贴附Master Label，请先去打印Master Label
                        throw ex;
                    }
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
