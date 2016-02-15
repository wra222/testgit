
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
    ///         应用于Travel Card Print
    /// </para>
    /// <para>
    /// 实现逻辑：
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
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateProductRecordForDocking : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public GenerateProductRecordForDocking()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var prodMo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            var prodNoList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);
        
            
            var subProdList = new List<IProduct>();
            CurrentSession.AddValue(Session.SessionKeys.ProdList, subProdList);

            IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //将所有的ProdId插入Product表，ID=ProdId#, MOID=mo#, ModelID=model#
            string ecr = (string)CurrentSession.GetValue(Session.SessionKeys.ECR);
            foreach (string item in prodNoList)
            {

                Product prod = new Product(item);

                prod.MO = prodMo.Key.ToString();
                prod.Model = prodMo.Model;
                ProductStatus prodStatus = new ProductStatus();
                prodStatus.Editor = this.Editor;
                prodStatus.Line = this.Line;
                prodStatus.StationId = "F0";
                prodStatus.ProId = item;
                prodStatus.Status = StationStatus.Pass;
                prodStatus.ReworkCode = string.Empty;
                prod.Status = prodStatus;
                
                if (!String.IsNullOrEmpty(ecr))
                    prod.SetExtendedProperty("ECR", ecr.ToUpper(), this.Editor);

                prodRepository.Add(prod, CurrentSession.UnitOfWork);
                subProdList.Add(prod);
            }
            IList<string> range = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);
            string mo = (string)CurrentSession.GetValue(Session.SessionKeys.MONO);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "PrdId");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, range[0]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, range[range.Count - 1]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, mo);

            return base.DoExecute(executionContext);
        }
    }
}