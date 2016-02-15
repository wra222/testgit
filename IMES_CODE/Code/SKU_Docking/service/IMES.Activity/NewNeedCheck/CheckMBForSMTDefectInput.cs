/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Check MB for SMT Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI SMT Defect Input.docx –2012/05/21
 * UC:CI-MES12-SPEC-SA-UC SMT Defect Input.docx –2012/05/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-21  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
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
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// MB check used in SMT defect input page.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于 SMT defect input
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// 	若PCB/PCBStatus不存在MBSno，则报错：“MBSN：XXX不存在”
    /// 	检查是否已经投线，若投线，则报错：“该MB已投线”
    /// 参考方法：
    /// select top 1 Status from PCBLog where PCBNo=@MBSno and Station='10' and Status='1' order by Cdt
    /// 	如果PCBStatus.Station=‘CL’，则报错：“该MB已经切分，不能再使用”；
    /// 	如果PCBStatus.Status=0，则报错：“MBSN:XXX需去修护”
    /// 	如果PCBTestLog存在PCBNo=#MBSN and Station=’08’ and Status=’0’的记录，则报错：“MBSN：XXX已经在该站判了不良”
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
    ///         Session.SessionKeys.MB 
    ///         
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
    /// </para> 
    /// </remarks>
    public partial class CheckMBForSMTDefectInput : BaseActivity
    {
        /// <summary>
        /// CheckMBForSMTDefectInput
        /// </summary>
        public CheckMBForSMTDefectInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// MB check used in SMT defect input page.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbSNo = this.Key;

            List<string> errpara = new List<string>();
            errpara.Add(mbSNo);

            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            //若PCB/PCBStatus不存在MBSno，则报错：“MBSN：XXX不存在”
            IMB mb = mbRepository.Find(mbSNo);
            if (mb == null || mb.MBStatus == null)
            {
                throw new FisException("CHK300", errpara);
            }

            //检查是否已经投线，若投线，则报错：“该MB已投线”
            IList<MBLog> mbLogList = mbRepository.GetMBLog(mbSNo, "10", 1);
            if (mbLogList.Count > 0)
            {
                throw new FisException("CHK301", errpara);
            }

            //如果PCBStatus.Station=‘CL’，则报错：“该MB已经切分，不能再使用”
            if (mb.MBStatus.Station == "CL")
            {
                throw new FisException("CHK302", errpara);
            }

            //如果PCBStatus.Status=0，则报错：“MBSN:XXX需去修护”
            if (mb.MBStatus.Status == 0)  //0 - Fail
            {
                throw new FisException("CHK303", errpara);
            }

            //如果PCBTestLog存在PCBNo=#MBSN and Station=’08’ and Status=’0’的记录，则报错：“MBSN：XXX已经在该站判了不良”
            IList<TestLog> pcbTestLogList = mbRepository.GetPCBTestLogListFromPCBTestLog(mbSNo, 0, "08");
            if (pcbTestLogList.Count > 0)
            {
                throw new FisException("CHK304", errpara);
            }

            CurrentSession.AddValue(Session.SessionKeys.MB, mb);

            return base.DoExecute(executionContext);
        }

    }
}
