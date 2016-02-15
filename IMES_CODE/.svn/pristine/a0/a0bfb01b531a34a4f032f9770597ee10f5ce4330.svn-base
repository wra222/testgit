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
    AutoEventWireup="true" CodeFile="RePrintCombineCartonDNfor146MB.aspx.cs" Inherits="PAK_RePrintCombineCartonDNfor146MB"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="Service/WebServiceCombineCartonDNfor146MB.asmx" />
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
                   <tr>
                    <td colspan="4" align="right">
                        <button id="btnReprint" runat="server" onclick="clkReprint()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
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

		var lstPrintItem;
		var cartonSN = "";
        
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
            getCommonInputObject().value = "";
            ShowInfo("");
            cartonSN = "";
        }

        function checkInput(inputData) {
            /*if ((inputData.length == 11) && (inputData.substring(0, 3) == "SCN")) {
                customerSN = inputData.substring(1, 11);
            }
            else if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {

                customerSN = inputData;
            }
            else {
                alert(msgInputCustSN);
                callNextInput();
                return;
            }
            */
            //document.getElementById("<%=txtReason.ClientID %>").focus();
            clkReprint();
            return;
        }

        function clkReprint() {
            var reason = document.getElementById("<%=txtReason.ClientID %>").value;
            var inputData = getCommonInputObject().value;
            
            lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
			cartonSN = "";
            WebServiceCombineCartonDNfor146MB.RePrint(inputData, reason, "", editor, station, customer, lstPrintItem, onPrintSucceed, onPrintFail);       
        }

        function setPrintItemListParam(backPrintItemList, cartonNo) {
            //============================================generate PrintItem List==========================================
            //var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@CartonSN";
            valueCollection[0] = generateArray(cartonNo);

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
                    cartonSN = result.CartonSn;
					lstPrintItem = result.PrintItem;
					setPrintItemListParam(lstPrintItem, cartonSN);
					printLabels(lstPrintItem, false);

                    ShowSuccessfulInfoFormat(true, "CartonSN", cartonSN); // Print 成功，带成功提示音！
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
