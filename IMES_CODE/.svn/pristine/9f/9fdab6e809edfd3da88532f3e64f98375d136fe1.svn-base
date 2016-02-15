// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{


    /// <summary>
    /// Update Packing Data With Edits
    /// </summary>
  
    public partial class GetCarton : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public GetCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 找不到Carton 報錯
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(bool), typeof(GetCarton), new PropertyMetadata(true));

        /// <summary>
        /// 找不到Carton Model 報錯
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of CheckBSamModel")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotExistException
        {
            get
            {
                return ((bool)(base.GetValue(GetCarton.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetCarton.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {  
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (currentProduct != null)
            {
                Carton carton = cartonRep.Find(currentProduct.CartonSN);
                if (carton == null && NotExistException)
                {

                    throw new FisException("CHK985", new string[] { currentProduct.CartonSN });
                }

                if (carton != null)
                {
                    CurrentSession.AddValue(Session.SessionKeys.Carton, carton);
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, prodRep.GetProductListByCartonNo(currentProduct.CartonSN));
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodRep.GetProductIDListByCarton(currentProduct.CartonSN));
                }
            }
            else if (CurrentSession.GetValue(Session.SessionKeys.boxId) !=null)
            {
                string boxId = (string)CurrentSession.GetValue(Session.SessionKeys.boxId);
                if (!string.IsNullOrEmpty(boxId))
                {
                    Carton carton = cartonRep.GetCartonByBoxId(boxId);
                    if (carton == null && NotExistException)
                    {
                      throw new FisException("CHK985", new string[] { currentProduct.CartonSN });
                     }

                    CurrentSession.AddValue(ExtendSession.SessionKeys.CartonSN, carton.CartonSN);
                    CurrentSession.AddValue(Session.SessionKeys.Carton, carton);
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, prodRep.GetProductListByCartonNo(carton.CartonSN));
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodRep.GetProductIDListByCarton(carton.CartonSN));
                }
                else if (NotExistException)
                {
                    throw new FisException("CHK985", new string[] { "" });
                }
            }
            else if (CurrentSession.GetValue(ExtendSession.SessionKeys.CartonSN) != null)
            {
                string cartonSN=(string) CurrentSession.GetValue(ExtendSession.SessionKeys.CartonSN);
                if (!string.IsNullOrEmpty(cartonSN))
                {
                    Carton carton = cartonRep.Find(cartonSN);
                     if (carton == null && NotExistException)
                     {

                       throw new FisException("CHK985", new string[] { currentProduct.CartonSN });
                     }
                    CurrentSession.AddValue(Session.SessionKeys.Carton, carton);
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, prodRep.GetProductListByCartonNo(carton.CartonSN));
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodRep.GetProductIDListByCarton(carton.CartonSN));
                }
                else if (NotExistException)
                {
                    throw new FisException("CHK985", new string[] { "" });
                }
            }else if(NotExistException)
            {
                 throw new FisException("CHK985", new string[] {"" });
            }

            return base.DoExecute(executionContext);
          
        }
	}
}
