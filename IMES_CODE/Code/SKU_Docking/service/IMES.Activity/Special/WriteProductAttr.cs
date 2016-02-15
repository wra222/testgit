// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Update Product Attribute
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-28   Kerwin                       create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;



namespace IMES.Activity
{
    /// <summary>
    /// Update Product Attribute,Write ProucutAttrLog
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UI None EPIA to EPIA
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.如果ProductAttr 中不存在AttrName = @AttributeName 的记录，则插入新记录(AttrName = @AttributeName, AttrValue=@AttributeValue)
    ///           否则Update AttrValue = @AttributeValue – 注意Update 时，记录ProductAttrLog (Station 取当时ProductStatus.Station)
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK155
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
    ///         ProductAttr
    ///         ProductAttrLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         ProductAttribute
    /// </para> 
    /// </remarks>
    public partial class WriteProductAttr : BaseActivity
	{
        /// <summary>
        ///  AttributeName
        /// </summary>
        public static DependencyProperty AttributeNameProperty = DependencyProperty.Register("AttributeName", typeof(string), typeof(WriteProductAttr));

        /// <summary>
        /// AttributeName
        /// </summary>
        [DescriptionAttribute("AttributeNameProperty")]
        [CategoryAttribute("InArguments of WriteProductAttr")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string AttributeName
        {
            get
            {
                return ((string)(base.GetValue(AttributeNameProperty)));
            }
            set
            {
                base.SetValue(AttributeNameProperty, value);
            }
        }

        /// <summary>
        /// AttributeValue: if doesn't set any value in workflow for this arguments,this function will
        ///                 get attributevalue from Session.SessionKeys.AttributeValue
        /// </summary>
        public static DependencyProperty AttributeValueProperty = DependencyProperty.Register("AttributeValue", typeof(string), typeof(WriteProductAttr));

        /// <summary>
        /// AttributeValue
        /// </summary>
        [DescriptionAttribute("AttributeValueProperty")]
        [CategoryAttribute("InArguments of WriteProductAttr")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string AttributeValue
        {
            get
            {
                return ((string)(base.GetValue(AttributeValueProperty)));
            }
            set
            {
                base.SetValue(AttributeValueProperty, value);
            }
        }

        /// <summary>
        ///  Descr of ProductAttrLog
        /// </summary>
        public static DependencyProperty DescrOfProductAttrLogProperty = DependencyProperty.Register("DescrOfProductAttrLog", typeof(string), typeof(WriteProductAttr));


        /// <summary>
        /// Descr of ProductAttrLog
        /// </summary>
        [DescriptionAttribute("DescrOfProductAttrLog")]
        [CategoryAttribute("InArguments of WriteProductAttr")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string DescrOfProductAttrLog
        {
            get
            {
                return ((string)(base.GetValue(DescrOfProductAttrLogProperty)));
            }
            set
            {
                base.SetValue(DescrOfProductAttrLogProperty, value);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public WriteProductAttr()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Update Product Attribute,Write ProucutAttrLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;

            string attributeValueStr = AttributeValue;

            if(string.IsNullOrEmpty(AttributeValue))
            {
                 attributeValueStr = CurrentSession.GetValue(Session.SessionKeys.AttributeValue) as string;

            }
            CurrentProduct.SetAttributeValue(AttributeName, attributeValueStr, CurrentSession.Editor, CurrentProduct.Status.StationId, DescrOfProductAttrLog);
            

            var ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductRepository.Update(CurrentProduct,CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
	}
}
