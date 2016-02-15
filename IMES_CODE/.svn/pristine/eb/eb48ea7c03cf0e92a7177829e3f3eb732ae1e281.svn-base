// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-03   207003                     create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using System.Globalization;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GenSequenceNum : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public GenSequenceNum()
        {
            InitializeComponent();
        }
        #region disable code
        ///// <summary>
        ///// 绑定到 NumType
        ///// </summary>
        //public static DependencyProperty NumTypeProperty = DependencyProperty.Register("NumType", typeof(string), typeof(GenSequenceNum), new PropertyMetadata("CUSTSN") );

        ///// <summary>
        ///// 绑定到 NumType
        ///// </summary>
        //[DescriptionAttribute("NumType")]
        //[CategoryAttribute("NumType Arugment")]
        //[BrowsableAttribute(true)]
        //[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        //public string NumType
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(NumTypeProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(NumTypeProperty, value);
        //    }
        //}
        #endregion
        /// <summary>
        /// 绑定到 StoreSessionName
        /// </summary>
        public static DependencyProperty StoreSessionNameProperty = DependencyProperty.Register("StoreSessionName", typeof(string), typeof(GenSequenceNum), new PropertyMetadata("Sequence"));

        /// <summary>
        /// 绑定到 StoreSessionName
        /// </summary>
        [DescriptionAttribute("StoreSessionName")]
        [CategoryAttribute("StoreSessionName Arugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreSessionName
        {
            get
            {
                return ((string)(base.GetValue(StoreSessionNameProperty)));
            }
            set
            {
                base.SetValue(StoreSessionNameProperty, value);
            }
        }

        /// <summary>
        /// 绑定到 StoreInfoType
        /// </summary>
        public static DependencyProperty StoreInfoTypeProperty = DependencyProperty.Register("StoreInfoType", typeof(string), typeof(GenSequenceNum));

        /// <summary>
        /// 绑定到 StoreInfoType
        /// </summary>
        [DescriptionAttribute("StoreInfoType")]
        [CategoryAttribute("StoreInfoType Arugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreInfoType
        {
            get
            {
                return ((string)(base.GetValue(StoreInfoTypeProperty)));
            }
            set
            {
                base.SetValue(StoreInfoTypeProperty, value);
            }
        }

        /// <summary>
        /// 绑定到 SpecType
        /// </summary>
        public static DependencyProperty SpecTypeProperty = DependencyProperty.Register("SpecType", typeof(string), typeof(GenSequenceNum), new PropertyMetadata("SNRule"));

        /// <summary>
        /// 绑定到SpecType
        /// </summary>
        [DescriptionAttribute("SpecType")]
        [CategoryAttribute("SpecType Arugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SpecType
        {
            get
            {
                return ((string)(base.GetValue(SpecTypeProperty)));
            }
            set
            {
                base.SetValue(SpecTypeProperty, value);
            }
        }

        /// <summary>
        /// 绑定到 SpecName
        /// </summary>
        public static DependencyProperty SpecNameProperty = DependencyProperty.Register("SpecName", typeof(string), typeof(GenSequenceNum), new PropertyMetadata("@Family"));

        /// <summary>
        /// 绑定到SpecName
        /// </summary>
        [DescriptionAttribute("SpecName")]
        [CategoryAttribute("SpecName Arugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SpecName
        {
            get
            {
                return ((string)(base.GetValue(SpecNameProperty)));
            }
            set
            {
                base.SetValue(SpecNameProperty, value);
            }
        }


      /// <summary>
      /// a)ConstValue.Name = @Family
        /// b)ConstValue.Value = ModelInfo@PN或是ModelInfo@Country~S~Date@YM~Number@0123456789,最小號碼,最大號碼
    ///  Date格式:
  ///          YM:年+月(1-9,A,B,C)
  ///          YW[0-6]:年+週別[每週開始week] 
        /// @Number:流水號的位置，流水號編碼規則描述在Description:0123456789,最小號碼,最大號碼 
   /// 若存在ConstValue.Type='ModelInfo@PN',使用ConstValue.Name=ModelInfo.InfoValue 抓對應的 ConstValue.Value 
   ///c)ConstValue.Description=0123456789~最小號碼~最大號碼 或 
   ///                                0123456789ABCDEF~最小號碼~最大號碼
      /// </summary>
      /// <param name="executionContext"></param>
      /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProduct currentProduct = null;
            string name = this.SpecName;
            string numType ="";
            if (name == "@Family")
            {
                currentProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
                name = currentProduct.Family;
            }
            else if (name == "@Model")
            {
                currentProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
                name = currentProduct.Model;
            }
            else
            {
                numType = this.SpecName;
                currentProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
            }

            string seqFormat = null;
            string seqDescr = null;
            IList<ConstValueInfo> valueList = utl.ConstValue(this.SpecType, name, out seqFormat, out seqDescr);
            string nextNum = utl.GenSN.GetNextSequence(currentProduct, this.Customer, string.IsNullOrEmpty(numType) ? seqDescr : numType,
                                                                       seqFormat);

            if (!string.IsNullOrEmpty(this.StoreInfoType) && currentProduct!=null)
            {               
               currentProduct.SetExtendedProperty(this.StoreInfoType, nextNum, this.Editor);
               IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
               prodRep.Update(currentProduct, session.UnitOfWork);                
            }

            if (!string.IsNullOrEmpty(StoreSessionName))
            {
                session.AddValue(this.StoreSessionName, nextNum);
            }
    
          return base.DoExecute(executionContext);
        }

       
        
    }

  
}
