<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/11/11 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/11/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-01   Du.Xuan               Create   
* Known issues:
* ITC-1360-0339 ITC-1360-0512 统一处理productid长度
* ITC-1360-0336 调整button位置
* ITC-1360-0337 整理错误显示
* ITC-1360-0485 调整信息显示
* ITC-1360-0505 整理错误显示
* ITC-1360-0436 uc参数定义和模板存在差异，模板调整后正常
* ITC-1360-0479 调整布局
* ITC-1360-1689 修改提示productID成功
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ContentType="text/html;Charset=UTF-8" CodeFile="GenerateCustomerSN.aspx.cs" Inherits="FA_GenerateCustomerSN"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/GenerateCustomerSNService.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div id="bg" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <center>
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td width="30%">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                         <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblProdID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="txtProdID" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" Width="99%" CanUseKeyboard="true"
                            IsClear="true" IsPaste="true" MaxLength="30" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                
            </table>
            <table width="99%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" align="right">
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
        
        var DEFAULT_ROW_NUM = 13;

        var editor = "";
        var customer = "";
        var station = "";


        var pdLine = "";
        var productID = "";
        var model = "";
        var configCode = "";
        var customerSN = "";
        var labelType = "";

        var WhetherNoCheckDDRCT = "";
        var WhetherCheckMBSno = "";

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
            document.getElementById("<%=txtProdID.ClientID %>").innerText = "";
            document.getElementById("<%=txtModel.ClientID %>").innerText = "";
            productID = "";
            model = "";
            labelType = "";
            WhetherNoCheckDDRCT = "";
            WhetherCheckMBSno = "";
            ShowInfo("");
        }

        function checkInput(inputData) {

            ShowInfo("");
            inputData = inputData.trim().toUpperCase();

            if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
                alert(msgPdLineNull);
                //ShowInfo(msgPdLineNull);
                callNextInput();
            }
            else {
                if (inputData == "" || inputData == null || inputData.length < 9) {
                    alert(msgProdIDNull);
                    //ShowInfo(msgProdIDNull);
                    callNextInput();
                }
                else {
                
                    if ('Y' == WhetherCheckMBSno)
                        InputMBSno(inputData);
                    else if ('N' == WhetherNoCheckDDRCT)
                        InputDDRCT(inputData);
                    else
                        InputProdID(inputData);
                }
            }

        }

        function InputProdID(prodID) {

            try {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                    return;
                }
            }
            catch (e) {
                alert(e);
                ResetPage();
                callNextInput();
                return;
            }
            
            beginWaitingCoverDiv();
            pdLine = getPdLineCmbValue();
            prodID = SubStringSN(prodID, "ProdId");
            GenerateCustomerSNService.inputProdId(pdLine, prodID, station, editor, customer, onInputSuccess, onInputFail);

        }
		
		function InputDDRCT(ddrct){
			beginWaitingCoverDiv();
			GenerateCustomerSNService.InputDDRCT(productID, ddrct, onInputDDRCTSuccess, onInputFail);
		}
		function InputMBSno(mbsno) {
		    beginWaitingCoverDiv();
		    GenerateCustomerSNService.InputMBSno(productID, mbsno, onInputMBSnoSuccess, onInputFail);
		}

        function onInputSuccess(result) {

            endWaitingCoverDiv();
            ShowInfo("");
            try {

                if (result[0] == SUCCESSRET) {
                    productID = result[1]["ProductID"];
                    customerSN = result[1]["CustSN"];
                    labelType = result[2];
                    document.getElementById("<%=txtProdID.ClientID%>").innerText = productID;
                    document.getElementById("<%=txtModel.ClientID%>").innerText = result[1]["Model"]; ;

                    WhetherNoCheckDDRCT = result[3];
                    WhetherCheckMBSno = result[4];
                    save();
                }

                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                    callNextInput();

                }
            }
            catch (e) {
                alert(e.description);
            }

            callNextInput();
        }
        function save() {
            if ('Y' == WhetherCheckMBSno) {
                ShowInfo('请输入 MBSno ');
                callNextInput();
                return;
            }
            if ('N' == WhetherNoCheckDDRCT) {
                ShowInfo('请输入 DDR CT');
                callNextInput();
                return;
            }
            beginWaitingCoverDiv();
            GenerateCustomerSNService.save(productID, saveSuccess, onInputFail);
         // GenerateCustomerSNService.save(productID, saveSuccess, onInputFail);
        }		
		function onInputDDRCTSuccess(result) {
			endWaitingCoverDiv();
            try {
                WhetherNoCheckDDRCT = "Y";
			    save();
			}
            catch (e) {
                alert(e.description);
            }
		}
		function onInputMBSnoSuccess(result) {
		    endWaitingCoverDiv();
		    try {
		        WhetherCheckMBSno = "N";
		        save();
		    }
		    catch (e) {
		        alert(e.description);
		    }
		}
		function saveSuccess(result) {
		    endWaitingCoverDiv();
		    try {
		     customerSN = result[0]
		     printProcess();
		     }
		    catch (e) {
		        alert(e.description);
		    }
		}
        function onInputFail(error) {
            try {
                endWaitingCoverDiv();
                ResetPage();
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }

            callNextInput();

        }

        function printProcess() {
            try {
                var printItemlist = getPrintItemCollection();
                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                }
                else {
                    beginWaitingCoverDiv();
                    GenerateCustomerSNService.Print(productID, printItemlist, onPrintSuccess, onPrintFail);
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

        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }

        function setPrintItemListParam(backPrintItemList, customerSN, labeltype) {
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

            //setPrintParam(lstPrtItem, "Customer SN Label", keyCollection, valueCollection);
            setAllPrintParam(backPrintItemList, keyCollection, valueCollection);

            /*if (pCode == "OPFA010") //Generate Customer SN(TMP)
            {
            setPrintParam(lstPrtItem, "CUSTSN_TMP", keyCollection, valueCollection);
            }
            else if (pCode == "OPFA034") //Generate Customer SN(BAT) 
            {
            setPrintParam(lstPrtItem, "CUSTSN_BAT", keyCollection, valueCollection);
            }
            */

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
                    //==========================================print process=======================================
                    var index = getTemp(result[1], labelType);
                    setPrintItemListParam(result[1], customerSN,labelType);
                    //printlist[0] = result[1][index];
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    printLabels(result[1], false);
                    //==========================================end print process===================================
                    var tmpinfo = productID;
                    ResetPage();
                    ShowSuccessfulInfoFormat(true, "Product ID", tmpinfo); // Print 成功，带成功提示音！

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


        function clkSetting() {
            //var pdLine = getPdLineCmbValue();
            //station = "58";
            //pCode = "OPFA007";
            showPrintSetting(station, pCode);
        }

        function ExitPage() {

            if (productID != "") {
                GenerateCustomerSNService.Cancel(productID, station);
                sleep(waitTimeForClear);
            }
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



    </script>

</asp:Content>
