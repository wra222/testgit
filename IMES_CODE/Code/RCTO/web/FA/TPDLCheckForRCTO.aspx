<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UI RCTO TPDL Check.docx 
* UC:CI-MES12-SPEC-FA-UC RCTO TPDL Check.docx             
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-06   Du.Xuan               Create   
* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="TPDLCheckForRCTO.aspx.cs" Inherits="FA_TPDLCheckForRCTO"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceTPDLCheckForRCTO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 120px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblLCMCT" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtLCMCT" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTPDLCT" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtTPDLCT" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txt" runat="server" IsClear="true" ProcessQuickInput="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
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
        var editor;
        var customer;
        var login;
        var accountId;
        var station = "";
        var pCode = "";
        var productID = "";

        var inputObj = "";
        var inputData = "";

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgInputLCM = '<%=this.GetLocalResourceObject(Pre + "_msgInputLCM").ToString() %>';
        var msgCheckPass = '<%=this.GetLocalResourceObject(Pre + "_msgCheckPass").ToString() %>';
           
        document.body.onload = function() {

            try {
                station = '<%=Request["Station"] %>';
                pCode = '<%=Request["PCode"] %>';

                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                accountId = '<%=Request["AccountId"] %>';
                login = '<%=Request["Login"] %>';

                inputObj = getCommonInputObject();
                inputData = inputObj.value;
                initPage();
                callNextInput();

            } catch (e) {
                alert(e.description);
            }
        }
        window.onbeforeunload = function() {

            ExitPage();

        };

        function initPage() {

            document.getElementById("<%=txtLCMCT.ClientID%>").innerText = "";
            document.getElementById("<%=txtTPDLCT.ClientID%>").innerText = "";
            ShowInfo("");
            productID = "";
        }

        function processDataEntry(inputData) {
        
            inputData = inputData.trim().toUpperCase();
            if (productID == "") {
                if (inputData.length == 10 || inputData.length == 14) {

                    if (inputData == 10) {
                        inputData = SubStringSN(inputData, "ProdId");
                    }
                    inputProductID(inputData);
                }
                else {
                    alert(msgInputLCM);
                    callNextInput();
                    return;
                }
            }
            else {
                if (inputData.length == 15) {
                    beginWaitingCoverDiv();
                    WebServiceTPDLCheckForRCTO.Save(productID, inputData, onSaveInputSucceed, onInputFail);
                }
                else{
                    alert(msgInputCustSN);
                    callNextInput();
                }
            }

        }

        function inputProductID(prodID) {
            beginWaitingCoverDiv();
            WebServiceTPDLCheckForRCTO.inputProductID(prodID, "<%=UserId%>", station, "<%=Customer%>", onIDInputSucceed, onInputFail);
        }

        function onIDInputSucceed(result) {
            try {
                endWaitingCoverDiv();
                if (result[0] == SUCCESSRET) {

                    productID = result[1];
                    document.getElementById("<%=txtLCMCT.ClientID%>").innerText = result[2];
                    document.getElementById("<%=txtTPDLCT.ClientID%>").innerText = result[3];
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                }
            } catch (e) {
                alert(e);
            }
            callNextInput();
        }

        function onInputFail(error) {
            endWaitingCoverDiv();
            try {
                ResetPage();
                ShowInfo(error.get_message());
            } catch (e) {
                alert(e.description);
            }
            callNextInput();
        }
        
        function  onSaveInputSucceed(){
           try {
                endWaitingCoverDiv();
                if (result[0] == SUCCESSRET) {
                    var tmpinfo = productID;
                    ResetPage();
                    //ShowSuccessfulInfoFormat(true, "Product ID", tmpinfo); // Print 成功，带成功提示音！
                    ShowSuccessfulInfo(true, "["+productID+"]"+ msgCheckPass);
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                }
            } catch (e) {
                alert(e);
            }
            callNextInput();
            
        }
        
        function ResetPage() {
            ExitPage();
            initPage();
        }

        function ExitPage() {
            if (productID != "") {
                WebServiceTPDLCheckForRCTO.cancel(productID);
            }
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }

        function OnClearInput() {
            getCommonInputObject().value = "";

        }

        
    </script>

</asp:Content>
