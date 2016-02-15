using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using PalletATD;

namespace Inventec.HPEDITS.XmlCreator
{
    public class PalletAShipmentXmlCreator : XmlCreator
    {

#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif
        public override bool LoadData(string aKey)
        {
            LoadPalletADatabaseData(aKey);
            return true;
        }
        public void LoadPalletADatabaseData(string id)
        {
            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            string palletID = idSplit[1];

            //fill packID table
            PALLETS palletList = new PALLETS();

            //string whereMainClause = "WHERE InternalID='" + internalID + "' AND PALLET_ID='" + palletID + "'";
            string whereMainClause = "'" + internalID.Substring(0, 16) + "','" + palletID + "'";
            List<string> fields = new List<string>();
            fields.Add("*");
            DataTable palletTable = DBFactory.PopulateTempTable_BySp("[op_Shipment_PAKPalletTypeALineItem]", whereMainClause);
            //DataTable palletTable = DBFactory.PopulateTempTable("[op_Shipment_PAKPalletTypeALineItem]", whereMainClause, fields);

            PALLETS.PALLETDataTable newTable = palletList.PALLET;
            int index = 0;
            foreach (DataRow readRow in palletTable.Rows)
            {
                PALLETS.PALLETRow newRow = newTable.NewPALLETRow();
                newRow.PALLET_Id_0 = index;
               // index++;

                foreach (DataColumn column in newTable.Columns)
                {
                    if (palletTable.Columns.Contains(column.ColumnName))
                    {
                        //PACK_ID_UNIT_QTY_PER_PALLET
                        if (column.ColumnName == "PACK_ID_UNIT_QTY_PER_PALLET")
                        {
                           /* List<string> fieldsUNITPALLET = new List<string>();
                            fieldsUNITPALLET.Add("Sum(convert(int,PACK_ID_UNIT_QTY_PER_PALLET)) as PACK_ID_UNIT_QTY_PER_PALLET");
                            string whereUNITPALLET = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";*/
                            string whereUNITPALLET = "'" + internalID.Substring(0, 16) + "','" + palletID + "'";
                            DataTable BOXUNITPALLET = DBFactory.PopulateTempTable_BySp("[op_Shipment_PAKPalletTypeALineItem]", whereUNITPALLET);
                            newRow[column] = BOXUNITPALLET.Rows[0]["PACK_ID_UNIT_QTY_PER_PALLET"];
                            continue;
                        }
                        //PACK_ID_BOX_QTY_PER_PALLET
                        if (column.ColumnName == "PACK_ID_BOX_QTY_PER_PALLET")
                        {
                            /*List<string> fieldsBOXPALLET = new List<string>();
                            fieldsBOXPALLET.Add("Sum(convert(int,PACK_ID_BOX_QTY_PER_PALLET)) as PACK_ID_BOX_QTY_PER_PALLET");
                            string whereBOXPALLET = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable BOXPERPALLET = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeALineItem]", whereBOXPALLET, fieldsBOXPALLET);*/

                            string whereBOXPALLET = "'" + internalID.Substring(0, 16) + "','" + palletID + "'";
                            DataTable BOXPERPALLET = DBFactory.PopulateTempTable_BySp("[op_Shipment_PAKPalletTypeALineItem]", whereBOXPALLET);
                            newRow[column] = BOXPERPALLET.Rows[0]["PACK_ID_BOX_QTY_PER_PALLET"];
                            continue;
                        }
                        if (column.ColumnName == "PALLET_ID")
                        {
                            List<string> fieldsplts = new List<string>();
                            fieldsplts.Add("*");
                            string wherePLTS = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable PLTUCC = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeB]", wherePLTS, fieldsplts);
                            if (PLTUCC.Rows[0]["UCC"].ToString().Trim().Length == 20)
                            {
                                newRow[column] = PLTUCC.Rows[0]["UCC"].ToString().Trim();

                            }
                            else
                            {
                                newRow[column] = palletTable.Rows[0][column.ColumnName].ToString();
                            }
                            continue;
                        }
                        //if table contains name, then populate

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
                //get the order item
                string whereOrderClause = "WHERE InternalID='" + internalID + "'";
                DataTable orderTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereOrderClause, fields);
                foreach (DataColumn column in newTable.Columns)
                {
                    if (orderTable.Columns.Contains(column.ColumnName))
                    {
                        //HP_PN
                        if (column.ColumnName == "HP_PN")
                        {
                            List<string> fieldsPN = new List<string>();
                            fieldsPN.Add("distinct HP_PN");
                            string whereHP_PN = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable HP_PNTable = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeA]", whereHP_PN, fieldsPN);
                            if (HP_PNTable.Rows.Count > 1)
                            {
                                newRow[column] = "MIXED";
                            }
                            else
                            {
                                newRow[column] = orderTable.Rows[0][column.ColumnName].ToString();//.Split('/')[1];
                            }
                            continue;
                        }
                        if (column.ColumnName == "PALLET_ID")
                        {
                            List<string> fieldsPLT = new List<string>();
                            fieldsPLT.Add("* ");
                            string wherePallet_ID = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable Pallet_IDTable = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeB]", wherePallet_ID, fieldsPLT);
                            if (Pallet_IDTable.Rows.Count > 1)
                            {
                                newRow[column] = "MIXED";
                            }
                            else
                            {
                                if (Pallet_IDTable.Rows[0]["UCC"].ToString().Trim().Length == 20)
                                {
                                    newRow[column] = Pallet_IDTable.Rows[0]["UCC"].ToString().Trim();

                                }
                                else
                                {
                                    newRow[column] = orderTable.Rows[0][column.ColumnName].ToString();
                                }
                                //  newRow[column] = orderTable.Rows[0][column.ColumnName].ToString();//.Split('/')[1];
                            }
                            continue;
                        }
                        if (column.ColumnName == "PALLET_BOX_QTY")
                        {
                            List<string> fieldsBOXQTY = new List<string>();
                            fieldsBOXQTY.Add("Sum(PALLET_BOX_QTY) as PALLET_BOX_QTY");
                            string whereBOXQTY = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable BOXQTYTable = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeA]", whereBOXQTY, fieldsBOXQTY);
                            newRow[column] = BOXQTYTable.Rows[0]["PALLET_BOX_QTY"];
                            continue;
                        }
                        if (column.ColumnName == "PALLET_UNIT_QTY")
                        {
                            List<string> fieldsUNITQTY = new List<string>();
                            fieldsUNITQTY.Add("Sum(PALLET_UNIT_QTY) as PALLET_UNIT_QTY");
                            string whereUNITQTY = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable UNITQTYTable = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeA]", whereUNITQTY, fieldsUNITQTY);
                            newRow[column] = UNITQTYTable.Rows[0]["PALLET_UNIT_QTY"];
                            continue;
                        }
                        //if table contains name, then populate
                        newRow[column] = orderTable.Rows[0][column.ColumnName];
                    }

                }
                newRow.SHIP_FROM_STREET_2 = "Shapingba district";
                newTable.Rows.Add(newRow);

                //get LABEL_INSTR_HEADER
                string whereLneItemClause_LABEL = "WHERE PO_NUM='" + orderTable.Rows[0]["PO_NUM"].ToString().Trim() + "' and FIELDS = 'LABEL_INSTR_HEAD'";
                DataTable lineItemTable_LABEL = DBFactory.PopulateTempTable("[PAKEDI_INSTR]", whereLneItemClause_LABEL, fields);
                foreach (DataRow readRow4 in lineItemTable_LABEL.Rows)
                {
                    LoadINSTRPairs(palletList.LABEL_INSTR_HEAD,
                        readRow4,
                        "VALUE",
                        "PALLET_Id_0",
                        newRow.PALLET_Id_0);
                }

                //get line item
                string whereLneItemClause = "WHERE SERIAL_NUM='" + palletID + "'";

                DataTable lineTable = DBFactory.PopulateTempTable("[PAKODMSESSION]", whereLneItemClause, fields);
                foreach (DataRow readRow2 in lineTable.Rows)
                {
                    LoadKeyValuePairs(palletList.UDF_HEADER,
                        readRow2,
                        "UDF_KEY_HEADER",
                        "UDF_VALUE_HEADER",
                        "PALLET_Id_0",
                        newRow.PALLET_Id_0);
                    LoadKeyValuePairs(palletList.UDF_DETAIL,
                        readRow2,
                        "UDF_KEY_DETAIL",
                        "UDF_VALUE_DETAIL",
                        "PALLET_Id_0",
                        newRow.PALLET_Id_0);
                }
                // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------

                string whereTwoD = "'" + internalID + "', '" + palletID + "'";
                DataTable TwoDTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution]", whereTwoD);
                string EnCoding = TwoDTable.Rows[0]["EnCoding"].ToString(); // Y/N
                string EnCoder = TwoDTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
                string StringIDValue = "";
                if (EnCoding == "Y")
                {
                    //encoder
                    if (EnCoder.Substring(0,6) == "PDF417")
                   {
                        //PDF417 generator
                        //StringIDValue = TransferCode.Encoder(TwoDTable.Rows[0]["StringIDValue"].ToString());
                        foreach (DataRow readRowtd in TwoDTable.Rows)
                        {
                            PALLETS.UDF_PALLETRow newRowtwod = palletList.UDF_PALLET.NewUDF_PALLETRow();
                            StringIDValue = TransferCode.Encoder(readRowtd["StringIDValue"].ToString().Trim());
                            newRowtwod.KEY = readRowtd["StringIDKey"].ToString().Trim();
                            newRowtwod.VALUE = StringIDValue.ToString().Trim();
                            newRowtwod.PALLET_Id_0 = index;
                            palletList.UDF_PALLET.Rows.Add(newRowtwod);
                        }
                    }
                    else //MaxICode
                  {
                        //MaxICode generator
                    }
                }
                else
                {

                    foreach (DataRow readRowtd in TwoDTable.Rows)
                    {
                        PALLETS.UDF_PALLETRow newRowtwod = palletList.UDF_PALLET.NewUDF_PALLETRow();
                        newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                        newRowtwod.VALUE = readRowtd["StringIDValue"].ToString();
                        newRowtwod.PALLET_Id_0 = index;
                        palletList.UDF_PALLET.Rows.Add(newRowtwod);
                    }

                }
                //----------------------- end -------------------------------------------------------------- 

                index++;
                


            }
            //populate mini tables
            //set this dataset to packinglist so we can outputXML
            this.m_DataSet = palletList;

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

        private void LoadINSTRPairs(DataTable dataTable,
            DataRow readRow, string keyColumnName,
            string keyColumn, int keyValue)
        {
            DataRow newRow = dataTable.NewRow();
            newRow["LABEL_INSTR_HEAD_Column"] = readRow[keyColumnName];
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
