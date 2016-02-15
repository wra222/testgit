
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="QTime.aspx.cs" Inherits="DataMaintain_QTime" Title="无标题页" %>
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
                    <td width="200px">
                        <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt" Text="Line:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbLine" runat="server" Width="10%" AutoPostBack="true" OnSelectedIndexChanged="cmbLine_Selected"></asp:DropDownList>        
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
                        <asp:Label ID="lblQTimeList" runat="server" CssClass="iMes_label_13pt" Text="QTimeList:"></asp:Label>
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
                    </Triggers>
                    
                    <ContentTemplate>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                 <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%" GvExtWidth="100%" GvExtHeight="390px"  onrowdatabound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" RowStyle-Height="20">
                 
                 </iMES:GridViewExt>
                 
                 </ContentTemplate>
                 </asp:UpdatePanel>
            </div>
            <div id="div3">
            <table width="100%" border="0" style="table-layout:fixed;" class="edit">
                <tr>
                    <td style="width:9%">
                        <asp:Label ID="lblLine2" Text="Line:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:30%">
                        <asp:DropDownList ID="cmbLine2" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td style="width:9%">
                        <asp:Label ID="lblStation" Text="Station:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbStation" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td style="width:9%">
                        <asp:Label ID="lblFamily" Text="Family:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtFamily" runat="server" class="iMes_textbox_input_Normal" style="" />
                    </td>
                    <td>
                    
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCatagory" Text="Catagory:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCatagory" runat="server" Width="98%">
                            <asp:ListItem Text="Max" Value="Max" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Min" Value="Min"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblTimeOut" Text="TimeOut:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtTimeOut" runat="server" class="iMes_textbox_input_Normal" style="" />秒
                    </td>
                    <td>
                        <asp:Label ID="lblStopTime" Text="StopTime:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtStopTime" runat="server" class="iMes_textbox_input_Normal" style="" />秒
                    </td>
                    <td>
                        <input type="button" id="btnAdd" runat="server" value="Add" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblDefectCode" Text="DefectCode:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbDefectCode" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblHoldStation" Text="HoldStation:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbHoldStation" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblHoldStatus" Text="HoldStatus:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbHoldStatus" runat="server" Width="98%">
                            <asp:ListItem Text="Fail" Value="Fail" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Pass" Value="Pass"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input type="button" id="btnSave" runat="server" value="Save" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblExceptstation" Text="Exceptstation:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <input type="text" id="txtExceptstation" runat="server" class="iMes_textbox_input_Normal" style="Width:60%" />
                    </td>
                    <td>
                    
                    </td>
                    <td>
                    
                    </td>
                </tr>
            </table>
        </div>
    </div>    
    <input type="hidden" id="hidLine" runat="server" />
    <input type="hidden" id="hidLine2" runat="server" />
    <input type="hidden" id="hidStation" runat="server" />
    <input type="hidden" id="hidCatagory" runat="server" />
    <input type="hidden" id="hidDefectCode" runat="server" />
    <input type="hidden" id="hidHoldStation" runat="server" />
    <input type="hidden" id="hidHoldStatus" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    
    <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" Visible="false" >
    </asp:UpdatePanel>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div>
    <script type="text/javascript" language="javascript">
        var customer;
        var editor;
        var emptyPattern = /^\s*$/;
        var emptyString = "";
        var selectedRowIndex = -1;
        
        
        window.onload = function()
        {
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
            selectedRowIndex=parseInt(con.index, 10);
            
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
                document.getElementById("<%=cmbLine2.ClientID %>").value = row.cells[0].innerText;
                document.getElementById("<%=cmbStation.ClientID %>").value = row.cells[1].innerText;
                document.getElementById("<%=txtFamily.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=cmbCatagory.ClientID %>").value = row.cells[3].innerText;
                document.getElementById("<%=txtTimeOut.ClientID %>").value = row.cells[4].innerText;
                document.getElementById("<%=txtStopTime.ClientID %>").value = row.cells[5].innerText;
                document.getElementById("<%=cmbDefectCode.ClientID %>").value = row.cells[6].innerText;
                document.getElementById("<%=cmbHoldStation.ClientID %>").value = row.cells[7].innerText;
                document.getElementById("<%=cmbHoldStatus.ClientID %>").value = row.cells[8].innerText;
                document.getElementById("<%=txtExceptstation.ClientID %>").value = row.cells[9].innerText;
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=cmbLine2.ClientID %>").value = "";
            document.getElementById("<%=cmbStation.ClientID %>").value = "";
            document.getElementById("<%=txtFamily.ClientID %>").value = emptyString;
            document.getElementById("<%=cmbCatagory.ClientID %>").value = "";
            document.getElementById("<%=txtTimeOut.ClientID %>").value = emptyString;
            document.getElementById("<%=txtStopTime.ClientID %>").value = emptyString;
            document.getElementById("<%=cmbDefectCode.ClientID %>").value = "";
            document.getElementById("<%=cmbHoldStation.ClientID %>").value = "";
            document.getElementById("<%=cmbHoldStatus.ClientID %>").value = "";
            document.getElementById("<%=txtExceptstation.ClientID %>").value = emptyString;
        }
        
        function clkSave() {
            if (checkInput()) 
            {
                document.getElementById("<%=hidLine.ClientID %>").value = document.getElementById("<%=cmbLine.ClientID %>").value;
                document.getElementById("<%=hidLine2.ClientID %>").value = document.getElementById("<%=cmbLine2.ClientID %>").value;
                document.getElementById("<%=hidStation.ClientID %>").value = document.getElementById("<%=cmbStation.ClientID %>").value;
                document.getElementById("<%=hidCatagory.ClientID %>").value = document.getElementById("<%=cmbCatagory.ClientID %>").value;
                document.getElementById("<%=hidDefectCode.ClientID %>").value = document.getElementById("<%=cmbDefectCode.ClientID %>").value;
                document.getElementById("<%=hidHoldStation.ClientID %>").value = document.getElementById("<%=cmbHoldStation.ClientID %>").value;
                document.getElementById("<%=hidHoldStatus.ClientID %>").value = document.getElementById("<%=cmbHoldStatus.ClientID %>").value;
                return true;
            }
            
            return false;           
        }
        
        function checkInput()
        {
            var Line2 = document.getElementById("<%=cmbLine2.ClientID %>").value;

            if (emptyPattern.test(Line2))
            {
                alert("Please select [Line]!");
                document.getElementById("<%=cmbLine2.ClientID %>").focus();
                return false;
            }

            var Station = document.getElementById("<%=cmbStation.ClientID %>").value;

            if (emptyPattern.test(Station)) {
                alert("Please select [Station]!");
                document.getElementById("<%=cmbStation.ClientID %>").focus();
                return false;
            }

            var Catagory = document.getElementById("<%=cmbCatagory.ClientID %>").value;

            if (emptyPattern.test(Catagory)) {
                alert("Please select [Catagory]!");
                document.getElementById("<%=cmbCatagory.ClientID %>").focus();
                return false;
            }

            var TimeOut = document.getElementById("<%=txtTimeOut.ClientID %>").value;

            if (emptyPattern.test(TimeOut)) {
                alert("Please select [TimeOut]!");
                document.getElementById("<%=txtTimeOut.ClientID %>").focus();
                return false;
            }

            var StopTime = document.getElementById("<%=txtStopTime.ClientID %>").value;

            if (emptyPattern.test(StopTime)) {
                alert("Please select [StopTime]!");
                document.getElementById("<%=txtStopTime.ClientID %>").focus();
                return false;
            }

            var HoldStatus = document.getElementById("<%=cmbHoldStatus.ClientID %>").value;

            if (emptyPattern.test(HoldStatus)) {
                alert("Please select [HoldStatus]!");
                document.getElementById("<%=cmbHoldStatus.ClientID %>").focus();
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
                document.getElementById("<%=hidLine2.ClientID %>").value = row.cells[0].innerText;
                document.getElementById("<%=hidStation.ClientID %>").value = row.cells[1].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
                ShowWait();
                return true;
            }
            return false;
        }
    </script>
</asp:Content>

