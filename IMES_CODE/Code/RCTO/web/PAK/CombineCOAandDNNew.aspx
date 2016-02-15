<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine COA and DN
* CI-MES12-SPEC-PAK-UI Combine COA and DN.docx
* CI-MES12-SPEC-PAK-UC Combine COA and DN.docx             
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/08/10   Du.Xuan               Create   
* Known issues:

* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineCOAandDNNew.aspx.cs" Inherits="PAK_CombineCoaAndDn" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <bgsound src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceCombineCoaAndDn.asmx" />
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
                    <asp:Label ID="lblLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>
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
                        <td align="left">
                            <asp:Label ID="lblCustSNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblProductIDContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblBT" runat="server" CssClass="iMes_label_15pt_Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: auto">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblCheckItem" runat="server"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 80px;" />
                        <col />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblCOA" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="40%">
                            <asp:Label ID="lblCOAContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td width="60%">
                            <asp:Label ID="lblCOAIEC" runat="server" CssClass="iMes_label_30pt_Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                        <td align="right" style="width: 110px;">
                            <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                        </td>
                        <td align="right" style="width: 110px;">
                            <button id="btnRePrint" runat="server" onclick="btnRePrint_onclick()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <input id="modelHidden" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
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
        var stationId = "";
        var pCode = "";
        var hostname = getClientHostName();
        var accountId;
        var customer;
        var login;
        var userName;

        var inputObj;
        var emptyPattern = /^\s*$/;

        var customerSN = "";
        var productID = "";
        var model = "";
        var dn = "";

        var coaPN = "";

        var btFlag = false;
        var coaFlag = false;
        var fullFlag = false;

        //error message
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgInputPDLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';

        var msgNoNeedCOA = '<%=this.GetLocalResourceObject(Pre + "_msgNoNeedCOA").ToString() %>';
        var msgInputCOA = '<%=this.GetLocalResourceObject(Pre + "_msgInputCOA").ToString() %>';

        var msgWin8 = '<%=this.GetLocalResourceObject(Pre + "_msgWin8").ToString() %>';
        var msgCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgCDSI").ToString() %>';
        var msgNotCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgNotCDSI").ToString() %>';
        var msgChangeModel = '<%=this.GetLocalResourceObject(Pre + "_msgChangeModel").ToString() %>';
        var msgDnFull = '<%=this.GetLocalResourceObject(Pre + "_msgDnFull").ToString() %>';
        
        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();

            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            accountId = '<%=Request["AccountId"] %>';
            login = '<%=Request["Login"] %>';
            userName = '<%=Request["UserName"] %>';
            
            
            getPdLineCmbObj().selectedIndex = 0;

            initPage();
            callNextInput();

        };

        window.onbeforeunload = function() {

            OnCancel();
        };

        function initValue() {
            customerSN = "";
            productID = "";
            model = "";
            coaPN = "";
            btFlag = false;
            coaFlag = false;
            fullFlag = false;
        }
        function initPage() {

            tbl = "<%=gd.ClientID %>";

            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblCOAContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblCOAIEC.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblBT.ClientID %>"), "");

            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            initValue();

        }

        function input(data) {

            var line = getPdLineCmbValue();

            if (line == "") {
                alert(msgInputPDLine);
                callNextInput();
                return;
            }

            if (customerSN == "") {

                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == "" || lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    callNextInput();
                    return;
                }
                ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                inputCustomerSN(data);
                return;
            }
            else {
                beginWaitingCoverDiv();
                WebServiceCombineCoaAndDn.checkCOA(productID, data, onInputCOASucc, onInputCOAFail);
            }
        }

        function inputCustomerSN(inputData) {

            if ((inputData.length == 11) && (inputData.substring(0, 3) == "SCN")) {
                inputData = inputData.substring(1, 11);
            }

            if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {

                inputData = inputData;
            }
            else {
                alert(msgInputCustSN);
                ShowInfo(msgInputCustSN);
                callNextInput();
                return;
            }

            var line = getPdLineCmbValue();

            ShowInfo("");
            beginWaitingCoverDiv();
            WebServiceCombineCoaAndDn.InputSN(inputData, line, editor, stationId, customer, onInputSNSucc, onFail);
            return;
        }

        function onInputSNSucc(result) {

            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                setInfo(result);
                if (coaFlag) {
                    callNextInput();
                }
                else {
                    save();
                }
            }
            else {
                ShowInfo("");
                var content = result[0];
                ResetPage();
                ShowMessage(content);
                ShowInfo(content);
                callNextInput();
            }
        }

        function onFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function onInputCOASucc(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            var nodestr = "";
            if (result[0] == SUCCESSRET) {

                nodestr = result[1];
                if (nodestr != "") {
                    ShowMessage(nodestr);
                    ShowInfo(nodestr);
                }
                else {
                    coaPN = result[2];
                    setInputOrSpanValue(document.getElementById("<%=lblCOAContent.ClientID %>"), coaPN);
                    save();
                    return;
                }
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onInputCOAFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function setInfo(result) {
            //set value to the label
            var showstr = "";

            var oldmodel = model;
            var oldBtFlag = btFlag;
            productID = result[1]["ProductID"];
            customerSN = result[1]["CustSN"];
            model = result[1]["Model"];
            btFlag = result[2];

            if (fullFlag) {
                showstr = showstr + "DN = " + dn + " " + msgDnFull;
                fullFlag = false;             
            }

            if (model != oldmodel && oldmodel != "" && oldBtFlag==btFlag && !btFlag) {
                showstr = showstr + msgChangeModel;
            }
            
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), customerSN);
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
            setInputOrSpanValue(document.getElementById("<%=lblCOAContent.ClientID %>"), "");
            
            
            if (btFlag) {
                setInputOrSpanValue(document.getElementById("<%=lblBT.ClientID %>"), "BT");
            }
            else {
                setInputOrSpanValue(document.getElementById("<%=lblBT.ClientID %>"), "Non BT");
            }

            if (result[5] != "") {
                showstr = showstr + msgWin8;
            }

            if (result[6] != "") {
                showstr = showstr + msgCDSI;
            }
            else {
                showstr = showstr + msgNotCDSI;
            }
            
            if (result[3] == "") {
                coaFlag = false;
                setInputOrSpanValue(document.getElementById("<%=lblCOAIEC.ClientID %>"), msgNoNeedCOA);
            }
            else {
                coaFlag = true;
                setInputOrSpanValue(document.getElementById("<%=lblCOAIEC.ClientID %>"), result[3]);
                showstr = showstr + msgInputCOA;
                if (result[7] == "") {
                    showstr = showstr + ":" + result[3];
                }
            }
    
            ShowInfo(showstr);
            if (!btFlag) {
                setTable(result[4]);
            }
            else {
                ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            }

        }

        function setTable(info) {

            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            
            var dnInfo = info;
           
            var rowArray = new Array();
            var rw;

            rowArray.push(dnInfo["DeliveryNo"]);
            rowArray.push(dnInfo["ModelName"]);
            rowArray.push(dnInfo["Editor"]); //CustomerPN
            rowArray.push(dnInfo["PoNo"]);
            rowArray.push(dnInfo["ShipDate_Str"]);
            rowArray.push(dnInfo["Qty"]); //CustomerPN
            rowArray.push(dnInfo["ShipmentID"]); //packetQty

            dn = dnInfo["DeliveryNo"];
            //add data to table
            eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, 1);");
            setSrollByIndex(0, true, tbl);
        }

        function save() {

            ShowInfo("");
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            WebServiceCombineCoaAndDn.save(productID,lstPrintItem, onSaveSucc, onFail);

        }

        function onSaveSucc(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            var index = 0;
            var printlist = new Array();
            var count = 0;
            var showList = "";

            try {
                if (result[0] == SUCCESSRET) {
                        
                    var jpflag = result[2];
                    index = getTemp(result[1], "PIZZA Label-1");
                    if (index >= 0) {
                        setPrintItemListParam(result[1][index], "PIZZA Label-1");
                        printlist[count] = result[1][index];
                        count++;
                    }

                    if (jpflag ) {
                        index = getTemp(result[1], "PIZZA Label-2");
                        if (index >= 0) {
                            setPrintItemListParam(result[1][index], "PIZZA Label-2");
                            printlist[count] = result[1][index];
                            count++;
                        }
                    }
                    if (btFlag) {
                        index = getTemp(result[1], "BT COO Label");
                        if (index >= 0) {
                            setPrintItemListParam(result[1][index], "BT COO Label");
                            printlist[count] = result[1][index];
                            count++;
                        }
                    }
                    //==========================================print process=======================================
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    printLabels(printlist, true);
                    //==========================================end print process==================================
                    var tmpinfo = customerSN;
                    //ResetPage();
                    //initValue();
                    customerSN = "";
                    if (!btFlag) {
                        setTable(result[3]);
                    }
                    setInputOrSpanValue(document.getElementById("<%=lblCOAContent.ClientID %>"), "");
                    ShowSuccessfulInfoFormat(true, "Customer SN", tmpinfo);
                }
                else if (result == null) {
                    ResetPage();
                    ShowInfo("msgSystemError");
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                }
                callNextInput();
            }
            catch (e) {
                ResetPage();
                ShowInfo(e.description);
               
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

        function setPrintItemListParam(backPrintItemList, labelType) {
            //============================================generate PrintItem List==========================================
            //var lstPrtItem = backPrintItemList;
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
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        }

        function OnCancel() {
            if (!(productID == "")) {
                WebServiceCombineCoaAndDn.cancel(productID);
            }
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            //callNextInput();
        }

        function callNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function clkSetting() {

            showPrintSetting(stationId, pCode);
        }

        function btnRePrint_onclick() {
            var url = "CombineCOAandDNReprint.aspx?Station=" + stationId + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + userName + "&AccountId=" + accountId + "&Login=" + login + "&COO=" + "true";
            window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
            callNextInput();
        }

        function PlaySound() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["DuplicateAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            obj.src = sUrl;
        }

        function PlaySoundClose() {

            var obj = document.getElementById("bsoundInModal");
            obj.src = "";
        }                                                                              
    </script>

</asp:Content>
