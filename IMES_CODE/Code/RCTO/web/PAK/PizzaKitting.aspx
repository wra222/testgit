﻿<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Pizza Kitting
* UI:CI-MES12-SPEC-PAK-UI Pizza Kitting.docx –2011/11/21 
* UC:CI-MES12-SPEC-PAK-UC Pizza Kitting.docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011/11/21    Du.Xuan               Create   
* Known issues:
* ITC-1360-0782  输入错误PizzaID不再reset页面
* ITC-1360-0783  7777清空session
* ITC-1360-0784  添加对ACN PCN SCN的支持
* ITC-1360-0826  应该是906引发
* ITC-1360-0906  修改pizzaID判断错误
* ITC-1360-0928  增加custsn重新刷入提示
* ITC-1360-0981  新版已不存在该问题
* ITC-1360-1060  设置table第一行高亮
* ITC-1360-1066  增加本地检查QTY到达上限
* ITC-1360-1115  reset不再重置line和station
* ITC-1360-1159  增加tip
* ITC-1360-1182  同1159，增加tip
* ITC-1360-1236  清空message
* ITC-1360-1226  7777全部重置页面
* ITC-1360-1538  调整PDLine和Station显示位置
* ITC-1360-1625  前一台资料未收集完，刷到后1台的SN将直接跳到后1台的信息继续结合。（前一台的资料清空不做保存，状态还停留在该站）
* ITC-1360-1627  在刷入下一个SN前，这些信息都可以保留在页面上面，供产线参考
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PizzaKitting.aspx.cs" Inherits="PAK_PizzaKitting" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.expressInput.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePizzaKitting.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblStation" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbStationByType ID="CmbStation" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
            </table>
            <hr />
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 80px;" />
                        <col />
                        <col style="width: 80px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblCustSNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIDContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPizza" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPizzaContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True" >
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                             <asp:TextBox ID="txtInput" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%" Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" />                          
                        </td>
                        <td colspan="4" align="right" style="width: 110px;">
                            <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" id="modelHidden"  runat="server" />
                <input type="hidden" id="customerHidden" runat="server"/>
                <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" ></button>
                <button id="btnFru" runat="server" onserverclick="btnFru_Click" style="display: none" ></button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var customer = "";
        var pCode = "";
        var stationId="";
        var inputObj;
        var emptyPattern = /^\s*$/;
        var customerSN = "";
        var pizzaID = "";
        var inPizzaID = "";
        var productID = "";
        var model = "";
        var iSelectedRowIndex = -1;
        var needReset = false;
        
        //error message
        var msgInputLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine").ToString() %>';
        var msgInputStation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputStation").ToString() %>';
        var msgInputPart = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPart").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgNoCustSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputCustSN").ToString() %>';
        var msgNotFinish = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNotFinish").ToString() %>';
        var msgPartFinish = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPartFinish").ToString() %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            //inputObj = getCommonInputObject();
            //getAvailableData("input");
            $('#<%= txtInput.ClientID %>').expressInput({ callback: input, filter: "\\s" });

            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';

            getPdLineCmbObj().selectedIndex = 0;
            getStationByTypeCmbObj().selectedIndex = 0;

            initPage();
            //callNextInput();
        };

        window.onbeforeunload = function() {
                OnCancel();
        };
        function initPage() {

            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblPizzaContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtInput.ClientID %>"), "");
            
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            productID = "";
            pizzaID = "";
            inPizzaID = "";
            customerSN = "";
            iSelectedRowIndex = -1;
            needReset = false;
            //getPdLineCmbObj().selectedIndex = 0;
            //getStationByTypeCmbObj().selectedIndex = 0;
            //callNextInput();
        }


        function input(data) {

            var line = getPdLineCmbValue();
            var station = getStationByTypeCmbValue();

            if (line == "") {
                alert(msgInputLine);
                callNextInput();
                return;
            }
            
            if (station == "") {
                alert(msgInputStation);
                callNextInput();
                return;
            }

            if (station == "PKOK") {
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == "" || lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    callNextInput();
                    return;
                }
            }
          
            if (data == "7777") {
                //执行Reset
                ShowInfo("");
                ResetPage();
                document.getElementById("<%=btnReset.ClientID%>").click();
                callNextInput();
                return;
            }
            
            if (needReset == true) {
                ShowInfo("");
                ResetPage();
                //callNextInput();
                //return;
            }
            
            if (customerSN == "") {
                //刷入CustomerSN
                inputCustomSN(data);
                return;
            }
            else {
                if (checkCustomSN(data)) {
                    //alert(msgNotFinish);
                    //callNextInput();
                    ResetPage();
                    inputCustomSN(data);
                    return;
                }
                if (inPizzaID == "") {

                    //刷入PizzaID
                    inputPizzaID(data);
                    return;
                }
                else {
                    checkPart(data);
                    return;
                }
            }

            callNextInput();
        }
       
        function checkCustomSN(inputData) {
            var flag = false;
            if (inputData.length == 10 && inputData.substring(0, 2) == "CN") {
                flag = true;
            }
            else if (inputData.length == 11 && (inputData.substring(0, 3) == "SCN" || inputData.substring(0, 3) == "PCN" || inputData.substring(0, 3) == "ACN")) {
                flag = true;
            }
            return flag;
        }

        function inputCustomSN(inputData) {
        
            if (inputData.length == 10 && inputData.substring(0, 2) == "CN")
            {
                inputData = inputData; 
            }
            else if (inputData.length == 11 && (inputData.substring(0, 3) == "SCN" || inputData.substring(0, 3) == "PCN" || inputData.substring(0, 3) == "ACN"))
            {
                inputData = inputData.substring(1, 11);
            }
            else
            {
                alert(msgNoCustSN);
                callNextInput();
                return;
            }

            var line = getPdLineCmbValue();
            var station = getStationByTypeCmbValue();
            customerSN = inputData.substring(inputData.length - 10, 10);
            //beginWaitingCoverDiv();
            WebServicePizzaKitting.inputSN(line, station, customerSN, editor, stationId, customer, onCustSNSucc, onCustSNFail);
            
        }

        function onCustSNSucc(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            if (result[0] == SUCCESSRET) {
                defectInTable = result[2];
                setInfo(result);
                setTable(defectInTable);
                inputFlag = true;
            }
            else {
                ResetPage();            
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onCustSNFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            customerSN = "";
            callNextInput();
        }

        function setInfo(info) {
            //set value to the label
            productID = info[1]["id"];
            model = info[1]["modelId"];
            customerSN = info[1]["customSN"];
            pizzaID = info[1]["pizzaId"];
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), customerSN);
            //setInputOrSpanValue(document.getElementById("<%=lblPizzaContent.ClientID %>"), pizzaID);
            //setInputOrSpanValue(document.getElementById("<%=modelHidden.ClientID%>"), model);
        }

        function setTable(info) {

            var bomList = info;

            for (var i = 0; i < bomList.length; i++) {

                var rowArray = new Array();
                var rw;
                var collection = bomList[i]["collectionData"];
                var parts = bomList[i]["parts"];
                var tmpstr="";

                /*for (var j = 0; j < parts.length; j++) {
                    tmpstr = tmpstr + " " + parts[j]["id"];
                }
                rowArray.push(tmpstr); //part no/name*/
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
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                setSrollByIndex(0, true, tbl);                 
            }
        }

        function inputPizzaID(inputData) {
        
            ShowInfo("");
            if (inputData.length == 10) {
                var line = getPdLineCmbValue();
                var station = getStationByTypeCmbValue();
                inPizzaID = inputData.substring(0,9); 
                //pizzaID = "10050000067"; //inputData.substring(0, 9);
                WebServicePizzaKitting.InputPizzaID(productID, inPizzaID, line, station, model, editor, stationId, customer, onPizzaIDSucc, onPizzaIDFail);
            }
            else {
                alert(msgNoCustSN);
                callNextInput();
            }
        }

        function onPizzaIDSucc(result) {

            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                setInputOrSpanValue(document.getElementById("<%=lblPizzaContent.ClientID %>"), pizzaID);
                ShowInfo(msgInputPart);
                callNextInput();
                //StartPrint();
            }
            else {
                //ResetPage();
                inPizzaID = "";            
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                callNextInput();
            }
            
        }

        function onPizzaIDFail(result) {

            endWaitingCoverDiv();
            inPizzaID = "";
            //ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();

        }

        function checkPart(data) {

            var finished = checkPartFinished(data);
            if (finished) {
                alert(msgPartFinish);
                callNextInput();
                return;
            }
            
            WebServicePizzaKitting.checkPart(productID, data, onCheckSuccess, onCheckFail);
        }

        function onCheckSuccess(result) {
        
            var index = updateTable(result[1]);
            if (index< 0) {
                ShowInfo("System error!");
                callNextInput();
                return; 
            }

            var con = document.getElementById(tbl).rows[index + 1];
            con.cells[4].innerText = defectInTable[index]["scannedQty"];
            con.cells[5].innerText = con.cells[5].innerText + " " + result[1]["PNOrItemName"];
            con.cells[5].title = con.cells[5].innerText;

            //setTable(defectInTable);
            var bFinished = checkFinished(index);
            if (bFinished == true) {
            
                //beginWaitingCoverDiv();
                WebServicePizzaKitting.Save(productID, onSaveSucceed, onSaveFail);
                inputFlag = false;
                return;
            }
            callNextInput();
        }

        function onCheckFail(result) {
            ShowInfo(result.get_message());
            callNextInput();
        }

        function checkFinished(index) {
            var ret = true;

            //if (defectInTable[index]["qty"] == defectInTable[index]["scannedQty"]) {
            //    ret = true;
            //}
            
            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i]["qty"] != defectInTable[i]["scannedQty"]) {
                    ret = false; 
                    break;
                }
            }
            
            return ret;
        }
        
        function checkPartFinished(partno) {
            var ret = false;

            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i]["PartNoItem"] == partno)
                {
                    if (defectInTable[i]["qty"] == defectInTable[i]["scannedQty"]) {
                        ret = true;
                    }
                    else {
                        ret = false;
                    }
                    break;
                }
            }

            return ret;
        }
        
        function setGdHighLight(index) {
            if ((iSelectedRowIndex != -1) && (iSelectedRowIndex != index)) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, tbl);     //去掉过去高亮行           
            }
            setRowSelectedOrNotSelectedByIndex(index, true, tbl);     //设置当前高亮行
            iSelectedRowIndex = index;    //记住当前高亮行
        }

        function onSaveSucceed(result) {
            endWaitingCoverDiv();
            StartPrint();
        }

        function onSaveFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
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

        function StartPrint() {
            try {
                
                var line = getPdLineCmbValue();
                var station = getStationByTypeCmbValue();

                if (station != "PKOK") {
                    //ResetPage();
                    needReset = true;
                    ShowSuccessfulInfo(true);
                    callNextInput();
                    return;
                }
                
                ShowInfo("");
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == "" || lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    callNextInput();
                    return;
                }

                //beginWaitingCoverDiv();
                WebServicePizzaKitting.Print(productID, line, station, "", editor, stationId, customer, lstPrintItem,onPrintSucc, onFail);
            }
            catch (e) {
                alert(e);
            }
        }
        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }

        function setPrintItemListParam(backPrintItemList,labelTemp) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = new Array();
            lstPrtItem[0] = backPrintItemList;
            
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@sn";
            
            valueCollection[0] = generateArray(customerSN);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */
            setPrintParam(lstPrtItem, labelTemp, keyCollection, valueCollection);
        }

        function onPrintSucc(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                var index = 0;
                var printlist = new Array();
                
                if (result[0] == SUCCESSRET) {
                    index = getTemp(result[1], result[2]);
                    setPrintItemListParam(result[1][index], result[2]);
                    printlist[0] = result[1][index];
                    //==========================================print process=======================================
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */

                    printLabels(printlist, false);
                    //==========================================end print process===================================
                    //ResetPage();
                    needReset = true;
                    ShowSuccessfulInfo(true);
                }
                else if (result == null) {
                    ShowMessage("msgSystemError");
                    ShowInfo("msgSystemError");
                }
                else {
                    ShowMessage(result);
                    ShowInfo(result);
                }
            }
            catch (e) {
                alert(e.description);
            }
            
            callNextInput();
        }

        function onFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function OnCancel() {
            if (productID != "") {
                WebServicePizzaKitting.cancel(productID);
            }
        }
        
        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
        }

        //set commport For Led
        function setobjMSCommParaForLights() {
            var objMSComm = document.getElementById("objMSComm");
            //    alert("2");
            //	alert(objMSComm.OutBufferSize);
            if (objMSComm.CommPort != 1) {
                if (objMSComm.PortOpen) {
                    objMSComm.PortOpen = false;
                }
                objMSComm.CommPort = 1;
            }

            objMSComm.Settings = "9600,n,8,1";
            objMSComm.RThreshold = 1;
            objMSComm.SThreshold = 1;
            objMSComm.Handshaking = 0;

            try {
                if (objMSComm.PortOpen == false)
                    objMSComm.PortOpen = true;
            } catch (e) {
                alert(e.description);
            }
        }

        function callNextInput() {

            //getCommonInputObject().focus();
            //getAvailableData("input");
            $('#<%= txtInput.ClientID %>').expressInput('setCallback', input);

        }

        function clkSetting() {

            var line = getPdLineCmbValue();
            //var station = getStationByTypeCmbValue();
            //pCode = "OPPA043";
            showPrintSetting(stationId, pCode);
        }                                                                                                        
    </script>

</asp:Content>
