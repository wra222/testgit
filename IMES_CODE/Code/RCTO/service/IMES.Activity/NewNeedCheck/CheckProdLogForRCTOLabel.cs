using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.ComponentModel;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///  
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class CheckProdLogForRCTOLabel : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public CheckProdLogForRCTOLabel()
        {
            InitializeComponent();
        }
        
        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            if (CurrentProduct != null)
            {
                string prodid = CurrentProduct.ProId;
                bool bFlag = false;
                //string RCTOLabelPrintStation = "81";
                string RCTOLabelPrintStation = "RL";
                IList<ProductLog> list = new List<ProductLog>();

                list = productRepository.GetProductLogs(prodid, RCTOLabelPrintStation);
                foreach (ProductLog temp in list)
                {
                    if (temp.Status == IMES.FisObject.Common.Station.StationStatus.Pass)
                    {
                        bFlag = true;
                        break;
                    }
                }

                if (!bFlag)
                {   
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK957", erpara);
                    throw ex;
                }
                else
                {
                    
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}