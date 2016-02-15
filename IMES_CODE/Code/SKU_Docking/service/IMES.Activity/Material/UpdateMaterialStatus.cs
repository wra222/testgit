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
    public partial class UpdateMaterialStatus : BaseActivity
    {
        ///<summary>
        ///</summary>
        public UpdateMaterialStatus()
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
           

            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
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

            string cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.CartonSN);

            if (IsSingleCT)
            {
                string materialCT =(string)CurrentSession.GetValue(Session.SessionKeys.MaterialCT);
                if (materialCT==null)
                {
                }
                Material material = materialRep.Find(materialCT);

                if (material == null)
                {
                }
                if (!string.IsNullOrEmpty(deliveryNo))
                {
                    material.DeliveryNo = deliveryNo;
                }
                if (!string.IsNullOrEmpty(palletNo))
                {
                    material.PalletNo = palletNo;
                }

                if (!string.IsNullOrEmpty(cartonSN))
                {
                    material.CartonSN = cartonSN;
                }

                material.PreStatus = material.Status;
                material.Status = this.Station;
                material.Editor = this.Editor;
                material.Udt = DateTime.Now;
                materialRep.Update(material, CurrentSession.UnitOfWork);
            }
            else
            {
                IList<string> materialCTList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialCTList);
                if (materialCTList == null || materialCTList.Count == 0)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCTList });
                }
                Material material = materialRep.Find(materialCTList[0]);
                  

                materialRep.UpdateDnPalletbyMultiCTDefered(CurrentSession.UnitOfWork,
                                                                                        materialCTList,
                                                                                        deliveryNo,
                                                                                        palletNo,
                                                                                        cartonSN,
                                                                                        material==null?"": material.Status,
                                                                                        this.Station,
                                                                                        this.Line,
                                                                                        this.Editor);
                                                                                         

             
            }
          

           
            return base.DoExecute(executionContext);
        }
      

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty IsSingleCTProperty = DependencyProperty.Register("IsSingleCT", typeof(bool), typeof(UpdateMaterialStatus), new PropertyMetadata(true));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("IsSingleCT")]
        [CategoryAttribute("InArugment Of UpdateMaterialStatus")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingleCT
        {
            get
            {
                return ((bool)(base.GetValue(UpdateMaterialStatus.IsSingleCTProperty)));
            }
            set
            {
                base.SetValue(UpdateMaterialStatus.IsSingleCTProperty, value);
            }
        }

       
    }
}
