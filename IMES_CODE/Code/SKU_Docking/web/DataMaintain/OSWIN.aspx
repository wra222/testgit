
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="OSWIN.aspx.cs" Inherits="DataMaintain_OSWIN" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <style type="text/css">
        table.edit
        {
            border: thin solid Black; 
            background-color: #99CDFF;
            margin:0 0 10 0;
        }
        
    .iMes_textbox_input_Normal
    {
	border-width:1px;
	border-style:ridge;
	border-left-color:Black;
	border-right-color:Gray;
	border-bottom-color:Black;
	border-top-color:Gray;
	height: 20px;
    line-height: 20px;
	font-family:Verdana;
	font-size: 9pt;
    text-align:left;
    vertical-align:middle;
    padding-left:2px;
    padding-right:2px;
    margin-left:1px;
    margin-right:1px;
    word-break: break-all;
	word-wrap: break-word;
    }
    
    </style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td width="100px">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbFamily" runat="server" Width="250px" OnSelectedIndexChanged="btnQuery_ServerClick" AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                        
                    </td>
                </tr> 
            </table>
         </div>   
         <div id="div1" class="iMes_div_MainTainDiv1">
               <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblOSWIN" runat="server" Text="OSWIN List:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                        <input type="button" id="btnDelete" value="Delete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
                    </td>    
                </tr>
             </table>                                     
        </div>
            <div id="div2">
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnLoadFirstDataList" EventName="ServerClick" />
                    </Triggers>
                    <ContentTemplate></ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <ContentTemplate>
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" GvExtWidth="100%" GvExtHeight="390px"  onrowdatabound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" RowStyle-Height="20">
                        </iMES:GridViewExt>
                     </ContentTemplate>
                 </asp:UpdatePanel>
            </div>
            <div id="div3">
            <table width="100%" border="0" style="table-layout:fixed;" class="edit">
                <tr>
                    <td width="100px">
                        <asp:Label ID="lblFamily2" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbFamily2" runat="server" Width="50%"></asp:DropDownList>
                    </td>
                    <td width="100px">
                        <asp:Label ID="lblAV" runat="server" CssClass="iMes_label_13pt" Text="AV:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtAV" runat="server" class="iMes_textbox_input_Normal" style="width:48%;" maxlength="16" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100px">
                        <asp:Label ID="lblZmod" runat="server" CssClass="iMes_label_13pt" Text="Zmod:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtZmod" runat="server" class="iMes_textbox_input_Normal" style="width:48%;" maxlength="16" />
                    </td>
                    <td width="100px">
                        <asp:Label ID="lblImage" runat="server" CssClass="iMes_label_13pt" Text="Image:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtImage" runat="server" class="iMes_textbox_input_Normal" style="width:48%;" maxlength="16" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100px">
                        <asp:Label ID="lblOS" runat="server" CssClass="iMes_label_13pt" Text="OS:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtOS" runat="server" class="iMes_textbox_input_Normal" style="width:48%;" maxlength="16" />
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <input type="button" value="Save" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>    
    <input type="hidden" id="hidID" runat="server" />
    <input type="hidden" id="hidFamily" runat="server" />
    <input type="hidden" id="hidFamily2" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    <button id="btnLoadFirstDataList" runat="server" type="button" style="display:none" onserverclick ="btnLoadFirstDataList_ServerClick"> </button>
    <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" Visible="false" >
    </asp:UpdatePanel>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div>
    <script type="text/javascript" language="javascript">
        var customer;
        var editor;
        var emptyPattern = /^\s*$/;
        var SupplierPattern = /^[0-9A-Z]{2}$/;
        var emptyString = "";
        var selectedRowIndex = -1;
        
        window.onload = function()
        {
            resetTableHeight();
            showFirstDataList();
        };

        function showFirstDataList() {
            document.getElementById("<%=btnLoadFirstDataList.ClientID%>").click();
        }
        
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
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
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
            selectedRowIndex = parseInt(con.index, 10);
            setDetailInfo();      
            
            if (hasEditData())
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }     
            else
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            } 
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
            var Family = document.getElementById("<%=cmbFamily.ClientID %>").value;

            if (Family == "")
            {
                alert("Please select [Family]!");
                document.getElementById("<%=cmbFamily.ClientID %>").focus();
                return false;
            }
            selectedRowIndex = -1;
            document.getElementById("<%=hidFamily.ClientID %>").value = Family;
            clearDetailInfo();
            ShowWait();
            return true;
        }
        
        function setDetailInfo()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var row = tblObj.rows[selectedRowIndex + 1];

            if(row.cells[0].innerText.trim()=="")
            {
                clearDetailInfo();
            }
            else
            {
                document.getElementById("<%=cmbFamily2.ClientID %>").value = row.cells[0].innerText;
                document.getElementById("<%=txtZmod.ClientID %>").value = row.cells[1].innerText;
                document.getElementById("<%=txtOS.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=txtAV.ClientID %>").value = row.cells[3].innerText;
                document.getElementById("<%=txtImage.ClientID %>").value = row.cells[4].innerText;
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=cmbFamily2.ClientID %>").value = "";
            document.getElementById("<%=txtZmod.ClientID %>").value = emptyString;
            document.getElementById("<%=txtOS.ClientID %>").value = emptyString;
            document.getElementById("<%=txtAV.ClientID %>").value = emptyString;
            document.getElementById("<%=txtImage.ClientID %>").value = emptyString;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                document.getElementById("<%=hidFamily2.ClientID %>").value = document.getElementById("<%=cmbFamily2.ClientID %>").value;
                selectedRowIndex = -1;
                ShowWait();
                return true;
            }
            
            return false;           
        }
        
        function checkInput()
        {
            var Family2 = document.getElementById("<%=cmbFamily2.ClientID %>").value;

            if (Family2 == "") {
                alert("Please select [Family]!");
                document.getElementById("<%=cmbFamily2.ClientID %>").focus();
                return false;
            }

            var Zmod = document.getElementById("<%=txtZmod.ClientID %>").value;

            if (Zmod == "") {
                alert("Please input [Zmod]!");
                document.getElementById("<%=txtZmod.ClientID %>").focus();
                return false;
            }

            var OS = document.getElementById("<%=txtOS.ClientID %>").value;

            if (OS == "") {
                alert("Please input [OS]!");
                document.getElementById("<%=txtOS.ClientID %>").focus();
                return false;
            }

            var AV = document.getElementById("<%=txtAV.ClientID %>").value;

            if (AV == "") {
                alert("Please input [AV]!");
                document.getElementById("<%=txtAV.ClientID %>").focus();
                return false;
            }
            return true;      
        }
        
        function clkDelete()
        {
            if (confirm("Do you really want to delete this item?")) 
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var row = tblObj.rows[selectedRowIndex + 1];
                document.getElementById("<%=hidID.ClientID %>").value = row.cells[8].innerText;
                document.getElementById("<%=hidFamily2.ClientID %>").value = row.cells[0].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
                ShowWait();
                return true;
            }
            return false;
        }
    </script>
</asp:Content>

