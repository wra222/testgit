<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:COAStatusChange
 * CI-MES12-SPEC-PAK-COAStatusChange.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="COAStatusChangeForBuyer.aspx.cs" Inherits="PAK_COAStatusChangeForBuyer" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">
        <Services>  
			<asp:ServiceReference Path="Service/WebServiceCoaChange.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript" language="javascript"  >
        var statusWrong = "";
        var editor = '<%=userId%>';
        var customer = '<%=customer%>';
        var station = '<%=station%>';
        var outStatus = "";
        var inStatus = "";
        var currentStatus = "";
        var table;
        var indexDN = 1;
        var initRowsCount = 6;
        var strBegNO = "";
        var strEndNO = "";
        var earlyNO = "";
        var numBegNO = 0;
        var numEndNO = 0;
        var txtTable;
        var msgCOA_InputFormat = '<%=this.GetLocalResourceObject(Pre + "_msgCOA_InputFormat").ToString() %>';
        var cn_txtTable = new Array(new Array("RE", "01,16", "???Coa Center ?????")
    );
        var en_txtTable = new Array(new Array("RE", "01,16", "???Coa Center ?????")

    );
        var msgErrorSameStatus = '<%=this.GetLocalResourceObject(Pre + "_msgErrorSameStatus").ToString() %>';
        var msgWrongStatus = '<%=this.GetLocalResourceObject(Pre + "_msgWrongStatus").ToString() %>';
        var msgWrongBuyer = '<%=this.GetLocalResourceObject(Pre + "_msgWrongBuyer").ToString() %>';
        var msgA1 = '<%=this.GetLocalResourceObject(Pre + "_msgA1").ToString() %>';
        var msg11 = '<%=this.GetLocalResourceObject(Pre + "_msg11").ToString() %>';
        var msgSame = '<%=this.GetLocalResourceObject(Pre + "_msgSame").ToString() %>';
        var msgSelectTaget = '<%=this.GetLocalResourceObject(Pre + "_msgSelectTaget").ToString() %>';
        var msgInputTxt = '<%=this.GetLocalResourceObject(Pre + "_msgInputTxt").ToString() %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';


        var msgSaveWrong = '<%=this.GetLocalResourceObject(Pre + "_msgSaveWrong").ToString() %>';
        var msgInvalidFileType = '<%=msgInvalidFileType %>';
        var msgEmptyFile = '<%=msgEmptyFile %>';
        var coaLists;
        var tempCoa = "";
        var msgUpload1 = '<%=this.GetLocalResourceObject(Pre + "_msgUpload1").ToString() %>';
        var msgUpload2 = '<%=this.GetLocalResourceObject(Pre + "_msgUpload2").ToString() %>';
        window.onload = function() {
            var language = '<%=Pre%>';
            if (language == "cn") {
                txtTable = cn_txtTable;
            }
            else {
                txtTable = en_txtTable;
            }
            table = document.getElementById("<%=gridview.ClientID %>");
            document.getElementById("<%=TextBox1.ClientID%>").focus();
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
        }
        function IsNumber(src) {
            // var regNum = /^\d$/;
            //var regNum = /^(0|[1-9]\d*)$/;
            //var regNum = /^\d*/;
            //var regNum = /^\\d+$/;
            var regNum = /^[0-9]+$/;
            // var aa = parseInt(src,10);
            var aa = regNum.test(src);
            return regNum.test(src);
            //return aa;
        }
        function DisplsyMsg(src) {
            ShowMessage(src);
            ShowInfo(src);
        }
        function onTextBox1KeyDown() {
            strBegNO = "";
            ShowInfo("");
            if (event.keyCode == 9 || event.keyCode == 13) {
                var inputTextBox1 = document.getElementById("<%=TextBox1.ClientID%>").value.trim();
                inputTextBox1 = inputTextBox1.toUpperCase();
                if (inputTextBox1.length < 6 || inputTextBox1.trim() == ""
            || !IsNumber(inputTextBox1.substr(inputTextBox1.length - 6))) {
                    DisplsyMsg(msgCOA_InputFormat);
                    document.getElementById("<%=TextBox1.ClientID%>").value = "";
                    document.getElementById("<%=TextBox2.ClientID%>").value = "";
                    return;
                }
                strBegNO = inputTextBox1;
                numBegNO = parseInt(inputTextBox1.substr(inputTextBox1.length - 6), 10);
                strEndNO = "";
                numEndNO = 0;
                document.getElementById("<%=TextBox2.ClientID%>").value = "";
                document.getElementById("<%=TextBox2.ClientID%>").focus();
            }
        }
        function GetTextData() {

            ShowInfo("");
            var inputTextBox1 = document.getElementById("<%=TextBox1.ClientID%>").value.trim();

            if (inputTextBox1.length < 6 || inputTextBox1.trim() == ""
            || !IsNumber(inputTextBox1.substr(inputTextBox1.length - 6))) {
                DisplsyMsg(msgCOA_InputFormat);
                document.getElementById("<%=TextBox1.ClientID%>").value = "";
                document.getElementById("<%=TextBox2.ClientID%>").value = "";
                document.getElementById("<%=TextBox1.ClientID%>").focus();
                return false;
            } else {
                strBegNO = inputTextBox1;
                numBegNO = parseInt(inputTextBox1.substr(inputTextBox1.length - 6), 10);
            }
            var inputTextBox2 = document.getElementById("<%=TextBox2.ClientID%>").value.trim();
            inputTextBox2 = inputTextBox2.toUpperCase();
            if (inputTextBox2.length < 6 || inputTextBox2.trim() == ""
            || !IsNumber(inputTextBox2.substr(inputTextBox2.length - 6))) {
                DisplsyMsg(msgCOA_InputFormat);
                document.getElementById("<%=TextBox1.ClientID%>").value = "";
                document.getElementById("<%=TextBox2.ClientID%>").value = "";
                return false;
            }
            strEndNO = inputTextBox2;
            numEndNO = parseInt(inputTextBox2.substr(inputTextBox2.length - 6), 10);
            var rangeToDisplay = "";
            if (numEndNO - numBegNO < 0) {
                var numTemp = numEndNO;
                numEndNO = numBegNO;
                numBegNO = numTemp;
                var strTemp = strEndNO;
                strEndNO = strBegNO;
                strBegNO = strTemp;
            }
            var rangeToDisplay = "Range:" + strBegNO + "~" + strEndNO;
            ShowInfo(rangeToDisplay);

        }
        function onTextBox2KeyDown() {
            strEndNO = "";
            ShowInfo("");
            if (event.keyCode == 9 || event.keyCode == 13) {
                //document.getElementById("<%=drpCOACardChange.ClientID%>").selectedIndex = 0;
                if (false == GetTextData()) {
                    return;
                }
                OnClick();
            }
        }
        function StatusCheck() {
            statusWrong = "";
            var ret = false;

            var getReturn = false;
            for (var i = 0; i < txtTable.length; i++) {
                if (getReturn == true) {
                    break;
                }
                var getIn = false;
                for (var j = 0; j < txtTable[i].length; j++) {
                    if (currentStatus.indexOf(txtTable[i][j]) != -1
                && j == 0) {
                        getIn = true;
                        continue;
                    }
                    if (j == 1
                    && getIn == true) {
                        if (txtTable[i][j].indexOf(inStatus.substr(0, 2)) != -1) {
                            ret = true;
                            continue;
                        }
                    }
                    if (j == 2
                && getIn == true
                && ret == false) {
                        //ShowInfo(txtTable[i][j]);
                        statusWrong = txtTable[i][j];
                        getReturn = true;
                        break;
                    }
                }
            }
            return ret;
        }
        function drpOnChange() {
            var obj = document.getElementById("<%=drpCOACardChange.ClientID%>");
            outStatus = obj.value;
            var id = obj.selectedIndex;
            //inStatus = obj[id].innerText;
            inStatus = obj.value;
        }

        function AddRowInfoForDN(RowArray) {
            try {
                if (indexDN <= initRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + table.id + "(RowArray,false, indexDN)");
                } else {
                    eval("AddCvExtRowToBottom_" + table.id + "(RowArray,false)");
                }
                setSrollByIndex(1, false);
                indexDN++;
                var i = indexDN - 1;
                for (; i > 1; i--) {
                    table.moveRow(i, i - 1);
                }

            } catch (e) {
                ShowInfo(e.description);
            }
        }
        function AddRowInfoForLot(RowArray) {
            try {
                if (indexDN <= initRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + table.id + "(RowArray,false, indexDN)");
                } else {
                    eval("AddCvExtRowToBottom_" + table.id + "(RowArray,false)");
                }
                indexDN++;

            } catch (e) {
                ShowInfo(e.description);
            }
        }
        function getQueryPno(pno) {
            document.getElementById("<%=lbPNValue.ClientID%>").innerText = pno;
            var rangeToDisplay = strBegNO + "~" + strEndNO;
            document.getElementById("<%=lbRangeValue.ClientID%>").innerText = rangeToDisplay;
            rangeToDisplay = "Range:" + rangeToDisplay;
            ShowInfo(rangeToDisplay);
            document.getElementById("<%=lbCountValue.ClientID%>").innerText = numEndNO - numBegNO + 1;
        }
        function getQueryPlace(place) {
            document.getElementById("<%=lbPlaceValue.ClientID%>").innerText = place;
        }
        function getQueryStatus(status) {
            currentStatus = status;
            if (currentStatus == "A1") {
                ResetPage();
                DisplsyMsg(msgA1);
            }
            else if (
             currentStatus == "01"
            || currentStatus == "02"
            || currentStatus == "05"
            || currentStatus == "16"
            || currentStatus == "11") {
                ResetPage();
                DisplsyMsg(msg11);
            }
            else {
                document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            }

        }
        function getQueryStatusNoEnter(status) {
            currentStatus = status;

            if (currentStatus == "A1") {
                ResetPage();
                DisplsyMsg(msgA1);
                return;
            }
            else if (
             currentStatus == "01"
            || currentStatus == "02"
            || currentStatus == "05"
            || currentStatus == "16"
            || currentStatus == "11") {
                ResetPage();
                DisplsyMsg(msg11);
                return;
            }
            else {
                document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            }

            if (inStatus == "") {
                DisplsyMsg(msgSelectTaget);
                return;
            }
            CheckStatus();
            if (currentStatus != "") {

                document.getElementById("<%=status.ClientID%>").value = inStatus;
                document.getElementById("<%=btnUpdateCOAList.ClientID%>").click(); 
            }
        }
        function OnClick() {
            document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
            document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
            document.getElementById("<%=range.ClientID%>").value = numEndNO - numBegNO + 1;
            earlyNO = "";
            document.getElementById("<%=btnGetCOAList.ClientID%>").click();
        }

        function btnQuery_onclick() {
            ShowInfo("");
            //document.getElementById("<%=drpCOACardChange.ClientID%>").selectedIndex = 0;
            if (strBegNO != "" && strEndNO != "") {
                OnClick();
            }
            else {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
            || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                    document.getElementById("<%=TextBox1.ClientID%>").value = "";
                    document.getElementById("<%=TextBox2.ClientID%>").value = "";
                    strBegNO = "";
                    strEndNO = "";
                    ResetPage();
                    DisplsyMsg(msgInputTxt);
                    return;
                }
                if (strBegNO == "" || strEndNO == "") {
                    if (false == GetTextData()) {
                        return;
                    }
                }
                if (strBegNO == "" || strEndNO == "") {
                    ResetPage();
                    DisplsyMsg(msgInputTxt);
                    return;
                }
                OnClick();
            }


        }
        function CheckStatus() {

            if (currentStatus == inStatus
            && currentStatus != "") {
                DisplsyMsg(msgErrorSameStatus);
                ResetPage();
                return;
            }
            if (currentStatus.trim() != "RE") {
                DisplsyMsg(msgWrongBuyer);
                ResetPage();
                return;
            }
            if (false == StatusCheck()) {
                ResetPage();
                if (statusWrong != "") {
                    statusWrong = "Fail!" + statusWrong;
                    DisplsyMsg(statusWrong);
                }
                else {
                    msgWrongStatus = "Fail!" + msgWrongStatus;
                    DisplsyMsg(msgWrongStatus);
                }
            }
        }
        function checkFileSize() {
            var filePath = document.getElementById("<%=txtBrowse.ClientID %>").value;
            if (filePath == "") {
                DisplsyMsg(msgEmptyFile);
                return false;
            }
            var extend = filePath.substring(filePath.lastIndexOf(".") + 1);
            if (extend == "") {
                DisplsyMsg(msgInvalidFileType);
                return false;
            }
            else {
                if (!(extend == "txt" )) {
                    DisplsyMsg(msgInvalidFileType);
                    return false;
                }
            }
            return true;
        }
        function ResetPage() {
            document.getElementById("<%=lbRangeValue.ClientID%>").innerText = "";
            document.getElementById("<%=lbCountValue.ClientID%>").innerText = "";
            document.getElementById("<%=lbPNValue.ClientID%>").innerText = "";
            document.getElementById("<%=lbPlaceValue.ClientID%>").innerText = "";
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            currentStatus = "";
            strBegNO = "";
            strEndNO = "";
            earlyNO = "";
            numEndNO = 0;
            numBegNO = 0;
            outStatus = "";
            inStatus = "";
            document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
            document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
            document.getElementById("<%=range.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
            document.getElementById("<%=drpCOACardChange.ClientID%>").selectedIndex = 0;
        }
        function ResetPageForSave() {
            document.getElementById("<%=lbRangeValue.ClientID%>").innerText = "";
            document.getElementById("<%=lbCountValue.ClientID%>").innerText = "";
            document.getElementById("<%=lbPNValue.ClientID%>").innerText = "";
            document.getElementById("<%=lbPlaceValue.ClientID%>").innerText = "";
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            currentStatus = "";
            strBegNO = "";
            strEndNO = "";
            earlyNO = "";
            numEndNO = 0;
            numBegNO = 0;
            //outStatus = "";
            //inStatus = "";
            document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
            document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
            document.getElementById("<%=range.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
            //document.getElementById("<%=drpCOACardChange.ClientID%>").selectedIndex = 0;
        }
        function btnReceive_onclick() {
            ShowInfo("");
            if (strBegNO == "" || strEndNO == "") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
            || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                    ResetPage();
                    DisplsyMsg(msgInputTxt);
                    return;
                }
                else {
                    if (false == GetTextData()) {
                        ResetPage();
                        return;
                    }
                }
            }
            if (strBegNO == "" || strEndNO == "") {
                ResetPage();
                DisplsyMsg(msgInputTxt);
                return;
            }
            document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
            document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
            document.getElementById("<%=btnReceiveCOAList.ClientID%>").click();
        }
        function getQueryEarly(number) {
            earlyNO = number;
        }
        function btnSave_onclick() {

            ShowInfo("");
            var enter = "true";
            if (strBegNO == "" || strEndNO == "") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value != ""
            && document.getElementById("<%=TextBox2.ClientID%>").value != "") {
                    if (false == GetTextData()) {
                        return;
                    }
                    document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
                    document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
                    document.getElementById("<%=range.ClientID%>").value = numEndNO - numBegNO + 1;
                    earlyNO = "";
                    enter = "false";
                    document.getElementById("<%=btnGetCOAListNoEnter.ClientID%>").click();
                }
                else {
                    ResetPage();
                    DisplsyMsg(msgInputTxt);
                    return;
                }
            }

            if (enter == "true" && document.getElementById("<%=lbCountValue.ClientID%>").innerText == "") {
                DisplsyMsg("Please , Press Query button at fisrt!");
                return;
            }

            if (enter == "true") {
                if (inStatus == "") {
                    DisplsyMsg(msgSelectTaget);
                    return;
                }
                CheckStatus();
                if (currentStatus != "") {

                    document.getElementById("<%=status.ClientID%>").value = inStatus;
                    document.getElementById("<%=btnUpdateCOAList.ClientID%>").click();
                }
            }
        }
        function getReceive() {
            var successTemp = "";
            var temp = strBegNO + "~" + strEndNO;
            ResetPage();
            if (temp != "") {
                successTemp = "[" + temp + "]Receive," + msgSuccess;
            }
            if (successTemp != "") {
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
            //temp = temp + ", Receive success";
            //ShowInfo(temp);
        }
        function getSave() {
            var successTemp = "";
            var temp = strBegNO + "~" + strEndNO;
            if (temp != "") {
                successTemp = "[" + temp + "]Save," + msgSuccess;
            }
            var rowInfo = new Array();
            rowInfo.push(document.getElementById("<%=lbPNValue.ClientID%>").innerText);
            rowInfo.push(strBegNO);
            rowInfo.push(strEndNO);
            rowInfo.push(document.getElementById("<%=lbCountValue.ClientID%>").innerText);
            AddRowInfoForDN(rowInfo);
            ResetPageForSave();
            if (successTemp != "") {
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
            //ShowInfo("Save success");
        }
        function SelectFile() {

            ShowInfo("");
            ResetPage();
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
            //document.getElementById("<%=btnTableClear.ClientID%>").click();
            if (checkFileSize()) {
                readFile(document.getElementById("<%=txtBrowse.ClientID %>").value);
            }
            else {
                document.getElementById("<%=hidFileName.ClientID %>").value = "";
                document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
            }
        }
        function DisableUpload() {
            document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
            document.getElementById("<%=txtBrowse.ClientID %>").disabled = false;
            document.getElementById("<%=txtBrowse.ClientID %>").focus();
        }
        function UploadCheck() {
            if (inStatus == "") {
                DisplsyMsg(msgSelectTaget);
                document.getElementById("<%=drpCOACardChange.ClientID%>").focus();
                return false;
            }
            tempCoa = "";
            var coastrList = new Array(); 
            for (var i = 0; i < coaLists.length; i++) {
                var temp = coaLists[i];
                tempCoa = temp[0];
                coastrList.push(tempCoa);
                if (temp[1] == "notfindcoa") {
                    DisplsyMsg("COA:" + tempCoa + ", " + "Wrong Code!");
                    ResetPage();
                    DisableUpload();
                    return false;
                 }
                if (temp[1] == "A1") {
                    DisplsyMsg("COA:" + tempCoa + ", " + msgA1);
                    ResetPage();
                    DisableUpload();
                    return false;
                }
                else if (
                 temp[1] == "01"
                || temp[1] == "02"
                || temp[1] == "05"
                || temp[1] == "16"
                || temp[1] == "11") {

                DisplsyMsg("COA:" + tempCoa + ", " + msg11);
                ResetPage();
                DisableUpload();
                return false;
                   
                }
                
                if (false == CheckStatusForLot(temp[1])) {
                    DisableUpload();
                    return false;
                }
            }
            if (document.getElementById("<%=txtBrowse.ClientID %>").FileName == "") {
                DisplsyMsg(msgEmptyFile);
                DisableUpload();
                return false;
            }
            ShowSuccessfulInfo(true, "Upload! Wait...");
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
            document.getElementById("<%=txtBrowse.ClientID %>").disabled = true;
            
            WebServiceCoaChange.UpdateCOAListLot(coastrList, editor, inStatus, "", station, onUpSuc, OnUpfaild);
            return true;
        }
        function usual_search(data, key) {
            var m = data.length
            for (i = 0; i < m; i++) {
                if (data[i] == key) {
                    return i
                }
            }
            return -1;
        }


        //读文件
        function readFile(filename) {
            coaLists = null;
            var fso = new ActiveXObject("Scripting.FileSystemObject");
            var f = fso.OpenTextFile(filename, 1);
            var coaList= new Array(); ;
            while (!f.AtEndOfStream) {
                var s = "";
                s = f.ReadLine();
                s = s.trim();
                if (s == "") {
                    continue;
                 }
                 if (s.length < 6 || s.trim() == ""
                    || !IsNumber(s.substr(s.length - 6))) {
  
                    DisplsyMsg("COA:" + s + ", " + msgCOA_InputFormat);
                    f.Close();
                    document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                    document.getElementById("<%=txtBrowse.ClientID %>").focus();
                    return;
                }
                
                coaList.push(s);
                
            }
            f.Close();
            if (coaList.length == 0)
            {
                ShowMessage("NO data!,empty file !");
                ShowInfo("NO data!,empty file !");
                document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                ResetPage();
                return;
            }
            ShowSuccessfulInfo(true, "Check file! Wait...");
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
            document.getElementById("<%=txtBrowse.ClientID %>").disabled = true;
            WebServiceCoaChange.GetCoaListForBuyer(coaList, onLotSuc,OnLotfaild);
        }
        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                event.cancelBubble = true;
                return false;
            }
        }
        function onLotSuc(result) {
            document.getElementById("<%=btnUpload.ClientID %>").hideFocus = true;
            document.getElementById("<%=txtBrowse.ClientID %>").disabled = false;
            document.getElementById("<%=btnUpload.ClientID %>").disabled = false;
            
            ShowInfo("");
            coaLists = null;
            try {
                if (result[0] == SUCCESSRET) {
                    coaLists = result[1];
                    if (null != coaLists) {
                        if (inStatus == "") {
                            ShowSuccessfulInfo(true, msgUpload2);
                            document.getElementById("<%=drpCOACardChange.ClientID%>").focus();
                        }
                        else {
                            ShowSuccessfulInfo(true, msgUpload1);
                        }
                    }
                    else {
                        document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
                        ShowMessage("NO data!");
                        ShowInfo("NO data!");
                    }
                    return;
                }
                else if (result == null) {
                    ShowInfo("System Error");
                }
                else {
                    ShowMessage(result);
                    ShowInfo(result);
                }
            }
            catch (e) {
                ShowInfo(e.description);
            }
            document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
        }
        function onUpSuc(result) {
            document.getElementById("<%=btnUpload.ClientID %>").disabled = true;
            document.getElementById("<%=txtBrowse.ClientID %>").disabled = false;
            try {
                if (result[0] == SUCCESSRET) {
                    ShowSuccessfulInfo(true,"Display data!wait...");
                    var indexTemp = indexDN;
                    for (var i = 0; i < coaLists.length; i++) {
                        var rowInfo = new Array();
                        rowInfo.push(coaLists[i][2]);
                        rowInfo.push(coaLists[i][0]);
                        rowInfo.push(coaLists[i][0]);
                        rowInfo.push("1");
                        AddRowInfoForLot(rowInfo);
                    }
                    if (indexTemp > 0) {
                        setSrollByIndex(indexTemp, false);
                    }
                    ShowSuccessfulInfo(true);
                    document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                    coaLists = null;
                }
                else if (result == null) {
                    document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                    ShowInfo("System Error");
                }
                else {
                    document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                    ShowMessage(result);
                    ShowInfo(result);
                }
            }
            catch (e) {
                document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                ShowInfo(e.description);
            }
        }
        function CheckStatusForLot(LotStatus) {

            if (LotStatus == inStatus
            && LotStatus != "") {
                DisplsyMsg("COA:" + tempCoa + ", " +  msgErrorSameStatus);
                ResetPage();
                return false;
            }
            if (LotStatus.trim() != "RE") {
                DisplsyMsg("COA:" + tempCoa + ", " +  msgWrongBuyer);
                ResetPage();
                return false;
            }
            if (false == StatusCheckForLot(LotStatus)) {
                ResetPage();
                if (statusWrong != "") {
                    statusWrong = "Fail!" + statusWrong;
                    DisplsyMsg("COA:" + tempCoa + ", " + statusWrong);
                }
                else {
                    msgWrongStatus = "Fail!" + msgWrongStatus;
                    DisplsyMsg("COA:" + tempCoa + ", " + msgWrongStatus);
                }
                return false;
            }
            return true;
        }
        function StatusCheckForLot(LotStatus) {
            statusWrong = "";
            var ret = false;

            var getReturn = false;
            for (var i = 0; i < txtTable.length; i++) {
                if (getReturn == true) {
                    break;
                }
                var getIn = false;
                for (var j = 0; j < txtTable[i].length; j++) {
                    if (LotStatus.indexOf(txtTable[i][j]) != -1
                && j == 0) {
                        getIn = true;
                        continue;
                    }
                    if (j == 1
                    && getIn == true) {
                        if (txtTable[i][j].indexOf(inStatus.substr(0, 2)) != -1) {
                            ret = true;
                            continue;
                        }
                    }
                    if (j == 2
                && getIn == true
                && ret == false) {
                        statusWrong = txtTable[i][j];
                        getReturn = true;
                        break;
                    }
                }
            }
            return ret;
        }
        function OnLotfaild(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            DisableUpload();
        }
        function OnUpfaild(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            DisableUpload();
        }
    </script>
 <div id="div1" style="width: 98%; border: solid 0px red; margin: 0 auto;">
  <fieldset>
            <legend><asp:Label ID="lbAutoTitle" CssClass="iMes_label_13pt" runat="server">Auto</asp:Label></legend>
                <table width="100%" border="0"> 
                    <tr>
                            <td style="width:10%">
                            <asp:Label ID="lbCOA" runat="server" CssClass="iMes_DataEntryLabel"  >File:</asp:Label>
                             </td>
                              <td style="width:80%">
                            <asp:FileUpload ID="txtBrowse" ame="txtBrowse" Style="width: 95%"
                                runat="server" ContentEditable="false" onkeypress='return OnKeyPress(this)' onchange="SelectFile()"  />
                                </td>
                                <td style="width:10%" align="left">
                            <input id="btnUpload" type="button" onclick="UploadCheck()" 
                                         class="iMes_button" runat="server" 
                                        onmouseover="this.className='iMes_button_onmouseover'" 
                                        onmouseout="this.className='iMes_button_onmouseout'" value="Upload" 
                                        style="color: #000000"/>
                             </td>
                             </tr>
                            
                 </table>  
                   </fieldset>
                 </div>
 <div style="width: 98%; border: solid 0px red; margin: 0 auto;">
   <center >
   <fieldset>
    <legend><asp:Label ID="Label1" CssClass="iMes_label_13pt" runat="server">Manual</asp:Label></legend>
   <table width="100%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
    <tr>
    <br/>
        <td align="left" style="width:80%">
         
            <asp:Label ID="lbCardNo" runat="server" CssClass="iMes_DataEntryLabel" style="width:10%" ></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"  style="width:35%" />
            <asp:Label ID="lbto"   runat="server" CssClass="iMes_DataEntryLabel" style="width:5%"> ~ </asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" style="width:35%" />
        </td>
            <td align="center" style="width:20%">
            <input id="btnQuery" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True" onclick="return btnQuery_onclick()"  />
        </td>
       
        
        
    </tr>
     <tr>
     <td>
     <br/>
     </td> 
      </tr>
        <tr>
        <td align="left"style="width:80%">
        <asp:Label ID="lbRange" Width = "10%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbRangeValue" Width = "32%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbCount" Width = "13%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbCountValue" Width = "25%" runat="server" CssClass="iMes_label_13pt" />
        </td>
        </tr>
         <tr>
     <td>
     <br><br>  
     </td> 
      </tr>
        <tr>
        <td align="left" style="width:80%">
        <asp:Label ID="lbPN"       Width = "10%"  runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbPNValue"   Width = "32%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbPlace"     Width = "13%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbPlaceValue" Width = "25%" runat="server"  CssClass="iMes_label_13pt" />
        </td>
        <td align="center" style="width:20%">
         <input id="btnSave" type="button"  runat="server" 
                class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True"
                onclick="btnSave_onclick()"/>
                
        </td>
      </tr>
        
        
  
    <tr>
     <td>
     <br/> 
     </td> 
      </tr>
      
     </table>  
    </fieldset>
    <table width="100%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
    <tr>
    <td  align="left">
        <asp:Label ID="lbTarget" runat="server" CssClass="iMes_label_13pt"  Width="12%" ></asp:Label>
        <asp:DropDownList runat="server" ID="drpCOACardChange" Width="50%"  >
         </asp:DropDownList>
         </td>   
         </tr>
    <tr>
    <td colspan="4" align="left">
        <asp:UpdatePanel ID="updatePanelGrid" runat="server" UpdateMode="Conditional" >
          <ContentTemplate>
	         <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                GetTemplateValueEnable="False" GvExtHeight="220px" GvExtWidth="100%" OnGvExtRowClick="" Height="210px" 
                OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                onrowdatabound="GridViewExt1_RowDataBound" >
                <Columns>
                    <asp:BoundField DataField="HP P/N"  />
                    <asp:BoundField DataField="Begin No"  />
                    <asp:BoundField DataField="End No" />
                    <asp:BoundField DataField="Qty"  />
                </Columns>
             </iMES:GridViewExt>
          </ContentTemplate>   
         </asp:UpdatePanel> 
    </td>
</tr>
    <tr>
	<td>
	<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
		<ContentTemplate>
		  <button id="btnGetCOAList" runat="server" type="button" onclick="" style="display:none" ></button>	
		  <button id="btnGetCOAListNoEnter" runat="server" type="button" onclick="" style="display:none" ></button>
		  <button id="btnUpdateCOAList" runat="server" type="button" style="display:none" ></button>	
		  <button id="btnReceiveCOAList" runat="server" type="button" style="display:none" ></button>	
		  <button id="btnQueryEarly" runat="server" type="button" style="display:none" ></button>	
		  <button id="btnTableClear" runat="server" type="button" style="display: none" onserverclick="btnTableClear_ServerClick"/>

		  <input type="hidden" runat="server" id="begNO" /> 
		  <input type="hidden" runat="server" id="endNO" />
		  <input type="hidden" runat="server" id="range" />
		  <input type="hidden" runat="server" id="status" />
		  <input type="hidden" runat="server" id="hidFileName"/>
		</ContentTemplate>   
	</asp:UpdatePanel> 
    </td>
</tr>
    </table>
    <input 
                id="btnReceive" type="button"  runat="server" 
                class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="False"
                 onclick="btnReceive_onclick()"/>
    </center>
</div>

</asp:Content>