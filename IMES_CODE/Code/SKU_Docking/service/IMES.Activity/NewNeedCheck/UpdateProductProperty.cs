// INVENTEC corporation (c)2012 all rights reserved. 
// Description: 更新Product的一个属性
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-05   itc202017                    create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 用于更新Product的属性
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
    ///         1.从Session中获取Product，调用Product的Update方法
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新Product属性
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class UpdateProductProperty : BaseActivity
    {
        /// <summary>
        /// PropertyName Property
        /// </summary>
        public static DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(UpdateProductProperty));

        /// <summary>
        /// PropertyName Property
        /// </summary>
        [DescriptionAttribute("PropertyName")]
        [CategoryAttribute("PropertyName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PropertyName
        {
            get
            {
                return ((string)(base.GetValue(UpdateProductProperty.PropertyNameProperty)));
            }
            set
            {
                base.SetValue(UpdateProductProperty.PropertyNameProperty, value);
            }
        }

        /// <summary>
        /// PropertyValueKey Property
        /// </summary>
        public static DependencyProperty PropertyValueKeyProperty = DependencyProperty.Register("PropertyValueKey", typeof(string), typeof(UpdateProductProperty));

        /// <summary>
        /// PropertyValueKey Property
        /// </summary>
        [DescriptionAttribute("PropertyValueKey")]
        [CategoryAttribute("PropertyValueKey Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PropertyValueKey
        {
            get
            {
                return ((string)(base.GetValue(UpdateProductProperty.PropertyValueKeyProperty)));
            }
            set
            {
                base.SetValue(UpdateProductProperty.PropertyValueKeyProperty, value);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public UpdateProductProperty()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update Product Property
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var val = (string)CurrentSession.GetValue(PropertyValueKey);

            if (currentProduct != null && val != null)
            {
                currentProduct.SetProperty(PropertyName, val);

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }
}
