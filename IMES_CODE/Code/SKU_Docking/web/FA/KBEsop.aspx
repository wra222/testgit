<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:KBEsop page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-09-11 Kerwin                Create      
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="KBEsop.aspx.cs" Inherits="FA_KBEsop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/CommonFunction/Service/ESOPWebService.asmx" />
            <asp:ServiceReference Path="~/FA/Service/KBEsopWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <table width="100%">
            <colgroup>
                <col style="width: 5%;" />
                <col style="width: 15%;" />
                <col style="width: 80%;" />
            </colgroup>
            <tr style="height: 60px;">
                <td>
                </td>
                <td>
                    <label id="LabelProdId">
                        ProdId:</label>
                </td>
                <td>
                    <label id="InputProdId">
                    </label>
                </td>
            </tr>
            <tr style="height: 60px;">
                <td>
                </td>
                <td>
                    <label id="LabelCustSN">
                        CustSN:</label>
                </td>
                <td>
                    <label id="InputCustSN">
                    </label>
                </td>
            </tr>
            <tr style="height: 60px;">
                <td>
                </td>
                <td>
                    <label id="LabelModel">
                        Model:</label>
                </td>
                <td>
                    <label id="InputModel">
                    </label>
                </td>
            </tr>
            <tr style="height: 60px;">
                <td>
                </td>
                <td>
                    <asp:Label ID="LabelDataEntry" runat="server" class="iMes_DataEntryLabel">Data Entry:</asp:Label>
                </td>
                <td>
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="10" />
                 <%--       <input id="Hidden1" type="button" style=" width:100px" value="Show SOP"  onclick="ShowSOPWindow()" />--%>
                </td>
            </tr>
        </table>
<%--        <input id="BtnShowSOP" type="hidden"  onclick="ShowSOPWindow()" />--%>
    </div>

    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var hostName = getClientHostName();
        var langPre = "eng_";
        var station = "KBPJ";
        var line = "";
        var key = "";
        var keyType = "";
        var PopSOPWindow;
        var AlertWrongFormat = "Wrong Code!";
        var AlertSuccess = "Success!";

        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';
            station = '<%=Request["Station"] %>';
            if (!station) {
                station = "KBPJ";
            }
            langPre = '<%=Pre %>';
            AlertSuccess = '<%=AlertSuccess %>';
            AlertWrongFormat = '<%=AlertWrongFormat %>';
            ShowSOPWindow();
            getAvailableData("InputDataEntry");
        };

        function InputDataEntry(InputData) {
            var InputDataLength = InputData.length;
            switch (InputDataLength) {
                case 9:
                    keyType = "Product";
                    key = InputData;
                    KBEsopWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    break;
                case 10:
                    //if (InputData.substr(0, 3) == "CNU") {
					if (CheckCustomerSN(inputData)) {
                        keyType = "CustSN";
                        key = InputData;
                        KBEsopWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    } else {
                        keyType = "Product";
                        key = InputData.substr(0, 9);
                        KBEsopWebService.InputKey(key, keyType, editor, line, station, customer, InputKeySucceed);
                    }
                    break;
                default:
                    alert(AlertWrongFormat);
                    getAvailableData("InputDataEntry");
                    break;
            }
        }

        function InputKeySucceed(result) {
            if (result != null) {
                if (result.length == 3) {
                    document.getElementById("InputModel").innerText = result[1].Model;
                    document.getElementById("InputCustSN").innerText = result[1].CustSN;
                    document.getElementById("InputProdId").innerText = result[1].ProductID;
                    ESOPWebService.PutMessage({ Title: 'Message From KBEsop', Content: result[2] }, [{ hostName: hostName, station: station}]);

                    getAvailableData("InputDataEntry");
                    ShowSuccessfulInfo(true);
                } else if (result.length == 1) {
                    ESOPWebService.PutMessage({ Title: 'Message From KBEsop', Content: null }, [{ hostName: hostName, station: station}]);
                    ResetUI();
                    ShowError(result[0]);
                }
            }
        }

        function ResetUI() {
            document.getElementById("InputModel").innerText = "";
            document.getElementById("InputCustSN").innerText = "";
            document.getElementById("InputProdId").innerText = "";
            keyType = "";
            key = "";
        }

        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
            getAvailableData("InputDataEntry");
        }

        function ShowSOPWindow() {
            var XWidth = window.screen.width;
            var YHeight = window.screen.height;

            var imgWidth = 1506;
            var imgHeight = 1131;


            if (imgWidth / imgHeight >= XWidth / YHeight) {
                YHeight = (XWidth / imgWidth) * imgHeight;
            }
            else {
                XWidth = (YHeight / imgHeight) * imgWidth;
            }
            if (PopSOPWindow) {
                PopSOPWindow.close();
            }
            
            var currentTimeStr = new Date().toString() + new Date().getMilliseconds().toString();
            PopSOPWindow = window.self.showModelessDialog("../CommonFunction/ShowKBEsop.aspx?HostName=" + hostName + "&Station=" + station + "@OpenTime=" + currentTimeStr, null, 'dialogTop:0px;dialogLeft:0px;dialogWidth:' + XWidth + 'px;dialogHeight:' + YHeight + 'px;status:no;help:no;menubar:no;toolbar:no;resize:yes;scroll:no;center:no;');

        }
        window.onunload = function() {
        if (PopSOPWindow) {           
                PopSOPWindow.close();
            }
        }
    </script>

</asp:Content>
