/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CI-MES12-SPEC-PAK-UC PA Cosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei           Create 
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
    /// 获取Alarm数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于FA各需要alarm站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.调用sp获得数据;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         Session.SessionKeys.MB 
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
    public partial class Alarm : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum AlarmType
        {
            /// <summary>
            /// 
            /// </summary>
            MB,
            /// <summary>
            /// 
            /// </summary>
            PrdId
        }

        /// <summary>
        /// alarm类型
        /// </summary>
        public static DependencyProperty alarmTypeProperty = DependencyProperty.Register("alarmType", typeof(AlarmType), typeof(Alarm));
        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("alarmType")]
        [CategoryAttribute("alarmType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public AlarmType alarmType
        {
            get
            {
                return ((AlarmType)(base.GetValue(Alarm.alarmTypeProperty)));
            }
            set
            {
                base.SetValue(Alarm.alarmTypeProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Alarm()
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
            IMiscRepository currentMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            var defect = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);

            if (this.alarmType == AlarmType.MB)
            {
                var currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);

                if (null == defect)
                {
                    //currentMiscRepository.GetAlarmInfo(currentMB.Sn, currentMB.Family, currentMB.Model, this.Station, this.Line, string.Empty);
                currentMiscRepository.GetAlarmInfoDefered(CurrentSession.UnitOfWork ,currentMB.Sn, currentMB.Family, currentMB.Model, this.Station, this.Line, string.Empty);
                }
                else
                {
                    currentMiscRepository.GetAlarmInfoBatchDefered(CurrentSession.UnitOfWork ,currentMB.Sn, currentMB.Family, currentMB.Model, this.Station, this.Line, defect.ToArray<string>());
                }
            }
            else
            {
                var prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (null == defect)
                {

                    currentMiscRepository.GetAlarmInfoDefered(CurrentSession.UnitOfWork ,prod.ProId, prod.Family, prod.Model, this.Station, this.Line, string.Empty);
                }
                else
                {
                    currentMiscRepository.GetAlarmInfoBatchDefered(CurrentSession.UnitOfWork ,prod.ProId, prod.Family, prod.Model, this.Station, this.Line, defect.ToArray<string>());
                }
            }


            return base.DoExecute(executionContext);
        }
    }
}
