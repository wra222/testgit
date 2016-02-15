/* INVENTEC corporation (c)2011 all rights reserved. 
 * Description: MoveIn FloorAreaLoc 
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
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.Common;

namespace IMES.Activity
{
   /// <summary>
    ///  CheckScanedProductList 
   /// </summary>
    public partial class CheckScanedProductList: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckScanedProductList()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckScanedProductList), new PropertyMetadata(true));

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("InArguments Of SFC")]
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
        ///  
        /// </summary>
        public static DependencyProperty ScanListSessionNameProperty = DependencyProperty.Register("ScanListSessionName", typeof(ScanListSessionypeEnum), typeof(CheckScanedProductList), new PropertyMetadata(ScanListSessionypeEnum.NewScanedProductIDList));

        /// <summary>
        ///  
        /// </summary>
        [DescriptionAttribute("ScanListSessionName")]
        [CategoryAttribute("InArguments Of ScanListSessionName")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ScanListSessionypeEnum ScanListSessionName
        {
            get
            {
                return ((ScanListSessionypeEnum)(base.GetValue(ScanListSessionNameProperty)));
            }
            set
            {
                base.SetValue(ScanListSessionNameProperty, value);
            }
        }

        /// <summary>
        /// Get MultiProductInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IList<IProduct> prodList = utl.IsNull<IList<IProduct>>(session, Session.SessionKeys.ProdList);
            IList<string> scannedSnList = utl.IsNull<IList<string>>(session, ScanListSessionName.ToString());

            if (prodList.Count != scannedSnList.Count)
            {
                throw new FisException("CHK1116", new string[] { });
            }
            var snList = prodList.Select(x =>x.ProId).Distinct().ToList();
            var diffList = scannedSnList.Except(snList);
            if (diffList.Count() > 0)
            {
                throw new FisException("CHK1115", new string[] { "scanned more " + string.Join(",", diffList.ToArray()) });
            }
            var lossList = snList.Except(scannedSnList);
            if (lossList.Count() > 0)
            {
                throw new FisException("CHK1115", new string[] { "scanned less " + string.Join(",", lossList.ToArray()) });
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// Get scan  list session name
        /// </summary>
        public enum ScanListSessionypeEnum
        {
            /// <summary>
            /// 输入的是Session.NewScanedProductIDList
            /// </summary>
            NewScanedProductIDList = 0,
            
            /// <summary>
            /// 输入的是Session.NewScanedProductCustSNList
            /// </summary>
            NewScanedProductCustSNList = 1,

            /// <summary>
            /// 输入的是Session.NewScanedProductLineList
            /// </summary>
            NewScanedProductLineList = 3
        }
        
    }
}
