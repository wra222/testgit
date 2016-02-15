/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  iMES web constant 
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * Known issues:
 */
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace com.inventec.iMESWEB
{
    ///// <summary>
    ///// 区分是SA/FA/PAK/Common
    ///// </summary>
    //[AttributeUsage(AttributeTargets.Field)]
    //public class PhaseAttribute : Attribute
    //{
    //    public PhaseAttribute(Phase phs)
    //    {
    //        _phs = phs;
    //    }
    //    private Phase _phs = default(Phase);
    //    public Phase Phase 
    //    {
    //        get
    //        {
    //            return _phs;
    //        }
    //    }
    //}

    //public enum Phase
    //{
    //    Common = 0,
    //    SA,
    //    FA,
    //    PAK
    //}

    /// <summary>
    /// Summary description for iMESConstant
    /// </summary>
    public partial class WebConstant
    {
        public WebConstant()
        {
        }
        #region language configuration key
        public const string LANGUAGE = "language";
        # endregion

        //#region  common interface 34
        ////private const string commonService = "";
        //public const string IMB_CODE_SERVICE = "IMB_CODE";
        //public const string I111LEVEL_SERVICE = "I111Level";
        //public const string IPDLINE_SERVICE = "IPdLine";
        //public const string IPRINTTEMPLATE_SERVICE = "IPrintTemplate";
        //public const string ISMTMO_SERVICE = "ISMTMO";
        //public const string IMO_SERVICE = "IMO";
        //public const string IDOCTYPE_SERVICE = "IDocType";
        //public const string ITESTSTATION_SERVICE = "ITestStation";
        //public const string IFAMILY_SERVICE = "IFamily";
        //public const string I1397LEVEL_SERVICE = "I1397Level";
        //public const string IVGA_SERVICE = "IVGA";
        //public const string IFAN_SERVICE = "IFAN";
        //public const string IDefect_SERVICE = "IDefect";
        //public const string ICause_SERVICE = "ICause";
        //public const string IMajorPart_SERVICE = "IMajorPart";
        //public const string IComponent_SERVICE = "IComponent";
        //public const string IObligation_SERVICE = "IObligation";
        //public const string IMark_SERVICE = "IMark";
        //public const string IFloor_SERVICE = "IFloor";
        //public const string IPPIDType_SERVICE = "IPPIDType";
        //public const string IPPIDDescription_SERVICE = "IPPIDDescription";
        //public const string IPartNo_SERVICE = "IPartNo";
        //public const string ISubDefect_SERVICE = "ISubDefect";
        //public const string IResponsibility_SERVICE = "IResponsibility";
        //public const string I4M_SERVICE = "I4M";
        //public const string ICover_SERVICE = "ICover";
        //public const string IUncover_SERVICE = "IUncover";
        //public const string ITrackingStatus_SERVICE = "ITrackingStatus";
        //public const string IDistribution_SERVICE = "IDistribution";
        //public const string IModel_SERVICE = "IModel";
        //public const string IProdIdRange_SERVICE = "IProdIdRange";
        //public const string IKPType_SERVICE = "IKPType";
        //public const string IDN_SERVICE = "IDN";
        //public const string IBOLNo_SERVICE = "IBOLNo";
        //public const string IPallet_SERVICE = "IPallet";
        //public const string ICheckItem_SERVICE = "ICheckItem";
        //public const string IRepair_SERVICE = "IRepair";
        //public const string IMB_SERVICE = "IMB";
        //public const string IProduct_SERVICE = "IProduct";
        //public const string IPartType_SERVICE = "IPartType";
        //public const string IQCStatistic_SERVICE = "IQCStatistic";
        //public const string IDCode_SERVICE = "IDCode";
        //public const string ISession_SERVICE = "ISession";
        //public const string ICollectionData_SERVICE = "ICollectionData";

        ///// <summary>
        ///// return an Object which implement all common interfaces.
        ///// you can get it and upcast to the type you need.
        ///// </summary>
        //public const string ICommon_SERVICE = "ICommon";

        //#endregion
        //# region station interface
        //public const string IPCATestStation_SERVICE = "IPCATestStation";
        //public const string IMBLabelPrint_SERVICE = "IMBLabelPrint";
        //public const string IPCARepair_SERVICE = "IPCARepair";
        //public const string IPCARepairInput_SERVICE = "IPCARepairInput";
        //public const string IPCARepairOutput_SERVICE = "IPCARepairOutput";
        //public const string IPrintComMB_SERVICE = "IPrintComMB";
        //public const string IMBCombineCPU_SERVICE = "IMBCombineCPU";
        //public const string IPCAShippingLabel_SERVICE = "IPCAShippingLabel";
        //public const string IGenerateCustomerSN_SERVICE = "IGenerateCustomerSN";
        //public const string IPrint1397Label_SERVICE = "IPrint1397Label";
        //public const string IIMAGEOutput_SERVICE = "IIMAGEOutput";
        //public const string IIMAGEDownloadCheck_SERVICE = "IIMAGEDownloadCheck";
        //public const string IFARepair_SERVICE = "IFARepair";
        //public const string IOQCInput_SERVICE = "IOQCInput";
        //public const string IOQCOutput_SERVICE = "IOQCOutput";
        //public const string IOQCRepair_SERVICE = "IOQCRepair";
        //public const string IProIdPrint_SERVICE = "IProIdPrint";
        //public const string ITravelCard_SERVICE = "ITravelCard";
        //public const string IMACReprint_SERVICE = "IMACReprint";
        //public const string IBoard_SERVICE = "IBoardInput";
        //public const string IGenSMTMO_SERVICE = "IGenSMTMO";
        //public const string IICTInput_SERVICE = "IICTInput";
        //public const string IFATestStation_SERVICE = "IFATestStation";
        //public const string IInitialPartsCollection_SERVICE = "IInitialPartsCollection";
        //public const string IKittingInput_SERVICE = "IKittingInput";
        //public const string IOfflinePrintCT_SERVICE = "IOfflinePrintCT";
        //public const string ITravelCardPrint_SERVICE = "ITravelCardPrint";

        //# endregion

        public const string SUCCESSRET = "SUCCESSRET";

        //print.cab version
        public static string version = "1,0,0,12";

        #region service path

        //add by qy
        public const string MaintainKittingLocationObject = "IMESMaintainService.KittingLocation";
        public const string MaintainFAStationObject = "IMESMaintainService.FAStation";
        public const string MaintainDefectMaintainObject = "IMESMaintainService.DefectMaintain";
        public const string MaintainTPCBMaintainObject = "IMESMaintainService.TPCBMaintain";
        public const string MaintainAllKindsOfTypeObject = "IMESMaintainService.AllKindsOfType";
        //add by xmzh
        public const string MaintainCommonObject = "IMESMaintainService.Common";
        public const string MaintainCheckItemObject = "IMESMaintainService.CheckItem";
        public const string MaintainPartCheckObject = "IMESMaintainService.PartCheck";
        public const string MaintainPartCheckSettingObject = "IMESMaintainService.PartCheckSetting";
        public const string MaintainQCRatioObject = "IMESMaintainService.QCRatio";
        public const string MaintainModelBOMObject = "IMESMaintainService.ModelBOM";
        public const string MaintainReworkObject = "IMESMaintainService.Rework";
        public const string MaintainLightNoObject = "IMESMaintainService.LightNo";
        public const string MaintainLightStationObject = "IMESMaintainService.LightStation";
        public const string MaintainForwarderObject = "IMESMaintainService.Forwarder";

        public const string MaintainLineObject = "IMESMaintainService.Line";
        public const string MaintainStationObject = "IMESMaintainService.Station";
        public const string MaintainPilotRunPrintInfoObject = "IMESMaintainService.PilotRunPrintInfo";
        
                                                    
        public const string CommonObject = "IMESService.Common";
        public const string MBLabelPrintObject = "IMESService._MBLabelPrint";
    

        public const string MACReprintObject = "IMESService._MACReprint";
        public const string PCATestStationObject = "IMESService._PCATestStation";
        public const string Print1397LabelObject = "IMESService._Print1397Label";
        public const string PCAShippingLabeObject = "IMESService._PCAShippingLabel";
        public const string ReprintProdIdObject = "IMESService._ReprintProdId";
        public const string ProIdPrintObject = "IMESService._ProIdPrint";
        public const string KittingInputObject = "IMESService._KittingInput";
        public const string BoardInputObject = "IMESService._BoardInput";
        public const string FATestStationObject = "IMESService._FATestStation";
        public const string IMAGEDownloadCheckObject = "IMESService._IMAGEDownloadCheck";
        public const string PCARepairInputObject = "IMESService.PCARepairInput";
        public const string PCARepairOutputObject = "IMESService.PCARepairOutput";
        public const string PCARepairObject = "IMESService.PCARepair";
        public const string FARepairObject = "IMESService.FARepair";
        public const string OQCRepairInputObject = "IMESService.OQCInput";
        public const string OQCRepairOutputObject = "IMESService.OQCOutput";
        public const string OQCRepairObject = "IMESService.OQCRepair";
        public const string OfflinePrintCTObject = "IMESService.OfflinePrintCT";
        public const string GenerateCustomerSNObject = "IMESService.GenerateCustomerSN";
        public const string CombineCPUObject = "IMEService.CombineCPU";
        public const string GenerateSMTMOObject = "IMESService.GenerateSMTMO";
        public const string ICTInputObject = "IMESService.ICTInput";
        public const string TravelCardPrintObject = "IMESService.TravelCardPrint";
        public const string InitialPartsCollectionObject = "IMESService.InitialPartsCollection";
        public const string FinalPartsCollectionObject = "IMESService.FinalPartsCollection";
        public const string PrintComMBObject = "IMESService.PrintComMB";
        public const string MBPrintDismantleObject = "IMESService.MBPrintDismantle";
        public const string HDDTestObject = "IMESService.HDDTest";
        public const string CombineKPCTObject = "IMESService.CombineKPCT";
        public const string IMAGEOutputObject = "IMESService.IMAGEOutput";
        public const string VGAShippingLabelReprintObject = "IMESService.VGAShippingLabelReprint";
        public const string IVGAShippingLabelObject = "IMESService._VGAShippingLabel";
        public const string VirtualMoObject = "IMESService._VirtualMo";
        public const string FruTravelCardObject = "IMESService._FRUTravelCard";
        public const string FRUIECSNOObject = "IMESService._FRUIECSNO";
        public const string AdjustMOObject = "IMESService.AdjustMO";
        public const string CombineLCM = "IMESService.CombineLCM";
        public const string MacPrintObject = "IMESService.MacPrint";
        public const string JapaneseLabelPrintObject = "IMESService.JapaneseLabelPrint";
        public const string JPNSVNStationObject = "IMESService.JPNPVS";
        public const string InitialPVSObject = "IMESService.InitialPVS";
        public const string FinalPVSObject = "IMESService.FinalPVS";
        public const string MBReflowManager = "IMESService.MBReflow";
        public const string TPCBTPLabelObject = "IMESService.TPCBTPLabel";
        public const string VirtualCartonObject = "IMESService.VirtualCarton";
        public const string VirtualPalletObject = "IMESService.VirtualPallet";
        public const string GiftFRUCartonPrint = "IMESService.FRUCartonLabelPrint";
        public const string UnitWeight = "IMESService.UnitWeight";
        public const string PCACosmeticObject = "IMESService.PCACosmetic";
        public const string PCACosmeticFAObject = "IMESService.MBBufferCosmetic";
        public const string CombinePOInCartonObject = "IMESService.CombinePOInCarton";
        public const string FGShippingLabelPrint = "IMESService.FGShippingLabel";
        public const string FGShippingLabelPrintTRO = "IMESService.FGShippingLabelTRO";
        public const string FRUWeight = "IMESService.FRUWeight";
        public const string FRUWeightShipment = "IMESService.FRUWeightShipment";
        public const string FRUGiftLabelPrint = "IMESService.FRUGiftLabelPrint";
        public const string PODataObject = "IMESService.POData";
        public const string PVSReprintObject = "IMESService.PVSReprint";
        public const string FRUShiptoLabelPrintObject = "IMESService.FRUShiptoLabelPrint";
        public const string CTFRUCartonPrintObject = "IMESService.FRUCartonLabelPrintCT";
        public const string LightGuideObject = "IMESService.LightGuide";
        public const string DismantleFA = "IMESService.DismantleFA";
        public const string TPCBCollectionObject = "IMESService.TPCBCollection";
        public const string CartonWeight = "IMESService.CartonWeight";
        public const string PizzaWeight = "IMESService.PizzaWeight";
        public const string PalletWeight = "IMESService.PalletWeight";
        public const string SwitchBoardLabelPrint = "IMESService.SwitchBoardLabelPrint";
	    public const string KPPrintObject = "IMESService.KPPrint";
        public const string BorrowAndReturnObject = "IMESService.BorrowAndReturn";
        public const string SmallPartsObject = "IMESService.SmallPartsLabelPrint";
        public const string SmallPartsUploadObject = "IMESService.SmallPartsUpload";
        public const string PrintShiptoCartonLabelObject = "IMESService.PrintShiptoCartonLabel";
	    public const string UnpackObject = "IMESService.Unpack";
      
        //2010-03-08 Tong.Zhi-Yong 
        public const string UnitLabelObject = "IMESService._UnitLabel";
        public const string PalletDataCollectionObject = "IMESService._PalletDataCollection";
        public const string PalletDataCollectionTROObject = "IMESService._PalletDataCollectionTRO";
        public const string PAQCOutputObject = "IMESService._PAQCOutput";
        public const string PAQCRepair = "IMESService._PAQCRepair";
        public const string FACosmeticObject = "IMESService.FACosmetic";
        public const string PilotRunPrintObject = "IMESService.PilotRunPrint";
        public const string Print2dDoorLabelObject = "IMESService.Print2dDoorLabel";
        public const string PrintBottomCaseLabelObject = "IMESService.PrintBottomCaseLabel";
        public const string RePrintCustsnLabelObject = "IMESService.RePrintCustsnLabel";

        //add by Shao.Rong-hua 2011/05/21
        public const string CombineBatteryCTObject = "IMESService.CombineBatteryCT";

        //Add by IES106137 Jiali 
        public const string PACosmeticObject = "IMESService.PACosmeticImpl";
        public const string ChangeMB = "IMESService.ChangeMB";

        //added for maintain
        public const string IModelProcess = "IMESMaintainService.IModelProcess";
        public const string IECRVersion = "IMESMaintainService.IECRVersion";

        //added by liu xiaoling 2010-1
        public const string IModelManager = "IMESMaintainService.IModelManager";
        public const string IPartManager = "IMESMaintainService.IPartManager";
        public const string IMOBOMManager = "IMESMaintainService.IMOBOMManager";
        public const string IProcessManager = "IMESMaintainService.IProcessManager";
        public const string IPartTypeManager = "IMESMaintainService.IPartTypeManager";
        public const string IPartForbiddenManager = "IMESMaintainService.IPartForbiddenManager";
        public const string ILabelSettingManager = "IMESMaintainService.ILabelSettingManager";
 
        //added by itc207024
        public const string IModelWeightTolerance = "IMESMaintainService.ModelWeightTolerance";
        public const string IPalletWeight = "IMESMaintainService.PalletWeight";
        public const string IRegion = "IMESMaintainService.Region";
        public const string IPdLineStation = "IMESMaintainService.PdLineStation";
        public const string IRunInTimeControl = "IMESMaintainService.RunInTimeControlManager";
        public const string IWLBTDescr = "IMESMaintainService.WLBTDescrManager";
        public const string IShipType = "IMESMaintainService.ShipTypeManager";

        //added by Tong.Zhi-Yong 2011-03-28
        public const string PAQCInputObject = "IMESService.PAQCInput";
        public const string PickCardObject = "IMESService.PickCard";
        public const string FinalScanObject = "IMESService.FinalScan";

        //Added by Dean 20110625
        public const string ForceEOQC = "IMESService.ForceEOQC";
        public const string PCAQuery = "IMESService.PCAQuery";
        public const string FAQuery = "IMESService.FAQuery";

        //Added by Benson 20110711
        public const string ImportDataToTestBoxDataLog = "IMESService.ImportDataToTestBoxDataLog";

        //Added by Benson 20110914
        public const string CombinePOInCartonForFRU = "IMESService.CombinePOInCartonForFRU";

        //Added by Benson 20110916
        public const string CommonCollectPartObject = "IMESService.CommonCollectPart";
        #endregion

        #region other
        public const string CHANNELNAME = "IMESChanelClient";
        #endregion

        //#region implementClass

        //public const string Common = "Common";
        //public const string MBLabelPrint = "MBLabelPrint";
        //public const string MACReprint = "MACReprint";
        //public const string PCATestStation = "PCATestStation";
        //public const string Print1397Label = "Print1397Label";
        //public const string PCAShippingLabel = "PCAShippingLabel";
        //public const string ReprintProdId = "ReprintProdId";
        //public const string ProIdPrint = "ProIdPrint";
        //public const string KittingInput = "KittingInput";
        //public const string BoardInput = "BoardInput";
        //public const string FATestStation = "FATestStation";
        //public const string IMAGEDownloadCheck = "IMAGEDownloadCheck";
        //public const string PCARepairInput = "PCARepairInput";
        //public const string PCARepairOutput = "PCARepairOutput";
        //public const string PCARepair = "PCARepair";
        //public const string FARepair = "FARepair";
        //public const string OQCRepairInput = "OQCInput";
        //public const string OQCRepairOutput = "OQCOutput";
        //public const string OQCRepair = "OQCRepair";
        //public const string OfflinePrintCT = "OfflinePrintCT";
        //public const string GenerateCustomerSN= "GenerateCustomerSN";
        //public const string CombineCPU = "CombineCPU";
        //public const string GenerateSMTMO = "GenerateSMTMO";
        //public const string ICTInput = "ICTInput";
        //public const string TravelCardPrint = "TravelCardPrint";
        //public const string InitialPartsCollection = "InitialPartsCollection";
        //public const string PrintComMB = "PrintComMB";
        //public const string MBPrintDismantle = "MBPrintDismantle";
        //public const string IMAGEoutput = "IMAGEoutput";

        //#endregion

        # region "session type"
        public const string SessionMB = "1";
        public const string SessionProduct = "2";
        public const string SessionCommon = "0";
        # endregion
    }
}
