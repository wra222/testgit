﻿// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的TruckID,获取Forwarder对象，并放到Session中
// UI:          CI-MES12-SPEC-PAK-UI Pick Card
// UC:          CI-MES12-SPEC-PAK-UC Pick Card             
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-27   zhu lei                      create
// Known issues:
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的TruckID,获取Forwarder对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
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
    ///         this.Key
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
    public partial class GetForwarderByTruckID : BaseActivity
	{
		///<summary>
		///</summary>
        public GetForwarderByTruckID()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 根据输入的TruckID,获取Forwarder对象，并放到Session中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<ForwarderInfo> lstFwder = palletRepository.GetForwarderInfoByTruckID(Key);
            ForwarderInfo fwderInfo;

            if (lstFwder == null || lstFwder.Count == 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("CHK147", errpara);
            }

            //fwderInfo = lstFwder[0];
            string dateStr = (string)CurrentSession.GetValue(Session.SessionKeys.DateCode);
            try
            {
                lstFwder = (from fr in lstFwder where fr.Date == dateStr select fr).ToList();
                if (lstFwder.Count == 0) throw new NullReferenceException();
                //fwderInfo = lstFwder.First(fi => fi.Date == dateStr);
                fwderInfo = lstFwder[0];
            }
            catch (Exception)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("CHK147", errpara);
            }

            if (string.IsNullOrEmpty(fwderInfo.MAWB))
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("CHK148", errpara);
            }

			if (!palletRepository.PickCardCheck(Key, dateStr))
			{
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("CHK189", errpara);
			}
			
            CurrentSession.AddValue(Session.SessionKeys.Forwarder, fwderInfo);
            CurrentSession.AddValue(Session.SessionKeys.Forwarder+"List", lstFwder);
            return base.DoExecute(executionContext);
        }
	}
}
