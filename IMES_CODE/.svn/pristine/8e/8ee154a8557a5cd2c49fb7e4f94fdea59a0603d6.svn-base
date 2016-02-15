<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/07    ITC000052             Create   
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineCartonInDNForRCTO.aspx.cs" Inherits="PAK_CombineCartonInDNForRCTO"
    Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <bgsound src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceCombineCartonInDNForRCTO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="TitleDelivery" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDelivery" Text="" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:CmbDeliveryByCarton ID="cmbDelivery" runat="server" ProcessQuickInput="true"
                                        CanUseKeyboard="true" Width="99%" IsClear="true" ReadOnly="True" />
                                    <button id="btnGetDn" runat="server" onserverclick="btnGetDn_Click" style="display: none" ></button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                   </table>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelDn" UpdateMode="Conditional">
                                <ContentTemplate>
                    <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTotalQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTotalQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPackedQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPackedQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                 </table>
                    <button id="btnResetDnInfo" runat="server" onserverclick="btnResetDnInfo_Click" style="display: none" ></button>
                    </ContentTemplate>
                            </asp:UpdatePanel>

            </fieldset>
            <hr />
              <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                        <td align="right" style="width: 110px;">
                            <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <input id="modelHidden" type="hidden" runat="server" />
                <input id="dnListHidden" type="hidden" runat="server" />
                <input id="dnQtyHidden" type="hidden" runat="server" />
                <input id="dnPcsHidden" type="hidden" runat="server" />
                <input id="dnPackedHidden" type="hidden" runat="server" />
                <input id="deliverymodelHidden" type="hidden" runat="server" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var customer = "";
        var stationId = "";
        var pCode = "";

        var inputObj;
        var emptyPattern = /^\s*$/;

        var TotalQty = 0;
        var PackedQty = 0;
        var CartonQty = 0;

        var customerSN = "";
        var productID = "";
        var model = "";
        var deliveryNo = "";
        var firstProID = "";


        //error message
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgInputModel = '<%=this.GetLocalResourceObject(Pre + "_msgInputModel").ToString() %>';
        var msgValidModel = '<%=this.GetLocalResourceObject(Pre + "_msgValidModel").ToString() %>';
        var msgOverTotal = '<%=this.GetLocalResourceObject(Pre + "_msgOverTotal").ToString() %>';
        var msgCartonNotFull = '<%=this.GetLocalResourceObject(Pre + "_msgCartonNotFull").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';
        var msgPcsError = '<%=this.GetLocalResourceObject(Pre + "_msgPcsError").ToString() %>';
        var msgOverPCs = '<%=this.GetLocalResourceObject(Pre + "_msgOverPCs").ToString() %>';
        var msgInputDev = '<%=this.GetLocalResourceObject(Pre + "_msgInputDev").ToString() %>';
        var msgNullDev = '<%=this.GetLocalResourceObject(Pre + "_msgNullDev").ToString() %>';
        var msgOverQty = '<%=this.GetLocalResourceObject(Pre + "_msgOverQty").ToString() %>';
        var msgDeliveryOK = '<%=this.GetLocalResourceObject(Pre + "_msgDeliveryOK").ToString() %>';

        var needReset = false;

        window.onload = function() {
            inputObj = getCommonInputObject();

            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';

            initPage();
            getAvailableData("input");          
        };

        window.onbeforeunload = function() {

            OnCancel();
        };

        function initPage() {

            reworkCache = [];
            //set Delivery
            setInputOrSpanValue(document.getElementById("<%=lblTotalQtyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");

            customerSN = "";
            productID = "";
            model = "";
            deliveryNo = "";
            firstProID = "";
            needReset = false;

            PackedQty = 0;
            TotalQty = 0;
            CartonQty = 0;
            document.getElementById("<%=modelHidden.ClientID %>").value = "";

        }

        function input(inputData) {


            if (inputData == "7777") {
                ResetPage();
                callNextInput();
                return;
            }
            //1.	Carton No – 如果用户录入的数据长度为9，且以字符’T’开头，则可以认定用户录入的数据Carton No
            if ((inputData.length == 9) || (inputData.substring(0, 1) == "T")) {
                inputSN(inputData);
                return;
            }

            alert(msgInputCustSN);
            callNextInput();
            return;

        }

        function inputSN(inputData) {

            ShowInfo("");
            beginWaitingCoverDiv();            
            WebServiceCombineCartonInDNForRCTO.InputSN(inputData,  "", editor, stationId, customer, onInputSNSucc, onFail);
            return;
        }

        function onInputSNSucc(result) {
        
            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                setInfo(result);
                callNextInput();
            }
            else {
                ShowInfo("");
                var content = result[0];
                ResetPage();
                ShowMessage(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onFail(result) {
            endWaitingCoverDiv();
            //ResetPage();        
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function saveProcess() {
            try {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    callNextInput();
                }
                else {
                    //ShowInfo("");
                    beginWaitingCoverDiv();
                    WebServiceCombineCartonInDNForRCTO.Save(firstProID, deliveryNo, printItemlist, onSaveSuccess, onSaveFail);
               }
            }
            catch (e) {
                alert(e);
            }
        }

        function setPrintItemListParam(backPrintItemList, cartonNo) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@CartonNo";
            valueCollection[0] = generateArray(cartonNo);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            //setPrintParam(lstPrtItem, "Customer SN Label", keyCollection, valueCollection);
            setPrintParam(lstPrtItem, "RCTO_Carton_Label", keyCollection, valueCollection);
        }

        function onSaveSuccess(result) {

            endWaitingCoverDiv();      //打印流程完成，打印的过程交给打印机
            ShowInfo("");
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {

                    //==========================================print process=======================================
                    var printflag = result[7];
                    if (printflag == "Y") {
                        setPrintItemListParam(result[1], result[3]);
                        /*
                        * Function Name: printLabels
                        * @param: printItems
                        * @param: isSerial
                        */
                        printLabels(result[1], false);
                    }
                    //==========================================end print process===================================
                    if (result[2] == "N") {
                        var ret = CallEDITSFunc(result);
                        if (ret) {
                            ret = CallPdfCreateFunc(result);
                        }
                    }
                    checkQtyProcess(result);
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


        function onSaveFail(error) {

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

        function checkQtyProcess(result) {

            PackedQty = result[2];
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), PackedQty);
            if (PackedQty == TotalQty) {
                ClearScreen();       
                ShowInfo(msgDeliveryOK);
            }
         
            callNextInput();

        }
        function inputModel(inputDate) {

            if (model == "") {
                alert(msgInputModel);
                callNextInput();
                return;
            }

            if (inputDate != model) {
                alert(msgValidModel);
                callNextInput();
                return;
            }

            if (TotalQty < PackedQty + ScannedQty) {
                alert(msgOverTotal);
                callNextInput();
                return;
            }

            if (ScannedQty < PcsInCarton) {
                alert(msgCartonNotFull);
                callNextInput();
                return;
            }

            saveProcess();
            return;
        }

        function onInputCOAFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function onInputCOAQuerySucc(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            if (result[0] == SUCCESSRET) {
                setTable(result);
                ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
                ExitPage();
                needReset = true;
                callNextInput();
            }
        }

        function setDelivery() {
            var dnnum = document.getElementById("<%=dnListHidden.ClientID%>").value;
            if (dnnum == 1) {
                ShowInfo(msgNullDev);
            }
            else if (dnnum > 2) {
                ShowInfo(msgInputDev);
            }
            else {
                getDeliveryCmbObj().selectedIndex = 1;
                document.getElementById("<%=btnResetDnInfo.ClientID%>").click();
                return;
            }
            callNextInput();
        }

        function ChangeDn() {

            deliveryNo = getDeliveryCmbValue();
            var model1 = document.getElementById("<%=deliverymodelHidden.ClientID%>").value;
            if (model != model1) {
                ShowInfo(msgValidModel);
                callNextInput();
                return;
            }
            var deliverytxt = getDeliveryCmbText();
            var len = deliverytxt.lastIndexOf("_");
            var deliverytmp = deliverytxt.substring(len + 1, deliverytxt.length);

            TotalQty = document.getElementById("<%=dnQtyHidden.ClientID%>").value;
            TotalQty = parseInt(TotalQty);
            PackedQty = document.getElementById("<%=dnPackedHidden.ClientID%>").value;
            PackedQty = parseInt(PackedQty);
           
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
            setInputOrSpanValue(document.getElementById("<%=lblTotalQtyContent.ClientID %>"), TotalQty);
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), PackedQty);

            checkDelivery();
        }

        function checkDelivery() {
            
            if (CartonQty + PackedQty > TotalQty) {
                ShowInfo(msgOverQty);
                callNextInput();
                return;

            }

            if (TotalQty == PackedQty) {
                ShowMessage(msgDeliveryOK);
                //saveProcess();
                ClearScreen();
                return;
            }

            saveProcess();
            callNextInput();
        }
        
        function setInfo(info) {

            //var devstr = info[6];
            //set Delivery
            CartonQty = info[2];
            productID = info[1]["ProductID"];
            //customerSN = info[1]["CustSN"];
            if (firstProID == "") {
                firstProID = productID;
            }
 
            if (model == "") {
                model = info[1]["Model"];
                setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
                document.getElementById("<%=modelHidden.ClientID %>").value = model;
                document.getElementById("<%=btnGetDn.ClientID%>").click();
            }

            return;
        }



        function inputSucc(result) {
        
            setInfo(result);
            inputFlag = true;
            endWaitingCoverDiv();

            getAvailableData("input");
            inputObj.focus();

        }

        function OnCancel() {
            if (!(deliveryNo == "")) {
                WebServiceCombineCartonInDNForRCTO.cancel(deliveryNo);
            }
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function clkSetting() {
            //stationId="92";
            //PCode="OPPA006"
            showPrintSetting(stationId, pCode);
        }


        function ClearScreen() {

            initPage();
            ShowInfo("");
            callNextInput();
            return true;
        }
    
                                                                          
    </script>

</asp:Content>
