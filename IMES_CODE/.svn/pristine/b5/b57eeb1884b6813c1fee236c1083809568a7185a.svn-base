// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Insert PCBInfo
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============

// Known issues:
using System;
using System.Linq;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
   /// <summary>
    /// WriteAndGetDeliveryAttribute
   /// </summary>
    public partial class WriteAndGetDeliveryAttribute : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public WriteAndGetDeliveryAttribute()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Attribute Name
        /// </summary>
        public static DependencyProperty AtttrNameProperty = DependencyProperty.Register("AtttrName", typeof(string), typeof(WriteAndGetDeliveryAttribute),new PropertyMetadata("PrintedQty"));

        /// <summary>
        /// InfoType of PCBInfo
        /// </summary>
        [DescriptionAttribute("AtttrName Property")]
        [CategoryAttribute("InArguments Of WriteAndGetDeliveryAttribute")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string AtttrName
        {
            get
            {
                return ((string)(base.GetValue(AtttrNameProperty)));
            }
            set
            {
                base.SetValue(AtttrNameProperty, value);
            }
        }

        /// <summary>
        ///  用那个sesionkey来获取InfoValue的值,不填默认从Session.SessionKeys.InfoValue里获取
        /// </summary>
        public static DependencyProperty AttrValueSessionKeyProperty = DependencyProperty.Register("AttrValueSessionKey", typeof(string), typeof(WriteAndGetDeliveryAttribute),new PropertyMetadata("PrintedQty"));

        /// <summary>
        /// 用那个sesionkey来获取InfoValue的值,不填默认从Session.SessionKeys.InfoValue里获取
        /// </summary>
        [DescriptionAttribute("AttrValueSessionKey Property")]
        [CategoryAttribute("WriteAndGetDeliveryAttribute")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string AttrValueSessionKey
        {
            get
            {
                return ((string)(base.GetValue(AttrValueSessionKeyProperty)));
            }
            set
            {
                base.SetValue(AttrValueSessionKeyProperty, value);
            }
        }

        ///<summary>
        /// GetAttribute flag
        ///</summary>
        public static DependencyProperty IsGetAttributeProperty = DependencyProperty.Register("IsGetAttribute", typeof(bool), typeof(WriteAndGetDeliveryAttribute), new PropertyMetadata(false));

        ///<summary>
        /// GetAttribute flag
        ///</summary>
        [DescriptionAttribute("IsGetAttribute")]
        [CategoryAttribute("IsGetAttribute Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsGetAttribute
        {
            get
            {
                return ((bool)(base.GetValue(IsGetAttributeProperty)));
            }
            set
            {
                base.SetValue(IsGetAttributeProperty, value);
            }
        }

        /// <summary>
        /// Get DeliveryAttr
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>(); 
            string dnNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            Delivery dn = null;
            if (string.IsNullOrEmpty(dnNo))
            {
                dn = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
                if (dn != null)
                {
                    dnNo = dn.DeliveryNo;
                }
            }
            if (dnNo == null)
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.DeliveryNo });
            }
            if (IsGetAttribute)
            {
                DeliveryAttrInfo attrInfo= new DeliveryAttrInfo() {
                     attrName = this.AtttrName,
                     deliveryNo = dnNo
                };
               IList<DeliveryAttrInfo> attrInfoList=  dnRep.GetDeliveryAttr(attrInfo);
               if (attrInfoList == null || attrInfoList.Count == 0)
               {
                   CurrentSession.AddValue(this.AttrValueSessionKey, null);
               }
               else 
               {
                   CurrentSession.AddValue(this.AttrValueSessionKey, attrInfoList[0].attrValue);
               }
            }
            else
            {
                string attrValue =(string)CurrentSession.GetValue(this.AttrValueSessionKey);
                if (attrValue != null)
                {
                    dnRep.UpdateAndInsertDeliveryAttrDefered(CurrentSession.UnitOfWork, dnNo, this.AtttrName, attrValue, "", this.Editor);
                }
            }        

            return base.DoExecute(executionContext);
        }

       
    }
}
