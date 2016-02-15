using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using Microsoft.Office.Interop.Excel;

namespace UTL
{
    public class GenExcelFile
    {
        public static void ExcelFromDataTable(System.Data.DataTable dt, string file)
        {
            // Create an Excel object and add workbook...
            ApplicationClass excel = new ApplicationClass();
            Workbook workbook = excel.Application.Workbooks.Add(Type.Missing);
            var worksheets = workbook.Sheets;
            var worksheet = (Worksheet)worksheets[1];

            // Add column headings...
            int iCol = 0;
            foreach (DataColumn c in dt.Columns)
            {
                iCol++;
                excel.Cells[1, iCol] = c.ColumnName.Trim();
            }

            WriteNumberFormatByColumn(dt.Rows.Count + 1, dt.Columns.Count + 1, worksheet);
            WriteArray(dt.Rows.Count, dt.Columns.Count, worksheet, dt.Rows);

            //// for each row of data...
            //int iRow = 0;
            //foreach (DataRow r in dt.Rows)
            //{
            //    iRow++;

            //    // add each row's cell data...
            //    iCol = 0;
            //    foreach (DataColumn c in dt.Columns)
            //    {
            //        iCol++;
            //        // data type format is Text
            //        ((Range)excel.Cells[iRow + 1, iCol]).NumberFormat = "@";
            //        excel.Cells[iRow + 1, iCol] = r[c.ColumnName].ToString().Trim();

            //    }
            //}

            // Global missing reference for objects we are not defining...
            object missing = System.Reflection.Missing.Value;

            //remove file
            //File.Delete(file);
            excel.DisplayAlerts = false;
            workbook.SaveAs(file,
                XlFileFormat.xlWorkbookNormal, missing, missing,
                false, false, XlSaveAsAccessMode.xlShared,
                false, false, missing, missing, missing);

            workbook.Close(false, missing, missing);

            excel.Quit();
            releaseObject(workbook);
            releaseObject(worksheets);
            releaseObject(excel);

        }

        public static void ExcelFromDataTable(List <DataRow> drList, 
                                                                       DataColumnCollection dcList, 
                                                                       string file)
        {
            // Create an Excel object and add workbook...
            ApplicationClass excel = new ApplicationClass();
          
            

            //Workbook workbook = excel.Application.Workbooks.Add(true);
            Workbook workbook = excel.Application.Workbooks.Add(Type.Missing);
            var worksheets = workbook.Sheets;
            var worksheet = (Worksheet)worksheets[1];

            // Add column headings...
            int iCol = 0;
            foreach (DataColumn c in dcList)
            {
                iCol++;
                excel.Cells[1, iCol] = c.ColumnName.Trim();
            }

            WriteNumberFormatByColumn(drList.Count + 1, dcList.Count + 1, worksheet);
            WriteArray(drList.Count, dcList.Count, worksheet, drList);
           

            // Global missing reference for objects we are not defining...
            object missing = System.Reflection.Missing.Value;

            //remove file
            //File.Delete(file);
            excel.DisplayAlerts = false;
            workbook.SaveAs(file,
                XlFileFormat.xlWorkbookNormal, missing, missing,
                false, false, XlSaveAsAccessMode.xlShared,
                false, false, missing, missing, missing);

            workbook.Close(false, missing, missing);
            
            excel.Quit();

            releaseObject(workbook);
            releaseObject(worksheets);
            releaseObject(excel);

        }


        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch 
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }


        private static void WriteArray(int rows, int columns, Worksheet worksheet,List<DataRow>  drList)
        {
            var data = new object[rows, columns];
            //var formatData = new object[rows, columns];
            for (var row = 1; row <= rows; row++)
            {
                for (var column = 1; column <= columns; column++)
                {
                    data[row - 1, column - 1] = drList[row-1][column - 1];
                   // formatData[row - 1, column - 1] = "@";
                }
            }

            var startCell = (Range)worksheet.Cells[2, 1];
            var endCell = (Range)worksheet.Cells[rows+1, columns];
            var writeRange = worksheet.get_Range(startCell, endCell);

            //writeRange.NumberFormat = formatData;
            writeRange.Value2 = data;
            
        }

        private static void WriteArray(int rows, int columns, Worksheet worksheet,  DataRowCollection drList)
        {
            var data = new object[rows, columns];
            
            for (var row = 1; row <= rows; row++)
            {
                for (var column = 1; column <= columns; column++)
                {
                    data[row - 1, column - 1] = drList[row - 1][column - 1];
                   
                }
            }

            var startCell = (Range)worksheet.Cells[2, 1];
            var endCell = (Range)worksheet.Cells[rows + 1, columns];
            var writeRange = worksheet.get_Range(startCell, endCell);

           
            writeRange.Value2 = data;

        }

        private static void WriteNumberFormatByColumn(int rows, int columns, Worksheet worksheet)
        {
            for (var column = 1; column <= columns; column++)
            {
                var startCell = (Range)worksheet.Cells[1, column];
                var endCell = (Range)worksheet.Cells[rows, column];
                var writeRange = worksheet.get_Range(startCell, endCell);

                writeRange.NumberFormat = "@";
            }
        }


        static public void XmlFromDataTable(List<DataRow> drList,
                                                                      DataColumnCollection dcList, 
                                                                      string file,
                                                                      string senderID)
        {
            XmlDocument doc = new XmlDocument();
            doc.PrependChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement element = doc.CreateElement("Data");
            element.SetAttribute("reportDate", DateTime.Now.ToString("MM/dd/yy HH:mm"));
            element.SetAttribute("senderID", senderID);
            doc.AppendChild(element);

            XmlElement devices = doc.CreateElement("Devices");
            element.AppendChild(devices);

            foreach (DataRow r in drList)
            {
                XmlElement device = doc.CreateElement("Device");

                // add each row's cell data...
                foreach (DataColumn c in dcList)
                {
                    insertElement(doc, device, c.ColumnName.Trim(), r[c.ColumnName].ToString().Trim());
                }
                devices.AppendChild(device);
            }
            doc.Save(file);
        }

        static public void XmlFromDataTable(System.Data.DataTable dt, 
                                                                      string file,
                                                                      string senderID)
        {
            XmlDocument doc = new XmlDocument();
            doc.PrependChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement element = doc.CreateElement("Data");
            element.SetAttribute("reportDate", DateTime.Now.ToString("MM/dd/yy HH:mm"));
            element.SetAttribute("senderID", senderID);
            doc.AppendChild(element);

            XmlElement devices = doc.CreateElement("Devices");
            element.AppendChild(devices);

            foreach (DataRow r in dt.Rows)
            {
                XmlElement device = doc.CreateElement("Device");

                // add each row's cell data...
                foreach (DataColumn c in dt.Columns)
                {
                   insertElement(doc, device, c.ColumnName.Trim(), r[c.ColumnName].ToString().Trim());
                 }
                devices.AppendChild(device);
            }
            doc.Save(file);
        }

        static private void insertElement(XmlDocument doc,
                                                            XmlElement element,
                                                            string name,
                                                            string value)
        {
            XmlElement elem = doc.CreateElement(name);
            elem.InnerText = value;
            element.AppendChild(elem);
        }

    }
}
