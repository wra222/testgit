<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Pizza Kitting
* UI:CI-MES12-SPEC-PAK-UI Pizza Kitting.docx –2011/11/21 
* UC:CI-MES12-SPEC-PAK-UC Pizza Kitting.docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/04/10    Du.Xuan               Create   
* ITC-1360-1742 取消Input [Pizza Id] 步骤
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PAKReviewSorting.aspx.cs" Inherits="PAK_PAKReviewSorting" Title="PAKReviewSorting" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePizzaKitting.asmx" />
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
                      
                              <asp:Label runat="server" ID="Label1" Text="Upload SN:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                           <input id="BtnBrowse" onclick="UploadCustsnList()" type="button"   style ="width:110px; height:24px;" 
                             value="Input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
            </table>
            <hr />
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                            <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblCustSNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIDContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPizza" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPizzaContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True" >
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
                                 InputRegularExpression="^[-0-9a-zA-Z#\+\s\*]*$" Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" id="modelHidden"  runat="server" />
                <input type="hidden" id="customerHidden" runat="server"/>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var customer = "";
        var pCode = "";
        var stationId="";
        var inputObj;
        var emptyPattern = /^\s*$/;
        var customerSN = "";
        var pizzaID = "";
        var inPizzaID = "";
        var productID = "";
        var model = "";
        var iSelectedRowIndex = -1;
        var needReset = false;
        var snOnPizza="";
        var checkStation = "PKCK";
   
        //error message
        var msgInputLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine").ToString() %>';
        var msgInputStation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputStation").ToString() %>';
        var msgInputPart = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPart").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgNoCustSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputCustSN").ToString() %>';
        var msgNotFinish = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNotFinish").ToString() %>';
        var msgPartFinish = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPartFinish").ToString() %>';
        var msgAllFinish = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAllFinish").ToString() %>';
        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            checkStation = stationId;
            initPage();
            callNextInput();
        };

        window.onbeforeunload = function() {
                OnCancel();
        };
        function initPage() {

            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblPizzaContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            productID = "";
            pizzaID = "";
            inPizzaID = "";
            customerSN = "";
            iSelectedRowIndex = -1;
            needReset = false;
            //getPdLineCmbObj().selectedIndex = 0;
            //callNextInput();
        }

        function input(data) {

            var line = "";
            var station = checkStation;

          
            if (data == "7777") {
                //执行Reset
                ShowInfo("");
                ResetPage();
                callNextInput();
                return;
            }
            
            if (needReset == true) {
                ShowInfo("");
                ResetPage();
            }
            
            if (customerSN == "") {
                //刷入CustomerSN
                inputCustomSN(data);
            }
            else {
//                if (checkCustomSN(data)) {
//                    ResetPage();
//                    inputCustomSN(data);
//                    return;
//                }
				if(snOnPizza=="")
				{
				    checkSnOnPizza(data);
				    callNextInput();
				    return;
				}
                /*if (inPizzaID == "") {

                    //刷入PizzaID
                    inputPizzaID(data);
                }
                else {

                    checkPart(data);
                }*/
                checkPart(data);
            }

            callNextInput();
        }
        function checkSnOnPizza(inputData) {
            ShowInfo("");
		    if(inputData.length != 11 || inputData.substr(1,10)!=customerSN)
			{alert("Wrong sn on Pizza");}
			else
			{ShowInfo("Correct sn on pizza, please scan part no!");   snOnPizza=inputData;}
		}
        function checkCustomSN(inputData) {
            var flag = false;
            //if (inputData.length == 10 && inputData.substring(0, 2) == "CN") {
			if (inputData.length == 10 && CheckCustomerSN(inputData)) {
                flag = true;
            }
            //else if (inputData.length == 11 && (inputData.substring(0, 3) == "SCN" || inputData.substring(0, 3) == "PCN" || inputData.substring(0, 3) == "ACN")) {
			else if (inputData.length == 11 && CheckCustomerSN(inputData.substr(1,10)) && (inputData.substr(0,1) == "S" || inputData.substr(0,1) == "P" || inputData.substr(0,1) == "A")) {
                flag = true;
            }
            return flag;
        }

        function inputCustomSN(inputData) {
        
            //if (inputData.length == 10 && inputData.substring(0, 2) == "CN")
			if (inputData.length == 10 && CheckCustomerSN(inputData))
            {
                inputData = inputData; 
            }
            //else if (inputData.length == 11 && (inputData.substring(0, 3) == "SCN" || inputData.substring(0, 3) == "PCN" || inputData.substring(0, 3) == "ACN"))
			else if (inputData.length == 11 && CheckCustomerSN(inputData.substr(1,10)) && (inputData.substr(0,1) == "S" || inputData.substr(0,1) == "P" || inputData.substr(0,1) == "A"))
            {
                inputData = inputData.substring(1, 11);
            }
            else
            {
                alert(msgNoCustSN);
                callNextInput();
                return;
            }
            

            var line = "";
            var station = checkStation;
            customerSN = inputData.substring(inputData.length - 10, 10);
            beginWaitingCoverDiv();
            WebServicePizzaKitting.inputPizzaCheckSNforSorting(line, station, customerSN, editor, stationId, customer, onCustSNSucc, onCustSNFail);
            
        }

        function onCustSNSucc(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            if (result[0] == SUCCESSRET) {
                ShowInfo("Please sn on pizza!!");
                defectInTable = result[2];
                setInfo(result);
                setTable(defectInTable);
                inputFlag = true;
            }
            else {
                ResetPage();            
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onCustSNFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            customerSN = "";
            callNextInput();
        }

        function setInfo(info) {
            //set value to the label
            productID = info[1]["id"];
            model = info[1]["modelId"];
            customerSN = info[1]["customSN"];
            pizzaID = info[1]["pizzaId"];
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), customerSN);
            setInputOrSpanValue(document.getElementById("<%=lblPizzaContent.ClientID %>"), pizzaID);
            //setInputOrSpanValue(document.getElementById("<%=modelHidden.ClientID%>"), model);
        }

        function setTable(info) {

            var bomList = info;

            for (var i = 0; i < bomList.length; i++) {

                var rowArray = new Array();
                var rw;
                var collection = bomList[i]["collectionData"];
                var parts = bomList[i]["parts"];
                var tmpstr="";

                /*for (var j = 0; j < parts.length; j++) {
                    tmpstr = tmpstr + " " + parts[j]["id"];
                }
                rowArray.push(tmpstr); //part no/name*/
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

        function inputPizzaID(inputData) {
        
            ShowInfo("");
            if (inputData.length == 10) {
                var line = "";
                var station = checkStation;
                inPizzaID = inputData.substring(0,9); 
                //pizzaID = "10050000067"; //inputData.substring(0, 9);
                WebServicePizzaKitting.InputPizzaID(productID, inPizzaID, line, station, model, editor, stationId, customer, onPizzaIDSucc, onPizzaIDFail);
            }
            else {
                alert(msgNoCustSN);
                callNextInput();
            }
        }

        function onPizzaIDSucc(result) {

            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                setInputOrSpanValue(document.getElementById("<%=lblPizzaContent.ClientID %>"), pizzaID);
                ShowInfo(msgInputPart);
                callNextInput();
            }
            else {
                //ResetPage();
                inPizzaID = "";            
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                callNextInput();
            }
            
        }

        function onPizzaIDFail(result) {

            endWaitingCoverDiv();
            inPizzaID = "";
            //ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();

        }

        function checkPart(data) {

            /*var finished = checkPartFinished(data);
            if (finished) {
                alert(msgPartFinish);
                callNextInput();
                return;
            }
            */
            WebServicePizzaKitting.checkPart(productID, data, onCheckSuccess, onCheckFail);
        }

        function onCheckSuccess(result) {
            ShowInfo("");
            var index = updateTable(result[1]);
            if (index< 0) {
                ShowInfo("System error!");
                callNextInput();
                return; 
            }

            var con = document.getElementById(tbl).rows[index + 1];
            con.cells[4].innerText = defectInTable[index]["scannedQty"];
            con.cells[5].innerText = con.cells[5].innerText + " " + result[1]["PNOrItemName"];
            con.cells[5].title = con.cells[5].innerText;

            //setTable(defectInTable);
            var bFinished = checkFinished(index);
            if (bFinished == true) {

                beginWaitingCoverDiv();
                WebServicePizzaKitting.Save(productID, onSaveSucceed, onSaveFail);
                inputFlag = false;
            }
            callNextInput();
        }

        function onCheckFail(result) {
            ShowInfo(result.get_message());
            callNextInput();
        }

        function onSaveSucceed(result) {
            endWaitingCoverDiv();
            //alert(msgAllFinish);
			snOnPizza="";
			customerSN="";
            ShowInfo(msgAllFinish,"green");              
        }

        function onSaveFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function checkFinished(index) {
            var ret = true;

            //if (defectInTable[index]["qty"] == defectInTable[index]["scannedQty"]) {
            //    ret = true;
            //}
            
            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i]["qty"] != defectInTable[i]["scannedQty"]) {
                    ret = false; 
                    break;
                }
            }
            
            return ret;
        }
        
        function checkPartFinished(partno) {
            var ret = false;

            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i]["PartNoItem"] == partno)
                {
                    if (defectInTable[i]["qty"] == defectInTable[i]["scannedQty"]) {
                        ret = true;
                    }
                    else {
                        ret = false;
                    }
                    break;
                }
            }

            return ret;
        }
        
        function setGdHighLight(index) {
            if ((iSelectedRowIndex != -1) && (iSelectedRowIndex != index)) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, tbl);     //去掉过去高亮行           
            }
            setRowSelectedOrNotSelectedByIndex(index, true, tbl);     //设置当前高亮行
            iSelectedRowIndex = index;    //记住当前高亮行
        }

        function updateTable(result) {
            
            var index = -1;
            for (var i = 0; i < defectInTable.length; i++) {
                var found = -1;
                for (var j = 0; j < defectInTable[i]["parts"].length; j++) {
                    if (defectInTable[i]["parts"][j]["id"] == result["PNOrItemName"]) {
                        found = j;
                        defectInTable[i]["scannedQty"]++; 
                        break;
                    }
                }
                if (found >= 0) {
                    index = i;
                    break; 
                }
            }
            return index;
        }
        
        function OnCancel() {
            if (productID != "") {
                WebServicePizzaKitting.cancel(productID);
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

        function callNextInput() {

            getCommonInputObject().focus();
            getAvailableData("input");
        }
        // Read a page's GET URL variables and return them as an associative array.
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }
        function getUrlVars2() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function UploadCustsnList() {
            var paramArr = getUrlVars2();
            var a = paramArr["AccountId"];
            var ui = paramArr["UserId"];
            var un = paramArr["UserName"];
            var c = paramArr["Customer"];
            var lo = paramArr["Login"];
            var dlgFeature = "dialogHeight:630px;dialogWidth:650px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "InputSnListForPAKSorting.aspx?Editor=" + editor + "&Station=" + stationId +"&UserId=" + ui+
                                       "&UserName=" + un + "&AccountId=" + a + "&Login=" + lo + "&Customer=" + c;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);


        }

                                      
    </script>

</asp:Content>
