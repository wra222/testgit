// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前ChepPallet是否Exists
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-03   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前ChepPallet是否Exists
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-PAK-UC Final Scan
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.如果使用IMES_PAK..ChepPallet.PLT = @ChepPalletNo 在IMES_PAK..ChepPalletPLT 表中查询不到记录，则报告错误：“Not exists this chep!”
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK010
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ChepPallet
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
    ///         IPalletRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckChepPallet : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckChepPallet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前ChepPallet是否Exists
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            string CurrentChepPallet = (string)CurrentSession.GetValue(Session.SessionKeys.ChepPallet);

            IPalletRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            ChepPalletInfo resultChepPallet = currentRepository.GetChepPalletInfo(CurrentChepPallet);
            if (resultChepPallet == null)
            {

                throw new FisException("PAK010", new string[] { CurrentChepPallet });

            }

            return base.DoExecute(executionContext);
        }

    }
}

