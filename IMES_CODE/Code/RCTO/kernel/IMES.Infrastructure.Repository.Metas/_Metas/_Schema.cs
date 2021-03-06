﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Infrastructure.Repository._Metas
{
    internal static class Constants
    {
        public const long DateTimeMinVal = 552877920000000000L;//Ticks  //new DateTime(1753,1,1)
        public const long DateTimeMaxVal = 3155378112000000000L;//Ticks  //new DateTime(9999, 12, 31)
        public const long SmallDateTimeMinVal = 599266080000000000L;//Ticks  //new DateTime(1900,1,1)
        public const long SmallDateTimeMaxVal = 655888320000000000L;//Ticks  //new DateTime(2079, 6, 6)
        public const long CurrencyMinVal = -999999999999999999L;//-79228162514264337593543950335M;           //decimal.MinValue
        public const long CurrencyMaxVal = 999999999999999999L;//79228162514264337593543950335M;            //decimal.MaxValue
    }
}

namespace IMES.Infrastructure.Repository._Metas
{
    #region GetData

    #region BOM

    [Table("Part")]
    public class Part_NEW
    {
        public const string fn_autoDL = "AutoDL";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String autoDL = null;

        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_custPartNo = "CustPartNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custPartNo = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_flag = "Flag";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 flag = int.MinValue;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String partType = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 900, true, false, "")]
        public String remark = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MoBOM")]
    public class MoBOM_NEW
    {
        public const string fn_alternative_item_group = "Alternative_item_group";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String alternative_item_group = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_component = "Component";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String component = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_material = "Material";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String material = null;

        public const string fn_mo = "Mo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String mo = null;

        public const string fn_plant = "Plant";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String plant = null;

        public const string fn_priority = "Priority";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String priority = null;

        public const string fn_quantity = "Quantity";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String quantity = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ModelBOM")]
    public class ModelBOM_NEW
    {
        public const string fn_alternative_item_group = "Alternative_item_group";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String alternative_item_group = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_component = "Component";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String component = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_flag = "Flag";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 flag = int.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_material = "Material";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String material = null;

        public const string fn_plant = "Plant";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String plant = null;

        public const string fn_priority = "Priority";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String priority = null;

        public const string fn_quantity = "Quantity";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String quantity = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MoPart")]
    public class MoPart
    {
        public const string fn_autoDL = "AutoDL";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String autoDL = null;

        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_custPartNo = "CustPartNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custPartNo = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_flag = "Flag";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 flag = int.MinValue;

        public const string fn_mo = "Mo";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String mo = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String partType = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String remark = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MoPartInfo")]
    public class MoPartInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
        public String infoValue = null;

        public const string fn_mo = "Mo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String mo = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Battery")]
    public class Battery
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hppn = "HPPN";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public String hppn = null;

        public const string fn_hssn = "HSSN";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String hssn = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ACAdapMaintain")]//"AcAdapMaintain")]
    public class AcAdapMaintain
    {
        public const string fn_adppn = "ADPPN";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String adppn = null;

        public const string fn_agency = "AGENCY";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String agency = null;

        public const string fn_assemb = "ASSEMB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String assemb = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cur = "CUR";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cur = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_supplier = "SUPPLIER";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String supplier = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_voltage = "VOLTAGE";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String voltage = null;

        //public const string fn_adppn = "ADPPN";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public String adppn = null;

        //public const string fn_agency = "AGENCY";
        //[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        //public String agency = null;

        //public const string fn_assemb = "ASSEMB";
        //[DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        //public String assemb = null;

        //public const string fn_cdt = "Cdt";
        //[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        //public DateTime cdt = DateTime.MinValue;

        //public const string fn_cur = "CUR";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public String cur = null;

        //public const string fn_editor = "Editor";
        //[DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        //public String editor = null;

        //public const string fn_id = "ID";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        //public Int32 id = int.MinValue;

        //public const string fn_supplier = "SUPPLIER";
        //[DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        //public String supplier = null;

        //public const string fn_udt = "Udt";
        //[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        //public DateTime udt = DateTime.MinValue;

        //public const string fn_voltage = "VOLTAGE";
        //[DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        //public String voltage = null;
    }

    [Table("MO")]//Same as the one in DB.cs
    public class Mo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_createDate = "CreateDate";
        [DBField(SqlDbType.SmallDateTime, Constants.SmallDateTimeMinVal, Constants.SmallDateTimeMaxVal, false, false, "")]
        public DateTime createDate = DateTime.MinValue;

        public const string fn_customerSN_Qty = "CustomerSN_Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 customerSN_Qty = int.MinValue;

        public const string fn_mo = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String mo = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_plant = "Plant";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String plant = null;

        public const string fn_print_Qty = "Print_Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 print_Qty = int.MinValue;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_sapqty = "SAPQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 sapqty = int.MinValue;

        public const string fn_sapstatus = "SAPStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String sapstatus = null;

        public const string fn_startDate = "StartDate";
        [DBField(SqlDbType.SmallDateTime, Constants.SmallDateTimeMinVal, Constants.SmallDateTimeMaxVal, false, false, "")]
        public DateTime startDate = DateTime.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_transfer_Qty = "Transfer_Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 transfer_Qty = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String poNo = null;
    }

    [Table("PartInfo")]//Same as the one in DB.cs
    public class PartInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
        public String infoValue = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    //[Table("Part")]//Same as the one in DB.cs
    //public class Part
    //{
    //    public const string fn_PartNo = "PartNo";
    //    [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
    //    public string PartNo = null;
    //    public const string fn_Descr = "Descr";
    //    [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
    //    public string Descr = null;
    //    public const string fn_PartType = "PartType";
    //    [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
    //    public string PartType = null;
    //    public const string fn_CustPartNo = "CustPartNo";
    //    [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
    //    public string CustPartNo = null;
    //    //public const string fn_FruNo = "FruNo";
    //    //[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
    //    //public string FruNo = null;
    //    //public const string fn_Vendor = "Vendor";
    //    //[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
    //    //public string Vendor = null;
    //    //public const string fn_IECVersion = "IECVersion";
    //    //[DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
    //    //public string IECVersion = null;
    //    public const string fn_AutoDL = "AutoDL";
    //    [DBField(SqlDbType.Char, 0, 1, false, false, "")]
    //    public string AutoDL = null;
    //    public const string fn_Remark = "Remark";
    //    [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
    //    public string Remark = null;
    //    public const string fn_Flag = "Flag";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
    //    public int Flag = int.MinValue;
    //    public const string fn_Editor = "Editor";
    //    [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
    //    public string Editor = null;
    //    public const string fn_Cdt = "Cdt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime Cdt = DateTime.MinValue;
    //    public const string fn_Udt = "Udt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime Udt = DateTime.MinValue;
    //    public const string fn_Descr2 = "Descr2";
    //    [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
    //    public string Descr2 = null;
    //}

    //[Table("PartInfo")]//Same as the one in DB.cs
    //public class PartInfo
    //{
    //    public const string fn_ID = "ID";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
    //    public int ID = int.MinValue;
    //    public const string fn_PartNo = "PartNo";
    //    [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
    //    public string PartNo = null;
    //    public const string fn_InfoType = "InfoType";
    //    [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
    //    public string InfoType = null;
    //    public const string fn_InfoValue = "InfoValue";
    //    [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
    //    public string InfoValue = null;
    //    public const string fn_Editor = "Editor";
    //    [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
    //    public string Editor = null;
    //    public const string fn_Cdt = "Cdt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime Cdt = DateTime.MinValue;
    //    public const string fn_Udt = "Udt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime Udt = DateTime.MinValue;
    //}

    [Table("PartType")]
    public class PartType
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 6, false, false, "")]
        public String code = null;

        public const string fn_cust = "Cust";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String cust = null;

        public const string fn_description = "Description";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_indx = "Indx";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String indx = null;

        public const string fn_site = "Site";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String site = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Model")]//Same as the one in DB.cs
    public class Model
    {
        public const string fn_bomapprovedate = "BOMApproveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime bomapprovedate = DateTime.MinValue;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_custPN = "CustPN";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String custPN = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_oscode = "OSCode";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String oscode = null;

        public const string fn_osdesc = "OSDesc";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String osdesc = null;

        public const string fn_price = "Price";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String price = null;

        public const string fn_region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String region = null;

        public const string fn_shipType = "ShipType";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String shipType = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("AssemblyCode")]//Same as the one in DB.cs
    public class AssemblyCode
    {
        public const string fn_assemblyCode = "AssemblyCode";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String assemblyCode = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String region = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MoBOM")]//Same as the one in DB.cs
    public class MoBOM
    {
        public const string fn_alternative_item_group = "Alternative_item_group";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String alternative_item_group = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_component = "Component";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String component = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_material = "Material";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String material = null;

        public const string fn_mo = "Mo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String mo = null;

        public const string fn_plant = "Plant";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String plant = null;

        public const string fn_priority = "Priority";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String priority = null;

        public const string fn_quantity = "Quantity";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String quantity = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Family")]
    public class Family
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_customerID = "CustomerID";
        [DBField(SqlDbType.VarChar, 0, 80, false, false, "")]
        public String customerID = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("CTOBom")]
    public class Ctobom
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mpno = "MPno";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String mpno = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String remark = null;

        public const string fn_spno = "SPno";
        [DBField(SqlDbType.Char, 0, 50, true, false, "")]
        public String spno = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MO_Excel")]
    public class Mo_Excel
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_printQty = "PrintQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 printQty = int.MinValue;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    #endregion

    [Table("PartCheckSetting")]
    public class PartCheckSetting_NEW
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String customer = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String partType = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_valueType = "ValueType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String valueType = null;
    }

    [Table("ModelInfo")]//Same as the one in DB.cs
    public class ModelInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, int.MinValue, int.MaxValue, false, true, "")]
        public Int64 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_name = "Name";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String name = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_value = "Value";
        [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
        public String value = null;
    }

    [Table("Line")]//Same as the one in DB.cs
    public class Line
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_customerID = "CustomerID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String customerID = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String stage = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("AssetRange")]
    public class AssetRange
    {
        public const string fn__Begin_ = "[Begin]";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String _Begin_ = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn__End_ = "[End]";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String _End_ = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String remark = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1,false, false, "")]
        public String status = null;
    }

    [Table("CELDATA")] //add by yunfeng
    public class CELDATA
    {
        public const string fn_platform = "Platform";
        [DBField(SqlDbType.Char, 0, 100, false, false, "")]
        public String platform = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_productSeriesName = "ProductSeriesName";
        [DBField(SqlDbType.Char, 0, 100, false, false, "")]
        public String productSeriesName = null;

        public const string fn_category = "Category";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String category = null;

        public const string fn_grade = "Grade";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 grade = int.MinValue;

        public const string fn_tec = "TEC";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tec = null;

        public const string fn_zmod = "ZMOD";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String zmod = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String editor = null;
    }

    [Table("DescType")]
    public class DescType
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String code = null;

        public const string fn_description = "Description";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Station")]
    public class Station
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_name = "Name";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String name = null;

        public const string fn_operationObject = "OperationObject";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 operationObject = int.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_stationType = "StationType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String stationType = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MBCode")]
    public class Mbcode
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.VarChar, 0, 3, false, false, "")]
        public String mbcode = null;

        public const string fn_multiQty = "MultiQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 multiQty = short.MinValue;

        //public const string fn_status_int = "Status_int";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        //public Int32 status_int = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Stage")]
    public class Stage
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String stage = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("SupplierCode")]
    public class SupplierCode
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_idex = "Idex";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String idex = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String vendor = null;
    }

    [Table("MBCFG")]
    public class Mbcfg
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cfg = "CFG";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String cfg = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String mbcode = null;

        public const string fn_series = "Series";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String series = null;

        public const string fn_tp = "TP";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Bom_Code")]
    public class Bom_Code
    {
        public const string fn_description = "description";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String description = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_os_code = "os_code";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String os_code = null;

        public const string fn_os_desc = "os_desc";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String os_desc = null;

        public const string fn_part_number = "part_number";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String part_number = null;

        public const string fn_part_number_type = "part_number_type";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String part_number_type = null;

        public const string fn_product_family = "product_family";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String product_family = null;

        public const string fn_uio_buyer_code = "uio_buyer_code";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String uio_buyer_code = null;
    }

    [Table("ConstValue")]
    public class ConstValue
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_name = "Name";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String name = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_value = "Value";
        [DBField(SqlDbType.NVarChar, 0, 3000, true, false, "")]
        public String value = null;
    }

    [Table("ConstValueType")]
    public class ConstValueType
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_value = "Value";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String value = null;
    }

	[Table("SysSetting")]
	public class SysSetting
	{
		public const string fn_description = "Description";
		[DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
		public String description = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_name = "Name";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String name = null;

		public const string fn_value = "Value";
		[DBField(SqlDbType.NVarChar, 0, 3000, true, false, "")]
		public String value = null;
	}

    [Table("HPWeekCode")]
    public class Hpweekcode
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String code = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.Char, 0, 50, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.Char, 0, 255, true, false, "")]
        public String remark = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("StationCheck")]
    public class StationCheck
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String customer = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("CheckItemType")]
    public class CheckItemType
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkModule = "CheckModule";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String checkModule = null;

        public const string fn_displayName = "DisplayName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String displayName = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_filterModule = "FilterModule";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String filterModule = null;

        public const string fn_matchModule = "MatchModule";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String matchModule = null;

        public const string fn_name = "Name";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String name = null;

        public const string fn_needCommonSave = "NeedCommonSave";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Nullable<bool> needCommonSave = null;

        public const string fn_needUniqueCheck = "NeedUniqueCheck";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Nullable<bool> needUniqueCheck = null;

		public const string fn_needPartForbidCheck = "NeedPartForbidCheck";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Nullable<bool> needPartForbidCheck = null;//default(bool);

        public const string fn_repairPartType = "RepairPartType";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Nullable<bool> repairPartType = null;//default(bool);

        public const string fn_needDefectComponentCheck = "NeedDefectComponentCheck";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Nullable<bool> needDefectComponentCheck = null;//default(bool);

        public const string fn_saveModule = "SaveModule";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String saveModule = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PrintLog")]//Same as the one in DB.cs
    public class PrintLog
    {
        public const string fn_begNo = "BegNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String begNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endNo = "EndNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String endNo = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_name = "Name";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String name = null;

        public const string fn_labelTemplate = "LabelTemplate";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String labelTemplate = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String station = null;
    }

    [Table("DefectInfo")]//Same as the one in DB.cs
    public class DefectInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String code = null;

        public const string fn_customerID = "CustomerID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String customerID = null;

        public const string fn_description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_engDescr = "EngDescr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String engDescr = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("NumControl")]//Same as the one in DB.cs
    public class NumControl
    {
        public const string fn_customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String customer = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_noName = "NoName";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public String noName = null;

        public const string fn_noType = "NoType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String noType = null;

        public const string fn_value = "Value";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String value = null;
    }

    [Table("ForceNWC")]
    public class ForceNWC
    {
        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_forceNWC = "ForceNWC";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String forceNWC = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_preStation = "PreStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String preStation = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String productID = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

	[Table("FamilyInfo")]
	public class FamilyInfo
	{
		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_descr = "Descr";
		[DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
		public String descr = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
		public String editor = null;

		public const string fn_family = "Family";
		[DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
		public String family = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_name = "Name";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String name = null;

		public const string fn_udt = "Udt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime udt = DateTime.MinValue;

		public const string fn_value = "Value";
		[DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
		public String value = null;
	}

    [Table("RePrintLog")]
    public class RePrintLog
    {
        public const string fn_begNo = "BegNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String begNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endNo = "EndNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String endNo = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_labelName = "LabelName";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String labelName = null;

        public const string fn_reason = "Reason";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public String reason = null;
    }

    [Table("Line_Station")]
    public class Line_Station
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Process")]
    public class Process
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String process = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ProcessRule")]
    public class ProcessRule
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String process = null;

        public const string fn_ruleSetID = "RuleSetID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 ruleSetID = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_value1 = "Value1";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String value1 = null;

        public const string fn_value2 = "Value2";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String value2 = null;

        public const string fn_value3 = "Value3";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String value3 = null;

        public const string fn_value4 = "Value4";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String value4 = null;

        public const string fn_value5 = "Value5";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String value5 = null;

        public const string fn_value6 = "Value6";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String value6 = null;
    }

    [Table("Process_Station")]
    public class Process_Station
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_preStation = "PreStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String preStation = null;

        public const string fn_process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String process = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DefectCode")]
    public class DefectCode
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_defect = "Defect";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public String defect = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_engDescr = "EngDescr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String engDescr = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PrintTemplate")]
    public class PrintTemplate
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_labelType = "LabelType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String labelType = null;

        public const string fn_layout = "Layout";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 layout = int.MinValue;

        public const string fn_piece = "Piece";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 piece = int.MinValue;

        public const string fn_spName = "SpName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String spName = null;

        public const string fn_templateName = "TemplateName";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String templateName = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PartProcess")]
    public class PartProcess
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_mbfamily = "MBFamily";
        [DBField(SqlDbType.VarChar, 0, 80, false, false, "")]
        public String mbfamily = null;

        public const string fn_pilotRun = "PilotRun";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public String pilotRun = null;

        public const string fn_process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String process = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Warranty")]
    public class Warranty
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String customer = null;

        public const string fn_dateCodeType = "DateCodeType";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String dateCodeType = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_shipTypeCode = "ShipTypeCode";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String shipTypeCode = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_warrantyCode = "WarrantyCode";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String warrantyCode = null;

        public const string fn_warrantyFormat = "WarrantyFormat";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String warrantyFormat = null;
    }

    [Table("ConcurrentLocks")]
    public class ConcurrentLocks
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime2, 0, 8, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_clientAddr = "ClientAddr";
        [DBField(SqlDbType.VarChar, 0, 512, false, false, "")]
        public String clientAddr = null;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 512, false, false, "")]
        public String customer = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 512, false, false, "")]
        public String editor = null;

        public const string fn_guid = "GUID";
        [DBField(SqlDbType.UniqueIdentifier, 0, 16, false, false, "")]
        public Guid guid = Guid.Empty;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 512, false, false, "")]
        public String line = null;

        public const string fn__LockValue = "LockValue";
        [DBField(SqlDbType.VarChar, 0, 128, false, false, "")]
        public String _LockValue = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 512, false, false, "")]
        public String station = null;

        public const string fn_timeoutSpan4Hold = "TimeoutSpan4Hold";
        [DBField(SqlDbType.BigInt, 0, 10, false, false, "")]
        public long timeoutSpan4Hold = long.MinValue;

        public const string fn_timeoutSpan4Wait = "TimeoutSpan4Wait";
        [DBField(SqlDbType.BigInt, 0, 10, false, false, "")]
        public long timeoutSpan4Wait = long.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String type = null;
    }

    [Table("Dept")]
    public class Dept
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_dept = "Dept";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public String dept = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_endTime = "EndTime";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String endTime = null;

        public const string fn_fisline = "FISLine";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public String fisline = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String remark = null;

        public const string fn_section = "Section";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String section = null;

        public const string fn_startTime = "StartTime";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String startTime = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("CacheUpdate")]
    public class CacheUpdate
    {
        public const string fn_appName = "AppName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String appName = null;

        public const string fn_cacheServerIP = "CacheServerIP";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String cacheServerIP = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_item = "Item";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String item = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_updated = "Updated";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean updated = default(bool);
    }

    [Table("KeyPartDefectCollection")]
    public class KeyPartDefectCollection
    {
        public const string fn_appearance = "Appearance";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String appearance = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_date = "Date";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String date = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_module = "Module";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String module = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String partNo = null;

        public const string fn_parts = "Parts";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String parts = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String pdLine = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, false, false, "")]
        public String remark = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String vendor = null;
    }

    [Table("FAI_INFO")]
    public class Fai_Info
    {
        public const string fn_bat_typ = "BAT_TYP";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String bat_typ = null;

        public const string fn_bios_typ = "BIOS_TYP";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String bios_typ = null;

        public const string fn_chk_stat = "CHK_Stat";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String chk_stat = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_fdd_sup = "FDD_Sup";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String fdd_sup = null;

        public const string fn_fin_time = "FIN_Time";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime fin_time = DateTime.MinValue;

        public const string fn_hdd_sup = "HDD_Sup";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String hdd_sup = null;

        public const string fn_hpqpn = "HPQPN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String hpqpn = null;

        public const string fn_iecpn = "IECPN";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String iecpn = null;

        public const string fn_imp_record = "IMP_Record";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String imp_record = null;

        public const string fn_kbc_ver = "KBC_Ver";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String kbc_ver = null;

        public const string fn_ng_record = "NG_Record";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String ng_record = null;

        public const string fn_opt_sup = "OPT_Sup";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String opt_sup = null;

        public const string fn_ram_typ = "RAM_TYP";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String ram_typ = null;

        public const string fn_rec_time = "REC_Time";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime rec_time = DateTime.MinValue;

        public const string fn_sno = "SNO";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String sno = null;

        public const string fn_upc_code = "UPC_Code";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String upc_code = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vdo_bios = "VDO_BIOS";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String vdo_bios = null;
    }

    #endregion

    #region FA

    [Table("ProductBT")]
    public class ProductBT
    {
        public const string fn_bt = "BT";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String bt = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("FA_SnoBTDet")]
    public class Fa_Snobtdet
    {
        public const string fn_bt = "BT";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String bt = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, true, false, "")]
        public String snoId = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ProductLog")]//Same as the one in DB.cs
    public class ProductLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;
    }

    [Table("Product_Part")]//Same as the one in DB.cs
    public class Product_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Product")]//Same as the one in DB.cs
    public class Product
    {
        public const string fn_cartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String cartonSN = null;

        public const string fn_cartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cartonWeight = decimal.MinValue;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String custsn = null;

        public const string fn_cvsn = "CVSN";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public String cvsn = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String deliveryNo = null;

        public const string fn_ecr = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String ecr = null;

        public const string fn_mac = "MAC";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public String mac = null;

        public const string fn_mbecr = "MBECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String mbecr = null;

        public const string fn_mo = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String mo = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_ooaid = "OOAID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String ooaid = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String palletNo = null;

        public const string fn_pcbid = "PCBID";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public String pcbid = null;

        public const string fn_pcbmodel = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String pcbmodel = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String pizzaID = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_prsn = "PRSN";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String prsn = null;

        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String state = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;

        public const string fn_uuid = "UUID";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public String uuid = null;
    }

    [Table("ProductInfo")]//Same as the one in DB.cs
    public class ProductInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 225, true, false, "")]
        public String infoValue = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String productID = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Special_Det")]
    public class Special_Det
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_sno1 = "Sno1";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String sno1 = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String snoId = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("KittingCode")]//Same as the one in DB.cs
    public class KittingCode
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String code = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.Char, 0, 255, false, false, "")]
        public String remark = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ProductStatus")]//Same as the one in DB.cs
    public class ProductStatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_reworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public String reworkCode = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_testFailCount = "TestFailCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 testFailCount = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("AssetRule")]
    public class AssetRule
    {
        public const string fn_astType = "AstType";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String astType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkingType = "CheckingType";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public String checkingType = null;

        public const string fn_checkItem = "CheckItem";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String checkItem = null;

        public const string fn_custName = "CustName";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String custName = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 6, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ICASA")]
    public class Icasa
    {
        public const string fn_antel1 = "Antel1";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String antel1 = null;

        public const string fn_antel2 = "Antel2";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String antel2 = null;

        public const string fn_av = "AV";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String av = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String editor = null;

        public const string fn_icasa = "ICASA";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String icasa = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vc = "VC";
        [DBField(SqlDbType.Char, 0, 5, false, false, "")]
        public String vc = null;
    }

    [Table("IqcPnoBom")]
    public class IqcPnoBom
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String descr = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pno = null;

        public const string fn_sps = "SPS";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String sps = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vc = "VC";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String vc = null;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.VarChar, 0, 40, false, false, "")]
        public String vendor = null;
    }

    [Table("SupplierCode_FA")]
    public class SupplierCode_FA
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_idex = "Idex";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 idex = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String vendor = null;
    }

    [Table("MasterLabel")]
    public class MasterLabel
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vc = "VC";
        [DBField(SqlDbType.Char, 0, 6, false, false, "")]
        public String vc = null;
    }

    [Table("RunInTimeControl")]
    public class RunInTimeControl
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_controlType = "ControlType";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean controlType = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hour = "Hour";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String hour = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_testStation = "TestStation";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String testStation = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("RunInTimeControlLog")]
    public class RunInTimeControlLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_controlType = "ControlType";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean controlType = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hour = "Hour";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String hour = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_testStation = "TestStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String testStation = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;
    }

    [Table("QCStatus")]//Same as the one in DB.cs
    public class Qcstatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String remark = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MP_BTOrder")]
    public class Mp_Btorder
    {
        public const string fn_bt = "BT";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String bt = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_outQty = "OutQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 outQty = int.MinValue;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String pno = null;

        public const string fn_prtQty = "PrtQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 prtQty = int.MinValue;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_ref_date = "REF_DATE";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String ref_date = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String remark = null;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String shipDate = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Special_Maintain")]
    public class Special_Maintain
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_db = "DB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String db = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 18, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_l_WC = "L_WC";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String l_WC = null;

        public const string fn_m_WC = "M_WC";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String m_WC = null;

        public const string fn_message = "Message";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String message = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String remark = null;

        public const string fn_swc = "SWC";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String swc = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String tp = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("AstRule")]
    public class AstRule
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItem = "CheckItem";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItem = null;

        public const string fn_checkTp = "CheckTp";
        [DBField(SqlDbType.VarChar, 0, 4, true, false, "")]
        public String checkTp = null;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String code = null;

        public const string fn_custName = "CustName";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String custName = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 500, true, false, "")]
        public String remark = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 6, false, false, "")]
        public String station = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 6, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("TSModel")]
    public class Tsmodel
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String mark = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String model = null;
    }

    [Table("DefectCode_Station")]
    public class DefectCode_Station
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cause = "Cause";
        [DBField(SqlDbType.Char, 0, 10, false, false,"")]
        public String cause =null;

        public const string fn_majorPart = "MajorPart";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String majorPart = null;

        public const string fn_crt_stn = "CRT_STN";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String crt_stn = null;

        public const string fn_defect = "Defect";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String defect = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_nxt_stn = "NXT_STN";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String nxt_stn = null;

        public const string fn_pre_stn = "PRE_STN";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String pre_stn = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String family = null;
    }

    [Table("UnpackProduct")]//Same as the one in DB.cs
    public class UnpackProduct
    {
        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String custsn = null;

        public const string fn_cvsn = "CVSN";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public String cvsn = null;

        public const string fn_cartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String cartonSN = null;

        public const string fn_cartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cartonWeight = decimal.MinValue;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String deliveryNo = null;

        public const string fn_ecr = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String ecr = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mac = "MAC";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public String mac = null;

        public const string fn_mbecr = "MBECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String mbecr = null;

        public const string fn_mo = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String mo = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_ooaid = "OOAID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String ooaid = null;

        public const string fn_pcbid = "PCBID";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public String pcbid = null;

        public const string fn_pcbmodel = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String pcbmodel = null;

        public const string fn_prsn = "PRSN";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String prsn = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String palletNo = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String pizzaID = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String state = null;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_uuid = "UUID";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public String uuid = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;
    }

    [Table("IqcKp")]
    public class IqcKp
    {
        public const string fn_cause = "Cause";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public String cause = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_ctLabel = "CtLabel";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String ctLabel = null;

        public const string fn_defect = "Defect";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public String defect = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String location = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String model = null;

        public const string fn_obligation = "Obligation";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String obligation = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 500, true, false, "")]
        public String remark = null;

        public const string fn_result = "Result";
        [DBField(SqlDbType.VarChar, 0, 500, true, false, "")]
        public String result = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("IqcCause1")]
    public class IqcCause1
    {
        public const string fn_ctLabel = "CtLabel";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String ctLabel = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iqcDefect = "IqcDefect";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public String iqcDefect = null;

        public const string fn_mpDefect = "MpDefect";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public String mpDefect = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_veDefect = "VeDefect";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String veDefect = null;

        public const string fn_veLabel = "VeLabel";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String veLabel = null;
    }

    [Table("FA_PA_LightSt")]
    public class Fa_Pa_Lightst
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.NVarChar, 0, 16, false, false, "")]
        public String pno = null;

        public const string fn_stn = "Stn";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String stn = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ITCNDCheckSetting")]
    public class Itcndchecksetting
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkCondition = "CheckCondition";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String checkCondition = null;

        public const string fn_checkItem = "CheckItem";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String checkItem = null;

        public const string fn_checkType = "CheckType";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String checkType = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;
    }

    [Table("ITCND_DCDet")]
    public class Itcnd_Dcdet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_dc = "DC";
        [DBField(SqlDbType.NVarChar, 0, 2500, true, false, "")]
        public String dc = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, true, false, "")]
        public String snoId = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("CDSIAST")]
    public class Cdsiast
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_sno = "Sno";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String sno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String snoId = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String tp = null;
    }

    [Table("UnpackProductInfo")]
    public class UnpackProductInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public String infoValue = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String productID = null;

        public const string fn_productInfoID = "ProductInfoID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 productInfoID = int.MinValue;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UnpackProduct_Part")]
    public class UnpackProduct_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_product_PartID = "Product_PartID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 product_PartID = int.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UnpackProductStatus")]
    public class UnpackProductStatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_reworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public String reworkCode = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_testFailCount = "TestFailCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 testFailCount = int.MinValue;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

	[Table("ProductRepair_DefectInfo")]
	public class ProductRepair_DefectInfo
	{
        public const string fn__4M_ = "[4M]";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String _4M_ = null;

		public const string fn_action = "Action";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String action = null;

		public const string fn_cause = "Cause";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String cause = null;

		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_component = "Component";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String component = null;

		public const string fn_cover = "Cover";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String cover = null;

		public const string fn_defectCode = "DefectCode";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String defectCode = null;

		public const string fn_distribution = "Distribution";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String distribution = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
		public String editor = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_isManual = "IsManual";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
		public Int32 isManual = int.MinValue;

		public const string fn_location = "Location";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String location = null;

		public const string fn_majorPart = "MajorPart";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String majorPart = null;

		public const string fn_manufacture = "Manufacture";
		[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
		public String manufacture = null;

		public const string fn_mark = "Mark";
		[DBField(SqlDbType.Char, 0, 1, false, false, "")]
		public String mark = null;

		public const string fn_mtaid = "MTAID";
		[DBField(SqlDbType.VarChar, 0, 14, true, false, "")]
		public String mtaid = null;

		public const string fn_newPart = "NewPart";
		[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
		public String newPart = null;

		public const string fn_newPartSno = "NewPartSno";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String newPartSno = null;

		public const string fn_obligation = "Obligation";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String obligation = null;

		public const string fn_oldPart = "OldPart";
		[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
		public String oldPart = null;

		public const string fn_oldPartSno = "OldPartSno";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String oldPartSno = null;

		public const string fn_partType = "PartType";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String partType = null;

		public const string fn_piastation = "PIAStation";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String piastation = null;

		public const string fn_productRepairID = "ProductRepairID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
		public Int32 productRepairID = int.MinValue;

		public const string fn_remark = "Remark";
		[DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
		public String remark = null;

		public const string fn_responsibility = "Responsibility";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String responsibility = null;

		public const string fn_returnSign = "ReturnSign";
		[DBField(SqlDbType.Char, 0, 1, false, false, "")]
		public String returnSign = null;

		public const string fn_returnStn = "ReturnStn";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String returnStn = null;

		public const string fn_site = "Site";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String site = null;

		public const string fn_subDefect = "SubDefect";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String subDefect = null;

		public const string fn_trackingStatus = "TrackingStatus";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String trackingStatus = null;

		public const string fn_type = "Type";
		[DBField(SqlDbType.Char, 0, 10, false, false, "")]
		public String type = null;

		public const string fn_udt = "Udt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime udt = DateTime.MinValue;

		public const string fn_uncover = "Uncover";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String uncover = null;

		public const string fn_vendorCT = "VendorCT";
		[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
		public String vendorCT = null;

		public const string fn_versionA = "VersionA";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String versionA = null;

		public const string fn_versionB = "VersionB";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String versionB = null;
	}

    [Table("Kitting_Loc_PLMapping")]
    public class Kitting_Loc_PLMapping
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lightNo = "LightNo";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 lightNo = short.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String pdLine = null;

        public const string fn_tagID = "TagID";
        [DBField(SqlDbType.NChar, 0, 12, false, false, "")]
        public String tagID = null;
    }

    //[Table("KittingBoxSN")]
    //public class KittingBoxSN
    //{
    //    public const string fn_cdt = "Cdt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime cdt = DateTime.MinValue;

    //    public const string fn_remark = "Remark";
    //    [DBField(SqlDbType.Char, 0, 30, false, false, "")]
    //    public String remark = null;

    //    public const string fn_sno = "Sno";
    //    [DBField(SqlDbType.Char, 0, 4, false, false, "")]
    //    public String sno = null;

    //    public const string fn_snoId = "SnoId";
    //    [DBField(SqlDbType.Char, 0, 9, false, false, "")]
    //    public String snoId = null;

    //    public const string fn_status = "Status";
    //    [DBField(SqlDbType.Char, 0, 1, false, false, "")]
    //    public String status = null;

    //    public const string fn_tp = "Tp";
    //    [DBField(SqlDbType.Char, 0, 3, false, false, "")]
    //    public String tp = null;

    //    public const string fn_udt = "Udt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime udt = DateTime.MinValue;
    //}

    [Table("FAKittingBoxSN")]
    public class Fakittingboxsn
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String pdLine = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String remark = null;

        public const string fn_sno = "Sno";
        [DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
        public String sno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String snoId = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("BorrowLog")]
    public class BorrowLog
    {
        public const string fn_acceptor = "Acceptor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String acceptor = null;

        public const string fn_bdate = "Bdate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime bdate = DateTime.MinValue;

        public const string fn_borrower = "Borrower";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String borrower = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lender = "Lender";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String lender = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_rdate = "Rdate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime rdate = DateTime.MinValue;

        public const string fn_returner = "Returner";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String returner = null;

        public const string fn_sn = "Sn";
        [DBField(SqlDbType.VarChar, 0, 14, false, false, "")]
        public String sn = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }

    [Table("SnoDet_PoMo")]
    public class SnoDet_PoMo
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String boxId = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_delivery = "Delivery";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String delivery = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_mo = "Mo";
        [DBField(SqlDbType.Char, 0, 14, true, false, "")]
        public String mo = null;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String plt = null;

        public const string fn_po = "PO";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String po = null;

        public const string fn_poitem = "POItem";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String poitem = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String remark = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, true, "")]
        public String snoId = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("KittingLog")]
    public class KittingLog
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String boxId = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_configedLEDStatus = "ConfigedLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean configedLEDStatus = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_ledvalues = "LEDValues";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String ledvalues = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_priority = "Priority";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 priority = int.MinValue;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_runningLEDStatus = "RunningLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean runningLEDStatus = default(bool);

        public const string fn_tableName = "TableName";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String tableName = null;

        public const string fn_tagID = "TagID";
        [DBField(SqlDbType.NChar, 0, 12, false, false, "")]
        public String tagID = null;

        public const string fn_time = "Time";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 time = int.MinValue;

        public const string fn_id = "id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;
    }

    [Table("ITCNDCheckQCHold")]
    public class Itcndcheckqchold
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String code = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 200, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_isHold = "IsHold";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String isHold = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("QCRatio")]
    public class Qcratio
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_eoqcratio = "EOQCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 eoqcratio = int.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_paqcratio = "PAQCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 paqcratio = int.MinValue;

        public const string fn_qcratio = "QCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 qcratio = int.MinValue;

        public const string fn_rpaqcratio = "RPAQCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 rpaqcratio = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UnitWeightLog")]
    public class UnitWeightLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;
    }

    [Table("HP_WWANLabel")]
    public class Hp_Wwanlabel
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 100, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_labelESn = "LabelESn";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String labelESn = null;

        public const string fn_labelICCID = "LabelICCID";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String labelICCID = null;

        public const string fn_labelIMEI = "LabelIMEI";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String labelIMEI = null;

        public const string fn_labelMEID = "LabelMEID";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String labelMEID = null;

        public const string fn_moduleNo = "ModuleNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String moduleNo = null;

        public const string fn_printType = "PrintType";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String printType = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ProductRepair_DefectInfo_BackUp")]
    public class ProductRepair_DefectInfo_BackUp
    {
        public const string fn__4M_ = "[4M]";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String _4M_ = null;

        public const string fn_action = "Action";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String action = null;

        public const string fn_backUpCdt = "BackUpCdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime backUpCdt = DateTime.MinValue;

        public const string fn_backUpEditor = "BackUpEditor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String backUpEditor = null;

        public const string fn_backUpID = "BackUpID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 backUpID = int.MinValue;

        public const string fn_backUpProductID = "BackUpProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String backUpProductID = null;

        public const string fn_cause = "Cause";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cause = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_component = "Component";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String component = null;

        public const string fn_cover = "Cover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cover = null;

        public const string fn_defectCode = "DefectCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String defectCode = null;

        public const string fn_distribution = "Distribution";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String distribution = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 id = int.MinValue;

        public const string fn_isManual = "IsManual";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 isManual = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String location = null;

        public const string fn_mtaid = "MTAID";
        [DBField(SqlDbType.VarChar, 0, 14, true, false, "")]
        public String mtaid = null;

        public const string fn_majorPart = "MajorPart";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String majorPart = null;

        public const string fn_manufacture = "Manufacture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String manufacture = null;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mark = null;

        public const string fn_newPart = "NewPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String newPart = null;

        public const string fn_newPartSno = "NewPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String newPartSno = null;

        public const string fn_obligation = "Obligation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String obligation = null;

        public const string fn_oldPart = "OldPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String oldPart = null;

        public const string fn_oldPartSno = "OldPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String oldPartSno = null;

        public const string fn_piastation = "PIAStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String piastation = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partType = null;

        public const string fn_productRepairID = "ProductRepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 productRepairID = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_responsibility = "Responsibility";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String responsibility = null;

        public const string fn_returnSign = "ReturnSign";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String returnSign = null;

        public const string fn_returnStn = "ReturnStn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String returnStn = null;

        public const string fn_site = "Site";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String site = null;

        public const string fn_subDefect = "SubDefect";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String subDefect = null;

        public const string fn_trackingStatus = "TrackingStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String trackingStatus = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_uncover = "Uncover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String uncover = null;

        public const string fn_vendorCT = "VendorCT";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String vendorCT = null;

        public const string fn_versionA = "VersionA";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String versionA = null;

        public const string fn_versionB = "VersionB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String versionB = null;
    }

    #endregion

    #region PAK

    [Table("CSNMas")]
    public class Csnmas
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_csn1 = "CSN1";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String csn1 = null;

        public const string fn_csn2 = "CSN2";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String csn2 = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 6, false, false, "")]
        public String pdLine = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String pno = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PoPlt")]
    public class PoPlt
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_combineQty = "CombineQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 combineQty = int.MinValue;

        public const string fn_conQTY = "ConQTY";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 conQTY = int.MinValue;

        public const string fn_consolidate = "Consolidate";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String consolidate = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String plt = null;

        public const string fn_qty = "QTY";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_ucc = "UCC";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String ucc = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ShipBoxDet")]
    public class ShipBoxDet
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String boxId = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 40, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_invioce = "Invioce";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String invioce = null;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String plt = null;

        public const string fn_shipment = "Shipment";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String shipment = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String snoId = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    //[Table("DeliveryLog")]
    //public class DeliveryLog
    //{
    //    public const string fn_cdt = "Cdt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime cdt = DateTime.MinValue;

    //    public const string fn_editor = "Editor";
    //    [DBField(SqlDbType.Char, 0, 20, false, false, "")]
    //    public String editor = null;

    //    public const string fn_id = "ID";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
    //    public Int32 id = int.MinValue;

    //    public const string fn_isPass = "IsPass";
    //    [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
    //    public Int16 isPass = short.MinValue;

    //    public const string fn_pdLine = "PdLine";
    //    [DBField(SqlDbType.Char, 0, 30, false, false, "")]
    //    public String pdLine = null;

    //    public const string fn_pno = "Pno";
    //    [DBField(SqlDbType.Char, 0, 12, false, false, "")]
    //    public String pno = null;

    //    public const string fn_snoId = "SnoId";
    //    [DBField(SqlDbType.Char, 0, 14, false, false, "")]
    //    public String snoId = null;

    //    public const string fn_tp = "Tp";
    //    [DBField(SqlDbType.Char, 0, 10, false, false, "")]
    //    public String tp = null;

    //    public const string fn_wc = "WC";
    //    [DBField(SqlDbType.Char, 0, 2, false, false, "")]
    //    public String wc = null;
    //}

    [Table("CSNLog")]
    public class Csnlog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_isPass = "IsPass";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 isPass = short.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String pdLine = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String pno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String snoId = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_wc = "WC";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String wc = null;
    }

    [Table("COAMas")]
    public class Coamas
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 25, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pdLine = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.Char, 0, 16, false, false, "")]
        public String pno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String snoId = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_wc = "WC";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String wc = null;
    }

    [Table("SnoDet_BTLoc")]
    public class SnoDet_BTLoc
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cpqsno = "CPQSNO";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String cpqsno = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 25, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String pdLine = null;

        public const string fn_sno = "Sno";
        [DBField(SqlDbType.Char, 0, 35, false, false, "")]
        public String sno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String snoId = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String status = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAK_BTLocMas")]
    public class Pak_Btlocmas
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cmbQty = "CmbQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 cmbQty = int.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 25, false, false, "")]
        public String editor = null;

        public const string fn_fl = "FL";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String fl = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

        public const string fn_locQty = "LocQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 locQty = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 24, false, false, "")]
        public String model = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pdLine = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String snoId = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String status = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("COAStatus")]//Same as the one in DB.cs
    public class Coastatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_coasn = "COASN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String coasn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_iecpn = "IECPN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("COALog")]//Same as the one in DB.cs
    public class Coalog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_coasn = "COASN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String coasn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String tp = null;
    }

    [Table("DeliveryInfo")]//Same as the one in DB.cs
    public class DeliveryInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.NVarChar, 0, 200, true, false, "")]
        public String infoValue = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    [Table("V_Delivery")]//Same as the one in DB.cs
    public class V_Delivery
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String poNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime shipDate = DateTime.MinValue;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String shipmentNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    [Table("V_DeliveryInfo")]//Same as the one in DB.cs
    public class V_DeliveryInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.NVarChar, 0, 200, true, false, "")]
        public String infoValue = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    [Table("V_Delivery_Pallet")]//Same as the one in DB.cs
    public class V_Delivery_Pallet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_deliveryQty = "DeliveryQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 deliveryQty = short.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String shipmentNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_deviceQty = "DeviceQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 deviceQty = int.MinValue;

        public const string fn_palletType = "PalletType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletType = null;
    }
    [Table("V_Dummy_ShipDet")]
    public class V_Dummy_ShipDet
    {
        public const string fn_bol = "BOL";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String bol = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String plt = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, true, "")]
        public String snoId = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    [Table("Delivery_Pallet")]//Same as the one in DB.cs
    public class Delivery_Pallet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_deliveryQty = "DeliveryQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 deliveryQty = short.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String shipmentNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_deviceQty = "DeviceQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 deviceQty = int.MinValue;

        public const string fn_palletType = "PalletType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletType = null;
    }

    [Table("Pallet")]//Same as the one in DB.cs
    public class Pallet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_height = "Height";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal height = decimal.MinValue;

        public const string fn_length = "Length";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal length = decimal.MinValue;

        public const string fn_palletModel = "PalletModel";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String palletModel = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_ucc = "UCC";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String ucc = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal weight = decimal.MinValue;

		public const string fn_weight_L = "Weight_L";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal weight_L = decimal.MinValue;

        public const string fn_width = "Width";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal width = decimal.MinValue;

        public const string fn_floor = "Floor";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String floor = "";
    }

    [Table("PalletAttr")]
    public class PalletAttr
    {
        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String attrName = null;

        public const string fn_attrValue = "AttrValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrValue = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("COATrans_Log")]
    public class Coatrans_Log
    {
        public const string fn_begNo = "BegNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String begNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endNo = "EndNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String endNo = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.Char, 0, 14, true, false, "")]
        public String pno = null;

        public const string fn_preStatus = "PreStatus";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public String preStatus = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public String status = null;
    }

    [Table("WipBuffer")]//Same as the one in DB.cs
    public class WipBuffer
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_kittingType = "KittingType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String kittingType = null;

        public const string fn_lightNo = "LightNo";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String lightNo = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String line = null;

        public const string fn_max_Stock = "Max_Stock";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 max_Stock = int.MinValue;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_picture = "Picture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String picture = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String remark = null;

        public const string fn_safety_Stock = "Safety_Stock";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 safety_Stock = int.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public String station = null;

        public const string fn_sub = "Sub";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String sub = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("LabelKitting")]
    public class LabelKitting
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String remark = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 15, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("KitLoc")]
    public class KitLoc
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String location = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String pdLine = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAKitLoc")]
    public class Pakitloc
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 20, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String location = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.NVarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String pdLine = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.NVarChar, 0, 30, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ChepPallet")]
    public class ChepPallet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("BT_SeaShipmentSku")]
    public class Bt_Seashipmentsku
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String pdLine = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("InternalCOA")]
    public class InternalCOA
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.Char, 0, 16, true, false, "")]
        public String model = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String type = null;
    }

    [Table("OfflineLabelSetting")]
    public class OfflineLabelSetting
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_description = "Description";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_fileName = "FileName";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String fileName = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_labelSpec = "LabelSpec";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String labelSpec = null;

        public const string fn_param1 = "Param1";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param1 = null;

        public const string fn_param2 = "Param2";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param2 = null;

        public const string fn_param3 = "Param3";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param3 = null;

        public const string fn_param4 = "Param4";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param4 = null;

        public const string fn_param5 = "Param5";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param5 = null;

        public const string fn_param6 = "Param6";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param6 = null;

        public const string fn_param7 = "Param7";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param7 = null;

        public const string fn_param8 = "Param8";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String param8 = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_spname = "SPName";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String spname = null;

        public const string fn_printMode = "PrintMode";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int printMode = int.MinValue;
    }

    [Table("PODLabelPart")]
    public class Podlabelpart
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("OlymBattery")]
    public class OlymBattery
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hppn = "HPPN";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String hppn = null;

        public const string fn_hssn = "HSSN";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String hssn = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAK_LocMas")]
    public class Pak_Locmas
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 25, false, false, "")]
        public String editor = null;

        public const string fn_fl = "FL";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String fl = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pdLine = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.Char, 0, 16, false, false, "")]
        public String pno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String snoId = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_wc = "WC";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String wc = null;
    }

    [Table("ChinaLabel")]
    public class ChinaLabel
    {
        public const string fn_family = "Family";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pn = "PN";
        [DBField(SqlDbType.NVarChar, 0, 20, false, false, "")]
        public String pn = null;
    }

    [Table("PAK_CHN_TW_Light")]
    public class Pak_Chn_Tw_Light
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lightNo = "LightNo";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String lightNo = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String model = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String partNo = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Pallet_RFID")]
    public class Pallet_RFID
    {
        public const string fn_carrier = "Carrier";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String carrier = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String plt = null;

        public const string fn_rfidcode = "RFIDCode";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String rfidcode = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAK_WHLoc_TMP")]
    public class Pak_Whloc_Tmp
    {
        public const string fn_bol = "BOL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String bol = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_loc = "LOC";
        [DBField(SqlDbType.NChar, 0, 4, true, false, "")]
        public String loc = null;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.NVarChar, 0, 14, true, false, "")]
        public String plt = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAK_WH_LocMas")]
    public class Pak_Wh_Locmas
    {
        public const string fn_bol = "BOL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String bol = null;

        public const string fn_carrier = "Carrier";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String carrier = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_col = "Col";
        [DBField(SqlDbType.NChar, 0, 2, true, false, "")]
        public String col = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_loc = "Loc";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 loc = int.MinValue;

        public const string fn_plt1 = "PLT1";
        [DBField(SqlDbType.NVarChar, 0, 14, true, false, "")]
        public String plt1 = null;

        public const string fn_plt2 = "PLT2";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String plt2 = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("WH_PLTLog")]
    public class Wh_Pltlog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.NVarChar, 0, 14, true, false, "")]
        public String plt = null;

        public const string fn_wc = "WC";
        [DBField(SqlDbType.NChar, 0, 4, true, false, "")]
        public String wc = null;
    }

    [Table("WH_PLTMas")]
    public class Wh_Pltmas
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.NVarChar, 0, 14, true, false, "")]
        public String plt = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_wc = "WC";
        [DBField(SqlDbType.NChar, 0, 4, true, false, "")]
        public String wc = null;
    }

    [Table("SAP_WEIGHT")]
    public class Sap_Weight
    {
        public const string fn__DNDIVShipment_ = "[DN/Shipment]";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String _DNDIVShipment_ = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_kg = "KG";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal kg = decimal.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;
    }

	[Table("WH_PltWeight")]
	public class Wh_Pltweight
	{
		public const string fn_actualCartonWeight = "ActualCartonWeight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal actualCartonWeight = decimal.MinValue;

		public const string fn_actualPltWeight = "ActualPltWeight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal actualPltWeight = decimal.MinValue;

		public const string fn_dn = "DN";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String dn = null;

		public const string fn_forecasetCartonWeight = "ForecasetCartonWeight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal forecasetCartonWeight = decimal.MinValue;

		public const string fn_forecasetPltWeight = "ForecasetPltWeight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal forecasetPltWeight = decimal.MinValue;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_plt = "Plt";
		[DBField(SqlDbType.VarChar, 0, 12, true, false, "")]
		public String plt = null;

		public const string fn_pltMaterialWeight = "PltMaterialWeight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal pltMaterialWeight = decimal.MinValue;

		public const string fn_pltQty = "PltQty";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
		public Int32 pltQty = int.MinValue;

		public const string fn_pltWeightInaccuracy = "PltWeightInaccuracy";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
		public Decimal pltWeightInaccuracy = decimal.MinValue;

		public const string fn_remark = "Remark";
		[DBField(SqlDbType.VarChar, 0, 1, true, false, "")]
		public String remark = null;
	}

	[Table("Dummy_ShipDet")]
	public class Dummy_ShipDet
	{
		public const string fn_bol = "BOL";
		[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
		public String bol = null;

		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String editor = null;

		public const string fn_plt = "PLT";
		[DBField(SqlDbType.Char, 0, 12, true, false, "")]
		public String plt = null;

		public const string fn_snoId = "SnoId";
		[DBField(SqlDbType.Char, 0, 14, false, true, "")]
		public String snoId = null;

		public const string fn_udt = "Udt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
		public DateTime udt = DateTime.MinValue;
	}

	[Table("SAP_VOLUME_PLT")]
	public class Sap_Volume_Plt
	{
		public const string fn_a = "a";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal a = decimal.MinValue;

		public const string fn_b = "b";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal b = decimal.MinValue;

		public const string fn_cm = "CM";
		[DBField(SqlDbType.VarChar, 0, 2, false, false, "")]
		public String cm = null;

		public const string fn_h = "h";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal h = decimal.MinValue;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_kg = "KG";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal kg = decimal.MinValue;

		public const string fn_shippmentAndDn = "Shippment&Dn";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String shippmentAndDn = null;

		public const string fn_snoId = "SnoId";
		[DBField(SqlDbType.VarChar, 0, 15, false, false, "")]
		public String snoId = null;

		public const string fn_weight = "Weight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal weight = decimal.MinValue;
	}

	[Table("Dn_Cn_Volume")]
	public class Dn_Cn_Volume
	{
		public const string fn_a = "a";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal a = decimal.MinValue;

		public const string fn_b = "b";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal b = decimal.MinValue;

		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_dn = "Dn";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String dn = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String editor = null;

		public const string fn_h = "h";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal h = decimal.MinValue;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_pallet = "Pallet";
		[DBField(SqlDbType.VarChar, 0, 3, false, false, "")]
		public String pallet = null;

		public const string fn_po = "Po";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String po = null;

		public const string fn_qty = "Qty";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
		public Int32 qty = int.MinValue;

		public const string fn_shippment = "Shippment";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String shippment = null;

		public const string fn_snoId = "SnoId";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String snoId = null;

		public const string fn_tp = "Tp";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String tp = null;

		public const string fn_udt = "Udt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime udt = DateTime.MinValue;

		public const string fn_weight = "Weight";
		[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
		public Decimal weight = decimal.MinValue;
	}

    [Table("Pizza_Part")]//Same as the one in DB.cs
    public class Pizza_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String partType = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String pizzaID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("HP_Grade")]
    public class Hp_Grade
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_energia = "Energia";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String energia = null;

        public const string fn_espera = "Espera";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String espera = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String family = null;

        public const string fn_grade = "Grade";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String grade = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_series = "Series";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String series = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PalletStandard")]
    public class PalletStatndard
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_fullQty = "FullQty";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        [DBField(SqlDbType.NVarChar, 0, 32, false, false, "")]
        public String fullQty = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_litterQty = "LitterQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 litterQty = int.MinValue;

        public const string fn_mediumQty = "MediumQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 mediumQty = int.MinValue;

        public const string fn_tierQty = "TierQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 tierQty = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ACAdapMaintain")]
    public class Acadapmaintain
    {
        public const string fn_adppn = "ADPPN";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String adppn = null;

        public const string fn_agency = "AGENCY";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String agency = null;

        public const string fn_assemb = "ASSEMB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String assemb = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cur = "CUR";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cur = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_supplier = "SUPPLIER";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String supplier = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_voltage = "VOLTAGE";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String voltage = null;
    }

    [Table("PoPlt_EDI")]
    public class PoPlt_EDI
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_combineQty = "CombineQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 combineQty = int.MinValue;

        public const string fn_conQTY = "ConQTY";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 conQTY = int.MinValue;

        public const string fn_consolidate = "Consolidate";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String consolidate = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String plt = null;

        public const string fn_qty = "QTY";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_ucc = "UCC";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String ucc = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PoData_EDI")]
    public class PoData_EDI
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 3900, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String model = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 40, false, false, "")]
        public String poNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String shipDate = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("SnoCtrl_BoxId_SQ")]
    public class SnoCtrl_BoxId_SQ
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String boxId = null;

        public const string fn_cust = "Cust";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String cust = null;
    }

    [Table("SnoCtrl_BoxId")]
    public class SnoCtrl_BoxId
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String boxId = null;

        public const string fn_cust = "Cust";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String cust = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_valid = "valid";
        [DBField(SqlDbType.Char, 0, 5, false, false, "")]
        public String valid = null;
    }

    [Table("PAK_WHPLT_Type")]
    public class Pak_Whplt_Type
    {
        public const string fn_bol = "BOL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String bol = null;

        public const string fn_carrier = "Carrier";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String carrier = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.NVarChar, 0, 14, true, false, "")]
        public String plt = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.NChar, 0, 2, true, false, "")]
        public String tp = null;
    }

    [Table("WH_PLTLocLog")]
    public class Wh_Pltloclog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_loc = "LOC";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String loc = null;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String plt = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String remark = null;
    }

    [Table("FISTOSAP_WEIGHT")]
    public class Fistosap_Weight
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn__DNDIVShipment_ = "[DN/Shipment]";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String _DNDIVShipment_ = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_kg = "KG";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal kg = decimal.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }

    [Table("v_Bom_Code")]
    public class V_Bom_Code
    {
        public const string fn_description = "description";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String description = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_os_code = "os_code";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String os_code = null;

        public const string fn_os_desc = "os_desc";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String os_desc = null;

        public const string fn_part_number = "part_number";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String part_number = null;

        public const string fn_part_number_type = "part_number_type";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String part_number_type = null;

        public const string fn_product_family = "product_family";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String product_family = null;

        public const string fn_uio_buyer_code = "uio_buyer_code";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String uio_buyer_code = null;
    }

    [Table("Delivery")]//Same as the one in DB.cs
    public class Delivery
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 40, false, false, "")]
        public String poNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime shipDate = DateTime.MinValue;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String shipmentNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DeletedDelivery")]
    public class DeletedDelivery
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String poNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime shipDate = DateTime.MinValue;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String shipmentNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DeletedDeliveryPallet")]
    public class DeletedDeliveryPallet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_deliveryQty = "DeliveryQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 deliveryQty = short.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String shipmentNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_deviceQty = "DeviceQty";
        [DBField(SqlDbType.Int, short.MinValue, int.MaxValue, false, false, "")]
        public Int32 deviceQty = short.MinValue;

        public const string fn_palletType = "PalletType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletType = null;
    }

    [Table("TmpTable")]
    public class TmpTable
    {
        public const string fn_begSN = "BegSN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String begSN = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cust = "Cust";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cust = null;

        public const string fn_custPN = "CustPN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String custPN = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endSN = "EndSN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String endSN = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPN";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public String iecpn = null;

        public const string fn_mspn = "MSPN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String mspn = null;

        public const string fn_pc = "PC";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String pc = null;

        public const string fn_po = "PO";
        [DBField(SqlDbType.Char, 0, 16, true, false, "")]
        public String po = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime shipDate = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_upload = "Upload";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String upload = null;
    }

    [Table("COAReceive")]
    public class Coareceive
    {
        public const string fn_begSN = "BegSN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String begSN = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cust = "Cust";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cust = null;

        public const string fn_custPN = "CustPN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String custPN = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endSN = "EndSN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String endSN = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPN";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public String iecpn = null;

        public const string fn_mspn = "MSPN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String mspn = null;

        public const string fn_po = "PO";
        [DBField(SqlDbType.Char, 0, 16, true, false, "")]
        public String po = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, true, false, "")]
        public Int16 qty = short.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.SmallDateTime, Constants.SmallDateTimeMinVal, Constants.SmallDateTimeMaxVal, true, false, "")]
        public DateTime shipDate = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_upload = "Upload";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String upload = null;
    }

    [Table("PAK_PQCLog")]
    public class Pak_Pqclog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String pdLine = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String snoId = null;

        public const string fn_wc = "WC";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String wc = null;
    }

    [Table("COMSetting")]
    public class Comsetting
    {
        public const string fn_baudRate = "BaudRate";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String baudRate = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_commPort = "CommPort";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String commPort = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_handshaking = "Handshaking";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 handshaking = int.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_name = "Name";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String name = null;

        public const string fn_rthreshold = "RThreshold";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 rthreshold = int.MinValue;

        public const string fn_sthreshold = "SThreshold";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 sthreshold = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PizzaStatus")]//Same as the one in DB.cs
    public class PizzaStatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String pizzaID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAK_PizzaKittingBySt_FV")]
    public class Pak_Pizzakittingbyst_Fv
    {
        public const string fn_bin = "Bin";
        [DBField(SqlDbType.Char, 0, 3, true, false, "")]
        public String bin = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String code = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String desc = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 14, true, false, "")]
        public String model = null;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String pno = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 1000, true, false, "")]
        public String remark = null;

        public const string fn_spno = "SPno";
        [DBField(SqlDbType.NVarChar, 0, 1500, true, false, "")]
        public String spno = null;

        public const string fn_sqty = "Sqty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 sqty = int.MinValue;

        public const string fn_st = "St";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String st = null;

        public const string fn_sub = "Sub";
        [DBField(SqlDbType.NVarChar, 0, 8, true, false, "")]
        public String sub = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 4, true, false, "")]
        public String tp = null;

        public const string fn_vct = "Vct";
        [DBField(SqlDbType.NVarChar, 0, 1000, true, false, "")]
        public String vct = null;
    }

    [Table("DeliveryLog")]
    public class DeliveryLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;
    }

    [Table("TmpKit")]
    public class TmpKit
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String pdLine = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String type = null;
    }

    [Table("UnpackPizzaStatus")]
    public class UnpackPizzaStatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String pizzaID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DismantlePalletWeightLog")]
    public class DismantlePalletWeightLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal weight = decimal.MinValue;

        public const string fn_weight_L = "Weight_L";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal weight_L = decimal.MinValue;
    }

    [Table("ModelWeight")]
    public class ModelWeight
    {
        public const string fn_cartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cartonWeight = decimal.MinValue;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;

        public const string fn_sendStatus = "SendStatus";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string sendStatus = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string remark = null;
    }

    [Table("PalletWeight")]
    public class PalletWeight_NEW //It is the NEW Definition.
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletType = "PalletType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String palletType = null;

        public const string fn_palletWeight = "PalletWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal palletWeight = decimal.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("CartonInfo")]
    public class CartonInfo
    {
        public const string fn_cartonNo = "CartonNo";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String cartonNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 35, false, false, "")]
        public String infoValue = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("CartonLog")]
    public class CartonLog
    {
        public const string fn_cartonNo = "CartonNo";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String cartonNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;
    }

    [Table("CartonStatus")]
    public class CartonStatus
    {
        public const string fn_cartonNo = "CartonNo";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String cartonNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("FwdPlt")]
    public class FwdPlt
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_date = "Date";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public String date = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn__Operator = "Operator";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public String _Operator = null;

        public const string fn_pickID = "PickID";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public String pickID = null;

        public const string fn_plt = "Plt";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public String plt = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 5, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PAQCSorting")]
    public class Paqcsorting
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_previousFailTime = "PreviousFailTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime previousFailTime = DateTime.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 1000, true, false, "")]
        public String remark = null;
    }

    [Table("PAQCSorting_Product")]
    public class Paqcsorting_Product
    {
        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String custsn = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_paqcsortingid = "PAQCSortingID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 paqcsortingid = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;
    }

    [Table("COAReturn")]
    public class Coareturn
    {
        public const string fn_coasn = "COASN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public String coasn = null;

        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String custsn = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_originalStatus = "OriginalStatus";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String originalStatus = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DeliveryAttr")]
    public class DeliveryAttr
    {

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String attrName = null;

        public const string fn_attrValue = "AttrValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrValue = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;       

        //public const string fn_delivoryNo = "DelivoryNo";
        //[DBField(SqlDbType.Char, 0, 20, false, false, "")]
        //public String delivoryNo = null;     

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DeliveryAttrLog")]
    public class DeliveryAttrLog
    {
        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String attrName = null;

        public const string fn_attrNewValue = "AttrNewValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrNewValue = null;

        public const string fn_attrOldValue = "AttrOldValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrOldValue = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_shipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String shipmentNo = null;
    }

    [Table("PalletAttrLog")]
    public class PalletAttrLog
    {
        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String attrName = null;

        public const string fn_attrNewValue = "AttrNewValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrNewValue = null;

        public const string fn_attrOldValue = "AttrOldValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrOldValue = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_palletModel = "PalletModel";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletModel = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;
    }

    [Table("ECOAReturn")]
    public class Ecoareturn
    {
        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String custsn = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_ecoano = "ECOANo";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String ecoano = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_groupNo = "GroupNo";
        [DBField(SqlDbType.Char, 0, 9, false, false, "")]
        public String groupNo = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_message = "Message";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String message = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PLTStandard")]
    public class Pltstandard
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_high = "High";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal high = decimal.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_len = "Len";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal len = decimal.MinValue;

        public const string fn_pltno = "PLTNO";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pltno = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_wide = "Wide";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal wide = decimal.MinValue;
    }

    [Table("PLTSpecification")]
    public class Pltspecification
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_high = "High";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal high = decimal.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_len = "Len";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal len = decimal.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_wide = "Wide";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal wide = decimal.MinValue;
    }

    [Table("PalletLog")]
    public class PalletLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletNo = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;
    }

    [Table("UnpackPizza_Part")]
    public class UnpackPizza_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String pizzaID = null;

        public const string fn_pizza_PartID = "Pizza_PartID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 pizza_PartID = int.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }


    #endregion

    #region PCA

	[Table("Maintain_ITCNDefect_Check")]
	public class Maintain_ITCNDefect_Check
	{
		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_code = "Code";
		[DBField(SqlDbType.Char, 0, 20, true, false, "")]
		public String code = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
		public String editor = null;

		public const string fn_family = "Family";
		[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
		public String family = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_type = "Type";
		[DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
		public String type = null;
	}

	[Table("PCAICTCount")]
	public class Pcaictcount
	{
		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_pdLine = "PdLine";
		[DBField(SqlDbType.Char, 0, 10, true, false, "")]
		public String pdLine = null;

		public const string fn_qty = "Qty";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
		public Int32 qty = int.MinValue;
	}

	[Table("SnoCtrl_Off")]
	public class SnoCtrl_Off
	{
		public const string fn_begNo = "BegNo";
		[DBField(SqlDbType.Char, 0, 20, false, false, "")]
		public String begNo = null;

		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_description = "Description";
		[DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
		public String description = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.Char, 0, 25, false, false, "")]
		public String editor = null;

		public const string fn_endNo = "EndNo";
		[DBField(SqlDbType.Char, 0, 20, false, false, "")]
		public String endNo = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_partNo = "PartNo";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String partNo = null;

		public const string fn_tp = "Tp";
		[DBField(SqlDbType.Char, 0, 10, false, false, "")]
		public String tp = null;
	}

	[Table("PCBTestLog")]
	public class Pcbtestlog
	{
		public const string fn_actionName = "ActionName";
		[DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
		public String actionName = null;

		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_descr = "Descr";
		[DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
		public String descr = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
		public String editor = null;

		public const string fn_errorCode = "ErrorCode";
		[DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
		public String errorCode = null;

		public const string fn_fixtureID = "FixtureID";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String fixtureID = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;

		public const string fn_joinID = "JoinID";
		[DBField(SqlDbType.VarChar, 0, 36, true, false, "")]
		public String joinID = null;

		public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
		public String line = null;

		public const string fn_pcbno = "PCBNo";
		[DBField(SqlDbType.Char, 0, 11, false, false, "")]
		public String pcbno = null;

		public const string fn_remark = "Remark";
		[DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
		public String remark = null;

		public const string fn_station = "Station";
		[DBField(SqlDbType.Char, 0, 10, false, false, "")]
		public String station = null;

		public const string fn_status = "Status";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
		public Int32 status = int.MinValue;

		public const string fn_type = "Type";
		[DBField(SqlDbType.Char, 0, 10, false, false, "")]
		public String type = null;
	}

    [Table("EcrVersion")]//Same as the one in DB.cs
    public class EcrVersion
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_ecr = "ECR";
        [DBField(SqlDbType.Char, 0, 5, false, false, "")]
        public String ecr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecver = "IECVer";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String iecver = null;

        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String mbcode = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String remark = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MB_Test")]
    public class Mb_Test
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.Char, 0, 50, true, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String remark = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean type = default(bool);

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("FruDet")]
    public class FruDet
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 25, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_sno = "Sno";
        [DBField(SqlDbType.Char, 0, 35, false, false, "")]
        public String sno = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 14, false, false, "")]
        public String snoId = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("rpt_PCARep")]
    public class Rpt_PCARep
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mark = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.Char, 0, 400, false, false, "")]
        public String remark = null;

        public const string fn_snoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String snoId = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.Char, 0, 5, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_username = "Username";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String username = null;
    }

    [Table("v_MBCFG")]
    public class V_MBCFG
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_cfg = "CFG";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String cfg = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String mbcode = null;

        public const string fn_series = "Series";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String series = null;

        public const string fn_tp = "TP";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("v_SupplierCode")]
    public class V_SupplierCode
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_idex = "Idex";
        [DBField(SqlDbType.VarChar, 0, 2, false, false, "")]
        public String idex = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String vendor = null;
    }

    [Table("PCB_Part")]
    public class Pcb_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCBStatus")]
    public class Pcbstatus
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_testFailCount = "TestFailCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 testFailCount = int.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("ProductRepair")]
    public class ProductRepair
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_logID = "LogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 logID = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_testLogID = "TestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 testLogID = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MTA_Mark")]
    public class Mta_Mark
    {
        public const string fn_defect = "Defect";
        [DBField(SqlDbType.Char, 0, 8, true, false, "")]
        public String defect = null;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mark = null;

        public const string fn_rep_Id = "Rep_Id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 rep_Id = int.MinValue;

        public const string fn_version = "Version";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String version = null;
    }

    [Table("SMTMO")]
    public class Smtmo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_iecpartno = "IECPartNo";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public String iecpartno = null;

        public const string fn_pcbfamily = "PCBFamily";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String pcbfamily = null;

        public const string fn_printQty = "PrintQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 printQty = int.MinValue;

        public const string fn_process = "Process";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String process = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String remark = null;

        public const string fn_smtmo = "SMTMO";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public String smtmo = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCB")]
    public class Pcb
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String custsn = null;

        public const string fn_custver = "CUSTVER";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String custver = null;

        public const string fn_cvsn = "CVSN";
        [DBField(SqlDbType.Char, 0, 35, true, false, "")]
        public String cvsn = null;

        public const string fn_dateCode = "DateCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String dateCode = null;

        public const string fn_ecr = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String ecr = null;

        public const string fn_iecver = "IECVER";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String iecver = null;

        public const string fn_mac = "MAC";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String mac = null;

        public const string fn_pcbmodelid = "PCBModelID";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String pcbmodelid = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_shipMode = "ShipMode";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String shipMode = null;

        public const string fn_smtmo = "SMTMO";
        [DBField(SqlDbType.Char, 0, 8, true, false, "")]
        public String smtmo = null;

        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String state = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_uuid = "UUID";
        [DBField(SqlDbType.Char, 0, 32, true, false, "")]
        public String uuid = null;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;

        public const string fn_cartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cartonWeight = decimal.MinValue;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String pizzaID = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String palletNo = null;

        public const string fn_cartonSN = "CartonSN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String cartonSN = null;     

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String deliveryNo = null;

        public const string fn_qcStatus= "QCStatus";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String qcStatus = null;

        public const string fn_skuModel = "SkuModel";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String skuModel = null;	

    }

    [Table("PCBRepair")]
    public class Pcbrepair
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_logID = "LogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 logID = int.MinValue;

        public const string fn_pcbmodelid = "PCBModelID";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pcbmodelid = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_testLogID = "TestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 testLogID = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCBLog")]
    public class Pcblog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_pcbmodel = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pcbmodel = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;
    }

    [Table("PCBInfo")]
    public class Pcbinfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_infoType = "InfoType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String infoType = null;

        public const string fn_infoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 80, false, false, "")]
        public String infoValue = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Change_PCB")]
    public class Change_PCB
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_newPCBNo = "NewPCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String newPCBNo = null;

        public const string fn_oldPCBNo = "OldPCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String oldPCBNo = null;

        public const string fn_reason = "Reason";
        [DBField(SqlDbType.VarChar, 0, 100, false, false, "")]
        public String reason = null;
    }

    [Table("PCBRepair_DefectInfo")]
    public class Pcbrepair_Defectinfo
    {
        public const string fn__4M_ = "[4M]";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String _4M_ = null;

        public const string fn_action = "Action";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String action = null;

        public const string fn_cause = "Cause";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cause = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_component = "Component";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String component = null;

        public const string fn_cover = "Cover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cover = null;

        public const string fn_defectCode = "DefectCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String defectCode = null;

        public const string fn_distribution = "Distribution";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String distribution = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_isManual = "IsManual";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 isManual = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String location = null;

        public const string fn_majorPart = "MajorPart";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String majorPart = null;

        public const string fn_manufacture = "Manufacture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String manufacture = null;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mark = null;

        public const string fn_mtaid = "MTAID";
        [DBField(SqlDbType.VarChar, 0, 14, true, false, "")]
        public String mtaid = null;

        public const string fn_newPart = "NewPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String newPart = null;

        public const string fn_newPartDateCode = "NewPartDateCode";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public String newPartDateCode = null;

        public const string fn_newPartSno = "NewPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String newPartSno = null;

        public const string fn_obligation = "Obligation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String obligation = null;

        public const string fn_oldPart = "OldPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String oldPart = null;

        public const string fn_oldPartSno = "OldPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String oldPartSno = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partType = null;

        public const string fn_pcarepairid = "PCARepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 pcarepairid = int.MinValue;

        public const string fn_piastation = "PIAStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String piastation = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_responsibility = "Responsibility";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String responsibility = null;

        public const string fn_returnSign = "ReturnSign";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String returnSign = null;

        public const string fn_side = "Side";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String side = null;

        public const string fn_site = "Site";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String site = null;

        public const string fn_subDefect = "SubDefect";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String subDefect = null;

        public const string fn_trackingStatus = "TrackingStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String trackingStatus = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_uncover = "Uncover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String uncover = null;

        public const string fn_vendorCT = "VendorCT";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String vendorCT = null;

        public const string fn_versionA = "VersionA";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String versionA = null;

        public const string fn_versionB = "VersionB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String versionB = null;
    }

    [Table("MACRange")]
    public class Macrange
    {
        public const string fn_begNo = "BegNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String begNo = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endNo = "EndNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String endNo = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCATest_Check")]
    public class Pcatest_Check
    {
        public const string fn_bios = "BIOS";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String bios = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hddv = "HDDV";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String hddv = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mac = "MAC";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mac = null;

        public const string fn_mbct = "MBCT";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mbct = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Lot")]
    public class Lot
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String lotNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String mbcode = null;
    }

    [Table("LotSetting")]
    public class LotSetting
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkQty = "CheckQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 checkQty = int.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_failQty = "FailQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 failQty = int.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_passQty = "PassQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 passQty = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCBLot")]
    public class Pcblot
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String lotNo = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCBLotCheck")]
    public class Pcblotcheck
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String lotNo = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }

    [Table("PCBOQCRepair")]
    public class Pcboqcrepair
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public String lotNo = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 200, true, false, "")]
        public String remark = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PCBOQCRepair_DefectInfo")]
    public class Pcboqcrepair_Defectinfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_defect = "Defect";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String defect = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pcboqcrepairid = "PCBOQCRepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 pcboqcrepairid = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }

    #endregion

    #region DOA

    [Table("DOAList")]
    public class Doalist
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 mark = int.MinValue;

        public const string fn_pno = "Pno";
        [DBField(SqlDbType.VarChar, 0, 14, false, false, "")]
        public String pno = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String poNo = null;

        public const string fn_rma = "RMA";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String rma = null;

        public const string fn_sno = "Sno";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String sno = null;

        public const string fn_tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
        public String tp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PoData")]
    public class PoData
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 3900, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String model = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 40, false, false, "")]
        public String poNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String shipDate = null;

        public const string fn_shipment = "Shipment";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String shipment = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("PoPlt")]
    public class PoPlt_DOA
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_combineQty = "CombineQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 combineQty = int.MinValue;

        public const string fn_conQTY = "ConQTY";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 conQTY = int.MinValue;

        public const string fn_consolidate = "Consolidate";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String consolidate = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_plt = "PLT";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String plt = null;

        public const string fn_qty = "QTY";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_ucc = "UCC";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String ucc = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("DOAMBList")]
    public class DOAMBList
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_groupNo = "GroupNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String groupNo = null;

        public const string fn_message = "Message";
        [DBField(SqlDbType.NVarChar, 0, 100, false, false, "")]
        public String message = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }


    #endregion

    #region HP_EDI

	[Table("DN_PrintList")]
	public class Dn_Printlist
	{
		public const string fn_cdt = "Cdt";
		[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
		public DateTime cdt = DateTime.MinValue;

		public const string fn_dn = "DN";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String dn = null;

		public const string fn_doc_cat = "DOC_CAT";
		[DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
		public String doc_cat = null;

		public const string fn_editor = "Editor";
		[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
		public String editor = null;

		public const string fn_id = "ID";
		[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
		public Int32 id = int.MinValue;
	}

    [Table("m_SHIP_FROM_ADDRESS")]
    public class M_SHIP_FROM_ADDRESS
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_ship_from_city = "SHIP_FROM_CITY";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_city = null;

        public const string fn_ship_from_country_name = "SHIP_FROM_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_country_name = null;

        public const string fn_ship_from_name = "SHIP_FROM_NAME";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_name = null;

        public const string fn_ship_from_name_2 = "SHIP_FROM_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_name_2 = null;

        public const string fn_ship_from_name_3 = "SHIP_FROM_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_name_3 = null;

        public const string fn_ship_from_street = "SHIP_FROM_STREET";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_street = null;

        public const string fn_ship_from_street_2 = "SHIP_FROM_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_street_2 = null;

        public const string fn_ship_from_telephone = "SHIP_FROM_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_telephone = null;

        public const string fn_ship_from_zip = "SHIP_FROM_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_from_zip = null;

        public const string fn_ship_type = "SHIP_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String ship_type = null;
    }

    [Table("Packinglist_RePrint")]
    public class Packinglist_RePrint
    {
        public const string fn_dn = "DN";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String dn = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String shipDate = null;
    }

    [Table("PAK.PAKComn")]
    public class PakDotpakcomn
    {
        public const string fn_actual_shipdate = "ACTUAL_SHIPDATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String actual_shipdate = null;

        public const string fn_area_group_id = "AREA_GROUP_ID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String area_group_id = null;

        public const string fn_box_unit_qty = "BOX_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String box_unit_qty = null;

        public const string fn_c12 = "C12";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c12 = null;

        public const string fn_c121 = "C121";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c121 = null;

        public const string fn_c124 = "C124";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c124 = null;

        public const string fn_c135 = "C135";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c135 = null;

        public const string fn_c146 = "C146";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c146 = null;

        public const string fn_c22 = "C22";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c22 = null;

        public const string fn_c23 = "C23";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c23 = null;

        public const string fn_c24 = "C24";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c24 = null;

        public const string fn_c25 = "C25";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c25 = null;

        public const string fn_c26 = "C26";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c26 = null;

        public const string fn_c28 = "C28";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c28 = null;

        public const string fn_c29 = "C29";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c29 = null;

        public const string fn_c30 = "C30";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c30 = null;

        public const string fn_c36 = "C36";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c36 = null;

        public const string fn_c40 = "C40";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c40 = null;

        public const string fn_c43 = "C43";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c43 = null;

        public const string fn_c46 = "C46";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c46 = null;

        public const string fn_c47 = "C47";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c47 = null;

        public const string fn_c48 = "C48";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c48 = null;

        public const string fn_c49 = "C49";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c49 = null;

        public const string fn_c50 = "C50";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c50 = null;

        public const string fn_c51 = "C51";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c51 = null;

        public const string fn_c52 = "C52";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c52 = null;

        public const string fn_c53 = "C53";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c53 = null;

        public const string fn_c54 = "C54";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c54 = null;

        public const string fn_c56 = "C56";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c56 = null;

        public const string fn_c57 = "C57";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c57 = null;

        public const string fn_c6 = "C6";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c6 = null;

        public const string fn_c60 = "C60";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c60 = null;

        public const string fn_c61 = "C61";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c61 = null;

        public const string fn_c62 = "C62";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c62 = null;

        public const string fn_c63 = "C63";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c63 = null;

        public const string fn_c64 = "C64";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c64 = null;

        public const string fn_c69 = "C69";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c69 = null;

        public const string fn_c74 = "C74";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c74 = null;

        public const string fn_c93 = "C93";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c93 = null;

        public const string fn_c99 = "C99";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c99 = null;

        public const string fn_carton_qty = "CARTON_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal carton_qty = decimal.MinValue;

        public const string fn_cond_priority = "COND_PRIORITY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cond_priority = decimal.MinValue;

        public const string fn_consol_invoice = "CONSOL_INVOICE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String consol_invoice = null;

        public const string fn_customer_id = "CUSTOMER_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String customer_id = null;

        public const string fn_cust_ord_ref = "CUST_ORD_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_ord_ref = null;

        public const string fn_cust_po = "CUST_PO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_po = null;

        public const string fn_cust_so_num = "CUST_SO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_so_num = null;

        public const string fn_dest_code = "DEST_CODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String dest_code = null;

        public const string fn_doc_set_number = "DOC_SET_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String doc_set_number = null;

        public const string fn_duty_code = "DUTY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String duty_code = null;

        public const string fn_edi_distrib_channel = "EDI_DISTRIB_CHANNEL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_distrib_channel = null;

        public const string fn_edi_intl_carrier = "EDI_INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String edi_intl_carrier = null;

        public const string fn_edi_mfg_code = "EDI_MFG_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_mfg_code = null;

        public const string fn_edi_phc = "EDI_PHC";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_phc = null;

        public const string fn_edi_pl_code = "EDI_PL_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_pl_code = null;

        public const string fn_edi_trans_serv_level = "EDI_TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String edi_trans_serv_level = null;

        public const string fn_hp_cust_pn = "HP_Cust_PN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_cust_pn = null;

        public const string fn_hp_except = "HP_EXCEPT";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String hp_except = null;

        public const string fn_hp_pn = "HP_PN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_pn = null;

        public const string fn_hp_so = "HP_SO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_so = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_india_generic_desc = "INDIA_GENERIC_DESC";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String india_generic_desc = null;

        public const string fn_india_price = "INDIA_PRICE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String india_price = null;

        public const string fn_india_price_id = "INDIA_PRICE_ID";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String india_price_id = null;

        public const string fn_instr_flag = "INSTR_FLAG";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String instr_flag = null;

        public const string fn_intl_carrier = "INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String intl_carrier = null;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String internalID = null;

        public const string fn_language = "LANGUAGE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String language = null;

        public const string fn_mand_carrier_id = "MAND_CARRIER_ID";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String mand_carrier_id = null;

        public const string fn_master_waybill_number = "MASTER_WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String master_waybill_number = null;

        public const string fn_multi_line_id = "MULTI_LINE_ID";
        [DBField(SqlDbType.NVarChar, 0, 8, true, false, "")]
        public String multi_line_id = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_order_date_hp_po = "ORDER_DATE_HP_PO";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime order_date_hp_po = DateTime.MinValue;

        public const string fn_order_type = "ORDER_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 6, true, false, "")]
        public String order_type = null;

        public const string fn_packing_lv = "PACKING_LV";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String packing_lv = null;

        public const string fn_packing_type = "PACKING_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String packing_type = null;

        public const string fn_pack_id = "PACK_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id = null;

        public const string fn_pack_id_cons = "PACK_ID_CONS";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id_cons = null;

        public const string fn_pack_id_line_item_box_max_unit_qty = "PACK_ID_LINE_ITEM_BOX_MAX_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String pack_id_line_item_box_max_unit_qty = null;

        public const string fn_pack_id_line_item_unit_qty = "PACK_ID_LINE_ITEM_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_unit_qty = decimal.MinValue;

        public const string fn_pack_id_unit_qty = "PACK_ID_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String pack_id_unit_qty = null;

        public const string fn_pack_id_unit_uom = "PACK_ID_UNIT_UOM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String pack_id_unit_uom = null;

        public const string fn_pak_type = "PAK_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String pak_type = null;

        public const string fn_pallet_qty = "PALLET_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pallet_qty = decimal.MinValue;

        public const string fn_phys_consol = "PHYS_CONSOL";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String phys_consol = null;

        public const string fn_po_num = "PO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String po_num = null;

        public const string fn_pref_gateway = "PREF_GATEWAY";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String pref_gateway = null;

        public const string fn_region = "REGION";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String region = null;

        public const string fn_reg_carrier = "REG_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reg_carrier = null;

        public const string fn_reseller_name = "RESELLER_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reseller_name = null;

        public const string fn_return_to_city = "RETURN_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_city = null;

        public const string fn_return_to_contact = "RETURN_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_contact = null;

        public const string fn_return_to_country_code = "RETURN_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String return_to_country_code = null;

        public const string fn_return_to_country_name = "RETURN_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_country_name = null;

        public const string fn_return_to_id = "RETURN_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_id = null;

        public const string fn_return_to_name = "RETURN_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name = null;

        public const string fn_return_to_name_2 = "RETURN_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_2 = null;

        public const string fn_return_to_name_3 = "RETURN_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_3 = null;

        public const string fn_return_to_state = "RETURN_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String return_to_state = null;

        public const string fn_return_to_street = "RETURN_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street = null;

        public const string fn_return_to_street_2 = "RETURN_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street_2 = null;

        public const string fn_return_to_telephone = "RETURN_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_telephone = null;

        public const string fn_return_to_zip = "RETURN_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String return_to_zip = null;

        public const string fn_sales_chan = "SALES_CHAN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sales_chan = null;

        public const string fn_shipment = "SHIPMENT";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String shipment = null;

        public const string fn_ship_cat_type = "SHIP_CAT_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_cat_type = null;

        public const string fn_ship_from_city = "SHIP_FROM_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_city = null;

        public const string fn_ship_from_contact = "SHIP_FROM_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_contact = null;

        public const string fn_ship_from_country_code = "SHIP_FROM_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_from_country_code = null;

        public const string fn_ship_from_country_name = "SHIP_FROM_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_country_name = null;

        public const string fn_ship_from_id = "SHIP_FROM_ID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_id = null;

        public const string fn_ship_from_name = "SHIP_FROM_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name = null;

        public const string fn_ship_from_name_2 = "SHIP_FROM_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_2 = null;

        public const string fn_ship_from_name_3 = "SHIP_FROM_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_3 = null;

        public const string fn_ship_from_state = "SHIP_FROM_STATE";
        [DBField(SqlDbType.NVarChar, 0, 2, true, false, "")]
        public String ship_from_state = null;

        public const string fn_ship_from_street = "SHIP_FROM_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_from_street = null;

        public const string fn_ship_from_street_2 = "SHIP_FROM_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_from_street_2 = null;

        public const string fn_ship_from_telephone = "SHIP_FROM_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_telephone = null;

        public const string fn_ship_from_zip = "SHIP_FROM_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_from_zip = null;

        public const string fn_ship_mode = "SHIP_MODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String ship_mode = null;

        public const string fn_ship_ref = "SHIP_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_ref = null;

        public const string fn_ship_to_city = "SHIP_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_to_city = null;

        public const string fn_ship_to_contact = "SHIP_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_contact = null;

        public const string fn_ship_to_country_code = "SHIP_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_to_country_code = null;

        public const string fn_ship_to_country_name = "SHIP_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_country_name = null;

        public const string fn_ship_to_id = "SHIP_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_id = null;

        public const string fn_ship_to_name = "SHIP_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name = null;

        public const string fn_ship_to_name_2 = "SHIP_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_2 = null;

        public const string fn_ship_to_name_3 = "SHIP_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_3 = null;

        public const string fn_ship_to_state = "SHIP_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_to_state = null;

        public const string fn_ship_to_street = "SHIP_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street = null;

        public const string fn_ship_to_street_2 = "SHIP_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street_2 = null;

        public const string fn_ship_to_telephone = "SHIP_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_telephone = null;

        public const string fn_ship_to_zip = "SHIP_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_to_zip = null;

        public const string fn_ship_via_city = "SHIP_VIA_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_via_city = null;

        public const string fn_ship_via_contact = "SHIP_VIA_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_contact = null;

        public const string fn_ship_via_country_code = "SHIP_VIA_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_via_country_code = null;

        public const string fn_ship_via_country_name = "SHIP_VIA_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_country_name = null;

        public const string fn_ship_via_id = "SHIP_VIA_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_id = null;

        public const string fn_ship_via_name = "SHIP_VIA_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name = null;

        public const string fn_ship_via_name_2 = "SHIP_VIA_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_2 = null;

        public const string fn_ship_via_name_3 = "SHIP_VIA_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_3 = null;

        public const string fn_ship_via_state = "SHIP_VIA_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_via_state = null;

        public const string fn_ship_via_street = "SHIP_VIA_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street = null;

        public const string fn_ship_via_street_2 = "SHIP_VIA_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street_2 = null;

        public const string fn_ship_via_telephone = "SHIP_VIA_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_telephone = null;

        public const string fn_ship_via_zip = "SHIP_VIA_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_via_zip = null;

        public const string fn_sold_to_contact = "SOLD_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String sold_to_contact = null;

        public const string fn_sub_region = "SUB_REGION";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sub_region = null;

        public const string fn_trans_serv_level = "TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String trans_serv_level = null;

        public const string fn_ucc_code = "UCC_CODE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ucc_code = null;

        public const string fn_waybill_number = "WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String waybill_number = null;

        public const string fn_sold_to_city = "SOLD_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_city = null;

        public const string fn_sold_to_country_name = "SOLD_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_country_name = null;

        public const string fn_sold_to_name2 = "SOLD_TO_NAME2";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_name2 = null;

        public const string fn_sold_to_name3 = "SOLD_TO_NAME3";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_name3 = null;

        public const string fn_sold_to_state = "SOLD_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_state = null;

        public const string fn_sold_to_street = "SOLD_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 35, true, false, "")]
        public String sold_to_street = null;

        public const string fn_sold_to_street2 = "SOLD_TO_STREET2";
        [DBField(SqlDbType.NVarChar, 0, 35, true, false, "")]
        public String sold_to_street2 = null;

        public const string fn_sold_to_zip = "SOLD_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_zip = null;

        public const string fn_container_id = "CONTAINER_ID";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String container_id = null;

        public const string fn_sold_to_name = "SOLD_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String sold_to_name = null;

        public const string fn_incoterm = "INCOTERM";
        [DBField(SqlDbType.NVarChar, 0, 64, true, false, "")]
        public String incoterm = null;

        public const string fn_logID = "LogID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, false, "")]
        public Int64 logID = 0;


    }

    [Table("PAK.PAKEdi850raw")]
    public class PakDotpakedi850raw
    {
        public const string fn_carrier_inst_head = "CARRIER_INST_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String carrier_inst_head = null;

        public const string fn_component_qty = "COMPONENT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String component_qty = null;

        public const string fn_config_instr_detail = "CONFIG_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String config_instr_detail = null;

        public const string fn_customer_instr_detail = "CUSTOMER_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String customer_instr_detail = null;

        public const string fn_customer_instr_head = "CUSTOMER_INSTR_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String customer_instr_head = null;

        public const string fn_delivery_instr_detail = "DELIVERY_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String delivery_instr_detail = null;

        public const string fn_delivery_instr_head = "DELIVERY_INSTR_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String delivery_instr_head = null;

        public const string fn_department_code = "DEPARTMENT_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String department_code = null;

        public const string fn_e2 = "E2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String e2 = null;

        public const string fn_e39 = "E39";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String e39 = null;

        public const string fn_edi_lang_code = "EDI_LANG_CODE";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String edi_lang_code = null;

        public const string fn_export_notes_detail = "EXPORT_NOTES_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String export_notes_detail = null;

        public const string fn_export_notes_head = "EXPORT_NOTES_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String export_notes_head = null;

        public const string fn_handling_instr_detail = "HANDLING_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String handling_instr_detail = null;

        public const string fn_hp_pn_component = "HP_PN_COMPONENT";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_pn_component = null;

        public const string fn_hp_so_line_item = "HP_SO_LINE_ITEM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String hp_so_line_item = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_invoice_udf_head = "INVOICE_UDF_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String invoice_udf_head = null;

        public const string fn_label_instr_head = "LABEL_INSTR_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String label_instr_head = null;

        public const string fn_line_item = "LINE_ITEM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String line_item = null;

        public const string fn_maxkit_detail = "MAXKIT_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String maxkit_detail = null;

        public const string fn_picking_instr_detail = "PICKING_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String picking_instr_detail = null;

        public const string fn_picking_instr_head = "PICKING_INSTR_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String picking_instr_head = null;

        public const string fn_po_num = "PO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String po_num = null;

        public const string fn_prod_desc_base = "PROD_DESC_BASE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String prod_desc_base = null;

        public const string fn_prod_desc_comp = "PROD_DESC_COMP";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String prod_desc_comp = null;

        public const string fn_ship_complete_id = "SHIP_COMPLETE_ID";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String ship_complete_id = null;

        public const string fn_shipping_instr_detail = "SHIPPING_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String shipping_instr_detail = null;

        public const string fn_shipping_instr_head = "SHIPPING_INSTR_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String shipping_instr_head = null;

        public const string fn_special_instr_detail = "SPECIAL_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String special_instr_detail = null;

        public const string fn_special_instr_head = "SPECIAL_INSTR_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String special_instr_head = null;

        public const string fn_udf_detail = "UDF_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String udf_detail = null;

        public const string fn_udf_head = "UDF_HEAD";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String udf_head = null;

        public const string fn_udf_key_detail = "UDF_KEY_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String udf_key_detail = null;

        public const string fn_udf_key_header = "UDF_KEY_HEADER";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String udf_key_header = null;

        public const string fn_udf_value_detail = "UDF_VALUE_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String udf_value_detail = null;

        public const string fn_udf_value_header = "UDF_VALUE_HEADER";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String udf_value_header = null;

        public const string fn_uid_instr_detail = "UID_INSTR_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String uid_instr_detail = null;

        public const string fn_viSTA_ITEM_NUM = "ViSTA_ITEM_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String viSTA_ITEM_NUM = null;

        public const string fn_warehouse_code = "WAREHOUSE_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String warehouse_code = null;
    }

    [Table("PAK.PAKPaltno")]
    public class PakDotpakpaltno
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String internalID = null;

        public const string fn_p4 = "P4";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String p4 = null;

        public const string fn_p5 = "P5";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String p5 = null;

        public const string fn_p6 = "P6";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String p6 = null;

        public const string fn_pallet_box_qty = "PALLET_BOX_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pallet_box_qty = decimal.MinValue;

        public const string fn_pallet_id = "PALLET_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pallet_id = null;

        public const string fn_pallet_unit_qty = "PALLET_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String pallet_unit_qty = null;
    }
    
    [Table("PAK.PAKRT")]
    public class PakDotpakrt
    {
        public const string fn_doc_cat = "DOC_CAT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String doc_cat = null;

        public const string fn_doc_set_number = "DOC_SET_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String doc_set_number = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_image_files = "IMAGE_FILES";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String image_files = null;

        public const string fn_schema_file_name = "SCHEMA_FILE_NAME";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String schema_file_name = null;

        public const string fn_string_id_flag = "STRING_ID_FLAG";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String string_id_flag = null;

        public const string fn_xsl_template_name = "XSL_TEMPLATE_NAME";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String xsl_template_name = null;
    }

    [Table("PAK_PackkingData_Del")]
    public class Pak_Packkingdata_Del
    {
        public const string fn_actual_shipdate = "ACTUAL_SHIPDATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String actual_shipdate = null;

        public const string fn_box_id = "BOX_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String box_id = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_delDt = "DelDt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime delDt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String internalID = null;

        public const string fn_pallet_id = "PALLET_ID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String pallet_id = null;

        public const string fn_prod_type = "PROD_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String prod_type = null;

        public const string fn_serial_num = "SERIAL_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String serial_num = null;

        public const string fn_track_no_parcel = "TRACK_NO_PARCEL";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String track_no_parcel = null;
    }

    [Table("PAK_PAKComn")]
    public class Pak_Pakcomn
    {
        public const string fn_actual_shipdate = "ACTUAL_SHIPDATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String actual_shipdate = null;

        public const string fn_area_group_id = "AREA_GROUP_ID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String area_group_id = null;

        public const string fn_box_unit_qty = "BOX_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String box_unit_qty = null;

        public const string fn_c12 = "C12";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c12 = null;

        public const string fn_c121 = "C121";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c121 = null;

        public const string fn_c124 = "C124";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c124 = null;

        public const string fn_c135 = "C135";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String c135 = null;

        public const string fn_c146 = "C146";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c146 = null;

        public const string fn_c22 = "C22";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c22 = null;

        public const string fn_c23 = "C23";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c23 = null;

        public const string fn_c24 = "C24";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c24 = null;

        public const string fn_c25 = "C25";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c25 = null;

        public const string fn_c26 = "C26";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c26 = null;

        public const string fn_c28 = "C28";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c28 = null;

        public const string fn_c29 = "C29";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c29 = null;

        public const string fn_c30 = "C30";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c30 = null;

        public const string fn_c36 = "C36";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c36 = null;

        public const string fn_c40 = "C40";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c40 = null;

        public const string fn_c43 = "C43";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c43 = null;

        public const string fn_c46 = "C46";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c46 = null;

        public const string fn_c47 = "C47";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c47 = null;

        public const string fn_c48 = "C48";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c48 = null;

        public const string fn_c49 = "C49";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c49 = null;

        public const string fn_c50 = "C50";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c50 = null;

        public const string fn_c51 = "C51";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c51 = null;

        public const string fn_c52 = "C52";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c52 = null;

        public const string fn_c53 = "C53";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c53 = null;

        public const string fn_c54 = "C54";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c54 = null;

        public const string fn_c56 = "C56";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c56 = null;

        public const string fn_c57 = "C57";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c57 = null;

        public const string fn_c6 = "C6";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c6 = null;

        public const string fn_c60 = "C60";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c60 = null;

        public const string fn_c61 = "C61";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c61 = null;

        public const string fn_c62 = "C62";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c62 = null;

        public const string fn_c63 = "C63";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c63 = null;

        public const string fn_c64 = "C64";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c64 = null;

        public const string fn_c69 = "C69";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c69 = null;

        public const string fn_c74 = "C74";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c74 = null;

        public const string fn_c93 = "C93";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c93 = null;

        public const string fn_c99 = "C99";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c99 = null;

        public const string fn_carton_qty = "CARTON_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal carton_qty = decimal.MinValue;

        public const string fn_cond_priority = "COND_PRIORITY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cond_priority = decimal.MinValue;

        public const string fn_consol_invoice = "CONSOL_INVOICE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String consol_invoice = null;

        public const string fn_customer_id = "CUSTOMER_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String customer_id = null;

        public const string fn_cust_ord_ref = "CUST_ORD_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_ord_ref = null;

        public const string fn_cust_po = "CUST_PO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_po = null;

        public const string fn_cust_so_num = "CUST_SO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_so_num = null;

        public const string fn_dest_code = "DEST_CODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String dest_code = null;

        public const string fn_doc_set_number = "DOC_SET_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String doc_set_number = null;

        public const string fn_duty_code = "DUTY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String duty_code = null;

        public const string fn_edi_distrib_channel = "EDI_DISTRIB_CHANNEL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_distrib_channel = null;

        public const string fn_edi_intl_carrier = "EDI_INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String edi_intl_carrier = null;

        public const string fn_edi_mfg_code = "EDI_MFG_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_mfg_code = null;

        public const string fn_edi_phc = "EDI_PHC";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_phc = null;

        public const string fn_edi_pl_code = "EDI_PL_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_pl_code = null;

        public const string fn_edi_trans_serv_level = "EDI_TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String edi_trans_serv_level = null;

        public const string fn_hp_cust_pn = "HP_Cust_PN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_cust_pn = null;

        public const string fn_hp_except = "HP_EXCEPT";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String hp_except = null;

        public const string fn_hp_pn = "HP_PN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_pn = null;

        public const string fn_hp_so = "HP_SO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_so = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_india_generic_desc = "INDIA_GENERIC_DESC";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String india_generic_desc = null;

        public const string fn_india_price = "INDIA_PRICE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String india_price = null;

        public const string fn_india_price_id = "INDIA_PRICE_ID";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String india_price_id = null;

        public const string fn_instr_flag = "INSTR_FLAG";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String instr_flag = null;

        public const string fn_intl_carrier = "INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String intl_carrier = null;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String internalID = null;

        public const string fn_language = "LANGUAGE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String language = null;

        public const string fn_mand_carrier_id = "MAND_CARRIER_ID";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String mand_carrier_id = null;

        public const string fn_master_waybill_number = "MASTER_WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String master_waybill_number = null;

        public const string fn_multi_line_id = "MULTI_LINE_ID";
        [DBField(SqlDbType.NVarChar, 0, 8, true, false, "")]
        public String multi_line_id = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_order_date_hp_po = "ORDER_DATE_HP_PO";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime order_date_hp_po = DateTime.MinValue;

        public const string fn_order_type = "ORDER_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 6, true, false, "")]
        public String order_type = null;

        public const string fn_packing_lv = "PACKING_LV";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String packing_lv = null;

        public const string fn_packing_type = "PACKING_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String packing_type = null;

        public const string fn_pack_id = "PACK_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id = null;

        public const string fn_pack_id_cons = "PACK_ID_CONS";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id_cons = null;

        public const string fn_pack_id_line_item_box_max_unit_qty = "PACK_ID_LINE_ITEM_BOX_MAX_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String pack_id_line_item_box_max_unit_qty = null;

        public const string fn_pack_id_line_item_unit_qty = "PACK_ID_LINE_ITEM_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_unit_qty = decimal.MinValue;

        public const string fn_pack_id_unit_qty = "PACK_ID_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String pack_id_unit_qty = null;

        public const string fn_pack_id_unit_uom = "PACK_ID_UNIT_UOM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String pack_id_unit_uom = null;

        public const string fn_pak_type = "PAK_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String pak_type = null;

        public const string fn_pallet_qty = "PALLET_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pallet_qty = decimal.MinValue;

        public const string fn_phys_consol = "PHYS_CONSOL";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String phys_consol = null;

        public const string fn_po_num = "PO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String po_num = null;

        public const string fn_pref_gateway = "PREF_GATEWAY";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String pref_gateway = null;

        public const string fn_region = "REGION";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String region = null;

        public const string fn_reg_carrier = "REG_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reg_carrier = null;

        public const string fn_reseller_name = "RESELLER_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reseller_name = null;

        public const string fn_return_to_city = "RETURN_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_city = null;

        public const string fn_return_to_contact = "RETURN_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_contact = null;

        public const string fn_return_to_country_code = "RETURN_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String return_to_country_code = null;

        public const string fn_return_to_country_name = "RETURN_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_country_name = null;

        public const string fn_return_to_id = "RETURN_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_id = null;

        public const string fn_return_to_name = "RETURN_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name = null;

        public const string fn_return_to_name_2 = "RETURN_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_2 = null;

        public const string fn_return_to_name_3 = "RETURN_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_3 = null;

        public const string fn_return_to_state = "RETURN_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String return_to_state = null;

        public const string fn_return_to_street = "RETURN_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street = null;

        public const string fn_return_to_street_2 = "RETURN_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street_2 = null;

        public const string fn_return_to_telephone = "RETURN_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_telephone = null;

        public const string fn_return_to_zip = "RETURN_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String return_to_zip = null;

        public const string fn_sales_chan = "SALES_CHAN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sales_chan = null;

        public const string fn_shipment = "SHIPMENT";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String shipment = null;

        public const string fn_ship_cat_type = "SHIP_CAT_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_cat_type = null;

        public const string fn_ship_from_city = "SHIP_FROM_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_city = null;

        public const string fn_ship_from_contact = "SHIP_FROM_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_contact = null;

        public const string fn_ship_from_country_code = "SHIP_FROM_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_from_country_code = null;

        public const string fn_ship_from_country_name = "SHIP_FROM_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_country_name = null;

        public const string fn_ship_from_id = "SHIP_FROM_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_id = null;

        public const string fn_ship_from_name = "SHIP_FROM_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name = null;

        public const string fn_ship_from_name_2 = "SHIP_FROM_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_2 = null;

        public const string fn_ship_from_name_3 = "SHIP_FROM_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_3 = null;

        public const string fn_ship_from_state = "SHIP_FROM_STATE";
        [DBField(SqlDbType.NVarChar, 0, 2, true, false, "")]
        public String ship_from_state = null;

        public const string fn_ship_from_street = "SHIP_FROM_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_from_street = null;

        public const string fn_ship_from_street_2 = "SHIP_FROM_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_from_street_2 = null;

        public const string fn_ship_from_telephone = "SHIP_FROM_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_from_telephone = null;

        public const string fn_ship_from_zip = "SHIP_FROM_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_from_zip = null;

        public const string fn_ship_mode = "SHIP_MODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String ship_mode = null;

        public const string fn_ship_ref = "SHIP_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_ref = null;

        public const string fn_ship_to_city = "SHIP_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_to_city = null;

        public const string fn_ship_to_contact = "SHIP_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_contact = null;

        public const string fn_ship_to_country_code = "SHIP_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_to_country_code = null;

        public const string fn_ship_to_country_name = "SHIP_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_country_name = null;

        public const string fn_ship_to_id = "SHIP_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_id = null;

        public const string fn_ship_to_name = "SHIP_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name = null;

        public const string fn_ship_to_name_2 = "SHIP_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_2 = null;

        public const string fn_ship_to_name_3 = "SHIP_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_3 = null;

        public const string fn_ship_to_state = "SHIP_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_to_state = null;

        public const string fn_ship_to_street = "SHIP_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street = null;

        public const string fn_ship_to_street_2 = "SHIP_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street_2 = null;

        public const string fn_ship_to_telephone = "SHIP_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_telephone = null;

        public const string fn_ship_to_zip = "SHIP_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_to_zip = null;

        public const string fn_ship_via_city = "SHIP_VIA_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_via_city = null;

        public const string fn_ship_via_contact = "SHIP_VIA_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_contact = null;

        public const string fn_ship_via_country_code = "SHIP_VIA_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_via_country_code = null;

        public const string fn_ship_via_country_name = "SHIP_VIA_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_country_name = null;

        public const string fn_ship_via_id = "SHIP_VIA_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_id = null;

        public const string fn_ship_via_name = "SHIP_VIA_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name = null;

        public const string fn_ship_via_name_2 = "SHIP_VIA_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_2 = null;

        public const string fn_ship_via_name_3 = "SHIP_VIA_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_3 = null;

        public const string fn_ship_via_state = "SHIP_VIA_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_via_state = null;

        public const string fn_ship_via_street = "SHIP_VIA_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street = null;

        public const string fn_ship_via_street_2 = "SHIP_VIA_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street_2 = null;

        public const string fn_ship_via_telephone = "SHIP_VIA_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_telephone = null;

        public const string fn_ship_via_zip = "SHIP_VIA_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_via_zip = null;

        public const string fn_sold_to_contact = "SOLD_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String sold_to_contact = null;

        public const string fn_sub_region = "SUB_REGION";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sub_region = null;

        public const string fn_trans_serv_level = "TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String trans_serv_level = null;

        public const string fn_ucc_code = "UCC_CODE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ucc_code = null;

        public const string fn_waybill_number = "WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String waybill_number = null;

        public const string fn_sold_to_city = "SOLD_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_city = null;

        public const string fn_sold_to_country_name = "SOLD_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_country_name = null;

        public const string fn_sold_to_name2 = "SOLD_TO_NAME2";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_name2 = null;

        public const string fn_sold_to_name3 = "SOLD_TO_NAME3";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_name3 = null;

        public const string fn_sold_to_state = "SOLD_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_state = null;

        public const string fn_sold_to_street = "SOLD_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 35, true, false, "")]
        public String sold_to_street = null;

        public const string fn_sold_to_street2 = "SOLD_TO_STREET2";
        [DBField(SqlDbType.NVarChar, 0, 35, true, false, "")]
        public String sold_to_street2 = null;

        public const string fn_sold_to_zip = "SOLD_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sold_to_zip = null;

        public const string fn_container_id = "CONTAINER_ID";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String container_id = null;

        public const string fn_sold_to_name = "SOLD_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String sold_to_name = null;

        public const string fn_incoterm = "INCOTERM";
        [DBField(SqlDbType.NVarChar, 0, 64, true, false, "")]
        public String incoterm = null;
    }

    [Table("PAK_ShipmentWeight_FIS")]
    public class Pak_Shipmentweight_Fis
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_shipment = "Shipment";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String shipment = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.NVarChar, 0, 2, true, false, "")]
        public String type = null;

        public const string fn_weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal weight = decimal.MinValue;
    }

    [Table("PAK_SkuMasterWeight_FIS")]
    public class Pak_Skumasterweight_Fis
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String model = null;

        public const string fn_weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal weight = decimal.MinValue;
    }

    [Table("PAKEDI_INSTR")]
    public class Pakedi_Instr
    {
        public const string fn_fields = "FIELDS";
        [DBField(SqlDbType.Char, 0, 50, true, false, "")]
        public String fields = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_po_item = "PO_ITEM";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String po_item = null;

        public const string fn_po_num = "PO_NUM";
        [DBField(SqlDbType.Char, 0, 50, true, false, "")]
        public String po_num = null;

        public const string fn_value = "VALUE";
        [DBField(SqlDbType.Char, 0, 100, true, false, "")]
        public String value = null;
    }

    [Table("PAKODMSESSION")]
    public class Pakodmsession
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_serial_num = "SERIAL_NUM";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String serial_num = null;

        public const string fn_show_order = "SHOW_ORDER";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 show_order = int.MinValue;

        public const string fn_udf_key_detail = "UDF_KEY_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String udf_key_detail = null;

        public const string fn_udf_key_header = "UDF_KEY_HEADER";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String udf_key_header = null;

        public const string fn_udf_value_detail = "UDF_VALUE_DETAIL";
        [DBField(SqlDbType.NVarChar, 0, 500, true, false, "")]
        public String udf_value_detail = null;

        public const string fn_udf_value_header = "UDF_VALUE_HEADER";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String udf_value_header = null;
    }

    [Table("PAK_PackkingData")]
    public class Pak_Packkingdata
    {
        public const string fn_actual_shipdate = "ACTUAL_SHIPDATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String actual_shipdate = null;

        public const string fn_box_id = "BOX_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String box_id = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String internalID = null;

        public const string fn_pallet_id = "PALLET_ID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String pallet_id = null;

        public const string fn_prod_type = "PROD_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String prod_type = null;

        public const string fn_serial_num = "SERIAL_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String serial_num = null;

        public const string fn_track_no_parcel = "TRACK_NO_PARCEL";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String track_no_parcel = null;
    }

    [Table("MEID_Log")]
    public class Meid_Log
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_enCoder = "EnCoder";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String enCoder = null;

        public const string fn_enCoding = "EnCoding";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String enCoding = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_isPass = "IsPass";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, true, false, "")]
        public Int16 isPass = short.MinValue;

        public const string fn_pallet_id = "PALLET_ID";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String pallet_id = null;

        public const string fn_stringIDKey = "StringIDKey";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String stringIDKey = null;

        public const string fn_stringIDValue = "StringIDValue";
        [DBField(SqlDbType.NVarChar, 0, 0, true, false, "")]
        public String stringIDValue = null;
    }

    [Table("v_PAKComn")]
    public class V_PAKComn
    {
        public const string fn_actual_shipdate = "ACTUAL_SHIPDATE";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String actual_shipdate = null;

        public const string fn_area_group_id = "AREA_GROUP_ID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String area_group_id = null;

        public const string fn_box_sequence = "BOX_SEQUENCE";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 box_sequence = int.MinValue;

        public const string fn_box_unit_qty = "BOX_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String box_unit_qty = null;

        public const string fn_c12 = "C12";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c12 = null;

        public const string fn_c121 = "C121";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c121 = null;

        public const string fn_c124 = "C124";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c124 = null;

        public const string fn_c135 = "C135";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c135 = null;

        public const string fn_c146 = "C146";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c146 = null;

        public const string fn_c22 = "C22";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c22 = null;

        public const string fn_c23 = "C23";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c23 = null;

        public const string fn_c24 = "C24";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c24 = null;

        public const string fn_c25 = "C25";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c25 = null;

        public const string fn_c26 = "C26";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c26 = null;

        public const string fn_c28 = "C28";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c28 = null;

        public const string fn_c29 = "C29";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c29 = null;

        public const string fn_c30 = "C30";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c30 = null;

        public const string fn_c36 = "C36";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c36 = null;

        public const string fn_c40 = "C40";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c40 = null;

        public const string fn_c43 = "C43";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c43 = null;

        public const string fn_c46 = "C46";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c46 = null;

        public const string fn_c47 = "C47";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c47 = null;

        public const string fn_c49 = "C49";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c49 = null;

        public const string fn_c50 = "C50";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c50 = null;

        public const string fn_c51 = "C51";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c51 = null;

        public const string fn_c52 = "C52";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c52 = null;

        public const string fn_c53 = "C53";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c53 = null;

        public const string fn_c54 = "C54";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c54 = null;

        public const string fn_c56 = "C56";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c56 = null;

        public const string fn_c6 = "C6";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c6 = null;

        public const string fn_c60 = "C60";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c60 = null;

        public const string fn_c61 = "C61";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c61 = null;

        public const string fn_c62 = "C62";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c62 = null;

        public const string fn_c63 = "C63";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c63 = null;

        public const string fn_c64 = "C64";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c64 = null;

        public const string fn_c69 = "C69";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c69 = null;

        public const string fn_c74 = "C74";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c74 = null;

        public const string fn_c93 = "C93";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c93 = null;

        public const string fn_c99 = "C99";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c99 = null;

        public const string fn_cond_priority = "COND_PRIORITY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cond_priority = decimal.MinValue;

        public const string fn_config_id_number = "CONFIG_ID_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String config_id_number = null;

        public const string fn_consol_invoice = "CONSOL_INVOICE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String consol_invoice = null;

        public const string fn_customer_id = "CUSTOMER_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String customer_id = null;

        public const string fn_cust_ord_ref = "CUST_ORD_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_ord_ref = null;

        public const string fn_cust_po = "CUST_PO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_po = null;

        public const string fn_cust_so_num = "CUST_SO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_so_num = null;

        public const string fn_dest_code = "DEST_CODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String dest_code = null;

        public const string fn_doc_set_number = "DOC_SET_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String doc_set_number = null;

        public const string fn_duty_code = "DUTY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String duty_code = null;

        public const string fn_edi_distrib_channel = "EDI_DISTRIB_CHANNEL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_distrib_channel = null;

        public const string fn_edi_intl_carrier = "EDI_INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String edi_intl_carrier = null;

        public const string fn_edi_mfg_code = "EDI_MFG_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_mfg_code = null;

        public const string fn_edi_phc = "EDI_PHC";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_phc = null;

        public const string fn_edi_pl_code = "EDI_PL_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_pl_code = null;

        public const string fn_edi_trans_serv_level = "EDI_TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String edi_trans_serv_level = null;

        public const string fn_hp_cust_pn = "HP_Cust_PN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_cust_pn = null;

        public const string fn_hp_except = "HP_EXCEPT";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String hp_except = null;

        public const string fn_hp_pn = "HP_PN";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String hp_pn = null;

        public const string fn_hp_so = "HP_SO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_so = null;

        public const string fn_hp_so_line_item = "HP_SO_LINE_ITEM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String hp_so_line_item = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 id = int.MinValue;

        public const string fn_instr_flag = "INSTR_FLAG";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String instr_flag = null;

        public const string fn_intl_carrier = "INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String intl_carrier = null;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String internalID = null;

        public const string fn_language = "LANGUAGE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String language = null;

        public const string fn_mand_carrier_id = "MAND_CARRIER_ID";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String mand_carrier_id = null;

        public const string fn_master_waybill_number = "MASTER_WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String master_waybill_number = null;

        public const string fn_multi_line_id = "MULTI_LINE_ID";
        [DBField(SqlDbType.NVarChar, 0, 8, true, false, "")]
        public String multi_line_id = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_order_date_hp_po = "ORDER_DATE_HP_PO";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String order_date_hp_po = null;

        public const string fn_order_type = "ORDER_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 6, true, false, "")]
        public String order_type = null;

        public const string fn_pack_id = "PACK_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id = null;

        public const string fn_pack_id_box_qty = "PACK_ID_BOX_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_box_qty = decimal.MinValue;

        public const string fn_pack_id_cons = "PACK_ID_CONS";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id_cons = null;

        public const string fn_pack_id_line_item_box_max_unit_qty = "PACK_ID_LINE_ITEM_BOX_MAX_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_box_max_unit_qty = decimal.MinValue;

        public const string fn_pack_id_line_item_box_qty = "PACK_ID_LINE_ITEM_BOX_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_box_qty = decimal.MinValue;

        public const string fn_pack_id_line_item_unit_qty = "PACK_ID_LINE_ITEM_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_unit_qty = decimal.MinValue;

        public const string fn_pack_id_unit_qty = "PACK_ID_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_unit_qty = decimal.MinValue;

        public const string fn_pack_id_unit_uom = "PACK_ID_UNIT_UOM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String pack_id_unit_uom = null;

        public const string fn_pak_type = "PAK_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String pak_type = null;

        public const string fn_phys_consol = "PHYS_CONSOL";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String phys_consol = null;

        public const string fn_po_num = "PO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String po_num = null;

        public const string fn_pref_gateway = "PREF_GATEWAY";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String pref_gateway = null;

        public const string fn_prod_desc_base = "PROD_DESC_BASE";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String prod_desc_base = null;

        public const string fn_prod_type = "PROD_TYPE";
        [DBField(SqlDbType.VarChar, 0, 17, false, false, "")]
        public String prod_type = null;

        public const string fn_region = "REGION";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String region = null;

        public const string fn_reg_carrier = "REG_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reg_carrier = null;

        public const string fn_reseller_name = "RESELLER_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reseller_name = null;

        public const string fn_return_to_city = "RETURN_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_city = null;

        public const string fn_return_to_contact = "RETURN_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_contact = null;

        public const string fn_return_to_country_code = "RETURN_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String return_to_country_code = null;

        public const string fn_return_to_country_name = "RETURN_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_country_name = null;

        public const string fn_return_to_id = "RETURN_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_id = null;

        public const string fn_return_to_name = "RETURN_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name = null;

        public const string fn_return_to_name_2 = "RETURN_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_2 = null;

        public const string fn_return_to_name_3 = "RETURN_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_3 = null;

        public const string fn_return_to_state = "RETURN_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String return_to_state = null;

        public const string fn_return_to_street = "RETURN_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street = null;

        public const string fn_return_to_street_2 = "RETURN_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street_2 = null;

        public const string fn_return_to_telephone = "RETURN_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_telephone = null;

        public const string fn_return_to_zip = "RETURN_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String return_to_zip = null;

        public const string fn_sales_chan = "SALES_CHAN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sales_chan = null;

        public const string fn_shipment = "SHIPMENT";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String shipment = null;

        public const string fn_ship_cat_type = "SHIP_CAT_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_cat_type = null;

        public const string fn_ship_from_city = "SHIP_FROM_CITY";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_city = null;

        public const string fn_ship_from_contact = "SHIP_FROM_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_contact = null;

        public const string fn_ship_from_country_code = "SHIP_FROM_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_country_code = null;

        public const string fn_ship_from_country_name = "SHIP_FROM_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_country_name = null;

        public const string fn_ship_from_id = "SHIP_FROM_ID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_id = null;

        public const string fn_ship_from_name = "SHIP_FROM_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name = null;

        public const string fn_ship_from_name_2 = "SHIP_FROM_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_2 = null;

        public const string fn_ship_from_name_3 = "SHIP_FROM_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_3 = null;

        public const string fn_ship_from_state = "SHIP_FROM_STATE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_state = null;

        public const string fn_ship_from_street = "SHIP_FROM_STREET";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_street = null;

        public const string fn_ship_from_street_2 = "SHIP_FROM_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_street_2 = null;

        public const string fn_ship_from_telephone = "SHIP_FROM_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_telephone = null;

        public const string fn_ship_from_zip = "SHIP_FROM_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_zip = null;

        public const string fn_ship_mode = "SHIP_MODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String ship_mode = null;

        public const string fn_ship_ref = "SHIP_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_ref = null;

        public const string fn_ship_to_city = "SHIP_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_to_city = null;

        public const string fn_ship_to_contact = "SHIP_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_contact = null;

        public const string fn_ship_to_country_code = "SHIP_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_to_country_code = null;

        public const string fn_ship_to_country_name = "SHIP_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_country_name = null;

        public const string fn_ship_to_id = "SHIP_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_id = null;

        public const string fn_ship_to_name = "SHIP_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name = null;

        public const string fn_ship_to_name_2 = "SHIP_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_2 = null;

        public const string fn_ship_to_name_3 = "SHIP_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_3 = null;

        public const string fn_ship_to_state = "SHIP_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_to_state = null;

        public const string fn_ship_to_street = "SHIP_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street = null;

        public const string fn_ship_to_street_2 = "SHIP_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street_2 = null;

        public const string fn_ship_to_telephone = "SHIP_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_telephone = null;

        public const string fn_ship_to_zip = "SHIP_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_to_zip = null;

        public const string fn_ship_via_city = "SHIP_VIA_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_via_city = null;

        public const string fn_ship_via_contact = "SHIP_VIA_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_contact = null;

        public const string fn_ship_via_country_code = "SHIP_VIA_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_via_country_code = null;

        public const string fn_ship_via_country_name = "SHIP_VIA_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_country_name = null;

        public const string fn_ship_via_id = "SHIP_VIA_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_id = null;

        public const string fn_ship_via_name = "SHIP_VIA_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name = null;

        public const string fn_ship_via_name_2 = "SHIP_VIA_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_2 = null;

        public const string fn_ship_via_name_3 = "SHIP_VIA_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_3 = null;

        public const string fn_ship_via_state = "SHIP_VIA_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_via_state = null;

        public const string fn_ship_via_street = "SHIP_VIA_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street = null;

        public const string fn_ship_via_street_2 = "SHIP_VIA_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street_2 = null;

        public const string fn_ship_via_telephone = "SHIP_VIA_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_telephone = null;

        public const string fn_ship_via_zip = "SHIP_VIA_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_via_zip = null;

        public const string fn_sold_to_contact = "SOLD_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String sold_to_contact = null;

        public const string fn_sub_region = "SUB_REGION";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sub_region = null;

        public const string fn_trans_serv_level = "TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String trans_serv_level = null;

        public const string fn_warehouse_code = "WAREHOUSE_CODE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String warehouse_code = null;

        public const string fn_waybill_number = "WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String waybill_number = null;
    }

    [Table("v_Shipment_PAKComn")]
    public class V_Shipment_PAKComn
    {
        public const string fn_actual_shipdate = "ACTUAL_SHIPDATE";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String actual_shipdate = null;

        public const string fn_area_group_id = "AREA_GROUP_ID";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String area_group_id = null;

        public const string fn_box_sequence = "BOX_SEQUENCE";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 box_sequence = int.MinValue;

        public const string fn_box_unit_qty = "BOX_UNIT_QTY";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String box_unit_qty = null;

        public const string fn_c12 = "C12";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c12 = null;

        public const string fn_c121 = "C121";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c121 = null;

        public const string fn_c124 = "C124";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c124 = null;

        public const string fn_c135 = "C135";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String c135 = null;

        public const string fn_c146 = "C146";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c146 = null;

        public const string fn_c22 = "C22";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c22 = null;

        public const string fn_c23 = "C23";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c23 = null;

        public const string fn_c24 = "C24";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c24 = null;

        public const string fn_c25 = "C25";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c25 = null;

        public const string fn_c26 = "C26";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c26 = null;

        public const string fn_c28 = "C28";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c28 = null;

        public const string fn_c29 = "C29";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c29 = null;

        public const string fn_c30 = "C30";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c30 = null;

        public const string fn_c36 = "C36";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c36 = null;

        public const string fn_c40 = "C40";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c40 = null;

        public const string fn_c43 = "C43";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c43 = null;

        public const string fn_c46 = "C46";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c46 = null;

        public const string fn_c47 = "C47";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c47 = null;

        public const string fn_c49 = "C49";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c49 = null;

        public const string fn_c50 = "C50";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c50 = null;

        public const string fn_c51 = "C51";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c51 = null;

        public const string fn_c52 = "C52";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c52 = null;

        public const string fn_c53 = "C53";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c53 = null;

        public const string fn_c54 = "C54";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c54 = null;

        public const string fn_c56 = "C56";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c56 = null;

        public const string fn_c6 = "C6";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c6 = null;

        public const string fn_c60 = "C60";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c60 = null;

        public const string fn_c61 = "C61";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c61 = null;

        public const string fn_c62 = "C62";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c62 = null;

        public const string fn_c63 = "C63";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c63 = null;

        public const string fn_c64 = "C64";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c64 = null;

        public const string fn_c69 = "C69";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c69 = null;

        public const string fn_c74 = "C74";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c74 = null;

        public const string fn_c93 = "C93";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c93 = null;

        public const string fn_c99 = "C99";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String c99 = null;

        public const string fn_carton_qty = "CARTON_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal carton_qty = decimal.MinValue;

        public const string fn_cond_priority = "COND_PRIORITY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cond_priority = decimal.MinValue;

        public const string fn_config_id_number = "CONFIG_ID_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String config_id_number = null;

        public const string fn_consol_invoice = "CONSOL_INVOICE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String consol_invoice = null;

        public const string fn_customer_id = "CUSTOMER_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String customer_id = null;

        public const string fn_cust_ord_ref = "CUST_ORD_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_ord_ref = null;

        public const string fn_cust_po = "CUST_PO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_po = null;

        public const string fn_cust_so_num = "CUST_SO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String cust_so_num = null;

        public const string fn_dest_code = "DEST_CODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String dest_code = null;

        public const string fn_doc_set_number = "DOC_SET_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String doc_set_number = null;

        public const string fn_duty_code = "DUTY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String duty_code = null;

        public const string fn_edi_distrib_channel = "EDI_DISTRIB_CHANNEL";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_distrib_channel = null;

        public const string fn_edi_intl_carrier = "EDI_INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String edi_intl_carrier = null;

        public const string fn_edi_mfg_code = "EDI_MFG_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_mfg_code = null;

        public const string fn_edi_phc = "EDI_PHC";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_phc = null;

        public const string fn_edi_pl_code = "EDI_PL_CODE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String edi_pl_code = null;

        public const string fn_edi_trans_serv_level = "EDI_TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String edi_trans_serv_level = null;

        public const string fn_hp_cust_pn = "HP_Cust_PN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_cust_pn = null;

        public const string fn_hp_except = "HP_EXCEPT";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String hp_except = null;

        public const string fn_hp_pn = "HP_PN";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String hp_pn = null;

        public const string fn_hp_so = "HP_SO";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String hp_so = null;

        public const string fn_hp_so_line_item = "HP_SO_LINE_ITEM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String hp_so_line_item = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 id = int.MinValue;

        public const string fn_instr_flag = "INSTR_FLAG";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String instr_flag = null;

        public const string fn_intl_carrier = "INTL_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String intl_carrier = null;

        public const string fn_internalID = "InternalID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String internalID = null;

        public const string fn_language = "LANGUAGE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String language = null;

        public const string fn_mand_carrier_id = "MAND_CARRIER_ID";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String mand_carrier_id = null;

        public const string fn_master_waybill_number = "MASTER_WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String master_waybill_number = null;

        public const string fn_multi_line_id = "MULTI_LINE_ID";
        [DBField(SqlDbType.NVarChar, 0, 8, true, false, "")]
        public String multi_line_id = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_order_date_hp_po = "ORDER_DATE_HP_PO";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String order_date_hp_po = null;

        public const string fn_order_type = "ORDER_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 6, true, false, "")]
        public String order_type = null;

        public const string fn_packing_lv = "PACKING_LV";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String packing_lv = null;

        public const string fn_packing_type = "PACKING_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String packing_type = null;

        public const string fn_pack_id = "PACK_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id = null;

        public const string fn_pack_id_box_qty = "PACK_ID_BOX_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_box_qty = decimal.MinValue;

        public const string fn_pack_id_cons = "PACK_ID_CONS";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String pack_id_cons = null;

        public const string fn_pack_id_line_item_box_max_unit_qty = "PACK_ID_LINE_ITEM_BOX_MAX_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_box_max_unit_qty = decimal.MinValue;

        public const string fn_pack_id_line_item_box_qty = "PACK_ID_LINE_ITEM_BOX_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_box_qty = decimal.MinValue;

        public const string fn_pack_id_line_item_unit_qty = "PACK_ID_LINE_ITEM_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_line_item_unit_qty = decimal.MinValue;

        public const string fn_pack_id_unit_qty = "PACK_ID_UNIT_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pack_id_unit_qty = decimal.MinValue;

        public const string fn_pack_id_unit_uom = "PACK_ID_UNIT_UOM";
        [DBField(SqlDbType.NVarChar, 0, 10, true, false, "")]
        public String pack_id_unit_uom = null;

        public const string fn_pak_type = "PAK_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String pak_type = null;

        public const string fn_pallet_qty = "PALLET_QTY";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal pallet_qty = decimal.MinValue;

        public const string fn_phys_consol = "PHYS_CONSOL";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String phys_consol = null;

        public const string fn_po_num = "PO_NUM";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String po_num = null;

        public const string fn_pref_gateway = "PREF_GATEWAY";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String pref_gateway = null;

        public const string fn_prod_desc_base = "PROD_DESC_BASE";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String prod_desc_base = null;

        public const string fn_prod_type = "PROD_TYPE";
        [DBField(SqlDbType.VarChar, 0, 17, false, false, "")]
        public String prod_type = null;

        public const string fn_region = "REGION";
        [DBField(SqlDbType.NVarChar, 0, 4, true, false, "")]
        public String region = null;

        public const string fn_reg_carrier = "REG_CARRIER";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reg_carrier = null;

        public const string fn_reseller_name = "RESELLER_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String reseller_name = null;

        public const string fn_return_to_city = "RETURN_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_city = null;

        public const string fn_return_to_contact = "RETURN_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_contact = null;

        public const string fn_return_to_country_code = "RETURN_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String return_to_country_code = null;

        public const string fn_return_to_country_name = "RETURN_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_country_name = null;

        public const string fn_return_to_id = "RETURN_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_id = null;

        public const string fn_return_to_name = "RETURN_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name = null;

        public const string fn_return_to_name_2 = "RETURN_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_2 = null;

        public const string fn_return_to_name_3 = "RETURN_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String return_to_name_3 = null;

        public const string fn_return_to_state = "RETURN_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String return_to_state = null;

        public const string fn_return_to_street = "RETURN_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street = null;

        public const string fn_return_to_street_2 = "RETURN_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String return_to_street_2 = null;

        public const string fn_return_to_telephone = "RETURN_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String return_to_telephone = null;

        public const string fn_return_to_zip = "RETURN_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String return_to_zip = null;

        public const string fn_sales_chan = "SALES_CHAN";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sales_chan = null;

        public const string fn_shipment = "SHIPMENT";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public String shipment = null;

        public const string fn_ship_cat_type = "SHIP_CAT_TYPE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_cat_type = null;

        public const string fn_ship_from_city = "SHIP_FROM_CITY";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_city = null;

        public const string fn_ship_from_contact = "SHIP_FROM_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_contact = null;

        public const string fn_ship_from_country_code = "SHIP_FROM_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_country_code = null;

        public const string fn_ship_from_country_name = "SHIP_FROM_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_country_name = null;

        public const string fn_ship_from_id = "SHIP_FROM_ID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_id = null;

        public const string fn_ship_from_name = "SHIP_FROM_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name = null;

        public const string fn_ship_from_name_2 = "SHIP_FROM_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_2 = null;

        public const string fn_ship_from_name_3 = "SHIP_FROM_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_name_3 = null;

        public const string fn_ship_from_state = "SHIP_FROM_STATE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_state = null;

        public const string fn_ship_from_street = "SHIP_FROM_STREET";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_street = null;

        public const string fn_ship_from_street_2 = "SHIP_FROM_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_street_2 = null;

        public const string fn_ship_from_telephone = "SHIP_FROM_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_telephone = null;

        public const string fn_ship_from_zip = "SHIP_FROM_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_from_zip = null;

        public const string fn_ship_mode = "SHIP_MODE";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String ship_mode = null;

        public const string fn_ship_ref = "SHIP_REF";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_ref = null;

        public const string fn_ship_to_city = "SHIP_TO_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_to_city = null;

        public const string fn_ship_to_contact = "SHIP_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_contact = null;

        public const string fn_ship_to_country_code = "SHIP_TO_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_to_country_code = null;

        public const string fn_ship_to_country_name = "SHIP_TO_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_country_name = null;

        public const string fn_ship_to_id = "SHIP_TO_ID";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_id = null;

        public const string fn_ship_to_name = "SHIP_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name = null;

        public const string fn_ship_to_name_2 = "SHIP_TO_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_2 = null;

        public const string fn_ship_to_name_3 = "SHIP_TO_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_to_name_3 = null;

        public const string fn_ship_to_state = "SHIP_TO_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_to_state = null;

        public const string fn_ship_to_street = "SHIP_TO_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street = null;

        public const string fn_ship_to_street_2 = "SHIP_TO_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_to_street_2 = null;

        public const string fn_ship_to_telephone = "SHIP_TO_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_to_telephone = null;

        public const string fn_ship_to_zip = "SHIP_TO_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_to_zip = null;

        public const string fn_ship_via_city = "SHIP_VIA_CITY";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String ship_via_city = null;

        public const string fn_ship_via_contact = "SHIP_VIA_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_contact = null;

        public const string fn_ship_via_country_code = "SHIP_VIA_COUNTRY_CODE";
        [DBField(SqlDbType.NVarChar, 0, 3, true, false, "")]
        public String ship_via_country_code = null;

        public const string fn_ship_via_country_name = "SHIP_VIA_COUNTRY_NAME";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_country_name = null;

        public const string fn_ship_via_id = "SHIP_VIA_ID";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_id = null;

        public const string fn_ship_via_name = "SHIP_VIA_NAME";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name = null;

        public const string fn_ship_via_name_2 = "SHIP_VIA_NAME_2";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_2 = null;

        public const string fn_ship_via_name_3 = "SHIP_VIA_NAME_3";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String ship_via_name_3 = null;

        public const string fn_ship_via_state = "SHIP_VIA_STATE";
        [DBField(SqlDbType.NVarChar, 0, 5, true, false, "")]
        public String ship_via_state = null;

        public const string fn_ship_via_street = "SHIP_VIA_STREET";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street = null;

        public const string fn_ship_via_street_2 = "SHIP_VIA_STREET_2";
        [DBField(SqlDbType.NVarChar, 0, 55, true, false, "")]
        public String ship_via_street_2 = null;

        public const string fn_ship_via_telephone = "SHIP_VIA_TELEPHONE";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String ship_via_telephone = null;

        public const string fn_ship_via_zip = "SHIP_VIA_ZIP";
        [DBField(SqlDbType.NVarChar, 0, 15, true, false, "")]
        public String ship_via_zip = null;

        public const string fn_sold_to_contact = "SOLD_TO_CONTACT";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String sold_to_contact = null;

        public const string fn_sub_region = "SUB_REGION";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String sub_region = null;

        public const string fn_trans_serv_level = "TRANS_SERV_LEVEL";
        [DBField(SqlDbType.NVarChar, 0, 12, true, false, "")]
        public String trans_serv_level = null;

        public const string fn_warehouse_code = "WAREHOUSE_CODE";
        [DBField(SqlDbType.NVarChar, 0, 60, true, false, "")]
        public String warehouse_code = null;

        public const string fn_waybill_number = "WAYBILL_NUMBER";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String waybill_number = null;

        public const string fn_container_id = "CONTAINER_ID";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String container_id = null;

        public const string fn_sold_to_name = "SOLD_TO_NAME";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String sold_to_name = null;
    }

    [Table("PrintList")]
    public class PrintList
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_doc_Name = "Doc_Name";
        [DBField(SqlDbType.NVarChar, 0, 150, true, false, "")]
        public String doc_Name = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;


    }

    #endregion

    #region KIT

    [Table("Kitting_Loc_PLMapping_St")]
    public class Kitting_Loc_PLMapping_St
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lightNo = "LightNo";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 lightNo = short.MinValue;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public String pdLine = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.NVarChar, 0, 20, false, false, "")]
        public String station = null;

        public const string fn_tagID = "TagID";
        [DBField(SqlDbType.NChar, 0, 12, false, false, "")]
        public String tagID = null;
    }

    [Table("Kitting_Location_FV")]
    public class Kitting_Location_FV
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_comm = "Comm";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean comm = default(bool);

        public const string fn_configedDate = "ConfigedDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime configedDate = DateTime.MinValue;

        public const string fn_configedLEDB_Lock = "ConfigedLEDBlock";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 configedLEDB_Lock = short.MinValue;

        public const string fn_configedLEDStatus = "ConfigedLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean configedLEDStatus = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_gateWayIP = "GateWayIP";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 gateWayIP = short.MinValue;

        public const string fn_gateWayPort = "GateWayPort";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 gateWayPort = int.MinValue;

        public const string fn_ledvalues = "LEDValues";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String ledvalues = null;

        public const string fn_mgroup = "MGroup";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 mgroup = int.MinValue;

        public const string fn_rackID = "RackID";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 rackID = short.MinValue;

        public const string fn_runningDate = "RunningDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime runningDate = DateTime.MinValue;

        public const string fn_runningLEDB_Lock = "RunningLEDBlock";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 runningLEDB_Lock = short.MinValue;

        public const string fn_runningLEDStatus = "RunningLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean runningLEDStatus = default(bool);

        public const string fn_tagDescr = "TagDescr";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String tagDescr = null;

        public const string fn_tagID = "TagID";
        [DBField(SqlDbType.NChar, 0, 12, false, false, "")]
        public String tagID = null;

        public const string fn_tagTp = "TagTp";
        [DBField(SqlDbType.NChar, 0, 4, false, false, "")]
        public String tagTp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Kitting_Location_FA_A")]
    public class Kitting_Location_FA_A
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_comm = "Comm";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean comm = default(bool);

        public const string fn_configedDate = "ConfigedDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime configedDate = DateTime.MinValue;

        public const string fn_configedLEDB_Lock = "ConfigedLEDBlock";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 configedLEDB_Lock = short.MinValue;

        public const string fn_configedLEDStatus = "ConfigedLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean configedLEDStatus = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_gateWayIP = "GateWayIP";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 gateWayIP = short.MinValue;

        public const string fn_gateWayPort = "GateWayPort";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 gateWayPort = int.MinValue;

        public const string fn_group = "Group";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 group = int.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_ledvalues = "LEDValues";
        [DBField(SqlDbType.NChar, 0, 10, true, false, "")]
        public String ledvalues = null;

        public const string fn_mgroup = "MGroup";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 mgroup = int.MinValue;

        public const string fn_priority = "Priority";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 priority = int.MinValue;

        public const string fn_rackID = "RackID";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 rackID = short.MinValue;

        public const string fn_runningDate = "RunningDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime runningDate = DateTime.MinValue;

        public const string fn_runningLEDB_Lock = "RunningLEDBlock";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public Int16 runningLEDB_Lock = short.MinValue;

        public const string fn_runningLEDStatus = "RunningLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean runningLEDStatus = default(bool);

        public const string fn_tagDescr = "TagDescr";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public String tagDescr = null;

        public const string fn_tagID = "TagID";
        [DBField(SqlDbType.NChar, 0, 12, false, false, "")]
        public String tagID = null;

        public const string fn_tagTp = "TagTp";
        [DBField(SqlDbType.NChar, 0, 4, false, false, "")]
        public String tagTp = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    #endregion

    #region "RCTO"

    [Table("RCTOMBMaintain")]
    public class Rctombmaintain
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String type = null;

        public const string fn_udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("FA_ITCNDefect_Check")]
    public class Fa_Itcndefect_Check
    {
        public const string fn_bios = "BIOS";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String bios = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String code = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hddv = "HDDV";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String hddv = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mac = "MAC";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String mac = null;

        public const string fn_mbct = "MBCT";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String mbct = null;

        public const string fn_udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Family_MB")]
    public class Family_MB
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_mb = "MB";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String mb = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("SMTLine")]
    public class Smtline    //PCA
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_endTime = "EndTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime endTime = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_obTime = "ObTime";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal obTime = decimal.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_startTime = "StartTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime startTime = DateTime.MinValue;

        public const string fn_udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("SMTCT")]
    public class Smtct
    {
        public const string fn_ct = "CT";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal ct = decimal.MinValue;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_optRate = "OptRate";
        [DBField(SqlDbType.Float, double.MinValue, double.MaxValue, false, false, "")]
        public Double optRate = double.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("SMTTime")]
    public class Smttime
    {
        public const string fn_actTime = "ActTime";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public Decimal actTime = decimal.MinValue;

        public const string fn_actTime1 = "ActTime1";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal actTime1 = decimal.MinValue;

        public const string fn_actTime2 = "ActTime2";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal actTime2 = decimal.MinValue;

        public const string fn_actTime3 = "ActTime3";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal actTime3 = decimal.MinValue;

        public const string fn_cause = "Cause";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String cause = null;

        public const string fn_cause1 = "Cause1";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String cause1 = null;

        public const string fn_cause2 = "Cause2";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String cause2 = null;

        public const string fn_cause3 = "Cause3";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String cause3 = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_date = "Date";
        [DBField(SqlDbType.SmallDateTime, Constants.SmallDateTimeMinVal, Constants.SmallDateTimeMaxVal, false, false, "")]
        public DateTime date = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("BorrowLog2")]
    public class BorrowLog2
    {
        public const string fn_accepter = "Accepter";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String accepter = null;

        public const string fn_bqty = "BQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 bqty = int.MinValue;

        public const string fn_bdate = "Bdate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime bdate = DateTime.MinValue;

        public const string fn_borrower = "Borrower";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String borrower = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lender = "Lender";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String lender = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_rqty = "RQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 rqty = int.MinValue;

        public const string fn_rdate = "Rdate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime rdate = DateTime.MinValue;

        public const string fn_returner = "Returner";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public String returner = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }

    [Table("KeyPartRepair")]
    public class KeyPartRepair
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_logID = "LogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 logID = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_testLogID = "TestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 testLogID = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("KeyPartRepair_DefectInfo")]
    public class KeyPartRepair_DefectInfo
    {
        public const string fn__4M_ = "[4M]";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String _4M_ = null;

        public const string fn_action = "Action";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String action = null;

        public const string fn_cause = "Cause";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cause = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_component = "Component";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String component = null;

        public const string fn_cover = "Cover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String cover = null;

        public const string fn_defectCode = "DefectCode";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String defectCode = null;

        public const string fn_distribution = "Distribution";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String distribution = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_isManual = "IsManual";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 isManual = int.MinValue;

        public const string fn_keyPartRepairID = "KeyPartRepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 keyPartRepairID = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String location = null;

        public const string fn_mtaid = "MTAID";
        [DBField(SqlDbType.VarChar, 0, 14, true, false, "")]
        public String mtaid = null;

        public const string fn_majorPart = "MajorPart";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String majorPart = null;

        public const string fn_manufacture = "Manufacture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String manufacture = null;

        public const string fn_mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String mark = null;

        public const string fn_newPart = "NewPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String newPart = null;

        public const string fn_newPartSno = "NewPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String newPartSno = null;

        public const string fn_obligation = "Obligation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String obligation = null;

        public const string fn_oldPart = "OldPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String oldPart = null;

        public const string fn_oldPartSno = "OldPartSno";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String oldPartSno = null;

        public const string fn_piastation = "PIAStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String piastation = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String partType = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_responsibility = "Responsibility";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String responsibility = null;

        public const string fn_returnSign = "ReturnSign";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String returnSign = null;

        public const string fn_returnStn = "ReturnStn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String returnStn = null;

        public const string fn_site = "Site";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String site = null;

        public const string fn_subDefect = "SubDefect";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String subDefect = null;

        public const string fn_trackingStatus = "TrackingStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String trackingStatus = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_uncover = "Uncover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String uncover = null;

        public const string fn_vendorCT = "VendorCT";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String vendorCT = null;

        public const string fn_versionA = "VersionA";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String versionA = null;

        public const string fn_versionB = "VersionB";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String versionB = null;
    }

    [Table("FRUMBVer")]//Same as the one in DB.cs
    public class FruMBVer
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;      

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public String partNo = null;      
               
        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public String mbcode = null;

        public const string fn_ver = "Ver";
        [DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
        public String ver = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    #endregion

    #region "Alarm"

    [Table("Alarm")]
    public class Alarm
    {
        public const string fn_action = "Action";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String action = null;

        public const string fn_actionPIC = "ActionPIC";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String actionPIC = null;

        public const string fn_actionTime = "ActionTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime actionTime = DateTime.MinValue;

        public const string fn_alarmSettingID = "AlarmSettingID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 alarmSettingID = int.MinValue;

        public const string fn_cause = "Cause";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String cause = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_defect = "Defect";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String defect = null;

        public const string fn_endTime = "EndTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime endTime = DateTime.MinValue;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_holdLine = "HoldLine";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String holdLine = null;

        public const string fn_holdModel = "HoldModel";
        [DBField(SqlDbType.VarChar, 0, 511, true, false, "")]
        public String holdModel = null;

        public const string fn_holdStation = "HoldStation";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String holdStation = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String line = null;

        public const string fn_reason = "Reason";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String reason = null;

        public const string fn_reasonCode = "ReasonCode";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String reasonCode = null;

        public const string fn_releasePIC = "ReleasePIC";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String releasePIC = null;

        public const string fn_releaseTime = "ReleaseTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime releaseTime = DateTime.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 1021, true, false, "")]
        public String remark = null;

        public const string fn_skipHoldPIC = "SkipHoldPIC";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String skipHoldPIC = null;

        public const string fn_skipHoldTime = "SkipHoldTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime skipHoldTime = DateTime.MinValue;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String stage = null;

        public const string fn_startTime = "StartTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime startTime = DateTime.MinValue;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String status = null;
    }

    [Table("AlarmSetting")]
    public class AlarmSetting
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_defectQty = "DefectQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 defectQty = int.MinValue;

        public const string fn_defectType = "DefectType";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String defectType = null;

        public const string fn_defects = "Defects";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String defects = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lifeCycle = "LifeCycle";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public Boolean lifeCycle = default(bool);

        public const string fn_minQty = "MinQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 minQty = int.MinValue;

        public const string fn_period = "Period";
        [DBField(SqlDbType.Float, double.MinValue, double.MaxValue, true, false, "")]
        public Double period = double.MinValue;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String stage = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_yieldRate = "YieldRate";
        [DBField(SqlDbType.Float, double.MinValue, double.MaxValue, true, false, "")]
        public Double yieldRate = double.MinValue;
    }

    [Table("PCBTestLogBack")]
    public class Pcbtestlogback
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String type = null;
    }

    [Table("PCBTestLogBack_DefectInfo")]
    public class Pcbtestlogback_Defectinfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_defectCodeID = "DefectCodeID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String defectCodeID = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_pcbtestlogbackid = "PCBTestLogBackID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 pcbtestlogbackid = int.MinValue;

        public const string fn_triggerAlarm = "TriggerAlarm";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean triggerAlarm = default(bool);
    }

    [Table("ProductTestLogBack")]
    public class ProductTestLogBack
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public String line = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String station = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 status = int.MinValue;

        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String type = null;
    }

    [Table("ProductTestLogBack_DefectInfo")]
    public class ProductTestLogBack_DefectInfo
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_defectCodeID = "DefectCodeID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String defectCodeID = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_productTestLogBackID = "ProductTestLogBackID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 productTestLogBackID = int.MinValue;

        public const string fn_triggerAlarm = "TriggerAlarm";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean triggerAlarm = default(bool);
    }

    #endregion


    #region Assembly Vendor Code/Material/MaterialAttr/MaterialAttrLog/MaterialBox/MaterialBoxAttr/MatterialBoxAttrLog
    [Table("AssemblyVC")]
    public class AssemblyVC
    {

        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, Int64.MinValue, Int64.MaxValue, false, true, "")]
        public Int64 id = Int64.MinValue;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String family = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String partNo = null;       

        public const string fn_vc = "VC";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String vc = null;

        public const string fn_combineVC = "CombineVC";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String combineVC = null;

        public const string fn_combinePartNo = "CombinePartNo";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String combinePartNo = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String remark = null;       

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;
        
        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("Material")]//Same as the one in DB.cs
    public class Material
    {
        public const string fn_materialCT = "MaterialCT";
        [DBField(SqlDbType.VarChar, 0, 32, false, true, "")]
        public String materialCT = null;

        public const string fn_materialType= "MaterialType";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String materialType = null;

        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.Char, 0, 16, false, false, "")]
        public String lotNo = null;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String stage = null;        
       
        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String line = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;

        public const string fn_preStatus = "PreStatus";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String preStatus = null;     

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_cartonSN = "CartonSN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String cartonSN = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String pizzaID = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String deliveryNo = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String palletNo = null;

        public const string fn_qcStatus = "QcStatus";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String qcStatus = null;

        public const string fn_cartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cartonWeight = decimal.MinValue;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;

        public const string fn_shipMode = "ShipMode";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String shipMode = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

    }

    [Table("MaterialLog")]//Same as the one in DB.cs
    public class MaterialLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_materialCT = "MaterialCT";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String materialCT = null;

        public const string fn_action = "Action";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String action = null;       

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String stage = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String line = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;

        public const string fn_preStatus = "PreStatus";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String preStatus = null;       

        public const string fn_comment = "Comment";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String comment = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

    }

    [Table("MaterialAttr")]
    public class MaterialAttr
    {
        public const string fn_materialCT = "MaterialCT";
        [DBField(SqlDbType.VarChar, 0, 32, false, true, "")]
        public String materialCT = null;

        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, true, "")]
        public String attrName = null;

        public const string fn_attrValue = "AttrValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrValue = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;     

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MaterialAttrLog")]
    public class MaterialAttrLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_materialCT = "MaterialCT";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String materialCT = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;

        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String attrName = null;

        public const string fn_attrNewValue = "AttrNewValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrNewValue = null;

        public const string fn_attrOldValue = "AttrOldValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrOldValue = null;
       
        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue; 
    }

    [Table("MaterialBox")]//Same as the one in DB.cs
    public class MaterialBox
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public String boxId = null;

        public const string fn_materialType = "MaterialType";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String materialType = null;

        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.Char, 0, 16, false, false, "")]
        public String lotNo = null;

        public const string fn_specNo = "SpecNo";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String specNo = null;

        public const string fn_feedType= "FeedType";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String feedType = null;

        public const string fn_revision = "Revision";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String revision = null;

        public const string fn_dateCode = "DateCode";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String dateCode = null;

        public const string fn_supplier = "Supplier";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String supplier = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String partNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;

        public const string fn_comment = "Comment";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String comment = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String line = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String model = null;

        public const string fn_cartonSN = "CartonSN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String cartonSN = null;

        //public const string fn_pizzaID = "PizzaID";
        //[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        //public String pizzaID = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String deliveryNo = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String palletNo = null;

        public const string fn_qcStatus = "QcStatus";
        [DBField(SqlDbType.VarChar, 0, 8, true, false,"")]
        public String qcStatus = null;

        public const string fn_boxWeight = "BoxWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal boxWeight = decimal.MinValue;

        //public const string fn_unitWeight = "UnitWeight";
        //[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        //public Decimal unitWeight = decimal.MinValue;

        public const string fn_shipMode = "ShipMode";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String shipMode = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

    }

    [Table("MaterialBoxAttr")]
    public class MaterialBoxAttr
    {
        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public String boxId = null;

        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, true, "")]
        public String attrName = null;

        public const string fn_attrValue = "AttrValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrValue = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("MaterialBoxAttrLog")]
    public class MaterialBoxAttrLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_boxId = "BoxId";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String boxId = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;

        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String attrName = null;

        public const string fn_attrNewValue = "AttrNewValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrNewValue = null;

        public const string fn_attrOldValue = "AttrOldValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String attrOldValue = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;
    }

    [Table("MaterialLot")]//Same as the one in DB.cs
    public class MaterialLot
    {
        public const string fn_lotNo = "LotNo";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public String lotNo = null;

        public const string fn_materialType = "MaterialType";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public String materialType = null;
       
        public const string fn_specNo = "SpecNo";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String specNo = null;

        
        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;        

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

    }

    [Table("HoldRule")]
    public class HoldRule
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String line = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String family = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0,32, false, false, "")]
        public String model = null;

        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String custsn = null;

        public const string fn_holdStation = "HoldStation";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String holdStation = null;

        public const string fn_checkInStation = "CheckInStation";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String checkInStation = null;

        public const string fn_holdCode = "HoldCode";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String holdCode = null;

        public const string fn_holdDescr = "HoldDescr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String holdDescr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

    }

    [Table("DefectHoldRule")]
    public class DefectHoldRule
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_checkInStation = "CheckInStation";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String checkInStation = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String line = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String family = null;

        public const string fn_defectCode = "DefectCode";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String defectCode = null;

        public const string fn_equalSameDefectCount = "EqualSameDefectCount";
        [DBField(SqlDbType.Int, 0, 4, false, false, "")]
        public Nullable<int> equalSameDefectCount =null;

        public const string fn_overDefectCount = "OverDefectCount";
        [DBField(SqlDbType.Int, 0, 4, false, false, "")]
        public Nullable<int> overDefectCount =null;

        public const string fn_exceptCause = "ExceptCause";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String exceptCause = null;     

        public const string fn_holdStation = "HoldStation";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String holdStation = null;        

        public const string fn_holdCode = "HoldCode";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String holdCode = null;

        public const string fn_holdDescr = "HoldDescr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String holdDescr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

    }
    #endregion

    #region EngeryLabel & IndonesiaLabel
    [Table("EnergyLabel")]
    public class EnergyLabel
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String family = null;

        public const string fn_seriesName = "SeriesName";
        [DBField(SqlDbType.NVarChar, 0, 64, false, false, "")]
        public String seriesName = null;       

        public const string fn_level = "Level";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String level = null;

        public const string fn_chinaLevel = "ChinaLevel";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String chinaLevel = null;

        public const string fn_saveEnergy = "SaveEnergy";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String saveEnergy = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("IndonesiaLabel")]
    public class IndonesiaLabel
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;       

        public const string fn_sku = "SKU";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String sku= null;        

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String descr = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_approvalNo = "ApprovalNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String approvalNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
   #endregion

    #region HPEDI
    [Table("UploadPOLog")]
    public class UploadPOLog
    {     

        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_textFileRecordCount = "TextFileRecordCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 textFileRecordCount = int.MinValue;

        public const string fn_uploadOKRecordCount = "UploadOKRecordCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 uploadOKRecordCount = int.MinValue;

        public const string fn_uploadNGDeliveryNo = "UploadNGDeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 512, false, false, "")]
        public String uploadNGDeliveryNo = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;       
       
    }
    #endregion

    #region PilotMo
    [Table("PilotMo")]
    public class PilotMo
    {
        public const string fn_mo = "Mo";
        [DBField(SqlDbType.VarChar, 0, 32, false, true, "")]
        public String mo = null;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String stage = null;

        public const string fn_moType = "MoType";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String moType = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String model = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String partNo = null;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.NVarChar, 0, 32, false, false, "")]
        public String vendor = null;

        public const string fn_causeDescr = "CauseDescr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String causeDescr = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int qty = int.MinValue;

        public const string fn_planStartTime = "PlanStartTime";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime planStartTime = DateTime.MinValue;

        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String state = null;

        public const string fn_combinedQty = "CombinedQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int combinedQty = int.MinValue;

        public const string fn_combinedState = "CombinedState";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String combinedState = null;

        public const string fn_refID = "RefID";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String refID = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    #endregion

    #region  PartForbidRule table
    [Table("PartForbidRule")]
    public class PartForbidRule
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String customer = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String line = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String family = null;

        public const string fn_category = "Category";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String category = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string status = null;

        public const string fn_exceptModel = "ExceptModel";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String exceptModel = null;

        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String bomNodeType = null;

        public const string fn_vendorCode = "VendorCode";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String vendorCode = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String partNo = null;

        public const string fn_noticeMsg = "NoticeMsg";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String noticeMsg = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String remark = null;

        public const string fn_refID = "RefID";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public String refID = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

    }
    #endregion

    #region CombinedAstNumber table
    [Table("CombinedAstNumber")]
    public class CombinedAstNumber
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String code = null;

        public const string fn_astType = "AstType";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public String astType = null;

        public const string fn_astNo = "AstNo";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String astNo = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string station = null;

        public const string fn_unBindProductID = "UnBindProductID";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String unBindProductID = null;

        public const string fn_unBindStation = "UnBindStation";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string unBindStation = null;

        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String state = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

    }
    #endregion

    #region Repaired Defect Component

    [Table("DefectComponent")]
    public class DefectComponent
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_repairID = "RepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int repairID = int.MinValue;

        public const string fn_batchID = "BatchID";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string batchID = null;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string customer = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string model = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string family = null;

        public const string fn_defectCode = "DefectCode";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string defectCode = null;

        public const string fn_defectDescr = "DefectDescr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string defectDescr = null;

        public const string fn_returnLine = "ReturnLine";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string returnLine = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string partSn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string partNo = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string partType = null;

        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string bomNodeType = null;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string iecpn = null;

        public const string fn_customerPn = "CustomerPn";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string customerPn = null;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.NVarChar, 0, 32, false, false, "")]
        public string vendor = null;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string checkItemType = null;

        public const string fn_comment = "Comment";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string comment = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string status = null;


        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

    }

    [Table("DefectComponentLog")]
    public class DefectComponentLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_actionName = "ActionName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string actionName = null;

        public const string fn_componentID = "ComponentID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, false, "")]
        public Int64 componentID = long.MinValue;

        public const string fn_repairID = "RepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int repairID = int.MinValue;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string partSn = null;

        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string customer = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string model = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string family = null;

        public const string fn_defectCode = "DefectCode";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string defectCode = null;

        public const string fn_defectDescr = "DefectDescr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string defectDescr = null;

        public const string fn_returnLine = "ReturnLine";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string returnLine = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string remark = null;

        public const string fn_batchID = "BatchID";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string batchID = null;

        public const string fn_comment = "Comment";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string comment = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string status = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

    }

    [Table("DefectComponentBatchStatus")]
    public class DefectComponentBatchStatus
    {
        public const string fn_batchID = "BatchID";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public string batchID = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string status = null;

        public const string fn_printDate = "PrintDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime printDate = DateTime.MinValue;

        public const string fn_vendor = "Vendor";
        [DBField(SqlDbType.NVarChar, 0, 32, false, false, "")]
        public string vendor = null;

        public const string fn_returnLine = "ReturnLine";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string returnLine = null;

        public const string fn_totalQty = "TotalQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int totalQty = int.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

    }

    [Table("DefectComponentBatchStatusLog")]
    public class DefectComponentBatchStatusLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_batchID = "BatchID";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string batchID = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string status = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

    }

    #endregion

    #region FAI Model table 
    #region FAIModel
    [Table("FAIModel")]
    public class FAIModel
    {
        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 32, true, true, "")]
        public String model = null;

        public const string fn_modelType = "ModelType";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String modelType = null;

        public const string fn_customSKU = "CustomSKU";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String customSKU = null;

        public const string fn_skucode = "SKUCode";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String skucode = null;

        public const string fn_skusuffix = "SKUSuffix";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String skusuffix = null;

        public const string fn_planInputDate = "PlanInputDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime planInputDate = DateTime.MinValue;

        public const string fn_faqty = "FAQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int faqty = int.MinValue;

        public const string fn_inFAQty = "InFAQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int inFAQty = int.MinValue;

        public const string fn_pakqty = "PAKQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int pakqty = int.MinValue;

        public const string fn_inPAKQty = "InPAKQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int inPAKQty = int.MinValue;

        public const string fn_pakstartdate = "PAKStartDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime pakstartdate = DateTime.MinValue;

        public const string fn_fastate = "FAState";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String fastate = null;

        public const string fn_pakstate = "PAKState";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String pakstate = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region ApprovalItem
    [Table("ApprovalItem")]
    public class ApprovalItem
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_module = "Module";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String module = null;

        public const string fn_actionName = "ActionName";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String actionName = null;

        public const string fn_department = "Department";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String department = null;

        public const string fn_isNeedApprove = "IsNeedApprove";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String isNeedApprove = null;

        public const string fn_ownerEmail = "OwnerEmail";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String ownerEmail = null;

        public const string fn_ccemail = "CCEmail";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String ccemail = null;

        public const string fn_isNeedUploadFile = "IsNeedUploadFile";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String isNeedUploadFile = null;

        public const string fn_noticeMsg = "NoticeMsg";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String noticeMsg = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region ApprovalStatus
    [Table("ApprovalStatus")]
    public class ApprovalStatus
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_approvalItemID = "ApprovalItemID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, false, "")]
        public Int64 approvalItemID = long.MinValue;

        public const string fn_moduleKeyValue = "ModuleKeyValue";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String moduleKeyValue = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String status = null;


        public const string fn_comment = "Comment";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String comment = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region ApprovalItemAttr
    [Table("ApprovalItemAttr")]
    public class ApprovalItemAttr
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_approvalItemID = "ApprovalItemID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, false, "")]
        public Int64 approvalItemID = long.MinValue;

        public const string fn_attrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String attrName = null;

        public const string fn_attrValue = "AttrValue";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String attrValue = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region UploadFiles
    [Table("UploadFiles")]
    public class UploadFiles
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_approvalStatusID = "ApprovalStatusID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, false, "")]
        public Int64 approvalStatusID = long.MinValue;

        public const string fn_uploadServerName = "UploadServerName";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String uploadServerName = null;

        public const string fn_uploadFileGUIDName = "UploadFileGUIDName";
        [DBField(SqlDbType.VarChar, 0, 48, true, false, "")]
        public String uploadFileGUIDName = null;

        public const string fn_uploadFileName = "UploadFileName";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public String uploadFileName = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;


    }
    #endregion
    #endregion

    #region AstDefine table
    [Table("AstDefine")]
    public class AstDefine
    {
        public const string fn_astType = "AstType";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public string astType = null;

        public const string fn_astCode = "AstCode";
        [DBField(SqlDbType.VarChar, 0, 16, false, true, "")]
        public string astCode = null;

        public const string fn_astLocation = "AstLocation";
        [DBField(SqlDbType.NVarChar, 0, 16, false, false, "")]
        public string astLocation = null;

        public const string fn_needAssignAstSN = "NeedAssignAstSN";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string needAssignAstSN = null;

        public const string fn_assignAstSNStation = "AssignAstSNStation";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string assignAstSNStation = null;

        public const string fn_combineStation = "CombineStation";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string combineStation = null;

        public const string fn_hasCDSIAst = "HasCDSIAst";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string hasCDSIAst = null;

        public const string fn_needPrint = "NeedPrint";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string needPrint = null;

        public const string fn_needScanSN = "NeedScanSN";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string needScanSN = null;

        public const string fn_checkUnique = "CheckUnique";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string checkUnique = null;

        public const string fn_comment = "Comment";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string comment = null;


        public const string fn_hasUPSAst = "HasUPSAst";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string hasUPSAst = null;

        //public const string fn_upsassignstation = "UPSAssignStation";
        //[DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        //public string upsassignstation = null;

        //public const string fn_upscombinestation = "UPSCombineStation";
        //[DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        //public string upscombinestation = null;

        public const string fn_needBindUPSPO = "NeedBindUPSPO";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string needBindUPSPO = null;
 
        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

    }
    #endregion

    #region FloorAreaLoc
    [Table("FloorAreaLoc")]
    public class FloorAreaLoc
    {
        public const string fn_locID = "LocID";
        [DBField(SqlDbType.VarChar, 0, 64, false, true, "")]
        public string locID = null;

        public const string fn_floor = "Floor";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string floor = null;

        public const string fn_area = "Area";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string area = null;

        public const string fn_loc = "Loc";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String loc = null;

        public const string fn_unit = "Unit";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String unit = null;

        public const string fn_category = "Category";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String category = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String model = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_remainQty = "RemainQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 remainQty = int.MinValue;

        public const string fn_fullLocQty = "FullLocQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 fullLocQty = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String status = null;

        public const string fn_holdInput = "HoldInput";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String holdInput = null;

        public const string fn_holdOutput = "HoldOutput";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String holdOutput = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 128, false, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

    }

    #endregion


    #region ModelAssemblyCode
    [Table("ModelAssemblyCode")]
    public class ModelAssemblyCode
    {
        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 32, false, true, "")]
        public string model = null;

        public const string fn_assemblyCode = "AssemblyCode";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string assemblyCode = null;

        public const string fn_revision = "Revision";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string revision = null;

        public const string fn_supplierCode = "SupplierCode";
        [DBField(SqlDbType.VarChar, 0, 128, false, false, "")]
        public string supplierCode = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 128, false, false, "")]
        public string remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;

    }
    #endregion

    #region UPS

    [Table("UPSHPPO")]
    public class UPSHPPO
    {
        public const string fn_hppo = "HPPO";
        [DBField(SqlDbType.VarChar, 0, 32, false, true, "")]
        public String hppo = null;

        public const string fn_plant = "Plant";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String plant = null;

        public const string fn_potype = "POType";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public String potype = null;

        public const string fn_endCustomerPO = "EndCustomerPO";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String endCustomerPO = null;

        public const string fn_hpsku = "HPSKU";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String hpsku = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int  qty= int.MinValue;

        public const string fn_receiveDate = "ReceiveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime receiveDate = DateTime.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String status = null;

        public const string fn_errorText = "ErrorText";
        [DBField(SqlDbType.VarChar, 0, 512, true, false, "")]
        public String errorText = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UPSIECPO")]
    public class UPSIECPO
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_iecpo = "IECPO";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String iecpo = null;

        public const string fn_iecpoitem = "IECPOItem";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String iecpoitem = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String model = null;

        public const string fn_hppo = "HPPO";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String hppo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int qty = int.MinValue;
       
        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UPSPOAVPart")]
    public class UPSPOAVPart
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_hppo = "HPPO";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String hppo = null;

        public const string fn_avpartno = "AVPartNo";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String avpartno = null;

        public const string fn_iecpartno = "IECPartNo";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String iecpartno = null;

        public const string fn_iecparttype = "IECPartType";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String iecparttype = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UPSCombinePO")]
    public class UPSCombinePO
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public Int64 id = long.MinValue;

        public const string fn_hppo = "HPPO";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String hppo = null;

        public const string fn_iecpo = "IECPO";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public String iecpo = null;

        public const string fn_iecpoitem = "IECPOItem";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String iecpoitem = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String model = null;        

        public const string fn_receiveDate = "ReceiveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime receiveDate = DateTime.MinValue;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String productID = null;

        public const string fn_custsn = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String custsn = null;

        public const string fn_isShipPO = "IsShipPO";
        [DBField(SqlDbType.VarChar, 0, 4, true, false, "")]
        public String isShipPO = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String station = null;


        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String status = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("UPSModel")]
    public class UPSModel
    {
        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 32, false, true, "")]
        public String model = null;

        public const string fn_firstReceiveDate = "FirstReceiveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime firstReceiveDate = DateTime.MinValue;

        public const string fn_lastReceiveDate = "LastReceiveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime lastReceiveDate = DateTime.MinValue;           

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String status = null;

        public const string fn_remark= "Remark";
        [DBField(SqlDbType.VarChar, 0,64, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region MAWB
    [Table("MAWB")]
    public class MAWB
    {
        public const string fn_id = "ID";// ITC-1361-0047 ITC-1361-0048
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int id = int.MinValue;

        public const string fn_mawb = "MAWB";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public string mawb = null;

        public const string fn_delivery = "Delivery";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string delivery = null;

        public const string fn_declarationId = "DeclarationId";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string declarationId = null;

        public const string fn_oceanContainer = "OceanContainer";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string oceanContainer = null;

        public const string fn_hawb = "HAWB";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string hawb = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;
    }
    #endregion MAWB

    #region Small Board Define/ECR
    [Table("SmallBoardDefine")]
    public class SmallBoardDefine
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.BigInt, long.MinValue, long.MaxValue, false, true, "")]
        public long id = long.MinValue;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string family = null;

        public const string fn_mbtype = "MBType";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string mbtype = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string partNo = null;

        public const string fn_qty= "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int qty = int.MinValue;

        public const string fn_maxQty = "MaxQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int maxQty = int.MinValue;

        public const string fn_priority = "Priority";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int priority = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 128, true, false, "")]
        public String remark = null;

        public const string fn_ecr = "ECR";
        [DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
        public string ecr = null;

        public const string fn_iecver = "IECVer";
        [DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
        public string iecver = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }


    [Table("SmallBoardECR")]
    public class SmallBoardECR
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int id = int.MinValue;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string family = null;

        public const string fn_mbcode = "MBCode";
        [DBField(SqlDbType.VarChar, 0, 3, false, false, "")]
        public string mbcode = null;

        public const string fn_mbtype = "MBType";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string mbtype = null;

        public const string fn_ecr = "ECR";
        [DBField(SqlDbType.VarChar, 0, 3, false, false, "")]
        public string ecr = null;

        public const string fn_iecver = "IECVer";
        [DBField(SqlDbType.VarChar, 0, 5, false, false, "")]
        public string iecver = null;

        public const string fn_effectiveDate = "EffectiveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime effectiveDate = DateTime.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region  MBRepairControl
    [Table("MBRepairControl")]
    public class MBRepairControl
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int id = int.MinValue;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string partNo = null;

        public const string fn_materialType = "MaterialType";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string materialType = null;

        public const string fn_stage = "Stage";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string stage = null;


        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string family = null;

        public const string fn_pcbmodelid = "PCBModelID";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string pcbmodelid = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public string line = null;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string location = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int qty = int.MinValue;

        public const string fn_status = "Status";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string status = null;

        public const string fn_loc = "Loc";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public string loc = null;

        public const string fn_assignUser = "AssignUser";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string assignUser = null;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String remark = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;




      
    }
    #endregion


    #region PartTypeEx
    [Table("PartTypeEx")]
    public class PartTypeEx
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int id = int.MinValue;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string partType = null;

        public const string fn_partTypeGroup = "PartTypeGroup";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string partTypeGroup = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;
    }
    #endregion

    #region Customer & Region
    [Table("Customer")]
    public class Customer
    {
        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public String customer = null;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_plant = "Plant";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String plant = null;

        public const string fn_description = "Description";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String description = null;

    }

    [Table("Region")]
    public class Region
    {
        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public String customer = null;

        public const string fn_region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public String region = null;


        public const string fn_regionCode = "RegionCode";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public String regionCode = null;

        public const string fn_description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 128, false, false, "")]
        public String description = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public String editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

    }
    #endregion


    #region CheckItemTypeRule
    [Table("CheckItemTypeRule")]
    public class CheckItemTypeRule
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int id = int.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string checkItemType = null;

        public const string fn_line = "Line";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string line = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string station = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string family = null;

        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string bomNodeType = null;

        public const string fn_partDescr = "PartDescr";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string partDescr = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string partType = null;

        public const string fn_filterExpression = "FilterExpression";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public string filterExpression = null;

        public const string fn_matchRule = "MatchRule";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public string matchRule = null;

        public const string fn_checkRule = "CheckRule";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public string checkRule = null;

        public const string fn_saveRule = "SaveRule";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public string saveRule = null;

        public const string fn_needUniqueCheck = "NeedUniqueCheck";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string needUniqueCheck = null;

        public const string fn_needCommonSave = "NeedCommonSave";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string needCommonSave = null;

        public const string fn_needSave = "NeedSave";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string needSave = null;

        public const string fn_checkTestKPCount = "CheckTestKPCount";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string checkTestKPCount = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public string descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public string editor = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime udt = DateTime.MinValue;


    }
    #endregion

    #region UnpackPCB_Part
    [Table("UnpackPCB_Part")]
    public class UnpackPCB_Part
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;        

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;

        public const string fn_ueditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String ueditor = null;

        public const string fn_updt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime updt = DateTime.MinValue;

        public const string fn_pcb_partid = "PCB_PartID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 pcb_partid = int.MinValue;
    }
    #endregion

    #region TxnDataLog
    [Table("TxnDataLog")]
    public class TxnDataLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_category = "Category";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string category = null;

        public const string fn_action = "Action";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string action = null;

        public const string fn_keyValue1 = "KeyValue1";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string keyValue1 = null;

        public const string fn_keyValue2 = "KeyValue2";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string keyValue2 = null;

        public const string fn_txnId = "TxnId";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string txnId = null;

        public const string fn_errorCode = "ErrorCode";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string errorCode = null;

        public const string fn_errorDescr = "ErrorDescr";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string errorDescr = null;

        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string state = null;

        public const string fn_comment = "Comment";
        [DBField(SqlDbType.VarChar, 0, 1024, false, false, "")]
        public string comment = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;
      
    }
    #endregion
}