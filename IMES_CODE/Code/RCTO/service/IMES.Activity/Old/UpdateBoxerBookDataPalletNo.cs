// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-04-02   Tong.Zhi-Yong                create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
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
using IMES.FisObject.PAK.BoxerBookData;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     053 Combine PO In Carton
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class UpdateBoxerBookDataPalletNo : BaseActivity
    {
        public UpdateBoxerBookDataPalletNo()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IList<IProduct> productList = (IList<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);
            var pltNo = CurrentSession.GetValue(Session.SessionKeys.PalletNo).ToString();

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            foreach (IProduct pdt in productList)
            {
                //productRepository.UpdateBoxerBookDataDefered(CurrentSession.UnitOfWork, pltNo, pdt.CUSTSN);
                productRepository.UpdateTestBoxDataLogDefered(CurrentSession.UnitOfWork, pltNo, pdt.CUSTSN);
            }
            
            PalletIdInfo info = new PalletIdInfo();
            info.PalletNo = pltNo;
            info.PalletId = ((IList<string>)CurrentSession.GetValue(Session.SessionKeys.PalletIdList))[0];
            info.Editor = Editor;
            info.Cdt = DateTime.Now;

            palletRepository.InsertPalletIdDefered(CurrentSession.UnitOfWork, info);

            CurrentSession.AddValue(Session.SessionKeys.Qty, 1);
            return base.DoExecute(executionContext);
        }
    }
}
