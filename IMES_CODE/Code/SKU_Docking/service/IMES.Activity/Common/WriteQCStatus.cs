/* INVENTEC corporation (c)2011 all rights reserved. 
 * Description: 向QCStatus寫入記錄
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2011-12-01   Kerwin                        Create
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 向QCStatus寫入記錄
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      OQC Input,PAQC Input,PAQC Output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Insert QCStatus
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    ///         ProductQCStatus
    /// </para> 
    /// </remarks>
    public partial class WriteQCStatus : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WriteQCStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 向QCStatus寫入記錄
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            ProductQCStatus status = new ProductQCStatus(-1, product.ProId, Type, product.Status.Line, product.Model, Convert.ToString(Status, 16).ToUpper(), Editor, DateTime.Now, DateTime.Now);
            status.Remark = Remark;

            product.AddQCStatus(status);

            IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();


            ipr.Update(product, CurrentSession.UnitOfWork);

            //Add by Benson

            if (!string.IsNullOrEmpty(ProductAttrName))
            {
                product.SetAttributeValue(ProductAttrName, Convert.ToString(Status, 16).ToUpper(), Editor, Station, "");

            }
            else if (!string.IsNullOrEmpty(Type))
            {
                product.SetAttributeValue(Type+"_QCStatus", Convert.ToString(Status, 16).ToUpper(), Editor, Station, "");
               
            }


            //Add by Benson

            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// 状态值
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(int), typeof(WriteQCStatus));

        /// <summary>
        /// 状态值
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int Status
        {
            get
            {
                return ((int)(base.GetValue(WriteQCStatus.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteQCStatus.StatusProperty, value);
            }
        }

        /// <summary>
        /// 类型值
        /// </summary>
        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(WriteQCStatus));

        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("Type")]
        [CategoryAttribute("Type Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Type
        {
            get
            {
                return ((string)(base.GetValue(WriteQCStatus.TypeProperty)));
            }
            set
            {
                base.SetValue(WriteQCStatus.TypeProperty, value);
            }
        }

        /// <summary>
        /// 类型值
        /// </summary>
        public static DependencyProperty RemarkProperty = DependencyProperty.Register("Remark", typeof(string), typeof(WriteQCStatus));

        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("Remark")]
        [CategoryAttribute("Remark Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Remark
        {
            get
            {
                return ((string)(base.GetValue(WriteQCStatus.RemarkProperty)));
            }
            set
            {
                base.SetValue(WriteQCStatus.RemarkProperty, value);
            }
        }
		 //Add by Benson
        /// <summary>
        /// 类型值 Product Attr Name
        /// </summary>
        public static DependencyProperty ProductAttrNameProperty = DependencyProperty.Register("ProductAttrName", typeof(string), typeof(WriteQCStatus));


        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("ProductAttrName")]
        [CategoryAttribute("ProductAttrName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ProductAttrName
        {
            get
            {
                return ((string)(base.GetValue(WriteQCStatus.ProductAttrNameProperty)));
            }
            set
            {
                base.SetValue(WriteQCStatus.ProductAttrNameProperty, value);
            }
        }
        //Add by Benson
    }
}
