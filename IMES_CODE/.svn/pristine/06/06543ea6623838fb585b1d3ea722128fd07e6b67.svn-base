<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/10/13 Warrant
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/10/13            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-01-17   Du.Xuan               Create   
* ITC-1360-0679 修改焦点设置问题
* ITC-1360-0681 reason长度限制
* ITC-1360-0682 修改messge全局资源调用错误
* ITC-1360-0683 修改检查逻辑
* ITC-1360-0684 custSN不存在提示本版已经没问题
* ITC-1360-0685 修改SN验证逻辑错误
* ITC-1360-0686 修改打印参数错误
* ITC-1360-1188 完成后清空输入
* ITC-1360-1614 Reprint 不再要求Reason 必须输入，刷入相关数据后，可以直接Reprint，不需要按Print按钮
* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ReprintPalletWeight.aspx.cs" Inherits="SA_PrintContentWarranty"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PalletWeightWebService.asmx" />
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
            customerSN = "";
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
            var cust = getCommonInputObject().value;
            
           if (cust.length == 10 && cust.substring(0, 2) == "CN") {
                customerSN = cust;
            }
            else if (cust.length == 11 && cust.substring(0, 3) == "SCN" ) {
                customerSN = cust.substring(1, 11);
            }
            else {
                alert(msgInputCustSN);
                //ShowMessage(msgInputCustSN);
                callNextInput();
                return;
            }
            
            /*if (reason == "") {
                alert(msgInputReason);
                callNextInput();
                //ShowMessage(msgInputReason);
                return;
            }*/
            var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            PalletWeightWebService.ReprintPalletWeightLabel(customerSN, reason, "", editor, station, customer,lstPrintItem, onPrintSucceed, onPrintFail);       
            
        }

        function setPrintItemListParam(backPrintItemList, palletNo, weight) {

            // @prdid --Product ID
            // @mac --MAC Address   reprint的sp不用Mac
            // @model --机型12码
            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@palletno";
            keyCollection[1] = "@weight";

            valueCollection[0] = generateArray(palletNo);
            valueCollection[1] = generateArray(weight);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            setPrintParam(lstPrtItem, "Weight Label", keyCollection, valueCollection);
        }

        function onPrintSucceed(result) {

            ShowInfo("");
            endWaitingCoverDiv();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {

                    setPrintItemListParam(result[1], result[2], result[3]); // 使用Reprint的存储过程

                    //==========================================print process=======================================

                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */

                    printLabels(result[1], false);

                    //==========================================end print process===================================
                    //ResetPage();
                    ShowSuccessfulInfoFormat(true, "Customer SN", customerSN); // Print 成功，带成功提示音！
                }
                else {
                    ResetPage();
                    ShowMessage(result[0]);
                    ShowInfo(result[0]);
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
     
    </script>

</asp:Content>
