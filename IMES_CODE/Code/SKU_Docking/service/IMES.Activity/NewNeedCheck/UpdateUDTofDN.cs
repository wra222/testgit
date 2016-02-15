// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块上传Po Data时更新已存在DN的UDT
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块上传Po Data时更新已存在DN的UDT
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Upload DN for 173 shipment 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         按照DN更新UDT
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         UpdateUDTDNList
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
    ///              IDeliveryRepository
    /// </para> 
    /// </remarks>
    public partial class UpdateUDTofDN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateUDTofDN()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IList<string> dnList = CurrentSession.GetValue("UpdateUDTDNList") as IList<string>;

            if (dnList != null && dnList.Count != 0)
            {
                IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                foreach (string dn in dnList)
                {
                    currentDNRepository.UpdateDNUdtDefered(CurrentSession.UnitOfWork, dn);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
