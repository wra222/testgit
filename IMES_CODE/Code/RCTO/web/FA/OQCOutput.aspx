<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for RCTO OQC Output Page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-21  itc202017             Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="OQCOutput.aspx.cs" Inherits="FA_OQCOutput" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceOQCOutput.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="txtPdLine" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                </tr>          
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblProId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="txtProId" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td></td>
                </tr>
                
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblPassCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 25%" align="left">
                        <asp:Label ID="txtPassCnt" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblFailCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 25%" align="left">
                        <asp:Label ID="txtFailCnt" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td style="width: 20%" align="left"></td>
                </tr>   
                
                <tr>
                    <td colspan="3" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2" align="left">
                        <asp:Label ID="lblSupportDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3">
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                            GvExtWidth="100%" GvExtHeight="200px" OnRowDataBound="gd_DataBound" OnGvExtRowClick="selectRow(this)" OnGvExtRowDblClick=""
                            Width="99.9%" Height="190px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                            HighLightRowPosition="3" HorizontalAlign="Left">
                            <Columns>
                                <asp:BoundField DataField="dCode"  />
                                <asp:BoundField DataField="Descr"  />
                            </Columns>
                        </iMES:GridViewExt> 
                    </td>
                    <td colspan="2">
                        <asp:ListBox ID="lbDefectList" runat="server" Width="100%" Height="200px"></asp:ListBox>
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" colspan="3">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                    <td>
                        <asp:CheckBox id="chk9999" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">        
        
        var DEFAULT_ROW_COUNT = <%=initRowsCount %>;
        var tableClientID = "<%=gd.ClientID %>";
        var selectedRowIndex = -1;
        var objProdID = document.getElementById("<%=txtProId.ClientID%>");
        var objModel = document.getElementById("<%=txtModel.ClientID%>");
        var objLine = document.getElementById("<%=txtPdLine.ClientID%>");
        var objPassCnt = document.getElementById("<%=txtPassCnt.ClientID%>");
        var objFailCnt = document.getElementById("<%=txtFailCnt.ClientID%>");
        var objChk = document.getElementById("<%=chk9999.ClientID%>");
        var objSPDefectList = document.getElementById("<%=lbDefectList.ClientID%>");
        var qcStatus = "";
        var defectList = new Array();

        document.body.onload = function() {
            try {
                callNextInput();
            } catch (e) {
                alert(e.description);
            }
        }

        function ClearTable() {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, tableClientID);
            ClearGvExtTable(tableClientID, DEFAULT_ROW_COUNT + 1); 
            selectedRowIndex = -1;  
            defectList = new Array();             
        }

        function selectRow(row) {
            if (selectedRowIndex == row.index) {
                return;
            }
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, tableClientID);
            selectedRowIndex = row.index;
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, tableClientID);
        }

        function processDataEntry(inputData) {
            ShowInfo("");
            inputData = inputData.trim();
            if (inputData == "7777") {
                ClearTable();
                callNextInput();
                return;
            }
            if (inputData == "9999") {
                save();
                return;
            }
            if (objProdID.innerText.trim() == "") { //Should input prodid or custsn
                if (inputData.length == 9) {
                    OQCOutputService.input(inputData, true, "<%=UserId %>", "<%=StationId %>", "<%=Customer %>", InputSuccess, InputFail);
                }
                else if (inputData.length == 10 && inputData.indexOf("CNU") != 0) {
                    OQCOutputService.input(inputData.substring(0, 9), true, "<%=UserId %>", "<%=StationId %>", "<%=Customer %>", InputSuccess, InputFail);
                }
                else if (inputData.length == 10 && inputData.indexOf("CNU") == 0) {
                    OQCOutputService.input(inputData, false, "<%=UserId %>", "<%=StationId %>", "<%=Customer %>", InputSuccess, InputFail);
                }
                else {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgBadInput").ToString() %>');    //Bad Input
                    callNextInput();
                    return;
                }
            }
            else {  //Should input defect code
                if (inputData.length != 4) {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgBadDefect").ToString() %>');    //wrong defect code
                    callNextInput();
                    return;
                }
                var bAllow = false;
                var desc = ""
                for (var i = 0; i < objSPDefectList.options.length; i++) {
                    if (inputData == objSPDefectList.options[i].value) {
                        bAllow = true;
                        desc = objSPDefectList.options[i].text.substring(objSPDefectList.options[i].value.length + 1);
                    }
                }
                if (!bAllow) {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgNotSupportedDefect").ToString() %>');    //not support defect
                    callNextInput();
                    return;
                }

                for (var i = 0; i < defectList.length; i++) {
                    if (inputData == defectList[i]) {
                        alert('<%=this.GetLocalResourceObject(Pre + "_msgRepeatedDefect").ToString() %>');    //repeated defect
                        callNextInput();
                        return;
                    }
                }

                var rowArray = new Array();
                var rw;
                rowArray.push(inputData);
                rowArray.push(desc);
                if (defectList.length < DEFAULT_ROW_COUNT)
                {   
                    eval("ChangeCvExtRowByIndex_" + tableClientID + "(rowArray, false, defectList.length + 1);");
                    setSrollByIndex(defectList.length, true, tableClientID);
                    selectedRowIndex = defectList.length;
                }
                else
                {
                    eval("rw = AddCvExtRowToBottom_" + tableClientID + "(rowArray, false);");
                    setSrollByIndex(defectList.length, true, tableClientID);
                    selectedRowIndex = defectList.length;
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                
                defectList.push(inputData);
                callNextInput();
                return;
            }
        }

        function InputSuccess(result) {
            objProdID.innerText = result[0][0];
            objModel.innerText = result[0][1];
            objLine.innerText = result[0][2];
            qcStatus = result[0][3];
            if (objChk.checked) {
                save();
                return;
            }
            
            if (qcStatus == "EFI") {
                ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgNeedDefect").ToString() %>',"red");
            }

            callNextInput();
            return;
        }

        function InputFail(result) {
            ShowMessage(result._message);
            callNextInput();
            return;
        }

        function save() {
            if (qcStatus == "EFI" && defectList.length == 0) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNeedDefect").ToString() %>');    //EFI,need defect
                callNextInput();
                return;
            }
            OQCOutputService.save(objProdID.innerText.trim(), defectList, SaveSuccess, SaveFail);
        }

        function SaveSuccess(result) {
            ShowSuccessfulInfoFormat(true, "ProductID", objProdID.innerText);
            objProdID.innerText = "";
            objModel.innerText = "";
            objLine.innerText = "";
            qcStatus = "";
            if (defectList.length > 0)
            {
                objFailCnt.innerText = parseInt(objFailCnt.innerText.trim()) + 1;
            }
            else
            {
                objPassCnt.innerText = parseInt(objPassCnt.innerText.trim()) + 1;
            }
            ClearTable();
            callNextInput();
            return;
        }

        function SaveFail(result) {
            objProdID.innerText = "";
            objModel.innerText = "";
            objLine.innerText = "";
            qcStatus = "";
            ClearTable();
            ShowMessage(result._message);     
            callNextInput();
            return;
        }

        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        window.onbeforeunload = function() {
            OQCOutputService.cancel(objProdID.innerText.trim());
        }

        window.onunload = function() {
            OQCOutputService.cancel(objProdID.innerText.trim());
        }
    </script>

</asp:Content>
