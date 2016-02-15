<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:ChangeModel page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-07-26 Kerwin                Create      
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ChangeModel.aspx.cs" Inherits="Docking_ChangeModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/ChangeModelWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <table width="100%">
            <tr>
                <td style="width: 10%" />
                <td style="width: 25%">
                    <asp:Label ID="LabelModel1" runat="server"></asp:Label>
                </td>
                <td style="width: 65%">
                    <label id="InputModel1">
                    </label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%" />
                <td style="width: 25%">
                    <asp:Label ID="LabelCurrentStation" runat="server"></asp:Label>
                </td>
                <td style="width: 65%">
                    <select id="SelectStation" style="width: 220px" onchange="DisplayQty()">
                    </select>
                </td>
            </tr>
            <tr>
                <td style="width: 10%" />
                <td style="width: 25%">
                    <asp:Label ID="LabelQty" runat="server"></asp:Label>
                </td>
                <td style="width: 65%">
                    <label id="InputQty">
                    </label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%" />
                <td style="width: 25%">
                    <asp:Label ID="LabelChangeQty" runat="server"></asp:Label>
                </td>
                <td style="width: 65%">
                    <input type="text" id="InputChangeQty" style="width: 218px" />
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%">
            <tr>
                <td style="width: 10%" />
                <td style="width: 25%">
                    <asp:Label ID="LabelModel2" runat="server"></asp:Label>
                </td>
                <td style="width: 65%">
                    <label id="InputModel2">
                    </label>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td style="width: 60%">
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="12" />
                </td>
                <td style="width: 20%">
                    <input id="BtnAccept" type="button" value="Change" onclick="Change()" style="width: 80px;
                        text-align: center; cursor: pointer;" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        var sessionKey = "";
        var editor;
        var customer;
        var hostname = "";
        var langPre = "eng_";
        var station = "";
        var line = "";
        var dataEntryObj;

        var model1 = "";
        var model2 = "";
        var StationDescQty;
        var AlertInputModel1 = "";
        var AlertInputModel2 = "";
        var AlertSelectStation = "";
        var AlertInputChangeQty = "";
        var AlertWrongChangeQty = "";
        var AlertWrongCode = "";
        var AlertModelSame = "";
        var AlertExcel = "";
        var ChangeModelSuccess = "";
        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';
            langPre = '<%=Pre %>';

            AlertInputModel1 = '<%=AlertInputModel1 %>';
            AlertInputModel2 = '<%=AlertInputModel2 %>';
            AlertSelectStation = '<%=AlertSelectStation %>';
            AlertInputChangeQty = '<%=AlertInputChangeQty %>';
            AlertWrongChangeQty = '<%=AlertWrongChangeQty %>';
            AlertModelSame = '<%=AlertModelSame %>';
            AlertWrongCode = '<%=AlertWrongCode %>';
            AlertExcel = '<%=AlertExcel %>';
            ChangeModelSuccess = '<%=ChangeModelSuccess %>';
            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                dataEntryObj.focus();
            } catch (e) { }
        };

        function InputDataEntry(InputData) {
            var InputDataLength = InputData.length;
            switch (InputDataLength) {
                case 12:
                    if (document.getElementById("InputModel1").innerText == "") {
                        model1 = InputData;
                        document.getElementById("InputModel1").innerText = InputData;
                        ChangeModelWebService.InputModel1(InputData, editor, line, station, customer, '<%=Request["TypeChangeModel"]%>', InputModel1Succeed);

                    } else if (document.getElementById("InputModel2").innerText == "") {
                        if (InputData == model1) {
                            alert(AlertModelSame);
                            dataEntryObj.focus();
                            getAvailableData("InputDataEntry");
                            break;
                        }
                        model2 = InputData;
                        document.getElementById("InputModel2").innerText = InputData;
                        ChangeModelWebService.InputModel2(model1, InputData, InputModel2Succeed);
                    } else {
                        alert(AlertWrongCode);
                        dataEntryObj.focus();
                        getAvailableData("InputDataEntry");
                    }
                    break;
                default:
                    alert(AlertWrongCode);
                    dataEntryObj.focus();
                    getAvailableData("InputDataEntry");
                    break;
            }
        }

        function InputModel1Succeed(result) {
            if (result != null) {
                if (result.length == 2) {
                    var SelectStationObject = document.getElementById("SelectStation");
                    SelectStationObject.options.length = 0;
                    StationDescQty = result[1];
                    var StationLength = StationDescQty.length;
                    SelectStationObject.options[0] = new Option("", "");
                    for (var i = 0; i < StationLength; i++) {
                        SelectStationObject.options[i + 1] = new Option(StationDescQty[i].Descr, StationDescQty[i].Station);
                    }
                    getAvailableData("InputDataEntry");
                } else if (result.length == 1) {
                    model1 = "";
                    document.getElementById("InputModel1").innerText = "";
                    ShowError(result[0]);
                }
            }
        }

        function InputModel2Succeed(result) {
            if (result != null) {
                if (result.length == 1 && result[0] == "SUCCESSRET") {

                    getAvailableData("InputDataEntry");
                } else if (result.length == 1) {
                    ResetUI();
                    ShowError(result[0]);
                }
            }
        }

        function DisplayQty() {
            var SelectStation = document.getElementById("SelectStation").value.trim();
            if (SelectStation != "") {
                for (var i = 0; i < StationDescQty.length; i++) {
                    if (StationDescQty[i].Station.trim() == SelectStation) {
                        document.getElementById("InputQty").innerText = StationDescQty[i].Qty;
                        break;
                    }
                }
            }
        }

        function Change() {
            var model1 = document.getElementById("InputModel1").innerText.trim();
            if (model1 == "") {
                alert(AlertInputModel1);
                dataEntryObj.focus();
                return;
            }

            var model2 = document.getElementById("InputModel2").innerText.trim();
            if (model2 == "") {
                alert(AlertInputModel2);
                dataEntryObj.focus();
                return;
            }

            var SelectStation = document.getElementById("SelectStation").value.trim();
            if (SelectStation == "") {
                alert(AlertSelectStation);
                return;
            }

            var Qty = document.getElementById("InputQty").innerText.trim();
            var ChangeQty = document.getElementById("InputChangeQty").value.trim();

            if (ChangeQty == "") {
                alert(AlertInputChangeQty);
                document.getElementById("InputChangeQty").focus();
                return;
            }

            var patrn = /^[1-9][0-9]*$/;
            if (!patrn.exec(ChangeQty)) {
                alert(AlertWrongChangeQty);
                document.getElementById("InputChangeQty").focus();
                return;
            }

            if (isNaN(ChangeQty)) {
                alert(AlertWrongChangeQty);
                document.getElementById("InputChangeQty").focus();
                return;
            }

            ChangeQty = parseInt(ChangeQty, 10);


            if (ChangeQty > Qty || ChangeQty < 1) {
                alert(AlertWrongChangeQty);
                document.getElementById("InputChangeQty").focus();
                return;
            }


            ChangeModelWebService.Change(model1, SelectStation, ChangeQty, ChangeSucceed);
        }

        function ChangeSucceed(result) {
            
            
            if (result != null) {
                if (result.length == 2) {
                    //var SuccessProductIDS = result[1].replace(/-/g, ",");
                    //ShowSuccessfulInfo(true, "[" + SuccessProductIDS + "] " + ChangeModelSuccess);
                    ShowSuccessfulInfo(true, ChangeModelSuccess);
                    ImportToExcel(result[1]);
                } else if (result.length == 1) {
                    ShowError(result[0]);
                }
            }
            ResetUI();
            dataEntryObj.focus();
        }

        var idTmr = "";

        function ImportToExcel(ProductIDCustSNTable) {
            var model1 = document.getElementById("InputModel1").innerText;
            var model2 = document.getElementById("InputModel2").innerText;
            var station= document.getElementById("SelectStation").value;
            try {
                //Start Excel and get Application object. 
                var oXL;
                try {
                    oXL = new ActiveXObject("Excel.Application");
                }
                catch (e) {
                    alert(AlertExcel);
                    return false;
                }
                //Get a new workbook.
                var oWB = oXL.Workbooks.Add();
                var oSheet = oWB.ActiveSheet;
                //设置标题
                //oWB.Name = model1 + "ChangeTo" + model2 + "By" + editor;
                oXL.Caption = "ChangeModel";
                oSheet.Name = "Product List";
                //设置表头
                oSheet.Rows(1).Font.Bold = true;

                oSheet.Cells(1, 1).Value = "Model";
                oSheet.Cells(1, 2).Value = "ProductID";
                oSheet.Cells(1, 3).Value = "CUSTSN";
                oSheet.Cells(1, 4).Value = "Station";
                oSheet.Cells(1, 5).Value = "ChangedModel";
                oSheet.Cells(1, 6).Value = "Editor";

                //获取数据并填充数据到EXCEL
                var ProductNum = ProductIDCustSNTable.length;
                for (var i = 0; i < ProductNum; i++) {
                    oSheet.Cells(i + 2, 1).Value = model1;
                    oSheet.Cells(i + 2, 2).Value = ProductIDCustSNTable[i].ProductID;
                    oSheet.Cells(i + 2, 3).Value = ProductIDCustSNTable[i].CustSN;
                    oSheet.Cells(i + 2, 4).Value = station;
                    oSheet.Cells(i + 2, 5).Value = model2;
                    oSheet.Cells(i + 2, 6).Value = editor;
                   
                }
                //设置自动列宽 
                oSheet.Columns.AutoFit();
                //设置excel为可见 
                //oXL.Visible = true;
                //将Excel交由用户控制 
                //oXL.UserControl = true;
                //禁止提示
                oXL.DisplayAlerts = false;
                oXL.Save();

//                var fileDialog = oXL.FileDialog(2); // 1 打开，2 保存
//                fileDialog.show();
//                var savePath = dialog.SelectedItems(1);
//                // alert(savePath);
//                var ss = oWB.SaveAs(savePath);

                //释放资源
                oSheet = null;
                oWB = null;
                oXL.Quit();
                oXL = null;
                idTmr = window.setInterval("Cleanup();", 50);

            }
            catch (e) {
            }
        }
        
        function Cleanup() {
            window.clearInterval(idTmr);
            CollectGarbage();
        }

        function ResetUI() {
            model1 = "";
            document.getElementById("InputModel1").innerText = "";
            model2 = "";
            document.getElementById("InputModel2").innerText = "";
            document.getElementById("SelectStation").options.length = 0;
            document.getElementById("InputQty").innerText = "";
            document.getElementById("InputChangeQty").value = "";
        }
        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
            getAvailableData("InputDataEntry");
            dataEntryObj.focus();
        }

        function Cancel() {
            if (model1 != "") {
                ChangeModelWebService.Cancel(model1);
                key = "";
            }
        }

        window.onbeforeunload = function() {
            Cancel();
        };
    </script>

</asp:Content>
