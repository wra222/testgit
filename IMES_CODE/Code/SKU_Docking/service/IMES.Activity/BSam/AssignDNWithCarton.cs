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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Repository.PAK;
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.PAK.BSam;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
namespace IMES.Activity
{


    /// <summary>
    /// Generate Cartion SN
    /// </summary>
  
    public partial class AssignDNWithCarton : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public AssignDNWithCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        ///  get Product session key
        /// </summary>
        public static DependencyProperty ProdSessionKeyProperty = DependencyProperty.Register("ProdSessionKey", typeof(string), typeof(AssignDNWithCarton), new PropertyMetadata(Session.SessionKeys.Product));
        /// <summary>
        /// get Product session key
        /// </summary>
        [DescriptionAttribute("ProdSessionKey")]
        [CategoryAttribute("ProdSessionKey")]
        [BrowsableAttribute(true)]
        //[DefaultValue(Session.SessionKeys.Product)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ProdSessionKey
        {
            get
            {
                return ((string)(base.GetValue(AssignDNWithCarton.ProdSessionKeyProperty)));
            }
            set
            {
                base.SetValue(AssignDNWithCarton.ProdSessionKeyProperty, value);
            }
        }

        /// <summary>
        /// Carton Location 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(AssignDNWithCarton), new PropertyMetadata(true));
        /// <summary>
        /// Carton Location 禁用時要停止workflow 
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(AssignDNWithCarton.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(AssignDNWithCarton.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 分配新的Delivery
        /// </summary>
        public static DependencyProperty IsAssignDNProperty = DependencyProperty.Register("IsAssignDN", typeof(bool), typeof(AssignDNWithCarton), new PropertyMetadata(true));
        /// <summary>
        ///  分配新的Delivery
        /// </summary>
        [DescriptionAttribute("IsAssignDN")]
        [CategoryAttribute("IsAssignDN")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsAssignDN
        {
            get
            {
                return ((bool)(base.GetValue(AssignDNWithCarton.IsAssignDNProperty)));
            }
            set
            {
                base.SetValue(AssignDNWithCarton.IsAssignDNProperty, value);
            }
        }

        /// <summary>
        /// 需要Check Edits船务有没有上传!
        /// </summary>
        public static DependencyProperty IsCheckEditsProperty = DependencyProperty.Register("IsCheckEdits", typeof(bool), typeof(AssignDNWithCarton), new PropertyMetadata(true));
        /// <summary>
        ///  需要Check Edits船务有没有上传!
        /// </summary>
        [DescriptionAttribute("IsCheckEdits")]
        [CategoryAttribute("IsCheckEdits")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsCheckEdits
        {
            get
            {
                return ((bool)(base.GetValue(AssignDNWithCarton.IsCheckEditsProperty)));
            }
            set
            {
                base.SetValue(AssignDNWithCarton.IsCheckEditsProperty, value);
            }
        }

        /// <summary>
        /// 分配新的Delivery跨兩Carton以上
        /// </summary>
        public static DependencyProperty IsOneCartonWithSingleDNProperty = DependencyProperty.Register("IsOneCartonWithSingleDN", typeof(bool), typeof(AssignDNWithCarton), new PropertyMetadata(true));
        /// <summary>
        ///  分配新的Delivery跨兩Carton以上
        /// </summary>
        [DescriptionAttribute("IsOneCartonWithSingleDN")]
        [CategoryAttribute("IsOneCartonWithSingleDN")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsOneCartonWithSingleDN
        {
            get
            {
                return ((bool)(base.GetValue(AssignDNWithCarton.IsOneCartonWithSingleDNProperty)));
            }
            set
            {
                base.SetValue(AssignDNWithCarton.IsOneCartonWithSingleDNProperty, value);
            }
        }

        /// <summary>
        /// 分配新的PalletNo
        /// </summary>
        public static DependencyProperty IsAssignPalletNoProperty = DependencyProperty.Register("IsAssignPalletNo", typeof(bool), typeof(AssignDNWithCarton), new PropertyMetadata(true));
        /// <summary>
        ///  分配新的PalletNo
        /// </summary>
        [DescriptionAttribute("IsAssignPalletNo")]
        [CategoryAttribute("IsAssignPalletNo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsAssignPalletNo
        {
            get
            {
                return ((bool)(base.GetValue(AssignDNWithCarton.IsAssignPalletNoProperty)));
            }
            set
            {
                base.SetValue(AssignDNWithCarton.IsAssignPalletNoProperty, value);
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
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IProduct currentProduct = (IProduct)CurrentSession.GetValue(this.ProdSessionKey);
            if (currentProduct == null)
            {
                FisException e = new FisException("CHK975", new string[] { this.ProdSessionKey });
                e.stopWF = this.IsStopWF;
                throw e;
            }            

            BSamModel bsamModel=   (BSamModel)CurrentSession.GetValue(ExtendSession.SessionKeys.BSamModel);
            string cartonSN = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.CartonSN);
            if (string.IsNullOrEmpty( cartonSN))
            {
                FisException e = new FisException("CHK978", new string[] {currentProduct.ProId});
                e.stopWF = IsStopWF;
                throw e;
            }

            Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
            if (carton == null)
            {

                throw new FisException("CHK978", new string[] { currentProduct.ProId });
            }
           
            int qtyPerCarton = 0;

            if (bsamModel != null)
            {
                qtyPerCarton = bsamModel.QtyPerCaton;
            }
   
            if (IsAssignDN)
            {
                IList<AvailableDelivery> selectedDNList = new List<AvailableDelivery>();
                IList<DeliveryCarton> selectedDNCartonList = new List<DeliveryCarton>();
                IList<AvailableDelivery> dnList = new List<AvailableDelivery>();
                if (this.IsOneCartonWithSingleDN)
                {
                    if (currentProduct.IsBindedPo)
                    {
                        dnList = cartonRep.GetAvailableDNListWithSingleByPo(currentProduct.Model,currentProduct.BindPoNo, "00", -3);
                    }
                    else
                    {
                        dnList = cartonRep.GetAvailableDNListWithSingle(currentProduct.Model, "00", -3);
                    }
                }
                else
                {
                    if (currentProduct.IsBindedPo)
                    {
                        dnList = cartonRep.GetAvailableDNListByPo(currentProduct.Model, currentProduct.BindPoNo, "00", -3);
                    }
                    else
                    {
                        dnList = cartonRep.GetAvailableDNList(currentProduct.Model, "00", -3);
                    }
                }

                if (dnList.Count == 0)
                {
                    FisException e = new FisException("CHK979", new string[] { currentProduct.Model });
                    e.stopWF = IsStopWF;
                    throw e;
                }
                //Check Edits
                Delivery tmpDn = dnRep.GetDelivery(dnList[0].DeliveryNo);
                string flag = (string)tmpDn.GetExtendedProperty("Flag");
                if (IsCheckEdits && flag == "N")
                {
                    if (!CheckCorrectEdits(dnList))
                    {
                        FisException e = new FisException("CHK874", new string[] { });
                        e.stopWF = IsStopWF;
                        throw e;
                    }
                }

                //Check Edits  

                int needQty=qtyPerCarton;
                foreach (AvailableDelivery item in dnList)
                 {
                     if (qtyPerCarton == 0) 
                     {
                       qtyPerCarton = item.QtyPerCarton;
                       needQty = qtyPerCarton;
                     }
                   
                     if (item.RemainQty >= needQty)
                     {
                         item.CartonQty = needQty;
                         selectedDNList.Add(item);
                         needQty = 0;
                         break;
                     }
                     else 
                     {
                         item.CartonQty = item.RemainQty;
                         needQty = needQty - item.RemainQty;
                         selectedDNList.Add(item);
                     }

                 }

                //if (selectedDNList.Count == 0)
                //{
                //    FisException e = new FisException("CHK979", new string[] { currentProduct.Model });
                //    e.stopWF = IsStopWF;
                //    throw e;
                //}


                //檢查所有併板DN 是否上傳
                //if (needQty > 0 || (selectedDNList.Count>1 && dnList[0].ShipmentNo != dnList[0].DeliveryNo.Substring(0,10)) )
                bool consolidateByShipment = (dnList[0].ShipmentNo != dnList[0].DeliveryNo.Substring(0, 10));
                if (consolidateByShipment && (needQty > 0 || selectedDNList.Count > 1))
                {
                    int ConsolidateQty= dnRep.Find(dnList[0].DeliveryNo).DeliveryEx.ConsolidateQty;
                    int  uploadQty=dnRep.GetUploadDNQtyByShipment(dnList[0].ShipmentNo);
                    if (uploadQty < ConsolidateQty)
                    {
                        FisException e = new FisException("CHK980", new string[] { currentProduct.Model , ConsolidateQty.ToString(), uploadQty.ToString()});
                        e.stopWF = IsStopWF;
                        throw e;
                    }
                }


                foreach(AvailableDelivery item in selectedDNList)
                {
                    //DeliveryCarton dc = new DeliveryCarton
                    //{
                    //    CartonSN = cartonSN,
                    //    Model = item.Model,
                    //    AssignQty = 0,
                    //    DeliveryNo = item.DeliveryNo,
                    //    Qty = item.CartonQty,
                    //    Editor = CurrentSession.Editor,
                    //    Cdt = DateTime.Now,
                    //    Udt = DateTime.Now
                    //};
                    //selectedDNCartonList.Add(dc);
                    selectedDNCartonList.Add(cartonRep.BindDeliveryCarton(cartonSN, item, CurrentSession.Editor));                                    
                }
                int cartonQty= qtyPerCarton - needQty;
                CurrentSession.AddValue(ExtendSession.SessionKeys.BindDNList, selectedDNCartonList);                
                CurrentSession.AddValue(ExtendSession.SessionKeys.AvailableDNList, selectedDNList);
                CurrentSession.AddValue(ExtendSession.SessionKeys.BindDeiviceQty, cartonQty);
                CurrentSession.AddValue(ExtendSession.SessionKeys.FullCartonQty, qtyPerCarton);
                carton.FullQty = qtyPerCarton;
                carton.Qty = cartonQty;
                carton.DNQty = selectedDNCartonList.Count;
                carton.Model = currentProduct.Model;
                if (this.IsAssignPalletNo)
                {
                    carton.PalletNo = selectedDNList[0].PalletNo.Trim();
                    CurrentSession.AddValue(Session.SessionKeys.Pallet, selectedDNList[0].PalletNo.Trim());
                }
                //carton.Status = carton.FullQty > carton.Qty ? CartonStatusEnum.Partial : CartonStatusEnum.Full;
                carton.Status = CartonStatusEnum.Reserve;
                cartonRep.Update(carton, CurrentSession.UnitOfWork);                
            }
                       
             //IList<AvailableDelivery> availableDNList = (IList<AvailableDelivery>)CurrentSession.GetValue(ExtendSession.SessionKeys.AvailableDNList);
             IList<DeliveryCarton> bindDNList = (IList<DeliveryCarton>)CurrentSession.GetValue(ExtendSession.SessionKeys.BindDNList);

             if (bindDNList==null  || bindDNList.Count == 0)
             {
                 FisException e = new FisException("CHK981", new string[] { currentProduct.Model });
                 e.stopWF = IsStopWF;
                 throw e;
             }

           
            //分配 DN
             string selectedDN = "";
             foreach (DeliveryCarton item in bindDNList)
             {
                 if (item.Qty > item.AssignQty)
                 {
                     selectedDN = item.DeliveryNo;
                     item.AssignQty++;
                     break;
                 }
             }

             if (string.IsNullOrEmpty(selectedDN))
             {
                 FisException e = new FisException("CHK979", new string[] { currentProduct.Model });
                 e.stopWF = IsStopWF;
                 throw e;
             }

            //檢查ProductID綁定的Carton狀態是否為Reserve狀態，則Abort Carton
            IList<string> cartonProduct = cartonRep.GetReserveCartonByProdId(currentProduct.ProId);
            foreach(string item in cartonProduct)
            {
                if (!item.Equals(carton.CartonSN))
                {
                    cartonRep.RollBackAssignCarton(item, CurrentSession.Editor);
                }
            }

            //Carton Bind ProductID 
            cartonRep.BindCartonProduct(cartonSN,
                                                              currentProduct.ProId,
                                                              selectedDN,
                                                              "",
                                                              CurrentSession.Editor);

             //分配 Carton
             currentProduct.CartonSN = cartonSN;
             currentProduct.DeliveryNo = selectedDN;
             IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
             IList<string> prodIdList = CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) as IList<string>;
             if (prodList == null)
             {
                 prodList = new List<IProduct>();
                 prodIdList = new List<string>();
                 CurrentSession.AddValue(Session.SessionKeys.ProdList, prodList);
                 CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodIdList);
             } 
             prodList.Add(currentProduct);
             prodIdList.Add(currentProduct.ProId);

            return base.DoExecute(executionContext);
          
        }

        private bool CheckCorrectEdits(IList<AvailableDelivery> dnList)
        {
            bool isCorrect = true;
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            foreach (AvailableDelivery dn in dnList)
            {
                IList<string> docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(dn.DeliveryNo.Substring(0, 10));
                //SELECT @templatename = XSL_TEMPLATE_NAME 	FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                if (docnumList.Count > 0)
                {
                    if (CheckIsMRP(dn.DeliveryNo) && dn.QtyPerCarton > 1)
                    {
                        IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label_Over Pack_Wholesale", docnumList[0]);
                        if (tempList.Count == 0)
                        { isCorrect = false; break; }
                    }
                    else
                    {
                        IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label", docnumList[0]);
                        if (tempList.Count == 0)
                        { isCorrect = false; break; }
                    }
                }
                else
                {
                    isCorrect = false; break;
                }
            
            }
            return isCorrect;
        }

        private bool CheckIsMRP(string dn)
        {
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery dnObj = dnRep.GetDelivery(dn);
            string flag = (string)dnObj.GetExtendedProperty("Flag");
            MRPLabelDef mrpDef = dnRep.GetMRPLabel(dn);
            return string.Compare(flag, "N") == 0 && mrpDef != null && !string.IsNullOrEmpty(mrpDef.IndiaPriceID);

        }
    }
}
