﻿<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:ECR Version Maintain
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2010-04-27  Tong.Zhi-Yong         Create     
 * Known issues:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SmallBoardDefine.aspx.cs" Inherits="DataMaintain_SmallBoardDefine" Title="无标题页" %>
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
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbFamilyTop" runat="server" Width="82%"></asp:DropDownList>
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
                        <asp:Label ID="lblEcrVersionList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnFamilyChange" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnLoadFirstDataList" EventName="ServerClick" />
                    </Triggers>
                    
                    <ContentTemplate>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                 <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" GvExtWidth="100%" GvExtHeight="390px"  onrowdatabound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" OnDataBound="gd_DataBound" RowStyle-Height="20">
                 
                 </iMES:GridViewExt>
                 </ContentTemplate>
                 </asp:UpdatePanel>
            </div>
            <div id="div3">
            <table width="100%" border="0" style="table-layout:fixed;" class="edit">
                <colgroup>
	                <col style="width:90px;" />
	                <col />
	                <col style="width:100px;" />
	                <col />
	                <col style="width:110px;" />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbFamily" runat="server" Width="82%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblMBType" runat="server" CssClass="iMes_label_13pt" Text="MBType:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbMBType" runat="server" Width="82%"></asp:DropDownList>
                    </td>
                    <td>
                        <input type="button" id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Add_ServerClick"/>
                    </td>                                        
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt" Text="PartNo:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbPartNo" runat="server" Width="82%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt" Text="Qty:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtQty" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="1"/>
                    </td>
                    <td>
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>
                 
                <tr>
                    <td>
                        <asp:Label ID="lblMaxQty" runat="server" CssClass="iMes_label_13pt" Text="MaxQty:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtMaxQty" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="1"/>
                    </td>
                    <td>
                        <asp:Label ID="lblPriority" runat="server" CssClass="iMes_label_13pt" Text="Priority:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtPriority" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="2"/>
                    </td>   
                    <td></td>                                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt" Text="ECR:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtECR" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="5"/>
                    </td>
                    <td>
                        <asp:Label ID="lblIECVersion" runat="server" CssClass="iMes_label_13pt" Text="IECVer:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtIECVersion" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="4"/>  
                    </td>   
                    <td></td>                                   
                </tr>
            </table>
        </div>
    </div>    
    <input type="hidden" id="hidFamily" runat="server" />
    <input type="hidden" id="hidFamily2" runat="server" />
    <input type="hidden" id="hidDeleteID" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    
    <button id="btnLoadFirstDataList" runat="server" type="button" style="display:none" onserverclick ="btnLoadFirstDataList_ServerClick"> </button>
    <button id="btnFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyChange_ServerClick"> </button>
    <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" Visible="false" >
    </asp:UpdatePanel>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div>
    <script type="text/javascript" language="javascript">
        var customer;
        var editor;
        var emptyPattern = /^\s*$/;
        var qtyPattern = /^[1-9]{1}$/;
        var maxQtyPattern = /^[1-9]{1}$/;
        var PriorityPattern = /^[0-9]{2}$/;
        var ecrPattern = /^[0-9A-Z]{5}$/;
        var iecVersionPattern = /^[0-9]\.[0-9]{2}$/;
        var emptyString = "";
        var selectedRowIndex = -1;
        //error message
        var msgInputFamily = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputFamily").ToString() %>';
        var msgInputMBCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBCode").ToString() %>';
        var msgInputECR = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputECR").ToString() %>';
        var msgInputIECVersion = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputIECVersion").ToString() %>';
        var msgMBCodeFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBCodeFormatError").ToString() %>';
        var msgECRFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgECRFormatError").ToString() %>';
        var msgIECVersionFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgIECVersionFormatError").ToString() %>';
        var msgCustVersionFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCustVersionFormatError").ToString() %>';
        var msgConfirmDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDelete").ToString() %>';
        
        window.onload = function()
        {
            resetTableHeight();
            initContorls();
        };
        
        function initContorls()
        {
            var tempname = document.getElementById("<%=cmbFamilyTop.ClientID%>");
            tempname.onchange=cmbFamilyTopChange;
        }
         function cmbFamilyTopChange()
        {
            clkQuery();
            document.getElementById("<%=btnFamilyChange.ClientID%>").click();
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
        
        function clkQuery()
        {
            var familyValue = document.getElementById("<%=cmbFamilyTop.ClientID %>").value;
            
            if (emptyPattern.test(familyValue))
            {
                alert(msgInputFamily);
                document.getElementById("<%=cmbFamilyTop.ClientID %>").focus();
                return false;
            }
            selectedRowIndex = -1;
            document.getElementById("<%=hidFamily.ClientID %>").value = familyValue;
            clearDetailInfo();
            ShowWait();
            return true;
        }
        
        function setDetailInfo()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var row = tblObj.rows[selectedRowIndex + 1];

            if(row.cells[1].innerText.trim()=="")
            {
                clearDetailInfo();
            }
            else
            {
                document.getElementById("<%=cmbFamily.ClientID %>").value = row.cells[1].innerText;
                document.getElementById("<%=cmbMBType.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=cmbPartNo.ClientID %>").value = row.cells[3].innerText;
                document.getElementById("<%=txtQty.ClientID %>").value = row.cells[4].innerText;
                document.getElementById("<%=txtMaxQty.ClientID %>").value = row.cells[5].innerText;
                document.getElementById("<%=txtPriority.ClientID %>").value = row.cells[6].innerText;
                document.getElementById("<%=txtECR.ClientID %>").value = row.cells[7].innerText;
                document.getElementById("<%=txtIECVersion.ClientID %>").value = row.cells[8].innerText;
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=cmbFamily.ClientID %>").value = "";
            document.getElementById("<%=cmbMBType.ClientID %>").value = "";
            document.getElementById("<%=cmbPartNo.ClientID %>").value = "";
            document.getElementById("<%=txtQty.ClientID %>").value = "";
            document.getElementById("<%=txtMaxQty.ClientID %>").value = "";
            document.getElementById("<%=txtPriority.ClientID %>").value = "";
            document.getElementById("<%=txtECR.ClientID %>").value = "";
            document.getElementById("<%=txtIECVersion.ClientID %>").value = "";
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                var familyValue = document.getElementById("<%=cmbFamily.ClientID %>").value;
                document.getElementById("<%=hidFamily2.ClientID %>").value = familyValue;
                
                if (hasEditData())
                {
                    var tblObj = document.getElementById("<%=gd.ClientID %>");
                    var row = tblObj.rows[selectedRowIndex + 1];
             
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                }
                else
                {
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = emptyString;
                }
                
                selectedRowIndex = -1;
                ShowWait();
                return true;
            }
            return false;           
        }
        
        function checkInput()
        {
            var familyValue = document.getElementById("<%=cmbFamily.ClientID %>").value;
            if (emptyPattern.test(familyValue))
            {
                alert(msgInputFamily);
                document.getElementById("<%=cmbFamily.ClientID %>").focus();
                return false;
            }

            var PartNoValue = document.getElementById("<%=cmbPartNo.ClientID %>").value;
            if (emptyPattern.test(PartNoValue)) {
                alert("Please Select PartNo");
                document.getElementById("<%=cmbPartNo.ClientID %>").focus();
                return false;
            }

            var mbTypeValue = document.getElementById("<%=cmbMBType.ClientID %>").value;
            if (emptyPattern.test(mbTypeValue)) {
                alert("Please Select MBType");
                document.getElementById("<%=cmbMBType.ClientID %>").focus();
                return false;
            }
            
            var qtyObj = document.getElementById("<%=txtQty.ClientID %>");
            if (emptyPattern.test(qtyObj.value))
            {
                alert("Please Input Qty");
                qtyObj.value = emptyString;
                qtyObj.focus();
                return false;
            }

            if (!qtyPattern.test(qtyObj.value))
            {
                alert("Qty 只能輸入1~9整數");
                qtyObj.focus();
                return false;
            }
            
            var maxQtyObj = document.getElementById("<%=txtMaxQty.ClientID %>");
            
            if (emptyPattern.test(maxQtyObj.value))
            {
                alert("Please Input MaxQty");
                maxQtyObj.value = emptyString;
                maxQtyObj.focus();
                return false;
            }
            
            if (!maxQtyPattern.test(maxQtyObj.value))
            {
                alert("MaxQty 只能輸入1~9整數");
                maxQtyObj.focus();
                return false;
            }
            
            var PriorityObj = document.getElementById("<%=txtPriority.ClientID %>");
            
            if (emptyPattern.test(PriorityObj.value))
            {
                alert("Please Input Priority");
                PriorityObj.value = emptyString;
                PriorityObj.focus();
                return false;
            }

            if (isNaN(PriorityObj.value))
            {
                alert("Priority 只能輸入大於0的2位整數");
                PriorityObj.focus();
                return false;
            }
            
            if(PriorityObj.value == "00" || PriorityObj.value=="0")
            {
                alert("Priority 只能輸入大於0的2位整數");
                PriorityObj.focus();
                return false;
            }

            var ecrObj = document.getElementById("<%=txtECR.ClientID %>");

            if (emptyPattern.test(ecrObj.value)) {
                alert(msgInputECR);
                ecrObj.value = emptyString;
                ecrObj.focus();
                return false;
            }

            if (!ecrPattern.test(ecrObj.value.toUpperCase())) {
                alert(msgECRFormatError);
                ecrObj.focus();
                return false;
            }

            var iecVersionObj = document.getElementById("<%=txtIECVersion.ClientID %>");

            if (emptyPattern.test(iecVersionObj.value)) {
                alert(msgInputIECVersion);
                iecVersionObj.value = emptyString;
                iecVersionObj.focus();
                return false;
            }

            if (!iecVersionPattern.test(iecVersionObj.value.toUpperCase())) {
                alert(msgIECVersionFormatError);
                iecVersionObj.focus();
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

