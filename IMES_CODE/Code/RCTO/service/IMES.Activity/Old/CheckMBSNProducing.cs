/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: check该MB是否投产
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 2009-01-08   207013     Modify: ITC-1103-0074 、ITC-1103-0011、ITC-1103-0233
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
//using IMES.Infrastructure.Repository.PCA;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// check该MB是否投产
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
    /// check该MB是否投产
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
    public partial class CheckMBSNProducing : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBSNProducing()
        {
            InitializeComponent();
        }

        /// <summary>
        /// check该MB是否投产
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IList<string> mbsnlist = (List<string>)CurrentSession.GetValue(Session.SessionKeys.MBNOList);
            IList<string> retmbsnlist = mbRepository.FilterMBNOList(mbsnlist, "P0");
            string producedMb = "";
            if (retmbsnlist != null && retmbsnlist.Count > 0)
            {
                foreach (string mbsn in retmbsnlist)
                {
                    if (string.IsNullOrEmpty(producedMb))
                    {
                        producedMb = mbsn;
                    }
                    else
                    {
                        producedMb = producedMb + "," + mbsn;
                    }
                }

                    erpara.Add(producedMb);
                    ex = new FisException("CHK022", erpara);
                    throw ex;
           
            }

            //var mbsn = CurrentSession.GetValue(Session.SessionKeys.MBSN).ToString();
            //IMB mbobject = mbRepository.Find(mbsn);
            //if (mbobject == null)
            //{
            //    erpara.Add(mbsn);
            //    ex = new FisException("SFC001", erpara);
            //    throw ex;
            //}
            //else
            //{
            //    string mbStation = mbobject.MBStatus.Station;
            //    //检查该MB SNo 在PCBStatus 中的记录是否是Pass Print MB Label by Mo 站，如果不是则可以说明该MB Sno 已经投入生产了
            //    //ITC-1103-0074 、ITC-1103-0011、ITC-1103-0145
            //    if ("P0" != mbStation)
            //    {
            //        erpara.Add(mbsn);
            //        ex = new FisException("CHK022", erpara);
            //        throw ex;
            //    }
            //}
            return base.DoExecute(executionContext);
        }
    }
}
