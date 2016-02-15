// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于Pallet称重检查Pallet是否和SN绑定
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   Yuan XiaoWei                 create
// Known issues:
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
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{   
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckPalletSNBind : BaseActivity
    {

        /// <summary>
        /// 用于Pallet称重检查Pallet是否和SN绑定
        /// </summary>
        /// <remarks>
        /// <para>
        /// 基类：
        ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
        /// </para>
        /// <para>
        /// 应用场景：
        ///      应用于PalletWeight
        /// </para>
        /// <para>
        /// 实现逻辑：
        ///         1.从session中获取Pallet，SN
        ///         2.根据SN检查Product表中的PalletNo是否和session中的PalletNo一致
        ///</para> 
        /// <para> 
        /// 异常类型：
        ///         1.系统异常：
        ///         2.业务异常：
        /// 
        /// </para> 
        /// <para>    
        /// 输入：
        ///         Session.SN
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
        ///         
        /// </para> 
        ///<para> 
        /// 相关FisObject:
        /// </para> 
        /// </remarks>
        public CheckPalletSNBind()
        {
            //InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            string pltNo = ((Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet)).PalletNo;

            if (currentProduct.PalletNo != pltNo)
            {
                List<string> errpara = new List<string>();
                errpara.Add(pltNo);
                errpara.Add(currentProduct.CUSTSN);

                throw new FisException("CHK006", errpara);
            }
            return base.DoExecute(executionContext);
        }
    }
}
