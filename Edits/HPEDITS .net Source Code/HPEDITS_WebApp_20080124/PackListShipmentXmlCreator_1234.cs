using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using PackListTD;

namespace Inventec.HPEDITS.XmlCreator
{
    public class PackListShipmentXmlCreator : XmlCreator
    {

#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif
        public override bool LoadData(string aKey)
        {
            LoadPackingListDatabaseData(aKey);
            return true;
        }
        public void LoadPackingListDatabaseData(string id)
        {

            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            string Tp = idSplit[1];
            //fill packID table
            //--------------------- add  by fan  20100107 ----------
            string Type = "";
            string whereTDcheck = "'" + internalID + "','" + Tp + "','2'";
            DataTable TDcheckTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Check]", whereTDcheck);
            Type = TDcheckTable.Rows[0][0].ToString().Trim();
            //--------------------  end  -----------------------------
            NewDataSet packingList = new NewDataSet();
            string whereMainClause = "WHERE InternalID='" + internalID + "'";
            List<string> fields = new List<string>();
            fields.Add("*");
            DataTable pakTable = DBFactory.PopulateTempTable("[v_Shipment_PackList]", whereMainClause, fields);
            NewDataSet.PACK_IDRow newRow = packingList.PACK_ID.NewPACK_IDRow();
            newRow.PACK_ID_Id = 1;
            foreach (DataColumn column in packingList.PACK_ID.Columns)
            {
                if (pakTable.Columns.Contains(column.ColumnName))
                {
                    //if table contains name, then populate                    
                    newRow[column] = pakTable.Rows[0][column.ColumnName];

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
            packingList.PACK_ID.Rows.Add(newRow);
            //HAWB_BOX_QTY
            //string whereClause = "'" + id.Substring(0,10)+ "'";
            //DataTable HAWBBOXTable = DBFactory.PopulateTempTable_BySp("op_GETHAWB_BOX_QTY",whereClause);
            //newRow["HAWB_BOX_QTY"] = HAWBBOXTable.Rows[0][0];
            //HAWB_UNIT_QTY
            //DataTable HAWBUNITTable = DBFactory.PopulateTempTable_BySp("op_GETHAWB_UNIT_QTY", whereClause);
            //newRow["HAWB_UNIT_QTY"] = HAWBUNITTable.Rows[0][0];
            //get line item            

            string whereLneItemClause = "WHERE InternalID LIKE '" + internalID.Substring(0, 10) + "%'";
            DataTable lineTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereLneItemClause, fields);

            int index = 0;
            int index2 = 0;
            foreach (DataRow readRow in lineTable.Rows)
            {
                NewDataSet.PACK_ID_LINE_ITEMRow newLineRow = packingList.PACK_ID_LINE_ITEM.NewPACK_ID_LINE_ITEMRow();
                newLineRow.PACK_ID_LINE_ITEM_Id = index;
                foreach (DataColumn column in packingList.PACK_ID_LINE_ITEM.Columns)
                {
                    if (lineTable.Columns.Contains(column.ColumnName))
                    {
                        newLineRow[column] = readRow[column.ColumnName];
                    }
                    else
                    {
                        if (column.DataType == typeof(string))
                            newLineRow[column] = string.Empty;
                        else if (column.DataType == typeof(Double))
                            newLineRow[column] = 0;
                        else if (column.DataType == typeof(DateTime))
                            newLineRow[column] = string.Empty;
                    }
                }
                string whereWeightClause = "WHERE Model='" + lineTable.Rows[index]["Model"].ToString() + "'";
                DataTable weightTable = DBFactory.PopulateTempTable("[PAK_SkuMasterWeight_FIS]", whereWeightClause, fields);
                index++;

                newLineRow.EXTD_BOX_WEIGHT = Convert.ToDouble(string.Format("{0:0.##}", newLineRow.PACK_ID_LINE_ITEM_BOX_QTY * Convert.ToDouble(weightTable.Rows[0]["Weight"])));

                newLineRow.PACK_ID_UNIT_UOM = "EA";
                newLineRow.PACK_ID_Id = newRow.PACK_ID_Id;

                packingList.PACK_ID_LINE_ITEM.Rows.Add(newLineRow);

                // string where850Clause = "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "' AND HP_PN_COMPONENT !='' order by HP_SO_LINE_ITEM ";
                string where850Clause = "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "' AND HP_PN_COMPONENT !='' and LINE_ITEM = '" + readRow["HP_SO_LINE_ITEM"].ToString() + "' order by HP_SO_LINE_ITEM ";
                DataTable edi850Table = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", where850Clause, fields);
                foreach (DataRow ediRow in edi850Table.Rows)
                {
                    NewDataSet.HP_PN_COMPONENTSRow newHPPNRow = packingList.HP_PN_COMPONENTS.NewHP_PN_COMPONENTSRow();
                    foreach (DataColumn column in packingList.HP_PN_COMPONENTS.Columns)
                    {
                        if (edi850Table.Columns.Contains(column.ColumnName))
                        {
                            newHPPNRow[column] = ediRow[column.ColumnName];
                        }
                        else
                        {
                            if (column.ColumnName == "COMPONENT_UNIT_QTY")
                                newHPPNRow[column] = ediRow["COMPONENT_QTY"];
                            else if (column.DataType == typeof(string))
                                newHPPNRow[column] = string.Empty;
                            else if (column.DataType == typeof(Double))
                                newHPPNRow[column] = 0;
                            else if (column.DataType == typeof(DateTime))
                                newHPPNRow[column] = string.Empty;
                        }
                    }
                    newHPPNRow.PACK_ID_LINE_ITEM_Id = newLineRow.PACK_ID_LINE_ITEM_Id;
                    packingList.HP_PN_COMPONENTS.Rows.Add(newHPPNRow);

                }

                LoadMiniTable_Detail(packingList.EXPORT_NOTES_DETAIL,
                    "EXPORT_NOTES_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'EXPORT_NOTES_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.SHIPPING_INSTR_DETAIL,
                    "SHIPPING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'SHIPPING_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.PICKING_INSTR_DETAIL,
                    "PICKING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'PICKING_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.DELIVERY_INSTR_DETAIL,
                    "DELIVERY_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'DELIVERY_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.SPECIAL_INSTR_DETAIL,
                    "SPECIAL_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "' and PO_ITEM = '" + readRow["C26"].ToString().Substring(1, 5) + "'and FIELDS = 'SPECIAL_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.CUSTOMER_INSTR_DETAIL,
                    "CUSTOMER_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'CUSTOMER_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.HANDLING_INSTR_DETAIL,
                    "HANDLING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'HANDLING_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.UID_INSTR_DETAIL,
                    "UID_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'UID_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.CONFIG_INSTR_DETAIL,
                    "CONFIG_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'CONFIG_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.MAXKIT_DETAIL,
                    "MAXKIT_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'MAXKIT_DETAIL' order by ID", "VALUE");


                /*
                LoadKeyValuePairs(packingList.UDF_DETAIL,
                    null,
                    "UDF_KEY_DETAIL",
                    "UDF_VALUE_DETAIL",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "'");
                */
                //populate box information
                string whereBoxClause = "WHERE InternalID='" + readRow["InternalID"].ToString() + "'";
                DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);

                if (boxTable.Rows.Count == 0)
                {
                    NewDataSet.BOXRow newBoxRowPacking = packingList.BOX.NewBOXRow();
                    newBoxRowPacking.BOX_ID = "PACKING";
                    newBoxRowPacking.BOX_Id_0 = index2;
                    newBoxRowPacking.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                    newBoxRowPacking.BOX_UNIT_QTY = Convert.ToDouble(pakTable.Rows[0]["BOX_UNIT_QTY"]);
                    newBoxRowPacking.BOX_WEIGHT_UOM = "KG";
                    newBoxRowPacking.PACK_ID_LINE_ITEM_Id = newLineRow.PACK_ID_LINE_ITEM_Id;

                    newBoxRowPacking.TRACK_NO_PARCEL = string.Empty;
                    packingList.BOX.Rows.Add(newBoxRowPacking);

                    NewDataSet.SERIAL_NUMRow newSerialRowPacking = packingList.SERIAL_NUM.NewSERIAL_NUMRow();
                    newSerialRowPacking.BOX_Id_0 = index2;
                    newSerialRowPacking.SERIAL_NUM_Column = string.Empty;
                    packingList.SERIAL_NUM.Rows.Add(newSerialRowPacking);


                    // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------
                    /*  if (Type == "Y")
                      {
                          string whereTwoDP = "'" + id + "','" + readRow2["BOX_ID"].ToString() + "'";
                          DataTable TwoDPTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution_packlist]", whereTwoDP);
                          string EnCoding = TwoDPTable.Rows[0]["EnCoding"].ToString(); // Y/N
                          string EnCoder = TwoDPTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
                          string StringIDValue = "";
                          if (EnCoding == "Y")
                          {
                              //encoder
                              if (EnCoder == "PDF417")
                              {
                                  //PDF417 generator
                                  //StringIDValue = TransferCode.Encoder(TwoDTable.Rows[0]["StringIDValue"].ToString());
                                  foreach (DataRow readRowtd in TwoDPTable.Rows)
                                  {
                                      NewDataSet.UDF_BOXRow newRowtwod = packingList.UDF_BOX.NewUDF_BOXRow();
                                      StringIDValue = TransferCode.Encoder(readRowtd["StringIDValue"].ToString());
                                      newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                      newRowtwod.VALUE = StringIDValue.ToString();
                                      newRowtwod.BOX_Id_0 = index2;
                                      packingList.UDF_BOX.Rows.Add(newRowtwod);

                                  }
                              }
                              else //MaxICode
                              {
                                  //MaxICode generator
                              }
                          }
                          else
                          {

                              foreach (DataRow readRowtd in TwoDPTable.Rows)
                              {
                                  NewDataSet.UDF_BOXRow newRowtwod = packingList.UDF_BOX.NewUDF_BOXRow();
                                  newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                  newRowtwod.VALUE = readRowtd["StringIDValue"].ToString();
                                  newRowtwod.BOX_Id_0 = index2;
                                  packingList.UDF_BOX.Rows.Add(newRowtwod);
                              }

                          }
                      }*/
                    //----------------------- end -------------------------------------------------------------- 
                    index2++;
                }
                else
                {
                    foreach (DataRow readRow2 in boxTable.Rows)
                    {
                        NewDataSet.BOXRow newBoxRow = packingList.BOX.NewBOXRow();
                        newBoxRow.BOX_ID = readRow2["BOX_ID"].ToString().Trim();
                        newBoxRow.BOX_Id_0 = index2;
                        newBoxRow.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                        newBoxRow.BOX_UNIT_QTY = Convert.ToDouble(pakTable.Rows[0]["BOX_UNIT_QTY"]);
                        newBoxRow.BOX_WEIGHT_UOM = "KG";
                        newBoxRow.PACK_ID_LINE_ITEM_Id = newLineRow.PACK_ID_LINE_ITEM_Id;

                        newBoxRow.TRACK_NO_PARCEL = readRow2["TRACK_NO_PARCEL"].ToString().Trim();
                        packingList.BOX.Rows.Add(newBoxRow);

                        ////LoadMiniTable(packingList.SERIAL_NUM,
                        //    "SERIAL_NUM_Column",
                        //    readRow2["SERIAL_NUM_Column"].ToString(),
                        //    "BOX_Id_0",
                        //    index2);

                        NewDataSet.SERIAL_NUMRow newSerialRow = packingList.SERIAL_NUM.NewSERIAL_NUMRow();
                        newSerialRow.BOX_Id_0 = index2;
                        newSerialRow.SERIAL_NUM_Column = readRow2["SERIAL_NUM"].ToString().Trim();

                        packingList.SERIAL_NUM.Rows.Add(newSerialRow);

                        // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------
                        if (Type == "Y")
                        {
                            string whereTwoDP = "'" + internalID + "','" + readRow2["BOX_ID"].ToString() + "','" + Tp + "'";
                            DataTable TwoDPTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution_packlist]", whereTwoDP);
                            string EnCoding = TwoDPTable.Rows[0]["EnCoding"].ToString(); // Y/N
                            string EnCoder = TwoDPTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
                            string StringIDValue = "";
                            if (EnCoding == "Y")
                            {
                                //encoder
                                if (EnCoder == "PDF417")
                                {
                                    //PDF417 generator
                                    //StringIDValue = TransferCode.Encoder(TwoDTable.Rows[0]["StringIDValue"].ToString());
                                    foreach (DataRow readRowtd in TwoDPTable.Rows)
                                    {
                                        NewDataSet.UDF_BOXRow newRowtwod = packingList.UDF_BOX.NewUDF_BOXRow();
                                        StringIDValue = TransferCode.Encoder(readRowtd["StringIDValue"].ToString());
                                        newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                        newRowtwod.VALUE = StringIDValue.ToString();
                                        newRowtwod.BOX_Id_0 = index2;
                                        packingList.UDF_BOX.Rows.Add(newRowtwod);

                                    }
                                }
                                else //MaxICode
                                {
                                    //MaxICode generator
                                }
                            }
                            else
                            {

                                foreach (DataRow readRowtd in TwoDPTable.Rows)
                                {
                                    NewDataSet.UDF_BOXRow newRowtwod = packingList.UDF_BOX.NewUDF_BOXRow();
                                    newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                    newRowtwod.VALUE = readRowtd["StringIDValue"].ToString();
                                    newRowtwod.BOX_Id_0 = index2;
                                    packingList.UDF_BOX.Rows.Add(newRowtwod);
                                }

                            }
                        }
                        //----------------------- end -------------------------------------------------------------- 
                        index2++;

                    }
                }


            }



            LoadMiniTable_PACK(packingList.EXPORT_NOTES_HEAD,
                        "EXPORT_NOTES_HEAD_Column",
                        "",
                        "PACK_ID_Id",
                        newRow.PACK_ID_Id,
                        "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'EXPORT_NOTES_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.SHIPPING_INSTR_HEAD,
                "SHIPPING_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'SHIPPING_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.PICKING_INSTR_HEAD,
                "PICKING_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'PICKING_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.DELIVERY_INSTR_HEAD,
                "DELIVERY_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'DELIVERY_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.SPECIAL_INSTR_HEAD,
                "SPECIAL_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'SPECIAL_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.CUSTOMER_INSTR_HEAD,
                "CUSTOMER_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'CUSTOMER_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.LABEL_INSTR_HEAD,
                "LABEL_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'LABEL_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.CARRIER_INST_HEAD,
                "CARRIER_INST_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'CARRIER_INST_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.HANDLING_INSTR_HEAD,
                "HANDLING_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'HANDLING_INSTR_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.INVOICE_UDF_HEAD,
                "INVOICE_UDF_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'INVOICE_UDF_HEAD' order by ID", "VALUE");
            LoadMiniTable_PACK(packingList.UDF_HEAD,
                "UDF_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "' and FIELDS = 'UDF_HEAD' order by ID", "VALUE");

            //get UDF_KEY/VALUE_HEADER
            string whereUDFHEADERClause = "'" + pakTable.Rows[0]["InternalID"].ToString().Trim() + "'";
            DataTable UDFHEADERTable = DBFactory.PopulateTempTable_BySp("op_GetUDF_KEY_VALUE_HEADER_Shipment", whereUDFHEADERClause);
            foreach (DataRow readRow2 in UDFHEADERTable.Rows)
            {
                LoadKeyValuePairs(packingList.UDF_HEADER,
                readRow2,
                "UDF_KEY_HEADER",
                "UDF_VALUE_HEADER",
                "PACK_ID_Id",
                 newRow.PACK_ID_Id);
            }
            /*
            LoadKeyValuePairs(packingList.UDF_HEADER,
                null,
                "UDF_KEY_HEADER",
                "UDF_VALUE_HEADER",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "'");
             */
            //populate mini tables
            //set this dataset to packinglist so we can outputXML
            this.m_DataSet = packingList;
        }
        /*
        private void LoadKeyValuePairs(DataTable dataTable,
            DataRow readRow, string keyColumnName, string valueColumnName,
            string keyColumn, int keyValue, string whereClause)
        {
            List<string> fields = new List<string>();
            fields.Add("DISTINCT " + keyColumnName + ", " + valueColumnName);
            DataTable tempTable = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereClause, fields);
            foreach (DataRow readRow2 in tempTable.Rows)
            {
                DataRow newRow = dataTable.NewRow();
                newRow["KEY"] = readRow2[keyColumnName];
                newRow["VALUE"] = readRow2[valueColumnName];
                newRow[keyColumn] = keyValue;
                dataTable.Rows.Add(newRow);
            }
        }*/

        void LoadMiniTable(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, string whereClause, string distinctField)
        {
            List<string> fields = new List<string>();
            fields.Add("DISTINCT " + distinctField);
            DataTable miniTableName = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereClause, fields);
            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }
        }

        void LoadMiniTable_PACK(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, string whereClause, string distinctField)
        {
            List<string> fields = new List<string>();
            //fields.Add("DISTINCT " + distinctField);
            fields.Add(distinctField);
            DataTable miniTableName = DBFactory.PopulateTempTable("[PAKEDI_INSTR]", whereClause, fields);
            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }
        }

        void LoadMiniTable_Detail(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, string whereClause, string distinctField)
        {
            List<string> fields = new List<string>();
            //fields.Add("DISTINCT " + distinctField);
            fields.Add(distinctField);
            DataTable miniTableName = DBFactory.PopulateTempTable("[v_PO_ITEM_DETAIL]", whereClause, fields);
            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }
        }
        void LoadKeyValuePairs(DataTable dataTable,
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
