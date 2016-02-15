<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfflineLabelSetting.aspx.cs"
    Inherits="DataMaintain_OfflineLabelSetting" MasterPageFile="~/MasterPageMaintain.master"
    ContentType="text/html;Charset=UTF-8" %>

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
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="200%"
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
                    <td style="width: 90px;">
                        <asp:Label ID="lblFileName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="dFileName" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="dDescr" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblLabelSpec" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="dLabelSpec" runat="server" MaxLength="50" Width="96%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 90px;">
                        <asp:Label ID="lblPrintModel" runat="server" CssClass="iMes_label_13pt" Text="PrintModel:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="cmbPrintModel" runat="server">
                            <asp:ListItem Text="Bat方式" Value="0"></asp:ListItem>
                            <asp:ListItem Text="模板方式" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Bartender方式" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Bartender Server方式" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblSPName" runat="server" CssClass="iMes_label_13pt" Text="SPName:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSPName" runat="server" MaxLength="32" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px"></td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td style="width: 90px;">
                        <asp:Label ID="lblParam1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam1" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblParam2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam2" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblParam3" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam3" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblParam4" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam4" runat="server" MaxLength="50" Width="95%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px;" align="right">
                        <button id="btnAdd" runat="server" class="iMes_button" onclick="if(clickAdd())" onserverclick="btnAdd_serverClick" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 90px;">
                        <asp:Label ID="lblParam5" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam5" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblParam6" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam6" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblParam7" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam7" runat="server" MaxLength="50" Width="100%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px; padding-left: 8px">
                        <asp:Label ID="lblParam8" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="dParam8" runat="server" MaxLength="50" Width="95%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px" align="right">
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
        <input type="hidden" id="hidFileName" runat="server" value="" />
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
            setNewItemValue();
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
            alert(msg6);
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
            var fileName = document.getElementById("<%=dFileName.ClientID %>").value;
            if (fileName.trim() == "") {
                DealHideWait();
                alert(msg3);
                return false;

            }
            var descri = document.getElementById("<%=dDescr.ClientID %>").value;
            if (descri.trim() == "") {
                DealHideWait();
                alert(msg4);
                return false;

            }
            var labelSpec = document.getElementById("<%=dLabelSpec.ClientID %>").value;
            if (labelSpec.trim() == "") {
                DealHideWait();
                alert(msg5);
                return false;

            }

            var printmodel = document.getElementById("<%=cmbPrintModel.ClientID %>").value;
            if (printmodel == 3 || printmodel == 4) {
                var spName = document.getElementById("<%=txtSPName.ClientID %>").value;
                if (spName == "") {
                    DealHideWait();
                    alert("Please Input SPName");
                    return false;
                }
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
            document.getElementById("<%=dFileName.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=dDescr.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=dLabelSpec.ClientID %>").value = con.cells[2].innerText.trim();

            var printModel = con.cells[3].innerText.trim();
            if (printModel == "Bat方式") {
                document.getElementById("<%=cmbPrintModel.ClientID %>").value = "0";
            }
            else if (printModel == "模板方式") {
                document.getElementById("<%=cmbPrintModel.ClientID %>").value = "1";
            }
            else if (printModel == "Bartender方式") {
                document.getElementById("<%=cmbPrintModel.ClientID %>").value = "3";
            }
            else if (printModel == "Bartender Server方式") {
                document.getElementById("<%=cmbPrintModel.ClientID %>").value = "4";
            }
            
            document.getElementById("<%=txtSPName.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=dParam1.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=dParam2.ClientID %>").value = con.cells[6].innerText.trim();
            document.getElementById("<%=dParam3.ClientID %>").value = con.cells[7].innerText.trim();
            document.getElementById("<%=dParam4.ClientID %>").value = con.cells[8].innerText.trim();
            document.getElementById("<%=dParam5.ClientID %>").value = con.cells[9].innerText.trim();
            document.getElementById("<%=dParam6.ClientID %>").value = con.cells[10].innerText.trim();
            document.getElementById("<%=dParam7.ClientID %>").value = con.cells[11].innerText.trim();
            document.getElementById("<%=dParam8.ClientID %>").value = con.cells[12].innerText.trim();
            document.getElementById("<%=hidFileName.ClientID %>").value = con.cells[0].innerText.trim();
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
        
    </script>

</asp:Content>
