<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MBAssemblyCodeMaitain.aspx.cs"
    Inherits="DataMaintain_MBAssemblyCodeMaitain" MasterPageFile="~/MasterPageMaintain.master" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%">
                <tr>
                    <td style="width: 200px">
                        <asp:Label ID="lblMBAssemblyCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="dSearch" runat="server" MaxLength="5" Width="249px" TabIndex="3"
                            SkinID="textBoxSkin" onkeypress="OnKeyPress(this)"></asp:TextBox>
                    </td>
                    <td align="right" width="90px">
                        <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkDelete())"
                            onmouseout="this.className='iMes_button_onmouseout'" onmouseover="this.className='iMes_button_onmouseover'"
                            onserverclick="btnDelete_ServerClick" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="height: 390px">
            <input id="hidRecordCount" type="hidden" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                Visible="true">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
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
                    <td style="width: 10%;">
                        <asp:Label ID="lblMBCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 35%">
                        <iMESMaintain:CmbMaintainMBCode ID="drpMBCode" runat="server" ssClass="iMes_textbox_input_Yellow"
                            Height="28px" Width="95%" FirstNullItem="false" />
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblSeries" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 35%">
                        <asp:TextBox ID="dSeries" runat="server" MaxLength="10" Width="95%" TabIndex="4"
                            SkinID="textBoxSkin"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 10%">
                        <input type="button" id="btnAdd" runat="server" class="iMes_button" onclick="if(clkAdd())"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" Width="95%">
                            <asp:ListItem Selected="True" Value="PC">PC SKU</asp:ListItem>
                            <asp:ListItem Value="RCTO">RCTO & Base Unit</asp:ListItem>
                            <asp:ListItem Value="BRZL">BRZL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblAssemblyCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="dAssemblyCode" runat="server" MaxLength="5" Width="95%" TabIndex="3"
                            CssClass="iMes_textbox_input_Yellow" onkeypress='checkCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                    </td>
                    <td align="right">
                        <input id="btnSave" runat="server" class="iMes_button" onclick="if(clkSave())" onmouseout="this.className='iMes_button_onmouseout'"
                            onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btnSave_ServerClick"
                            type="button" />
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
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidMBCode" runat="server" />
        <input type="hidden" id="hidSeries" runat="server" />
        <input type="hidden" id="hidType" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidID" runat="server" />
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
    
        function checkCodePress(obj)
        { 
           var key = event.keyCode;

           if(!((key >= 65 && key <= 90)||(key >= 97 && key <= 122)))
           {  
               event.keyCode = 0;
           }
     
         }
    
        window.onload = function() {
            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg6 = "<%=pmtMessage6%>";
            msg7 = "<%=pmtMessage7%>";
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
        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "<%=dSearch.ClientID %>") {
                    var value = document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findRunInTime(value, true);
                    }
                }
            }

        }
        function findRunInTime(searchValue, isNeedPromptAlert) {
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
                    alert(msg6);
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
            alert(msg6);
            return;
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
        function clkAdd() {
            return check();
        }
        function clkSave() {
            return check();
        }
        function check() {
            var mbcode = getMaintainMBCodeCmbObj();
            if (mbcode.value == "") {
                alert(msg1);
                return false;
            }
            var series = document.getElementById("<%=dSeries.ClientID %>");
            if (series.value == "") {
                alert(msg3);
                return false;
            }
            var type = document.getElementById("<%=drpType.ClientID %>");
            if (type.value == "") {
                alert(msg4);
                return false;
            }

            var assemblyCode = document.getElementById("<%=dAssemblyCode.ClientID %>");
            if (assemblyCode.value == "") {
                alert(msg5);
                return false;
            }
            
            
            if(AssemblyCodeCheck(assemblyCode.value)==false)
            {
                alert(msg7);
                return false;
            }
            
            return true;
        }
        function DeleteComplete() {
            ShowRowEditInfo(null);
        }

        function clickTable(con) {
            ShowInfo("");
            var selectedRowIndex = con.index;
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

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            getMaintainMBCodeCmbObj().value = con.cells[0].innerText.trim();
            document.getElementById("<%=dSeries.ClientID %>").value = con.cells[1].innerText.trim();
            var value = con.cells[2].innerText.trim();
            //ITC-1361-0104  ITC210012  2012-02-28
            var typeSelVal="";
            if (value == "PC SKU") {
                document.getElementById("<%=drpType.ClientID %>").value = "PC";
                typeSelVal="PC";
            }
            else if (value == "RCTO & Base Unit") {
                document.getElementById("<%=drpType.ClientID %>").value = "RCTO";
                 typeSelVal="RCTO";
            }
            else if (value == "BRZL") {
                document.getElementById("<%=drpType.ClientID %>").value = "BRZL";
                 typeSelVal="BRZL";
            }

            document.getElementById("<%=dAssemblyCode.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=hidMBCode.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidSeries.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=hidType.ClientID %>").value = typeSelVal;
            var currentId = con.cells[7].innerText.trim();
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
            document.getElementById("<%=dSearch.ClientID %>").value = "";
            getMaintainMBCodeCmbObj().selectedIndex = 0;
            document.getElementById("<%=dAssemblyCode.ClientID %>").value = "";
            document.getElementById("<%=dSeries.ClientID %>").value = "";
            document.getElementById("<%=drpType.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();
        }

        function checkFloat() {
            var data = window.clipboardData.getData("text");
            var test = /^[1-9]*[0-9]{1}\.[1-9]{1}$/;
            if (!test.test(data) || data.length > 4) {
                window.clipboardData.setData("text", '');
            }

        }       
        
        
        function AssemblyCodeCheck(value)
        {
            var reExp = /^[a-z,A-Z]*$/;
	        if (!reExp.exec(value)){
		        return false;
	        }

            if(value.length!=5)
            {
                return false;
            }
            return true;

        }
    
    </script>

</asp:Content>
