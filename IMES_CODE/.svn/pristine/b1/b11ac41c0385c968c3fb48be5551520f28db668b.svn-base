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
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using System.Collections.Generic;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.Common.Misc;
using IMES.DataModel;
using IMES.FisObject.Common.Warranty;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的DCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取DCode最大值;
    ///         2.产生指定机型的DCode;
    ///         3.更新DCode最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         SessionKeys.SelectedWarrantyRuleID
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.DCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新NumControl
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class AcquireDCode : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireDCode()
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

            Warranty SelectedWarranty = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>().Find(CurrentSession.GetValue(Session.SessionKeys.SelectedWarrantyRuleID));

            
            if(SelectedWarranty == null){
                throw new FisException("ICT010",new string[]{});
            }

            DateTime CreateTime = DateTime.Now;
            string WarrantyDate = "";
            
            if (SelectedWarranty.WarrantyFormat == "YMM")
            {
                WarrantyDate = (CreateTime.Year % 10).ToString() + CreateTime.Month.ToString("00");
            }
            else if (SelectedWarranty.WarrantyFormat == "YYM")
            {
                WarrantyDate = (CreateTime.Year % 100).ToString() + GetMonth(CreateTime.Month);
            }
            else {
                throw new FisException("ICT010", new string[] { });
            }
            

            string DateCode = SelectedWarranty.ShipTypeCode + WarrantyDate+SelectedWarranty.WarrantyCode;

            CurrentSession.AddValue(Session.SessionKeys.DCode, DateCode);

            return base.DoExecute(executionContext);
        }

        private string GetMonth(int month)
        {
            if (month > 9)
            {
                switch (month)
                {
                    case 10:
                        return "A";
                    case 11:
                        return "B";
                    default:
                        return "C";
                }

            }
            else
            {
                return month.ToString();
            }
        }


    }
}
