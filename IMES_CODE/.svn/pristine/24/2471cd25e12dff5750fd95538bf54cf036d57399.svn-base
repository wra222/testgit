using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Inventec.HPEDITS.XmlCreator.Database
{
    public class EditsSqlProc
    {
        static string ConnectionString = ConfigurationManager.AppSettings["Database"].ToString();

        // mainly in BoxLabelXmlCreator

        public static DataTable v_PAKComn__by__InternalID(string internalID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_PAKComn] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        public static DataTable PAK_PackkingData__by__InternalID__SERIAL_NUM(string internalID, string SERIAL_NUM)
        {
            // 20121120 avoid *
            string SQLText = "select BOX_ID, SERIAL_NUM from [PAK_PackkingData]  (nolock) WHERE InternalID=@InternalID AND SERIAL_NUM=@SERIAL_NUM ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", internalID),
                    new SqlParameter("@SERIAL_NUM", SERIAL_NUM));
            return dt;
        }

        public static DataTable count__PAK_PackkingData__by__InternalID__SERIAL_NUM__InternalID10(string internalID, string SERIAL_NUM, string internalID10)
        {
            // Subquery returned more than 1 value. This is not permitted when the subquery follows =, !=, <, <= , >, >= or when ...
            // frank mark
            //string SQLText = "select count(distinct SERIAL_NUM) as BOX_SEQUENCE from [PAK_PackkingData] (nolock) WHERE ID<= (select ID from [PAK_PackkingData] where InternalID=@InternalID and SERIAL_NUM=@SERIAL_NUM) and left(InternalID,10)=@InternalID10 ";
            string SQLText = "select count(distinct BOX_ID) as BOX_SEQUENCE from [PAK_PackkingData] (nolock) WHERE ID<= (select top 1 ID from [PAK_PackkingData] where InternalID=@InternalID and SERIAL_NUM=@SERIAL_NUM) and left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", internalID),
                    new SqlParameter("@SERIAL_NUM", SERIAL_NUM),
                    new SqlParameter("@InternalID10", internalID10));
            return dt;
        }

        public static DataTable count__PAK_PackkingData__by__BOX_ID(string BOX_ID)
        {
            string SQLText = "select count(distinct SERIAL_NUM) as BOX_UNIT_QTY from [PAK_PackkingData] (nolock) where BOX_ID =@BOX_ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@BOX_ID", BOX_ID));
            return dt;
        }
        public static DataTable PAK_PackkingData__by__SerialNUM(string SERIAL_NUM)
        {
            // 20121120 avoid *
            string SQLText = "select @SERIAL_NUM as SERIAL_NUM";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@SERIAL_NUM", SERIAL_NUM));
            return dt;
        }
        public static DataTable PAK_PackkingData__by__BOX_ID(string BOX_ID)
        {
            // 20121120 avoid *
            string SQLText = "select SERIAL_NUM from [PAK_PackkingData] (nolock) WHERE BOX_ID =@BOX_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@BOX_ID", BOX_ID));
            return dt;
        }

        public static DataTable PAKEDI_INSTR__by__PO_NUM__FIELDS(string PO_NUM)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [PAKEDI_INSTR] (nolock) WHERE PO_NUM=@PO_NUM and FIELDS = 'LABEL_INSTR_HEAD'";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM));
            return dt;
        }

        public static DataTable VALUE__from__PAKEDI_INSTR__by__PO_NUM__FIELDS(string PO_NUM, string FIELDS)
        {
            string SQLText = "select VALUE from [PAKEDI_INSTR] (nolock) WHERE PO_NUM=@PO_NUM and FIELDS =@FIELDS order by ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM),
                    new SqlParameter("@FIELDS", FIELDS));
            return dt;
        }

        public static DataTable PAKODMSESSION__by__SERIAL_NUM(string SERIAL_NUM)
        {
            // 20121120 avoid *
            string SQLText = "select UDF_KEY_HEADER, UDF_VALUE_HEADER, UDF_KEY_DETAIL, UDF_VALUE_DETAIL, SERIAL_NUM from [PAKODMSESSION] (nolock) WHERE SERIAL_NUM=@SERIAL_NUM order by SHOW_ORDER";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@SERIAL_NUM", SERIAL_NUM));
            return dt;
        }

        public static DataTable PAKODMSESSION__by__SERIAL_NUM_forUdfDetail(string SERIAL_NUM)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_PAKODMSESSION]",
                    new SqlParameter("@SERIAL_NUM", SERIAL_NUM));
            return dt;
        }

        //ADD FOR TABLET
        public static DataTable PAKODMSESSION__by__SERIAL_NUM_throughFunction(string SERIAL_NUM, string InternalID)
        {
            // 20121120 avoid *
            string SQLText = "select UDF_KEY_HEADER as UDF_KEY_HEADER, UDF_VALUE_HEADER as UDF_VALUE_HEADER, UDF_KEY_DETAIL as UDF_KEY_DETAIL, UDF_VALUE_DETAIL as UDF_VALUE_DETAIL, SERIAL_NUM as SERIAL_NUM from [dbo].[fn_GetODMSESSION](@SERIAL_NUM,@InternalID) order by SHOW_ORDER";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@SERIAL_NUM", SERIAL_NUM),
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }
        public static DataTable sp__op_TwoDCode_Solution_Box(string InternalID, string boxid)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_TwoDCode_Solution_Box]",
                    new SqlParameter("@InternalID", InternalID),
                    new SqlParameter("@boxid", boxid));
            return dt;
        }

        // mainly in BoxLabelShipmentXmlCreator

        public static DataTable v_Shipment_PAKComn__by__InternalID(string internalID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_Shipment_PAKComn] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        // mainly in PackListShipmentXmlCreator

        public static DataTable sp__op_TwoDCode_Check(string dn, string docs, string tp)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_TwoDCode_Check]",
                    new SqlParameter("@dn", dn),
                    new SqlParameter("@docs", docs),
                    new SqlParameter("@tp", tp));
            return dt;
        }

        public static DataTable v_Shipment_PackList__by__InternalID(string internalID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_Shipment_PackList] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        public static DataTable v_Shipment_PAKComn__like__InternalID(string internalID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_Shipment_PAKComn] WHERE InternalID LIKE @internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        public static DataTable PAK_SkuMasterWeight_FIS__by__Model(string Model)
        {
            // 20121120 avoid *
            string SQLText = "select Weight from [PAK_SkuMasterWeight_FIS] (nolock) WHERE Model =@Model";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@Model", Model));
            return dt;
        }

        public static DataTable PAK_PAKEdi850raw__by__PO_NUM__LINE_ITEM(string PO_NUM, string LINE_ITEM)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [PAK.PAKEdi850raw] (nolock) WHERE PO_NUM=@PO_NUM and HP_PN_COMPONENT !='' and LINE_ITEM =@LINE_ITEM order by HP_SO_LINE_ITEM";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM),
                    new SqlParameter("@LINE_ITEM", LINE_ITEM));
            return dt;
        }

        public static DataTable PAK_PackkingData__by__InternalID(string InternalID)
        {
            // 20121120 avoid *
            string SQLText = "select BOX_ID, SERIAL_NUM, TRACK_NO_PARCEL from [PAK_PackkingData] (nolock) WHERE InternalID =@InternalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        public static DataTable sp__op_TwoDCode_Solution_packlist(string InternalID, string boxid, string tp)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_TwoDCode_Solution_packlist]",
                    new SqlParameter("@InternalID", InternalID),
                    new SqlParameter("@boxid", boxid),
                    new SqlParameter("@tp", tp));
            return dt;
        }

        public static DataTable sp__op_GetUDF_KEY_VALUE_HEADER_Shipment(string InternalID)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "op_GetUDF_KEY_VALUE_HEADER_Shipment",
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        // LoadMiniTable_Detail
        public static DataTable v_PO_ITEM_DETAIL__by__PO_NUM__FIELDS(string PO_NUM, string FIELDS)
        {
            string SQLText = "select VALUE from [v_PO_ITEM_DETAIL] WHERE PO_NUM=@PO_NUM and FIELDS=@FIELDS order by ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM),
                    new SqlParameter("@FIELDS", FIELDS));
            return dt;
        }

        public static DataTable v_PO_ITEM_DETAIL__by__PO_NUM__PO_ITEM__FIELDS(string PO_NUM, string PO_ITEM, string FIELDS)
        {
            string SQLText = "select VALUE from [v_PO_ITEM_DETAIL] WHERE PO_NUM=@PO_NUM and PO_ITEM=@PO_ITEM and FIELDS=@FIELDS order by ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM),
                    new SqlParameter("@PO_ITEM", PO_ITEM),
                    new SqlParameter("@FIELDS", FIELDS));
            return dt;
        }

        // mainly in PalletAShipmentXmlCreator

        public static DataTable sp__op_Shipment_PAKPalletTypeALineItem(string DNo, string plt)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_Shipment_PAKPalletTypeALineItem]",
                    new SqlParameter("@DNo", DNo),
                    new SqlParameter("@plt", plt));
            return dt;
        }

        public static DataTable v_Shipment_PAKPalletTypeB__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            // 20121120 avoid *
            string SQLText = "select UCC from [v_Shipment_PAKPalletTypeB] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable HP_PN__from__v_Shipment_PAKPalletTypeA__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            //timeout
            //string SQLText = "select distinct HP_PN from [v_Shipment_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            InternalID10 += "%";
            string SQLText = "select distinct dbo.fn_GetHP_PN(HP_PN) as HP_PN from [PAK_PAKComn] a join PAK_PackkingData p on a.InternalID=p.InternalID where PALLET_ID=@PALLET_ID and a.InternalID like @InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable sum_PALLET_BOX_QTY__from__v_Shipment_PAKPalletTypeA__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            string SQLText = "select Sum(PALLET_BOX_QTY) as PALLET_BOX_QTY from [v_Shipment_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable sum_PALLET_UNIT_QTY__from__v_Shipment_PAKPalletTypeA__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            string SQLText = "select Sum(PALLET_UNIT_QTY) as PALLET_UNIT_QTY from [v_Shipment_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable sp__op_TwoDCode_Solution(string InternalID, string PalletID)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_TwoDCode_Solution]",
                    new SqlParameter("@InternalID", InternalID),
                    new SqlParameter("@PalletID", PalletID));
            return dt;
        }

        // mainly in PalletAXmlCreator

        public static DataTable v_PAKPalletTypeALineItem__by__InternalID__PALLET_ID(string InternalID, string PALLET_ID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_PAKPalletTypeALineItem] WHERE InternalID=@InternalID and PALLET_ID=@PALLET_ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID),
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        public static DataTable sum_PACK_ID_UNIT_QTY_PER_PALLET__from__v_PAKPalletTypeALineItem__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            string SQLText = "select Sum(convert(int,PACK_ID_UNIT_QTY_PER_PALLET)) as PACK_ID_UNIT_QTY_PER_PALLET from [v_PAKPalletTypeALineItem] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable sum_PACK_ID_BOX_QTY_PER_PALLET__from__v_PAKPalletTypeALineItem__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            string SQLText = "select Sum(convert(int,PACK_ID_BOX_QTY_PER_PALLET)) as PACK_ID_BOX_QTY_PER_PALLET from [v_PAKPalletTypeALineItem] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable HP_PN__from__v_PAKPalletTypeA__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            string SQLText = "select distinct HP_PN from [v_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable sum_PALLET_BOX_QTY__from__v_PAKPalletTypeA__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            string SQLText = "select Sum(PALLET_BOX_QTY) as PALLET_BOX_QTY from [v_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        public static DataTable sum_PALLET_UNIT_QTY__from__v_PAKPalletTypeA__by__PALLET_ID__InternalID10(string PALLET_ID, string InternalID10)
        {
            //string SQLText = "select Sum(PALLET_UNIT_QTY) as PALLET_UNIT_QTY from [v_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            string SQLText = "select Sum(convert(int,PALLET_UNIT_QTY)) as PALLET_UNIT_QTY from [v_PAKPalletTypeA] WHERE PALLET_ID=@PALLET_ID and Left(InternalID,10)=@InternalID10 ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID),
                    new SqlParameter("@InternalID10", InternalID10));
            return dt;
        }

        // mainly in PalletBShipmentXmlCreator

        public static DataTable v_Shipment_PAKPalletTypeB__by__InternalID__PALLET_ID(string InternalID, string PALLET_ID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_Shipment_PAKPalletTypeB] WHERE InternalID=@InternalID and PALLET_ID=@PALLET_ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID),
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        public static DataTable REGION__CUSTOMER_ID__SHIP_MODE__from__v_Shipment_PAKComn__by__InternalID(string internalID)
        {
            string SQLText = "select REGION, CUSTOMER_ID, SHIP_MODE from [v_Shipment_PAKComn] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        public static DataTable PACK_ID__WAYBILL_NUMBER__from__v_PAKPalletTypeBLineItem__by__PALLET_ID(string PALLET_ID)
        {
            string SQLText = "select PACK_ID, WAYBILL_NUMBER from [v_PAKPalletTypeBLineItem] WHERE PALLET_ID=@PALLET_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        public static DataTable sp__op_GetLAOceanWaybill(string internalID)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_GetLAOceanWaybill]",
                    new SqlParameter("@internalId", internalID));
            return dt;
        }

        public static DataTable sp__op_Shipment_PAKPalletTypeBLineItem(string pallet_id)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_Shipment_PAKPalletTypeBLineItem]",
                    new SqlParameter("@pallet_id", pallet_id));
            return dt;
        }

        public static DataTable sp__op_Shipment_PACKBox(string packid, string palletid, string box, string tp)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_Shipment_PACKBox]",
                    new SqlParameter("@packid", packid),
                    new SqlParameter("@palletid", palletid),
                    new SqlParameter("@box", box),
                    new SqlParameter("@tp", tp));
            return dt;
        }

        // mainly in PalletBXmlCreator

        public static DataTable v_PAKPalletTypeB__by__InternalID__PALLET_ID(string InternalID, string PALLET_ID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_PAKPalletTypeB] WHERE InternalID=@InternalID and PALLET_ID=@PALLET_ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID),
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        public static DataTable v_PAKPalletTypeBLineItem__by__PALLET_ID(string PALLET_ID)
        {
            // 20121120 avoid *
            // 20130313 need WAYBILL_NUMBER and PACK_ID ... ?
            string SQLText = "select * from [v_PAKPalletTypeBLineItem] WHERE PALLET_ID=@PALLET_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        public static DataTable REGION__CUSTOMER_ID__SHIP_MODE__from__v_PAKComn__by__InternalID(string internalID)
        {
            string SQLText = "select REGION, CUSTOMER_ID, SHIP_MODE from [v_PAKComn] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        // mainly in WayBillShipmentXmlCreator

        public static DataTable SUB_REGION__MASTER_WAYBILL_NUMBER__WAYBILL_NUMBER__PREF_GATEWAY__INTL_CARRIER__ACTUAL_SHIPDATE__from__v_Shipment_WayBillRegion__by__C46(string C46)
        {
            string SQLText = @"select distinct SUB_REGION, MASTER_WAYBILL_NUMBER, WAYBILL_NUMBER, PREF_GATEWAY, INTL_CARRIER, ACTUAL_SHIPDATE,
SHIP_FROM_ID, SHIP_FROM_NAME, SHIP_FROM_NAME_2, SHIP_FROM_NAME_3, SHIP_FROM_STREET, SHIP_FROM_STREET_2, SHIP_FROM_CITY, SHIP_FROM_STATE, SHIP_FROM_ZIP, SHIP_FROM_COUNTRY_CODE, SHIP_FROM_COUNTRY_NAME, SHIP_FROM_CONTACT, SHIP_FROM_TELEPHONE, SHIP_MODE
from [v_Shipment_WayBillRegion] WHERE C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        public static DataTable sp__op_PAK_ShipmentWeight_FIS(string shipment, string tp)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_PAK_ShipmentWeight_FIS]",
                    new SqlParameter("@shipment", shipment),
                    new SqlParameter("@tp", tp));
            return dt;
        }

        public static DataTable sum_DEST_CODE_BOX_QTY__from__v_Shipment_WayBillList__by__C46(string C46)
        {
            string SQLText = "select sum(DEST_CODE_BOX_QTY) as DEST_CODE_BOX_QTY from [v_Shipment_WayBillList] WHERE C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        public static DataTable PALLET_ID__from__PAK_PAKPaltno__by__C46(string C46)
        {
            string SQLText = "select distinct PALLET_ID from [PAK.PAKPaltno] (nolock) WHERE InternalID in (select InternalID from PAK_PAKComn (nolock) WHERE  C46=@C46 )";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        public static DataTable PALLET_ID__from__PAK_PAKPaltno__by__WAYBILL_NUMBER(string WAYBILL_NUMBER)
        {
            string SQLText = "select distinct PALLET_ID from [PAK.PAKPaltno] (nolock) WHERE InternalID in (select InternalID from PAK_PAKComn (nolock) WHERE  WAYBILL_NUMBER=@WAYBILL_NUMBER )";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@WAYBILL_NUMBER", WAYBILL_NUMBER));
            return dt;
        }

        public static DataTable v_Shipment_WayBillDest__by__C46__SUB_REGION(string C46, string SUB_REGION)
        {
            // 20121120 avoid *
            string SQLText = "select DEST_CODE, SUB_REGION, DEST_CODE_EXTD_BOX_WGT, DEST_CODE_BOX_QTY from [v_Shipment_WayBillDest] WHERE C46=@C46 and SUB_REGION=@SUB_REGION ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46),
                    new SqlParameter("@SUB_REGION", SUB_REGION));
            return dt;
        }

        public static DataTable v_Shipment_PAKComn__by__C46__DEST_CODE__SUB_REGION(string C46, string DEST_CODE, string SUB_REGION)
        {
            // 20121120 avoid *
            string SQLText = "select InternalID, DUTY_CODE from [v_Shipment_PAKComn] WHERE C46=@C46 and DEST_CODE=@DEST_CODE and SUB_REGION=@SUB_REGION";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46),
                    new SqlParameter("@DEST_CODE", DEST_CODE),
                    new SqlParameter("@SUB_REGION", SUB_REGION));
            return dt;
        }

        public static DataTable v_Shipment_PAKComnWeight__by__InternalID(string InternalID)
        {
            // 20121120 avoid *
            string SQLText = "select Weight from [v_Shipment_PAKComnWeight] WHERE InternalID =@InternalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        public static DataTable Qty__from__P_PoPlt__by__Consolidate(string Consolidate)
        {
            string SQLText = "select Qty as DEST_CODE_BOX_QTY from [P_PoPlt] WHERE Consolidate =@Consolidate";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@Consolidate", Consolidate));
            return dt;
        }

        public static DataTable PLT__from__P_PoPlt__by__Consolidate(string Consolidate)
        {
            string SQLText = "select distinct PLT as PALLET_ID from [P_PoPlt] WHERE Consolidate =@Consolidate";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@Consolidate", Consolidate));
            return dt;
        }

        public static DataTable DEST_CODE__CONSOL_INVOICE__from__v_Shipment_PAKComn__by__InternalID(string internalID)
        {
            string SQLText = "select DEST_CODE, CONSOL_INVOICE from [v_Shipment_PAKComn] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        public static DataTable PAK_PAKEdi850raw__by__PO_NUM(string PO_NUM)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [PAK.PAKEdi850raw] (nolock) WHERE PO_NUM=@PO_NUM";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM));
            return dt;
        }

        public static DataTable selects__from__PAK_PAKEdi850raw__by__PO_NUM(string selects, string PO_NUM)
        {
            string SQLText = "select DISTINCT " + selects + " from [PAK.PAKEdi850raw] (nolock) WHERE PO_NUM=@PO_NUM";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PO_NUM", PO_NUM));
            return dt;
        }

        // mainly in WayBillXmlCreator

        public static DataTable SUB_REGION__MASTER_WAYBILL_NUMBER__WAYBILL_NUMBER__INTL_CARRIER__ACTUAL_SHIPDATE__from__v_WayBillList__by__C46(string C46)
        {
            string SQLText = "select distinct CONSOL_INVOICE, SUB_REGION, MASTER_WAYBILL_NUMBER, WAYBILL_NUMBER, DUTY_CODE, INTL_CARRIER, ACTUAL_SHIPDATE from [v_WayBillList] WHERE C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        public static DataTable v_WayBillDest__by__C46__SUB_REGION(string C46, string SUB_REGION)
        {
            // 20121120 avoid *
            string SQLText = "select DEST_CODE, SUB_REGION, DEST_CODE_EXTD_BOX_WGT, DEST_CODE_BOX_QTY from [v_WayBillDest] WHERE C46=@C46 and SUB_REGION=@SUB_REGION ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46),
                    new SqlParameter("@SUB_REGION", SUB_REGION));
            return dt;
        }

        public static DataTable v_PAKComn__by__C46__DEST_CODE__SUB_REGION(string C46, string DEST_CODE, string SUB_REGION)
        {
            // 20121120 avoid *
            string SQLText = "select InternalID, DUTY_CODE from [v_PAKComn] WHERE C46=@C46 and DEST_CODE=@DEST_CODE and SUB_REGION=@SUB_REGION";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46),
                    new SqlParameter("@DEST_CODE", DEST_CODE),
                    new SqlParameter("@SUB_REGION", SUB_REGION));
            return dt;
        }

        public static DataTable v_PAKComnWeight__by__InternalID(string InternalID)
        {
            // 20121120 avoid *
            string SQLText = "select Weight from [v_PAKComnWeight] WHERE InternalID =@InternalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        public static DataTable PAK_ShipmentWeight_FIS__by__Shipment(string Shipment)
        {
            // 20121120 avoid *
            string SQLText = "select Weight from [PAK_ShipmentWeight_FIS] (nolock) WHERE Shipment =@Shipment";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@Shipment", Shipment));
            return dt;
        }

        public static DataTable PALLET_UNIT_QTY__from__PAK_PAKPaltno__by__InternalID(string InternalID)
        {
            string SQLText = "select PALLET_UNIT_QTY as DEST_CODE_BOX_QTY from [PAK.PAKPaltno] (nolock) WHERE InternalID=@InternalID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        public static DataTable PALLET_ID__from__PAK_PAKPaltno__by__InternalID(string InternalID)
        {
            string SQLText = "select distinct PALLET_ID from [PAK.PAKPaltno] (nolock) WHERE InternalID in (SELECT InternalID FROM [v_PAKComn] WHERE InternalID=@InternalID )";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        public static DataTable DEST_CODE__CONSOL_INVOICE__from__v_PAKComn__by__InternalID(string internalID)
        {
            string SQLText = "select DEST_CODE, CONSOL_INVOICE from [v_PAKComn] WHERE InternalID=@internalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        // mainly in PackListXmlCreator

        public static DataTable v_PackList__by__InternalID(string InternalID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_PackList] WHERE InternalID=@InternalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@InternalID", InternalID));
            return dt;
        }

        public static DataTable v_PAKComn__like__InternalID(string internalID)
        {
            // x 20121120 avoid *
            string SQLText = "select * from [v_PAKComn] WHERE InternalID LIKE @internalID order by InternalID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID));
            return dt;
        }

        // mainly in bsam BoxLabelXmlCreator.cs

        public static DataTable InternalID__from__PAK_PackkingData__by__BOX_ID(string BOX_ID)
        {
            string SQLText = "select distinct InternalID from [PAK_PackkingData] (nolock) WHERE BOX_ID =@BOX_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@BOX_ID", BOX_ID));
            return dt;
        }

        public static DataTable sp__op_GETHAWB_UNIT_QTY(string InternalID10)
        {
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.StoredProcedure,
                    "[op_TwoDCode_Solution_Box]",
                    new SqlParameter("@InternalID", InternalID10));
            return dt;
        }

        // HAWB_BOX_QTY: box qty per HAWB
        public static DataTable sum_DEST_CODE_BOX_QTY__from__v_WayBillList__by__CONSOL_INVOICE(string CONSOL_INVOICE)
        {
            //string SQLText = "select sum(DEST_CODE_BOX_QTY) as DEST_CODE_BOX_QTY from [v_WayBillList] WHERE CONSOL_INVOICE=@CONSOL_INVOICE";
            string SQLText = "select ISNULL(sum(DEST_CODE_BOX_QTY),0) as DEST_CODE_BOX_QTY from [v_WayBillList] WHERE CONSOL_INVOICE=@CONSOL_INVOICE";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@CONSOL_INVOICE", CONSOL_INVOICE));
            return dt;
        }

        // HAWB_BOX_QTY
        public static DataTable HAWB_BOX_QTY__by__C46(string C46)
        {
            string SQLText = "select floor(sum( floor(convert(decimal(8,3), PACK_ID_UNIT_QTY)) / floor(convert(decimal(8,3), CARTON_QTY)) ) ) as HAWB_BOX_QTY from PAK_PAKComn (nolock) where C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        // HAWB_UNIT_QTY
        public static DataTable HAWB_UNIT_QTY__by__C46(string C46)
        {
            string SQLText = "select sum(floor(convert(decimal(8,3), PACK_ID_UNIT_QTY))) as HAWB_UNIT_QTY from PAK_PAKComn (nolock) where C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        // BOX_PACKID_QTY: PACK ID qty per box
        //public static DataTable count__PACK_ID__PAK_PAKComn__by__BOX_ID(string BOX_ID)
        public static DataTable BOX_PACKID_QTY__by__BOX_ID(string BOX_ID)
        {
            //string SQLText = "select count(distinct PACK_ID) as BOX_PACKID_QTY from PAK_PAKComn (nolock) where InternalID in ( select distinct InternalID from PAK_PackkingData (nolock) WHERE BOX_ID=@BOX_ID )";
            string SQLText = "select count(distinct left(InternalID,10)) as BOX_PACKID_QTY from PAK_PackkingData (nolock) where BOX_ID=@BOX_ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@BOX_ID", BOX_ID));
            return dt;
        }

        public static DataTable PACK_ID_LINE_ITEM_BOX_UNIT_QTY__by__PACK_ID(string PACK_ID, string BOX_ID)
        {
            //string SQLText = "select sum(PACK_ID_LINE_ITEM_UNIT_QTY) as PACK_ID_LINE_ITEM_BOX_UNIT_QTY from PAK_PAKComn (nolock) where PACK_ID=@PACK_ID ";
            string SQLText = @"select count(a.SERIAL_NUM) as PACK_ID_LINE_ITEM_BOX_UNIT_QTY from PAK_PackkingData a (nolock),[PAK.PAKComn] b (nolock) where a.InternalID=b.InternalID and
b.PACK_ID=@PACK_ID and a.BOX_ID=@BOX_ID group by a.BOX_ID ";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PACK_ID", PACK_ID),
                    new SqlParameter("@BOX_ID", BOX_ID));
            return dt;
        }

        // mainly in bsam HouseWaybillsXmlCreator

        public static DataTable InternalID__from__v_PAKComn__by__waybill_number(string WAYBILL_NUMBER)
        {
            string SQLText = "select InternalID from [v_PAKComn] WHERE WAYBILL_NUMBER=@WAYBILL_NUMBER";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@WAYBILL_NUMBER", WAYBILL_NUMBER));
            return dt;
        }

        public static DataTable v_PAKComn__by__waybill_number(string WAYBILL_NUMBER)
        {
            string SQLText = "select * from [v_PAKComn] WHERE WAYBILL_NUMBER=@WAYBILL_NUMBER";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@WAYBILL_NUMBER", WAYBILL_NUMBER));
            return dt;
        }

        public static DataTable v_PAKComn__by__C46(string C46)
        {
            string SQLText = "select * from [v_PAKComn] WHERE C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        // HAWB_BOX_QTY
        public static DataTable HAWB_BOX_QTY__by__WAYBILL_NUMBER(string WAYBILL_NUMBER)
        {
            string SQLText = "select COUNT(distinct BOX_ID) as HAWB_BOX_QTY from PAK_PackkingData (nolock) where InternalID in (SELECT InternalID FROM [PAK.PAKComn] (nolock) where WAYBILL_NUMBER=@WAYBILL_NUMBER)";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@WAYBILL_NUMBER", WAYBILL_NUMBER));
            return dt;
        }

        // HAWB_UNIT_QTY
        public static DataTable HAWB_UNIT_QTY__by__WAYBILL_NUMBER(string WAYBILL_NUMBER)
        {
            string SQLText = "select ISNULL(SUM(PACK_ID_LINE_ITEM_UNIT_QTY),0) as HAWB_UNIT_QTY from [PAK.PAKComn] (nolock) where WAYBILL_NUMBER=@WAYBILL_NUMBER";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@WAYBILL_NUMBER", WAYBILL_NUMBER));
            return dt;
        }

        // mainly in bsam PalletBShipmentBsamXmlCreator

        // HAWB_PALLET_QTY
        public static DataTable HAWB_PALLET_QTY__by__C46__PalletBShipmentBsam(string C46)
        {
            string SQLText = "select count(distinct PALLET_ID) as HAWB_PALLET_QTY from [PAK.PAKComn] a (nolock),[PAK.PAKPaltno] b (nolock) where a.InternalID=b.InternalID and a.C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        // HAWB_BOX_QTY
        public static DataTable HAWB_BOX_QTY__by__C46__PalletBShipmentBsam(string C46)
        {
            string SQLText = "select COUNT(distinct BOX_ID) as HAWB_BOX_QTY from PAK_PackkingData (nolock) where InternalID in (SELECT InternalID FROM [PAK.PAKComn] (nolock) where C46=@C46)";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        // HAWB_UNIT_QTY
        public static DataTable HAWB_UNIT_QTY__by__C46__PalletBShipmentBsam(string C46)
        {
            string SQLText = "select ISNULL(SUM(PACK_ID_LINE_ITEM_UNIT_QTY),0) as HAWB_UNIT_QTY from [PAK.PAKComn] (nolock) where C46=@C46";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@C46", C46));
            return dt;
        }

        // PALLET_UNIT_QTY
        public static DataTable PALLET_UNIT_QTY__by__PALLET_ID__PalletBShipmentBsam(string PALLET_ID)
        {
            string SQLText = "select ISNULL(sum(convert(int,PALLET_UNIT_QTY)),0) as PALLET_UNIT_QTY from [PAK.PAKPaltno] (nolock) where PALLET_ID=@PALLET_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        /*public static DataTable distinct_BOX_ID__PAK_PackkingData(string internalID, string PALLET_ID)
        {
            string SQLText = "select distinct BOX_ID from PAK_PackkingData (nolock) WHERE InternalID=@internalID and PALLET_ID=@PALLET_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID", internalID),
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }*/
		public static DataTable distinct_BOX_ID__PAK_PackkingData(string PALLET_ID)
        {
            string SQLText = "select distinct BOX_ID from PAK_PackkingData (nolock) WHERE PALLET_ID=@PALLET_ID";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@PALLET_ID", PALLET_ID));
            return dt;
        }

        public static DataTable count_BOX_ID__PAK_PackkingData(string internalID10)
        {
            string SQLText = "select COUNT(distinct BOX_ID) as PACK_ID_BOX_QTY from PAK_PackkingData (nolock) where InternalID like @internalID10";
            DataTable dt = SQLHelper.ExecuteDataFill(ConnectionString,
                    System.Data.CommandType.Text,
                    SQLText,
                    new SqlParameter("@internalID10", internalID10));
            return dt;
        }

    }
}
