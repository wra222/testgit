using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using WaybillTD_new;


namespace Inventec.HPEDITS.XmlCreator
{
    public class WayBill_AdditionXMLCreat_new:XmlCreator
    {
#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif

        public void LoadWayBillDatabaseData(string id)
        {

            //--------------------- add  by fan  20100107 ----------
            /* string Type = "";
             string whereTDcheck = "'" + id + "','1'";
             DataTable TDcheckTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Check]", whereTDcheck);
             Type = TDcheckTable.Rows[0][0].ToString().Trim();*/
            //--------------------  end  -----------------------------
            string Consolidate = "";
            //fill boxList table
            WAYBILL_ADDITION wayBillAddition = new WAYBILL_ADDITION();

            string whereConsolidateClause = "WHERE InternalID='" + id + "'";
            List<string> fields11 = new List<string>();
            fields11.Add("*");
            DataTable ConsolidateTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereConsolidateClause, fields11);
            Consolidate = ConsolidateTable.Rows[0]["CONSOL_INVOICE"].ToString().Trim();

            if (Consolidate.Trim() != "")
            {

                List<string> fields = new List<string>();
                fields.Add("*");
                //DISTINCT CONSOL_INVOICE,SUB_REGION,WAYBILL_NUMBER,INTL_CARRIER,ACTUAL_SHIPDATE 
                List<string> fieldssubregion = new List<string>();
                fieldssubregion.Add("distinct CONSOL_INVOICE");
                fieldssubregion.Add("SUB_REGION");
                fieldssubregion.Add("MASTER_WAYBILL_NUMBER");
                fieldssubregion.Add("WAYBILL_NUMBER");
                //fieldssubregion.Add("DUTY_CODE");
                fieldssubregion.Add("INTL_CARRIER");
                fieldssubregion.Add("ACTUAL_SHIPDATE");
                string whereMainClause = "WHERE CONSOL_INVOICE='" + Consolidate + "'";
                DataTable packIDTable = DBFactory.PopulateTempTable("[v_WayBillList]", whereMainClause, fieldssubregion);
                int index = 0;
                int index3 = 0;
                int index2 = 0;
                int index4 = 0;
                foreach (DataRow readRow in packIDTable.Rows)
                {
                    WAYBILL_ADDITION.SUBREGIONRow newRow = wayBillAddition.SUBREGION.NewSUBREGIONRow();
                    newRow.SUBREGION_Id = index;

                    foreach (DataColumn column in wayBillAddition.SUBREGION.Columns)
                    {
                        if (packIDTable.Columns.Contains(column.ColumnName) && (column.ColumnName != "BOX_UNIT_QTY"))
                        {
                            //if table contains name, then populate
                            if (column.DataType != packIDTable.Columns[column.ColumnName].DataType)
                            {
                                if (column.DataType == typeof(Double))
                                {
                                    newRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                                }
                                else if (column.DataType == typeof(String))
                                {
                                    newRow[column] = Convert.ToString(readRow[column.ColumnName]);
                                }
                            }
                            else
                                newRow[column] = readRow[column.ColumnName];
                        }
                        else
                        {
                            if (column.DataType == typeof(string))
                                newRow[column] = string.Empty;
                            else if (column.DataType == typeof(Double))
                                newRow[column] = 0;
                            else if (column.DataType == typeof(DateTime))
                                newRow[column] = DateTime.Today;
                        }
                    }
                    //HAWB_ACT_WEIGHT
                    string whereHAWBClause_ACT = "WHERE Shipment='" + readRow["CONSOL_INVOICE"].ToString() + "'";
                    DataTable hawbTable_ACT = DBFactory.PopulateTempTable("[PAK_ShipmentWeight_FIS]", whereHAWBClause_ACT, fields);
                    newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());
                    //HAWB_BOX_QTY
                    //string whereHAWBClause = "WHERE WAYBILL_NUMBER='" + readRow["WAYBILL_NUMBER"].ToString() + "'";
                    //DataTable hawbTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereHAWBClause, fields);
                    List<string> fields2 = new List<string>();
                    fields2.Add("sum(DEST_CODE_BOX_QTY) as DEST_CODE_BOX_QTY");
                    string whereHAWBClause = "WHERE CONSOL_INVOICE='" + readRow["CONSOL_INVOICE"].ToString() + "'";
                    DataTable hawbTable = DBFactory.PopulateTempTable("[v_WayBillList]", whereHAWBClause, fields2);
                    //add up PACK_ID_UNIT_QTY
                    newRow.HAWB_BOX_QTY = double.Parse(hawbTable.Rows[0]["DEST_CODE_BOX_QTY"].ToString());

                    //HAWB_PALLET_QTY
                    List<string> fieldspallet = new List<string>();
                    fieldspallet.Add("distinct PALLET_ID");
                    string whereHAWB2Clause = "WHERE InternalID in (SELECT InternalID FROM [v_PAKComn] " + whereHAWBClause + ")";
                    DataTable hawb2Table = DBFactory.PopulateTempTable("[PAK.PAKPaltno]", whereHAWB2Clause, fieldspallet);
                    newRow.HAWB_PALLET_QTY = hawb2Table.Rows.Count;
                    //CUR DATA
                    // newRow.CURRENT_DATE = DateTime.Today;
                    newRow.CURRENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                    // newRow.ACTUAL_SHIPDATE = Convert.ToDateTime(packIDTable.Rows[0]["ACTUAL_SHIPDATE"]);
                    wayBillAddition.SUBREGION.Rows.Add(newRow);

                    // ��DEST_CODE�ACONSOL_INVOICE �P�_�`������  �P�@DEST_CODE�ACONSOL_INVOICE
                    List<string> fields1 = new List<string>();
                    fields1.Add("*");
                    //string whereHAWB3Clause = "WHERE InternalID in (SELECT InternalID FROM [v_PAKComn] " + whereHAWBClause + ") Group by " + fields1[0].ToString() + "," + fields1[1].ToString();
                    string whereHAWB3Clause = "WHERE CONSOL_INVOICE = '" + Consolidate + "'  and SUB_REGION = '" + packIDTable.Rows[index]["SUB_REGION"] + "'";
                    DataTable hawb3Table = DBFactory.PopulateTempTable("[v_WayBillList]", whereHAWB3Clause, fields1);
                    int indexcount3 = 0;  //DESTINATION_CODE_Id
                    int indexcount2 = 0;  //boxid
                    foreach (DataRow pakRow in hawb3Table.Rows)
                    {
                        //newRow.HAWB_BOX_QTY += Convert.ToDouble(pakRow["PACK_ID_UNIT_QTY"]);
                        WAYBILL_ADDITION.DESTINATION_CODERow newDestRow = wayBillAddition.DESTINATION_CODE.NewDESTINATION_CODERow();

                        newDestRow.DESTINATION_CODE_Id = index3;
                        newDestRow.DEST_CODE = pakRow["DEST_CODE"].ToString();
                        newDestRow.DEST_CODE_EXTD_BOX_WGT = double.Parse(pakRow["DEST_CODE_EXTD_BOX_WGT"].ToString());
                        newDestRow.DEST_CODE_BOX_QTY = double.Parse(pakRow["DEST_CODE_BOX_QTY"].ToString());
                        newDestRow.SUBREGION_Id = index;
                        wayBillAddition.DESTINATION_CODE.Rows.Add(newDestRow);
                        /*
                        List<string> fieldscon = new List<string>();
                        fieldscon.Add("Sum(DEST_CODE_BOX_QTY) as DEST_CODE_BOX_QTY");
                        fieldscon.Add("Sum(DEST_CODE_EXTD_BOX_WGT) as DEST_CODE_EXTD_BOX_WGT");
                        string whereHAWBClause_Weight = "WHERE CONSOL_INVOICE = '" + hawb3Table.Rows[index3]["CONSOL_INVOICE"] + "' and DEST_CODE='" + hawb3Table.Rows[index3]["DEST_CODE"] + "' group by CONSOL_INVOICE    ";
                        DataTable hawb3Table_Weight = DBFactory.PopulateTempTable("[v_WayBillWeight]", whereHAWBClause_Weight, fieldscon);
                        newDestRow.DEST_CODE_EXTD_BOX_WGT = double.Parse(hawb3Table_Weight.Rows[0]["DEST_CODE_EXTD_BOX_WGT"].ToString());
                        newDestRow.DEST_CODE_BOX_QTY = double.Parse(hawb3Table_Weight.Rows[0]["DEST_CODE_BOX_QTY"].ToString());
                        newDestRow.SUBREGION_Id = index;
                        wayBillAddition.DESTINATION_CODE.Rows.Add(newDestRow);
                        */

                        //���o �P�@DEST_CODE�ACONSOL_INVOICE �����
                        string whereHAWB4Clause = "WHERE CONSOL_INVOICE = '" + hawb3Table.Rows[indexcount3]["CONSOL_INVOICE"] + "' and DEST_CODE='" + hawb3Table.Rows[indexcount3]["DEST_CODE"] + "' and SUB_REGION ='" + hawb3Table.Rows[indexcount3]["SUB_REGION"] + "'";
                        DataTable hawb4Table = DBFactory.PopulateTempTable("[v_PAKComn]", whereHAWB4Clause, fields);

                        int indexcount4 = 0;
                        foreach (DataRow pakRow4 in hawb4Table.Rows)
                        {
                            //�C����Ȫ�boxid,weight
                            string whereBoxClause = "WHERE InternalID='" + hawb4Table.Rows[indexcount4]["InternalID"].ToString() + "'";
                            DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                            //string whereWeightClause = "WHERE Model='" + readRow["Model"].ToString() + "'";
                            DataTable weightTable = DBFactory.PopulateTempTable("[v_PAKComnWeight]", whereBoxClause, fields);
                            string whereDNClause = "WHERE InternalID='" + hawb4Table.Rows[indexcount4]["InternalID"].ToString() + "'";
                            DataTable DNTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereDNClause, fields);
                            foreach (DataRow readRow3 in boxTable.Rows)
                            {
                                WAYBILL_ADDITION.BOXRow newBoxRow = wayBillAddition.BOX.NewBOXRow();
                                newBoxRow.BOX_ID = readRow3["BOX_ID"].ToString().Trim();
                                newBoxRow.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                                newBoxRow.BOX_WEIGHT_UOM = string.Empty;
                                newBoxRow.PACK_ID = DNTable.Rows[0]["PACK_ID"].ToString();
                                newBoxRow.HP_SO = DNTable.Rows[0]["CUSTOMER_ID"].ToString();
                                newBoxRow.DUTY_CODE=DNTable.Rows[0]["DUTY_CODE"].ToString();
                                newBoxRow.DESTINATION_CODE_Id = index3;
                                newBoxRow.BOX_Id_0 = index2;

                                wayBillAddition.BOX.Rows.Add(newBoxRow);


                                // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------
                                /* if (Type == "Y")
                                 {
                                     string whereTwoDW = "'" + id + "','" + readRow3["BOX_ID"].ToString() + "'";
                                     DataTable TwoDWTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution_waybill]", whereTwoDW);
                                     string EnCoding = TwoDWTable.Rows[0]["EnCoding"].ToString(); // Y/N
                                     string EnCoder = TwoDWTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
                                     string StringIDValue = "";
                                     if (EnCoding == "Y")
                                     {
                                         //encoder
                                         if (EnCoder == "PDF417")
                                         {
                                             //PDF417 generator
                                             //StringIDValue = TransferCode.Encoder(TwoDTable.Rows[0]["StringIDValue"].ToString());
                                             foreach (DataRow readRowtd in TwoDWTable.Rows)
                                             {
                                                 WAYBILL_ADDITION.UDF_BOXRow newRowtwod = wayBillAddition.UDF_BOX.NewUDF_BOXRow();
                                                 StringIDValue = TransferCode.Encoder(readRowtd["StringIDValue"].ToString());
                                                 newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                                 newRowtwod.VALUE = StringIDValue.ToString();
                                                 newRowtwod.BOX_Id_0 = index2;
                                                 wayBillAddition.UDF_BOX.Rows.Add(newRowtwod);

                                             }
                                         }
                                         else //MaxICode
                                         {
                                             //MaxICode generator
                                         }
                                     }
                                     else
                                     {

                                         foreach (DataRow readRowtd in TwoDWTable.Rows)
                                         {
                                             WAYBILL_ADDITION.UDF_BOXRow newRowtwod = wayBillAddition.UDF_BOX.NewUDF_BOXRow();
                                             newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                             newRowtwod.VALUE = readRowtd["StringIDValue"].ToString();
                                             newRowtwod.BOX_Id_0 = index2;
                                             wayBillAddition.UDF_BOX.Rows.Add(newRowtwod);
                                         }

                                     }
                                 }*/
                                //----------------------- end -------------------------------------------------------------- 
                                index2++;
                                indexcount2++;

                            }
                            index4++;
                            indexcount4++;
                        }
                        index3++;
                        indexcount3++;
                    }
                    index++;

                }
                //populate mini tables
                //set this dataset to packinglist so we can outputXML
                this.m_DataSet = wayBillAddition;
            }
            else if (Consolidate.Trim() == "")
            {
                string whereMainClause = "WHERE InternalID='" + id + "'";
                List<string> fields = new List<string>();
                fields.Add("*");
                DataTable packIDTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereMainClause, fields);
                int index = 0;
                foreach (DataRow readRow in packIDTable.Rows)
                {
                    WAYBILL_ADDITION.SUBREGIONRow newRow = wayBillAddition.SUBREGION.NewSUBREGIONRow();
                    index++;
                    newRow.SUBREGION_Id = index;

                    foreach (DataColumn column in wayBillAddition.SUBREGION.Columns)
                    {
                        if (packIDTable.Columns.Contains(column.ColumnName) && (column.ColumnName != "BOX_UNIT_QTY"))
                        {
                            //if table contains name, then populate
                            if (column.DataType != packIDTable.Columns[column.ColumnName].DataType)
                            {
                                if (column.DataType == typeof(Double))
                                {
                                    newRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                                }
                                else if (column.DataType == typeof(String))
                                {
                                    newRow[column] = Convert.ToString(readRow[column.ColumnName]);
                                }
                            }
                            else
                                newRow[column] = readRow[column.ColumnName];
                        }
                        else
                        {
                            if (column.DataType == typeof(string))
                                newRow[column] = string.Empty;
                            else if (column.DataType == typeof(Double))
                                newRow[column] = 0;
                            else if (column.DataType == typeof(DateTime))
                                newRow[column] = string.Empty;
                        }
                    }
                    //HAWB_ACT_WEIGHT
                    string whereHAWBClause_ACT = "WHERE Shipment='" + id.Substring(0, 10) + "'";
                    DataTable hawbTable_ACT = DBFactory.PopulateTempTable("[PAK_ShipmentWeight_FIS]", whereHAWBClause_ACT, fields);
                    newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());
                    //HAWB_BOX_QTY
                    //string whereHAWBClause = "WHERE WAYBILL_NUMBER='" + readRow["WAYBILL_NUMBER"].ToString() + "'";
                    //DataTable hawbTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereHAWBClause, fields);
                    List<string> fields2 = new List<string>();
                    fields2.Add("PALLET_UNIT_QTY as DEST_CODE_BOX_QTY");
                    string whereHAWBClause = "WHERE InternalID='" + id + "'";
                    DataTable hawbTable = DBFactory.PopulateTempTable("[PAK.PAKPaltno]", whereHAWBClause, fields2);
                    //add up PACK_ID_UNIT_QTY
                    newRow.HAWB_BOX_QTY = double.Parse(hawbTable.Rows[0]["DEST_CODE_BOX_QTY"].ToString());

                    //HAWB_PALLET_QTY
                    List<string> fieldspallet = new List<string>();
                    fieldspallet.Add("distinct PALLET_ID");
                    string whereHAWB2Clause = "WHERE InternalID in (SELECT InternalID FROM [v_PAKComn] " + whereHAWBClause + ")";
                    DataTable hawb2Table = DBFactory.PopulateTempTable("[PAK.PAKPaltno]", whereHAWB2Clause, fieldspallet);
                    newRow.HAWB_PALLET_QTY = hawb2Table.Rows.Count;
                    //CUR DATA
                    newRow.CURRENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                    // newRow.CURRENT_DATE = DateTime.Today;

                    wayBillAddition.SUBREGION.Rows.Add(newRow);

                    // ��DEST_CODE�ACONSOL_INVOICE �P�_�`������  �P�@DEST_CODE�ACONSOL_INVOICE
                    List<string> fields1 = new List<string>();
                    fields1.Add("DEST_CODE");
                    fields1.Add("CONSOL_INVOICE");
                    string whereHAWB3Clause = "WHERE InternalID = '" + id + "'";
                    DataTable hawb3Table = DBFactory.PopulateTempTable("[v_PAKComn]", whereHAWB3Clause, fields1);
                    int index3 = 0;  //DESTINATION_CODE_Id
                    int index2 = 0;  //boxid
                    foreach (DataRow pakRow in hawb3Table.Rows)
                    {
                        //newRow.HAWB_BOX_QTY += Convert.ToDouble(pakRow["PACK_ID_UNIT_QTY"]);
                        WAYBILL_ADDITION.DESTINATION_CODERow newDestRow = wayBillAddition.DESTINATION_CODE.NewDESTINATION_CODERow();

                        newDestRow.DESTINATION_CODE_Id = index3;
                        newDestRow.DEST_CODE = pakRow["DEST_CODE"].ToString();
                        newDestRow.DEST_CODE_EXTD_BOX_WGT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());
                        newDestRow.DEST_CODE_BOX_QTY = double.Parse(hawbTable.Rows[0]["DEST_CODE_BOX_QTY"].ToString());
                        newDestRow.SUBREGION_Id = index;
                        wayBillAddition.DESTINATION_CODE.Rows.Add(newDestRow);


                        int index4 = 0;
                        //�C����Ȫ�boxid,weight
                        string whereBoxClause = "WHERE InternalID='" + id + "'";
                        DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                        //string whereWeightClause = "WHERE Model='" + readRow["Model"].ToString() + "'";
                        DataTable weightTable = DBFactory.PopulateTempTable("[v_PAKComnWeight]", whereBoxClause, fields);
                        string whereDNClause = "WHERE InternalID='" + id + "'";
                        DataTable DNTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereDNClause, fields);
                        foreach (DataRow readRow3 in boxTable.Rows)
                        {
                            WAYBILL_ADDITION.BOXRow newBoxRow = wayBillAddition.BOX.NewBOXRow();
                            newBoxRow.BOX_ID = readRow3["BOX_ID"].ToString().Trim();
                            newBoxRow.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                            newBoxRow.BOX_WEIGHT_UOM = string.Empty;
                            newBoxRow.PACK_ID = DNTable.Rows[0]["PACK_ID"].ToString();
                            newBoxRow.HP_SO = DNTable.Rows[0]["CUSTOMER_ID"].ToString();
                            newBoxRow.DUTY_CODE = DNTable.Rows[0]["DUTY_CODE"].ToString();
                            newBoxRow.DESTINATION_CODE_Id = index3;
                            newBoxRow.BOX_Id_0 = index2;
                            index2++;

                            wayBillAddition.BOX.Rows.Add(newBoxRow);


                            //get line item
                            string whereLneItemClause = "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "'";

                            DataTable lineItemTable = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereLneItemClause, fields);
                            foreach (DataRow readRow2 in lineItemTable.Rows)
                            {
                                LoadKeyValuePairs(wayBillAddition.UDF_HEADER,
                                    readRow2,
                                    "UDF_KEY_HEADER",
                                    "UDF_VALUE_HEADER",
                                    "BOX_Id_0",
                                    newBoxRow.BOX_Id_0);
                                LoadKeyValuePairs(wayBillAddition.UDF_DETAIL,
                                    readRow2,
                                    "UDF_KEY_DETAIL",
                                    "UDF_VALUE_DETAIL",
                                    "BOX_Id_0",
                                    newBoxRow.BOX_Id_0);
                            }
                        }
                        index4++;
                        index3++;
                    }
                }
                //populate mini tables
                //set this dataset to packinglist so we can outputXML
                this.m_DataSet = wayBillAddition;
                /* Consolidate = Consolidate.Substring(0, 10);
                string whereMainClause = "WHERE CONSOL_INVOICE='" + Consolidate + "'";
                List<string> fields = new List<string>();
                fields.Add("*");
                DataTable packIDTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereMainClause, fields);
                int index = 0;
                
                foreach (DataRow readRow in packIDTable.Rows)
                {
                    WAYBILL_ADDITION.SUBREGIONRow newRow = wayBillAddition.SUBREGION.NewSUBREGIONRow();
                    index++;
                    newRow.SUBREGION_Id = index;

                    foreach (DataColumn column in wayBillAddition.SUBREGION.Columns)
                    {
                        if (packIDTable.Columns.Contains(column.ColumnName) && (column.ColumnName != "BOX_UNIT_QTY"))
                        {
                            //if table contains name, then populate
                            if (column.DataType != packIDTable.Columns[column.ColumnName].DataType)
                            {
                                if (column.DataType == typeof(Double))
                                {
                                    newRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                                }
                                else if (column.DataType == typeof(String))
                                {
                                    newRow[column] = Convert.ToString(readRow[column.ColumnName]);
                                }
                            }
                            else
                                newRow[column] = readRow[column.ColumnName];
                        }
                        else
                        {
                            if (column.DataType == typeof(string))
                                newRow[column] = string.Empty;
                            else if (column.DataType == typeof(Double))
                                newRow[column] = 0;
                            else if (column.DataType == typeof(DateTime))
                                newRow[column] = string.Empty;
                        }
                    }
                    //HAWB_BOX_QTY
                    string whereHAWBClause = "WHERE WAYBILL_NUMBER='" + readRow["WAYBILL_NUMBER"].ToString() + "'";
                    DataTable hawbTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereHAWBClause, fields);
                    //add up PACK_ID_UNIT_QTY
                    newRow.HAWB_BOX_QTY = 0;

                    //HAWB_PALLET_QTY
                    string whereHAWB2Clause = "WHERE InternalID in (SELECT InternalID FROM [v_PAKComn] " + whereHAWBClause + ")";
                    DataTable hawb2Table = DBFactory.PopulateTempTable("[PAK.PAKPaltno]", whereHAWB2Clause, fields);
                    newRow.HAWB_PALLET_QTY = hawb2Table.Rows.Count;
                    //CUR DATA
                    newRow.CURRENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");

                    wayBillAddition.SUBREGION.Rows.Add(newRow);

                    foreach (DataRow pakRow in hawbTable.Rows)
                    {
                        newRow.HAWB_BOX_QTY += Convert.ToDouble(pakRow["PACK_ID_UNIT_QTY"]);
                        WAYBILL_ADDITION.DESTINATION_CODERow newDestRow = wayBillAddition.DESTINATION_CODE.NewDESTINATION_CODERow();

                        newDestRow.DESTINATION_CODE_Id = 0;
                        newDestRow.DEST_CODE = pakRow["DEST_CODE"].ToString();

                        newDestRow.DEST_CODE_EXTD_BOX_WGT = 0;
                        newDestRow.DEST_CODE_BOX_QTY = 0;
                        newDestRow.SUBREGION_Id = index;
                        wayBillAddition.DESTINATION_CODE.Rows.Add(newDestRow);
                    }



                    string whereBoxClause = "WHERE InternalID='" + id + "'";
                    DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                    int index2 = 0;
                    string whereWeightClause = "WHERE Model='" + readRow["Model"].ToString() + "'";
                    DataTable weightTable = DBFactory.PopulateTempTable("[PAK_SkuMasterWeight_FIS]", whereWeightClause, fields);

                    foreach (DataRow readRow3 in boxTable.Rows)
                    {
                        WAYBILL_ADDITION.BOXRow newBoxRow = wayBillAddition.BOX.NewBOXRow();
                        newBoxRow.BOX_ID = readRow3["BOX_ID"].ToString().Trim();
                        newBoxRow.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                        newBoxRow.BOX_WEIGHT_UOM = string.Empty;
                        newBoxRow.PACK_ID = readRow["PACK_ID"].ToString();
                        newBoxRow.HP_SO = readRow["HP_SO"].ToString();

                        newBoxRow.DESTINATION_CODE_Id = 0;
                        newBoxRow.BOX_Id_0 = index2;
                        index2++;
                        wayBillAddition.BOX.Rows.Add(newBoxRow);


                        //get line item
                        string whereLneItemClause = "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "'";

                        DataTable lineItemTable = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereLneItemClause, fields);
                        foreach (DataRow readRow2 in lineItemTable.Rows)
                        {
                            LoadKeyValuePairs(wayBillAddition.UDF_HEADER,
                                readRow2,
                                "UDF_KEY_HEADER",
                                "UDF_VALUE_HEADER",
                                "BOX_Id_0",
                                newBoxRow.BOX_Id_0);
                            LoadKeyValuePairs(wayBillAddition.UDF_DETAIL,
                                readRow2,
                                "UDF_KEY_DETAIL",
                                "UDF_VALUE_DETAIL",
                                "BOX_Id_0",
                                newBoxRow.BOX_Id_0);
                        }
                    }
                }
                //populate mini tables
                //set this dataset to packinglist so we can outputXML
                this.m_DataSet = wayBillAddition;
                */
            }

        }
        public override bool LoadData(string aKey)
        {
            LoadWayBillDatabaseData(aKey);
            return true;
        }
        private void LoadKeyValuePairs(DataTable dataTable,
            DataRow readRow, string keyColumnName, string valueColumnName,
            string keyColumn, int keyValue)
        {
            DataRow newRow = dataTable.NewRow();
            newRow["KEY"] = readRow[keyColumnName];
            newRow["VALUE"] = readRow[valueColumnName];
            newRow[keyColumn] = keyValue;
            dataTable.Rows.Add(newRow);
        }

        void LoadMiniTable(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue)
        {
            DataRow row = dataTable.NewRow();
            row[keyColumn] = keyValue;
            row[fieldName] = value;
            dataTable.Rows.Add(row);
        }        
	
    }
}