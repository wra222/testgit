// INVENTEC corporation (c)2012 all rights reserved. 
// Description:检查mbSno是否存在 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-20   zhueli                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;
using IMES.FisObject.Common.PrintLog;

namespace IMES.Activity
{

    /// <summary>
    /// 检查mbSno是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC PCA Shipping Label Reprint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.	select * from IMES2012_GetData..PrintLog where @mbSno between BegNo and EndNo and Name=@Tp
    ///          若不存在，则报错：mbSno不存在
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckPrintLogForIEC : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPrintLogForIEC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查mbSno是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string dCode = "";
            string rev = "";
            CurrentSession.AddValue(Session.SessionKeys.DCode, dCode);
            CurrentSession.AddValue(Session.SessionKeys.IECVersion, rev);

            string ct = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);


            var CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();

            IList<PrintLog> PrintLogList = CurrentRepository.GetPrintLogListByRange(ct, "KP");
            if (PrintLogList == null || PrintLogList.Count == 0)
            {
                throw new FisException("CHK209", new string[] { ct });
            }
            else
            {
                string[] tmpLst = PrintLogList[0].Descr.Split(',');
                if (tmpLst == null || tmpLst.Length < 2)
                {
                    throw new FisException("IEC001", new string[] { });
                }
                dCode = tmpLst[0];
                rev = tmpLst[1];
                CurrentSession.AddValue(Session.SessionKeys.DCode, dCode);
                CurrentSession.AddValue(Session.SessionKeys.IECVersion, rev);
            }
            string descr = dCode + "," + rev;
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "KP");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, ct);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, ct);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, descr);


            return base.DoExecute(executionContext);
        }

    }
}

