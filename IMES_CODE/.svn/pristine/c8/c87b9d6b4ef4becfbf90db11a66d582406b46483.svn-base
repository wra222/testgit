/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: RCTO Change Single
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-07-16   Kerwin            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Utility.Generates.impl;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;


namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的MBNO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-SA-UC PCA ICT Input For RCTO 非链板
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MB
    ///             1.修改MBSn
    ///             2.保存MB对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         Update PCB/PCBInfo/PCB_Part/PCBStatus/PCBLog/PCBTestLog Set PCBNo = @RCTOMBSn where PCBNo= @MBSn
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class PalletPrintLabelForRCTO : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public PalletPrintLabelForRCTO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /// 
        /*
IDeliveryRepository::IList<DeliveryInfo> GetDeliveryInfoFromDeliveryByPalletNo(string palletNo, DeliveryInfo condition);
IDeliveryRepository::int GetCountOfPonoFromDeliveryByPalletNo(string palletNo, Delivery condition);
IDeliveryRepository::int GetCountOfModelFromDeliveryByPalletNo(string palletNo, Delivery condition);
*/
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            string PalletNo =(string) CurrentSession.GetValue(Session.SessionKeys.Pallet) ;
            string PrintLable="";
            IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            DeliveryInfo condition = new DeliveryInfo();
            condition.InfoType = "ShiptoId";
            
            IList<DeliveryInfo> DnInfoList = DeliveryRepository.GetDeliveryInfoFromDeliveryByPalletNo(PalletNo, condition);
            if (DnInfoList.Count > 0)
            {
                string InfoValue = DnInfoList[0].InfoValue;
                
                if (InfoValue.Length >=3 &&(InfoValue.Substring(0, 3) == "ICZ" || InfoValue.Substring(0, 3) == "IHC"))
                {
                    if (DeliveryRepository.GetCountOfPonoFromDeliveryByPalletNo(PalletNo, new Delivery()) ==1)
                    {
                        if (DeliveryRepository.GetCountOfModelFromDeliveryByPalletNo(PalletNo, new Delivery()) == 1)
                            PrintLable = "RCTO_Pallet_Verify_Label00";
                        else
                            PrintLable = "RCTO_Pallet_Verify_Label01";
                    }
                    else 
                        PrintLable = "RCTO_Pallet_Verify_Label02";
                }
                else {
                    PrintLable = "RCTO_Pallet_verify_Label";
                }                

            }
            else
            {
                PrintLable = "RCTO_Pallet_verify_Label";
            }


            CurrentSession.AddValue(Session.SessionKeys.QCStatus, PrintLable);
            return base.DoExecute(executionContext);
        }

    
    }
}
