// INVENTEC corporation (c)201all rights reserved. 
// Description:  执行不需要用户输入的CheckItem检查
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-06   YangWeihua              create
using System;
using System.Collections.Generic;
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
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;


namespace IMES.Activity
{
    /// <summary>
    /// 根据CheckItem, CheckItemSetting的设定，执行不需要用户输入的CheckItem检查
    /// 对于PCAShippingLabel站, 需要的check:
    ///         a.	是否绑定了Mo            由CheckItem定义
    ///         b.	是否存在重复的MB SNo    由表结构的Unique约束保证,无需检查
    ///         c.	是否存在重复的MAC       由表结构的Unique约束保证,无需检查
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于各个检料站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取CheckItem列表, 保存至Session.ImplicitCheckItemList;
    ///         2.针对每个CheckItem依次执行CheckItem.Check
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                指定要检查的属性不存在;
    ///                指定要检查的属性值与预设值不符
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         Session.ImplicitCheckItemList
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
    ///         IMB
    ///         CheckItem
    /// </para> 
    /// </remarks>
	public partial class CheckImplicitCheckItem: BaseActivity
	{

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var objectToCheck = GetCheckObject();

            if (objectToCheck != null)
            {
                var checkItems = objectToCheck.GetImplicitCheckItem(Customer, Station);
                if (checkItems != null)
                {
                    foreach (var item in checkItems)
                    {
                        if (item.CheckItem.Equals("HDVD") && item.CheckType == CheckType.ExistCheck)
                        {
                            //对HDVD检查的特殊处理；DVD(且Descr是 like '%HDVD%’的)
                            if (((IProduct)objectToCheck).ContainHDVD())
                            {
                                item.ImplicitCheck(objectToCheck);
                            }
                        }
                        else
                        {
                            item.ImplicitCheck(objectToCheck);
                        }
                    }
                }
            }

            return base.DoExecute(executionContext);
        }
	}
}
