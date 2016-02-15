<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13 
* UC:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-08   Du.Xuan               Create   
* ITC-1360-0771 调整资源&布局
* ITC-1360-0794 修改错误的全局文字资源指向
* ITC-1360-0806 添加不需打印配置清单提示
* ITC-1360-0808 修改PCN长度检查错误
* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PrintContentWarranty.aspx.cs" Inherits="SA_PrintContentWarranty"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePrintContentWarranty.asmx" />
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
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txt" runat="server" IsClear="true" ProcessQuickInput="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtProductID" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCustomerSN" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCustomerSN" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCustomerPN" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCustomerPN" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblWarranty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtWarranty" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfiguration" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtConfiguration" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAssetCheck" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAssetCheck" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="99%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td></td>
                    <td colspan="4" align="right">
                         <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                        <button id="btnConfReprint" runat="server" onclick="clkConfReprint()" class="iMes_button" style="width: 150px;"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                        <button id="btnWarrantyReprint" runat="server" onclick="clkWarrantyReprint()" class="iMes_button" style="width: 150px;"
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
        var editor;
        var customer;
        var login;
        var accountId;
        var station = "";
        var pCode = "";
        var productID = "";
        var customerSN = "";
        
        var assetCheck = "";
        
        var inputObj = "";
        var inputData = "";

        var configLableFlag = true;
        var WarrantyPrintFlag = true;
        var snInput = true;
        var assetflag = false;
        var flowflag = false;


        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputAssetError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputAssetError").ToString() %>';
        var msgCheckOK = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCheckOK").ToString() %>';
        var msgNeedCheck = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNeetCheck").ToString() %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgConfigPrint = '<%=this.GetLocalResourceObject(Pre + "_msgConfigPrint").ToString() %>';
        var msgNoConfigPrint = '<%=this.GetLocalResourceObject(Pre + "_msgNoConfigPrint").ToString() %>';

        var msgWarrantyPrint = '<%=this.GetLocalResourceObject(Pre + "_msgWarrantyPrint").ToString() %>';
        var msgNoWarrantyPrint = '<%=this.GetLocalResourceObject(Pre + "_msgNoWarrantyPrint").ToString() %>';
        
        document.body.onload = function() {
        
            try 
            {
                //station = document.getElementById("<%=station.ClientID %>").value.trim();
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
            document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
            document.getElementById("<%=txtCustomerSN.ClientID%>").innerText = "";
            document.getElementById("<%=txtCustomerPN.ClientID%>").innerText = "";
            document.getElementById("<%=txtWarranty.ClientID%>").innerText = "";
            document.getElementById("<%=txtConfiguration.ClientID%>").innerText = "";
            document.getElementById("<%=txtAssetCheck.ClientID%>").innerText = "";
            ShowInfo("");
            assetflag=false;
            flowflag = false;
            configLableFlag = true;
            WarrantyPrintFlag = true;
            productID = "";
            customerSN = "";
            assetCheck = "";

        }
        
        function checkInput(inputData)
        {
            snInput = true;
            inputData = inputData.trim().toUpperCase();
             
            if (inputData == "") {

                alert(msgInputCustSN);
                //ShowInfo(msgInputCustSN);
                snInput = false;
                callNextInput();
            }

            if (snInput) 
            {
                getAvailableData("checkInput");
                if (!assetflag) {
                    //1 刷入[Customer S/N]
                    inputCustomSN(inputData);
                }
                else {
                    //2 Input [Asset Tag No] – Optional
                    inputAssetCheck(inputData);
                }
            }
            else 
            {
                OnClearInput();
                callNextInput();
            }
        }
        
        
        function inputCustomSN(inputData)
        {
            if ((inputData.length == 11) && (inputData.substring(0, 3) == "PCN"))
            {
                customerSN = inputData.substring(1, 11);
            }
            else if ( (inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {

                customerSN = inputData;
            }
            else {

                alert(msgInputCustSN);
                //ShowInfo(msgInputCustSN);
                callNextInput();
                return;
            }
            beginWaitingCoverDiv();
            //station = "8D";
            //customerSN = "1010000010000001"; //"30012444Q";
            WebServicePrintContentWarranty.inputCustomerSN(customerSN, "<%=UserId%>", station, "<%=Customer%>", onSNInputSucceed, onSNInputFail);
        }

        function onSNInputSucceed(result) {

            try {
                endWaitingCoverDiv();
                var assetMessage = "";
                if (result == null) {
                    ShowInfo("");
                    ResetPage();
                    var content = msgSystemError;
                    //ShowMessage(content);
                    ShowInfo(content);
                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {

                    productID = result[1][0];
                    //assetCheck = "TEST"; //result[1][4];
                    assetCheck = result[1][4];
                    configLableFlag = result[1][5];
                    WarrantyPrintFlag = result[1][7];
                    assetMessage = result[1][6];
                    document.getElementById("<%=txtProductID.ClientID%>").innerText = productID;
                    document.getElementById("<%=txtCustomerSN.ClientID%>").innerText = customerSN;
                    document.getElementById("<%=txtCustomerPN.ClientID%>").innerText = result[1][1];
                    document.getElementById("<%=txtWarranty.ClientID%>").innerText = result[1][2];
                    document.getElementById("<%=txtConfiguration.ClientID%>").innerText = result[1][3];
                    document.getElementById("<%=txtAssetCheck.ClientID%>").innerText ="" ;
                    
                    if (assetCheck == "") {
                        printLabel();
                    }
                    else {
                        assetflag = true;
                        //ShowMessage(assetMessage);
                        ShowInfo(assetMessage);
                    }
                }
                else {
                    ShowInfo("");
                    ResetPage();
                    var content = result[0];
                    //ShowMessage(content);
                    ShowInfo(content);
                }
            } catch (e) {
                alert(e);
            }
            callNextInput();
        }

        function onSNInputFail(error) {
        
            endWaitingCoverDiv();
            try {
                ResetPage();
                //ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            } catch (e) {
                alert(e.description);
            }
            callNextInput();
        }

        function inputAssetCheck(inputData) {
            
            if ( ( inputData.length != 0) && (inputData == assetCheck )) {

                document.getElementById("<%=txtAssetCheck.ClientID%>").innerText = assetCheck;
                ShowInfo(msgCheckOK);
                printLabel();
            }
            else {
                ResetPage();
                //ShowMessage(msgInputAssetError);
                ShowInfo(msgInputAssetError);
                callNextInput(); 
            }
        }
         
         function printLabel() {

             var printItemlist = getPrintItemCollection();

             if (printItemlist == null || printItemlist == "") {
                 alert(msgPrintSettingPara);
                 ResetPage();
                 callNextInput();
             }
             else {
                beginWaitingCoverDiv();
                WebServicePrintContentWarranty.print(productID, configLableFlag, printItemlist, onPrintSucceed, onPrintFail);
                //mbsnPrint();
                //SubStringSN(inputData, "MB")
             }
        }

        function setPrintItemListParam(backPrintItemList, labelName) {

            
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

            setPrintParam(lstPrtItem, labelName, keyCollection, valueCollection);
        }

        function getTemp(result, label) {
        
            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }
        
        function onPrintSucceed(result) {

            ShowInfo("");
            endWaitingCoverDiv();
            var index = 0;
            var printlist = new Array();
            var premessage = "";
            try {
                if (result == null) {
                    //ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                if (WarrantyPrintFlag) {
                    index = getTemp(result[1],"Warranty Label");
                    setPrintItemListParam(result[1][index], "Warranty Label");
                    printlist[0] = result[1][index];
                }
                    //==========================================print process=======================================
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    //==========================================end print process===================================
                    if (configLableFlag) {
                        index = getTemp(result[1],"Content label");
                        setPrintItemListParam(result[1][index], "Content label");
                        if (WarrantyPrintFlag)
                            printlist[1] = result[1][index];
                        else
                            printlist[0] = result[1][index];
                        //printLabels(result[1], false);
                    }

                    //printLabels(printlist, false);
                    //Debug Mantis 1185
                    //---------------------------
                    printLabels(printlist, true);
                    //---------------------------
                    if (!WarrantyPrintFlag) {
                        premessage = msgNoWarrantyPrint;
                    } 
                    else {
                        premessage = msgWarrantyPrint;
                    }
                    
                    if (!configLableFlag) {
                        premessage = premessage + msgNoConfigPrint;
                    }
                    else {
                        premessage = premessage + msgConfigPrint;
                    }
                    var tmpinfo = customerSN;
                    ResetPage();
                    ShowSuccessfulInfoFormatDetail(true, premessage,"Customer SN", tmpinfo,"");
                }
                else {
                    //ShowMessage(result[0]);
                    ShowInfo(result[0]);
                }

            }
            catch (e) {
                alert(e.description);
            }

            callNextInput();
        }

        function onPrintFail(error) {

            try {
                document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
                document.getElementById("<%=txtCustomerSN.ClientID%>").innerText = "";
                document.getElementById("<%=txtCustomerPN.ClientID%>").innerText = "";
                document.getElementById("<%=txtWarranty.ClientID%>").innerText = "";
                document.getElementById("<%=txtConfiguration.ClientID%>").innerText = "";
                document.getElementById("<%=txtAssetCheck.ClientID%>").innerText = "";

                endWaitingCoverDiv();
                ShowInfo("");
                //ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
                endWaitingCoverDiv();
            }

        }

        function ResetPage() {
            ExitPage();
            initPage();
            callNextInput();
        }

        function ExitPage() {
            if (productID != "") {
                WebServicePrintContentWarranty.cancel(productID);
            }
        }

        function callNextInput() {
        
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }

        function OnClearInput() {
            getCommonInputObject().value = "";

        }
        
        function clkSetting() {
            
            //PCode = "OPPA033"
            showPrintSetting(station, pCode);
            callNextInput();
        }

        function clkConfReprint() {

            var url = "../PAK/ReprintConfigurationLabel.aspx" + "?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login;
            window.showModalDialog(url, "", 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
            callNextInput();           
        }

        function clkWarrantyReprint() {

            var url = "../PAK/ReprintWarrantyLabel.aspx" + "?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login;
            window.showModalDialog(url, "", 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
            callNextInput();
        }
        
        
    </script>

</asp:Content>
