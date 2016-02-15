<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Print Service
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-12-19  Chen Xu (eB1-4)      Create 
 
 Known issues:
 --%>
 

<%@ WebService Language="C#" Class="PrintService" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using log4net;
using com.inventec.template;
using com.inventec.template.structure;
using com.inventec.imes.BLL;
using IMES.DataModel;



//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class PrintService  : System.Web.Services.WebService 
{

    IPrintTemplate iPrintLabelType = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);
    IList<PrintItem> PrintDataItem = new List<PrintItem>();
   
     
    ILog logger = log4net.LogManager.GetLogger("iMesLog");
    
    [WebMethod]
    public ArrayList GetLabelTypeList(string pCode)
    
   {
        ArrayList ret = new ArrayList();
        IList<string> labelTypeLst = new List<string>();
        
        try
        {
            labelTypeLst = iPrintLabelType.GetPrintLabelTypeList(pCode);

            string remoteBatPath=iPrintLabelType.GetRemoteBatPath(pCode);
            remoteBatPath = string.IsNullOrEmpty(remoteBatPath) ? "" : remoteBatPath;
            //ArrayList m = new ArrayList();              //testing
            //for (int i = 0; i < 6; i++)                 //testing
            //{                                           //testing       
            //    PrintInfo printInfo = new PrintInfo();  //testing
            //    printInfo.labelType = "c" + i;          //testing
            //    m.Add(printInfo.labelType);             //testing
            //}                                           //testing   

            ret.Add(WebConstant.SUCCESSRET);
            
            //ret.Add(m);                                 //testing

            ret.Add(labelTypeLst);
            ret.Add(remoteBatPath);         
            return ret;
        }
            
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }

        catch (Exception ex)
        {
            throw ex;
        }
            

    }


    
    //[WebMethod]
    //public ArrayList getPrintInfo(string stationid, printItemEntityForPrint curPrintItem, int i)
    //{
    //    try
    //    {
    //        ArrayList retList = new ArrayList();
    //   //     PrintTemplateInfo PrintTemplateInfoLst;
    //    //    PrintTemplateInfoLst = iPrintTemplate.GetPrintTemplateList(stationid,curPrintItem.LabelType);
    //        retList.Add(WebConstant.SUCCESSRET);
    //    //    retList.Add(PrintTemplateInfoLst);
    //        retList.Add(i);
    //        retList.Add(curPrintItem);
    //        return retList;
    //    }

    //    catch (FisException ex)
    //    {
    //        throw new Exception(ex.mErrmsg);
    //        logger.Error("getPrintTemplate exceptin: " + ex.mErrmsg + "  curprintItem:" + curPrintItem.TemplateName);
    //    }

    //    catch (Exception ex)
    //    {
    //        throw ex;
    //        logger.Error("getPrintTemplate exceptin: " + ex.Message + "  curprintItem:" + curPrintItem.TemplateName);
    //    }
    //}

    [WebMethod]
    public List<string> getBatContent(PrintItem batItem, string pCode, string clientBatFilePath)
    {
        List<string> ret = new List<string>();
        string lblType = string.IsNullOrEmpty(batItem.LabelType) ? string.Empty : batItem.LabelType.Trim();
        string batName = string.IsNullOrEmpty(batItem.TemplateName) ? string.Empty : batItem.TemplateName.Trim();

        ret.Add(lblType);
        ret.Add(batName);
        ret.Add(PrintControl.GetBatPrint(batItem, pCode, clientBatFilePath));
        
        return ret;
    }

    //[WebMethod]
    //public List<string> getBartenderContent(PrintItem batItem, string pCode)
    //{
    //    List<string> ret = new List<string>();
    //    string lblType = string.IsNullOrEmpty(batItem.LabelType) ? string.Empty : batItem.LabelType.Trim();
    //    string batName = string.IsNullOrEmpty(batItem.TemplateName) ? string.Empty : batItem.TemplateName.Trim();

    //    ret.Add(lblType);
    //    ret.Add(batName);
    //    ret.Add(PrintControl.GetBartenderPrint(batItem, pCode));
    //    ret.Add(batItem.BatPiece.ToString());
    //    ret.Add(batItem.PrinterPort);
		
    //    return ret;
    //}
	
    [WebMethod]
    public List<ImageInfo> getImageContent(PrintItem imgItem)
    {
        if (imgItem.PrintMode == 4)
        {
            return PrintControl.GetBartenderServicePrint(imgItem);
        }
        else
        {
            return PrintControl.GetTemplatePrint(imgItem, false);
        }
    }

    [WebMethod]
    public List<string> getBtwXMLScript(PrintItem btwItem, string pCode)
    {
        List<string> ret = new List<string>();
        string lblType = string.IsNullOrEmpty(btwItem.LabelType) ? string.Empty : btwItem.LabelType.Trim();
        string batName = string.IsNullOrEmpty(btwItem.TemplateName) ? string.Empty : btwItem.TemplateName.Trim();

        ret.Add(lblType);  //LabelType
        ret.Add(batName); //Template Name
        ret.Add(PrintControl.GetBartenderXMLScript(btwItem, pCode)); //XMLScript

        return ret;
    }

    public struct S_PrintService
    {
        public string LabelType;
        public string Template;
        public IList<NameValueDataTypeInfo> SpReturn;
        public string FilePath;
        public int Piece;
        public string Printer;
    }
	
	[WebMethod]
    public S_PrintService getBtwNameValueList(PrintItem btwItem, string pCode)
    {
        S_PrintService ret = new S_PrintService();
        string lblType = string.IsNullOrEmpty(btwItem.LabelType) ? string.Empty : btwItem.LabelType.Trim();
        string batName = string.IsNullOrEmpty(btwItem.TemplateName) ? string.Empty : btwItem.TemplateName.Trim();
		
        string btwFilePath=""; 
		IList<NameValueDataTypeInfo> spReturn = PrintControl.GetBartenderNameValueList(btwItem, pCode, out btwFilePath);
        
        ret.LabelType = lblType;
        ret.Template = batName;
        ret.SpReturn = spReturn;
        ret.FilePath = btwFilePath;
		ret.Piece = btwItem.BatPiece;
        ret.Printer = btwItem.PrinterPort;
        return ret;
    }
    
    //public class PrintInfo
    //{
    //    public string labelType;
       
    //}
}

