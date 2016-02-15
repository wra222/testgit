<%--
 /*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:offline pizza kitting page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2013-11-27   Benson               Create          
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */ 
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="OfflinePizzaKittingForRCTO.aspx.cs" Inherits="PAK_OfflinePizzaKittingForRCTO" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceOfflinePizzaKittingForRCTO.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 8%" align="left">
                        <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width:8%" align="left">
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server" Width="100" IsPercentage="true"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 8%" align="left">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbModel ID="cmbModel1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 8%" align="left">
                        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt" Text="PartNo:"></asp:Label>
                    </td>
                    <td colspan="5" align="left"   >
                        <select id="drpPartNo" style="width:100%">
                            <option></option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 8%" align="left">
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" Text="Qty:"></asp:Label>
                    </td>
                    <td colspan="5" align="left"   >
                       <input id="inputQty" type="text" value="1"  style="width:100%"/>
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
        var selPart = "";
        var inputQty = 0;
        var setQty = 0;
        var index = 1;
        var isVendorCode = false;
        var line = "";
        document.body.onload = function() {
            try {
                customer = "<%=customer%>";
                station = "<%=station%>";
                user = "<%=userId%>";
                getAvailableData("processDataEntry");
                pCode = "<%=pCode%>";
                accountid = '<%=AccountId%>';
        
                login = '<%=Login%>';
                setPdLineCmbFocus();

            } catch (e) {
                alert(e.description);
            }
            //            var url = "../FA/RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login;
        }
        function RemoveAllPart() {
           document.getElementById("drpPartNo").innerHTML = "";
       }
       function CheckPositiveInteger(data) {
          var re = /^[1-9]\d*$/;
           if (!re.test(data)) {
               return false;
           }
           return true;
       }
        function AddPart(value) {
            var opt = document.createElement("option");
            document.getElementById("drpPartNo").options.add(opt);
            opt.text = value.replace(',','/');
            opt.value = value.replace(',', '/');

        }
        function DisableControl(isEnable) {
           
            document.getElementById("drpPartNo").disabled = isEnable;
            getConstValueTypeCmbObj().disabled = isEnable;
            getModelCmbObj().disabled = isEnable;
            getPdLineCmbObj().disabled = isEnable;
            document.getElementById("inputQty").disabled = isEnable;
        }
        function GetSelectPartNo() {
            var e = document.getElementById("drpPartNo");
            return e.options[e.selectedIndex].text;
        }
        function CheckNeedInput() {
            var Error="";
            if (getPdLineCmbValue() == "") {
                Error = mesNoSelPdLine;
                setPdLineCmbFocus();
                return Error;
            }
            if (getConstValueTypeCmbText() == "") {
                Error = "Please select Family";
             //   setPdLineCmbFocus();
                return Error;
            }
            if (getModelCmbText() == "") {
               Error="Please select Model"
                //setPdLineCmbFocus();
               return Error;
            }
            if (GetSelectPartNo() == "") {
                Error="Please select PartNo"
              //  setPdLineCmbFocus();
                return Error;
            }
            var q = document.getElementById("inputQty").value;
            if (!CheckPositiveInteger(q)) {
                Error = "Please input correct qty";
                return Error;
            } else if (q > 100) {
            Error = "Qty must be between 1 ~100 ";
            return Error;
            }
            return Error;
        }
        function ClearPart() {
//            var tab = document.getElementById(gvClientID);
//            var x = 0;
//            for (var i = 1; i < tab.rows.length; i++) {
//                if (tab.rows[i].cells[1].innerText.trim() != "") {
//                    tab.rows[i].cells[4].innerText = "0";
//                    tab.rows[i].cells[5].innerText = "";
//                }

//            }
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
            needClearPart = true
            getCommonInputObject().value = "";
            RemoveAllPart();
            var line = getPdLineCmbValue();
            var model = getModelCmbText();
            if (line == "" || model == "") {
                return;
            }
            beginWaitingCoverDiv();
            // GetBOM(string model, string key, string lastKey, string line, string editor, string station, string customer)
            WebServiceOfflinePizzaKittingForRCTO.GetBOM(model, Key, line, user, station, customer, onGetBOMSucc, onGetBOMSNFail);
        }
        var needClearPart = true;


        function onGetBOMSucc(result) {

            endWaitingCoverDiv();
            if (needClearPart) {
                RemoveAllPart();
            }
          
            callNextInput();
            ShowInfo("");
            if (result[0] == SUCCESSRET && needClearPart) {
                //    lastKey = Key;
                Key = result[1];
                defectInTable = result[2];
                if (defectInTable.length > 0) {
                    AddPartItem(defectInTable)
                }

            }
            else {
                Key = result[1];
            }

        }
        function onLineChange() {
            setModelCmbToDefault();
            RemoveAllPart();
        }
        function onGetBOMSNFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            customerSN = "";
            callNextInput();

        }
      
        function AddPartItem(bomList) {
            AddPart("");
            for (var i = 0; i < bomList.length; i++) {
                AddPart(bomList[i].PartNoItem);
            }
        }
        function AddRowInfo(RowArray) {
            try {
                if (index < initRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + gvClientID + "(RowArray,false, index)");
                }
                else {
                    eval("AddCvExtRowToBottom_" + gvClientID + "(RowArray,false)");
                }
                setSrollByIndex(index, false);
                setRowSelectedOrNotSelectedByIndex(index - 2, false, gvClientID);
                setRowSelectedOrNotSelectedByIndex(index - 1, true, gvClientID);     //设置当前高亮行
                index++;

            }
            catch (e) {
            //    ShowInfo2(e.description);
                //  PlaySound();
            }
        }
        function setTable(info) {
            AddPartItem(info);
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
                if (i < 12) {
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
                if (inputData == "7777") {
                    ClearPage();
                    getAvailableData("processDataEntry");
                    return;
                }
                printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    getAvailableData("processDataEntry");
                    return;
                }
                var check = CheckNeedInput();
                if (check != "") {
                    ShowMessage(check);
                    getAvailableData("processDataEntry");
                    return;
                }
                if (inputQty > 0 && isVendorCode) {
                    if (CheckExistData(inputData)) {
                        ShowMessage("此CT 已刷過!");
                        getAvailableData("processDataEntry");
                        return;
                    }
                
                }
                checkPart(inputData);
            }
            catch (e) {
                alert(e.description);
            }

        }
        function checkPart(data) {
            if (inputQty == 0)
            { selPart = GetSelectPartNo(); selPart = selPart.replace('/', ','); }
            WebServiceOfflinePizzaKittingForRCTO.checkPart(Key, data, selPart, onCheckSuccess, onCheckFail);
        }
        function CheckExistData(data) {

            var gdObj = document.getElementById(gvClientID);
            var result = false;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (data.trim() != "" && gdObj.rows[i].cells[4].innerText.toUpperCase() == data) {
                    result = true;
                    break;
                }
            }
            return result;
        }
        function onCheckSuccess(result) {
            if (inputQty == 0) {

                DisableControl(true);
                setQty = document.getElementById("inputQty").value;
                line = getPdLineCmbValue();
                var q = parseInt(result[1][4], 10);
                if (q > 0)
                { isVendorCode = true; }
                else
                { isVendorCode = false; }
            }
           
            inputQty++;
            var rowInfo = new Array();

            rowInfo.push(inputQty);
            rowInfo.push(result[1][0]);
            rowInfo.push(result[1][1]);
            rowInfo.push(result[1][2]);
            rowInfo.push(result[1][3]);
            AddRowInfo(rowInfo);
            if (inputQty == setQty) {
                beginWaitingCoverDiv();
                WebServiceOfflinePizzaKittingForRCTO.Save(printItemlist, Key, line, onSaveSucceed, onSaveFail);
            }
           callNextInput();
        }
        function onSaveSucceed(result) {
            endWaitingCoverDiv();
            DisableControl(false);
            clearTable();
       //     ClearPart();
            Key = result[3];
          //  defectInTable = result[4];
            var printItem = result[2];
            setPrintItemListParam(printItem, result[1]);
            printLabels(printItem, false);
            index = 1;
            inputQty = 0;
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
                eval("setRowNonSelected_" + "<%=gd.ClientID%>" + "()");
               // setSrollByIndex(i, true, "<%=gd.ClientID%>");
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
            setPrintParam(lstPrtItem, "RCTO_Offline_PizzaLabel", keyCollection, valueCollection);
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
                WebServiceOfflinePizzaKittingForRCTO.Cancel(Key);

            }
        }
        function ClearPage() {
            index = 1;
            inputQty = 0;
            ExitPage();
            clearTable();
            DisableControl(false);
            var check = CheckNeedInput();
            if (check == "") {
                needClearPart = false;
                var line = getPdLineCmbValue();
                var model = getModelCmbText();
                if (line == "" || model == "") {
                    return;
                }
                beginWaitingCoverDiv();
                // GetBOM(string model, string key, string lastKey, string line, string editor, string station, string customer)
                WebServiceOfflinePizzaKittingForRCTO.GetBOM(model, Key, line, user, station, customer, onGetBOMSucc, onGetBOMSNFail);
               
               
            }
        }
        function ResetPage() {
            ExitPage();
           // reset();
            getCommonInputObject().value = "";
            getPdLineCmbObj().selectedIndex = 0;
            index = 1;
            inputQty = 0;
            setPdLineCmbFocus();
            RemoveAllPart();
        }

        function showPrintSettingDialog() {
            showPrintSetting(station, "<%=pCode%>");
        }
        function reprint() {
            //Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var url = "../PAK/RePrintOfflinePizzaKittingForRCTO.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + user + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; 
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = user;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }
    </script>

</asp:Content>
