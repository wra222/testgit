// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 生成SMT MO数据
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   Yuan XiaoWei                 create
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
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 生成SMT MO数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于SMT MO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.创建MBMO对象, 插入DB;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///     Session.PN111
    ///     Session.IsMassProduction
    ///     Session.Qty
    ///     Session.Remark
    ///     Session.SMTMONO
    /// </para> 
    ///<para> 
    /// 输出：
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         新增至SMTMO
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBMORepository
    ///         MBMO
    /// </para> 
    /// </remarks>
    public partial class GenerateSMTMO : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GenerateSMTMO()
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
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.PN111);
            //bool IsMassProduction = (bool)CurrentSession.GetValue(Session.SessionKeys.IsMassProduction);
            int Qty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
            string Remark = (string)CurrentSession.GetValue(Session.SessionKeys.Remark);
            string SMTMONO = (string)CurrentSession.GetValue(Session.SessionKeys.MBMONO );

            IPartRepository currentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart currentPart = currentPartRepository.Find(model);

            IMBMO currentMBMO = new MBMO();
            currentMBMO.Editor = Editor;
            
            currentMBMO.Family = currentPart.Descr;
            currentMBMO.Model = model;
            currentMBMO.MONo = SMTMONO;
            currentMBMO.PrintedQty = 0;
            currentMBMO.Qty = Qty;
            currentMBMO.Remark = Remark;
            currentMBMO.Status = "H";

            //约定由Repository来更新时间
            //currentMBMO.Cdt = DateTime.Now;
            //currentMBMO.Udt = currentMBMO.Cdt;

            IMBMORepository currentMBMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            currentMBMORepository.Add(currentMBMO, CurrentSession.UnitOfWork);


            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "SMTMO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, SMTMONO);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, SMTMONO);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, model);
            
            return base.DoExecute(executionContext);
        }
    }
}
