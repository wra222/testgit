// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  保存当站刷的料，更新ProductPart or PCBPart or PizzaPart表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Yuan XiaoWei                 create
// Known issues:
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 保存当站刷的料，更新ProductPart or PCBPart or PizzaPart表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有检料站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取PartOwner对象
    ///         2.从SessionBom中获取当前刷的料
    ///         3.调用PartOwner对象的BindPart方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product,Session.MB,Session.Pizza
    ///         Session.SessionBOM
    ///         
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
    ///         更新Product_Part,or PCB_Part or Pizza_Part depends on BindTo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              IProduct
    ///              ProductPart
    /// </para> 
    /// </remarks>
    public partial class BindParts : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public BindParts()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从Session中获取当前新刷的料,绑定到ProductParts
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IFlatBOM bom = (IFlatBOM)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            if ((bom != null) && (bom.BomItems != null))
            {
                foreach (var bomItem in bom.BomItems)
                {
                    BindPartsToPartOwner(bomItem, BindTo);
                }
            }

            return base.DoExecute(executionContext);
        }

        private void BindPartsToPartOwner(IFlatBOMItem bomItem, BindToEnum bindTo)
        {
            switch (bindTo)
            {
                case BindToEnum.PCB:
                    IMB CurrentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
                    bomItem.Save(CurrentMB, Station, Editor, CurrentSession.Key);
                    IMBRepository rep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    rep.Update(CurrentMB, CurrentSession.UnitOfWork);
                    break;
                case BindToEnum.Pizza:
                    Pizza pizzaPartOwner = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
                    if (pizzaPartOwner == null)
                    {
                        Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                        pizzaPartOwner = newProduct.PizzaObj;
                    }
                    bomItem.Save(pizzaPartOwner, Station, Editor, CurrentSession.Key);
                    IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    repPizza.Update(pizzaPartOwner, CurrentSession.UnitOfWork);
                    break;
                case BindToEnum.Product:
                    Product productPartOwner = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                    bomItem.Save(productPartOwner, Station, Editor, CurrentSession.Key);
                    IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    repProduct.Update(productPartOwner, CurrentSession.UnitOfWork);
                    break;
            }

        }

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        public static DependencyProperty BindToProperty = DependencyProperty.Register("BindTo", typeof(BindToEnum), typeof(BindParts));

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        [DescriptionAttribute("BindTo")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public BindToEnum BindTo
        {
            get
            {
                return ((BindToEnum)(base.GetValue(BindParts.BindToProperty)));
            }
            set
            {
                base.SetValue(BindParts.BindToProperty, value);
            }
        }

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        public enum BindToEnum
        {
            /// <summary>
            /// 绑定到ProductPart
            /// </summary>
            Product = 1,

            /// <summary>
            /// 绑定到PCBPart
            /// </summary>
            PCB = 2,

            /// <summary>
            /// 绑定到PizzaPart
            /// </summary>
            Pizza = 4
        }
    }
}
