<%--
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: DefactMaitain
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ================ =====================================
 * 2010-06-02   Yu Qian              Create     
 * 2011-12-12    ShhWang             Modified               
 * Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="FAITCNDefectCheck.aspx.cs"
    Inherits="DataMaintain_FAITCNDefectCheck" ValidateRequest="false" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1" style="margin-top: 15;">
            <table width="100%" class="iMes_div_MainTainDiv1">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblDefectCheckList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="dSearch" runat="server" onkeypress="OnKeyPress(this)" SkinID="textBoxSkin"></asp:TextBox>
                    </td>
                    <td align="right">
                        <input type="button" id="btnDelete" runat="server" onclick="if(clkDelete())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnDelete_ServerClick" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="height: 395px">
            <input id="hidRecordCount" type="hidden" runat="server" />
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="392px" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        EnableViewState="false" OnRowDataBound="gd_RowDataBound" Style="top: 0px; left: 23px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 150px;" />
                </colgroup>
                <tr>
                    <td >
                        <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt" meta:resourcekey="lblFamilyResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="50" Width="95%" SkinID="textBoxSkin"
                            onkeypress='checkDefectCodePress(this)' Style='ime-mode: disabled;' onpaste="return false"
                            meta:resourcekey="txtTPResource1"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="CheckMAC" runat="server" CssClass="iMes_CheckBox" />
                        <asp:CheckBox ID="CheckMBCT" runat="server" CssClass="iMes_CheckBox" />
                    </td>
                    <td align="center">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick" />
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblHDDV" runat="server" CssClass="iMes_label_13pt" meta:resourcekey="lblTPResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtHDDV" runat="server" MaxLength="50" Width="95%" SkinID="textBoxSkin"
                            meta:resourcekey="txtTPResource1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblBIOS" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBIOS" runat="server" Width="95%" SkinID="textBoxSkin" MaxLength="50"
                            Style='ime-mode: disabled;' onpaste="if(/^[\u4E00-\u9FA5]+$/g.test(window.clipboardData.getData('text'))) event.returnValue=false"></asp:TextBox>
                    </td>
                    <td align="center">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnSave_ServerClick" />
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="selFamily" runat="server" />
        <input type="hidden" id="selPartNo" runat="server" />
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidDesc" runat="server" />
        <input type="hidden" id="HiddenSelectedId" runat="server" />
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

    <script language="javascript" type="text/javascript">
        var msg1 = "";
        var msg2 = "";
        var msg3 = "";
        var msg4 = "";
        var msg5 = "";
        var msg6 = "";
        var msg7 = "";
        var msg8 = "";
        window.onload = function() {
            msg1 = "<%=pmtMessage1 %>";
            msg2 = "<%=pmtMessage2 %>";
            msg3 = "<%=pmtMessage3 %>";
            msg4 = "<%=pmtMessage4 %>";
            msg5 = "<%=pmtMessage5 %>";
            msg6 = "<%=pmtMessage6 %>";
            msg7 = "<%=pmtMessage7 %>";
            msg8 = "<%=pmtMessage8 %>";
            resetTableHeight();
            ShowRowEditInfo(null);
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

        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "<%=dSearch.ClientID %>") {
                    var value = document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findDefect(value, true);
                    }
                }
            }

        }
        function findDefect(searchValue, isNeedPromptAlert) {
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
                    alert(msg4);
                    //alert("找不到列表中与btoceanorder栏位匹配的项");     
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

        function getCheckStr(flag) {

            if (flag == true) {
                return "Y";
            }
            else {
                return "N";
            }
        }

        function getCheckFlag(str) {

            if (str == "Y") {
                return true;
            }
            else {
                return false;
            }
        }
        
        function ShowRowEditInfo(con) {

            if (con == null) {
                setNewItemValue();
                return;
            }
            document.getElementById("<%=HiddenSelectedId.ClientID %>").value = con.cells[5].innerText.trim();

            document.getElementById("<%=txtCode.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=txtHDDV.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=txtBIOS.ClientID %>").value = con.cells[4].innerText.trim();

            document.getElementById("<%=CheckMAC.ClientID%>").checked = getCheckFlag(con.cells[1].innerText.trim());
            document.getElementById("<%=CheckMBCT.ClientID%>").checked = getCheckFlag(con.cells[2].innerText.trim());

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
            document.getElementById("<%=dSearch.ClientID %>").value = "";
            document.getElementById("<%=txtCode.ClientID %>").value = "";
            document.getElementById("<%=txtBIOS.ClientID %>").value = "";
            document.getElementById("<%=txtHDDV.ClientID %>").value = "";
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        var iSelectedRowIndex = null;
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

        function DealHideWait() {
            HideWait();
        }

        function clkAdd() {
            return checkAdaptorInfo();
        }

        function clkSave() {
            return checkAdaptorInfo();
        }

        function checkAdaptorInfo() {

            var code = document.getElementById("<%=txtCode.ClientID %>").value;
            var hddv = document.getElementById("<%=txtHDDV.ClientID %>").value;
            var bios = document.getElementById("<%=txtBIOS.ClientID %>").value;
            if (code.trim() == "") {
                alert(msg2);
                document.getElementById("<%=txtCode.ClientID %>").focus();
                return false;
            }
            else if (hddv.trim() == "") {
                alert(msg7);
                desc.focus();
                return false;
            }
            else if (bios.trim() == "") {
                alert(msg8);
                engDesc.focus();
                return false;
            }
            ShowWait();
            return true;
        }

        function clkDelete() {
            ShowInfo("");
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                return false;
            }


            var ret = confirm(msg3);
            if (!ret) {
                return false;
            }

            ShowWait();
            return true;
        }

        function AddUpdateComplete(id) {
            DealHideWait();
            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[5].innerText == id) {
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
            document.getElementById("<%=dSearch.ClientID %>").value = "";
        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
        }

        function checkDefectCodePress(obj) {

            var key = event.keyCode;
            if (!((key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122))) {
                event.keyCode = 0;
            }
        }

        function checkInputChinese(con) {
            alert(event.keyCode);
            alert(con);
            var reg = /[\u4E00-\u9FA5]/g;
            if (reg.test(con)) { alert("含有汉字"); }
            else { alert("不含有汉字"); }
        }
        function HasRecordError() {
            alert(msg6);
        }

        function trySetFocusCode() {
            var descObj = document.getElementById("<%=txtCode.ClientID %>");

            if (descObj != null && descObj != undefined) {
                descObj.focus();
            }
        }
    
    
    </script>

</asp:Content>
