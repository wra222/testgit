﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Extend.Dictionary
{
    public static class ConstName
    {
        public static class SNFormat
        {
            public const string NUMBER = "NUMBER";
            public const string DATE = "DATE";
            public const string NUMBERAT = "NUMBER@";
            public const string CHECKSUM = "CHECKSUM";
            public const string CHECKSUMAT = "CHECKSUM@";
            public const string LINE = "LINE";
            public const string ALIASLINE = "ALIASLINE";
            public const string ALIAS = "ALIAS";
            public const string SYS = "SYS";
            public const string PREFIXColon = "PREFIX:";

            public const string Lock = "Lock";
        }

        public static class CartonOrPalletStatus
        {
            public const string Full = "Full";
            public const string Partial = "Partial";
            public const string UnPack = "UnPack";
            public const string Empty = "Empty";
            public const string Abort = "Abort";

            public const string full = "full";
            public const string partial = "partial";
        }

        public const string FRU = "FRU";

        public const string Name = "Name";

        public static class Customer
        {
            public const string ASUS = "ASUS";
            public const string Acer = "Acer";
            public const string HP = "HP";
        }
       
        public static class CUSTSN
        {
            public const string CNU = "CNU";
        }

        public static class Letter
        {
            public const string X = "X";
            public const string Y = "Y";
            public const string N = "N";
            public const string M = "M";
            public const string D = "D";
            public const string C = "C";
            public const string T = "T";
            public const string P = "P";
            public const string Zero = "0";
            public const string Nine = "9"; 


        }

        public static class DateFormat
        {
            public const string yyyyMMddHHmmss = "yyyyMMddHHmmss";
            public const string yyyyMMddHHmmssfff = "yyyyMMddHHmmss.fff";
            public const string yyyyMMdd = "yyyyMMdd";
            public const string yyyy = "yyyy";
            public const string MM = "MM";
            public const string YM = "YM";
        }

        public static class Digit
        {
            public const string D1 = "D1";
            public const string D2 = "D2";
            public const string D3 = "D3";
            public const string D4 = "D4";
            public const string D6 = "D6";
            public const string D7 = "D7";

            public const string X1 = "X1";
            public const string X2 = "X2";
            public const string X3 = "X3";
            public const string X4 = "X4";
            public const string X6 = "X6";
            public const string X7 = "X7";
            public const string X12 = "X112";
        }

        public static class Site
        {
            public const string IPC = "IPC";
            public const string ICC = "ICC";
            public const string ICI = "ICI";
        }

        //ConstValue.Type Name List
        public static class ConstValue
        {
            public const string ProcReg = "ProcReg";
            public const string CustSNExceptRule = "CustSNExceptRule";
            public const string PreFixDateAST = "PreFixDateAST";
            public const string PreFixSNAST="PreFixSNAST";
            public const string AssignCUSTSNStation = "AssignCUSTSNStation";
            public const string CustSNRule = "CustSNRule";
            public const string MBSNRule = "MBSNRule";
            public const string CartonSNRule = "CartonSNRule";
            public const string AST = "AST";
            public const string CheckMaterialStatus = "CheckMaterialStatus";
            public const string NoCheckCPUStatus = "NoCheckCPUStatus";
           
        }

        //ConstValueType.Type Name List
        public static class ConstValueType
        {
            public const string SpecialOrderInfoTypeList = "SpecialOrderInfoTypeList";
            public const string ASTPostFixCheckSum = "ASTPostFixCheckSum";
            public const string Customer_PltWeightRoundingShipWay = "{0}_PltWeightRoundingShipWay";
        }

        //SysSetting
        public static class SysSetting
        {
            public const string Site = "Site";
            public const string AssignPOCustomerList = "AssignPOCustomerList";//Assign PO Customer 
            public const string AssignBoxIdCustomerList = "AssignBoxIdCustomerList";//Assign BoxId Customer 
        }

        public static class DeliveryStatus
        {
            public const string Status00 = "00"; //Delivery available

            public const string Status86 = "86"; //Delivery Full
            public const string Status87 = "87"; //Pallet full per delivery
            
            public const string Status90 = "90";  //Withdraw Delivery 
            public const string Status91 = "91";  //SAP PGI
            public const string Status93 = "93";  //Sent B2B SN Report
            public const string Status95 = "95";   //B2B CBR Report
            public const string Status97 = "97";   //B2B KP Report
            public const string Status98 = "98";   //SAP SN Report

            public const string StatusP0 = "P0"; //PO available
            public const string StatusP2 = "P2"; //PO full
            public const string StatusP4 = "P4"; //Pallet full in PO
            public const string StatusP6 = "P6"; //PO combined Delivery
        }

        public static class DeliveryPalletstatus
        {
            public const string NotFullStatus = "0"; //not full
            public const string FullStatus = "1"; //full
        }

        public static class DeliveryHoldStatus
        {
            public const string Active = "A"; 
            public const string Hold = "H"; 
        }

        public static class COAStatus
        {
            public const string StatusP1 = "P1"; 
            //public const string StatusA0 = "A0"; 
            //public const string StatusA1 = "A1";
            public const string StatusA2 = "A2";
        }

        public static class KeyBoxStatus
        {
            public const string Created = "Created";
            public const string Release = "Release";
            public const string Used = "Used";
            public const string ReAssigned = "ReAssigned";

        }

        public static class BomNodeType
        {
            public const string P1 = "P1";
            public const string PL = "PL";
            public const string KP = "KP";
            public const string MB = "MB";
            public const string C1 = "C1";
            public const string C2 = "C2";
            public const string C3 = "C3";
            public const string C4 = "C4";
            public const string PS = "PS";
            public const string AT = "AT";
        }

        public static class AstType
        {
            public const string ATSN3="ATSN3";
        }

        public static class PartInfo
        {
            public const string MACRule = "MACRule";
            public const string AV="AV";
        }      

        public static class NumControlType
        {
            public const string MAC = "MAC";
            public const string IMEI = "IMEI";
            public const string MBSN = "MBSN";
            public const string CUSTSN = "CUSTSN";
            public const string CARTONNO = "CARTONNO";
            public const string AST = "AST";
        }

        public static class FamilyInfo
        {
            public const string MACRule = "MACRule";  //none-HP MAC Rule
        }

        public static class ProductInfo
        {
            public const string LabelPO = "LabelPO";  //LabelPO
        }

        public static class MACRule
        {
            public const string NewMACStatus2 = "2";  //none-HP MAC Rule
        }

       
        public static class PAKLocMasType
        {
            public const string PakLoc = "PakLoc";
        }

        public static class DeliveryInfo
        {
            public const string PalletLayer = "PalletLayer";
            public const string OceanType = "OceanType";
            public const string PltType = "PltType";
            public const string PALLET_TYPE = "PALLET_TYPE";
        }

        public static class PalletAttr
        {
            public const string SendStatus = "SendStatus";
        }

        public static class ModelInfo
        {
            public const string Cust = "Cust";
            public const string Cust3 = "Cust3";
            public const string ATSNAV="ATSNAV";
        }

        public static class MaterialType
        {
            public const string CPU = "CPU";            
        }

        public static class PCBInfo
        {
            public const string SmallMBSN = "SmallMBSN";
            public const string ParentMBSN = "ParentMBSN";
            public const string ChildMBSN = "ChildMBSN";
            public const string IsSmallBoard = "IsSmallBoard";  //Y/N
            public const string PilotMo = "PilotMo";
        }

    }
}
