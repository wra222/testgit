<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PartForbidRule.aspx.cs" Inherits="DataMaintain_PartForbidRule" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ></asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td style="width:10%">
                        <asp:Label ID="lblCustomerTop" runat="server" CssClass="iMes_label_13pt" Text="Customer:"></asp:Label>
                    </td>
                    <td style="width:20%">
                        <asp:DropDownList ID="cmbCustomerTop" runat="server" Width="98%"></asp:DropDownList>            
                    </td >
                    <td style="width:10%">
                        <asp:Label ID="lblCategoryTop" runat="server" CssClass="iMes_label_13pt" Text="Category:"></asp:Label>
                    </td>
                    <td  style="width:20%">
                        <asp:DropDownList ID="cmbCategoryTop" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td align="right">
                       <input type="button" id="btnQuery" runat="server" value="Query" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkQuery())" onserverclick="Query_ServerClick"/>
                    </td> 
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblLineTop" runat="server" CssClass="iMes_label_13pt" Text="Line:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtLineTop" runat="server" style="width:96%" maxlength="8" onkeypress="checkCodePress_Line(this)" />
                    </td >
                    <td>
                        <asp:Label ID="lblFamilyTop" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtFamilyTop" runat="server" style="width:96%" maxlength="64" onkeypress="checkCodePress_Family(this)" />
                    </td >
                    <td></td>
                </tr>
            </table>
        </div>   
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblMasterLabelList" runat="server" CssClass="iMes_label_13pt" Text="Part Forbid Rule List"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                        <input type="button" id="btnDelete" runat="server" value="Delete" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
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
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt" Text="Customer:"></asp:Label>
                    </td>
                    <td style="width:30%">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbCustomer" runat="server" Width="98%"  AutoPostBack="true" ></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width:10%">
                        <asp:Label ID="lblBomNodeType" runat="server" CssClass="iMes_label_13pt" Text="BomNodeType:"></asp:Label>
                    </td>
                    <td style="width:30%">
                        <input type="text" id="txtBomNodeType" runat="server" style="width:97%" maxlength="8"/>
                    </td>
                    <td style="width:20%"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCategory" runat="server" CssClass="iMes_label_13pt" Text="Category:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCategory" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblVendorCode" runat="server" CssClass="iMes_label_13pt" Text="VendorCode(Reg):"></asp:Label>
                    </td>
                    <td colspan="2">
                        <input type="text" id="txtVendorCode" runat="server" style="width:97%" maxlength="255"/>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt" Text="Line:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbLine" runat="server" Width="98%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt" Text="PartNo(Reg):"></asp:Label>
                    </td>
                    <td colspan="2">
                        <input type="text" id="txtPartNo" runat="server" style="width:97%" maxlength="255"/>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtFamily" runat="server" style="width:97%" maxlength="64" onkeypress="checkCodePress_Family(this)" />
                    </td>
                    <td>
                        <asp:Label ID="lblNoticeMsg" runat="server" CssClass="iMes_label_13pt" Text="NoticeMsg:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtNoticeMsg" runat="server" style="width:97%" maxlength="255"/>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblExceptModel" runat="server" CssClass="iMes_label_13pt" Text="ExceptModel(Reg):"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtExceptModel" runat="server" style="width:97%" maxlength="255"/>
                    </td>
                    <td>
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Text="Remark:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtRemark" runat="server" style="width:97%" maxlength="255"/>
                    </td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" value="Save" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>  
            </table>
        </div>
    </div>    
    
    <input type="hidden" id="hidDeleteID" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    <input type="hidden" id="HiddenUserName" runat="server" />
    <input type="hidden" id="hidFamilyAndModel" runat="server" />
    <input type="hidden" id="hidLine" runat="server" />
    
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

        function checkCodePress_Line(obj) {
            var key = event.keyCode;
            if (!((key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122))) {
                event.keyCode = 0;
            }
        }

        function checkCodePress_Family(obj) {
            var key = event.keyCode;
            if (!((key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122) || key == 32 || key == 46)) {
                event.keyCode = 0;
            }
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
                document.getElementById("<%=hidDeleteID.ClientID %>").value = con.cells[0].innerText;
            }     
            else
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
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
                document.getElementById("<%=cmbCustomer.ClientID %>").selectedIndex = 0;
                document.getElementById("<%=txtBomNodeType.ClientID %>").value = "";
                document.getElementById("<%=cmbCategory.ClientID %>").selectedIndex = 0;
                document.getElementById("<%=txtVendorCode.ClientID %>").value = "";
                document.getElementById("<%=cmbLine.ClientID %>").selectedIndex = 0;
                document.getElementById("<%=txtPartNo.ClientID %>").value = "";
                
                document.getElementById("<%=txtFamily.ClientID %>").value = "";
                document.getElementById("<%=txtNoticeMsg.ClientID %>").value = "";
                document.getElementById("<%=txtExceptModel.ClientID %>").value = "";
                document.getElementById("<%=txtRemark.ClientID %>").value = "";
                document.getElementById("<%=hidLine.ClientID %>").value = "";
                
            }
            else {
                document.getElementById("<%=cmbCustomer.ClientID %>").value = con.cells[1].innerText.trim();
                document.getElementById("<%=txtBomNodeType.ClientID %>").value = con.cells[6].innerText.trim();
                document.getElementById("<%=cmbCategory.ClientID %>").value = con.cells[2].innerText.trim();
                document.getElementById("<%=txtVendorCode.ClientID %>").value = con.cells[7].innerText.trim();
                document.getElementById("<%=cmbLine.ClientID %>").value = con.cells[3].innerText.trim();
                document.getElementById("<%=txtPartNo.ClientID %>").value = con.cells[8].innerText.trim();

                document.getElementById("<%=txtFamily.ClientID %>").value = con.cells[4].innerText.trim();
                document.getElementById("<%=txtNoticeMsg.ClientID %>").value = con.cells[9].innerText.trim();
                document.getElementById("<%=txtExceptModel.ClientID %>").value = con.cells[5].innerText.trim();
                document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[11].innerText.trim();
                PageMethods.initline_WEB(con.cells[1].innerText.trim(), con.cells[3].innerText.trim(), onSucess, onError);
            }
        }

        function onSucess(result) {
            document.getElementById("<%=cmbLine.ClientID %>").options.length = 0;
            var items = result[0];
            document.getElementById("<%=cmbLine.ClientID %>").options.add(new Option("", ""))
            for (var j = 0; j < items.length; j++) {
                document.getElementById("<%=cmbLine.ClientID %>").options.add(new Option(items[j], items[j]))
            }

            document.getElementById("<%=hidLine.ClientID %>").value = result[1];
            document.getElementById("<%=cmbLine.ClientID %>").value = result[1];
        }
        
        function onError(result) {
            HideWait();
            alert(result.get_message());

        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=cmbCustomer.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=txtBomNodeType.ClientID %>").value = "";
            document.getElementById("<%=cmbCategory.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=txtVendorCode.ClientID %>").value = "";
            document.getElementById("<%=cmbLine.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=txtPartNo.ClientID %>").value = "";

            document.getElementById("<%=txtFamily.ClientID %>").value = "";
            document.getElementById("<%=txtNoticeMsg.ClientID %>").value = "";
            document.getElementById("<%=txtExceptModel.ClientID %>").value = "";
            document.getElementById("<%=txtRemark.ClientID %>").value = "";
            document.getElementById("<%=hidLine.ClientID %>").value = "";
        }
        
        function clkSave() {
            if (checkInput()) {
                selectedRowIndex = -1;
                ShowWait();
                document.getElementById("<%=hidLine.ClientID %>").value = document.getElementById("<%=cmbLine.ClientID %>").value;
                return true;
            }
            return false;
        }

        function checkInput() {

            var Customer = document.getElementById("<%=cmbCustomer.ClientID %>").value;
            var Category = document.getElementById("<%=cmbCategory.ClientID %>").value;
            var Line = document.getElementById("<%=cmbLine.ClientID %>").value;
            var Family = document.getElementById("<%=txtFamily.ClientID %>").value;
            var BomNodeType = document.getElementById("<%=txtBomNodeType.ClientID %>").value;

            if (Customer == "") {
                alert("Please Select Customer");
                return false;
            }

            if (Category == "") {
                alert("Please Select Category");
                return false;
            }

            if (Category == "Allow") {
                if (Line == "" && Family=="") {
                    alert("Please Select Line or Family");
                    return false;
                } 
            }

            if (BomNodeType == "") {
                alert("Please Input BomNodeType");
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

