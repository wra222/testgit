// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// CreateMaterial
    /// </summary>
    public partial class GetRCTO146Category : BaseActivity
    {
        ///<summary>
        ///CreateMaterial
        ///</summary>
        public GetRCTO146Category()
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
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            Delivery dn ;
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            var materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
            var mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            string cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.CartonSN);
            string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            IsByPassCheck = true;
            if (IsCaronSNKey)
            {
                if (string.IsNullOrEmpty(cartonSN))
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.CartonSN });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(deliveryNo))
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.DeliveryNo });
                }
            }
            List<string> palletNoList =null;
            List<string> deliveryNoList =null;
            List<string> stationList = null;

            IList<RCTO146MBInfo> mbInfoList = IsCaronSNKey ?
                                                                mbRep.GetRCTO146MBByCartonSN(new List<string>() { cartonSN }) :
                                                                mbRep.GetRCTO146MBByDeliveryNo(deliveryNo);
            if (mbInfoList != null && mbInfoList.Count > 0)
            {
                palletNoList = (from p in mbInfoList
                                where !string.IsNullOrEmpty(p.PalletNo)
                                select p.PalletNo).Distinct().ToList();
               

                deliveryNoList = (from p in mbInfoList                                                
                                               select p.DeliveryNo
                                               ).Distinct().ToList();

                stationList = (from p in mbInfoList
                                     where this.Station != p.Station
                                   select p.Station).Distinct().ToList();


                if (!IsByPassCheck)
                {
                    checkData(cartonSN, palletNoList, deliveryNoList, stationList);
                }
           
                CurrentSession.AddValue(Session.SessionKeys.RCTO146Category, "MBCT");
                CurrentSession.AddValue(Session.SessionKeys.MBList, mbInfoList);
                CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, deliveryNoList[0]);
                dn = dnRep.Find(deliveryNoList[0]);
                CurrentSession.AddValue(Session.SessionKeys.Delivery, dn);
                CurrentSession.AddValue(Session.SessionKeys.Qty, mbInfoList.Count);

                if (!this.NeedPalletNotExist)
                {
                    CurrentSession.AddValue(Session.SessionKeys.PalletNo, palletNoList[0]);
                }

                return base.DoExecute(executionContext);
            } 


            IList<Material> materialList =  IsCaronSNKey ?
                materialRep.GetMaterial(new Material() { CartonSN = cartonSN }):
                materialRep.GetMaterial(new Material() { DeliveryNo = deliveryNo });
            if (materialList != null && materialList.Count > 0)
            {

                palletNoList = (from p in materialList
                                where !string.IsNullOrEmpty(p.PalletNo)
                                select p.PalletNo).Distinct().ToList();


                deliveryNoList = (from p in materialList
                                  select p.DeliveryNo
                                               ).Distinct().ToList();

                stationList = (from p in materialList
                               where this.Station != p.Status
                               select p.Status).Distinct().ToList();


                if (!IsByPassCheck)
                {
                    checkData(cartonSN, palletNoList, deliveryNoList, stationList);
                }
          
                CurrentSession.AddValue(Session.SessionKeys.RCTO146Category, "MaterialCT");
                CurrentSession.AddValue(Session.SessionKeys.MaterialList, materialList);

                //
                IList<string> lst= materialList.Select(x=>x.MaterialCT).ToList();
                CurrentSession.AddValue(Session.SessionKeys.MaterialCTList, lst);

                //

                CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, deliveryNoList[0]);
                dn = dnRep.Find(deliveryNoList[0]);
                CurrentSession.AddValue(Session.SessionKeys.Delivery, dn);
                CurrentSession.AddValue(Session.SessionKeys.Qty, materialList.Count);

                if (!this.NeedPalletNotExist)
                {
                    CurrentSession.AddValue(Session.SessionKeys.PalletNo, palletNoList[0]);
                }

                return base.DoExecute(executionContext);
            }

            IList<MaterialBox> materialBoxList= IsCaronSNKey ?
                materialBoxRep.GetMaterialBox(new MaterialBox() {  BoxId=cartonSN}):
                materialBoxRep.GetMaterialBox(new MaterialBox() { DeliveryNo = deliveryNo });
            if (materialBoxList != null && materialBoxList.Count > 0)
            {
                
                palletNoList = (from p in materialBoxList
                                       where !string.IsNullOrEmpty(p.PalletNo)
                                select p.PalletNo).Distinct().ToList();


                deliveryNoList = (from p in materialBoxList
                                  select p.DeliveryNo
                                               ).Distinct().ToList();

                stationList = (from p in materialBoxList
                                    where this.Station!= p.Status
                               select p.Status).Distinct().ToList();
                checkData(cartonSN,palletNoList, deliveryNoList, stationList);
                CurrentSession.AddValue(Session.SessionKeys.RCTO146Category, "NoMaterialCT");
                CurrentSession.AddValue(Session.SessionKeys.MaterialBox, materialBoxList[0]);
                CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, deliveryNoList[0]);
                dn = dnRep.Find(deliveryNoList[0]);
                CurrentSession.AddValue(Session.SessionKeys.Delivery, dn);
                CurrentSession.AddValue(Session.SessionKeys.Qty, materialBoxList[0].Qty);
                CurrentSession.AddValue(Session.SessionKeys.MaterialBoxList, materialBoxList);

                if (!this.NeedPalletNotExist)
                {
                    CurrentSession.AddValue(Session.SessionKeys.PalletNo, palletNoList[0]);
                }

                return base.DoExecute(executionContext);
            }

            throw new FisException("CQCHK0014", new string[] { cartonSN });
            //return base.DoExecute(executionContext);
        }

        private void checkData(string cartonSN, List<string> palletNoList, List<string> deliveryNoList, List<string> stationList)
        {
            if (palletNoList != null && palletNoList.Count > 0)
            {
                if (this.NeedPalletNotExist)
                    throw new FisException("CQCHK0015", new string[] { cartonSN, string.Join(",", palletNoList.ToArray()) });
            }
            else
            {
                if (!this.NeedPalletNotExist)
                {
                    // 此Carton SN: %1 尚未結合棧板
                    throw new FisException("CQCHK0033", new string[] { cartonSN });
                }
            }

            if (deliveryNoList != null && deliveryNoList.Count != 1)
            {
                 throw new FisException("CQCHK0016", new string[]{cartonSN, string.Join(",",deliveryNoList.ToArray())});
            }
            else if (deliveryNoList != null && string.IsNullOrEmpty(deliveryNoList[0]))
            {
                throw new FisException("CQCHK0017", new string[]{cartonSN});
            }

            if (this.NeedCheckStation)
            {
                if (stationList != null && stationList.Count > 0)
                {
                    throw new FisException("CQCHK0018", new string[] { cartonSN, string.Join(",", stationList.ToArray()) });
                }
            }

        }

        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty IsByPassCheckProperty = DependencyProperty.Register("IsByPassCheck", typeof(bool), typeof(GetRCTO146Category), new PropertyMetadata(false));

        /// <summary>
        /// NeedPalletNotExist:True Or False
        /// </summary>
        [DescriptionAttribute("IsByPassCheck")]
        [CategoryAttribute("IsByPassCheck Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsByPassCheck
        {
            get
            {
                return ((bool)(base.GetValue(GetRCTO146Category.IsByPassCheckProperty)));
            }
            set
            {
                base.SetValue(GetRCTO146Category.IsByPassCheckProperty, value);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty NeedPalletNotExistProperty = DependencyProperty.Register("NeedPalletNotExist", typeof(bool), typeof(GetRCTO146Category), new PropertyMetadata(true));

        /// <summary>
        /// NeedPalletNotExist:True Or False
        /// </summary>
        [DescriptionAttribute("NeedPalletNotExist")]
        [CategoryAttribute("NeedPalletNotExist Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NeedPalletNotExist
        {
            get
            {
                return ((bool)(base.GetValue(GetRCTO146Category.NeedPalletNotExistProperty)));
            }
            set
            {
                base.SetValue(GetRCTO146Category.NeedPalletNotExistProperty, value);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty NeedCheckStationProperty = DependencyProperty.Register("NeedCheckStation", typeof(bool), typeof(GetRCTO146Category), new PropertyMetadata(true));

        /// <summary>
        /// NeedCheckStation:True Or False
        /// </summary>
        [DescriptionAttribute("NeedCheckStation")]
        [CategoryAttribute("NeedCheckStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NeedCheckStation
        {
            get
            {
                return ((bool)(base.GetValue(GetRCTO146Category.NeedCheckStationProperty)));
            }
            set
            {
                base.SetValue(GetRCTO146Category.NeedCheckStationProperty, value);
            }
        }

        /// <summary>
        ///  抓資料是by CatonSN or DeliveryNo
        /// </summary>
        public static DependencyProperty IsCaronSNKeyProperty = DependencyProperty.Register("IsCaronSNKey", typeof(bool), typeof(GetRCTO146Category), new PropertyMetadata(true));

        /// <summary>
        /// IsCaronSNKey:True Or False (by DeliveryNo)
        /// </summary>
        [DescriptionAttribute("IsCaronSNKey")]
        [CategoryAttribute("IsCaronSNKey Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsCaronSNKey
        {
            get
            {
                return ((bool)(base.GetValue(GetRCTO146Category.IsCaronSNKeyProperty)));
            }
            set
            {
                base.SetValue(GetRCTO146Category.IsCaronSNKeyProperty, value);
            }
        }



    }
    
}
