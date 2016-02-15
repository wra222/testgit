<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="DefectHoldRule.aspx.cs" Inherits="DataMaintain_DefectHoldRule" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td style="width:10%">
                        <asp:Label ID="lblLineTop" runat="server" CssClass="iMes_label_13pt">Line:</asp:Label>
                    </td>
                    <td style="width:20%">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbLineTop" runat="server" Width="98%"></asp:DropDownList>            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td >
                    <td style="width:10%">
                        <asp:Label ID="lblFamilyAndModelTop" runat="server" CssClass="iMes_label_13pt">Family/Model:</asp:Label>
                    </td>
                    <td  style="width:20%">
                        <asp:TextBox ID="txtFamilyAndModelTop" runat="server" Width="96%" MaxLength="30"></asp:TextBox>
                    </td>
                    
                    
                    <td align="right">
                       <input type="button" id="btnQuery" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkQuery())" onserverclick="Query_ServerClick"/>
                    </td> 
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDefectTop" runat="server" CssClass="iMes_label_13pt">Defect:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDefectTop" runat="server" Width="96%" MaxLength="10"></asp:TextBox>
                    </td >
                    <td colspan="3"></td>
                </tr>
            </table>
        </div>   
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblMasterLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                        <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
                    </td>    
                </tr>
            </table>                                     
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" 
                        HighLightRowPosition="3" 
                        AutoGenerateColumns="true" Width="100%" GvExtWidth="100%" GvExtHeight="390px"  
                        onrowdatabound="gd_RowDataBound" 
                        OnGvExtRowClick="clickTable(this)" 
                        RowStyle-Height="20" 
                        AutoHighlightScrollByValue ="true">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" border="0"  class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width:10%">
                        <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt">Line:</asp:Label>
                    </td>
                    <td style="width:30%">
                        <asp:DropDownList ID="cmbLine" runat="server" Width="98%" ></asp:DropDownList>
                    </td>
                    <td style="width:10%">
                        <asp:Label ID="lblFamilyAndModel" runat="server" CssClass="iMes_label_13pt">Family/Model:</asp:Label>
                    </td>
                    <td style="width:30%">
                        <asp:TextBox ID="txtFamilyAndModel" runat="server" Width="96%" MaxLength="30"></asp:TextBox>
                    </td>
                    <td style="width:20%"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDefect" runat="server" CssClass="iMes_label_13pt">Defect:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDefect" runat="server" Width="75%" MaxLength="10"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblExceptCause" runat="server" CssClass="iMes_label_13pt">ExceptCause:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtExceptCause" runat="server" Width="75%" MaxLength="64" ></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEqualSameDefectCount" runat="server" CssClass="iMes_label_13pt">EqualSameDefectCount:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEqualSameDefectCount" runat="server" Width="75%" MaxLength="5" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblOverDefectCount" runat="server" CssClass="iMes_label_13pt">OverDefectCount:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOverDefectCount" runat="server" Width="75%" MaxLength="5" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCheckINStation" runat="server" CssClass="iMes_label_13pt">CheckINStation:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCheckINStation" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblHoldCode" runat="server" CssClass="iMes_label_13pt">HoldCode:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbHoldCode" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblHoldStation" runat="server" CssClass="iMes_label_13pt">HoldStation:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbHoldStation" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblHoldDescr" runat="server" CssClass="iMes_label_13pt">HoldDescr:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtHoldDescr" runat="server" Width="96%" MaxLength="255"></asp:TextBox>
                    </td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>  
            </table>
        </div>
    </div>    
    
    <input type="hidden" id="hidDeleteID" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    <input type="hidden" id="HiddenUserName" runat="server" />
    <input type="hidden" id="hidFamilyAndModel" runat="server" />
    
    <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" Visible="false" >
    </asp:UpdatePanel>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div>
    <script type="text/javascript" language="javascript">
        var customer;
        var editor;
        var emptyString = "";
        var selectedRowIndex = -1;
        var msgConfirmDelete="";
        var msgInputFamily;
        var emptyPattern = /^\s*$/;
        window.onload = function()
        {
            msgConfirmDelete="<%=pmtMessage1 %>";
            msgInputFamily="<%=pmtMessage4 %>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=UserId%>";
            resetTableHeight();
        };
         
        function resetTableHeight()
        {
            //动态调整表格的高度
            var adjustValue=60;     
            var marginValue=10;  
            var tableHeigth=300;
            
            try{
                tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div4.offsetHeight-div3.offsetHeight-adjustValue;
            }
            catch(e){
                //ignore
            }                
            //为了使表格下面有写空隙
            var extDivHeight=tableHeigth+marginValue;
            document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
            div2.style.height=extDivHeight+"px";
            document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
        }

        function clickTable(con)
        {
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gd.ClientID %>");                
            }
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            selectedRowIndex=parseInt(con.index, 10);
            setDetailInfo(con);      
            if (hasEditData())
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=cmbLine.ClientID %>").disabled = true;
                document.getElementById("<%=txtFamilyAndModel.ClientID %>").disabled = true;
                document.getElementById("<%=txtDefect.ClientID %>").disabled = true;
                document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").disabled = true;
                document.getElementById("<%=txtOverDefectCount.ClientID %>").disabled = true;
                document.getElementById("<%=hidDeleteID.ClientID %>").value = con.cells[0].innerText;
            }     
            else
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                document.getElementById("<%=cmbLine.ClientID %>").disabled = false;
                document.getElementById("<%=txtFamilyAndModel.ClientID %>").disabled = false;
                document.getElementById("<%=txtDefect.ClientID %>").disabled = false;
                document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").disabled = false;
                document.getElementById("<%=txtOverDefectCount.ClientID %>").disabled = false;
                document.getElementById("<%=hidDeleteID.ClientID %>").value = "";
            } 
        }

        function AddUpdateComplete(id) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id) {
                    selectedRowIndex = i;
                }
            }
            if (selectedRowIndex < 0) {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                clearDetailInfo();
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            }
        }
        var iSelectedRowIndex=null;

        function DeleteComplete() {
            document.getElementById("<%=hidDeleteID.ClientID %>").value = "";
        }
       
        function setGdHighLight(con)
        {
            if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
            {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
            }     
            //设置当前高亮行   
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            //记住当前高亮行
            iSelectedRowIndex=parseInt(con.index, 10);
            selectedRowIndex=parseInt(con.index, 10);
        }
        
        function hasEditData()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText))
            {
                return false;
            }
            return true;
        }        
        
        function clkQuery()
        {
            clearDetailInfo();
            ShowWait();
            return true;
        }
        var updateFlag = "";
        
        function setDetailInfo(con)
        {
            if (con.cells[0].innerText.trim() == "") {
                document.getElementById("<%=cmbLine.ClientID %>").value = "";
                document.getElementById("<%=txtFamilyAndModel.ClientID %>").value = "";
                document.getElementById("<%=txtDefect.ClientID %>").value = "";
                document.getElementById("<%=txtExceptCause.ClientID %>").value = "";
                document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").value = "";
                document.getElementById("<%=txtOverDefectCount.ClientID %>").value = "";
                //
                document.getElementById("<%=cmbCheckINStation.ClientID %>").value = "";
                document.getElementById("<%=cmbHoldStation.ClientID %>").value = "";
                document.getElementById("<%=cmbHoldCode.ClientID %>").value = "";
                document.getElementById("<%=txtHoldDescr.ClientID %>").value = "";
                
            }
            else {
                document.getElementById("<%=cmbLine.ClientID %>").value = con.cells[1].innerText.trim();
                document.getElementById("<%=txtFamilyAndModel.ClientID %>").value = con.cells[2].innerText.trim();
                document.getElementById("<%=txtDefect.ClientID %>").value = con.cells[3].innerText.trim();
                document.getElementById("<%=txtExceptCause.ClientID %>").value = con.cells[4].innerText.trim();
                document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").value = con.cells[5].innerText.trim();
                document.getElementById("<%=txtOverDefectCount.ClientID %>").value = con.cells[6].innerText.trim();
                
                document.getElementById("<%=cmbCheckINStation.ClientID %>").value = con.cells[7].innerText.trim();
                document.getElementById("<%=cmbHoldStation.ClientID %>").value = con.cells[8].innerText.trim();
                document.getElementById("<%=cmbHoldCode.ClientID %>").value = con.cells[9].innerText.trim();
                document.getElementById("<%=txtHoldDescr.ClientID %>").value = con.cells[10].innerText.trim();
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=cmbLine.ClientID %>").value = "";
            document.getElementById("<%=txtFamilyAndModel.ClientID %>").value = "";
            document.getElementById("<%=txtDefect.ClientID %>").value = "";
            document.getElementById("<%=txtExceptCause.ClientID %>").value = "";
            document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").value = "";
            document.getElementById("<%=txtOverDefectCount.ClientID %>").value = "";
            document.getElementById("<%=cmbCheckINStation.ClientID %>").value = "";
            document.getElementById("<%=cmbHoldStation.ClientID %>").value = "";
            document.getElementById("<%=cmbHoldCode.ClientID %>").value = "";
            document.getElementById("<%=txtHoldDescr.ClientID %>").value = "";
            document.getElementById("<%=cmbLine.ClientID %>").disabled = false;
            document.getElementById("<%=txtFamilyAndModel.ClientID %>").disabled = false;
            document.getElementById("<%=txtDefect.ClientID %>").disabled = false;
            document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").disabled = false;
            document.getElementById("<%=txtOverDefectCount.ClientID %>").disabled = false;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                selectedRowIndex = -1;
                ShowWait();
                return true;
            }
            return false;
        }

        function checkInput() {

            var EqualSameDefectCount = document.getElementById("<%=txtEqualSameDefectCount.ClientID %>").value;
            var OverDefectCount = document.getElementById("<%=txtOverDefectCount.ClientID %>").value;
            if (EqualSameDefectCount == "" && OverDefectCount == "") {
                alert("Please Input EqualSameDefectCount or OverDefectCount...");
                return;
            }

            if (EqualSameDefectCount > 0 && OverDefectCount != 0) {
                alert("OverDefectCount need input 0...");
                return;
            }

            if (OverDefectCount > 0 && EqualSameDefectCount != 0) {
                alert("EqualSameDefectCount need input 0...");
                return;
            }
            
            var checkInStation = document.getElementById("<%=cmbCheckINStation.ClientID %>").options[document.getElementById("<%=cmbCheckINStation.ClientID %>").selectedIndex].value;
            var holdStation = document.getElementById("<%=cmbHoldStation.ClientID %>").options[document.getElementById("<%=cmbHoldStation.ClientID %>").selectedIndex].value;
            var holdCode = document.getElementById("<%=cmbHoldCode.ClientID %>").options[document.getElementById("<%=cmbHoldCode.ClientID %>").selectedIndex].value;
            var holdDescr = document.getElementById("<%=txtHoldDescr.ClientID %>").value

            if (checkInStation == "") {
                alert("checkInStation can't Empty ");
                return false;
            }
            if (holdStation == "") {
                alert("holdStation can't Empty ");
                return false;
            }
            if (holdCode == "") {
                alert("holdCode can't Empty ");
                return false;
            }
            if (holdDescr == "") {
                alert("holdDescr can't Empty ");
                return false;
            }
            
            return true;
        }
        
        function clkDelete()
        {
            if (confirm("确定要删除这条记录么?"))
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var row = tblObj.rows[selectedRowIndex + 1];
                document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
                ShowWait();
                return true;
            }
            return false;
        }
    </script>
</asp:Content>

