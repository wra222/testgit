<%--
* INVENTEC corporation ©2011 all rights reserved. 
* Description:UI for ConmbineTPM Page
* UI:CI-MES12-SPEC-FA-UI ConmbineTPM.docx –2011/10/11 
* UC:CI-MES12-SPEC-FA-UC ConmbineTPM.docx –2011/10/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   yang jie               (Reference Ebook SourceCode) Create
* 2012-01-07   Zhangkaisheng           modify
* Known issues:
 --%>

 
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="CollectTabletFaPart.aspx.cs" Inherits="FA_CollectTabletFaPart" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceCollectTabletFaPart.asmx" />
             <asp:ServiceReference Path="~/Service/PrintSettingService.asmx" />
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
                <legend align="left" style="height: 20px">
                </legend>
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
		                    <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="showPrintSettingDialog()" />
		                    <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
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
        var msgNoVendorCT =  '<%=this.GetLocalResourceObject(Pre + "_msgNoVendorCT").ToString()%>';
        var lstPrintItem;
        var accountid;
        var login;
        var havePrintItem = false;
        window.onload = function() 
        {
            tbl = "<%=GridViewExt1.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';

             accountid = '<%=AccountId%>';
          
             login = '<%=Login%>';
             PrintSettingService.GetPrintInfo(pCode, onGetPSucceed, onGetPFail); 
        }

        window.onbeforeunload = function() 
        {
            OnCancel();
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
            productID = "";
            callNextInput();
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
            if (line == "") 
            {
                ShowInfo(mesNoSelPdLine);
                //callNextInput();
                //alert(errmsg);

            }
            else if (lstPrintItem == null && havePrintItem) {
                alert(msgPrintSettingPara);

            }
            else if (data == "7777") 
            {
                //执行Reset
                ResetPage();
            }
            else if (customerSN == "") 
            {
                //刷入CustomerSN
                inputCustomSNorProdId(data);
            }

          
            else 
            {
                //刷入VenderCT
                inputVenderCT(data);
            }

            callNextInput();
        }

        function inputCustomSNorProdId(inputData) 
        {
            //        //if (!(inputData.substring(0, 3) == "SCN" || inputData.substring(0, 2) == "CN"))
            if (!((inputData.length == 9) || (inputData.length == 10)))
            {
                //alert(msgNoCustSN);
                ShowInfo(msgNoCustSN);
                callNextInput();
            }
            else 
            {
                var line = getPdLineCmbValue();
                customerSN = inputData.trim(); //inputData.substring(inputData.length - 10, 10);
                beginWaitingCoverDiv();
                WebServiceCollectTabletFaPart.inputSN(customerSN, line, station, editor, station, customer, onCustSNSucc, onCustSNFail);
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
            inputFlag = false;
            getPdLineCmbObj().disabled = false;
            callNextInput();
        }
        
        function setInfo(info) 
        {
            //set value to the label
            productID = info[1].id;//["id"];
            model = info[1].modelId;//["modelId"];
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

        function setTable(info) 
        {

            bomList = info[2];
            passQty = 0;
            totalQty = 0;
            var vclist = "";
            for (var i = 0; i < bomList.length; i++) {

                var rowArray = new Array();
                var rw;
                //rowArray.push(bomList[i]["type"]);
                //rowArray.push(bomList[i]["description"]);
                //vclist = getvcstring(bomList[i]["parts"][0]["properties"]);
                //rowArray.push(vclist);
                //rowArray.push(bomList[i]["qty"]);
                //rowArray.push(bomList[i]["scannedQty"]);//0?
                //rowArray.push(" ");
                //totalQty =totalQty + parseInt(bomList[i]["qty"], 10);
                //passQty = passQty + parseInt(bomList[i]["scannedQty"], 10);//=0??
                if ((bomList[i].type == null) || (bomList[i].type == "")) {
                    rowArray.push("Null");
                }
                else {
                    rowArray.push(bomList[i].type);                     //0
                }
                if ((bomList[i].description == null) || (bomList[i].description == "")) {
                    rowArray.push("None");
                }
                else {
                    rowArray.push(bomList[i].description);              //1
                }
                //PartNoItem
                //vclist = getvcstring(bomList[i].parts);
                //if ((vclist == null) || (vclist == "")) {
                //    rowArray.push("None");
                //}
                //else {
                //    rowArray.push(vclist);                              //2
                //}
                if ((bomList[i].PartNoItem == null) || (bomList[i].PartNoItem == "")) {
                    rowArray.push("None");
                }
                else {
                    rowArray.push(bomList[i].PartNoItem);              //1
                }
                if ((bomList[i].qty == null) || (bomList[i].qty.toString() == "")) {
                    rowArray.push("0");
                }
                else {
                    rowArray.push(bomList[i].qty.toString());           //3
                }
                if ((bomList[i].scannedQty == null) || (bomList[i].scannedQty.toString() == "")) {
                    rowArray.push("0");
                }
                else {
                    rowArray.push(bomList[i].scannedQty.toString());    //0?//4
                }
                rowArray.push(" ");                                 //5
                totalQty = totalQty + bomList[i].qty; //yparseInt(bomList[i].qty, 10);
                passQty = passQty + bomList[i].scannedQty; //parseInt(bomList[i].scannedQty, 10);//=0??
                
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
                    if (subFindFlag) 
                    {
                        //newScanedQty = parseInt(oldScanedQty, 10) + 1;
                        //if (newScanedQty > parseInt(rowQty, 10)) {
                        //if (scanQty == parseInt(qty, 10)) {
                        var scanfinish = CheckQtyScanQty();
                        if ((passQty >= totalQty) || (scanfinish == true))
                        {
                            beginWaitingCoverDiv();
                            WebServiceCollectTabletFaPart.save(productID, lstPrintItem,onSaveSuccess, onSaveFail);
                        }
                    }
                    else 
                    {
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

        function onSaveSuccess(result) {
            if (result[0] == SUCCESSRET) {
                //DEBUG ITC-1360-0563. remove msgbox info
                //ShowMessage(msgSuccess);
                //ShowInfo(msgSuccess);
                endWaitingCoverDiv();
                if (havePrintItem) {
                    setPrintItemListParam(result[1], result[1][0].LabelType, customerSN);
                    printLabels(result[1], false);
                }
           
                ShowSuccessfulInfo(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgSuccess);
                //result[1][0].LabelType
                //initPage();
                //getPdLineCmbObj().disabled = false;
                
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
            var url = "RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; ;
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

    </script>

</asp:Content>
