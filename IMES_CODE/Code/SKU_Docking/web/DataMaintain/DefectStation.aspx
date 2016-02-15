<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="DefectStation.aspx.cs" Inherits="DataMaintain_Grade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <style type="text/css">
        .iMes_div_MainTainEdit
        {
            border: thin solid Black;
            background-color: #99CDFF;
            margin: 0 0 20 0;
        }
        .iMes_textbox_input_Yellow
        {
        }
        #btnDel
        {
            width: 14px;
        }
        .style1
        {
            width: 5px;
        }
    </style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td width="10%" class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblPreWCTop" runat="server" CssClass="iMes_label_13pt" Text="Pre WC:"></asp:Label>
                    </td>
                    <td width="15%">
                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbPreWCTop" runat="server" Width="98%" AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                    <td width="10%">
                        <input id="btnQuery" runat="server" class="iMes_button" onclick="if(clkQuery())"
                             type="button" value="Query" onserverclick="btnQuery_ServerClick" />
                    </td>
                    <td width="65%"></td>
                </tr>
            </table>
        </div>
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="40%">
                        <%--<input type="text" ID="dSearch" MaxLength="10" Width="50%" CssClass="iMes_textbox_input_Yellow"
                            onkeypress='OnKeyPress(this)' Visible="false" />--%>
                    </td>
                    <td width="32%" align="right">
                        <input type="button" id="btnDel" runat="server" class="iMes_button" onclick="if(clkDelete())"
                            onserverclick="btnDelete_ServerClick"/>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                    <%--<asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />--%>
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <%--<iMES:GridViewExt SkinID="clearStyle" ID="gd" runat="server" AutoGenerateColumns="true"
                        Width="130%" GvExtHeight="676px" RowStyle-Height="20" GvExtWidth="100%" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false" Style="top: -163px;
                        left: -286px">
                    </iMES:GridViewExt>--%>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%" GvExtWidth="100%" GvExtHeight="390px"  onrowdatabound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" RowStyle-Height="20">
                 
                 </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblPreWC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <iMESMaintain:CmbMaintainWC ID="ddlPreWC" runat="server" Width="100%"></iMESMaintain:CmbMaintainWC>
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblCurWC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <iMESMaintain:CmbMaintainWC ID="ddlCurWC" runat="server" Width="100%"></iMESMaintain:CmbMaintainWC>
                    </td>
                    
                    <td width="10%">
                        <asp:Label ID="lblFamilyAndModel" runat="server" CssClass="iMes_label_13pt" Text="Family/Model:"></asp:Label>
                        
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtFamilyAndModel" runat="server" Width="98%"></asp:TextBox>
                        
                    </td>
                    <td align="right" width="10%">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick" />
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblDefect" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td>
                    <td width="20%">
                        <iMESMaintain:CmbMaintainDefect ID="ddlDefect" runat="server" Width="100%"></iMESMaintain:CmbMaintainDefect>
                        
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblCause" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td>
                    <td width="20%">
                        <iMESMaintain:CmbMaintainCause ID="ddlCause" runat="server" Width="100%"></iMESMaintain:CmbMaintainCause>
                        
                    </td>
                    
                    <td width="10%">
                        <asp:Label ID="lblMajorPart" runat="server" CssClass="iMes_label_13pt" Text="MajorPart:"></asp:Label>
                        
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="cmbMajorPart" runat="server" Width="100%"></asp:DropDownList>
                        
                    </td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnSave_ServerClick" />
                        <input type="hidden" id="dTableHeight" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNextWC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMESMaintain:CmbMaintainWC ID="ddlNextWC" runat="server" Width="100%"></iMESMaintain:CmbMaintainWC>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                
                </tr>
            </table>
        </div>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="HiddenSelectedId" runat="server" />
        <input type="hidden" id="hidPerWCTopSelect" runat="server" />
        <input type="hidden" id="hidPerWCSelect" runat="server" />
        <input type="hidden" id="hidMajorPart" runat="server" />
        <%--<button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none">
        </button>--%>
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td>
                <td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        var customerObj;
        var descriptionObj;
        var emptyPattern = /^\s*$/;
        var emptyString = "";
        
        var msgSelectOne = "";
        var msgDelConfirm = "";
        var msgPreWC = "";
        var msgCurWC = "";
        var msgNextWC = "";
        var msgDefectSel = "";
        var msgCauseSel = "";
        var msgNotFound = "";
        window.onload = function() {
            msgSelectOne = "<%=MsgSelectOne%>";
            msgDelConfirm = "<%=MsgDelConfirm%>";
            msgPreWC = "<%=MsgPreWC%>";
            msgCurWC = "<%=MsgCurWC %>";
            msgNextWC = "<%=MsgNextWC %>";
            msgDefectSel = "<%=MsgDefectSel%>";
            msgCauseSel = "<%=MsgCauseSel%>";
            msgNotFound ="<%=MsgNotFound%>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";

            ShowRowEditInfo(null);

            resetTableHeight();
        };

        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 80;
            var marginValue = 10;
            var tableHeigth = 300;

            try {
                //tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div4.offsetHeight - div3.offsetHeight - adjustValue;
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight  - div3.offsetHeight - adjustValue;
            }
            catch (e) {
                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;

            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
            div2.style.height = extDivHeight + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
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


        function clkDelete() {
            ShowInfo("");
            

            var ret = confirm("Do you really want to delete this item?");
            if (!ret) {
                return false;
            }
            ShowWait();

            return true;

        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
        }

        function clkSave() {
            return checkAdaptorInfo();
        }

        function clkAdd() {
            return checkAdaptorInfo();
        }
        
        function clkQuery() {
            var PerWCTop = document.getElementById("<%=cmbPreWCTop.ClientID %>").value;

            if (emptyPattern.test(PerWCTop)) {
                alert("Please select [PerWC]!");
                document.getElementById("<%=cmbPreWCTop.ClientID %>").focus();
                return false;
            }
            document.getElementById("<%=hidPerWCTopSelect.ClientID %>").value = document.getElementById("<%=cmbPreWCTop.ClientID %>").value;
            ShowWait();
            return true;
        }

        function checkAdaptorInfo() {

            ShowInfo("");

            var defectObj = getMaintainDefectCmbObj();
            var causeObj = getMaintainCauseCmbObj();
            var preWCObj = document.getElementById("<%=this.ddlPreWC.ClientID%>_drpMaintainWC");
            var curWCObj = document.getElementById("<%=this.ddlCurWC.ClientID%>_drpMaintainWC");
            var nextWCObj = document.getElementById("<%=this.ddlNextWC.ClientID%>_drpMaintainWC");

            if (preWCObj.value.trim() == "") {
                alert(msgPreWC);
                return false;
            }

            document.getElementById("<%=hidPerWCTopSelect.ClientID%>").value = document.getElementById("<%=cmbPreWCTop.ClientID %>").value;
            document.getElementById("<%=hidPerWCSelect.ClientID%>").value = preWCObj.value.trim();
            document.getElementById("<%=hidMajorPart.ClientID%>").value = document.getElementById("<%=cmbMajorPart.ClientID %>").value;
            if (curWCObj.value.trim() == "") {
                alert(msgCurWC);
                return false;
            }

            if (nextWCObj.value.trim() == "") {
                alert(msgNextWC);
                return false;
            }

//            if (defectObj.value.trim() == "") {
//                alert(msgDefectSel);
//                return false;
//            }
            /*
            if (causeObj.value.trim() == "") {
                alert(msgCauseSel);
                return false;
            }
            */
            ShowWait();
            return true;
        }
        
        function clickTable(con) {
            ShowInfo("");
            var selectedRowIndex = con.index;
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }

        function ShowRowEditInfo(con) {

            var defectObj = getMaintainDefectCmbObj();
            var preWCObj = document.getElementById("<%=this.ddlPreWC.ClientID%>_drpMaintainWC");
            var curWCObj = document.getElementById("<%=this.ddlCurWC.ClientID%>_drpMaintainWC");
            var nextWCObj = document.getElementById("<%=this.ddlNextWC.ClientID%>_drpMaintainWC");
            var causeObj = getMaintainCauseCmbObj();
            if (con == null) {
                preWCObj.selectedIndex = 0;
                curWCObj.selectedIndex = 0;
                nextWCObj.selectedIndex = 0;
                defectObj.selectedIndex = 0;
                causeObj.selectedIndex = 0;
                document.getElementById("<%=cmbMajorPart.ClientID %>").value = "";
                document.getElementById("<%=txtFamilyAndModel.ClientID%>").value = "";
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
                return;
            }
            
            var curPreWC = con.cells[0].innerText.trim();
            var curCurWC = con.cells[1].innerText.trim();
            var family = con.cells[2].innerText.trim();
            var curDefect = con.cells[3].innerText.trim();
            var curCause = con.cells[4].innerText.trim();
            var curNextWC = con.cells[6].innerText.trim();
            

            preWCObj.value = curPreWC.split(" ")[0];
            curWCObj.value = curCurWC.split(" ")[0];
            document.getElementById("<%=txtFamilyAndModel.ClientID%>").value = family;
            defectObj.value = curDefect.split(" ")[0];
            causeObj.value = curCause.split(" ")[0];
            nextWCObj.value = curNextWC.split(" ")[0];
            document.getElementById("<%=cmbMajorPart.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=hidMajorPart.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=HiddenSelectedId.ClientID %>").value = con.cells[10].innerText.trim();
            if (curPreWC == "") {
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnDel.ClientID %>").disabled = false;
            }

        }

        function AddUpdateComplete(id) {
            var defectObj = getMaintainDefectCmbObj();
            var causeObj = getMaintainCauseCmbObj();
            var preWCObj = document.getElementById("<%=this.ddlPreWC.ClientID%>_drpMaintainWC");
            var curWCObj = document.getElementById("<%=this.ddlCurWC.ClientID%>_drpMaintainWC");
            var nextWCObj = document.getElementById("<%=this.ddlNextWC.ClientID%>_drpMaintainWC");

            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[10].innerText == id) {
                    selectedRowIndex = i;
                }
            }

            if (selectedRowIndex < 0) {
                preWCObj.selectedIndex = 0;
                curWCObj.selectedIndex = 0;
                nextWCObj.selectedIndex = 0;
                defectObj.selectedIndex = 0;
                causeObj.selectedIndex = 0;
                document.getElementById("<%=cmbMajorPart.ClientID %>").value = "";
                document.getElementById("<%=txtFamilyAndModel.ClientID%>").value = "";
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                //去掉标题行
                setGdHighLight(con);
                //setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }

        }
        
        function disposeTree(sender, args) {
            var elements = args.get_panelsUpdating();
            for (var i = elements.length - 1; i >= 0; i--) {
                var element = elements[i];
                var allnodes = element.getElementsByTagName('*'),
                length = allnodes.length;
                var nodes = new Array(length)
                for (var k = 0; k < length; k++) {
                    nodes[k] = allnodes[k];
                }
                for (var j = 0, l = nodes.length; j < l; j++) {
                    var node = nodes[j];
                    if (node.nodeType === 1) {
                        if (node.dispose && typeof (node.dispose) === "function") {
                            node.dispose();
                        }
                        else if (node.control && typeof (node.control.dispose) === "function") {
                            node.control.dispose();
                        }

                        var behaviors = node._behaviors;
                        if (behaviors) {
                            behaviors = Array.apply(null, behaviors);
                            for (var k = behaviors.length - 1; k >= 0; k--) {
                                behaviors[k].dispose();
                            }
                        }
                    }
                }
                element.innerHTML = "";
            }

        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);
    </script>

</asp:Content>
