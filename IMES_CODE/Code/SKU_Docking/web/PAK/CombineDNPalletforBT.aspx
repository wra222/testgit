<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine DN & Pallet for BT
 * CI-MES12-SPEC-PAK-Combine DN & Pallet for BT.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>
 
 <%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CombineDNPalletforBT.aspx.cs" Inherits="PAK_CombineDNPalletforBT" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="Service/CombineDNBTWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" language="javascript"  >

    var editor = '<%=userId%>';
    var customer = '<%=customer%>';
    var station = '<%=station%>';
    var currentNO = "";
    var lastId = 1;
    var table = "";
    var inputObj;
    var msgModelCheck = '<%=this.GetLocalResourceObject(Pre + "_msgModelCheck").ToString() %>';
    var msgModelNoFind = '<%=this.GetLocalResourceObject(Pre + "_msgModelNoFind").ToString() %>';
    var msgNoPath = '<%=this.GetLocalResourceObject(Pre + "_msgNoPath").ToString() %>';
    var msgModeChange = '<%=this.GetLocalResourceObject(Pre + "_msgModeChange").ToString() %>';
    var msgNoCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgNoCDSI").ToString() %>';

    var PLEditsURL = '';
    var PLEditsTemplate = '';
    var PLEditsXML = '';
    var PLEditsPDF = '';
    var PLEditsImage = '';
    var FOPFullFileName = '';
    var PDFPrintPath = '';
    var successTemp = "";
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgCreatePDFFail = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCreatePDFFail") %>';
    var pdfCreateFailed = "";
    var isFocus = 0;

    window.onload = function() { 
        getPdLineCmbObj().selectedIndex = 0;
        document.getElementById("<%=HSN.ClientID%>").value = "";
        getLocFloorCmbObj().selectedIndex = 0;
        table = document.getElementById("<%=gridViewDN.ClientID%>");
        inputObj = getCommonInputObject();
        callNextInput();
        var nameCollection = new Array();
        nameCollection.push("PLEditsImage");
        nameCollection.push("PLEditsURL");
        nameCollection.push("PLEditsXML");
        nameCollection.push("PLEditsTemplate");
        nameCollection.push("PLEditsPDF");
        nameCollection.push("FOPFullFileName");
        beginWaitingCoverDiv();
        CombineDNBTWebService.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);
        //document.getElementById("<%=btnGetSetting.ClientID%>").click();
    }
    function onGetSetting(result) {

        endWaitingCoverDiv();
        callNextInput();
        if (result[0] == SUCCESSRET) {
            PLEditsImage = result[1][0];
            PLEditsURL = result[1][1];
            PLEditsXML = result[1][2];
            PLEditsTemplate = result[1][3];
            PLEditsPDF = result[1][4];
            FOPFullFileName = result[1][5];
            var path = PLEditsImage + "\*.jpg";
            //var fso = new ActiveXObject("Scripting.FileSystemObject");
            //var fileflag = fso.FolderExists(PLEditsImage);
            //if (fileflag) {
            //    fso.CopyFile(path, "C:\\");
            //}
            //else {
            //    alert(msgNoPath);
            //}

        } else {
            ShowInfo("");
            var content = result[0];
            alert(content);
            ShowInfo(content);
        }

        callNextInput();

        /*document.getElementById("<%=HLine.ClientID%>").value = "AAA";
        document.getElementById("<%=HDN.ClientID%>").value = "4108932703000010";
        document.getElementById("<%=HSN.ClientID%>").value = "CNU2070078";
        document.getElementById("<%=btnGetSetting.ClientID%>").click();*/

    }

    function onGetSettingFailed(result) {
        endWaitingCoverDiv();
        //ResetPage();
        ShowMessage(result.get_message());
        ShowInfo(result.get_message());
        callNextInput();
    }
    function IsNumber(src)
    {
        return src == parseInt(src);
    }
    function GetDNByModel(Model) {

        var dgData = document.getElementById("<%=gridViewDN.ClientID%>");
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model) {
                    //dgData.rows[iRow].cells[0].click();
                    var el = dgData.rows[iRow].cells[0].all.tags("input");
                    for (i = 0; i < el.length; i++) {
                        if (el[i].type == "radio") {
                            if (el[i].disabled == false && el[i].isDisabled == false) {
                                if (iRow - 1 < 0) {
                                    setSrollByIndex(0, false, "<%=gridViewDN.ClientID%>");
                                }
                                else {
                                    setSrollByIndex(iRow - 1, false, "<%=gridViewDN.ClientID%>");
                                }
                                el[i].click();
                                return;
                            }
                        }
                    }
                }
                else {
                    break;
                }
            }
        }
    }
    function GetDNByDN(DN) {

        var dgData = document.getElementById("<%=gridViewDN.ClientID%>");
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == DN) {
                    //dgData.rows[iRow].cells[0].click();
                    var el = dgData.rows[iRow].cells[0].all.tags("input");
                    for (i = 0; i < el.length; i++) {
                        if (el[i].type == "radio") {
                            if (el[i].disabled == false && el[i].isDisabled == false) {
                                if (iRow - 1 < 0) {
                                    setSrollByIndex(0, false, "<%=gridViewDN.ClientID%>");
                                }
                                else {
                                    setSrollByIndex(iRow - 1, false, "<%=gridViewDN.ClientID%>");
                                }
                                el[i].click();
                                return;
                            }
                        }
                    }
                }
                else {
                    break;
                }
            }
        }
        FirstRadioToCheck();
    }
    function FirstRadioToCheck() {
       table = document.getElementById("<%=gridViewDN.ClientID%>");
        var el = table.all.tags("input");
        for (i = 0; i < el.length; i++) {
            if (el[i].type == "radio") {
                if (el[i].disabled == false && el[i].isDisabled == false) {
                    setSrollByIndex(0, false, "<%=gridViewDN.ClientID%>");
                    el[i].click();
                    break;
                }
            }
        }
    }
    function FirstRadioCheckAndEndSuccess() {

        if (document.getElementById("<%=HDNTemp.ClientID%>").value != "") {
            GetDNByDN(document.getElementById("<%=HDNTemp.ClientID%>").value);
            document.getElementById("<%=HDNTemp.ClientID%>").value = "";
        }
        else {
            table = document.getElementById("<%=gridViewDN.ClientID%>");
            var el = table.all.tags("input");
            for (i = 0; i < el.length; i++) {
                if (el[i].type == "radio") {
                    if (el[i].disabled == false && el[i].isDisabled == false) {
                        setSrollByIndex(0, false, "<%=gridViewDN.ClientID%>");
                        el[i].click();
                        break;
                    }
                }
            }
        }
        //ShowInfo("Success");
        if (successTemp != "") {
            //ShowSuccessfulInfo(true);
            ShowSuccessfulInfo(true, successTemp);
        }
        else {
            ShowSuccessfulInfo(true);
        }
        endWaitingCoverDiv();
    }
    function FirstRadioToCheckAndEnd() {

        if (document.getElementById("<%=HDNTemp.ClientID%>").value != "") {
            GetDNByDN(document.getElementById("<%=HDNTemp.ClientID%>").value);
            document.getElementById("<%=HDNTemp.ClientID%>").value = "";
        }
        else {
            table = document.getElementById("<%=gridViewDN.ClientID%>");
            var el = table.all.tags("input");
            for (i = 0; i < el.length; i++) {
                if (el[i].type == "radio") {
                    if (el[i].disabled == false && el[i].isDisabled == false) {
                        setSrollByIndex(0, false, "<%=gridViewDN.ClientID%>");
                        el[i].click();
                        break;
                    }
                }
            }
        }
        endWaitingCoverDiv();
    }
    function setSelectVal(span, id) {
        //ShowInfo("");
        endWaitingCoverDiv();
       
        theRadio = span;
        oState = theRadio.checked;
        if (oState) {
            document.getElementById("<%=txtScanQty.ClientID%>").value = "";
            document.getElementById("<%=txtPalletQty.ClientID%>").value = "";
            //document.getElementById("<%=btnClear.ClientID%>").click();
            currentNO = id;
            document.getElementById("<%=HDN.ClientID%>").value = id;
            //callNextInput();
            
            if (lastId != "") {
                var temp = document.getElementById(lastId);
                if (null != temp) {
                    temp.checked = false;
                }
            }
            theRadio.checked = true;
            lastId = theRadio.id;
        }
        callNextInput();
        document.getElementById("<%=btnFocus.ClientID%>").click();
       
        //callNextInput();
    }
    function drpOnChange() {
        document.getElementById("<%=txtScanQty.ClientID%>").value = "";
        document.getElementById("<%=txtPalletQty.ClientID%>").value = "";
        document.getElementById("<%=HPQty.ClientID%>").value = "";
        document.getElementById("<%=HPalletNO.ClientID%>").value = "";
        var obj = document.getElementById("<%=drpPalletChange.ClientID%>");
        outStatus = obj.value;
        var id = obj.selectedIndex;
        inStatus = obj[id].innerText;
        document.getElementById("<%=HPQty.ClientID%>").value = outStatus;
        document.getElementById("<%=txtPalletQty.ClientID%>").value = document.getElementById("<%=HPQty.ClientID%>").value;
        document.getElementById("<%=HPalletNO.ClientID%>").value = inStatus;
        callNextInput();
        GetProductClick();
    }
    
    function getScanQty(Qty) {
        document.getElementById("<%=txtScanQty.ClientID%>").value = Qty;
    }
    function getPQty(Qty) {
        document.getElementById("<%=txtPalletQty.ClientID%>").value = Qty;
    }
    function GetProductClick() {
        document.getElementById("<%=btnGetProduct.ClientID%>").click();
    }
    
    function callNextInput() {
        beginWaitingCoverDiv();
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("input");
        endWaitingCoverDiv();
        
    }
    function callNextInput2() {
        beginWaitingCoverDiv();
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("input");
        endWaitingCoverDiv();
        document.getElementById("<%=btnGetPallet.ClientID%>").click();
    }
   
    function DisplsyMsg(src) {
        ShowMessage(src);
        ShowInfo(src);
    }
    /*function input(data) {
    ShowInfo("");
    var inputTextBox1 = data.trim();
    GetDNByModel(data);
    callNextInput();
    }*/
    function input(data) {
        ShowInfo("");
        successTemp = "";
        pdfCreateFailed = "";
        var inputTextBox1 = data.trim();
        var line = "";
        var floor = "";
        line = getPdLineCmbValue();
        floor = getLocFloorCmbValue();
        var pdLineObject = getPdLineCmbObj();
        if(line == "") {
            alert("Please input PdLine first!")
            callNextInput();
            return;
        }
        document.getElementById("<%=HLine.ClientID%>").value = line;
        if (floor == "") {
            alert("Please input Floor first!")
            callNextInput();
            return;
        }
    //    floor = document.getElementById("CmbFloor")[document.getElementById("CmbFloor").selectedIndex].text;
        floor = getLocFloorCmbValue();
        document.getElementById("<%=HFloor.ClientID%>").value = floor;
        inputTextBox1 = inputTextBox1.toUpperCase();
        if (document.getElementById("<%=HSN.ClientID%>").value == "") {
            if (inputTextBox1.length == 10 || inputTextBox1.length == 11) {
                pattCustSN1 = /^CN.{8}$/;
                pattCustSN2 = /^SCN.{8}$/;
                //if (pattCustSN1.exec(inputTextBox1) || pattCustSN2.exec(inputTextBox1)) {
				if (CheckCustomerSN(inputTextBox1)) {
                if (inputTextBox1.length == 11) {
                    inputTextBox1 = inputTextBox1.substring(1, 11);
                }
            }
            else {
                resetLast();
                DisplsyMsg("Wrong Code!");
                return;
            }
        }
        else {
            resetLast();
            DisplsyMsg("Wrong Code!");
            return;
        }
            document.getElementById("<%=HSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=btnCheckProduct.ClientID%>").click();
            callNextInput();
        }
    }
    
    function getSuccess(name) {
        pdfCreateFailed = "";
        if (name != "") {
            getPDF(name);
        }
        var temp = document.getElementById("<%=HSN.ClientID%>").value;
        successTemp = "";
        if (temp != "") {
            //successTemp = msgSuccess + " Customer SN: " + temp  ;
            successTemp =  "[" + temp + "] " + msgSuccess;
        }
        if (pdfCreateFailed != "") {
            successTemp = successTemp + "\n" + pdfCreateFailed;
        }
        document.getElementById("<%=HSN.ClientID%>").value = "";
        //document.getElementById("<%=HLine.ClientID%>").value = "";
        //document.getElementById("<%=HFloor.ClientID%>").value = "";
        document.getElementById("<%=txtScanQty.ClientID%>").value = "";
        document.getElementById("<%=txtPalletQty.ClientID%>").value = "";
        callNextInput();
        beginWaitingCoverDiv();
        document.getElementById("<%=btnGridFreshSuccess.ClientID%>").click();
        
        //document.getElementById("<%=HDisplay.ClientID%>").value = "Success";
        //document.getElementById("<%=btnDisplay.ClientID%>").click();
        
    }
    function getWaitEnd() {
        endWaitingCoverDiv();
    }
    function resetLast() {
        document.getElementById("<%=HSN.ClientID%>").value = "";
        callNextInput();
    }
    function ResetAll() {
        //getPdLineCmbObj().selectedIndex = 0;
        //getFloorCmbObj().selectedIndex = 0;
        document.getElementById("<%=HSN.ClientID%>").value = "";
        //document.getElementById("<%=HLine.ClientID%>").value = "";
        //document.getElementById("<%=HFloor.ClientID%>").value = "";
        document.getElementById("<%=txtScanQty.ClientID%>").value = "";
        document.getElementById("<%=txtPalletQty.ClientID%>").value = "";
        //FirstRadioToCheck();
        callNextInput();
        beginWaitingCoverDiv();
        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }
    function getModelFalse(modelBySN) {
        if (document.getElementById("<%=HDN.ClientID%>").value == "------------") {
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            GetDNByModel(modelBySN);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                FirstRadioToCheck(); 
            }
            else {
                DisplsyMsg(msgModelNoFind + "\n Model:" + modelBySN);
                ResetAll();
                return;
            }
            document.getElementById("<%=HDN.ClientID%>").value = "------------";
            document.getElementById("<%=btnAssign.ClientID%>").click(); 
        }
        else {
           /* var strTemp = modelBySN + msgModelCheck;
            if (confirm(strTemp)) {
                var temp = document.getElementById("<%=HDN.ClientID%>").value;
                GetDNByModel(modelBySN);
                if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                document.getElementById("<%=btnAssign.ClientID%>").click(); 
                }
                else {
                ResetAll();
                DisplsyMsg(msgModelNoFind);
                }
            }
            else {
                ResetAll();
            }*/
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            GetDNByModel(modelBySN);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                FirstRadioToCheck();
                ShowInfo(msgModeChange); 
            }
            else {
                DisplsyMsg(msgModelNoFind + "\n Model:" + modelBySN);
                //getPdLineCmbObj().selectedIndex = 0;
                //getFloorCmbObj().selectedIndex = 0;
                document.getElementById("<%=HSN.ClientID%>").value = "";
                //document.getElementById("<%=HLine.ClientID%>").value = "";
                //document.getElementById("<%=HFloor.ClientID%>").value = "";
                document.getElementById("<%=txtScanQty.ClientID%>").value = "";
                document.getElementById("<%=txtPalletQty.ClientID%>").value = "";
                //FirstRadioToCheck();
                callNextInput();
                beginWaitingCoverDiv();
                document.getElementById("<%=btnGridFresh.ClientID%>").click();
                return;
            }
            document.getElementById("<%=HDN.ClientID%>").value = "------------";
            document.getElementById("<%=btnAssign.ClientID%>").click();
        }
    }
    function GetDNByModelCDSI(Model) {
        var dgData = document.getElementById("<%=gridViewDN.ClientID%>");
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                var temp = dgData.rows[iRow].cells[2].innerText + "#@#" + dgData.rows[iRow].cells[4].innerText;
                if (temp == Model) {
                    var el = dgData.rows[iRow].cells[0].all.tags("input");
                    for (i = 0; i < el.length; i++) {
                        if (el[i].type == "radio") {
                            if (el[i].disabled == false && el[i].isDisabled == false) {
                                if (iRow - 1 < 0) {
                                    setSrollByIndex(0, false, "<%=gridViewDN.ClientID%>");
                                }
                                else {
                                    setSrollByIndex(iRow - 1, false, "<%=gridViewDN.ClientID%>");
                                }
                                el[i].click();
                                return;
                            }
                        }
                    }
                }
                else {
                    break;
                }
            }
        }
    }
    function getModelCDSI(modelBySN) {
        var FNO = modelBySN.substring(modelBySN.indexOf("#@#")+3);
        if (document.getElementById("<%=HDN.ClientID%>").value == "------------") {
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            GetDNByModelCDSI(modelBySN);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                FirstRadioToCheck();
            }
            else {
                var tmp = msgNoCDSI;
                tmp = tmp.replace("%1", FNO);
                DisplsyMsg(tmp + "\n Model:" + modelBySN);
                ResetAll();
                return;
            }
            document.getElementById("<%=HDN.ClientID%>").value = "------------";
            document.getElementById("<%=btnAssign.ClientID%>").click();
        }
        else {
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            GetDNByModelCDSI(modelBySN);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                FirstRadioToCheck();
                ShowInfo(msgModeChange);
            }
            else {
                var tmp = msgNoCDSI;
                tmp = tmp.replace("%1", FNO);
                DisplsyMsg(tmp + "\n Model:" + modelBySN);
                document.getElementById("<%=HSN.ClientID%>").value = "";
                document.getElementById("<%=txtScanQty.ClientID%>").value = "";
                document.getElementById("<%=txtPalletQty.ClientID%>").value = "";
                callNextInput();
                beginWaitingCoverDiv();
                document.getElementById("<%=btnGridFresh.ClientID%>").click();
                return;
            }
            document.getElementById("<%=HDN.ClientID%>").value = "------------";
            document.getElementById("<%=btnAssign.ClientID%>").click();
        }
    }
    function getDisplay(display) {
        ShowInfo(display);
    }
    
   
   
   
    function getEditURL(edit) {
        PLEditsURL = edit;
    }
    function getXML(xml) {
        PLEditsXML = xml;
    }
     function getEditsPDF(editsPDF) {
         PLEditsPDF = editsPDF;
    }
  
    
    
    function getImage(image) {
        PLEditsImage = image;
        var path = PLEditsImage + "\\*.jpg";
        //var fso = new ActiveXObject("Scripting.FileSystemObject");
        //var fileflag = fso.FolderExists(path);
        //if (fileflag) {
        //    fso.CopyFile(path, "C:\\");
        //}
        //else {
        //    alert(msgNoPath);
        //}
    }
    
    function getTemplate(template)
    {
        PLEditsTemplate = template;
    }

    function getFOPFullFileName(fop) {
        
        FOPFullFileName = fop;
        //endWaitingCoverDiv();
    }
    function getAllSetting(ret) {

        PLEditsImage = ret[0];
        PLEditsURL = ret[1];
        PLEditsXML = ret[2];
        PLEditsTemplate = ret[3];
        PLEditsPDF = ret[4];
        FOPFullFileName = ret[5];
        var path = PLEditsImage + "*.jpg";
        //var fso = new ActiveXObject("Scripting.FileSystemObject");
        //var fileflag = fso.FolderExists(PLEditsImage);
        //if (fileflag) {
        //    fso.CopyFile(path, "C:\\");
        //}
        //else {
        //    alert(msgNoPath);
        //}
    }
    
    function getPDF(tempName) {
        var CmbPL = document.getElementById("<%=HLine.ClientID%>").value;
        var DeliveryNo = document.getElementById("<%=HDN.ClientID%>").value;
        if ("" == DeliveryNo || "------------" == DeliveryNo) {
            if ("" != document.getElementById("<%=HDNTemp.ClientID%>").value) {
                DeliveryNo = document.getElementById("<%=HDNTemp.ClientID%>").value;
            }
        }
        var CPQ = document.getElementById("<%=HSN.ClientID%>").value;
        var cmbPLSub = CmbPL.substr(0, 1);
        var XmlFilename = cmbPLSub + "\\" + DeliveryNo + "-" + CPQ + "-[BoxShipLabel].xml";
        var PdfFilename = cmbPLSub + "\\" + DeliveryNo + "-" + CPQ + "-[BoxShipLabel].pdf";
        CallEDITSFunc(XmlFilename, DeliveryNo, CPQ);
        CallPdfCreateFunc(XmlFilename, tempName, PdfFilename);
    }
    
    function CallEDITSFunc(XmlFilename, DeliveryNo, CPQ) {
        var Paralist = new EDITSFuncParameters();
        var xmlpathfile = "";
        var webEDITSaddr = "";
      
        if (PLEditsXML == "" || PLEditsURL == "") {
            alert("EDIT Path error!");
            return false;
        }
        xmlpathfile = PLEditsXML + "XML\\" + XmlFilename;
        webEDITSaddr = PLEditsURL;
       
        Paralist.add(1, "FilePH", xmlpathfile);
        Paralist.add(2, "Dn", DeliveryNo);
        Paralist.add(3, "SerialNum", CPQ);
        var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
        return IsSuccess;



        /* if (GetDebugMode()) {
        //Debug Mode get Root path from Web.conf
        xmlpathfile = GetCreateXMLfileRootPath() + "XML\\" +  XmlFilename;
        CheckMakeDir(xmlpathfile);
        //webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
        webEDITSaddr = GetEDITSTempIP();   //Packing List for aaaa
        }
        else {
        if (PLEditsXML == "" || PLEditsURL == "") {
        alert("EDIT Path error!");
        return false;
        } 
        //Run Mode Get Path from DB, set Full Path
        //xmlpathfile = "\\\\10.190.40.68\\OUT\\XML\\packingShipmentLabel01.xml";
        //webEDITSaddr = "http://10.190.40.68:80/edits.asmx";
        xmlpathfile = PLEditsXML + "XML\\" + XmlFilename;
        webEDITSaddr = PLEditsURL;
        //webEDITSaddr = "http://10.190.40.68:80/edits.asmx";
        //webEDITSaddr = "http://10.190.40.68:8080/edits.asmx"; // Packing List for aaa
        }*/
    }

    function CallPdfCreateFunc(XmlFilename, xsl, PdfFilename) {
        var xmlfilename, xslfilename, pdffilename;
        if (PLEditsXML == "" || PLEditsURL == "" || PLEditsTemplate == "") {
            alert("EDIT Path error!");
            return false;
        }

        //Run Mode Get Path from DB, set Full Path
        xmlfilename = PLEditsXML + "XML\\" + XmlFilename;
        xslfilename = PLEditsTemplate + xsl;
        pdffilename = PLEditsPDF + "pdf\\" + PdfFilename;
       
        var islocalCreate = false;
        //var islocalCreate = true;
        //================================================================
        //var IsSuccess = CreatePDFfileAsyn(FOPFullFileName, xmlfilename, xslfilename, pdffilename, islocalCreate);
        var IsSuccess = CreatePDFfileAsynGenPDF(PLEditsURL, xmlfilename, xslfilename, pdffilename, islocalCreate);
        if (!IsSuccess) {
            pdfCreateFailed = msgCreatePDFFail;
        }
        else {
            pdfCreateFailed = "";
        }
        return IsSuccess;


        /*if (GetDebugMode()) {
        //Debug Mode get Root path from Web.conf
        xmlfilename = "XML\\" + XmlFilename;
        xslfilename = xsl;
        pdffilename = "pdf\\" + PdfFilename;
        }
        else {
        if (PLEditsXML == "" || PLEditsURL == "" || PLEditsTemplate == "") {
        alert("EDIT Path error!");
        return false;
        } 
            
        //Run Mode Get Path from DB, set Full Path
        xmlfilename = PLEditsXML + "XML\\" + XmlFilename;
        xslfilename = PLEditsTemplate + xsl;
        pdffilename = PLEditsPDF + "pdf\\" + PdfFilename;
        }*/
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
    function DoFocus() {
        isFocus = 1;
    }
    function DoNotFocus() {
        isFocus = 0;
    }
    </script>

 <div>
   <center >
   <table border="0" width="100%" >
    <tr>
        <td align="left" >
            <asp:Label ID="lbPdLine" Width = "9%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="80" IsPercentage="true" />
        </td>
    </tr>
    <tr>
	    <td align="left" >
	    <asp:Label ID="lbFloor" Width = "9%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	       
              <iMES:CmbLocFloor id="CmbLocFloor"  name='CmbFloor'  runat="server" Width="80" IsPercentage="true" />
	    </td>
    </tr>
    <tr>
        <td>
        <fieldset>
            <legend><asp:Label ID="lbTableDNTitle" runat="server"></asp:Label></legend>
            <asp:UpdatePanel runat="server" ID="UpdatePanelTableDN" UpdateMode="Conditional">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gridViewDN" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                        GvExtWidth="99%" GvExtHeight="148px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                        Width="97%" Height="148px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                        HighLightRowPosition="3" HorizontalAlign="Left" 
                        onrowdatabound="GridView1_RowDataBound"> 
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton id="radio" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Delivery NO" />
                            <asp:BoundField DataField="Model" />
                            <asp:BoundField DataField="Customer P/N" />
                            <asp:BoundField DataField="PoNo" />
                            <asp:BoundField DataField="Date" />
                            <asp:BoundField DataField="Qty" />
                            <asp:BoundField DataField="Packed Qty" />
                        </Columns>
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
            </fieldset>
        </td>
    </tr>
    
     <tr>
        <td align="left" >
        <fieldset style="width: auto">
            <legend><asp:Label ID="lbPalletList" runat="server"></asp:Label></legend>
             <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lbPallet"  Width = "13%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:DropDownList ID="drpPalletChange" runat="server"  Width="35%"   ></asp:DropDownList>
                    <asp:Label ID="lbScanQty"  Width = "8%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                      <asp:TextBox ID="txtScanQty" runat="server"  style="width:8%"    />
                      <asp:Label ID="lbPalletQty"  Width = "8%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                      <asp:TextBox ID="txtPalletQty" runat="server"  style="width:8%"    />
                </ContentTemplate>
            </asp:UpdatePanel>
         </fieldset>
        </td>
    </tr>
    <tr>
        <td >
        <fieldset>
            <legend><asp:Label ID="lbProduct" runat="server"></asp:Label></legend>
            <asp:UpdatePanel runat="server" ID="UpdatePanelTableProduct" UpdateMode="Conditional">
                <ContentTemplate>
                 <iMES:GridViewExt ID="gridViewProduct" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="99%" GvExtHeight="82px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                  Width="97%" Height="82px"   SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left" >
                        <Columns>
                            <asp:BoundField DataField="Product Id" />
                            <asp:BoundField DataField="Customer S/N" />
                        </Columns>
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
            </fieldset>
        </td>
    </tr>
   
    <tr>
        <td align="left" colspan="1">
            <asp:Label ID="lbDataEntry" Width = "13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
            <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="60%" IsClear="true" IsPaste="true" onfocus="DoFocus();" onblur = "DoNotFocus();"/>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input id="btnPrintSetting" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'" visible="False"   />
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
	            <ContentTemplate>
	              <button id="btnGridFresh" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshSuccess" runat="server" type="button" style="display: none" />
	              <button id="btnGetPallet" runat="server" type="button" style="display: none" />
	              <button id="btnGetProduct" runat="server" type="button" style="display: none" />
	              <button id="btnCheckProduct" runat="server" type="button" style="display: none" />
	              <button id="btnClear" runat="server" type="button" style="display: none" />
	              <button id="btnAssign" runat="server" type="button" style="display: none" />
	              <button id="btnDisplay" runat="server" type="button" style="display: none" />
	              <button id="btnGetSetting" runat="server" type="button" style="display: none" />
	              <button id="btnFocus" runat="server" type="button" style="display: none" />
	              <input type="hidden" runat="server" id="HDN" />  
	              <input type="hidden" runat="server" id="HSN" />  
	              <input type="hidden" runat="server" id="HPQty" /> 
	              <input type="hidden" runat="server" id="HPalletNO" /> 
	              <input type="hidden" runat="server" id="HLine" /> 
	              <input type="hidden" runat="server" id="HFloor" /> 
	              <input type="hidden" runat="server" id="HDisplay" />
	              <input type="hidden" runat="server" id="HDNTemp" />
	            </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
    </center>
</div>

</asp:Content>