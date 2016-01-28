using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using HOUSE_WAYBILLS_001_20130124;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
	/// <summary>
	/// HouseWaybills Xml Creator
	/// </summary>
	public class HouseWaybillsXmlCreator : XmlCreator
	{
          
#if DEBUG
        private static NLog.Logger m_Nlogger    = NLog.LogManager.GetCurrentClassLogger();
#endif
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override bool LoadData(string aKey)
        {
            LoadHouseWaybillsDatabaseData(aKey);
            return true;
        }
        public void LoadHouseWaybillsDatabaseData(string id)
        {
            logger.Debug("LoadHouseWaybillsDatabaseData begin id="+id);

			string Consolidate = "";
            //fill boxList table
            //WAYBILL_ADDITION wayBillAddition = new WAYBILL_ADDITION();
            HOUSE_WAYBILLS houseWaybillsList = new HOUSE_WAYBILLS();

            logger.Debug("LoadHouseWaybillsDatabaseData [v_PAKComn] begin");
            DataTable ConsolidateTable = EditsSqlProc.v_PAKComn__by__InternalID(id);
            logger.Debug("LoadHouseWaybillsDatabaseData [v_PAKComn] end");

            ChkRowNotNull("WayBillXmlCreator id=" + id + ", v_PAKComn internalID=" + id, ConsolidateTable);
            Consolidate = ConsolidateTable.Rows[0]["CONSOL_INVOICE"].ToString().Trim();
            string waybill_number = ConsolidateTable.Rows[0]["WAYBILL_NUMBER"].ToString().Trim();
            string HAWB = waybill_number;

            if (string.IsNullOrEmpty(waybill_number))
            {
                throw new Exception(id + " ªº WAYBILL_NUMBER ¬°ªÅ");
            }
			
			/*if (Consolidate.Trim() != "")
            {*/
				logger.Debug("LoadHouseWaybillsDatabaseData [v_WayBillList] begin");
                DataTable packIDTable = EditsSqlProc.v_PAKComn__by__waybill_number(HAWB);
				//SQLText = "select distinct SUB_REGION, MASTER_WAYBILL_NUMBER, WAYBILL_NUMBER, INTL_CARRIER, ACTUAL_SHIPDATE from [v_WayBillList] where C46=@C46";
				logger.Debug("LoadHouseWaybillsDatabaseData [v_WayBillList] end");

				int index = 0;
				//foreach (DataRow readRow in packIDTable.Rows)
                if ((packIDTable != null) && (packIDTable.Rows.Count > 0))
				{
                    DataRow readRow = packIDTable.Rows[0];

                    HOUSE_WAYBILLS.HOUSE_WAYBILLRow newRow = houseWaybillsList.HOUSE_WAYBILL.NewHOUSE_WAYBILLRow();
					//newRow.HOUSE_WAYBILL_Id = index;

					foreach (DataColumn column in houseWaybillsList.HOUSE_WAYBILL.Columns)
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

                    DataTable hawbTable = null;

                    /*
					logger.Debug("LoadHouseWaybillsDatabaseData [op_PAK_ShipmentWeight_FIS] begin");
					DataTable hawbTable_ACT = EditsSqlProc.sp__op_PAK_ShipmentWeight_FIS(HAWB, "3");
					logger.Debug("LoadHouseWaybillsDatabaseData [op_PAK_ShipmentWeight_FIS] end");
					ChkRowNotNull("WayBillXmlCreator id=" + id + ", op_PAK_ShipmentWeight_FIS shipment=" + HAWB + ", tp=3", hawbTable_ACT);
					newRow.HAWB_ACT_WEIGHT = double.Parse(hawbTable_ACT.Rows[0]["Weight"].ToString());
                    */
                    newRow.HAWB_ACT_WEIGHT = 0;

                    //
                    logger.Debug("LoadHouseWaybillsDatabaseData [op_PAK_ShipmentWeight_FIS] begin");
                    hawbTable = EditsSqlProc.sp__op_PAK_ShipmentWeight_FIS(HAWB, "4");
                    logger.Debug("LoadHouseWaybillsDatabaseData [op_PAK_ShipmentWeight_FIS] end");
                    ChkRowNotNull("WayBillXmlCreator id=" + id + ", op_PAK_ShipmentWeight_FIS shipment=" + HAWB + ", tp=4", hawbTable);
                    newRow.HAWB_GROSS_WEIGHT = double.Parse(hawbTable.Rows[0]["Weight"].ToString());

					logger.Debug("LoadHouseWaybillsDatabaseData [v_WayBillList] begin");
                    string CONSOL_INVOICE = readRow["CONSOL_INVOICE"].ToString();
                    CONSOL_INVOICE = CONSOL_INVOICE.Substring(0, 10);
                    hawbTable = EditsSqlProc.sum_DEST_CODE_BOX_QTY__from__v_WayBillList__by__CONSOL_INVOICE(CONSOL_INVOICE);
					logger.Debug("LoadHouseWaybillsDatabaseData [v_WayBillList] end");
                    ChkRowNotNull("WayBillXmlCreator id=" + id + ", v_WayBillList CONSOL_INVOICE=" + CONSOL_INVOICE, hawbTable);

					//add up PACK_ID_UNIT_QTY
					//newRow.HAWB_BOX_QTY = double.Parse(hawbTable.Rows[0]["DEST_CODE_BOX_QTY"].ToString());

					logger.Debug("LoadHouseWaybillsDatabaseData [PAK.PAKPaltno] begin");
                    DataTable hawb2Table = EditsSqlProc.PALLET_ID__from__PAK_PAKPaltno__by__WAYBILL_NUMBER(HAWB);
					logger.Debug("LoadHouseWaybillsDatabaseData [PAK.PAKPaltno] end");
					newRow.HAWB_PALLET_QTY = hawb2Table.Rows.Count;


                    logger.Debug("LoadHouseWaybillsDatabaseData HAWB_BOX_QTY__by__WAYBILL_NUMBER WAYBILL_NUMBER begin");
                    hawbTable = EditsSqlProc.HAWB_BOX_QTY__by__WAYBILL_NUMBER(HAWB);
                    logger.Debug("LoadHouseWaybillsDatabaseData HAWB_BOX_QTY__by__WAYBILL_NUMBER WAYBILL_NUMBER end");
                    double HAWB_BOX_QTY = Convert.ToDouble(hawbTable.Rows[0]["HAWB_BOX_QTY"].ToString());
                    newRow.HAWB_BOX_QTY = HAWB_BOX_QTY;

                    logger.Debug("LoadHouseWaybillsDatabaseData HAWB_UNIT_QTY__by__WAYBILL_NUMBER WAYBILL_NUMBER begin");
                    hawbTable = EditsSqlProc.HAWB_UNIT_QTY__by__WAYBILL_NUMBER(HAWB);
                    logger.Debug("LoadHouseWaybillsDatabaseData HAWB_UNIT_QTY__by__WAYBILL_NUMBER WAYBILL_NUMBER end");
                    double HAWB_UNIT_QTY = Convert.ToDouble(hawbTable.Rows[0]["HAWB_UNIT_QTY"].ToString());
                    newRow.HAWB_UNIT_QTY = HAWB_UNIT_QTY;

					//CUR DATA
				   // newRow.CURRENT_DATE = DateTime.Today;
					newRow.CURRENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
				    // newRow.ACTUAL_SHIPDATE = Convert.ToDateTime(packIDTable.Rows[0]["ACTUAL_SHIPDATE"]);

                    houseWaybillsList.HOUSE_WAYBILL.Rows.Add(newRow);

                    logger.Debug("LoadHouseWaybillsDatabaseData bsam InternalID__from__v_PAKComn__by__waybill_number begin");
                    DataTable waybillTable = EditsSqlProc.InternalID__from__v_PAKComn__by__waybill_number(waybill_number);
                    logger.Debug("LoadHouseWaybillsDatabaseData bsam InternalID__from__v_PAKComn__by__waybill_number end");
                    int indexInternalID = 0;
					foreach (DataRow w in waybillTable.Rows){
						string tmpid = w["InternalID"].ToString();
						logger.Debug("LoadHouseWaybillsDatabaseData bsam [v_PAKComn] begin");
						DataTable dt = EditsSqlProc.v_PAKComn__by__InternalID(tmpid);
						logger.Debug("LoadHouseWaybillsDatabaseData bsam [v_PAKComn] end");
						if ((dt==null) || (dt.Rows.Count==0)){
							logger.Error("LoadHouseWaybillsDatabaseData bsam [v_PAKComn] InternalID , no data in PAK_PackkingData for InternalID "+tmpid);
							continue;
						}
						
						HOUSE_WAYBILLS.PACK_ID_LINE_ITEMRow newLineRow = houseWaybillsList.PACK_ID_LINE_ITEM.NewPACK_ID_LINE_ITEMRow();
                        //newLineRow.PACK_ID_LINE_ITEM_Id = indexInternalID;
                        newLineRow.HOUSE_WAYBILL_Id = index;
						foreach (DataColumn column in houseWaybillsList.PACK_ID_LINE_ITEM.Columns)
						{
							if (dt.Columns.Contains(column.ColumnName))
							{
								newLineRow[column] = dt.Rows[0][column.ColumnName];
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

                        indexInternalID++;
						houseWaybillsList.PACK_ID_LINE_ITEM.Rows.Add(newLineRow);
					}
					
					
					
					index++;
					
				}
				
				
			/*}
			else if (Consolidate.Trim() == "")
			{
				
			}*/
			
            //populate mini tables
            //set this dataset to HouseWaybills so we can outputXML

            this.m_DataSet = houseWaybillsList;

            logger.Debug("LoadHouseWaybillsDatabaseData end");
        }

        private void LoadKeyValuePairs__by__PO_NUM(DataTable dataTable,
            DataRow readRow, string keyColumnName, string valueColumnName,
            string keyColumn, int keyValue, /*string whereClause,*/ string PO_NUM)
        {
            logger.Debug("LoadKeyValuePairs__by__PO_NUM begin");

            logger.Debug("LoadKeyValuePairs PAK.PAKEdi850raw begin");
            DataTable tempTable = EditsSqlProc.selects__from__PAK_PAKEdi850raw__by__PO_NUM(keyColumnName + ", " + valueColumnName, PO_NUM);
            logger.Debug("LoadKeyValuePairs PAK.PAKEdi850raw end");

            foreach (DataRow readRow2 in tempTable.Rows)
            {
                DataRow newRow = dataTable.NewRow();
                newRow["KEY"] = readRow2[keyColumnName];
                newRow["VALUE"] = readRow2[valueColumnName];
                newRow[keyColumn] = keyValue;
                dataTable.Rows.Add(newRow);
            }

            logger.Debug("LoadKeyValuePairs__by__PO_NUM end");
        }

        

	}
}
