<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" 
    CodeFile="LabelTypeRuleDialog.aspx.cs" Inherits="LabelTypeRuleDialog" ValidateRequest="false" %>
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit" >            
                <tr >
                    <td width="5%">
                        <asp:Label ID="lblLebelType" runat="server" CssClass="iMes_label_13pt">LabelType:</asp:Label>
                    </td>
                    <td width="15%" align="left">
                        <asp:Label ID="lblLebelTypeName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="50%"></td>
                </tr>
             </table>
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%"></td> 
                    <td align="right">
                        <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick" />
                    </td>            
                </tr>
             </table>    
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ChildrenAsTriggers="false">
        <ContentTemplate>
        <div id="div2" style="height:400px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="390px" Height="382px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
    </div>
    <div id="div3">
        <table width="100%" class="iMes_div_MainTainEdit" >
            <tr>
                <td width="5%">
                    <asp:Label ID="lblInfoName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="20%">
                    <%--<asp:TextBox ID="txtInfoName" runat="server" MaxLength="20" Width="98%"></asp:TextBox>--%>
                    <input type="text" id="txtInfoName" runat="server" maxlength="20" style="width:98%" />
                </td>
                <td width="5%">
                    <asp:Label ID="lblInfoValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="45%" colspan="2">
                    <%--<asp:TextBox ID="txtInfoValue" runat="server" MaxLength="255" Width="94%"></asp:TextBox>--%>
                    <input type="text" id="txtInfoValue" runat="server" maxlength="255" style="width:94%" />
                </td>   
                <td width="25%"></td>   
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                </td>
                <td  colspan="3">
                    <%--<asp:TextBox ID="txtDescription" runat="server" MaxLength="255" Width="98%"></asp:TextBox>--%>
                    <input type="text" id="txtDescription" runat="server" maxlength="255" style="width:98%" />
                </td>
                <td>
                    <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick" />
                    <input type="button" id="btnClose" runat="server" onclick="btnClose_Click()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  />
                </td>
                <td></td>
            </tr>
        </table> 
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
        </Triggers>                      
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional"> 
            <ContentTemplate>
                <input id="hidLabelType" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        
        
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidID" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        
        <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
            <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
                <tr>
                    <td align="center">
                        <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/>
                    </td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td>
                </tr>
            </table>
        </div>  
    <script type="text/javascript">

        window.onload = function() {
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            //initControls();  
            ShowRowEditInfo(null);
            //设置表格的高度  
            resetTableHeight();

        };

        function clkButton() {
            switch (event.srcElement.id) {
                case "<%=btnSave.ClientID %>":

                    if (!clkSave()) {
                        return false;
                    }
                    break;

                case "<%=btnDelete.ClientID %>":

                    if (!clkDelete()) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;
        }

        function btnClose_Click() {
            if (document.getElementById("<%=hidLabelType.ClientID %>").value == "") {
                window.returnValue = false;
            }
            else {
                window.returnValue = document.getElementById("<%=hidLabelType.ClientID %>").value
            }
            window.close();
        }

        window.onbeforeunload = function() {
            if (document.getElementById("<%=hidLabelType.ClientID %>").value == "") {
                window.returnValue = false;
            }
            else {
                window.returnValue = document.getElementById("<%=hidLabelType.ClientID %>").value
            }

        };

        function clkSave() {
            if (document.getElementById("<%=txtInfoName.ClientID %>").value == "") {
                alert("InfoName必須要有值...");
                return false;
            }
            if (document.getElementById("<%=txtInfoValue.ClientID %>").value == "") {
                alert("InfoValue必須要有值...");
                return false;
            }
            return true;
        }

        function AddUpdateComplete(id) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[6].innerText == id) {
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

        function clkDelete() {
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                alert("請先選擇一筆資料...");
                return false;
            }
            var ret = confirm("是否確定刪除此記錄？");
            if (!ret) {
                return false;
            }
            return true;
        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
        }
        
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;
            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }
        
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }

        function setNewItemValue() {
            
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            //set null value to input  btnPartInfo
            document.getElementById("<%=hidID.ClientID %>").value = "";
            document.getElementById("<%=txtInfoName.ClientID %>").value = "";
            document.getElementById("<%=txtInfoValue.ClientID %>").value = "";
            document.getElementById("<%=txtDescription.ClientID %>").value = "";
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            //set table value to input
            var currentID = con.cells[6].innerText.trim();
            if (currentID == "") {
                setNewItemValue();
            }
            else {
                document.getElementById("<%=hidID.ClientID %>").value = currentID;
                
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=txtInfoName.ClientID %>").value = con.cells[0].innerText.trim();
                document.getElementById("<%=txtInfoValue.ClientID %>").value = con.cells[1].innerText.trim();
                document.getElementById("<%=txtDescription.ClientID %>").value = con.cells[2].innerText.trim();
            }
        }

        var iSelectedRowIndex = null;
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
    </script>
</asp:Content>
