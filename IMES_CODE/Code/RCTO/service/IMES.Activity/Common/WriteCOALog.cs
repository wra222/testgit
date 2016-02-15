// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 记录到COALog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-11   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.FisObject.PAK.COA;

namespace IMES.Activity
{
    /// <summary>
    /// 用于记录COALog
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-COA Removal
    ///      CI-MES12-SPEC-PAK-CN Card Status Change
    ///      CI-MES12-SPEC-PAK-COA Status Change
    ///      CI-MES12-SPEC-PAK-UC Pizza Kitting
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取COASN，调用InsertCOALog方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.COASN For COAFrom.COASN
    ///         Session.COASNList For COAFrom.COASNList
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
    ///         更新COALog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              ICOAStatusRepository
    ///              COALog
    /// </para> 
    /// </remarks>
    public partial class WriteCOALog : BaseActivity
    {
        private static readonly ILog logger = LogManager.GetLogger("fisLog");

        ///<summary>
        /// 构造函数
        ///</summary>
        public WriteCOALog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tp
        /// </summary>
        public static DependencyProperty TpProperty = DependencyProperty.Register("Tp", typeof(string), typeof(WriteCOALog));

        /// <summary>
        /// Tp
        /// </summary>
        [DescriptionAttribute("Tp")]
        [CategoryAttribute("Tp Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Tp
        {
            get
            {
                return ((string)(base.GetValue(WriteCOALog.TpProperty)));
            }
            set
            {
                base.SetValue(WriteCOALog.TpProperty, value);
            }
        }

        /// <summary>
        /// COASNFrom
        /// </summary>
        public static DependencyProperty COASNFromProperty = DependencyProperty.Register("COASNFrom", typeof(COAFrom), typeof(WriteCOALog));

        /// <summary>
        /// COASNFrom,Session.SessionKeys.COASN or Session.SessionKeys.COASNList
        /// </summary>
        [DescriptionAttribute("COASNFrom")]
        [CategoryAttribute("COASNFrom Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public COAFrom COASNFrom
        {
            get
            {
                return ((COAFrom)(base.GetValue(WriteCOALog.COASNFromProperty)));
            }
            set
            {
                base.SetValue(WriteCOALog.COASNFromProperty, value);
            }
        }

        /// <summary>
        /// Wrint COALog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (Editor.Trim() == "")
                logger.Error("Editor from activity is empty!");

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            if (COASNFrom == COAFrom.COASN)
            {
                var coaSN = CurrentSession.GetValue(Session.SessionKeys.COASN) as string;
                if (coaSN == null)
                {
                    throw new NullReferenceException("COASN in session is null");
                }

                var newLog = new COALog
                {
                    COASN = coaSN,
                    Tp = Tp,
                    Editor = Editor,
                    LineID = Line,
                    StationID = Station,
                    Cdt = DateTime.Now
                };

                currentRepository.InsertCOALogDefered(CurrentSession.UnitOfWork, newLog);
            }
            else
            {
                var coaSNList = CurrentSession.GetValue(Session.SessionKeys.COASNList) as List<string>;
                if (coaSNList == null)
                {
                    throw new NullReferenceException("COASNList in session is null");
                }
                foreach (var coaSN in coaSNList)
                {
                    var newLog = new COALog
                    {
                        COASN = coaSN,
                        Tp = Tp,
                        Editor = Editor,
                        LineID = Line,
                        StationID = Station,
                        Cdt = DateTime.Now
                    };

                    currentRepository.InsertCOALogDefered(CurrentSession.UnitOfWork, newLog);
                }
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入的类型:COASN,COASNList
        /// </summary>
        public enum COAFrom
        {
            /// <summary>
            /// 输入的是Session.COASN
            /// </summary>
            COASN = 0,

            /// <summary>
            /// 输入的是Session.COASNList
            /// </summary>
            COASNList = 1,
        }
    }
}
