/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
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

namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         product Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.ProdNoList中每个product
    ///             1.创建product对象
    ///             2.保存product对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ProdMO
    ///         Session.ProdNoList
    ///         Session.ECR
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
    ///         insert Product
    ///         insert ProductStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateProdId : BaseActivity
    {
        public GenerateProdId()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var prodMo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            var prodNoList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);
            var ecr = CurrentSession.GetValue(Session.SessionKeys.ECR).ToString();
            var subProdList = new List<IProduct>();
            CurrentSession.AddValue(Session.SessionKeys.ProdList, subProdList);

            IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //将所有的ProdId插入Product表，ID=ProdId#, MOID=mo#, ModelID=model#,ECR=ecr# 
            //记录Product的状态，在ProductStatus插入记录
            
            foreach (string item in prodNoList)
            {

                Product prod = new Product(item);

                prod.ECR = ecr;
                prod.MO = prodMo.Key.ToString ();
                prod.Model = prodMo.Model;

                ProductStatus prodStatus = new ProductStatus();
                prodStatus.Editor = this.Editor;
                prodStatus.Line = this.Line;
                prodStatus.StationId = this.Station;
                prodStatus.ProId = item;
                prodStatus.Status = StationStatus.Pass;
                prodStatus.ReworkCode = string.Empty;
                prod.Status  = prodStatus;
                prodRepository.Add(prod, CurrentSession.UnitOfWork);
                subProdList.Add(prod);

            }


            return base.DoExecute(executionContext);
        }
    }
}