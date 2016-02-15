﻿/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for KB CT Check Page
 * UI:CI-MES12-SPEC-FA-UI KB CT Check.docx –2012/6/12 
 * UC:CI-MES12-SPEC-FA-UC KB CT Check.docx –2012/6/12            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-6-12   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1413-0019, Jessica Liu, 2012-6-20
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Prodid和KBCT的对应关系是否合法
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FA站KB CT Check 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1. 获取ProdId已绑定KB的PartSn进行check
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         KBCT
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    ///
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         
    /// </para> 
    /// </remarks>
	public partial class CheckProdidAndKBCT: BaseActivity
	{
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProdidAndKBCT()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 检查Prodid和KBCT的对应关系是否合法
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentKBCT = (string)CurrentSession.GetValue("KBCT");    
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            CurrentSession.AddValue("NotMatchKBCT", "N");

            //ITC-1413-0019, Jessica Liu, 2012-6-20
            string currentProdModel = currenProduct.Model;
            if (currentProdModel.Substring(0, 2) != "PC")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK286", errpara);  //非PC Model，不能刷此界面
            }

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductPart condition = new ProductPart();
            condition.ProductID = currenProduct.ProId;
            condition.CheckItemType = "KB";
            IList<ProductPart> prodPartList = productRepository.GetProductPartList(condition);

            if (prodPartList != null && prodPartList.Count > 0)
            {
                string firstProdPartSn = prodPartList[0].PartSn;
                if (string.IsNullOrEmpty(firstProdPartSn))
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK116", errpara);  //ProdId未绑定KB
                }
                else if (firstProdPartSn.Trim() != CurrentKBCT.Trim())
                {
                    if ("Y".Equals(NotExceptionWhenNotMatch))
                    {
                        CurrentSession.AddValue("NotMatchKBCT", "Y");
                    }
                    else
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK117", errpara);  //ProdId与 KB CT 匹配错误
                    }
                }
            }
            else
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK115", errpara);  //找不到对应的Product Part信息
            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  True不停止，false 停止
        /// </summary>
        public static DependencyProperty NotExceptionWhenNotMatchProperty = DependencyProperty.Register("NotExceptionWhenNotMatch", typeof(string), typeof(CheckProdidAndKBCT));

        /// <summary>
        /// NotExceptionWhenNotMatch:True Or False
        /// </summary>
        [DescriptionAttribute("NotExceptionWhenNotMatch")]
        [CategoryAttribute("NotExceptionWhenNotMatch Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string NotExceptionWhenNotMatch
        {
            get
            {
                return ((string)(base.GetValue(CheckProdidAndKBCT.NotExceptionWhenNotMatchProperty)));
            }
            set
            {
                base.SetValue(CheckProdidAndKBCT.NotExceptionWhenNotMatchProperty, value);
            }
        }

	}
}
