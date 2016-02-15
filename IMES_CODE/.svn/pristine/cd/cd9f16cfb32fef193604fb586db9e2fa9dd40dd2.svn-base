<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FAIModelRule.aspx.cs" Inherits="DataMaintain_FAIModelRule" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td>
<asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
<Triggers>
	<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
	<asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
</Triggers>
<ContentTemplate>
						<asp:DropDownList ID="drpFamilyFromFamilyInfo" runat="server" Height="25px" Width="170px"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
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
                        AutoGenerateColumns="true" Width="100%" GvExtWidth="100%" GvExtHeight="290px"  
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
                    <td>
                        <asp:Label ID="lblFamily2" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFamily" runat="server" Height="25px" Width="170px"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblModelType" runat="server" CssClass="iMes_label_13pt">Model Type:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpModelType" runat="server" Height="25px" Width="170px"></asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMOLimitQty" runat="server" CssClass="iMes_label_13pt">MO Limit Qty:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMOLimitQty" runat="server" Width="96%" MaxLength="255" ></asp:TextBox>
                    </td>
					<td></td>
					<td></td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>  
            </table>
			
        </div>
    </div>    
    
    <input type="hidden" id="hidSelectedID" runat="server" />
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
		
		var msgConfirmDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDelete").ToString() %>';
		var msgNeedFamily = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNeedFamily").ToString() %>';
		var msgNeedModelType = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNeedModelType").ToString() %>';
		var msgNeedMOLimitQty = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNeedMOLimitQty").ToString() %>';
		
		var drpFamilyFromFamilyInfo = document.getElementById("<%=drpFamilyFromFamilyInfo.ClientID %>");
		var drpFamily = document.getElementById("<%=drpFamily.ClientID %>");
		var drpModelType = document.getElementById("<%=drpModelType.ClientID %>");
		var txtMOLimitQty = document.getElementById("<%=txtMOLimitQty.ClientID %>");
		var hidSelectedID = document.getElementById("<%=hidSelectedID.ClientID %>");
        window.onload = function()
        {
            document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=UserId%>";
            resetTableHeight();
        };
         
        function resetTableHeight()
        {
            
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

        var iSelectedRowIndex=null;

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
        
		function QueryComplete() {
			iSelectedRowIndex=null;
			selectedRowIndex = -1;
			clearDetailInfo();
		}
		
        function clkQuery()
        {
            drpFamilyFromFamilyInfo = document.getElementById("<%=drpFamilyFromFamilyInfo.ClientID %>");
			if(""==drpFamilyFromFamilyInfo.options[drpFamilyFromFamilyInfo.selectedIndex].value){
				alert('Please choose Family');
				return false;
			}
			clearDetailInfo();
            ShowWait();
            return true;
        }
        var updateFlag = "";
        
        function setDetailInfo(con)
        {
            if (con.cells[0].innerText.trim() != "") {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
				drpFamily.value = con.cells[1].innerText;
				drpModelType.value = con.cells[2].innerText;
				txtMOLimitQty.value = con.cells[3].innerText;
				hidSelectedID.value = con.cells[0].innerText;
            }
            else {
                clearDetailInfo();
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
			drpFamily.value = "";
			drpModelType.value = "";
			txtMOLimitQty.value = "";
			hidSelectedID.value = "";
        }
        var regNum = /^\d+$/;
        function clkSave()
        {
            if(""==drpFamily.value){
				alert(msgNeedFamily);
				return false;
			}
			if(""==drpModelType.value){
				alert(msgNeedModelType);
				return false;
			}
			if(!regNum.test(txtMOLimitQty.value)){
				alert(msgNeedMOLimitQty);
				return false;
			}
			ShowWait();
			return true;
        }

        function clkDelete()
        {
			if (confirm(msgConfirmDelete)){
				ShowWait();
				return true;
			}
			return false;
        }
		
    </script>
</asp:Content>

