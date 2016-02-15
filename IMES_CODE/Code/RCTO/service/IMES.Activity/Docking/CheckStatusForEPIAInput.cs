// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Change To PIA EPIA 特殊检查
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-21   Chen Peng                    create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{
    /// <summary>
    /// TestEpiaForDocking 特殊检查
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change To PIA EPIA
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.已经正在EPIA的机器不能再change To EPIA
    ///         2.已经正在PIA的机器不能再Change To PIA
    ///         3.只有以下站可进入(65 66 67 69 6A 70 71 73 74 75 79)
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         Session.SessionKeys.HasDefect
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
    ///        无 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProduct
    ///              
    /// </para> 
    /// </remarks>
    public partial class CheckStatusForEPIAInput : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckStatusForEPIAInput()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
             var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

             List<string> errpara = new List<string>();
             ProductStatus Status	= currentProduct.Status;

             if ( Status.Status == 0)
             {
                 //errpara.Add("请先维修该机器");
                 errpara.Add(currentProduct.ProId);
                 throw new FisException("CHK870", errpara);
             }

            return base.DoExecute(executionContext);
        }

    }
}
