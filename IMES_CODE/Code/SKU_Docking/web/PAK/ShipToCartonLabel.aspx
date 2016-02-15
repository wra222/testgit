
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ShipToCartonLabel.aspx.cs" Inherits="PAK_ShipToCartonLabel" Title="Untitled Page" ValidateRequest="False" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Services>
            <asp:ServiceReference Path="Service/ShipToCartonLabel.asmx" />
        </Services>
    </asp:ScriptManager>

<div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">

   
   <div id="div1">
    <table border="0" width="95%">
        <tr>
            <td style="width:70%" align="left"><asp:Label ID="lblPalletList" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
            <td style="width:15%" align="right">  
               	        <input id="btnClear" type="button" style="width:90%" runat="server" class="iMes_button" 
                onclick="clickClear()" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'"/>                              
            </td>
            <td style="width:15%" align="right">
                <input id="btnPrintSetting" type="button"  runat="server" 
                    class="iMes_button" onclick="showPrintSettingDialog()" 
                    onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" style="width:90%"/>                                                                           
            </td>
        </tr>
    </table>
   </div>
                    
   
   <div id="div2">
    <table border="0" width="95%">
    <tr>
        <td style="width:60%" rowspan="3">
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="hidbtn2" EventName="ServerClick" />  
                        <asp:AsyncPostBackTrigger ControlID="hidbtn3" EventName="ServerClick" />                 
                    </Triggers>                               
                <ContentTemplate>  
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        GvExtWidth="100%" GvExtHeight="245px" OnRowDataBound="gd_RowDataBound" 
                        Height="310px" OnGvExtRowClick="" 
                        OnGvExtRowDblClick="dblclickTable(this)" style="top: -1px; left: 0px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td style="width:40%">
            <fieldset id="Fieldset1">
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="hidbtn3" EventName="ServerClick" />
                    </Triggers>
                    <ContentTemplate>   
                        <table border="0" width="95%" >
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblDN" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblDNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblPoNo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblPoNoContent" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblPartNoContent" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>  
            </fieldset>
        </td> 
    </tr>
        
    <tr>
        <td style="width:60%">
            <fieldset id="Fieldset2">
                <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" > 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" /> 
                        <asp:AsyncPostBackTrigger ControlID="hidbtn2" EventName="ServerClick" /> 
                        <asp:AsyncPostBackTrigger ControlID="hidbtn3" EventName="ServerClick" />                  
                    </Triggers>                               
                    <ContentTemplate>                    
                        <table border="0" width="95%" >
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblPalletNo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><iMES:CmbPallet ID="CmbPallet" runat="server" Width="99%" /></td>
                            </tr>              
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblQty2" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblQty2Content" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width:30%"><asp:Label ID="lblScanQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                <td style="width:70%"><asp:Label ID="lblScanQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </td> 
    </tr>
    <tr>
        <td style="width:60%">
        <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" > 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                                                <asp:AsyncPostBackTrigger ControlID="hidbtn3" EventName="ServerClick" />                     
                    </Triggers>                               
                    <ContentTemplate> 
            <table width="100%">
                <tr>
                    <td style="width:30%"><asp:Label ID="lblEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
                    <td style="width:70%">
                        <iMES:Input ID="txtInput" runat="server" ProcessQuickInput="true" Width="98%" CanUseKeyboard="true" IsPaste="true" IsClear="true" />
                    </td>
                </tr>
            </table>
                 </ContentTemplate>
                </asp:UpdatePanel>
        </td> 
    </tr>   
    </table>
   </div>
   
   <div id="div3">
    <table border="0" width="95%">       
        <tr>
            <td style="width:40%"><asp:Label ID="lblDNList" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>         
            <td style="width:30%">
                <button id="hidbtn" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_gdDoubleClick"></button>

                <button id="hidbtn2" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_DataEntry"></button>
                <button id="hidbtn3" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_Clear"></button>
                <button id="hidbtnClose" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_Close"></button>
                <input type="hidden" id="hidSessionKey" runat="server" />
                <input type="hidden" id="hidData" runat="server" />
                <input type="hidden" runat="server" id="station1" />
                <input type="hidden" runat="server" id="editor1" />
                <input type="hidden" runat="server" id="customer1" />
                <input type="hidden" runat="server" id="addr" />
                <input type="hidden" runat="server" id="exepath" />
                <input type="hidden" runat="server" id="pdfpath" />
                <input type="hidden" id="hidDeliveryNo" runat="server" />
                    <input type="hidden" id="hidQty" runat="server" />
                    <input type="hidden" id="hidPoNo" runat="server" />
                    <input type="hidden" id="hidModel" runat="server" />
                    <input type="hidden" id="hidPartNo" runat="server" />
                    <input type="hidden" id="hidShipDate" runat="server" />
                    <input type="hidden" id="hidPallet" runat="server" />
            </td>         
            <td style="width:30%">
                &nbsp;
            </td>         
        </tr>
        
        <tr>
            <td style="width:100%" colspan="3">
            <asp:UpdatePanel ID="updatePanel6" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate> 
                    <input type="hidden" id="hidRecordCount" runat="server" /> 
                    <iMES:GridViewExt ID="gd_dn" runat="server" AutoGenerateColumns="true" 
                        GvExtWidth="100%" GvExtHeight="200px" OnRowDataBound="gd_RowDataBound_dn" 
                        Height="290px" OnGvExtRowClick="clickDNTable(this)" 
                        OnGvExtRowDblClick="dblDNclickTable(this)" style="top: -1px; left: 0px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
            </td> 
        </tr>    
    </table>
   </div>
   
</div>

<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 

<script type="text/javascript">

    var selectedRowIndex = -1;
    var selectedRowIndex_dn = -1;
    var emptyPattern = /^\s*$/;
    var inputObj;
    var data_len;
    var dn_tblObj;
    var GridViewExt1ClientID = "<%=gd_dn.ClientID%>";
    var index = 1;
    var initRowsCount = 12;
    var sessionKey;
    var gloInputData;
    var station;
    var editor;
    var customer;
    var preType;
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesPrintTip = '<%=this.GetLocalResourceObject(Pre + "_mesPrintTip").ToString()%>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

    window.onload = function() {
        inputObj = getCommonInputObject();
        inputObj.focus();

        getAvailableData("processData");
        sessionKey = document.getElementById("<%=hidSessionKey.ClientID %>").value;
        station = document.getElementById("<%=station1.ClientID%>").value;
        editor = document.getElementById("<%=editor1.ClientID%>").value;
        customer = document.getElementById("<%=customer1.ClientID%>").value;
        preType = 0;
        //ITC-1360-0595
        var hidaddr = document.getElementById("<%=addr.ClientID%>").value;
        if (hidaddr != "") {

        }
    };


    function clickClear() {
        beginWaitingCoverDiv();
        inputObj.value = "";
        //ITC-1360-0700
        setRowSelectedOrNotSelectedByIndex(selectedRowIndex_dn, false, "<%=gd_dn.ClientID %>");
        ShipToCartonLabel.ClearUI("none", onClearSucceed, onClearFail);
    }

    function onClearSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                alertAndCallNext(msgSystemError);                
            }
            else if (result[0] == SUCCESSRET) {
                
                document.getElementById("<%=hidbtn3.ClientID %>").click();                
                getAvailableData("processData");                
                ShowInfo("");
            }
            else {
                endWaitingCoverDiv();
                var content = result[0];
                showErrorMessageAndCallNext(content);                
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onClearFail(error) {
        try {
            endWaitingCoverDiv();
            onSysError(error);            
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
        

    function processData(inputData) {
        ShowInfo("");
        //ITC-1360-580       
        setRowSelectedOrNotSelectedByIndex(selectedRowIndex_dn, false, "<%=gd_dn.ClientID %>");
        gloInputData = inputData;

        if (inputData.length == 16) {
            beginWaitingCoverDiv();
            ShipToCartonLabel.InputProcess(inputData, 0, "", null, "", station, editor, customer,
                                            onSucceed, onFail);
        }
        else{
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem != null) {
                beginWaitingCoverDiv();
                ShipToCartonLabel.InputProcess(inputData, preType, sessionKey, lstPrintItem, "", station, editor, customer,
                                            onSucceed, onFail);
            }
            else {
                alert(msgPrintSettingPara);
            }
        }
        getAvailableData("processData");        
        document.getElementById("<%=hidData.ClientID %>").value = inputData;        
    }

    function setPrintItemListParam1(backPrintItemList) {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(gloInputData);

        setPrintParam(lstPrtItem, "Shipto Label", keyCollection, valueCollection);
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
        try {
            var WshSheel = new ActiveXObject("wscript.shell");
            var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
            var cmdpdfprint = exe1 + "\\PDFPrint.exe" + " " + exe1 + "\\pdfprintlist.txt 100";
            WshSheel.Run(cmdpdfprint, 2, false);
        }
        catch (e) {
            endWaitingCoverDiv();
            if (e.description == "")
                alert(exe1 + "PDFPrint exec error\n");
            else
                alert(e.description);
            getAvailableData("processData");
        }
    }
    function printPDFBAT() {
        try {
            var WshSheel = new ActiveXObject("wscript.shell");
            var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
            if (exe1.charAt(exe1.length - 1) != "\\")
                exe1 = exe1 + "\\";
            var ClientPDFBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
            if (ClientPDFBatFilePath.charAt(ClientPDFBatFilePath.length - 1) != "\\")
                ClientPDFBatFilePath = ClientPDFBatFilePath + "\\";

            var cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + exe1 + "pdfprintlist.txt 100";
            
            WshSheel.Run(cmdpdfprint, 2, false);
        }
        catch (e) {
            endWaitingCoverDiv();
            if (e.description == "")
                alert(ClientPDFBatFilePath + "PrintPDF exec error\n");
            else
                alert(e.description);
            getAvailableData("processData");
        }
    }


    function onSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                alertAndCallNext(msgSystemError);              
            }
            else if (result[0] == SUCCESSRET) {
                if (result[1].length == 1 && result[1][0] != null) {                    
                    ChangeLabel("出拉美的机器,请更换标签纸", result[1][0]);
                }
                else {
                    
                    
                    document.getElementById("<%=lblPartNoContent.ClientID%>").innerHTML = result[1][0];
                    document.getElementById("<%=lblModelContent.ClientID%>").innerHTML = result[1][1];
                    document.getElementById("<%=lblPoNoContent.ClientID%>").innerHTML = result[1][2];
                    document.getElementById("<%=lblDNContent.ClientID%>").innerHTML = result[1][3];
                    document.getElementById("<%=lblQtyContent.ClientID%>").innerHTML = result[1][4];
                    document.getElementById("<%=hidPallet.ClientID %>").value = result[1][8];
                    document.getElementById("<%=hidDeliveryNo.ClientID %>").value = result[1][3];
                    document.getElementById("<%=hidbtn2.ClientID %>").click();
                    if (result[1][9] == "1") {
                        if (result[2]) {
                            //alert("请注意贴附件MRP Label...");
                            ShowSuccessfulInfo(true, result[1][7] + " [" + gloInputData + "] 请注意贴附件MRP Label..." + msgSuccess);
                        }
                        else {
                            ShowSuccessfulInfo(true, result[1][7] + " [" + gloInputData + "] " + msgSuccess);
                        }
                        if (result[1][5] == "1") {
                            setPrintItemListParam1(result[1][6]);
                            printLabels(result[1][6], false);
                        }
                        else {
                            if (result[1][11] != "") {
                                var pdf = new ActiveXObject("Scripting.FileSystemObject");
                                var pdf_path = document.getElementById("<%=pdfpath.ClientID%>").value;
                                var exe_path = document.getElementById("<%=exepath.ClientID%>").value;
                                var file_path = pdf_path + "\\pdf" + result[1][11];
                                CheckMakeDir(exe_path);
                                if (pdf.FileExists(exe_path + "\\pdfprintlist.txt")) {
                                    pdf.DeleteFile(exe_path + "\\pdfprintlist.txt");
                                }
                                var file = pdf.CreateTextFile(exe_path + "\\pdfprintlist.txt", true);
                                file.WriteLine(file_path);
                                file.Close();

                                printPDFBAT(); //printPDF();
                            }
                            else
                                alert("pdf name is null");
                        }
                        
                        if (result[1][10] == "1") {
                            preType = 1;
                        }
                        else {
                            preType = 0;
                        }
                    }
                    else {
                        if (result[2]) {
                            //alert("请注意贴附件MRP Label...");
                            ShowSuccessfulInfo(true, "[" + gloInputData + "] 请注意贴附件MRP Label..." + msgSuccess);
                        }
                        else {
                            ShowSuccessfulInfo(true, "[" + gloInputData + "] " + msgSuccess);
                        }
                        
                    }

                    //ITC-1360-0697
                    getCommonInputObject().focus();
                }     
                getAvailableData("processData");
            }
            else {
                endWaitingCoverDiv();
                var content = result[0];
                showErrorMessageAndCallNext(content);                
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            onSysError(error);            
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function ChangeLabel(message, key) {
        var url = "./ShipToCartonLabel_ChangeLabel.aspx";
        var paramArray = new Array();
        paramArray[0] = message;
        paramArray[1] = key;
        //ITC-1360-1415
        //window.showModalDialog(url, paramArray, 'dialogWidth:600px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;');
//        alert(message);
        ShipToCartonLabel.ServiceChangeLabel(key, onSucceed, onFail);
    }   

    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("processData");
    }

    function showErrorMessageAndCallNext(message) {
        endWaitingCoverDiv();
        ShowMessage(message);
        ShowInfo(message);        
        getAvailableData("processData");
    }


    function onSysError(error) {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        getAvailableData("processData");
    }

    function showError1(error) {
        ShowMessage(error, true);
    }


    function inputFinish() {
        endWaitingCoverDiv();
        inputObj = getCommonInputObject();
        inputObj.focus();
        inputObj.select();
        getAvailableData("processData");                      
    }
    
    function clickDNTable(con) {
        if ((selectedRowIndex_dn != null) && (selectedRowIndex_dn != parseInt(con.index, 10))) {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_dn, false, "<%=gd_dn.ClientID %>");
        }

        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd_dn.ClientID %>");
        selectedRowIndex_dn = parseInt(con.index, 10);
    }

    function dblDNclickTable(con) {

        dn_tblObj = document.getElementById("<%=gd_dn.ClientID %>");
        if (selectedRowIndex_dn == -1 || emptyPattern.test(dn_tblObj.rows[selectedRowIndex_dn + 1].cells[0].innerText)) {
            alert("Please Select Other Row!");
        }
        else {
            document.getElementById("<%=hidDeliveryNo.ClientID %>").value = dn_tblObj.rows[selectedRowIndex_dn + 1].cells[4].innerText;
            document.getElementById("<%=hidModel.ClientID %>").value = dn_tblObj.rows[selectedRowIndex_dn + 1].cells[0].innerText;
            document.getElementById("<%=hidPoNo.ClientID %>").value = dn_tblObj.rows[selectedRowIndex_dn + 1].cells[2].innerText;
            document.getElementById("<%=hidQty.ClientID %>").value = dn_tblObj.rows[selectedRowIndex_dn + 1].cells[5].innerText;
            document.getElementById("<%=hidPartNo.ClientID %>").value = dn_tblObj.rows[selectedRowIndex_dn + 1].cells[1].innerText;

            document.getElementById("<%=hidbtn.ClientID %>").click();
        }
    }



    function clickTable(con) {
        if ((selectedRowIndex != null) && (selectedRowIndex != parseInt(con.index, 10))) {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gd.ClientID %>");
        }

        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
        selectedRowIndex = parseInt(con.index, 10);

    }

    function dblclickTable(con) {        
        
    }


    function showPrintSettingDialog() {
        //     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
    
    
</script>

</asp:Content>