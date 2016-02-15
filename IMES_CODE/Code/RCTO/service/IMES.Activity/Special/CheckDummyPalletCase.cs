// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的PalletNo,获取CheckDummyPalletCase类型，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                        
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-22   Chen Xu (itc208014)          create
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
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository.PAK;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的PalletNo,获取Pallet对象，并放到Session中
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
    ///         1.根据palletNo，区分 Dummy Pallet Case
    ///         2.根据productId，调用IPalletRepository的GetDummyShipDet方法，获取DummyShipDet对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PalletNo, Session.ProductId
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.DummyPalletCase,Session.DummyShipDet
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPalletRepository
    ///              palletNo, productId
    /// </para> 
    /// </remarks>
    public partial class CheckDummyPalletCase : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public CheckDummyPalletCase()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check Dummy Pallet Case and put it into Session.SessionKeys.DummyPalletCase
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string palletNo = (string) CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string productId = currentProduct.ProId;
            IPalletRepository ipalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();


            //获取Pallet No : IMES_FA..Product.PalletNo: 如果该PalletNo 不是以'NA'或者'BA' 为前缀，则需要报告错误：“非散装FDE 机器，不能使用本功能！”

            string DPC = string.Empty; //Dummy Pallet Case
            DummyShipDetInfo dummyshipdet = new DummyShipDetInfo();
            string snoId = productId;
            dummyshipdet = ipalletRepository.GetDummyShipDet(snoId);
            if (palletNo.Substring(0, 2) == "NA")
            {
                if (!(dummyshipdet == null || dummyshipdet.snoId == "")) 
                {
                    DPC = "NA";        //  Case NA Dummy Pallet
                }
                else DPC = "NAN";       //  Case NA Non Dummy Pallet
            }
            else if (palletNo.Substring(0, 2) == "BA")
            {
                if (!(dummyshipdet == null || dummyshipdet.snoId == ""))
                {
                    DPC = "BA";         //  Case BA Dummy Pallet
                }
                else DPC = "BAN";       //  Case BA Non Dummy Pallet
            }
            else
            {
                FisException ex = new FisException("PAK019", new string[] {}); //非散装FDE 机器，不能使用本功能！
                throw ex;
            }
           
            CurrentSession.AddValue(Session.SessionKeys.DummyPalletCase, DPC);
            CurrentSession.AddValue(Session.SessionKeys.DummyShipDet, dummyshipdet);
	        return base.DoExecute(executionContext);
        }
	}
}
