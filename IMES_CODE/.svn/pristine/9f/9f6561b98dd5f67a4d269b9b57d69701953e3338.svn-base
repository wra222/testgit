<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FAIModelMaintain.aspx.cs" Inherits="DataMaintain_FAIModelMaintain" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td width="10%">
                        <asp:Label ID="lblFAIModel" runat="server" CssClass="iMes_label_13pt">FAI Model:</asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtFAIModel" runat="server" Width="96%" MaxLength="20"></asp:TextBox>
                    </td >
                    <td align="right">
                       <input type="button" id="btnQuery" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkQuery())" onserverclick="Query_ServerClick"/>
                    </td> 
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
                        <input type="button" id="btnReOpen" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkReOpen())" onserverclick="ReOpen_ServerClick"/>
                    </td>    
                </tr>
            </table>                                     
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnReOpen" EventName="ServerClick" />
					<asp:AsyncPostBackTrigger ControlID="btnFindFAIModelInfo" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" 
                        HighLightRowPosition="3" 
                        AutoGenerateColumns="true" Width="100%" GvExtWidth="100%" GvExtHeight="100px"  
                        onrowdatabound="gd_RowDataBound" 
                        OnGvExtRowClick="clickTable(this)" 
                        RowStyle-Height="20" 
                        AutoHighlightScrollByValue ="true">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <asp:UpdatePanel ID="updatePane2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="btnFindFAIModelInfo" EventName="ServerClick" />
			</Triggers>
            <ContentTemplate>
			<table width="100%" border="0"  class="iMes_div_MainTainEdit">
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblFAIModelAdd" runat="server" CssClass="iMes_label_13pt">FAI Model:</asp:Label>
                    </td>
                    <td width="15%" align="left">
                        <asp:TextBox ID="txtFAIModelAdd" runat="server" Width="96%" MaxLength="255" onblur="FindFAIModelInfo()" ></asp:TextBox>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblModelType" runat="server" CssClass="iMes_label_13pt">Model Type:</asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblModelTypeContent" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblFamilyContent" runat="server" CssClass="iMes_label_13pt" />
                    </td>
					<td></td>
					<td></td>
                    <td align="right">
                        <input type="button" id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkAdd())" onserverclick="Add_ServerClick"/>
						<button id="btnFindFAIModelInfo" runat="server" onserverclick="FindFAIModelInfo_ServerClick" style="display: none" />
                    </td>
                </tr>  
            </table>
			</ContentTemplate>
			</asp:UpdatePanel>
        </div>
    </div>    
    
    <input type="hidden" id="hidReOpenID" runat="server" />
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
        var emptyPattern = /^\s*$/;
        window.onload = function()
        {
            document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=UserId%>";
            resetTableHeight();
        };
         
        function resetTableHeight()
        {
            //动态调整表格的高度
            /*var adjustValue=60;     
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
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
            */
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
        }

        function AddUpdateComplete(id) {
            /*
			var gdObj = document.getElementById("<%=gd.ClientID %>");
            selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id) {
                    selectedRowIndex = i;
                }
            }
            if (selectedRowIndex < 0) {
                document.getElementById("<%=btnReOpen.ClientID %>").disabled = true;
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                clearDetailInfo();
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            }
			*/
			ShowMessage("SUCCESS");
        }
        var iSelectedRowIndex=null;

        function ReOpenComplete() {
            document.getElementById("<%=btnReOpen.ClientID %>").disabled = true;
			ShowMessage("SUCCESS");
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
            if(""==document.getElementById("<%=txtFAIModel.ClientID %>").value){
				alert('Please input FAI Model');
				return false;
			}
			clearDetailInfo();
            ShowWait();
            return true;
        }
        var updateFlag = "";
        
        function setDetailInfo(con)
        {
            if (con.cells[5].innerText.trim() == "Release" && con.cells[8].innerText.trim() == "Release") {
                document.getElementById("<%=btnReOpen.ClientID %>").disabled = false;
            }
            else {
                document.getElementById("<%=btnReOpen.ClientID %>").disabled = true;
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=btnReOpen.ClientID %>").disabled = true;
        }
        
        function clkAdd()
        {
            if(""==document.getElementById("<%=txtFAIModelAdd.ClientID %>").value){
				alert('Please input FAI Model');
				return false;
			}
			if (checkInput())
            {
                selectedRowIndex = -1;
                ShowWait();
                return true;
            }
            return false;
        }

        function checkInput() {
            return true;
        }
        
        function clkReOpen()
        {
			var tblObj = document.getElementById("<%=gd.ClientID %>");
			var row = tblObj.rows[selectedRowIndex + 1];
			document.getElementById("<%=hidReOpenID.ClientID %>").value = row.cells[0].innerText;
			selectedRowIndex = -1;
			clearDetailInfo();
			ShowWait();
			return true;
        }
		
		function FindFAIModelInfo() {
			document.getElementById("<%=lblModelTypeContent.ClientID %>").innerText = "";
			document.getElementById("<%=lblFamilyContent.ClientID %>").innerText = "";
			document.getElementById("<%=btnFindFAIModelInfo.ClientID %>").click();
		}
		
    </script>
</asp:Content>

