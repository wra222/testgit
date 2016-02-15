// INVENTEC corporation (c)2011 all rights reserved. 
// Description: insert PartSn
//
// Update:
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-03-04   Li.Ming-Jun                  create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Insert PartSn
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PartSn表写入数据 -- CI-MES12-SPEC-FA-UC IEC Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         详见UC
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PrintLogDescr
    ///         Session.PrintLogBegNo
    ///         Session.PrintLogEndNo
    ///         Session.PrintLogName
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///        无 
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         insert PartSn
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPartSnRepository
    /// </para> 
    /// </remarks>
    public partial class WritePartSnForIECLabel : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public WritePartSnForIECLabel()
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
            IList vendorCTList = new ArrayList();
            vendorCTList = (IList)CurrentSession.GetValue(Session.SessionKeys.VCodeInfoLst);

            string iecPn = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);
            string partType = "DDR";
            string dateCode = (string)CurrentSession.GetValue(Session.SessionKeys.DCode);
            string vendorDCode = null;
            string vCode = null;
            string pn151 = null;
            string editor = Editor;

            var repository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            foreach (string vendorCT in vendorCTList)
            {
                PartSn item = new PartSn(vendorCT, iecPn, partType, vendorCT, vendorDCode, vCode, pn151, editor, dateCode);
                repository.Add(item, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }
}
