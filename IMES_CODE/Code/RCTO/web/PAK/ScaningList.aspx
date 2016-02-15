<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/PAK/PackingList Page
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/11/14 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/11/14            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   chenpeng                (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 1.自定义控件combox Doc_Type;DNShipment;Region;Carrier
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ScaningList.aspx.cs" Inherits="PAK_ScaningList" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Services>
         <asp:ServiceReference Path="Service/WebServiceScanningList.asmx" />
        </Services>
    </asp:ScriptManager>
   <center>
   
    <table border="0" width="95%">
    <tr><td style="width:70%"> &nbsp; </td></tr>
   
    <tr>
        <td style="width:70%">
            <table border="0" width="95%">
   				<td style="width:20%"><asp:Label ID="lbDocType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
				<td style="width:57%"><iMES:CmbDocTypeForScannig ID="CmbDocType1" runat="server" Width="100" IsPercentage="true"/></td>
				<td style="width:23%">&nbsp;</td>
            </table>
        </td>
        
        <td style="width:30%" rowspan="7" valign="top">
            <fieldset id="Fieldset2" style="height: 350px" >
                <legend align ="left"  ><asp:Label ID="lbDNShipList" runat="server" CssClass="iMes_DataEntryLabel" /></legend>
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
                                        onrowdatabound="GridViewExt1_RowDataBound" UseAccessibleHeader="False" ShowHeader="False">                                     
                                        <Columns>
                                            <asp:BoundField DataField="Item" SortExpression="DefectId" />
                                            <asp:BoundField DataField="hidCol" SortExpression="DefectId" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>   
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
   
                            </td>
                            <td style="width:40%" align="right">
                                <input id="btPrint" type="button" style="width:80%" runat="server" class="iMes_button" 
                                    onclick="fprint();" onmouseover="this.className='iMes_button_onmouseover'" onserverclick="serclear" 
                                    onmouseout="this.className='iMes_button_onmouseout'"/>
                            </td>
                        </tr>       
                    </table>
            </fieldset>
        </td>
    </tr>
 
      
    <tr>
        <td style="width:70%">
            <table border="0" width="95%">
   			    <td style="width:20%"><asp:Label ID="lbDNShip" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
				<td style="width:57%"><iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
                    CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
                </td>
				<td style="width:23%">&nbsp;</td>
            </table>
        </td>
    </tr>
   
    <tr><td style="width:70%">&nbsp;</td><td><input type="hidden" runat="server" id="hiddenCostCenter" name="hiddenCostCenter" /></td></tr>

    <tr>
        <td style="width:70%"> 
      		<fieldset id="Fieldset1" >
            <legend align ="left"  ><asp:Label ID="lbInserFile" runat="server" CssClass="iMes_label_13pt" /></legend>
			<table border="0" width="100%">
			    <tr>					   	    
	                <td style="width:100%" align="left">
                        <iframe name="action" id="action" src="ScaningList_Upload.aspx"  scrolling="no"  frameborder="0" width="100%" height="50px" ></iframe>
	                </td>
                </tr>				
			</table>
			</fieldset>
        </td>
    </tr>
   
    <tr><td style="width:70%"> &nbsp; </td></tr>
   
    <tr>
        <td style="width:70%" align="right">   
   		
          
   	        <input id="btClear" type="button" style="width:25%" runat="server" class="iMes_button" 
                onclick="fclear();" onserverclick="serclear" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'"/>
        </td>
        <td>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input id="btnUploadOver" type="button" onclick="" onserverclick="uploadOver" style="display:
                none" runat="server" />
                 <input type="hidden" id="hidRowCount" runat="server" />

            </ContentTemplate>   
            </asp:UpdatePanel> 
  
        </td>
    </tr>
   
    <tr><td style="width:70%" align="right">&nbsp;</td><td>&nbsp;</td></tr>
   
    <tr>
        
    </tr>
    </table>
  
        
    </center>
</div>
<div style="display:none">        
            <input type="text" id="pdflist" runat="server"/>            
        </div>
<script type="text/javascript">

    var toDate = document.getElementById('txtTo');
    var dataEntryControl;
    var curDay;
    var curDayString;
    var GridViewExt1ClientID = "<%=gridview.ClientID%>";
    var errorMessage;
    var index = 0;
    var initRowsCount = 14;
    var mesNoSelDocType = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectDocType").ToString()%>';
    var mesSNError = "intput length must equal to 10 byte";
    var uuid="";
    var path2 = "c:\\FIS\\";
    var path1 = "c:\\EFIS-Workdir\\pdfprintlist.txt";
    var emptyPattern = /^\s*$/;
    
    var OAEditsURL='';
    var OAEditsTemplate = '';
    var OAEditsXML = '';
    var OAEditsPDF = '';
    var OAEditsImage = '';
    var FOPFullFileName = '';
    var PDFPrintPath = '';


    document.body.onload = function() {
        dataEntryControl = getCommonInputObject();
        getAvailableData("processDataEntry");

        //curDay = new Date();
        //curDayString = curDay.getFullYear().toString() + '-' + (curDay.getMonth() + 1).toString() + '-' + curDay.getDate().toString();
        //fromDate.value = toDate.value = curDayString;
        // customer = '<%=Request["Station"]%>';
        station = '<%=Request["Station"] %>';
        document.getElementById("<%=pdflist.ClientID%>").value = "";

        WebServiceScanningList.FilePath(onPathSuccess, onPathFail);

        getCommonInputObject().focus();

    }

    function print1() {
        var printList = new Array();
        var table = document.getElementById(GridViewExt1ClientID);
        for (var i = 0; i < table.rows.length; i++) {
            if (table.rows[i].cells[0].innerText != " "){
            printList.push(table.rows[i].cells[0].innerText);
            }
        }
        var print_str = printList.toString();
        alert("print data :" + print_str);

        fclear();
    }
    
    function onPrintSuccess(result) {
        endWaitingCoverDiv();
        var content = result[0];
        ShowMessage(content);
        ShowInfo(content);
    }
    
    function onPrintFail(error) {
        try {
            endWaitingCoverDiv();
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }


    
    function onPathSuccess(result) {
        try {
             OAEditsURL =result[0];
             OAEditsTemplate = result[1];
             OAEditsXML = result[2];
             OAEditsPDF = result[3];
             OAEditsImage = result[4];
             FOPFullFileName = result[5];
             PDFPrintPath = result[6];
            
            //var fso = new ActiveXObject("Scripting.FileSystemObject");
            //if (OAEditsImage != '')
            //    fso.CopyFile(OAEditsImage+"*.jpg", "C:\\");
        }
        catch (e) {
            alert(e.description);
        }
    }

    function onPathFail() {
        OAEditsURL = '';
          OAEditsTemplate = '';
          OAEditsXML = '';
          OAEditsPDF = '';
          OAEditsImage = '';
          FOPFullFileName = '';
          PDFPrintPath = '';
    }
    
    function printPDF() {
        var WshSheel = new ActiveXObject("wscript.shell");
        var cmdpdfprint = "";
        if (PDFPrintPath.charAt(PDFPrintPath.length - 1) != "\\")
            cmdpdfprint = PDFPrintPath + "\\PDFPrint.exe" + " " + PDFPrintPath + "\\pdfprintlist.txt 2000";
        else
            cmdpdfprint = PDFPrintPath + "PDFPrint.exe" + " " + PDFPrintPath + "pdfprintlist.txt 2000";    
        
        WshSheel.Run(cmdpdfprint, 1, true);
    }
    function printPDFBAT() {
        var WshSheel = new ActiveXObject("wscript.shell");
        var cmdpdfprint = "";
        if (PDFPrintPath.charAt(PDFPrintPath.length - 1) != "\\")
            PDFPrintPath = PDFPrintPath + "\\";
        var ClientPDFBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
        if (ClientPDFBatFilePath.charAt(ClientPDFBatFilePath.length - 1) != "\\")
            ClientPDFBatFilePath = ClientPDFBatFilePath + "\\";
        //if (PDFPrintPath.charAt(PDFPrintPath.length - 1) != "\\")
        //    cmdpdfprint = PDFPrintPath + "\\PDFPrint.exe" + " " + PDFPrintPath + "\\pdfprintlist.txt 2000";
        //else
        //    cmdpdfprint = PDFPrintPath + "PDFPrint.exe" + " " + PDFPrintPath + "pdfprintlist.txt 2000";
        cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + PDFPrintPath + "pdfprintlist.txt 2000"; 
        WshSheel.Run(cmdpdfprint, 1, true);
    }
    function fprint() {
       
            var pdf = new ActiveXObject("Scripting.FileSystemObject");
            var printfilename = PDFPrintPath + "\\pdfprintlist.txt";
            CheckMakeDir(PDFPrintPath);
            if (pdf.FileExists(printfilename)) {
                pdf.DeleteFile(printfilename);
            }
            var file = pdf.CreateTextFile(printfilename, true);

            if (document.getElementById("<%=pdflist.ClientID %>").value != "") {
                var strArry = new Array();
                strArry = document.getElementById("<%=pdflist.ClientID %>").value.split(";");
                for (var i = 0; i < strArry.length; i++) {
                    if (!emptyPattern.test(strArry[i])) {
                        file.WriteLine(strArry[i]);
                    }

                }
                file.Close();
                printPDFBAT(); //printPDF();
            }
           
            //file.Close();
           // printPDF();
        }
    function clientUpload() {
        beginWaitingCoverDiv();
    }

    function finish(param) {
        //writeToSuccessMessage(errorMsg);
        document.getElementById("<%=hiddenCostCenter.ClientID%>").value = param;
        document.getElementById("<%=btnUploadOver.ClientID%>").click();
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
            var DocType =getDocTypeCmbValue();
            if (DocType == "") {
                alertAndCallNext(mesNoSelDocType);
                errorFlag = true;
                setDocTypeCmbFocus();
                return;
            }
            if (!errorFlag) {
                var table = document.getElementById(GridViewExt1ClientID);         
                for (var i = 0; i < table.rows.length; i++) {
                    if (table.rows[i].cells[0].innerText == inputData) {
                        errorMessage = "already exist";
                        errorFlag = true;
                        dataEntryControl.focus();
                        break;
                    }
                }                                 
            }
            if (!errorFlag) {
                if (inputData.toString().length != 10) {
                    alert(mesSNError);
                 //   getAvailableData("processDataEntry");
                }
                else {
                    beginWaitingCoverDiv();
                    WebServiceScanningList.Check("", station, "<%=UserId%>", "<%=Customer%>", DocType, inputData, onQuerySuccess, onQueryFail);                               
                }
                getAvailableData("processDataEntry");
            }
            else {
                alertAndCallNext(errorMessage);
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }


    function onQuerySuccess(result) {
        ShowInfo("");
        var AddPdfName = "";
        try {
            if (result == null) {
                endWaitingCoverDiv();
                
                var content = "this error";
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();

                var rowInfo = new Array();
                rowInfo.push(result[1]);
                rowInfo.push("111");
                AddRowInfo(rowInfo);
                uuid = result[2];
                var xmlname = result[3][0][1];
                var internalID = result[3][0][0];
                var template = result[3][1][4];
                var pdfname = result[3][1][3];
                CallEDITSFunc(xmlname, internalID);
                CallPdfCreateFunc(xmlname, template, pdfname);
                
                if (OAEditsPDF.charAt(OAEditsPDF.length - 1) != "\\")
                    AddPdfName = OAEditsPDF + "\\PACKINGLISTPDF\\" + pdfname;
                else
                    AddPdfName = OAEditsPDF + "PACKINGLISTPDF\\" + pdfname;
                    
                document.getElementById("<%=pdflist.ClientID%>").value += AddPdfName + ";";
               
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                dataEntryControl.focus();
                //getcommonobject.set
            }
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    function onQueryFail(error) {
        try {
            endWaitingCoverDiv();
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }
    
    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("processDataEntry");
    }
    
    
    function fclear() {
        beginWaitingCoverDiv();
        var table = document.getElementById(GridViewExt1ClientID);

        for (var i = 1; i < table.rows.length; i++) {
            table.rows[i].cells[0].innerText == "";
        }
        getCommonInputObject().focus();
    }


    function getHidInfo() {
        var AddPdfName = "";
        var table = document.getElementById(GridViewExt1ClientID);
        var rowcount = document.getElementById("<%=hidRowCount.ClientID%>").value;
        for (var i = 0; i < parseInt(rowcount); i++) {
            var valueArray = table.rows[i].cells[1].innerText.split("\u0003");
            var count = valueArray[0];
            var index = 1;
            for (var j = 0; j < parseInt(count); j++) {
                var doc_type = valueArray[index]; index++;
                var path = valueArray[index]; index++;

                var internalID = valueArray[index]; index++;
                var xmlname = valueArray[index]; index++;

                var dn = valueArray[index]; index++;
                var shipment = valueArray[index]; index++;
                var waybill = valueArray[index]; index++;
                var pdfname = valueArray[index]; index++;
                var template = valueArray[index]; index++;
                               
                //CallEDITSFunc(xmlname, internalID);

                CallEDITSFunc(xmlname, internalID);
                CallPdfCreateFunc(xmlname, template, pdfname);

                if (OAEditsPDF.charAt(OAEditsPDF.length - 1) != "\\")
                    AddPdfName = OAEditsPDF + "\\PACKINGLISTPDF\\" + pdfname;
                else
                    AddPdfName = OAEditsPDF + "PACKINGLISTPDF\\" + pdfname;

                document.getElementById("<%=pdflist.ClientID%>").value += AddPdfName + ";";
                }
            }

        }

    

    function fileProcess() {
        getHidInfo();
        endWaitingCoverDiv();
    }

    function fileProcess2() {
        ShowMessage("Upload Success!");
        ShowSuccessfulInfo(true, "Upload Success!");
    }


    function CallEDITSFunc(filename, internalID) {
        var Paralist = new EDITSFuncParameters();
        var xmlpathfile = "";
        var webEDITSaddr = "";
        if (GetDebugMode()) {
            //Debug Mode get Root path from Web.conf
            xmlpathfile = GetCreateXMLfileRootPath() + "PACKINGLISTXML\\" + filename;
            CheckMakeDir(xmlpathfile);
            //webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
            webEDITSaddr = GetEDITSTempIP();   //Packing List for aaaa
        }
        else {
            //Run Mode Get Path from DB, set Full Path
            //xmlpathfile = "\\\\10.190.40.68\\OUT\\PACKINGLISTXML\\packingShipmentLabel01.xml";
            //webEDITSaddr = "http://10.190.40.68:80/edits.asmx";
            //webEDITSaddr = "http://10.190.40.68:8080/edits.asmx"; // Packing List for aaa
            if (OAEditsXML.charAt(OAEditsXML.length - 1) != "\\")
                xmlpathfile = OAEditsXML + "\\PACKINGLISTXML\\" + filename;
            else
                xmlpathfile = OAEditsXML + "PACKINGLISTXML\\" + filename;
            webEDITSaddr = OAEditsURL;
        }
        Paralist.add(1, "FilePH", xmlpathfile);
        Paralist.add(2, "Dn", internalID);
        var IsSuccess = invokeEDITSFunc(webEDITSaddr, "WayBillShipmentList", Paralist);
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
            //Run Mode Get Path from DB, set Full Path
            /*
            xmlfilename = "\\\\10.190.40.68\\OUT\\PACKINGLISTXML\\packingShipmentLabel01.xml";
            xslfilename = "\\\\10.190.40.68\\OUT\\templatepacking01.xslt";
            pdffilename = "\\\\10.190.40.68\\OUT\\PACKINGLISTPDF\\packingShipmentLabel01.pdf";
           */
            if (OAEditsXML.charAt(OAEditsXML.length - 1) != "\\")
                xmlfilename = OAEditsXML + "\\PACKINGLISTXML\\" + xml;
            else
                xmlfilename = OAEditsXML + "PACKINGLISTXML\\" + xml;
            if (OAEditsTemplate.charAt(OAEditsTemplate.length - 1) != "\\")
                xslfilename = OAEditsTemplate + "\\" + xsl;
            else
                xslfilename = OAEditsTemplate + xsl;
            if (OAEditsPDF.charAt(OAEditsPDF.length - 1) != "\\")
                pdffilename = OAEditsPDF + "\\PACKINGLISTPDF\\" + pdf;
            else
                pdffilename = OAEditsPDF + "PACKINGLISTPDF\\" + pdf;
        }

        //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
        //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
        //---------------------------------------------------------------
        var islocalCreate = false;
        //var islocalCreate = true;
        //================================================================
        //var IsSuccess = CreatePDFfileAsyn(FOPFullFileName, xmlfilename, xslfilename, pdffilename, islocalCreate);
        var IsSuccess = CreatePDFfileAsynGenPDF(OAEditsURL, xmlfilename, xslfilename, pdffilename, islocalCreate);
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