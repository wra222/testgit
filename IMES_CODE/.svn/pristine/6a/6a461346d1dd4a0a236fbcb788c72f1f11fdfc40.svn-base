// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的ProductID,获取Product对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的ProductID,获取Product对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Product为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据ProductID，调用IProductRepository的Find方法，获取Product对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：SFC002
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class GetProduct : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetProduct()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.InfoFormat("GetProductActivity: Key: {0}", this.Key);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct=null;
            switch (InputType)
            {
                case InputTypeEnum.ProductID:
                    currentProduct = productRepository.Find(this.Key);
                    break;
                case InputTypeEnum.CustSN:
                    currentProduct = productRepository.GetProductByCustomSn(this.Key);
                    break;
                case InputTypeEnum.ProductIDOrCustSN:
                    currentProduct = productRepository.GetProductByIdOrSn(this.Key);
                    break;
                default:
                    currentProduct = productRepository.Find(this.Key);
                    break;
            }
            
            if (currentProduct == null)
            {
                List<string> errpara = new List<string>();

                errpara.Add(this.Key);
                if (string.IsNullOrEmpty(NotExistException))
                {
                    NotExistException = "SFC002";
                }
                throw new FisException(NotExistException, errpara);
            }

            if ("Y".Equals(this.AddObjectWithSessionKey))
            {
                CurrentSession.AddValue(this.Key, currentProduct);
            }

            CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(string), typeof(GetProduct));

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of GetProduct")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string NotExistException
        {
            get
            {
                return ((string)(base.GetValue(GetProduct.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetProduct.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(GetProduct));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of GetProduct")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(GetProduct.InputTypeProperty)));
            }
            set
            {
                base.SetValue(GetProduct.InputTypeProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 输入的是Session.ProductID
            /// </summary>
            ProductID = 0,

            /// <summary>
            /// 输入的是Session.CustSN
            /// </summary>
            CustSN = 1,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSN
            /// </summary>
            ProductIDOrCustSN = 2,
        }

        /// <summary>
        /// AddObjectWithSessionKey Property
        /// </summary>
        public static DependencyProperty AddObjectWithSessionKeyProperty = DependencyProperty.Register("AddObjectWithSessionKey", typeof(string), typeof(GetProduct));

        /// <summary>
        /// AddObjectWithSessionKey Property
        /// </summary>
        [DescriptionAttribute("AddObjectWithSessionKey")]
        [CategoryAttribute("AddObjectWithSessionKey Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AddObjectWithSessionKey
        {
            get
            {
                return ((string)(base.GetValue(GetProduct.AddObjectWithSessionKeyProperty)));
            }
            set
            {
                base.SetValue(GetProduct.AddObjectWithSessionKeyProperty, value);
            }
        }
    }
}
