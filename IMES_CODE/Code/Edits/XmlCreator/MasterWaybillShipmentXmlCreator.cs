using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using MasterWaybilTD_New;


namespace Inventec.HPEDITS.XmlCreator
{
    public class MasterWaybillShipmentXmlCreator : XmlCreator
    {
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

            MASTER_WAYBILLS masterwaybillList = new MASTER_WAYBILLS();

            string whereMainClause = "WHERE InternalID='" + internalID + "'";
            List<string> fields = new List<string>();
            fields.Add("distinct *");
            List<string> Master_fields = new List<string>();
            Master_fields.Add("distinct MASTER_WAYBILL_NUMBER");
            DataTable packTable = DBFactory.PopulateTempTable("[v_Shipment_MasterWaybillList]", whereMainClause, fields);
            //List<string> Master_fields_1 = new List<string>();
            //Master_fields_1.Add("DISTINCT MASTER_WAYBILL_NUMBER ");
            //Master_fields_1.Add(" REGION");
            //Master_fields_1.Add(" SHIP_MODE ");
            //Master_fields_1.Add(" INTL_CARRIER");
            //Master_fields_1.Add(" [CURRENT_DATE] ");
            //Master_fields_1.Add(" ACTUAL_SHIPDATE ");
            //Master_fields_1.Add(" CONTAINER_ID ");
            //Master_fields_1.Add(" SHIP_FROM_ID ");
            //Master_fields_1.Add(" SHIP_FROM_NAME ");
            //Master_fields_1.Add(" SHIP_FROM_NAME_2 ");
            //Master_fields_1.Add(" SHIP_FROM_NAME_3 ");
            //Master_fields_1.Add(" SHIP_FROM_STREET ");
            //Master_fields_1.Add(" SHIP_FROM_STREET_2 ");
            //Master_fields_1.Add(" SHIP_FROM_CITY ");
            //Master_fields_1.Add(" SHIP_FROM_STATE ");
            //Master_fields_1.Add(" SHIP_FROM_ZIP ");
            //Master_fields_1.Add(" SHIP_FROM_COUNTRY_NAME ");
            //Master_fields_1.Add(" SHIP_FROM_COUNTRY_CODE");
            //Master_fields_1.Add(" SHIP_FROM_CONTACT ");
            //Master_fields_1.Add(" SHIP_FROM_TELEPHONE ");
            //Master_fields_1.Add(" SUM(MASTER_WAYBILL_UNIT_QTY) AS MASTER_WAYBILL_UNIT_QTY ");
            //Master_fields_1.Add("  SUM(MASTER_WAYBILL_BOX_QTY) AS MASTER_WAYBILL_BOX_QTY ");
            //Master_fields_1.Add(" SUM(MASTER_WAYBILL_PALLET_QTY)   AS MASTER_WAYBILL_PALLET_QTY");
            //Master_fields_1.Add("  SUM(MASTER_WAYBILL_ACT_WEIGHT)  AS MASTER_WAYBILL_ACT_WEIGHT ");
            //Master_fields_1.Add(" SUM(MASTER_WAYBILL_GROSS_WEIGHT)   AS MASTER_WAYBILL_GROSS_WEIGHT");
            //Master_fields_1.Add(" DOC_SET_NUMBER ");

            //string whereMainClause_master = "WHERE MASTER_WAYBILL_NUMBER='" + packTable_Mastre.Rows[0]["MASTER_WAYBILL_NUMBER"] + "'";
            //whereMainClause_master = whereMainClause_master + "   GROUP BY MASTER_WAYBILL_NUMBER, REGION, SHIP_MODE, INTL_CARRIER, [CURRENT_DATE], ACTUAL_SHIPDATE, CONTAINER_ID, SHIP_FROM_ID, SHIP_FROM_NAME," 
            //          +"SHIP_FROM_NAME_2, SHIP_FROM_NAME_3, SHIP_FROM_STREET, SHIP_FROM_STREET_2, SHIP_FROM_CITY, SHIP_FROM_STATE, SHIP_FROM_ZIP, "
            //          +"SHIP_FROM_COUNTRY_NAME, SHIP_FROM_COUNTRY_CODE, SHIP_FROM_CONTACT, "
            //          +"SHIP_FROM_TELEPHONE, DOC_SET_NUMBER";
            //DataTable packTable = DBFactory.PopulateTempTable("[v_Master_shipment]", whereMainClause_master, Master_fields_1);

            MASTER_WAYBILLS.MASTER_WAYBILLDataTable newTable = masterwaybillList.MASTER_WAYBILL;

            int indexmaster = 0;
            int indexHouseWayBill = 0;
            int indexPallet = 0;
            int indexBox = 0;
            int indexProd = 0;
            foreach (DataRow ReadRow in packTable.Rows)
            {
                MASTER_WAYBILLS.MASTER_WAYBILLRow newRow = newTable.NewMASTER_WAYBILLRow();
                newRow.MASTER_WAYBILL_Id = indexmaster;
                foreach (DataColumn column in newTable.Columns)
                {
                    if (packTable.Columns.Contains(column.ColumnName))
                    {
                        newRow[column] = ReadRow[column.ColumnName];
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
                indexmaster++;
                //------------------HouseWayBill-----------------------------------
           
                MASTER_WAYBILLS.HOUSE_WAYBILLDataTable newTable2 = masterwaybillList.HOUSE_WAYBILL;
                string whereHouseWayBill = "WHERE MASTER_WAYBILL_NUMBER='" + ReadRow["MASTER_WAYBILL_NUMBER"] + "'";
                DataTable HouseWayBillTable = new DataTable();
                if (ReadRow["DOC_SET_NUMBER"].ToString().Trim() == "DS-EM-00004")// for EMEA-RKZ 
                {
                    HouseWayBillTable = DBFactory.PopulateTempTable("[v_HOUSEWAYBILL_RKZ]", whereHouseWayBill, fields);
                }
                else
                {
                    HouseWayBillTable = DBFactory.PopulateTempTable("[v_HOUSEWAYBILL]", whereHouseWayBill, fields);
                }
                //DataTable HouseWayBillTable = DBFactory.PopulateTempTable("[v_HOUSEWAYBILL]", whereHouseWayBill, fields);
                foreach (DataRow readRow2 in HouseWayBillTable.Rows)
                {
                    MASTER_WAYBILLS.HOUSE_WAYBILLRow newRow2 = newTable2.NewHOUSE_WAYBILLRow();
                    newRow2.HOUSE_WAYBILL_Id = indexHouseWayBill;

                    foreach (DataColumn column in masterwaybillList.HOUSE_WAYBILL.Columns)
                    {
                        if (HouseWayBillTable.Columns.Contains(column.ColumnName))
                        {
                            newRow2[column] = readRow2[column.ColumnName];
                        }
                        #region else
                        else
                        {
                            if (column.DataType == typeof(string))
                                newRow2[column] = string.Empty;
                            else if (column.DataType == typeof(Double))
                                newRow2[column] = 0;
                            else if (column.DataType == typeof(DateTime))
                                newRow2[column] = string.Empty;
                        }
                        #endregion
                    }//20130424
                    newRow2.MASTER_WAYBILL_Id = newRow.MASTER_WAYBILL_Id;
                    indexHouseWayBill++;
                    masterwaybillList.HOUSE_WAYBILL.Rows.Add(newRow2);
                    List<string> field_Prod = new List<string>();
                    field_Prod.Add("distinct [PROD_TYPE],[HS_CODE_PER_PROD_TYPE],[PALLET_QTY_PER_PROD_TYPE] ,[UNIT_QTY_PER_PROD_TYPE] ,[NET_WEIGHT_PER_PROD_TYPE]  ,[GROSS_WEIGHT_PER_PROD_TYPE],BOX_QTY_PER_PROD_TYPE,WAYBILL_NUMBER,Model");
                    #region Modify by 20130312
                    string whereProdType = "WHERE WAYBILL_NUMBER='" + readRow2["WAYBILL_NUMBER"] + "'";
                        //readRow2["WAYBILL_NUMBER"] + "'";
                    DataTable Prod_Type_Table = DBFactory.PopulateTempTable("[v_PROD_TYPE]", whereProdType, field_Prod);
                   
                    foreach (DataRow rowprod in Prod_Type_Table.Rows)
                    {
                        MASTER_WAYBILLS.PROD_TYPE_NAMERow prodrow = masterwaybillList.PROD_TYPE_NAME.NewPROD_TYPE_NAMERow();
                        prodrow.PROD_TYPE_NAME_Id = indexProd;

                        foreach (DataColumn columnprod in masterwaybillList.PROD_TYPE_NAME.Columns)
                        {
                            if (Prod_Type_Table.Columns.Contains(columnprod.ColumnName))
                            {
                                prodrow[columnprod] = rowprod[columnprod.ColumnName];
                            }
                            else
                            {
                                if (columnprod.DataType == typeof(string))
                                    rowprod[columnprod] = string.Empty;
                                else if (columnprod.DataType == typeof(Double))
                                    rowprod[columnprod] = 0;
                                else if (columnprod.DataType == typeof(DateTime))
                                    rowprod[columnprod] = string.Empty;
                            }
                        }
                            prodrow.HOUSE_WAYBILL_Id = newRow2.HOUSE_WAYBILL_Id;
                            indexProd++;
                            masterwaybillList.PROD_TYPE_NAME.Rows.Add(prodrow);
                   
                           List<string> field_Waybill = new List<string>();
                           //field_Waybill.Add("distinct InternalID");
                           string whereWAYBILL_NUMBER_DN = "WHERE WAYBILL_NUMBER='" + readRow2["WAYBILL_NUMBER"] + "'";
                           DataTable WAYBILL_NUMBER_DN_Table = DBFactory.PopulateTempTable("[v_Shipment_WayBillRegion]", whereWAYBILL_NUMBER_DN, fields);
                          foreach (DataRow readRowdn in WAYBILL_NUMBER_DN_Table.Rows)
                          {
                              foreach (DataColumn columndn in WAYBILL_NUMBER_DN_Table.Columns)
                               {
                                    if (columndn.ColumnName == "InternalID")
                                      {
                                //----------------------------------------------------------------------
                                           MASTER_WAYBILLS.PALLETDataTable newTable3 = masterwaybillList.PALLET;
                                           string wherePallet = "WHERE InternalID='" + readRowdn["InternalID"] + "' and Model like '" +rowprod["Model"]+"%'";
                                           DataTable PalletTable = DBFactory.PopulateTempTable("[v_PAKPaltno]", wherePallet, fields);

                                          //indexPallet = 0;
                                          foreach (DataRow readRow3 in PalletTable.Rows)
                                             {
                                                MASTER_WAYBILLS.PALLETRow newRow3 = newTable3.NewPALLETRow();
                                                  newRow3.PALLET_Id_0 = indexPallet;
                                                  foreach (DataColumn column_3 in masterwaybillList.PALLET.Columns)
                                                    {
                                                     if (PalletTable.Columns.Contains(column_3.ColumnName))
                                                             {
                                                                 newRow3[column_3] = readRow3[column_3.ColumnName];
                                                              }
                                                        #region else
                                                     else
                                                             {
                                                                  if (column_3.DataType == typeof(string))
                                                                      newRow3[column_3] = string.Empty;
                                                                 else if (column_3.DataType == typeof(Double))
                                                                     newRow3[column_3] = 0;
                                                                 else if (column_3.DataType == typeof(DateTime))
                                                                      newRow3[column_3] = string.Empty;
                                                              }
                                                             #endregion
                                                    }

                                                  newRow3.PROD_TYPE_NAME_Id = prodrow.PROD_TYPE_NAME_Id;
                                              // newRow3.HOUSE_WAYBILL_Id = newRow.MASTER_WAYBILL_Id;
                                          indexPallet++;
                                          masterwaybillList.PALLET.Rows.Add(newRow3);

                                    //----------------add Box List----------------

                                          MASTER_WAYBILLS.BOXDataTable newTable4 = masterwaybillList.BOX;

                                        // string wherePallet_Box = "WHERE InternalID='" + readRowdn["InternalID"] + "'and PALLET_ID='" + readRow3["PALLET_ID"] + "'";
                                      string wherePallet_Box = "WHERE InternalID='" + readRowdn["InternalID"] + "'and PALLET_ID='" + readRow3["PALLET_ID"] + "'";

                                      DataTable Pallet_Box_Table = DBFactory.PopulateTempTable("[v_Masterwaybill_weight]", wherePallet_Box, fields);
                                        foreach (DataRow readRow4 in Pallet_Box_Table.Rows)
                                            {
                                        MASTER_WAYBILLS.BOXRow newRow4 = newTable4.NewBOXRow();
                                        // newRow4.BOX_Id_0 = indexBox;
                                        newRow4.PALLET_Id_0 = indexBox;
                                        foreach (DataColumn column_1 in masterwaybillList.BOX.Columns)
                                        {
                                            if (Pallet_Box_Table.Columns.Contains(column_1.ColumnName))
                                            {
                                                newRow4[column_1] = readRow4[column_1.ColumnName].ToString().Trim();
                                            }
                                            #region else
                                            else
                                            {
                                                if (column_1.DataType == typeof(string))
                                                {
                                                    newRow4[column_1] = string.Empty;
                                                }
                                                else if (column_1.DataType == typeof(Double))
                                                {
                                                    newRow4[column_1] = 0;
                                                }
                                                else if (column_1.DataType == typeof(DateTime))
                                                {
                                                    newRow4[column_1] = string.Empty;
                                                }
                                            }
                                            #endregion
                                        }
                                        //newRow4.PALLET_Id_0 = newRow.MASTER_WAYBILL_Id;
                                        newRow4.PALLET_Id_0 = newRow3.PALLET_Id_0;

                                        indexBox++;
                                        masterwaybillList.BOX.Rows.Add(newRow4);
                                        #region mark
                                        //-------------Add SN List---------
                                        //string whereSERIAL_NUM = "WHERE InternalID='" + readRowdn["InternalID"] + "'and PALLET_ID='" + readRow3["PALLET_ID"] + "'and BOX_ID='" + readRow4["BOX_ID"] + "'";
                                        //DataTable SERIAL_NUMTable = DBFactory.PopulateTempTable("[v_Masterwaybill_weight]", whereSERIAL_NUM, fields);

                                        //foreach (DataRow readRow9 in SERIAL_NUMTable.Rows)
                                        //{
                                        //    LoadSeralNumPairs(masterwaybillList.SERIAL_NUM, readRow9, "SERIAL_NUM", "BOX_Id_0", newRow4.BOX_Id_0);//newRow.MASTER_WAYBILL_Id);
                                        //}       
                                        //-----------------end-------------
                                        #endregion
                                    }
                                }
                            }
                            #region else
                            else
                            {
                                break;
                                //continue;
                            }
                            #endregion
                        }
                    }

                    }
                    #endregion
                    #region old
                    //--------------------PALLET List--------------------------------------------------------
                    //------------Find DN---------------------------------------------------
                    /*  string whereWAYBILL_NUMBER_DN = "WHERE WAYBILL_NUMBER='" + readRow2["WAYBILL_NUMBER"] + "'";
                      DataTable WAYBILL_NUMBER_DN_Table = DBFactory.PopulateTempTable("[v_Shipment_WayBillRegion]", whereWAYBILL_NUMBER_DN, fields);
                      foreach (DataRow readRowdn in WAYBILL_NUMBER_DN_Table.Rows)
                      {
                          foreach (DataColumn columndn in WAYBILL_NUMBER_DN_Table.Columns)
                          {
                              if (columndn.ColumnName == "InternalID")
                              {
                     //----------------------------------------------------------------------
                                  MASTER_WAYBILLS.PALLETDataTable newTable3 = masterwaybillList.PALLET;
                                  string wherePallet = "WHERE InternalID='" + readRowdn["InternalID"] + "'";
                                  DataTable PalletTable = DBFactory.PopulateTempTable("[PAK.PAKPaltno]", wherePallet, fields);

                                  //int indexPallet = 0;
                                  foreach (DataRow readRow3 in PalletTable.Rows)
                                  {
                                      MASTER_WAYBILLS.PALLETRow newRow3 = newTable3.NewPALLETRow();
                                      newRow3.PALLET_Id_0 = indexPallet;
                                      foreach (DataColumn column in masterwaybillList.PALLET.Columns)
                                      {
                                          if (PalletTable.Columns.Contains(column.ColumnName))
                                          {
                                              newRow3[column] = readRow3[column.ColumnName];
                                          }
                                          else
                                          {
                                              if (column.DataType == typeof(string))
                                                  newRow3[column] = string.Empty;
                                              else if (column.DataType == typeof(Double))
                                                  newRow3[column] = 0;
                                              else if (column.DataType == typeof(DateTime))
                                                  newRow3[column] = string.Empty;
                                          }
                                      }
                                      newRow3.HOUSE_WAYBILL_Id = newRow.MASTER_WAYBILL_Id;
                                      indexPallet++;
                                      masterwaybillList.PALLET.Rows.Add(newRow3);

                                      //----------------add Box List----------------
                                    
                                      MASTER_WAYBILLS.BOXDataTable newTable4=masterwaybillList.BOX;
                                      string wherePallet_Box = "WHERE InternalID='" + readRowdn["InternalID"] + "'and PALLET_ID='" + readRow3["PALLET_ID"] + "'";
                                      DataTable Pallet_Box_Table = DBFactory.PopulateTempTable("[v_Masterwaybill_weight]", wherePallet_Box, fields);
                                      foreach (DataRow readRow4 in Pallet_Box_Table.Rows)
                                      {
                                          MASTER_WAYBILLS.BOXRow newRow4 = newTable4.NewBOXRow();
                                          newRow4.BOX_Id_0 = indexBox;
                                          newRow4.PALLET_Id_0 = indexBox;
                                          foreach (DataColumn column in masterwaybillList.BOX.Columns)
                                          {
                                              if (Pallet_Box_Table.Columns.Contains(column.ColumnName))
                                              {
                                                  newRow4[column] = readRow4[column.ColumnName];
                                              }
                                              else
                                              {
                                                  if (column.DataType == typeof(string))
                                                  {
                                                      newRow4[column] = string.Empty;
                                                  }
                                                  else if (column.DataType == typeof(Double))
                                                  {
                                                      newRow4[column] = 0;
                                                  }
                                                  else if (column.DataType == typeof(DateTime))
                                                  {
                                                      newRow4[column] = string.Empty;
                                                  }
                                              }                                            
                                          }
                                          //newRow4.PALLET_Id_0 = newRow.MASTER_WAYBILL_Id;
                                          newRow4.PALLET_Id_0 = newRow3.PALLET_Id_0;
                                        
                                          indexBox++;
                                          masterwaybillList.BOX.Rows.Add(newRow4); 


                                          //-------------Add SN List---------
                                          string whereSERIAL_NUM = "WHERE InternalID='" + readRowdn["InternalID"] + "'and PALLET_ID='" + readRow3["PALLET_ID"] + "'and BOX_ID='" + readRow4["BOX_ID"] + "'";
                                          DataTable SERIAL_NUMTable = DBFactory.PopulateTempTable("[v_Masterwaybill_weight]", whereSERIAL_NUM, fields);

                                          foreach (DataRow readRow9 in SERIAL_NUMTable.Rows)
                                          {
                                              LoadSeralNumPairs(masterwaybillList.SERIAL_NUM, readRow9, "SERIAL_NUM", "BOX_Id_0", newRow4.BOX_Id_0);//newRow.MASTER_WAYBILL_Id);
                                          }       
                                          //-----------------end-------------
                                      }
                                      //-------------------end----------------------
                                  }
                              }
                              else
                              {
                                  break;
                              }
                          }
                      }*/
                    #endregion
               
                }
        

            }
            this.m_DataSet = masterwaybillList;
        }

        void LoadSeralNumPairs(DataTable dataTable,
        DataRow readRow, string valueColumnName,
        string keyColumn, int keyValue)
        {
            DataRow newRow = dataTable.NewRow();
            newRow["SERIAL_NUM_Column"] = readRow[valueColumnName];
            newRow[keyColumn] = keyValue;
            dataTable.Rows.Add(newRow);
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
