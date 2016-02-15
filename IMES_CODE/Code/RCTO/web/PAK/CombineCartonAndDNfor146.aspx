<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineCartonAndDNfor146.aspx.cs" Inherits="CombineCartonAndDNfor146"
    Title="CombineCarton And DN for146" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="20%" align="left">
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPdLine ID="cmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="Label7" runat="server" CssClass="iMes_label_13pt" Text="Part Type:"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server"  Width="100"
                            IsPercentage="true"  />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                    </td>
                    <td id="tdModel">
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDelivery" runat="server" CssClass="iMes_label_13pt" Text=""></asp:Label>
                    </td>
                    <td>
                        <select id="cmbDnList" name="D1" style="width: 100%" onchange='SelectDN(this)'>
                            <option></option>
                            "
                        </select>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="PCS In Carton:"></asp:Label>
                    </td>
                    <td id="tdPcsInCarton">
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="DeliveryQty:"></asp:Label>
                    </td>
                    <td id="tdDnQty">
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt" Text="Carton Remain Qty:"></asp:Label>
                    </td>
                    <td id="tdRemainCartonQty">
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" Text="Delivery Remain Qty:"></asp:Label>
                    </td>
                    <td id="tdDnRemainQty">
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                    </td>
                    <td>
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt" Text="Vendor Code:"></asp:Label>
                    </td>
                    <td>
                        <select id="cmxbVendorCode" style="width: 100%">
                            <option></option>
                        </select>
                    </td>
                </tr>
            </table>
            <fieldset style="width: 95%">
                <legend id="Legend1" runat="server" style="color: Blue" class="iMes_label_13pt">
                    <asp:Label ID="Label6" CssClass="iMes_label_13pt" runat="server" Text="CT"></asp:Label>
                </legend>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="grvProduct" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" HighLightRowPosition="3"
                                        Style="top: 208px; left: 35px">
                                        <Columns>
                                            <asp:BoundField DataField="Material CT" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <hr />
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 12%" align="left">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td style="width: 75%">
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" IsCanInputChinese="true" />
                        </td>
                        <td align="right">
                            <input id="btnPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                                align="right" />
                            &nbsp;<input id="btnReprint" type="button" runat="server" class="iMes_button" onclick="reprint()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <input id="modelHidden" type="hidden" runat="server" />
                <input id="dnListHidden" type="hidden" runat="server" />
                <input id="dnQtyHidden" type="hidden" runat="server" />
                <input id="dnPcsHidden" type="hidden" runat="server" />
                <input id="dnPackedHidden" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">


        var inputObj;
        var PcsInCarton = 0;
        var customerSN = "";

        var model = "";

        var deliveryNo = "";
        var line = "";
        //cn_NotInVendorCode cn_msgInputModel cn_msgNeed9999 message cn_msgDuplicateData msgNotInVendorCode cn_msgInputDev cn_msgInput9999 cn_msgValidModel
        var msgInputDev = '<%=this.GetLocalResourceObject(Pre + "_msgInputDev").ToString() %>';
        var msgInput9999 = '<%=this.GetLocalResourceObject(Pre + "_msgInput9999").ToString() %>';
        var msgValidModel = '<%=this.GetLocalResourceObject(Pre + "_msgValidModel").ToString() %>';
        var msgNeed9999 = '<%=this.GetLocalResourceObject(Pre + "_msgNeed9999").ToString() %>';
        var msgNotInVendorCode = '<%=this.GetLocalResourceObject(Pre + "_msgNotInVendorCode").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgInputModel = '<%=this.GetLocalResourceObject(Pre + "_msgInputModel").ToString() %>';
        var msgInputPdLine = '<%=this.GetLocalResourceObject(Pre + "_msgInputPdLine").ToString() %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

        var isInputFirstCT = false;
        var dnArr;
        var scanQ = 0;
        var isCrSN = false;
        var model = "";
        var index = 1;
        var initPrdRowsCount;
        var grvProductClientID = "<%=grvProduct.ClientID%>";
        var inputSn = "";
        var ctArray = [];
        var paramArr;
        window.onload = function() {

            inputObj = getCommonInputObject();
            initPrdRowsCount = parseInt('<%=initProductTableRowsCount%>', 10) + 1;
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            paramArr = getUrlVars();
            getAvailableData("input");
            callNextInput();
            
        };

        window.onbeforeunload = function() {

            //OnCancel();
        };

        function ChangeType() {
            var type = getConstValueTypeCmbText();
            if (type == "RMAMB") {
                ShowInfo("Please input PF Model and Qty");

            }
            else if (type == "DockingMB") {
                ShowInfo("Please input 146 Model then Select DN");
            }

        }
        function initPage() {
            ResetTxtValue();
            scanQ = 0;
            ctArray = [];
            index = 1;
            $("#cmbDnList").val("");
            $("#cmxbVendorCode").val("");
        }

        function SelectDN(obj) {
            var type = getConstValueTypeCmbText();
            var idx = obj.selectedIndex;
            var dn = obj.options[idx].value;
            if (dn == "")
            { ResetTxtValue(); }
            else
            { PageMethods.GetDnInfo(dn, type, onGetDnInfoSucess, onError); }
        }
        function onGetDnInfoSucess(result) {
            $("#tdPcsInCarton").text(result.CartonQty);
            $("#tdDnRemainQty").text(result.RemainQty);
            $("#tdDnQty").text(result.Qty);
            var cq = parseInt(result.CartonQty);
            var dr = parseInt(result.RemainQty);
            if (dr < cq)
            { $("#tdRemainCartonQty").text(result.RemainQty); }
            else
            { $("#tdRemainCartonQty").text(result.CartonQty); }


        }
        function GetDnInfoByDnNo(dn) {

        }


        function onSucess(result) {
            endWaitingCoverDiv();
            var dnArr = result[1];
            $("#tdScanQty").text(result[0]); //tdCartonSN
            $("#tdCartonSN").text(cartonSN);
            var dnObj = document.getElementById("cmbDnList");

            removeDnList();
            var d = document.createElement("option");
            d.text = "";
            d.value = "";
            dnObj.add(d);


            for (var i = 0; i < dnArr.length; ++i) {
                var option = document.createElement("option");
                option.text = dnArr[i];
                option.value = dnArr[i];
                dnObj.add(option);
            }

            callNextInput();
        }



        function onError(result) {
            EnableSelObj(false);
            endWaitingCoverDiv();
            ShowMessage(result._message);
            callNextInput();
        }
        function CheckExistVendorCode(sn) {
            return $('#cmxbVendorCode').find('option[value=' + sn + ']').length > 0;

        }

        function input(inputData) {
            CheckExistData(inputData);
            ShowInfo("");
            line = getPdLineCmbValue();
            if (line == "") {
                ShowMessage(msgInputPdLine);
                callNextInput();
                return;
            }
            if (getConstValueTypeCmbText() == "") {
                ShowMessage("Please select part type!");
                callNextInput();
                return;
            }
            var lstPrintItem = getPrintItemCollection();
//            if (lstPrintItem == null)//判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
//            {
//                alert(msgPrintSettingPara);
//                callNextInput();
//                return;
//            }

            if (scanQ == 0 && inputData.length == 12) {
                if (inputData.substr(0, 3) != "146" && inputData.substr(0, 2) != "PF") {
                    ShowAllMsg(msgValidModel);
                    callNextInput();
                    return;
                }
                model = inputData;
                beginWaitingCoverDiv();
                PageMethods.GetDeliveryAndVendorCodeList(inputData, getConstValueTypeCmbText(), -10, 0, onGetDnSucess, onError);
               
              
                callNextInput();
                return;
            }
           if ($('#tdModel').text() == '') {
                ShowMessage(msgInputModel); //cn_msgInputModel
                callNextInput();
                return;
            }
            if (getConstValueTypeCmbText() == "RMAMB") {

                if (inputData != "9999" && !isNaN(inputData)) {
                    if (parseFloat(inputData) > 0) {
                        $("#tdPcsInCarton").text(inputData);
                        $("#tdRemainCartonQty").text(inputData);
                        callNextInput();
                        return;
                    }
                }
                else {
                    if ($("#tdPcsInCarton").text() == '') {
                        ShowMessage("Please input CartonQty"); //cn_msgInputModel
                        callNextInput();
                        return;
                    
                    }
                }
            }
            
            var isLastCarton = CheckLastCarton();
            if (isLastCarton) {
                if (inputData != "9999")
                { ShowAllMsg(msgInput9999); }
                else
                { Save(); }
                callNextInput();
                return;
            }
            if (!isLastCarton && inputData == "9999") {
                ShowAllMsg(msgNeed9999);
                callNextInput();
                return;
            }
            if (getConstValueTypeCmbText() != "RMAMB") {
                if ($('#cmbDnList :selected').text() == "") {
                    ShowAllMsg(msgInputDev);
                    callNextInput();
                    return;
                }
            }

            if ((inputData.length != 14 && inputData.length != 10) && inputData.length < 99) {
                alert("Wrong Code!!");
                callNextInput();
                return;
            }
            if (inputData.length > 99)
            { inputData = inputData.substr(76, 14); }


            if (inputData.length == 14 || inputData.length == 10) {
               
                if (CheckExistData(inputData)) {
                    ShowAllMsg(msgDuplicateData);
                    callNextInput();
                    return;
                }
                if (getConstValueTypeCmbValue() == "CRLCM" && inputData.substr(0, 1) != "C") {
                    ShowAllMsg("Wrong clear room sn !!");
                    callNextInput();
                    return;
                }
                if ((getConstValueTypeCmbValue() == "DockingMB" || getConstValueTypeCmbValue() == "RMAMB") && (inputData.substr(4, 1) != "B" && inputData.substr(4, 1) != "M" )) {
                    ShowAllMsg("Wrong mb sn !!");
                    callNextInput();
                    return;
                }
                if (inputData.length == 14) {
                    if (!CheckExistVendorCode(inputData.substr(0, 5))) {
                        ShowAllMsg(msgNotInVendorCode);
                        callNextInput();
                        return;
                    }
                }
                if (inputData.length == 10) {
                    if (!CheckExistVendorCode(inputData.substr(0, 2))) {
                        ShowAllMsg(msgNotInVendorCode);
                        callNextInput();
                        return;
                    }
                }                
//                if (isCrSN && scanQ > 0 && inputData.substr(0, 1) != "C") {
//                    ShowAllMsg("Wrong clear room sn !!");
//                    callNextInput();
//                    return;
                //                }
                
                inputSn = inputData;

                PageMethods.CheckSn(inputData, line, paramArr["UserId"], paramArr["Customer"], getConstValueTypeCmbValue(), paramArr["Station"],model,onCheckSnSucess, onCheckSnError);
                callNextInput();
                return;
            }
            callNextInput();
        }
        function CheckLastCarton() {
            var pcsInCarton = $("#tdPcsInCarton").text();
            var dnRemainQ = $("#tdDnRemainQty").text();
            var cartonRemainQ = parseInt($("#tdRemainCartonQty").text());
            if (cartonRemainQ == 0 && pcsInCarton != "" && dnRemainQ != "" && dnRemainQ != "0"
                 && parseInt(pcsInCarton) > parseInt(dnRemainQ))
            { return true; }
            return false;

        }
        function onCheckSnSucess() {
            if (scanQ == 0) {
                EnableSelObj(false);
                isCrSN = getConstValueTypeCmbValue() == "CRLCM";
            }
            ctArray.push(inputSn);
            scanQ++; //$("#tdModel").text();
            var q = parseInt($("#tdRemainCartonQty").text());
            q--;
            $("#tdRemainCartonQty").text(q);
            AddTableByValue(inputSn);
            if (q == "0") {
                if (CheckLastCarton())
                { ShowInfo(msgInput9999); }
                else
                { { Save(); clearTable(); } }
            }

        }
        function onCheckSnError(error) {
            endWaitingCoverDiv();
            try {
                //  initPage();
                //clearTable();
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                if (scanQ > 0) {
                    EnableSelObj(false);

                }
            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();
        }
        function onGetDnInfoForSaveSucess(result) {
            //   endWaitingCoverDiv();

            $("#tdPcsInCarton").text(result.CartonQty);
            $("#tdDnRemainQty").text(result.RemainQty);
            $("#tdDnQty").text(result.Qty);
            var cq = parseInt(result.CartonQty);
            var dr = parseInt(result.RemainQty);
            if (dr < cq)
            { $("#tdRemainCartonQty").text(result.RemainQty); }
            else
            { $("#tdRemainCartonQty").text(result.CartonQty); }
            if ($("#tdDnRemainQty").text() == "0") {
                PageMethods.GetDeliveryAndVendorCodeList(model, getConstValueTypeCmbText(), -10, 0, onGetDnSucess, onError);
                ResetTxtValue(); $("#cmbDnList").val('');
            }
            //     else { ShowAllMsg("No Delivery data!!"); }
        }
        function Save() {
            beginWaitingCoverDiv();
            deliveryNo = $('#cmbDnList').find('option:selected').val();
            PageMethods.Save(ctArray, $("#tdModel").text(), deliveryNo, scanQ,
                                           getPdLineCmbValue(), paramArr["UserId"], paramArr["Station"], paramArr["Customer"],
                                           getConstValueTypeCmbText(), getPrintItemCollection(), onSaveSuccess, onSaveFail);
        }
        function onSaveSuccess(result) {
            callNextInput();
            endWaitingCoverDiv();
            //   $("#cmbDnList").val('');
            $("#cmbDnList").val(deliveryNo);
            if (getConstValueTypeCmbText() != "RMAMB") {
                PageMethods.GetDnInfo(deliveryNo, getConstValueTypeCmbText(), onGetDnInfoForSaveSucess, onError);
            }
            else {
                $("#tdPcsInCarton").text('');
                $("#tdRemainCartonQty").text('');
            }




            EnableSelObj(true);
            setPrintItemListParam(result[1], result[0]);
            printLabels(result[1], false);
            ShowSuccessfulInfo(true, "Success,Carton SN: " + result[0]);
            clearTable();
            //            if ($("#tdDnRemainQty").text() == "0")
            //            { ResetTxtValue(); $("#cmbDnList").val(''); }
            //
            ctArray = [];
            index = 1;
            scanQ = 0;
            callNextInput();
        }
        function onSaveFail(error) {
            endWaitingCoverDiv();
            try {
                initPage();
                clearTable();
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();
        }
        function onCheckMaSnSucess() { //cmbConstValueType1 CmbPdLine1
            isCrSN = false;
        }
        function EnableSelObj(enable) {
            if (enable) {
                $('select[id^="cmb"]').attr('disabled', false);
            }
            else {
                $('select[id^="cmb"]').attr('disabled', 'disabled');
            }
            getPdLineCmbObj().disabled = !enable;
          //  getConstValueTypeCmbObj().disabled = !enable;
        }
        function onGetDnSucess(result) {
            endWaitingCoverDiv();

            var dnObj = result[0];
            if (result[0].length > 0) {
                //tdModel

                dnArr = result[0];
                SetDnList(result[0]);
                SetVendorCodeList(result[1]);
                ResetTxtValue();
                $("#tdModel").text(model);
            }
            else {
                if (getConstValueTypeCmbText() != "RMAMB") {
                    ShowAllMsg("No Delivery data!!");
                }
                else {
                    SetVendorCodeList(result[1]);
                    ResetTxtValue();
                    $("#tdModel").text(model);
                }
            }
            getConstValueTypeCmbObj().disabled = true;
        }
        function ResetTxtValue() {
            $('td[id^="td"]').text('');
        }

        function removeSeList(selName) {
            var dnObj = document.getElementById(selName);
            var i;
            for (i = dnObj.options.length - 1; i >= 0; i--) {
                dnObj.remove(i);
            }
        }

        function SetDnList(dnArr) {
            //_ShipDate_CnQty_ShipWay
            removeSeList("cmbDnList");
            var dnObj = document.getElementById("cmbDnList");
            var d = document.createElement("option");
            d.text = "";
            d.value = "";
            var way = "";
            dnObj.add(d);
            for (var i = 0; i < dnArr.length; ++i) {

                if (dnArr[i].ShipWay == "T001")//T001:顯示空運, T002:顯示海運
                { way = "空運"; }
                else if (dnArr[i].ShipWay == "T002")
                { way = "海運"; }

                var option = document.createElement("option");
                option.text = dnArr[i].DnNo + "_" + dnArr[i].ShipDate + "_" + dnArr[i].CartonQty + "_" + way;
                option.value = dnArr[i].DnNo;
                dnObj.add(option);
            }

        }
        function SetVendorCodeList(dnArr) {
            removeSeList("cmxbVendorCode");
            var dnObj = document.getElementById("cmxbVendorCode");
            var d = document.createElement("option");
            d.text = "";
            d.value = "";
            dnObj.add(d);
            for (var i = 0; i < dnArr.length; ++i) {
                var option = document.createElement("option");
                option.text = dnArr[i];
                option.value = dnArr[i];
                dnObj.add(option);
            }

        }
        function onGetDnListSucess(result) {
            removeDnList();
            var dnObj = document.getElementById("cmbDnList");
            var d = document.createElement("option");
            d.text = "";
            d.value = "";
            dnObj.add(d);
            var dnArr = result;

            for (var i = 0; i < dnArr.length; ++i) {
                var option = document.createElement("option");
                option.text = dnArr[i].DnNo;
                option.value = dnArr[i].DnNo;
                dnObj.add(option);
            }

        }




        function onFail(result) {
            endWaitingCoverDiv();
            //ResetPage();        
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function AddTableByValue(data) {
            var rowInfo = new Array();
            rowInfo.push(data);
            AddRowInfo(rowInfo);
        }
        function clearTable() {
            try {
                ClearGvExtTable(grvProductClientID, initPrdRowsCount); //grvDNClientID
                index = 1;
                setSrollByIndex(0, false);
            }
            catch (e) {
                alert(e.description);
            }
        }

        function AddRowInfo(RowArray) {
            try {
                if (index < initPrdRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + grvProductClientID + "(RowArray,false, index)");
                }
                else {
                    eval("AddCvExtRowToBottom_" + grvProductClientID + "(RowArray,false)");
                }
                setSrollByIndex(index, false);
                setRowSelectedOrNotSelectedByIndex(index - 2, false, grvProductClientID);
                setRowSelectedOrNotSelectedByIndex(index - 1, true, grvProductClientID);     //设置当前高亮行
                index++;

            }
            catch (e) {
                ShowInfo2(e.description);
                //  PlaySound();
            }
        }



        function OnCancel() {
            if (cartonSN != "") {
                PageMethods.Cancel(cartonSN, onCancelSucess, onError);
            }
        }
        function onCancelSucess() {
        }

        function ExitPage() {
            OnCancel();
        }
        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }

        function callNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }
        function ShowAllMsg(msg) {
            ShowMessage(msg);
            ShowInfo(msg);
        }
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function showPrintSettingDialog() {
            showPrintSetting(paramArr["Station"], paramArr["PCode"]);
        }
        function reprint() {
            var url = "../PAK/RePrint146Label.aspx?Station=" + stationId + "&PCode=" + paramArr["PCode"] + "&UserId=" + paramArr["UserId"] + "&Customer=" + paramArr["Customer"] + "&AccountId=" + paramArr["AccountId"] + "&Login=" + paramArr["UserId"];
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = paramArr["UserId"];
            paramArray[2] = paramArr["Customer"];
            paramArray[3] = paramArr["Station"]; ;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
        function setPrintItemListParam(backPrintItemList, sn) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@CartonSN";
            valueCollection[0] = generateArray(sn);
            for (var i = 0; i < backPrintItemList.length; i++) {
                backPrintItemList[i].ParameterKeys = keyCollection;
                backPrintItemList[i].ParameterValues = valueCollection;
            }
            //setPrintParam(lstPrtItem, "RCTO_Label", keyCollection, valueCollection);
        }
        function CheckExistData(data) {
            var _b = false;
            $('#' + grvProductClientID).find('td').each(function() {
                if ($(this).text().toUpperCase() == data)
                { _b = true; return false; }
            });
            return _b;
            //            var gdObj = document.getElementById(grvProductClientID);
            //            var result = false;
            //            for (var i = 1; i < gdObj.rows.length; i++) {
            //                if (data.trim() != "" && gdObj.rows[i].cells[0].innerText.toUpperCase() == data) {
            //                    result = true;
            //                    break;
            //                }
            //            }
            //            return result;
        }                             
    </script>

</asp:Content>
