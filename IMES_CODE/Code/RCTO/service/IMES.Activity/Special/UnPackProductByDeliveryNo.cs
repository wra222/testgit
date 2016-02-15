// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

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
    ///           update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0 
    ///           where DeliveryNo =@DeliveryNo and (P.Model LIKE 'PC%' or P.Model LIKE 'QC%')
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
    public partial class UnPackProductByDeliveryNo : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackProductByDeliveryNo()
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


            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            IDeliveryRepository currentDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            IPalletRepository currentPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();

            /*
             * Answer to: ITC-1360-0845
             * Description: Update PAKLocMas info.
             */
            IList<string> pnList = currentDeliveryRepository.GetPalletNoListByDeliveryNo(CurrentDeliveryNo);
            currentProductRepository.UnPackProductByDeliveryNoDefered(CurrentSession.UnitOfWork, CurrentDeliveryNo);
            foreach (string pn in pnList)
            {
                PakLocMasInfo setVal = new PakLocMasInfo();
                PakLocMasInfo cond = new PakLocMasInfo();
                setVal.editor = Editor;
                setVal.pno = "";
                cond.pno = pn;
                cond.tp = "PakLoc";
                currentPalletRepository.UpdatePakLocMasInfoDefered(CurrentSession.UnitOfWork, setVal, cond);
            }
            return base.DoExecute(executionContext);
        }
	}
}
