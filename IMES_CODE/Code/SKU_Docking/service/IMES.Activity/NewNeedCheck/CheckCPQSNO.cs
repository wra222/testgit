/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Image Download Page
 * UI:CI-MES12-SPEC-FA-UI Image Download.docx –2011/10/28 
 * UC:CI-MES12-SPEC-FA-UC Image Download.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：[FA].[dbo].[rpt_ITCNDTS_SET_IMAGEDOWN_14]-数据接口尚未定义（in Activity：DoImageDownloadSave）
* UC 具体业务：检查Customer SN 是否存在（具体见接口需求表）-数据接口尚未定义（in Activity：CheckCPQSNO）
* UC 具体业务：select @grade=Grade from PAK.dbo.HP_Grade where Family=@descr-数据接口尚未定义（in Activity：DoImageDownloadSave）
* UC 具体业务：根据@flag 进行不同的处理（具体见接口需求表）-数据接口尚未定义（in Activity：DoImageDownloadSave）
*/


using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;


namespace IMES.Activity
{
    /// <summary>
    /// 检查Customer SN是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ImageDownload
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.CustSN 
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class CheckCPQSNO : BaseActivity
	{
        /// <summary>
        /// InitializeComponent
        /// </summary>
        public CheckCPQSNO()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 检查Customer SN是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.InfoFormat("CheckCPQSNOActivity: Key: {0}", this.Key);

            string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN); 
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var currentProduct = productRepository.GetProductByCustomSn(customerSN);             
            if (currentProduct == null)
            {
                List<string> errpara = new List<string>();

                errpara.Add(customerSN);

                throw new FisException("PAK042", errpara);  
            }
            
            CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);

            return base.DoExecute(executionContext);
        }
	
	}
}
