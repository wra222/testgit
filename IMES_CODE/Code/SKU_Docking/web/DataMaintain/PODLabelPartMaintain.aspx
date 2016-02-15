<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PODLabelPartMaintain.aspx.cs"
    Inherits="DataMaintain_PODLabel" MasterPageFile="~/MasterPageMaintain.master"
    ValidateRequest="false" ContentType="text/html;Charset=UTF-8" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="content1" ContentPlaceHolderID="iMESContent" runat="server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js">
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" border="0">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 42%">
                        <asp:TextBox ID="dSearch" runat="server" MaxLength="50" Width="56%" onkeypress="OnKeyPress(this)"></asp:TextBox>
                    </td>
                    <td width="40%" align="right">
                        <input type="button" runat="server" id="btnDelete" class="iMes_button" onserverclick="btnDelete_ServerClick"
                            onclick="if(clickDelete())" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="height: 392px">
            <input type="hidden" id="hidRecordCount" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="376px" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" Height="366px" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        EnableViewState="false" OnRowDataBound="gd_RowDataBound">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 90px;">
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="35%">
                        <asp:TextBox ID="dPartNo" runat="server" MaxLength="20" Width="65%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width: 90px;">
                        <asp:Label ID="lblFamilyLabel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="35%">
                        <iMESMaintain:CmbFamilyForMaintain ID="CmbFamily" runat="server" Height="28px" Width="65%" FirstNullItem="false" />
                    </td>
                    <td align="center" width="10%">
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onclick="if(clickSave())"
                            onserverclick="btnSave_serverClick" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input id="HiddenUsername" type="hidden" name="HiddenUsername" runat="server" />
        <input type="hidden" id="hidPartNo" runat="server" value="" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="HidFamily" runat="server" value="" />
        <button id="btnFamilyChange" runat="server" type="button" style="display: none">
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
            msg6 = "<%=pmtMessage6 %>";
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
                alert(msg2);
                return false;
            }
            var ret = confirm(msg3);
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
            ShowRowEditInfo(null);
            DealHideWait();
        }

        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "<%=dSearch.ClientID %>") {
                    var value = document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findPartNo(value, true);
                    }
                }
            }

        }
        function findPartNo(searchValue, isNeedPromptAlert) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            searchValue = searchValue.toUpperCase();
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (searchValue.trim() != "" && gdObj.rows[i].cells[0].innerText.toUpperCase().indexOf(searchValue) == 0) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                if (isNeedPromptAlert == true) {
                    alert(msg1);
                }
                else if (isNeedPromptAlert == null) {
                    ShowRowEditInfo(null);
                }
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }
        }
        function NoMatchFamily() {
            alert(msg2);
            return;
        }

        function clickSave() {
            ShowWait();
            var partNo = document.getElementById("<%=dPartNo.ClientID %>").value;

            if (partNo.trim() == "") {
                DealHideWait();
                alert(msg4);
                document.getElementById("<%=dPartNo.ClientID %>").focus();
                return false;

            }
            return true;
        }
        function AddUpdateComplete(id) {
            DealHideWait();
           
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
            //ITC-1361-0099 itc210012 2012-02-22
                if (gdObj.rows[i].cells[0].innerText.toUpperCase() == id.toUpperCase()) {
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
//                alert(msg6);
            }

        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {

            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            //设置当前高亮行   
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            //记住当前高亮行
            iSelectedRowIndex = parseInt(con.index, 10);

        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            getFamilyCmbObj().value = con.cells[1].innerText.trim();
            document.getElementById("<%=dPartNo.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidPartNo.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=HidFamily.ClientID %>").value = con.cells[1].innerText.trim();
            var currentId = con.cells[0].innerText.trim();
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }

        }

        function setNewItemValue() {
            getFamilyCmbObj().selectedIndex = 0;
            document.getElementById("<%=dPartNo.ClientID %>").value = ""
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();
            getFamilyCmbObj().disabled = false;

        }
        
    </script>

</asp:Content>
