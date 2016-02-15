﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using POS;



namespace Inventec.HPEDITS.XmlCreator
{
    /// <summary>
    /// POSXmlCreator Xml Creator
    /// </summary>
    public class POSXmlCreator : XmlCreator
    {

        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();

        public override bool LoadData(string aKey)
        {
            LoadPOSDatabaseData(aKey);
            return true;
        }

        private void LoadPOSDatabaseData(string id)
        {
            //throw new Exception("The method or operation is not implemented.");
            string[] idSplit = id.Split(@"@".ToCharArray());
            string TruckID = idSplit[0];
            string ShipDate = idSplit[1];
            NewDataSet Pos_Data = new NewDataSet();
            //////modify by 20140310 ----dyh
            //string whereClause = InternalID;
            //DataTable PosTable = DBFactory.PopulateTempTable_BySp("op_PosLabel_Shipment", whereClause);
            ///////modify end
            List<string> fields_MAWB = new List<string>();
            //fields_MAWB.Add("*");
            fields_MAWB.Add("DISTINCT    TRUCK_ID ");
            fields_MAWB.Add("ACTUAL_SHIPDATE ");
            fields_MAWB.Add("CONTAINER_ID ");
            fields_MAWB.Add("INCOTERM ");
            fields_MAWB.Add("SHIP_FROM_NAME ");
            fields_MAWB.Add(" SHIP_FROM_NAME_2 ");
            fields_MAWB.Add("SHIP_FROM_NAME_3");
            fields_MAWB.Add("SHIP_FROM_ID ");
            fields_MAWB.Add(" SHIP_FROM_STREET ");
            fields_MAWB.Add("SHIP_FROM_STREET_2 ");
            fields_MAWB.Add("SHIP_FROM_STREET_2 ");
            fields_MAWB.Add("SHIP_FROM_STREET_2 ");
            fields_MAWB.Add("SHIP_FROM_CITY ");
            fields_MAWB.Add("SHIP_FROM_STATE ");
            fields_MAWB.Add("SHIP_FROM_ZIP ");
            fields_MAWB.Add("SHIP_FROM_COUNTRY_NAME ");
            fields_MAWB.Add("SHIP_FROM_CONTACT ");
            fields_MAWB.Add("SHIP_FROM_TELEPHONE ");
            //fields_MAWB.Add ("SHIP_MODE ");
            //fields_MAWB.Add("WAYBILL_NUMBER");
            //fields_MAWB.Add("C46");
            fields_MAWB.Add("Date");
            fields_MAWB.Add("MAWB");

            string whereClause = "WHERE TRUCK_ID='" + TruckID + "' and Date='" + ShipDate + "'";

            DataTable PosTable = DBFactory.PopulateTempTable("[v_PosLabel_Shipment]", whereClause, fields_MAWB);

            /* 
            //int index = 1;
          
           
            //newRow.TRUCK_Id_0 = index;
          
            //foreach(DataRow row in PosTable.Rows)
           // { */
            NewDataSet.TRUCKRow newRow = Pos_Data.TRUCK.NewTRUCKRow();
            //newRow.TRUCK_Id_0 = 2;

            newRow.TRUCK_Id_0 = 1;


            foreach (DataColumn column in Pos_Data.TRUCK.Columns)
            {
                if (PosTable.Columns.Contains(column.ColumnName))
                {
                    //if table contains name, then populate                    
                    newRow[column] = PosTable.Rows[0][column.ColumnName];

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
            } /////Test 20140125
            //Pos_Data.TRUCK.Rows.Add(newRow);
            Pos_Data.TRUCK.Rows.Add(newRow);
            #region mark
            //string DOC_SET = PosTable.Rows[0]["DOC_SET_NUMBER"].ToString();
            //#region EMEA
            //if (DOC_SET.Substring(0, 5) == "DS-EM")
            //    {
            /* //string whereMainClause = "WHERE C46='" + PosTable.Rows[0]["C46"].ToString() + "'";
             string whereMainClause = "WHERE TRUCK_ID='" + TruckID + "' and Date='" + ShipDate + "' Group By INTL_CARRIER";
             List<string> fields = new List<string>();
             //fields.Add("*");
             fields.Add("distinct INTL_CARRIER");
             fields.Add("SUM(TRUCK_LSP_PALLET_QTY)AS TRUCK_LSP_PALLET_QTY");
             fields.Add("SUM(TRUCK_LSP_BOX_QTY) AS  TRUCK_LSP_BOX_QTY");
             fields.Add("SUM(TRUCK_LSP_UNIT_QTY) AS TRUCK_LSP_UNIT_QTY ");
             fields.Add("SUM(TRUCK_LSP_ACT_WEIGHT) AS TRUCK_LSP_ACT_WEIGHT");
             DataTable Forward1 = DBFactory.PopulateTempTable("[v_Pos_FORWARDER]", whereMainClause, fields);
         */
            #endregion

            string where_Fwd = "'" + TruckID + "', '" + ShipDate + "'";

            DataTable Forward1 = DBFactory.PopulateTempTable_BySp("Pos_FORWARDER", where_Fwd);
            int index2 = 0;
            foreach (DataRow ForRow in Forward1.Rows)
            {
                NewDataSet.FORWARDERRow ForwardnewRow = Pos_Data.FORWARDER.NewFORWARDERRow();

                // ForwardnewRow.TRUCK_Id_0 = index2;

                ForwardnewRow.FORWARDER_Id = index2;
                ForwardnewRow.TRUCK_Id_0 = newRow.TRUCK_Id_0;

                foreach (DataColumn ForCol in Pos_Data.FORWARDER.Columns)
                {
                    if (Forward1.Columns.Contains(ForCol.ColumnName))
                    {
                        ForwardnewRow[ForCol] = Forward1.Rows[0][ForCol.ColumnName];
                    }
                    else
                    {
                        if (ForCol.DataType == typeof(string))
                            ForwardnewRow[ForCol] = string.Empty;
                        else if (ForCol.DataType == typeof(Double))
                            ForwardnewRow[ForCol] = 0;
                        else if (ForCol.DataType == typeof(DateTime))
                            ForwardnewRow[ForCol] = string.Empty;
                    }

                }
                Pos_Data.FORWARDER.Rows.Add(ForwardnewRow);

                string whereLAOceanWaybill = "WHERE TRUCK_ID='" + TruckID + "' and Dt='" + ShipDate + "'";
                List<string> UDF_file = new List<string>();
                UDF_file.Add("distinct *");
                DataTable UDF_Data1 = DBFactory.PopulateTempTable("[v_Pos_UDF_HEADER]", whereLAOceanWaybill, UDF_file);
                NewDataSet.UDF_HEADERRow UDF_NEW = Pos_Data.UDF_HEADER.NewUDF_HEADERRow();
                UDF_NEW.FORWARDER_Id = ForwardnewRow.FORWARDER_Id;

                foreach (DataRow readRow2 in UDF_Data1.Rows)
                {
                    LoadKeyValuePairs(Pos_Data.UDF_HEADER,
                        readRow2,
                        "UDF_KEY_HEADER",
                        "UDF_VALUE_HEADER",
                        "FORWARDER_Id",
                         ForwardnewRow.FORWARDER_Id
                         );
                    #region UDF_DETAIL
                    /* For UDF_DETAIL
                                //LoadKeyValuePairs(boxList.UDF_DETAIL,
                                //    readRow2,
                                //    "UDF_KEY_DETAIL",
                                //    "UDF_VALUE_DETAIL",
                                //    "BOX_Id_0",
                                //    newRow.BOX_Id_0);

                                */
                    #endregion
                }
                //}

                #region Mark
                /*
                List<string> fields_MAWB1 = new List<string>();
                fields_MAWB1.Add("distinct *");
                string whereClause_pos = "WHERE TRUCK_ID='" + TruckID + "' and Date='" + ShipDate + "'";
                
                DataTable PosTable1 = DBFactory.PopulateTempTable("[v_PosLabel_Shipment]", whereClause, fields_MAWB1);
                */
                #endregion
                int index3 = 0;
                foreach (DataRow ForRow1 in PosTable.Rows)
                {

                    string wherePallet = "WHERE MAWB='" + ForRow1["MAWB"].ToString() + "'";// and INTL_CARRIER='" + Forward1.Rows[0]["INTL_CARRIER"].ToString() + "'";
                    List<string> fields_Pallet = new List<string>();
                   // fields_Pallet.Add("distinct [PALLET_ID],PALLET_BOX_QTY");
                    #region mark
                    //fields.Add("*");
                    //fields_Pallet.Add("distinct [MASTER_WAYBILL_NUMBER]");
                    //fields_Pallet.Add(" [C46] as WAYBILL_NUMBER ");
                    //fields_Pallet.Add(" [REGION]");
                    //fields_Pallet.Add(" [PALLET_ACT_WEIGHT]");
                    //fields_Pallet.Add(" [PALLET_BOX_QTY]");
                    //fields_Pallet.Add(" [PALLET_UNIT_QTY]");
                    #endregion
                    fields_Pallet.Add("distinct [MASTER_WAYBILL_NUMBER]");
                    fields_Pallet.Add("MAWB as [WAYBILL_NUMBER] ");
                    fields_Pallet.Add(" [REGION]");
                    fields_Pallet.Add(" [PALLET_ID]");
                    fields_Pallet.Add(" [PALLET_ACT_WEIGHT]");
                    fields_Pallet.Add(" [PALLET_BOX_QTY]");
                    fields_Pallet.Add(" [PALLET_UNIT_QTY]");
                    
                    DataTable Pallet_Data = DBFactory.PopulateTempTable("[v_Pos_PALLET]", wherePallet, fields_Pallet);
                    //int index3 = 0;

                    foreach (DataRow Pallte_row in Pallet_Data.Rows)
                    {
                        
                        NewDataSet.PALLETRow PalletnewRow = Pos_Data.PALLET.NewPALLETRow();
                        PalletnewRow.PALLET_Id_0 = index3;
                        PalletnewRow.FORWARDER_Id = index2;
                        #region Mark
                        //string wherePallet2 = "WHERE MAWB='" + ForRow1["MAWB"].ToString() + "' and PALLET_ID='" + Pallte_row["PALLET_ID"].ToString() + "'";
                        //List<string> fields_Pallet2 = new List<string>();
                        ////fields.Add("*");
                        //fields_Pallet2.Add("distinct [MASTER_WAYBILL_NUMBER]");
                        //fields_Pallet2.Add("MAWB as [WAYBILL_NUMBER] ");
                        //fields_Pallet2.Add(" [REGION]");
                        //fields_Pallet2.Add(" [PALLET_ID]");
                        //fields_Pallet2.Add(" [PALLET_ACT_WEIGHT]");
                        //fields_Pallet2.Add(" [PALLET_BOX_QTY]");
                        //fields_Pallet2.Add(" [PALLET_UNIT_QTY]");
                        //DataTable Pallet_Table = DBFactory.PopulateTempTable("[v_Pos_PALLET]", wherePallet2, fields_Pallet2);
                        #endregion
                        foreach (DataColumn PalCol in Pos_Data.PALLET.Columns)
                        {
                            //if (Pallet_Table.Columns.Contains(PalCol.ColumnName))
                            //{
                            //    PalletnewRow[PalCol] = Pallet_Table.Rows[0][PalCol.ColumnName];

                            //}
                            if(Pallte_row.Table.Columns.Contains(PalCol.ColumnName))
                            {
                                 PalletnewRow[PalCol] = Pallte_row[PalCol.ColumnName];
                            }
                            else
                            {
                                if (PalCol.DataType == typeof(string))
                                    PalletnewRow[PalCol] = string.Empty;
                                else if (PalCol.DataType == typeof(Double))
                                    PalletnewRow[PalCol] = 0;
                                else if (PalCol.DataType == typeof(DateTime))
                                    PalletnewRow[PalCol] = string.Empty;
                            }
                        }
                        index3++;
                        Pos_Data.PALLET.Rows.Add(PalletnewRow);
                    }
                }
                //index3++;
            }
            index2++;




            // }
            //#endregion
            #region Other
            //   else
            //   {


            ///*       
            //        //string whereMainClause = "WHERE WAYBILL_NUMBER='" + PosTable.Rows[0]["WAYBILL_NUMBER"].ToString() + "'";
            //       string whereMainClause = "WHERE TRUCK_ID='" + TruckID + "' and Date='" + ShipDate + "' Group By INTL_CARRIER";
            //           //string whereMainClause = "WHERE WAYBILL_NUMBER='" + ForRow1["WAYBILL_NUMBER"].ToString()+"'";
            //        List<string> fields = new List<string>();
            //        //fields.Add("*");
            //        fields.Add("distinct INTL_CARRIER");
            //        fields.Add("SUM(TRUCK_LSP_PALLET_QTY)AS TRUCK_LSP_PALLET_QTY");
            //        fields.Add("SUM(TRUCK_LSP_BOX_QTY) AS  TRUCK_LSP_BOX_QTY");
            //        fields.Add("SUM(TRUCK_LSP_UNIT_QTY) AS TRUCK_LSP_UNIT_QTY ");
            //        fields.Add("SUM(TRUCK_LSP_ACT_WEIGHT) AS TRUCK_LSP_ACT_WEIGHT");
            //        DataTable Forward1 = DBFactory.PopulateTempTable("[v_Pos_FORWARDER]", whereMainClause, fields);
            //*/

            //       string where_Fwd = "'" + TruckID + "', '" + ShipDate + "'";
            //       DataTable Forward1 = DBFactory.PopulateTempTable_BySp("Pos_FORWARDER", where_Fwd);
            //        int index2 = 0;
            //        foreach (DataRow ForRow in Forward1.Rows)
            //        {
            //            NewDataSet.FORWARDERRow ForwardnewRow = Pos_Data.FORWARDER.NewFORWARDERRow();
            //            // ForwardnewRow.TRUCK_Id_0 = index2;
            //            ForwardnewRow.TRUCK_Id_0 = newRow.TRUCK_Id_0;
            //            ForwardnewRow.FORWARDER_Id = index2;
            //            foreach (DataColumn ForCol in Pos_Data.FORWARDER.Columns)
            //            { 
            //                if (Forward1.Columns.Contains(ForCol.ColumnName))
            //                {
            //                    ForwardnewRow[ForCol] = Forward1.Rows[0][ForCol.ColumnName];
            //                }
            //                else
            //                {
            //                    if (ForCol.DataType == typeof(string))
            //                        ForwardnewRow[ForCol] = string.Empty;
            //                    else if (ForCol.DataType == typeof(Double))
            //                        ForwardnewRow[ForCol] = 0;
            //                    else if (ForCol.DataType == typeof(DateTime))
            //                        ForwardnewRow[ForCol] = string.Empty;

            //                }

            //            }


            //            //}

            //            Pos_Data.FORWARDER.Rows.Add(ForwardnewRow);
            //            string whereLAOceanWaybill = "WHERE WAYBILL_NUMBER='" + PosTable.Rows[0]["WAYBILL_NUMBER"].ToString().Trim() + "'";
            //            List<string> UDF_file = new List<string>();
            //            UDF_file.Add("distinct *");
            //            DataTable UDF_Data1 = DBFactory.PopulateTempTable("[v_Pos_UDF_HEADER]", whereLAOceanWaybill, UDF_file);
            //            NewDataSet.UDF_HEADERRow UDF_NEW = Pos_Data.UDF_HEADER.NewUDF_HEADERRow();
            //            UDF_NEW.FORWARDER_Id = ForwardnewRow.FORWARDER_Id;

            //            foreach (DataRow readRow2 in UDF_Data1.Rows)
            //            {
            //                LoadKeyValuePairs(Pos_Data.UDF_HEADER,
            //                    readRow2,
            //                    "UDF_KEY_HEADER",
            //                    "UDF_VALUE_HEADER",
            //                    "FORWARDER_Id",
            //                     ForwardnewRow.FORWARDER_Id
            //                     );
            //                #region UDF_DETAIL
            //                //LoadKeyValuePairs(boxList.UDF_DETAIL,
            //                //    readRow2,
            //                //    "UDF_KEY_DETAIL",
            //                //    "UDF_VALUE_DETAIL",
            //                //    "BOX_Id_0",
            //                //    newRow.BOX_Id_0);
            //                #endregion

            //            }
            //            //Pos_Data.UDF_HEADER.Rows.Add(UDF_NEW);
            //        List<string> fields_MAWB1 = new List<string>();
            //       fields_MAWB1.Add(" distinct *");
            //       string whereClause_pos = "WHERE TRUCK_ID='" + TruckID + "' and Date='" + ShipDate + "'";

            //       DataTable PosTable1 = DBFactory.PopulateTempTable("[v_PosLabel_Shipment]", whereClause, fields_MAWB1);
            //       int index3 = 0;
            //       foreach (DataRow ForRow1 in PosTable1.Rows)
            //       {

            //            //string wherePallet1 = "WHERE WAYBILL_NUMBER='" + Forward1.Rows[0]["WAYBILL_NUMBER"].ToString() + "' and INTL_CARRIER='" + Forward1.Rows[0]["INTL_CARRIER"].ToString() + "'";
            //           string wherePallet1 = "WHERE WAYBILL_NUMBER='" + ForRow1["WAYBILL_NUMBER"].ToString() + "'";// and INTL_CARRIER='" + Forward1.Rows[0]["INTL_CARRIER"].ToString() + "'";
            //            List<string> fields_Pallet1 = new List<string>();
            //            fields_Pallet1.Add("distinct  [PALLET_ID],PALLET_BOX_QTY");
            //            #region mark
            //            //fields.Add("*");
            //            //fields_Pallet1.Add("distinct [MASTER_WAYBILL_NUMBER]");
            //            //fields_Pallet1.Add(" [WAYBILL_NUMBER] ");
            //            //fields_Pallet1.Add(" [REGION]");
            //            //fields_Pallet1.Add(" [PALLET_ACT_WEIGHT]");
            //            //fields_Pallet1.Add(" [PALLET_BOX_QTY]");
            //            //fields_Pallet1.Add(" [PALLET_UNIT_QTY]");
            //            #endregion
            //            DataTable Pallet_Data1 = DBFactory.PopulateTempTable("[v_Pos_PALLET]", wherePallet1, fields_Pallet1);



            //            foreach (DataRow Pallte_row1 in Pallet_Data1.Rows)
            //            {
            //                NewDataSet.PALLETRow PalletnewRow1 = Pos_Data.PALLET.NewPALLETRow();
            //                PalletnewRow1.FORWARDER_Id = index2;
            //                PalletnewRow1.PALLET_Id_0 = index3;
            //                //string wherePallet2 = "WHERE WAYBILL_NUMBER='" + Forward1.Rows[0]["WAYBILL_NUMBER"].ToString() + "' and PALLET_ID='" +  Pallte_row1.Rows[0]["PALLET_ID"].ToString() + "'";
            //                //string wherePallet2 = "WHERE WAYBILL_NUMBER='" + Forward1.Rows[0]["WAYBILL_NUMBER"].ToString() + "' and PALLET_ID='" + Pallte_row1["PALLET_ID"].ToString() + "'";
            //                string wherePallet2 = "WHERE WAYBILL_NUMBER='" + ForRow1["WAYBILL_NUMBER"].ToString() + "' and PALLET_ID='" + Pallte_row1["PALLET_ID"].ToString() + "'";
            //                List<string> fields_Pallet2 = new List<string>();
            //                //fields.Add("*");
            //                fields_Pallet2.Add("distinct [MASTER_WAYBILL_NUMBER]");
            //                fields_Pallet2.Add(" [WAYBILL_NUMBER] ");
            //                fields_Pallet2.Add(" [REGION]");
            //                fields_Pallet2.Add(" [PALLET_ID]");
            //                fields_Pallet2.Add(" [PALLET_ACT_WEIGHT]");
            //                fields_Pallet2.Add(" [PALLET_BOX_QTY]");
            //                fields_Pallet2.Add(" [PALLET_UNIT_QTY]");
            //                DataTable Pallet_Table = DBFactory.PopulateTempTable("[v_Pos_PALLET]", wherePallet2, fields_Pallet2);
            //                foreach (DataColumn PalCol1 in Pos_Data.PALLET.Columns)
            //                {
            //                    if (Pallet_Table.Columns.Contains(PalCol1.ColumnName))
            //                    {
            //                        PalletnewRow1[PalCol1] = Pallet_Table.Rows[0][PalCol1.ColumnName];

            //                    }
            //                    else
            //                    {
            //                        if (PalCol1.DataType == typeof(string))
            //                            PalletnewRow1[PalCol1] = string.Empty;
            //                        else if (PalCol1.DataType == typeof(Double))
            //                            PalletnewRow1[PalCol1] = 0;
            //                        else if (PalCol1.DataType == typeof(DateTime))
            //                            PalletnewRow1[PalCol1] = string.Empty;
            //                    }
            //                }

            //                index3++;
            //                Pos_Data.PALLET.Rows.Add(PalletnewRow1);
            //            }
            //            //index3++;
            //        }


            //        }  // Pos_Data.FORWARDER.Rows.Add(ForwardnewRow);
            //        index2++;

            //    }
            #endregion
            //Pos_Data.FORWARDER.Rows.Add(ForwardnewRow);
            //////   // }
            ////////index++;
            ////// //Pos_Data.TRUCK.Rows.Add(newRow);
            this.m_DataSet = Pos_Data;

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

    }

}
