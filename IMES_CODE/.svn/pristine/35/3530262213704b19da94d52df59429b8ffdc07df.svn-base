<%--
 INVENTEC corporation (c)2008 all rights reserved. 

 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="OfflinePizzaKitting.aspx.cs" Inherits="PAK_OfflinePizzaKitting" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceOfflinePizzaKitting.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server" Width="100" IsPercentage="true"
                            on />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbModel ID="cmbModel1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
            </table>
            <table border="0" width="95%">
                <tr>
                    <td colspan="5">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Always">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                    GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                    HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table border="0" width="95%">
                <tr>
                    <td align="left" width="15%">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" width="65%">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                    <td align="right">
                        <input id="btpPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                  <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">

        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var defectInTable = [];
        var model = "";
        var customer;
        var scanQty = 0;
        var gvClientID = "<%=gd.ClientID %>";
        var gvTable = document.getElementById(gvClientID);
        var strRowsCount = "<%=initRowsCount%>";
        var initRowsCount = parseInt(strRowsCount, 10) + 1;
        var pCode;
        var printflag = false;
        var user;
        var station;
        var lastKey = "";
        var Key = "";
        var printItemlist;
        var accountid;
        var login;
        document.body.onload = function() {
            try {
                customer = "<%=customer%>";
                station = "<%=station%>";
                user = "<%=userId%>";
                getAvailableData("processDataEntry");
                pCode = "<%=pCode%>";
                accountid = '<%=AccountId%>';

                login = '<%=Login%>';
            } catch (e) {
                alert(e.description);
            }
            //            var url = "../FA/RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; 
        }
        function ClearPart() {
            var tab = document.getElementById(gvClientID);
            var x = 0;
            for (var i = 1; i < tab.rows.length; i++) {
                if (tab.rows[i].cells[1].innerText.trim() != "") {
                    tab.rows[i].cells[4].innerText = "0";
                    tab.rows[i].cells[5].innerText = "";
                }

            }
        }
        function GetPartCount() {

            var tab = document.getElementById(gvClientID);
            var x = 0;
            for (var i = 1; i < tab.rows.length; i++) {
                if (tab.rows[i].cells[1].innerText.trim() != "") {
                    x++;
                    break;
                }

            }
            return x;

        }
        function onModelChange() {
            getCommonInputObject().value = "";
            var line = getPdLineCmbValue();
            var model = getModelCmbText();
            if (line == "" || model == "") {
                return;
            }
            beginWaitingCoverDiv();
            // GetBOM(string model, string key, string lastKey, string line, string editor, string station, string customer)
            WebServiceOfflinePizzaKitting.GetBOM(model, Key, line, user, station, customer, onGetBOMSucc, onGetBOMSNFail);
        }



        function onGetBOMSucc(result) {

            endWaitingCoverDiv();
            callNextInput();
            ShowInfo("");
            if (result[0] == SUCCESSRET) {
                //    lastKey = Key;
                Key = result[1];
                defectInTable = result[2];
                if (defectInTable.length == 0) {
                }
                else {
                    setTable(defectInTable);

                }
            }


        }
        function onGetBOMSNFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            customerSN = "";
            callNextInput();

        }
        function checkFinished() {
            var ret = true;
            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i]["qty"] != defectInTable[i]["scannedQty"]) {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
        function setTable(info) {

            var bomList = info;
            tblRows = bomList.length;
            if (tblRows == 0) { finishPart = true; }
            for (var i = 0; i < bomList.length; i++) {

                var rowArray = new Array();
                var rw;
                var collection = bomList[i]["collectionData"];
                var parts = bomList[i]["parts"];
                var tmpstr = "";
                if (bomList[i]["PartNoItem"] == null) {
                    rowArray.push(" ");
                }
                else {
                    rowArray.push(bomList[i]["PartNoItem"]);
                }

                rowArray.push(bomList[i]["tp"]);
                if (bomList[i]["description"] == null) {
                    rowArray.push(" ");
                }
                else {
                    rowArray.push(bomList[i]["description"]);
                }

                rowArray.push(bomList[i]["qty"]);
                rowArray.push(bomList[i]["scannedQty"]);
                coll = "";
                for (var j = 0; j < collection.length; j++) {
                    tmpstr = tmpstr + " " + collection[j]["pn"];
                }
                rowArray.push(tmpstr); //["collectionData"]);

                //add data to table
                if (i < strRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + gvClientID + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, gvClientID);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + gvClientID + "(rowArray, false);");
                    setSrollByIndex(i, true, gvClientID);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                setSrollByIndex(0, true, gvClientID);
            }
        }


        var lstPrintItem;
        function processDataEntry(inputData) {
            try {
                ShowInfo("");
                var errorFlag = false;
                if (getPdLineCmbValue() == "") {
                    alert(mesNoSelPdLine);
                    setPdLineCmbFocus();
                    getAvailableData("processDataEntry");
                    return;
                }
                printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    getAvailableData("processDataEntry");
                    return;
                }
                if (GetPartCount() == 0) {
                    ShowMessage("Please get BOM first!");
                    getAvailableData("processDataEntry");
                    return;
                }
                checkPart(inputData);
            }
            catch (e) {
                alert(e.description);
            }

        }
        function checkPart(data) {


            WebServiceOfflinePizzaKitting.checkPart(Key, data, onCheckSuccess, onCheckFail);


        }
        function onCheckSuccess(result) {

            var index = updateTable(result[1]);
            if (index < 0) {
                ShowInfo("System error!");
                callNextInput();
                return;
            }

            var con = document.getElementById(gvClientID).rows[index + 1];
            con.cells[4].innerText = defectInTable[index]["scannedQty"];
            con.cells[5].innerText = con.cells[5].innerText + " " + result[1]["PNOrItemName"];
            con.cells[5].title = con.cells[5].innerText;
            if (checkFinished()) {
                var line = getPdLineCmbValue();
                beginWaitingCoverDiv();
                WebServiceOfflinePizzaKitting.Save(printItemlist, Key, line, onSaveSucceed, onSaveFail);
                //                inputFlag = false;
            }
            callNextInput();
        }
        function onSaveSucceed(result) {
            endWaitingCoverDiv();
            ClearPart();
            Key = result[3];
            defectInTable = result[4];
            var printItem = result[2];
            setPrintItemListParam(printItem, result[1]);
            printLabels(printItem, false);
            ShowInfo(msgSuccess + ", Pizza ID :" + result[1]);
        }
        function onSaveFail(result) {
            endWaitingCoverDiv();
            ShowInfo(result.get_message());
            callNextInput();
        }

        function onCheckFail(result) {
            ShowInfo(result.get_message());
            callNextInput();
        }

        function updateTable(result) {

            var index = -1;
            for (var i = 0; i < defectInTable.length; i++) {
                var found = -1;
                for (var j = 0; j < defectInTable[i]["parts"].length; j++) {
                    if (defectInTable[i]["parts"][j]["id"] == result["PNOrItemName"]) {
                        found = j;
                        defectInTable[i]["scannedQty"]++;
                        break;
                    }
                }
                if (found >= 0) {
                    index = i;
                    break;
                }
            }
            return index;
        }


        function clearTable() {
            try {
                ClearGvExtTable("<%=gd.ClientID%>", initRowsCount);

            } catch (e) {
                alert(e.description);

            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Print using~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        function setPrintItemListParam(backPrintItemList, custsn) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@PizzaID";
            valueCollection[0] = generateArray(custsn);
            setPrintParam(lstPrtItem, "Offline_PizzaLabel", keyCollection, valueCollection);
        }
        function callNextInput() {
            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }


        window.onbeforeunload = function() {
            ExitPage();

        }

        function ExitPage() {
            //ITC-1360-0824, Jessica Liu, 2012-2-28
            if (Key != "") {
                WebServiceOfflinePizzaKitting.Cancel(Key);

            }
        }
        function ResetPage() {
            ExitPage();
            reset();
            getCommonInputObject().value = "";
            getPdLineCmbObj().selectedIndex = 0;
            setPdLineCmbFocus();

        }

        function showPrintSettingDialog() {
            showPrintSetting(station, "<%=pCode%>");
        }
        function reprint() {
            //Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var url = "../PAK/RePrintOfflinePizzaKitting.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + user + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; 
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = user;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }
    </script>

</asp:Content>
