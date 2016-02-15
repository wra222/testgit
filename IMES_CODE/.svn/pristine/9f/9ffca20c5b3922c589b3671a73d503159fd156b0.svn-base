<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/PAK/PackingList For Product Line Page
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/11/14 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/11/14            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 1.自定义控件combox
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PackingListForPL.aspx.cs" Inherits="PAK_PackingListForPL" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>



<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Services>
            <asp:ServiceReference Path="Service/WebServicePackingListForPL.asmx" />
        </Services>
    </asp:ScriptManager>
   <center>
   
    <table border="0" width="95%">
    <tr><td style="width:70%"> &nbsp; </td></tr>
   
    <tr>
        <td style="width:70%">
            <table border="0" width="95%">
   				<td style="width:20%"><asp:Label ID="lbDocType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
				<td style="width:57%"><iMES:CmbDocType ID="cmbDocType" runat="server" Width="100" IsPercentage="true"/></td>
				<td style="width:23%">&nbsp;</td>
            </table>
        </td>
        
        <td style="width:30%" rowspan="6" valign="top">
            <fieldset id="Fieldset2">
                <legend align ="left"  ><asp:Label ID="lbDNList" runat="server" CssClass="iMes_label_13pt" /></legend>
                    <table border="0" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
	                                <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" 
                                        GetTemplateValueEnable="False" GvExtHeight="300px" Height="300px" 
                                        GvExtWidth="100%" OnGvExtRowClick=""
                                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                                        HighLightRowPosition="1" HorizontalAlign="Left"
                                        onrowdatabound="GridViewExt1_RowDataBound" ShowHeader="False">                                     
                                        <Columns>
                                            <asp:BoundField DataField="Item" SortExpression="DefectId" />
                                        </Columns>
                                    </iMES:GridViewExt>                                                                                                                                                                                                                                                                                                                                                                                                                                             
                                </ContentTemplate>   
                                </asp:UpdatePanel>
                            </td>
                        </tr>       
                    </table>
            </fieldset>
        </td>
    </tr>
    <tr><td style="width:70%">&nbsp;</td></tr>
      
    <tr>
        <td style="width:70%">
            <table border="0" width="95%">
   			    <td style="width:20%"><asp:Label ID="lbCustomer" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
				<td style="width:57%"><iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
                    CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
                </td>
				<td style="width:23%">&nbsp;</td>
            </table>
        </td>
    </tr>
      
    <tr>
        <td style="width:70%">&nbsp;</td>
    </tr>
   
    <tr>
        <td style="width:70%">&nbsp;</td>
    </tr>
   
   
   <tr><td style="width:70%" align="right">
        <table border="0" width="100%">
   			    <td style="width:20%">&nbsp;</td>
				<td style="width:57%">&nbsp;</td>
				<td style="width:23%" align="left">
				    <input id="btnClear" type="button" style="width:80%" runat="server" class="iMes_button" 
                    onclick="funclear();" onserverclick="serclear" onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'"/>                                      
                </td>
         </table>
         </td>  
    </tr>
   
    
    <td><input type="hidden" runat="server" id="hiddenCostCenter" name="hiddenCostCenter" /></td></tr>
   
    <tr>
        <td style="width:70%" align="right">&nbsp;</td>
        <td>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input type="hidden" id="hidStation" runat="server" />
                <input type="hidden" id="hidEditor" runat="server" />
                <input type="hidden" id="hidCustomer" runat="server" />
                <input type="hidden" runat="server" id="addr" /> 
                <input type="hidden" runat="server" id="editsxml" /> 
                <input type="hidden" runat="server" id="editstemp" /> 
                <input type="hidden" runat="server" id="editspdf" />
                <input type="hidden" runat="server" id="editspath" />
                <input type="hidden" runat="server" id="foppath" /> 
                <input type="hidden" runat="server" id="exepath" />                                             
            </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
  
        
    </center>
</div>
        <div style="display:none">        
            <input type="text" id="pdflist" runat="server"/>            
        </div>
<script type="text/javascript">

    var dataEntryControl;
    var errorMessage;
    var index = 0;
    var initRowsCount = 14;
    var mesNoSelDocType = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectDocType").ToString()%>';
    var GridViewExt1ClientID = "<%=gridview.ClientID%>";
    var DocTypeID = "<%=cmbDocType.ClientID%>";
    var mesAlreadyExist = '<%=this.GetLocalResourceObject(Pre + "_mesAlreadyExist").ToString()%>';
    var mesSNError = '<%=this.GetLocalResourceObject(Pre + "_mesSNError").ToString()%>';
    var mesSN = "CNU";
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var station;
    var editor;
    var customer;
    var fullcount;
    
    document.body.onload = function() {
        station = document.getElementById("<%=hidStation.ClientID%>").value;
        editor = document.getElementById("<%=hidEditor.ClientID%>").value;
        customer = document.getElementById("<%=hidCustomer.ClientID%>").value;
        dataEntryControl = getCommonInputObject();
        getAvailableData("processDataEntry");
        getCommonInputObject().focus();
        fullcount = 0;
        document.getElementById("<%=pdflist.ClientID%>").value = "";

        var hidaddr = document.getElementById("<%=addr.ClientID%>").value;
        //Modify ->GenPDF, don't need copy jpeg to c:\
        //if (hidaddr != "") {
        //    var fso = new ActiveXObject("Scripting.FileSystemObject");
        //    fso.CopyFile(hidaddr + "\\*.jpg", "C:\\");
        //}
    }

    function AddRowInfo(RowArray) {
        if (index < initRowsCount) {
            eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
        } else {
            eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
        }
        index++;
        setSrollByIndex(index, false);
    }

    function processDataEntry(inputData) {
        try {
            ShowInfo(" ");
            var errorFlag = false;

            if (getDocTypeCmbValue() == "") {
                alertAndCallNext(mesNoSelDocType);
                errorFlag = true;
                setDocTypeCmbFocus();
                return;
            }

            if (!errorFlag) {
                var table = document.getElementById(GridViewExt1ClientID);         
                for (var i = 0; i < table.rows.length; i++) {
                    if (table.rows[i].cells[0].innerText == inputData) {
                        errorMessage = mesAlreadyExist;
                        errorFlag = true;
                        dataEntryControl.focus();
                        break;
                    }
                }                                 
            }
            if (!errorFlag) {
                if (inputData.toString().length != 10
                        || inputData.toString().indexOf(mesSN, 0) != 0) {
                    alert(mesSNError);
                }
                else {
                    beginWaitingCoverDiv();
                    WebServicePackingListForPL.CheckSN(inputData, "print", fullcount, getDocTypeCmbValue(), "", station, editor, customer, onSucceed, onFail);
                }
                getAvailableData("processDataEntry");
                getCommonInputObject().focus();
            }
            else {
                alertAndCallNext(errorMessage);
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function processPDF(path) {
        var pdf = new ActiveXObject("Scripting.FileSystemObject");
        if (pdf.FileExists(path)) {
            pdf.DeleteFile(path);
        }
        var file = pdf.CreateTextFile(path, true);
        file.WriteLine("Hello World!");
        file.Close();
    }

    function printPDF() {
        var WshSheel = new ActiveXObject("wscript.shell");
        var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
        var cmdpdfprint = exe1 + "\\PLPDFPrint.exe" + " " + exe1 + "\\pdfprintlist.txt 7000";
        WshSheel.Run(cmdpdfprint, 2, false);
    }

    //Print PDF -> .bat 
    function printPDFBAT() {
        var WshSheel = new ActiveXObject("wscript.shell");
        var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
        var ClientPDFBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
        if (exe1.charAt(exe1.length - 1) != "\\")
            exe1 = exe1 + "\\";
        if (ClientPDFBatFilePath.charAt(ClientPDFBatFilePath.length - 1) != "\\")
            ClientPDFBatFilePath = ClientPDFBatFilePath + "\\";

        var cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + exe1 + "pdfprintlist.txt 7000";
        WshSheel.Run(cmdpdfprint, 2, false);
    }
    
    function onSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().focus();
            }
            else if (result[0] == SUCCESSRET) {
                var rowInfo = new Array();
                rowInfo.push(result[1][0]);
                AddRowInfo(rowInfo);
                endWaitingCoverDiv();
                getCommonInputObject().focus();
                var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
                var pdf = new ActiveXObject("Scripting.FileSystemObject");
                var printfilename = exe1 + "\\pdfprintlist.txt";
                CheckMakeDir(exe1);
                if (pdf.FileExists(printfilename)) {
                    pdf.DeleteFile(printfilename);
                }
                var file = pdf.CreateTextFile(printfilename, true);
                //FULLL
                var fullpath_pdf = "";
                var pdf_path = document.getElementById("<%=editspdf.ClientID%>").value;
                for (var i = 0; i < result[1][1].length; i++) {
                    var dn = result[1][1][i][0];
                    var xmlname = result[1][1][i][1];
                    CallEDITSFunc(xmlname, dn, "");
                    for (var j = 0; j < result[1][1][i][2].length; j++) {
                        var template = result[1][1][i][2][j];
                        for (var k = 0; k < result[1][1][i][3].length; k++) {
                            var pdfname = result[1][1][i][3][k];
                            CallPdfCreateFunc(xmlname, template, pdfname);
                            //FULLL
                            if (pdf_path.charAt(pdf_path.length - 1) != "\\")
                                fullpath_pdf = pdf_path + "\\PACKINGLISTPDF\\" + pdfname;
                            else
                                fullpath_pdf = pdf_path + "PACKINGLISTPDF\\" + pdfname;
                            file.WriteLine(fullpath_pdf);                
                        }
                    }
                }
                file.Close();
                printPDFBAT(); //printPDF(); 
                
                fullcount++;
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().focus();
            }
        }
        catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
            getCommonInputObject().focus();
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            getCommonInputObject().focus();
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
            getCommonInputObject().focus();
        }
    }

    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("processDataEntry");
        getCommonInputObject().focus();
    }

    function clearTable() {
        ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
        //没有表头，数据是第0行，因此index=0
        index = 0;
    }

    function funclear() {
        beginWaitingCoverDiv();
        var dt = document.getElementById(DocTypeID);
        getDocTypeCmbObj().selectedIndex = 0;
        clearTable();
        ShowInfo("");
        getCommonInputObject().focus();
    }

    function CallEDITSFunc(filename, dn, p3) {
        var Paralist = new EDITSFuncParameters();
        var xmlpathfile = "";
        var webEDITSaddr = "";
        if (GetDebugMode()) {
            //Debug Mode get Root path from Web.conf
            xmlpathfile = GetCreateXMLfileRootPath() + "PACKINGLISTXML\\" + filename;
            CheckMakeDir(xmlpathfile);
            webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
            //webEDITSaddr = GetEDITSTempIP();   //Packing List for aaaa
        }
        else {
            var xmlpath = document.getElementById("<%=editsxml.ClientID%>").value;
            var edits_path = document.getElementById("<%=editspath.ClientID%>").value;
            //Run Mode Get Path from DB, set Full Path
            if (xmlpath.charAt(xmlpath.length - 1) != "\\")
                xmlpathfile = xmlpath + "\\PACKINGLISTXML\\" + filename;
            else
                xmlpathfile = xmlpath + "PACKINGLISTXML\\" + filename;    
            webEDITSaddr = edits_path;
            //webEDITSaddr = "http://10.190.40.68:8080/edits.asmx"; // Packing List for aaa
        }
        Paralist.add(1, "FilePH", xmlpathfile);
        Paralist.add(2, "Dn", dn);
        Paralist.add(3, "Tp", p3);
        var IsSuccess = invokeEDITSFunc(webEDITSaddr, "PackingShipmentLabel", Paralist);
        return IsSuccess;
    }

    function CallPdfCreateFunc(xml, xsl, pdf) {
        var xmlfilename, xslfilename, pdffilename;

        if (GetDebugMode()) {
            //Debug Mode get Root path from Web.conf
            xmlfilename = "PACKINGLISTXML\\" + xml;
            xslfilename =  xsl;
            pdffilename = "PACKINGLISTPDF\\" + pdf;
        }
        else {
            var xml_path = document.getElementById("<%=editsxml.ClientID%>").value;
            var temp_path = document.getElementById("<%=editstemp.ClientID%>").value;
            var pdf_path = document.getElementById("<%=editspdf.ClientID%>").value;
            //Run Mode Get Path from DB, set Full Path
            if (xml_path.charAt(xml_path.length - 1) != "\\")
                xmlfilename = xml_path + "\\PACKINGLISTXML\\" + xml;
            else
                xmlfilename = xml_path + "PACKINGLISTXML\\" + xml;
            if (temp_path.charAt(temp_path.length - 1) != "\\")
                xslfilename = temp_path + "\\" + xsl;
            else
                xslfilename = temp_path + xsl;
            if (pdf_path.charAt(pdf_path.length - 1) != "\\")
                pdffilename = pdf_path + "\\PACKINGLISTPDF\\" + pdf;
            else
                pdffilename = pdf_path + "PACKINGLISTPDF\\" + pdf;
        }

        //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
        //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
        //---------------------------------------------------------------
        //var islocalCreate = false;
        var islocalCreate = true;
        //================================================================
        var exe_path = document.getElementById("<%=foppath.ClientID%>").value;
        //var IsSuccess = CreatePDFfile(exe_path, xmlfilename, xslfilename, pdffilename, islocalCreate);
        var edits_path = document.getElementById("<%=editspath.ClientID%>").value;
        var IsSuccess = CreatePDFfileGenPDF(edits_path, xmlfilename, xslfilename, pdffilename, islocalCreate);
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
    
    
</script>

</asp:Content>