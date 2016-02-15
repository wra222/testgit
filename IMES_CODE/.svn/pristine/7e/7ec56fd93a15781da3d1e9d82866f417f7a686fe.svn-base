// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 备份Product / ProductStatus / Product_Part / ProductInfo 表中将被解绑的记录
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-03-02    itc202017                   create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 备份Product / ProductStatus / Product_Part / ProductInfo 表中将被解绑的记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         需要将Product / ProductStatus / Product_Part / ProductInfo 表中的记录备份到UnpackProduct / UnpackProductStatus / UnpackProduct_Part / UnpackProductInfo
    ///         UnpackProduct / UnpackProductStatus / UnpackProduct_Part / UnpackProductInfo 相对于源表增加了两个字段UEditor 和UPdt 分别记录解资料的Operator和时间
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class BackupProductToUnpackByDN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackupProductToUnpackByDN()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 备份Product / ProductStatus / Product_Part / ProductInfo 表中将被解绑的记录
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string dn = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            IList<string> nameList = new List<string>();
            nameList.Add("UCC");
            nameList.Add("BoxId");
            currentProductRepository.BackUpProductByDnDefered(CurrentSession.UnitOfWork, dn, Editor);
            currentProductRepository.BackUpProductInfoByDnDefered(CurrentSession.UnitOfWork, dn, Editor, nameList);
            currentProductRepository.BackUpProductStatusByDnDefered(CurrentSession.UnitOfWork, dn, Editor);

            return base.DoExecute(executionContext);
        }
    }
}
