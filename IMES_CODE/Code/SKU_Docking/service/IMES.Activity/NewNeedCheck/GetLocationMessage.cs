//2013-03-13    Vincent           release
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure.Utility.Common;
using IMES.Infrastructure.Extend;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GetLocationMessage : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetLocationMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPalletRepository palletRepository =
                   RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IDeliveryRepository DeliveryRepository =
                    RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            string pallet="";
            if (CurrentSession.GetValue(Session.SessionKeys.Pallet) is Pallet) 
            {
                pallet = (CurrentSession.GetValue(Session.SessionKeys.Pallet) as Pallet).PalletNo;
            }
            else
            {
                   pallet = (string)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            }

            if (String.IsNullOrEmpty(pallet))
            {
                CurrentSession.AddValue("LocationMess", "");
                return base.DoExecute(executionContext);
            }

            string strCarrier = string.Empty;
            string strLocation = string.Empty;
            string tmpstrLocation = string.Empty;
            string strPalletQty = string.Empty;

            strCarrier = (string)currentDelivery.GetExtendedProperty("Carrier");
           // tmpstrLocation = palletRepository.GetSnoIdFromPakLocMasByPno(pallet);
            
            string loc = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.PalletLoc);
            tmpstrLocation = !string.IsNullOrEmpty(loc) ? loc : palletRepository.GetSnoIdFromPakLocMasByPno(pallet);
           
         
            //strLocation = "MyTestForLocation";
            if (String.IsNullOrEmpty(tmpstrLocation) && pallet.Substring(0, 2) != "BA" && pallet.Substring(0, 2) != "NA")
            {
                strLocation = "Other";
            }
            else if (pallet.Substring(0, 2) == "NA")
            {
                strLocation = "Others";
            }
            else if (pallet.Substring(0, 2) == "BA")
            {
                if (String.IsNullOrEmpty(strCarrier))
                    strLocation = "";
                else
                    strLocation = strCarrier.TrimEnd();
            }
            else
            {
                //ITC-1360-1393
                strLocation = tmpstrLocation;
            }


            strPalletQty = (string)currentDelivery.GetExtendedProperty("PalletQty");
            if (String.IsNullOrEmpty(strPalletQty))
            {
                strPalletQty = "60";
            }
            else
            {
                /*if (strPalletQty.Contains('.'))
                {
                    string[] tmp = strPalletQty.Split('.');
                    strPalletQty = tmp[0];
                }*/
            }

            //int iPalletTotal = 10;
            int iPalletTotal = DeliveryRepository.GetSumDeliveryQtyOfACertainPallet(pallet);
            //int iPalletTier = 6;
            //int iPalletTier = palletRepository.GetTierQtyFromPalletQtyInfo(iPalletTotal.ToString());
            int iPalletTier = palletRepository.GetTierQtyFromPalletQtyInfo(strPalletQty);

            string strShipWay = string.Empty;
            string strRegId = string.Empty;
            //string strRegIdM = string.Empty;

            strShipWay = (string)currentDelivery.GetExtendedProperty("ShipWay");
            strRegId = (string)currentDelivery.GetExtendedProperty("RegId");
            if (strRegId != null && strRegId.Length == 3)
            { strRegId = strRegId.Substring(1, 2); }
            else
            { strRegId = ""; }
            //strRegIdM = (string)currentDelivery.GetExtendedProperty("RegIdM");

            //strShipWay = "T001";
            //strRegId = "SAF";

            string strMessage = string.Empty;
            if (String.IsNullOrEmpty(strLocation))
            {
                strMessage = "此栈板上机器数量小於等於6台,未分配库位";
            }
            else
            {
               
                if (CommonUti.GetSite()=="ICC")
                {
                    IList<PalletType> lstPalletType = CommonUti.GetPalletType(pallet, currentDelivery.DeliveryNo);
                    string type = "";
                    if (lstPalletType.Count > 0)
                    {
                        PalletType palletType = lstPalletType[0];
                        type = palletType.Type;
                    }
                    if (string.IsNullOrEmpty(type))
                    { strMessage = "请搬入" + strLocation + " 滿一層請使用Other棧板"; }
                    else
                    { strMessage = "请搬入" + strLocation + " 滿一層請使用" + type; } 
                }
                else
                {
                    if (iPalletTier == 0)
                    {
                        strMessage = "请搬入 " + strLocation + " IE未maintain此种栈板的一层数量";
                    }
                    else if (strShipWay == "T002" && strRegId != "NE" && strRegId != "CE" && iPalletTotal > iPalletTier)
                    {
                        strMessage = "请搬入 " + strLocation + " 海運，滿一層請使用大的木頭棧板";
                    }
                    else if (strShipWay == "T002" && (strRegId == "NE" || strRegId == "CE"))
                    {
                        strMessage = "请搬入 " + strLocation + " EMEA海運，請使用大的木頭棧板";
                    }
                    else if (strShipWay == "T001" && strRegId == "NL")
                    {
                        strMessage = "请搬入 " + strLocation + " 滿一層請使用藍色塑料棧板";
                    }
                    else if (strShipWay == "T001" && (strRegId == "NE" || strRegId == "CE") && iPalletTotal > iPalletTier)
                    {
                        strMessage = "请搬入 " + strLocation + " 滿一層請使用chep棧板";
                    }
                    else if (strShipWay == "T001" && (strRegId == "NU" || strRegId == "CU") && iPalletTotal > iPalletTier)
                    {
                        strMessage = "请搬入 " + strLocation + " 滿一層請使用藍色塑料棧板";
                    }
                    else if (strShipWay == "T001" && strRegId == "AF" && iPalletTotal > iPalletTier)
                    {
                        strMessage = "请搬入 " + strLocation + " 滿一層請使用绿色塑料棧板";
                    }
                        
                    //else if (strShipWay == "T001" && strRegId == "CN" && iPalletTotal > iPalletTier)
                    else if (strShipWay == "T001" && ActivityCommonImpl.Instance.CheckDomesticDN(strRegId) && iPalletTotal > iPalletTier)
                    {
                        strMessage = "请搬入 " + strLocation + " 滿一層請使用大的木頭棧板";
                    }
                    else
                    {
                        strMessage = "请搬入 " + strLocation;
                    }
                }

             
            }
            CurrentSession.AddValue("LocationMess", strMessage);
            return base.DoExecute(executionContext);
        }

     
    }
}

