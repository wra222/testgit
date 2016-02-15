<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Sorting.docx –2012/07/18
* UC:CI-MES12-SPEC-PAK-UC PAQC Sorting.docx –2012/07/18            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-18   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PAQCSorting.aspx.cs" Inherits="PAK_PAQCSorting" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePAQCSorting.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="99%" border="0" style="table-layout: fixed;">
                <tr>
                    <td style="width: 80px;">
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
            </table>
            <hr />
        </div>
        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="div2" style="height: 366px">
                    <fieldset style="width: 98%">
                        <legend align="left" style="height: 20px">
                            <asp:Label ID="lblCheckStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </legend>
                        <table width="100%" border="0" style="table-layout: fixed;">
                            <tr>
                                <td>
                                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                        GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                        HighLightRowPosition="3" AutoHighlightScrollByValue="True" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                                        OnRowDataBound="gd_RowDataBound" EnableViewState="false">
                                    </iMES:GridViewExt>
                                    <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none">
                                    </button>
                                </td>
                            </tr>
                        </table>
                     </fieldset>
                </div>     
                <div id="div3">
                    <table width="99%">
                        <tr>
                            <td align="right">
                                <button id="btnResetPage" runat="server" onclick="btnResetPage_Click()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dOldId" runat="server" />
        <input type="hidden" id="dVendor" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var gprodId = "";
        var editor;
        var customer;
        var stationId;
        var inputObj;
        var emptyPattern = /^\s*$/;

        var productID = "";
        var customerSN = "";

        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSN").ToString() %>';
        var msgInputPdLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>'
        var msgInputStation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputStation").ToString() %>';
        var msgErrLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgErrLine").ToString() %>';
        var msgFinish = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgFinish").ToString() %>';

        var time = 30;

        window.onload = function() {

            //getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            getPdLineCmbObj().selectedIndex = 0;

            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=UserId%>";

            //设置表格的高度  
            //resetTableHeight();
            WebServicePAQCSorting.GetSysSetting("PAQCSortingInterval", onGetSuccess, onGetFail);
            //initPage();
            //getPdLineCmbObj().selectedIndex = 1;
            //resetTable();
        };

        window.onbeforeunload = function() {
            //OnCancel();
        };

        function onGetSuccess(result) {
            time = result[1];
            initPage();
            getPdLineCmbObj().selectedIndex = 1;
            resetTable();
        }
        function onGetFail(result) {
            time = 30;
            initPage();
            getPdLineCmbObj().selectedIndex = 1;
            resetTable();
        }
        function initPage() {

            tbl = "<%=gd.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            productID = "";
            customerSN = "";
            //getPdLineCmbObj().selectedIndex = 247;
        }

        var iSelectedRowIndex = null;
        var submitIndex = null;
        function input(inputData) {

            var line = getPdLineCmbValue();
            if (line == "") {
                alert(msgInputPdLine);
                callNextInput();
                return;
            }
            if (iSelectedRowIndex == null) {
                alert(msgInputStation);
                callNextInput();
                return;
            }
            //ITC-1413-0091
            if (inputData.length == 10 && inputData.substring(0, 2) == "CN") {
                inputData = inputData;
            }
            else if (inputData.length == 11 && (inputData.substring(0, 3) == "SCN" || inputData.substring(0, 3) == "PCN" || inputData.substring(0, 3) == "ACN")) {
                inputData = inputData.substring(1, 11);
            }
            else {
                alert(msgInputSN);
                callNextInput();
                return;
            }

            ShowInfo("");
            beginWaitingCoverDiv();
            customerSN = inputData;
            submitIndex = iSelectedRowIndex;
            WebServicePAQCSorting.input(line, customerSN, editor, stationId, customer, inputSucc, inputFail);

        }

        function inputSucc(result) {

            endWaitingCoverDiv();

            productID = result[0];
            /*var proline = result[1];
            var line = getPdLineCmbValue();
            if (proline != line) {
            alert(msgErrLine);
            callNextInput();
            return;
            }
            */
            save();

        }

        function inputFail(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            resetTable();
            callNextInput();
        }

        function save() {

            var gvTable = document.getElementById("<%=gd.ClientID%>");
            var sortingID = gvTable.rows[iSelectedRowIndex + 1].cells[6].innerText;
            var station = gvTable.rows[iSelectedRowIndex + 1].cells[1].innerText;
            beginWaitingCoverDiv();
            //save(sortingID, editor, station, customer);
            WebServicePAQCSorting.save(sortingID, customerSN, editor, station, customer, saveSucc, saveFail);
        }

        function saveSucc(result) {

            endWaitingCoverDiv();
            var endFlag = result[0];
            if (endFlag) {
                ShowInfo(msgFinish);
            }
            resetTable();
            callNextInput();
        }

        function saveFail(result) {
            endWaitingCoverDiv();
            resetTable();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function resetTable() {
            submitIndex = iSelectedRowIndex;
            document.getElementById("<%=btnReset.ClientID%>").click();
        }


        function clickTable(con) {
            setGdHighLight(con);
        }

        function setGdHighLight(con) {
            var gvTable = document.getElementById("<%=gd.ClientID%>");

            var currentId = con.cells[6].innerText.trim();

            if ((iSelectedRowIndex != null) && (iSelectedRowIndex == parseInt(con.Index, 10))) {
                if (currentId == "" || currentId < 0) {
                    setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
                    iSelectedRowIndex = null;
                }
                return;
            }

            if (currentId == "" || currentId < 0) {
                return;
            }

            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
                gvTable.rows[iSelectedRowIndex + 1].cells[0].innerText = "";
            }

            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            con.cells[0].innerText = "√";
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function reHighLight() {

            var gvTable = document.getElementById("<%=gd.ClientID%>");
            if (submitIndex != null) {

                var currentId = gvTable.rows[submitIndex + 1].cells[6].innerText.trim();
                if (currentId == "" || currentId < 0) {
                    submitIndex = null;
                    return;
                }
                setRowSelectedOrNotSelectedByIndex(submitIndex, true, "<%=gd.ClientID %>");
                gvTable.rows[submitIndex + 1].cells[0].innerText = "√";
                //ITC-1413-0090
                iSelectedRowIndex = submitIndex;
            }

        }

        function callNextInput() {

        }

        function ExitPage() {
            //OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            //callNextInput();
        }

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 200;
            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }

        function setDefault() {
            iSelectedRowIndex = null;
            window.setTimeout("timeOpp()", 100);
        }

        function showStation(stationstr) {
            var message = "";
            var line = getPdLineCmbValue();
            //Line: ”+ LEFT(@Line, 1) + “ Station: ” + @Station + “（” + @StationDescr + “）”
            if (stationstr != "") {
                message = msgFinish + "  Line: " + line.substring(0, 1) + "  Station: " + stationstr;
                ShowInfo(message);
            }
            reHighLight();
            window.setTimeout("resetTable()", time*1000);
        }

        function btnResetPage_Click() {

            if (iSelectedRowIndex == null) {
                alert(msgInputStation);
                return;
            }
            var gvTable = document.getElementById("<%=gd.ClientID%>");
            var sortingID = gvTable.rows[iSelectedRowIndex + 1].cells[6].innerText;
            var station = gvTable.rows[iSelectedRowIndex + 1].cells[1].innerText;
            submitIndex = iSelectedRowIndex;
            WebServicePAQCSorting.updateStation(sortingID, editor, station, customer, updateEnd);

        }
        
        function updateEnd(result) {
            resetTable(); 
        }                                                                                                              
    </script>

</asp:Content>
