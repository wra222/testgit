
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PalletType.aspx.cs" Inherits="DataMaintain_PalletType" Title="无标题页" %>
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
                        <asp:Label ID="lblShipWay" runat="server" CssClass="iMes_label_13pt" Text="ShipWay:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbShipWay" runat="server" Width="100px"></asp:DropDownList>
                    </td>
                    <td width="100px">
                        <asp:Label ID="lblRegId" runat="server" CssClass="iMes_label_13pt" Text="RegId:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbRegId" runat="server" Width="100px"></asp:DropDownList>
                    </td>
                    <td>
                        <input type="button" id="btnQuery" class="iMes_button" value="Query" runat="server" onclick="if(clkQuery())" onserverclick="btnQuery_ServerClick"/>
                    </td>
                </tr> 
            </table>
            
         </div>   
         <div id="div1" class="iMes_div_MainTainDiv1">
               <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblPlletTypeforICC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                        <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
                    </td>    
                </tr>
             </table>                                     
        </div>
            <div id="div2">
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
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
                    <td width="80px">
                        <asp:Label ID="lblShipWay2" runat="server" CssClass="iMes_label_13pt" Text="ShipWay:"></asp:Label>
                    </td>
                    <td colspan="2" align="right">
                        <asp:DropDownList ID="cmbShipWay2" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblMinQty" runat="server" CssClass="iMes_label_13pt" Text="MinQty:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtMinQty" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="4" 
                        onkeypress="TextBoxNumCheck_Int();"/>
                    </td>
                    <td>
                        <asp:Label ID="lblWeight" runat="server" CssClass="iMes_label_13pt" Text="Weight:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtWeight" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="10" 
                        onKeyPress="if((event.keyCode<48 || event.keyCode>57) && event.keyCode!=46 || /\.\d\d$/.test(value))event.returnValue=false" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2">
                    <asp:UpdatePanel ID="updatePanel3" runat="server" >
                        <ContentTemplate>
                            <asp:RadioButton ID="ByRegId" runat="server" Checked="true" Text="ByRegId" GroupName="Type" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                            <asp:RadioButton ID="ByCarrier" runat="server" Text="ByCarrier" GroupName="Type" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                            <button id="btnRadioChange" runat="server" type="button" style="display:none" onserverclick ="btn_ServerClick"> </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                    <td align="right">
                        <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbRegId_Carrier" runat="server" Width="100%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblMaxQty" runat="server" CssClass="iMes_label_13pt" Text="MaxQty:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtMaxQty" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="4" onkeypress="TextBoxNumCheck_Int();"/>
                    </td>
                    <td>
                        <asp:Label ID="lblInPltWeight" runat="server" CssClass="iMes_label_13pt" Text="MinusPltWeight:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbInPltWeight" runat="server" Width="100px">
                            <asp:ListItem Text="不扣棧板重" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="扣棧板重" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStdFullQty" runat="server" CssClass="iMes_label_13pt" Text="StdFullQty:"></asp:Label>
                    </td>
                    <td  colspan="2" align="right">
                        <input type="text" id="txtStdFullQty" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="4" onkeypress="TextBoxNumCheck_Int();"/>
                    </td>
                    <td>
                        <asp:Label ID="lblPalletType" runat="server" CssClass="iMes_label_13pt" Text="PalletType:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbPalletType" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCheckCode" runat="server" CssClass="iMes_label_13pt" Text="CheckCode:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtCheckCode" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="8" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPalletLayer" runat="server" CssClass="iMes_label_13pt" Text="PalletLayer:"></asp:Label>
                    </td>
                    <td  colspan="2" align="right">
                        <input type="text" id="txtPalletLayer" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="2" 
                        onkeypress="TextBoxNumCheck_Int();"/>
                    </td>
                    <td>
                        <asp:Label ID="lblPalletCode" runat="server" CssClass="iMes_label_13pt" Text="PalletCode:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbPalletCode" runat="server" Width="100px"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblChepPallet" runat="server" CssClass="iMes_label_13pt" Text="ChepPallet:"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="cmbChepPallet" runat="server" Width="100%">
                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
				<tr>
                    <td>
						<asp:Label ID="lblOceanType" runat="server" CssClass="iMes_label_13pt" Text="OceanType:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbOceanType" runat="server" Width="100%"></asp:DropDownList>
					</td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
                    <td>
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
				</tr>
            </table>
        </div>
    </div>    
    
    
    
    <input type="hidden" id="hidShipWay" runat="server" />
    <input type="hidden" id="hidShipWay2" runat="server" />
    <input type="hidden" id="hidRegid" runat="server" />
    <input type="hidden" id="hidRegId_Carrier" runat="server" />
    <input type="hidden" id="hidPalletType" runat="server" />
	<input type="hidden" id="hidOceanType" runat="server" />
    <input type="hidden" id="hidPalletCode" runat="server" />
    <input type="hidden" id="hidInPltWeight" runat="server" />
    <input type="hidden" id="hidChepPallet" runat="server" />
    <input type="hidden" id="hidCheckCode" runat="server" />
    <input type="hidden" id="hidID" runat="server" />
    
    <input type="hidden" id="hidDeleteMBCode" runat="server" />
    <input type="hidden" id="hidDeletePCB" runat="server" />
    
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
        var mbCodePattern = /^[0-9A-Z]{2}$/;
        var PCBPattern = /^[0-9A-Z]{3}$/;
        var CTVerPattern = /^[0-9A-Z]{2}$/;
        var SupplierPattern = /^[0-9A-Z]{2}$/;
        var emptyString = "";
        var selectedRowIndex = -1;
        
        //error message
        var msgInputFamily = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputFamily").ToString() %>';
        var msgInputMBCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBCode").ToString() %>';
        var msgInputPCB = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPCB").ToString() %>';
        var msgInputCTVer = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputCTVer").ToString() %>';
        var msgInputSupplier = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSupplier").ToString() %>';
        var msgMBCodeFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBCodeFormatError").ToString() %>';
        var msgPCBFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPCBFormatError").ToString() %>';
        var msgCTVerFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCTVerFormatError").ToString() %>';
        var msgSupplierFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSupplierFormatError").ToString() %>';
        var msgConfirmDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDelete").ToString() %>';
        
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
            selectedRowIndex = parseInt(con.index, 10);
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var row = tblObj.rows[selectedRowIndex + 1];
            
            document.getElementById("<%=hidRegId_Carrier.ClientID %>").value = row.cells[1].innerText;
            document.getElementById("<%=btnRadioChange.ClientID %>").click();
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
            
            var ShipWayValue = document.getElementById("<%=cmbShipWay.ClientID %>").value;
            var RegIdValue = document.getElementById("<%=cmbRegId.ClientID %>").value;
            if (ShipWayValue == "")
            {
                alert(msgInputFamily);
                
                document.getElementById("<%=cmbShipWay.ClientID %>").focus();
                return false;
            }
            if (RegIdValue == "") 
            {
                alert(msgInputFamily);

                document.getElementById("<%=cmbRegId.ClientID %>").focus();
                return false;
            }
            selectedRowIndex = -1;
            document.getElementById("<%=hidShipWay.ClientID %>").value = ShipWayValue;
            document.getElementById("<%=hidRegid.ClientID %>").value = RegIdValue;
			document.getElementById("<%=hidID.ClientID %>").value = "";
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
				
				document.getElementById("<%=hidID.ClientID %>").value = "";
            }
            else
            {
                document.getElementById("<%=cmbShipWay2.ClientID %>").value = row.cells[0].innerText;
                document.getElementById("<%=cmbRegId_Carrier.ClientID %>").value = row.cells[1].innerText;
                document.getElementById("<%=txtStdFullQty.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=txtPalletLayer.ClientID %>").value = row.cells[3].innerText;
                
                
                document.getElementById("<%=txtMinQty.ClientID %>").value = row.cells[4].innerText;
                document.getElementById("<%=txtMaxQty.ClientID %>").value = row.cells[5].innerText;
                document.getElementById("<%=cmbPalletType.ClientID %>").value = row.cells[6].innerText;
				document.getElementById("<%=cmbOceanType.ClientID %>").value = row.cells[7].innerText;
                document.getElementById("<%=cmbPalletCode.ClientID %>").value = row.cells[8].innerText;
                document.getElementById("<%=txtWeight.ClientID %>").value = row.cells[9].innerText;
                document.getElementById("<%=cmbInPltWeight.ClientID %>").value = row.cells[10].innerText;
                document.getElementById("<%=txtCheckCode.ClientID %>").value = row.cells[11].innerText;
                document.getElementById("<%=cmbChepPallet.ClientID %>").value = row.cells[12].innerText;
				
				document.getElementById("<%=hidID.ClientID %>").value = row.cells[16].innerText;
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=cmbShipWay2.ClientID %>").value = "";
            document.getElementById("<%=txtStdFullQty.ClientID %>").value = emptyString;
            document.getElementById("<%=cmbPalletCode.ClientID %>").value = "";
            document.getElementById("<%=ByRegId.ClientID %>").checked = true;
            document.getElementById("<%=txtMinQty.ClientID %>").value = emptyString;
            document.getElementById("<%=txtWeight.ClientID %>").value = emptyString;
            document.getElementById("<%=cmbPalletType.ClientID %>").value = "";
			document.getElementById("<%=cmbOceanType.ClientID %>").value = "";
            document.getElementById("<%=txtMaxQty.ClientID %>").value = emptyString;
            document.getElementById("<%=cmbInPltWeight.ClientID %>").value = "";
            document.getElementById("<%=cmbChepPallet.ClientID %>").value = "";
            document.getElementById("<%=txtCheckCode.ClientID %>").value = emptyString;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=txtPalletLayer.ClientID %>").value = 0;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                document.getElementById("<%=hidShipWay2.ClientID %>").value = document.getElementById("<%=cmbShipWay2.ClientID %>").value;
                document.getElementById("<%=hidRegId_Carrier.ClientID %>").value = document.getElementById("<%=cmbRegId_Carrier.ClientID %>").value;
                document.getElementById("<%=hidPalletType.ClientID %>").value = document.getElementById("<%=cmbPalletType.ClientID %>").value;
				document.getElementById("<%=hidOceanType.ClientID %>").value = document.getElementById("<%=cmbOceanType.ClientID %>").value;
                document.getElementById("<%=hidPalletCode.ClientID %>").value = document.getElementById("<%=cmbPalletCode.ClientID %>").value;
                document.getElementById("<%=hidInPltWeight.ClientID %>").value = document.getElementById("<%=cmbInPltWeight.ClientID %>").value;
                document.getElementById("<%=hidChepPallet.ClientID %>").value = document.getElementById("<%=cmbChepPallet.ClientID %>").value;
                document.getElementById("<%=hidCheckCode.ClientID %>").value = document.getElementById("<%=txtCheckCode.ClientID %>").value;
                selectedRowIndex = -1;
                
                ShowWait();
                return true;
            }
            
            return false;           
        }
        
        function checkInput()
        {
            var ShipWayValue = document.getElementById("<%=cmbShipWay2.ClientID %>").value;
            
            if (ShipWayValue == "") {
                alert("Please select [ShipWay]!");
                document.getElementById("<%=cmbShipWay.ClientID %>").focus();
                return false;
            }

            var RegIdValue = document.getElementById("<%=cmbRegId_Carrier.ClientID %>").value;

            if (ShipWayValue == "") {
                alert("Please select [RegId]!");
                document.getElementById("<%=cmbRegId_Carrier.ClientID %>").focus();
                return false;
            }

            var StdFullQty = document.getElementById("<%=txtStdFullQty.ClientID %>").value;

            if (StdFullQty == "") {
                alert("Please input [StdFullQty]!");
                document.getElementById("<%=txtStdFullQty.ClientID %>").focus();
                return false;
            }

            var PalletLayer = document.getElementById("<%=txtPalletLayer.ClientID %>").value;
            if (PalletLayer == "") {
                alert("Please input [PalletLayer]!");
                document.getElementById("<%=txtPalletLayer.ClientID %>").focus();
                return false;
            }

            var PalletCode = document.getElementById("<%=cmbPalletCode.ClientID %>").value;

            if (PalletCode == "") {
                alert("Please select [PalletCode]!");
                document.getElementById("<%=cmbPalletCode.ClientID %>").focus();
                return false;
            }

            var MinQty = document.getElementById("<%=txtMinQty.ClientID %>").value;

            if (MinQty == "") {
                alert("Please input [MinQty]!");
                document.getElementById("<%=txtMinQty.ClientID %>").focus();
                return false;
            }

            var MaxQty = document.getElementById("<%=txtMaxQty.ClientID %>").value;

            if (MaxQty == "") {
                alert("Please input [MaxQty]!");
                document.getElementById("<%=txtMaxQty.ClientID %>").focus();
                return false;
            }
            
            if ((parseInt(MinQty) > parseInt(MaxQty))) {
                alert("[MinQty] must be less than [MaxQty]!");
                document.getElementById("<%=txtMinQty.ClientID %>").focus();
                return false;
            }

            var Weight = document.getElementById("<%=txtWeight.ClientID %>").value;

            if (Weight == "") {
                alert("Please input [Weight]!");
                document.getElementById("<%=txtWeight.ClientID %>").focus();
                return false;
            }

            var PalletType = document.getElementById("<%=cmbPalletType.ClientID %>").value;

            if (PalletType == "") {
                alert("Please select [PalletType]!");
                document.getElementById("<%=cmbPalletType.ClientID %>").focus();
                return false;
            }

            var InPltWeight = document.getElementById("<%=cmbInPltWeight.ClientID %>").value;

            if (InPltWeight == "") {
                alert("Please select [InPltWeight]!");
                document.getElementById("<%=cmbInPltWeight.ClientID %>").focus();
                return false;
            }
            return true;      
        }
        
        function clkDelete()
        {
            if (confirm(msgConfirmDelete)) 
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var row = tblObj.rows[selectedRowIndex + 1];
                document.getElementById("<%=hidID.ClientID %>").value = row.cells[16].innerText;
                document.getElementById("<%=hidShipWay2.ClientID %>").value = row.cells[0].innerText;
                document.getElementById("<%=hidRegId_Carrier.ClientID %>").value = row.cells[1].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
                ShowWait();
                return true;
            }
            
            return false;
        }

        function TextBoxNumCheck_Int() {
            if (event.keyCode < 48 || window.event.keyCode > 57) event.returnValue = false;
        }
    </script>
</asp:Content>

