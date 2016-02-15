// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.StandardWeight;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// CheckCombineCartonPalletFor146
    /// </summary>
    public partial class CheckCombineCartonPalletFor146 : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckCombineCartonPalletFor146()
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

            string deliveryNo = CurrentSession.GetValue(Session.SessionKeys.DeliveryNo) as string;
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            Delivery dn = dnRep.GetDelivery(deliveryNo);
            string model = dn.ModelName;

            IModelWeightRepository mvRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, IMES.FisObject.PAK.StandardWeight.ModelWeight>();
            IMES.FisObject.PAK.StandardWeight.ModelWeight currentModelWeight = mvRepository.Find(model);
            if (currentModelWeight == null)
            {
                // 此 Carton 的Model %1 無重量資料
                throw new FisException("CQCHK0032", new string[] { model });
            }

            string stationCheck = "R81";
            if ("F".Equals( this.Station.Substring(0,1)))
            {
                stationCheck = "F81";
            }

            string cartonSn = (string) CurrentSession.GetValue(Session.SessionKeys.CartonSN);
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            var materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
            var mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            string category = CurrentSession.GetValue(Session.SessionKeys.RCTO146Category) as string;
            if ("MBCT".Equals(category))
            {
                IList<RCTO146MBInfo> mbInfoList = (IList<RCTO146MBInfo>) CurrentSession.GetValue(Session.SessionKeys.MBList);
                foreach (RCTO146MBInfo v in mbInfoList)
                {
                    if (!stationCheck.Equals(v.Station))
                    {
                        // 此 CartonSN 站點需為 %1
                        throw new FisException("CQCHK0031", new string[] { stationCheck });
                    }
                }
            }
            else if ("MaterialCT".Equals(category))
            {
                IList<Material> materialList = (IList<Material>) CurrentSession.GetValue(Session.SessionKeys.MaterialList);
                foreach (Material v in materialList)
                {
                    if (!stationCheck.Equals(v.Status))
                    {
                        // 此 CartonSN 站點需為 %1
                        throw new FisException("CQCHK0031", new string[] { stationCheck });
                    }
                }

                IList<string> materialCTList = new List<string>();
                foreach (Material m in materialList)
                {
                    materialCTList.Add(m.MaterialCT);
                }
                CurrentSession.AddValue(Session.SessionKeys.MaterialCTList, materialCTList);
            }
            else if ("NoMaterialCT".Equals(category))
            {
                MaterialBox v = (MaterialBox) CurrentSession.GetValue(Session.SessionKeys.MaterialBox);
                if (!stationCheck.Equals(v.Status))
                {
                    // 此 CartonSN 站點需為 %1
                    throw new FisException("CQCHK0031", new string[] { stationCheck });
                }
            }

            return base.DoExecute(executionContext);
        }        
       
    }
}
