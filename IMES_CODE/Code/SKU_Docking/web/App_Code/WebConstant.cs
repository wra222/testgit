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
        public const string ASSETRANGE = "IMESMaintainService.IASSETRANGEMANAGER";
        public const string ACADAPTORMAITAIN = "IMESMaintainService.IACADAPTORMANAGER";
        public const string GradeMAITAIN = "IMESMaintainService.IGRADEMANAGER";
        public const string PAKITLOCMAITAIN = "IMESMaintainService.IPAKITLOCMANAGER";
        public const string MASTERMAITAIN = "IMESMaintainService.IMASTERLABELMANAGER";
        public const string COARECEIVING = "IMESMaintainService.ICOARECEIVINGMANAGER";
        public const string ITCNDDEFECTCHECK = "IMESMaintainService.IITCNDDEFECTCHECKMANAGER";
        public const string ITCNDCHECKSETTING = "IMESMaintainService.IITCNDCHECKSETTINGMANAGER";
        public const string REPAIRINFOMAINTAIN = "IMESMaintainService.REPAIRINFOMAINTAINMANAGER";
        public const string LIGHTSTATIONMAINTAIN = "IMESMaintainService.LIGHTSTATIONMANAGER";
        public const string WEIGHTSETTINGMAINTAIN = "IMESMaintainService.WEIGHTSETTINGMANAGER";
        public const string SATESTCHECKRULE = "IMESMaintainService.SATESTCHECKRULE";
        public const string LOTSETTING = "IMESMaintainService.LOTSETTINGMANAGER";
        public const string ICOMBINEKPSETTING = "IMESMaintainService.ICOMBINEKPSETTINGMANAGER";
        public const string IFAMILYMBCODE = "IMESMaintainService.IFAMILYMBCODEMANAGER";
        public const string IFAIINFOMAINTAIN = "IMESMaintainService.IFAIINFOMAINTAINMANAGER";
        public const string ASTDEFINE = "IMESMaintainService.IAstDefine";
        public const string IModelAssemblyCode = "IMESMaintainService.IModelAssemblyCode";
        # endregion



        public const string SUCCESSRET = "SUCCESSRET";

        //print.cab version
        public static string version = "1,0,0,12";

        #region service path

        public const string MaintainCommonFunctionObject = "IMESMaintainService.CommonFunction";
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
        public const string MaintainFaKittingUploadObject = "IMESMaintainService.FaKittingUploadManager";
        public const string MaintainPakKittingUploadObject = "IMESMaintainService.PakKittingUploadManager";

        //add by xmzh 2011,10,17
        public const string MaintainAssetRuleObject = "IMESMaintainService.AssetRule";
        public const string MaintainModelWeightObject = "IMESMaintainService.ModelWeightManager";

        public const string MaintainLineObject = "IMESMaintainService.Line";
        public const string MaintainStationObject = "IMESMaintainService.Station";
        public const string MaintainPilotRunPrintInfoObject = "IMESMaintainService.PilotRunPrintInfo";



        //add by itc202017, 2012-05-15
        public const string MaintainCDSIPOObject = "IMESMaintainService.CDSIPOManager";
        public const string MaintainActualProductionTimeObject = "IMESMaintainService.ActualProductionTimeManager";

        //add by Dorothy 2012.5.11
        public const string MaintainDefectStationObject = "IMESMaintainService.DefectStationManager";
        public const string FAITCNDefectCheckObject = "IMESMaintainService.FAITCNDefectCheckManager";

        //add by Benson 2013.04.03
        public const string CollectionMaterialLotObject = "IMESMaintainService.CollectionMaterialLot";
        public const string MaintainFRUMBVersionManagerObject = "IMESMaintainService.FRUMBVersionManager";
        //add by zhanghe
        public const string MaintainMBObject = "IMESMaintainService.MBMaintainManager";

        public const string MaintainPLTStandardObject = "IMESMaintainService.PLTStandardManager";
        public const string MaintainSysSettingObject = "IMESMaintainService.SysSetting";

        public const string CommonObject = "IMESService.Common";
        public const string MACReprintObject = "IMESService._MACReprint";
        public const string PCATestStationObject = "IMESService._PCATestStation";
        public const string Print1397LabelObject = "IMESService._Print1397Label";
        public const string PCAShippingLabeObject = "IMESService._PCAShippingLabel";
        public const string ReprintProdIdObject = "IMESService._ReprintProdId";
        public const string ProIdPrintObject = "IMESService._ProIdPrint";
        public const string BoardInputObject = "IMESService._BoardInput";
        public const string FATestStationObject = "IMESService.FATestStation";
        public const string FunctionTestForRCTOObject = "IMESService.FunctionTestForRCTO";
        public const string IMAGEDownloadCheckObject = "IMESService._IMAGEDownloadCheck";
        public const string PCARepairOutputObject = "IMESService.PCARepairOutput";
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
        public const string VirtualMoObject = "IMESService.VirtualMo";
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
        public const string MBBorrowObject = "IMESService.MBBorrowControl";
        public const string ProductBorrowObject = "IMESService.ProductBorrowControl";
        public const string CTBorrowObject = "IMESService.CTBorrowControl";
        public const string QueryBorrowObject = "IMESService.QueryBorrow";
        public const string DTPalletControlObject = "IMESService.DTPalletControl";
        public const string DismantlePalletWeightObject = "IMESService.DismantlePalletWeight";
        public const string ChangeModelObject = "IMESService.ChangeModel";
        public const string ChangeSamplePOObject = "IMESService.ChangeSamplePO";
        public const string PalletCollectionObject = "IMESService.PalletCollection";
        public const string KBEsopObject = "IMESService.KBEsop";
        public const string EEPLabelPrintObject = "IMESService.EEPLabelPrint";
        // add by chenpeng

        public const string PIAOutputObject = "IMESService.PIAOutput";
        public const string MasterLabelPrintObject = "IMESService.MasterLabelPrint";
        public const string PrintInatelICASAObject = "IMESService.PrintInatelICASA";
        public const string SpecialModelForItcndObject = "IMESService.SpecialModelForItcnd";
        public const string ChangeToEPIAPIAObject = "IMESService.ChangeToEPIAPIA";
        public const string ScanningListObject = "IMESService.ScanningList";
        public const string ReturnUsedKeysObject = "IMESService.ReturnUsedKeys";
        public const string PCAMBContactObject = "IMESService.PCAMBContact";
        public const string PalletVerifyForRCTOObject = "IMESService.PalletVerifyForRCTO";
        public const string UnpackForRCTOObject = "IMESService.UnpackForRCTO";

        //add by itc202017
        //public const string UnpackObject = "IMESService.Unpack";
        public const string AFTMVSObject = "IMESService._AFTMVS";
        public const string ChangeKeyPartsObject = "IMESService._ChangeKeyParts";
        public const string UploadShipmentDataObject = "IMESService._UploadShipmentData";
        public const string LabelLightGuideObject = "IMESService._LabelLightGuide";
        public const string LabelLightGuideForDockingObject = "IMESService._LabelLightGuideForDocking";
        public const string PoDataObject = "IMESService._PoData";
        public const string PCARepairInputObject = "IMESService._PCARepairInput";//PCAReceiveReturnMB
        public const string PCARepairObject = "IMESService.PCARepair";
        public const string SMTDefectInputObject = "IMESService._SMTDefectInput";
        public const string KPDefectInputObject = "IMESService._KPDefectInput";
        public const string KPRepairObject = "IMESService._KPRepair";
        public const string RepairInfoObject = "IMESService._RepairInfo";
        public const string LCMRepairObject = "IMESService._LCMRepair";
        public const string OQCOutputObject = "IMESService._OQCOutput";
        public const string RCTOWeightObject = "IMESService._RCTOWeight";
        public const string DOAMBUploadObject = "IMESService._DOAMBUpload";

        //add by zhu lei
        public const string PackPizzaObject = "IMESService.PackingPizza";
        public const string IECLabelPrintObject = "IMESService.IECLabelPrint";
        public const string PCAShippingLabelPrintObject = "IMESService.PCAShippingLabelPrint";
        public const string VGALabelPrintObject = "IMESService.VGALabelPrint";
        public const string PCARepairTestObject = "IMESService.PCARepairTest";
        public const string PickCardImplObject = "IMESService.PickCardImpl";
        public const string MBLabelPrintObject = "IMESService._MBLabelPrint";
        public const string PACosmeticObject = "IMESService.PACosmeticImpl";
        public const string PCAICTInputObject = "IMESService.ICTInput";

        //add by zhanghe itc202007
        public const string TravelCardPrint2012Object = "IMESService.TravelCardPrint2012";
        public const string ITCNDCheckObject = "IMESService.ITCNDCheck";
        public const string PrDeleteObject = "IMESService.PrDelete";
        public const string PackingListObject = "IMESService.PackingList";
        public const string FARepairObject = "IMESService.FARepair";
        public const string ShipToCartonLabelObject = "IMESService.ShipToCartonLabel";
        public const string CNCardReceiveObject = "IMESService.CNCardReceive";
        public const string GenSMTMOObject = "IMESService.GenSMTMO";
        public const string ChangeASTObject = "IMESService.ChangeAST";
        public const string PCAOQCCosmeticObject = "IMESService.PCAOQCCosmetic";
        public const string TravelCardPrintExcelObject = "IMESService.TravelCardPrintExcel";
        public const string KeyPartsRequirementQueryObject = "IMESService.KeyPartsRequirementQuery";
        public const string CT13KeyPartsRequirementQueryObject = "IMESService.CT13KeyPartsRequirementQuery";
        public const string UpdateConsolidateObject = "IMESService.UpdateConsolidate";
        public const string PAQCCosmetic_rctoObject = "IMESService.PAQCCosmetic_rcto";
        public const string RCTOLabelPrintObject = "IMESService.RCTOLabelPrint";

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
        public const string ChangeMB = "IMESService.ChangeMB";

        //added for maintain
        public const string IModelProcess = "IMESMaintainService.IModelProcess";
        public const string IECRVersion = "IMESMaintainService.IECRVersion";
        public const string ISmallBoardECR = "IMESMaintainService.ISmallBoardECR";
        public const string ISmallBoardDefine = "IMESMaintainService.ISmallBoardDefine";

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
        public const string IPCBVersion = "IMESMaintainService.IPCBVersion";
        public const string IQTime = "IMESMaintainService.IQTime";
        public const string IPalletTypeforICC = "IMESMaintainService.IPalletTypeforICC";
        public const string IOSWIN = "IMESMaintainService.IOSWIN";

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
        public const string DismantlePilotRun = "IMESService.DismantlePilotRun";
        //Added by Benson 20110914
        public const string IOfflinePizzaKitting = "IMESService.OfflinePizzaKitting";
        public const string ITravelCardWithCollectKP = "IMESService.TravelCardWithCollectKP";
        public const string ICollectTabletFaPart = "IMESService.CollectTabletFaPart";
        //Added by Jessica Liu 20111013
        public const string AssetTagLabelPrintObject = "IMESService.AssetTagLabelPrint";

        // Added by liuqingbiao 20111101
        public const string AssignWHLocationObject = "IMESService.AssignWHLocation";
        public const string KittingInputObject = "IMESService.KittingInput";
        public const string KittingInputForTRISObject = "IMESService.KittingInputForTRIS";
        public const string COARemovalObject = "IMESService.COARemoval";
        public const string CombinePizzaObject = "IMESService.CombinePizza";
        public const string OfflineLabelPrintObject = "IMESService.OfflineLabelPrint";
        public const string BGAInputObject = "IMESService.BGAInput";

        //Added by Jessica Liu 20111020
        public const string SNCheckObject = "IMESService.SNCheck";
        //Added by Jessica Liu 20111031
        public const string BTChangeObject = "IMESService.BTChange";
        //Added by Jessica Liu 20111102
        public const string ImageDownloadObject = "IMESService.ImageDownload";
        //Added by Jessica Liu 20111122
        public const string OnlineGenerateASTObject = "IMESService.OnlineGenerateAST";
        //Added by Jessica Liu 20111202
        public const string CombineASTObject = "IMESService.CombineAST";
        //Added by Jessica Liu 2012010
        public const string MBCombineCPUObject = "IMESService.MBCombineCPU";
        //Added by Jessica Liu 20120314
        public const string PAQCRepairObject = "IMESService.PAQCRepair";
        //Added by Jessica Liu 20120511
        public const string ITCNDCheckQCHoldSettingObject = "IMESMaintainService.ITCNDCheckQCHoldSetting";
        //Added by Jessica Liu 20120612
        public const string KBCTCheckObject = "IMESService.KBCTCheck";
        //Added by Jessica Liu 20120711
        public const string SMTObjectiveTimeObject = "IMESMaintainService.SMTObjectiveTime";
        //Added by Jessica Liu 20120717
        public const string PAQCOutputForRCTOObject = "IMESService.PAQCOutputForRCTO";
        //Added by Jessica Liu 20120806
        public const string ConstValueMaintainObject = "IMESMaintainService.ConstValueMaintain";
        //Added by Jessica Liu 20120724
        public const string RCTOMBChangeObject = "IMESService.RCTOMBChange";
        public const string ConstValueTypeObject = "IMESMaintainService.ConstValueType";
        //Added by chenpeng 
        public const string LotSettingObject = "IMESMaintainService.LotSetting";
        public const string SMTLineSpeed = "IMESMaintainService.SMTLineSpeed";


        //Added by Chen Xu itc208014 
        public const string PodLabelCheckObject = "IMESService.PodLabelCheck";
        public const string PalletVerifyObject = "IMESService.PalletVerify";
        public const string PalletVerifyFDEOnlyObject = "IMESService.PalletVerifyFDEOnly";
        public const string PakUnitWeightObject = "IMESService.PakUnitWeight";
        public const string PakUnitWeightNewObject = "IMESService.PakUnitWeightNew";
        public const string MBSplitObject = "IMESService.MBSplit";
        public const string PCAOQCOutputObject = "IMESService.PCAOQCOutput";
        public const string ModelWeightObject = "IMESService.ModelWeight";

        public const string PCAOQCInputObject = "IMESService.PCAOQCInput";
        public const string CombinePcbinLotObject = "IMESService.CombinePcbinLot";

        //Added by ShhWang 20111027
        public const string IPODLabelPartManager = "IMESMaintainService.PODLabelPartManager";
        public const string IOfflineLabelSettingManager = "IMESMaintainService.OfflineLabelSettingManager";
        public const string IBTOceanOrderManager = "IMESMaintainService.BTOceanOrderManager";
        public const string IUniteMBManager = "IMESMaintainService.UniteMBManager";
        public const string ITestMBManager = "IMESMaintainService.TestMBManager";
        public const string IMBAssemblyCodeManager = "IMESMaintainService.MBAssemblyCodeManager";
        public const string SQEDefectReportManager = "IMESMaintainService.SQEDefectReportManager";

        //Added by wangxl
        public const string MaintainBatteryObject = "IMESMaintainService.Battery";
        public const string MaintainInternalCOAObject = "IMESMaintainService.InternalCOA";
        public const string MaintainKittingCodeObject = "IMESMaintainService.KittingCodeMaintain";
        public const string MaintainBomDescrObject = "IMESMaintainService.BomDescrMaintain";
        public const string MaintainVendorCodeObject = "IMESMaintainService.VendorCode";

        //Added by jianghao 
        public const string MaintainPalletQtyOject = "IMESMaintainService.PalletQty";
        public const string MaintainChepPalletObjet = "IMESMaintainService.ChepPalletManager";
        public const string FAFloatLocationObjet = "IMESMaintainService.FAFloatLocation";
        public const string PartTypeAttributeObjet = "IMESMaintainService.PartTypeAttribute";
        public const string ICASAObjet = "IMESMaintainService.ICASAObjet";

        //Kaisheng 
        public const string IPAKLABELLIGHTNOObject = "IMESMaintainService.IPAKLABELLIGHTNOMANAGER";
        public const string IFAMILYINFOObject = "IMESMaintainService.IFAMILYINFOMANAGER";


        //Added by Dorothy
        public const string IPrintContentWarranty = "IMESService.PrintContentWarranty";
        public const string IPDPALabel01 = "IMESService.PDPALabel01";
        public const string IPDPALabel02 = "IMESService.PDPALabel02";
        public const string IPizzaKitting = "IMESService.PizzaKitting";
        public const string IBGAOutput = "IMESService.BGAOutput";
        public const string IROMEOBattery = "IMESService.ROMEOBattery";
        public const string CombinePoInCartonObjectForRCTO = "IMESService.CombinePoInCartonForRCTO";
        public const string LCMDefectInputObjectForRCTO = "IMESService.LCMDefectInputForRCTO";
        public const string TPDLCheckObjectForRCTO = "IMESService.TPDLCheckForRCTO";
        public const string CombineTPMObject = "IMESService.CombineTPM";
        public const string PAQCSortingObject = "IMESService.PAQCSortingObject";
        public const string CombineCOAandDNNewObject = "IMESService.CombineCOAandDNNewObject";

        public const string ProductReinputObject = "IMESService.ProductReinput";

        public const string PackingPizzaObject = "IMESMaintainService.PackingPizza";

        //Added by itc200052 20111026
        public const string CombineKeyPartsObject = "IMESService._CombineKeyParts";
        //add by itc00052 2012-7-16
        public const string MaintainDeptObject = "IMESMaintainService.DeptManager";
        public const string HPPNLabelforRCTOObject = "IMESService.HPPNLabelforRCTO";
        public const string CombineCartonInDNObjectForRCTO = "IMESService.CombineCartonInDNForRCTO";


        //Added by 207003
        public const string CNCardStatusChangeObject = "IMESService._CNCardStatusChange";
        public const string COAStatusChangeObject = "IMESService._COAStatusChange";
        public const string CombineCOAandDNObject = "IMESService._CombineCOAandDN";
        public const string CombineDNPalletforBTObject = "IMESService._CombineDNPalletforBT";
        public const string UpdateShipDateObject = "IMESService._UpdateShipDate";
        public const string WHPalletControlObject = "IMESService._WHPalletControl";
        public const string FAMBReturnObject = "IMESService._FAMBReturn";
        public const string COAReturnObject = "IMESService._COAReturn";
        public const string CTLabelPrintObjectForRCTO = "IMESService.CTLabelPrintObjectForRCTO";
        public const string ConstValueType = "IMESService.ConstValueType";
        public const string ProductionPlanObject = "IMESService.ProductionPlan";
        public const string TravelCardPrintProductPlanObject = "IMESService.TravelCardPrintProductPlan";

        //add by xuyunfeng
        public const string Celdata = "IMESMaintainService.ICeldata";
        public const string MaintainBSamLocation = "IMESMaintainService.BSamLocation";
        public const string KeyPartsDefectCollection = "IMESMaintainService.KeyPartsDefectCollection";

        //add by Benson 
        public const string IESOPandAoiKbTest = "IMESService.ESOPandAoiKbTest";
        public const string IAoiOfflineKbCheck = "IMESService.AoiOfflineKbCheck";

        //add by VicLin
        public const string PCAReceiveReturnMBObject = "IMESService.PCAReceiveReturnMB";
        public const string MaintainAssemblyVCObject = "IMESMaintainService.IAssemblyVC";
        public const string ReleaseProductIDHoldObject = "IMESService.ReleaseProductIDHold";
        public const string EnergyLabelMaintainObject = "IMESMaintainService.IEnergyLabel";
        public const string IndornesiaLabelMaintainObject = "IMESMaintainService.IIndornesiaLabel";
        public const string HoldRuleMaintainObject = "IMESMaintainService.IHoldRule";
        public const string DefectHoldRuleMaintainObject = "IMESMaintainService.IDefectHoldRule";
        public const string CheckCombineCTObject = "IMESService.CheckCombineCT";
        public const string MaterialReturnObject = "IMESService.MaterialReturn";
        public const string WHInspectionObject = "IMESService.WHInspection";
        public const string PCAReceiveReturnMBForDockingObject = "IMESDockingService.PCAReceiveReturnMB";
        public const string PilotRunMOObject = "IMESService.PilotRunMO";
        public const string MaintainCheckItemTypeObject = "IMESMaintainService.CheckItemType";
        public const string PartForbidRuleMaintainObject = "IMESMaintainService.PartForbidRule";

        public const string MBAssignTestType = "IMESService.MBAssignTestType";
        public const string CheckAST = "IMESService.CheckAST";
        public const string ModuleApprovalItemObject = "IMESMaintainService.ModuleApprovalItem";
        public const string FAIFAReleaseObject = "IMESService.FAIFARelease";
        public const string FAIPAKReleaseObject = "IMESService.FAIPAKRelease";
        public const string DefectComponentRejudgeObject = "IMESService.DefectComponentRejudge";
        public const string DefectComponentPrintObject = "IMESService.DefectComponentPrint";
        public const string SAInputXRayObject = "IMESService.SAInputXRay";
        public const string SAMBRepairControl = "IMESService.SAMBRepairControl";
        public const string FIXTure = "IMESService.FIXTure";


        #endregion


        #region Docking objectUri
        public const string ICTInputDocking = "IMESDockingService.ICTInput";
        public const string ChangeModelDocking = "IMESDockingService.ChangeModel";

        //Add by itc200052
        public const string BoardInputForDockingObject = "IMESDockingService._BoardInputForDocking";
        public const string CombinePackObject = "IMESDockingService.CombinePack";
        public const string OfflineLabelPrintForDockingObject = "IMESDockingService.OfflineLabelPrint";

        //Added by Jessica Liu
        public const string GenerateCustomerSNForDockingObject = "IMESDockingService.GenerateCustomerSNForDocking";
        public const string PAQCOutputForDockingObject = "IMESDockingService.PAQCOutputForDocking";
        public const string REVLabelPrintForDockingObject = "IMESDockingService.REVLabelPrintForDocking";
        public const string OQCRepairForDockingObject = "IMESDockingService.OQCRepairForDocking";
        public const string PAQCRepairForDockingObject = "IMESDockingService.PAQCRepairForDocking";

        //Added by itc98066 Dorothy
        public const string CombinePoInCartonObject = "IMESDockingService.CombinePoInCarton";
        public const string PalletWeightForDocking = "IMESDockingService.PalletWeight";
        public const string PACosmeticObjectDocking = "IMESDockingService.PACosmeticForDocking";

        //Added by Kaisheng
        public const string FATestStationDocking = "IMESDockingService.FATestStation";
        public const string PCATestStationDocking = "IMESDockingService._PCATestStation";
        public const string CombinePcbinLotDocking = "IMESDockingService.CombinePcbInLot";

        //Added by itc208014
        public const string PCAOQCOutputObjectDocking = "IMESDockingService.PCAOQCOutput";
        public const string PCAOQCInputObjectDocking = "IMESDockingService.PCAOQCInput";
        //add by chenpeng
        public const string EPIAOutputObject = "IMESDockingService.EPIAOutput";
        public const string EPIAInputObject = "IMESDockingService.EPIAInput";
        public const string PalletVerifyDataForDockingObject = "IMESDockingService.PalletVerifyDataForDocking";
        public const string UnpackForDockingObject = "IMESDockingService.UnpackForDocking";
        public const string RegenerateCustSNForDockingObject = "IMESDockingService.RegenerateCustSNForDocking";
        //Add by itc202017
        public const string PakUnitWeightForDockingObject = "IMESDockingService.PakUnitWeight";
        public const string UploadShipmentDataForDockingObject = "IMESDockingService._UploadShipmentData";
        public const string PoDataForDockingObject = "IMESDockingService._PoData";
        public const string PCARepairForDockingObject = "IMESDockingService.PCARepair";
        public const string FRUCartonLabelForDockingObject = "IMESDockingService._FRUCartonLabel";

        //Add by itc202007
        public const string ProdIdPrintForDocking = "IMESDockingService.ProdIdPrintForDocking";
        public const string ShipToLabelPrintForDocking = "IMESDockingService.ShipToLabelPrintForDocking";
        public const string VirtualMoForDocking = "IMESDockingService.VirtualMoForDocking";
        public const string FARepairForDocking = "IMESDockingService.FARepairForDocking";
        public const string PCAOQCCosmeticForDocking = "IMESDockingService.PCAOQCCosmeticForDocking";

        public const string MBLabelPrintDocking = "IMESDockingService._MBLabelPrint";
        public const string GenSMTMODocking = "IMESDockingService.GenSMTMODocking";

        //Add by itc207003
        public const string CooLabel = "IMESDockingService._CooLabel";
        public const string UnpackAllbyDN = "IMESDockingService._UnpackAllbyDN";
        public const string DismantleForDocking = "IMESDockingService.DismantleForDocking";

        public const string FAMBReturnObjectDocking = "IMESDockingService._FAMBReturn";
        public const string CombineTmpObject = "IMESDockingService.CombineTmp";

        public const string AlarmCriteriaObject = "IMESAlarmService.AlarmCriteria";
        public const string AlarmCommonObject = "IMESAlarmService.CommonImp";
        public const string FPCBoardLabelPrintDocking = "IMESDockingService.FPCBoardLabelPrint";

        public const string MultiUnPack = "IMESService.MultiUnPack";

        public const string ConbimeOfflinePizza = "IMESService.ConbimeOfflinePizza";
        public const string PCAOQCCollection = "IMESService.PCAOQCCollection";

        public const string AssignModelObject = "IMESService.AssignModel";
        public const string MaintainAssignModelMgrObject = "IMESMaintainService.AssignModelMgr";
        public const string CheckItemTypeListObject = "IMESMaintainService.CheckItemTypeListMaintain";
        public const string LabelTypeRuleObject = "IMESMaintainService.LabelTypeRuleMaintain";
        public const string SpecialOrderObject = "IMESMaintainService.SpecialOrder";

        public const string UnpackCombinePizzaObject = "IMESService.UnpackCombinePizza";
        public const string UnpackOfflinePizzaObject = "IMESService.UnpackOfflinePizza";

        public const string FAIInputObject = "IMESService.FAIInput";
        public const string FAIOutputObject = "IMESService.FAIOutput";
        public const string FAIModelMaintain = "IMESMaintainService.FAIModelMaintain";
        public const string FAIModelRule = "IMESMaintainService.FAIModelRule";
        public const string CommonLabelPrint = "IMESService.CommonLabelPrint";

        #endregion

        #region other
        public const string CHANNELNAME = "IMESChanelClient";
        #endregion



        # region "session type"
        public const string SessionMB = "1";
        public const string SessionProduct = "2";
        public const string SessionCommon = "0";
        # endregion
    }
}
