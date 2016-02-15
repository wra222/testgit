using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
//using WAYBILL_NEW;
using WaybillTD_20121127_new;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
    public class WayBillShipmentXmlCreator : XmlCreator
    {

#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void LoadWayBillDatabaseData(string id)
        {
            logger.Debug("LoadWayBillDatabaseData begin id="+id);

            //--------------------- add  by fan  20100107 ----------
            /* string Type = "";
             string whereTDcheck = "'" + id + "','1'";
             DataTable TDcheckTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Check]", whereTDcheck);
             Type = TDcheckTable.Rows[0][0].ToString().Trim();*/
            //--------------------  end  -----------------------------
            string Consolidate = "";
            //fill boxList table
            WAYBILL_ADDITION wayBillAddition = new WAYBILL_ADDITION();

            /*
            string whereConsolidateClause = "WHERE InternalID='" + id + "'";
            List<string> fields11 = new List<string>();
            fields11.Add("*");
            DataTable ConsolidateTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereConsolidateClause, fields11);
            */

            logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] begin");
            DataTable ConsolidateTable = EditsSqlProc.v_Shipment_PAKComn__by__InternalID(id);
            logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] end");

            ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn internalID=" + id, ConsolidateTable);
            Consolidate = ConsolidateTable.Rows[0]["CONSOL_INVOICE"].ToString().Trim();

            if (Consolidate.Trim() != "")
            {
                string HAWB = ConsolidateTable.Rows[0]["C46"].ToString().Trim();
                List<string> fields = new List<string>();
                fields.Add("*");
                //DISTINCT CONSOL_INVOICE,SUB_REGION,WAYBILL_NUMBER,INTL_CARRIER,ACTUAL_SHIPDATE 
                /*
                List<string> fieldssubregion = new List<string>();
                fieldssubregion.Add("distinct SUB_REGION");
                fieldssubregion.Add("MASTER_WAYBILL_NUMBER");
                fieldssubregion.Add("WAYBILL_NUMBER");
                fieldssubregion.Add("PREF_GATEWAY");
                //fieldssubregion.Add("DUTY_CODE");     //  mark  by  Shao  20120730
                fieldssubregion.Add("INTL_CARRIER");
                fieldssubregion.Add("ACTUAL_SHIPDATE");
                //string whereMainClause = "WHERE CONSOL_INVOICE='" + Consolidate + "'";
                //HP Jennifer reuest the scanning list should calculate by the HAWB,instead of the consolidate
                string whereMainClause = "WHERE C46='" + HAWB + "'";   //modified by Shao 20120730
                DataTable packIDTable = DBFactory.PopulateTempTable("[v_Shipment_WayBillRegion]", whereMainClause, fieldssubregion);
                */

                logger.Debug("LoadWayBillDatabaseData [v_Shipment_WayBillRegion] begin");
                DataTable packIDTable = EditsSqlProc.SUB_REGION__MASTER_WAYBILL_NUMBER__WAYBILL_NUMBER__PREF_GATEWAY__INTL_CARRIER__ACTUAL_SHIPDATE__from__v_Shipment_WayBillRegion__by__C46(HAWB);
                logger.Debug("LoadWayBillDatabaseData [v_Shipment_WayBillRegion] end");

                //     DataTable packIDTable = DBFactory.PopulateTempTable("[v_Shipment_WayBillList]", whereMainClause, fieldssubregion);
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
                                //  else if (column.DataType == typeof(String))
                                //   {
                                //      newRow[column] = Convert.ToString(readRow[column.ColumnName]);
                                // }
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
                            //  else if (column.DataType == typeof(DateTime))
                            //     newRow[column] = string.Empty;
                        }
                    }
                    /*
                    string whereHAWBClause_ACT = "'" + HAWB + "','3'";
                    DataTable hawbTable_ACT = DBFactory.PopulateTempTable_BySp("[op_PAK_ShipmentWeight_FIS]", whereHAWBClause_ACT);
                    */

                    logger.Debug("LoadWayBillDatabaseData [op_PAK_ShipmentWeight_FIS] begin");
                    DataTable hawbTable_ACT = EditsSqlProc.sp__op_PAK_ShipmentWeight_FIS(HAWB, "3");
                    logger.Debug("LoadWayBillDatabaseData [op_PAK_ShipmentWeight_FIS] end");

                    ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", op_PAK_ShipmentWeight_FIS shipment=" + HAWB + ", tp=3", hawbTable_ACT);
                    newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());

                    //HAWB_ACT_WEIGHT
                    /*string whereHAWBClause_ACT = "WHERE Shipment='" + readRow["CONSOL_INVOICE"].ToString() + "'";
                    DataTable hawbTable_ACT = DBFactory.PopulateTempTable("[PAK_ShipmentWeight_FIS]", whereHAWBClause_ACT, fields);
                    newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());*/
                    //HAWB_BOX_QTY
                    //string whereHAWBClause = "WHERE WAYBILL_NUMBER='" + readRow["WAYBILL_NUMBER"].ToString() + "'";
                    //DataTable hawbTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereHAWBClause, fields);
                    /*
                    List<string> fields2 = new List<string>();
                    fields2.Add("sum(DEST_CODE_BOX_QTY) as DEST_CODE_BOX_QTY");
                    string whereHAWBClause = "WHERE C46='" + HAWB + "'";
                    DataTable hawbTable = DBFactory.PopulateTempTable("[v_Shipment_WayBillList]", whereHAWBClause, fields2);
                    */

                    logger.Debug("LoadWayBillDatabaseData [v_Shipment_WayBillList] begin");
                    DataTable hawbTable = EditsSqlProc.sum_DEST_CODE_BOX_QTY__from__v_Shipment_WayBillList__by__C46(HAWB);
                    logger.Debug("LoadWayBillDatabaseData [v_Shipment_WayBillList] end");
                    ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_WayBillList C46=" + HAWB, hawbTable);

                    //add up PACK_ID_UNIT_QTY
                    newRow.HAWB_BOX_QTY = double.Parse(hawbTable.Rows[0]["DEST_CODE_BOX_QTY"].ToString());
                    //SELECT distinct PALLET_ID from [PAK.PAKPaltno] WHERE InternalID in (select InternalID from PAK_PAKComn WHERE  C46='SHA-62910465')

                    //HAWB_PALLET_QTY
                    /*
                    List<string> fieldspallet = new List<string>();
                    fieldspallet.Add("distinct PALLET_ID");
                    string whereHAWB2Clause = "WHERE InternalID in (select InternalID from PAK_PAKComn WHERE  C46= '" + HAWB + "')";
                    DataTable hawb2Table = DBFactory.PopulateTempTable("[PAK.PAKPaltno]", whereHAWB2Clause, fieldspallet);
                    */

                    logger.Debug("LoadWayBillDatabaseData [PAK.PAKPaltno] begin");
                    DataTable hawb2Table = EditsSqlProc.PALLET_ID__from__PAK_PAKPaltno__by__C46(HAWB);
                    logger.Debug("LoadWayBillDatabaseData [PAK.PAKPaltno] end");

                    newRow.HAWB_PALLET_QTY = hawb2Table.Rows.Count;
                    //CUR DATA
                    DateTime T1 = DateTime.Today;

                    //newRow.CURRENT_DATE = DateTime.Today;//  ----modeify 20090911
                    newRow.CURRENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                    //newRow.CURRENT_DATE = DateTime.Today;
                    //newRow.ACTUAL_SHIPDATE = Convert.ToDateTime(packIDTable.Rows[0]["ACTUAL_SHIPDATE"]);
                    //   (Convert.ToDateTime(dr["Date"])).ToString("
                    wayBillAddition.SUBREGION.Rows.Add(newRow);

                    // 取DEST_CODE，CONSOL_INVOICE 判斷循環次數  同一DEST_CODE，CONSOL_INVOICE
                    /*
                    List<string> fields1 = new List<string>();
                    fields1.Add("*");
                    //string whereHAWB3Clause = "WHERE InternalID in (SELECT InternalID FROM [v_PAKComn] " + whereHAWBClause + ") Group by " + fields1[0].ToString() + "," + fields1[1].ToString();
                    string whereHAWB3Clause = "WHERE C46 = '" + HAWB + "'  and SUB_REGION = '" + packIDTable.Rows[index]["SUB_REGION"] + "'";
                    DataTable hawb3Table = DBFactory.PopulateTempTable("[v_Shipment_WayBillDest]", whereHAWB3Clause, fields1);
                    */

                    logger.Debug("LoadWayBillDatabaseData [v_Shipment_WayBillDest] begin");
                    DataTable hawb3Table = EditsSqlProc.v_Shipment_WayBillDest__by__C46__SUB_REGION(HAWB, packIDTable.Rows[index]["SUB_REGION"].ToString());
                    logger.Debug("LoadWayBillDatabaseData [v_Shipment_WayBillDest] end");

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

                        //取得 同一DEST_CODE，CONSOL_INVOICE 的船務
                        /*
                        string whereHAWB4Clause = "WHERE C46 = '" + HAWB + "' and DEST_CODE='" + hawb3Table.Rows[indexcount3]["DEST_CODE"] + "' and SUB_REGION ='" + hawb3Table.Rows[indexcount3]["SUB_REGION"] + "'";
                        DataTable hawb4Table = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereHAWB4Clause, fields);
                        */

                        logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] begin");
                        DataTable hawb4Table = EditsSqlProc.v_Shipment_PAKComn__by__C46__DEST_CODE__SUB_REGION(HAWB, hawb3Table.Rows[indexcount3]["DEST_CODE"].ToString(), hawb3Table.Rows[indexcount3]["SUB_REGION"].ToString());
                        logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] end");

                        int indexcount4 = 0;
                        foreach (DataRow pakRow4 in hawb4Table.Rows)
                        {
                            //每筆船務的boxid,weight
                            /*
                            string whereBoxClause = "WHERE InternalID='" + hawb4Table.Rows[indexcount4]["InternalID"].ToString() + "'";
                            DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                            */

                            logger.Debug("LoadWayBillDatabaseData [PAK_PackkingData] begin");
                            DataTable boxTable = EditsSqlProc.PAK_PackkingData__by__InternalID(hawb4Table.Rows[indexcount4]["InternalID"].ToString());
                            logger.Debug("LoadWayBillDatabaseData [PAK_PackkingData] end");
                            //string whereWeightClause = "WHERE Model='" + readRow["Model"].ToString() + "'";

                            /*
                            DataTable weightTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComnWeight]", whereBoxClause, fields);
                            */
                            logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComnWeight] begin");
                            DataTable weightTable = EditsSqlProc.v_Shipment_PAKComnWeight__by__InternalID(hawb4Table.Rows[indexcount4]["InternalID"].ToString());
                            logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComnWeight] end");
                            //ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComnWeight", weightTable);

                            /*
                            string whereDNClause = "WHERE InternalID='" + hawb4Table.Rows[indexcount4]["InternalID"].ToString() + "'";
                            DataTable DNTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereDNClause, fields);
                            */
                            logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] begin");
                            DataTable DNTable = EditsSqlProc.v_Shipment_PAKComn__by__InternalID(hawb4Table.Rows[indexcount4]["InternalID"].ToString());
                            logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] end");
                            //ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn", DNTable);

                            if (boxTable.Rows.Count > 0)
                            {
                                ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_PAKComnWeight internalID=" + hawb4Table.Rows[indexcount4]["InternalID"].ToString(), weightTable);
                                ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_PAKComn internalID=" + hawb4Table.Rows[indexcount4]["InternalID"].ToString(), DNTable);
                            }
                            foreach (DataRow readRow3 in boxTable.Rows)
                            {
                                WAYBILL_ADDITION.BOXRow newBoxRow = wayBillAddition.BOX.NewBOXRow();
                                newBoxRow.BOX_ID = readRow3["BOX_ID"].ToString().Trim();
                                newBoxRow.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                                newBoxRow.BOX_WEIGHT_UOM = "KG";//string.Empty;
                                newBoxRow.PACK_ID = DNTable.Rows[0]["PACK_ID"].ToString();
                                newBoxRow.HP_SO = DNTable.Rows[0]["CUSTOMER_ID"].ToString();
                                //ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn", hawb4Table);
                                newBoxRow.DUTY_CODE = hawb4Table.Rows[0]["DUTY_CODE"].ToString();

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
                                             wayBillAddition.UDF_BOX.Rows.Add(newRowtwod);
                                             newRowtwod.BOX_Id_0 = index2;
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

                this.m_DataSet = wayBillAddition;
            }
            else if (Consolidate.Trim() == "")
            {
                /*
                string whereMainClause = "WHERE InternalID='" + id + "'";
                List<string> fields = new List<string>();
                fields.Add("*");
                DataTable packIDTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereMainClause, fields);
                */

                logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] begin");
                DataTable packIDTable = EditsSqlProc.v_Shipment_PAKComn__by__InternalID(id);
                logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] end");

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

                            if (column.DataType != packIDTable.Columns[column.ColumnName].DataType)
                            {
                                if (column.DataType == typeof(Double))
                                {
                                    newRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                                }
                                // else if (column.DataType == typeof(String))
                                // {
                                //     newRow[column] = Convert.ToString(readRow[column.ColumnName]);
                                // }
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
                    /* string whereHAWBClause_ACT = "WHERE Shipment='" + id.Substring(0, 10) + "'";
                     DataTable hawbTable_ACT = DBFactory.PopulateTempTable("[PAK_ShipmentWeight_FIS]", whereHAWBClause_ACT, fields);
                     newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());*/
                    /*
                    string whereHAWBClause_ACT = "'" + id.Substring(0, 10) + "','2'";
                    DataTable hawbTable_ACT = DBFactory.PopulateTempTable_BySp("[op_PAK_ShipmentWeight_FIS]", whereHAWBClause_ACT);
                    */

                    logger.Debug("LoadWayBillDatabaseData [op_PAK_ShipmentWeight_FIS] begin");
                    DataTable hawbTable_ACT = EditsSqlProc.sp__op_PAK_ShipmentWeight_FIS(id.Substring(0, 10), "2");
                    logger.Debug("LoadWayBillDatabaseData [op_PAK_ShipmentWeight_FIS] end");

                    ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", op_PAK_ShipmentWeight_FIS shipment=" + id.Substring(0, 10) + ", tp=2", hawbTable_ACT);
                    newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());
                    //HAWB_BOX_QTY
                    /*
                    List<string> fields2 = new List<string>();
                    fields2.Add("Qty as DEST_CODE_BOX_QTY");
                    string whereHAWBClause = "WHERE Consolidate='" + id.Substring(0, 10) + "'";
                    DataTable hawbTable = DBFactory.PopulateTempTable("[P_PoPlt]", whereHAWBClause, fields2);
                    */

                    logger.Debug("LoadWayBillDatabaseData [P_PoPlt] begin");
                    DataTable hawbTable = EditsSqlProc.Qty__from__P_PoPlt__by__Consolidate(id.Substring(0, 10));
                    logger.Debug("LoadWayBillDatabaseData [P_PoPlt] end");

                    //add up PACK_ID_UNIT_QTY
                    ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", P_PoPlt Consolidate=" + id.Substring(0, 10), hawbTable);
                    newRow.HAWB_BOX_QTY = double.Parse(hawbTable.Rows[0]["DEST_CODE_BOX_QTY"].ToString());

                    //HAWB_PALLET_QTY
                    /*
                    List<string> fieldspallet = new List<string>();
                    fieldspallet.Add("distinct PLT as PALLET_ID");
                    string whereHAWB2Clause = "WHERE Consolidate = '" + id.Substring(0, 10) + "'";
                    DataTable hawb2Table = DBFactory.PopulateTempTable("[P_PoPlt]", whereHAWB2Clause, fieldspallet);
                    */

                    logger.Debug("LoadWayBillDatabaseData [P_PoPlt] begin");
                    DataTable hawb2Table = EditsSqlProc.PLT__from__P_PoPlt__by__Consolidate(id.Substring(0, 10));
                    logger.Debug("LoadWayBillDatabaseData [P_PoPlt] end");

                    newRow.HAWB_PALLET_QTY = hawb2Table.Rows.Count;
                    //CUR DATA
                    newRow.CURRENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                    //newRow.CURRENT_DATE = DateTime.Today;
                    //newRow.ACTUAL_SHIPDATE = Convert.ToDateTime(packIDTable.Rows[0]["ACTUAL_SHIPDATE"]);

                    wayBillAddition.SUBREGION.Rows.Add(newRow);

                    // 取DEST_CODE，CONSOL_INVOICE 判斷循環次數  同一DEST_CODE，CONSOL_INVOICE
                    /*
                    List<string> fields1 = new List<string>();
                    fields1.Add("DEST_CODE");
                    fields1.Add("CONSOL_INVOICE");
                    string whereHAWB3Clause = "WHERE InternalID = '" + id + "'";
                    DataTable hawb3Table = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereHAWB3Clause, fields1);
                    */

                    logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] begin");
                    DataTable hawb3Table = EditsSqlProc.DEST_CODE__CONSOL_INVOICE__from__v_Shipment_PAKComn__by__InternalID(id);
                    logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] end");

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
                        //每筆船務的boxid,weight
                        /*
                        string whereBoxClause = "WHERE InternalID='" + id + "'";
                        DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                        */

                        logger.Debug("LoadWayBillDatabaseData [PAK_PackkingData] begin");
                        DataTable boxTable = EditsSqlProc.PAK_PackkingData__by__InternalID(id);
                        logger.Debug("LoadWayBillDatabaseData [PAK_PackkingData] end");

                        //string whereWeightClause = "WHERE Model='" + readRow["Model"].ToString() + "'";

                        /*
                        DataTable weightTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComnWeight]", whereBoxClause, fields);
                        */
                        logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComnWeight] begin");
                        DataTable weightTable = EditsSqlProc.v_Shipment_PAKComnWeight__by__InternalID(id);
                        logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComnWeight] end");

                        string whereDNClause = "WHERE InternalID='" + id + "'";

                        /*
                        DataTable DNTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereDNClause, fields);
                        */
                        logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] begin");
                        DataTable DNTable = EditsSqlProc.v_Shipment_PAKComn__by__InternalID(id);
                        logger.Debug("LoadWayBillDatabaseData [v_Shipment_PAKComn] end");
                        ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn internalID=" + id, DNTable);

                        if (boxTable.Rows.Count > 0)
                        {
                            ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComnWeight internalID=" + id, weightTable);
                            ChkRowNotNull("WayBillShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn internalID=" + id, DNTable);
                        }
                        foreach (DataRow readRow3 in boxTable.Rows)
                        {
                            WAYBILL_ADDITION.BOXRow newBoxRow = wayBillAddition.BOX.NewBOXRow();
                            newBoxRow.BOX_ID = readRow3["BOX_ID"].ToString().Trim();
                            newBoxRow.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                            newBoxRow.BOX_WEIGHT_UOM = string.Empty;
                            newBoxRow.PACK_ID = DNTable.Rows[0]["PACK_ID"].ToString();
                            newBoxRow.HP_SO = DNTable.Rows[0]["CUSTOMER_ID"].ToString();

                            newBoxRow.DESTINATION_CODE_Id = index3;
                            newBoxRow.BOX_Id_0 = index2;
                            index2++;

                            wayBillAddition.BOX.Rows.Add(newBoxRow);


                            //get line item
                            /*
                            string whereLneItemClause = "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "'";
                            DataTable lineItemTable = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereLneItemClause, fields);
                            */

                            logger.Debug("LoadWayBillDatabaseData [PAK.PAKEdi850raw] begin");
                            DataTable lineItemTable = EditsSqlProc.PAK_PAKEdi850raw__by__PO_NUM(readRow["PO_NUM"].ToString());
                            logger.Debug("LoadWayBillDatabaseData [PAK.PAKEdi850raw] end");

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
                            // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------

                            /*     string whereTwoDW = "'" + id + "','"+readRow3["BOX_ID"].ToString()+"'";
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
                                         wayBillAddition.UDF_BOX.Rows.Add(newRowtwod);
                                     }

                                 } */
                            //----------------------- end -------------------------------------------------------------- 




                        }
                        index4++;
                        index3++;
                    }
                }
                //populate mini tables
                //set this dataset to packinglist so we can outputXML
                this.m_DataSet = wayBillAddition;

            }

            logger.Debug("LoadWayBillDatabaseData end");
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
