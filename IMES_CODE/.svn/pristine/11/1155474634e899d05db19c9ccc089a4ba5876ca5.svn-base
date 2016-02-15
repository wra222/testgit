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
using IMES.DataModel;

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
    public partial class UnPackUpdateCSNMas : BaseActivity
    {
        private static readonly ILog logger = LogManager.GetLogger("fisLog");

        ///<summary>
        /// 构造函数
        ///</summary>
        public UnPackUpdateCSNMas()
        {
            InitializeComponent();
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
            string csn2 = CurrentSession.GetValue(Session.SessionKeys.Pno) as string;


            var setCSN = new CSNMasInfo
            {       
                editor = Editor,
                pdLine = Line,
                status = "P1",
                //StationID = Station,
                //cdt = DateTime.Now
            };
            var conditionCSN = new CSNMasInfo
            {
                csn2 = csn2
            };


            currentRepository.UpdateCSNMas(setCSN, conditionCSN);
       
           
            return base.DoExecute(executionContext);
        }

    
    }
}
