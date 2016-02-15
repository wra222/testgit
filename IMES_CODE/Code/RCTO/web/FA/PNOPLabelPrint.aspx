<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: Japanese Label Print
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-15  Chen Xu(EB1-4)       Create
 Known issues:
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PNOPLabelPrint.aspx.cs" Inherits="FA_PNOPLabelPrint" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServicePNOPLabelPrint.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table width="95%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lbPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lblDCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <%--<asp:Label ID="cmbDCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>--%>
                        <asp:DropDownList ID="cmbDCode" runat="server" Width="100%" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <asp:Label ID="lblInputModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lblMBSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <asp:Label ID="lblInputMBSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="labelprintqty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <asp:Label ID="inputprintqty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td>
                </td>
                 <td>
                </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanel2" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none">
                                </button>
                                <button id="btnGridClear" runat="server" type="button" onclick="" style="display: none">
                                </button>
                                <input id="prodHidden" type="hidden" runat="server" />
                                <input id="sumCountHidden" type="hidden" runat="server" />
                                <input type="hidden" runat="server" id="Hidden1" />
                                <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr style="height: 40px; vertical-align: middle">
                    <td align="left">
                    </td>
                    <td align="right">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnPrintSetting" runat="server" OnClientClick="clkSetting()"
                                    UseSubmitBehavior="false" TabIndex="2" onmouseover="this.className='iMes_button_onmouseover'"
                                    onmouseout="this.className='iMes_button_onmouseout'" />
                                &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="hiddenbtn" runat="server" onserverclick="hiddenbtn_Click" style="display: none">
                                </button>
                                <input type="hidden" runat="server" id="Hidden_ProdID" />
                                <input type="hidden" runat="server" id="hidModel" />
                                <input type="hidden" runat="server" id="hidInfoValue" />
                                <input type="hidden" runat="server" id="station" />
                                <input type="hidden" runat="server" id="pCode" />
                                 <input type="hidden" runat="server" id="hid_printqty" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
        <script type="text/javascript">

            var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
            var msgNoSelDCode = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectDCode").ToString() %>';
            var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
            var msgModelNull = '<%=this.GetLocalResourceObject(Pre + "_msgModelNull").ToString() %>';
            var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
            var msgModelError = '<%=this.GetLocalResourceObject(Pre + "_msgModelError").ToString() %>';
            var msgMBSNError = '<%=this.GetLocalResourceObject(Pre + "_msgMBSNError").ToString() %>';
            var strModelDisplay = "";
            var station = "";
            var inputObj = "";
            var inputData = "";
            var FistInput = true;
            var flowflag = false;
            var stationId = "";
            var pCode = "";

            window.onload = function() {
                stationId = '<%=Request["Station"] %>';
                pCode = '<%=Request["PCode"] %>';
                station = document.getElementById("<%=station.ClientID %>").value.trim();
                inputObj = getCommonInputObject();
                inputData = inputObj.value;
                getAvailableData("input");
                setPdLineCmbFocus();
                ShowInfo("");
            }

            function initFlag() {
                flowflag = false;
            }

            function clkSetting() {
                showPrintSetting(stationId, pCode);
            }

            function CheckInputModel(inputData) {
                var line = getPdLineCmbValue();
                if (line == "") {
                    alert(mesNoSelPdLine);
                    setPdLineCmbFocus();
                    getAvailableData("input");
                    return false;
                }
                var DCode = document.getElementById("<%=cmbDCode.ClientID %>").value;
                if (DCode == "") {
                    alert(msgNoSelDCode);
                    document.getElementById("<%=cmbDCode.ClientID %>").focus();
                    getAvailableData("input");
                    return false;
                }
                if (inputData.length != 12) {
                    alert(msgModelError);
                    setPdLineCmbFocus();
                    getAvailableData("input");
                    return false;
                }
//                if (inputData.substring(0, 2) != "PF"&& inputData.substring(0, 2) !="SF") {
//                    alert(msgModelError);
//                    setPdLineCmbFocus();
//                    getAvailableData("input");
//                    return false;
//                }
                return true;
            }
            
            function input(inputData) {
                if (FistInput) {
                    if (!CheckInputModel(inputData)) {
                        return;
                    }
                    
                    try {
                        WebServicePNOPLabelPrint.InputModel(inputData, onModelSuccess, onModelFail);
                    }
                    catch (e) {
                        alert(e);
                        getAvailableData("input");
                    }
                }
                else if (!checkNumber(inputData)) {
                setInputOrSpanValue(document.getElementById("<%=inputprintqty.ClientID %>"), inputData);
                document.getElementById("<%=hid_printqty.ClientID %>").value = inputData;
                getAvailableData("input");
                }
                else {
                    var line = getPdLineCmbValue();
                    if (line == "") {
                        alert(mesNoSelPdLine);
                        setPdLineCmbFocus();
                        getAvailableData("input");
                        return false;
                    }
                    var DCode = document.getElementById("<%=cmbDCode.ClientID %>").value;
                    if (DCode == "") {
                        alert(msgNoSelDCode);
                        document.getElementById("<%=cmbDCode.ClientID %>").focus();
                        getAvailableData("input");
                        return false;
                    }
                    var printqty = document.getElementById("<%=hid_printqty.ClientID %>").value;
                    if (printqty == "" || printqty>100) {
                        alert("please input PrintQty");
                        getAvailableData("input");
                        return ;
                    }
                    if (inputData.length != 10 && inputData.length != 11) {
                        alert(msgMBSNError);
                        setPdLineCmbFocus();
                        getAvailableData("input");
                        return;
                    }
                    var lstPrintItem = null;
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null) {
                        alert(msgPrintSettingPara);
                        getAvailableData("input");
                        return;
                    }
                    mbSno = SubStringSN(inputData, "MB");
                    try {
                        WebServicePNOPLabelPrint.display(mbSno,
                                                            line,
                                                            DCode,
                                                            document.getElementById("<%=hidModel.ClientID %>").value,
                                                            document.getElementById("<%=hidInfoValue.ClientID %>").value,
                                                            lstPrintItem, "<%=UserId%>",
                                                            station,
                                                            "<%=Customer%>",
                                                            onDisplaySuccess, 
                                                            onDisplayFail);
                    }
                    catch (e) {
                        alert(e);
                        getAvailableData("input");
                    }
                }
            }

            function setPrintPara() {
                var lstPrintItem = null;
                //打印部分
                
                try {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null) {
                        return null;
                    }
                    var keyCollection = new Array();
                    var valueCollection = new Array();

                    keyCollection[0] = "@ProductID";
                    valueCollection[0] = generateArray(strProdID);
                    
                    setPrintParam(lstPrintItem, "Master Label", keyCollection, valueCollection);
                }
                catch (e) {
                    alert(e.description);
                    return null;
                 }
               // lstPrintItem[0].PrintMode = 0; //bat
               // lstPrintItem[0].SpName = "FA_MasterLabe_Print.sql";
                return lstPrintItem;
            }

            function onDisplaySuccess(result) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag()
                try {
                    setInfo(result[1][2]);
                    var Pinintqty = document.getElementById("<%=hid_printqty.ClientID %>").value;
                    for (i = 0; i < Pinintqty; i++) {
                        setPrintItemListParam(result[1][3], result[1][0], result[1][1]);
                        printLabels(result[1][3], false);
                    }
                    var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
                    var MBSN = result[1][2];
                    ShowSuccessfulInfo(true, "'" + MBSN + "' " + msgSuccess1);
                    //ShowSuccessfulInfoFormat(true, "Product ID", strProdID); // Print 成功，带成功提示音！
                }
                catch (e) {
                    alert(e.description);
                }
                getAvailableData("input");
            }

            function setPrintItemListParam(backPrintItemList, Model,DCOde) //Modify By Benson at 2011/03/30
            {
                var keyCollection = new Array();
                var valueCollection = new Array();
                keyCollection[0] = "@Model";
                keyCollection[1] = "@dcode";
              
                valueCollection[0] = generateArray(Model);
                valueCollection[1] = generateArray(DCOde);
            

                for (var jj = 0; jj < backPrintItemList.length; jj++) {
                    backPrintItemList[jj].ParameterKeys = keyCollection;
                    backPrintItemList[jj].ParameterValues = valueCollection;
                }
            }
            
            function onModelSuccess(result) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag()
                try {
                    setModelInfo(result);
                    FistInput = false;
                }
                catch (e) {
                    alert(e.description);
                }
                getAvailableData("input");
            }

            function setModelInfo(info) {
                //set value to the label
                setInputOrSpanValue(document.getElementById("<%=lblInputModel.ClientID %>"), info[1]);
                document.getElementById("<%=hidModel.ClientID %>").value = info[1]
                document.getElementById("<%=hidInfoValue.ClientID %>").value = info[2]
            }
            
            function setInfo(info) {
                //set value to the label
                setInputOrSpanValue(document.getElementById("<%=lblInputMBSN.ClientID %>"), info);
            }
            
            function setnull() {
                //set value to the label
                setInputOrSpanValue(document.getElementById("<%=lblInputModel.ClientID %>"), "");
            }       

            function onDisplayFail(error) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag();
                try {
                    //alert(error.get_message());
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());

                }
                catch (e) {
                    alert(e.description);
                }

                OnInputClearClick();
                getAvailableData("input");
            }

            function onModelFail(error) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag();
                try {
                    //alert(error.get_message());
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());

                }
                catch (e) {
                    alert(e.description);
                }
                getAvailableData("input");
            }

            function EnterOrTab(cusnData) {
                if (flowflag) {
                    var inputContent = cusnData.value;

                    if (event.keyCode == 9 || event.keyCode == 13) {
                        inputContent = inputContent.toUpperCase();

                        var pattern = /[^0-9a-zA-Z]?$/;

                        if (pattern.test(inputContent)) {
                            checkCustSn();
                            event.returnValue = false;
                        }
                        else {
                            alert(msgInvalidCustSN);
                            event.returnValue = false;
                        }
                    }
                }
                else {
                    alert(msgProdIDNull);
                    setInputFocus();
                    event.returnValue = false;
                }
            }

            function generateArray(val) {
                var ret = new Array();
                ret[0] = val;
                return ret;
            }

            function OnClearAll() {
                getCommonInputObject().value = "";
                inputData = "";
                strProdID = "";
                strModelDisplay = "";

                setInputFocus();
            }

            function OnInputClearClick() {
                getCommonInputObject().value = "";
                inputData = "";
                strProdID = "";
                strModelDisplay = "";
                setInputFocus();
            }

            function setInputFocus() {
                getCommonInputObject().focus();
            }

            function showPrintSettingDialog() {
                showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
            }

            window.onbeforeunload = function() {
                ExitPage();
            }

            function ExitPage() {
                if (flowflag) {
                    WebServicePNOPLabelPrint.Cancel(strProdID, onClearSucceeded, onClearFailed);
                    sleep(waitTimeForClear);
                }
            }

            function onClearSucceeded(result) {
                ShowInfo("");
                try {
                    if (result == null) {
                        ShowMessage(msgSystemError);
                        ShowInfo(msgSystemError);
                    }
                    else if (result == SUCCESSRET) {
                    }
                    else {
                        ShowMessage(result);
                        ShowInfo(result);
                    }
                }
                catch (e) {
                    alert(e.description);
                }
                OnClearAll();
            }

            function onClearFailed(error) {
                ShowInfo("");
                try {
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());
                }
                catch (e) {
                    alert(e.description);
                }

                OnClearAll();
            }

            function ResetPage() {
                ExitPage();
                OnClearAll();
                ShowInfo("");
                document.getElementById("<%=hiddenbtn.ClientID %>").click();
            }
            function checkNumber(value) {
                var errorFlag = false;
                try {
                    var pattern = /^([1-9][0-9]?|100)$/;


                    if (pattern.test(value)) {
                        errorFlag = false;
                    }
                    else {
                        errorFlag = true;
                    }
                    return errorFlag;
                }
                catch (e) {
                    alert(e.description);
                }
            }

        </script>
</asp:Content>
