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
    public partial class UpdateMultiMBStatus : BaseActivity
    {
        ///<summary>
        ///</summary>
        public UpdateMultiMBStatus()
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
            IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
          
            string palletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo)??"";
            string cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.CartonSN)??"";
            string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName) ?? "";
            string shipMode = (string)CurrentSession.GetValue(Session.SessionKeys.ShipMode) ?? "";
            string fruNo = (string)CurrentSession.GetValue(Session.SessionKeys.FRUNO)??"";

            if (string.IsNullOrEmpty(cartonSN))
            {
                cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.Carton) ?? "";
            }

            IList<string> mbSNList = null;
            IList<RCTO146MBInfo>  mbInfoList=CurrentSession.GetValue(Session.SessionKeys.MBList) as IList<RCTO146MBInfo> ;
            if (mbInfoList!=null && mbInfoList.Count>0)
            {

                 var tbProductStatusList = (from p in mbInfoList
                                                         select new TbProductStatus(){  ProductID=p.PCBNo,
                                                              Station=p.Station,
                                                              Status =p.Status,
                                                              Line=p.Line,
                                                              TestFailCount=p.TestFailCount,
                                                              ReworkCode="",
                                                               Editor =p.Editor,
                                                               Udt=p.Udt
                                                         }).ToList();

                 mbSNList = (from p in mbInfoList
                                              select p.PCBNo).ToList();

                 if (!string.IsNullOrEmpty(palletNo) &&
                      !string.IsNullOrEmpty(cartonSN))
                 {
                     mbRep.UpdatePalletNobyCaronSnDefered(CurrentSession.UnitOfWork, new List<string>() { cartonSN }, palletNo, this.Editor);
                 }          


                 mbRep.UpdatePCBPreStationDefered(CurrentSession.UnitOfWork, tbProductStatusList);



                 mbRep.UpdatePCBStatusByMultiMBDefered(CurrentSession.UnitOfWork, mbSNList, 
                                                                                     this.Station, 
                                                                                     (int)this.IsPass, 
                                                                                     this.Line, 
                                                                                     this.Editor);
                 return base.DoExecute(executionContext);
            }


            mbSNList = CurrentSession.GetValue(Session.SessionKeys.MBSNOList) as IList<string>;
            if (mbSNList != null && mbSNList.Count > 0)
            {
                 if (!string.IsNullOrEmpty( deliveryNo) &&
                     !string.IsNullOrEmpty(cartonSN))
                    {
                        mbRep.UpdateRCTO146MBbyMultiMBDefered(CurrentSession.UnitOfWork, mbSNList,
                                                                                                 modelName,
                                                                                                 fruNo,
                                                                                                 (decimal)0.0,
                                                                                                 cartonSN,
                                                                                                 deliveryNo,
                                                                                                 palletNo,
                                                                                                 shipMode,
                                                                                                 this.Editor);

                     }


                IList<TbProductStatus> tbProductStatusList = mbRep.GetMBStatus(mbSNList);
                mbRep.UpdatePCBPreStationDefered(CurrentSession.UnitOfWork, tbProductStatusList);
                mbRep.UpdatePCBStatusByMultiMBDefered(CurrentSession.UnitOfWork, mbSNList,
                                                                                    this.Station,
                                                                                    (int)this.IsPass,
                                                                                    this.Line,
                                                                                    this.Editor);
            }
          

           
            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// 过站结果Pass or Fail
        /// </summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(MBStatusEnum), typeof(UpdateMultiMBStatus), new PropertyMetadata(MBStatusEnum.Pass));

     


        ///<summary>
        /// 过站结果Pass or Fail
        ///</summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public MBStatusEnum IsPass
        {
            get
            {
                return ((MBStatusEnum)(base.GetValue(UpdateMultiMBStatus.IsPassProperty)));
            }
            set
            {
                base.SetValue(UpdateMultiMBStatus.IsPassProperty, value);
            }
        }
       
    }
}
