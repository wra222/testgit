/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 将符合条件的所有Product 资料(Product and ProductStatus) 备份到UnpackProduct and UnpackProductStatus 表
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-03-10   Luy Liu           Create 
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using log4net;
namespace IMES.Activity
{
    /// <summary>
    /// 备份Product数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于070UnpackCarton.xoml,070UnpackDN.xoml,070UnpackPallet.xoml,121DismantleFA.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// 将符合条件的所有Product 资料(Product and ProductStatus) 备份到UnpackProduct and UnpackProductStatus 表
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///         备份到UnpackProduct and UnpackProductStatus 表
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class BackupForUnpack : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackupForUnpack()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  备份数据
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //Unpack Carton/DN,会存在Session.SessionKeys.Carton变量,Unpack Pallet会存在Session.SessionKeys.PalletNo变量,PO Data会存在Session.SessionKeys.DeliveryNo变量
            //DismantleFA没有这3个变量，只有Session.SessionKeys.Product
            String cartonSN = (String)CurrentSession.GetValue(Session.SessionKeys.Carton);
            String palletNo = (String)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            String dn = (String)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo );
            string productId = "";
            if (currentProduct != null)
            {
                productId = currentProduct.ProId;
            }
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            productRepository.CopyProductToUnpackDefered(CurrentSession.UnitOfWork, cartonSN, palletNo, dn, productId, this.Editor);

            //Modify:2012/03/09 UC:Copy ProducInfo,Product_Part资料到UnpackProductInfo, UnpackProduct_Part 
            productRepository.BackUpProductInfoDefered(CurrentSession.UnitOfWork, productId, this.Editor);
            productRepository.BackUpProductPartDefered(CurrentSession.UnitOfWork, productId, this.Editor);
            
            return base.DoExecute(executionContext);
        }
    }
}
