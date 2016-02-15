<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UniteMBMaintain.aspx.cs"
    MasterPageFile="~/MasterPageMaintain.master" Inherits="DataMaintain_UniteMBMaintain" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="content1" ContentPlaceHolderID="iMESContent" runat="server">
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js">
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 37%">
                        &nbsp;
                    </td>
                    <td width="35%" align="right">
                        <button runat="server" id="btnDelete" class="iMes_button" onserverclick="btnDelete_ServerClick"
                            onclick="if(clickDelete())">
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="hidRecordCount" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div id="div2" style="height: 366px">
                    <imes:gridviewext id="gd" runat="server" autogeneratecolumns="true" width="150%"
                        rowstyle-height="20" gvextwidth="100%" gvextheight="100%" autohighlightscrollbyvalue="true"
                        highlightrowposition="3" height="366px" ongvextrowclick='if(typeof(clickTable)=="function") clickTable(this)'
                        enableviewstate="false" onrowdatabound="gd_RowDataBound">
                    </imes:gridviewext>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblMB" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 14%">
                        <asp:TextBox ID="dMB" runat="server" MaxLength="3" Width="90%" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                    </td>
                    <td style="width: 10%;" align="center">
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 25%">
                        <asp:TextBox ID="dQty" runat="server" MaxLength="5" Width="83%" SkinId="textBoxSkin" 
                            onkeypress="event.returnValue=isDigit()" Style="ime-mode: disabled;" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"></asp:TextBox>
                    </td>
                    <td style="width: 90px">
                        <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" Width="95%">
                            <asp:ListItem Selected="True">First Test</asp:ListItem>
                            <asp:ListItem>First Split</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%;" align="center">
                        <button id="btnAdd" runat="server" class="iMes_button" onclick="if(clickAdd())" onserverclick="btnAdd_serverClick" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="dRemark" runat="server" MaxLength="50" Width="98%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 10%" align="center">
                        <button id="btnSave" runat="server" class="iMes_button" onclick="if(clickSave())"
                            onserverclick="btnSave_serverClick" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input id="HiddenUsername" type="hidden" name="HiddenUsername" runat="server" />
        <input type="hidden" id="hidMBcode" runat="server" value="" />
        <input type="hidden" id="dTableHeight" runat="server" />
    </div>
    <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">

        window.onload = function() {
            msg1 = "<%=pmtMessage1 %>";
            msg2 = "<%=pmtMessage2 %>";
            msg3 = "<%=pmtMessage3 %>";
            msg4 = "<%=pmtMessage4 %>";
            msg5 = "<%=pmtMessage5 %>";
            msg6 = "<%=pmtMessage6 %>";
            ShowRowEditInfo(null);
            resetTableHeight();

        }
        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;
            //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
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
        function clickDelete() {

            if (iSelectedRowIndex == null) {
                alert(msg1);
                return false;
            }
            var ret = confirm(msg2);
            if (!ret) {

                var gdObj = document.getElementById("<%=gd.ClientID %>");
                var con = gdObj.rows[iSelectedRowIndex + 1];
                setGdHighLight(con);
                return false;
            }
            ShowWait();
            return true;

        }

        function DeleteComplete() {
            DealHideWait();
            ShowRowEditInfo(null);
        }

        function NoMatchFamily() {
            alert(msg2);
            return;
        }

        function clickSave() {
            ShowWait();
            if (isNull() == false) {
                return false;
            }

            return true;
        }

        function clickAdd() {
            ShowWait();
            if (isNull() == false) {
                return false;
            }
            return true;
        }
        function isNull() {
            var mbValue = document.getElementById("<%=dMB.ClientID %>").value;
            if (mbValue.trim() == "") {
                DealHideWait();
                alert(msg3);
                return false;
            }
            var qty = document.getElementById("<%=dQty.ClientID %>").value;
            if (qty.trim() == "") {
                DealHideWait();
                alert(msg4);
                return false;
            }
            if (qty.trim() < 0 || parseInt(qty.trim()) > 32767) {
                DealHideWait();
                document.getElementById("<%=dQty.ClientID %>").focus();
                alert(msg6);
                return false;
            }
            return true;
        }
        function AddUpdateComplete(id) {
            DealHideWait();
            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                ShowRowEditInfo(null);
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }

        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            document.getElementById("<%=dMB.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=dQty.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=dRemark.ClientID %>").value = con.cells[1].innerText.trim();
            if (con.cells[3].innerText.trim() == "First Test") {
                document.getElementById("<%=drpType.ClientID %>").selectedIndex = 0;
            }
            else if (con.cells[3].innerText.trim() == "First Split") {
                document.getElementById("<%=drpType.ClientID %>").selectedIndex = 1;
            }
            document.getElementById("<%=hidMBcode.ClientID %>").value = con.cells[0].innerText.trim();
            var currentId = con.cells[0].innerText.trim();
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            }
        }

        function setNewItemValue() {
            var input = document.getElementsByTagName("input");
            for (var i = 0; i < input.length; i++) {
                if (input[i].type == "text") {
                    input[i].value = "";
                }
            }
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();

        }
        function isDigit() {
            return ((event.keyCode >= 48) && (event.keyCode <= 57));
        }

    </script>

</asp:Content>
