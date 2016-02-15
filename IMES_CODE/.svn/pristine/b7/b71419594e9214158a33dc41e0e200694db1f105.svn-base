// INVENTEC corporation (c)2011 all rights reserved. 
// Description: DELETE ChepPallet WHERE PLT=@ChepPalletNo
//              DELETE Pallet_RFID WHERE PLT=@PalletNo and RFIDCode=@ChepPalletNo
//              INSERT Pallet_RFID           @PalletNo,@ChepPalletNo,@carrier,@user,getdate(),getdate()           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-20   ZhangKaiSheng                 create
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// DELETE ChepPallet and update Pallet_RFID
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Final Scan
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.DELETE ChepPallet WHERE PLT=@ChepPalletNo
    ///         2.Update Pallet_RFID @PalletNo,@ChepPalletNo,@carrier,@user,getdate(),getdate()  
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet,Session.ChepPallet
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
    ///              IDeliveryRepository
    ///		     IPalletRepository       
    ///              Pallet
    /// </para> 
    /// </remarks>
    public partial class DeleteChepPLTAndUpdatePalletRfid : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public DeleteChepPLTAndUpdatePalletRfid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DELETE ChepPallet and update Pallet_RFID
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if ((bool)this.CurrentSession.GetValue("IsCheckChepPallet"))
            {
                string CurrentPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
                string CurrentChepPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.ChepPallet);

                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                //UC Update:AND a.InfoType = 'Carrier' Modify 2012/03/07
                //--------------------------------------------------------------------------------------------------------
                //IList<string> carrier = DeliveryRepository.GetInfoValuesByPalletNoOnDeliveryNo(CurrentPalletNo);
                IList<string> carrier = DeliveryRepository.GetInfoValuesByPalletNoOnDeliveryNo(CurrentPalletNo, "Carrier");
                //DeliveryInfo中Pallet對應的Carrier屬性為空
                //--------------------------------------------------------------------------------------------------------
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                //DELETE ChepPallet WHERE PLT=@ChepPalletNo
                itemRepository.DeleteChepPalletInfoDefered(CurrentSession.UnitOfWork, CurrentChepPalletNo);
                //DELETE Pallet_RFID WHERE PLT=@PalletNo and RFIDCode=@ChepPalletNo
                itemRepository.DeletePalletRfidDefered(CurrentSession.UnitOfWork, CurrentPalletNo, CurrentChepPalletNo);

                //INSERT Pallet_RFID SELECT @PalletNo,@ChepPalletNo,@carrier,@user,getdate(),getdate()
                PalletRfidInfo newRfidInfo = new PalletRfidInfo();
                newRfidInfo.plt = CurrentPalletNo;
                newRfidInfo.rfidcode = CurrentChepPalletNo;
                newRfidInfo.editor = Editor;
                if ((carrier ==null) || (carrier.Count==0))
                    newRfidInfo.carrier ="null";
                else
	                newRfidInfo.carrier = carrier[0];
                newRfidInfo.udt = DateTime.Now;
                newRfidInfo.cdt = DateTime.Now;
                itemRepository.InsertPalletRfidDefered(CurrentSession.UnitOfWork, newRfidInfo);
            }
            return base.DoExecute(executionContext);
        }

    }
}
