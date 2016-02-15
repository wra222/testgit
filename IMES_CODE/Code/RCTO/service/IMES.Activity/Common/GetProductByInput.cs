// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的参数获取Product对象，并放到Session
// 参数可以是CustSN,或者Carton，Pallet
// 该Activity是为了不是以Product为主线的站，获取Product对象所
// 当获取不到时，抛出异常，根据使用的IsStopWF选项决定是否终止workflow            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的参数获取Product对象，并放到Session
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类
    /// <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景
    /// </para>
    /// <para>
    /// 实现逻辑 1.根据输入的参数，调用IProductRepository的不同Find方法，获取Product对象，添加到Session
    /// </para> 
    /// <para> 
    /// 异常类型  1.系统异常
    ///           2.业务异常：CHK079
    /// 
    /// </para> 
    /// <para>    
    /// 输入
    /// </para> 
    /// <para>    
    /// 中间变量
    ///  </para> 
    ///<para> 
    /// 输出   Session.Product
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
    public partial class GetProductByInput : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetProductByInput()
        {
            InitializeComponent();
        }


        /// <summary>
        ///  根据输入参数找不到Product时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(IsStopWFEnum), typeof(GetProductByInput));

        /// <summary>
        ///  根据输入参数找不到Product时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public IsStopWFEnum IsStopWF
        {
            get
            {
                return ((IsStopWFEnum)(base.GetValue(GetProductByInput.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(GetProductByInput.IsStopWFProperty, value);
            }
        }


        /// <summary>
        /// 输入的类型，共有两种CustSN，Carton
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(GetProductByInput));

        /// <summary>
        /// 输入的类型，共有两种CustSN，Carton
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(GetProductByInput.InputTypeProperty)));
            }
            set
            {
                base.SetValue(GetProductByInput.InputTypeProperty, value);
            }
        }

        /// <summary>
        /// 输入序号找不到Product报错信息的ErroCode
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(string), typeof(GetProductByInput));

        /// <summary>
        /// 输入序号找不到Product报错信息的ErroCode
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string NotExistException
        {
            get
            {
                return ((string)(base.GetValue(GetProductByInput.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetProductByInput.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            CurrentSession.AddValue(Session.SessionKeys.Product, null);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct = null;
            string inputNo = "";
            switch (InputType)
            {
                case InputTypeEnum.CustSN:
                    inputNo = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
                    currentProduct = productRepository.GetProductByCustomSn(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.GetProductByCustomSn(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.Carton:
                    inputNo = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);
                    List<string> productIDList = productRepository.GetProductIDListByCarton(inputNo);
                    if (productIDList == null || productIDList.Count == 0 && inputNo.Length > 1)
                    {
                        productIDList = productRepository.GetProductIDListByCarton(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    if (productIDList != null && productIDList.Count != 0)
                    {
                        currentProduct = productRepository.Find(productIDList[0]);
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSN:
                    inputNo = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSN);
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSN(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSN(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSNOrCarton:
                    inputNo = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSNOrCarton);
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrCarton(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrCarton(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSNOrPallet:
                    inputNo = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSNOrPallet);
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrPallet(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrPallet(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;

            }

            if (currentProduct == null)
            {
                if( string.IsNullOrEmpty(NotExistException))
                {
                    NotExistException = "CHK079";
                }
                FisException fe = new FisException(NotExistException, new string[] { inputNo });
                if (IsStopWF == IsStopWFEnum.NotStop)
                {
                    fe.stopWF = false;
                }
                throw fe;
            }
            CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            return base.DoExecute(executionContext);
        }



        /// <summary>
        /// 输入的类型，共有两种CustSN，Carton
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 输入的是Session.CustSN
            /// </summary>
            CustSN = 1,

            /// <summary>
            /// 输入的是Session.Carton
            /// </summary>
            Carton = 2,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSN
            /// For:Unit Label Print
            /// </summary>
            ProductIDOrCustSN = 4,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSNOrCarton
            /// </summary>
            ProductIDOrCustSNOrCarton = 8,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSNOrPallet
            /// </summary>
            ProductIDOrCustSNOrPallet = 16
        }


        /// <summary>
        /// 根据输入参数找不到Product时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public enum IsStopWFEnum
        {
            /// <summary>
            /// 停止WorkFlow
            /// </summary>
            Stop = 1,

            /// <summary>
            /// 不停止WorkFlow
            /// </summary>
            NotStop = 2
        }
    }
}
