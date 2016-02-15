// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录MB过站log
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   YangWeihua                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 记录MB过站log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB为主线对象的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Property设定创建MBLog对象;
    ///         2.保存MBLog对象;
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         insert PCBLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class WriteMBLog : BaseActivity
    {

        ///<summary>
        /// 过站结果
        ///</summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(bool), typeof(WriteMBLog));

        ///<summary>
        /// 过站结果
        ///</summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsPass
        {
            get
            {
                return ((bool)(base.GetValue(WriteMBLog.IsPassProperty)));
            }
            set
            {
                base.SetValue(WriteMBLog.IsPassProperty, value);
            }
        }

        ///<summary>
        /// 单条还是成批插入，如果成批从Session.SessionKeys.MBList中取出所有要插入的MB
        ///</summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(WriteMBLog));

        /// <summary>
        /// 单条还是成批插入，如果成批从Session.SessionKeys.MBList中取出所有要插入的MB
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(WriteMBLog.IsSingleProperty)));
            }
            set
            {
                base.SetValue(WriteMBLog.IsSingleProperty, value);
            }
        }

        /// <summary>
        /// 更新为指定站 SessionKey
        /// </summary>
        public static DependencyProperty ManualStationProperty = DependencyProperty.Register("ManualStation", typeof(string), typeof(WriteMBLog));

        ///<summary>
        /// 更新为指定站 SessionKey
        ///</summary>
        [DescriptionAttribute("ManualStation")]
        [CategoryAttribute("ManualStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ManualStation
        {
            get
            {
                return ((string)(base.GetValue(WriteMBLog.ManualStationProperty)));
            }
            set
            {
                base.SetValue(WriteMBLog.ManualStationProperty, value);
            }
        }

        /// <summary>
        /// 更新为指定站
        /// </summary>
        public static DependencyProperty LineSuffixProperty = DependencyProperty.Register("LineSuffix", typeof(string), typeof(WriteMBLog));

        ///<summary>
        /// 更新为指定站 string or blank
        ///</summary>
        [DescriptionAttribute("LineSuffix")]
        [CategoryAttribute("LineSuffix Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string LineSuffix
        {
            get
            {
                return ((string)(base.GetValue(WriteMBLog.LineSuffixProperty)));
            }
            set
            {
                base.SetValue(WriteMBLog.LineSuffixProperty, value);
            }
        }
        ///<summary>
        ///</summary>
        public WriteMBLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// write MB LOG
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string station = this.Station;
            if (!string.IsNullOrEmpty(ManualStation))
            {
                string mStation = CurrentSession.GetValue(ManualStation) as string;
                if (mStation != null)
                {
                    station = mStation;
                }
            }
            int status = this.IsPass ? 1 : 0;
            if (station == "CL")
            {
                status = 2;
            }
            string line = default(string);
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            if (IsSingle)
            {
                var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
                if (mb == null)
                {
                    throw new NullReferenceException("MB in session is null");
                }

                line = string.IsNullOrEmpty(this.Line) ? mb.MBStatus.Line : this.Line;
                if (!string.IsNullOrEmpty(this.LineSuffix))
                {
                    line = line + LineSuffix;
                }
                var mbLog = new MBLog(
                    0,
                    mb.Sn,
                    mb.Model,
                    station,
                    status,
                    line,
                    this.Editor,
                    DateTime.Now);

                mb.AddLog(mbLog);
                mbRepository.Update(mb, CurrentSession.UnitOfWork);
                CurrentSession.AddValue(Session.SessionKeys.MBLog, mbLog);
            }
            else
            {
                var mbList = CurrentSession.GetValue(Session.SessionKeys.MBList) as IList<IMB>;
                if (mbList == null)
                {
                    throw new NullReferenceException("MBList in session is null");
                }
                foreach (var mb in mbList)
                {
                    line = string.IsNullOrEmpty(this.Line) ? mb.MBStatus.Line : this.Line;
                    var mbLog = new MBLog(
                        0,
                        mb.Sn,
                        mb.Model,
                        station,
                        status,
                        line,
                        this.Editor,
                        DateTime.Now);

                    mb.AddLog(mbLog);
                    mbRepository.Update(mb, CurrentSession.UnitOfWork);
                }

            }
            return base.DoExecute(executionContext);
        }

    }
}
