// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 根据Pallet号码将其所有product的Pallet设定为空
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-11   Lucy Liu                     Create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Activity
{
    /// <summary>
    /// 根据Pallet号码将其所有product的Pallet设定为空
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Unpack Pallet
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Pallet No,根据Pallet No号码将其所有product的Pallet No设定为空
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PalletNo
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
    ///         根据Pallet号码将其所有product的Pallet设定为空
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    /// </para> 
    /// </remarks>
    public partial class PalletUnpack : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public PalletUnpack()
		{
			InitializeComponent();
		}



        /// <summary>
        /// 执行根据Pallet No修改所有属于该Pallet的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string palletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            currentProductRepository.PalletUnpackDefered(CurrentSession.UnitOfWork, palletNo);

            IPalletRepository currentPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();

            Pallet pallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            pallet.Editor = this.Editor;
            pallet.Station = "0";
            pallet.Udt = DateTime.Now;
            currentPalletRepository.Update(pallet, CurrentSession.UnitOfWork);
            
            //string custSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            //currentProductRepository.UpdateTestBoxDataLogForUnpackPalletDefered(CurrentSession.UnitOfWork, custSN);
            IList<string> ProductCustSNList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductCustSNList);
            currentProductRepository.UpdateTestBoxDataLogListForUnpackPalletDefered(CurrentSession.UnitOfWork, ProductCustSNList);

            
            currentPalletRepository.DeletePalletIDByPalletNo(palletNo);

            return base.DoExecute(executionContext);
        }
	}
}
