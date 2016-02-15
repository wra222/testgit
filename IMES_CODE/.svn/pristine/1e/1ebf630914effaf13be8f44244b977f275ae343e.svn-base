/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  iMES Print Control
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-02-01 Yuan XiaoWei           Create 
 * 2010-04-10 Yuan XiaoWei          ITC-1103-0326
 * Known issues:
 */

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using com.inventec.template;
using com.inventec.template.structure;
using com.inventec.imes.BLL;
using log4net;
using System.Text;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using com.inventec.imes;

using IMES.DataModel;

namespace com.inventec.iMESWEB
{
    /// <summary>
    /// Summary description for PrintControl
    /// </summary>
    public static class PrintControl
    {

        //private static IPrintItem currentPrintItemService = ServiceAgent.getInstance().GetObjectByName<IPrintItem>(WebConstant.CommonObject);
        //private static LabelManager printRoomLlbManager = ServiceAgent.getInstance().getPrintTemplateObject(true);
        //private static LabelManager commonLlbManager = ServiceAgent.getInstance().getPrintTemplateObject(false);
        //private static IPrintTemplate printTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);
        //private static LabelManager bartenderManager = ServiceAgent.getInstance().getBartenderPrintTemplateObject();

         private static IPrintItem currentPrintItemService = null;
        private static LabelManager printRoomLlbManager = null;
        private static LabelManager commonLlbManager = null;
        private static IPrintTemplate printTemplate = null;
        private static LabelManager bartenderManager =null;

        #region  initialization Remoting object
        private static IPrintItem getPrintService()
        {
            if (currentPrintItemService==null)
            {
                currentPrintItemService = ServiceAgent.getInstance().GetObjectByName<IPrintItem>(WebConstant.CommonObject);
            }
            return currentPrintItemService;
        }

        private static LabelManager getPrintRoomLlbManager()
        {
            if (printRoomLlbManager == null)
            {
                printRoomLlbManager = ServiceAgent.getInstance().getPrintTemplateObject(true);
            }
            return printRoomLlbManager;
        }

        private static LabelManager getCommonLlbManager()
        {
            if (commonLlbManager == null)
            {
                commonLlbManager = ServiceAgent.getInstance().getPrintTemplateObject(false);
            }
            return commonLlbManager;
        }

        private static IPrintTemplate getPrintTemplate()
        {
            if (printTemplate == null)
            {
                printTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);
            }
            return printTemplate;
        }

        private static LabelManager getbartenderManager()
        {
            if (bartenderManager == null)
            {
                bartenderManager = ServiceAgent.getInstance().getBartenderPrintTemplateObject();
            }
            return bartenderManager;
        }
        #endregion

        public static string GetBatPrint(PrintItem currentPrinItem, string pCode)
        {
            string batFilePath = System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"];
            string result = null;
            if (!string.IsNullOrEmpty(currentPrinItem.SpName) && currentPrinItem.PrintMode == 0)
            {
                currentPrinItem.ParameterKeys.Add("@x");
                currentPrinItem.ParameterKeys.Add("@y");
                //currentPrinItem.ParameterKeys.Add("@bat");
                List<string> xoffset = new List<string>();
                List<string> yoffset = new List<string>();
                //List<string> bat = new List<string>();
                xoffset.Add(currentPrinItem.OffsetX.ToString());
                yoffset.Add(currentPrinItem.OffsetY.ToString());
                //bat.Add(currentPrinItem.TemplateName);
                currentPrinItem.ParameterValues.Add(xoffset);
                currentPrinItem.ParameterValues.Add(yoffset);
                //currentPrinItem.ParameterValues.Add(bat);
               // result = currentPrintItemService.GetMainBat(currentPrinItem.SpName, currentPrinItem.ParameterKeys, currentPrinItem.ParameterValues);
                result = getPrintService().GetMainBat(currentPrinItem.SpName, currentPrinItem.ParameterKeys, currentPrinItem.ParameterValues);

                //Get Remote bat path
                string remoteBatPath = getPrintTemplate().GetRemoteBatPath(pCode);

                if (string.IsNullOrEmpty(remoteBatPath))
                {
                    remoteBatPath = batFilePath;
                }
                
                //Vincent [2014-03-24]: Set Sequence
                //result = result + "\r\nSET BATPATH=" + remoteBatPath;                         

                //result = result + "\r\nSET PORT=" + currentPrinItem.PrinterPort + "\r\npushd \"" + batFilePath + "\"\r\nCALL \"" + remoteBatPath + "\\" + currentPrinItem.TemplateName + "\"";

                string appendHeader = "SET BATPATH=" + remoteBatPath + "\r\nSET PORT=" + currentPrinItem.PrinterPort + "\r\npushd \"" + batFilePath + "\"\r\n";

                result = appendHeader + result;
                
                //result = result + "\"\r\nCALL \"" + remoteBatPath + "\\" + currentPrinItem.TemplateName + "\"";

               
                if (currentPrinItem.BatPiece > 0)
                {
                    StringBuilder copyCall = new StringBuilder();
                    for (int i = 0; i < currentPrinItem.BatPiece; i++)
                    {
                        copyCall.Append("\r\nCALL \"" + remoteBatPath + "\\" + currentPrinItem.TemplateName + "\"");
                    }

                    result = result + copyCall.ToString() + "\r\npopd";
                }
            }
            return result;

        }

        //public static string GetBartenderPrint(PrintItem currentPrinItem, string pCode)
        //{
        //    string batFilePath = System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"];
        //    string result = null;
        //    if (!string.IsNullOrEmpty(currentPrinItem.SpName) && currentPrinItem.PrintMode == 3)
        //    {
        //        result = getPrintService().GetMainBat(currentPrinItem.SpName, currentPrinItem.ParameterKeys, currentPrinItem.ParameterValues);
        //    }
        //    return result;

        //}

        public static List<ImageInfo> GetTemplatePrint(PrintItem currentPrinItem, Boolean isPrintRoom)
        {
            List<ImageInfo> result = null;
            if (!string.IsNullOrEmpty(currentPrinItem.TemplateName) && currentPrinItem.PrintMode == 1)
            {

                List<ParamInfo> inputParas = new List<ParamInfo>();
                for (int i = 0; i < currentPrinItem.ParameterKeys.Count; i++)
                {
                    ParamInfo newParam = new ParamInfo();
                    newParam.ParamName = currentPrinItem.ParameterKeys[i];
                    newParam.Values = currentPrinItem.ParameterValues[i];
                    inputParas.Add(newParam);
                }

                if (isPrintRoom)
                {
                    if (string.IsNullOrEmpty(currentPrinItem.dpi))
                    {

                        result = getPrintRoomLlbManager().getImageListForPrint(currentPrinItem.TemplateName, null, inputParas, isPrintRoom);
                    }
                    else
                    {
                        result = getPrintRoomLlbManager().getImageListForPrint(currentPrinItem.TemplateName, null, inputParas, isPrintRoom, currentPrinItem.dpi);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(currentPrinItem.dpi))
                    {

                        result = getCommonLlbManager().getImageListForPrint(currentPrinItem.TemplateName, null, inputParas, isPrintRoom);
                    }
                    else
                    {
                        result = getCommonLlbManager().getImageListForPrint(currentPrinItem.TemplateName, null, inputParas, isPrintRoom, currentPrinItem.dpi);
                    }
                }

            }
            return result;

        }       

        public static string GetBartenderXMLScript(PrintItem currentPrinItem, string pCode)
        {
            string btwFilePath = System.Configuration.ConfigurationManager.AppSettings["ClientBartenderFilePath"];
            string result = "";
            if ( !string.IsNullOrEmpty(currentPrinItem.TemplateName) &&
                 !string.IsNullOrEmpty(currentPrinItem.SpName) && 
                currentPrinItem.PrintMode == 3)
            {
              
                currentPrinItem.ParameterKeys.Add("@Bartender");
                currentPrinItem.ParameterValues.Add(new List<string> {currentPrinItem.TemplateName});

                IList<NameValueDataTypeInfo> valueList = getPrintService().GetBartenderNameValueList(currentPrinItem.SpName, currentPrinItem.ParameterKeys, currentPrinItem.ParameterValues);

                //Get Remote bat path
                string remotePath = getPrintTemplate().GetRemoteBartenderPath(pCode);

                if (string.IsNullOrEmpty(remotePath))
                {
                    remotePath = btwFilePath;
                }

               //產生xml script 
                result = SerializeXMLUtility.GenBtwXMLScript(remotePath,currentPrinItem,valueList);
            }
            return result;
        }

        public static IList<NameValueDataTypeInfo> GetBartenderNameValueList(PrintItem currentPrinItem, string pCode,out string btwFilePath)
        {
			btwFilePath = System.Configuration.ConfigurationManager.AppSettings["ClientBartenderFilePath"];
            IList<NameValueDataTypeInfo> result = new List<NameValueDataTypeInfo>();
            if (!string.IsNullOrEmpty(currentPrinItem.TemplateName) &&
                 !string.IsNullOrEmpty(currentPrinItem.SpName) &&
                currentPrinItem.PrintMode == 3)
            {

                currentPrinItem.ParameterKeys.Add("@Bartender");
                currentPrinItem.ParameterValues.Add(new List<string> { currentPrinItem.TemplateName });

                result = getPrintService().GetBartenderNameValueList(currentPrinItem.SpName, currentPrinItem.ParameterKeys, currentPrinItem.ParameterValues);

                //Get Remote bat path
                string remotePath = getPrintTemplate().GetRemoteBartenderPath(pCode);

                if (!string.IsNullOrEmpty(remotePath))
                {
                    btwFilePath = remotePath;
                }

            }
            return result;
        }

        public static List<ImageInfo> GetBartenderServicePrint(PrintItem currentPrinItem)
        {
            List<ImageInfo> result = null;
            if (!string.IsNullOrEmpty(currentPrinItem.TemplateName) &&
                 !string.IsNullOrEmpty(currentPrinItem.SpName) &&
                 currentPrinItem.PrintMode == 4)
            {

                List<ParamInfo> inputParas = new List<ParamInfo>();
                for (int i = 0; i < currentPrinItem.ParameterKeys.Count; i++)
                {
                    ParamInfo newParam = new ParamInfo();
                    newParam.ParamName = currentPrinItem.ParameterKeys[i];
                    newParam.Values = currentPrinItem.ParameterValues[i];
                    inputParas.Add(newParam);
                }

                inputParas.Add(new ParamInfo
                {
                    ParamName = "@Bartender",
                    Values = new List<string> { currentPrinItem.TemplateName }
                });

                string templateName = currentPrinItem.TemplateName + "~" + currentPrinItem.SpName;
                if (string.IsNullOrEmpty(currentPrinItem.dpi))
                {
                    result = getbartenderManager().getImageListForPrint(templateName, null, inputParas, false);
                }
                else
                {
                    result = getbartenderManager().getImageListForPrint(templateName, null, inputParas, false, currentPrinItem.dpi);
                }

            }
            return result;

        }

    }

}