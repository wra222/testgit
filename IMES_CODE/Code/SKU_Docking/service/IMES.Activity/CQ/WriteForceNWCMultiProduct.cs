// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新ProductStatus
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-02-15   Vincent                 create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WriteForceNWCMultiProduct : BaseActivity
    {
        
        /// <summary>
        /// constructor
        /// </summary>
        public WriteForceNWCMultiProduct()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", 
                                                                                                                                    typeof(bool),
                                                                                                                                    typeof(WriteForceNWCMultiProduct),
                                                                                                                                    new PropertyMetadata(false));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.ProdList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(WriteForceNWCMultiProduct.IsSingleProperty)));
            }
            set
            {
                base.SetValue(WriteForceNWCMultiProduct.IsSingleProperty, value);
            }
        } 

        /// <summary>
        /// Update ForceNWC
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> prodIDList = new List<string>();
            if (this.IsSingle)
            {
                IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (currentProduct == null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.Product });
                }
                prodIDList.Add(currentProduct.ProId);
            }
            else
            {
               prodIDList = CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) as IList<string>;
                if (prodIDList == null || prodIDList.Count == 0)
                {
                    IList<IProduct> productList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
                    if (productList == null || productList.Count == 0)
                    {
                        throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.ProdList + " and " + Session.SessionKeys.NewScanedProductIDList });
                    }
                    prodIDList = (from p in productList
                                  select p.ProId).ToList();                   
                }
                
            }

             string goToStation =(string) CurrentSession.GetValue("GoToStation");
            if(string.IsNullOrEmpty(goToStation))
            {
                throw new FisException("CQCHK0006", new string[] {"GoToStation"});
            }
            prodRep.UpdateForceNWCByProductIDDefered(CurrentSession.UnitOfWork, prodIDList, goToStation, this.Editor);

            
            return base.DoExecute(executionContext);
        }

       
    }
}
