<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/PAK/PackingList Page
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/11/14 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/11/14            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 1.自定义控件combox Doc_Type;DNShipment;Region;Carrier
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" AsyncTimeout="3000" CodeFile="PackingList.aspx.cs" Inherits="PAK_PackingList" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>



<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000">
         <Services>
            <asp:ServiceReference Path="Service/WebServicePackingList.asmx" />
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
        
        <td style="width:30%" rowspan="7" valign="top">
            <fieldset id="Fieldset2" style="height: 350px" >
                <legend align ="left"  ><asp:Label ID="lbDNShipList" runat="server" CssClass="iMes_label_13pt" /></legend>
                    <table border="0" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="hidbtnClear" EventName="ServerClick" />                    
                                </Triggers>   
                                <ContentTemplate>
	                                <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" 
                                        GetTemplateValueEnable="False" GvExtHeight="300px" Height="300px" 
                                        GvExtWidth="100%" OnGvExtRowClick=""
                                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                                        HighLightRowPosition="1" HorizontalAlign="Left"
                                        onrowdatabound="GridViewExt1_RowDataBound" ShowHeader="False">                                     
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
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <input type="radio" name="qty" id="radiox1" checked runat="server" /><asp:Label ID="lbx1" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;
                                    <input type="radio" name="qty" id="radiox2" runat="server" /><asp:Label ID="lbx2" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                                </ContentTemplate>   
                                </asp:UpdatePanel>
                            </td>
                            <td style="width:40%" align="right">
                                <input id="btPrint" type="button" style="width:80%" runat="server" class="iMes_button" 
                                    onclick="fprint()" onmouseover="this.className='iMes_button_onmouseover'" 
                                    onmouseout="this.className='iMes_button_onmouseout'"/>
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
   			    <td style="width:20%"><asp:Label ID="lbDNShip" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
				<td style="width:57%"><iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
                    CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
                </td>
				<td style="width:23%">&nbsp;</td>
            </table>
        </td>
    </tr>
   
    <tr><td style="width:70%">&nbsp;</td></tr>

    <tr>
        <td style="width:70%"> 
      		<fieldset id="Fieldset1" >
            <legend align ="left"  ><asp:Label ID="lbInserFile" runat="server" CssClass="iMes_label_13pt" /></legend>
			<table border="0" width="100%">
			    <tr>					   	    
	                <td style="width:100%" align="left">
                        <iframe name="action" id="action" src="PackingList_Upload.aspx"  scrolling="no"  frameborder="0" width="100%" height="50px" ></iframe>
	                </td>
                </tr>				
			</table>
			</fieldset>
        </td>
    </tr>
   
    <tr><td style="width:70%">&nbsp;
     </td></tr>
   
    <tr>
        <td style="width:70%">   
   			<fieldset id="Fieldset3" >
            <legend align ="left"><asp:Label ID="lbQuery" runat="server" CssClass="iMes_label_13pt" /></legend>			
			<table border="0" width="100%">
				<tr>
				    <td style="width:20%"><asp:Label ID="lbFrom" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
				    <td style="width:57%"><input type="text" id="txtFrom" style="width:90px;" readonly="readonly"/>
                        <input id="btFrom" type="button" value=".." onclick="showCalendar('txtFrom')" style="width: 17px" class="iMes_button"/></td>
				    <td style="width:23%">&nbsp;</td>
				</tr>
				<tr>
				    <td><asp:Label ID="lbTo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
				    <td><input type="text" id="txtTo" style="width:90px;" readonly="readonly" />
                        <input id="btTo" type="button" value=".." onclick="showCalendar('txtTo')" style="width: 17px" class="iMes_button"  />
				    </td>
				    <td>&nbsp;</td>
				</tr>
				<tr>
				    <td><asp:Label ID="lbRegion" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
				    <td><iMES:CmbRegion ID="cmbRegion" runat="server" Width="100" IsPercentage="true"/></td>
				    <td>&nbsp;</td>
				</tr>
				
				<tr>
				    <td><asp:Label ID="lbCarrier" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
			        <td><iMES:CmbCarrier ID="cmbCarrier" runat="server" Width="100" IsPercentage="true"/></td>
				    <td align="left">
                        <input id="btQuery" type="button" style="width:65%" runat="server" class="iMes_button" 
                            onclick="query()" onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/>
				    </td>
				</tr>
				
				<%--
				<tr><td>&nbsp;</td>
				<td>&nbsp;</td><td>&nbsp;</td></tr>				
				--%>
				
			</table>			
			</fieldset>
        </td>
    </tr>
   
    <tr><td style="width:70%" align="right"><asp:Label ID="lbHid" runat="server" CssClass="iMes_label_13pt" 
                        Visible="True"></asp:Label>
    </td><td><input type="hidden" runat="server" id="hiddenCostCenter" name="hiddenCostCenter" /></td></tr>
   
    <tr>
        <td style="width:70%" align="right"> 
   	        <input id="btClear" type="button" style="width:25%" runat="server" class="iMes_button" 
                onclick="fclear()" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'"/>
        </td>
        <td>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input id="btnUploadOver" type="button" onclick="" onserverclick="uploadOver" style="display:
                none" runat="server" />
                <input type="hidden" id="hidSessionKey" runat="server" />
                <input type="hidden" id="hidStation" runat="server" />
                <input type="hidden" id="hidEditor" runat="server" />
                <input type="hidden" id="hidCustomer" runat="server" />
                <input type="hidden" id="hidPrintType" runat="server" />
                <input type="hidden" id="hidRowCount" runat="server" />
                <input type="hidden" runat="server" id="addr" /> 
                <input type="hidden" runat="server" id="editsxml" /> 
                <input type="hidden" runat="server" id="editstemp" /> 
                <input type="hidden" runat="server" id="editspdf" />
                <input type="hidden" runat="server" id="editspath" /> 
                <input type="hidden" runat="server" id="foppath" /> 
                <input type="hidden" runat="server" id="exepath" /> 
                <button id="hidbtnClose" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_Close"></button>
                <button id="hidbtnClear" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_Clear"></button>
                <%--<asp:Button id="btnUploadOver" Text="789" runat="server" OnClick="uploadOver1" />--%>

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

    var fromDate = document.getElementById('txtFrom');
    var toDate = document.getElementById('txtTo');
    var dataEntryControl;
    var curDay;
    var curDayString;        
    var errorMessage;
    var index = 0;
    var initRowsCount = 14;
    var GridViewExt1ClientID = "<%=gridview.ClientID%>";
    var sessionKey;
    var station;
    var editor;
    var customer;
    var mesNoSelDocType = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectDocType").ToString()%>';
    var mesNoType = '<%=this.GetLocalResourceObject(Pre + "_mesNoType").ToString()%>';
    var mesNoRegion = '<%=this.GetLocalResourceObject(Pre + "_mesNoRegion").ToString()%>';
    var mesNoCarrier = '<%=this.GetLocalResourceObject(Pre + "_mesNoCarrier").ToString()%>';
    var mesAlreadyExist = '<%=this.GetLocalResourceObject(Pre + "_mesAlreadyExist").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var queryAllErrors;
    var queryQty;
    var resQty;
    var hasDocType;
    var printType;
    var emptyPattern = /^\s*$/;




    document.body.onload = function() {
        dataEntryControl = getCommonInputObject();
        getAvailableData("processDataEntry");

        curDay = new Date();
        curDayString = curDay.getFullYear().toString() + '-' + (curDay.getMonth() + 1).toString() + '-' + curDay.getDate().toString();
        fromDate.value = toDate.value = curDayString;

        sessionKey = document.getElementById("<%=hidSessionKey.ClientID %>").value;
        station = document.getElementById("<%=hidStation.ClientID%>").value;
        editor = document.getElementById("<%=hidEditor.ClientID%>").value;
        customer = document.getElementById("<%=hidCustomer.ClientID%>").value;
        printType = document.getElementById("<%=hidPrintType.ClientID%>").value;

        document.getElementById("<%=pdflist.ClientID%>").value = "";
        getCommonInputObject().focus();
        var hidaddr = document.getElementById("<%=addr.ClientID%>").value;
        //Modify ->GenPDF, don't need copy jpeg to c:\
        //if (hidaddr != "") {
        //    var fso = new ActiveXObject("Scripting.FileSystemObject");
        //    fso.CopyFile(hidaddr + "\\*.jpg", "C:\\");
        //}
    }

    function DocTypeValue() {
        return getDocTypeCmbValue();
    }


    function printPDF() {
        var WshSheel = new ActiveXObject("wscript.shell");
        var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
        var cmdpdfprint = exe1 + "\\PDFPrint.exe" + " " + exe1 + "\\pdfprintlist.txt 2500";
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

        var cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + exe1 + "pdfprintlist.txt 2500";
        WshSheel.Run(cmdpdfprint, 2, false);
    }

    function fprint() {
        if (printType == "Client") {
            var pdf = new ActiveXObject("Scripting.FileSystemObject");
            var exe_path = document.getElementById("<%=exepath.ClientID %>").value;
            var printfilename = exe_path + "\\pdfprintlist.txt";
            CheckMakeDir(exe_path);
            if (pdf.FileExists(printfilename)) {
                pdf.DeleteFile(printfilename);
            }
            var file = pdf.CreateTextFile(printfilename, true);

            if (document.getElementById("<%=pdflist.ClientID %>").value != "") {
                var strArry = new Array();
                strArry = document.getElementById("<%=pdflist.ClientID %>").value.split(";");
                for (var i = 0; i < strArry.length; i++) {
                    if (document.getElementById("<%=radiox1.ClientID%>").checked) {
                        if (!emptyPattern.test(strArry[i])) {
                            file.WriteLine(strArry[i]);
                        }
                    }
                    else if (document.getElementById("<%=radiox2.ClientID%>").checked) {
                        if (!emptyPattern.test(strArry[i])) {
                            file.WriteLine(strArry[i]);
                            file.WriteLine(strArry[i]);
                        }
                    }
                }
            }
            file.Close();
            beginWaitingCoverDiv();
            WebServicePackingList.WSClientPrint("none", onPrintSuccess, onPrintFail);
            
        }
        else if (printType == "Server") {
            var count = 0;
            var list = document.getElementById("<%=pdflist.ClientID %>").value;
            if (document.getElementById("<%=radiox1.ClientID%>").checked) {
                count = 1;
            }
            else if (document.getElementById("<%=radiox2.ClientID%>").checked) {
                count = 2;
            }            
            beginWaitingCoverDiv();
            WebServicePackingList.InsertPrintList(list, count, onPrintSuccess, onPrintFail);
        }
        else {
            alert(mesNoType);
        }
    }

    function onPrintSuccess(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                ShowInfo("");
                clearTable();
                dataEntryControl.value = "";
                dataEntryControl.focus();
                document.getElementById("<%=pdflist.ClientID%>").value = "";
                if(printType == "Client")
                    printPDFBAT(); //printPDF();         
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
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


    function fclear() {
        dataEntryControl.value = "";
        beginWaitingCoverDiv();
        WebServicePackingList.WSClear("none", onClearSuccess, onClearFail);
        getAvailableData("processDataEntry");
        dataEntryControl.focus();
    }

    function initUI() {
        curDay = new Date();
        curDayString = curDay.getFullYear().toString() + '-' + (curDay.getMonth() + 1).toString() + '-' + curDay.getDate().toString();
        fromDate.value = toDate.value = curDayString;
        ShowInfo("");
        clearTable();
        document.getElementById("<%=pdflist.ClientID%>").value = "";
        getCarrierCmbObj().value = "ALL";
        getRegionCmbObj().value = "";
        getDocTypeCmbObj().value = "";
    }


    function onClearSuccess(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == SUCCESSRET) {
            endWaitingCoverDiv();
            clearTable();
                initUI();
                document.getElementById("<%=hidbtnClear.ClientID %>").click();
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }
    function onClearFail(error) {
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


    function clearTable() {
        ClearGvExtTable1("<%=gridview.ClientID%>", initRowsCount);
        //没有表头，数据是第0行，因此index=0
        index = 0;
    }
    
    function query() {
        var strFromDate = fromDate.value.toString();
        var strToDate = toDate.value.toString();
        var strRegion = getRegionCmbValue();
        var strCarrier = getCarrierCmbValue();

        if (getDocTypeCmbValue() == "") {
            alertAndCallNext(mesNoSelDocType);            
            setDocTypeCmbFocus();
            return;
        }
        if (strRegion == "") {
            alert(mesNoRegion);
            setRegionCmbFocus();
            return;
        }
        if (strCarrier == "") {
            alert(mesNoCarrier);
            setCarrierCmbFocus();
            return;
        }
        beginWaitingCoverDiv();
        WebServicePackingList.WSQuery("", station, editor, customer, strRegion, strCarrier, strFromDate, strToDate, onQuerySuccess, onQueryFail);       
    }

    function onQuerySuccess(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == SUCCESSRET) {
                clearTable();
                endWaitingCoverDiv();
                queryAllErrors = "";
                queryQty = 0;
                resQty = 0;
                document.getElementById("<%=pdflist.ClientID%>").value = "";          
                for (var i = 0; i < result[1].length; i++) {
                    var errorFlag = false;                    
                    var table = document.getElementById(GridViewExt1ClientID);
                    for (var j = 0; j < table.rows.length; j++) {
                        if (table.rows[j].cells[0].innerText == result[1][i]) {
                            errorFlag = true;
                            break;
                        }
                    }
                    if (!errorFlag) {
                        beginWaitingCoverDiv();
                        var strDocType = getDocTypeCmbValue();
                        WebServicePackingList.WSCheck("", station, editor, customer, result[1][i],
                                                strDocType, "", "", sessionKey, onQueryAndCheckSuccess, onQueryAndCheckFail);
                        queryQty++;                            
                    }
                }
                //alert(queryQty);
                
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    function myShowMessage(info) {
        resQty++;
        if (resQty >= queryQty) {
            endWaitingCoverDiv();
            //alert(resQty);
            ShowInfo("");
            if (queryAllErrors == "") {
                ShowInfo("");
                //ShowMessage("success");
            }
            else {
                ShowInfo(queryAllErrors);
                ShowMessage(queryAllErrors);
            }
        }        
    }
    
    function changeFileName(ori, ext, append){
		var i = ori.indexOf(ext);
		if(i>0) ori = ori.substring(0,i) + append + ext;
		else ori += append;
		return ori;
	}
	
	function onQueryAndCheckSuccess(result) {
        try {
            //
            if (result == null) {                
                queryAllErrors = queryAllErrors + msgSystemError + "\n";
                myShowMessage(queryAllErrors);
            }
            else if (result[0] == SUCCESSRET) {                
                var rowInfo = new Array();
                rowInfo.push(result[1][0]);
                rowInfo.push("111");
                AddRowInfo(rowInfo);
                myShowMessage("");
                //FULLL
                var fullpath_pdf = "";
                var pdf_path = document.getElementById("<%=editspdf.ClientID%>").value;
                for (var i = 0; i < result[1][1].length; i++) {
                    var internalID = result[1][1][i][0];
                    var xmlname = result[1][1][i][1];
                    for (var j = 0; j < result[1][1][i][2].length; j++) {
                        var xmlnameNew = xmlname;
                        if (j > 0)
                            xmlnameNew = changeFileName(xmlnameNew, '.xml', '_' + j);
                        var xsdname = result[1][1][i][4][j];
                        CallEDITSFunc(xmlnameNew, internalID, xsdname);
                        
                        var template = result[1][1][i][2][j];
                        for (var k = 0; k < result[1][1][i][3].length; k++) {
                            var pdfname = changeFileName(result[1][1][i][3][k], '.pdf', '_' + j + '_' + k);
                            CallPdfCreateFunc(xmlnameNew, template, pdfname);
                            //FULLL
                            if (pdf_path.charAt(pdf_path.length - 1) != "\\")
                                fullpath_pdf = pdf_path + "\\PACKINGLISTPDF\\" + pdfname;
                            else
                                fullpath_pdf = pdf_path + "PACKINGLISTPDF\\" + pdfname;


                            document.getElementById("<%=pdflist.ClientID%>").value += fullpath_pdf + ";";
                        }
                    }
                } 
            }
            else {                
                queryAllErrors = queryAllErrors + result[0] + "\n";
                myShowMessage(queryAllErrors);              
            }
        } catch (e) {
            //
            queryAllErrors = queryAllErrors + e.description + "\n";
            myShowMessage(queryAllErrors);
        }
    }
    function onQueryAndCheckFail(error) {
        try {
            //
            queryAllErrors = queryAllErrors + error.get_message() + "\n";
            myShowMessage(queryAllErrors);
        } catch (e) {
            //
            queryAllErrors = queryAllErrors + e.description + "\n";
            myShowMessage(queryAllErrors);
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

    function finish(param) {
        document.getElementById("<%=hiddenCostCenter.ClientID%>").value = param;
        document.getElementById("<%=pdflist.ClientID%>").value = "";
        //ITC-1360-1094
        ShowInfo("");
        //ITC-1360-1102
        clearTable();
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
            ShowInfo("");
            var errorFlag = false;
            var strRegion = getRegionCmbValue();
            var strCarrier = getCarrierCmbValue();
            var strDocType = getDocTypeCmbValue();

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
                beginWaitingCoverDiv();
                WebServicePackingList.WSCheck("", station, editor, customer, inputData, strDocType, strRegion, strCarrier, sessionKey, onCheckSuccess, onCheckFail);
                getAvailableData("processDataEntry");
                dataEntryControl.focus();
            }
            else {
                alertAndCallNext(errorMessage);
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }


    function onCheckSuccess(result) {
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
                rowInfo.push("111");
                AddRowInfo(rowInfo);
                endWaitingCoverDiv();
                getCommonInputObject().focus();
                //FULLL
                var fullpath_pdf = "";
                var pdf_path = document.getElementById("<%=editspdf.ClientID%>").value;


                //6.18 BOL
                if (result[1][2] == "BOL") {
                    for (var bo = 0; bo < result[1][1].length; bo++) {
                        for (var i = 0; i < result[1][1][bo].length; i++) {
                            var internalID = result[1][1][bo][i][0];
                            var xmlname = result[1][1][bo][i][1];
                            for (var j = 0; j < result[1][1][bo][i][2].length; j++) {
                                var xmlnameNew = xmlname;
                                if (j > 0)
                                    xmlnameNew = changeFileName(xmlnameNew, '.xml', '_' + j);
                                var xsdname = result[1][1][bo][i][4][j];
                                CallEDITSFunc(xmlnameNew, internalID, xsdname);
                                
                                var template = result[1][1][bo][i][2][j];
                                for (var k = 0; k < result[1][1][bo][i][3].length; k++) {
                                    var pdfname = changeFileName(result[1][1][bo][i][3][k], '.pdf', '_' + j + '_' + k);

                                    CallPdfCreateFunc(xmlnameNew, template, pdfname);
                                    //FULLL
                                    if (pdf_path.charAt(pdf_path.length - 1) != "\\")
                                        fullpath_pdf = pdf_path + "\\PACKINGLISTPDF\\" + pdfname;
                                    else
                                        fullpath_pdf = pdf_path + "PACKINGLISTPDF\\" + pdfname;
                                    document.getElementById("<%=pdflist.ClientID%>").value += fullpath_pdf + ";";
                                }
                            }
                        }
                    }
                }
                else {
                    for (var i = 0; i < result[1][1].length; i++) {
                        var internalID = result[1][1][i][0];
                        var xmlname = result[1][1][i][1];
                        for (var j = 0; j < result[1][1][i][2].length; j++) {
                            var xmlnameNew = xmlname;
                            if (j > 0)
                                xmlnameNew = changeFileName(xmlnameNew, '.xml', '_' + j);
                            var xsdname = result[1][1][i][4][j];
                            CallEDITSFunc(xmlnameNew, internalID, xsdname);
                            
                            var template = result[1][1][i][2][j];
                            for (var k = 0; k < result[1][1][i][3].length; k++) {
                                var pdfname = changeFileName(result[1][1][i][3][k], '.pdf', '_' + j + '_' + k);

                                CallPdfCreateFunc(xmlnameNew, template, pdfname);
                                //FULLL
                                if (pdf_path.charAt(pdf_path.length - 1) != "\\")
                                    fullpath_pdf = pdf_path + "\\PACKINGLISTPDF\\" + pdfname;
                                else
                                    fullpath_pdf = pdf_path + "PACKINGLISTPDF\\" + pdfname;
                                document.getElementById("<%=pdflist.ClientID%>").value += fullpath_pdf + ";";
                            }
                        }
                    }
                }
                                 
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().focus();
            }

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
            getCommonInputObject().focus();
        }
    }
    function onCheckFail(error) {
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

    function ClearGvExtTable1(gvExtClientID, InitRowCount) {
        var table = document.getElementById(gvExtClientID);
        var rowCount = table.rows.length;
        var colCount = table.rows[0].cells.length;
        var rowArray = new Array();
        rowArray.push(' ');

        for (var j = 0; j < colCount - 1; j++) {
            rowArray.push('');
        }

        if ((InitRowCount != null) && (InitRowCount < rowCount) && (InitRowCount > 1)) {
            while (rowCount - InitRowCount > 0) {
                table.deleteRow(rowCount - 1);
                rowCount = table.rows.length;
            }
        }
        for (var i = 0; i < rowCount; i++) {
            eval("ChangeCvExtRowByIndex_" + gvExtClientID + "(rowArray,false,i)");
        }
    }

    function getHidInfo() {
        var table = document.getElementById(GridViewExt1ClientID);
        var rowcount = document.getElementById("<%=hidRowCount.ClientID%>").value;
        //FULLL
        var fullpath_pdf = "";
        var pdf_path = document.getElementById("<%=editspdf.ClientID%>").value;
        for (var i = 0; i < parseInt(rowcount); i++) {
            var valueArray = table.rows[i].cells[1].innerText.split("\u0003");
            var count = valueArray[0];
            var index = 1;
            for (var j = 0; j < parseInt(count); j++) {
                var internalID = valueArray[index]; index++;
                var xmlname = valueArray[index]; index++;

                var templateCount = valueArray[index]; index++;
                for (var k = 0; k < parseInt(templateCount); k++) {
                    var template = valueArray[index]; index++;

                    var xmlnameNew = xmlname;
                    if (k > 0)
                        xmlnameNew = changeFileName(xmlnameNew, '.xml', '_' + k);
                    
                    var words = template.split('|||');
                    template = words[0];
                    var xsdname = ''
                    if (words.length > 1)
                        xsdname = words[1];
                    CallEDITSFunc(xmlnameNew, internalID, xsdname);
                    
                    var pdfcount = valueArray[index]; index++;
                    for (var l = 0; l < parseInt(pdfcount); l++) {
                        var pdf = changeFileName(valueArray[index], '.pdf', '_' + k + '_' + l); index++;
                        CallPdfCreateFunc(xmlnameNew, template, pdf);
                        //FULLL
                        if (pdf_path.charAt(pdf_path.length - 1) != "\\")
                            fullpath_pdf = pdf_path + "\\PACKINGLISTPDF\\" + pdf;
                        else
                            fullpath_pdf = pdf_path + "PACKINGLISTPDF\\" + pdf;
                        
                        //ITC-1360-1492
                        document.getElementById("<%=pdflist.ClientID%>").value += fullpath_pdf + ";";
                        //alert(pdf);
                    }
                }
            }

        }

    }

    function fileProcess() {
        var rowcount = document.getElementById("<%=hidRowCount.ClientID%>").value;
        index = index + parseInt(rowcount);
        getHidInfo();
        endWaitingCoverDiv();
    }

    function fileProcess2() {
        ShowMessage("Upload Success!");
        ShowSuccessfulInfo(true, "Upload Success!");
    }

    function CallEDITSFunc(filename, internalID, xsd) {
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
            var xmlpath = document.getElementById("<%=editsxml.ClientID%>").value;
            var edits_path = document.getElementById("<%=editspath.ClientID%>").value;
            //Run Mode Get Path from DB, set Full Path
            //xmlpathfile = xmlpath + "\\PACKINGLISTXML\\" + filename;
            if (xmlpath.charAt(xmlpath.length - 1) != "\\")
                xmlpathfile = xmlpath + "\\PACKINGLISTXML\\" + filename;
            else
                xmlpathfile = xmlpath + "PACKINGLISTXML\\" + filename;    
            webEDITSaddr = edits_path;
            //webEDITSaddr = "http://10.190.40.68:8080/edits.asmx"; // Packing List for aaa
        }
        Paralist.add(1, "FilePH", xmlpathfile);
        Paralist.add(2, "Dn", internalID);
        Paralist.add(3, "Tp", "");
        var IsSuccess = false;
        if (xsd.toUpperCase().indexOf('BSAM') > 0)
            IsSuccess = invokeEDITSFunc(webEDITSaddr, "HouseWaybills", Paralist);
        else if (xsd.toUpperCase().indexOf('MASTER') > 0)
            IsSuccess = invokeEDITSFunc(webEDITSaddr, "MasterWaybillList", Paralist);
		else
            IsSuccess = invokeEDITSFunc(webEDITSaddr, "PackingShipmentLabel", Paralist);
        return IsSuccess;
    }

    function CallPdfCreateFunc(xml, xsl, pdf) {
        var xmlfilename, xslfilename, pdffilename;

        if (GetDebugMode()) {
            //Debug Mode get Root path from Web.conf
            xmlfilename = "PACKINGLISTXML\\" + xml;
            xslfilename =  xsl;
            pdffilename = "\PACKINGLISTPDF\\" + pdf;
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
        var islocalCreate = false;
        //var islocalCreate = true;
        //================================================================
        var exe_path = document.getElementById("<%=foppath.ClientID%>").value;
        //var IsSuccess = CreatePDFfileAsyn(exe_path, xmlfilename, xslfilename, pdffilename, islocalCreate);
        //Create PDF ->EDITS Service GenPDF
        var edits_path = document.getElementById("<%=editspath.ClientID%>").value;
        var IsSuccess = CreatePDFfileAsynGenPDF(edits_path, xmlfilename, xslfilename, pdffilename, islocalCreate);
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