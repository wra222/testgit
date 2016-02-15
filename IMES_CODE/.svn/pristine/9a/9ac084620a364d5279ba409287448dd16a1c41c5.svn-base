<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: 
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================

* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="RePrintCombinePalletWithoutCartonForFRU.aspx.cs" Inherits="PAK_RePrintCombinePalletWithoutCartonForFRU"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="Service/WebServiceCombinePalletWithoutCartonForFRU.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 150px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDeliveryNo" Text="Delivery No:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
				        <asp:Label runat="server" ID="txtDeliveryNo" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblPalletNo" Text="Pallet No:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
				        <asp:Label runat="server" ID="txtPalletNo" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				<tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txt" runat="server"  ProcessQuickInput="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" IsClear="false" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox id="txtReason" rows="5" style="width:100%;" 
                            runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                            onblur="ismaxlength(this)" cols="20" name="S1"/>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                 <tr style="height: 30px">
                    <td colspan="4" align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                   </tr>
                   <!--tr>
                    <td colspan="4" align="right">
                        <button id="btnReprint" runat="server" onclick="clkReprint()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td-->
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server">
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="hiddenbtn" runat="server" onserverclick="hiddenbtn_Click" style="display: none">
                    </button>
                    <input type="hidden" runat="server" id="station" />
                    <input type="hidden" runat="server" id="pCode" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript" language="javascript">

        var editor = "";
        var customer = "";
        var station = "";
        var pcode = "";

        var customerSN = "";

        var inputObj = "";
        var inputData = "";

        var snInput = true;

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara")%>';
        var msgInputReason = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgReasonNull") %>';
        var msgNoPrintItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrintItem")%>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
		var msgInputDeliveryNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputDeliveryNo").ToString() %>';
		var msgInputPalletNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputPalletNo").ToString() %>';
		var msgInputValidDeliveryNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputValidDeliveryNo").ToString() %>';
		var msgInputValidPalletNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputValidPalletNo").ToString() %>';
		var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

		var lstPrintItem;
		var dnsn = "";
		var palletNo = "";
        
        document.body.onload = function() {
            try {
                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                station = '<%=Request["Station"]%>';
                pcode = '<%=Request["PCode"]%>';

                inputObj = getCommonInputObject();
                inputData = inputObj.value;
                DisplayInfoText();
                initPage();

                callNextInput();

            } catch (e) {
                alert(e.description);
            }
        }

        function initPage() {
            document.getElementById("<%=txtReason.ClientID %>").value = "";
			document.getElementById("<%=txtDeliveryNo.ClientID %>").innerText = "";
			document.getElementById("<%=txtPalletNo.ClientID %>").innerText = "";
            getCommonInputObject().value = "";
            ShowInfo("");
            dnsn = "";
			palletNo = "";
        }

        function checkInput(inputData) {
            if (''==document.getElementById("<%=txtDeliveryNo.ClientID %>").innerText){
				if (inputData.length == 16){
					document.getElementById("<%=txtDeliveryNo.ClientID %>").innerText = inputData;
					ShowInfo(msgInputPalletNo);
				}
				else{
					ShowInfo(msgInputValidDeliveryNo);
				}
			}
			else if (''==document.getElementById("<%=txtPalletNo.ClientID %>").innerText){
				if (inputData.length == 10){
					document.getElementById("<%=txtPalletNo.ClientID %>").innerText = inputData;
					
					clkReprint();
				}
				else{
					ShowInfo(msgInputValidPalletNo);
				}
			}
            //document.getElementById("<%=txtReason.ClientID %>").focus();
			getCommonInputObject().value = "";
            callNextInput();
        }

        function clkReprint() {
            lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist
            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }
			
			var reason = document.getElementById("<%=txtReason.ClientID %>").value;
            dnsn = document.getElementById("<%=txtDeliveryNo.ClientID %>").innerText;
			palletNo = document.getElementById("<%=txtPalletNo.ClientID %>").innerText;

            beginWaitingCoverDiv();
            WebServiceCombinePalletWithoutCartonForFRU.RePrint(dnsn, palletNo, reason, "", editor, station, customer, lstPrintItem, onPrintSucceed, onPrintFail);       
        }

        function setPrintItemListParam(backPrintItemList, dnsn, palletNo) {
            //============================================generate PrintItem List==========================================
            //var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
			keyCollection[0] = "@DeliveryNo";
			valueCollection[0] = generateArray(dnsn);
			keyCollection[1] = "@PalletNo";
			valueCollection[1] = generateArray(palletNo);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            //setPrintParam(lstPrtItem, "Customer SN Label", keyCollection, valueCollection);
			for (var jj = 0; jj < backPrintItemList.length; jj++) {
				backPrintItemList[jj].ParameterKeys = keyCollection;
				backPrintItemList[jj].ParameterValues = valueCollection;
			}
        }

        function onPrintSucceed(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result.Success == SUCCESSRET) {
					lstPrintItem = result.PrintItem;
					setPrintItemListParam(lstPrintItem, dnsn, palletNo);
					printLabels(lstPrintItem, false);

                    var successtmp = "[" + "DN:" + dnsn + ", PalletNo:" + palletNo + "] " + msgSuccess;
					ResetPage();
					
					ShowSuccessfulInfo(true, successtmp);
					//ShowSuccessfulInfoFormat(true, "CartonSN", cartonSN); // Print 成功，带成功提示音！
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                }
            }
            catch (e) {
                alert(e.description);
            }
            getCommonInputObject().value = "";
            callNextInput();
        }

        function onPrintFail(error) {
            endWaitingCoverDiv();
            try {
                ResetPage();
                getCommonInputObject().value = "";
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
            }
        }
        
        function clkSetting() {
            showPrintSetting(station, pcode);
            callNextInput();
        }

        function ResetPage() {
            initPage();
            callNextInput();
        }

        function callNextInput() {
            //getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }

        function imposeMaxLength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            return (obj.value.length < mlength);
        }

        function ismaxlength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");    
                alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
                obj.value = obj.value.substring(0, mlength);
                reasonFocus();
            }
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
