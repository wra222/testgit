// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 判断当前CartonNo否符合UnPack条件
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-10   Lucy Liu                     create
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Activity
{
    /// <summary>
    /// 判断当前CartonNo否符合UnPack条件  
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///			070UnpackCarton.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          检查Product中属于该Carton的机器是否有绑定PALLET，若绑定了Pallet不允许UnPack
    ///     
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
	///			???: 该Product 已绑定Pallet，不能被unpack!!									
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
	///         Session.SN
	///         Session.CartonNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.DeliveryNo
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
    
    public partial class CheckCartonUnpack: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCartonUnpack()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据Session.Product获取Product对象
        /// 
        /// 检查Product中属于该Carton的机器是否有绑定PALLET，若绑定了Pallet不允许UnPack
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            //if (currentProduct.CartonSN == "" || currentProduct.CartonSN == null)
            //{
            //    throw new FisException("CHK101", new string[] { currentProduct.ProId });
            //}
            //else 
            if (!(currentProduct.PalletNo == "" || currentProduct.PalletNo == null))
            {
                throw new FisException("PAK001", new string[] { currentProduct.ProId });
                //throw new FisException("???", new string[] { currentProduct.CartonSN });
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
            }

            return base.DoExecute(executionContext);
        }

	}
}
