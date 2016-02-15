/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:从PCB表获得PCBModelID
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  select PCBModelID from PCB nolock where PCBNo = @MBSno
 * 
 */

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Process;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Model是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于SA： MB SPlit
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
    ///         Model 
    ///         
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
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IModelRepository
    ///         IModel
    /// </para> 
    /// </remarks>
    public partial class GetPCBModel : BaseActivity
	{
        /// <summary>
        /// GetPCBModel
        /// </summary>
        public GetPCBModel()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 获得PCBModelID
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbsno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;

          //  string PCBModelID = mb.ModelObj.Mbcode;
            string PCBModelID = mb.PCBModelID;

            if (string.IsNullOrEmpty(PCBModelID))
            {
                List<string> errpara = new List<string>();
                errpara.Add(mbsno);
                FisException ex = new FisException("PAK077", errpara);  //没有找到MBSno %1 对应的PCB！
                throw ex;
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.PCBModelID, PCBModelID);
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
