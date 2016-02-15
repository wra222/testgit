<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/11/11 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/11/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-1-29   Du.Xuan               Create   
* ITC-1360-0842 去掉完成后reset动作
* ITC-1360-0987 根据UC调整UI
* ITC-1360-0992 去掉输入框自动清空属性
* ITC-1360-1212 结束清空输入框
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ContentType="text/html;Charset=UTF-8" CodeFile="GenerateCustomerSNReprint.aspx.cs"
    Inherits="FA_GenerateCustomerSNReprint" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/GenerateCustomerSNService.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div id="bg">
        <center>
            <table width="95%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
                <tr style="height: 20px">
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <div id="div3" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <table width="100%" border="0" style="table-layout: fixed;">
            <colgroup>
                <col style="width: 150px;" />
                <col />
                <col style="width: 80px;" />
                <col />
            </colgroup>
            <tr>
                <td width="30%">
                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td colspan="3">
                    <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" Width="99%" CanUseKeyboard="true"
                        IsPaste="true" IsClear="false" MaxLength="30" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtReason" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%"
                        IsClear="true" />
                </td>
            </tr>
          
        </table>

        <table width="100%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
            <tr style="height: 30px">
                <td colspan="4" align="right" >
                    <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                    </button>
                </td>
            </tr>
            <tr style="height: 30px">
                <td colspan="4" align="right">
                <button id="btnRePrint" runat="server" onclick="clkReprint()" class="iMes_button"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                </button>
                </td>
            </tr>
        </table>
    </div>
    <div id="div4">
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <input type="hidden" runat="server" id="station" />
                            <input type="hidden" runat="server" id="pCode" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';
        var msgProdIDNull = '<%=this.GetLocalResourceObject(Pre + "_msgProdIDNull").ToString() %>';
        var msgInvalidInput = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidInput").ToString() %>';
        var msgSFCFailed = '<%=this.GetLocalResourceObject(Pre + "_msgSFCFailed").ToString() %>';
        var msgSFCSucc = '<%=this.GetLocalResourceObject(Pre + "_msgSFCSucc").ToString() %>';
        var msgNotMatchProdIDRuler = '<%=this.GetLocalResourceObject(Pre + "_msgNotMatchProdIDRuler").ToString() %>';
        var msgWorkFlow = '<%=this.GetLocalResourceObject(Pre + "_msgWorkFlow").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgReprint = '<%=this.GetLocalResourceObject(Pre + "_msgReprint").ToString() %>';
        var msgInputReason = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgReasonNull") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        
        var DEFAULT_ROW_NUM = 13;

        var editor = "";
        var customer = "";
        var station = "";


        var pdLine = "";
        var productID = "";
        var model = "";
        var configCode = "";
        var customerSN = "";

        var reprintFlag = false;


        window.onload = function() {

            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            initPage();
            callNextInput();
        }

        function initPage() {
            
            productID = "";
            model = "";
            ShowInfo("");
            reprintFlag = false;

        }

        function checkInput(inputData) {

            /*if (inputData == "" || inputData == null || inputData.length < 9) {
                ShowInfo("");
                alert(msgProdIDNull);
                callNextInput();
                return;
            }
            productID = SubStringSN(inputData, "ProdId");
            document.getElementById("<%=txtReason.ClientID %>").focus();
            */
            clkReprint();
            return;
        }

        function clkReprint() {

            ShowInfo("");
            var reason = document.getElementById("<%=txtReason.ClientID %>").value;
            /*var proid = getCommonInputObject().value;

            productID = proid;

            if (productID == "" || productID == null || productID.length < 9) {
                alert(msgProdIDNull);
                //ShowMessage(msgInputCustSN);
                callNextInput();
                return;
            }
            
            productID = SubStringSN(productID, "ProdId");
            reprintProcess(productID);*/
            var cust = getCommonInputObject().value;

            if (cust.length == 10 && cust.substring(0, 3) == "5CG") {
                customerSN = cust;
            }
           else if (cust.length == 9) {
                 customerSN = cust;
            }
            else {
                alert(msgInputCustSN);
                callNextInput();
                return;
            }
            reprintProcess(customerSN);
        }
        
        function reprintProcess(prodID) {
            try {
                var reason = document.getElementById("<%=txtReason.ClientID %>").value;
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                }
                else {
                    beginWaitingCoverDiv();
                    GenerateCustomerSNService.Reprint(prodID, editor, station, customer, printItemlist, reason, onPrintSuccess, onPrintFail);
                }
            }
            catch (e) {
                alert(e);
                ResetPage();
                callNextInput();
            }
        }

        function generateArray(val) {
            var ret = new Array();

            ret[0] = val;

            return ret;
        }
        
        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }
        
        function setPrintItemListParam(backPrintItemList, customerSN,labeltype) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = new Array();
            lstPrtItem[0] = backPrintItemList;
            
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(customerSN);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            setPrintParam(lstPrtItem, labeltype, keyCollection, valueCollection);

        }

        function onPrintSuccess(result) {
            ShowInfo("");
            endWaitingCoverDiv();      //打印流程完成，打印的过程交给打印机
            var printlist = new Array();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                    //==========================================print process=======================================
                    customerSN = result[2]["CustSN"];
                    var index = getTemp(result[1], result[3]);
                    setPrintItemListParam(result[1][index], customerSN, result[3]);
                    printlist[0] = result[1][index];
                   
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    printLabels(printlist, false);
                    //==========================================end print process===================================
                    //ResetPage();
                    ShowSuccessfulInfoFormat(true, "CustomerSN", customerSN); // Print 成功，带成功提示音！

                }
                else {
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
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();
        }

        function clkSetting() {

            showPrintSetting(station, pCode);
            callNextInput();
        }

        function callNextInput() {

            //getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }

        function ResetPage() {
            //ExitPage();
            initPage();
        }

    </script>

</asp:Content>
