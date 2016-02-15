// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据Session中的DummyPalletCase,获取 FDE Scan Qty数量，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-23   Chen Xu (itc208014)          create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity.NewNeedCheck
{
    /// <summary>
    /// 根据Session中的DummyPalletCase,获取 FDE Scan Qty对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pallet为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据PalletNo，调用IPalletRepository的Find方法，获取Pallet对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ScanQty
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.DummyPalletNo
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPalletRepository
    ///              Pallet
    /// </para> 
    /// </remarks>
    public partial class GetFDEScanQty : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetFDEScanQty()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Get Dummy Pallet No and put it into Session.SessionKeys.DummyPalletNo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string DPC = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletCase);
            string dummyPalletNo = (string) CurrentSession.GetValue(Session.SessionKeys.DummyPalletNo);
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            int scanQty = 0;
            // IList<DummyShipDetInfo> dummyshipdetLst =null;

            //switch (DPC)  //2012-3-16 : Revision: 9302: Get Pallet Info 中Scan Qty 不再区分各种Case进行处理 : For All Case, Scan Qty = 0
            //{
            //    case "NA":
            //        scanQty = 0;
            //        break;
            //    case "NAN":
            //         dummyshipdetLst = new List<DummyShipDetInfo>();
            //         dummyshipdetLst = iPalletRepository.GetDummyShipDetListByBol(dummyPalletNo);
            //        if(dummyshipdetLst!=null && dummyshipdetLst.Count > 0)
            //        {
            //            scanQty = dummyshipdetLst.Count;
            //        }
            //        else scanQty = 0;
            //        break;
            //    case "BA":
            //        scanQty = 0;
            //        break;
            //    case "BAN":
            //         dummyshipdetLst = new List<DummyShipDetInfo>();
            //         dummyshipdetLst = iPalletRepository.GetDummyShipDetListByBol(dummyPalletNo);
            //        if(dummyshipdetLst!=null && dummyshipdetLst.Count > 0)
            //        {
            //            scanQty = dummyshipdetLst.Count;
            //        }
            //        else scanQty = 0;
            //        break;
            //    default:
            //        break;
            //}

            CurrentSession.AddValue(Session.SessionKeys.ScanQty, scanQty.ToString());

            return base.DoExecute(executionContext);
        }
	}

}
