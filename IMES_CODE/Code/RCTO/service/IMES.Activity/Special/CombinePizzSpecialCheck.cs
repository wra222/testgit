using System;
using System.Collections.Generic;
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity.Special
{
    /// <summary>
    /// 1.check Content Label
    ///如果PRODUCT 绑定的Delivery (IMES_FA..Product.DeliveryNo) 的RegId = 'SCN' and 
    /// ShipTp = 'CTO' (RegId: IMES_PAK..DeliveryInfo.InfoType = 'RegId'；ShipTp: 
    /// IMES_PAK..DeliveryInfo.InfoType = 'ShipTp')时，如果在IMES_FA..ProductLog 
    /// 中不存在Station = '8D' 的记录时，需要报告错误：“未打印Content Label!”
    ///
    ///2.	检查干燥剂
    ///如果机器是海运方式出货（PRODUCT 绑定的Delivery (IMES_FA..Product.DeliveryNo) 
    /// 的ShipWay = 'T002' (ShipWay: IMES_PAK..DeliveryInfo.InfoType = 'ShipWay')时，
    /// 表示海运方式出货)，需要弹出Message Box 提示用户：“此机器为海运方式出货,
    /// 请检查是否有装乾燥剂”
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-PAK-UC Combine Pizza
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             “未打印Content Label!”
    /// </para> 
    /// <para>    
    /// 输入：
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.IsOceanShipping
    /// </para> 
    ///<para>
    /// 数据更新:
    ///        无
    /// </para>
    ///<para> 
    /// 相关FisObject:
    ///         Delivery
    ///         Product    
    /// </para> 
    /// </remarks>
    public partial class CombinePizzSpecialCheck : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CombinePizzSpecialCheck()
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
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string dn = currentProduct.DeliveryNo;
            if (!string.IsNullOrEmpty(dn))
            {
                IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery dnObj = dnRep.Find(dn);
                if (dnObj != null)
                {
                    // 1.check Content Label
                    //如果PRODUCT 绑定的Delivery (IMES_FA..Product.DeliveryNo) 的RegId = 'SCN' and 
                    // ShipTp = 'CTO' (RegId: IMES_PAK..DeliveryInfo.InfoType = 'RegId'；ShipTp: 
                    // IMES_PAK..DeliveryInfo.InfoType = 'ShipTp')时，如果在IMES_FA..ProductLog 
                    // 中不存在Station = '8D' 的记录时，需要报告错误：“未打印Content Label!”
                    object regid = dnObj.GetExtendedProperty("RegId");
                    object shiptp = dnObj.GetExtendedProperty("ShipTp");
                    if (regid != null 
                        && regid.ToString().CompareTo("SCN") == 0 
                        && shiptp != null
                        && shiptp.ToString().CompareTo("CTO") == 0)
                    {
                        bool havePassed8D = false;
                        if (currentProduct.ProductLogs != null)
                        {
                            foreach (var log in currentProduct.ProductLogs)
                            {
                                if (log.Station.CompareTo("8D") == 0)
                                {
                                    havePassed8D = true;
                                }
                            }
                        }
                        if (!havePassed8D)
                        {
                            throw new FisException("CHK850", new List<string>());  
                        }
                    }
                    //2.whether ocean shipping (检查干燥剂)
                    //如果机器是海运方式出货（PRODUCT 绑定的Delivery (IMES_FA..Product.DeliveryNo) 
                    // 的ShipWay = 'T002' (ShipWay: IMES_PAK..DeliveryInfo.InfoType = 'ShipWay')时，
                    // 表示海运方式出货)，需要弹出Message Box 提示用户：“此机器为海运方式出货,
                    // 请检查是否有装乾燥剂”
                    object shipway = dnObj.GetExtendedProperty("ShipWay");
                    if (shipway != null && shipway.ToString().CompareTo("T002") == 0)
                    {
                        CurrentSession.AddValue(Session.SessionKeys.IsOceanShipping, true);
                    }
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
