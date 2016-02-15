/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: dismantle MB SNO
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
//using IMES.Infrastructure.Repository.PCA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PCA.MBMO;
//using IMES.Infrastructure.Repository.Common;
using System.Data.Common;
using IMES.FisObject.Common.MO;
namespace IMES.Activity
{
    /// <summary>
    /// dismantle MB SNO
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
    /// 更新及删除数据
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MBNOList
    ///         Session.MBMO
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
    /// Update SMTMO
    /// Insert MoDismantleLog
    /// 删除PCB，更新PCBStatus，Insert PCB Log
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///      IMBMORepository 
    ///      IMBRepository
    ///      IMODismantleLogRepository
    /// </para> 
    /// </remarks>
    public partial class DismantleMBSNMO : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DismantleMBSNMO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update SMTMO
        /// Insert MoDismantleLog
        /// 删除PCB，更新PCBStatus，Insert PCB Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //FisException ex;
            //List<string> erpara = new List<string>();
            
            IList<string> mbsnlist = (List<string>)CurrentSession.GetValue(Session.SessionKeys.MBNOList);
            if (mbsnlist != null && mbsnlist.Count > 0)
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

                ////	Update SMTMO
                IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                IMBMO mbmoObject = (IMBMO)CurrentSession.GetValue(Session.SessionKeys.MBMO);
                mbmoObject.PrintedQty = mbmoObject.PrintedQty - mbsnlist.Count;
                CurrentSession.AddValue(Session.SessionKeys.MBMO, mbmoObject);
                mbmoRepository.Update(mbmoObject, CurrentSession.UnitOfWork);
                //	Insert MoDismantleLog
                //	Delete PCB/PCBStatus
                var disReason = CurrentSession.GetValue(Session.SessionKeys.Reason).ToString();               
                IList<MODismantleLog> modislogs = new List<MODismantleLog>();
                IList<IMB> mbs = new List<IMB>();
                IList<MBStatus> mbstatuslist=new List<MBStatus>();
                IList<MBLog> mbloglist = new List<MBLog>();
                foreach (string mbsno in mbsnlist)
                {
                    IMB mbobject = mbRepository.Find(mbsno);
                    MODismantleLog modisObject = new MODismantleLog(mbsno, "SMTMo", mbobject.SMTMO, disReason, this.Editor, DateTime.Now, 0);
                    MBStatus mbstatus = new MBStatus(mbsno, this.Station, MBStatusEnum.Pass, this.Editor, this.Line, DateTime.Now, DateTime.Now);
                    MBLog mblog = new MBLog(0, mbsno, mbobject.Model, this.Station, 1, this.Line, this.Editor, DateTime.Now);

                    modislogs.Add(modisObject);
                    mbs.Add(mbobject);
                    mbstatuslist.Add(mbstatus);
                    mbloglist.Add(mblog);
                }
                IMODismantleLogRepository modismRepository = RepositoryFactory.GetInstance().GetRepository<IMODismantleLogRepository, MODismantleLog>();
                modismRepository.AddBatchDefered(CurrentSession.UnitOfWork, modislogs);
                mbRepository.RemoveBatchDefered(CurrentSession.UnitOfWork, mbs);
                //<Gao Yongbo> 修改为只删除PCB，更新PCBStatus，Insert PCB Log
                mbRepository.UpdateMBStatusBatchDefered(CurrentSession.UnitOfWork, mbstatuslist);
                mbRepository.AddMBLogBatchDefered(CurrentSession.UnitOfWork, mbloglist);
            }

                     
            # region "old"
            //  //   针对[Start MB SNo]，[End MB SNo] 范围内的每一个MB SNo，都要进行如下操作：
          //  //	Update SMTMO
          //  //PrintQty  ITC-1103-0148     

          //  IMBMO mbmoObject = (IMBMO)CurrentSession.GetValue(Session.SessionKeys.MBMO);
          //  mbmoObject.PrintedQty = mbmoObject.PrintedQty - 1;
          //  CurrentSession.AddValue(Session.SessionKeys.MBMO, mbmoObject);
          //  mbmoRepository.Update(mbmoObject, CurrentSession.UnitOfWork);
          //  //	Insert MoDismantleLog
          //  //参考方法：
          //  //INSERT MoDismantleLog SELECT PCBNo,'SMTMo',SMTMO,@reason,@Editor,GETDATE() FROM PCB (nolock) WHERE PCBNo = @mbsno

          //  IMODismantleLogRepository modismRepository = RepositoryFactory.GetInstance().GetRepository<IMODismantleLogRepository, MODismantleLog>();
          //  //(string pcbNo, string tp, string smtMo, string reason, string editor, DateTime cdt, int id)
          //var disReason=CurrentSession .GetValue(Session.SessionKeys.Reason).ToString();
          //  MODismantleLog modisObject = new MODismantleLog(mbSn, "SMTMo",mbobject.SMTMO,disReason,this.Editor,DateTime.Now,0);
          //  modismRepository.Add(modisObject, CurrentSession.UnitOfWork);
          //  //	Delete PCB/PCBStatus
          //  //由于投产的MB 不允许进行Dismantle，因此，没有必要清理PCB_Parts            
          //  mbRepository.Remove(mbobject, CurrentSession.UnitOfWork);
# endregion
            return base.DoExecute(executionContext);
        }
    }
}
