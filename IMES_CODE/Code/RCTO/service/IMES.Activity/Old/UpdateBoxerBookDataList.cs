// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的NewScanedProductCustSNList作为条件
//              更新BoxerBookData表中的CartonNo(用于053 Combine PO In Carton站)
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-11   Lucy Liu                     create
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

namespace IMES.Activity
{
    /// <summary>
    /// 用于更新一组BoxerBookData表中符合条件记录的CartonNo
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
    ///         1.从Session中获取NewScanedProductCustSNList及CartonNo，调用ProductRepository.UpdateBoxerBookDataDefered
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.NewScanedProductCustSNList，Session.CartonNoList
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
    ///         更新BoxerBookData  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              BoxerBookData
    /// </para> 
    /// </remarks>
    public partial class UpdateBoxerBookDataList : BaseActivity
    {
        public UpdateBoxerBookDataList()
        {
            InitializeComponent();
        }

       
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            

            List<string> cartonNoList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.CartonNoList);
            string cartonNo = cartonNoList[0];

            BoxerBookData newData = new BoxerBookData();
            newData.CartonSN = cartonNo;
                   

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IList<string> ProductCustSNList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductCustSNList);
            productRepository.UpdateBoxerBookDataListDefered(CurrentSession.UnitOfWork, newData, ProductCustSNList);
            

            return base.DoExecute(executionContext);
        }
    }
}
