/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: check if start mbsnno and end mbsnno has the same mo
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// check 起止MB是否同一个MO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// 比较起止MB的MO
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MBNOList
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
    ///      IMBRepository   
    /// </para> 
    /// </remarks>
    public partial class CheckMBSNsMO : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBSNsMO()
        {
            InitializeComponent();
        }
        /// <summary>
        /// check 起止MB是否同一个MO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            var startMBSn = (string)CurrentSession.GetValue(Session.SessionKeys.startProdId);
            var endMBSn = (string)CurrentSession.GetValue(Session.SessionKeys.endProdId);
            IMB startMBobject = mbRepository.Find(startMBSn);
            IMB endMBobject = mbRepository.Find(endMBSn);
            //如果用户输入的[End MB SNo]的Mo与[Start MB SNo] 的Mo 不同，则报告错误：“Start MB SNo 与 End MB SNo 的Mo 不同！”
            if (startMBobject == null)
            {
                erpara.Add(startMBSn);
                ex = new FisException("SFC001", erpara);
                throw ex;
            }

            if(endMBobject == null)
            {
                erpara.Add(endMBSn);
                ex = new FisException("SFC001", erpara);
                throw ex;
            }

            if (startMBobject.SMTMO != endMBobject.SMTMO)
            {
                erpara.Add(startMBSn);
                erpara.Add(endMBSn);
                ex = new FisException("CHK023", erpara);
                throw ex;
            }

            else
            {
                Session sessionInfo = SessionManager.GetInstance.GetSession(this.Key, Session.SessionType.Common);
                //ITC-1103-0148，add  GetMO activity,set mo value for it
                sessionInfo.AddValue(Session.SessionKeys.MBMONO, startMBobject.SMTMO);
                sessionInfo.AddValue(Session.SessionKeys.LoopCount, 0);
                //获取范围内的同一个mo下的mbsn序列，初始化session值
                sessionInfo.AddValue(Session.SessionKeys.LoopCount, 0);
                IList<IMB> mbobjlist = mbRepository.GetMBBySectionAndMO(startMBSn, endMBSn, startMBobject.SMTMO);
               IList<string> mbsnlist = new List<string>();
               if ((mbobjlist != null) && (mbobjlist.Count > 0))
               {
                   sessionInfo.AddValue(Session.SessionKeys.Qty, mbobjlist.Count);
                   foreach (IMB mbobj in mbobjlist)
                   {
                       mbsnlist.Add(mbobj.Sn);
                   }                   
               }
               else
               {
                   sessionInfo.AddValue(Session.SessionKeys.Qty,0);
               }
               sessionInfo.AddValue(Session.SessionKeys.MBNOList, mbsnlist);
            }
            return base.DoExecute(executionContext);
        }
    }
}
