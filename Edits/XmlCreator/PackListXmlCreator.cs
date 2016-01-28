using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using PackListTD;
using System.Reflection;
using log4net;

namespace Inventec.HPEDITS.XmlCreator
{
	/// <summary>
	/// Packlist Xml Creator
	/// </summary>
	public class PackListXmlCreator : XmlCreator
	{
          
#if DEBUG
        private static NLog.Logger m_Nlogger    = NLog.LogManager.GetCurrentClassLogger();
#endif
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override bool LoadData(string aKey)
        {
            LoadPackingListDatabaseData(aKey);
            return true;
        }
        public void LoadPackingListDatabaseData(string id)
        {
            logger.Debug("LoadPackingListDatabaseData begin id="+id);

            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            string Tp = idSplit[1];
            //fill packID table
            //--------------------- add  by fan  20100107 ----------
            string Type = "";
            /*
            string whereTDcheck = "'" + internalID + "','" + Tp + "','2'";
            DataTable TDcheckTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Check]", whereTDcheck);
            */

            logger.Debug("LoadPackingListDatabaseData op_TwoDCode_Check begin");
            DataTable TDcheckTable = EditsSqlProc.sp__op_TwoDCode_Check(internalID, Tp, "2");
            logger.Debug("LoadPackingListDatabaseData op_TwoDCode_Check end");

            ChkRowNotNull("PackListXmlCreator id=" + id + ", op_TwoDCode_Check internalID=" + internalID + ", docs=" + Tp + ", tp=2", TDcheckTable);
            Type = TDcheckTable.Rows[0][0].ToString().Trim();
            //--------------------  end  -----------------------------
            //fill packID table
            NewDataSet packingList = new NewDataSet();
            /*
            string whereMainClause = "WHERE InternalID='" + internalID  + "'";            
            List<string> fields=new List<string>();
            fields.Add("*");
            DataTable pakTable = DBFactory.PopulateTempTable("[v_PackList]", whereMainClause, fields);
            */

            logger.Debug("LoadPackingListDatabaseData v_PackList begin");
            DataTable pakTable = EditsSqlProc.v_PackList__by__InternalID(internalID);
            logger.Debug("LoadPackingListDatabaseData v_PackList end");
            ChkRowNotNull("PackListXmlCreator id=" + id + ", v_PackList internalID=" + internalID, pakTable);

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

            /*
            string whereLneItemClause = "WHERE InternalID LIKE '" + internalID.Substring(0, 10) + "%' order by InternalID";
            DataTable lineTable = DBFactory.PopulateTempTable("[v_PAKComn]", whereLneItemClause, fields);
            */

            logger.Debug("LoadPackingListDatabaseData v_PAKComn begin");
            DataTable lineTable = EditsSqlProc.v_PAKComn__like__InternalID(internalID.Substring(0, 10) + "%");
            logger.Debug("LoadPackingListDatabaseData v_PAKComn end");

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
                /*
                string whereWeightClause = "WHERE Model='" + lineTable.Rows[index]["Model"].ToString() + "'";
                DataTable weightTable = DBFactory.PopulateTempTable("[PAK_SkuMasterWeight_FIS]", whereWeightClause, fields);
                */

                logger.Debug("LoadPackingListDatabaseData PAK_SkuMasterWeight_FIS begin");
                DataTable weightTable = EditsSqlProc.PAK_SkuMasterWeight_FIS__by__Model(lineTable.Rows[index]["Model"].ToString());
                logger.Debug("LoadPackingListDatabaseData PAK_SkuMasterWeight_FIS end");
                ChkRowNotNull("PackListXmlCreator id=" + id + ", PAK_SkuMasterWeight_FIS Model=" + lineTable.Rows[index]["Model"].ToString(), weightTable);

                index++;
                newLineRow.EXTD_BOX_WEIGHT = Convert.ToDouble(string.Format("{0:0.##}",newLineRow.PACK_ID_LINE_ITEM_BOX_QTY * Convert.ToDouble(weightTable.Rows[0]["Weight"])));

                newLineRow.PACK_ID_UNIT_UOM = "EA";
                newLineRow.PACK_ID_Id = newRow.PACK_ID_Id;
                
                packingList.PACK_ID_LINE_ITEM.Rows.Add(newLineRow);

                /*
                string where850Clause = "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "' AND HP_PN_COMPONENT !='' and LINE_ITEM = '" + readRow["HP_SO_LINE_ITEM"].ToString() + "' order by HP_SO_LINE_ITEM ";
                DataTable edi850Table = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", where850Clause, fields);
                */

                logger.Debug("LoadPackingListDatabaseData PAKEdi850raw begin");
                DataTable edi850Table = EditsSqlProc.PAK_PAKEdi850raw__by__PO_NUM__LINE_ITEM(readRow["PO_NUM"].ToString(), readRow["HP_SO_LINE_ITEM"].ToString());
                logger.Debug("LoadPackingListDatabaseData PAKEdi850raw end");

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
                //details

                /*
                LoadMiniTable_Detail(packingList.EXPORT_NOTES_DETAIL,
                    "EXPORT_NOTES_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +  "' and FIELDS = 'EXPORT_NOTES_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.SHIPPING_INSTR_DETAIL,
                    "SHIPPING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +  "' and FIELDS = 'SHIPPING_INSTR_DETAIL' order by ID", "VALUE");
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
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +  "' and FIELDS = 'DELIVERY_INSTR_DETAIL' order by ID", "VALUE");
               LoadMiniTable_Detail(packingList.SPECIAL_INSTR_DETAIL,
                    "SPECIAL_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "' and PO_ITEM = '" + readRow["C26"].ToString() + "'and FIELDS = 'SPECIAL_INSTR_DETAIL' order by ID", "VALUE");

                LoadMiniTable_Detail(packingList.CUSTOMER_INSTR_DETAIL,
                    "CUSTOMER_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +  "' and FIELDS = 'CUSTOMER_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.HANDLING_INSTR_DETAIL,
                    "HANDLING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +"' and FIELDS = 'HANDLING_INSTR_DETAIL' order by ID", "VALUE");
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
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +  "' and FIELDS = 'CONFIG_INSTR_DETAIL' order by ID", "VALUE");
                LoadMiniTable_Detail(packingList.MAXKIT_DETAIL,
                    "MAXKIT_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() +  "' and FIELDS = 'MAXKIT_DETAIL' order by ID", "VALUE");
                */
                LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.EXPORT_NOTES_DETAIL,
                    "EXPORT_NOTES_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "EXPORT_NOTES_DETAIL");
                LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.SHIPPING_INSTR_DETAIL,
                    "SHIPPING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "SHIPPING_INSTR_DETAIL");
                LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.PICKING_INSTR_DETAIL,
                    "PICKING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "PICKING_INSTR_DETAIL");
                LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.DELIVERY_INSTR_DETAIL,
                    "DELIVERY_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "DELIVERY_INSTR_DETAIL");
                LoadMiniTable_Detail__by__PO_NUM__PO_ITEM__FIELDS(packingList.SPECIAL_INSTR_DETAIL,
                    "SPECIAL_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", readRow["PO_NUM"].ToString(), readRow["C26"].ToString(), "SPECIAL_INSTR_DETAIL");

               LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.CUSTOMER_INSTR_DETAIL,
                    "CUSTOMER_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "CUSTOMER_INSTR_DETAIL");
               LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.HANDLING_INSTR_DETAIL,
                    "HANDLING_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "HANDLING_INSTR_DETAIL");
               LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.UID_INSTR_DETAIL,
                    "UID_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "UID_INSTR_DETAIL");
               LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.CONFIG_INSTR_DETAIL,
                    "CONFIG_INSTR_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "CONFIG_INSTR_DETAIL");
               LoadMiniTable_Detail__by__PO_NUM__FIELDS(packingList.MAXKIT_DETAIL,
                    "MAXKIT_DETAIL_Column",
                    "",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "MAXKIT_DETAIL");

               /*LoadKeyValuePairs(packingList.UDF_DETAIL,
                    null,
                    "UDF_KEY_DETAIL",
                    "UDF_VALUE_DETAIL",
                    "PACK_ID_LINE_ITEM_Id",
                    newLineRow.PACK_ID_LINE_ITEM_Id,
                    "WHERE PO_NUM='" + readRow["PO_NUM"].ToString() + "'");*/
               LoadKeyValuePairs__by__PO_NUM(packingList.UDF_DETAIL,
                   null,
                   "UDF_KEY_DETAIL",
                   "UDF_VALUE_DETAIL",
                   "PACK_ID_LINE_ITEM_Id",
                   newLineRow.PACK_ID_LINE_ITEM_Id,
                   readRow["PO_NUM"].ToString());

                //populate box information
                /*
                string whereBoxClause = "WHERE InternalID='" + readRow["InternalID"].ToString() + "'";
                DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);
                */

                logger.Debug("LoadPackingListDatabaseData PAK_PackkingData begin");
                DataTable boxTable = EditsSqlProc.PAK_PackkingData__by__InternalID(readRow["InternalID"].ToString());
                logger.Debug("LoadPackingListDatabaseData PAK_PackkingData end");

                if (boxTable.Rows.Count == 0)
                {
                    NewDataSet.BOXRow newBoxRowPacking = packingList.BOX.NewBOXRow();
                    newBoxRowPacking.BOX_ID = "PACKING";
                    newBoxRowPacking.BOX_Id_0 = index2;
                    newBoxRowPacking.BOX_WEIGHT = Convert.ToDouble(weightTable.Rows[0]["Weight"]);
                    newBoxRowPacking.BOX_UNIT_QTY = Convert.ToDouble(pakTable.Rows[0]["BOX_UNIT_QTY"]);
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
                        newBoxRow.BOX_WEIGHT_UOM = "KG";
                        newBoxRow.BOX_UNIT_QTY = Convert.ToDouble(pakTable.Rows[0]["BOX_UNIT_QTY"]);
                        newBoxRow.PACK_ID_LINE_ITEM_Id = newLineRow.PACK_ID_LINE_ITEM_Id;

                        newBoxRow.TRACK_NO_PARCEL = readRow2["TRACK_NO_PARCEL"].ToString().Trim();
                        packingList.BOX.Rows.Add(newBoxRow);

                        NewDataSet.SERIAL_NUMRow newSerialRow = packingList.SERIAL_NUM.NewSERIAL_NUMRow();
                        newSerialRow.BOX_Id_0 = index2;
                        newSerialRow.SERIAL_NUM_Column = readRow2["SERIAL_NUM"].ToString().Trim();

                        packingList.SERIAL_NUM.Rows.Add(newSerialRow);



                     
                        // DataSet ds = LoadTwoD(internalID);------------------------encoding---------------------
                        if (Type == "Y")
                        {
                            /*
                            //2012??? parameter not match
                            string whereTwoDP = "'" + internalID + "','" + readRow2["BOX_ID"].ToString() + "'";
                            DataTable TwoDPTable = DBFactory.PopulateTempTable_BySp("[op_TwoDCode_Solution_packlist]", whereTwoDP);
                            */

                            logger.Debug("LoadPackingListDatabaseData op_TwoDCode_Solution_packlist begin");
                            DataTable TwoDPTable = EditsSqlProc.sp__op_TwoDCode_Solution_packlist(internalID, readRow2["BOX_ID"].ToString(), Tp);
                            logger.Debug("LoadPackingListDatabaseData op_TwoDCode_Solution_packlist end");

                            ChkRowNotNull("PackListXmlCreator id=" + id + ", op_TwoDCode_Solution_packlist internalID=" + internalID + ", boxid=" + readRow2["BOX_ID"].ToString() + ", tp=" + Tp, TwoDPTable);
                            string EnCoding = TwoDPTable.Rows[0]["EnCoding"].ToString(); // Y/N
                            string EnCoder = TwoDPTable.Rows[0]["EnCoder"].ToString(); //PDF417/MaxICode
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
                                    foreach (DataRow readRowtd in TwoDPTable.Rows)
                                    {
                                        NewDataSet.UDF_BOXRow newRowtwod = packingList.UDF_BOX.NewUDF_BOXRow();
                                        StringIDValue = TransferCode.Encoder(readRowtd["StringIDValue"].ToString());
                                        newRowtwod.KEY = readRowtd["StringIDKey"].ToString();
                                        newRowtwod.VALUE = StringIDValue.ToString();
                                        newRowtwod.BOX_Id_0 = index2;
                                        packingList.UDF_BOX.Rows.Add(newRowtwod);

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

            /*
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
            */
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.EXPORT_NOTES_HEAD,
                "EXPORT_NOTES_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "EXPORT_NOTES_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.SHIPPING_INSTR_HEAD,
                "SHIPPING_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "SHIPPING_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.PICKING_INSTR_HEAD,
                "PICKING_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "PICKING_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.DELIVERY_INSTR_HEAD,
                "DELIVERY_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "DELIVERY_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.SPECIAL_INSTR_HEAD,
                "SPECIAL_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "SPECIAL_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.CUSTOMER_INSTR_HEAD,
                "CUSTOMER_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "CUSTOMER_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.LABEL_INSTR_HEAD,
                "LABEL_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "LABEL_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.CARRIER_INST_HEAD,
                "CARRIER_INST_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "CARRIER_INST_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.HANDLING_INSTR_HEAD,
                "HANDLING_INSTR_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "HANDLING_INSTR_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.INVOICE_UDF_HEAD,
                "INVOICE_UDF_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "INVOICE_UDF_HEAD");
            LoadMiniTable_PACK__by__PO_NUM__FIELDS(packingList.UDF_HEAD,
                "UDF_HEAD_Column",
                "",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "VALUE", pakTable.Rows[0]["PO_NUM"].ToString(), "UDF_HEAD");
            
            /*LoadKeyValuePairs(packingList.UDF_HEADER,
                null,
                "UDF_KEY_HEADER",
                "UDF_VALUE_HEADER",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "'");*/
            LoadKeyValuePairs__by__PO_NUM(packingList.UDF_HEADER,
                null,
                "UDF_KEY_HEADER",
                "UDF_VALUE_HEADER",
                "PACK_ID_Id",
                newRow.PACK_ID_Id,
                pakTable.Rows[0]["PO_NUM"].ToString());

            
            //populate mini tables
            //set this dataset to packinglist so we can outputXML

            this.m_DataSet = packingList;

            logger.Debug("LoadPackingListDatabaseData end");
        }

        /*private void LoadKeyValuePairs(DataTable dataTable, 
            DataRow readRow, string keyColumnName,string valueColumnName,
            string keyColumn, int keyValue,string whereClause)
        {
            logger.Debug("LoadKeyValuePairs begin");

            List<string> fields = new List<string>();
            fields.Add("DISTINCT "+keyColumnName+", "+valueColumnName);

            logger.Debug("LoadKeyValuePairs PAK.PAKEdi850raw begin");
            DataTable tempTable = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereClause, fields);
            logger.Debug("LoadKeyValuePairs PAK.PAKEdi850raw end");

            foreach (DataRow readRow2 in tempTable.Rows)
            {
                DataRow newRow = dataTable.NewRow();
                newRow["KEY"] = readRow2[keyColumnName];
                newRow["VALUE"] = readRow2[valueColumnName];
                newRow[keyColumn] = keyValue;
                dataTable.Rows.Add(newRow);
            }

            logger.Debug("LoadKeyValuePairs end");
        }*/

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

        /*void LoadMiniTable(DataTable dataTable, string fieldName,string value,string keyColumn,int keyValue,string whereClause,string distinctField)
        {
            logger.Debug("LoadMiniTable begin");

            List<string> fields = new List<string>();
            fields.Add("DISTINCT "+distinctField);

            logger.Debug("LoadMiniTable PAK.PAKEdi850raw begin");
            DataTable miniTableName = DBFactory.PopulateTempTable("[PAK.PAKEdi850raw]", whereClause, fields);
            logger.Debug("LoadMiniTable PAK.PAKEdi850raw end");

            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }

            logger.Debug("LoadMiniTable end");
        }*/

        /*void LoadMiniTable_PACK(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, string whereClause, string distinctField)
        {
            logger.Debug("LoadMiniTable_PACK begin");

            List<string> fields = new List<string>();
            //fields.Add("DISTINCT " + distinctField);
            fields.Add( distinctField);

            logger.Debug("LoadMiniTable_PACK PAKEDI_INSTR begin");
            DataTable miniTableName = DBFactory.PopulateTempTable("[PAKEDI_INSTR]", whereClause, fields);
            logger.Debug("LoadMiniTable_PACK PAKEDI_INSTR end");

            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }

            logger.Debug("LoadMiniTable_PACK end");
        }*/

        void LoadMiniTable_PACK__by__PO_NUM__FIELDS(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, /*string whereClause,*/ string distinctField, string PO_NUM, string FIELDS)
        {
            logger.Debug("LoadMiniTable_PACK__by__PO_NUM__FIELDS begin");

            logger.Debug("LoadMiniTable_PACK PAKEDI_INSTR begin");
            DataTable miniTableName = EditsSqlProc.VALUE__from__PAKEDI_INSTR__by__PO_NUM__FIELDS(PO_NUM, FIELDS);
            logger.Debug("LoadMiniTable_PACK PAKEDI_INSTR end");

            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }

            logger.Debug("LoadMiniTable_PACK__by__PO_NUM__FIELDS end");
        }

        /*void LoadMiniTable_Detail(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, string whereClause, string distinctField)
        {
            logger.Debug("LoadMiniTable_Detail begin");

            List<string> fields = new List<string>();
            //fields.Add("DISTINCT " + distinctField);
            fields.Add(distinctField);

            logger.Debug("LoadMiniTable_Detail v_PO_ITEM_DETAIL begin");
            DataTable miniTableName = DBFactory.PopulateTempTable("[v_PO_ITEM_DETAIL]", whereClause, fields);
            logger.Debug("LoadMiniTable_Detail v_PO_ITEM_DETAIL end");

            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }

            logger.Debug("LoadMiniTable_Detail end");
        }*/

        void LoadMiniTable_Detail__by__PO_NUM__FIELDS(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, /*string whereClause,*/ string distinctField, string PO_NUM, string FIELDS)
        {
            logger.Debug("LoadMiniTable_Detail__by__PO_NUM__FIELDS begin");

            logger.Debug("LoadMiniTable_Detail v_PO_ITEM_DETAIL begin");
            DataTable miniTableName = EditsSqlProc.v_PO_ITEM_DETAIL__by__PO_NUM__FIELDS(PO_NUM, FIELDS);
            logger.Debug("LoadMiniTable_Detail v_PO_ITEM_DETAIL end");

            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }

            logger.Debug("LoadMiniTable_Detail__by__PO_NUM__FIELDS end");
        }

        void LoadMiniTable_Detail__by__PO_NUM__PO_ITEM__FIELDS(DataTable dataTable, string fieldName, string value, string keyColumn, int keyValue, /*string whereClause,*/ string distinctField, string PO_NUM, string PO_ITEM, string FIELDS)
        {
            logger.Debug("LoadMiniTable_Detail__by__PO_NUM__PO_ITEM__FIELDS begin");

            logger.Debug("LoadMiniTable_Detail v_PO_ITEM_DETAIL begin");
            DataTable miniTableName = EditsSqlProc.v_PO_ITEM_DETAIL__by__PO_NUM__PO_ITEM__FIELDS(PO_NUM, PO_ITEM, FIELDS);
            logger.Debug("LoadMiniTable_Detail v_PO_ITEM_DETAIL end");

            foreach (DataRow readRow in miniTableName.Rows)
            {
                DataRow row = dataTable.NewRow();
                row[keyColumn] = keyValue;
                row[fieldName] = readRow[distinctField].ToString();
                dataTable.Rows.Add(row);
            }

            logger.Debug("LoadMiniTable_Detail__by__PO_NUM__PO_ITEM__FIELDS end");
        }

	}
}
