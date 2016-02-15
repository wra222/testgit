// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块的删除DN功能中的删除HP EDI部分子功能
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

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块的删除DN功能中的删除PAK EDI部分子功能
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Delete DN for OB user
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         按照DN删除PAK EDI DB中的相关数据
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.DeliveryNo
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
    public partial class DeletePAKEDIDNData : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeletePAKEDIDNData()
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
            IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            
            if (null == CurrentSession.GetValue("IsBatch") || true != (bool)CurrentSession.GetValue("IsBatch"))
            {
                string dn = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                currentRepository.DeletePoDataEdiDefered(CurrentSession.UnitOfWork, dn);
                currentRepository.DeletePoPltEdiDefered(CurrentSession.UnitOfWork, dn);
            }
            else
            {
                IList<string> dnList = CurrentSession.GetValue(Session.SessionKeys.DeliveryNoList) as IList<string>;
                currentRepository.DeletePoDataEdiDefered(CurrentSession.UnitOfWork, dnList);
                currentRepository.DeletePoPltEdiDefered(CurrentSession.UnitOfWork, dnList);
            }

            return base.DoExecute(executionContext);
        }
    }
}
