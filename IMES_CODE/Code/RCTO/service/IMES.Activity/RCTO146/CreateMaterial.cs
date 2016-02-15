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
    /// CreateMaterial
    /// </summary>
    public partial class CreateMaterial : BaseActivity
    {
        ///<summary>
        ///CreateMaterial
        ///</summary>
        public CreateMaterial()
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
            string modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            string materialType = (string)CurrentSession.GetValue(Session.SessionKeys.MaterialType);
            if (materialType == null)
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialType });
            }

            string shipMode = (string)CurrentSession.GetValue(Session.SessionKeys.ShipMode) ?? "";
            string materialStage = (string)CurrentSession.GetValue("MaterialStage") ?? "";
            if (IsSingleCT)
            {
                string materialCT =(string)CurrentSession.GetValue(Session.SessionKeys.MaterialCT);
                if (materialCT==null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCT });
                }
                Material material = new Material();

                material.MaterialCT = materialCT;
                material.MaterialType = materialType;
                material.Model = modelName;
                material.Line = this.Line;
                material.DeliveryNo = deliveryNo;
                material.PalletNo = palletNo;
                material.CartonSN = cartonSN;
                material.PreStatus = "";
                material.Status = this.Station;
                material.ShipMode = shipMode;
                material.Editor = this.Editor;
                material.Cdt = DateTime.Now;
                material.Udt = DateTime.Now;             
                materialRep.Add(material, CurrentSession.UnitOfWork);
                CurrentSession.AddValue(Session.SessionKeys.Material, material);
            }
            else
            {
                IList<string> materialCTList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialCTList);
                if (materialCTList == null || materialCTList.Count == 0)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCTList });
                }
                materialRep.AddMultiMaterialCTDefered(CurrentSession.UnitOfWork, materialCTList,
                                                                             materialType,"",materialStage,this.Line,"",this.Station,
                                                                             modelName,deliveryNo,palletNo,cartonSN,
                                                                             "",shipMode,this.Editor);
             
            }      

           
            return base.DoExecute(executionContext);
        }
      

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty IsSingleCTProperty = DependencyProperty.Register("IsSingleCT", typeof(bool), typeof(CreateMaterial), new PropertyMetadata(true));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("IsSingleCT")]
        [CategoryAttribute("InArugment Of CreateMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingleCT
        {
            get
            {
                return ((bool)(base.GetValue(CreateMaterial.IsSingleCTProperty)));
            }
            set
            {
                base.SetValue(CreateMaterial.IsSingleCTProperty, value);
            }
        }

       
    }
}
