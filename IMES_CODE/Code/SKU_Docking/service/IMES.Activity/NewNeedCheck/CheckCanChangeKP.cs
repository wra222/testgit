/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Check if product can do change key parts.
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Activity
{
    /// <summary>
    /// Check if product can do change key parts.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change Key Parts.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     特殊卡站：存在ProductLog where .Sation=69 productID=prdID#   提示“已经到包装，不能Change KeyParts!”
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class CheckCanChangeKP : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckCanChangeKP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if product can do change key parts.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));

            IList<ProductLog> logList = productRepository.GetProductLogs(currentProduct.ProId, "69");

            if (logList != null && logList.Count != 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentProduct.ProId);
                throw new FisException("CHK501", errpara);
            }

            return base.DoExecute(executionContext);
        }
    }
}
