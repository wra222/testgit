// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PAK.DN;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateMaterialBoxStatus : BaseActivity
    {
        ///<summary>
        ///</summary>
        public UpdateMaterialBoxStatus()
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
            var materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
            string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            Delivery dn = null;
            if (string.IsNullOrEmpty(deliveryNo))
            {
                dn = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
                if (dn != null)
                {
                    deliveryNo = dn.DeliveryNo;
                }
            }

            string palletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);  

            if (IsSingle)
            {
                MaterialBox materialBox = (MaterialBox)CurrentSession.GetValue(Session.SessionKeys.MaterialBox);
                if (materialBox == null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialBox });
                }
               
                materialBox.DeliveryNo = deliveryNo;
                materialBox.PalletNo = palletNo;
                materialBox.Status = this.Station;
                materialBox.Editor = this.Editor;
                materialBoxRep.Update(materialBox, CurrentSession.UnitOfWork);                
            }
            else
            {
                IList<string> materialBoxIdList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialBoxIdList);
                if (materialBoxIdList == null || materialBoxIdList.Count == 0)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialBoxIdList });
                }                         

                materialBoxRep.UpdateDnPalletStatusbyMultiBoxIdDefered(CurrentSession.UnitOfWork,
                                                                                                        materialBoxIdList,
                                                                                                       deliveryNo,
                                                                                                        "",
                                                                                                        palletNo,
                                                                                                        this.Station,
                                                                                                        this.Line,
                                                                                                        this.Editor);
                                                                                       

               
            }
          

           
            return base.DoExecute(executionContext);
        }

       

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(UpdateMaterialBoxStatus), new PropertyMetadata(true));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("InArugment Of UpdateMaterialBoxStatus")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(UpdateMaterialBoxStatus.IsSingleProperty)));
            }
            set
            {
                base.SetValue(UpdateMaterialBoxStatus.IsSingleProperty, value);
            }
        }

       
    }
}
