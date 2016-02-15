<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Reprint Pallet Collection
 * UI:CI-MES12-SPEC-PAK-UI Pallet Collection.docx
 * UC:CI-MES12-SPEC-PAK-UC Pallet Collection.docx         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-16  Kerwin                Create
 * Known issues:
 *           
 */
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PalletCollectionReprint.aspx.cs" Inherits="PAK_PalletCollectionReprint" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PalletCollectionWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="divPalletVerify" style="z-index: 0; width: 95%" class="iMes_div_center">
        <br />
        <table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr style="height: 50px" align="left" valign="middle">
                <td width="20%">
                    <asp:Label ID="LabelCartonNo" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td width="80%">
                    <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="20" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" TabIndex="1" />
                </td>
            </tr>
            <tr>
                <td width="20%" align="left">
                    <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="80%" align="left">
                    <textarea id="txtReason" rows="5" style="width: 98%;" runat="server" maxlength="80"
                        onkeypress="return imposeMaxLength(this)" onblur="ismaxlength(this)" tabindex="2"
                        cols="20" name="S1"></textarea>
                </td>
            </tr>
        </table>
        <br />
        <table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr style="height: 50px" align="left" valign="middle">
                <td width="50%" align="center">
                    <input id="btnPrint" type="button" onclick="Reprint()" runat="server" style="height: auto"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                        class=" iMes_button" tabindex="4" />
                </td>
                <td width="50%" align="center">
                    <input id="btnPrintSetting" style="height: auto" type="button" runat="server" onclick="showPrintSettingDialog()"
                        class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                        tabindex="3" />
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" language="javascript">

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgCartonNoNull = '<%=this.GetLocalResourceObject(Pre + "_msgCartonNoNull").ToString() %>';
        var line = "";
        var station = "";
        var inputSNControl;
        var station = '<%=Request["Station"] %>';
        var pcode = '<%=Request["PCode"] %>';
        var editor = "<%=UserId%>";
        var customer = "<%=Customer%>";

        window.onload = function() {
            inputSNControl = getCommonInputObject();
            inputSNControl.focus();
            getAvailableData("InputDataEntry");

        };

        function InputDataEntry(InputData) {
            Reprint(InputData);
        }
        function Reprint(inputStr) {
            if (!inputStr) {
                inputStr = inputSNControl.value;
            }
            ShowInfo("");
            var strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();
            var carton = inputStr;

            if (carton) {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == "" || printItemlist == null) {
                    ShowInfo(msgPrintSettingPara);
                    return;
                }

                beginWaitingCoverDiv();
                PalletCollectionWebService.Reprint(carton, strReason, line, editor, station, customer, printItemlist, onSucceed);
            }
            else {
                ShowInfo(msgCartonNoNull);
                inputSNControl.value = "";
                inputSNControl.focus();
                getAvailableData("InputDataEntry");
            }

        }

        function onSucceed(result) {
            if (result && result.length == 3) {
                ShowSuccessfulInfo(true, "PalletNo " + result[1] + msgSuccess);
                inputSNControl.value = "";
                inputSNControl.focus();
                getAvailableData("InputDataEntry");
                endWaitingCoverDiv();
                setPrintItemListParam(result[2], result[1], "RCTO_Pallet_Num_Label");
                
            } else {
                ShowErrorMessage(result[0]);
            }
            
        }

        function ismaxlength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength) {
                ShowInfo(msgInputMaxLength1 + mlength + msgInputMaxLength2);
                obj.value = obj.value.substring(0, mlength);
                document.getElementById("<%=txtReason.ClientID %>").focus();
            }
        }

        function imposeMaxLength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            return (obj.value.length < mlength);
        }

        function setPrintItemListParam(backPrintItemList, palletNo, labeltype) {
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@PalletNo";

            valueCollection[0] = generateArray(palletNo);

            setPrintParam(backPrintItemList, labeltype, keyCollection, valueCollection);

            printLabels(backPrintItemList, false);
        }


        function ClearData() {
            endWaitingCoverDiv();
            ShowInfo("");
            palletNo = "";
            inputSNControl.value = "";
            inputSNControl.focus();

            getAvailableData("InputDataEntry");

        }

        function ShowErrorMessage(result) {
            endWaitingCoverDiv();

            ShowInfo(result);
            inputSNControl.value = "";
            inputSNControl.focus();
            getAvailableData("InputDataEntry");
        }


        function showPrintSettingDialog() {
            showPrintSetting(station, pcode);
        }
     
    </script>

</asp:Content>
