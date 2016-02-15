/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: check 该MB是否做过本站
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
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// check 该MB是否做过本站
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
    /// 通过log中的站号，check 该MB是否做过本站
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///      IStationRepository   
    /// </para> 
    /// </remarks>
    public partial class CheckMBPrintlog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBPrintlog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// check 该MB是否做过本站，未做过本站则报错
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IList<MBLog> mblogs = currentMB.MBLogs;
            var properlog = (from p in mblogs
                    where p.StationID == this.Station 
                    select p).ToList();
            ////FRU
            //var proper2log = (from p in mblogs                             
            //                 where p.StationID == "8G"
            //                 select p).ToList();
            //未做过指定站则报错
            //if ((properlog == null || properlog.Count < 1) && (proper2log == null || proper2log.Count < 1))
            if (properlog == null || properlog.Count < 1)
            {
                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
                IStation curStation = stationRepository.Find(this.Station);
                erpara.Add(currentMB.Sn);
                erpara.Add(this.Station +"("+curStation.Descr+")");
                ex = new FisException("CHK057", erpara);
                throw ex;
            }           
            return base.DoExecute(executionContext);
        }
    }
}
