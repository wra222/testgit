<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Pizza
* UI:CI-MES12-SPEC-PAK-UI CombinePizza.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC CombinePizza.docx –2011/10/20 
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   liuqingbiao           Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombinePizza.aspx.cs" Inherits="PAK_CombinePizza" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="Service/WebServiceCombinePizza.asmx" />
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
            <hr />
            <fieldset style="width: 100%">
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
                        <td>
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset> 
            
            <fieldset style="width: 100%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblCollectionData" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>   
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3"  AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>   
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                 InputRegularExpression="^[-0-9a-zA-Z#\+\s\*]*$" Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var msgOceanShipping = '<%=this.GetLocalResourceObject(Pre + "_msgOceanShipping").ToString()%>';
        var msgInputCooLabelSuccess = '<%=this.GetLocalResourceObject(Pre + "_msgInputCooLabelSuccess").ToString()%>';
        var msgCoolableCustSNDoesnotMatchPizzaLabel = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCoolableCustSNDoesnotMatchPizzaLabel").ToString() %>';
        var msgPizzaLabelCanNotBeFound = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPizzaLabelCanNotBeFound").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgQualityCheck = '<%=this.GetLocalResourceObject(Pre + "_msgQualityCheck").ToString() %>';
        var msgInputWarrantyCard = '<%=this.GetLocalResourceObject(Pre + "_msgInputWarrantyCard").ToString() %>';
        var msgWarrantySNNotMatch = '<%=this.GetLocalResourceObject(Pre + "_msgWarrantySNNotMatch").ToString() %>';
        var msgModelWeightError = '<%=this.GetLocalResourceObject(Pre + "_msgModelWeightError").ToString() %>';
        var isHaveModelWeightErr = "";
        var inputFlag = 0;
        var scanFlag = false;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        //var gprodId = "";msgModelWeightError
        var __current_prodID = "";
        var __current_input_custSN_on_pizza_label = "";

        var PDLINE_CLEAR = 1;
        var PDLINE_KEEP = -1;
        
		var editor;
        var customer;
        var stationId;
        var curStation = "";
        var inputObj;
        var emptyPattern = /^\s*$/;

        var successtmp = "";
        
        var checkwarrantycardflag = "";

        window.onload = function() {
            try {
                tbl = "<%=gd.ClientID %>";
                inputObj = getCommonInputObject();
                getAvailableData("input");
                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                stationId = '<%=Request["Station"] %>';
                setPdLineCmbFocus();
            }
            catch (e) {
                alert(e.description);
            }
        };

        function IsCustomerSN_OnPizzaLabel(data) {

        //if ((data.toString().length == 11) && ((data.substring(0, 3) == 'PCN') || (data.substring(0, 3) == 'ACN')) && (data.substring(7, 8) != 'V') && (data.substring(7, 8) != 'W') && (data.substring(7, 8) != 'X') && (data.substring(7, 8) != 'Y') && (data.substring(7, 8) != 'Z'))
		if ((data.toString().length == 11) && CheckCustomerSN(data.toString().substr(1,10)) && ((data.substring(0, 1) == 'P') || (data.substring(0, 1) == 'A')) && (data.substring(7, 8) != 'V') && (data.substring(7, 8) != 'W') && (data.substring(7, 8) != 'X') && (data.substring(7, 8) != 'Y') && (data.substring(7, 8) != 'Z'))
            {
                return true;
            }
            
            return false;
        }
        
        function IsRightCustomerSN_OnCooLabel(data, customerSN)
        {
            var got_Sn;
            if (data.toString().length == 11) { got_Sn = data.substring(1, 11); }
            else { got_Sn = data; }
            
            if (got_Sn == customerSN) return true;
            
            return false;
        }

        function IsRightCustomerSN_OnWarrantyCard(data, customerSN) {
//            var got_Sn;
//            if (data.toString().length == 11) { got_Sn = data.substring(1, 11); }
//            else { got_Sn = data; }

//            if (got_Sn == customerSN) return true;

            //            return false;
            if (data.toString().length == 13 && data.toString().substring(0, 3) == "SN*" && data.substring(3, 13) == customerSN)
            { return true; }
            else
            { return false; }
        }

        function input(data) {
            
            if (inputFlag != 0) {
                if (check_if_can_jian_liao_finished() == true) 
                {
                    var custSN_got_from_page = document.getElementById("<%=lblCustSNContent.ClientID %>").innerText;

                    if (IsRightCustomerSN_OnWarrantyCard(data, custSN_got_from_page) == false) {
                        /*1. check input customer SN on Warranty Card if same as the one 
                        we just inputed SN on Pizza label, if does not the same,
                        return "Warranty Card SN do not match!". 
                        */
                        ShowMessage(msgWarrantySNNotMatch);
                        ShowInfo(msgWarrantySNNotMatch);
                        endWaitingCoverDiv();
                        getAvailableData("input");
                        inputObj.focus();
                        
                    }
                    else {
                        //checkwarrantycardflag = "";
                        var prodId = document.getElementById("<%=lblProductIDContent.ClientID%>").innerText;
                        WebServiceCombinePizza.save(prodId, saveSucceed, saveFail);
                        getPdLineCmbObj().disabled = false;
                        inputFlag = 0;

                    }

                }
            }
            if (checkwarrantycardflag == "") {
                if (data == "7777") {
                    var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;
                    if (prodId != "") {
                        WebServiceCombinePizza.cancel(prodId);
                    }
                    initPage(PDLINE_CLEAR);
                    if (getPdLineCmbObj().disabled == true) {
                        getPdLineCmbObj().disabled = false;
                    }
                    getPdLineCmbObj().selectedIndex = 0;
                    setPdLineCmbFocus();
                    getAvailableData("input");
                    return;
                }

                if (inputFlag > 0) {
                    if (IsCustomerSN_OnPizzaLabel(data)) // 1st input data.
                    {
                        // here cancel current process.
                        var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;
                        if (prodId != "") {
                            WebServiceCombinePizza.cancel(prodId);
                        }
                        // set flag.
                        //inputFlag = 0;
                        initPage(PDLINE_KEEP); // inputFlag = 0 has been in it.
                    }
                }

                if (inputFlag == 1) {
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    //data = "30012444Q"; //"1034000034";
                    //data = SubStringSN(data, "ProdId");
                    var custSN_got_from_page = document.getElementById("<%=lblCustSNContent.ClientID %>").innerText;
                    if (IsRightCustomerSN_OnCooLabel(data, custSN_got_from_page) == false) {
                        /*1. check input customer SN on COO Label if same as the one 
                        we just inputed SN on Pizza label, if does not the same,
                        return "customer SN do not match!". 
                        */
                        ShowMessage(msgCoolableCustSNDoesnotMatchPizzaLabel);
                        ShowInfo(msgCoolableCustSNDoesnotMatchPizzaLabel);
                        endWaitingCoverDiv();
                        getAvailableData("input");
                        inputObj.focus();
                    }
                    else {
                        /*
                        2. Get part List.
                        If get part list success, then set the: inputflag = inputflag + 1;
                        now the value of the inputflag == 2, so jiaoLiao process is made active :)
                        */
                        //var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;
                        //WebServiceCombinePizza.inputSN_OnCooLabel(prodId, data, inputSNCooLabelSucc, inputSNCooLabelFail);
                        inputSNCooLabelSucc();
                    }
                }
                else if (inputFlag == 0) {
                    var line = getPdLineCmbValue();
                    if (line == "") {
                        alert("Please input pdline first.");
                        getAvailableData("input");
                        inputObj.focus();
                        return;
                    }
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    //data = "30012444Q"; //"1034000034";

                    /* Call interface of input [Cunstomer SN] on pizza label.
                    inner this interface, it deals with the following two functions:
                    1. special check.
                    2. get product info
                    then in the successful return function of this interface - inputSucc, we display them.
                    and in successful return function, we set flag to flag that we have successfully 
                    inputed it, and we can do the following actions, e.g. input another and do with it.
                    */
                    if (IsCustomerSN_OnPizzaLabel(data)) {
                        //data = SubStringSN(data.substring(1, 11), "ProdId");
                        data = data.substring(1, 11);
                        __current_input_custSN_on_pizza_label = data;
                        //WebServiceCombinePizza.inputSN_OnPizzaLabel(line, data, editor, stationId, customer, inputSN_OnPizzaLabelSucc, inputSN_OnPizzaLabelFail);
                        //WebServiceCombinePizza.inputSN(string custSN, string pdLine, string curStation, string editor, string station, string customer);

                        if (stationId == "") stationId = "8C"; // when test on my machine, it will be useful. :)
                        //stationId = "8C";
                        //data = "CNZ8C00012";

                        curStation = stationId;
                        WebServiceCombinePizza.inputSN(data, line, curStation, editor, stationId, customer, inputSN_OnPizzaLabelSucc, inputSN_OnPizzaLabelFail);
                    }
                    else {
                        ShowMessage(msgPizzaLabelCanNotBeFound);
                        ShowInfo(msgPizzaLabelCanNotBeFound);
                        endWaitingCoverDiv();
                        inputObj.focus();
                        getAvailableData("input");
                    }
                }
                else // inputFlag > 1  -- jian liao.
                {
                    var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;
                    WebServiceCombinePizza.jianLiao(prodId, data, JianLiaoSucc, JianLiaoFail);
                    getAvailableData("input");
                    inputObj.focus();
                }
            }
        }

        function CheckQCStatusSucc(result) {
            if (result[0] == SUCCESSRET) {
                var tmp = "";
                if (result[1] == "8") {

                    //ShowMessage(msgQualityCheck);
                    tmp = msgQualityCheck.replace("PAQC", "SPAQC");
                }
                else if (result[1] == "B") {
                     tmp = msgQualityCheck;
                }
                else if (result[1] == "C") {
                    tmp = msgQualityCheck.replace("PAQC", "HP PAQC");
                }
                successtmp = successtmp + "\n" + tmp;
                if (isHaveModelWeightErr != "")
                { successtmp = successtmp + "," + msgModelWeightError; }
                
                ShowSuccessfulInfo(true, successtmp);
           
            }
                   
        }

        function CheckQCStatusFail(result) {

        }

        function CheckWarrantyCardSucc(result) {
            if (result[0] == SUCCESSRET) {
                var tmp = "";
                if (result[1] != "") {
                    checkwarrantycardflag = "needcheck";
                  //  ShowMessage(msgInputWarrantyCard); //请刷入Warranty Card 上的Customer S/N!
                    ShowInfo(msgInputWarrantyCard);
                    inputObj.focus();
                    getAvailableData("input");
                   
                }
                else {
                    checkwarrantycardflag = "";
                    var prodId = document.getElementById("<%=lblProductIDContent.ClientID%>").innerText;
                    WebServiceCombinePizza.save(prodId, saveSucceed, saveFail);
                    getPdLineCmbObj().disabled = false;
                    inputFlag = 0;
               
                }
            }

        }

        function CheckWarrantyCardFail(result) {

        }      
         
        function inputSN_OnPizzaLabelSucc(result) 
        {
            var has_set_flag = false;
            if (result[0] == SUCCESSRET) 
		    {
		        endWaitingCoverDiv();
		        defectInTable = result[2];
		        setInfo(result);
		        
		        if (defectInTable.length == 0) 
		        {
		            //var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;
		            //if (prodId != "") 
		            //{
		            //    WebServiceCombinePizza.cancel(prodId, cancel_proc_succeed, cancel_proc_fail);
		            //    //initPage();
		            //}
		            //ShowInfo("To this custSN, we need not do anything, please input other valid custSN!");
		            //has_set_flag = true;
		        }
		        if (result[3] == true) 
		        {
		            //ShowMessage(msgOceanShipping);
		            ShowInfo(msgOceanShipping);
		        }
		        isHaveModelWeightErr = result[4];
		    }
		    else 
		    {
		        endWaitingCoverDiv();
		        var content = result[0];
		        ShowMessage(content);
		        ShowInfo(content);
		    }
		    
		    if (has_set_flag == false) inputFlag = inputFlag + 1;
            getAvailableData("input");
            inputObj.focus();
        }

        function cancel_proc_succeed(result) 
        {
            var xxx = result;
        }

        function cancel_proc_fail(result) 
        {
            var xxx = result;
        }

        function inputSN_OnPizzaLabelFail(result) 
		{
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            endWaitingCoverDiv();
            getAvailableData("input");
            inputObj.focus();
        }

		function inputSNCooLabelSucc() //result
		{
			// in success, that is to say, two SN same, and have got list,  do display.
			// 1. display.
		    // 2. set focus to Data Entry.
		    ShowInfo(msgInputCooLabelSuccess);
		    //Display_Table(result);
		    endWaitingCoverDiv();
		    
		    // -----------------------------------
		    if (check_if_can_jian_liao_finished() == true) {
		    
		        var prodId = document.getElementById("<%=lblProductIDContent.ClientID%>").innerText;
		        beginWaitingCoverDiv();
		        WebServiceCombinePizza.save(prodId, saveSucceed, saveFail);
		        endWaitingCoverDiv();
		        inputFlag = 0;
		    }
		    // -----------------------------------
		    else 
		    {
		        getPdLineCmbObj().disabled = true;
		        inputFlag = inputFlag + 1;
		    }
		    
		    getAvailableData("input");
		    inputObj.focus();
		}
		
		function inputSNCooLabelFail(result)
		{
		    // here		    
		    ShowMessage(result.get_message());
		    ShowInfo(result.get_message());
		    Display_Table(result);
		    endWaitingCoverDiv();
		    getAvailableData("input");
		    inputObj.focus();
		}

		function check_if_can_jian_liao_finished()
		{
		    var ret = true;
		    //defectInTable[0]["scannedQty"] = 1;
		    for (var i = 0; i < defectInTable.length; i++)
		        //for (var i = 0; i < 2; i++)
            
            {
                if (defectInTable[i]["qty"] != defectInTable[i]["scannedQty"])
                {
                    ret = false; break;
                }
            }
                
            return ret;		   
		}

		var __current_highlight = -1;
		function JianLiaoSucc(result) 
		{
		    var findIndex = updateTable(result);
		    if (findIndex == -1) { ShowInfo("error!"); return; }

		    if (__current_highlight >= 0) setRowSelectedOrNotSelectedByIndex(__current_highlight, false, "<%=gd.ClientID %>");
		    setTable(defectInTable, findIndex);
		    setRowSelectedOrNotSelectedByIndex(findIndex, true, "<%=gd.ClientID %>");__current_highlight = findIndex;
		    //ShowInfo("pick a matierial:" + result.PNOrItemName + " success.");
		    ShowInfo("");
			var bFinished = check_if_can_jian_liao_finished();
			if (bFinished == true) {
			    var prodId = document.getElementById("<%=lblProductIDContent.ClientID%>").innerText;
			
			    WebServiceCombinePizza.CheckWarrantyCard(prodId, CheckWarrantyCardSucc, CheckWarrantyCardFail);

			}
		}

		function JianLiaoFail(result) 
		{
		    ShowMessage(result.get_message());
			ShowInfo(result.get_message());
		}

		function setInfo(info) 
		{
		    //set value to the label
		    productID = info[1]["id"];
		    model = info[1]["modelId"];
		    customerSN = info[1]["customSN"];
		    pizzaID = info[1]["pizzaId"];
		    setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
		    __current_prodID = productID;
		    setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
		    setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), customerSN);
		    setTable(info[2], -1);
		}

		function setTable(info, updateIndex) {

		    var bomList = info;

		    for (var i = 0; i < bomList.length; i++) 
		    {
		        if (updateIndex == -1) 
		        {
		        }
		        else 
		        {
		            if (updateIndex != i) continue;
		        }
		        
		        var rowArray = new Array();
		        var rw;
		        var collection = bomList[i]["collectionData"];
		        var parts = bomList[i]["parts"];
		        var tmpstr = "";

		        //for (var j = 0; j < parts.length; j++) {
		        //    tmpstr = tmpstr + " " + parts[j]["id"];
		        //}
		        if (bomList[i]["PartNoItem"] == null) 
		        {
		            tmpstr = " ";
		        }
		        else 
		        {
		            tmpstr = bomList[i]["PartNoItem"];
		            // wu.de-hong, said that: 
		            //     UC上有一句：@PartNo 中的Part No 如果是以'DIB' 为前缀的时候，需要删除该前缀
                    // Liu, Qing-Biao (劉慶彪 ITC) [16:51]:
                    //     应该在 service 里改，还是在 asp 端？
                    // Wu, De-Hong (吳德宏 ITC) [16:51]:
                    //     如果在改变PartNo值，就麻烦了。
		            //     只是显示时，不显示罢了
		            if ((tmpstr.length > 3) && (tmpstr.substring(0, 3) == "DIB")) 
		            {
		                tmpstr = tmpstr.substring(3);
		            }
		            tmpstr = tmpstr.replace(",DIB", ",");
		            // fix the issue end.
		        }
		        tmpstr = add_changeline_symbol(tmpstr);
		        rowArray.push(tmpstr); //part no/name

                if ((bomList[i]["tp"] == null) || (bomList[i]["tp"] == ""))
                {
                    rowArray.push(" ");
                }
                else
                {
		            rowArray.push(bomList[i]["tp"]); //"type"// must modified into "tp";
		        }
		        
		        if (bomList[i]["description"] == null) 
		        {
		            rowArray.push(" ");
		        }
		        else 
		        {
		            rowArray.push(bomList[i]["description"]);
		        }
		        rowArray.push(bomList[i]["qty"]);
		        rowArray.push(bomList[i]["scannedQty"]);
		        coll = "";
		        
		        tmpstr = bomList[i]["collectionData"];
		        rowArray.push(tmpstr); //["collectionData"]);

		        //add data to table
		        if (i < 12) 
		        {
		            eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
		            if (updateIndex != -1) 
		            {
		                //setSrollByIndex(i, true, tbl);
		            }
		        }
		        else {
		            eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
		            if (updateIndex != -1) 
		            {
		                setSrollByIndex(i, true, tbl);
		            }
		            rw.cells[1].style.whiteSpace = "nowrap";
		        }
		    }

		    //if ((bomList.length > 0) && (updateIndex == -1))
		    //{
		    //    setSrollByIndex(0, true, tbl);
		    //}
		}
		
		/*
		function setTable(info) 
		{

		    var bomList = info;
		    defectInTable = bomList;

		    for (var i = 0; i < bomList.length; i++) 
		    {

		        var rowArray = new Array();
		        var rw;

		        rowArray.push(bomList[i]["parts"][0]["id"]); //part no/name
		        rowArray.push(bomList[i]["type"]);
		        rowArray.push(bomList[i]["description"]);
		        rowArray.push(bomList[i]["qty"]);
		        rowArray.push(bomList[i]["scannedQty"]);
		        rowArray.push(bomList[i]["collectionData"][0]["pn"]); //["collectionData"]);

		        //add data to table
		        if (i < 12) 
		        {
		            eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
		            setSrollByIndex(i, true, tbl);
		        }
		        else 
		        {
		            eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
		            setSrollByIndex(i, true, tbl);
		            rw.cells[1].style.whiteSpace = "nowrap";
		        }
		    }
		}
		*/

		function saveSucceed(result) 
		{
		    successtmp = "[" + "customerSN:" + __current_input_custSN_on_pizza_label + "] " + msgSuccess;
		
		    var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;
		    WebServiceCombinePizza.CheckQCStatus(prodId, CheckQCStatusSucc, CheckQCStatusFail);
		    //tmp = tmp + "\n" + successtmp;
		    initPage(PDLINE_KEEP);
		    //ShowSuccessfulInfo(true, tmp);
		}

		function saveFail(result) 
		{
		    initPage(PDLINE_KEEP);
		    ShowMessage(result.get_message());
		    ShowInfo(result.get_message());
		}

		function Display_Table(result)
		{
			//ShowInfo("Display table now....");
		}

		function updateTable(result)
		{
		    var ret = -1;

            var found = 0;
            for (var i = 0; i < defectInTable.length; i++)
            {
                var ok = 0;
                for (var j = 0; j < defectInTable[i]["parts"].length; j++)
                {
                    if (defectInTable[i]["parts"][j]["id"] == result["PNOrItemName"]) 
                    {
                        if (defectInTable[i]["type"] == result["ValueType"]) 
                        {
                            ret = i;
                            ok = 1;
                            if (defectInTable[i].scannedQty < defectInTable[i].qty)
                            {
                                defectInTable[i].scannedQty++;
                                defectInTable[i].collectionData += result["CollectionData"] + " ";
                            }
                            else
                            {
                                defectInTable[i].collectionData = result["CollectionData"];
                            }
                            //setSrollByIndex(i, true, "<%=gd.ClientID%>");
                            break;
                        }
                        else 
                        {
                            var xx = 0;
                        }
                    }
                }
                
                if (ok == 1) {found = 1; break;}
            }
            //if (found == 1) { ret = true; }
            
            return ret;
        }

        function add_changeline_symbol(data) 
        {
            var count = 0;
            var i = 0;
            var index = 0;
            var retStr = "";

            while (1) 
            {
                index = 0;
                count = 0;
                i = 0;
                for (i = 0; i < data.length; i++) 
                {
                    if (data.substring(i, i + 1) == ',') 
                    {
                        count++;
                        if (count == 3) index = i;
                    }
                }

                if (count <= 2) { retStr += data; return retStr; }
                if (index > 1) 
                {
                    retStr += data.substring(0, index) + " \r\n";
                    data = data.substring(index + 1, data.length);
                    continue;
                }
            }
        }

		function initPage(pdline_select) 
		{
		    tbl = "<%=gd.ClientID %>";
		    if (pdline_select == PDLINE_CLEAR) 
		    {
		        if (getPdLineCmbObj().disabled == false) 
		        {
		            getPdLineCmbObj().selectedIndex = 0;
		        }
		    }
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            __current_prodID = "";
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            // the line following disable last set highlight item in the table.
            eval("setRowNonSelected_" + tbl + "()"); 
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = 0;
            defectCount = 0;
            defectInTable = [];
            ShowInfo("");
            checkwarrantycardflag = "";
        }

        window.onbeforeunload = function() 
		{
            //if (inputFlag) 
			//{
                OnCancel();
            //}
        };

        function OnCancel() 
		{
		    if (__current_prodID != "") 
			{
			    WebServiceCombinePizza.cancel(__current_prodID);
			    sleep(waitTimeForClear);
            }
        }

        function ExitPage() 
		{
            OnCancel();
        }

        function ResetPage(pdline_select) 
		{
		    ExitPage();
            initPage(pdline_select);
            ShowInfo("");
        }

    </script>

</asp:Content>
