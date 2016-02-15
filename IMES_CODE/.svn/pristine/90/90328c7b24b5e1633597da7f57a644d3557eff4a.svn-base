
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineCartonandDNfor146_CommonParts.aspx.cs" Inherits="PAK_CombineCartonandDNfor146_CommonParts"
    Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <bgsound src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceCombineCartonandDNfor146_CommonParts.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="TitleDelivery" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td style="width:10%">
                            <asp:Label runat="server" ID="lblPdLine" CssClass="iMes_label_13pt">PdLine:</asp:Label>
                        </td>
                        <td style="width:90%">
                            <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="300" IsPercentage="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblModel" CssClass="iMes_label_13pt">Model:</asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" >
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="cmbModel" Width="100%"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnupdateselect" EventName="ServerClick" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDelivery" CssClass="iMes_label_13pt">Delivery:</asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" >
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="cmbDelivery" Width="100%"></asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                   </table>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelDn" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" border="0" style="table-layout: fixed;">
                                <tr >
                                    <td>
                                        <asp:Label runat="server" ID="lblTotalQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblPackedQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPackedQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblPCSQty" Text="" CssClass="iMes_label_13pt">PCS in Carton:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPCSQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

            </fieldset>
            <hr />
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
                            <input id="btnReprint" type="button" runat="server" class="iMes_button" onclick="reprint()" />
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
                <input id="deliverymodelHidden" type="hidden" runat="server" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <button id="btnupdateselect" runat="server" onserverclick="btnReflishSelect_Click" style="display:none"></button>
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var customer = "";
        var stationId = "";
        var pCode = "";
        var inputObj;
        var emptyPattern = /^\s*$/;

        var totalQty = 0;
        var remainQty = 0;
        var PCSinCarton = 0;
        var model = "";
        var deliveryNo = "";
        var paramArr;


        //error message
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgInputModel = '<%=this.GetLocalResourceObject(Pre + "_msgInputModel").ToString() %>';
        var msgValidModel = '<%=this.GetLocalResourceObject(Pre + "_msgValidModel").ToString() %>';
        var msgOverTotal = '<%=this.GetLocalResourceObject(Pre + "_msgOverTotal").ToString() %>';
        var msgCartonNotFull = '<%=this.GetLocalResourceObject(Pre + "_msgCartonNotFull").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';
        var msgPcsError = '<%=this.GetLocalResourceObject(Pre + "_msgPcsError").ToString() %>';
        var msgOverPCs = '<%=this.GetLocalResourceObject(Pre + "_msgOverPCs").ToString() %>';
        var msgInputDev = '<%=this.GetLocalResourceObject(Pre + "_msgInputDev").ToString() %>';
        var msgNullDev = '<%=this.GetLocalResourceObject(Pre + "_msgNullDev").ToString() %>';
        var msgOverQty = '<%=this.GetLocalResourceObject(Pre + "_msgOverQty").ToString() %>';
        var msgDeliveryOK = '<%=this.GetLocalResourceObject(Pre + "_msgDeliveryOK").ToString() %>';

        var needReset = false;

        window.onload = function() {
            inputObj = getCommonInputObject();
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            paramArr = getUrlVars();
            initPage();
            getAvailableData("input");
           
        };

        function initPage() {
            setInputOrSpanValue(document.getElementById("<%=lblTotalQtyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), "");
            getPdLineCmbObj().disabled = false;
            document.getElementById("<%=cmbModel.ClientID %>").disabled = false;
            document.getElementById("<%=cmbDelivery.ClientID %>").disabled = false;
            model = "";
            deliveryNo = "";
            totalQty = 0;
            remainQty = 0;
            PCSinCarton = 0;
        }

        function setpagevalue(qty, remainQty) {
            setInputOrSpanValue(document.getElementById("<%=lblTotalQtyContent.ClientID %>"), qty);
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), remainQty);
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

        function input(inputData) {

            if (inputData == "7777") {
                ResetPage();
                callNextInput();
                return;
            }
            if (getPdLineCmbValue() == "") {
                alert("Please Select PdLine");
                setPdLineCmbFocus();
                callNextInput();
                return;
            }
            model = document.getElementById("<%=cmbModel.ClientID %>").value;
            if (model == "") {
                alert("Please Select Model");
                document.getElementById("<%=cmbModel.ClientID %>").focus();
                callNextInput();
                return;
            }
            deliveryNo = document.getElementById("<%=cmbDelivery.ClientID %>").options[document.getElementById("<%=cmbDelivery.ClientID %>").selectedIndex].text
            if (deliveryNo == "") {
                alert("Please Select Delivery");
                document.getElementById("<%=cmbDelivery.ClientID %>").focus();
                callNextInput();
                return;
            }

            if (isNaN(inputData)) {
                alert("Please input int");
                callNextInput();
                return;
            }
            totalQty = document.getElementById("<%=lblTotalQtyContent.ClientID %>").innerHTML;
            remainQty = document.getElementById("<%=lblPackedQtyContent.ClientID %>").innerHTML;
            if (parseInt(inputData) > parseInt(remainQty)) {
                alert("inputData can't over " + remainQty);
                callNextInput();
                return;
            }
            inputQty(inputData);
        }

        function inputQty(inputData) {
            ShowInfo("");
            try {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    callNextInput();
                }
                else {
                    beginWaitingCoverDiv();
                    WebServiceCombineCartonandDNfor146_CommonParts.InputPCSinCarton(getPdLineCmbValue(),inputData,deliveryNo,model,editor,stationId,customer,printItemlist, onInputQtySucc, onFail);
                }
            }
            catch (e) {
                alert(e);
            }
        }

        function onInputQtySucc(result) {
        
            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                setInfo(result);
                
                callNextInput();
            }
            else {
                ShowInfo("");
                var content = result[0];
                ResetPage();
                ShowMessage(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onFail(result) {
            endWaitingCoverDiv();
            //ResetPage();        
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function setInfo(info) {
            CartonSN = info[1];
            remainQty = info[2];
            PCSinCarton = info[3];
            document.getElementById("<%=lblPackedQtyContent.ClientID %>").innerHTML = remainQty;
            document.getElementById("<%=lblPCSQtyContent.ClientID %>").innerHTML = PCSinCarton;
            

            if (remainQty == 0) {
                getPdLineCmbObj().disabled = false;
                document.getElementById("<%=cmbModel.ClientID %>").disabled = false;
                document.getElementById("<%=cmbDelivery.ClientID %>").disabled = false;
                document.getElementById("<%=btnupdateselect.ClientID %>").click();
                
                initPage();
            }
            else {
                getPdLineCmbObj().disabled = true;
                document.getElementById("<%=cmbModel.ClientID %>").disabled = true;
                document.getElementById("<%=cmbDelivery.ClientID %>").disabled = true;    
            }
            setPrintItemListParam(info[4], model, CartonSN, PCSinCarton);
            printLabels(info[4], false);
            ShowInfo("SUCCESS! Carton SN: " + CartonSN, "green");
            return;
        }

        function setPrintItemListParam(backPrintItemList, model, cartonNo, PCSinCarton) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@Model";
            valueCollection[0] = generateArray(model);
            keyCollection[1] = "@CartonSN";
            valueCollection[1] = generateArray(cartonNo);
            keyCollection[2] = "@Qty";
            valueCollection[2] = generateArray(parseInt(PCSinCarton));

            for (var jj = 0; jj < backPrintItemList.length; jj++) {
                backPrintItemList[jj].ParameterKeys = keyCollection;
                backPrintItemList[jj].ParameterValues = valueCollection;
            }
            //setPrintParam(lstPrtItem, "Customer SN Label", keyCollection, valueCollection);
            //setPrintParam(lstPrtItem, "RCTO_Carton_Label", keyCollection, valueCollection);
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

        function clkSetting() {
            showPrintSetting(stationId, pCode);
        }

        function ClearScreen() {
            initPage();
            ShowInfo("");
            callNextInput();
            return true;
        }
        function reprint() {
            var url = "../PAK/ReprintCombineCartonandDNfor146_CommonParts.aspx?Station=" + stationId + "&PCode=" + paramArr["PCode"] + "&UserId=" + paramArr["UserId"] + "&Customer=" + paramArr["Customer"] + "&AccountId=" + paramArr["AccountId"] + "&Login=" + paramArr["UserId"];
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = paramArr["UserId"];
            paramArray[2] = paramArr["Customer"];
            paramArray[3] = paramArr["Station"]; ;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
    </script>

</asp:Content>
