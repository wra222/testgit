<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Reprint Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-10   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. Reprint Pallet Verify 列印所有Label；
 *           
 */
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReprintPalletVerify.aspx.cs" Inherits="PAK_ReprintPalletVerify"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path=  "~/PAK/Service/WebServicePalletVerify.asmx" />
    </Services>
</asp:ScriptManager>   
<style type="text/css">
</style>

<div id="divPalletVerify" style="z-index: 0; width:95%" class="iMes_div_center">
       
        <br />
        
        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr style="height: 30px" align="left" valign="middle">
                <td width="20%">
                   &nbsp;   <asp:Label id="lblPalletNo" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td width="80%">
                        <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                            InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/>
                             
                </td>
             </tr>          
             <tr><td colspan="2"> &nbsp; </td></tr>
             <tr>   
                <td width="20%" align="left" >
                   &nbsp;   <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                 
                <td width="80%" align="left" >
                        <textarea id="txtReason" rows="5" style="width:98%;" 
                    runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                    onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="2" cols="20" name="S1"></textarea></td>
             </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
             <tr >
                 <td width="20%" align="left"></td>
                 <td align="right">
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                            tabindex ="3"/> 
                            &nbsp; 
                </td>
                
             </tr>
             <tr><td colspan="2">&nbsp;</td></tr>
              <tr>
                 <td width="20%" align="left"></td>
                 <td align="right" >
                    <input id="btnPrint" type="button"  runat="server" onclick="Reprint()" style="height:auto"
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    class=" iMes_button"  tabindex="4"/>
                     &nbsp; 
               </td>
             </tr>
           
            
        </table>
        
        <br />        
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel> 
    </div>

 <script type="text/javascript" language="javascript">

     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
     var msgCallEditsFail = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCallEditsFail") %>';
     var msgCreatePDFFail = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCreatePDFFail") %>';

     var msgPalletNoNull = '<%=this.GetLocalResourceObject(Pre + "_msgPalletNoNull").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
     var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
     var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
     var msgNullPDFFiles = '<%=this.GetLocalResourceObject(Pre + "_msgNullPDFFiles").ToString() %>';
     var line = "";
     var station = "";
     var palletNo = "";
     var strReason = "";
     var inputSNControl;
     var station = '<%=Request["Station"] %>';
     var pcode = '<%=Request["PCode"] %>';
     var editor = "<%=UserId%>";
     var customer = "<%=Customer%>";
     var PDFPLLst = new Array();
     var DeliveryPerPalletList = new Array();
     var modelList = new Array();
     var arrTemp = new Array();
     var PdfFilename = "";
     var XmlFilename = "";
     var Templatename = "";
     var printFlag =false;
         
     window.onload = function() {
         inputSNControl = getCommonInputObject();
         DisplayInfoText();
         setStart();
     };

     function setStart() {
         palletNo = "";
         inputSNControl.focus();
         
         getAvailableData("processDataEntry");

     }

     function checkReprintReason()      
     {
         strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

//         if ((strReason == "") || (strReason.length == 0) || (strReason == null)) {
//             ShowInfo(msgReasonNull);
//             PlaySound();
//             reasonFocus();
//             return false;
//         }
//         else return true;
         return true;
     }
     
     function Reprint() {
         PlaySoundClose();
         ShowInfo("");
       
        try
         {       
         
          if (inputSNControl.value != "") {
             if (checkReprintReason()) {
                 var printItemlist = getPrintItemCollection();

                 if (printItemlist == "" || printItemlist == null) {
                     ShowInfo(msgPrintSettingPara);
                     PlaySound();
                     return;
                 }

                 beginWaitingCoverDiv();
                 palletNo = inputSNControl.value.trim();
                 WebServicePalletVerify.reprint(palletNo, strReason, line, editor, station, customer, printItemlist, onSucceed, onFailed);
             }
             else {
                 getAvailableData("processDataEntry");
             }
         }
         else {
             ShowInfo(msgPalletNoNull);
             PlaySound();
             inputSNControl.value = "";
             inputSNControl.focus();
             getAvailableData("processDataEntry");
           }

     }
     catch (e) {
     }
         
     }

     function processDataEntry(inputStr) {
         Reprint();
     }

     function ismaxlength(obj) {
         var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
         if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");
             ShowInfo(msgInputMaxLength1 + mlength + msgInputMaxLength2);
             PlaySound();
             obj.value = obj.value.substring(0, mlength);
             reasonFocus();
         }
     }

     function imposeMaxLength(obj) {
         var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
         return (obj.value.length < mlength);
     }

     function reasonFocus() {
         document.getElementById("<%=txtReason.ClientID %>").focus();
     }

     function clearReason() {
         document.getElementById("<%=txtReason.ClientID %>").value = "";
         strReason = "";
     }


     function onSucceed(result) {
         endWaitingCoverDiv();
         if (result == null) {
             
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == SUCCESSRET) {
            DeliveryPerPalletList = result[2];
            PDFPLLst = result[4];
            modelList = result[5];
         //========================================print===========================================
             //// "A" 自动单：Pallet Label ; "C" 手动单:Pallet Label ; "X" 手动单:Pallet Label + CPMO Label
             if (result[3] == "A") {
                 // PDF打印，无参数传递,Non-bulk  Pallet Label for EDITS order
                 //PDFPrint();
				 printFlag = false;
                 WebServicePalletVerify.callTemplateCheckLaNew(DeliveryPerPalletList[0], "Pallet Ship Label- Pack ID Single", onCallOPSucceed, onFailed);

             }
             else if (result[3] == "C" || result[3] == "X") {
             
                 for (var i = 0; i < result[2].length; i++) {
                     for (var j = 0; j < result[1].length; j++) {
                         if (result[1][j].LabelType == "Non-bulk Pallet Label for Manual order") {
                             setPrintItemListParam(result[1][j], result[2][i], "Non-bulk Pallet Label for Manual order");    //手动单:Pallet Label
                            // printLabels(result[1][j], false);
                         }
                     }
                     if (result[3] == "X") {
                         for (var j = 0; j < result[1].length; j++) {
                             if (result[1][j].LabelType == "Non-bulk CPMO Label for Manual order") {
                                 setPrintItemListParam(result[1][j], result[2][i], "Non-bulk CPMO Label for Manual order");   //手动单:CPMO Label
                              //   printLabels(result[1][j], false);
                             }
                         }
                     }
                    // printLabels(result[1], false);
                 }

                 var SuccessItem = "[" + palletNo + "]"; 
                 
                 ResetPage();
                 ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
             }

             
         }
     }

     
     function onFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     function setPrintItemListParam(backPrintItemList, dn, labeltype) {
         //============================================generate PrintItem List==========================================
         //        /*
         //         * Function Name: setPrintParam
         //         * @param: printItemCollection
         //         * @param: labelType
         //         * @param: keyCollection(Client: Array of string.    Server: List<string>)
         //         * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
         //         */
         var lstPrtItem = new Array();
         lstPrtItem[0] = backPrintItemList;

         var keyCollection = new Array();
         var valueCollection = new Array();

         keyCollection[0] = "@palletno";
         keyCollection[1] = "@deliveryno"

         valueCollection[0] = generateArray(palletNo);
         valueCollection[1] = generateArray(dn);

         setPrintParam(lstPrtItem, labeltype, keyCollection, valueCollection);

         printLabels(lstPrtItem, false);
     }

     function onCallOPSucceed(result) {
         if (result == null) {
             //service方法没有返回
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == SUCCESSRET) {
             //CP170000049
             var rowsCount = result[2];
             var colsCount = result[3];
             var begNo = 0;
             var endNo = result[1].length;
             arrTemp = new Array();
             for (var i = 0; i < rowsCount; i++) {
                 arrTemp[i] = new Array();
                 for (var j = 0; j < colsCount; j++) {
                     if (begNo < endNo) {
                         arrTemp[i][j] = result[1][begNo];
                         begNo++;
                     }
                 }
             }

             generatePDFinFirstScanCustSN();
             PDFPrint();
         }
         else {
             var content = result[0];
             ShowErrorMessage(content);
         }
     }

     function generatePDFinFirstScanCustSN() {

         try {
             var plt = palletNo;
             var arrDelivery = new Array();
             arrDelivery = DeliveryPerPalletList;
             var edi_Delivery = DeliveryPerPalletList[0];


             if (arrTemp != null && arrTemp.length > 0) {

                 var rowsCount = arrTemp.length;


                 if (rowsCount > 1) {
                     for (var TpCount = 0; TpCount < rowsCount; TpCount++) {
                         var Template = arrTemp[TpCount][0].trim();
                         var DOC_SET = arrTemp[TpCount][2].trim();
                         var Sntp = "";
                         var Attp = "";
                         var plttp = "";

                         //if (DOC_SET.search("LA") != -1 || DOC_SET.search("NA-00010") != -1 || DOC_SET.search("EM") != -1) {
                             if (Template.search("Serial") != -1) {
                                 Sntp = "SN";
                             }
                             else if (Template.search("General") != -1) {
                                 Sntp = "Ship";
                             }
                             else if (Template.search("BOX_ID") != -1) {
                                 Sntp = "EMEA";
                             }
                             else Sntp = "";
                         //}

                         var TCount = 0;

                         if (Template.search("_ATT") != -1) {
                             Attp = "ATT";
                         }
                         else if (Template.search("Verizon2D") != -1) {
                             Attp = "2D";
                         }
                         else Attp = "";

                         if (Template.search("TypeB") != -1) {
                             plttp = "B";
                             CmdGeneratePdf(edi_Delivery, plt, "PLT", Template, Attp, TCount, Sntp);
                         }
                         else {
                             plttp = "A";

                             var arrDNlength = arrDelivery.length;
                             for (var DNCount = 0; DNCount < arrDNlength; DNCount++) {
                                 CmdGeneratePdf(arrDelivery[DNCount], plt, "PLT", Template, Attp, TpCount, Sntp);
                             }
                         }

                     }
                 }
                 else {
                     var Template = arrTemp[0][0].trim();
                     var Sntp = "";
                     var Attp = "";
                     var TpCount = 0;
                     var plttp = "";

                     if (Template.search("TypeB") != -1) {
                         plttp = "B";
                         CmdGeneratePdf(edi_Delivery, plt, "PLT", Template, Attp, TpCount, Sntp);
                     }
                     else {
                         plttp = "A";

                         var arrDNlength = arrDelivery.length;
                         for (var DNCount = 0; DNCount < arrDNlength; DNCount++) {
                             CmdGeneratePdf(arrDelivery[DNCount], plt, "PLT", Template, Attp, TpCount, Sntp);
                         }
                     }
                 }
             }
         }
         catch (e) {
             PlaySound();
             ResetPage();
             ShowInfo(e);

         }

     }

     function CmdGeneratePdf(DeliveryNo, Pallet, plttp, Template, Attp, TCount, Sntp) {
         try {
             var cmdGeneratePdf = false;

             Templatename = Template;

             if (Template.search("TypeB") != -1) {
                 plttp = "B";
             }
             else plttp = "A";


             XmlFilename = DeliveryNo + "-" + Pallet + "-[PalletShipLabel]" + plttp.trim() + Sntp.trim() + ".xml";



             if (Attp == "ATT") {
                 PdfFilename = DeliveryNo + "-" + Pallet + "-[ATTPalletShipLabel]" + Sntp.trim() + ".pdf";
             }
             else if (Attp == "2D") {
                 PdfFilename = DeliveryNo + "-" + Pallet + "-[2DPalletShipLabel]" + Sntp.trim() + ".pdf";
             }
             else {
                 PdfFilename = DeliveryNo + "-" + Pallet + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
             }

             // para_transfer();

             if (TCount == 0) {

                 GenerateCaseSGTIN96(DeliveryNo, Pallet, plttp);

             }

             //generatePDF();
             cmdGeneratePdf = true;

         }
         catch (e) {
             PlaySound();
             ResetPage();
             ShowInfo(e);

         }

     }

     function GenerateCaseSGTIN96(DeliveryNo, PLT, LType) {

         if (CallEDITSFunc(DeliveryNo, PLT, LType)) {

             if (CallPdfCreateFunc()) {
                 if (!printFlag){
				 //PDFPrint();
				 }
             }
         }
     }


     function CallEDITSFunc(dn, plt, LType) {
         var Paralist = new EDITSFuncParameters();
         var xmlpathfile = "";
         var webEDITSaddr = "";
         if (GetDebugMode()) {
             //Debug Mode get Root path from Web.conf
             xmlpathfile = GetCreateXMLfileRootPath() + "PLTXML\\" + XmlFilename;
             CheckMakeDir(xmlpathfile);
             //webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
             webEDITSaddr = GetEDITSTempIP();   //Packing List for aaaa
         }
         else {
             //Run Mode Get Path from DB, set Full Path
             xmlpathfile = PDFPLLst[2] + "PLTXML\\" + XmlFilename;
             webEDITSaddr = PDFPLLst[0];
             //webEDITSaddr = "http://10.190.40.68:8080/edits.asmx"; // Packing List for aaa
         }
         Paralist.add(1, "FilePH", xmlpathfile);
         Paralist.add(2, "Dn", dn);
         Paralist.add(3, "Pallet", plt);
         if (LType == "A") {
             var IsSuccess = invokeEDITSFunc(webEDITSaddr, "PalletAShipmentLabel", Paralist);
         }
         else var IsSuccess = invokeEDITSFunc(webEDITSaddr, "PalletBShipmentLabel", Paralist);
         if (!IsSuccess) {
             PlaySound();
             ShowInfo(palletNo + " " + msgCallEditsFail);
         }
         return IsSuccess;
     }

     function CallPdfCreateFunc() {
         var xmlfilename, xslfilename, pdffilename;

         if (GetDebugMode()) {
             //Debug Mode get Root path from Web.conf
             xmlfilename = "PLTXML\\" + XmlFilename;
             xslfilename = Templatename;
             pdffilename = "PLTPDF\\" + PdfFilename;
         }
         else {
             var xml_path = PDFPLLst[2];
             var temp_path = PDFPLLst[1];
             var pdf_path = PDFPLLst[3];
             //Run Mode Get Path from DB, set Full Path
             xmlfilename = xml_path + "PLTXML\\" + XmlFilename;
             xslfilename = temp_path + "\\" + Templatename;
             pdffilename = pdf_path + "PLTPDF\\" + PdfFilename;
         }

         //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
         //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
         //---------------------------------------------------------------
         var islocalCreate = false;
         //var islocalCreate = true;
         //================================================================
         var exe_path = PDFPLLst[5];
         var webEDITSaddr = PDFPLLst[0];
         //var IsSuccess = CreatePDFfile(exe_path, xmlfilename, xslfilename, pdffilename, islocalCreate);
         var IsSuccess = CreatePDFfileGenPDF(webEDITSaddr, xmlfilename, xslfilename, pdffilename, islocalCreate);
         if (!IsSuccess) {
             PlaySound();
             ShowInfo(palletNo + " " + msgCreatePDFFail);
         }
         return IsSuccess;
     }



     function GetEDITSIP() {
         var HPEDITSIP = '<%=ConfigurationManager.AppSettings["HPEDITSIP"].Replace("\\", "\\\\")%>';
         return HPEDITSIP;
     }

     function GetEDITSTempIP() {
         var HPEDITSTempIP = '<%=ConfigurationManager.AppSettings["HPEDITSTEMPIP"].Replace("\\", "\\\\")%>';
         return HPEDITSTempIP;
     }
     function GetFopCommandPathfile() {
         var FopCommandPathfile = '<%=ConfigurationManager.AppSettings["FopCommandPathfile"].Replace("\\", "\\\\")%>';
         return FopCommandPathfile;
     }

     function GetTEMPLATERootPath() {
         var TEMPLATERootPath = '<%=ConfigurationManager.AppSettings["TEMPLATERootPath"].Replace("\\", "\\\\")%>';
         return TEMPLATERootPath;
     }
     function GetCreateXMLfileRootPath() {
         var CreateXMLfileRootPath = '<%=ConfigurationManager.AppSettings["CreateXMLfileRootPath"].Replace("\\", "\\\\")%>';
         return CreateXMLfileRootPath;
     }
     function GetCreatePDFfileRootPath() {
         var CreatePDFfileRootPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileRootPath"].Replace("\\", "\\\\")%>';
         return CreatePDFfileRootPath;
     }
     function GetCreatePDFfileClientPath() {
         var CreatePDFfileClientPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileClientPath"].Replace("\\", "\\\\")%>';
         return CreatePDFfileClientPath;
     }
     function GetDebugMode() {
         var DEBUGmode = '<%=ConfigurationManager.AppSettings["DEBUGmode"]%>';
         if (DEBUGmode == "True")
             return true;
         else
             return false;
     }


     function PDFPrint() {
         try {
		     printFlag = true;
             var Fs = new ActiveXObject("Scripting.FileSystemObject");
             var FsFile = "C:\\FIS\\Reprint\\pdfprintlist.txt";
             var Fsfolder = "C:\\FIS\\Reprint";
             var FsEnabled = true;
             var File;
             //var printerpath = PDFPLLst[6];
             var printerpath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
             if (printerpath == null || printerpath == "") {
                 printerpath = Fsfolder;
             }

             if (printerpath.slice(-1) != "\\") {
                 printerpath = printerpath + "\\";
             }


             //testing:
             //FsFile = "D:\\chenxu\\Desktop\\imes2012\\FIS\\Reprint\\pdfprintlist.txt";
             //Fsfolder = "D:\\chenxu\\Desktop\\imes2012\\FIS\\Reprint";
             //printerpath = "D:\\chenxu\\Desktop\\imes2012\\FIS";

             if (!FsEnabled) {
                 ResetPage();
                 return;
             }
             if (!Fs.FolderExists(Fsfolder)) {
                 Fs.CreateFolder(Fsfolder);
             }

             if (Fs.FileExists(FsFile)) {
                 Fs.DeleteFile(FsFile);
             }

             File = Fs.CreateTextFile(FsFile, FsEnabled);


             var arrDelivery = new Array();
             arrDelivery = DeliveryPerPalletList;


             // 调SP：HP_EDI.dbo.op_TemplateCheck_LANEW, ==> arrTemp

             var Template = "";
             var DOC_SET = "";
             var Sntp = "";
             var plttp = "";
             var EditsFISAddr = PDFPLLst[3];
             var rowsCount = arrTemp.length;

             if (rowsCount > 1) {

                 for (var TmpCount = 0; TmpCount < rowsCount; TmpCount++) {

                     Template = arrTemp[TmpCount][0].trim();
                     DOC_SET = arrTemp[TmpCount][2].trim();
                     Sntp = "";
                     plttp = "";

                     //if (DOC_SET.search("LA") != -1 || DOC_SET.search("NA-00010") != -1 || DOC_SET.search("EM") != -1) {
                         if (Template.search("Serial") != -1)  //ITC-1360-1596
                         {
                             Sntp = "SN";
                         }
                         else if (Template.search("General") != -1) {
                             Sntp = "Ship";
                         }
                         else if (Template.search("BOX_ID") != -1) {
                             Sntp = "EMEA";
                         }
                         else Sntp = "";
                     //}

                     if (Template.search("_ATT") == -1 && Template.search("2D") == -1) {
                         if (Template.search("TypeB") != -1) {
                             plttp = "B";
                         }
                         else plttp = "A";

                         if (Sntp != "SN" && Sntp != "EMEA") {
                             if (plttp == "A") {
                                 for (j = 0; j < arrDelivery.length; j++) {
                                     if (!(((modelList[j].substr(9, 2) == "AA" || modelList[j].substr(9, 2) == "AM")) && Sntp == "Ship")) {
                                         var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                         var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                         File.WriteLine(pdfPath);
                                         File.WriteLine(pdfPath);
                                     }
                                 }
                             }
                             else {
                                 if (!(((modelList[0].substr(9, 2) == "AA" || modelList[0].substr(9, 2) == "AM")) && Sntp == "Ship")) {
                                     var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                     var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                     File.WriteLine(pdfPath);
                                     File.WriteLine(pdfPath);
                                 }
                             }
                         }
                         else {
                             if (plttp == "A") {
                                 for (j = 0; j < arrDelivery.length; j++) {
                                     var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                     var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                     File.WriteLine(pdfPath);

                                 }
                             }
                             else {
                                 var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                 var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                 File.WriteLine(pdfPath);

                             }
                         }
                     }
                     else {
                         if (Template.search("TypeA") != -1 && Template.search("2D") != -1) {
                             plttp = "2DA";
                         }
                         else if (Template.search("TypeB") != -1 && Template.search("2D") != -1) {
                             plttp = "2DB";
                         }
                         else if (Template.search("TypeB") != -1 && Template.search("_ATT") != -1) {
                             plttp = "ATTB";
                         }
                         else plttp = "ATTA";

                         if (plttp == "ATTA") {
                             for (j = 0; j < arrDelivery.length; j++) {
                                 var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[ATTPalletShipLabel]" + Sntp.trim() + ".pdf";
                                 var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                 File.WriteLine(pdfPath);
                             }
                         }
                         else if (plttp == "ATTB") {
                             var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[ATTPalletShipLabel]" + Sntp.trim() + ".pdf";
                             var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                             File.WriteLine(pdfPath);

                         }
                         else if (plttp == "2DA") {
                             for (j = 0; j < arrDelivery.length; j++) {
                                 var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[2DpalletShipLabel]" + Sntp.trim() + ".pdf";
                                 var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                 File.WriteLine(pdfPath);
                             }
                         }
                         else {
                             var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[2DpalletShipLabel]" + Sntp.trim() + ".pdf";
                             var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                             File.WriteLine(pdfPath);

                         }
                     }
                 }
             }
             else {
                 var Template = arrTemp[0][0].trim();
                 var plttp = "";

                 if (Template.search("TypeB") != -1) {
                     plttp = "B";
                 }
                 else plttp = "A";

                 if (plttp == "A") {
                     for (j = 0; j < arrDelivery.length; j++) {
                         if (!(((modelList[j].substr(9, 2) == "AA" || modelList[j].substr(9, 2) == "AM")) && Sntp == "Ship")) {
                             var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                             var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                             File.WriteLine(pdfPath);
                             File.WriteLine(pdfPath);
                         }
                     }
                 }
                 else {
                     if (!(((modelList[0].substr(9, 2) == "AA" || modelList[0].substr(9, 2) == "AM")) && Sntp == "Ship")) {
                         var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                         var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                         File.WriteLine(pdfPath);
                         File.WriteLine(pdfPath);
                     }
                 }

             }

             File.Close();

             var wsh = new ActiveXObject("wscript.shell");
             //var cmdpdfprint = "PDFPrint.exe C:\\FIS\\Reprint\\pdfprintlist.txt 4000";
             var cmdpdfprint = "PrintPDF.bat C:\\FIS\\Reprint\\pdfprintlist.txt 4000"; 
             wsh.run("cmd /k " + getHomeDisk(printerpath) + "&cd " + printerpath + "&" + cmdpdfprint + "&exit", 2, false);  //打印步驟改爲異步操作
             wsh = null;
             

             var SuccessItem = "[" + palletNo + "]";
             //  ResetPage();
            
             inputSNControl.value = "";
             inputSNControl.focus();

             getAvailableData("processDataEntry");
             ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
			             

          }

         catch (err) {
             ResetPage();
             PlaySound();
             ShowInfo(err.description);
         }
     }
     
     
     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
         palletNo = "";
         inputSNControl.value = "";
         inputSNControl.focus();      
         
         getAvailableData("processDataEntry");

     }

     function PlaySound() {
         var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
         var obj = document.getElementById("bsoundInModal");
         obj.src = sUrl;
     }

     function PlaySoundClose() {

         var obj = document.getElementById("bsoundInModal");
         obj.src = "";
     }
     
     function ShowErrorMessage(result) {
         endWaitingCoverDiv();

         ShowInfo(result);
         PlaySound();
         inputSNControl.value = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");
     }


     function showPrintSettingDialog() {
         showPrintSetting(station, pcode);
     }


     function Tab(reasonPara) {
         if (event.keyCode == 9) {
             inputSNControl.focus();
             event.returnValue = false;
         }
     }

     function ExitPage() {
     }

     function ResetPage() {
         ExitPage();
         ClearData();
     }

     window.onunload = function() {
         ExitPage();
     };
     
 </script>
</asp:Content>



