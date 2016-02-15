<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for KP Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI KeyParts Repair.docx
 * UC:CI-MES12-SPEC-FA-UC KeyParts Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-26  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="KPRepair.aspx.cs" Inherits="FA_KPRepair" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/KPRepairService.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                         
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblCTNO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtCTNO" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>                      
                    </td>
                </tr>       
                
                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="200px" OnGvExtRowClick="selectRow(this)" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="190px" OnRowDataBound="gd_DataBound" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="Icon"  />
                                        <asp:BoundField DataField="Defect" />
                                        <asp:BoundField DataField="Cause" />
                                        <asp:BoundField DataField="CDate" />
                                        <asp:BoundField DataField="UDate" />
                                        <asp:BoundField DataField="hidCol" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="500">
                            <tr>
                                <td style="width:150" align="center">
                                    <button type="button" onclick="addLog();" style ="width:110px; height:24px;">
                                        <%=this.GetLocalResourceObject(Pre + "_btnAdd").ToString()%>
                                    </button>
                                </td>
                                <td style="width:150" align="center">
                                    <button type="button" onclick="editLog();" style ="width:110px; height:24px;">
                                        <%=this.GetLocalResourceObject(Pre + "_btnEdit").ToString()%>
                                    </button>
                                </td>
                                <td style="width:150" align="center">
                                    <button type="button" onclick="finishRepair();" style ="width:110px; height:24px;">
                                        <%=this.GetLocalResourceObject(Pre + "_btnFinish").ToString()%>
                                    </button>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="btnProcess" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnRefresh" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnReset" runat="server" type="button" onclick="" style="display: none" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <input type="hidden" runat="server" id="hidInput" />
                    <input type="hidden" runat="server" id="hidRowCnt" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <input type="hidden" id="hidSelectedData" />
        </center>
    </div>

    <script type="text/javascript">        
        
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var selectedRowIndex = -1;
        //Used in modeldialog
        var feature = "dialogHeight:350px;dialogWidth:800px;center:yes;status:no;help:no";
        var url = "KPRepairLogAddEdit.aspx?Customer=<%=Customer %>";
        var status;
        var editor = "<%=UserId %>";
        var customer = "<%=Customer %>";
        var stationId = "<%=Station %>";
        var editObj;
	    var globalCTNO = "";

        document.body.onload = function() {
            try {
                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }
        }

        function processDataEntry(inputData) {
            ShowInfo("");
            inputData = inputData.trim();

            if (getPdLineCmbValue() == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString()%>');
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            
            if (inputData == "7777") {
                document.getElementById("<%=btnReset.ClientID%>").click();
                ShowInfo("");
                getAvailableData("processDataEntry");
                return;
            }
            
            if (inputData.length != 14) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadCTNO").ToString()%>');
                callNextInput();
                return;
            }
            
            if (globalCTNO != "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNotFinish").ToString()%>');
                getAvailableData("processDataEntry");
                return;
            }
            
            document.getElementById("<%=hidInput.ClientID%>").value = inputData;
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnProcess.ClientID%>").click();
        }

        function selectRow(row) {
            if (selectedRowIndex == row.index) {
                return;
            }
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=GridViewExt1.ClientID %>");
            selectedRowIndex = row.index;
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, "<%=GridViewExt1.ClientID %>");

            if (document.getElementById("hidSelectedData").value == row.cells[1].innerText.trim()) {
                return;
            }
            document.getElementById("hidSelectedData").value = row.cells[1].innerText.trim();
        }

        function callNextInput() {
            selectedRowIndex = -1;
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        function getDefectInfo() {
            editObj = new DefectInfo();
            tblObj = document.getElementById("<%=GridViewExt1.ClientID %>");
            selRowNo = parseInt(selectedRowIndex) + 1;

            var valueArray = tblObj.rows[selRowNo].cells[5].innerText.split("\u0003");

            editObj.id = valueArray[0];
            editObj.repairID = valueArray[1];
            editObj.type = valueArray[2];
            editObj.obligation = valueArray[3];
            editObj.component = valueArray[4];
            editObj.site = valueArray[5];
            editObj.majorPart = valueArray[6];
            editObj.remark = valueArray[7].replace(/___M_NL___/g, "\r\n");
            editObj.vendorCT = valueArray[8];
            editObj.partType = valueArray[9];
            editObj.oldPart = valueArray[10];
            editObj.oldPartSno = valueArray[11];
            editObj.newPart = valueArray[12];
            editObj.newPartSno = valueArray[13];
            editObj.manufacture = valueArray[14];
            editObj.versionA = valueArray[15];
            editObj.versionB = valueArray[16];
            editObj.returnSign = valueArray[17];
            editObj.mark = valueArray[18];
            editObj.subDefect = valueArray[19];
            editObj.piaStation = valueArray[20];
            editObj.distribution = valueArray[21];
            editObj._4M = valueArray[22];
            editObj.responsibility = valueArray[23];
            editObj.action = valueArray[24];
            editObj.cover = valueArray[25];
            editObj.uncover = valueArray[26];
            editObj.trackingStatus = valueArray[27];
            editObj.isManual = valueArray[28];
            editObj.editor = valueArray[29];
            editObj.newPartDateCode = valueArray[30];
            editObj.defectCodeID = valueArray[31];
            editObj.cause = valueArray[32];
            editObj.side = valueArray[33];
        }

        function addLog() {
            if (globalCTNO == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoCTNOInput").ToString()%>');
                return;
            }
            status = "A";
            window.showModalDialog(url, window, feature);
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnRefresh.ClientID%>").click();
        }

        function editLog() {
            if (globalCTNO == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoCTNOInput").ToString()%>');
                return;
            }
            cnt = parseInt(document.getElementById("<%=hidRowCnt.ClientID%>").value);            
            if (selectedRowIndex == -1 || selectedRowIndex >= cnt) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectData").ToString()%>');
                return;
            }
            getDefectInfo();
            status = "E";
            window.showModalDialog(url, window, feature);
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnRefresh.ClientID%>").click();
        }

        function finishRepair() {
            if (globalCTNO == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoCTNOInput").ToString()%>');
                return;
            }
            if (!canSave()) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgCannotSave").ToString()%>');
                return;
            }
            ShowInfo("");
            beginWaitingCoverDiv();
            
            KPRepairService.save(globalCTNO, saveSucc, saveFail);
        }

        function saveSucc(result) {
            endWaitingCoverDiv();
            ShowSuccessfulInfo(true, '[' + globalCTNO + ']' + '<%=this.GetLocalResourceObject(Pre + "_msgSaveSuccess").ToString()%>');
            document.getElementById("<%=btnReset.ClientID%>").click();
            callNextInput();
        }

        function saveFail(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            endWaitingCoverDiv();
            document.getElementById("<%=btnReset.ClientID%>").click();
            callNextInput();
        }

        function canSave() {
            cnt = parseInt(document.getElementById("<%=hidRowCnt.ClientID%>").value);
            tblObj = document.getElementById("<%=GridViewExt1.ClientID %>");
            for (i = 1; i <= cnt; i++) {
                if (tblObj.rows[i].cells[2].innerText == "") return false;
            }
            return true;
        }
        
        function isExistDefectInTable(defect, objId)
        {
            recordCount = parseInt(document.getElementById("<%=hidRowCnt.ClientID %>").value);
            
            tblObj = document.getElementById("<%=GridViewExt1.ClientID %>");
            
            for (i = 0; i < recordCount; i++)
            {
                if (tblObj.rows[i + 1].cells[1].innerText.indexOf(defect) == 0)
                {
                    if (status == "E")
                    {
                        id = tblObj.rows[i + 1].cells[5].innerText.split("\u0003")[0];
                    
                        if (id != objId)
                        {
                            return true;
                        }
                    }
                    else 
                    {
                        return true;
                    }
                    
                }
            }
            
            return false;
        }

        function clearSession() {
            if (globalCTNO != "") {
                KPRepairService.cancel(globalCTNO);
            }
        }

        window.onbeforeunload = clearSession;
        window.onunload = clearSession;
    </script>

</asp:Content>
