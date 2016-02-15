// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 记录到CSNLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-11   ITC203019                    create
// 2011-12-29   ITC206010                    Implement
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.FisObject.PAK.COA;
using IMES.DataModel;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 用于记录CSNLog
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
    ///         1.从Session中获取COASN，调用InsertCSNLog方法
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
    ///         更新CSNLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              ICOAStatusRepository
    ///              CSNLog
    /// </para> 
    /// </remarks>
    public partial class WriteCSNLog : BaseActivity
    {
        private static readonly ILog logger = LogManager.GetLogger("fisLog");

        ///<summary>
        /// 构造函数
        ///</summary>
        public WriteCSNLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tp
        /// </summary>
        public static DependencyProperty TpProperty = DependencyProperty.Register("Tp", typeof(string), typeof(WriteCSNLog));

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
                return ((string)(base.GetValue(WriteCSNLog.TpProperty)));
            }
            set
            {
                base.SetValue(WriteCSNLog.TpProperty, value);
            }
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(short), typeof(WriteCSNLog));

        /// <summary>
        /// IsPass
        /// </summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public short IsPass
        {
            get
            {
                return ((short)(base.GetValue(WriteCSNLog.IsPassProperty)) );
            }
            set
            {
                base.SetValue(WriteCSNLog.IsPassProperty, value);
            }
        }


        /// <summary>
        /// Wrint CSNLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();

            Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
            string pno = CurrentSession.GetValue(Session.SessionKeys.Pno) as string;
            var newLog = new CSNLogInfo();
            newLog.tp = Tp;
            newLog.editor = Editor;
            newLog.pdLine = currentProduct.CUSTSN;
            newLog.wc = Station;
            newLog.isPass = IsPass;
            newLog.pno = pno;
            newLog.snoId = currentProduct.ProId;
            newLog.cdt = DateTime.Now;


            currentRepository.InsertCSNLogDefered(CurrentSession.UnitOfWork, newLog);

            return base.DoExecute(executionContext);
        }


    }
}
