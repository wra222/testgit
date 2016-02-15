// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Insert ProductInfo
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using System.Collections;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// Insert ProductInfo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC 2PP Print
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
    ///         Session.InfoValue
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
    ///         ProductInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class RemoveProductInfoByTypeAndValue : BaseActivity
	{
        /// <summary>
        ///  InfoType
        /// </summary>
        public static DependencyProperty InfoTypeProperty = DependencyProperty.Register("InfoType", typeof(string), typeof(RemoveProductInfoByTypeAndValue));

        /// <summary>
        /// InfoType
        /// </summary>
        [DescriptionAttribute("InfoTypeProperty")]
        [CategoryAttribute("InArguments of RemoveProductInfoByTypeAndValue")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string InfoType
        {
            get
            {
                return ((string)(base.GetValue(InfoTypeProperty)));
            }
            set
            {
                base.SetValue(InfoTypeProperty, value);
            }
        }

        /// <summary>
        /// InfoValue: if doesn't set any value in workflow for this arguments,this function will
        ///                 get InfoValue from Session.SessionKeys.InfoValue
        /// </summary>
        public static DependencyProperty InfoValueSessionKeyProperty = DependencyProperty.Register("InfoValueSessionKey", typeof(string), typeof(RemoveProductInfoByTypeAndValue));

        /// <summary>
        /// InfoValue
        /// </summary>
        [DescriptionAttribute("InfoValueSessionKeyProperty")]
        [CategoryAttribute("InArguments of WriteProductInfo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string InfoValueSessionKey
        {
            get
            {
                return ((string)(base.GetValue(InfoValueSessionKeyProperty)));
            }
            set
            {
                base.SetValue(InfoValueSessionKeyProperty, value);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public RemoveProductInfoByTypeAndValue()
		{
			InitializeComponent();
		}

        /// <summary>
        /// write ProductInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string infoValue = "";
            if (string.IsNullOrEmpty(InfoValueSessionKey))
            {
                infoValue = CurrentSession.GetValue(Session.SessionKeys.InfoValue) as string;
            }
            else
            {
                infoValue = CurrentSession.GetValue(InfoValueSessionKey) as string;
            }

            if (infoValue.Trim() != "")
            {
                IList<string> itemTypes = new List<string>();
                itemTypes.Add(InfoType);
                var myRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<ProductInfo> lst = myRepository.GetProductInfoListByKeyAndValue(InfoType, infoValue);
                foreach (ProductInfo pi in lst)
                {
                    myRepository.BackUpProductInfoDefered(CurrentSession.UnitOfWork, pi.ProductID, this.Editor);
                    myRepository.RemoveProductInfosByTypeDefered(CurrentSession.UnitOfWork, pi.ProductID, itemTypes);
                }
            }
            
            return base.DoExecute(executionContext);
        }
	}
}
