/* INVENTEC corporation (c)2011 all rights reserved. 
 * Description: 檢查QCStatus記錄
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2014-10-04   Vincent                        Create
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.Common;
namespace IMES.Activity
{
   /// <summary>
    ///  檢查QCStatus記錄
   /// </summary>
    public partial class CheckQCStatus : BaseActivity
    {
        private static string postfixAttrName = "_QCStatus";
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckQCStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 檢查QCStatus寫入記錄
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            string attrName = this.QCType.ToString() + postfixAttrName;
            if (!string.IsNullOrEmpty(ProductAttrName))
            {
                attrName = ProductAttrName;
            }

            string attrValue = product.GetAttributeValue(attrName);

            string[] allowStatus = this.QCStatusList.Split(new char[] { ',', '~' });

            if (!string.IsNullOrEmpty(attrValue))
            {
                if (allowStatus.Contains(attrValue))
                {
                    session.AddValue(ExtendSession.SessionKeys.WarningMsg, "此機器為" + this.QCType + " QCStatus:" + attrValue);
                }
                else
                {
                    if (this.NotMatchStatusThrowError)
                    {
                        throw new FisException("CQCHK50029", new List<string> { this.QCType.ToString(), attrValue ?? "" });
                    }
                    else
                    {
                        session.AddValue(ExtendSession.SessionKeys.WarningMsg, "此機器為" + this.QCType + " QCStatus:" + attrValue);
                    }

                }
            }
            else if (HasQC)
            {
                if (this.NotMatchStatusThrowError)
                {
                    throw new FisException("CQCHK50029", new List<string> { this.QCType.ToString(), attrValue ?? "" });
                }
                else
                {
                    session.AddValue(ExtendSession.SessionKeys.WarningMsg, "此機器為" + this.QCType + " QCStatus:" + attrValue);
                }
            }
            return base.DoExecute(executionContext);
        }
        
        /// <summary>
        /// 状态值
        /// </summary>
        public static DependencyProperty QCStatusListProperty = DependencyProperty.Register("QCStatusList", typeof(string), typeof(CheckQCStatus), new PropertyMetadata("1,9"));

        /// <summary>
        /// 状态值
        /// </summary>
        [DescriptionAttribute("QCStatusList")]
        [CategoryAttribute("QCStatusList Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string QCStatusList
        {
            get
            {
                return ((string)(base.GetValue(QCStatusListProperty)));
            }
            set
            {
                base.SetValue(QCStatusListProperty, value);
            }
        }      


       

        /// <summary>
        /// 类型值
        /// </summary>
        public static DependencyProperty QCTypeProperty = DependencyProperty.Register("QCType", typeof(QCTypeEnum), typeof(CheckQCStatus), new PropertyMetadata(QCTypeEnum.PAQC));

        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("QCType")]
        [CategoryAttribute("QCType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public QCTypeEnum QCType
        {
            get
            {
                return ((QCTypeEnum)(base.GetValue(QCTypeProperty)));
            }
            set
            {
                base.SetValue(QCTypeProperty, value);
            }
        }

        /// <summary>
        /// 类型值 Product Attr Name
        /// </summary>
        public static DependencyProperty ProductAttrNameProperty = DependencyProperty.Register("ProductAttrName", typeof(string), typeof(CheckQCStatus), new PropertyMetadata(""));


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
                return ((string)(base.GetValue(ProductAttrNameProperty)));
            }
            set
            {
                base.SetValue(ProductAttrNameProperty, value);
            }
        }
		 
        /// <summary>
        /// throw error message
        /// </summary>
        public static DependencyProperty NotMatchStatusThrowErrorProperty = DependencyProperty.Register("NotMatchStatusThrowError", typeof(bool), typeof(CheckQCStatus), new PropertyMetadata(true));
        /// <summary>
        ///  Exception時要停止workflow
        /// </summary>
        [DescriptionAttribute("NotMatchStatusThrowError")]
        [CategoryAttribute("NotMatchStatusThrowError")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotMatchStatusThrowError
        {
            get
            {
                return ((bool)(base.GetValue(NotMatchStatusThrowErrorProperty)));
            }
            set
            {
                base.SetValue(NotMatchStatusThrowErrorProperty, value);
            }
        }


        /// <summary>
        /// throw error message
        /// </summary>
        public static DependencyProperty HasQCProperty = DependencyProperty.Register("HasQC", typeof(bool), typeof(CheckQCStatus), new PropertyMetadata(true));
        /// <summary>
        ///  Exception時要停止workflow
        /// </summary>
        [DescriptionAttribute("HasQC")]
        [CategoryAttribute("HasQC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool HasQC
        {
            get
            {
                return ((bool)(base.GetValue(HasQCProperty)));
            }
            set
            {
                base.SetValue(HasQCProperty, value);
            }
        }
        
    }
}
