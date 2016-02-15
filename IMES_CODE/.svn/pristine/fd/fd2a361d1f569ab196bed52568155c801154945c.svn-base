// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Check Change PO Product Condition
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2015-01-21 Vincent             create
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// Check ChangePo Product
    /// </summary>
    public partial class CheckChangePo : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckChangePo()
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
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProduct prod = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            
            if (this.InputType == InputTypeEnum.SourceProduct)
            {
                //檢查機器的MO是否綁定PO，若無，則報錯
                //檢查機器是否綁定DN，若是，則報錯
                //Add SessionKey:SrcProduct
                if (string.IsNullOrEmpty(prod.MOObject.PoNo))
                {
                    throw new FisException("CQCHK1102", this.IsStopWF, new string[] { prod.ProId});
                }
                if (!string.IsNullOrEmpty(prod.DeliveryNo))
                {
                    throw new FisException("CQCHK1103", this.IsStopWF, new string[] { prod.ProId, prod.DeliveryNo });
                }
                session.AddValue(Session.SessionKeys.SrcProduct, prod);
            }
            else
            {
                //檢查是否Source Product 且Model 一致
                IProduct srcProd=utl.IsNull<IProduct>(session, Session.SessionKeys.SrcProduct);
                if (srcProd.Model != prod.Model)
                {
                    throw new FisException("CQCHK1106", this.IsStopWF, new string[] { prod.ProId, prod.Model,srcProd.Model });
                }
                //檢查機器的MO是否綁定PO，若是，則報錯
               // 檢查機器的當前站點是否為65站(MVS)前的站點，若否，則報錯。
                if (prod.IsBindedPo)
                {
                    throw new FisException("CQCHK1104", this.IsStopWF, new string[] { prod.ProId, prod.BindPoNo });
                }
                IList<ConstValueTypeInfo> stationList = utl.ConstValueType("StationBeforeMVS", null);
                if (!stationList.Any(x=> x.value==prod.Status.StationId))
                {
                    throw new FisException("CQCHK1105", this.IsStopWF, new string[] { prod.ProId, prod.Status.StationId });
                }
                session.AddValue(Session.SessionKeys.DestProduct, prod);
            }         
           
            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckChangePo), new PropertyMetadata(true));
        /// <summary>
        /// 禁用時要停止workflow 
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(IsStopWFProperty)));
            }
            set
            {
                base.SetValue(IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型::SourceProduct,DestProduct
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(CheckChangePo),new PropertyMetadata(InputTypeEnum.SourceProduct));

        /// <summary>
        /// 输入的类型:SourceProduct,DestProduct
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of CheckChangePo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(CheckChangePo.InputTypeProperty)));
            }
            set
            {
                base.SetValue(CheckChangePo.InputTypeProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:SourceProduct,DestProduct
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 來源Product
            /// </summary>
            SourceProduct = 0,

            /// <summary>
            ///  換成的Product
            ///  </summary>
            DestProduct = 1,

        }
        
    }
}
