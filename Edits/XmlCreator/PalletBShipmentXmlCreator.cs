using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using PalletTDB;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
    public class PalletBShipmentXmlCreator : XmlCreator
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




            //fill packID table
            PALLETS palletList = new PALLETS();


            /*
            string whereMainClause = "WHERE InternalID='" + internalID + "' AND PALLET_ID='" + palletID + "'";
            List<string> fields = new List<string>();
            fields.Add("*");
            DataTable palletTable = DBFactory.PopulateTempTable("[v_Shipment_PAKPalletTypeB]", whereMainClause, fields);
            */

            logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKPalletTypeB] begin");
            DataTable palletTable = EditsSqlProc.v_Shipment_PAKPalletTypeB__by__InternalID__PALLET_ID(internalID, palletID);
            logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKPalletTypeB] end");

            PALLETS.PALLETDataTable newTable = palletList.PALLET;
            int index = 0;
            int index2 = 0;
            int indexbox = 0;
            int indexpn = 0;     //add by Duan
            string tempStr = "";  //add by Duan
            foreach (DataRow readRow in palletTable.Rows)
            {
                PALLETS.PALLETRow newRow = newTable.NewPALLETRow();
                newRow.PALLET_Id_0 = index;

                //index++;
                foreach (DataColumn column in newTable.Columns)
                {


                    if (palletTable.Columns.Contains(column.ColumnName))
                    {
                        //PALLET_ID
                        if (column.ColumnName == "PALLET_ID")
                        {
                            //ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_Shipment_PAKPalletTypeB internalID=" + internalID + ", palletID=" + palletID, palletTable);
                            if ((palletTable.Rows.Count >= 1) && palletTable.Rows[0]["UCC"].ToString().Trim().Length == 20)
                            {
                                newRow[column] = palletTable.Rows[0]["UCC"].ToString().Trim();
                            }
                            else
                            {
                                newRow[column] = readRow[column.ColumnName];
                            }
                        }
                        //WAYBILL_NUMBER
                        else if (column.ColumnName == "WAYBILL_NUMBER")
                        {


                            /*
                            string whereMainClauseRegion = "WHERE InternalID='" + internalID + "'";
                            List<string> fieldsregion = new List<string>();
                            fieldsregion.Add("REGION");
                            fieldsregion.Add("CUSTOMER_ID");
                            fieldsregion.Add("SHIP_MODE");
                            DataTable RegionTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereMainClauseRegion, fieldsregion);
                            */

                            logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKComn] begin");
                            DataTable RegionTable = EditsSqlProc.REGION__CUSTOMER_ID__SHIP_MODE__from__v_Shipment_PAKComn__by__InternalID(internalID);
                            logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKComn] end");

                            ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn internalID=" + internalID, RegionTable);
                            if (RegionTable.Rows[0]["REGION"].ToString().Trim() == "AP")
                            {
                                newRow[column] = string.Empty;
                            }
                            else if (RegionTable.Rows[0]["REGION"].ToString().Trim() == "LA")
                            {
                                //if (RegionTable1.Rows[0]["CUSTOMERID"].ToString().Trim() == "LA_CUSTOMER")// LA TO LA
                                /*
                                string sqlCmd = "WHERE PALLET_ID='" + palletID + "'";
                                List<string> fieldsPackID = new List<string>();
                                fieldsPackID.Add("PACK_ID");
                                fieldsPackID.Add("WAYBILL_NUMBER");
                                DataTable PackIDTable = DBFactory.PopulateTempTable("[v_PAKPalletTypeBLineItem]", sqlCmd, fieldsPackID);
                                */

                                logger.Debug("LoadPalletBDatabaseData [v_PAKPalletTypeBLineItem] begin");
                                DataTable PackIDTable = EditsSqlProc.PACK_ID__WAYBILL_NUMBER__from__v_PAKPalletTypeBLineItem__by__PALLET_ID(palletID);
                                logger.Debug("LoadPalletBDatabaseData [v_PAKPalletTypeBLineItem] end");

                                if (RegionTable.Rows[0]["CUSTOMER_ID"].ToString().Trim() != "0590000015" &&
                                      RegionTable.Rows[0]["CUSTOMER_ID"].ToString().Trim() != "0590000024")// LA TO LA
                                {
                                    //string sqlCmd=" select count(*)  from v_PAKPalletTypeBLineItem  where PALLET_ID='"+palletID+"'";
                                    newRow[column] = readRow[column.ColumnName];

                                        //newRow[column] = string.Empty;
                                        //continue;
                                        //LA空運的PO  waybill位置設置為已經分配好的值.對於海運的PO,需要FIS產生符合HP要求的虛擬值填寫進去
                                        if (RegionTable.Rows[0]["SHIP_MODE"].ToString().Trim().ToUpper() == "OCEAN")
                                        {
                                            /*
                                            string whereLAOceanWaybill = "'" + internalID + "'";
                                            DataTable LAOceanWaybillTable = DBFactory.PopulateTempTable_BySp("op_GetLAOceanWaybill", whereLAOceanWaybill);
                                            */
                                            logger.Debug("LoadPalletBDatabaseData op_GetLAOceanWaybill begin");
                                            DataTable LAOceanWaybillTable = EditsSqlProc.sp__op_GetLAOceanWaybill(internalID);
                                            logger.Debug("LoadPalletBDatabaseData op_GetLAOceanWaybill end");

                                            ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", op_GetLAOceanWaybill internalID=" + internalID, LAOceanWaybillTable);
                                            newRow[column] = LAOceanWaybillTable.Rows[0]["waybill"].ToString();
                                            continue;
                                        }
                                        else
                                        {
                                            ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_PAKPalletTypeBLineItem palletID=" + palletID, PackIDTable);
                                            newRow[column] = PackIDTable.Rows[0]["WAYBILL_NUMBER"].ToString();
                                            continue;
                                        }
                                }
                                else// ToNALA
                                {
                                    ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_PAKPalletTypeBLineItem palletID=" + palletID, PackIDTable);
                                    newRow[column] = PackIDTable.Rows[0]["WAYBILL_NUMBER"].ToString();
                                    continue;
                                }
                            }
                            else
                            {
                                newRow[column] = readRow[column.ColumnName];
                            }

                        }
                        //if table contains name, then populate
                        else
                        {
                            newRow[column] = readRow[column.ColumnName];
                        }
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

                newTable.Rows.Add(newRow);



                PALLETS.PACK_ID_LINE_ITEMDataTable lineTable = palletList.PACK_ID_LINE_ITEM;
                /*
                string whereOrderClause = "'" + palletID.Trim() + "'";
                DataTable orderTable = DBFactory.PopulateTempTable_BySp("[op_Shipment_PAKPalletTypeBLineItem]", whereOrderClause);
               // int index2 = 0;
                */

                logger.Debug("LoadPalletBDatabaseData [op_Shipment_PAKPalletTypeBLineItem] begin");
                DataTable orderTable = EditsSqlProc.sp__op_Shipment_PAKPalletTypeBLineItem(palletID.Trim());
                logger.Debug("LoadPalletBDatabaseData [op_Shipment_PAKPalletTypeBLineItem] end");

                //---------------add 3 parameters by lck 20120305----------------------
                int pnQty = 0;
                string prePn = default(string);
                string nowPn = default(string); 
                //--------------------------------------------------------------------


                foreach (DataRow readRow3 in orderTable.Rows)
                {
                    //int index2 = 0;
                    //  int indexbox = 0;
                    //add by Duan
                    foreach (DataColumn columnPN in orderTable.Columns)
                    {
                        if (columnPN.ColumnName == "HP_PN")
                        {
                            if (readRow3[columnPN.ColumnName].ToString() != tempStr)
                            {
                                indexpn++;
                            }
                            tempStr = readRow3[columnPN.ColumnName].ToString();
                        }
                    }
                    //add by Duan
                    foreach (DataColumn column in newTable.Columns)
                    {
                        if (orderTable.Columns.Contains(column.ColumnName))
                        {
                            //HP_PN
                            if (column.ColumnName == "HP_PN")
                            {
                                newRow[column] = readRow3[column.ColumnName].ToString();//.Split('/')[1];
                                continue;
                            }
 
                            //PALLET_ID
                            if (column.ColumnName == "PALLET_ID")
                            {
                                ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", op_Shipment_PAKPalletTypeBLineItem palletID=" + palletID.Trim(), orderTable);
                                if (orderTable.Rows[0]["UCC"].ToString().Trim().Length == 20)
                                {
                                    newRow[column] = orderTable.Rows[0]["UCC"].ToString().Trim();
                                    continue;
                                }
                            }
                            //WAYBILL_NUMBER
                            if (column.ColumnName == "WAYBILL_NUMBER")
                            {
                                /*
                                string whereMainClauseRegion = "WHERE InternalID='" + internalID + "'";
                                List<string> fieldsregion = new List<string>();
                                fieldsregion.Add("REGION");
                                fieldsregion.Add("CUSTOMER_ID");
                                fieldsregion.Add("SHIP_MODE");
                                DataTable RegionTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereMainClauseRegion, fieldsregion);
                                */

                                logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKComn] begin");
                                DataTable RegionTable = EditsSqlProc.REGION__CUSTOMER_ID__SHIP_MODE__from__v_Shipment_PAKComn__by__InternalID(internalID);
                                logger.Debug("LoadPalletBDatabaseData [v_Shipment_PAKComn] end");

                                ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_Shipment_PAKComn internalID=" + internalID, RegionTable);
                                if (RegionTable.Rows[0]["REGION"].ToString().Trim() == "AP")
                                {
                                    newRow[column] = string.Empty;
                                }
                                else if (RegionTable.Rows[0]["REGION"].ToString().Trim() == "LA")
                                {
                                    //if (RegionTable1.Rows[0]["CUSTOMERID"].ToString().Trim() == "LA_CUSTOMER")// LA TO LA
                                    /*
                                    string sqlCmd = "WHERE PALLET_ID='" + palletID + "'";
                                    List<string> fieldsPackID = new List<string>();
                                    fieldsPackID.Add("PACK_ID");
                                    fieldsPackID.Add("WAYBILL_NUMBER");
                                    DataTable PackIDTable = DBFactory.PopulateTempTable("[v_PAKPalletTypeBLineItem]", sqlCmd, fieldsPackID);
                                    */

                                    logger.Debug("LoadPalletBDatabaseData [v_PAKPalletTypeBLineItem] begin");
                                    DataTable PackIDTable = EditsSqlProc.PACK_ID__WAYBILL_NUMBER__from__v_PAKPalletTypeBLineItem__by__PALLET_ID(palletID);
                                    logger.Debug("LoadPalletBDatabaseData [v_PAKPalletTypeBLineItem] end");

                                    if (RegionTable.Rows[0]["CUSTOMER_ID"].ToString().Trim() != "0590000015" &&
                                          RegionTable.Rows[0]["CUSTOMER_ID"].ToString().Trim() != "0590000024")// LA TO LA
                                    {
                                        //string sqlCmd=" select count(*)  from v_PAKPalletTypeBLineItem  where PALLET_ID='"+palletID+"'";
                                        newRow[column] = readRow[column.ColumnName];


                                            //newRow[column] = string.Empty;
                                            //continue;
                                            //LA空運的PO  waybill位置設置為已經分配好的值.對於海運的PO,需要FIS產生符合HP要求的虛擬值填寫進去
                                            if (RegionTable.Rows[0]["SHIP_MODE"].ToString().Trim().ToUpper() == "OCEAN")
                                            {
                                                /*
                                                string whereLAOceanWaybill = "'" + internalID + "'";
                                                DataTable LAOceanWaybillTable = DBFactory.PopulateTempTable_BySp("op_GetLAOceanWaybill", whereLAOceanWaybill);
                                                */

                                                logger.Debug("LoadPalletBDatabaseData op_GetLAOceanWaybill begin");
                                                DataTable LAOceanWaybillTable = EditsSqlProc.sp__op_GetLAOceanWaybill(internalID);
                                                logger.Debug("LoadPalletBDatabaseData op_GetLAOceanWaybill end");

                                                ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", op_GetLAOceanWaybill internalID=" + internalID, LAOceanWaybillTable);
                                                newRow[column] = LAOceanWaybillTable.Rows[0]["waybill"].ToString();
                                                continue;
                                            }
                                            else
                                            {
                                                ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_PAKPalletTypeBLineItem palletID=" + palletID, PackIDTable);
                                                newRow[column] = PackIDTable.Rows[0]["WAYBILL_NUMBER"].ToString();
                                                continue;
                                            }
                                    }
                                    else// ToNANA
                                    {
                                        ChkRowNotNull("PalletBShipmentXmlCreator id=" + id + ", v_PAKPalletTypeBLineItem palletID=" + palletID, PackIDTable);
                                        newRow[column] = PackIDTable.Rows[0]["WAYBILL_NUMBER"].ToString();
                                        continue;
                                    }
                                }
                                else
                                {
                                    newRow[column] = readRow[column.ColumnName];
                                }

                            }
                            //if table contains name, then populate
                            newRow[column] = readRow3[column.ColumnName];
                        }

                    }
                    //line item
                    PALLETS.PACK_ID_LINE_ITEMRow newItemRow = lineTable.NewPACK_ID_LINE_ITEMRow();
                    newItemRow.PACK_ID_LINE_ITEM_Id = index2;
                    //index2++;
                    foreach (DataColumn column in lineTable.Columns)
                    {
                        if (orderTable.Columns.Contains(column.ColumnName))
                        {
                            //HP_PN
                            if (column.ColumnName == "HP_PN" || column.ColumnName == "PACK_ID_LINE_ITEM_UNIT_QTY_PER_PALLET"
                                    || column.ColumnName == "PACK_ID_LINE_ITEM_BOX_QTY_PER_PALLET")
                            {
                                newItemRow[column] = readRow3[column.ColumnName].ToString();//.Split('/')[1];
                                //-----------add by lck 20120305---------------------------
                                if (column.ColumnName == "HP_PN")
                                {                                    
                                    nowPn = readRow3[column.ColumnName].ToString();

                                    if (nowPn != prePn)
                                    {
                                        pnQty++;
                                    }
                                    prePn = nowPn;                                    
                                }
                                //---------------------------------------------------------
                                continue;
                            }

                            //if table contains name, then populate
                            newItemRow[column] = readRow3[column.ColumnName];

                        }
                        else
                        {
                            if (column.DataType == typeof(string))
                                newItemRow[column] = string.Empty;
                            else if (column.DataType == typeof(Double))
                                newItemRow[column] = 0;
                            else if (column.DataType == typeof(DateTime))
                                newItemRow[column] = string.Empty;
                        }

                    }
                    newItemRow.PALLET_Id_0 = newRow.PALLET_Id_0;
                    lineTable.Rows.Add(newItemRow);
                    //-------------  add  by  fan   20090920----Nov go live-------------
                    //int indexbox = 0;





                    if (index2>= orderTable.Rows.Count)
                      break;
                    /*
                    string whereBoxClause = "'" + orderTable.Rows[index2]["PACK_ID"] + "','" + orderTable.Rows[0]["PALLET_ID"] + "','','1'";
                    DataTable boxorderTable = DBFactory.PopulateTempTable_BySp("op_Shipment_PACKBox", whereBoxClause);
                    */

                    logger.Debug("LoadPalletBDatabaseData op_Shipment_PACKBox begin");
                    DataTable boxorderTable = EditsSqlProc.sp__op_Shipment_PACKBox(orderTable.Rows[index2]["PACK_ID"].ToString(), orderTable.Rows[0]["PALLET_ID"].ToString(), "", "1");
                    logger.Debug("LoadPalletBDatabaseData op_Shipment_PACKBox end");

                    int indexbox2 = 0;
                    foreach (DataRow box in boxorderTable.Rows)
                    {
                       // if (indexbox>= boxorderTable.Rows.Count)
                        //    break;

                        PALLETS.BOXRow boxlist = palletList.BOX.NewBOXRow();
                        boxlist.BOX_ID = box["BOX_ID"].ToString();
                        boxlist.BOX_UNIT_QTY = Convert.ToDouble(box["BOX_UNIT_QTY"]);
                        boxlist.BOX_WEIGHT = Convert.ToDouble(box["BOX_WEIGHT"]);
                        boxlist.TRACK_NO_PARCEL = box["TRACK_NO_PARCEL"].ToString();
                        boxlist.BOX_WEIGHT_UOM = box["BOX_WEIGHT_UOM"].ToString();
                        boxlist.PACK_ID_LINE_ITEM_Id = index2;
                        boxlist.BOX_Id_0 = indexbox;
                        palletList.BOX.Rows.Add(boxlist);



                        /*
                        string whereSerClause = "'" + orderTable.Rows[index2]["PACK_ID"] + "','" + orderTable.Rows[0]["PALLET_ID"] + "','" + boxorderTable.Rows[indexbox2]["BOX_ID"] + "','2'";
                        DataTable serialtable = DBFactory.PopulateTempTable_BySp("op_Shipment_PACKBox", whereSerClause);
                        */

                        logger.Debug("LoadPalletBDatabaseData op_Shipment_PACKBox begin");
                        DataTable serialtable = EditsSqlProc.sp__op_Shipment_PACKBox(orderTable.Rows[index2]["PACK_ID"].ToString(), orderTable.Rows[0]["PALLET_ID"].ToString(), boxorderTable.Rows[indexbox2]["BOX_ID"].ToString(), "2");
                        logger.Debug("LoadPalletBDatabaseData op_Shipment_PACKBox end");

                        foreach (DataRow serial in serialtable.Rows)
                        {
                            PALLETS.SERIAL_NUMRow newserial = palletList.SERIAL_NUM.NewSERIAL_NUMRow();
                            newserial.SERIAL_NUM_Column = serial["SERIAL_NUM"].ToString().Trim();
                            newserial.BOX_Id_0 = indexbox;
                            palletList.SERIAL_NUM.Rows.Add(newserial);
                        }
                        indexbox++;
                        indexbox2++;
                    }

                    //----------------------------20090920 end------------------------

                    //get LABEL_INSTR_HEADER
                    /*
                    string whereLneItemClause_LABEL = "WHERE PO_NUM='" + readRow3["PO_NUM"].ToString().Trim() + "' and FIELDS = 'LABEL_INSTR_HEAD'";
                    DataTable lineItemTable_LABEL = DBFactory.PopulateTempTable("[PAKEDI_INSTR]", whereLneItemClause_LABEL, fields);
                    */

                    logger.Debug("LoadPalletBDatabaseData [PAKEDI_INSTR] begin");
                    DataTable lineItemTable_LABEL = EditsSqlProc.PAKEDI_INSTR__by__PO_NUM__FIELDS(readRow3["PO_NUM"].ToString().Trim());
                    logger.Debug("LoadPalletBDatabaseData [PAKEDI_INSTR] end");

                    foreach (DataRow readRow4 in lineItemTable_LABEL.Rows)
                    {
                        LoadINSTRPairs(palletList.LABEL_INSTR_HEAD,
                            readRow4,
                            "VALUE",
                            "PALLET_Id_0",
                            newRow.PALLET_Id_0);
                    }

                    index2++;
                }

                if (index2 > 1 && pnQty > 1 && indexpn >1)   //modify by Duan
                {
                    PALLETS.UDF_PALLETRow newRowtwod = palletList.UDF_PALLET.NewUDF_PALLETRow();
                    newRowtwod.KEY = "KEY_PALLET_MIXED_PN";
                    newRowtwod.VALUE = "Y";
                    newRowtwod.PALLET_Id_0 = index;
                    palletList.UDF_PALLET.Rows.Add(newRowtwod);
                }
                else
                {
                    PALLETS.UDF_PALLETRow newRowtwod1 = palletList.UDF_PALLET.NewUDF_PALLETRow();
                    newRowtwod1.KEY = "KEY_PALLET_MIXED_PN";
                    newRowtwod1.VALUE = "N";
                    newRowtwod1.PALLET_Id_0 = index;
                    palletList.UDF_PALLET.Rows.Add(newRowtwod1);
                }
                index++;
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
