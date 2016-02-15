﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Workflow.Runtime;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates;

namespace IMES.Infrastructure
{
    /// <summary>
    /// 一个站操作交互过程中的共享数据
    /// </summary>
    [Serializable]
    public class Session : IValueProvider
    {
        /// <summary>
        /// Session中可用的变量名
        /// </summary>
        public static class SessionKeys
        {
            public static string ValueToCheck = "ValueToCheck"; //用户输入的检查值, 用于Explicit Check/Part Check
            public static string MatchedCheckItem = "MatchedCheckItem"; //根据用户输入的检查值匹配到的CheckItem, 用于Explicit Check
            public static string MatchedParts = "MatchedPart"; //IList<IBomPart>根据用户输入的检查值匹配到的BomPart, 用于Part Check
            public static string ReturnStation = "ReturnStation";
            public static string PCode = "PCode"; // 页面代号

            ///string  由用户输入的用于Explicit ItemCheck的检查值
            public const string SessionBom = "SessionBom"; //当站使用的session bom
            public const string ProductID = "ProductID";   //IProductID
            public const string Product = "Product";          //IProduct
            public const string MBMONO = "MBMONO";    //string  
            public const string ModelName = "ModelName";        //string
            public const string ModelForRule = "ModelForRule";
            public const string ModelObj = "ModelObj";        //IMES.FisObject.Common.Model.Model
            public const string FamilyName = "FamilyName";      //string
            public const string Qty = "Qty";            //string
            public const string IsExperiment = "IsExperiment"; //bool 是否为试产

            public const string PrintLogName = "PrintLogName";  //string
            public const string PrintLogBegNo = "PrintLogBegNo"; //string
            public const string PrintLogEndNo = "PrintLogEndNo"; //string
            public const string PrintLogDescr = "PrintLogDescr"; //string

            public const string DateCode = "DateCode";        //string
            public const string IsNextMonth = "IsNextMonth";      //bool
            public const string MBNOList = "MBNOList";      //List<string>
            public const string GeneratedNoList = "GeneratedNoList";      //List<string>

            public const string MB = "MB";      //IMB
            public const string SVB = "SVB";    //SVB
            public const string VGA = "VGA"; 
            public const string MBList = "MBList";      //List<IMB>
            public const string DefectList = "DefectList";      //List<string>
            public const string FixtureID = "FixtureID";      //string
            public const string MAC = "MAC";      //string
            public const string UUID = "UUID";      //string
            public const string ECR = "ECR";      //string
            public const string IECVersion = "IECVersion";      //string
            public const string CPUVendorSn = "CPUVendorSn";

            public const string WFTerminated = "WFTerminated"; //bool

            public const string TypeOfSNRule = "TypeOfSNRule"; //string
            public const string SerialNumbersGenerated = "SerialNumbersGenerated"; //SequencialNumberRanges

            public const string IsComplete = "IsComplete";

            public const string MBCode = "MBCode"; //string
            public const string MBType = "MBType"; //string
            public const string MBLog = "MBLog"; //MBLog

            public const string presentTime = "presentTime"; //DateTime
            public const string RaiseExceptionToSession = "RaiseExceptionToSession";

            public const string PrintItems = "PrintItems";

            public const string CurrentRepairdefect = "CurrentRepairdefect";  //MBRepairDefect
            public const string MaintainAction = "MaintainAction";  //int 0, add; 1, update; 2, delete;
            public const string RepairDefectID = "RepairDefectID";

            public const string PCARepairInputOutputFlag = "PCARepairInputOutputFlag";  //string 'I', input; 'O' output;
            public const string RepairStation = "RepairStation"; // 'PCA', 'FA', 'OQC', 'PAK'
            public const string RepairTimes = "RepairTimes";

            public const string RandomInspectionStation = "RandomInspectionStation"; //"EOQC", "OQC", or "SKIP"
            public const string PartID = "PartID";
            public const string VendorSN = "VendorSN";
            public const string PartSN = "PartSN";//PartSN
            public const string KPType = "KPType";
            public const string VendorDCode = "VendorDCode";

            public const string NewMB = "NewMB";
            public const string OldMB = "OldMB";
            public const string MBMO = "MBMO";  //Object
            public const string MBMONOList = "MBMONOList";  //IList<string>

            public const string BatteryPN = "BatteryPN";

            public const string CustomVersion = "CustomVersion";

            public const string ActuralWeight = "ActuralWeight";
            public const string StandardWeight = "StandardWeight";
            public const string Tolerance = "Tolerance";
            public const string ModelWeight = "ModelWeight";
            public const string CartonWeight = "CartonWeight";

            public const string Carton = "Carton";

            public const string Pallet = "Pallet";//Pallet object
            public const string VirtualCarton = "VirtualCarton";

            public const string VirtualPallet = "VirtualPallet";

            public const string FirstProductModel = "FirstProductModel";
            public const string NeedCheckPalletWeight = "NeedCheckPalletWeight";//bool
            public const string PalletWeight = "PalletWeight";//

            public const string Delivery = "Delivery";//Delivery Object
            public const string DeliveryNo = "DeliveryNo";//string
            public const string PalletNo = "PalletNo";//string
            public const string UCC = "UCC";//string

            public const string Pizza = "PizzaID";
            public const string NewCheckedPizzaParts = "NewCheckedPizzaParts";
            public const string NewScanedProductIDList = "NewScanedProductIDList";//List<string>
            public const string NewScanedProductModelList = "NewScanedProductModelList";//List<string>
            public const string NewScanedProductLineList = "NewScanedProductLineList";//List<string>
            public const string NewScanedProductCustSNList = "NewScanedProductCustSNList";//List<string>

            public const string startProdId = " startProdId";
            public const string endProdId = "endProdId";

            public const string StartMBNO = " StartMBNO";
            public const string EndMBNO = "EndMBNO";

            public const string motherOrChild = "motherOrChild";  //=0,母板; =1~9,子板

            public const string ProdList = "ProdList";            //IList<IProduct>
            public const string MONO = "MONO";//string
            public const string ProdMO = "ProdMO";//MO
            public const string ProdNoList = "ProdNoList";   //IList<string>

            public const string PN111 = "PN111";
            public const string PN111Code = "PN111Code"; //string :  111 Level Part 的Code 属性 (PartInfo.InfoType ='Code' 记录的InfoValue)
            public const string IsMassProduction = "IsMassProduction";
            public const string Remark = "Remark";
            public const string Reason = "Reason";
            public const string Floor = "Floor";

            public const string HasDefect = "HasDefect"; //PaqcRepair用来表示是否有Defect

            public const string CustSN = "CustSN"; //PaqcRepair用来表示是否有Defect

            public const string MultiQty = "MultiQty";   //ict input 连板Qty

            public const string ExplicityCheckItemList = "ExplicityCheckItemList";  //IList<ICheckItem>
            public const string LoopCount = "LoopCount";// int,added by itc207013,used to indict loop times
            public const string MBSN = "MBSN";//string,added by itc207013,used for child mb sn

            public const string MachineNo = "MachineNo";
            public const string OriginalHDD = "OriginalHDD";
            public const string ConnectNo = "ConnectNo";
            public const string LogType = "LogType";
            public const string _1397No = "_1397No";

            public const string LineCode = "LineCode"; //string
            public const string SFGCustomizingSite = "SFGCustomizingSite"; //string
            public const string ProductionSite = "ProductionSite"; //string
            public const string CustomSnList = "CustomSnList"; //List<string>
            public const string CTList = "CTList"; //List<string>

            public const string AssemblyCode = "AssemblyCode"; //string
            public const string CustomerCode = "CustomerCode"; //string

            public const string SelectedWarrantyRuleID = "SelectedWarrantyRuleID"; //IMES.FisObject.Common.Warranty.Warranty
            public const string WarrantyCode = "WarrantyCode"; //string
            public const string ShipTypeCode = "ShipTypeCode"; //string
            public const string DCode = "DCode"; //string
            public const string VCode = "VCode"; //string
            public const string SVBCode = "SVBCode"; //string
            public const string V8 = "V8"; //string
            public const string SVBSnList = "SVBSnList"; //List<string>
            public const string boxId = "boxId";
            public const string FRUNO = "FRUNO";
            public const string PartNo = "PartNo";//string
            public const string Version = "Version";//string

	        public const string CheckBattery = "CheckBattery"; //string

            public const string VirtualMOIdentifier = "VirtualMOIdentifier"; //string
            public const string VirtualMOList = "VirtualMOList"; //List<string>

            public const string PCS = "PCS"; //int
            public const string OldMONO = "OldMONO";
            public const string NewMONO = "NewMONO";

            public const string BTDLSN = "BTDLSN";//string LCM BTDLSN
            public const string TPDLSN = "TPDLSN";//string LCM TPDLSN
            public const string BTDLLCMCTNO = "BTDLLCMCTNO";//string LCM BTDL LCMCTNO
            public const string TPDLLCMCTNO = "TPDLLCMCTNO";//string LCM TPDL LCMCTNO


            public const string ProductIDOrCustSN = "ProductIDOrCustSN";
            public const string ProductIDOrCustSNOrCarton = "ProductIDOrCustSNOrCarton";
            public const string ProductIDOrCustSNOrPallet = "ProductIDOrCustSNOrPallet";

            public const string TPCB = "TPCB";  //string: TPCB
            public const string TP = "TP";   //string: Touch Pad
            public const string Editor = "Editor";//string: Editor
            public const string ifElseBranch = "ifElseBranch";  //string: Branch
            public const string VCodeInfoLst = "VCodeInfoLst";    // List<string>
            public const string TPCBType = "TPCBType"; // string : Type


            public const string GiftNoList = "GiftNoList";   //IList<string>
            public const string PizzaNoList = "PizzaNoList";   //IList<string>
            public const string VirtualCartonNoList = "VirtualCartonNoList";   //IList<string>
            public const string VirtualPalletNoList = "VirtualPalletNoList";   //IList<string>
            public const string CartonNoList = "CartonNoList";   //IList<string>

            public const string CN = "CN";   //string
            public const string FRUCartonNoList = "FRUCartonNoList";   //IList<string>
            public const string C = "C";    //string
            public const string GiftPartNoList = "GiftPartNoList";   //IList<string>
            public const string GiftScanPartList = "GiftScanPartList";   //IList<IList<string>>
            public const string GiftScanPartCount = "GiftScanPartCount";   //int
            public const string GiftID = "GiftID";   //string
            public const string Borrower = "Borrower";   //string
            public const string Lender = "Lender";   //string
            public const string BorrowOrReturn = "BorrowOrReturn";   //string

            public const string IECPN = "IECPN"; //string
            public const string CreateDateTime = "CreateDateTime"; //CreateDateTime 
            public const string TPCBInfoLst = "TPCBInfoLst";  // List<string> 
            public const string AXH = "AXH";   //string
            public const string isDNFull = "isDNFull";  // Boolean (true/false)
            public const string Forwarder = "Forwarder";
            public const string PickIDCtrl = "PickIDCtrl";
            public const string ForwarderPallet = "ForwarderPallet";

            public const string PalletIdFixedPrefix = "PalletIdFixedPrefix";
            public const string PalletIdList = "PalletIdList";
            public const string DNCombineProduct = "DNCombineProduct";
            public const string CartonSNList = "CartonSNList";//List<string>
            public const string Lines = "Lines";
            public const string DOCType = "DOCType";
            public const string Carrier = "Carrier";
            public const string IsPalletChange = "IsPalletChange"; //bool 是否变化
            public const string DeliveryList = "DeliveryList"; //delivery list of the same pallet 
            public const string ModelExist = "ModelExist";//判断刷入的model，在Model表中是否存在
            public const string IsBT = "IsBT";
            public const string DNInfoValue = "DNInfoValue";
            public const string Consolidated = "Consolidated";//Delivery No 的Consolidated 属性
            public const string QCStatus = "QCStatus"; // FA..QCStatus 
            public const string QCStatusList = "QCStatusList"; //IList<string>

            public const string WHLocationObj = "WHLocationObj";//PakBtLocMasInfo Object
            public const string WHLocation = "WHLocation";//string

            public const string ChepPallet = "ChepPallet";//string



            public const string COASN = "COASN";//string

            public const string COASNList = "COASNList";//IList<string>

            public const string COAStatus = "COAStatus";//COAStatus

            public const string DummyPalletCase = "DummyPalletCase";    // string: Dummy Pallet Case (NA/NANon/BA/BANon)
            public const string DummyPalletNo = "DummyPalletNo";    // string: Dummy Pallet No
            public const string GenerateDummyPalletNo = "GenerateDummyPalletNo";    // string: Generate DummyPalletNo
            public const string DummyShipDet = "DummyShipDet";    // Object: Dummy Ship Det 类型
            public const string ScanQty = "ScanQty";    // string: FDE Pallet Verfiy Scan Qty
            public const string PalletQty = "PalletQty";    // string: FDE Pallet Verfiy Scan Qty
            public const string RMN = "RMN"; // string
            public const string labelBranch = "labelBranch";
            public const string InfoValue = "InfoValue";//string:InfoValue

            public const string ShipDate = "ShipDate";//DateTime:ShipDate Of Delivery

            public const string WHPltMas = "WHPltMas";//WHPltMasInfo object

            public const string AttributeValue = "AttributeValue";//string:AttributeValue

            public const string PizzaID = "PizzaID";// string :PizzaID

            public const string isPassStationLog = "isPassStationLog"; // Boolean(true/false) : 是否存在过站记录
            public const string PrintLogList = "PrintLogList";  //IList<ProductLog> 
            public const string IsOceanShipping = "IsOceanShipping"; //Boolean : (CombinePizz) 是否海运出货
            public const string PizzaKitWipBuffer = "PizzaKitWipBuffer";// IList<WipBufferDef>

            public const string Pno = "Pno";//string

            public const string Action = "Action";//string

            public const string PCBModel = "PCBModel"; //string:  PCB表类型
            public const string PCBModelID = "PCBModelID"; //string:  PCB.PCBModelID
            public const string IsPolitRun = "IsPolitRun";//bool

            public const string IsCheckPass = "IsCheckPass"; //bool
            public const string AOINo = "AOINo";//string

            public const string EEP = "EEP";//string

            public const string MBCT = "MBCT";//string

            public const string EEPList = "EEPList";//IList<string> 

            public const string MBCTList = "MBCTList";//IList<string> 

            public const string MACList = "MACList";//IList<string> 

            public const string MBSNOList = "MBSNOList"; //IList<string>

            public const string ReturnWC = "ReturnWC";//string

            public const string StartDate = "StartDate"; //string: StartDate 

            public const string StationDescr = "StationDescr";//string

            public const string lockToken_DN = "lockToken_DN";//Guid
            public const string lockToken_Pallet = "lockToken_Pallet";//Guid
            public const string lockToken_Loc = "lockToken_Loc";//Guid
            public const string lockToken_Box = "lockToken_Box";//Guid
            public const string lockToken_Ucc = "lockToken_Ucc";//Guid

            public const string LotNo = "LotNo";  // string 
            public const string LotNoList = "LotNoList"; //IList<string>

            public const string ProductPartList = "ProductPartList";
            public const string Model1 = "Model1";
            public const string Model2 = "Model2";
            public const string ASTInfoList = "ASTInfoList";
            public const string Prod1 = "Prod1";
            public const string Prod2 = "Prod2";
            public const string DnIndex = "DnIndex";
            public const string DnCount = "DnCount";
            public const string SelectStation = "SelectStation";//string
            public const string StationTable = "StationTable";//DataTable
            public const string ChangeModelCheckItem = "ChangeModelCheckItem";//string
            public const string ProductIDListStr = "ProductIDListStr";//string
            public const string RCTOChildMBSnList = "RCTOChildMBSnList";//List<string>
            public const string IsRCTO = "IsRCTO";//bool
            public const string CheckCode = "CheckCode";//bool

            public const string PalletCollectionUI = "PalletCollectionUI";//PalletCollectionUI
            public const string PicPositionName = "PicPositionName";//string[][]
            public const string LabelTypeList = "LabelTypeList";// selected LabelType List 

            //for Material
            public const string MaterialType = "MaterialType";
            public const string MaterialCT = "MaterialCT";
            public const string MaterialCTList = "MaterialCTList";
            public const string Material = "Material";
            public const string MaterialList = "MaterialList";

            public const string MaterialBoxId = "MaterialBoxId";
            public const string MaterialBoxIdList = "MaterialBoxIdList";
            public const string MaterialBox = "MaterialBox";
            public const string MaterialBoxList = "MaterialBoxList";

            public const string MaterialLotNo = "MaterialLotNo";
            public const string MaterialLotNoList = "MaterialLotNoList";
            public const string MaterialLot = "MaterialLot";
            public const string MaterialLotList = "MaterialLotList";

            public const string PCBNoList = "PCBNoList";
            public const string SkuModel = "SkuModel";

            public const string CartonSN = "CartonSN";
            public const string ShipMode = "ShipMode";
            public const string RCTO146Category = "RCTO146Category";

            //for Pilot Run
            public const string PilotMoNo = "PilotMoNo";
            public const string PilotMo = "PilotMo";
            public const string PilotMoQty = "PilotMoQty";
            public const string PilotMoSuffix = "PilotMoSuffix";

            //for LineObject
            public const string LineInfo = "LineInfo";     //Line Object


            //for PalletTypeRuleInfo
            public const string PalletTypeRuleInfo = "PalletTypeRuleInfo"; //PalletTypeRule object

            //for CheckPrintedLog
            public const string HasAllPrintedLog = "HasAllPrintedLog"; //string

            public const string PartNotice = "PartNotice";  //string

            //for all 
            public const string ProductIDList = "ProductIDList";//IList<string>
            //public const string CustSNList = "CustSNList";//IList<string>

            public const string FloorAreaLoc = "FloorAreaLoc";//FloorAreaLoc object
            public const string Area = "Area";//Area string
            public const string Location = "Location"; //string FloorAreaLoc.LocID

            public const string Message = "Message"; //string
            public const string PackMode = "PackMode"; //string Carton/Device/Pallet

            public const string DummyPallet = "DummyPallet"; //Pallet
            public const string DummyPalletDeviceQty = "DummyPalletDeviceQty"; //int Qty
            public const string PackPalletMode = "PackPalletMode";//string DummyPallet/UnPackPallet/NewPallet/AssignedPallet  

            public const string AvailableDeliveryList = "AvailableDeliveryList";//IList<DeliveryForRCTO146>
            public const string SelectedDeliveryPalletList = "SelectedDeliveryPalletList";//IList<AvailableDeliveryPallet>
            public const string AssignDeliveryPallet = "AssignDeliveryPallet";//AvailableDeliveryPallet
            public const string DeliveryNoList = "DeliveryNoList"; //IList<string>

            public const string CartonProdList = "CartonProdList"; //IList<IProduct> 
            public const string NewScanedCartunSNList = "NewScanedCartunSNList";//List<string>

            public const string DeliveryPalletInfoList = "DeliveryPalletInfoList";//List<DeliveryPalletInfo>
            public const string PltForecastWeightTolerance = "PltForecastWeightTolerance"; //string
            public const string EmptyPltStardardWeight = "EmptyPltStardardWeight"; // string
            public const string ProductWeight = "ProductWeight";// string
            public const string OverStandardTolerance = "OverStandardTolerance";// string
            public const string OverForecastTolerance = "OverForecastTolerance";// string
            public const string PltForecastWeight = "PltForecastWeight";//decimal

            public const string PalletList = "PalletList"; //IList<Pallet>

            public const string SAPDeliveryFile = "SAPDeliveryFile"; //string
            public const string SAPDeliveryPalletFile = "SAPDeliveryPalletFile"; //string
            public const string DeliveryPalletList = "DeliveryPalletList";// IList<DeliveryPalletInfo>
            public const string NotSavePalletList = "NotSavePalletList";//IList<Pallet>

            public const string MRPPrice = "MRPPrice"; // string
            public const string MRPWeight = "MRPWeight"; // string
            public const string WIFILogo = "WIFILogo"; // string
            public const string WIFIDescrList = "WIFIDescrList"; //IList<string>
            public const string DeviceConfigList = "DeviceConfigList";//IList<string>

            public const string UploadDNInfoList = "UploadDNInfoList";//IList<UploadDNInfo>

            public const string FwdPltInList = "FwdPltInList";//IList<FwdPltInfo>
            public const string FwdPltOutList = "FwdPltOutList";//IList<FwdPltInfo>

            public const string PasteLabelTemplate = "PasteLabelTemplate";//string

            public const string MoBOMList = "MoBOMList";//IList<MoBOMInfo>
            public const string RemainQty = "RemainQty";            //string

            //For Assset Tag
            public const string NeedCombineAstDefineList = "NeedCombineAstDefineList"; //IList<AstDefinitInfo>
            public const string NeedCombineAstPartList = "NeedCombineAstPartList"; //IList<IPart>
            public const string NeedGenAstPartList = "NeedGenAstPartList"; //IList<IPart>
            public const string NeedGenAstDefineList = "NeedGenAstDefineList"; //IList<IPart>
            public const string ImageFileList = "ImageFileList"; //IList<string>
            public const string HasMN2 = "HasMN2"; //string   Y/N
            public const string GenerateASTSN = "GenerateASTSN"; //string   Y/N
            public const string HasCDSI = "HasCDSI"; //string   Y/N 
            public const string HasCNRS = "HasCNRS"; //string   Y/N
            public const string AVPartNo = "AVPartNo"; //string   
            public const string CDSIPart = "CDSIPart"; //IPart
            public const string CNRSPart = "CNRSPart"; //IPart
            public const string HasATSN5 = "HasATSN5"; //string   Y/N

           //for TestLogFile
            public const string TestLogType = "TestLogType"; //string
            public const string TestLogFileName = "TestLogFileName"; //string
            public const string TestToolIP = "TestToolIP"; //string
            public const string TestLogFileRemark = "TestLogFileRemark"; //string

            //for test count limitation
            public const string IsOverTestCount = "IsOverTestCount"; //string
            public const string TestCountLimitation = "TestCountLimitation"; //string

            //For PO Change
            public const string SrcProduct = "SrcProduct"; //IProduct
            public const string DestProduct = "DestProduct"; //IProduct

            //For UPS
            public const string UPSCombinePO = "UPSCombinePO"; // UPSCombinePO object
            public const string IsUPSDevice = "IsUPSDevice"; // Y,N

            //For IPC other BU Acer,ASUS ... 
            public const string PalletNoList = "PalletNoList";  //IList<String>

            public const string SAPDelivery = "SAPDelivery"; //SAP Delivery for PO
            public const string SAPDeliveryPallet = "SAPDeliveryPallet"; //SAP Delivery_Pallet 

            public const string SAPDeliveryPalletList = "SAPDeliveryPalletList"; //List for SAP Delivery_Pallet       
            public const string SAPPallet = "SAPPallet"; //Pallet
            public const string SAPPalletNo = "SAPPalletNo";//string SAP Pallet No
            public const string PalletMode = "PalletMode"; //string, PO,DNPO, Dummy
            public const string PalletType = "PalletType"; //PalletType object
            public const string NeedDeleteRelativeDN = "NeedDeleteRelativeDN"; //string
            public const string TotalCartonQty = "TotalCartonQty";//int
            public const string BindCartonQty = "BindCartonQty";//int 
            public const string DeleteSAPDeliveryList = "DeleteSAPDeliveryList";//IList<Delivery>

            public const string UnpackDefineInfo = "UnpackDefineInfo"; //UnpackDefineInfo
            public const string CartonList = "CartonList"; //IList<Carton>

            public const string PoNo = "PoNo";//string
            public const string PoItem = "PoItem";//string
            public const string PoNoItem = "PoNoItem";//string
            public const string ReturnCode = "ReturnCode"; //string

            public const string IsBindDN = "IsBindDN"; //string
            public const string IsShipment = "IsShipment"; //string,Y/N
            public const string CompareShipmentInfoList = "CompareShipmentInfoList"; //IList<CompareShipmentInfo>

            public const string CheckMaterialItem = "CheckMaterialItem";  //CheckMaterialItem
            public const string CheckMaterialItemList = "CheckMaterialItemList";  //IList<CheckMaterialItem>
            public const string CheckItemType = "CheckItemType";  //string
            public const string MatchMaterialItem = "MatchMaterialItem";  //CheckMaterialItem

            public const string CheckInputCTByPartMatch = "CheckInputCTByPartMatch"; //IList<ConstValueInfo>
            public const string NeedHoldDevice = "NeedHoldDevice"; //string Y/N
            public const string HoldStation = "HoldStation";

            public const string PrintPoItemList = "PrintPoItemList";//IList<PrintPoItem>

            public const string IsDummyModel = "IsDummyModel";//string Y/N
            public const string SpecialOrderName = "SpecialOrderName"; //string
            public const string SpecialOrderValue = "SpecialOrderValue"; //string
            public const string IsSpecialOrder = "IsSpecialOrder";//Y/N
            public const string DummyModel = "DummyModel";//string
        }

        /// <summary>
        /// Session类型
        /// </summary>
        public enum SessionType
        {
            Common = 0,
            MB,
            Product,
            
        }

        internal static string GetSessionTypeStr(SessionType sessType)
        {
            return string.Format("{0}", Enum.GetName(sessType.GetType(), sessType));
        }

        internal static string GetGlobalSessionKey(Session sess)
        {
            return sess._key + GetSessionTypeStr(sess._type);
        }

        internal static string GetGlobalSessionKey(string key, Session.SessionType type)
        {
            return key + GetSessionTypeStr(type);
        }


        #region . Kernel Factors .
        [NonSerialized]
        private WorkflowInstance _workflowInstance;

        /// <summary>
        /// 与session关联的wf实例
        /// </summary>
        public WorkflowInstance WorkflowInstance
        {
            get { return this._workflowInstance; }
        }

        /// <summary>
        /// Guid of session
        /// </summary>
        public Guid Id
        {
            get
            {
                if (_workflowInstance != null)
                    return this._workflowInstance.InstanceId;
                else
                    return Guid.Empty;
            }
        }

        private Exception _exception;

        /// <summary>
        /// wf 执行期间产生的,不应使wf中断的Excepiton
        /// </summary>
        public Exception Exception
        {
            get { return this._exception; }
            set { this._exception = value; }
        }

        private string _key;

        /// <summary>
        /// session的键值
        /// </summary>
        public string Key
        {
            get { return _key; }
        }

        private SessionType _type;

        /// <summary>
        /// Session的种类
        /// </summary>
        public SessionType Type
        {
            get { return _type; }
        }

        public Session(string key, SessionType type, string editor, string station, string line, string customer)
        {
            this._key = key;
            _unitOfWork = new UnitOfWork.UnitOfWork();
            this._type = type;

            _editor = editor;
            _station = station;
            _line = line;
            _customer = customer;
            _cdt = DateTime.Now;
        }

        /// <summary>
        /// 将wf实例与Session建立关联
        /// </summary>
        /// <param name="instance">wf实例</param>
        public void SetInstance(WorkflowInstance instance)
        {
            this._workflowInstance = instance;
        }
        #endregion

        #region . Basic Properties

        private string _editor;
        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get { return this._editor; }
            set { this._editor = value; }
        }

        private string _line;
        /// <summary>
        /// Line
        /// </summary>
        public string Line
        {
            get { return this._line; }
            set { this._line = value; }
        }

        private string _station;
        /// <summary>
        /// Station
        /// </summary>
        public string Station
        {
            get { return this._station; }
            set { this._station = value; }
        }

        private string _customer;
        /// <summary>
        /// Customer
        /// </summary>
        public string Customer
        {
            get { return this._customer; }
            set { this._customer = value; }
        }

        private DateTime _cdt = DateTime.Now;
        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return this._cdt; }
        }

        #endregion

        #region . Customized Values .

        [NonSerialized]
        readonly Hashtable _values = new Hashtable();

        /// <summary>
        /// 获取指定Session变量值
        /// </summary>
        /// <param name="key">变量名</param>
        /// <returns>变量值</returns>
        public object GetValue(string key)
        {
            lock (this._values)
            {
                if (this._values.ContainsKey(key))
                    return this._values[key];
            }
            return null;
        }
        /// <summary>
        /// generic cast type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            object ret = null;
            ret = GetValue(key);
            if (ret == null)
            {
                return default(T);
            }
            else
            {
                return (T)ret;
            }
        }

        /// <summary>
        /// generic cast type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, T defaultValue)
        {
            object ret = null;
            ret = GetValue(key);
            if (ret == null)
            {
                return defaultValue;
            }
            else
            {
                return (T)ret;
            }
        }

        /// <summary>
        /// 添加指定变量到Session
        /// </summary>
        /// <param name="key">变量名</param>
        /// <param name="value">变量值</param>
        public void AddValue(string key, object value)
        {
            lock (this._values)
            {
                if (this._values.ContainsKey(key))
                    this._values[key] = value;
                else
                    this._values.Add(key, value);
            }
        }

        /// <summary>
        /// 删除指定session变量
        /// </summary>
        /// <param name="key">变量名</param>
        public void RemoveValue(string key)
        {
            lock (this._values)
            {
                if (this._values.ContainsKey(key))
                    this._values.Remove(key);
            }
        }

        #endregion

        #region . Synchronization .

        [NonSerialized]
        private AutoResetEvent _waitWorkflow = new AutoResetEvent(false);
        ///// <summary>
        ///// 应用层等待wf执行的AutoResetEvent
        ///// </summary>
        //public AutoResetEvent WaitWorkflowHandle
        //{
        //    get { return this._waitWorkflow; }
        //    set { this._waitWorkflow = value; }
        //}

        [NonSerialized]
        private AutoResetEvent _waitHost = new AutoResetEvent(false);

        private readonly IUnitOfWork _unitOfWork;
        private object _input;
        private object _output;

        ///// <summary>
        ///// wf等待应用层执行的AutoResetEvent
        ///// </summary>
        //public AutoResetEvent WaitHostHandle
        //{
        //    get { return this._waitHost; }
        //    set { this._waitHost = value; }
        //}

        /// <summary>
        /// 完成数据写入功能的UnitOfWork对象实例
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        /// <summary>
        /// wf执行过程中的输入参数
        /// </summary>
        public object InputParameter
        {
            get { return _input; }
            set { _input = value; }
        }

        /// <summary>
        /// wf执行过程中的输入参数
        /// </summary>
        public object OutputParameter
        {
            get { return _output; }
            set { _output = value; }
        }

        /// <summary>
        /// indicate session completed
        /// </summary>
        private int _completed = 0;
        /// <summary>
        /// over 0 indicate session complete
        /// </summary>
        public int IsCompleted
        {
            get { return _completed; }
        }
        /// <summary>
        /// Set Session Completed
        /// </summary>
        /// <returns></returns>
        public int SetSessionCompleted()
        {
            return Interlocked.CompareExchange(ref _completed, 1, 0);
        }

        /// <summary>
        /// counting host thread
        /// </summary>
        private int _hostWaitingCount = 0;
        /// <summary>
        /// Get counting host thread
        /// </summary>
        public int GetHostWaitingCount
        {
            get { return _hostWaitingCount; }
        }

        /// <summary>
        /// Counting wf thread
        /// </summary>
        private int _wfWaitingCount = 0;
        /// <summary>
        /// Get wf thread count
        /// </summary>
        public int GetWFWaitingCount
        {
            get { return _wfWaitingCount; }
        }
        /// <summary>
        /// Release and Close WF Thread
        /// </summary>
        public void WFThreadClose()
        {
            while (_wfWaitingCount > 0)
            {
                this.ResumeWorkFlow();
                Thread.Sleep(5);
            }
            this._waitWorkflow.Close();
        }

        /// <summary>
        /// Release and Close Host Thread
        /// </summary>
        public void HostThreadClose()
        {
            while (_hostWaitingCount > 0)
            {
                this.ResumeHost();
                Thread.Sleep(5);
            }
            this._waitHost.Close();
        }

        #region . Host Side .

        /// <summary>
        /// 执行绪从wf转至host
        /// </summary>
        public void SwitchToHost()
        {
            this._waitHost.Set();
            if (!this._waitWorkflow.SafeWaitHandle.IsClosed)
            {
                Interlocked.Increment(ref _wfWaitingCount);
                this._waitWorkflow.WaitOne();
                Interlocked.Decrement(ref _wfWaitingCount);
            }
        }

        /// <summary>
        /// 执行绪从wf转至host
        /// </summary>
        public bool SwitchToHost(int millisecondsTimeout)
        {
            this._waitHost.Set();
            return this._waitWorkflow.WaitOne(millisecondsTimeout, false);
        }

        /// <summary>
        /// 恢复host执行
        /// </summary>
        public void ResumeHost()
        {
            if (!this._waitHost.SafeWaitHandle.IsClosed)
            {
                this._waitHost.Set();               
            }
        }

        /// <summary>
        /// 阻塞wf执行
        /// </summary>
        public void SetWorkFlowWaitOne()
        {
            if (!this._waitWorkflow.SafeWaitHandle.IsClosed)
            {
                Interlocked.Increment(ref _wfWaitingCount);
                this._waitWorkflow.WaitOne();
                Interlocked.Decrement(ref _wfWaitingCount);
            }
        }

        #endregion

        #region . WF Side .
       

        /// <summary>
        /// 将执行绪转至wf
        /// </summary>
        public void SwitchToWorkFlow()
        {
            this._waitWorkflow.Set();
            if (!this._waitHost.SafeWaitHandle.IsClosed && _completed == 0)
            {
                Interlocked.Increment(ref _hostWaitingCount);
                this._waitHost.WaitOne();
                Interlocked.Decrement(ref _hostWaitingCount);
            }
        }

        /// <summary>
        /// 将执行绪转至wf
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool SwitchToWorkFlow(int millisecondsTimeout)
        {
            this._waitWorkflow.Set();
            return this._waitHost.WaitOne(millisecondsTimeout, false);
        }

        /// <summary>
        /// 恢复wf执行
        /// </summary>
        public void ResumeWorkFlow()
        {
            if (!this._waitWorkflow.SafeWaitHandle.IsClosed)
                this._waitWorkflow.Set();
        }

        /// <summary>
        /// 阻塞host执行
        /// </summary>
        public void SetHostWaitOne()
        {
            if (!this._waitHost.SafeWaitHandle.IsClosed && _completed==0)
            {
                Interlocked.Increment(ref _hostWaitingCount);
                this._waitHost.WaitOne();
                Interlocked.Decrement(ref _hostWaitingCount);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return GetGlobalSessionKey(this);
        }
    }
}
