// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Get QCStatus of product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-08-21    itc202017                     create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// Get QCStatus of product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      OQC output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         	若QCStatus存在QCStatus.ProductID=[ProductID] and Remark=’1’的记录，则Product为EPIA
    ///         	若QCStatus.Status=1(Codition: ProductID=[ProductID] order by Cdt desc)，则Product为免检
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Product
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
    ///         none
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class GetQCStatus : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetQCStatus()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Get QCStatus of Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /// 

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            List<string> errpara = new List<string>();
            string productID = currentProduct.ProId;

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //ProductLog 
            string[] tps = new string[1];
            tps[0] = "PIA2";
            string status = "EFI";  //exemption from inspection
            IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
            QCStatusList = productRepository.GetQCStatusOrderByCdtDesc(productID, tps);
            if (QCStatusList != null && QCStatusList.Count > 0)
            {
                foreach (ProductQCStatus qcStatus in QCStatusList)
                {
                    if (qcStatus.Remark == "1")
                    {
                        status = "EPIA";
                        break;
                    }
                }
            }
            CurrentSession.AddValue(Session.SessionKeys.QCStatus, status);
            return base.DoExecute(executionContext);
        }
    }
}
