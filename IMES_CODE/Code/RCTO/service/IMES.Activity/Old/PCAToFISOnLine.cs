/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */



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
using IMES.FisObject.Common.Misc;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 调用TransferIMESPCAToFISOnLine sp上传数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于30及QF站
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///        
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///          
    /// </para> 
    /// </remarks>
    public partial class PCAToFISOnLine : BaseActivity
    {
        public PCAToFISOnLine()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (this.Station == "30" || this.Station == "QF")
            {
                IMiscRepository currentMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                string mbsn = ((IMB)CurrentSession.GetValue(Session.SessionKeys.MB)).Sn;

                currentMiscRepository.TransferIMESPCAToFISOnLine(mbsn, this.Station);
            }
            return base.DoExecute(executionContext);
        }
    }
}

