// INVENTEC corporation (c)2011 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC PCA Shipping Label Print.docx
//              Check MAC      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-08   zhu lei                      create
// Known issues:
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 检查MB是否有重复的MAC
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站
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
    ///         Session.SessionKeys.MB 
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        MB
    /// </para> 
    /// </remarks>
    public partial class CheckMac : BaseActivity
	{
        /// <summary>
        /// CheckMac
        /// </summary>
        public CheckMac()
		{
			InitializeComponent();
		}

        //ITC-1103-0050
        /// <summary>
        /// MB是否有重复的MAC
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IPartRepository CurrentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string macValue = CurrentPartRepository.GetPartInfoValue(currentMB.PCBModelID, "MAC");
            string mac = (string)currentMB.MAC;
            if (macValue == "T")
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<IProduct> prd = productRepository.GetProductlistByMac(mac);
                //IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                //是否存在重复的MAC，需要报告错误：“Duplicate MAC Address!”
                if (prd.Count() > 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(mac);
                    throw new FisException("CHK160", errpara);
                }
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
