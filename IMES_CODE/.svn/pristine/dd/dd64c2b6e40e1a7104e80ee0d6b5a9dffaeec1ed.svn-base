/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: CheckPalletLWH
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-05-26   207013     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
//using IMES.Infrastructure;
using System.Data.Common;

namespace IMES.Activity
{
    /// <summary>
    /// CheckPalletLWH
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// check length,width,height 
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet
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
    /// 无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// 
    /// </para> 
    /// </remarks>
    public partial class CheckPalletLWH : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPalletLWH()
        {
            InitializeComponent();
        }

        /// <summary>
        /// get pallet model
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);

            if (CurrentPallet.Length.Equals(0) || CurrentPallet.Width.Equals(0) || CurrentPallet.Height.Equals(0))
            {
                List<string> errpara = new List<string>();
                errpara.Add(CurrentPallet.PalletNo);
                throw new FisException("CHK133", errpara);
            }
               
            return base.DoExecute(executionContext);
        }
    }
}
