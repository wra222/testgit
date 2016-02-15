<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine COA and DN
 * CI-MES12-SPEC-PAK-Combine COA and DN.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
--%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CombineCOAandDNQuick.aspx.cs" Inherits="PAK_CombineCOAandDNQuick" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"  >
        <Services>  
			<asp:ServiceReference Path="Service/CombineCOAandDNWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script  type="text/javascript" language= "javascript ">   
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest (EndRequestHandler);   
        function   EndRequestHandler(sender,   args)   
        {   
            if   (args.get_error()   !=   undefined)   
            {
                //ShowMessage(args.get_error().message);
                //ShowInfo(args.get_error().message);
                if(args.get_error().message.substring(0,   51)   ==   "Sys.WebForms.PageRequestManagerParserErrorException")   
                { 
                    window.location.reload();   //出现Session丢失时的错误处理，可以自己定义。 
                }   
                else 
                { 
                    alert( "发生错误!原因可能是数据不完整,或网络延迟。 ");   //其他错误的处理。 
                } 
                args.set_errorHandled(true);   
            } 
        }   
    </script>


    <script type="text/javascript" language="javascript"  >
    var editor = '<%=userId%>';
    var customer = '<%=customer%>';
    var station = '<%=station%>';
    var pCode = '<%=code%>';
    var username = '<%=UserName%>';
    var login = '<%=Login%>';
    var accountid = '<%=AccountId%>';
    var msgInputPDLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
    var msgModelCheck = '<%=this.GetLocalResourceObject(Pre + "_msgModelCheck").ToString() %>';
    var msgModelNoFind = '<%=this.GetLocalResourceObject(Pre + "_msgModelNoFind").ToString() %>';
    var msgNeedCoa = '<%=this.GetLocalResourceObject(Pre + "_msgNeedCoa").ToString() %>';
    var msgNotNeedCoa = '<%=this.GetLocalResourceObject(Pre + "_msgNotNeedCoa").ToString() %>';
    var msgModeChange = '<%=this.GetLocalResourceObject(Pre + "_msgModeChange").ToString() %>';
    var msgCoaInput = '<%=this.GetLocalResourceObject(Pre + "_msgCoaInput").ToString() %>';
    var msgCoaCheck = '<%=this.GetLocalResourceObject(Pre + "_msgCoaCheck").ToString() %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgQualityCheck = '<%=this.GetLocalResourceObject(Pre + "_msgQualityCheck").ToString() %>';
    var msgNoCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgNoCDSI").ToString() %>';
    var msgNomal = '<%=this.GetLocalResourceObject(Pre + "_msgNomal").ToString() %>';
	var msgBSAM = '<%=this.GetLocalResourceObject(Pre + "_msgBsam").ToString() %>';
    var msgWin8 = '<%=this.GetLocalResourceObject(Pre + "_msgWin8").ToString() %>';
    var msgCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgCDSI").ToString() %>';
    var msgFNOWrong = '<%=this.GetLocalResourceObject(Pre + "_msgFNOWrong").ToString() %>';
    var msgException = '<%=this.GetLocalResourceObject(Pre + "_msgException").ToString() %>';
    var inputTextBox1;
    window.onload = function() {
        if ("" == pCode || null == pCode) {
            pCode = '<%=Request["PCode"] %>';
        }
        if ("" == station || null == station) {
            station = '<%=Request["Station"] %>';
        }
        document.getElementById("<%=HLine.ClientID%>").value = "";
        document.getElementById("<%=HClear.ClientID%>").value = "0";
        document.getElementById("<%=lastId.ClientID%>").value = 1;
        document.getElementById("<%=index.ClientID%>").value = 0;
        document.getElementById("<%=successTemp.ClientID%>").value = "";
        document.getElementById("<%=showTemp.ClientID%>").value = "";
        callNextInput();
    }
    function IsNumber(src)
    {
        var regNum = /^[0-9]+[0-9]*]*$/;
        return regNum.test(src);
    }

    function onGetDNByDN(result) {
        var bsam = document.getElementById("<%=HBSAM.ClientID%>").value;
		if ('' != bsam){
			return;
		}
		var modeTemp = result[4];
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (document.getElementById("<%=index.ClientID%>").value <= dgData.rows.length - 1) {
            if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[1].innerText == document.getElementById("<%=HDN.ClientID%>").value) {
                if (result[0] == SUCCESSRET) {
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[2].innerText = result[4];
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[4].innerText = result[6];
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[5].innerText = result[2];
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[6].innerText = result[7];
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[7].innerText = result[5];
                    if (result[7] == result[5]) {
                        document.getElementById("<%=HFull.ClientID%>").value = "1";
                        document.getElementById("<%=HMode.ClientID%>").value = result[4];
                        document.getElementById("<%=HClear.ClientID%>").value = "0";
                        document.getElementById("<%=HDN.ClientID%>").value = "------------";
                        document.getElementById("<%=HSN.ClientID%>").value = "";
                        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
                        document.getElementById("<%=HPartNO.ClientID%>").value = "";
                        document.getElementById("<%=HIsBT.ClientID%>").value = "";
                        document.getElementById("<%=HLine.ClientID%>").value = "";
                        document.getElementById("<%=ModeChange.ClientID%>").value = "";
                        beginWaitingCoverDiv();
                        document.getElementById("<%=btnGridFreshFull.ClientID%>").click();
                        return;
                    }
                }
                if (document.getElementById("<%=index.ClientID%>").value - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(document.getElementById("<%=index.ClientID%>").value - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[0].all[1].click();
                return;
            }
        }
        
        
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            document.getElementById("<%=index.ClientID%>").value = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == document.getElementById("<%=HDN.ClientID%>").value) {

                    if (result[0] == SUCCESSRET) {
                        dgData.rows[iRow].cells[2].innerText = result[4];
                        dgData.rows[iRow].cells[4].innerText = result[6];
                        dgData.rows[iRow].cells[5].innerText = result[2];
                        dgData.rows[iRow].cells[6].innerText = result[7];
                        dgData.rows[iRow].cells[7].innerText = result[5];
                        if (result[7] == result[5]) {
                            document.getElementById("<%=HFull.ClientID%>").value = "1";
                            document.getElementById("<%=HMode.ClientID%>").value = result[4]; ;
                            document.getElementById("<%=HClear.ClientID%>").value = "0";
                            document.getElementById("<%=HDN.ClientID%>").value = "------------";
                            document.getElementById("<%=HSN.ClientID%>").value = "";
                            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
                            document.getElementById("<%=HPartNO.ClientID%>").value = "";
                            document.getElementById("<%=HIsBT.ClientID%>").value = "";
                            document.getElementById("<%=HLine.ClientID%>").value = "";
                            document.getElementById("<%=ModeChange.ClientID%>").value = "";
                            beginWaitingCoverDiv();
                            document.getElementById("<%=btnGridFreshFull.ClientID%>").click();
                            return;
                        }
                    }

                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
        document.getElementById("<%=HMode.ClientID%>").value = modeTemp;
        GetDNByModelFull();
    }
  
    function GetDNByDN(DNNumber) {
        CombineCOAandDNWebService.GetDNInfo(DNNumber, onGetDNByDN, GetDNInfoFail);
    }
    function GetDNInfoFail(result) {
    }
    function GetDNByDNOnPage(DN) {

        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (document.getElementById("<%=index.ClientID%>").value <= dgData.rows.length - 1) {
            if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[1].innerText == DN) {

                if (document.getElementById("<%=index.ClientID%>").value - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(document.getElementById("<%=index.ClientID%>").value - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[0].all[1].click();
                return;
            }
        }
        
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            document.getElementById("<%=index.ClientID%>").value = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == DN) {

                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
        CheckFirst();
    }
    function GetModleByDN(DN) {

        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");

        if (document.getElementById("<%=index.ClientID%>").value <= dgData.rows.length - 1) {
            if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[1].innerText == DN) {
                document.getElementById("<%=HMode.ClientID%>").value = dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[2].innerText;
                document.getElementById("<%=HPONO.ClientID%>").value = dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[4].innerText;
                return;
            }
        }
 
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            document.getElementById("<%=index.ClientID%>").value = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[1].innerText == DN) {
                    document.getElementById("<%=HMode.ClientID%>").value = dgData.rows[iRow].cells[2].innerText;
                    document.getElementById("<%=HPONO.ClientID%>").value = dgData.rows[iRow].cells[4].innerText;
                    return;
                }
                else {
                    break;
                }
            }
        }
        
    }
    function CheckFirst() {
        var table = document.getElementById("<%=gridViewExt1.ClientID%>");
        var iRow = 1;
        if (iRow <= table.rows.length - 1) {
            document.getElementById("<%=index.ClientID%>").value = iRow;
            if (iRow - 1 < 0) {
                setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
            }
            else {
                setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
            }
            table.rows[iRow].cells[0].all[1].click();
            return;
        }
    }
    function FirstRadioToCheck() {

        if (document.getElementById("<%=HDN.ClientID%>").value != ""
        && document.getElementById("<%=HDN.ClientID%>").value != "------------") {
            document.getElementById("<%=HClear.ClientID%>").value = "0";
            GetDNByDNOnPage(document.getElementById("<%=HDN.ClientID%>").value);
        }
        else {
            var table = document.getElementById("<%=gridViewExt1.ClientID%>");
            var iRow = 1;
            if (iRow <= table.rows.length - 1) {
                document.getElementById("<%=index.ClientID%>").value = iRow;
                if (iRow - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                table.rows[iRow].cells[0].all[1].click();
                return;
            }
        }
        
    }
    
    function FirstRadioToCheckSuccess() {
        if (document.getElementById("<%=HDN.ClientID%>").value != ""
        && document.getElementById("<%=HDN.ClientID%>").value != "------------") {
            document.getElementById("<%=HClear.ClientID%>").value = "0";
            GetDNByDN(document.getElementById("<%=HDN.ClientID%>").value);
            beginWaitingCoverDiv();
            getSuccessFresh();
            endWaitingCoverDiv();
            return;
        }
        else {
            var table = document.getElementById("<%=gridViewExt1.ClientID%>");
            var iRow = 1;
            if (iRow <= table.rows.length - 1) {
                document.getElementById("<%=index.ClientID%>").value = iRow;
                if (iRow - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                table.rows[iRow].cells[0].all[1].click();
                beginWaitingCoverDiv();
                getSuccessFresh();
                //callNextInput();
                endWaitingCoverDiv();
                return;
            }
        }
    }

    function FirstRadioToCheckFailed() {
        if (document.getElementById("<%=HDN.ClientID%>").value != ""
        && document.getElementById("<%=HDN.ClientID%>").value != "------------") {
            document.getElementById("<%=HClear.ClientID%>").value = "0";
            GetDNByDN(document.getElementById("<%=HDN.ClientID%>").value);
            beginWaitingCoverDiv();
            var HInfoTemp = document.getElementById("<%=HInfo.ClientID%>").value ;
            if (HInfoTemp != "") {
                document.getElementById("<%=HInfo.ClientID%>").value = "";
                ShowInfo(HInfoTemp);
            }
            else {
                if (document.getElementById("<%=txtCOA.ClientID%>").value == "") {
                    document.getElementById("<%=successTemp.ClientID%>").value = document.getElementById("<%=successTemp.ClientID%>").value + "\n" + msgNotNeedCoa;
                    ShowInfo(document.getElementById("<%=successTemp.ClientID%>").value);
                }
                else {
                    ShowInfo(document.getElementById("<%=successTemp.ClientID%>").value);
                }
                document.getElementById("<%=successTemp.ClientID%>").value = "";
            }
            endWaitingCoverDiv();
            return;
        }
        else {
            var table = document.getElementById("<%=gridViewExt1.ClientID%>");
            var iRow = 1;
            if (iRow <= table.rows.length - 1) {
                document.getElementById("<%=index.ClientID%>").value = iRow;
                if (iRow - 1 < 0) {
                    setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                }
                else {
                    setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                }
                table.rows[iRow].cells[0].all[1].click();
                beginWaitingCoverDiv();
                var HInfoTemp = document.getElementById("<%=HInfo.ClientID%>").value;
                if (HInfoTemp != "") {
                    document.getElementById("<%=HInfo.ClientID%>").value = "";
                    ShowInfo(HInfoTemp);
                }
                else {
                    ShowInfo(document.getElementById("<%=successTemp.ClientID%>").value);
                    document.getElementById("<%=successTemp.ClientID%>").value = "";
                }
                endWaitingCoverDiv();
                return;
            }
        }
    }


    function GetDNByModel(Model) {

        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (document.getElementById("<%=index.ClientID%>").value <= dgData.rows.length - 1) {
            if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[2].innerText == Model) {
                if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[6].innerText == dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[7].innerText) {
                }
                else {
                    if (document.getElementById("<%=index.ClientID%>").value - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(document.getElementById("<%=index.ClientID%>").value - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[0].all[1].click();
                    return;
                }
            }
        }
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            document.getElementById("<%=index.ClientID%>").value = iRow; 
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model) {
                    if (dgData.rows[iRow].cells[6].innerText == dgData.rows[iRow].cells[7].innerText) {
                        continue;
                    }
                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
    }

    function GetDNByModelFull() {

        var Model = document.getElementById("<%=HMode.ClientID%>").value;
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (document.getElementById("<%=index.ClientID%>").value <= dgData.rows.length - 1) {
            if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[2].innerText == Model) {
                if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[6].innerText == dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[7].innerText) {
                }
                else {
                    if (document.getElementById("<%=index.ClientID%>").value - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(document.getElementById("<%=index.ClientID%>").value - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[0].all[1].click();
                    return;
                }
            }
        }
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            document.getElementById("<%=index.ClientID%>").value = iRow;
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model) {
                    if (dgData.rows[iRow].cells[6].innerText == dgData.rows[iRow].cells[7].innerText) {
                        continue;
                    }
                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    document.getElementById("<%=HMode.ClientID%>").value = "getMode";
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                   
                }
                else {
                    break;
                }
            }
        }
        document.getElementById("<%=HMode.ClientID%>").value = "getMode";
        CheckFirst();
    }

   
    function setSelectVal(span, idAndModeAndPono) {
        theRadio = span;
        oState = theRadio.checked;
        var id = "";
        var model = "";
        var pono = "";

        var idLen = idAndModeAndPono.indexOf("@#@#@");
        var ponoLen = idAndModeAndPono.indexOf("&!&!&");
        id = idAndModeAndPono.substring(0, idLen);
        model = idAndModeAndPono.substring(idLen + 5, ponoLen);
        pono = idAndModeAndPono.substring(ponoLen + 5);
        
        if (oState) {
            if (document.getElementById("<%=HSN.ClientID%>").value == "" &&
                id != "------------") {
                if (document.getElementById("<%=HClear.ClientID%>").value == "0") {
                    document.getElementById("<%=HClear.ClientID%>").value = "1"
                }
                else {
                    ResetWindow();
                }
            }
            document.getElementById("<%=HDN.ClientID%>").value = id;
            
            if (document.getElementById("<%=HMode.ClientID%>").value != "getMode" ) {
                if (document.getElementById("<%=HMode.ClientID%>").value == "first")
                { }
                else {
                    ResetLast();
                    document.getElementById("<%=txtCOA.ClientID%>").value = "";
                }
            }
            if (document.getElementById("<%=lastId.ClientID%>").value != "") {
                var temp = document.getElementById(document.getElementById("<%=lastId.ClientID%>").value);
                if (null != temp) {
                    temp.checked = false;
                }
            }
            if (document.getElementById("<%=HDN.ClientID%>").value != "------------") {
                document.getElementById("<%=HMode.ClientID%>").value = model;
                document.getElementById("<%=HPONO.ClientID%>").value = pono;
            }
            document.getElementById("<%=lastId.ClientID%>").value = theRadio.id;
            theRadio.checked = true;

        }

        endWaitingCoverDiv();
        callNextInput();
        if (document.getElementById("<%=HFull.ClientID%>").value == "1") {
            document.getElementById("<%=btnFocus.ClientID%>").click();
            document.getElementById("<%=HFull.ClientID%>").value = "0";
        }
        else {
            if (document.getElementById("<%=chkIsBT.ClientID%>").checked == true) {
                document.getElementById("<%=btnFocus.ClientID%>").click();
            }
        }
    }
    function DisplsyMsg(src) {
        ShowMessage(src);
        ShowInfo(src);
    }
	
	var SkipLabelBTCOO = "BT COO Label";
	function CheckBTtoSkipLabel(printItemlist, skipLabel){
		if (document.getElementById("<%=chkIsBT.ClientID%>").checked){
			var idxLabel = -1;
			for (var i = 0; i < printItemlist.length; i++){
				if (printItemlist[i].LabelType==skipLabel){
					idxLabel = i;
					break;
				}
			}
			if (idxLabel >= 0){
				printItemlist.splice(idxLabel,1);
			}
		}
		return printItemlist;
	}
	
    function input(data) {
        ShowInfo("");
        document.getElementById("<%=successTemp.ClientID%>").value = "";
        document.getElementById("<%=HInfo.ClientID%>").value = "";
        inputTextBox1 = data.trim();
        inputTextBox1 = inputTextBox1.toUpperCase();
        //CombineCOAandDNWebService.Start(StartSucc, StartFailed);
        if (document.getElementById("<%=HSN.ClientID%>").value == "") {
            ResetWindow();
        }
        if (inputTextBox1 == "7777") {
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=HDN.ClientID%>").value = "";
            document.getElementById("<%=HPONO.ClientID%>").value = "";
            document.getElementById("<%=HISWIN8.ClientID%>").value = "";
            document.getElementById("<%=HBSAM.ClientID%>").value = "";
            document.getElementById("<%=HFACTORYPO.ClientID%>").value = "";
            getPdLineCmbObj().selectedIndex = 0;
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
            ResetWindow();
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFreshReset.ClientID%>").click();
            return;
        }
        var line = getPdLineCmbValue();
        if (line == "") {
            alert(msgInputPDLine);
            ResetValue();
            ShowInfo(msgInputPDLine);
            return;
        }
        var printItemlist = getPrintItemCollection();
        if (printItemlist == null || printItemlist == "") {
            alert(msgPrintSettingPara);
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=HDN.ClientID%>").value = "";
            getPdLineCmbObj().selectedIndex = 0;
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
            callNextInput();
            ResetWindow();
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFreshReset.ClientID%>").click();
            return;
        }
        document.getElementById("<%=HLine.ClientID%>").value = line;
        if (document.getElementById("<%=HSN.ClientID%>").value == "") {
            ResetLast();
            if (inputTextBox1.length > 11 && inputTextBox1.indexOf(",") > 0) {
               
                inputTextBox1 = inputTextBox1.substring(0, 10);
            }
            if (inputTextBox1.length == 10 || inputTextBox1.length == 11) {
                pattCustSN1 = /^CN.{8}$/;
                pattCustSN2 = /^SCN.{8}$/;
                //if (pattCustSN1.exec(inputTextBox1) || pattCustSN2.exec(inputTextBox1)) {
				if (CheckCustomerSN(inputTextBox1)) {
                    if (inputTextBox1.length == 11) {
                        inputTextBox1 = inputTextBox1.substring(1, 11);
                    }
                }
                else {
                    ResetValue();
                    DisplsyMsg("Wrong Code!");
                    return;
                }
            }
            else {
                ResetValue();
                DisplsyMsg("Wrong Code!");
                return;
            }
            document.getElementById("<%=HSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=txtCustomerSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=HISWIN8.ClientID%>").value = "";
            document.getElementById("<%=HBSAM.ClientID%>").value = "";
            document.getElementById("<%=HFACTORYPO.ClientID%>").value = "";
            document.getElementById("<%=btnGetProduct.ClientID%>").click();
            return;
        }
        if (document.getElementById("<%=HCoaSN.ClientID%>").value == ""
            && document.getElementById("<%=HPartNO.ClientID%>").value != "") {
            document.getElementById("<%=HCoaSN.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=txtCOA.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=btnCheckInput.ClientID%>").click();
            return;
        }
    }
    
    
    function getModel(model) {
        document.getElementById("<%=txtModel.ClientID%>").value = model;
    }
    function getProductID(prod) {
        document.getElementById("<%=txtProductID.ClientID%>").value = prod;
    }
    function getIsBT() {
        document.getElementById("<%=showTemp.ClientID%>").value = "";
        document.getElementById("<%=HMode.ClientID%>").value = "getMode";
        document.getElementById("<%=btnGetDNQuick.ClientID%>").click();
    }
	function ChangeBsamTxt(isBSaM){
		if('Y'==isBSaM){
			document.getElementById("<%=txtBSAMLoc.ClientID%>").value = 'BSAM';
		}
		else{
			document.getElementById("<%=txtBSAMLoc.ClientID%>").value = '';
		}
	}
	function GetBsamTxt(){
		if('BSAM'==document.getElementById("<%=txtBSAMLoc.ClientID%>").value)
			return 'Y';
		return '';
	}
    function doIsBT() {
        document.getElementById("<%=txtModel.ClientID%>").value = document.getElementById("<%=HTxtModel.ClientID%>").value;
        document.getElementById("<%=txtProductID.ClientID%>").value = document.getElementById("<%=HTxtProductID.ClientID%>").value;
        var isBT = document.getElementById("<%=HTxtBT.ClientID%>").value;
        document.getElementById("<%=HTxtModel.ClientID%>").value = "";
        document.getElementById("<%=HTxtProductID.ClientID%>").value = "";
        document.getElementById("<%=HTxtBT.ClientID%>").value = "";
        
        var isBSAM = document.getElementById("<%=HBSAM.ClientID%>").value;
        ChangeBsamTxt(isBSAM);
        //document.getElementById("<%=HBSAM.ClientID%>").value = "";
        
        if (isBT == "false" || isBT == "False") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        }
        if (isBT == "true" || isBT == "True") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = true;
            document.getElementById("<%=showTemp.ClientID%>").value = "BT product";
        }
        document.getElementById("<%=HIsBT.ClientID%>").value = isBT;
        if (document.getElementById("<%=HISWIN8.ClientID%>").value == "true") {
            document.getElementById("<%=showTemp.ClientID%>").value = msgWin8;
        }
        else {
            if (isBT == "true" || isBT == "True") {
                document.getElementById("<%=showTemp.ClientID%>").value = "BT product";
            }
            else if ('Y' == isBSAM) {
				document.getElementById("<%=showTemp.ClientID%>").value = msgBSAM;
			}
			else {
                document.getElementById("<%=showTemp.ClientID%>").value = msgNomal;
            }
        }
        ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value);
        if ('Y' == isBSAM) {
			document.getElementById("<%=btnCheckCOA.ClientID%>").click();
		}
		else if (isBT == "false" || isBT == "False") {
            CheckMode();
        }
        else {
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function doIsCDSI() {
        document.getElementById("<%=txtModel.ClientID%>").value = document.getElementById("<%=HTxtModel.ClientID%>").value;
        document.getElementById("<%=txtProductID.ClientID%>").value = document.getElementById("<%=HTxtProductID.ClientID%>").value;
        var isBT = document.getElementById("<%=HTxtBT.ClientID%>").value;
        document.getElementById("<%=HTxtModel.ClientID%>").value = "";
        document.getElementById("<%=HTxtProductID.ClientID%>").value = "";
        document.getElementById("<%=HTxtBT.ClientID%>").value = "";
		ChangeBsamTxt(document.getElementById("<%=HBSAM.ClientID%>").value);
        document.getElementById("<%=HBSAM.ClientID%>").value = "";
        if (isBT == "false" || isBT == "False") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        }
        if (isBT == "true" || isBT == "True") {
            document.getElementById("<%=chkIsBT.ClientID%>").checked = true;
        }
        document.getElementById("<%=HIsBT.ClientID%>").value = isBT;
       
        if (document.getElementById("<%=HISWIN8.ClientID%>").value == "true") {
            document.getElementById("<%=showTemp.ClientID%>").value = msgCDSI +"\n" + msgWin8;
        }
        else {
            document.getElementById("<%=showTemp.ClientID%>").value = msgCDSI;
        }
        ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value);
        if (isBT == "false" || isBT == "False") {
            var FNO = document.getElementById("<%=HFACTORYPO.ClientID%>").value;
            CheckModeAndFNO(FNO);
        }
        else {
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function getIsCDSI() {
        document.getElementById("<%=HMode.ClientID%>").value = "getMode";
        document.getElementById("<%=btnGetDNQuick.ClientID%>").click();
    }
    function CheckModeAndFNO(FNO) {
        var model = document.getElementById("<%=txtModel.ClientID%>").value;
        var temp = document.getElementById("<%=HDN.ClientID%>").value;
        if (document.getElementById("<%=HDN.ClientID%>").value == "------------") {
            document.getElementById("<%=HMode.ClientID%>").value = "getMode";
            GetDNByModelAndFNO(model, FNO);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                document.getElementById("<%=HMode.ClientID%>").value = model;
            }
            else {
                getResetAll();
                ResetWindow();
                var tmp = msgNoCDSI;
                tmp = tmp.replace("%1", FNO);
                DisplsyMsg(tmp);
                callNextInput();
                return;
            }
        }
        var msgFNO = "";
        if (document.getElementById("<%=HMode.ClientID%>").value != model
        || document.getElementById("<%=HPONO.ClientID%>").value != FNO) {
            if (document.getElementById("<%=HMode.ClientID%>").value == model
            && document.getElementById("<%=HPONO.ClientID%>").value != FNO) {
                msgFNO = "FNO";
            }
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            document.getElementById("<%=HMode.ClientID%>").value = "getMode";
            GetDNByModelAndFNO(model, FNO);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                document.getElementById("<%=HMode.ClientID%>").value = model;
                if (msgFNO == "") {
                    document.getElementById("<%=ModeChange.ClientID%>").value = "true";
                }
                else {
                    document.getElementById("<%=ModeChange.ClientID%>").value = "FNO";
                }
                
                document.getElementById("<%=btnCheckCOA.ClientID%>").click();
            }
            else {
                getResetAll();
                ResetWindow();
                var tmp = msgNoCDSI;
                tmp = tmp.replace("%1", FNO);
                DisplsyMsg(tmp);
                callNextInput();
            }
        }
        else {
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function GetDNByModelAndFNO(Model, FNO) {
        var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
        if (document.getElementById("<%=index.ClientID%>").value <= dgData.rows.length - 1) {
            if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[2].innerText == Model
            && dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[4].innerText == FNO
            ) {
                if (dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[6].innerText == dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[7].innerText) {
                }
                else {
                    if (document.getElementById("<%=index.ClientID%>").value - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(document.getElementById("<%=index.ClientID%>").value - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[0].all[1].click();
                    return;
                }
            }
        }
        var iStartCol = 0;
        var iEndCol = 7;
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            document.getElementById("<%=index.ClientID%>").value = iRow;
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                if (dgData.rows[iRow].cells[2].innerText == Model
                && dgData.rows[document.getElementById("<%=index.ClientID%>").value].cells[4].innerText == FNO) {
                    if (dgData.rows[iRow].cells[6].innerText == dgData.rows[iRow].cells[7].innerText) {
                        continue;
                    }
                    if (iRow - 1 < 0) {
                        setSrollByIndex(0, false, "<%=gridViewExt1.ClientID%>");
                    }
                    else {
                        setSrollByIndex(iRow - 1, false, "<%=gridViewExt1.ClientID%>");
                    }
                    dgData.rows[iRow].cells[0].all[1].click();
                    return;
                }
                else {
                    break;
                }
            }
        }
    }
   

    function CheckMode() {
        var bsam = document.getElementById("<%=HBSAM.ClientID%>").value;
		if ('' != bsam){
			document.getElementById("<%=btnCheckCOA.ClientID%>").click();
			return;
		}
		var model = document.getElementById("<%=txtModel.ClientID%>").value.trim();
        var temp = document.getElementById("<%=HDN.ClientID%>").value;
        if (document.getElementById("<%=HDN.ClientID%>").value == "------------") {
            document.getElementById("<%=HMode.ClientID%>").value = "getMode";
            GetDNByModel(model);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                document.getElementById("<%=HMode.ClientID%>").value = model;
            }
            else {
                getResetAll();
                ResetWindow();
                var modeDis = msgModelNoFind + "\n Model:" + model;
                DisplsyMsg(modeDis);
                callNextInput();
                return;
            }
        }
        if (document.getElementById("<%=HMode.ClientID%>").value != model) {
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            document.getElementById("<%=HMode.ClientID%>").value = "getMode";
            GetDNByModel(model);
            if (temp != document.getElementById("<%=HDN.ClientID%>").value) {
                document.getElementById("<%=HMode.ClientID%>").value = model;
                document.getElementById("<%=ModeChange.ClientID%>").value = "true";
                document.getElementById("<%=btnCheckCOA.ClientID%>").click();
            }
            else {
                getResetAll();
                ResetWindow();
                var modeDis = msgModelNoFind + "\n Model:" + model;
                DisplsyMsg(modeDis);
                callNextInput();
            }
        }
        else {
            document.getElementById("<%=btnCheckCOA.ClientID%>").click();
        }
    }
    function getIfCheckCOAFocus() {
        ResetCoa();
    }
    function getNoCheckCOAFocus() {
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = msgNotNeedCoa;
    }
    function getNeedCheckCOAFocus(coa) {
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = coa;
    }
    function getSuccess() {
        ResetValue();
        getResetAll();
        ShowInfo("Success");
    }
    function getSuccessFresh() {
        if (document.getElementById("<%=successTemp.ClientID%>").value != "") {
            if (document.getElementById("<%=txtCOA.ClientID%>").value == "") {
                document.getElementById("<%=successTemp.ClientID%>").value = document.getElementById("<%=successTemp.ClientID%>").value + "\n" + msgNotNeedCoa;
                ShowSuccessfulInfo(true, document.getElementById("<%=successTemp.ClientID%>").value);
            }
            else {
                ShowSuccessfulInfo(true, document.getElementById("<%=successTemp.ClientID%>").value);
            }
        }
        else {
            ShowSuccessfulInfo(true, msgNotNeedCoa);
        }
    }
    function getIfCheckCOAShow(partNO) {
        document.getElementById("<%=HPartNO.ClientID%>").value = partNO;
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = partNO;
        if (document.getElementById("<%=ModeChange.ClientID%>").value == "true") {
            if (document.getElementById("<%=showTemp.ClientID%>").value != "") {
                ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value + "\n" + msgModeChange + msgCoaInput + partNO);
                document.getElementById("<%=showTemp.ClientID%>").value = "";
             }
             else {
                ShowInfo(msgModeChange + msgCoaInput + partNO);
            }
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
        }
        else if (document.getElementById("<%=ModeChange.ClientID%>").value == "FNO") {
        if (document.getElementById("<%=showTemp.ClientID%>").value != "") {
            ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value + "\n" + msgFNOWrong + "\n" + msgCoaInput + partNO);
            document.getElementById("<%=showTemp.ClientID%>").value = "";
            }
            else {
                ShowInfo(msgFNOWrong + "\n" + msgCoaInput + partNO);
            }
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
         }
         else {
             if (document.getElementById("<%=showTemp.ClientID%>").value != "") {
                 ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value + "\n" + msgCoaInput + partNO);
                 document.getElementById("<%=showTemp.ClientID%>").value = "";
             }
             else {
                 ShowInfo(msgCoaInput + partNO);
             }
        }
    }
    function getCheckCOAShow()
    {
        document.getElementById("<%=HPartNO.ClientID%>").value = "Check@Coa";
        if (document.getElementById("<%=ModeChange.ClientID%>").value == "true") {
            if (document.getElementById("<%=showTemp.ClientID%>").value != "") {
                ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value + "\n" + msgModeChange + msgCoaCheck);
                document.getElementById("<%=showTemp.ClientID%>").value = "";
             }
             else
             {
                ShowInfo(msgModeChange + msgCoaCheck);
             }
             document.getElementById("<%=ModeChange.ClientID%>").value = "";
        }
        else if (document.getElementById("<%=ModeChange.ClientID%>").value == "FNO") {
        if (document.getElementById("<%=showTemp.ClientID%>").value != "") {
            ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value + "\n" + msgFNOWrong + "\n" + msgCoaCheck);
            document.getElementById("<%=showTemp.ClientID%>").value = "";
                }
                else {
                    ShowInfo(msgFNOWrong + "\n" + msgCoaCheck);
                }
                document.getElementById("<%=ModeChange.ClientID%>").value = "";
            }
            else {
                if (document.getElementById("<%=showTemp.ClientID%>").value != "") {
                    ShowInfo(document.getElementById("<%=showTemp.ClientID%>").value + "\n" + msgCoaCheck);
                    document.getElementById("<%=showTemp.ClientID%>").value = "";
                 }
                else {
                    ShowInfo(msgCoaCheck);
                }
            }
    }
    function getProductFail(productWrong) {
        ResetLast();
        ShowInfo(productWrong);
    }
    function getDNFail() {
        ResetLast();
        DisplsyMsg("No DN data, operation pause !");
    }
    function callNextInput() {
        beginWaitingCoverDiv();
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("input");
        endWaitingCoverDiv();
    }
    function ResetValue() {
        document.getElementById("<%=HSN.ClientID%>").value = "";
        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
        document.getElementById("<%=HPartNO.ClientID%>").value = "";
        document.getElementById("<%=HIsBT.ClientID%>").value = "";
        ShowInfo("");
        callNextInput();
    }
    function ResetLast() {
        document.getElementById("<%=HSN.ClientID%>").value = "";
        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
        document.getElementById("<%=HPartNO.ClientID%>").value = "";
        document.getElementById("<%=HIsBT.ClientID%>").value = "";
        document.getElementById("<%=HBSAM.ClientID%>").value = "";
        
        
        document.getElementById("<%=txtModel.ClientID%>").value = "";
        document.getElementById("<%=txtProductID.ClientID%>").value = "";
        document.getElementById("<%=txtCOAIEC.ClientID%>").value = "";
        document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
        document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
        document.getElementById("<%=txtBSAMLoc.ClientID%>").value = "";
        ShowInfo("");
        callNextInput();
    }
    
    function ResetCoa() {
        document.getElementById("<%=HCoaSN.ClientID%>").value = "";
        document.getElementById("<%=txtCOA.ClientID%>").value = "";
        ShowInfo("");
        callNextInput();
    }




    
    
    function PrintPizzaAndCOO(changeDN) {
        if (changeDN != "") {
            document.getElementById("<%=HDN.ClientID%>").value = changeDN;
        }
        PrintPizzaFinal(changeDN);
    }
    function btnPrintSetting_onclick() {
        showPrintSetting(station, pCode);
    }
    function PrintCOO() {
        beginWaitingCoverDiv();
        try {
            var printItemlist = getPrintItemCollection();
			printItemlist = CheckBTtoSkipLabel(printItemlist, SkipLabelBTCOO);
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            if (temp == "") {
                if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                    temp = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
                }
                else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                    temp = document.getElementById("<%=HSN.ClientID%>").value.trim();
                }
            }
            CombineCOAandDNWebService.PrintCOOLabel(temp, editor, station, customer, printItemlist, printSuccCOO, printFailCOO);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }

    function printSuccCOO(result) {
        var labelCollection = [];
        for (var i in result[1]) {
            if (result[1][i].LabelType == "BT COO Label") {
                labelCollection.push(result[1][i]);
                setPrintItemListParamCOO(labelCollection, result[0]);
                printLabels(labelCollection, true);
                break;
            }
        }
        endWaitingCoverDiv();
        getResetFreshSuccess();
    }
    
    function setPrintItemListParamCOO(backPrintItemList, sn) {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@sn";
        if (sn == "") {
            if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                sn = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
            }
            else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                sn = document.getElementById("<%=HSN.ClientID%>").value.trim();
            }
        }
        valueCollection[0] = generateArray(sn);
        setPrintParam(lstPrtItem, "BT COO Label", keyCollection, valueCollection);
    } 
    function PrintPizza() {
        beginWaitingCoverDiv();
        try {
            var printItemlist = getPrintItemCollection();
			printItemlist = CheckBTtoSkipLabel(printItemlist, SkipLabelBTCOO);
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            CombineCOAandDNWebService.PrintPizzaLabel(document.getElementById("<%=HSN.ClientID%>").value, editor, station, customer, printItemlist, printSucc, printFail);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }

    function PrintPizzaFinal(changeDN) {
        beginWaitingCoverDiv();
        try {
            var printItemlist = getPrintItemCollection();
			printItemlist = CheckBTtoSkipLabel(printItemlist, SkipLabelBTCOO);
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            var temp = "";
            if (changeDN != "") {
                document.getElementById("<%=HDN.ClientID%>").value = changeDN;
                temp = document.getElementById("<%=HSN.ClientID%>").value + "#@#" + changeDN;
            }
            else
            {
                temp = document.getElementById("<%=HSN.ClientID%>").value + "#@#" + document.getElementById("<%=HDN.ClientID%>").value;
            }
            if (temp == "") {
                if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                    temp = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
                }
                else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                    temp = document.getElementById("<%=HSN.ClientID%>").value.trim();
                }
            }
            CombineCOAandDNWebService.PrintPizzaLabelFinal(temp, editor, station, customer, printItemlist, printSucc, printFailFinal);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }
    
    function PrintPizzaJapan() {
        try {
            var printItemlist = getPrintItemCollection();
			printItemlist = CheckBTtoSkipLabel(printItemlist, SkipLabelBTCOO);
            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                endWaitingCoverDiv();
                ResetValue();
                getResetAll();
                return;
            }
            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            if (temp == "") {
                if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                    temp = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
                }
                else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                    temp = document.getElementById("<%=HSN.ClientID%>").value.trim();
                }
            }
            CombineCOAandDNWebService.PrintPizzaLabel(temp, editor, station, customer, printItemlist, printSuccJapan, printFail);
        }
        catch (e) {

            endWaitingCoverDiv();
        }
    }
    function printSuccJapan(result) {
        var labelCollection = [];
        for (var i in result[1]) {
            if (result[1][i].LabelType == "PIZZA Label-2") {
                labelCollection.push(result[1][i]);
                setPrintItemListParam2(labelCollection, result[0]);
                printLabels(labelCollection, true);
                break;
            }  
        }
        endWaitingCoverDiv();
    }
    function printSucc(result)
    {
        var labelCollection = [];
        if (result[0] == "failedSession") {

            endWaitingCoverDiv();
            getFailed();
            return;
        }
        for (var i in result[1]) {
            if (result[1][i].LabelType == "PIZZA Label-1") {
                labelCollection.push(result[1][i]);
                setPrintItemListParam(labelCollection, result[0]);
                printLabels(labelCollection, true);
                break;
            }
        }
        endWaitingCoverDiv();
        var temp = document.getElementById("<%=HMode.ClientID%>").value.trim();
        if (temp.length > 11) {
            var japan = temp.substr(9, 2);
            if (japan == "29" || japan == "39") {
                if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
                    document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
                }
                else {
                    beginWaitingCoverDiv();
                    PrintPizzaJapan();
                }
            }
        } 
        if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
            document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
            PrintCOO();
        }
        else {
            getResetFreshSuccess();
        }
    }
    function setPrintItemListParam2(backPrintItemList, sn) {

        var lable = "PIZZA Label-2";
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@sn";
        if (sn == "") {
            if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                sn = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
            }
            else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                sn = document.getElementById("<%=HSN.ClientID%>").value.trim();
            }
        }
        valueCollection[0] = generateArray(sn);
        setPrintParam(lstPrtItem, lable, keyCollection, valueCollection);
     }
    function setPrintItemListParam(backPrintItemList, sn)
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        var lable = "PIZZA Label-1";
        keyCollection[0] = "@sn";
        if (sn == "") {
            if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                sn = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
            }
            else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                sn = document.getElementById("<%=HSN.ClientID%>").value.trim();
            }
        }
        valueCollection[0] = generateArray(sn);
        setPrintParam(lstPrtItem, lable, keyCollection, valueCollection);
    } 
        
        function printFail(result)
        {
            endWaitingCoverDiv();
            if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
                document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
                PrintCOO();
            }
            else {
                ResetValue();
                getResetAll();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
            }    
        }
        function printFailFinal(result) {
            endWaitingCoverDiv();
            document.getElementById("<%=btnCancel.ClientID%>").click();
            if (document.getElementById("<%=HIsBT.ClientID%>").value == "true" ||
                document.getElementById("<%=HIsBT.ClientID%>").value == "True") {
                PrintCOO();
            }
            else {
                ResetValue();
                getResetAll();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
            }
        }
        function printFailCOO(result) {
            endWaitingCoverDiv();
            ResetValue();
            getResetAll();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        } 
        function btnRePrint_onclick() {
            var paramArray = new Array();
            paramArray[0] = station;
            paramArray[1] = editor;
            paramArray[2] = customer;
            paramArray[3] = pCode;
            var url = "CombineCOAandDNReprint.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login + "&COO=" + "true";
            window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
        function getReset(msg) {
            endWaitingCoverDiv();
            beginWaitingCoverDiv();
            ShowInfo(msg);
            callNextInput();
            endWaitingCoverDiv();
        }
        function getCoaECPN(msg) {
            document.getElementById("<%=txtCOAIEC.ClientID%>").value = msg;
        }
        function getResetAll() {
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFresh.ClientID%>").click();
        }
        function getResetFreshSuccess() {

            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            document.getElementById("<%=successTemp.ClientID%>").value = "";
            if (temp != "") {
                document.getElementById("<%=successTemp.ClientID%>").value = "[" + temp + "] " + msgSuccess;
            }
            
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
            FirstRadioToCheckSuccess();
        }
        function getFailed() {
            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            document.getElementById("<%=successTemp.ClientID%>").value = "";
            if (temp != "") {
                document.getElementById("<%=successTemp.ClientID%>").value = "[" + temp + "] Failed!";
            }
           
            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("<%=HCoaSN.ClientID%>").value = "";
            document.getElementById("<%=HPartNO.ClientID%>").value = "";
            document.getElementById("<%=HIsBT.ClientID%>").value = "";
            document.getElementById("<%=HLine.ClientID%>").value = "";
            document.getElementById("<%=HMode.ClientID%>").value = "first";
            document.getElementById("<%=ModeChange.ClientID%>").value = "";
            FirstRadioToCheckFailed();
        }
        function ResetWindow() {
            document.getElementById("<%=txtModel.ClientID%>").value = "";
            document.getElementById("<%=txtProductID.ClientID%>").value = "";
            document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
            document.getElementById("<%=txtCOAIEC.ClientID%>").value = "";
            document.getElementById("<%=txtCOA.ClientID%>").value = "";
            document.getElementById("<%=chkIsBT.ClientID%>").checked = false;
            document.getElementById("<%=txtBSAMLoc.ClientID%>").value = "";
        }
        
        function getResetMode(mode) {
            document.getElementById("<%=HMode.ClientID%>").value = mode;
        }
        function getDisplay(display) {
            ShowInfo(display);
        }
        
        function GetProduct() {
             return true;
         }
         function getSaveNoCoaFromUI() {
             var printItemlist = getPrintItemCollection();
			 printItemlist = CheckBTtoSkipLabel(printItemlist, SkipLabelBTCOO);
             if (printItemlist == null || printItemlist == "") {
                 alert(msgPrintSettingPara);
                 ResetValue();
                 getResetAll();
                 return;
             }
             var upLine = document.getElementById("<%=HLine.ClientID%>").value.trim();
             var upDN = document.getElementById("<%=HDN.ClientID%>").value.trim();
             var upSN = document.getElementById("<%=HSN.ClientID%>").value.trim();
             var upCoaSN = document.getElementById("<%=HCoaSN.ClientID%>").value.trim();
             if (upDN == "" || upSN == "") {
                 return;
             }
             beginWaitingCoverDiv();
             CombineCOAandDNWebService.UpdateAndPrint(upLine, editor, station, customer, upDN, upSN, "", printItemlist, onSaveSucc, onSaveFail);
         }
         function getSaveCoaFromUI() {
             var printItemlist = getPrintItemCollection();
			 printItemlist = CheckBTtoSkipLabel(printItemlist, SkipLabelBTCOO);
             if (printItemlist == null || printItemlist == "") {
                 alert(msgPrintSettingPara);
                 ResetValue();
                 getResetAll();
                 return;
             }
             var upLine = document.getElementById("<%=HLine.ClientID%>").value.trim();
             var upDN = document.getElementById("<%=HDN.ClientID%>").value.trim();
             var upSN = document.getElementById("<%=HSN.ClientID%>").value.trim();
             var upCoaSN = document.getElementById("<%=HCoaSN.ClientID%>").value.trim();
             if (upDN == "" || upSN == "" || upCoaSN == "") {
                 return;
             }
             beginWaitingCoverDiv();
             CombineCOAandDNWebService.UpdateAndPrint(upLine, editor, station, customer, upDN, upSN, upCoaSN, printItemlist, onSaveSucc, onSaveFail);
         }
         function onSaveSucc(result) {
             if (globalIndex != -1) {
                 endWaitingCoverDiv();
                 beginWaitingCoverDiv();
                 CombineCOAandDNWebService.Start(result, onSaveSucc, onSaveFail);
                 return;
             }
             endWaitingCoverDiv();
             var index = 0;
             var printlist = new Array();
             var count = 0;

             try {
                 if (result instanceof Array && result[0] == SUCCESSRET) {
                     //merge code to bind QC result
                     document.getElementById("<%=lblQCStatus.ClientID%>").innerText = result[5];
                     //Get QC result
                     
                     // bsamLocId begin
					 var locId = result[4];
					 if(''!=locId){
						document.getElementById("<%=txtBSAMLoc.ClientID%>").value = 'BSAM Loc= '+locId;
					 }
					 // bsamLocId end
					 var jpflag = result[2];
                     index = getTemp(result[1], "PIZZA Label-1");
                     if (index >= 0) {
                         setPrintItemListParamDX(result[1][index], "PIZZA Label-1");
                         printlist[count] = result[1][index];
                         count++;
                     }
                     if (jpflag) {
                         index = getTemp(result[1], "PIZZA Label-2");
                         if (index >= 0) {
                             setPrintItemListParamDX(result[1][index], "PIZZA Label-2");
                             printlist[count] = result[1][index];
                             count++;
                         }
                     }
                     if (document.getElementById("<%=chkIsBT.ClientID%>").checked == true) {
                         index = getTemp(result[1], "BT COO Label");
                         if (index >= 0) {
                             setPrintItemListParamDX(result[1][index], "BT COO Label");
                             printlist[count] = result[1][index];
                             count++;
                         }
                     }
					 // JameStown L10_LABEL
					 index = getTemp(result[1], "L10_LABEL");
					 if (index >= 0) {
						 setPrintItemListParamDX(result[1][index], "L10_LABEL");
						 printlist[count] = result[1][index];
						 count++;
					 }
                     //==========================================print process=======================================
                     /*
                     * Function Name: printLabels
                     * @param: printItems
                     * @param: isSerial
                     */
                     printLabels(printlist, true);
                     //==========================================end print process==================================
                     var tmp = result[3];
                     if (tmp.indexOf("#@$#") != -1) {
                         var changeDN = tmp.substr(tmp.indexOf("#@$#") + 4);
                         document.getElementById("<%=HDN.ClientID%>").value = changeDN;
                         document.getElementById("<%=btnGridFreshSuccess.ClientID%>").click(); 
                         return;
                     }
                     //document.getElementById("<%=btnGridFreshSuccess.ClientID%>").click(); 
                     getResetFreshSuccess();
                 }
                 else if (result instanceof Array && result.length == 1) {
                     ResetValue();
                     getResetAll();
                     ShowMessage(result[0]);
                     ShowInfo(result[0]);
                 }
                 else if (typeof result === 'string') {
                     ResetValue();
                     getResetAll();
                     ShowMessage(result);
                     ShowInfo(result);
                 }
                 else { //if (result == null) {
                     ResetValue();
                     getResetAll();
                     ShowInfo("msgSystemError");
                 }
                 
             }
             catch (e) {
                 ResetValue();
                 getResetAll();
                 ShowInfo(e.description);
             }
         }
         function onSaveFail(result) {
             endWaitingCoverDiv();
             ResetValue();
             getResetAll();
             ShowMessage(result.get_message());
             ShowInfo(result.get_message());
         }
         function getTemp(result, label) {

             for (var i = 0; i < result.length; i++) {
                 if (result[i].LabelType == label) {
                     return i;
                 }
             }
             return -1;
         }
         function setPrintItemListParamDX(backPrintItemList, labelType) {
             //============================================generate PrintItem List==========================================
             //var lstPrtItem = backPrintItemList;
             var lstPrtItem = new Array();
             lstPrtItem[0] = backPrintItemList;

             var keyCollection = new Array();
             var valueCollection = new Array();

             keyCollection[0] = "@sn";
             var temp = document.getElementById("<%=HSN.ClientID%>").value;
             if (temp == "") {
                 if (document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim() != "") {
                     temp = document.getElementById("<%=txtCustomerSN.ClientID%>").value.trim();
                 }
                 else if (document.getElementById("<%=HSN.ClientID%>").value.trim() != "") {
                     temp = document.getElementById("<%=HSN.ClientID%>").value.trim();
                 }
             }
             valueCollection[0] = generateArray(temp);
             /*
             * Function Name: setPrintParam
             * @param: printItemCollection
             * @param: labelType
             * @param: keyCollection(Client: Array of string.    Server: List<string>)
             * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
             */
             setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
         }
    </script>

 <div>
   <center >
   <table width="100%" border="0" >
    <tr>
        <td align="left" >
            <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="80" IsPercentage="true" />
        </td>
    </tr>
    
    <tr>
        <td colspan="4" align="left">
        <fieldset>
            <legend><asp:Label ID="lbTableTitle" CssClass="iMes_label_13pt" runat="server"></asp:Label></legend>
            <asp:UpdatePanel runat="server" ID="UpdatePanelTable" UpdateMode="Conditional">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                        GvExtWidth="99%" GvExtHeight="172px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                        Width="97%" Height="167px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                        HighLightRowPosition="3" HorizontalAlign="Left" onrowdatabound="GridView1_RowDataBound"> 
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton id="radio" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Delivery NO" />
                            <asp:BoundField DataField="Model" />
                            <asp:BoundField DataField="Customer P/N" />
                            <asp:BoundField DataField="PoNo" />
                            <asp:BoundField DataField="Date" />
                            <asp:BoundField DataField="Qty" />
                            <asp:BoundField DataField="Packed Qty" />
                        </Columns>
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td align="left" >
        <fieldset style="width: auto">
            <legend>
             <asp:Label ID="lbProductInfo" runat="server"></asp:Label></legend>
             <table width="100%">
            <tr>
                <td Width = "13%">
                    <asp:Label ID="lbCustomerSN"   runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width:32%">
                    <asp:TextBox ID="txtCustomerSN" runat="server"  style="width:90%"     CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True"/>
                </td>
                <td Width = "13%">
                    <asp:Label ID="lbProductID"   runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width:32%">
                    <asp:TextBox ID="txtProductID" runat="server"  style="width:90%"     CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True"/>
                </td>
             </tr>
           <tr>
                <td>
                    <asp:Label ID="lbModel"   runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtModel" runat="server"  style="width:90%"    CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True" />
                </td>
                <td>
                    <asp:CheckBox ID="chkIsBT" runat="server" CssClass="iMes_label_13pt"  />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblQCStatus" runat="server" Font-Size="X-Large"  ForeColor="Red"/>
                </td>                
                <td>
                    <asp:Label ID="lbBSAMLoc"  style="font-weight:bold;color:Red;" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtBSAMLoc" runat="server"  style="font-weight:bold;color:Red;width:60%"  CssClass="iMes_textbox_input_Disabled" IsClear="true" ReadOnly="True"/>
                </td>
           </tr>
           </table>
         </fieldset>
        </td>
    </tr>
    <tr>
        <td align="left" >
        <fieldset style="width: auto">
            <legend><asp:Label ID="lbCheckItem" runat="server"></asp:Label></legend>
            <asp:Label ID="lbCOA"  Width = "6%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <asp:TextBox ID="txtCOA" runat="server" style="width:26% ;line-height:26px "     
                            IsClear="true" ReadOnly="True" Font-Bold="True" 
                Font-Names="Verdana" Font-Size="X-Large" Height="28px"  />
         &nbsp;<asp:TextBox ID="txtCOAIEC" runat="server" style="width:58% ;line-height:30px "     
                            IsClear="true" ReadOnly="True" Font-Bold="True" 
                Font-Names="Verdana" Font-Size="XX-Large" Height="32px" 
                ForeColor="#CC3300"  />
         </fieldset>
        </td>
    </tr>
    
    <tr>
        <td align="left" colspan="1">
            <asp:Label ID="lbDataEntry" Width = "13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
            <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" InputRegularExpression="^[-0-9a-zA-Z#,\+\s\*]*$"
                                Width="45%" IsClear="true" IsPaste="true"  />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input id="btnPrintSetting" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'"    onclick="return btnPrintSetting_onclick()" />
            <input id="btnRePrint" type="button"  runat="server" 
                class="iMes_button"  
                onmouseover="this.className='iMes_button_onmouseover'"  
                onmouseout="this.className='iMes_button_onmouseout'"  onclick="return btnRePrint_onclick()" /></td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server"  RenderMode="Inline" UpdateMode="Conditional">
	            <ContentTemplate>
	              <button id="btnGridFresh" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshFull" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshSuccess" runat="server" type="button" style="display: none" />
	              <button id="btnGridFreshReset" runat="server" type="button" style="display: none" />
	              <button id="btnGetProduct" runat="server" type="button" onclick="if(GetProduct())" onserverclick="btnGetProduct_ServerClick" style="display: none" />
	              <button id="btnCheckCOA" runat="server" type="button" style="display: none" />
	              <button id="btnCheckInput" runat="server" type="button" style="display: none" />
	              <button id="btnGetModel" runat="server" type="button" style="display: none" />
	              <button id="btnFocus" runat="server" type="button" style="display: none" />
	              <button id="btnCancel" runat="server" type="button" style="display: none" />
	              <button id="btnGetDNQuick" runat="server" type="button" style="display: none" />
	              <input type="hidden" runat="server" id="HSN" /> 
	              <input type="hidden" runat="server" id="HIsBT" />  
	              <input type="hidden" runat="server" id="HDN" />  
	              <input type="hidden" runat="server" id="HPartNO" />
	              <input type="hidden" runat="server" id="HCoaSN" />
	              <input type="hidden" runat="server" id="HLine" />
	              <input type="hidden" runat="server" id="HMode" /> 
	              <input type="hidden" runat="server" id="HInfo" />
	              <input type="hidden" runat="server" id="HTxtModel" />
	              <input type="hidden" runat="server" id="HTxtProductID" />
	              <input type="hidden" runat="server" id="HTxtBT" />
	              <input type="hidden" runat="server" id="HFACTORYPO" />
	              <input type="hidden" runat="server" id="HPONO" />
	              <input type="hidden" runat="server" id="HISWIN8" />
	              <input type="hidden" runat="server" id="HBSAM" />
	              <input type="hidden" runat="server" id="HFull" />
	              <input type="hidden" runat="server" id="HClear" />
	              <input type="hidden" runat="server" id="lastId" />
	              <input type="hidden" runat="server" id="ModeChange" />
	              <input type="hidden" runat="server" id="index" />
	              <input type="hidden" runat="server" id="showTemp" />
	              <input type="hidden" runat="server" id="successTemp" />
	            </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
    </center>
</div>

</asp:Content>