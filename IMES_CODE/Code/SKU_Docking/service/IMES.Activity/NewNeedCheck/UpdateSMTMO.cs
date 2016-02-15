/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:CI-MES12-SPEC-SA-UC MB Label Print.docx
 *             update SMTMO
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-03   zhu lei           Create 
 * 
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
using IMES.FisObject.PCA.MBMO ;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 更新SMTMO状态, 有两种类型的更新:
    ///     更新PrintedQty
    ///     更新Status
    /// 根据Activity Property的设定确定更新的内容
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         MB Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取MBMO对象;
    ///         2.根据Property设定更新MBMO对象;
    ///         3.保存对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SMTMONO
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
    ///         update SMTMO
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         MBMO
    ///         IMBMORepository
    /// </para> 
    /// </remarks>
    public partial class UpdateSMTMO : BaseActivity
    {
        private static Object _syncObj = new Object();

        /// <summary>
        /// 新的状态值, 值不为string.Empty时需要更新
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(UpdateSMTMO));
        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(UpdateSMTMO.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateSMTMO.StatusProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public UpdateSMTMO()
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
            //Replace MB case
            bool isReplaceMB = false;
            if (CurrentSession.GetValue(ExtendSession.SessionKeys.IsReplaceMB) != null)
            {
                isReplaceMB = (bool)CurrentSession.GetValue(ExtendSession.SessionKeys.IsReplaceMB);
            }

            if (isReplaceMB) return base.DoExecute(executionContext);

            var mbmo = (IMBMO)CurrentSession.GetValue(Session.SessionKeys.MBMO);

            if (this.Status != null && this.Status != "")
            {
                mbmo.Status = this.Status;
            }

            object Qty = CurrentSession.GetValue(Session.SessionKeys.Qty);

            int IncreasePrintedQty = 0;
            if (Qty != null)
            {
                IncreasePrintedQty = (int)(Qty);
            }
            else
            {
                IncreasePrintedQty = 0;
            }
            lock (_syncObj)
            {

                if (IncreasePrintedQty != 0)
                {
                    mbmo.PrintedQty = mbmo.PrintedQty + IncreasePrintedQty;
                }

                IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();

                mbmoRepository.Update(mbmo, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
    }
}

