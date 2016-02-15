/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date                 Name               Reason 
 * ==================================================================
 * 2011-09-08      104126            Create 
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
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// PCBID结合/更新Product & 产生/更新ProductStatus
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         FRU SA TEST 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.ProdNoList中每个product
    ///             1.创建product对象
    ///             2.保存product对象
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
    ///         Session.MB
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
    ///         insert/update Product
    ///         insert/update ProductStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class FRUBindProductIDWithPCBNo : BaseActivity
    {
        public FRUBindProductIDWithPCBNo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 过站结果Pass or Fail
        /// </summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(StationStatus), typeof(FRUBindProductIDWithPCBNo));
        ///<summary>
        /// 过站结果Pass or Fail
        ///</summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus IsPass
        {
            get
            {
                return ((StationStatus)(base.GetValue(FRUBindProductIDWithPCBNo.IsPassProperty)));
            }
            set
            {
                base.SetValue(FRUBindProductIDWithPCBNo.IsPassProperty, value);
            }
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string item = this.Key;

            if (mb.GetAttributeValue("Model") == null)
            {
                List<string> errpara = new List<string>();

                errpara.Add(item);

                throw new FisException("SFC014", errpara); // Need Change error code for No PCB Mapping to  Model
            }

            string model = (string)mb.GetAttributeValue("Model");

     
            IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IList<IProduct> lstProduct = prodRepository.GetProductListByPCBID(item);

            if (lstProduct != null)
            {
                foreach (var product in lstProduct)
                {
                    if (product.ProId != item)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("MB");
                        erpara.Add(item);
                        erpara.Add(product.ProId);
                        var ex = new FisException("CHK009", erpara);
                        throw ex;
                    }
                }
            }

            ProductStatus prodStatus = new ProductStatus();

            prodStatus.Editor = this.Editor;
            prodStatus.Line = this.Line;
            prodStatus.StationId = this.Station;
            prodStatus.ProId = item;
            prodStatus.Status = this.IsPass; //StationStatus.Pass;
            prodStatus.ReworkCode = string.Empty;
            prodStatus.Udt = DateTime.Now;

            Product prod = new Product(item);

            var currentProduct = prodRepository.GetProductByIdOrSn(item);
            if (currentProduct == null)
            {
                prod.MAC = mb.MAC;
                prod.MO = mb.SMTMO;
                prod.Model = model;
                prod.PCBID = item;
                prod.PCBModel = mb.Model;
                prod.MBECR = mb.ECR;
                prod.Status = prodStatus;
                prod.CUSTSN = mb.CustSn;

                prodRepository.Add(prod, CurrentSession.UnitOfWork);
                CurrentSession.AddValue(Session.SessionKeys.Product, prod);
             }
            else 
            {
                currentProduct.MAC = mb.MAC;
                currentProduct.MO = mb.SMTMO;
                currentProduct.Model = model;
                currentProduct.PCBID = item;
                currentProduct.PCBModel = mb.Model;
                currentProduct.MBECR = mb.ECR;
                currentProduct.CUSTSN = mb.CustSn;
                currentProduct.UpdateStatus(prodStatus);

                prodRepository.Update(currentProduct, CurrentSession.UnitOfWork);
                CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            }

            return base.DoExecute(executionContext);
        }
    }
}