﻿// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有PAK_PackkingData
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21   Yuan XiaoWei                 create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的UnPackProductByDeliveryNoDefered方法
    ///          DELETE PAKODMSESSION FROM PAKODMSESSION O
    ///         inner join IMES2012_FA.dbo.Product as P on O.SERIAL_NUM =P.CUSTSN
    ///         WHERE P.DeliveryNo =@DeliveryNo and (P.Model LIKE 'PC%' or P.Model LIKE 'QC%')
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnPackPakODMSessionByDeliveryNo : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackPakODMSessionByDeliveryNo()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentDeliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);


            IPalletRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();

            currentRepository.UnPackPakOdmSessionByDeliveryNoDefered(CurrentSession.UnitOfWork, CurrentDeliveryNo);
            return base.DoExecute(executionContext);
        }
	}
}