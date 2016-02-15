// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013/01/08 Benson          create
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;
namespace IMES.Activity
{
    /// <summary>
    /// WIN8 Label 的列印条件及方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
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
    ///         Session.Product
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
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class CheckWin8LabelPrint : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckWin8LabelPrint()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<IBOMNode> bomNodeList = (IList<IBOMNode>)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            string pno = string.Empty;
            pno = curProduct.Model;
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(pno);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;
        
            //Check ModelBOM 中Model 直接下阶的所有Descr =WIN8 BOX LABEL
            bool isWin8 = false;
            CurrentSession.AddValue("Win8BoxLabel", "");
             foreach (IBOMNode node in bomList)
            {
                if (string.Compare(node.Part.Descr,this.Win8Descr, true) == 0)
                { isWin8=true;break; }
           
            }
             if (isWin8) { CurrentSession.AddValue("Win8BoxLabel", "Win8BoxLabel"); }
            
       
	        return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 类型值 WIN8 Descr
        /// </summary>
        public static DependencyProperty Win8DescrProperty = DependencyProperty.Register("Win8Descr", typeof(string), typeof(CheckWin8LabelPrint));


        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("Win8Descr")]
        [CategoryAttribute("Win8Descr Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Win8Descr
        {
            get
            {
                return ((string)(base.GetValue(CheckWin8LabelPrint.Win8DescrProperty)));
            }
            set
            {
                base.SetValue(CheckWin8LabelPrint.Win8DescrProperty, value);
            }
        }
	}
}
