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
            string pallet = (string)CurrentSession.GetValue(Session.SessionKeys.Pallet);

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
            tmpstrLocation = palletRepository.GetSnoIdFromPakLocMasByPno(pallet);
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
                if (strPalletQty.Contains('.'))
                {
                    string[] tmp = strPalletQty.Split('.');
                    strPalletQty = tmp[0];
                }
            }

            //int iPalletTotal = 10;
            int iPalletTotal = DeliveryRepository.GetSumDeliveryQtyOfACertainPallet(pallet);
            //int iPalletTier = 6;
            int iPalletTier = palletRepository.GetTierQtyFromPalletQtyInfo(iPalletTotal.ToString());

            string strShipWay = string.Empty;
            string strRegId = string.Empty;
            //string strRegIdM = string.Empty;

            strShipWay = (string)currentDelivery.GetExtendedProperty("ShipWay");
            strRegId = (string)currentDelivery.GetExtendedProperty("RegId");
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
                if (iPalletTier == 0)
                {
                    strMessage = "请搬入 " + strLocation + " IE未maintain此种栈板的一层数量";
                }
                else if (strShipWay == "T002" && strRegId != "SNE" && strRegId != "SCE" && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入 " + strLocation + " 海運，滿一層請使用大的木頭棧板";
                }
                else if (strShipWay == "T002" && (strRegId == "SNE" || strRegId == "SCE"))
                {
                    strMessage = "请搬入 " + strLocation + " EMEA海運，請使用大的木頭棧板";
                }
                else if (strShipWay == "T001" && strRegId == "SNL")
                {
                    strMessage = "请搬入 " + strLocation + " 滿一層請使用綠色塑料棧板";
                }
                else if (strShipWay == "T001" && (strRegId == "SNE" || strRegId == "SCE") && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入 " + strLocation + " 滿一層請使用chep棧板";
                }
                else if (strShipWay == "T001" && (strRegId == "SNU" || strRegId == "SCU") && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入 " + strLocation + " 滿一層請使用藍色塑料棧板";
                }
                else if (strShipWay == "T001" && strRegId == "SAF" && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入 " + strLocation + " 滿一層請使用新的木頭棧板";
                }
                else if (strShipWay == "T001" && strRegId == "SCN" && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入 " + strLocation + " 滿一層請使用大的木頭棧板";
                }
                else
                {
                    strMessage = "请搬入 " + strLocation;
                }
            }
            CurrentSession.AddValue("LocationMess", strMessage);
            return base.DoExecute(executionContext);
        }

    }
}

