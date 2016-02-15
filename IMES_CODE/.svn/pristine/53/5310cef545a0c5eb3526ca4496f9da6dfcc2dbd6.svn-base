// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查Connector使用次数是否超过600次
//                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-04   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.HDDCopyInfo;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Activity
{

    /// <summary>
    /// 检查Connector使用次数是否超过600次
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于HDD Test
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK018
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ConnectNo
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
    ///         IHDDCopyInfoRepository
    /// </para> 
    /// </remarks>
    public partial class CheckConnector : BaseActivity
    {
        public CheckConnector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Connector使用次数是否超过600次
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string currentConnectNo = (string)CurrentSession.GetValue(Session.SessionKeys.ConnectNo);

            IHDDCopyInfoRepository currentHDDRepository = RepositoryFactory.GetInstance().GetRepository<IHDDCopyInfoRepository, HDDCopyInfo>();

            int ConnectorUseTimes = currentHDDRepository.GetCountByConnectorNo(currentConnectNo) ;
            if (ConnectorUseTimes >= ConnectorMAXUseTimes)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentConnectNo);
                ex = new FisException("CHK018", erpara);
                throw ex;
            }

            return base.DoExecute(executionContext);
        }

        private const int ConnectorMAXUseTimes = 600;
    }
}
