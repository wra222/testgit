// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo,InfoType,delete属于DeliveryNo的InfoType=@InfoType的所有ProductInfo
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                  create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据DeliveryNo,InfoType,delete属于DeliveryNo的InfoType=@InfoType的所有ProductInfo
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
    ///         1.从Session中获取DeliveryNo,调用ProductRepository的UnPackProductInfoByDeliveryNoDefered方法
    ///           delete ProductInfo from ProductInfo as I inner join Product as P ON I.ProductID = P.ProductID
    ///           where I.InfoType=@InfoType and P.DeliveryNo=@DeliveryNo 
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
    ///         InfoType
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
    ///         delete ProductInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class GetProductPartByPartType : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetProductPartByPartType()
		{
			InitializeComponent();
		}

        /// <summary>
        /// InfoType of ProductInfo to delete
        /// </summary>
        public static DependencyProperty PartTypeProperty = DependencyProperty.Register("PartType", typeof(string), typeof(GetProductPartByPartType));

        /// <summary>
        /// InfoType of ProductInfo to delete
        /// </summary>
        [DescriptionAttribute("PartType")]
        [CategoryAttribute("InArguments Of UnPackPartByProdId")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PartType
        {
            get
            {
                return ((string)(base.GetValue(GetProductPartByPartType.PartTypeProperty)));
            }
            set
            {
                base.SetValue(GetProductPartByPartType.PartTypeProperty, value);
            }
        }

        /// <summary>
        /// PartDescProperty of ProductInfo to delete
        /// </summary>
        public static DependencyProperty PartDescProperty = DependencyProperty.Register("PartDesc", typeof(string), typeof(GetProductPartByPartType));

        /// <summary>
        /// InfoType of ProductInfo to delete
        /// </summary>
        [DescriptionAttribute("PartDesc")]
        [CategoryAttribute("InArguments Of UnPackPartByProdId")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PartDesc
        {
            get
            {
                return ((string)(base.GetValue(GetProductPartByPartType.PartDescProperty)));
            }
            set
            {
                base.SetValue(GetProductPartByPartType.PartDescProperty, value);
            }
        }

        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product productPartOwner = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            //*
            IList<ProductPart> part = currentProductRepository.GetProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, PartType, PartDesc);
            bool isfind =false;
            if (part.Count > 0)
            {
                foreach (ProductPart part1 in part)
                {
                    if (part1.PartSn.Length > 0)
                    {
                        if (PartDesc == "Royalty")
                             CurrentSession.AddValue(Session.SessionKeys.COASN, part1.PartSn);
                        else
                             CurrentSession.AddValue(Session.SessionKeys.Pno, part1.PartSn);

                        isfind = true;
                        break;
                    }
                    //SetValue();
                }
            }
           
            CurrentSession.AddValue(Session.SessionKeys.HasDefect, isfind);
             //*/
            return base.DoExecute(executionContext);
        }
	}
}
