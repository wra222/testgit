﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates;
namespace IMES.Infrastructure.Extend
{
      [Serializable]
    public class ExtendSession //:IMES.Infrastructure.Session
    {
          //public ExtendSession(string key, SessionType type, string editor, string station, string line, string customer):base(key,type,editor,station,line,customer) 
          //{   }
          public static class SessionKeys
          {             
              //public static string ValueToCheck = "ValueToCheck";
              public static string ModelType = "ModelType";
              public static string StartRange = "StartRange";
              public static string EndRange = "EndRange";
              public static string PlantCode = "PlantCode";
              public static string CfgCode = "CfgCode";
              public static string DeliveryDate = "DeliveryDate";
              public static string ProductRepairID = "ProductRepairID";
              //public static string RepairDefect = "RepairDefect";
              public static string OQCRepairStation = "OQCRepairStation";
              public static string IsOQCRepair = "IsOQCRepair";
              public static string ShipMode = "ShipMode";
              public static string IsCorrectVersion = "IsCorrectVersion";
              public static string ForceEOQC = "ForceEOQC";
              public static string isCreate = "isCreate";
              public static string IsSuccessDownloadImage = "IsSuccessDownloadImage";
              public static string PartType = "PartType";
              public static string IsReplaceMB = "IsReplaceMB";

              public static string ProductPart = "ProductPart";
              public static string ReleaseMB = "ReleaseMB";
              public static string ReleasePart = "ReleasePart";

              public static string RepairPartType = "RepairPartType";//Dean FARepair在新增DefectCode時，去新增Sesssion變數

              public static string BatteryCT = "BatteryCT";

              public static string ChangMBReason = "ChangMBReason";


              //Dean 20110625,SA 測試站，第1/2站的Defect，在第3站輸入,新增兩個AllowPass/DefectStation 變數
              public static string AllowPass = "AllowPass";
              public static string DefectStation = "DefectStation";

              public static string Print2DStation = "Print2DStation";
              public static string SKU = "SKU";
              public static string TestBuild = "TestBuild";   
              public static string PilotRunModel = "PilotRunModel"; 
   
              //Vincent for BSam
              public static string IsBSamModel = "IsBSamModel";
              public static string BSamModel = "BSamModel";
              public static string BSamLoc = "BSamLoc";
              public static string CartonSN = "CartonSN";
              public static string AvailableDNList = "AvailableDNList";
              public static string BindDNList = "BindDNList";
              public static string BindProductList = "BindProductList";
              public static string BindDeiviceQty = "BindDeiviceQty";
              public static string FullCartonQty = "FullCartonQty";
              public static string NeedBindProduct = "NeedBindProduct";              
              public static string PalletLoc = "PalletLoc";
              public static string ActualCartonWeight = "ActualCartonWeight";
              public static string IsSameStation = "IsSameStation";

              public static string IsNeedAssignPallet = "IsNeedAssignPallet";
              public static string IsNeedAssignDevice = "IsNeedAssignDevice";

              public static string DeliveryNoList = "DeliveryNoList";
              public static string CartonProductInfoList = "CartonProductInfoList";


              //for PCBVersion
              public static string HasPCBVer = "HasPCBVer";
              public static string PCBVer = "PCBVer";
              public static string Revision = "Revision";
              public static string Supplier = "Supplier";


              //for EPIA & FA QCStatus
              public static string IsEPIA = "IsEPIA";
              public static string FAQCStatus = "FAQCStatus";

              public static string DLCheckStation = "DLCheckStation";

              //PCB Test Log 
              public static string TestLogActionName = "TestLogActionName";
              public static string TestLogErrorCode = "TestLogErrorCode";
              public static string TestLogDescr = "TestLogDescr";
              public static string TestLogRemark = "TestLogRemark";


              //HoldInfo
              public static string ProdHoldInfoList = "ProdHoldInfoList";
              public static string ReleaseCode = "ReleaseCode";


          }


    }

      public class CreateDataTable
      {
          public static System.Data.DataTable CreateProductStatusTb()
          {
              System.Data.DataTable status = new System.Data.DataTable("TbProductStatus");
              status.Columns.Add("ProductID", typeof(string));
              status.Columns.Add("Station", typeof(string));
              status.Columns.Add("Status", typeof(int));
              status.Columns.Add("ReworkCode", typeof(string));
              status.Columns.Add("Line", typeof(string));
              status.Columns.Add("TestFailCount", typeof(int));
              status.Columns.Add("Editor", typeof(string));
              status.Columns.Add("Udt", typeof(DateTime));
              return status;
          }

          public static System.Data.DataTable CreateStringListTb()
          {
              System.Data.DataTable list = new System.Data.DataTable("TbStringList");
              list.Columns.Add("data", typeof(string));
              
              return list;
          }

          public static System.Data.DataTable CreateIntListTb()
          {
              System.Data.DataTable list = new System.Data.DataTable("TbIntList");
              list.Columns.Add("data", typeof(int));

              return list;
          }

      }
};


//namespace IMES.Infrastructure
//{
//    [Serializable]
//    public partial class Session 
//    {
//        public static partial class SessionKeys
//        {
//            public static string ValueToCheck = "ValueToCheck";
//        }
//    }
//}