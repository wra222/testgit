// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 查询PickIDCtrl 表，取当天需要出货的Pick ID 记录的Forwarder,Driver,Truck ID
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-02   Kerwin                       create
// Known issues:
using System;
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
    /// 查询PickIDCtrl 表，取当天需要出货的Pick ID 记录的Forwarder,Driver,Truck ID
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-PAK-UC Final Scan
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK149
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
    ///      SessionKeys.PickIDCtrl,PickIDCtrlInfo 
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class GetPickIDCtrlByPickID : BaseActivity
	{
		///<summary>
        ///constructor
		///</summary>
        public GetPickIDCtrlByPickID()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 查询PickIDCtrl 表，取当天需要出货的Pick ID 记录的Forwarder,Driver,Truck ID
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<PickIDCtrlInfo> lstPickIDCtrlInfo = palletRepository.GetPickIDCtrlInfoByPickID(Key);
            PickIDCtrlInfo pickIDCtrlInfo;

            if (lstPickIDCtrlInfo == null || lstPickIDCtrlInfo.Count == 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("CHK149", errpara);
            }

            pickIDCtrlInfo = lstPickIDCtrlInfo[0];
            //ITC-1268-0052 Tong.Zhi-Yong 2011-04-14
            lstPickIDCtrlInfo = palletRepository.GetPickIDCtrlInfoByPickIDAndDate(Key, DateTime.Now);

            if (lstPickIDCtrlInfo != null && lstPickIDCtrlInfo.Count != 0)
            {
                pickIDCtrlInfo = lstPickIDCtrlInfo[0];
            }

            CurrentSession.AddValue(Session.SessionKeys.PickIDCtrl, pickIDCtrlInfo);
            //Add get Forwarder by TruckID  pickIDCtrlInfo.TruckID
            //DEBUG ITC-1360-0554
            IList<ForwarderInfo> lstFwder = palletRepository.GetForwarderInfoByTruckID(pickIDCtrlInfo.TruckID);
            ForwarderInfo fwderInfo;

            if (lstFwder == null || lstFwder.Count == 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(pickIDCtrlInfo.TruckID);
                throw new FisException("CHK147", errpara);
            }

            fwderInfo = lstFwder[0];
            CurrentSession.AddValue(Session.SessionKeys.Forwarder, fwderInfo);

            CurrentSession.AddValue("ShowDriver", pickIDCtrlInfo.Driver);
            CurrentSession.AddValue("ShowForwarder", pickIDCtrlInfo.Fwd);

            //Add end
            return base.DoExecute(executionContext);
        }
	}
}
