<%--
/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:UI for Generate Customer SN For Docking Reprint Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-18   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1414-0064,Jessica Liu,2012-6-5
* ITC-1414-0193, Jessica Liu, 2012-6-20
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ContentType="text/html;Charset=UTF-8" CodeFile="GenerateCustomerSNForDockingReprint.aspx.cs"
    Inherits="FA_GenerateCustomerSNForDockingReprint" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/GenerateCustomerSNForDockingService.asmx" />
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
            <tr style="display: none">
                <td></td>
                <td align="left" >
                    <input type="radio" id="radDock12" name="radAll" runat="server" checked onclick="" />
                    <asp:Label ID="lblDock12" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    <input type="radio" id="radDock09" name="radAll" runat="server" onclick="" />
                    <asp:Label ID="lblDock09" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <input type="radio" id="radSSeries" name="radAll" runat="server" onclick="" />
                    <asp:Label ID="lblSSeries" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
            var proid = getCommonInputObject().value;

            productID = proid;

            if (productID == "" || productID == null || productID.length < 9) {
                alert(msgProdIDNull);
                //ShowMessage(msgInputCustSN);
                callNextInput();
                return;
            }
            /*if (reason == "") {
                alert(msgInputReason);
                //ShowMessage(msgInputReason);
                callNextInput();
                return;
            }
            */
            
            /*2012-6-20
            productID = SubStringSN(productID, "ProdId");
            reprintProcess(productID);
            */
            var dataEntry = productID;
            if (dataEntry.length == 10) {
                //if (dataEntry.substring(0, 3) != "CNU") //prodid
				if (!CheckCustomerSN(dataEntry))
                {
                    dataEntry = SubStringSN(dataEntry, "ProdId");
                }
            }
            reprintProcess(dataEntry);
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
                    /*2012-6-20
                    prodID = SubStringSN(prodID, "ProdId");
                    //prodID = "101000001"; //"103000034";
                    //Reprint(string prodId, string editor, string stationid, string customer, IList<PrintItem> printItems, string reason)
                    GenerateCustomerSNForDockingService.Reprint(prodID, editor, station, customer, printItemlist, reason, onPrintSuccess, onPrintFail);
                    */
                    var dataEntry = prodID;
                    if (dataEntry.length == 10) {
                        //if (dataEntry.substring(0, 3) != "CNU") //prodid
						if (!CheckCustomerSN(dataEntry))
                        {
                            dataEntry = SubStringSN(dataEntry, "ProdId");
                        }
                    }
                    GenerateCustomerSNForDockingService.Reprint(dataEntry, editor, station, customer, printItemlist, reason, onPrintSuccess, onPrintFail);
                }
            }
            catch (e) {
                alert(e);
            }
        }

        function generateArray(val) {
            var ret = new Array();

            ret[0] = val;

            return ret;
        }

        function setPrintItemListParam(backPrintItemList, customerSN) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(customerSN);
            setAllPrintParam(lstPrtItem, keyCollection, valueCollection);
            printLabels(lstPrtItem, false);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */
            /* 2012-6-21
            //ITC-1414-0064,Jessica Liu,2012-6-5
            setPrintParam(lstPrtItem, "DK_SN_Label", keyCollection, valueCollection);
            */
            /* 2012-6-29, Mantis bug-1031
            if (document.getElementById("<%=radDock12.ClientID %>").checked) 
            {
            setPrintParam(lstPrtItem, "DK_SN_Label", keyCollection, valueCollection);
            }
            else if (document.getElementById("<%=radDock09.ClientID %>").checked) {
            setPrintParam(lstPrtItem, "DK_SN_Label_1", keyCollection, valueCollection);
            }
            else if (document.getElementById("<%=radSSeries.ClientID %>").checked) {
            setPrintParam(lstPrtItem, "DK_SN_Label_2", keyCollection, valueCollection);
            }
            */
            /*
            var labelCollection = [];
            if (document.getElementById("<%=radDock12.ClientID %>").checked) {
                for (var i = 0; i < lstPrtItem.length; i++) {
                    if (lstPrtItem[i].LabelType == "DK_SN_Label") {
                        labelCollection.push(lstPrtItem[i]);
                        break;
                    }
                }
                setPrintParam(labelCollection, "DK_SN_Label", keyCollection, valueCollection);
            }
            else if (document.getElementById("<%=radDock09.ClientID %>").checked) {
                for (var i = 0; i < lstPrtItem.length; i++) {
                    if (lstPrtItem[i].LabelType == "DK_SN_Label_1") {
                        labelCollection.push(lstPrtItem[i]);
                        break;
                    }
                }
                setPrintParam(labelCollection, "DK_SN_Label_1", keyCollection, valueCollection);
            }
            else if (document.getElementById("<%=radSSeries.ClientID %>").checked) {
                for (var i = 0; i < lstPrtItem.length; i++) {
                    if (lstPrtItem[i].LabelType == "DK_SN_Label_2") {
                        labelCollection.push(lstPrtItem[i]);
                        break;
                    }
                }
                setPrintParam(labelCollection, "DK_SN_Label_2", keyCollection, valueCollection);
            }
            
            printLabels(labelCollection, false);
            */
            
        }

        function onPrintSuccess(result) {
            ShowInfo("");

            endWaitingCoverDiv();      //打印流程完成，打印的过程交给打印机

            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                    //==========================================print process=======================================
                    customerSN = result[2]["CustSN"];
                    setPrintItemListParam(result[1], customerSN);
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    //2012-6-29, Mantis bug-1031
                    //printLabels(result[1], false);
                    //==========================================end print process===================================
                    //ResetPage();
                    ShowSuccessfulInfoFormat(true, "Product ID", productID); // Print 成功，带成功提示音！

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
