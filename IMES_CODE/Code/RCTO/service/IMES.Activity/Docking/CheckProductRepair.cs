/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Generate Customer SN For Docking Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-18   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1414-0108, Jessica Liu, 2012-6-7
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.Common.PrintLog;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Product是否有未修护的记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Generate Customer SN For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK206
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckProductRepair : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductRepair()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Product是否有未修护的记录
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();

            IList<string> productRepairLst = productRepository.GetProductRepairByProIdAndStatus(currentProduct.ProId, 0);
            if (productRepairLst.Count > 0)
            {
                //去ProductRepair表中去找，找到status等于0的，代表未修完(也就是维修过程中的)，就报错
                List<string> errpara = new List<string>();
                //ITC-1414-0108, Jessica Liu, 2012-6-7
                throw new FisException("CHK283", errpara);
            }

            return base.DoExecute(executionContext);
        }
    }
}
