using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace Inventec.HPEDITS.XmlCreator
{
    public class XmlCreator
    {
        /// <summary>
        /// Schema path and file name
        /// </summary>
        protected string m_SchemaPath;//Ä£°åµØÖ·
        /// <summary>
        /// get the Schema path and file name
        /// </summary>
        public string SchemaPath
        {
            get { return m_SchemaPath; }
            //set 
            //{
            //    if (System.IO.File.Exists(value) == true)
            //    {
            //        m_SchemaPath	= value;
            //    }
            //}
        }

        /// <summary>
        /// store data item filter by Schema
        /// </summary>
        protected DataSet m_DataSet;
        /// <summary>
        /// get data set 
        /// </summary>
        public DataSet DataSet
        {
            get { return m_DataSet; }
            //set { m_DataSet = value; }
        }

        protected SqlConnection m_Connection;
        protected SqlDataAdapter m_Adapter;

#if DEBUG
        private static NLog.Logger m_Nlogger = NLog.LogManager.GetCurrentClassLogger();
#endif


        /// <summary>
        /// Reads an XML schema into the DataSet. 
        /// </summary>
        /// <param name="aSchemaPath">schema file path and file name</param>
        /// <returns></returns>
        public bool LoadSchema(string aSchemaPath)
        {
#if DEBUG
            try
            {
#endif

                if (System.IO.File.Exists(aSchemaPath) == false)
                {
                    return false;
                }
                this.m_SchemaPath = aSchemaPath;

                if ((this.m_DataSet != null) && (string.IsNullOrEmpty(this.m_SchemaPath) == false))
                {
                    this.m_DataSet.ReadXmlSchema(aSchemaPath);
                    return true;
                }
                return false;

#if DEBUG
            }
            catch (Exception ex)
            {
                m_Nlogger.Info(string.Format("{0} \n\t {1}", "LoadSchema", ex.Message));
                return false;
            }
#endif
        }


        /// <summary>
        /// Utilize schema load data from database
        /// </summary>
        public virtual bool LoadData(string aKey)
        { return false; }
        public virtual bool LoadData(string aKey, string aSubKey)
        { return false; }
        public virtual void LoadData(int id)
        { }

        /// <summary>
        /// constructor
        /// </summary>
        public XmlCreator()
        {
#if DEBUG
            try
            {
#endif

                this.m_Connection = new SqlConnection("data source=10.99.183.27;Initial Catalog=HP_EDI;User ID=HPEDI;password=hpedi;");
                //this.m_Connection	= new SqlConnection(@"data source=HomeServer\SQLEXPRESS,1535;Initial Catalog=iWHS;User ID=sa;password=ies+123;Integrated Security=False;");
                this.m_DataSet = new DataSet();

#if DEBUG
            }
            catch (Exception ex)
            {
                m_Nlogger.Info(string.Format("{0} \n\t {1}", "XmlCreator", ex.Message));
            }
#endif
        }


        /// <summary>
        /// write xml file use dataset data
        /// </summary>
        /// <returns></returns>
        public bool WriteXml()
        {
#if DEBUG
            try
            {
#endif

                if (this.m_DataSet != null)
                {
                    FileInfo theSchemaFileInfo = new FileInfo(this.m_SchemaPath);
                    string theXmlPath = string.Format("{0}.xml", theSchemaFileInfo.Name);

                    this.m_DataSet.WriteXml(theXmlPath);
                    return true;
                }
                return false;

#if DEBUG
            }
            catch (Exception ex)
            {
                m_Nlogger.Info(string.Format("{0} \n\t {1}", "WriteXml", ex.Message));
                return false;
            }
#endif
        }
        /// <summary>
        /// write xml file use dataset data
        /// </summary>
        /// <param name="aXmlPath">set xml path and file name</param>
        /// <returns></returns>
        public bool WriteXml(string aXmlPath, bool useOuter)
        {
            XmlTextWriter writer = new XmlTextWriter(aXmlPath, Encoding.UTF8);
#if DEBUG
            try
            {
#endif
                // Create a writer of your own                
                writer.Formatting = Formatting.Indented;

                System.Xml.XmlDataDocument data = GetXML(useOuter);
                data.WriteContentTo(writer);
                return true;
#if DEBUG
            }
            catch (Exception ex)
            {
                m_Nlogger.Info(string.Format("{0} \n\t {1}", "WriteXml", ex.Message));
                throw;
            }
            finally
            {
                writer.Close();
            }
            return false;
#endif
        }
        public XmlDataDocument GetXML(bool useOuter)
        {
            try
            {
                if (this.m_DataSet != null)
                {

                    // Use the XmlDataDocument to write your dataset instead of DataSet
                    System.Xml.XmlDataDocument xmlDocument = new XmlDataDocument();

                    xmlDocument.LoadXml(this.m_DataSet.GetXml());

                    System.Xml.XmlDataDocument xmlDocument2 = new XmlDataDocument();
                    if (useOuter)
                    {    xmlDocument2.LoadXml(xmlDocument.InnerXml);
                       if (xmlDocument2.DocumentElement.Name == "BOXES")
                           {
                          XmlNodeList boxList = xmlDocument2.GetElementsByTagName("BOX");
                          foreach (XmlNode boxNode in boxList)
                           {
                               for (int i = 0; i < 3; i++)
                               {
                                   //XmlNode head = boxNode.SelectSingleNode("CONFIG_ID_NUMBER");
                                   XmlNode head = boxNode.ChildNodes[77];
                                   boxNode.RemoveChild(head);
                                   boxNode.InsertBefore(head, boxNode.SelectSingleNode("UDF_BOX"));
                                }
                             }

                         }
                         else if (xmlDocument2.DocumentElement.Name == "PALLETS")
                         {
                             XmlNodeList boxList = xmlDocument2.GetElementsByTagName("BOX");
                             if (boxList.Count>0)
                             {

                             foreach (XmlNode boxNode in boxList)
                             {
                                 for (int i = 0; i < 3;i++)
                                 {
                                     //XmlNode head = boxNode.SelectSingleNode("CONFIG_ID_NUMBER"); Pallet  B
                                     XmlNode head = boxNode.ChildNodes[2];
                                     boxNode.RemoveChild(head);
                                     boxNode.InsertAfter(head, boxNode.LastChild);
                                 }
                                 
                             }
                      
                           }

                         }
                      }
                       
                    else
                    {
                        //is packing list
                        xmlDocument2.LoadXml(xmlDocument.ChildNodes[0].InnerXml);

                        if (xmlDocument2.ChildNodes[0].Name == "PACK_ID")
                        {
                            XmlNodeList boxList = xmlDocument2.GetElementsByTagName("BOX");
                            foreach (XmlNode boxNode in boxList)
                            {
                                if (boxNode.ChildNodes.Count >= 6)
                                {
                                    //if we have more than 5 child, more than 0 serial num
                                    //for (int i = 5; i < boxNode.ChildNodes.Count; i++)
                                    for (int i = 5; i < 6; i++)
                                    {
                                      //XmlNode head = boxNode.SelectSingleNode("UDF_BOX");
                                        XmlNode serialNumNode = boxNode.ChildNodes[i];
                                        boxNode.RemoveChild(serialNumNode);
                                        boxNode.InsertAfter(serialNumNode, boxNode.ChildNodes[0]);
                                    }
                                     //----------  for smart PC test 20100429-------------------
                                   
                                   /* for (int i = 1; i < 4; i++)
                                    {
                                        XmlNode serialNumNode = boxNode.ChildNodes[i];
                                        boxNode.RemoveChild(serialNumNode);
                                        boxNode.InsertAfter(serialNumNode, boxNode.SelectSingleNode("SERIAL_NUM"));
                                    }*/
                                     
                                }
                            }
                        }
                    }
                    return xmlDocument2;
                }
                return null;
            }
            catch (Exception ex)
            {
                m_Nlogger.Info(string.Format("{0} \n\t {1}", "WriteXml", ex.Message));
                throw;
            }
            return null;
        }
        public void WriteSchemaToConsole()
        {
            foreach (DataTable table in this.m_DataSet.Tables)
            {
                Console.WriteLine("Table Name: " + table.TableName);
                foreach (DataColumn column in table.Columns)
                {
                    Console.WriteLine("  Column Name: " + column.ColumnName);
                }
            }
        }

        
    }
}
