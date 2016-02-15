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
* ITC-1360-1163, Jessica Liu, 2012-3-6
* ITC-1360-1279, Jessica Liu, 2012-3-7
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
    /// 调用专门接口CallRpt_ITCNDTS_SET_IMAGEDOWN_14进行ImageDownload的Save处理
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于BTChange
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
    ///         CPQSNO 
    ///         BIOS
    ///         Image
    ///         Flag
    ///         
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
    ///         IProductRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class DoImageDownloadSave : BaseActivity
	{
        /// <summary>
        /// InitializeComponent
        /// </summary>
        public DoImageDownloadSave()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 调用专门接口CallRpt_ITCNDTS_SET_IMAGEDOWN_14进行ImageDownload的Save处理
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string currentCPQSNO = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN); //"CPQSNO");
            string currentBIOS = (string)CurrentSession.GetValue("BIOS");
            string currentImage = (string)CurrentSession.GetValue("Image");
            string currentFlag = (string)CurrentSession.GetValue("Flag");

            //ITC-1360-1279, Jessica Liu, 2012-3-7, UC更新
            //ITC-1360-1163, Jessica Liu, 2012-3-6,UC更新，":"改成";"
            //2011-11-5，合成version
            //string currentVersion = "BIOS:"+ currentBIOS + "~" + currentImage;
            string currentVersion = "BIOS;" + currentBIOS + "~IMG;" + currentImage + "~";
            
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                        
            productRepository.CallRpt_ITCNDTS_SET_IMAGEDOWN_14(currentCPQSNO, currentFlag, currentVersion);

            return base.DoExecute(executionContext);
        }
	
	}
}
