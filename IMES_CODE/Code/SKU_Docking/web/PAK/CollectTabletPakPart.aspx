﻿<%--
* INVENTEC corporation ©2011 all rights reserved. 
* Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="CollectTabletPakPart.aspx.cs" Inherits="PAK_CollectTabletPakPart" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceCollectTabletFaPart.asmx" />
            <asp:ServiceReference Path="~/Service/PrintSettingService.asmx" />
			<asp:ServiceReference Path="~/PAK/Service/WebServiceUnitWeightNew.asmx" />

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
                        <asp:Label runat="server" ID="lbPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
            </table>
            <hr />
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px"></legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 80px;" />
                        <col />
                        <col style="width: 80px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lbCollection" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
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
                        <td nowrap="noWrap" style="width: 150px;">
                            <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <input id="Button1" type="button" runat="server" class="iMes_button" value="RePrint POD" onclick="reprintPOD()" />
                            <input id="btnPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()" />
                            <input id="btnReprint" type="button" runat="server" class="iMes_button" onclick="reprint()" />&nbsp;
                            <input type="hidden" runat="server" id="pCode" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <input id="modelHidden" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var inputFlag = false;
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 10;
        var passQty = 0;
        var totalQty = 0;
        var customerSN = "";
        var bomList;
        var customer;
        var station;
        var inputObj;
        var productID = "";
        var model = "";
        var pCode = "";
        var podLabelPath = "";
        //error message
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
        var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var gvTable = document.getElementById("<%=GridViewExt1.ClientID %>");
        var msgNoCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgNoCustSN").ToString()%>';
        var msgNoVendorCT = '<%=this.GetLocalResourceObject(Pre + "_msgNoVendorCT").ToString()%>';
        var msgEwasteLabel = '<%=this.GetLocalResourceObject(Pre + "_msgEwasteLabel").ToString()%>';
        var msgBAHASALabel = '<%=this.GetLocalResourceObject(Pre + "_msgBAHASALabel").ToString()%>'; 
        var msgNOMLabel = '<%=this.GetLocalResourceObject(Pre + "_msgNOMLabel").ToString()%>';
        var msgNoPODLabelFile = '<%=this.GetLocalResourceObject(Pre + "_msgNoPODLabelFile").ToString() %>';
        var secondCustomerSN = "";
        var msgJapan = '<%=this.GetLocalResourceObject(Pre + "_msgJapan").ToString()%>';
        var msgDomestic =  '<%=this.GetLocalResourceObject(Pre + "_msgDomestic").ToString()%>';
        var lstPrintItem;
        var accountid;
        var login;
        var defectInTable = [];
        var havePrintItem = false;
        var needPrintPOD = false;
        var podColor = "";
		var color = "";
		var printerpath="";
        var blackPrinter = "";
        var whitePrinter = "";
        var pdfClinetPath = "<%=PDFClinetPath%>";
        var checkModel = "";
        var allownullbomitem = "";
        window.onload = function() {
         

            tbl = "<%=GridViewExt1.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            allownullbomitem = '<%=Request["Allownullbomitem"] %>';
            printerpath= '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
          if (printerpath.charAt(printerpath.length - 1) != "\\")
           { printerpath = printerpath + "\\";}
            accountid = '<%=AccountId%>';

            login = '<%=Login%>';
            //          PrintSettingService.GetPrintInfo(pCode, onGetPSucceed, onGetPFail);
             havePrintItem= document.getElementById("<%=btnPrintSet.ClientID %>"); //btnPrintSet
		WebServiceUnitWeightNew.GetPODLabelPathAndSite(onGetPathSucceed, onFailed);

        }

        window.onbeforeunload = function() 
        {
            OnCancel();
        }
        function onGetPathSucceed(result) {
            
            podLabelPath = result[0];
        }
		 function onFailed(result) {
			 endWaitingCoverDiv();
			 ResetPage();
			 ShowMessage(result.get_message());
			 ShowInfo(result.get_message());
		 }


        function initPage() 
        {
            ShowInfo("");
            tbl = "<%=GridViewExt1.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            getPdLineCmbObj().disabled = false;
            inputFlag = false;
            customerSN = "";
            productID = "";
            needPrintPOD = false;
            model = "";
            podColor = "";
            checkModel = "";
            callNextInput(); 
        }
        function initPage_Success() {
            //ShowInfo("");
            tbl = "<%=GridViewExt1.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            getPdLineCmbObj().disabled = false;
            inputFlag = false;
            customerSN = "";
            secondCustomerSN = "";
            productID = "";
            callNextInput();
            needPrintPOD = false;
            model = "";
            podColor = "";
            checkModel = "";
        }
        function callNextInput() 
        {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function input(data) 
        {
            var line = getPdLineCmbValue();
            lstPrintItem = getPrintItemCollection();
            if (data.length > 10 && data.substr(0, 3) == "5CG" && customerSN == "") 
            {
                data = data.substr(0, 10)
            }
             if (line == "") 
            {
                ShowInfo(mesNoSelPdLine);
            }
            else if (lstPrintItem == null && havePrintItem) {
                alert(msgPrintSettingPara);
            }
            else if (data == "7777") 
            {
                //执行Reset
                ResetPage();
            }
            else if (customerSN == "" && station!="T4") 
            {
                //刷入CustomerSN
                inputCustomSNorProdId(data);
            }
            else if ( (customerSN == "" || secondCustomerSN=="" ) && station == "T4") {
                //刷入CustomerSN
                inputCustomSNorProdIdForT4(data);
            }
            else {
                if (station == "T3" && checkModel == "") {
                    inputModelCheck(data);
                }
                else {
                    //刷入VenderCT
                    inputVenderCT(data);
                }
            }
            callNextInput();
        }
        function inputModelCheck(inputData) {
            if (inputData == model) {
                checkModel = model;
                ShowInfo("");
            }
            else {
                ShowInfo("Wrong model!!");
            }
            callNextInput();
        }
        function inputCustomSNorProdIdForT4(inputData) {
            //        //if (!(inputData.substring(0, 3) == "SCN" || inputData.substring(0, 2) == "CN"))
            if (!((inputData.length == 10) || (inputData.length == 11))) {
                //alert(msgNoCustSN);
                ShowInfo(msgNoCustSN);
                callNextInput();
            }
//            else if (customerSN == "") {
//                    if (inputData.length == 10) {
//                        customerSN = inputData;
//                        ShowInfo("Please input Customer SN on Box");
//                        callNextInput();
//                        return;
//                    }
//                    else {
//                        ShowInfo("Please input Customer SN");
//                        callNextInput();
//                        return;
//                    }
//            
//            }
            else {
                if ((inputData.length == 11 && inputData == customerSN + " ")||(inputData.length == 10)) {
                    ShowInfo("");
                    secondCustomerSN = inputData;
                     var line = getPdLineCmbValue();
                customerSN = inputData.trim(); //inputData.substring(inputData.length - 10, 10);
                beginWaitingCoverDiv();
                WebServiceCollectTabletFaPart.inputSN(customerSN, line, station, editor, station, customer,allownullbomitem,onCustSNSucc, onCustSNFail);
                    //return;
                }
                else {
                    ShowInfo("Please input Customer SN on Box");
                    callNextInput();
                    return;
                }

            }

          
        }
        function inputCustomSNorProdId(inputData) 
        {
            //        //if (!(inputData.substring(0, 3) == "SCN" || inputData.substring(0, 2) == "CN"))
            if ( !inputData.substring(0, 3) == "5CG")
           // if (inputData.length!=10)
            {
                //alert(msgNoCustSN);
                ShowInfo(msgNoCustSN);
                callNextInput();
            }
            else 
            {
                var line = getPdLineCmbValue();
               // customerSN = inputData.trim(); //inputData.substring(inputData.length - 10, 10);
                customerSN = inputData.substring(0, 10);
                beginWaitingCoverDiv();
                WebServiceCollectTabletFaPart.inputSN(customerSN, line, station, editor, station, customer, allownullbomitem, onCustSNSucc, onCustSNFail);
            }
        }

        function onCustSNSucc(result) 
        {

            if (result[0] == SUCCESSRET) 
            {
                ShowInfo("");
                setInfo(result);
                inputFlag = true;
                endWaitingCoverDiv();
                if (station == "T3" && result[3]) {
                    ShowInfo("Please scan model!", "green");
                 }
                getPdLineCmbObj().disabled = true;
            }
            else 
            {
                endWaitingCoverDiv();
                getPdLineCmbObj().disabled = false;
                inputFlag = false;
                customerSN = "";
                //var content = result[0];
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }
            callNextInput();
        }

        function onCustSNFail(result) 
        {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            //endWaitingCoverDiv();
            customerSN = "";
            secondCustomerSN = "";
            inputFlag = false;
            getPdLineCmbObj().disabled = false;
            callNextInput();
        }
        
        function setInfo(info) 
        {
            //set value to the label
            productID = info[1].id;//["id"];
            model = info[1].modelId; //["modelId"];
            needPrintPOD = info[3];
            podColor = info[4];
   //         customerSN = productID; //info[1].customSN;//["customSN"];
            customerSN =info[1].customSN;
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), model);
            setTable(info)
        }
        
        function getvcstring(PartsArray) 
        {
            var strVclst = "";
            var strVCName = "";
            for (var iParts = 0 ;iParts< PartsArray.length; iParts++)
            {
                for (var i = 0; i < PartsArray[iParts].properties.length; i++) {
                    strVCName = PartsArray[iParts].properties[i].Name.trim().toUpperCase();
                    //if ((propertieslst[i]["Name"] != null) || (propertieslst[i]["Name"].trim() != "")) {
                    if (strVCName == "VENDORCODE") {
                        if (strVclst != "") {
                            strVclst = strVclst + ",";
                        }
                        strVclst = strVclst + PartsArray[iParts].properties[i].Value.trim();
                    }
                }
            }
            return strVclst;
        }

        function setTable(info) {
            defectInTable = info[2];
             bomList = info[2];
            tblRows = bomList.length;
            if (tblRows == 0) {
                finishPart = true;
                Save();
                return; 
            }
            for (var i = 0; i < bomList.length; i++) {

                var rowArray = new Array();
                var rw;
                var collection = bomList[i]["collectionData"];
                var parts = bomList[i]["parts"];
                var tmpstr = "";
                if (bomList[i]["PartNoItem"] == null) {
                    rowArray.push(" ");
                }
                else {
                    rowArray.push(bomList[i]["PartNoItem"]);
                }

                rowArray.push(bomList[i]["tp"]);
                if (bomList[i]["description"] == null) {
                    rowArray.push(" ");
                }
                else {
                    rowArray.push(bomList[i]["description"]);
                }

                rowArray.push(bomList[i]["qty"]);
                rowArray.push(bomList[i]["scannedQty"]);
                coll = "";
                for (var j = 0; j < collection.length; j++) {
                    tmpstr = tmpstr + " " + collection[j]["pn"];
                }
                rowArray.push(tmpstr); //["collectionData"]);

                //add data to table
                if (i < 12) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                setSrollByIndex(0, true, tbl);
            }
        
        }

        function inputVenderCT(inputData) {
//              if (inputData.length == 14) {

                //var line = getPdLineCmbValue();
                //pizzaID = "10050000067"; //inputData.substring(0, 9);
                beginWaitingCoverDiv();
                WebServiceCollectTabletFaPart.inputVenderCT(productID, inputData.trim(), onVenderCTSucc, onVenderCTFail);

//            }
//            else {
//                ShowInfo(msgNoVendorCT);
//                callNextInput();
//            }
        }
        
         function onVenderCTSucc(result) {

            if (result[0] == SUCCESSRET) 
            {
                ShowInfo("");
                endWaitingCoverDiv();
                setScrollCycle(result);
                
            }
            else if  (result == null)
            {
                endWaitingCoverDiv();
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
            }
            else 
            {
                endWaitingCoverDiv();
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }
            getPdLineCmbObj().disabled = true;
            callNextInput();
        }

        function onVenderCTFail(result) {
            if  (result == null)
            {
                endWaitingCoverDiv();
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
            }
            else 
            {
                endWaitingCoverDiv();
                ShowMessage(result[0]);
                ShowInfo(result[0]);
           }
            getPdLineCmbObj().disabled = true;
            callNextInput();
        }
        function checkFinished() {
            var ret = true;
            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i]["qty"] != defectInTable[i]["scannedQty"]) {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
         function setScrollCycle(matchDataItem) 
        {
            eval("setRowNonSelected_" + tbl + "()");
            //setScroll(matchDataItem[1], matchDataItem[2]);//1--PN;2--CT
             try {
                    var row;
                    var subArray;
                    var subScanQty;
                    var subFindFlag = false;
                    var table = document.getElementById(tbl);
                    for (var i = 1; i < table.rows.length; i++)
                    {
                        for (var j = 0; j < bomList[i-1].parts.length;j++)
                        {
                            if (bomList[i-1].parts[j].id == matchDataItem[1])
                            {
                                subFindFlag =true;
                                break;
                            }
                        }
                        if (subFindFlag)
                        {
                            subArray = table.rows[i].cells[5].innerText;
                            subScanQty = parseInt(table.rows[i].cells[4].innerText, 10);
                            if (subArray.trim() != "")  
                            {
                                subArray = subArray + ",";
                            }
                            subArray = subArray  +  matchDataItem[2].trim();
                            table.rows[i].cells[5].innerText = subArray;
                            //Add Tip
                            table.rows[i].cells[5].title = subArray;
                            subScanQty = subScanQty + 1;
                            table.rows[i].cells[4].innerText = subScanQty.toString();
                            passQty = passQty + 1;
                            row = eval("setScrollTopForGvExt_" + tbl + "('" + table.rows[i].cells[5].innerText + "',5,'','MUTISELECT')");
                            break;
                        }
                    }
                     if (subFindFlag) {
                         var scanfinish = CheckQtyScanQty();
                          if (scanfinish) {
                                   beginWaitingCoverDiv();
                                   Save();
                                   //       WebServiceCollectTabletFaPart.save(productID, lstPrintItem,onSaveSuccess, onSaveFail);
                                 }
                           }
                    else {
                        //alert(msgCollectNoItem);
                        ShowMessage(msgCollectNoItem);
                        ShowInfo(msgCollectNoItem);

                    }
                
                } 
                catch (e) 
                {
                    alert(e.description);
                }
            }

        function CheckQtyScanQty() 
        {
            var bScanFinished = true;
            var table = document.getElementById(tbl);
            for (var i = 1; i < table.rows.length; i++)
            {
               if (parseInt(table.rows[i].cells[3].innerText) > parseInt(table.rows[i].cells[4].innerText))  
               {
                  bScanFinished = false;
                  break;
               }
            }
            return bScanFinished;
        }
        function Save() {
            GetPrinterName(lstPrintItem);
            var newPrintItemArr = new Array();
            for (var i = 0; i < lstPrintItem.length; i++) {
                if (lstPrintItem[i].LabelType.toUpperCase().indexOf("WHITE") == -1 &&
                    lstPrintItem[i].LabelType.toUpperCase().indexOf("BLACK") == -1) {
                    newPrintItemArr.push(lstPrintItem[i]);
                }
            }
            WebServiceCollectTabletFaPart.save(productID, newPrintItemArr, onSaveSuccess, onSaveFail);
        }
        function onSaveSuccess(result) {
            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                var msgPrint = "";
                var msgFinal = msgSuccess;
                if (station == "T2" && needPrintPOD) {
                    PDFPrint();
                }
                if (havePrintItem) {
                    for (var i = 0; i < result[1].length; i++) {
                        var labelCollection = [];
                        if(result[1][i].LabelType.indexOf("TabletWarrantyLabel")>=0)
                        {msgPrint=msgDomestic;}
                          if(result[1][i].LabelType.indexOf("TabletJapanLabel")>=0)
                          { msgPrint = msgJapan; }
                       
                        labelCollection.push(result[1][i]);
                        setPrintItemListParam(labelCollection, result[1][i].LabelType, customerSN);
                        printLabels(labelCollection, false);

                    }
                }
                if (result[3])
                { msgPrint = msgPrint + "," + msgEwasteLabel; }
                if (result[4])
                { msgPrint = msgPrint + "," + msgBAHASALabel; }
                if (result[5])
                { msgPrint = msgPrint + "," + msgNOMLabel; }
         
                if(msgPrint!="")
                {
                    msgFinal = msgFinal + "," + msgPrint;
                }
                //tmp = "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgFinal + " ,PAQC";
                msgFinal ="[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] "+ msgFinal;
                if (result[2] == "8")
                { msgFinal = msgFinal + " ,PAQC"; }
                if (station == "T4") {

                    if (msgPrint != "" || result[2] == "8")
                    { ShowSuccessfulInfoWithColor(true, msgFinal, null, "red"); } //ShowSuccessfulInfoWithColor(isPlaySound, message, isSpecialSound,color)
                    else
                    { ShowSuccessfulInfoWithColor(true, msgFinal, null, "green"); }

                }
                else {

                   // ShowSuccessfulInfoWithColor(true, msgSuccess, null, "green")
                    ShowSuccessfulInfoWithColor(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgSuccess,null, "green");
                }
            }
            else if(result == null)
            {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else 
            {
                //var content = result[0];
                ShowMessage(result[0]);
                ShowInfo(result[0]);
                //clearData();
                endWaitingCoverDiv();
            }
            //initPage();
            initPage_Success(); 
            callNextInput();
        }
        function onSaveFail(result) {
            endWaitingCoverDiv();
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else {
           
                ShowMessage(result[0]);
                ShowInfo(result[0]);
                endWaitingCoverDiv();
            }
      
            initPage_Success(); 
            callNextInput();
        }
        function setPrintItemListParam(backPrintItemList,labelType,sn) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(sn);
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        }
        function reprint() {
            //Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var url = "../FA/RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; 
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = editor;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }
        function reprintPOD() {
            //Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var url = "../PAK/RePrintPOD.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login;
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = editor;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }
        
        function OnCancel() {
            if (productID != "") {
                //DEBUG ITC-1360-0915 cancel->Cancel
                WebServiceCollectTabletFaPart.Cancel(productID);
            }
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
        }
        window.onunload = function() {
            OnCancel();
        }
        function showPrintSettingDialog() {
            showPrintSetting(station, pCode);
        }
        function onGetPSucceed(result) {
            try {

                if (result == null) {
                    var content = msgSystemError;
                    alert(content);

                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                    if (result[1].length == 0) {
                        havePrintItem = false;
                     }
                    else {
                        havePrintItem = true;
                    }
                    SetPrintBtn();
                 }
                else {
                    alert(result[0]);
                    var content = result[0];

                }
            } catch (e) {
                alert(e.description);

            }

        }


        function onGetPFail(error) {
            try {

                alert(error.get_message());

            } catch (e) {
                alert(e.description);

            }
        }
        function SetPrintBtn() {
            var id1 = "#" + "<%=btnPrintSet.ClientID %>";
            var id2 = "#" + "<%=btnReprint.ClientID %>";
            
            if (havePrintItem) {
                $(id1).show();
                $(id2).show();
            }
            else {
                $(id1).hide();
                $(id2).hide();
            }
        }
        // ********************For Print POD using ********************
        function GetPrinterName(printItem) {
            for (i = 0; i < printItem.length; i++) {
                if (printItem[i].LabelType.toUpperCase().indexOf("WHITE") >= 0) {
                    whitePrinter = printItem[i].PrinterPort;
                    continue;
                }
                if (printItem[i].LabelType.toUpperCase().indexOf("BLACK") >= 0) {
                    blackPrinter = printItem[i].PrinterPort;
                    continue;
                }
            }
        }
        function PDFPrint() {
            var pdfFileName = model + ".Pdf";
            var FsFile = "";
            var Fs = new ActiveXObject("Scripting.FileSystemObject");
            if (pdfClinetPath.slice(-1) == "\\"){
                FsFile = pdfClinetPath + "tabletpodprintlist.txt";
            }
            else {
                pdfClinetPath = pdfClinetPath + "\\";
                FsFile = pdfClinetPath + "tabletpodprintlist.txt";
            }

            if (!Fs.FolderExists(pdfClinetPath)) {
                Fs.CreateFolder(pdfClinetPath);
            }

            if (Fs.FileExists(FsFile)) {
                Fs.DeleteFile(FsFile);
            }
            File = Fs.CreateTextFile(FsFile, true);
            var pdfPath;
            pdfPath = podLabelPath + pdfFileName;
            File.WriteLine(pdfPath);
            File.Close();
            var wsh = new ActiveXObject("wscript.shell");
            var cmdpdfprint;
            if (podColor != "") {
                if (podColor == "White") {
                    cmdpdfprint = "PrintPDF.bat " + FsFile + ' "' + whitePrinter + '"';
                }
                else {
                    cmdpdfprint = "PrintPDF.bat " + FsFile + ' "' + blackPrinter + '"';

                }
            }
            else {
                cmdpdfprint = "PrintPDF.bat " + FsFile + " 4000";
            }
			
            if (!Fs.FileExists(FsFile)) {
                alert(msgPDFFileNull1 + " " + pdfFileName + " " + msgPDFFileNull2);
            }
            else {
                pdfFlag = true;
                wsh.run("cmd /k " + getHomeDisk(printerpath) + "&cd " + printerpath + "&" + cmdpdfprint + "&exit", 2, false);
            }
            wsh = null;

        }
        // ********************For Print POD using ********************
    </script>

</asp:Content>
