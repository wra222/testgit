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
    /// 
    /// </summary>
    public partial class UpackCartonOrDn : BaseActivity
    {
        ///<summary>
        ///</summary>
        public UpackCartonOrDn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// unpack Carton or Dn 資料
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            var materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
            var mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

            string category = (string)CurrentSession.GetValue(Session.SessionKeys.RCTO146Category);
            if (string.IsNullOrEmpty(category))
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.RCTO146Category });
            }

            string cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.CartonSN);
            string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

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

            if (category == "MBCT")
            {
                if (this.IsCaronSNKey)
                {
                  //  mbRep.UnpackRCTO146MBbyCatonSN(cartonSN, this.Editor);
                    mbRep.UnpackRCTO146MBbyCatonSNDefered(CurrentSession.UnitOfWork, cartonSN,  this.Customer);
                    IList<string> lst=new List<string>();
                    lst.Add(cartonSN);
                    List<string> pcbNoList= mbRep.GetRCTO146MBByCartonSN(lst).Select(x=>x.PCBNo).ToList();
                    mbRep.UpdatePCBStatusByMultiMBDefered(CurrentSession.UnitOfWork, pcbNoList, 
                                                                                     this.Station,
                                                                                     0, 
                                                                                     this.Line, 
                                                                                     this.Editor);
                   
                }
                else
                {
                    mbRep.UnpackRCTO146MBbyDeliveryNoDefered(CurrentSession.UnitOfWork, deliveryNo, this.Editor);
                }
                
            }

            if (category == "MaterialCT")
            {
                if (this.IsCaronSNKey)
                {
                    materialRep.UnpackByCatonSNDefered(CurrentSession.UnitOfWork, cartonSN, this.Station, this.Editor);
                }
                else
                {
                    materialRep.UnpackByDeliveryNoDefered(CurrentSession.UnitOfWork, deliveryNo, this.Station, this.Editor);
                }
            }

            if (category == "NoMaterialCT")
            {
                 IList<MaterialBox> materialBoxList = (IList<MaterialBox>)CurrentSession.GetValue(Session.SessionKeys.MaterialBoxList);
                if (materialBoxList==null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialBoxList });
                }
                var boxIdList = materialBoxList.Select(x=>x.BoxId).Distinct().ToList();
                materialBoxRep.UpdateDnPalletStatusbyMultiBoxIdDefered(CurrentSession.UnitOfWork,
                                                                                                           boxIdList, "", "", "", this.Station, "", this.Editor);
            }
           
            return base.DoExecute(executionContext);
        }



        /// <summary>
        ///  抓資料是by CatonSN or DeliveryNo
        /// </summary>
        public static DependencyProperty IsCaronSNKeyProperty = DependencyProperty.Register("IsCaronSNKey", typeof(bool), typeof(UpackCartonOrDn), new PropertyMetadata(true));

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
                return ((bool)(base.GetValue(UpackCartonOrDn.IsCaronSNKeyProperty)));
            }
            set
            {
                base.SetValue(UpackCartonOrDn.IsCaronSNKeyProperty, value);
            }
        }

       
    }
}
