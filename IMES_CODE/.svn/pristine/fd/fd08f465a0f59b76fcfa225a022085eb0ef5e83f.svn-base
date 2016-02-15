<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
2011         ShhWang              Created
 
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="RunInTimeControl.aspx.cs" Inherits="DataMaintain_RunInTimeControl"
    ValidateRequest="false" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" border="0">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblLst" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="right">
                        <button id="btnDelete" runat="server" class="iMes_button" onclick="if(clkDelete())"
                            onmouseout="this.className='iMes_button_onmouseout'" onmouseover="this.className='iMes_button_onmouseover'"
                            onserverclick="btnDelete_ServerClick">
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="height: 390px">
            <input id="hidRecordCount" type="hidden" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                Visible="true">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="150%"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue="true" HighLightRowPosition="3"
                        RowStyle-Height="20" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false" Style="top: 0px; left: 23px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 35%">
                        <iMESMaintain:CmbFamilyForMaintain ID="CmbFamily" runat="server" Height="28px" Width="95%" FirstNullItem="false" />
                    </td>
                    <td style="width: 15%">
                        <asp:Label ID="lblControlType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <asp:DropDownList ID="drpControlType" runat="server" Width="95%">
                            <asp:ListItem Selected="True" Value="Longest">Longest</asp:ListItem>
                            <asp:ListItem Value="Shortest">Shortest</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="width: 10%">
                        <button id="btnAdd" runat="server" class="iMes_button" onclick="if(clkAdd())" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick">
                        </button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMESMaintain:CmbMaintainStationByStationType ID="CmbStation" runat="server" Height="28px" Width="95%" FirstNullItem="false" />
                    </td>
                    <td>
                        <asp:Label ID="lblTime" runat="server" Width="100%" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTime" runat="server" MaxLength="4" Width="93%" TabIndex="3" SkinID="textBoxSkin"
                            Style="ime-mode: disabled;" onKeyPress="if((event.keyCode<48 || event.keyCode>57) && event.keyCode!=46 || /^[1-9]*[0-9]{1}\.[0-9]{2}$/.test(value))event.returnValue=false"
                            onbeforepaste="checkFloat()" > </asp:TextBox>
                    </td>
                    <td align="right" style="width: 10%">
                        <button id="btnSave" runat="server" class="iMes_button" onclick="if(clkSave())" onmouseout="this.className='iMes_button_onmouseout'"
                            onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btnSave_ServerClick">
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" name="HiddenUsername" runat="server" />
        <input type="hidden" id="oldCode" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidStation" runat="server" />
        <input type="hidden" id="hidId" runat="server" />
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

    <script language="javascript" type="text/javascript">
        window.onload = function() {
            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg7 = "<%=pmtMessage7 %>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            ShowRowEditInfo(null);
            resetTableHeight();
        };
        var iSelectedRowIndex = null;
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
        function clkAdd() {
            return check();
        }
        function clkSave() {
            return check();
        }
        function check() {
            var cmbFamily = getFamilyCmbObj();
            if (cmbFamily.value == "") {
                alert(msg3);
                return false;
            }
            var txtTime = document.getElementById("<%=txtTime.ClientID %>");
            if (txtTime.value == "") {
                alert(msg1);
                return false;
            }
            var numFloat = parseFloat(txtTime.value);
            if (numFloat >= 100 || numFloat < 0) {
                alert(msg5);
                return false;
            }
            var controlType = document.getElementById("<%= drpControlType.ClientID %>").value;
            if (controlType == "") {
                alert(msg6);
                return false;
            }
            return true;
        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }
        function clkDelete() {
            var ret = confirm(msg2);
            if (!ret) {
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
        function DeleteComplete() {
            ShowRowEditInfo(null);
            DealHideWait();
            setNewItemValue();
        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }
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
            getFamilyCmbObj().value = con.cells[0].innerText.trim();
            getMaintainStationCmbObj().value = con.cells[1].innerText.trim();
            document.getElementById("<%= drpControlType.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=txtTime.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=hidStation.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=oldCode.ClientID %>").value = con.cells[0].innerText.trim();
            var currentId = con.cells[7].innerText.trim();            
            document.getElementById("<%=hidId.ClientID %>").value=currentId;
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }
        }

        function setNewItemValue() {
            getMaintainStationCmbObj().selectedIndex = 0;
            getFamilyCmbObj().selectedIndex = 0;
            document.getElementById("<%= drpControlType.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=txtTime.ClientID %>").value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        var msg1 = "";
        var msg2 = "";
        var msg3 = "";
        var msg4 = "";
        var msg5 = "";
        var msg6 = "";
        var msg7 = "";
        var msg8 = "";

        var typeFamily = "";
        var typeModel = "";
        var typeCustSN = "";

        function DealHideWait() {
            HideWait();
        }
        function checkFloat() {
            var data = window.clipboardData.getData("text");
            var test = /^[1-9]*[0-9]{1}\.[0-9]{1}$/;
            if (!test.test(data) || data.length > 4) {
                window.clipboardData.setData("text", '');
            }

        }
   
    </script>

</asp:Content>
