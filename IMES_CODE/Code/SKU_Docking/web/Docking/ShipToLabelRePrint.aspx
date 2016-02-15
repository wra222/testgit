
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ShipToLabelRePrint.aspx.cs" Inherits="ShipToLabelRePrint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceShipToLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    <tr>
        <td style="width:100%" colspan="7">
            <fieldset id="Fieldset2">
                <legend align ="left"  ><asp:Label ID="lblCarton" runat="server" CssClass="iMes_label_13pt" /></legend>
                <table border="0" width="95%">
                <tr>
                <td style="width:12%" align="left"><asp:Label ID="lblCartonNo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                <td style="width:40%" align="left"> 
	                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:Label ID="txtCartonNo" runat="server" CssClass="iMes_label_11pt"></asp:Label>                                           
                    </ContentTemplate>                                        
                    </asp:UpdatePanel>
                </td>
                <td>&nbsp;</td>
                </tr>
                </table>
            </fieldset>
        </td>
    </tr>
    <tr><td>
        <input type="hidden" runat="server" id="exepath" />
        <input type="hidden" runat="server" id="pdfpath" />
        <input type="hidden" runat="server" id="editsXML" />
        <input type="hidden" runat="server" id="editsURL" />
        <input type="hidden" runat="server" id="editsTEMP" />
        <input type="hidden" runat="server" id="editsFOP" />
        <input type="hidden" runat="server" id="editsPDF" />
    </td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></TD>
	    <td colspan="5" align="left">
	        <iMES:Input ID="DataEntry" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </td>
        <td style="width:12%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
    </tr>
    

    </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
</asp:UpdatePanel> 


<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var mesWrongCode = '<%=this.GetLocalResourceObject(Pre + "_mesWrongCode").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';



    var SUCCESSRET = "SUCCESSRET";
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var lstPrintItem;
    var inputObj;
    var inpuProdid;
    
    document.body.onload = function() {
        inputObj = getCommonInputObject();
        ShowInfo("");
        getAvailableData("ProcessInput");
        getCommonInputObject().focus();
    }

    function ProcessInput(inputData) {
        try{
            print();
            getAvailableData("ProcessInput");
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
    
    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }

    function print() {
        try {
            var errorFlag = false;
            var msg = "";
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            inpuProdid = getCommonInputObject().value.trim();

            if (inpuProdid == "") {
                errorFlag = true;
                msg = mesNoProdId;
                alert(msg);
                getCommonInputObject().focus();
            }
            else if (!checkInput(inpuProdid)) {
                errorFlag = true;
                msg = mesWrongCode;
                alert(msg);
                getCommonInputObject().focus();
            }

            if (!errorFlag) {
                var station = document.getElementById("<%=stationHF.ClientID %>").value;
                try {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null)
                    {
                        msg = msgPrintSettingPara;
                        alert(msg);
                        getCommonInputObject().focus();                        
                        return;
                    }
                    beginWaitingCoverDiv();
                    WebServiceShipToLabelPrint.ShipToRePrint("", editor, station, customer, inpuProdid, lstPrintItem, onSucceed, onFail);                    
                }
                catch (e1) {
                    alertAndCallNext(e1.description);
                }
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function checkInput(data) {
        if (data.length != 10 && data.length != 11)
            return false;
        pattCustSN = /^S{0,1}CN.{8}$/;
        //if (pattCustSN.exec(data)) return true;
		if (CheckCustomerSN(data)) return true;
        else return false;
    }

    function printPDF() {
        try {
            var WshSheel = new ActiveXObject("wscript.shell");
            var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
            var cmdpdfprint = "";

            if (exe1.charAt(exe1.length - 1) != "\\")
                cmdpdfprint = exe1 + "\\PDFPrint.exe" + " " + exe1 + "\\pdfprintlist.txt 100";
            else
                cmdpdfprint = exe1 + "PDFPrint.exe" + " " + exe1 + "pdfprintlist.txt 100";
            
            WshSheel.Run(cmdpdfprint, 2, false);
        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
            getAvailableData("processData");
        }
    }
    function printPDFBAT() {
        try {
            var WshSheel = new ActiveXObject("wscript.shell");
            var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
            var cmdpdfprint = "";
            var ClientPDFBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';

            if (exe1.charAt(exe1.length - 1) != "\\")
                exe1 = exe1 + "\\";
            if (ClientPDFBatFilePath.charAt(ClientPDFBatFilePath.length - 1) != "\\")
                ClientPDFBatFilePath = ClientPDFBatFilePath + "\\";

            cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + exe1 + "pdfprintlist.txt 100";
            WshSheel.Run(cmdpdfprint, 2, false);
        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
            getAvailableData("processData");
        }
    }

    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                document.getElementById("<%=txtCartonNo.ClientID%>").innerHTML = result[1][0];

                if (parseInt(result[1][3], 10) == 1) {
                    ShowSuccessfulInfo(true, "[" + result[1][1] + "] " + msgSuccess);
                    setPrintItemListParam1(result[1][2], result[1][1]);
                    printLabels(result[1][2], true);
                }
                else {
                    if (result[1][4] != "") {
                        recreatePDF(result[1][4], result[1][5], result[1][0], result[1][6]);
                    }
                    else {
                        alert("pdf name is null");
                    }
                }             
                
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }

    //mantis 1073
    function recreatePDF(pdf, dn, carton, temp) {
        var XmlFileName = pdf.substring(0, pdf.length - 4) + ".xml";
        var PdfFileName = pdf;


        CallEDITSFunc(XmlFileName, dn, carton);
        CallPdfCreateFunc(XmlFileName, temp, PdfFileName);

        var pdf_x = new ActiveXObject("Scripting.FileSystemObject");
        var pdf_path = document.getElementById("<%=pdfpath.ClientID%>").value;
        var exe_path = document.getElementById("<%=exepath.ClientID%>").value;


        var file_path = "";
        if (pdf_path.charAt(pdf_path.length - 1) != "\\")
            file_path = pdf_path + "\\DOCKPDF\\" + pdf;
        else
            file_path = pdf_path + "DOCKPDF\\" + pdf;

        var full_exe_path = "";
        if (exe_path.charAt(exe_path.length - 1) != "\\")
            full_exe_path = exe_path + "\\" + "pdfprintlist.txt";
        else
            full_exe_path = exe_path + "pdfprintlist.txt";
        
        
        CheckMakeDir(exe_path);
        if (pdf_x.FileExists(full_exe_path)) {
            pdf_x.DeleteFile(full_exe_path);
        }
        var file = pdf_x.CreateTextFile(full_exe_path, true);
        file.WriteLine(file_path);
        file.Close();
        //printPDF();
        printPDFBAT();
    }

    function CallEDITSFunc(XmlFilename, dn, carton) {
        var Paralist = new EDITSFuncParameters();
        var xmlpathfile = "";
        var webEDITSaddr = "";

        var PLEditsXML = document.getElementById("<%=editsXML.ClientID%>").value;
        var PLEditsURL = document.getElementById("<%=editsURL.ClientID%>").value;
        if (PLEditsXML == "" || PLEditsURL == "") {
            alert("EDIT Path error!");
            return false;
        }
        xmlpathfile = PLEditsXML + "DOCKXML\\" + XmlFilename;
        webEDITSaddr = PLEditsURL;

        Paralist.add(1, "FilePH", xmlpathfile);
        Paralist.add(2, "Dn", dn);
        Paralist.add(3, "SerialNum", carton);
        var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
        return IsSuccess;
    }

    function CallPdfCreateFunc(XmlFilename, xsl, PdfFilename) {
        var xmlfilename, xslfilename, pdffilename;

        var PLEditsXML = document.getElementById("<%=editsXML.ClientID%>").value;
        var PLEditsURL = document.getElementById("<%=editsURL.ClientID%>").value;
        var PLEditsTemplate = document.getElementById("<%=editsTEMP.ClientID%>").value;

        if (PLEditsXML == "" || PLEditsURL == "" || PLEditsTemplate == "") {
            alert("EDIT Path error!");
            return false;
        }

        var PLEditsPDF = document.getElementById("<%=editsPDF.ClientID%>").value;
        //Run Mode Get Path from DB, set Full Path
        xmlfilename = PLEditsXML + "DOCKXML\\" + XmlFilename;
        xslfilename = PLEditsTemplate + xsl;
        pdffilename = PLEditsPDF + "DOCKPDF\\" + PdfFilename;

        var FOPFullFileName = document.getElementById("<%=editsFOP.ClientID%>").value;

        var islocalCreate = false;
        //var islocalCreate = true;
        //================================================================
        //var IsSuccess = CreatePDFfile(FOPFullFileName, xmlfilename, xslfilename, pdffilename, islocalCreate);
        var IsSuccess = CreatePDFfileGenPDF(PLEditsURL, xmlfilename, xslfilename, pdffilename, islocalCreate);

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

    //mantis 1073
    
    function setPrintItemListParam1(backPrintItemList, sn)
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@CustomerSN";
        valueCollection[0] = generateArray(sn);

        setPrintParam(lstPrtItem, "DK_Shipto_Label", keyCollection, valueCollection);
    }



    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        ShowInfo("");
        endWaitingCoverDiv();

    }

    function showPrintSettingDialog() {
        //     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
</script>
</asp:Content>

