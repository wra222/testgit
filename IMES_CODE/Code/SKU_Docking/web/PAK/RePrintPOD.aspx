
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RePrintPOD.aspx.cs" Inherits="RePrintPOD" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="RePrintPOD" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceCollectTabletFaPart.asmx"/>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceUnitWeightNew.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
     <TR>
	   <td></td>
    </TR
    <TR>
	    <TD style="width:9%" align="left"><asp:Label ID="lblProdID" runat="server" Text="Customer SN:" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </ContentTemplate>
        </asp:UpdatePanel>                                        
       </TD>
    </TR>
    
   <tr>
	    <td style="width:9%" align="left" ><asp:Label ID="lbReason" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <textarea id="txtReason" style="width: 99%; height: 100px"></textarea>                
                </textarea>
                </ContentTemplate>                                     
         </asp:UpdatePanel>
         </td>  
    </tr>
                                      
    <tr>
	    <td style="width:9%" align="left"></td>
	    <td colspan="5" align="left"></td>
	   
	    <td align="right">
	        <table border="0" width="95%">
	            <td style="width:80%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'"  value="Print Setting"
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
	            <td><input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="print()"  value="RePrint POD"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        align="right"/></td>
	        </table>
        </td>
    </tr>
    </table>
    </center>
       
</div>

<script type="text/javascript">
    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var pdfClinetPath;
    var SUCCESSRET = "SUCCESSRET";
    var lstPrintItem;
    var inputObj;
    var line;
    var editor;
    var customer;
    var station;
    var blackPrinter = "";
    var whitePrinter = "";
    var podColor = "";
    var model = "";
    var pCode;
    var podLabelPath = "";
    var printerpath;
    document.body.onload = function() {
    WebServiceUnitWeightNew.GetPODLabelPathAndSite(onGetPathSucceed, onFailed);
        pdfClinetPath = "<%=PDFClinetPath%>";
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        station = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';
        inputObj = getCommonInputObject();
        ShowInfo("");
        getCommonInputObject().focus();
        inputData = inputObj.value;
        getAvailableData("ProcessInput");
        printerpath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
    }

    function onGetPathSucceed(result) {

        podLabelPath = result[0];
    }
    function onFailed(result) {
        endWaitingCoverDiv();
        ResetPage();
        ShowMessage(result.get_message());
        ShowInfo(result.get_message());
    }
    function ProcessInput(inputData) {
        try {
            print();
            getAvailableData("ProcessInput");
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function print() {
        
            var data = getCommonInputObject().value.trim();
            if (data == "") {
                alert("Please input or scan");
                callNextInput();
                return;
            }
            var reason = document.getElementById("txtReason").value;
            inpuProdid = getCommonInputObject().value.trim();
            lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null)                
                {
                    alert(msgPrintSettingPara);
                    callNextInput();
                    return;
                }
                else {
                    beginWaitingCoverDiv();
                    //RePrintPOD(string sn, string reason, string editor, string station, string customer)
                    WebServiceCollectTabletFaPart.RePrintPOD(inpuProdid, reason, editor, station, customer, onSucceed, onFail);
                }
             }

    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }
   

    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                //service方法没有返回
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
               GetPrinterName(lstPrintItem);
               model = result[1];
               podColor = result[2];
               PDFPrint();
               ShowInfo(msgSuccess, "green");
               document.getElementById("txtReason").value = "";
               callNextInput();
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                callNextInput();
            }
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }
    function callNextInput() {
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("print");
    }
    function onFail(error) {

        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
            callNextInput();

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }

    }
    // ********************For Print POD using ********************
    function GetPrinterName(printItem) {
        for (i = 0; i < printItem.length; i++) {
            if (printItem[i].LabelType.toUpperCase().indexOf("WHITE") >= 0) {
                whitePrinter = printItem[i].PrinterPort;
                continue;
            }
            if (printItem[i].LabelType.toUpperCase().indexOf("BLACK") >= 0) {
                blackPrinter = printItem[i].PrinterPort;
                continue;
            }
        }
    }
    function PDFPrint() {
        var pdfFileName = model + ".Pdf";
        var FsFile = "";
        var Fs = new ActiveXObject("Scripting.FileSystemObject");
        if (pdfClinetPath.slice(-1) == "\\") {
            FsFile = pdfClinetPath + "tabletpodprintlist.txt";
        }
        else {
            pdfClinetPath = pdfClinetPath + "\\";
            FsFile = pdfClinetPath + "tabletpodprintlist.txt";
        }

        if (!Fs.FolderExists(pdfClinetPath)) {
            Fs.CreateFolder(pdfClinetPath);
        }

        if (Fs.FileExists(FsFile)) {
            Fs.DeleteFile(FsFile);
        }
        File = Fs.CreateTextFile(FsFile, true);
        var pdfPath;
        pdfPath = podLabelPath + pdfFileName;
        File.WriteLine(pdfPath);
        File.Close();
        var wsh = new ActiveXObject("wscript.shell");
        var cmdpdfprint;
        if (podColor != "") {
            if (podColor == "White") {
                cmdpdfprint = "PrintPDF.bat " + FsFile + ' "' + whitePrinter + '"';
            }
            else {
                cmdpdfprint = "PrintPDF.bat " + FsFile + ' "' + blackPrinter + '"';

            }
        }
        else {
            cmdpdfprint = "PrintPDF.bat " + FsFile + " 4000";
        }

        if (!Fs.FileExists(FsFile)) {
            alert( pdfFileName + " does not exist");
        }
        else {
            pdfFlag = true;
            wsh.run("cmd /k " + getHomeDisk(printerpath) + "&cd " + printerpath + "&" + cmdpdfprint + "&exit", 2, false);
        }
        wsh = null;

    }
    // ********************For Print POD using ********************
    function showPrintSettingDialog() {
        showPrintSetting(station,pCode);
    }
</script>
</asp:Content>

