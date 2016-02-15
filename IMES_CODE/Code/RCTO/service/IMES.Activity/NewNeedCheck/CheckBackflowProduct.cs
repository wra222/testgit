/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for QC Repair Page
* UI:CI-MES12-SPEC-FA-UC QC Repair.docx –2012/2/16 
* UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2012/7/18            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-7-18   Jessica Liu           Create
* Known issues:
* TODO：
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

namespace IMES.Activity
{
    /// <summary>
    /// 判断是否是Product回流机器
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      QC Repair、PAQC Repair
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断是否是Product回流机器
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         IsBackflowProduct
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///             
    /// </para> 
    /// </remarks>
    public partial class CheckBackflowProduct : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBackflowProduct()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 判断是否是Product回流机器
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            bool isBackflowProductFlag = false;

            //select @RepairID = ID from ProductRepair nolock where ProductID = @PrdID order by Udt desc
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            int repairId = productRepository.GetNewestProductRepairIdRegardlessStatus(currenProduct.ProId);

            //select distinct Cause from ProductRepair_DefectInfo where ProductRepairID = @RepairID
            IList<string> causeLst = productRepository.GetCauseListByProductRepairId(repairId);

            if ((causeLst != null) && (causeLst.Count == 1))
            {
                if ((causeLst[0].Substring(0, 2).ToUpper() == "CN") || (causeLst[0].Substring(0, 2).ToUpper() == "WW"))
                {
                    isBackflowProductFlag = true;
                }
            }

            CurrentSession.AddValue("IsBackflowProduct", isBackflowProductFlag);

            return base.DoExecute(executionContext);
        }
    }
}
