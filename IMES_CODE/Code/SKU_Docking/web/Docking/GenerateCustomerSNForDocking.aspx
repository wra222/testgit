<%--
/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:UI for Generate Customer SN For Docking Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-18   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1414-0064,Jessica Liu,2012-6-5
* ITC-1414-0181, Jessica Liu, 2012-6-19
* ITC-1414-0193, Jessica Liu, 2012-6-20
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ContentType="text/html;Charset=UTF-8" CodeFile="GenerateCustomerSNForDocking.aspx.cs" Inherits="FA_GenerateCustomerSNForDocking"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/GenerateCustomerSNForDockingService.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div id="bg" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <center>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="15%">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td >
                         <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                
				<tr>
                    <td >
                        <asp:Label ID="lblCategory" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td >
                         <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Category_SelectedIndexChanged" Width="150px">
						 <asp:ListItem Selected="True" Value="1">Normal</asp:ListItem>
						 <asp:ListItem Value="2">Change Model</asp:ListItem>
						 </asp:DropDownList>
                    </td>
                </tr>
					<tr id="divCategoryChangeQty" style="background-color:lightblue;display: none">
						<td>
							<asp:Label ID="lblChangeToQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
						</td>
						<td>
							<asp:TextBox ID="txtChangeToQty" runat="server" width="100px" SkinId="textBoxSkin"  MaxLength="4"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
						</td>
					</tr>
					<tr id="divCategoryChangeModel" style="background-color:lightblue;display: none">
						<td>
							<asp:Label ID="lblChangeToModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
						</td>
						<td>
							<asp:Label ID="txtChangeToModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
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
                <tr><td></td><td></td></tr>
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
                <tr>
                    <td >
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td >
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
							<input type="hidden" runat="server" id="changeToModel" />
							<input type="hidden" runat="server" id="changeToFamily" />
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
		var msgChangeModelQtyAllFinished = '<%=this.GetLocalResourceObject(Pre + "_msgChangeModelQtyAllFinished").ToString() %>';
		var msgModelNull = '<%=this.GetLocalResourceObject(Pre + "_msgModelNull").ToString() %>';
        
        var DEFAULT_ROW_NUM = 13;

        var editor = "";
        var customer = "";
        var station = "";


        var pdLine = "";
        var productID = "";
        var model = "";
        var configCode = "";
        var customerSN = "";

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
            //document.getElementById("<%=CmbPdLine1.ClientID %>").selectedIndex = 1;
            productID = "";
            model = "";
            ShowInfo("");
        }

        function checkInput(inputData) {
            ShowInfo("");
            inputData = inputData.trim().toUpperCase();
			var ddlC = document.getElementById("<%=ddlCategory.ClientID%>");

            if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
                alert(msgPdLineNull);
                //ShowInfo(msgPdLineNull);
                callNextInput();
            }
            else {
                if (inputData == "" || inputData == null || inputData.length < 9) {
                    if ('1'==ddlC.options[ddlC.selectedIndex].value)
						alert(msgProdIDNull);
                    else if ('2'==ddlC.options[ddlC.selectedIndex].value){//change Model
						if(''==document.getElementById("<%=txtChangeToModel.ClientID %>").innerText)
							alert(msgModelNull);
						else
							alert(msgProdIDNull);
					}
					//ShowInfo(msgProdIDNull);
                    callNextInput();
                }
                else {
                    if ('1'==ddlC.options[ddlC.selectedIndex].value)
						InputProdID(inputData);
					else if ('2'==ddlC.options[ddlC.selectedIndex].value){//change Model
						if(''==document.getElementById("<%=txtChangeToModel.ClientID %>").innerText){
							InputChangeToModel(inputData);
						}
						else{
							var qty = document.getElementById("<%=txtChangeToQty.ClientID %>").value;
							qty = qty.replace(/^\s+|\s+$/g, '');
							if(''==qty || isNaN(qty)){
								alert('Qty must be numeric.');
								callNextInput();
								return;
							}
							if(Number(qty)<1){
								alert('Qty must >= 1');
								callNextInput();
								return;
							}
							InputProdIDAndChangeModel(inputData, document.getElementById("<%=changeToModel.ClientID %>").value);
						}
					}
                }
            }

        }

        function InputProdIDAndChangeModel(prodID, newmodel) {

            //ITC-1414-0181, Jessica Liu, 2012-6-19
            var printItemlist = getPrintItemCollection();

            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                ResetPage();
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            pdLine = getPdLineCmbValue();
            //prodID = "101000001"; //"103000034";
           // prodID = SubStringSN(prodID, "ProdId");
            GenerateCustomerSNForDockingService.inputProdIdAndChangeModel(pdLine, prodID, station, editor, customer, newmodel, onInputSuccess, onInputFail);
        }
		
		function InputChangeToModel(model){
			beginWaitingCoverDiv();
			document.getElementById("<%=changeToModel.ClientID %>").value = model;
			GenerateCustomerSNForDockingService.GetModelFamily(model, onInputChangeToModelSuccess, onInputFail);
		}
		
		function onInputChangeToModelSuccess(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            try {
				document.getElementById("<%=changeToFamily.ClientID %>").value = result;
				document.getElementById("<%=txtChangeToModel.ClientID %>").innerText = document.getElementById("<%=changeToModel.ClientID %>").value;
            }
            catch (e) {
                alert(e.description);
            }

            callNextInput();
        }
		
		function ChkChangeModelQty(){
			var ddlC = document.getElementById("<%=ddlCategory.ClientID%>");
			if ('2'!=ddlC.options[ddlC.selectedIndex].value)
				return;
			var existQty = document.getElementById("<%=txtChangeToQty.ClientID %>").value;
			if(Number(existQty) < 1){
				alert(msgChangeModelQtyAllFinished);
				document.getElementById("<%=changeToModel.ClientID %>").value = '';
				document.getElementById("<%=changeToFamily.ClientID %>").value = '';
				document.getElementById("<%=txtChangeToQty.ClientID %>").value = '';
				document.getElementById("<%=txtChangeToModel.ClientID %>").innerText = '';
			}
		}
		
		function InputProdID(prodID) {

            //ITC-1414-0181, Jessica Liu, 2012-6-19
            var printItemlist = getPrintItemCollection();

            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                ResetPage();
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            pdLine = getPdLineCmbValue();
            //prodID = "101000001"; //"103000034";
          //  prodID = SubStringSN(prodID, "ProdId");
            GenerateCustomerSNForDockingService.inputProdId(pdLine, prodID, station, editor, customer, onInputSuccess, onInputFail);

        }

        function onInputSuccess(result) {

            endWaitingCoverDiv();
            ShowInfo("");
            try {

                if ((result.length == 4) && (result[0] == SUCCESSRET)) {
                    productID = result[1];
                    customerSN = result[3];
                    document.getElementById("<%=txtProdID.ClientID%>").innerText = result[1];
                    document.getElementById("<%=txtModel.ClientID%>").innerText = result[2];
					
					var ddlC = document.getElementById("<%=ddlCategory.ClientID%>");
					if ('2'==ddlC.options[ddlC.selectedIndex].value){//change Model
						var existQty = document.getElementById("<%=txtChangeToQty.ClientID %>").value;
						document.getElementById("<%=txtChangeToQty.ClientID %>").value = Number(existQty)-1;
					}
					
                    printProcess();
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
                    //Print(string prodId, IList<PrintItem> printItems)
                    GenerateCustomerSNForDockingService.Print(productID, printItemlist, onPrintSuccess, onPrintFail);
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

            //setPrintParam(lstPrtItem, "Customer SN Label", keyCollection, valueCollection);
            //ITC-1414-0064,Jessica Liu,2012-6-5
            //setPrintParam(lstPrtItem, "SN Label", keyCollection, valueCollection);
            /* 2012-6-20
            setPrintParam(lstPrtItem, "DK_SN_Label", keyCollection, valueCollection);
            */
            /* 2012-6-29, Mantis bug-1031
            if (document.getElementById("<%=radDock12.ClientID %>").checked) {
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
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                    //==========================================print process=======================================

                    setPrintItemListParam(result[1], customerSN);

                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    //2012-6-29, Mantis bug-1031
                    //printLabels(result[1], false);
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
			ChkChangeModelQty();
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
			ChkChangeModelQty();
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
                GenerateCustomerSNForDockingService.Cancel(productID, station);
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
