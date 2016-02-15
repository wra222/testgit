<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PCAICTInput page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create          
 * 2012-03-19  Li.Ming-Jun           ITC-1360-1508
 * Known issues:
 * TODO:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DockingICTInput.aspx.cs" Inherits="Docking_ICTInput" Title="" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/WebServiceDockingICTInput.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 15%;" />
                    <col style="width: 35%;" />
                    <col style="width: 15%;" />
                    <col style="width: 35%;" />
                </colgroup>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" Stage="SA" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <label id="InputECR"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <label id="InputPassQty">
                            0</label>
                    </td>
                    <td>
                        <asp:Label ID="LabelFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <label id="InputFailQty">
                            0</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <label id="InputMBSno" ></label>
                    </td>
                    <td>
                        <asp:Label ID="lblAoiNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <label id="InputAOINo" ></label>
                    </td>
                </tr>
                <tr>
                     <td>
                        <asp:Label ID="lblBCCode" runat="server" CssClass="iMes_label_13pt" Text="B/C:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBCCodeContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                            GvExtHeight="220px" Style="top: 0px; left: 0px" Width="99.9%" Height="210px"
                            SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div3">
            <table width="100%">
                <colgroup>
                    <col style="width: 15%;" />
                    <col />
                    <col style="width: 20%;" />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                    <td>
                        <input id="btnPrintSet" type="button" runat="server" class="iMes_button" onclick="clkSetting()" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk9999" runat="server" Checked="false"></asp:CheckBox>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var defectCount = 0;
        var defectInTable = [];
        var mbSno = "";
        var customer;
        var stationId;
        var pCode;
        var inputObj;
        var inputCode = "";
        var qty;

        //error message
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSucc = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var msgInformation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInformation").ToString() %>';
        var msgOutputRepair = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgOutputRepair").ToString() %>';
        var msgReturnRepair = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReturnRepair").ToString() %>';
        var msgInputAQI = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputAQI").ToString() %>';
        var mesNoSelDateCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelectDateCode").ToString()%>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelPdLine").ToString()%>';
        var mesNoInputECR = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputECR").ToString()%>';
        var mesInputError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesInputError").ToString()%>';
        var mesNoInputMBSno = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputMBSno").ToString()%>';
        var mesNoInputAOINo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputAOINo").ToString()%>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
        var mesNoInputBC = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputB/C").ToString()%>';
        var mesBCCodeNoMatch = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesBCCodeNoMatch").ToString()%>';


        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            WebServiceDockingICTInput.GetDefectList(InitDefectList, inputFail)
            inputObj.focus();
        };

        function InitDefectList(result) {
            if (result[0] == SUCCESSRET) {
                defectCache = result[1];
            } else {
                ShowMessage(result[0], true);
                ShowInfo(result[0]);
            }
        }
        function clearFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function input(data) {
            data = data.trim();
            if (inputFlag) {
                if (data == "7777") {
                    ShowInfo("");
                    defectCount = 0;
                    defectInTable = [];

                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    setSrollByIndex(0, false);
                    getAvailableData("input");
                    return;
                }
                else if (data == "9999") {
                    ShowInfo("");
                    save();
                }
                else {
                    ShowInfo("");
                    inputCode = checkInput(data);
                    if (inputCode == "Defect") {
                        inputDefect(data);
                    }
                    else if (inputCode == "AOINo") {
                        document.getElementById("InputAOINo").innerText= data;
                        getAvailableData("input");
                        inputObj.focus();
                    }
                    else {
                        alert(mesInputError);
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }

            }
            else {
                if (data != "9999" && data != "7777") {
                    ShowInfo("");
                    inputCode = checkInput(data);
                    if (inputCode == "ECR") {
                        document.getElementById("InputECR").innerText= data;
                        getAvailableData("input");
                    }
                    else if (inputCode == "AOINo") {
                        document.getElementById("InputAOINo").innerText=data;
                        getAvailableData("input");
                    }
                    else if (inputCode == "MBSno") {
                        if (getPdLineCmbValue() == "") {
                            alert(mesNoSelPdLine);
                            getAvailableData("input");
                            setPdLineCmbFocus();
                            return;
                        }

                        if (document.getElementById("<%=lblBCCodeContent.ClientID %>").innerText.trim() == "") {
                            checkMBFlg = true;
                            alert(mesNoInputBC);
                            getAvailableData("input");
                            return;
                        }
                        
                        if (document.getElementById("InputECR").innerText.trim() == "") {
                            alert(mesNoInputECR);
                            getAvailableData("input");
                            return;
                        }

                        var BCCode = document.getElementById("<%=lblBCCodeContent.ClientID %>").innerText.trim();
                        var MBSnoCode = data.substr(0, 2);
                        if (BCCode != MBSnoCode) {
                            checkMBFlg = true;
                            alert(mesBCCodeNoMatch);
                            getAvailableData("input");
                            return;
                        }

                        WebServiceDockingICTInput.input(mbSno, document.getElementById("InputECR").innerText.trim(), '0', editor, getPdLineCmbValue(), stationId, customer, inputSucc, inputFail);
                    }
                    else if (inputCode == "B/CCode") {
                        setInputOrSpanValue(document.getElementById("<%=lblBCCodeContent.ClientID %>"), data);
                        getAvailableData("input");
                    }
                    else {
                        alert(mesInputError);
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }
                else {
                    alert(mesNoInputMBSno);
                    getAvailableData("input");
                    inputObj.focus();
                }
            }
        }

        function checkInput(value) {
            var code = "ERROR";
            try {
                if (value.length == 4) {
                    code = "Defect";
                }
                else if (value.length == 5) {
                    var pattern = /^([A-Z|0-9][0]{2}[A-Z|0-9]{2})$/;
                    if (pattern.test(value)) {
                        code = "ECR";
                    }
                }
                else if (value.length == 10) {
                    var val = value.substr(4, 1);
                    if (val == "M" || val =="B") {
                        code = "MBSno";
                        mbSno = value;
                    }
                    else if (val == "A") {
                        code = "AOINo";
                    }
                }
                else if (value.length == 11) {
                    var val = value.substr(4, 1);
                    if (val == "M" || val == "B") {
                        code = "MBSno";
                        mbSno = value.substr(0, 10);
                    } else {
                        var val = value.substr(5, 1);
                        if (val == "M" || val == "B") {
                            code = "MBSno";
                            mbSno = value;
                        }
                    }
                }
                else if (value.length == 2) {
                    code = "B/CCode";
                }
                return code;
            }
            catch (e) {
                alert(e.description);
            }
        }

        function inputSucc(result) {
            if (result[0] == SUCCESSRET) {
                document.getElementById("InputMBSno").innerText = mbSno;
                inputFlag = true;
                ShowInfo("");
                if (document.getElementById("<%=chk9999.ClientID%>").checked) {
                    save();
                    return;
                }
            }
            else {
                ShowMessage(result[0], true);
                ShowInfo(result[0]);
            }
            getAvailableData("input");
            inputObj.focus();
        }

        function inputFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }

            return false;
        }

        function save() {
            var checkSaveFlg = false;

            if (getPdLineCmbValue() == "") {
                checkSaveFlg = true;
                alert(mesNoSelPdLine);
                getAvailableData("input");
                setPdLineCmbFocus();
            }
            if (document.getElementById("InputECR").innerText.trim() == "") {
                checkSaveFlg = true;
                alert(mesNoInputECR);
                getAvailableData("input");
            }
            if (document.getElementById("InputMBSno").innerText.trim() == "") {
                checkSaveFlg = true;
                alert(mesNoInputMBSno);
                getAvailableData("input");
            }

            if (!checkSaveFlg) {
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    return;
                }

                WebServiceDockingICTInput.save(mbSno, document.getElementById("InputAOINo").innerText.trim(), defectInTable, lstPrintItem, saveSucc, saveFail);
            }
        }

        function saveSucc(result) {
            //endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                var printList = new Array();
                var k = 0;
                if (result[1] && result[1].length > 0) {

                    for (var i = 0; i < result[2].length; i++) {
                        for (var j = 0; j < result[1].length; j++) {
                            printList[k] = new PrintItem(result[1][j].PrintMode, result[1][j].RuleMode, result[1][j].LabelType, result[1][j].TemplateName, result[1][j].Piece, result[1][j].SpName, null, null, result[1][j].OffsetX, result[1][j].OffsetY, result[1][j].PrinterPort, result[1][j].dpi, result[1][j].BatPiece, result[1][j].Rotate180, result[1][j].Layout);
                            printList[k].ParameterKeys = new Array("@MBSno");
                            printList[k].ParameterValues = new Array(new Array(result[2][i]));
                            k++;
                        }
                        //setPrintItemListParam(result[1], result[2][i]);
                        //printLabels(result[1], false);
                    }
                    printLabels(printList, true);
                }
                if (isPass()) {
                    document.getElementById("InputPassQty").innerText = parseInt(document.getElementById("InputPassQty").innerText, 10) + 1;
                } else {
                    document.getElementById("InputFailQty").innerText = parseInt(document.getElementById("InputFailQty").innerText, 10) + 1;
                }
                ShowSuccessfulInfo(true, "[" + mbSno + "] " + msgSucc);
            }
            else {
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }
            initPage();
        }


        function saveFail(result) {
            //endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            initPage();
        }

        function inputDefect(data) {
            if (ExistInDefectTable(data)) {
                alert(msgDuplicateData);
                getAvailableData("input");
                inputObj.focus();
                return;
            }

            if (ExistInDefectCache(data)) {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;

                rowArray.push(data);
                rowArray.push(desc);

                if (defectInTable.length < 6) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    rw.cells[1].style.whiteSpace = "nowrap";
                }

                setSrollByIndex(defectInTable.length, false);

                defectInTable[defectCount++] = data;
                getAvailableData("input");
            }
            else {
                alert(msgInputValidDefect);
                getAvailableData("input");
                inputObj.focus();
            }
        }

        function ExistInDefectTable(data) {
            if (defectInTable != undefined && defectInTable != null) {
                for (var i = 0; i < defectInTable.length; i++) {
                    if (defectInTable[i] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function ExistInDefectCache(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function getDesc(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return defectCache[i]["description"];
                    }
                }
            }

            return "";
        }

        function clkSetting() {
            showPrintSetting(stationId, pCode);
        }

        window.onbeforeunload = function() {
            if (inputFlag) {
                OnCancel();
            }
        };

        function OnCancel() {
            WebServiceDockingICTInput.cancel(mbSno);
            mbSno = "";
        }

        function ResetPage() {
            OnCancel();
            initPage();
            ShowInfo("");
        }

        function initPage() {
            tbl = "<%=gd.ClientID %>";
            document.getElementById("InputMBSno").innerText= "";
            document.getElementById("InputAOINo").innerText= "";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0, false);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            getAvailableData("input");
            inputObj.focus();
        }
        function setPrintItemListParam(backPrintItemList, mbsn) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@MBSno";
            valueCollection[0] = generateArray(mbsn);
            //setPrintParam(lstPrtItem, "CT Label", keyCollection, valueCollection);
            setAllPrintParam(lstPrtItem, keyCollection, valueCollection);

        }           
    </script>

</asp:Content>
