<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* ITC-1360-0842 去掉完成后reset动作
* ITC-1360-0852 修改多label打印流程
* ITC-1360-1202 增加路径检查
* ITC-1360-1463 删除copy jpg功能
* ITC-1360-1715 label打印判断条件范围有误
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PDPALabel01.aspx.cs" Inherits="PAK_PDPALabel01" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<style type="text/css">
.maximizeStyle { height: auto; width: auto; position: fixed; border: 1px dotted black; left: 50px; top: 50px; }
</style>
    <bgsound src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>

    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePDPALabel01.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 99%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
		<table border="0" width="98%">
		<tr><td width="85%">
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
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCode" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbLabelKittingCode ID="CmbCode" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblFloor" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <select id="CmbFloor" name="CmbFloor" style="width: 99%">
                            <option value="1">&nbsp</option>
                            <option value="2">2F</option>
                            <option value="3">3F</option>
                            <option value="4">2C</option>
                        </select>
                    </td>
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
                        <td>
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
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
                        <td style="width: 120px;">
                            <asp:CheckBox ID="QueryChk" runat="server" CssClass="iMes_CheckBox" />
                        </td>
                        <td align="right" style="width: 110px;">
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td align="right" style="width: 20%">
                            <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                            <button id="btnReprint" runat="server" onclick="clkReprint()" class="iMes_button"
                                width="200px" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                        </td>
                    </tr>
                </table>
            </div>
        </td><td width="14%" valign="top">
			<asp:UpdatePanel runat="server" ID="JpgUp" UpdateMode="Conditional">
            <ContentTemplate>
			<table border="0">
				<tr><td width="50%"><asp:Image ID="ShowImage0" runat="server" Width="154" Height="100"/></td><td>
				<asp:Image ID="ShowImage1" runat="server" Width="154" Height="100"/></td></tr>
				<tr><td><asp:Image ID="ShowImage2" runat="server" Width="154" Height="100"/></td><td>
				<asp:Image ID="ShowImage3" runat="server" Width="154" Height="100"/></td></tr>
				<tr><td><asp:Image ID="ShowImage4" runat="server" Width="154" Height="100"/></td><td>
				&nbsp;</td></tr>
			</table>
			</ContentTemplate>
            </asp:UpdatePanel>
		</td></tr>
		</table>
		<img id="jpgCenter" class="maximizeStyle" style="display: none;">
		</div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <input id="modelHidden" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var editor;
        var customer;
        var stationId;
        var pCode = "";
        var hostname = getClientHostName();
        var accountId;
        var login;

        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var inputObj;
        var emptyPattern = /^\s*$/;
        var customerSN = "";
        var productID = "";

        var QueryFlag = false;

        var wlabel = "";
        var clabel = "";
        var cmessage = "";
        var llabel = "";
        var win8label = "";
        var postellabel = "";
        //error message
        //var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        //var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgInputPDLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
        var msgInputCode = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPcodeIsNull") %>'
        var msgInputFloor = '<%=this.GetLocalResourceObject(Pre + "_msgInputFloor").ToString() %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgPrintWLabel = '<%=this.GetLocalResourceObject(Pre + "_msgPrintWLabel").ToString() %>';
        var msgNoPath = '<%=this.GetLocalResourceObject(Pre + "_msgNoPath").ToString() %>';
        var msgPrintDetail = '<%=this.GetLocalResourceObject(Pre + "_msgPrintDetail").ToString() %>';


        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            accountId = '<%=Request["AccountId"] %>';
            login = '<%=Request["Login"] %>';

            getPdLineCmbObj().selectedIndex = 0;
            getLabelKittingCodeCmbObj().selectedIndex = 0;
            document.getElementById("CmbFloor").selectedIndex = 0;
            //document.getElementById("<%=QueryChk.ClientID%>").checked = false;

            initPage();

            //turnOnLight("");
            beginWaitingCoverDiv();
            //WebServicePDPALabel01.GetSysSetting("EditsFISAddr", onGetSetting, onGetSettingFailed);
            WebServicePDPALabel01.GetCommSetting(hostname, editor, onGetCommSucceed, onGetCommFailed);

        };

        window.onbeforeunload = function() {

            OnCancel();

        };

        function onGetSetting(result) {
            //endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {

                var path = result[1] + "\\*.jpg";
                //Do not need Copy Jpg  --UC has been deleted
                //var fso = new ActiveXObject("Scripting.FileSystemObject");
                //var fileflag = fso.FolderExists(result[1]);
                //Do not need Copy Jpg  --UC has been deleted
                //if (fileflag) {
                //    fso.CopyFile(path, "C:\\");
                //}
                //else {
                //    alert(msgNoPath);
                //}

            } else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }
        }

        function onGetSettingFailed(result) {
            //endWaitingCoverDiv();
            //ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

        function onGetCommSucceed(result) {
            endWaitingCoverDiv();

            if (result == null) {
                setobjMSCommParaForLights();
            }
            else if (result[0] == SUCCESSRET) {

                var objMSCommSet = document.getElementById("objMSComm");

                try {

                    objMSCommSet.CommPort = result[1];
                    objMSCommSet.Settings = result[2];
                    objMSCommSet.RThreshold = result[3];
                    objMSCommSet.SThreshold = result[4];
                    objMSCommSet.Handshaking = result[5];
                    objMSCommSet.PortOpen = true;

                } catch (e) {
                    alert(e.description);
                }

            }
            else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }
            //initPage();
            //var objMSComm = document.getElementById("objMSComm");
            //ShowInfo("CommPort,Settings,RThreshold,SThreshold,Handshaking,PortOpen  is: " + objMSComm.CommPort + " , " + objMSComm.Settings + " , " + objMSComm.RThreshold + " , " + objMSComm.SThreshold + " , " + objMSComm.Handshaking + " , " + objMSComm.PortOpen);     

            callNextInput();

        }

        function onGetCommFailed(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }


        function initPage() {

            tbl = "<%=gd.ClientID %>";

            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            document.getElementById("<%=QueryChk.ClientID%>").checked = false;
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];

            //QueryFlag = false;
            wlabel = "";
            clabel = "";
            cmessage = "";
            llabel = "";

        }

        function input(data) {

            var line = getPdLineCmbValue();
            var code = getLabelKittingCodeCmbValue();
            var floor = document.getElementById("CmbFloor")[document.getElementById("CmbFloor").selectedIndex].text; //getFloorCmbValue();

            if (line == "") {
                alert(msgInputPDLine);
                ShowInfo(msgInputPDLine);
                callNextInput();
                return;
            }
            if (code == "") {
                alert(msgInputCode);
                ShowInfo(msgInputCode);
                callNextInput();
                return;
            }

            if (floor == "") {
                alert(msgInputFloor);
                ShowInfo(msgInputFloor);
                callNextInput();
                return;
            }
            inputCustomerSN(data);

            return;
        }
        function inputCustomerSN(inputData) {

            QueryFlag = document.getElementById("<%=QueryChk.ClientID%>").checked;

            //if ((inputData.length == 11) && (inputData.substring(0, 3) == "SCN")) {
//			if ((inputData.length == 11) && CheckCustomerSN(inputData)) {
//                inputData = inputData.substring(1, 11);
//            }

//            //if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {
//			if ((inputData.length == 10) && CheckCustomerSN(inputData)) {

//                inputData = inputData;
//            }
            if (CheckCustomerSN(inputData)) {
                if (inputData.length == 11) {
                    inputData = inputData.substring(1, 11);
                }
                //   inputData = inputData;
            }
            else {
                alert(msgInputCustSN);
                ShowInfo(msgInputCustSN);
                callNextInput();
                return;
            }

            var line = getPdLineCmbValue();
            var code = getLabelKittingCodeCmbValue();
            var floor = document.getElementById("CmbFloor")[document.getElementById("CmbFloor").selectedIndex].text; //getFloorCmbValue();

            ShowInfo("");
            beginWaitingCoverDiv();
            //stationId = "69";
            //customerSN = "1010000010000001"; //"30012444Q";
            customerSN = inputData;
            WebServicePDPALabel01.input(QueryFlag, line, code, floor, customerSN, editor, stationId, customer, inputSucc, onFail);
            return;
        }

        function inputSucc(result) {

            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                initPage();
                setInfo(result);
                inputFlag = true;
                turnOnLight(result);
                if (!QueryFlag) {
                    StartPrint();
                }
                else {
                    ExitPage();
                    ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
                    document.getElementById("<%=QueryChk.ClientID%>").checked = false;
                    callNextInput();
                }
            }
            else {
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ResetPage();
                ShowInfo(content);
                callNextInput();
            }
        }

        function onFail(result) {

            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function setInfo(info) {
            //set value to the label
            productID = info[1]["ProductID"];
            customerSN = info[1]["CustSN"];
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), customerSN);
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[1]["Model"]);
            setTable(info);

            wlabel = info[3];
            clabel = info[4];
            cmessage = info[5];
            llabel = info[6];
            win8label = info[7];
            postellabel = info[8];
        }

        function ClearJpg(){
			document.getElementById("<%=ShowImage0.ClientID %>").src = '';
			document.getElementById("<%=ShowImage1.ClientID %>").src = '';
			document.getElementById("<%=ShowImage2.ClientID %>").src = '';
			document.getElementById("<%=ShowImage3.ClientID %>").src = '';
			document.getElementById("<%=ShowImage4.ClientID %>").src = '';
		}
		function SetJpg(cnt, partNo){
			var file = '<%=ConfigurationManager.AppSettings["RDS_Server_PAKLabel"]%>' + partNo + ".jpg";
			switch(cnt){
				case 0:
					document.getElementById("<%=ShowImage0.ClientID %>").src = file; break;
				case 1:
					document.getElementById("<%=ShowImage1.ClientID %>").src = file; break;
				case 2:
					document.getElementById("<%=ShowImage2.ClientID %>").src = file; break;
				case 3:
					document.getElementById("<%=ShowImage3.ClientID %>").src = file; break;
				case 4:
					document.getElementById("<%=ShowImage4.ClientID %>").src = file; break;
			}
		}
		function setTable(info) {

            var wipbufferList = info[2];
            if (wipbufferList.length > 0) {
                PlaySound();
            }
			ClearJpg();
			var cntJpg=0;
            for (var i = 0; i < wipbufferList.length; i++) {

                var rowArray = new Array();
                var rw;

                rowArray.push(wipbufferList[i]["PartNo"]);
                rowArray.push(wipbufferList[i]["Tp"]);
                rowArray.push(wipbufferList[i]["Qty"]);

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
				
				var PartNo=wipbufferList[i]["PartNo"];
				if (cntJpg<5){
					SetJpg(cntJpg, PartNo);
					cntJpg++;
				}
            }
        }

        function StartPrint() {
            try {
                ShowInfo("");
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == "" || lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    ShowInfo(msgPrintSettingPara);
                    callNextInput();
                    return;
                }

                var line = getPdLineCmbValue();
                var code = getLabelKittingCodeCmbValue();
                var floor = document.getElementById("CmbFloor")[document.getElementById("CmbFloor").selectedIndex].text; //getFloorCmbValue();

                beginWaitingCoverDiv();
                WebServicePDPALabel01.Print(productID, line, code, floor, editor, stationId, customer, lstPrintItem, onPrintSucc, onFail);
            }
            catch (e) {
                alert(e);
            }
        }

        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }

        function setPrintItemListParam(backPrintItemList, labelType) {
            //============================================generate PrintItem List==========================================
            //var lstPrtItem = backPrintItemList;
            var lstPrtItem = new Array();
            lstPrtItem[0] = backPrintItemList;

            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@sn";

            valueCollection[0] = generateArray(customerSN);
            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        }

        function onPrintSucc(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            var index = 0;
            var printlist = new Array();
            var tmpList = new Array("WWAN ID Label", "Wimax Label",
                    "China label", "Taiwan Label", "ICASA Label L", "GOST Lable", "KC Label",
                    "LA NOM Label","Win8BoxLabel");
            for(var i=0;i<=postellabel.length;i++)
            {
                tmpList.push(postellabel[i]);
            }
            var count = 0;
            var showList = "";

            try {
                if (result[0] == SUCCESSRET) {

                    for (var i = 0; i < tmpList.length; i++) {
                        //setPrintItemListParam(result[1], tmpList[i]);
                        if (i < 2) {
                            if (tmpList[i] != wlabel) {
                                continue;
                            }
                        }
                        else if (i <= 6) {
                            if (tmpList[i] != clabel) {
                                continue;
                            }
                        }
						else if (i <= 7) {
                            if (tmpList[i] != llabel) {
                                continue;
                            }
                        }

                        else if(i<=8) {
                            if (tmpList[i] != win8label) {
                                continue;
                            }
                        }


                        index = getTemp(result[1], tmpList[i]);
                        if (index >= 0) {
                            setPrintItemListParam(result[1][index], tmpList[i]);
                            printlist[count] = result[1][index];
                            count++;

                            if (showList != "") {
                                showList = showList + ",";
                            }
                            showList = showList + tmpList[i];
                        }
                    }

                    if (wlabel == "Wimax Label") {
                        alert(msgPrintWLabel);
                    }


                    //==========================================print process=======================================
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    
                
                    
                    if (count > 0) {
                        printLabels(printlist, true);
                    }
                    //==========================================end print process===================================
                    //ResetPage();
                    if (cmessage != "") {
                        alert(cmessage);
                    }
  
                    showList = msgPrintDetail + showList;
            
				
                    ShowSuccessfulInfoFormatDetail(true, "", "Customer SN", customerSN, showList);
                    document.getElementById("<%=QueryChk.ClientID%>").checked = false;
                    callNextInput();
                }
                else if (result == null) {
                    ResetPage();
                    ShowMessage("msgSystemError");
                    ShowInfo("msgSystemError");
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                }
            }
            catch (e) {
                alert(e.description);
                ResetPage();
                ShowInfo(e.description);
                callNextInput();
            }

        }


        function OnCancel() {
            if (!(productID == "")) {
                WebServicePDPALabel01.cancel(productID);
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

        //set commport For Led
        function setobjMSCommParaForLights() {
            var objMSComm = document.getElementById("objMSComm");
            if (objMSComm.CommPort != 1) {
                if (objMSComm.PortOpen) {
                    objMSComm.PortOpen = false;
                }
                objMSComm.CommPort = 1;
            }

            objMSComm.Settings = "9600,n,8,1";
            objMSComm.RThreshold = 1;
            objMSComm.SThreshold = 1;
            objMSComm.Handshaking = 0;

            try {
                if (!objMSComm.PortOpen)
                    objMSComm.PortOpen = true;
            } catch (e) {
                alert(e.description);
            }
        }

        function turnOnLight(info) {
            var labelList = info[2];
            var objMSComm = document.getElementById("objMSComm");
            var lightArray = new Array();


            for (var i = 0; i < 192; i++) {
                lightArray.push("0");
            }

            for (var i = 0; i < labelList.length; i++) {
                //alert("LightNo is " + labelList[i]["LightNo"]);
                var lightno = labelList[i]["LightNo"] - 1;
                lightArray[lightno] = "1";
            }

            var lightChar = "";
            for (var i = 0; i < 24; i++) {
                var temp = "";
                for (var j = 7; j > -1; j--) {

                    temp = temp + lightArray[i * 8 + j];
                }
                //alert("temp is" + temp + " parseInt(temp, 2) is " + parseInt(temp, 2));
                lightChar += String.fromCharCode(parseInt(temp, 2));
            }

            objMSComm.Output = lightChar;

        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function clkSetting() {

            //var code = getLabelKittingCodeCmbValue();
            //stationId = "91";
            //pCode = "OPPA005";
            showPrintSetting(stationId, pCode);
        }
        function clkReprint() {

            var url = "../PAK/PDPALabel01Reprint.aspx" + "?Station=" + stationId + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login;
            window.showModalDialog(url, "", 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
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
		
		var emptyJpgSrc = document.getElementById("<%=ShowImage0.ClientID%>").src;
		function maximizeImage(f) {
			var jpgCenter=document.getElementById("jpgCenter");
			if(emptyJpgSrc==f.src || ''==f.src)return;
			jpgCenter.src=f.src;
			jpgCenter.style.display = 'block';
        }
        function minimizeImage(f) {
			var jpgCenter=document.getElementById("jpgCenter");
			jpgCenter.style.display = 'none';
        }
    </script>

</asp:Content>
