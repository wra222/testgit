<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestMBMaintain.aspx.cs" MasterPageFile="~/MasterPageMaintain.master"
    Inherits="DataMaintain_TestMBMaintain" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="content1" ContentPlaceHolderID="iMESContent" runat="server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js">
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <table width="100%" class="iMes_div_MainTainEdit">
            <tr>
                <td width="200px">
                    <asp:Label ID="lblFamilyLst" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="82%">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" RenderMode="Inline" runat="server">
                        <ContentTemplate>
                            <iMESMaintain:CmbFamilyForMaintain2 ID="CmbFamilyList" runat="server" CssClass="iMes_textbox_input_Yellow"
                                Width="26%" FirstNullItem="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
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
                <div id="div2" style="height: 336px">
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="150%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="100%" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" Height="366px" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        EnableViewState="false" OnRowDataBound="gd_RowDataBound">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <iMESMaintain:CmbFamilyForMaintain2 ID="CmbFamily" runat="server" Height="28px" Width="95%"
                            FirstNullItem="false" />
                    </td>
                    <td style="width: 10%;" align="center">
                        <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <iMESMaintain:CmbMaintainMBTestCode ID="drpCode" runat="server" ssClass="iMes_textbox_input_Yellow"
                                     Width="95%" FirstNullItem="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:15%">
                        <asp:DropDownList ID="drpType" runat="server" Width="99%">
                            <asp:ListItem Selected="True" Value="Normal">FUNCATION</asp:ListItem>
                            <asp:ListItem Value="Image">ICT</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%;" align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="dRemark" runat="server" MaxLength="50" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 10%" align="center">
                        <button id="btnAdd" runat="server" class="iMes_button" onclick="if(clickAdd())" onserverclick="btnAdd_serverClick" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="familyChange" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="familyListChange" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input id="HiddenUsername" type="hidden" name="HiddenUsername" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidFamily" runat="server" />
        <input type="hidden" id="hidCode" runat="server" />
        <input type="hidden" id="hidType" runat="server" />
        <button id="familyChange" style="display: none" runat="server" onserverclick="FamilyChange_ServerClick">
        </button>
        <button id="familyListChange" runat="server" style="display: none" onserverclick="FamilyListChange_ServerClick">
        </button>
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
            ShowRowEditInfo(null);
            initControls();
        }
        function initControls() {
            document.getElementById("<%=CmbFamilyList.ClientID %>" + "_DropDownList1").onchange = familyChangeList;
            document.getElementById("<%=CmbFamily.ClientID %>" + "_DropDownList1").onchange = familyChange;
        }
        function familyChangeList() {
            ShowWait();
            document.getElementById("<%=familyListChange.ClientID %>").click();
        }
        function familyChange() {
            document.getElementById("<%=familyChange.ClientID %>").click();
        }

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 80;
            var marginValue = 10;
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
            setNewItemValue();
            ShowRowEditInfo(null);
        }
        function clickAdd() {

            if (isNull() == false) {
                return false;
            }
            return true;
            ShowWait();
        }
        function isNull() {
            var family = document.getElementById("<%=CmbFamily.ClientID%>" + "_DropDownList1");
            if (family.selectedValue == "") {
                alert(msg4);
                return false;
            }
            var code = getObjMBTestCode();
            if (code.value == "") {
                alert(msg3);
                return false;
            }

            var type = document.getElementById("<%=drpType.ClientID %>");
            if (type.selectedValue == "") {
                alert(msg5);
                return false;
            }
            return true;
        }
        function AddUpdateComplete(id) {
            DealHideWait();
            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[7].innerText == id) {
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
            ShowRowEditInfo(con);
            setGdHighLight(con);


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
            document.getElementById("<%=dRemark.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=drpType.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=CmbFamilyList.ClientID%>" + "_DropDownList1").value = con.cells[2].innerText.trim();
            document.getElementById("<%=CmbFamily.ClientID%>" + "_DropDownList1").value = con.cells[2].innerText.trim();
            document.getElementById("<%=hidCode.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidType.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=hidFamily.ClientID %>").value = con.cells[2].innerText.trim();
            var currentId = con.cells[7].innerText.trim();
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }
            familyChange();
        }

        function setNewItemValue() {
            document.getElementById("<%=CmbFamily.ClientID%>" + "_DropDownList1").selectedIndex = 0;
            document.getElementById("<%=familyChange.ClientID %>").click();
            document.getElementById("<%=dRemark.ClientID %>").value = "";
            document.getElementById("<%=drpType.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();
        }
        function isDigit() {
            return ((event.keyCode >= 48) && (event.keyCode <= 57));
        }

    </script>

</asp:Content>
