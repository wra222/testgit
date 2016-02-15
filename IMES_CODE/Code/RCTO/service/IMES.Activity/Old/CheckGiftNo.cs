// INVENTEC corporation (c)2009 all rights reserved. 
// Description:检查输入的GiftNo是否存在于数据库中
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-10   Lucy Liu                 create
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
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.FRU;

namespace IMES.Activity
{

    /// <summary>
    /// 检查输入的GiftNo是否存在于数据库中 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FRU Gift Label Reprint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;this.Key中保存着输入的GiftNo,调用Repository获取该GiftNo是否存在于数据库中
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK004
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.key
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
    public partial class CheckGiftNo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckGiftNo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// this.Key中保存着输入的GiftNo,调用Repository获取该GiftNo是否存在于数据库中
        /// (This is not valid gift no)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            
            //this.Key中保存着输入的GiftNo,调用Repository获取
            //select count(*) from FRUGift where ID=''
            IGiftRepository giftRep = RepositoryFactory.GetInstance().GetRepository<IGiftRepository, FRUGift>();
            int result = giftRep.GetCountOfFRUGift(this.Key);
           
            if (result == 0)
            {
                //model对象获取为空,抛异常
                var ex = new FisException("PAK004", new string[] { this.Key });
                throw ex;
            }
        
            return base.DoExecute(executionContext);
        }

   
    }
}


