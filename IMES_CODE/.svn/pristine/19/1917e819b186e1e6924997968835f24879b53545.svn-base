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
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="COAStatusChange.aspx.cs" Inherits="PAK_COAStatusChange" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">

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
    var cn_txtTable = new Array(new Array("A0", "P0,16,01,02,05", "可放置Coa Center 或退給採購"),
                                new Array("P0", "D1,A0", "可領至產線或退給物料"),
                                new Array("P1",	"D1,A1",	"可退回產線中轉處"),
                                new Array("D1", "P0,P1", "可放置Coa Center"),
                                new Array("A1"	,　""	,"已結合!"),
                                new Array("A2",	"P0,A1",	"可放置Coa Center"),
                                new Array("A3",	"P0,A1,D1",	"可放置Coa Center")
    );
    var en_txtTable = new Array(new Array("A0", "P0,16,01,02,05", "可放置Coa Center 或退給採購"),
                                new Array("P0", "D1,A0", "可領至產線或退給物料"),
                                new Array("P1", "D1,A1", "可退回產線中轉處"),
                                new Array("D1", "P0,P1", "可放置Coa Center"),
                                new Array("A1", "", "已結合!"),
                                new Array("A2", "P0,A1", "可放置Coa Center"),
                                new Array("A3", "P0,A1,D1", "可放置Coa Center")
    );    
    var msgErrorSameStatus = '<%=this.GetLocalResourceObject(Pre + "_msgErrorSameStatus").ToString() %>';
    var msgWrongStatus = '<%=this.GetLocalResourceObject(Pre + "_msgWrongStatus").ToString() %>';
    var msgA1 = '<%=this.GetLocalResourceObject(Pre + "_msgA1").ToString() %>';
    var msg11 = '<%=this.GetLocalResourceObject(Pre + "_msg11").ToString() %>';
    var msgSame = '<%=this.GetLocalResourceObject(Pre + "_msgSame").ToString() %>';
    var msgSelectTaget = '<%=this.GetLocalResourceObject(Pre + "_msgSelectTaget").ToString() %>';
    var msgInputTxt = '<%=this.GetLocalResourceObject(Pre + "_msgInputTxt").ToString() %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
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
            numBegNO = parseInt(inputTextBox1.substr(inputTextBox1.length - 6),10);
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
            numEndNO = parseInt(inputTextBox2.substr(inputTextBox2.length - 6),10);
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
            /*document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();*/
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
        if (
           inStatus == "01"
        || inStatus == "02"
        || inStatus == "05"
        || inStatus == "16"
        ) {
            inStatus = "RE";
            return true;
        }
        var getReturn = false;
        for (var i = 0; i < txtTable.length; i++) {
            if (getReturn == true) {
                break;
            }
            var getIn = false;
            for (var j = 0; j < txtTable[i].length; j++) {
                if (currentStatus.indexOf(txtTable[i][j]) != -1
                && j == 0)
                {
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
                && ret==false) 
               {
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
            if (inStatus == "P0" || inStatus == "D1") {
                if (earlyNO != "") {
                    var language = '<%=Pre%>';
                    var msgConfirmSave = "";
                    if (language == "cn") {
                        msgConfirmSave = earlyNO + "比此COA更早入料, 是否确定要发" + strBegNO;
                    }
                    else {
                        msgConfirmSave = earlyNO + " is early than this COA , do you sure to delivery the " + strBegNO;
                    }
                    if (confirm(msgConfirmSave)) {
                    }
                    else {
                        ResetPage();
                        return;
                    }
                }
            }
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
    function btnReceive_onclick()
    {
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
        if (strBegNO == "" || strEndNO == "") 
        {
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
                if (inStatus == "P0" || inStatus == "D1") {
                    if (earlyNO != "") {
                        var language = '<%=Pre%>';
                        var msgConfirmSave = "";
                        if (language == "cn") {
                            msgConfirmSave = earlyNO + "比此COA更早入料, 是否确定要发" + strBegNO;
                        }
                        else {
                            msgConfirmSave = earlyNO + " is early than this COA , do you sure to delivery the " + strBegNO;
                        }
                        if (confirm(msgConfirmSave)) {
                        }
                        else {
                            ResetPage();
                            return;
                        }
                    }
                }
                document.getElementById("<%=status.ClientID%>").value = inStatus;
                document.getElementById("<%=btnUpdateCOAList.ClientID%>").click();
            }
        }
    }
    function getReceive() {
        var successTemp = "";
        var temp =  strBegNO + "~" + strEndNO;
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
    /*function btnSave_onclick() {

        if (currentStatus == "") {
            if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
        || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                if (strBegNO != "" && strEndNO != "") {

                }
                else {
                    document.getElementById("<%=TextBox1.ClientID%>").value = "";
                    document.getElementById("<%=TextBox2.ClientID%>").value = "";
                    ResetPage();
                    DisplsyMsg(msgInputTxt);
                    return;
                }
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
        if (currentStatus == "") {
            return;
        }
        if (currentStatus == "same") {
            DisplsyMsg(msgSame);
            return;
        }
        if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
        || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
            if (strBegNO != "" && strEndNO != "") {

            }
            else {
                document.getElementById("<%=TextBox1.ClientID%>").value = "";
                document.getElementById("<%=TextBox2.ClientID%>").value = "";
                ResetPage();
                DisplsyMsg(msgInputTxt);
                return;
            }
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
        
        if (inStatus == "") {
            DisplsyMsg(msgSelectTaget);
            return;
        }
        CheckStatus();
        if (currentStatus != "") {
            document.getElementById("<%=status.ClientID%>").value = inStatus;
            document.getElementById("<%=btnUpdateCOAList.ClientID%>").click();
            var rowInfo = new Array();
            rowInfo.push(document.getElementById("<%=lbPNValue.ClientID%>").innerText);
            rowInfo.push(strBegNO);
            rowInfo.push(strEndNO);
            rowInfo.push(document.getElementById("<%=lbCountValue.ClientID%>").innerText);
            AddRowInfoForDN(rowInfo);
            ResetPage();
            currentStatus = "same";
            ShowInfo("Save success");
        }
    }*/
    </script>

 <div>
   <center >
   <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
    <tr>
        <td align="left" >
         <br>
            <asp:Label ID="lbCardNo" runat="server" CssClass="iMes_DataEntryLabel" ></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"  style="width:30%" />
            <asp:Label ID="lbto"   runat="server" CssClass="iMes_DataEntryLabel" > ~ </asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" style="width:30%" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            <input id="btnQuery" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True" onclick="return btnQuery_onclick()"  />
        <br><br>
        </td>
        
    </tr>
        <tr>
        <td align="left" >
        <asp:Label ID="lbRange" Width = "10%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbRangeValue" Width = "32%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbCount" Width = "13%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbCountValue" Width = "22%" runat="server" CssClass="iMes_label_13pt" />
        <br><br>
        </td>
        </tr>
        <tr>
        <td align="left" >
        <asp:Label ID="lbPN"       Width = "10%"  runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbPNValue"   Width = "32%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbPlace"     Width = "13%" runat="server" CssClass="iMes_label_13pt" />
        <asp:Label ID="lbPlaceValue" Width = "22%" runat="server"  CssClass="iMes_label_13pt" />
         &nbsp;
         <input id="btnSave" type="button"  runat="server" 
                class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True"
                onclick="btnSave_onclick()"/>
                <br><br>
        </td>
      </tr>
        
      <tr>
        <td  align="left">
        <asp:Label ID="lbTarget" runat="server" CssClass="iMes_label_13pt"  Width="12%" ></asp:Label>
        <asp:DropDownList runat="server" ID="drpCOACardChange" Width="50%"  >
         </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="btnReceive" type="button"  runat="server" 
                class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True"
                 onclick="btnReceive_onclick()"/><br><br>
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
		  <input type="hidden" runat="server" id="begNO" /> 
		  <input type="hidden" runat="server" id="endNO" />
		  <input type="hidden" runat="server" id="range" />
		  <input type="hidden" runat="server" id="status" />
		</ContentTemplate>   
	</asp:UpdatePanel> 
    </td>
</tr>
    </table>
    </center>
</div>

</asp:Content>