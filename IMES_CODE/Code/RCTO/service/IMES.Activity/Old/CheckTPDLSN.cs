/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 2009-01-08   207013     Modify: ITC-1103-0074 、ITC-1103-0011
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.BOM;

namespace IMES.Activity
{
    /// <summary>
    /// Check TPDLSN	
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于092CombineLCMBTDL.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// Check TPDL SN#	检查规则
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPDLSN
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///      ILCMRepository   
    /// </para> 
    /// </remarks>

    public partial class CheckTPDLSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckTPDLSN()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Check TPDL SN#	
        /// 长度大于15码，前2码=”BO”
        ///异常情况：
        ///A.	当输入的SN不正确时，提示”请输入正确的TPDL号码.”
        ///B.	当输入的TPDL SN已经存在于LCMBind,提示：“  ”
              //(select count(*) from IMES_FA..LCMBind where MESno=@tpdlsn)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            string tpdlsn = CurrentSession.GetValue(Session.SessionKeys.TPDLSN).ToString();
            ILCMRepository lcmRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();
            int lcmCount = lcmRepository.GetLCMBindCount(tpdlsn);

            if (lcmCount>0)
            {
                erpara.Add(tpdlsn);
                ex = new FisException("CHK081", erpara);
                ex.stopWF = false;
                throw ex;
            }
            return base.DoExecute(executionContext);
        }
    }
}
