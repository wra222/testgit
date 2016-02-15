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
    ///  CollectProduct 
   /// </summary>
    public partial class CollectProduct: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CollectProduct()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CollectProduct), new PropertyMetadata(true));

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
        /// Get MultiProductInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();           
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProduct prod = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            IList<IProduct> prodList =(IList<IProduct>)session.GetValue(Session.SessionKeys.ProdList);
            if (prodList == null)
            {
                prodList = new List<IProduct>();
                session.AddValue(Session.SessionKeys.ProdList, prodList);

            }
            else if (prodList.Any(x => x.CUSTSN == prod.CUSTSN))
            {
                FisException e = new FisException("CHK020", new string[] { prod.CUSTSN });
                e.stopWF = IsStopWF;
                throw e;

            }

             prodList.Add(prod);
             return base.DoExecute(executionContext);
        }     
       
        
    }
}
