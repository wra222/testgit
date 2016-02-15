// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
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

using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
    ///           其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    public partial class ClearProductCOAID_OOAID : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClearProductCOAID_OOAID()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
        /// 其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            /*
             * 4．点击“Save”	
             * 	针对List中的每一COA No
             * 	5．获取与此COA No绑定的Product的ID、CUSTSN,Model对应的HP Pno
             * 	6．解除Product与COA No的绑定：
             *          delete Product_Part where PartSn=@coano and ProductID=@productID
             *     设置对应Product的当前状态：
             *          Update ProductStatus set Station=’69’, Editor=@user, Udt=GetDate where ProducntID=@productID
             *     设置COA状态：
             *          Update COAStatus set Line=’REM’，Status=’A2’，Editor=@user, Udt=GetDate where COASN=@coano
             *     记录COA Log：
             *          insert COALog values (@coano, 'A2',custsn+'/'+hppno, rtrim(@user), GetDate, ‘COA’)
			 */



            return base.DoExecute(executionContext);
        }

    }
}

