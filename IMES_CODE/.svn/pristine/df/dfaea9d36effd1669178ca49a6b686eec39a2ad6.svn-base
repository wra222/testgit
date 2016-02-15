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
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

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
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            string currentCPQSNO = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN); //"CPQSNO");
            string currentBIOS = (string)CurrentSession.GetValue("BIOS");
            string currentImage = (string)CurrentSession.GetValue("Image");
            string currentFlag = (string)CurrentSession.GetValue("Flag");

            //ITC-1360-1279, Jessica Liu, 2012-3-7, UC更新
            //ITC-1360-1163, Jessica Liu, 2012-3-6,UC更新，":"改成";"
            //2011-11-5，合成version
            //string currentVersion = "BIOS:"+ currentBIOS + "~" + currentImage;

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //string currentVersion = "BIOS;" + currentBIOS + "~IMG;" + currentImage + "~";
            //mantis 1527
            string currentPN = "";
            string currentHash = "";
            string currentKey = "";
            DataTable tb = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_FA, "rpt_ITCNDTS_CHECK_Win8",
                new SqlParameter("CUSTSN", currentCPQSNO));
            if (tb != null && tb.Rows.Count > 0)
            {
                currentPN = tb.Rows[0][0].ToString();
                currentHash = currentKey = "SKIP";
            }
            string currentVersion = "BIOS;" + currentBIOS + "~IMG;" + currentImage;
            if (!string.IsNullOrEmpty(currentPN))
                currentVersion += "~PN;" + currentPN + "~HASH;" + currentHash + "~COA;" + currentKey;
            currentVersion += "~";
            
            //productRepository.CallRpt_ITCNDTS_SET_IMAGEDOWN_14(currentCPQSNO, currentFlag, currentVersion);
            //logger.Debug("cpqsno=" + currentCPQSNO + ", flag=" + currentFlag + ", version=" + currentVersion + ", editor=" + Editor);
            /*productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_FA, "rpt_ITCNDTS_SET_IMAGEDOWN_14_with_editor",
                new SqlParameter("cpqsno", currentCPQSNO),
                new SqlParameter("flag", currentFlag),
                new SqlParameter("version", currentVersion),
                new SqlParameter("editor", Editor));
            */

            tb = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_FA, "rpt_ITCNDTS_SET_IMAGEDOWN_14_with_editor",
                new SqlParameter("cpqsno", currentCPQSNO),
                new SqlParameter("flag", currentFlag),
                new SqlParameter("version", currentVersion),
                new SqlParameter("editor", Editor));
            string spResult = "";
            if (tb != null && tb.Rows.Count > 0)
            {
                spResult = tb.Rows[0][0].ToString();
                if (!"ok".Equals(spResult))
                {
                    if (tb.Rows[0].ItemArray.Length > 1)
                        spResult += tb.Rows[0][1].ToString();
                    throw new Exception(spResult);
                }
            }

            return base.DoExecute(executionContext);
        }
	
	}
}
