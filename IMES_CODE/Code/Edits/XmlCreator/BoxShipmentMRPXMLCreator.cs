using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using BoxTD;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
    public class BoxShipmentMRPXMLCreator : XmlCreator
    {

#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override bool LoadData(string aKey)
        {
            LoadBoxLabelDatabaseData(aKey);
            return true;
        }
        public void LoadBoxLabelDatabaseData(string id)
        {
            logger.Debug("LoadBoxLabelDatabaseData begin id=" + id);

            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            //string BoxID = idSplit[1];
            string SerialNum = idSplit[1];
            string mrp = idSplit[2];


            //fill boxList table
            BOXES boxList = new BOXES();
            //string whereMainClause = "WHERE InternalID='" + internalID + "' AND BoxID='" + BoxID + "'";            
            /*
            string whereMainClause = "WHERE InternalID='" + internalID + "'";
            List<string> fields = new List<string>();
            fields.Add("*");
            DataTable packIDTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereMainClause, fields);
            */

            logger.Debug("LoadBoxLabelDatabaseData v_Shipment_PAKComn begin");
            DataTable packIDTable = EditsSqlProc.v_Shipment_PAKComn__by__InternalID(internalID);
            logger.Debug("LoadBoxLabelDatabaseData v_Shipment_PAKComn end");

            BOXES.BOXDataTable newTable = boxList.BOX;

            int index = 0;
            foreach (DataRow readRow in packIDTable.Rows)
            {
                BOXES.BOXRow newRow = newTable.NewBOXRow();
                newRow.BOX_Id_0 = index;
                // index++;

                foreach (DataColumn column in newTable.Columns)
                {
                    if (packIDTable.Columns.Contains(column.ColumnName))
                    {
                        //HP_PN
                        if (column.ColumnName == "HP_PN")
                        {
                            newRow[column] = readRow[column.ColumnName].ToString();//.Split('/')[1];
                            continue;
                        }
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
                        //PACK_ID_BOX_QTY
                        if (column.ColumnName == "PACK_ID_BOX_QTY")
                        {
                            newRow[column] = Convert.ToDouble(readRow["PACK_ID_UNIT_QTY"]);
                            continue;
                        }

                        if (column.DataType == typeof(string))
                            newRow[column] = string.Empty;
                        else if (column.DataType == typeof(Double))
                            newRow[column] = 0;
                        else if (column.DataType == typeof(DateTime))
                            newRow[column] = string.Empty;
                    }
                }

                //get serial number
                /*
                string whereBoxClause = "WHERE InternalID='" + internalID + "' AND SERIAL_NUM='" + SerialNum + "'";
                //string whereMainClause = "WHERE InternalID='" + internalID + "' AND BoxID='" + BoxID + "'";            
                DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                */

                ////logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 1 begin");
                ////DataTable boxTable = EditsSqlProc.PAK_PackkingData__by__InternalID__SERIAL_NUM(internalID, SerialNum);
                ////logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 1 end");

                //ChkRowNotNull("BoxLabelShipmentXmlCreator id=" + id + ", PAK_PackkingData internalID=" + internalID + ", SerialNum=" + SerialNum, boxTable);
                ////newRow.BOX_ID = "";
                //get BOX_SEQUENCE
                /*
                List<string> fieldsSEQUENCE = new List<string>();
                fieldsSEQUENCE.Add("count(distinct SERIAL_NUM) as BOX_SEQUENCE");
                string whereBoxSEQUENCE = "WHERE ID<= (select ID from [PAK_PackkingData] where InternalID='" + internalID + "' and SERIAL_NUM='" + SerialNum + "') and left(InternalID,10)='" + internalID.Substring(0, 10) + "'";
                DataTable boxTableSEQUENCE = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxSEQUENCE, fieldsSEQUENCE);
                */

                ////logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 2 begin");
                ////DataTable boxTableSEQUENCE = EditsSqlProc.count__PAK_PackkingData__by__InternalID__SERIAL_NUM__InternalID10(internalID, SerialNum, internalID.Substring(0, 10));
                ////logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 2 end");

                ////ChkRowNotNull("BoxLabelShipmentXmlCreator id=" + id + ", PAK_PackkingData internalID=" + internalID + ", SerialNum=" + SerialNum, boxTableSEQUENCE);
                ////newRow.BOX_SEQUENCE = "";

                //get BOX_UNIT_QTY
                /*
                List<string> fieldsUNTIQTY = new List<string>();
                fieldsUNTIQTY.Add("count(distinct SERIAL_NUM) as BOX_UNIT_QTY");
                string whereBOX_UNIT_QTY = "where BOX_ID = '" + newRow.BOX_ID.ToString() + "'";
                DataTable BoxTableUNITQTY = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBOX_UNIT_QTY, fieldsUNTIQTY);
                */

                //logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 3 begin");
                //DataTable BoxTableUNITQTY = EditsSqlProc.count__PAK_PackkingData__by__BOX_ID(newRow.BOX_ID.ToString());
                //logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 3 end");

                ////ChkRowNotNull("BoxLabelShipmentXmlCreator id=" + id + ", PAK_PackkingData BOX_ID=" + newRow.BOX_ID.ToString(), BoxTableUNITQTY);
                ////newRow.BOX_UNIT_QTY =0;

                newTable.Rows.Add(newRow);

                //get SERIAL_NUMTable
                /*
                string whereSERIALClause = "WHERE BOX_ID = '" + newRow.BOX_ID.ToString().Trim() + "'";
                DataTable SERIALTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereSERIALClause, fields);
                */

                logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 4 begin");
                DataTable SERIALTable = EditsSqlProc.PAK_PackkingData__by__SerialNUM(SerialNum.ToString().Trim());
                logger.Debug("LoadBoxLabelDatabaseData PAK_PackkingData 4 end");

                foreach (DataRow readRow4 in SERIALTable.Rows)
                {
                    LoadSERIAL_NUMPairs(boxList.SERIAL_NUM,
                        readRow4,
                        "SERIAL_NUM",
                        "BOX_ID_0",
                        newRow.BOX_Id_0);
                }

                //get LABEL_INSTR_HEADER
                /*
                string whereLneItemClause_LABEL = "WHERE PO_NUM='" + packIDTable.Rows[0]["PO_NUM"].ToString().Trim() + "' and FIELDS = 'LABEL_INSTR_HEAD'";
                DataTable lineItemTable_LABEL = DBFactory.PopulateTempTable("[PAKEDI_INSTR]", whereLneItemClause_LABEL, fields);
                */

                logger.Debug("LoadBoxLabelDatabaseData PAKEDI_INSTR begin");
                ChkRowNotNull("BoxLabelShipmentXmlCreator id=" + id + ", PAKEDI_INSTR internalID=" + internalID, packIDTable);
                DataTable lineItemTable_LABEL = EditsSqlProc.PAKEDI_INSTR__by__PO_NUM__FIELDS(packIDTable.Rows[0]["PO_NUM"].ToString().Trim());
                logger.Debug("LoadBoxLabelDatabaseData PAKEDI_INSTR end");

                foreach (DataRow readRow4 in lineItemTable_LABEL.Rows)
                {
                    LoadINSTRPairs(boxList.LABEL_INSTR_HEAD,
                        readRow4,
                        "VALUE",
                        "BOX_ID_0",
                        newRow.BOX_Id_0);
                }

                //get line item
                /*
                string whereLneItemClause = "WHERE SERIAL_NUM='" + boxTable.Rows[0]["SERIAL_NUM"].ToString().Trim() + "' order by SHOW_ORDER";
                DataTable lineItemTable = DBFactory.PopulateTempTable("[PAKODMSESSION]", whereLneItemClause, fields);
                */

                logger.Debug("LoadBoxLabelDatabaseData PAKODMSESSION begin");
                //ChkRowNotNull("BoxLabelShipmentXmlCreator id=" + id + ", PAK_PackkingData", boxTable);
                DataTable lineItemTable = EditsSqlProc.PAKODMSESSION__by__SERIAL_NUM_throughFunction(SerialNum.ToString().Trim(), internalID.ToString().Trim());
                logger.Debug("LoadBoxLabelDatabaseData PAKODMSESSION end");

                foreach (DataRow readRow2 in lineItemTable.Rows)
                {
                    LoadKeyValuePairs(boxList.UDF_HEADER,
                        readRow2,
                        "UDF_KEY_HEADER",
                        "UDF_VALUE_HEADER",
                        "BOX_Id_0",
                        newRow.BOX_Id_0);
                    LoadKeyValuePairs(boxList.UDF_DETAIL,
                        readRow2,
                        "UDF_KEY_DETAIL",
                        "UDF_VALUE_DETAIL",
                        "BOX_Id_0",
                        newRow.BOX_Id_0);


                }
                //--------------add  by  Fan 20091119  -------------
                /*
                string whereTwoD = "'" + internalID + "', '" +newRow.BOX_ID.ToString().Trim()+ "'";
                DataTable TwoDTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution_Box]", whereTwoD);
                */

                logger.Debug("LoadBoxLabelDatabaseData op_TwoDCode_Solution_Box begin");
                DataTable TwoDTable = EditsSqlProc.sp__op_TwoDCode_Solution_Box(internalID, newRow.BOX_ID.ToString().Trim());
                logger.Debug("LoadBoxLabelDatabaseData op_TwoDCode_Solution_Box end");

                ChkRowNotNull("BoxLabelShipmentXmlCreator id=" + id + ", op_TwoDCode_Solution_Box internalID=" + internalID, TwoDTable);
                string EnCoding = TwoDTable.Rows[0]["EnCoding"].ToString(); // Y/N
                string EnCoder = TwoDTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
                string StringIDValue = "";
                if (EnCoding == "Y")
                {
                    //encoder
                    if (EnCoder == "PDF417")
                    {
                        //PDF417 generator
                        //StringIDValue = TransferCode.Encoder(TwoDTable.Rows[0]["StringIDValue"].ToString());
                        foreach (DataRow readRowtdbox in TwoDTable.Rows)
                        {
                            BOXES.UDF_BOXRow newRowtwodbox = boxList.UDF_BOX.NewUDF_BOXRow();
                            StringIDValue = TransferCode.Encoder(readRowtdbox["StringIDValue"].ToString());
                            newRowtwodbox.KEY = readRowtdbox["StringIDKey"].ToString();
                            newRowtwodbox.VALUE = StringIDValue.ToString();
                            newRowtwodbox.BOX_Id_0 = index;
                            boxList.UDF_BOX.Rows.Add(newRowtwodbox);
                        }
                    }

                }
                else
                {

                    foreach (DataRow readRowtdbox in TwoDTable.Rows)
                    {
                        BOXES.UDF_BOXRow newRowtwodbox = boxList.UDF_BOX.NewUDF_BOXRow();
                        newRowtwodbox.KEY = readRowtdbox["StringIDKey"].ToString();
                        newRowtwodbox.VALUE = readRowtdbox["StringIDValue"].ToString();
                        newRowtwodbox.BOX_Id_0 = index;
                        boxList.UDF_BOX.Rows.Add(newRowtwodbox);
                    }

                }
                //------------------------------  end   ----------------------------------------
                if (boxList.SERIAL_NUM.Rows.Count > 0)
                    boxList.SERIAL_NUM.Rows[0][0] = mrp;
                index++;

            }
            //populate mini tables
            //set this dataset to packinglist so we can outputXML
            this.m_DataSet = boxList;

            logger.Debug("LoadBoxLabelDatabaseData end");
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

        private void LoadSERIAL_NUMPairs(DataTable dataTable,
            DataRow readRow, string keyColumnName,
            string keyColumn, int keyValue)
        {
            DataRow newRow = dataTable.NewRow();

            newRow["SERIAL_NUM_Column"] = readRow[keyColumnName];
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
