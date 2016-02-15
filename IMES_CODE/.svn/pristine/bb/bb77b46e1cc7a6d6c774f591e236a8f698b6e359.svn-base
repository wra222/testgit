using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Inventec.HPEDITS.XmlCreator.Database;
using PackListTD;
using System.Xml;
namespace Inventec.HPEDITS.XmlCreator
{
    public class PackListShipmentXmlCreator_QVC_Test : XmlCreator
    {
#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif
        public override bool LoadData(string aKey,string path)
        {
            LoadPackingListDatabaseData(aKey,path);
            return true;
        }
        public void LoadPackingListDatabaseData(string id,string xmlpath)
        {

            XmlDocument xmlDoc = new XmlDocument();
            string[] idSplit = id.Split(@"/".ToCharArray());
            string internalID = idSplit[0];
            string Tp = idSplit[1];
            //根據internalID查找相關信息
            NewDataSet packingList = new NewDataSet();
            string whereMainClause = "WHERE InternalID='" + internalID + "'";
            List<string> fields = new List<string>();
            fields.Add("*");
            DataTable pakTable = DBFactory.PopulateTempTable("[v_Shipment_PackList]", whereMainClause, fields);
            NewDataSet.PACK_IDRow newRow = packingList.PACK_ID.NewPACK_IDRow();
            newRow.PACK_ID_Id = 1;
            int flag = 1;
            double Qty = 0;
            string return_to_zip = "";
            foreach (DataColumn column in packingList.PACK_ID.Columns)
            {
                if (pakTable.Columns.Contains(column.ColumnName))
                {
                    //if table contains name, then populate                    
                    //newRow[column] = pakTable.Rows[0][column.ColumnName];
                    if (flag == 1)
                    {
                        

                        //创建声明节点
                        XmlDeclaration newDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                        xmlDoc.AppendChild(newDec);

                        XmlElement newRoot = xmlDoc.CreateElement("PACK_ID");
                        xmlDoc.AppendChild(newRoot);
               
                        flag++;
                        return_to_zip = pakTable.Rows[0]["RETURN_TO_ZIP"].ToString();
                    }
                    ////else
                    ////{

                    //xmlDoc.Load("E:\\cc2.xml");
                    XmlElement xmlElemFileName = xmlDoc.CreateElement(column.ColumnName.ToString());

                    XmlText xmlTextFileName = xmlDoc.CreateTextNode(pakTable.Rows[0][column.ColumnName].ToString());
                    xmlElemFileName.AppendChild(xmlTextFileName);
                    xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName);
                    //xmlDoc.Save("E:\\cc2.xml");

                    //}
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
            //生成UDF_HEAD資料行
            string whereUDFHEADERClause_1 = "'" + pakTable.Rows[0]["InternalID"].ToString().Trim() + "'";
            DataTable UDFHEADERTable_1 = DBFactory.PopulateTempTable_BySp("op_GetUDF_KEY_VALUE_HEADER_Shipment_QVC", whereUDFHEADERClause_1);
            bool flag_return = UDFHEADERTable_1.Select("UDF_KEY_HEADER='KEY_HEADER_1ST_RETURN_MSG'").Length > 0;//判斷是否屬於non returnable  是則flag_return 為true
            //flag_return = flag_return^true;
        
            foreach (DataRow readRow2 in UDFHEADERTable_1.Rows)
            {
                //xmlDoc.Load("E:\\cc2.xml");
                clsBarCode idEcode = new clsBarCode();//實例化IDAutomation
                #region non returnable 版本
                if (flag_return)
                {
                    if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_UPSRN")
                    {
                        continue;
                    }
                    else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_UPSGTN")
                    {
                        continue;
                    }
                    else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_USPSDN")
                    {
                        continue;
                    }
                    else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_ENDCUST_PO")
                    {

                        XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                        string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                        XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                        xmlElemFileName2.InnerText = key;
                        xmlElemFileName1.AppendChild(xmlElemFileName2);
                        string KEY_HEADER_ENDCUST_PO_Value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                        XmlElement VALUE = xmlDoc.CreateElement("VALUE");
                        VALUE.InnerText = KEY_HEADER_ENDCUST_PO_Value;
                        xmlElemFileName1.AppendChild(VALUE);
                        xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);
                        XmlElement KEY_HEADER_ENDCUST_PO_ENCODE = xmlDoc.CreateElement("UDF_HEADER");
                        XmlElement xmlElemFileName3 = xmlDoc.CreateElement("KEY");
                        xmlElemFileName3.InnerText = "KEY_HEADER_ENDCUST_PO_ENCODE";
                        KEY_HEADER_ENDCUST_PO_ENCODE.AppendChild(xmlElemFileName3);
                        string value = idEcode.Code128C(readRow2["UDF_VALUE_HEADER"].ToString().Trim()).ToString();
                        XmlElement xmlElemFileName4 = xmlDoc.CreateElement("VALUE");
                        XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                        cdata.InnerText = value;
                        xmlElemFileName4.AppendChild(cdata);
                        KEY_HEADER_ENDCUST_PO_ENCODE.AppendChild(xmlElemFileName4);
                        xmlDoc.ChildNodes.Item(1).AppendChild(KEY_HEADER_ENDCUST_PO_ENCODE);

                    }
                    else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_1ST_RETURN_MSG")
                    {
                        XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                        string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                        XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                        xmlElemFileName2.InnerText = key;
                        xmlElemFileName1.AppendChild(xmlElemFileName2);
                        string value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                        XmlElement xmlElemFileName3 = xmlDoc.CreateElement("VALUE");
                        xmlElemFileName3.InnerText = value;
                        xmlElemFileName1.AppendChild(xmlElemFileName3);
                        xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);

                    }
                    else
                    {
                        XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                        string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                        XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                        xmlElemFileName2.InnerText = key;
                        xmlElemFileName1.AppendChild(xmlElemFileName2);
                        string value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                        XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                        cdata.InnerText = value;
                        XmlElement xmlElemFileName3 = xmlDoc.CreateElement("VALUE");
                        xmlElemFileName3.AppendChild(cdata);
                        xmlElemFileName1.AppendChild(xmlElemFileName3);
                        xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);
                    }
                }
                #endregion
                #region regular 版本
                else
                {

                if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_UPSRN")
                {
                    XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                    string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                    XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName2.InnerText = key;
                    xmlElemFileName1.AppendChild(xmlElemFileName2);
                    string KEY_HEADER_UPSRN_Value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                    XmlElement VALUE = xmlDoc.CreateElement("VALUE");
                    VALUE.InnerText = KEY_HEADER_UPSRN_Value;
                    xmlElemFileName1.AppendChild(VALUE);
                    xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);
                    XmlElement KEY_HEADER_UPSRN_ENCODE = xmlDoc.CreateElement("UDF_HEADER");
                    XmlElement xmlElemFileName3 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName3.InnerText = "KEY_HEADER_UPSRN_ENCODE";
                    KEY_HEADER_UPSRN_ENCODE.AppendChild(xmlElemFileName3);
                    //string value = "420" + readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                    string value = "420" + return_to_zip;
                    string value1 = idEcode.Code128C(value).ToString();
                    XmlElement xmlElemFileName4 = xmlDoc.CreateElement("VALUE");
                   //XmlElement xmlElemFileName3 = xmlDoc.CreateElement();
                    XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                    cdata.InnerText = value1;
                    xmlElemFileName4.AppendChild(cdata);
                    KEY_HEADER_UPSRN_ENCODE.AppendChild(xmlElemFileName4);
                    xmlDoc.ChildNodes.Item(1).AppendChild(KEY_HEADER_UPSRN_ENCODE);
                }
                else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_UPSGTN" )
                {

                    XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                    string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                    XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName2.InnerText = key;
                    xmlElemFileName1.AppendChild(xmlElemFileName2);
                    string KEY_HEADER_UPSGTN_value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                    XmlElement VALUE = xmlDoc.CreateElement("VALUE");
                    VALUE.InnerText = KEY_HEADER_UPSGTN_value;
                    xmlElemFileName1.AppendChild(VALUE);
                    xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);
                    XmlElement KEY_HEADER_UPSGTN_ENCODE = xmlDoc.CreateElement("UDF_HEADER");
                    XmlElement xmlElemFileName3 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName3.InnerText = "KEY_HEADER_UPSGTN_ENCODE";
                    KEY_HEADER_UPSGTN_ENCODE.AppendChild(xmlElemFileName3);
                    string value = idEcode.Code128(readRow2["UDF_VALUE_HEADER"].ToString().Trim(),true).ToString();
                    //XmlElement xmlElemFileName3 = xmlDoc.CreateElement();
                    XmlElement xmlElemFileName4 = xmlDoc.CreateElement("VALUE");
                    //XmlElement xmlElemFileName3 = xmlDoc.CreateElement();
                    XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                    cdata.InnerText = value;
                    xmlElemFileName4.AppendChild(cdata);
                    KEY_HEADER_UPSGTN_ENCODE.AppendChild(xmlElemFileName4);
                    xmlDoc.ChildNodes.Item(1).AppendChild(KEY_HEADER_UPSGTN_ENCODE);

                }
                else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_USPSDN" )
                {
                    XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                    string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                    XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName2.InnerText = key;
                    xmlElemFileName1.AppendChild(xmlElemFileName2);
                    string KEY_HEADER_USPSDN_Value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                    XmlElement VALUE = xmlDoc.CreateElement("VALUE");
                    VALUE.InnerText = KEY_HEADER_USPSDN_Value;
                    xmlElemFileName1.AppendChild(VALUE);
                    xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);
                    XmlElement KEY_HEADER_USPSDN_ENCODE = xmlDoc.CreateElement("UDF_HEADER");
                    XmlElement xmlElemFileName3 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName3.InnerText = "KEY_HEADER_USPSDN_ENCODE";
                    KEY_HEADER_USPSDN_ENCODE.AppendChild(xmlElemFileName3);
                    //string ups_bacode = "(" + readRow2["UDF_VALUE_HEADER"].ToString().Trim().Substring(0, 2) + ")" + readRow2["UDF_VALUE_HEADER"].ToString().Trim().Substring(2);
                    string value = "~21342056902~212" + readRow2["UDF_VALUE_HEADER"].ToString().Trim() + "~m" + readRow2["UDF_VALUE_HEADER"].ToString().Trim().Length;
                    //string value1 = idEcode.UCC128(value).ToString();
                    string value1 = idEcode.Code128(value, true);
                    XmlElement xmlElemFileName4 = xmlDoc.CreateElement("VALUE");
                    XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                    cdata.InnerText = value1;
                    xmlElemFileName4.AppendChild(cdata);
                    KEY_HEADER_USPSDN_ENCODE.AppendChild(xmlElemFileName4);
                    xmlDoc.ChildNodes.Item(1).AppendChild(KEY_HEADER_USPSDN_ENCODE);

                }
                else if (readRow2["UDF_KEY_HEADER"].ToString() == "KEY_HEADER_ENDCUST_PO" )
                {

                    XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                    string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                    XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName2.InnerText = key;
                    xmlElemFileName1.AppendChild(xmlElemFileName2);
                    string KEY_HEADER_ENDCUST_PO_Value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                    XmlElement VALUE = xmlDoc.CreateElement("VALUE");
                    VALUE.InnerText = KEY_HEADER_ENDCUST_PO_Value;
                    xmlElemFileName1.AppendChild(VALUE);
                    xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);
                    XmlElement KEY_HEADER_ENDCUST_PO_ENCODE = xmlDoc.CreateElement("UDF_HEADER");
                    XmlElement xmlElemFileName3 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName3.InnerText = "KEY_HEADER_ENDCUST_PO_ENCODE";
                    KEY_HEADER_ENDCUST_PO_ENCODE.AppendChild(xmlElemFileName3);
                    string value = idEcode.Code128C(readRow2["UDF_VALUE_HEADER"].ToString().Trim()).ToString();
                    XmlElement xmlElemFileName4 = xmlDoc.CreateElement("VALUE");
                    XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                    cdata.InnerText = value;
                    xmlElemFileName4.AppendChild(cdata);
                    KEY_HEADER_ENDCUST_PO_ENCODE.AppendChild(xmlElemFileName4);
                    xmlDoc.ChildNodes.Item(1).AppendChild(KEY_HEADER_ENDCUST_PO_ENCODE);

                }
                else
                {

                    XmlElement xmlElemFileName1 = xmlDoc.CreateElement("UDF_HEADER");
                    string key = readRow2["UDF_KEY_HEADER"].ToString().Trim();
                    XmlElement xmlElemFileName2 = xmlDoc.CreateElement("KEY");
                    xmlElemFileName2.InnerText = key;
                    xmlElemFileName1.AppendChild(xmlElemFileName2);
                    string value = readRow2["UDF_VALUE_HEADER"].ToString().Trim();
                    XmlElement xmlElemFileName3 = xmlDoc.CreateElement("VALUE");
                    xmlElemFileName3.InnerText = value;
                    xmlElemFileName1.AppendChild(xmlElemFileName3);
                    xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName1);

                }
            }
                #endregion
            //xmlDoc.Save("E:\\cc2.xml");
            }
            string whereLneItemClause = "WHERE InternalID LIKE '" + internalID.Substring(0, 10) + "%'";
            DataTable lineTable = DBFactory.PopulateTempTable("[v_Shipment_PAKComn]", whereLneItemClause, fields);
            //xmlDoc.Load("E:\\cc2.xml");
            XmlElement xmlElemFileName_1 = xmlDoc.CreateElement("PACK_ID_LINE_ITEM");
            int index = 0;
            int index2 = 0;
            foreach (DataRow readRow in lineTable.Rows)
            {
                NewDataSet.PACK_ID_LINE_ITEMRow newLineRow = packingList.PACK_ID_LINE_ITEM.NewPACK_ID_LINE_ITEMRow();
                newLineRow.PACK_ID_LINE_ITEM_Id = index;
                Qty = Convert.ToDouble(readRow["PACK_ID_UNIT_QTY"]);
                
                foreach (DataColumn column in packingList.PACK_ID_LINE_ITEM.Columns)
                {
                    if (lineTable.Columns.Contains(column.ColumnName))
                    {
                        XmlElement xmlElemFileName2 = xmlDoc.CreateElement(column.ColumnName);
                        xmlElemFileName2.InnerText = readRow[column.ColumnName].ToString();
                        xmlElemFileName_1.AppendChild(xmlElemFileName2);
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
                XmlElement xmlElemFileName3 = xmlDoc.CreateElement("EXTD_BOX_WEIGHT");


                newLineRow.EXTD_BOX_WEIGHT = Convert.ToDouble(string.Format("{0:0.##}", newLineRow.PACK_ID_LINE_ITEM_BOX_QTY * Convert.ToDouble(weightTable.Rows[0]["Weight"])));
                xmlElemFileName3.InnerText = newLineRow.EXTD_BOX_WEIGHT.ToString();
                XmlElement xmlElemFileName4 = xmlDoc.CreateElement("PACK_ID_UNIT_UOM");
                xmlElemFileName4.InnerText = "EA";
                
                xmlElemFileName_1.AppendChild(xmlElemFileName3);
                xmlElemFileName_1.AppendChild(xmlElemFileName4);
                newLineRow.PACK_ID_UNIT_UOM = "EA";
                newLineRow.PACK_ID_Id = newRow.PACK_ID_Id;

                //packingList.PACK_ID_LINE_ITEM.Rows.Add(newLineRow);

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
                            XmlElement xmlElemFileName5 = xmlDoc.CreateElement(column.ColumnName);
                            xmlElemFileName5.InnerText = readRow[column.ColumnName].ToString();
                            xmlElemFileName_1.AppendChild(xmlElemFileName5);
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
                    //packingList.HP_PN_COMPONENTS.Rows.Add(newHPPNRow);

                }
                #region 生成DETAILL字段
                string whereClause = "WHERE PO_NUM='" + pakTable.Rows[0]["PO_NUM"].ToString() + "'order by ID,VALUE";
                DataTable miniTableName = DBFactory.PopulateTempTable("[v_PO_ITEM_DETAIL]", whereClause, fields);
                double KEY_DETAIL_RETAIL_UNIT_SH = 0.00;
                double KEY_DETAIL_RETAIL_ITEM_TAX = 0.00;
                double KEY_DETAIL_RETAIL_ITEM_CR = 0.00;
                double KEY_DETAIL_MERCHANDISE_PRICE_vaule = 0.00;
                int flag_index = miniTableName.Rows.Count-1;
                int flag_index_2=0;
                /*
                 *  if (flag_return)  //non returnable 版本
                        {
                            string value = readRow_DETAIL["VALUE"].ToString().Trim();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_VALUE.AppendChild(cdata);
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        else //regular 版本
                        {
                            UDF_DETAIL_VALUE.InnerText = readRow_DETAIL["VALUE"].ToString();
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                 * 
                 * 
                 * 
                 * 
                 */
                foreach (DataRow readRow_DETAIL in miniTableName.Rows)
                {



                    if (readRow_DETAIL["FIELDS"].ToString() == "KEY_DETAIL_RETAIL_UNIT_SH")
                    {
                        KEY_DETAIL_RETAIL_UNIT_SH = Convert.ToDouble(readRow_DETAIL["VALUE"].ToString());
                        XmlElement UDF_DETAIL = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement UDF_DETAIL_KEY = xmlDoc.CreateElement("KEY");
                        UDF_DETAIL_KEY.InnerText = readRow_DETAIL["FIELDS"].ToString();
                        UDF_DETAIL.AppendChild(UDF_DETAIL_KEY);
                        XmlElement UDF_DETAIL_VALUE = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                            string value = readRow_DETAIL["VALUE"].ToString().Trim();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_VALUE.AppendChild(cdata);
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        else
                        {
                            UDF_DETAIL_VALUE.InnerText = readRow_DETAIL["VALUE"].ToString();
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        xmlElemFileName_1.AppendChild(UDF_DETAIL);
                    }
                    else if (readRow_DETAIL["FIELDS"].ToString() == "KEY_DETAIL_RETAIL_ITEM_TAX")
                    {
                        KEY_DETAIL_RETAIL_ITEM_TAX = Convert.ToDouble(readRow_DETAIL["VALUE"].ToString());
                        XmlElement UDF_DETAIL = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement UDF_DETAIL_KEY = xmlDoc.CreateElement("KEY");
                        UDF_DETAIL_KEY.InnerText = readRow_DETAIL["FIELDS"].ToString();
                        UDF_DETAIL.AppendChild(UDF_DETAIL_KEY);
                        XmlElement UDF_DETAIL_VALUE = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                            string value = readRow_DETAIL["VALUE"].ToString().Trim();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_VALUE.AppendChild(cdata);
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        else
                        {
                            UDF_DETAIL_VALUE.InnerText = readRow_DETAIL["VALUE"].ToString();
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        xmlElemFileName_1.AppendChild(UDF_DETAIL);
                    }
                    else if (readRow_DETAIL["FIELDS"].ToString() == "KEY_DETAIL_RETAIL_ITEM_CR")
                    {
                        KEY_DETAIL_RETAIL_ITEM_CR = Convert.ToDouble(readRow_DETAIL["VALUE"].ToString());
                        XmlElement UDF_DETAIL = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement UDF_DETAIL_KEY = xmlDoc.CreateElement("KEY");
                        UDF_DETAIL_KEY.InnerText = readRow_DETAIL["FIELDS"].ToString();
                        UDF_DETAIL.AppendChild(UDF_DETAIL_KEY);
                        XmlElement UDF_DETAIL_VALUE = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                            string value = readRow_DETAIL["VALUE"].ToString().Trim();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_VALUE.AppendChild(cdata);
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        else
                        {
                            UDF_DETAIL_VALUE.InnerText = readRow_DETAIL["VALUE"].ToString();
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        xmlElemFileName_1.AppendChild(UDF_DETAIL);
                    }
                    else if (readRow_DETAIL["FIELDS"].ToString() == "KEY_DETAIL_RETAIL_UNIT_PRICE")
                    {
                        XmlElement KEY_DETAIL_RETAIL_UNIT_PRICE = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement UDF_DETAIL_1_KEY = xmlDoc.CreateElement("KEY");
                        UDF_DETAIL_1_KEY.InnerText = readRow_DETAIL["FIELDS"].ToString();
                        KEY_DETAIL_RETAIL_UNIT_PRICE.AppendChild(UDF_DETAIL_1_KEY);
                        XmlElement UDF_DETAIL_1_VALUE = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                            string value = readRow_DETAIL["VALUE"].ToString().Trim();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_1_VALUE.AppendChild(cdata);
                            KEY_DETAIL_RETAIL_UNIT_PRICE.AppendChild(UDF_DETAIL_1_VALUE);
                        }
                        else
                        {
                            UDF_DETAIL_1_VALUE.InnerText = readRow_DETAIL["VALUE"].ToString();
                            KEY_DETAIL_RETAIL_UNIT_PRICE.AppendChild(UDF_DETAIL_1_VALUE);
                        }
                        xmlElemFileName_1.AppendChild(KEY_DETAIL_RETAIL_UNIT_PRICE);

                        KEY_DETAIL_MERCHANDISE_PRICE_vaule = Convert.ToDouble(readRow_DETAIL["VALUE"]) * Qty;
                        XmlElement KEY_DETAIL_MERCHANDISE_PRICE = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement UDF_DETAIL_KEY_2 = xmlDoc.CreateElement("KEY");
                        UDF_DETAIL_KEY_2.InnerText = "KEY_DETAIL_MERCHANDISE_PRICE";
                        KEY_DETAIL_MERCHANDISE_PRICE.AppendChild(UDF_DETAIL_KEY_2);
                        XmlElement UDF_DETAIL_VALUE_2 = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                            string value = KEY_DETAIL_MERCHANDISE_PRICE_vaule.ToString();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_VALUE_2.AppendChild(cdata);
                            KEY_DETAIL_MERCHANDISE_PRICE.AppendChild(UDF_DETAIL_VALUE_2);
                        }
                        else
                        {
                            UDF_DETAIL_VALUE_2.InnerText = KEY_DETAIL_MERCHANDISE_PRICE_vaule.ToString();
                            KEY_DETAIL_MERCHANDISE_PRICE.AppendChild(UDF_DETAIL_VALUE_2);
                        }

                        xmlElemFileName_1.AppendChild(KEY_DETAIL_MERCHANDISE_PRICE);




                    }
                    else
                    {
                        XmlElement UDF_DETAIL = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement UDF_DETAIL_KEY = xmlDoc.CreateElement("KEY");
                        UDF_DETAIL_KEY.InnerText = readRow_DETAIL["FIELDS"].ToString();
                        UDF_DETAIL.AppendChild(UDF_DETAIL_KEY);
                        XmlElement UDF_DETAIL_VALUE = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                            string value = readRow_DETAIL["VALUE"].ToString().Trim();
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = value;
                            UDF_DETAIL_VALUE.AppendChild(cdata);
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        else
                        {
                            UDF_DETAIL_VALUE.InnerText = readRow_DETAIL["VALUE"].ToString();
                            UDF_DETAIL.AppendChild(UDF_DETAIL_VALUE);
                        }
                        xmlElemFileName_1.AppendChild(UDF_DETAIL);
                    }

                    if (flag_index_2 == flag_index)
                    {

                        double KEY_DETAIL_TOTAL_AMOUNT_PRICE_vaule = KEY_DETAIL_MERCHANDISE_PRICE_vaule + KEY_DETAIL_RETAIL_UNIT_SH + KEY_DETAIL_RETAIL_ITEM_TAX - KEY_DETAIL_RETAIL_ITEM_CR;
                        XmlElement KEY_DETAIL_TOTAL_AMOUNT_PRICE = xmlDoc.CreateElement("UDF_DETAIL");
                        XmlElement KEY_DETAIL_TOTAL_AMOUNT_PRICE_2 = xmlDoc.CreateElement("KEY");
                        KEY_DETAIL_TOTAL_AMOUNT_PRICE_2.InnerText = "KEY_DETAIL_TOTAL_AMOUNT_PRICE";
                        KEY_DETAIL_TOTAL_AMOUNT_PRICE.AppendChild(KEY_DETAIL_TOTAL_AMOUNT_PRICE_2);
                        XmlElement KEY_DETAIL_TOTAL_AMOUNT_PRICE_3 = xmlDoc.CreateElement("VALUE");
                        if (flag_return)
                        {
                           
                            XmlCDataSection cdata = xmlDoc.CreateCDataSection("VALUE");
                            cdata.InnerText = KEY_DETAIL_TOTAL_AMOUNT_PRICE_vaule.ToString();
                            KEY_DETAIL_TOTAL_AMOUNT_PRICE_3.AppendChild(cdata);
                            KEY_DETAIL_TOTAL_AMOUNT_PRICE.AppendChild(KEY_DETAIL_TOTAL_AMOUNT_PRICE_3);
                        }
                        else
                        {
                            KEY_DETAIL_TOTAL_AMOUNT_PRICE_3.InnerText = KEY_DETAIL_TOTAL_AMOUNT_PRICE_vaule.ToString();
                            KEY_DETAIL_TOTAL_AMOUNT_PRICE.AppendChild(KEY_DETAIL_TOTAL_AMOUNT_PRICE_3);
                        }
   
                        xmlElemFileName_1.AppendChild(KEY_DETAIL_TOTAL_AMOUNT_PRICE);
                       
                    }
                    
                    else {
                        flag_index_2++;
                    }
                }
                #endregion


                string whereBoxClause = "WHERE InternalID='" + readRow["InternalID"].ToString() + "'";
                DataTable boxTable = DBFactory.PopulateTempTable("[PAK_PackkingData]", whereBoxClause, fields);

                XmlElement box = xmlDoc.CreateElement("BOX");

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
                    //packingList.BOX.Rows.Add(newBoxRowPacking);

                    NewDataSet.SERIAL_NUMRow newSerialRowPacking = packingList.SERIAL_NUM.NewSERIAL_NUMRow();
                    newSerialRowPacking.BOX_Id_0 = index2;
                    newSerialRowPacking.SERIAL_NUM_Column = string.Empty;
                    //packingList.SERIAL_NUM.Rows.Add(newSerialRowPacking);

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
                        //packingList.BOX.Rows.Add(newBoxRow);
                        NewDataSet.SERIAL_NUMRow newSerialRow = packingList.SERIAL_NUM.NewSERIAL_NUMRow();
                        newSerialRow.BOX_Id_0 = index2;
                        newSerialRow.SERIAL_NUM_Column = readRow2["SERIAL_NUM"].ToString().Trim();

                        //packingList.SERIAL_NUM.Rows.Add(newSerialRow);
                     
                        index2++;

                        XmlElement box1 = xmlDoc.CreateElement("BOX_ID");
                        box1.InnerText = readRow2["BOX_ID"].ToString().Trim();
                        box.AppendChild(box1);
                        XmlElement box2 = xmlDoc.CreateElement("BOX_UNIT_QTY");
                        box2.InnerText = Convert.ToDouble(pakTable.Rows[0]["BOX_UNIT_QTY"]).ToString();
                        box.AppendChild(box2);
                        XmlElement box3 = xmlDoc.CreateElement("SERIAL_NUM");
                        box3.InnerText = readRow2["SERIAL_NUM"].ToString().Trim();
                        box.AppendChild(box3);
                        XmlElement box4 = xmlDoc.CreateElement("TRACK_NO_PARCEL");
                        box4.InnerText = readRow2["TRACK_NO_PARCEL"].ToString().Trim();
                        box.AppendChild(box4);
                        XmlElement box5 = xmlDoc.CreateElement("BOX_WEIGHT");
                        box5.InnerText = Convert.ToDouble(weightTable.Rows[0]["Weight"]).ToString();
                        box.AppendChild(box5);
                        xmlElemFileName_1.AppendChild(box);
                        xmlDoc.ChildNodes.Item(1).AppendChild(xmlElemFileName_1);
                        //xmlDoc.Save("E:\\cc2.xml");

                    }
                }

            }
            XmlTextWriter tr = new XmlTextWriter(xmlpath, null);
          
            tr.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(tr);
            tr.Close();
        }
    
    }
}
