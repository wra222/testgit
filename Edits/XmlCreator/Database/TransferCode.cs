using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using IDAutomation.Windows.Forms.PDF417Barcode;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Windows;
//using Inheris.System.Web.UI.Page;



namespace Inventec.HPEDITS.XmlCreator.Database
{
    
    public class TransferCode
    {
        //[DllImport("idautomation.pdf417.dll")] 
        //static extern string 
       

        public static string Encoder(string StringIDValue)
        {                       
            PDF417Barcode NewBarcode = new PDF417Barcode();
            NewBarcode.Visible = false;
            ProcessStartInfo startInfo1 = new ProcessStartInfo();
            startInfo1.CreateNoWindow = true;
            startInfo1.WindowStyle = ProcessWindowStyle.Hidden;
            //http
             //Response.Write("<script language='javascript'>window.close();</script>");
             //string sScript = @"<script language='javascript'>forms.submit();</script>";
             //Page.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript);
             //HttpContext.Current.Resp
         
           // string StringValue = "";9,6
           // string StringValue = NewBarcode.FontEncoder(StringIDValue, 0, 0, 0, false, PDF417Barcode.PDF417Modes.Text, true);
            string StringValue = NewBarcode.FontEncoder(StringIDValue, 6, 9, 0, false, PDF417Barcode.PDF417Modes.Text, true);
            return StringValue.Replace("\0", ""); 
            //return StringValue.Replace("\r\n\0", "");  
            //return StringValue.Replace("\r\n","").Replace("\0","");  // \r \n ,'\r\n'
            
        }        
    }
}
