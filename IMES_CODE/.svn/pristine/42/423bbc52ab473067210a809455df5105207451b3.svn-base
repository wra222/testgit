<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine COA and DN
 * CI-MES12-SPEC-PAK-Combine COA and DN.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CombineCOAandDN.aspx.cs" Inherits="PAK_CombineCOAandDN" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>  
			<asp:ServiceReference Path="Service/CombineCOAandDNWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" language="javascript"  >
    var editor = '<%=userId%>';
    var customer = '<%=customer%>';
    var station = '<%=station%>';
    var pCode = '<%=code%>';
    var lastId = 1;
    var table = "";
    var inputObj;
    var username = '<%=UserName%>';
    var login = '<%=Login%>';
    var accountid = '<%=AccountId%>';
    var msgInputPDLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
    var msgModelCheck = '<%=this.GetLocalResourceObject(Pre + "_msgModelCheck").ToString() %>';
    var msgModelNoFind = '<%=this.GetLocalResourceObject(Pre + "_msgModelNoFind").ToString() %>';
    var msgNeedCoa = '<%=this.GetLocalResourceObject(Pre + "_msgNeedCoa").ToString() %>';
    var msgNotNeedCoa = '<%=this.GetLocalResourceObject(Pre + "_msgNotNeedCoa").ToString() %>';
    var msgModeChange = '<%=this.GetLocalResourceObject(Pre + "_msgModeChange").ToString() %>';
    var msgDNChange = '<%=this.GetLocalResourceObject(Pre + "_msgDNChange").ToString() %>';
    var msgCoaInput = '<%=this.GetLocalResourceObject(Pre + "_msgCoaInput").ToString() %>';
    var msgCoaCheck = '<%=this.GetLocalResourceObject(Pre + "_msgCoaCheck").ToString() %>';
    var ModeChange = "";
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var successTemp = "";
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgQualityCheck = '<%=this.GetLocalResourceObject(Pre + "_msgQualityCheck").ToString() %>';
    var msgNoCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgNoCDSI").ToString() %>';
    var msgNomal = '<%=this.GetLocalResourceObject(Pre + "_msgNomal").ToString() %>';
    var msgWin8 = '<%=this.GetLocalResourceObject(Pre + "_msgWin8").ToString() %>';
    var msgCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgCDSI").ToString() %>';
    var msgFNOWrong = '<%=this.GetLocalResourceObject(Pre + "_msgFNOWrong").ToString() %>';
    var index = 0;
    var showTemp = "";
    var msgException = '<%=this.GetLocalResourceObject(Pre + "_msgException").ToString() %>';
    var inputTextBox1;
    window.onload = function() {
        if ("" == pCode || null == pCode) {
            pCode = '<%=Request["PCode"] %>';
        }
        if ("" == station || null == station) {
            station = '<%=Request["Station"] %>';
        }
        document.getElementById("<%=HLine.ClientID%>").value = "";
        table = document.getElementById("<%=gridViewExt1.ClientID%>");
        callNextInput();
    }
    function IsNumber(src)
    {
        var regNum = /^[0-9]+[0-9]*]*$/;
        return regNum.test(src);
    }

    function onGetDNByDN(result) {
        var modeTemp = result[4];
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (index <= dgData.rows.length - 1) {
            if (dgData.rows[index].cells[1].innerText == document.getElementById("<%=HDN.ClientID%>").value) {
                if (result[0] == SUCCESSRET) {
                    dgData.rows[index].cells[2].innerText = result[4];
                    dgData.rows[index].cells[4].innerText = result[6];
                    dgData.rows[index].cells[5].innerText = result[2];
                    dgData.rows[index].cells[6].innerText = result[7];
                    dgData.rows[index].cells[7].innerText = result[5];
                    if (result[7] == result[5]) {
                        document.getElementById("<%=HFull.ClientID%>").value = "1";
                        document.getElementById("<%=HMode.ClientID%>").value = result[4];
                        document.getElementById("<%=HClear.ClientID%>").value = "0";
                        document.getElementById("<%=HDN.ClientID%>").value = "------------";
                        document.getElementById("<%=HSN.ClientID%>").value = "";
                        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
                        document.getElementById("<%=HPartNO.ClientID%>").value = "";
                        document.getElementById("<%=HIsBT.ClientID%>").value = "";
                        document.getElementById("<%=HLine.ClientID%>").value = "";
                        ModeChange = "";
                        beginWaitingCoverDiv();
                        document.getElementById("<%=btnGridFreshFull.ClientID%>").click();
                        return;
                    }
                }
                if (index - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(index - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                dgData.rows[index].cells[0].all[1].click();
                return;
            }
        }
        
        
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            index = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == document.getElementById("<%=HDN.ClientID%>").value) {

                    if (result[0] == SUCCESSRET) {
                        dgData.rows[iRow].cells[2].innerText = result[4];
                        dgData.rows[iRow].cells[4].innerText = result[6];
                        dgData.rows[iRow].cells[5].innerText = result[2];
                        dgData.rows[iRow].cells[6].innerText = result[7];
                        dgData.rows[iRow].cells[7].innerText = result[5];
                        if (result[7] == result[5]) {
                            document.getElementById("<%=HFull.ClientID%>").value = "1";
                            document.getElementById("<%=HMode.ClientID%>").value = result[4]; ;
                            document.getElementById("<%=HClear.ClientID%>").value = "0";
                            document.getElementById("<%=HDN.ClientID%>").value = "------------";
                            document.getElementById("<%=HSN.ClientID%>").value = "";
                            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
                            document.getElementById("<%=HPartNO.ClientID%>").value = "";
                            document.getElementById("<%=HIsBT.ClientID%>").value = "";
                            document.getElementById("<%=HLine.ClientID%>").value = "";
                            ModeChange = "";
                            beginWaitingCoverDiv();
                            document.getElementById("<%=btnGridFreshFull.ClientID%>").click();
                            return;
                        }
                    }

                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
        document.getElementById("<%=HMode.ClientID%>").value = modeTemp;
        GetDNByModelFull();
    }
  
    function GetDNByDN(DNNumber) {
        CombineCOAandDNWebService.GetDNInfo(DNNumber, onGetDNByDN, GetDNInfoFail);
    }
    function GetDNInfoFail(result) {
    }
    function GetDNByDNOnPage(DN) {

        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (index <= dgData.rows.length - 1) {
            if (dgData.rows[index].cells[1].innerText == DN) {

                if (index - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(index - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                dgData.rows[index].cells[0].all[1].click();
                return;
            }
        }
        
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            index = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == DN) {

                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
        CheckFirst();
    }
    function GetModleByDN(DN) {

        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");

        if (index <= dgData.rows.length - 1) {
            if (dgData.rows[index].cells[1].innerText == DN) {
                document.getElementById("<%=HMode.ClientID%>").value = dgData.rows[index].cells[2].innerText;
                document.getElementById("<%=HPONO.ClientID%>").value = dgData.rows[index].cells[4].innerText;
                return;
            }
        }
 
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            index = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == DN) {
                    document.getElementById("<%=HMode.ClientID%>").value = dgData.rows[iRow].cells[2].innerText;
                    document.getElementById("<%=HPONO.ClientID%>").value = dgData.rows[iRow].cells[4].innerText;
                    return;
                }
                else {
                    break;
                }
            }
        }
    }
    function CheckFirst() {
        table = document.getElementById("<%=gridViewExt1.ClientID%>");
        var iRow = 1;
        if (iRow <= table.rows.length - 1) {
            index = iRow;
            if (iRow - 1 < 0) {
                setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
            }
            else {
                setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
            }
            table.rows[iRow].cells[0].all[1].click();
            return;
        }
    }
    function FirstRadioToCheck() {

        if (document.getElementById("<%=HDN.ClientID%>").value != ""
        && document.getElementById("<%=HDN.ClientID%>").value != "------------") {
            document.getElementById("<%=HClear.ClientID%>").value = "0";
            GetDNByDNOnPage(document.getElementById("<%=HDN.ClientID%>").value);
        }
        else {
            table = document.getElementById("<%=gridViewExt1.ClientID%>");
            var iRow = 1;
            if (iRow <= table.rows.length - 1) {
                index = iRow;
                if (iRow - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                table.rows[iRow].cells[0].all[1].click();
                return;
            }
        }
        
    }
    
    function FirstRadioToCheckSuccess() {
        if (document.getElementById("<%=HDN.ClientID%>").value != ""
        && document.getElementById("<%=HDN.ClientID%>").value != "------------") {
            document.getElementById("<%=HClear.ClientID%>").value = "0";
            GetDNByDN(document.getElementById("<%=HDN.ClientID%>").value);
            beginWaitingCoverDiv();
            getSuccessFresh();
            endWaitingCoverDiv();
            return;
        }
        else {
            table = document.getElementById("<%=gridViewExt1.ClientID%>");
            var iRow = 1;
            if (iRow <= table.rows.length - 1) {
                index = iRow;
                if (iRow - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                table.rows[iRow].cells[0].all[1].click();
                beginWaitingCoverDiv();
                getSuccessFresh();
                endWaitingCoverDiv();
                return;
            }
        }
    }

    function FirstRadioToCheckFailed() {
        if (document.getElementById("<%=HDN.ClientID%>").value != ""
        && document.getElementById("<%=HDN.ClientID%>").value != "------------") {
            document.getElementById("<%=HClear.ClientID%>").value = "0";
            GetDNByDN(document.getElementById("<%=HDN.ClientID%>").value);
            beginWaitingCoverDiv();
            var HInfoTemp = document.getElementById("<%=HInfo.ClientID%>").value ;
            if (HInfoTemp != "") {
                document.getElementById("<%=HInfo.ClientID%>").value = "";
                ShowInfo(HInfoTemp);
            }
            else {
                if (document.getElementById("<%=txtCOA.ClientID%>").value == "") {
                    successTemp = successTemp + "\n" + msgNotNeedCoa;
                    ShowInfo(successTemp);
                }
                else {
                    ShowInfo(successTemp);
                }
                successTemp = "";
            }
            endWaitingCoverDiv();
            return;
        }
        else {
            table = document.getElementById("<%=gridViewExt1.ClientID%>");
            var iRow = 1;
            if (iRow <= table.rows.length - 1) {
                index = iRow;
                if (iRow - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                table.rows[iRow].cells[0].all[1].click();
                beginWaitingCoverDiv();
                var HInfoTemp = document.getElementById("<%=HInfo.ClientID%>").value;
                if (HInfoTemp != "") {
                    document.getElementById("<%=HInfo.ClientID%>").value = "";
                    ShowInfo(HInfoTemp);
                }
                else {
                    ShowInfo(successTemp);
                    successTemp = "";
                }
                endWaitingCoverDiv();
                return;
            }
        }
    }


    function GetDNByModel(Model) {
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (index <= dgData.rows.length - 1) {
            if (dgData.rows[index].cells[2].innerText == Model) {
                if (dgData.rows[index].cells[6].innerText == dgData.rows[index].cells[7].innerText) {
                }
                else {
                    if (index - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(index - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[index].cells[0].all[1].click();
                    return;
                }
            }
        }
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            index = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model) {
                    if (dgData.rows[iRow].cells[6].innerText == dgData.rows[iRow].cells[7].innerText) {
                        continue;
                    }
                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
    }

    function GetDNByModelFull() {

        var Model = document.getElementById("<%=HMode.ClientID%>").value;
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (index <= dgData.rows.length - 1) {
            if (dgData.rows[index].cells[2].innerText == Model) {
                if (dgData.rows[index].cells[6].innerText == dgData.rows[index].cells[7].innerText) {
                }
                else {
                    if (index - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(index - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                    dgData.rows[index].cells[0].all[1].click();
                    return;
                }
            }
        }
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            index = iRow;
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model) {
                    if (dgData.rows[iRow].cells[6].innerText == dgData.rows[iRow].cells[7].innerText) {
                        continue;
                    }
                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
        document.getElementById("<%=HMode.ClientID%>").value = "getMode";
        CheckFirst();
    }

   
    function setSelectVal(span, idAndModeAndPono) {
        theRadio = span;
        oState = theRadio.checked;
        var id = "";
        var model = "";
        var pono = "";
        var idLen = idAndModeAndPono.indexOf("@#@#@");
        var ponoLen = idAndModeAndPono.indexOf("&!&!&");
        id = idAndModeAndPono.substring(0, idLen);
        model = idAndModeAndPono.substring(idLen + 5, ponoLen);
        pono = idAndModeAndPono.substring(ponoLen + 5);
        if (oState) {
            if (document.getElementById("<%=HSN.ClientID%>").value == "" &&
                id != "------------") {
                if (document.getElementById("<%=HClear.ClientID%>").value == "0") {
                    document.getElementById("<%=HClear.ClientID%>").value = "1"
                }
                else {
                    ResetWindow();
                }
            }
            document.getElementById("<%=HDN.ClientID%>").value = id;
            
            if (document.getElementById("<%=HMode.ClientID%>").value != "getMode" ) {
                if (document.getElementById("<%=HMode.ClientID%>").value == "first")
                { }
                else {
                    ResetLast();
                    document.getElementById("<%=txtCOA.ClientID%>").value = "";
                }
            }
            if (lastId != "") {
                var temp = document.getElementById(lastId);
                if (null != temp) {
                    temp.checked = false;
                }
            }
            if (document.getElementById("<%=HDN.ClientID%>").value != "------------") {
                document.getElementById("<%=HMode.ClientID%>").value = model;
                document.getElementById("<%=HPONO.ClientID%>").value = pono;
            }
            lastId = theRadio.id;
            theRadio.checked = true;
        }

        endWaitingCoverDiv();
        callNextInput();
        if (document.getElementById("<%=HFull.ClientID%>").value == "1") {
            document.getElementById("<%=btnFocus.ClientID%>").click();
            document.getElementById("<%=HFull.ClientID%>").value = "0";
        }
        else {
            if (document.getElementById("<%=chkIsBT.ClientID%>").checked == true) {
                document.getElementById("<%=btnFocus.ClientID%>").click();
             }
        }
    }
    function DisplsyMsg(src) {
        ShowMessage(src);
        ShowInfo(src);
    }
    function input(data) {
        ShowInfo("");
        successTemp = "";
        document.getElementById("<%=HInfo.ClientID%>").value = "";
        inputTextBox1 = data.trim();
        inputTextBox1 = inputTextBox1.toUpperCase();
        if (document.getElementById("<%=HSN.ClientID%>").value == "") {
            ResetWindow();
        }
        if (inputTextBox1 == "7777") {
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=HDN.ClientID%>").value = "";
            document.getElementById("<%=HPONO.ClientID%>").value = "";
            document.getElementById("<%=HISWIN8.ClientID%>").value = "";
            document.getElementById("<%=HFACTORYPO.ClientID%>").value = "";
            getPdLineCmbObj().selectedIndex = 0;
            ModeChange = "";
            ResetWindow();
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFreshReset.ClientID%>").click();
            return;
        }
        var line = getPdLineCmbValue();
        if (line == "") {
            alert(msgInputPDLine);
            ResetValue();
            ShowInfo(msgInputPDLine);
            return;
        }
        var printItemlist = getPrintItemCollection();
        if (printItemlist == null || printItemlist == "") {
            alert(msgPrintSettingPara);
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=HDN.ClientID%>").value = "";
            getPdLineCmbObj().selectedIndex = 0;
            ModeChange = "";
            callNextInput();
            ResetWindow();
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFreshReset.ClientID%>").click();
            return;
        }
        document.getElementById("<%=HLine.ClientID%>").value = line;

        if (document.getElementById("<%=HSN.ClientID%>").value == "") {
            ResetLast();
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
                    ResetValue();
                    DisplsyMsg("Wrong Code!");
                    return;
                }
            }
            else {
                ResetValue();
                DisplsyMsg("Wrong Code!");
                return;
            }
            document.getElementById("<%=HSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=txtCustomerSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=HISWIN8.ClientID%>").value = "";
            document.getElementById("<%=HFACTORYPO.ClientID%>").value = "";
            document.getElementById("<%=HProduct.ClientID%>").value = "";
            document.getElementById("<%=btnGetProduct.ClientID%>").click();
            return;
        }
        if (document.getElementById("<%=HCoaSN.ClientID%>").value == ""
        && document.getElementById("<%=HPartNO.ClientID%>").value != "") {
            document.getElementById("<%=HCoaSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=txtCOA.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=btnCheckInput.ClientID%>").click();
            return;
        }
    }
    function getModel(model) {
        document.getElementById("<%=txtModel.ClientID%>").value = model;
    }
    function getProductID(prod) {
        document.getElementById("<%=txtProductID.ClientID%>").value = prod;
    }
    function getIsBT(isBT) {
        document.getElementById("<%=txtModel.ClientID%>").value = document.getElementById("<%=HTxtModel.ClientID%>").value;
        document.getElementById("<%=txtProductID.ClientID%>").value = document.getElementById("<%=HTxtProductID.ClientID%>").value;
        document.getElementById("<%=HTxtModel.ClientID%>").value = "";
        document.getElementById("<%=HTxtProductID.ClientID%>").value = "";
        if (isBT == "false" || isBT == "False") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        }
        if (isBT == "true" || isBT == "True") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = true;
            showTemp = "BT product";
        }
        beginWaitingCoverDiv();
        document.getElementById("<%=HIsBT.ClientID%>").value = isBT;
        if (document.getElementById("<%=HISWIN8.ClientID%>").value == "true") {
            showTemp = msgWin8;
        }
        else {
            if (isBT == "true" || isBT == "True") {
                showTemp = "BT product";
            }
            else {
                showTemp = msgNomal;
            }
        }
        ShowInfo(showTemp);
        endWaitingCoverDiv();
        if (isBT == "false" || isBT == "False") {
            CheckMode();
        }
        else {
            document.getElementById("<%=HMode.ClientID%>").value = document.getElementById("<%=txtModel.ClientID%>").value;
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function getIsCDSI(isBT) {
        document.getElementById("<%=txtModel.ClientID%>").value = document.getElementById("<%=HTxtModel.ClientID%>").value;
        document.getElementById("<%=txtProductID.ClientID%>").value = document.getElementById("<%=HTxtProductID.ClientID%>").value;
        document.getElementById("<%=HTxtModel.ClientID%>").value = "";
        document.getElementById("<%=HTxtProductID.ClientID%>").value = "";
        if (isBT == "false" || isBT == "False") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        }
        if (isBT == "true" || isBT == "True") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = true;
        }
        beginWaitingCoverDiv();
        document.getElementById("<%=HIsBT.ClientID%>").value = isBT;
        if (document.getElementById("<%=HISWIN8.ClientID%>").value == "true") {
            showTemp = msgCDSI + "\n" + msgWin8;
        }
        else {
            showTemp = msgCDSI;
        }
        ShowInfo(showTemp);
        
        endWaitingCoverDiv();
        
        if (isBT == "false" || isBT == "False") {
            var FNO = document.getElementById("<%=HFACTORYPO.ClientID%>").value;
            CheckModeAndFNO(FNO);
        }
        else {
            document.getElementById("<%=HMode.ClientID%>").value = document.getElementById("<%=txtModel.ClientID%>").value;
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function CheckModeAndFNO(FNO) {
        var model = document.getElementById("<%=txtModel.ClientID%>").value;
        var temp = document.getElementById("<%=HDN.ClientID%>").value;
        if (document.getElementById("<%=HDN.ClientID%>").value == "------------") {
            if (document.getElementById("<%=HDNTemp.ClientID%>").value != "") {
                document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                GetDNByDNOnPage(document.getElementById("<%=HDNTemp.ClientID%>").value);
                document.getElementById("<%=HMode.ClientID%>").value = model;
            }
            else {
                getResetAll();
                ResetWindow();
                var tmp = msgNoCDSI;
                tmp = tmp.replace("%1", FNO);
                DisplsyMsg(tmp);
                callNextInput();
                return;
            }
        }
        var msgFNO = "";
        if (document.getElementById("<%=HDN.ClientID%>").value != document.getElementById("<%=HDNTemp.ClientID%>").value) {
            if (document.getElementById("<%=HMode.ClientID%>").value == model
            && document.getElementById("<%=HPONO.ClientID%>").value != FNO) {
                msgFNO = "FNO";
            }
            else if (document.getElementById("<%=HMode.ClientID%>").value != model
            && document.getElementById("<%=HPONO.ClientID%>").value == FNO) {
                msgFNO = "mode";
            }
            if (document.getElementById("<%=HDNTemp.ClientID%>").value != "") {
                document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                GetDNByDNOnPage(document.getElementById("<%=HDNTemp.ClientID%>").value);
                document.getElementById("<%=HMode.ClientID%>").value = model;
                if (msgFNO == "FNO") {
                    ModeChange = "FNO";
                }
                else if (msgFNO == "mode") {
                    ModeChange = "true";
                }
                else {
                    ModeChange = "DNChange";
                }
                document.getElementById("<%=btnCheckCOA.ClientID%>").click();
            }
            else {
                getResetAll();
                ResetWindow();
                var tmp = msgNoCDSI;
                tmp = tmp.replace("%1", FNO);
                DisplsyMsg(tmp);
                callNextInput();
            }
        }
        else {
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function GetDNByModelAndFNO(Model, FNO) {
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (index <= dgData.rows.length - 1) {
            if (dgData.rows[index].cells[2].innerText == Model
            && dgData.rows[index].cells[4].innerText == FNO
            ) {
                if (dgData.rows[index].cells[6].innerText == dgData.rows[index].cells[7].innerText) {
                }
                else {
                    if (index - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(index - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[index].cells[0].all[1].click();
                    return;
                }
            }
        }
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            index = iRow;
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model
                && dgData.rows[index].cells[4].innerText == FNO) {
                    if (dgData.rows[iRow].cells[6].innerText == dgData.rows[iRow].cells[7].innerText) {
                        continue;
                    }
                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
    }
    function CheckMode() {
        var model = document.getElementById("<%=txtModel.ClientID%>").value;
        var temp = document.getElementById("<%=HDN.ClientID%>").value;
        if (document.getElementById("<%=HDN.ClientID%>").value == "------------") {
            if (document.getElementById("<%=HDNTemp.ClientID%>").value != "") {
                document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                GetDNByDNOnPage(document.getElementById("<%=HDNTemp.ClientID%>").value);
                document.getElementById("<%=HMode.ClientID%>").value = model;
            }
            else {
                getResetAll();
                ResetWindow();
                var modeDis = msgModelNoFind + "\n Model:" + model;
                DisplsyMsg(modeDis);
                callNextInput();
                return;
            }
        }
        if (document.getElementById("<%=HDN.ClientID%>").value != document.getElementById("<%=HDNTemp.ClientID%>").value) {
            if (document.getElementById("<%=HDNTemp.ClientID%>").value != "") {
                if (document.getElementById("<%=HMode.ClientID%>").value != model) {
                    ModeChange = "true";
                }
                else {
                    ModeChange = "DNChange";
                }
                document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                GetDNByDNOnPage(document.getElementById("<%=HDNTemp.ClientID%>").value);
                document.getElementById("<%=HMode.ClientID%>").value = model;
                document.getElementById("<%=btnCheckCOA.ClientID%>").click();
            }
            else {
                getResetAll();
                ResetWindow();
                var modeDis = msgModelNoFind + "\n Model:" + model;
                DisplsyMsg(modeDis);
                callNextInput();
            }
        }
        else {
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function getIfCheckCOAFocus() {
        ResetCoa();
    }
    function getNoCheckCOAFocus() {
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = msgNotNeedCoa;
    }
    function getNeedCheckCOAFocus(coa) {
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = coa;
    }
    function getSuccess() {
        ResetValue();
        getResetAll();
        ShowInfo("Success");
    }
    function getSuccessFresh() {
        if (successTemp != "") {
            if (document.getElementById("<%=txtCOA.ClientID%>").value == "") {
                successTemp = successTemp + "\n" + msgNotNeedCoa;
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true, successTemp);
            }
        }
        else {
            ShowSuccessfulInfo(true, msgNotNeedCoa);
        }
    }
    function getIfCheckCOAShow(partNO) {
        document.getElementById("<%=HPartNO.ClientID%>").value = partNO;
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = partNO;
        if (ModeChange == "true") {
            ShowInfo(msgModeChange + msgCoaInput + partNO);
            ModeChange = "";
        }
        else if (ModeChange == "DNChange") 
        {
            ShowInfo(msgDNChange + msgCoaInput + partNO);
            ModeChange = "";
        }
        else if (ModeChange == "FNO") {
            ShowInfo(msgFNOWrong + "\n" + msgCoaInput + partNO);
            ModeChange = "";
         }
        else {
            ShowInfo(msgCoaInput + partNO);
        }
    }
    function getCheckCOAShow()
    {
        document.getElementById("<%=HPartNO.ClientID%>").value = "Check@Coa";
        if (ModeChange == "true") {
            ShowInfo(msgModeChange + msgCoaCheck);
            ModeChange = "";
        }
        else if (ModeChange == "DNChange") {
            ShowInfo(msgDNChange + msgCoaCheck);
            ModeChange = "";
        }
        else if (ModeChange == "FNO") {
            ShowInfo(msgFNOWrong + "\n" + msgCoaCheck);
            ModeChange = "";
        }
        else {
            ShowInfo(msgCoaCheck);
        }
    }
    function getProductFail(productWrong) {
        ResetLast();
        ShowInfo(productWrong);
    }
    function getDNFail() {
        ResetLast();
        DisplsyMsg("No DN data, operation pause !");
    }
    function callNextInput() {
        beginWaitingCoverDiv();
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("input");
        endWaitingCoverDiv();
    }
    function ResetValue() {
        document.getElementById("<%=HSN.ClientID%>").value = "";
        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
        document.getElementById("<%=HPartNO.ClientID%>").value = "";
        document.getElementById("<%=HIsBT.ClientID%>").value = "";
        ShowInfo("");
        callNextInput();
    }
    function ResetLast() {
        document.getElementById("<%=HSN.ClientID%>").value = "";
        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
        document.getElementById("<%=HPartNO.ClientID%>").value = "";
        document.getElementById("<%=HIsBT.ClientID%>").value = "";
        
        document.getElementById("<%=txtModel.ClientID%>").value = "";
        document.getElementById("<%=txtProductID.ClientID%>").value = "";
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = "";
        document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
        document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        ShowInfo("");
        callNextInput();
    }
    
    function ResetCoa() {
        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
        document.getElementById("<%=txtCOA.ClientID%>").value = "";
        ShowInfo("");
        callNextInput();
    }
    function PrintPizzaAndCOO(changeDN) {
        if (changeDN != "") {
            document.getElementById("<%=HDN.ClientID%>").value = changeDN;
        }
        PrintPizzaFinal(changeDN);
    }
    function btnPrintSetting_onclick() {
        showPrintSetting(station, pCode);
    }
    function PrintCOO() {
        beginWaitingCoverDiv();
        try {
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            CombineCOAandDNWebService.PrintCOOLabel(document.getElementById("<%=HSN.ClientID%>").value, editor, station, customer, printItemlist, printSuccCOO, printFailCOO);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }

    function printSuccCOO(result) {
        var labelCollection = [];
        for (var i in result[1]) {
            if (result[1][i].LabelType == "BT COO Label") {
                labelCollection.push(result[1][i]);
                setPrintItemListParamCOO(labelCollection, result[0]);
                printLabels(labelCollection, false);
                break;
            }
        }
        endWaitingCoverDiv();
        getResetFreshSuccess();
    }

    function setPrintItemListParamCOO(backPrintItemList, sn) {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(sn);
        setPrintParam(lstPrtItem, "BT COO Label", keyCollection, valueCollection);
    } 
    function PrintPizza() {
        beginWaitingCoverDiv();
        try {
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            CombineCOAandDNWebService.PrintPizzaLabel(document.getElementById("<%=HSN.ClientID%>").value, editor, station, customer, printItemlist, printSucc, printFail);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }

    function PrintPizzaFinal(changeDN) {
        beginWaitingCoverDiv();
        try {
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            var temp = "";
            if (changeDN != "") {
                document.getElementById("<%=HDN.ClientID%>").value = changeDN;
                temp = document.getElementById("<%=HSN.ClientID%>").value + "#@#" + changeDN;
            }
            else
            {
                temp = document.getElementById("<%=HSN.ClientID%>").value + "#@#" + document.getElementById("<%=HDN.ClientID%>").value;
            }
            CombineCOAandDNWebService.PrintPizzaLabelFinal(temp, editor, station, customer, printItemlist, printSucc, printFailFinal);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }
    
    function PrintPizzaJapan() {
        try {
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            CombineCOAandDNWebService.PrintPizzaLabel(document.getElementById("<%=HSN.ClientID%>").value, editor, station, customer, printItemlist, printSuccJapan, printFail);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }
    function printSuccJapan(result) {
        var labelCollection = [];
        for (var i in result[1]) {
            if (result[1][i].LabelType == "PIZZA Label-2") {
                labelCollection.push(result[1][i]);
                setPrintItemListParam2(labelCollection, result[0]);
                printLabels(labelCollection, false);
                break;
            }  
        }
        endWaitingCoverDiv();
        if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
            document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
            PrintCOO();
        }
        else {
            getResetFreshSuccess();
        }
        
    }
    function printSucc(result)
    {
        var labelCollection = [];
        if (result[0] == "failedSession") {
            endWaitingCoverDiv();
            getFailed();
            return;
        }
        for (var i in result[1]) {
            if (result[1][i].LabelType == "PIZZA Label-1") {
                labelCollection.push(result[1][i]);
                setPrintItemListParam(labelCollection, result[0]);
                printLabels(labelCollection, false);
                break;
            }
        }
        endWaitingCoverDiv();
        var temp = document.getElementById("<%=HMode.ClientID%>").value.trim();
        if (temp.length > 11) {
            var japan = temp.substr(9, 2);
            if (japan == "29" || japan == "39") {
                beginWaitingCoverDiv();
                PrintPizzaJapan();
                return;
            }
        } 
        if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
            document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
            PrintCOO();
        }
        else {
            getResetFreshSuccess();
        }
    }
    function setPrintItemListParam2(backPrintItemList, sn) {

        var lable = "PIZZA Label-2";
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(sn);
        setPrintParam(lstPrtItem, lable, keyCollection, valueCollection);
     }
    function setPrintItemListParam(backPrintItemList, sn)
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        var lable = "PIZZA Label-1";
        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(sn);
        setPrintParam(lstPrtItem, lable, keyCollection, valueCollection);
    } 
        
        function printFail(result)
        {
            endWaitingCoverDiv();
            if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
                document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
                PrintCOO();
            }
            else {
                ResetValue();
                getResetAll();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
            }    
        }
        function printFailFinal(result) {
            endWaitingCoverDiv();
            document.getElementById("<%=btnCancel.ClientID%>").click();
            if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
                document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
                PrintCOO();
            }
            else {
                ResetValue();
                getResetAll();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
            }
        }
        function printFailCOO(result) {
            endWaitingCoverDiv();
            ResetValue();
            getResetAll();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }
        function btnRePrint_onclick() {
            var paramArray = new Array();
            paramArray[0] = station;
            paramArray[1] = editor;
            paramArray[2] = customer;
            paramArray[3] = pCode;
            var url = "CombineCOAandDNReprint.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login + "&COO=" + "true";
            window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
        function getReset(msg) {
            endWaitingCoverDiv();
            beginWaitingCoverDiv();
            ShowInfo(msg);
            callNextInput();
            endWaitingCoverDiv();
        }
        function getCoaECPN(msg) {
            document.getElementById("<%=txtCOAIEC.ClientID%>").value = msg;
        }
        function getResetAll() {
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            ModeChange = "";
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFresh.ClientID%>").click();
        }
        function getResetFreshSuccess() {
            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            successTemp = "";
            if (temp != "") {
                successTemp = "[" + temp + "] " + msgSuccess;
            }
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            ModeChange = "";
            FirstRadioToCheckSuccess();
        }
        function getFailed() {
            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            successTemp = "";
            if (temp != "") {
                successTemp = "[" + temp + "] Failed!" ;
            }
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            ModeChange = "";
            FirstRadioToCheckFailed();
        }
        function ResetWindow() {
            document.getElementById("<%=txtModel.ClientID%>").value = "";
            document.getElementById("<%=txtProductID.ClientID%>").value = "";
            document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
            document.getElementById("<%=txtCOAIEC.ClientID%>").value = "";
            document.getElementById("<%=txtCOA.ClientID%>").value = "";
            document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        }
        
        function getResetMode(mode) {
            document.getElementById("<%=HMode.ClientID%>").value = mode;
        }
        function getDisplay(display) {
            ShowInfo(display);
        }
        function StartSucc(result) {
            if (document.getElementById("<%=HSN.ClientID%>").value == "") {
                ResetWindow();
            }
            if (inputTextBox1 == "7777") {
                document.getElementById("<%=HSN.ClientID%>").value = "";
                document.getElementById("<%=HCoaSN.ClientID%>").value = "";
                document.getElementById("<%=HPartNO.ClientID%>").value = "";
                document.getElementById("<%=HIsBT.ClientID%>").value = "";
                document.getElementById("<%=HLine.ClientID%>").value = "";
                document.getElementById("<%=HMode.ClientID%>").value = "first";
                document.getElementById("<%=HDN.ClientID%>").value = "";
                document.getElementById("<%=HPONO.ClientID%>").value = "";
                document.getElementById("<%=HISWIN8.ClientID%>").value = "";
                document.getElementById("<%=HFACTORYPO.ClientID%>").value = "";
                getPdLineCmbObj().selectedIndex = 0;
                ModeChange = "";
                ResetWindow();
                beginWaitingCoverDiv();
                document.getElementById("<%=btnGridFreshReset.ClientID%>").click();
                return;
            }
            var line = getPdLineCmbValue();
            if (line == "") {
                alert(msgInputPDLine);
                ResetValue();
                ShowInfo(msgInputPDLine);
                return;
            }
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                document.getElementById("<%=HSN.ClientID%>").value = "";
                document.getElementById("<%=HCoaSN.ClientID%>").value = "";
                document.getElementById("<%=HPartNO.ClientID%>").value = "";
                document.getElementById("<%=HIsBT.ClientID%>").value = "";
                document.getElementById("<%=HLine.ClientID%>").value = "";
                document.getElementById("<%=HMode.ClientID%>").value = "first";
                document.getElementById("<%=HDN.ClientID%>").value = "";
                getPdLineCmbObj().selectedIndex = 0;
                ModeChange = "";
                callNextInput();
                ResetWindow();
                beginWaitingCoverDiv();
                document.getElementById("<%=btnGridFreshReset.ClientID%>").click();
                return;
            }
            document.getElementById("<%=HLine.ClientID%>").value = line;

            if (document.getElementById("<%=HSN.ClientID%>").value == "") {
                ResetLast();
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
                        ResetValue();
                        DisplsyMsg("Wrong Code!");
                        return;
                    }
                }
                else {
                    ResetValue();
                    DisplsyMsg("Wrong Code!");
                    return;
                }
                document.getElementById("<%=HSN.ClientID%>").value = inputTextBox1;
                document.getElementById("<%=txtCustomerSN.ClientID%>").value = inputTextBox1;
                document.getElementById("<%=HISWIN8.ClientID%>").value = "";
                document.getElementById("<%=HFACTORYPO.ClientID%>").value = "";
                document.getElementById("<%=HProduct.ClientID%>").value = "";
                document.getElementById("<%=btnGetProduct.ClientID%>").click();
                return;
            }
            if (document.getElementById("<%=HCoaSN.ClientID%>").value == ""
        && document.getElementById("<%=HPartNO.ClientID%>").value != "") {
                document.getElementById("<%=HCoaSN.ClientID%>").value = inputTextBox1;
                document.getElementById("<%=txtCOA.ClientID%>").value = inputTextBox1;
                document.getElementById("<%=btnCheckInput.ClientID%>").click();
                return;
            }
        }
        function StartFailed(result) {
            ResetValue();
            getResetAll();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }
        function GetProduct() {
            return true;
        } 
    </script>
 <div>
   <center >
   <table width="100%" border="0" >
    <tr>
        <td align="left" >
            <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="80" IsPercentage="true" />
        </td>
    </tr>
    
    <tr>
        <td colspan="4" align="left">
        <fieldset>
            <legend><asp:Label ID="lbTableTitle" CssClass="iMes_label_13pt" runat="server"></asp:Label></legend>
            <asp:UpdatePanel runat="server" ID="UpdatePanelTable" UpdateMode="Conditional">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                        GvExtWidth="99%" GvExtHeight="192px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                        Width="97%" Height="187px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                        HighLightRowPosition="3" HorizontalAlign="Left" onrowdatabound="GridView1_RowDataBound"> 
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
            <legend>
             <asp:Label ID="lbProductInfo" runat="server"></asp:Label></legend>
             <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lbCustomerSN"  Width = "13%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtCustomerSN" runat="server"  style="width:32%"     CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True"/>
                    <asp:Label ID="lbProductID"  Width = "13%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtProductID" runat="server"  style="width:32%"     CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True"/>
                </td>
             </tr>
           <tr>
                <td>
                    <asp:Label ID="lbModel"  Width = "13%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtModel" runat="server"  style="width:32%"    CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="chkIsBT" runat="server" CssClass="iMes_label_13pt"  />
                </td>
           </tr>
           </table>
         </fieldset>
        </td>
    </tr>
    <tr>
        <td align="left" >
        <fieldset style="width: auto">
            <legend><asp:Label ID="lbCheckItem" runat="server"></asp:Label></legend>
            <asp:Label ID="lbCOA"  Width = "6%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <asp:TextBox ID="txtCOA" runat="server" style="width:26% ;line-height:26px "     
                            IsClear="true" ReadOnly="True" Font-Bold="True" 
                Font-Names="Verdana" Font-Size="X-Large" Height="28px"  />
         &nbsp;<asp:TextBox ID="txtCOAIEC" runat="server" style="width:58% ;line-height:30px "     
                            IsClear="true" ReadOnly="True" Font-Bold="True" 
                Font-Names="Verdana" Font-Size="XX-Large" Height="32px" 
                ForeColor="#CC3300"  />
         </fieldset>
        </td>
    </tr>
    
    <tr>
        <td align="left" colspan="1">
            <asp:Label ID="lbDataEntry" Width = "13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
            <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="45%" IsClear="true" IsPaste="true"  />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input id="btnPrintSetting" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'"    onclick="return btnPrintSetting_onclick()" />
            <input id="btnRePrint" type="button"  runat="server" 
                class="iMes_button"  
                onmouseover="this.className='iMes_button_onmouseover'"  
                onmouseout="this.className='iMes_button_onmouseout'"  onclick="return btnRePrint_onclick()" /></td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server"  RenderMode="Inline" UpdateMode="Conditional">
	            <ContentTemplate>
	              <button id="btnGridFresh" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshFull" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshSuccess" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshReset" runat="server" type="button" style="display: none" />
	              <button id="btnGetProduct" runat="server" type="button" onclick="if(GetProduct())" onserverclick="btnGetProduct_ServerClick" style="display: none" />
	              <button id="btnCheckCOA" runat="server" type="button" style="display: none" />
	              <button id="btnCheckInput" runat="server" type="button" style="display: none" />
	              <button id="btnGetModel" runat="server" type="button" style="display: none" />
	              <button id="btnDisplay" runat="server" type="button" style="display: none" />
	              <button id="btnFocus" runat="server" type="button" style="display: none" />
	              <button id="btnCancel" runat="server" type="button" style="display: none" />
	              <input type="hidden" runat="server" id="HSN" /> 
	              <input type="hidden" runat="server" id="HIsBT" />  
	              <input type="hidden" runat="server" id="HDN" />  
	              <input type="hidden" runat="server" id="HPartNO" />
	              <input type="hidden" runat="server" id="HCoaSN" />
	              <input type="hidden" runat="server" id="HLine" />
	              <input type="hidden" runat="server" id="HMode" /> 
	              <input type="hidden" runat="server" id="HDisplay" />
	              <input type="hidden" runat="server" id="HClear" />
	              <input type="hidden" runat="server" id="HFull" />
	              <input type="hidden" runat="server" id="HInfo" />
	              <input type="hidden" runat="server" id="HTxtModel" />
	              <input type="hidden" runat="server" id="HTxtProductID" />
	              <input type="hidden" runat="server" id="HFACTORYPO" />
	              <input type="hidden" runat="server" id="HPONO" />
	              <input type="hidden" runat="server" id="HISWIN8" />
	              <input type="hidden" runat="server" id="HDNTemp" />
	              <input type="hidden" runat="server" id="HProduct" />
	            </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
    </center>
</div>

</asp:Content>