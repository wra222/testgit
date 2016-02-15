<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/18
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/18            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/08/01    Du.Xuan               Create   
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombinePoInCartonForRCTO.aspx.cs" Inherits="PAK_CombinePoInCartonForRCTO"
    Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <bgsound src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceCombinePoInCartonForRCTO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col />
                    <col style="width: 150px;" />
                </colgroup>
                <tr>
                    <td align="right">
                        <asp:Label runat="server" ID="lblPCs" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPCsContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
            </table>
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
                                    <iMES:CmbDeliveryByModel ID="cmbDelivery" runat="server" ProcessQuickInput="true"
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
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col />
                        <col style="width: 200px;" />
                    </colgroup>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblScannedQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblScannedQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var customer = "";
        var stationId = "";
        var pCode = "";
        var hostname = getClientHostName();

        var inputObj;
        var emptyPattern = /^\s*$/;

        var ScannedQty = 0;
        var TotalQty = 0;
        var PackedQty = 0;

        var PcsInCarton = 0;
        var dnPcs = 0;

        var customerSN = "";
        var productID = "";
        var model = "";
        var zmode = "";
        var deliveryNo = "";
        var firstProID = "";
		var tmpProID = "";

        var reworkCache;

        var setPcs = 0;

        //error message
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgInputModel = '<%=this.GetLocalResourceObject(Pre + "_msgInputModel").ToString() %>';
        var msgInputPdLine = '<%=this.GetLocalResourceObject(Pre + "_msgInputPdLine").ToString() %>';
        var msgValidModel = '<%=this.GetLocalResourceObject(Pre + "_msgValidModel").ToString() %>';
        var msgOverTotal = '<%=this.GetLocalResourceObject(Pre + "_msgOverTotal").ToString() %>';
        var msgCartonNotFull = '<%=this.GetLocalResourceObject(Pre + "_msgCartonNotFull").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';
        var msgPcsError = '<%=this.GetLocalResourceObject(Pre + "_msgPcsError").ToString() %>';
        var msgOverPCs = '<%=this.GetLocalResourceObject(Pre + "_msgOverPCs").ToString() %>';
        var msgInputDev = '<%=this.GetLocalResourceObject(Pre + "_msgInputDev").ToString() %>';
        var msgNullDev = '<%=this.GetLocalResourceObject(Pre + "_msgNullDev").ToString() %>';
        var msgPNError = '<%=this.GetLocalResourceObject(Pre + "_msgPNError").ToString() %>';

        var msgNotSame = '<%=this.GetLocalResourceObject(Pre + "_msgNotSame").ToString() %>';
        var msgNoPath = '<%=this.GetLocalResourceObject(Pre + "_msgNoPath").ToString() %>';
        var msgNoTemp = '<%=this.GetLocalResourceObject(Pre + "_msgNoTemp").ToString() %>';
        var msgErrCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgErrCreatePDF").ToString() %>';

        var imgAddr = "";
        var webEDITSaddr = "";
        var xmlFilePath = "";
        var pdfFilePath = "";
        var tmpFilePath = "";
        var fopFilePath = "";

        var needReset = false;

        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();

            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';

            initPage();
            //callNextInput();

            //beginWaitingCoverDiv();           
            /*var nameCollection = new Array();
            nameCollection.push("PLEditsImage");
            nameCollection.push("PLEditsURL");
            nameCollection.push("PLEditsXML");
            nameCollection.push("PLEditsPDF");
            nameCollection.push("PLEditsTemplate");
            nameCollection.push("FOPFullFileName");
            
            WebServiceCombinePoInCartonForRCTO.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);
            */
            WebServiceCombinePoInCartonForRCTO.GetSysSettingSafe("PcsInCarton", "5", hostname, editor, onGetSettingSafe, onGetSettingFailed);
        };

        window.onbeforeunload = function() {

            OnCancel();
        };

        function onGetSettingSafe(result) {

            if (result[0] == SUCCESSRET) {

                PcsInCarton = Number(result[1]);
                setInputOrSpanValue(document.getElementById("<%=lblPCsContent.ClientID %>"), PcsInCarton);

            } else {
                ShowInfo("");
                var content = result[0];
                //alert(content);
                ShowMessage(content);
                ShowInfo(content);
            }

            callNextInput();
        }
        function onGetSetting(result) {

            //endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                imgAddr = result[1][0];
                webEDITSaddr = result[1][1];
                xmlFilePath = result[1][2];
                pdfFilePath = result[1][3];
                tmpFilePath = result[1][4];
                fopFilePath = result[1][5];
                var path = imgAddr + "\\*.jpg";
                var fso = new ActiveXObject("Scripting.FileSystemObject");
                var fileflag = fso.FolderExists(imgAddr);
                if (fileflag) {
                    fso.CopyFile(path, "C:\\");
                }
                else {
                    alert(msgNoPath);
                }
            } else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onGetSettingFailed(result) {

            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

        function initPage() {

            tbl = "<%=gd.ClientID %>";
            reworkCache = [];
            setInputOrSpanValue(document.getElementById("<%=lblPCsContent.ClientID %>"), setPcs);
            //set Delivery
            setInputOrSpanValue(document.getElementById("<%=lblTotalQtyContent.ClientID %>"), "0");
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), "0");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblScannedQtyContent.ClientID %>"), "0");

            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            customerSN = "";
            productID = "";
            model = "";
            zmode = "";
            deliveryNo = "";
            firstProID = "";
            needReset = false;

            PackedQty = 0;
            ScannedQty = 0;
            TotalQty = 0;

            document.getElementById("<%=modelHidden.ClientID %>").value = "";

        }

        function input(inputData) {

            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                //alert(msgPrintSettingPara);
                ShowMessage(msgPrintSettingPara);
                callNextInput();
                return;
            }          

            var line = getPdLineCmbValue();      
            if (line == "") {
                //alert(msgInputPdLine);
                ShowMessage(msgInputPdLine);
                callNextInput();
                return;
            }
            
            if ((inputData.length == 9)) {

                inputData = SubStringSN(inputData, "ProdId");
                inputSN(inputData);
                return;
            }
            
            if (inputData.length == 14 || inputData.length == 15 || inputData.length == 18) {
                inputSN(inputData); //Vendor CT
                return;
            }
            var test = inputData.substring(6, 7);
            if ((inputData.length == 10) && (inputData.substring(6, 7) == "-")) {
                inputCustPN(inputData);
                return;
            }

            if (inputData.length == 12) {
                inputModel(inputData);
                return;
            }

            if ((inputData.length == 10)) {

               // if (CheckCustomerSN(inputData)) {
                    inputSN(inputData);
                    return;
                //}
            }
            
            /* if (checkPCs(inputData)) {
            inputPCs(inputData);
            return;
            }
            else {
            if (inputData == 0) {
            alert(msgPcsError);
            callNextInput();
            return;
            }
            }
            */
            //alert(msgInputCustSN);
            ShowMessage(msgInputCustSN);
            callNextInput();
            return;

        }

        function inputSN(inputData) {

            if (ScannedQty >= PcsInCarton) {
                //alert(msgOverPCs);
                ShowMessage(msgOverPCs);
                callNextInput();
                return;
            }
            if (isExistInCache(inputData)) {
                //alert(msgDuplicateData);
                ShowMessage(msgDuplicateData);
                callNextInput();
                return;
            }
			tmpProID = "";
			if (firstProID == "")
				tmpProID = inputData;
            ShowInfo("");
            var line = getPdLineCmbValue();
            beginWaitingCoverDiv();
            WebServiceCombinePoInCartonForRCTO.InputSN(inputData, model, firstProID, line, editor, stationId, customer, onInputSNSucc, onFail);
            return;
        }

        function onInputSNSucc(result) {

            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                setInfo(result);
            }
            else {
                ShowInfo("");
                ResetPage();
                ShowMessage(result[1]["mErrmsg"]);
                ShowInfo(result[1]["mErrmsg"]);
                callNextInput();
            }
            
        }

        function onFail(result) {
            endWaitingCoverDiv();
            //ResetPage();
			if (tmpProID != ""){
				OnCancel();
			}
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function saveProcess() {
            try {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    //alert(msgPrintSettingPara);
                    ShowMessage(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                }
                else {
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    WebServiceCombinePoInCartonForRCTO.Save(firstProID, deliveryNo, printItemlist, onSaveSuccess, onSaveFail);
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
            //ShowInfo("");
            //beginWaitingCoverDiv();
            //WebServiceCombinePoInCartonForRCTO.checkQty(deliveryNo, onCheckSuccess, onSaveFail);

            PackedQty = result[2];
            var remainQty = TotalQty - PackedQty;
            if (remainQty == 0) {
                var temp = deliveryNo;
                ResetPage();
                ShowSuccessfulInfo(true, result[6]);
                /*if (result[5] !=""){
                    //ShowSuccessfulInfo(true, result[5]);
                     ShowSuccessfulInfoFormat(true, "Customer SN", result[5]); // Print 成功，带成功提示音！   
                    }
                else {
                    //ShowSuccessfulInfo(true, result[3]);
                    ShowSuccessfulInfoFormat(true, "CartonSN", "CartonSN:" + result[3]); // Print 成功，带成功提示音！
                }*/
                //ShowSuccessfulInfoFormat(true, "Delivery NO", temp);
            }
            else {
                ClearScreen();
                PcsInCarton = document.getElementById("<%=dnPcsHidden.ClientID%>").value;
                if (remainQty < PcsInCarton) {
                    PcsInCarton = remainQty;
                }
                setInputOrSpanValue(document.getElementById("<%=lblPCsContent.ClientID %>"), PcsInCarton);
                setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), PackedQty);
                //ShowInfo(result[6]);
                ShowSuccessfulInfoFormat(true, "CartonSN", "CartonSN:" + result[3]); // Print 成功，带成功提示音！
                
            }
            callNextInput();

        }
        function inputPCs(inputData) {
            ShowInfo("");
            beginWaitingCoverDiv();
            setPcs = inputData;
            WebServiceCombinePoInCartonForRCTO.SetSysSetting("PcsInCarton", inputData, hostname, editor, onPCsSuccess);
        }
        function inputModel(inputDate) {

            if (model == "") {
                //alert(msgInputModel);
                ShowMessage(msgInputModel);
                callNextInput();
                return;
            }

            if (inputDate != model) {
                //alert(msgValidModel);
                ShowMessage(msgValidModel);
                callNextInput();
                return;
            }

            if (TotalQty < PackedQty + ScannedQty) {
                //alert(msgOverTotal);
                ShowMessage(msgOverTotal);
                callNextInput();
                return;
            }

            if (ScannedQty < PcsInCarton) {
                //alert(msgCartonNotFull);
                ShowMessage(msgCartonNotFull);
                callNextInput();
                return;
            }

            saveProcess();
            return;
        }

        function inputCustPN(inputDate) {

            if (model == "") {
                //alert(msgPNError);
                ShowMessage(msgPNError);
                callNextInput();
                return;
            }

            if (inputDate != zmode) {
                //alert(msgValidModel);
                ShowMessage(msgValidModel);
                callNextInput();
                return;
            }

            if (TotalQty < PackedQty + ScannedQty) {
                //alert(msgOverTotal);
                ShowMessage(msgOverTotal);
                callNextInput();
                return;
            }

            if (ScannedQty < PcsInCarton) {
                //alert(msgCartonNotFull);
                ShowMessage(msgCartonNotFull);
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

        function onPCsSuccess(result) {
            endWaitingCoverDiv();
            PcsInCarton = setPcs;
            setInputOrSpanValue(document.getElementById("<%=lblPCsContent.ClientID %>"), PcsInCarton);
            callNextInput();
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

            deliveryNo = getDeliveryCmbValue().trim();

            TotalQty = document.getElementById("<%=dnQtyHidden.ClientID%>").value;
            TotalQty = parseInt(TotalQty);
            PcsInCarton = document.getElementById("<%=dnPcsHidden.ClientID%>").value;
            PcsInCarton = parseInt(PcsInCarton);
            PackedQty = document.getElementById("<%=dnPackedHidden.ClientID%>").value;
            PackedQty = parseInt(PackedQty);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
            setInputOrSpanValue(document.getElementById("<%=lblTotalQtyContent.ClientID %>"), TotalQty);
            setInputOrSpanValue(document.getElementById("<%=lblPCsContent.ClientID %>"), PcsInCarton);
            setInputOrSpanValue(document.getElementById("<%=lblPackedQtyContent.ClientID %>"), PackedQty);

            if (deliveryNo == "") {
                ShowInfo(msgInputDev);
                callNextInput();
            }
            else {
                ShowInfo("");
                checkDelivery();
            }
        }

        function checkDelivery() {
              
            var remainQty = TotalQty - PackedQty;
            if (remainQty < PcsInCarton) {
                PcsInCarton = remainQty;
                setInputOrSpanValue(document.getElementById("<%=lblPCsContent.ClientID %>"), PcsInCarton);
            }
            if (ScannedQty >= PcsInCarton) {
                saveProcess();
                return;
            }
            callNextInput();
        }
        
        function setInfo(info) {
            //set value to the label
            //dnPcs = info[4];
            /*if (model == "") {
            PcsInCarton = dnPcs;/
            }*/
            productID = info[1]["ProductID"];
            //customerSN = info[1]["CustSN"];
            if (firstProID == "") {
                firstProID = productID;
            }
            //var devstr = info[6];
            setTable(info[1]);
            ScannedQty++;
            setInputOrSpanValue(document.getElementById("<%=lblScannedQtyContent.ClientID %>"), ScannedQty);
            //set Delivery
            if (model == "") {
                model = info[1]["Model"];
                zmode = info[2];
                document.getElementById("<%=modelHidden.ClientID %>").value = model;
                document.getElementById("<%=btnGetDn.ClientID%>").click();
                return;
            }
            else {
                checkDelivery();
                
            }
            return;
        }

        function setTable(info) {

            var rownumber = reworkCache.length;
            var rowArray = new Array();
            var rw;

            rowArray.push(info["ProductID"]);
            rowArray.push(info["Station"]);
            rowArray.push(info["Sn"]);

            //add data to table
            if (rownumber < DEFAULT_ROW_NUM - 1) {
                eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, rownumber+1);");
                setSrollByIndex(rownumber, true, tbl);
            }
            else {
                eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                setSrollByIndex(rownumber, true, tbl);
                rw.cells[1].style.whiteSpace = "nowrap";
            }
            reworkCache.push(info);

        }

        function isExistInCache(data) {
            if (reworkCache != undefined && reworkCache != null) {
                for (var i = 0; i < reworkCache.length; i++) {
                    if (reworkCache[i]["ProductID"] == data || reworkCache[i]["Station"] == data) {
                        return true;
                    }
                }
            }
            return false;
        }

        function inputSucc(result) {
            setInfo(result);
            inputFlag = true;
            endWaitingCoverDiv();

            getAvailableData("input");
            inputObj.focus();

        }

        function OnCancel() {
            var k = firstProID;
			if (k == "")
				k = tmpProID;
			if (!(k == "")) {
                WebServiceCombinePoInCartonForRCTO.cancel(k);
            }
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            document.getElementById("<%=btnGetDn.ClientID%>").click();
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


        function CallEDITSFunc(result) {

            var Paralist = new EDITSFuncParameters();
            var filepath = "";
            var devno = result[5];
            var cust = result[6];
            var cartno = result[4];
            //XmlFilename = DN & "-" & CN & "-[BoxShipLabel].xml"
            var filename = devno + "-" + cartno + "-[BoxShipLabel].xml"

            if (GetDebugMode()) {
                //Debug Mode get Root path from Web.conf
                xmlFilePath = GetCreateXMLfileRootPath();
                webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
            }
            if (xmlFilePath == "" || webEDITSaddr == "") {
                alert("Path error!");
                return false;
            }
            filepath = xmlFilePath + "DOCKXML\\" + filename;
            CheckMakeDir(filename);

            Paralist.add(1, "FilePH", filepath);
            Paralist.add(2, "Dn", devno);
            Paralist.add(3, "SerialNum", cust);

            var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
            return IsSuccess;
        }

        function CallPdfCreateFunc(result) {
            var devno = result[5];
            var cust = result[6];
            var cartno = result[4];
            var tempName = result[7];

            var xmlfilename = devno + "-" + cartno + "-[BoxShipLabel].xml";
            var xslfilename = devno + "-" + cartno + "-[BoxShipLabel].xslt";
            var pdffilename = devno + "-" + cartno + "-[BoxShipLabel].pdf";

            if (xmlFilePath == "" || webEDITSaddr == "") {
                alert("Path error!");
                return false;
            }

            var xmlfullpath = xmlFilePath + "DOCKXML\\" + xmlfilename;
            var xslfullpath = tmpFilePath + tempName;
            var pdffullpath = pdfFilePath + "DOCKPDF\\" + pdffilename;

            //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
            //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
            //--------------------------------------------------------------
            var islocalCreate = false;
            //var islocalCreate = true;
            //================================================================
            var IsSuccess = CreatePDFfileAsyn(fopFilePath, xmlfullpath, xslfullpath, pdffullpath, islocalCreate);
            return IsSuccess;
        }

        function GetEDITSIP() {
            var HPEDITSIP = '<%=ConfigurationManager.AppSettings["HPEDITSIP"].Replace("\\", "\\\\")%>';
            return HPEDITSIP;
        }

        function GetEDITSTempIP() {
            var HPEDITSTempIP = '<%=ConfigurationManager.AppSettings["HPEDITSTEMPIP"].Replace("\\", "\\\\")%>';
            return HPEDITSTempIP;
        }
        function GetFopCommandPathfile() {
            var FopCommandPathfile = '<%=ConfigurationManager.AppSettings["FopCommandPathfile"].Replace("\\", "\\\\")%>';
            return FopCommandPathfile;
        }

        function GetTEMPLATERootPath() {
            var TEMPLATERootPath = '<%=ConfigurationManager.AppSettings["TEMPLATERootPath"].Replace("\\", "\\\\")%>';
            return TEMPLATERootPath;
        }
        function GetCreateXMLfileRootPath() {
            var CreateXMLfileRootPath = '<%=ConfigurationManager.AppSettings["CreateXMLfileRootPath"].Replace("\\", "\\\\")%>';
            return CreateXMLfileRootPath;
        }
        function GetCreatePDFfileRootPath() {
            var CreatePDFfileRootPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileRootPath"].Replace("\\", "\\\\")%>';
            return CreatePDFfileRootPath;
        }
        function GetCreatePDFfileClientPath() {
            var CreatePDFfileClientPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileClientPath"].Replace("\\", "\\\\")%>';
            return CreatePDFfileClientPath;
        }
        function GetDebugMode() {
            var DEBUGmode = '<%=ConfigurationManager.AppSettings["DEBUGmode"]%>';
            if (DEBUGmode == "True")
                return true;
            else
                return false;
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

        function checkPCs(inputDate) {

            //则检查本框输入内容是否满足格式要求
            var reExp = /^[0-9]*[1-9][0-9]*$/;
            if (reExp.exec(inputDate) == null || reExp.exec(inputDate) == false) {
                return false;
            }

            var number = parseFloat(inputDate);
            if (number >= 999 || number <= 0) {
                return false;
            }

            return true;
        }

        function ClearScreen() {

            tbl = "<%=gd.ClientID %>";
            reworkCache = [];
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            ScannedQty = 0;
            firstProID = "";
            setInputOrSpanValue(document.getElementById("<%=lblScannedQtyContent.ClientID %>"), "0");

            callNextInput();
            return true;
        }
    
                                                                          
    </script>

</asp:Content>
