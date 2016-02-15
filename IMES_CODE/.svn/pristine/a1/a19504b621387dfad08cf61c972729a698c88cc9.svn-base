// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Check MB Type for CombinePCB in Lot
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 21012-07-11  Kaisheng,Zhang               create
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// Check MB Type 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.MB 
    ///         
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
    ///        MB
    /// </para> 
    /// </remarks>
    public partial class CheckMBType : BaseActivity
	{
        /// <summary>
        /// CheckMBType
        /// </summary>
        public CheckMBType()
		{
			InitializeComponent();
		}

        //ITC-1103-0050
        /// <summary>
        /// CheckMBType
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string strMbType = "";
            string mbSNo = this.Key;
            string[] lists = {"1","2","3","4","5","6","7","8","9","C","G"};
            //DEBUG ITC-1413-0053
            //MBCode 升位，CheckCode需要相应进行修改
            // CheckCode：若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码
            // CheckCode为数字，则为子板，为’R’，则为RCTO
            // ============================================================
            string strCheckCode = "";
            if (mbSNo.Substring(5, 1) == "M")
            {
                strCheckCode = mbSNo.Substring(6, 1);
            }
            else
            {
                strCheckCode = mbSNo.Substring(5, 1);
            }
            //=============================================================
            if (strCheckCode =="R")
            {
                //"RCTO"
                strMbType = "RCTO";
            }
            else
            {
                var flag = false;
                foreach (var lst in lists)
                {
                    if (strCheckCode == lst)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    strMbType = "PC";
                }
                else
                {
                    strMbType = "pc";
                }

            }

            CurrentSession.AddValue("GetMBType", strMbType);
            return base.DoExecute(executionContext);
        }
	
	}
}
