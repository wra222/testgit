using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

namespace FOPWrap
{
    class GeneratePdf
    {
        public static void CreatePDF(string xmlPath, string Dn, string SerialNum,string Tp)
        {
           
            string fopPath = ConfigurationManager.AppSettings["FOPPath"];//--1
            //xmlPath --2
            string filename_PDF = "";
            string filename_XSL = "";
            switch (Tp)
            {
                case "PackingLabel" :
                    break;
                case "PackingShipmentLabel" :
                    break;
                case "BoxShipToLabel":
                    break;
                case "BoxShipToShipmentLabel":
                    break;
                case "PalletALabel":
                    break;
                default :
                    break;

            }
            string docName_PDF = ConfigurationManager.AppSettings["HPFilePath"] + @"\" + filename_PDF;
            string XslPath = ConfigurationManager.AppSettings["XSLFilePath"] + @"\" + filename_XSL;
            FOPWrap.FOP.GeneratePDF(
                          fopPath,
                          xmlPath,
                          XslPath,
                          docName_PDF);
        }
    }
}
