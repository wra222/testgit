// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  更新Pallet 上所有PRODUCT 的状态
// UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-24   Du Xuan (itc98066)          create
// Known issues:
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     CI-MES12-SPEC-PAK-UC Pallet Weight.docx  
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     Insert IMES_FA..ProductLog – 记录Pallet 上所有PRODUCT 的Log
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet
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
    ///		     IProductRepository       
    ///             
    /// </para> 
    /// </remarks>
    public partial class UpdateProductStatusByPallet : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateProductStatusByPallet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            /*var newStatus = new IMES.FisObject.FA.Product.ProductStatus();
            newStatus.Status = IMES.FisObject.Common.Station.StationStatus.Pass;
            newStatus.StationId = this.Station;
            newStatus.Editor = this.Editor;
            newStatus.Line = this.Line;

            productRep.UpdateProductStatusByPallet(CurrentPalletNo, newStatus, -1, IMES.FisObject.Common.Station.StationStatus.Pass);
          
            var newLog = new ProductLog
            {
                Model = "",
                Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                Editor = this.Editor,
                Line = this.Line,
                Station = this.Station,
                Cdt = DateTime.Now
            };
            productRep.WriteProductLogByPalletNo(CurrentPalletNo, newLog);*/

            return base.DoExecute(executionContext);
        }

    }
}
