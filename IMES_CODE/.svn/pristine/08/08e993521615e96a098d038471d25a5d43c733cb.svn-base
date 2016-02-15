/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 2010-01-12   207006            ITC-1103-0093
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

//check:
//
//a.pcb log??
//b.如果MB 已经投入到FA 生产，则报告错误：“此MB 已经投入到FA 生产，不能Reprint!!“
//

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
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 检查ICT是否满足Reprint条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ICT Reprint
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
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class ICTReprintCheck : BaseActivity
    {
        public ICTReprintCheck()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (ProductRepository.IfBindPCB(mb.Sn))
            {
                List<string> erpara = new List<string>();
                //erpara.Add(mb.Sn);
                FisException ex = new FisException("CHK013", erpara);
                ex.stopWF = true;
                throw ex;
            }

            // 2010-01-12   207006     ITC-1103-0093
            if (string.IsNullOrEmpty(mb.MAC))
            {
                List<string> erpara = new List<string>();
                //erpara.Add(mb.Sn);
                FisException ex = new FisException("CHK033", erpara);
                ex.stopWF = true;
                throw ex;
            }

            if ((mb.MBStatus.Station.ToUpper () == "CL")&&(mb.Sn.Substring(5,1) == "0"))
            {
                List<string> erpara = new List<string>();
                //erpara.Add(mb.Sn);
                FisException ex = new FisException("CHK035", erpara);
                ex.stopWF = true;
                throw ex;
            }
            return base.DoExecute(executionContext);
        }
    }
}
