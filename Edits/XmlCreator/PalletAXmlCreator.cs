using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using PalletATD;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
	/// <summary>
	/// Packlist Xml Creator
	/// </summary>
	public class PalletAXmlCreator : XmlCreator
	{
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

#if DEBUG
        private static NLog.Logger m_Nlogger    = NLog.LogManager.GetCurrentClassLogger();
#endif
        public override bool LoadData(string aKey)
        {
            LoadPalletADatabaseData(aKey);
            return true;
        }
        public void LoadPalletADatabaseData(string id)
        {
            logger.Debug("LoadPalletADatabaseData begin id="+id);

            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            string palletID = idSplit[1];

            //fill packID table
            PALLETS palletList = new PALLETS();
            /*
            string whereMainClause = "WHERE InternalID='" + internalID + "' AND PALLET_ID='"+palletID+"'";            
            List<string> fields=new List<string>();
            fields.Add("*");
            DataTable palletTable = DBFactory.PopulateTempTable("[v_PAKPalletTypeALineItem]", whereMainClause, fields);
            */

            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeALineItem] begin");
            DataTable palletTable = EditsSqlProc.v_PAKPalletTypeALineItem__by__InternalID__PALLET_ID(internalID, palletID);
            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeALineItem] end");

            PALLETS.PALLETDataTable newTable = palletList.PALLET;
            int index = 0;
            foreach (DataRow readRow in palletTable.Rows)
            {
                PALLETS.PALLETRow newRow = newTable.NewPALLETRow();
                newRow.PALLET_Id_0 = index;
                //index++;

                foreach (DataColumn column in newTable.Columns)
                {
                    if (palletTable.Columns.Contains(column.ColumnName))
                    {
                        //PACK_ID_UNIT_QTY_PER_PALLET
                        if (column.ColumnName == "PACK_ID_UNIT_QTY_PER_PALLET")
                        {
                            /*
                            List<string> fieldsUNITPALLET = new List<string>();
                            fieldsUNITPALLET.Add("Sum(convert(int,PACK_ID_UNIT_QTY_PER_PALLET)) as PACK_ID_UNIT_QTY_PER_PALLET");
                            string whereUNITPALLET = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable BOXUNITPALLET = DBFactory.PopulateTempTable("[v_PAKPalletTypeALineItem]", whereUNITPALLET, fieldsUNITPALLET);
                            */

                            //2012??? Invalid column name 'PACK_ID_UNIT_QTY_PER_PALLET
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeALineItem] begin");
                            DataTable BOXUNITPALLET = EditsSqlProc.sum_PACK_ID_UNIT_QTY_PER_PALLET__from__v_PAKPalletTypeALineItem__by__PALLET_ID__InternalID10(palletID, internalID.Substring(0, 10));
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeALineItem] end");

                            ChkRowNotNull("PalletAXmlCreator id=" + id + ", v_PAKPalletTypeALineItem internalID=" + internalID.Substring(0, 10) + ", palletID=" + palletID, BOXUNITPALLET);
                            newRow[column] = BOXUNITPALLET.Rows[0]["PACK_ID_UNIT_QTY_PER_PALLET"];
                            continue;
                        }
                        //PACK_ID_BOX_QTY_PER_PALLET
                        if (column.ColumnName == "PACK_ID_BOX_QTY_PER_PALLET")
                        {
                            /*
                            List<string> fieldsBOXPALLET = new List<string>();
                            fieldsBOXPALLET.Add("Sum(convert(int,PACK_ID_BOX_QTY_PER_PALLET)) as PACK_ID_BOX_QTY_PER_PALLET");
                            string whereBOXPALLET = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable BOXPERPALLET = DBFactory.PopulateTempTable("[v_PAKPalletTypeALineItem]", whereBOXPALLET, fieldsBOXPALLET);
                            */

                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeALineItem] begin");
                            DataTable BOXPERPALLET = EditsSqlProc.sum_PACK_ID_BOX_QTY_PER_PALLET__from__v_PAKPalletTypeALineItem__by__PALLET_ID__InternalID10(palletID, internalID.Substring(0, 10));
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeALineItem] end");

                            ChkRowNotNull("PalletAXmlCreator id=" + id + ", v_PAKPalletTypeALineItem internalID=" + internalID.Substring(0, 10) + ", palletID=" + palletID, BOXPERPALLET);
                            newRow[column] = BOXPERPALLET.Rows[0]["PACK_ID_BOX_QTY_PER_PALLET"];
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
                /*
                string whereOrderClause = "WHERE InternalID='" + internalID + "'";
                DataTable orderTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereOrderClause, fields);
                */

                logger.Debug("LoadPalletADatabaseData [v_PAKComn] begin");
                DataTable orderTable = EditsSqlProc.v_PAKComn__by__InternalID(internalID);
                logger.Debug("LoadPalletADatabaseData [v_PAKComn] end");
                ChkRowNotNull("PalletAXmlCreator id=" + id + ", v_PAKComn internalID=" + internalID, orderTable);

                foreach (DataColumn column in newTable.Columns)
                {
                    if (orderTable.Columns.Contains(column.ColumnName))
                    {
						//HP_PN
						if (column.ColumnName == "HP_PN")
						{
                            /*
                            List<string> fieldsPN=new List<string>();
                           fieldsPN.Add("distinct HP_PN");
                            string whereHP_PN = "where PALLET_ID = '"+palletID+"' and Left(InternalID,10) = '"+ internalID.Substring(0,10) +"'";
                            DataTable HP_PNTable = DBFactory.PopulateTempTable("[v_PAKPalletTypeA]", whereHP_PN, fieldsPN);
                            */

                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeA] begin");
                            DataTable HP_PNTable = EditsSqlProc.HP_PN__from__v_PAKPalletTypeA__by__PALLET_ID__InternalID10(palletID, internalID.Substring(0, 10));
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeA] end");

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
                        if (column.ColumnName == "PALLET_BOX_QTY")
                        {
                            /*
                            List<string> fieldsBOXQTY = new List<string>();
                            fieldsBOXQTY.Add("Sum(PALLET_BOX_QTY) as PALLET_BOX_QTY");
                            string whereBOXQTY = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable BOXQTYTable = DBFactory.PopulateTempTable("[v_PAKPalletTypeA]", whereBOXQTY, fieldsBOXQTY);
                            */
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeA] begin");
                            DataTable BOXQTYTable = EditsSqlProc.sum_PALLET_BOX_QTY__from__v_PAKPalletTypeA__by__PALLET_ID__InternalID10(palletID, internalID.Substring(0, 10));
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeA] end");

                            ChkRowNotNull("PalletAXmlCreator id=" + id + ", v_PAKPalletTypeA internalID=" + internalID.Substring(0, 10) + ", palletID=" + palletID, BOXQTYTable);
                            newRow[column] = BOXQTYTable.Rows[0]["PALLET_BOX_QTY"];
                            continue;                            
                        }
                        if (column.ColumnName == "PALLET_UNIT_QTY")
                        {
                            /*
                            List<string> fieldsUNITQTY = new List<string>();
                            fieldsUNITQTY.Add("Sum(PALLET_UNIT_QTY) as PALLET_UNIT_QTY");
                            string whereUNITQTY = "where PALLET_ID = '" + palletID + "' and Left(InternalID,10) = '" + internalID.Substring(0, 10) + "'";
                            DataTable UNITQTYTable = DBFactory.PopulateTempTable("[v_PAKPalletTypeA]", whereUNITQTY, fieldsUNITQTY);
                            */
                            //2012??? PALLET_UNIT_QTY was nvarchar
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeA] begin");
                            DataTable UNITQTYTable = EditsSqlProc.sum_PALLET_UNIT_QTY__from__v_PAKPalletTypeA__by__PALLET_ID__InternalID10(palletID, internalID.Substring(0, 10));
                            logger.Debug("LoadPalletADatabaseData [v_PAKPalletTypeA] end");

                            ChkRowNotNull("PalletAXmlCreator id=" + id + ", v_PAKPalletTypeA internalID=" + internalID.Substring(0, 10) + ", palletID=" + palletID, UNITQTYTable);
                            newRow[column] = UNITQTYTable.Rows[0]["PALLET_UNIT_QTY"];
                            continue;
                        }
                        //if table contains name, then populate
                        newRow[column] = orderTable.Rows[0][column.ColumnName];
                    }
                    
                }                
                newTable.Rows.Add(newRow);

                //get LABEL_INSTR_HEADER
                /*
                string whereLneItemClause_LABEL = "WHERE PO_NUM='" + orderTable.Rows[0]["PO_NUM"].ToString().Trim() + "' and FIELDS = 'LABEL_INSTR_HEAD'";
                DataTable lineItemTable_LABEL = DBFactory.PopulateTempTable("[PAKEDI_INSTR]", whereLneItemClause_LABEL, fields);
                */

                logger.Debug("LoadPalletADatabaseData [PAKEDI_INSTR] begin");
                DataTable lineItemTable_LABEL = EditsSqlProc.PAKEDI_INSTR__by__PO_NUM__FIELDS(orderTable.Rows[0]["PO_NUM"].ToString().Trim());
                logger.Debug("LoadPalletADatabaseData [PAKEDI_INSTR] end");

                foreach (DataRow readRow4 in lineItemTable_LABEL.Rows)
                {
                    LoadINSTRPairs(palletList.LABEL_INSTR_HEAD,
                        readRow4,
                        "VALUE",
                        "PALLET_Id_0",
                        newRow.PALLET_Id_0);
                }

                //get line item
                /*
                string whereLneItemClause = "WHERE SERIAL_NUM='" + palletID + "'";
                DataTable lineTable = DBFactory.PopulateTempTable("[PAKODMSESSION]", whereLneItemClause, fields);
                */

                logger.Debug("LoadPalletADatabaseData [PAKODMSESSION] begin");
                DataTable lineTable = EditsSqlProc.PAKODMSESSION__by__SERIAL_NUM(palletID);
                logger.Debug("LoadPalletADatabaseData [PAKODMSESSION] end");

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

               // DataSet ds = LoadTwoD(internalID);
                // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------
                /*
                string whereTwoD = "'" + internalID + "', '" + palletID + "'";
                DataTable TwoDTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution]", whereTwoD);
                */
                logger.Debug("LoadPalletADatabaseData [op_TwoDCode_Solution] begin");
                DataTable TwoDTable = EditsSqlProc.sp__op_TwoDCode_Solution(internalID, palletID);
                logger.Debug("LoadPalletADatabaseData [op_TwoDCode_Solution] end");

                ChkRowNotNull("PalletAXmlCreator id=" + id + ", op_TwoDCode_Solution internalID=" + internalID + ", palletID=" + palletID, TwoDTable);
                string EnCoding = TwoDTable.Rows[0]["EnCoding"].ToString(); // Y/N
                string EnCoder = TwoDTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
                string StringIDValue = "";
                if (EnCoding == "Y")
                {
                    //encoder
                    if (EnCoder == "PDF417")
                    {
                        //20130314
                        /*
                        //PDF417 generator
                        //StringIDValue = TransferCode.Encoder(TwoDTable.Rows[0]["StringIDValue"].ToString());
                        foreach (DataRow readRowtd in TwoDTable.Rows)
                        {
                            PALLETS.UDF_PALLETRow newRowtwod = palletList.UDF_PALLET.NewUDF_PALLETRow();
                            StringIDValue = TransferCode.Encoder(readRowtd["StringIDValue"].ToString().Trim());
                            newRowtwod.KEY = readRowtd["StringIDKey"].ToString().Trim();
                            newRowtwod.VALUE = StringIDValue.ToString();
                            newRowtwod.PALLET_Id_0=index;
                            palletList.UDF_PALLET.Rows.Add(newRowtwod);
                        }
                        */
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

            logger.Debug("LoadPalletADatabaseData end");
        }

        private void LoadKeyValuePairs(DataTable dataTable, 
            DataRow readRow, string keyColumnName,string valueColumnName,
            string keyColumn, int keyValue)
        {
            DataRow newRow=dataTable.NewRow();                        
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

        void LoadMiniTable(DataTable dataTable, string fieldName,string value,string keyColumn,int keyValue)
        {
            DataRow row=dataTable.NewRow();
            row[keyColumn] = keyValue;
            row[fieldName] = value;
            dataTable.Rows.Add(row);
        }     


	}
}
