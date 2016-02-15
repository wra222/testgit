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
    /// CreateMaterialBox
    /// </summary>
    public partial class CreateMaterialBox : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CreateMaterialBox()
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
             string cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);
            string boxId = (string) CurrentSession.GetValue(Session.SessionKeys.MaterialBoxId);
            if (boxId == null)
            {

                if (cartonSN == null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialBoxId + " " + Session.SessionKeys.CartonSN });
                }

                boxId = cartonSN;
                CurrentSession.AddValue(Session.SessionKeys.MaterialBoxId, cartonSN);
            }

            string materialType = (string)CurrentSession.GetValue(Session.SessionKeys.MaterialType);
            if (materialType == null)
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialType });
            }
            string modelName =(string) CurrentSession.GetValue(Session.SessionKeys.ModelName); 
            int qty =(int)(CurrentSession.GetValue(Session.SessionKeys.Qty)??0);
          
            string shipMode = (string)CurrentSession.GetValue(Session.SessionKeys.ShipMode) ?? "";
       
            MaterialBox materialBox = new MaterialBox();            
            materialBox.BoxId = boxId;
            materialBox.DeliveryNo = deliveryNo;
            materialBox.PalletNo="";
            materialBox.Line = this.Line;
            materialBox.Model =modelName;
            materialBox.Qty =qty;           
            materialBox.MaterialType=materialType;
            materialBox.Status = this.Station;
            materialBox.ShipMode = shipMode;
            materialBox.CartonSN = cartonSN;
            materialBox.Editor = this.Editor;
            materialBoxRep.Add(materialBox, CurrentSession.UnitOfWork);  
            CurrentSession.AddValue(Session.SessionKeys.MaterialBox, materialBox);
                 

           
            return base.DoExecute(executionContext);
        }        
       
    }
}
