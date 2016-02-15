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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.BOM;

namespace IMES.Activity
{
    /// <summary>
    /// 用于check BTDL SN
    /// </summary>
    ///    <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// <para>
    /// 应用场景：
    ///      FA combine LCM BTDL/TPDL
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取BTDLSN，调用ILCMRepository.GetLCMBindCount
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.BTDLSN
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
    ///              ILCMRepository
    /// </para> 
    /// </remarks>
    public partial class CheckBTDLSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBTDLSN()
        {
            InitializeComponent();
        }

// 9. Check BTDL SN#	长度是17码或是20码，前2码=”BO”
//异常情况：
//A.	当输入的SN不正确时，提示”请输入正确的BTDL号码.”
//B.	当输入的BTDL SN#存在于LCMBind时，提示：“此BTDL已经绑定过灯管了！”
//        declare @btdlsn varchar(50)
//set @btdlsn=''
//select count(*) from IMES_FA..LCMBind where MESno=@btdlsn

/// <summary>
/// 根据规则检查BTDL SN
/// </summary>
/// <param name="executionContext"></param>
/// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            string btdlsn = CurrentSession.GetValue(Session.SessionKeys.BTDLSN).ToString();  
            ILCMRepository lcmRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();

            int lcmCount=lcmRepository.GetLCMBindCount(btdlsn);

            if (lcmCount>0)
            {
                erpara.Add(btdlsn);
                ex = new FisException("CHK080", erpara);
                ex.stopWF = false;  
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}
