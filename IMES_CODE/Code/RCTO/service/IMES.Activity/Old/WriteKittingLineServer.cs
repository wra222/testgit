/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 调用BindKitting sp为亮灯系统绑定数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于015Kitting Input站
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///        
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
    ///         KIT..BinData
    ///         KIT..kittingboxsn
    ///         IMES_FA..BinData
    ///         IMES_FA..KittingBoxSN
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository    
    /// </para> 
    /// </remarks>
    public partial class WriteKittingLineServer : BaseActivity
    {
        public WriteKittingLineServer()
        {
            InitializeComponent();
        }
 
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prdrd = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //UnitOfWork uow = new UnitOfWork();
            var currentProduct = (IProduct)this.CurrentSession.GetValue(Session.SessionKeys.Product);

            prdrd.BindKittingDefered(CurrentSession.UnitOfWork, currentProduct.MO , currentProduct.ProId, this.CurrentSession.GetValue(Session.SessionKeys.boxId).ToString(), this.Line);

            //uow.Commit();
            return base.DoExecute(executionContext);
        }
    }
}
