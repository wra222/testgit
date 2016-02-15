<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-FA-UI RCTO LCM Defect Input.docx
* UC:CI-MES12-SPEC-FA-UC RCTO LCM Defect Input.docx             
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-24   Du.Xuan               Create   
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LCMDefectInputForRCTO.aspx.cs" Inherits="FA_LCMDefectInputForRCTO"
    Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceLCMDefectInputForRCTO.asmx" />
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
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td style="width: 80px;">
                        <asp:Label ID="lblCTNO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblCTNOContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                            <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var editor;
        var customer;
        var stationId;
        var inputObj;
        var emptyPattern = /^\s*$/;

        var productID = "";
        var ctno = "";

        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSN").ToString() %>';
        var msgInputPdLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgConfirm = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirm").ToString() %>'; 

        window.onload = function() {

            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            getPdLineCmbObj().selectedIndex = 0;
            initPage();
            callNextInput();

        };

        window.onbeforeunload = function() {

            OnCancel();

        };
        function initPage() {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCTNOContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            productID = "";
            ctno = "";
        }


        function input(inputData) {

            if (inputData == "7777") {
                ResetPage();
                return;
            }

            var line = getPdLineCmbValue();
            if (line == "") {
                alert(msgInputPdLine);
                callNextInput();
                return;
            }

            if (inputData == "9999") {
                ShowInfo("");
                save();
            }
            else if (inputData.length == 14) {
                ShowInfo("");
                beginWaitingCoverDiv();
                ctno = inputData;
                WebServiceLCMDefectInputRCTO.inputCTNO(inputData, line, editor, stationId, customer, inputSucc, inputFail);
            }
            else if (inputData.length == 4) {
                inputDefect(inputData);
            }
            else {
                alert(msgInputSN);
                callNextInput();
            }

        }

        function inputSucc(result) {

            endWaitingCoverDiv();
            setInfo(result);
            callNextInput();

        }

        function inputFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }
            return false;
        }

        function setInfo(info) {
            //set value to the label
            productID = info[1];
            
            setInputOrSpanValue(document.getElementById("<%=lblCTNOContent.ClientID %>"), ctno);
            //set defectCache value
            defectCache = info[0];
        }

        function save() {
            if (defectInTable.length==0) {
                alert(msgInputDefect);
                callNextInput();
                return;
            }

            if (!confirm(msgConfirm)) {
                ResetPage();
                callNextInput();
                return;
            } 
            beginWaitingCoverDiv();
            WebServiceLCMDefectInputRCTO.save(productID, defectInTable, saveSucc, saveFail);
        }

        function saveSucc(result) {

            endWaitingCoverDiv();
            var tmpinfo = ctno;
            ResetPage();
            ShowSuccessfulInfoFormat(true, "CTNO", tmpinfo);
            callNextInput();
        }


        function saveFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function inputDefect(data) {

            if (isExistInTable(data)) {
                //error message
                alert(msgDuplicateData);
                callNextInput();
                return;
            }

            if (isExistInCache(data)) {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;

                rowArray.push(data);
                rowArray.push(desc);

                //add data to table
                if (defectInTable.length < 12) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }

                defectInTable[defectCount++] = data;
                callNextInput();
            }
            else {
                alert(msgInputValidDefect);
                callNextInput();
            }
        }

        function isExistInTable(data) {
            if (defectInTable != undefined && defectInTable != null) {
                for (var i = 0; i < defectInTable.length; i++) {
                    if (defectInTable[i] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function isExistInCache(data) {
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

        function OnCancel() {
            if (productID != "") {
                WebServiceLCMDefectInputRCTO.cancel(productID);
            }
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
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
    </script>

</asp:Content>
