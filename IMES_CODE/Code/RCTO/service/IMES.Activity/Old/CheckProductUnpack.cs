
// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 判断当前Product否符合UnPack条件 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-10   Lucy Liu                 create
// 2010-05-05  Lucy Liu(EB12)            Modify:   ITC-1155-0060
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

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product否符合UnPack条件  
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FG Shipping Label(TRO) Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;Product对象里面有DeliveryID和CartonSN但没有PalletID才可以UnPack，否则报错
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK001
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckProductUnpack : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductUnpack()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Session.Product获取Product对象
        /// Product对象里面有DeliveryID和CartonSN但没有PalletID才可以UnPack，否则报错       
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            if (((currentProduct.DeliveryNo != null) && (currentProduct.DeliveryNo.Trim() != string.Empty)) &&
                ((currentProduct.CartonSN != null) && (currentProduct.CartonSN.Trim() != string.Empty)) &&
                ((currentProduct.PalletNo == null) || (currentProduct.PalletNo.Trim() == string.Empty)))
            {
                //可以Unpack,保存DeliveryNo,为后续的GetDelivery Activity使用
                CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
            } else if ((currentProduct.DeliveryNo == null) || (currentProduct.DeliveryNo.Trim() == string.Empty)) {
                 throw new FisException("CHK130", new string[] { currentProduct.ProId }); 
            } else if ((currentProduct.CartonSN == null) || (currentProduct.CartonSN.Trim() == string.Empty)) {
                 throw new FisException("CHK101", new string[] { currentProduct.ProId }); 
            } else {
                
                //<bug>
                //    BUG NO:ITC-1155-0060
                //    REASON:修改错误描述
                //</bug>
                //不能Unpack,报错
                throw new FisException("PAK001", new string[] { currentProduct.ProId }); 
            }



            return base.DoExecute(executionContext);
        }

      


    }
}


