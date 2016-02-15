<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:MBBorrow page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-01-10 Kerwin                Create      
 * 2012-01-30 Kerwin                ITC-1360-0161,ITC-1360-0159,ITC-1360-0149
 * 2012-01-30 Kerwin                ITC-1360-0155    
 * 2012-01-30 Kerwin                ITC-1360-0146    
 * 2012-01-30 Kerwin                ITC-1360-0144
 * 2012-01-30 Kerwin                ITC-1360-0354
 * 2012-01-30 Kerwin                ITC-1360-0739
 * 2012-04-11 Kerwin                ITC-1360-1648
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MBBorrow.aspx.cs" Inherits="SA_MBBorrow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/MBBorrowWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <table width="100%">
            <tr>
                <td style="width: 20%" />
                <td style="width: 50%">
                    <input id="RadioBorrow" type="radio" name="OperateType" value="B" />
                    <a style="width: 120px">Borrow</a>
                    <input id="RadioReturn" type="radio" name="OperateType" value="R" />
                    <a style="width: 120px">Return</a>
                    <input id="RadioAll" type="radio" name="OperateType" checked="checked" value="" />
                    <a style="width: 120px">All</a>
                </td>
                <td style="width: 15%">
                    <input id="BtnQuery" type="button" value="Query" onclick="if(CheckQuery())" runat="server"
                        onserverclick="QueryBorrowList" style="width: 80px; text-align: center; cursor: pointer;" />
                </td>
                <td style="width: 15%">
                    <input id="BtnExcel" type="button" value="Excel" onclick="if(CheckQuery())" runat="server"
                        onserverclick="ToExcel" style="width: 80px; text-align: center; cursor: pointer;" />
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%">
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelMBPodIdCT" runat="server"></asp:Label>
                </td>
                <td style="width: 30%">
                    <input id="InputMBPodIdCT" type="text" readonly="readonly" style="width: 160px" />
                </td>
                <td style="width: 20%">
                    <asp:Label ID="LabelModel" runat="server"></asp:Label>
                </td>
                <td style="width: 30%">
                    <input id="InputModel" type="text" readonly="readonly" style="width: 160px" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelBorrower" runat="server"></asp:Label>
                </td>
                <td style="width: 30%">
                    <input id="InputBorrower" type="text" readonly="readonly" style="width: 160px" />
                </td>
                <td style="width: 20%">
                    <asp:Label ID="LabelLender" runat="server"></asp:Label>
                </td>
                <td style="width: 30%">
                    <input id="InputLender" type="text" readonly="readonly" style="width: 160px" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td style="width: 50%">
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="14" />
                </td>
                <td style="width: 15%">
                    <input id="BtnLend" type="button" value="Lend" onclick="Lend()" style="width: 80px;
                        text-align: center; cursor: pointer;" />
                </td>
                <td style="width: 15%">
                    <input id="BtnAccept" type="button" value="Accept" onclick="Accept()" style="width: 80px;
                        text-align: center; cursor: pointer;" />
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%">
            <tr>
                <td style="width: 100%;" align="left" colspan="4">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnQuery" EventName="ServerClick" />
                            <asp:PostBackTrigger ControlID="BtnExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                GvExtWidth="98%" GvExtHeight="170px" Width="98%" SetTemplateValueEnable="true"
                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                <Columns>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="Model" HeaderText="Model" HeaderStyle-Width="13%" />
                                    <asp:BoundField DataField="Borrower" HeaderText="Borrower" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="Lender" HeaderText="Lender" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="Returner" HeaderText="Returner" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="Accepter" HeaderText="Accepter" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="6%" />
                                    <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" HeaderStyle-Width="15%" />
                                </Columns>
                            </iMES:GridViewExt>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <input id="HiddenSelectType" type="hidden" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
        var sessionKey = "";
        var IsPass = false;
        var editor;
        var customer;
        var hostname = "";
        var langPre = "eng_";
        var station = "WJ";
        var line = "";
        var dataEntryObj;
        var key = "";
        var keyType = "";
        var productModel = "";

        var AlertModel = "Product and Model does not match!";
        var AlertInputModel = "please input Model!";
        var AlertInputBorrower = "please input Borrower!";
        var AlertInputReturner = "please input Returner!";
        var AlertInputMBProductCT = "please input MB Sno/Product ID/CT No!";
        var AlertWrongFormat = "Input data error!";
        var AlertSelectBorrowStatus = "please select query type!";
        var AlertLendSuccess = "Lend successfully!";
        var AlertReturnSuccess = "Return successfully!";
        var AlertNoNeedModel = "Do not need input Model!";

        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';
            langPre = '<%=Pre %>';

            AlertNoNeedModel = '<%=AlertNoNeedModel %>';
            AlertLendSuccess = '<%=AlertLendSuccess %>';
            AlertReturnSuccess = '<%=AlertReturnSuccess %>';
            AlertSelectBorrowStatus = '<%=AlertSelectBorrowStatus %>';
            AlertWrongFormat = '<%=AlertWrongFormat %>';
            AlertModel = '<%=AlertModel %>';
            AlertInputModel = '<%=AlertInputModel %>';
            AlertInputMBProductCT = '<%=AlertInputMBProductCT %>';
            AlertInputBorrower = '<%=AlertInputBorrower %>';
            AlertInputReturner = '<%=AlertInputReturner %>';

            document.getElementById("InputLender").value = editor;

            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                dataEntryObj.focus();
            } catch (e) { }
        };

        function InputDataEntry(InputData) {
            var InputDataLength = InputData.length;
            switch (InputDataLength) {
                case 6:
                    document.getElementById("InputBorrower").value = InputData;
                    getAvailableData("InputDataEntry");
                    break;
                case 9:
                    Cancel();
                    keyType = "Product";
                    key = InputData;
                    MBBorrowWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    break;
                case 10:
                    Cancel();
                    if (InputData.substr(4, 1) == "M" || InputData.substr(4, 1) == "B") {
                        keyType = "MB";
                        key = InputData;
                        MBBorrowWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    } else {
                        keyType = "Product";
                        key = InputData;
                        MBBorrowWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    }
                    break;
                case 11:
                    Cancel();
                    // if (InputData.substr(4, 1) != "M") {
                    if ((InputData.substr(4, 1) != "M") && (InputData.substr(5, 1) != "M")) {
                        if((InputData.substr(4, 1) != "B") && (InputData.substr(5, 1) != "B"))
                        {
                            alert(AlertWrongFormat);
                            getAvailableData("InputDataEntry");
                            return;
                        }
                    }
                    keyType = "MB";
                    if (InputData.substr(4, 1) == "M" || InputData.substr(4, 1) == "B")
                        key = InputData.substr(0, 10);
                    else
                        key = InputData;
                    MBBorrowWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    break;
                case 12:
                    if (keyType == "Product") {
                        if (productModel != InputData) {
                            alert(AlertModel);
                        } else {
                            document.getElementById("InputModel").value = InputData;
                        }
                    } else if (keyType == "") {
                        document.getElementById("InputModel").value = InputData;
                    } else {
                        alert(AlertNoNeedModel);
                    }
                    getAvailableData("InputDataEntry");
                    break;
                case 14:
                    Cancel();
                    keyType = "CT";
                    key = InputData;
                    MBBorrowWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    break;
                default:
                    alert(AlertWrongFormat);
                    dataEntryObj.focus();
                    getAvailableData("InputDataEntry");
                    break;
            }
        }

        function InputKeySucceed(result) {
            if (result != null) {
                if (result.length == 3) {
                    if (keyType == "Product" && document.getElementById("InputModel").value != "" && document.getElementById("InputModel").value != result[1]) {
                        alert(AlertModel);
                        Cancel();
                    } else {
                        if (keyType == "Product") {
                            productModel = result[1];
                        } else {
                            document.getElementById("InputModel").value = "";
                        }
                        document.getElementById("InputMBPodIdCT").value = result[2];
                    }
                    getAvailableData("InputDataEntry");
                } else if (result.length == 1) {
                    keyType = "";
                    key = "";
                    document.getElementById("InputMBPodIdCT").value = "";
                    ShowError(result[0]);
                }
            }
        }

        function Lend() {
            if (key == "") {
                alert(AlertInputMBProductCT);
                dataEntryObj.focus();
                return;
            }

            var borrowerOrReturner = document.getElementById("InputBorrower").value;
            if (borrowerOrReturner == "") {
                alert(AlertInputBorrower);
                dataEntryObj.focus();
                return;
            }
            if (keyType == "Product" && document.getElementById("InputModel").value == "") {
                alert(AlertInputModel);
                dataEntryObj.focus();
                return;
            }
            MBBorrowWebService.Save(key, keyType, borrowerOrReturner, "Borrow", SaveSucceed);
        }


        function Accept() {
            if (key == "") {
                alert(AlertInputMBProductCT);
                dataEntryObj.focus();
                return;
            }

            var borrowerOrReturner = document.getElementById("InputBorrower").value;
            if (borrowerOrReturner == "") {
                alert(AlertInputReturner);
                dataEntryObj.focus();
                return;
            }
            if (keyType == "Product" && document.getElementById("InputModel").value == "") {
                alert(AlertInputModel);
                dataEntryObj.focus();
                return;
            }
            MBBorrowWebService.Save(key, keyType, borrowerOrReturner, "Return", SaveSucceed);
        }

        function SaveSucceed(result) {
            var SuccessKey = "["+key + "] ";
            ResetUI();
            if (result != null) {
                if (result.length == 2) {
                    if (result[1] == "Borrow") {
                        ShowSuccessfulInfo(true, SuccessKey + AlertLendSuccess);
                    } else {
                        ShowSuccessfulInfo(true, SuccessKey + AlertReturnSuccess);
                    }
                } else if (result.length == 1) {
                    ShowError(result[0]);
                }
            }
            dataEntryObj.focus();
        }


        function CheckQuery() {
            var selectType = "NotChecked";
            var OperateTypeObjList = document.getElementsByName("OperateType");
            for (var i = 0; i < OperateTypeObjList.length; i++) {
                if (OperateTypeObjList[i].checked) {
                    selectType = OperateTypeObjList[i].value;
                    break;
                }
            }
            if (selectType == "NotChecked") {
                alert(AlertSelectBorrowStatus);
                return false;
            } else {
                document.getElementById('<%=HiddenSelectType.ClientID%>').value = selectType;
                return true;
            }

        }

        function ResetUI() {
            document.getElementById("InputModel").value = "";
            document.getElementById("InputMBPodIdCT").value = "";
            document.getElementById("InputBorrower").value = "";
            keyType = "";
            key = "";
            productModel = "";
        }
        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
            getAvailableData("InputDataEntry");
            dataEntryObj.focus();
        }

        function Cancel() {
            if (key != "") {
                MBBorrowWebService.Cancel(key, keyType);
                key = "";
                keyType = "";
                productModel = "";
            }
        }

        window.onbeforeunload = function() {
            Cancel();
        };
    </script>

</asp:Content>
