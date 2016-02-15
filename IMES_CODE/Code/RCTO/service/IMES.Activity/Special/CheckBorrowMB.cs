// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.检查MB是否已经结合,若结合,则报错;此MB已经结合,且未解除,不能外借
//      2.检查MB是否存在不良,若存在不良,则报告错误请先修复此MB
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{

    /// <summary>
    /// 检查MB是否能满足借出条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-SA-UC MB Borrow Control
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.检查MB是否已经结合,若结合,则报错;此MB已经结合,且未解除,不能外借
    ///      2.检查MB是否存在不良,若存在不良,则报告错误请先修复此MB
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK155
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
    public partial class CheckBorrowMB : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBorrowMB()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查MB是否能满足借出条件
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;

            IProductRepository CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct bindProduct = CurrentProductRepository.GetProductByMBSn(currenMB.Sn);
            if (bindProduct != null)
            {
                throw new FisException("BOR004", new string[] { });
            }

            IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            bool ExistDefect = CurrentMBRepository.ExistPCBRepair(currenMB.Sn,0);
            if (ExistDefect) {
                throw new FisException("BOR005", new string[] { });
            }
            return base.DoExecute(executionContext);
        }

    }
}

