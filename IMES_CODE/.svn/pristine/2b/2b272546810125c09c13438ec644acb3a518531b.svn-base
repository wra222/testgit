<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UI RCTO CT Label Print.docx –2012-09-11
* UC:CI-MES12-SPEC-FA-UC RCTO CT Label Print.docx –2012-09-11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-09-11   Du.Xuan               Create   
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ContentType="text/html;Charset=UTF-8" CodeFile="RCTOCTLabelPrint.aspx.cs" Inherits="FA_GenerateCustomerSN"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceRCTOCTLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div id="bg" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <center>
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 120px;" />
                    <col />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblCTNO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">                      
                        <asp:Label ID="txtCTNO" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  style="width: 40%">
                        <asp:TextBox ID="txtQty" MaxLength="3" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%" onkeypress='OnKeyPress(this)'/>
                    </td>
                    <td>
                        <asp:Label ID="lblQtyNote" runat="server" CssClass="iMes_label_15pt_Red" Text ="(1-100)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="2">
                        <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" Width="99%" CanUseKeyboard="true"
                            IsClear="true" IsPaste="true" MaxLength="30" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                
            </table>
            <table width="99%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" align="right">
                        <button id="btnPrint" runat="server" onclick="clkPrint()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <div id="div4">
        <table>
            <tr>
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
        var msgCtnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgCtnoNull").ToString() %>';
        var msgQtyNull = '<%=this.GetLocalResourceObject(Pre + "_msgQtyNull").ToString() %>';
        var msgQtyFormat = '<%=this.GetLocalResourceObject(Pre + "_msgQtyFormat").ToString() %>';
        var msgErrCTNO = '<%=this.GetLocalResourceObject(Pre + "_msgErrCTNO").ToString() %>';
        
        var msgInvalidInput = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidInput").ToString() %>';
        var msgSFCFailed = '<%=this.GetLocalResourceObject(Pre + "_msgSFCFailed").ToString() %>';
        var msgSFCSucc = '<%=this.GetLocalResourceObject(Pre + "_msgSFCSucc").ToString() %>';
        var msgNotMatchProdIDRuler = '<%=this.GetLocalResourceObject(Pre + "_msgNotMatchProdIDRuler").ToString() %>';
        var msgWorkFlow = '<%=this.GetLocalResourceObject(Pre + "_msgWorkFlow").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';      
        
        var DEFAULT_ROW_NUM = 13;

        var editor = "";
        var customer = "";
        var station = "";

        
        var configCode = "";
        var labelType = "";

        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            initPage();
            callNextInput();
        }

        window.onbeforeunload = function() {
            ExitPage();
        }

        function initPage() {
            document.getElementById("<%=txtCTNO.ClientID %>").innerText = "";
            document.getElementById("<%=txtQty.ClientID %>").innerText = "";
            ctno = "";
            model = "";
            labelType = "";
            ShowInfo("");
        }

        function checkQty(inputData)
        {   
            if (inputData.length>0 && inputData.length <= 3) {
                var inputQty = parseInt(inputData);
                if (inputQty >= 1 && inputQty <= 100 && inputQty.toString().length == inputData.length) {
                    return true;
                }
            }
            return false;
            
        }

        function checkInput(inputData) {

            ShowInfo("");
            inputData = inputData.trim().toUpperCase();

            /*if (inputData.length>0 && inputData.length <= 3) {
                var inputQty = parseInt(inputData);
                if (inputQty >= 1 && inputQty <= 100 && inputQty.toString().length == inputData.length) {
                    document.getElementById("<%=txtQty.ClientID %>").innerText = inputData;
                }
                else {
                    alert(msgInvalidInput);    
                }
            }*/
            if (checkQty(inputData))
            {
                document.getElementById("<%=txtQty.ClientID %>").innerText = inputData;    
            }
            else if (inputData.length == 15) {

                WebServiceRCTOCTLabelPrint.checkCTNO(ctno, onCheckCtno);
                return;
            }
            else {
                alert(msgInvalidInput);
            }
            callNextInput();
        }

        function onCheckCtno(result) {
            
            if (result[0] == SUCCESSRET) {
                if (result[1]=="")
                {
                    alert(msgErrCTNO);
                }
                else
                {
                    document.getElementById("<%=txtCTNO.ClientID %>").innerText = result[1];
                }
            }
            callNextInput();
        }
        
        function printProcess() {
            try {
                var printItemlist = getPrintItemCollection();
                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);                    
                }
                else {

                    var ctno = document.getElementById("<%=txtCTNO.ClientID %>").innerText;
                    var qty = document.getElementById("<%=txtQty.ClientID %>").innerText;

                    if (ctno.length != 15) {
                        alert(msgCtnoNull);
                    }
                    else if (!check(qty)) {
                        alert(msgQtyNull);
                    }
                    else {
                        beginWaitingCoverDiv();
                        WebServiceRCTOCTLabelPrint.inputCTNO(ctno,qty, printItemlist,"",editor,station,customer, onPrintSuccess, onPrintFail);
                    }                                     
                }
            }
            catch (e) {
                alert(e);
            }
            callNextInput();
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

        function setPrintItemListParam(backPrintItemList, ctno) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = new Array();
            lstPrtItem[0] = backPrintItemList;
            
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@CTNo";
            valueCollection[0] = ctno;

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            setPrintParam(lstPrtItem, "RCTO_CT_Label", keyCollection, valueCollection);

        }

        function onPrintSuccess(result) {

            endWaitingCoverDiv();      //打印流程完成，打印的过程交给打印机
            ShowInfo("");
            var printlist = new Array();
            
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                    var ctno = result[1];
                    var qty = result[2];
                    var ctnoList = result[4];
                    //==========================================print process=======================================
                    for (var i = 0; i < ctnoList.length; i++) {
                        setPrintItemListParam(result[3][0], ctnoList[i]);
                        printlist[i] = result[1][0];
                    }
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    printLabels(printlist, false);
                    //==========================================end print process===================================
                    var tmpinfo = ctno;
                    ResetPage();
                    ShowSuccessfulInfoFormat(true, "CTNO", ctno); // Print 成功，带成功提示音！
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

        function clkPrint() {
            printProcess();
        }

        function clkSetting() {
            showPrintSetting(station, pCode);
        }
        
        function ExitPage() {
        }


        function ResetPage() {
            ExitPage();
            initPage();
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }

        function OnKeyPress(obj) {
            var key = event.keyCode;

            if (key == 13 || key == 9)//enter || tab
            {
                if (event.srcElement.id == "<%=txtQty.ClientID %>") {
                    if (check() == false) {
                        //Input data format is not correct
                        alert(msgQtyFormat);
                        return;
                    }
                    callNextInput();
                }
            }
        }

        function check() {
            var txtqty = document.getElementById("<%=txtQty.ClientID %>").value;
            //则检查本框输入内容是否满足格式要求
            var reExp = /^[0-9]+(\.[0-9]?[0-9]?[0-9]?)?$/;
            if (reExp.exec(txtqty) == null || reExp.exec(txtqty) == false) {
                return false;
            }

            var qty = parseFloat(txtqty);
            if (qty > 100 || qty < 1) {
                return false;
            }

            return true;
        }

    </script>

</asp:Content>
