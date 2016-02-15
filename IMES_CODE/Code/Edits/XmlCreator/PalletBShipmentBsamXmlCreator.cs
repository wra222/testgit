using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using BsamPalletB;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
    public class PalletBShipmentBsamXmlCreator : XmlCreator
    {

#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override bool LoadData(string aKey)
        {
            LoadPalletBDatabaseData(aKey);
            return true;
        }
        public void LoadPalletBDatabaseData(string id)
        {
            logger.Debug("LoadPalletBDatabaseData begin id="+id);

            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            string palletID = idSplit[1];
            string internalID10 = internalID.Substring(0, 10) + "%";


            //fill packID table
            //PALLETS palletList = new PALLETS();
            HOUSE_WAYBILL palletList = new HOUSE_WAYBILL();
/*
            //PALLETS.PALLETDataTable newTable = palletList.PALLET;
            HOUSE_WAYBILL.WAYBILLDataTable newTable = palletList.WAYBILL;

            palletList.Tables.Add();

            HOUSE_WAYBILL.WAYBILLRow mainHouseWaybillRow = null;
            */

            logger.Debug("LoadPalletBDatabaseData [v_PAKComn__by__InternalID] begin");
            DataTable commTable = EditsSqlProc.v_PAKComn__by__InternalID(internalID);
            DataRow commRow = null;
            logger.Debug("LoadPalletBDatabaseData [v_PAKComn__by__InternalID] end");
            if (commTable.Rows.Count == 0)
            {
                this.m_DataSet = palletList;
                logger.Debug("LoadPalletBDatabaseData end. no v_PAKComn__by__InternalID");
                return;
            }
            else
            {
                commRow = commTable.Rows[0];
/*                mainHouseWaybillRow = newTable.NewWAYBILLRow();
                mainHouseWaybillRow.WAYBILL_Id = 0;

                DataRow readRow = commRow;
                foreach (DataColumn column in newTable.Columns)
                {
                    if (commTable.Columns.Contains(column.ColumnName))
                    {
                        //HP_PN
                        if (column.ColumnName == "HP_PN")
                        {
                            mainHouseWaybillRow[column] = readRow[column.ColumnName].ToString();//.Split('/')[1];
                            continue;
                        }
                        //if table contains name, then populate
                        if (column.DataType != commTable.Columns[column.ColumnName].DataType)
                        {
                            if (column.DataType == typeof(Double))
                            {
                                mainHouseWaybillRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                            }
                            else if (column.DataType == typeof(String))
                            {
                                mainHouseWaybillRow[column] = Convert.ToString(readRow[column.ColumnName]);
                            }
                        }
                        else
                            mainHouseWaybillRow[column] = readRow[column.ColumnName];
                    }
                    else
                    {
                        if (column.DataType == typeof(string))
                            mainHouseWaybillRow[column] = string.Empty;
                        else if (column.DataType == typeof(Double))
                            mainHouseWaybillRow[column] = 0;
                        else if (column.DataType == typeof(DateTime))
                            mainHouseWaybillRow[column] = string.Empty;
                    }
                }
                */
            }


            DataTable BoxTableHAWB = null;
            string C46 = commRow["C46"] as string;
            if (string.IsNullOrEmpty(C46))
                C46 = commRow["WAYBILL_NUMBER"] as string;
/*            
            // HAWB_PALLET_QTY
            //mainHouseWaybillRow["HAWB_PALLET_QTY"] = 0;
            logger.Debug("LoadPalletBDatabaseData HAWB_PALLET_QTY__by__C46__PalletBShipmentBsam C46 begin");
            BoxTableHAWB = EditsSqlProc.HAWB_PALLET_QTY__by__C46__PalletBShipmentBsam(C46);
            logger.Debug("LoadPalletBDatabaseData HAWB_PALLET_QTY__by__C46__PalletBShipmentBsam C46 end");
            double HAWB_PALLET_QTY = Convert.ToDouble(BoxTableHAWB.Rows[0]["HAWB_PALLET_QTY"].ToString());
            mainHouseWaybillRow["HAWB_PALLET_QTY"] = HAWB_PALLET_QTY;

            // HAWB_BOX_QTY
            //mainHouseWaybillRow["HAWB_BOX_QTY"] = 0;
            logger.Debug("LoadPalletBDatabaseData HAWB_BOX_QTY__by__C46__PalletBShipmentBsam C46 begin");
            BoxTableHAWB = EditsSqlProc.HAWB_BOX_QTY__by__C46__PalletBShipmentBsam(C46);
            logger.Debug("LoadPalletBDatabaseData HAWB_BOX_QTY__by__C46__PalletBShipmentBsam C46 end");
            double HAWB_BOX_QTY = Convert.ToDouble(BoxTableHAWB.Rows[0]["HAWB_BOX_QTY"].ToString());
            mainHouseWaybillRow["HAWB_BOX_QTY"] = HAWB_BOX_QTY;

            // HAWB_UNIT_QTY
            //mainHouseWaybillRow["HAWB_UNIT_QTY"] = 0;
            logger.Debug("LoadPalletBDatabaseData HAWB_UNIT_QTY__by__C46__PalletBShipmentBsam C46 begin");
            BoxTableHAWB = EditsSqlProc.HAWB_UNIT_QTY__by__C46__PalletBShipmentBsam(C46);
            logger.Debug("LoadPalletBDatabaseData HAWB_UNIT_QTY__by__C46__PalletBShipmentBsam C46 end");
            double HAWB_UNIT_QTY = Convert.ToDouble(BoxTableHAWB.Rows[0]["HAWB_UNIT_QTY"].ToString());
            mainHouseWaybillRow["HAWB_UNIT_QTY"] = HAWB_UNIT_QTY;

            //test
            mainHouseWaybillRow["HAWB_ACT_WEIGHT"] = 0;
            mainHouseWaybillRow["HAWB_GROSS_WEIGHT"] = 0;

            newTable.Rows.Add(mainHouseWaybillRow);
            */


            //mainHouseWaybillRow.GetPALLETRows
            HOUSE_WAYBILL.PALLETDataTable palletDataTable = palletList.PALLET;
            HOUSE_WAYBILL.PALLETRow palletRow = null;

            logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKPalletTypeB] begin");
            DataTable palletTable = EditsSqlProc.v_Shipment_PAKPalletTypeB__by__InternalID__PALLET_ID(internalID, palletID);
            logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKPalletTypeB] end");
            if (palletTable.Rows.Count == 0)
            {
                this.m_DataSet = palletList;
                logger.Debug("LoadPalletBDatabaseData end. no v_Shipment_PAKPalletTypeB");
                return;
            }
            else
            {
                palletRow = palletDataTable.NewPALLETRow();
/*                palletRow.WAYBILL_Id = 0;
                palletRow.WAYBILLRow = mainHouseWaybillRow;*/
                palletRow.PALLET_Id_0 = 0;

                DataRow readRow = palletTable.Rows[0];
                foreach (DataColumn column in palletDataTable.Columns)
                {
                    if (palletTable.Columns.Contains(column.ColumnName))
                    {
                        //HP_PN
                        if (column.ColumnName == "HP_PN")
                        {
                            palletRow[column] = readRow[column.ColumnName].ToString();//.Split('/')[1];
                            continue;
                        }
                        //if table contains name, then populate
                        if (column.DataType != palletTable.Columns[column.ColumnName].DataType)
                        {
                            if (column.DataType == typeof(Double))
                            {
                                palletRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                            }
                            else if (column.DataType == typeof(String))
                            {
                                palletRow[column] = Convert.ToString(readRow[column.ColumnName]);
                            }
                        }
                        else
                            palletRow[column] = readRow[column.ColumnName];
                    }
                    else
                    {
                        if (column.DataType == typeof(string))
                            palletRow[column] = string.Empty;
                        else if (column.DataType == typeof(Double))
                            palletRow[column] = 0;
                        else if (column.DataType == typeof(DateTime))
                            palletRow[column] = string.Empty;
                    }
                }

            }


            // PALLET_UNIT_QTY
            //palletRow["PALLET_UNIT_QTY"] = 9;
            logger.Debug("LoadPalletBDatabaseData PALLET_UNIT_QTY__by__PALLET_ID__PalletBShipmentBsam PALLET_ID begin");
            BoxTableHAWB = EditsSqlProc.PALLET_UNIT_QTY__by__PALLET_ID__PalletBShipmentBsam(palletID);
            logger.Debug("LoadPalletBDatabaseData PALLET_UNIT_QTY__by__PALLET_ID__PalletBShipmentBsam PALLET_ID end");
            double PALLET_UNIT_QTY = Convert.ToDouble(BoxTableHAWB.Rows[0]["PALLET_UNIT_QTY"].ToString());
            palletRow["PALLET_UNIT_QTY"] = PALLET_UNIT_QTY;

            palletDataTable.AddPALLETRow(palletRow);



            // PACK_ID_BOX_QTY
            logger.Debug("LoadPalletBDatabaseData  SERIAL_NUM,BOX_ID begin");
            BoxTableHAWB = EditsSqlProc.count_BOX_ID__PAK_PackkingData(internalID10);
            logger.Debug("LoadPalletBDatabaseData  SERIAL_NUM,BOX_ID end");
            double PACK_ID_BOX_QTY = Convert.ToDouble(BoxTableHAWB.Rows[0]["PACK_ID_BOX_QTY"].ToString());



            HOUSE_WAYBILL.BOXDataTable boxDataTable = palletList.BOX;
            logger.Debug("LoadPalletBDatabaseData [distinct_BOX_ID__PAK_PackkingData] begin");
            DataTable boxTable = EditsSqlProc.distinct_BOX_ID__PAK_PackkingData(palletID);
            logger.Debug("LoadPalletBDatabaseData [distinct_BOX_ID__PAK_PackkingData] end");
            if (boxTable.Rows.Count == 0)
            {
                this.m_DataSet = palletList;
                logger.Debug("LoadPalletBDatabaseData end. no distinct_BOX_ID__PAK_PackkingData");
                return;
            }
            int indexbox = 0;
            foreach (DataRow boxidRow in boxTable.Rows)
            {
                HOUSE_WAYBILL.BOXRow boxRow = boxDataTable.NewBOXRow();
                boxRow.BOX_Id_0 = indexbox;
                boxRow.PALLET_Id_0 = 0;
                boxRow.PALLETRow = palletRow;

                string box_id = boxidRow["BOX_ID"] as string;

                DataRow readRow = commRow;
                foreach (DataColumn column in boxDataTable.Columns)
                {
                    if (commTable.Columns.Contains(column.ColumnName))
                    {
                        //HP_PN
                        if (column.ColumnName == "HP_PN")
                        {
                            boxRow[column] = readRow[column.ColumnName].ToString();//.Split('/')[1];
                            continue;
                        }
                        //if table contains name, then populate
                        if (column.DataType != commTable.Columns[column.ColumnName].DataType)
                        {
                            if (column.DataType == typeof(Double))
                            {
                                boxRow[column] = Convert.ToDouble(readRow[column.ColumnName]);
                            }
                            else if (column.DataType == typeof(String))
                            {
                                boxRow[column] = Convert.ToString(readRow[column.ColumnName]);
                            }
                        }
                        else
                            boxRow[column] = readRow[column.ColumnName];
                    }
                    else
                    {
                        if (column.DataType == typeof(string))
                            boxRow[column] = string.Empty;
                        else if (column.DataType == typeof(Double))
                            boxRow[column] = 0;
                        else if (column.DataType == typeof(DateTime))
                            boxRow[column] = string.Empty;
                    }
                }

                boxRow["BOX_ID"] = box_id;

                // BOX_UNIT_QTY
                logger.Debug("LoadPalletBDatabaseData  BOX_ID begin");
                DataTable BoxTableUNITQTY = EditsSqlProc.count__PAK_PackkingData__by__BOX_ID(box_id);
                logger.Debug("LoadPalletBDatabaseData  BOX_ID end");
                boxRow["BOX_UNIT_QTY"] = Convert.ToDouble( BoxTableUNITQTY.Rows[0]["BOX_UNIT_QTY"].ToString() );

                // PACK_ID_BOX_QTY
                boxRow["PACK_ID_BOX_QTY"] = PACK_ID_BOX_QTY;

                //test
                boxRow["BOX_WEIGHT"] = 0;
                
                boxDataTable.AddBOXRow(boxRow);
                indexbox++;
            }



            //populate mini tables
            //set this dataset to packinglist so we can outputXML
            this.m_DataSet = palletList;
            
            logger.Debug("LoadPalletBDatabaseData end");
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
