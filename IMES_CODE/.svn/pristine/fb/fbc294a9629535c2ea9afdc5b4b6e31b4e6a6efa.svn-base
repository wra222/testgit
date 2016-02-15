<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  98079                Create 
 Known issues:
 qa bug no:ITC-1136-0118,ITC-1136-0161
 --%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelMaintain.aspx.cs" Inherits="ModelMaintain" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<style>
    table.edit
    {
        border: thin solid Black; 
        background-color: #99CDFF;
    }
     .hiddencol
    {
        display:none;
    }
</style>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintainEx/service/ModelMaintainService.asmx"/>
        </Services>
    </asp:ScriptManager>

    <center>
    <table style="width:100%">
        <tr>
            <td>
                <table style="width:100%" border=0 class="edit">
                    <tr>
                        <td width=6%>
                            <asp:Label ID="lblQueryFirstModelName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width=42%>
                            <asp:TextBox id="queryFirstModelName" runat="server" MaxLength="50" Width="70%" onkeypress="OnKeyPress(this)"></asp:TextBox>
                            <button id="btnModelNameChange" runat="server" type="button" onclick="" style="display: none" onserverclick="btnModelNameChange_Click"></button>
                        </td>
                        <td width=6%>
                            <asp:Label ID="lblCustomerTop" runat="server" CssClass="iMes_label_13pt" Text="Customer:"></asp:Label>
                        </td>
                        <td width=9%>
                            <asp:DropDownList ID="cmbCustomerTop" runat="server" Width="100%"></asp:DropDownList>        
                        </td>
                        <td width=6%>
                            <asp:Label ID="lblFamilyTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <%--<iMESMaintainEx:CmbFamilyForMaintain  ID="CmbFamily" runat="server" Width="97" IsPercentage="true"/>--%>
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbFamilyTop" runat="server" Width="97%"></asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                    
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table border=0 style="width:100%">
                    <tr>
                        <td width=8%>
                            <asp:Label ID="lblModelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width=42%>
                            <asp:TextBox id="querySecondModelName" runat="server" Width="70%" style="ime-mode:disabled" onkeypress="inputNumberAndEnglishChar(this)"></asp:TextBox>
                        </td>
                        <td align=right>
                            <input type="hidden" id="hidModelName" runat="server" value=""/>
                            <input type="hidden" id="hidBOMApproved" runat="server" value=""/>
                            <input type="hidden" id="hidFamily" runat="server" value=""/>
                            <button id="btnSaveAs" runat="server" onclick="clkButton()" class="iMes_button">SaveAs</button>
                            <input id="btnDelete" disabled type="button"  runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDelete_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <input id="btnInfoName" type="button"  runat="server" class="iMes_button" onclick="onInfoName();" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <input id="btnInfo" disabled type="button"  runat="server" class="iMes_button" onclick="onInfo('');" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan=3>
                            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshModelList" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnGetFamilyTopList" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnGetFamilyList" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnModelNameChange" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnGetCustomer" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>

                                <!-- gdModelList -->
                                
                                <iMES:GridViewExt   ID="gdModelList" runat="server" AutoGenerateColumns="False" 
                        GvExtHeight="370px" AutoHighlightScrollByValue="True" 
                        GetTemplateValueEnable="False"      Width="100%" GvExtWidth="98%" HiddenColCount="0" 
                        HighLightRowPosition="1" SetTemplateValueEnable="False" 
                        BorderStyle="Solid" 
                CssClass="iMes_grid_TableGvExt" meta:resourcekey="grvResource1" 
                OnRowDataBound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" OnGvExtRowDblClick="dblClickTable(this)">
                           <AlternatingRowStyle CssClass="iMes_grid_AlternatingRowGvExt" />
                           <Columns>
                               <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                   <HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="center" />
                                   <HeaderTemplate>
                                       <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);" runat="server" /> All
                                   </HeaderTemplate>
                                   <ItemTemplate>
                                       <asp:CheckBox ID="chbx" runat="server" meta:resourcekey="chbxResource1" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="Model"          HeaderText="Model" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource1"><HeaderStyle Width="110px" /></asp:BoundField>         
<asp:BoundField DataField="CustPN"         HeaderText="CustPN" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource2"><HeaderStyle Width="110px" /></asp:BoundField>        
<asp:BoundField DataField="Region"         HeaderText="Region" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource3"><HeaderStyle Width="110px" /></asp:BoundField>        
<asp:BoundField DataField="ShipType"       HeaderText="ShipType" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource4"><HeaderStyle Width="110px" /></asp:BoundField>      
<asp:BoundField DataField="Status"         HeaderText="Status" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource5"><HeaderStyle Width="110px" /></asp:BoundField>        
<asp:BoundField DataField="OsCode"         HeaderText="OsCode" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource6"><HeaderStyle Width="110px" /></asp:BoundField>        
<asp:BoundField DataField="BomApproveDate" HeaderText="BomApproveDate" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource7"><HeaderStyle Width="110px" /></asp:BoundField>
<asp:BoundField DataField="Editor"         HeaderText="Editor" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource8"><HeaderStyle Width="110px" /></asp:BoundField>        
<asp:BoundField DataField="Cdt"            HeaderText="Cdt" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource9"><HeaderStyle Width="110px" /></asp:BoundField>           
<asp:BoundField DataField="Udt"            HeaderText="Udt" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource10" ItemStyle-cssclass="hiddencol" HeaderStyle-CssClass="hiddencol"><HeaderStyle Width="110px" /></asp:BoundField>           
<asp:BoundField DataField="statusId"       HeaderText="statusId" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource11" ItemStyle-cssclass="hiddencol" HeaderStyle-CssClass="hiddencol"><HeaderStyle Width="110px" /></asp:BoundField>      
<asp:BoundField DataField="OSDesc"         HeaderText="OSDesc" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource12" ItemStyle-cssclass="hiddencol" HeaderStyle-CssClass="hiddencol"><HeaderStyle Width="110px" /></asp:BoundField>        
<asp:BoundField DataField="Family"         HeaderText="Family" HeaderStyle-HorizontalAlign="left" meta:resourcekey="BoundFieldResource13" ItemStyle-cssclass="hiddencol" HeaderStyle-CssClass="hiddencol"><HeaderStyle Width="110px" /></asp:BoundField>        

                           </Columns>
                           <HeaderStyle CssClass="iMes_grid_HeaderRowGvExt" />
          </iMES:GridViewExt>
                                
                            </ContentTemplate>   
                            </asp:UpdatePanel>                         
                            <button id="btnRefreshModelList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnRefreshModelList_Click"></button>
                            <button id="btnGetFamilyTopList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnGetFamilyTopList_Click"></button>
                            <button id="btnGetFamilyList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnGetFamilyList_Click"></button>
                            <button id="btnGetCustomer" runat="server" type="button" onclick="" style="display: none" onserverclick="btnGetCustomer_Click"></button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width=100% border=0 class="edit">
                    <tr>
                        <td>
                            <table width=100% border=0 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td><asp:TextBox id="txtModel" MaxLength=20 runat="server" style="ime-mode:disabled" onkeypress="inputNumberAndEnglishChar(this)"></asp:TextBox></td>                        
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><asp:Label ID="lblCustPN" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td colspan=2><asp:TextBox id="txtCustPN" MaxLength=80 runat="server" SkinId="textBoxSkin"></asp:TextBox>
                                       </td>
                                       <td>  </td>
                                       <td>  </td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt" Text="Customer"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="cmbCustomer" runat="server" Width="140"></asp:DropDownList>
                                    </td>                        
                                    <td></td>
                                    <td><asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Text="Family"></asp:Label></td>
                                    <td>
                                        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" >
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbFamily" runat="server" Width="140"></asp:DropDownList>
                                                
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                               </tr>
                                
                                <tr>
                                    <td><asp:Label ID="lblRegion" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="CmbRegion" runat="server" Width="140"></asp:DropDownList>
                                    </td>                        
                                    <td></td>
                                    <td><asp:Label ID="lblShipType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="CmbShipType" runat="server" Width="140"></asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td><asp:DropDownList id="selStatus"  runat="server"></asp:DropDownList></td>
                               </tr>
                                <tr>
                                    <td><asp:Label ID="lblOSCode" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="cmbOSCode" runat="server" Width="140"></asp:DropDownList>
                                    </td>                        
                                    <td></td>
                                    <td><asp:Label ID="lblOSDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td colspan=4>
                                        <asp:Label ID="lblOSDescContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>   
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align=center>
                            <input id="btnAdd" type="button"  runat="server" onclick="if(clkSave())" class="iMes_button" onserverclick="btnAdd_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <br>
                            <br>
                            <input id="btnSave" disabled type="button" onclick="if(clkSave())" runat="server" class="iMes_button" onserverclick="btnSave_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <input type="hidden" id="hidDescr" runat="server"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </center> 
</div>

<script for=window event=onload>

    var ObjFamily =document.getElementById("<%=cmbFamilyTop.ClientID%>");
    ObjFamily.onchange = GetModelList;
    GetModelList();
    
    document.getElementById("<%=cmbOSCode.ClientID%>").onchange = OSCodeSelectOnChange;
    
    var objcustomer = document.getElementById("<%=cmbCustomerTop.ClientID%>");
        objcustomer.onchange = GetFamilyTopList;
        var objcustomer2 = document.getElementById("<%=cmbCustomer.ClientID%>");
        objcustomer2.onchange = GetFamilyList;
</script>
<script type="text/javascript">
    var selectedRowIndex = null;
    var ObjRegion, ObjShipType;
    //error message
    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';
    var msgAddSave = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAddSave").ToString() %>';
    var msgAddSave_Region = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAddSave_Region").ToString() %>';


     // ----------------------------------------------------------------------------
     // Method to process key up event for queryFirstModelName_OnKeyPress.
     // @author liu xiao-ling
     // ----------------------------------------------------------------------------
     function queryFirstModelName_OnKeyPress() {
/*
         var queryFirstModelName = document.getElementById("<%=queryFirstModelName.ClientID%>").value;
         if (event.keyCode == 13 && queryFirstModelName.length != 0) {

             //beginWaitingCoverDiv();


             ModelMaintainService.GetFamilyInfoByModel(queryFirstModelName, getModelSucc, getModelFail);

         }*/
         return;
     }    

    function setFamilyTop()
    {
        var ObjFamily = document.getElementById("<%=cmbFamilyTop.ClientID%>");
        ObjFamily.onchange = GetModelList;
    }

    function getModelSucc(result){
        ObjFamily = getFamilyCmbObj();
        
        ObjFamily.value = result;
        GetModelList();
    };

    function getModelFail(result){
        //ShowMessage(result.get_message());
        alert(result.get_message());
    }
    
    function OSCodeSelectOnChange() {
        var value = document.getElementById("<%=cmbOSCode.ClientID %>")[document.getElementById("<%=cmbOSCode.ClientID %>").selectedIndex].value;
        document.getElementById("<%=hidDescr.ClientID %>").value = value;
        setInputOrSpanValue(document.getElementById("<%=lblOSDescContent.ClientID %>"), value);
    }
    
     // ----------------------------------------------------------------------------
     // Method to process key up event for querySecondModelName_OnKeyPress.
     // @author liu xiao-ling
     // ----------------------------------------------------------------------------
     function querySecondModelName_OnKeyPress() {

         var querySecondModelName = document.getElementById("<%=querySecondModelName.ClientID%>").value;
         if (event.keyCode == 13 && querySecondModelName.length != 0) {

             //beginWaitingCoverDiv();

             ObjFamily = getFamilyCmbObj();

             ModelMaintainService.GetFirstMatchedModelNameByModel(ObjFamily.value, querySecondModelName, getMatchedModelSucc, getMatchedModelFail);

         }
         return;
     }     
     
    function getMatchedModelSucc(result){
            var gdModelListClientID="<%=gdModelList.ClientID%>";
                  
            
            row=eval("setScrollTopForGvExt_"+gdModelListClientID+"('"+result+"',1)");

            clickTable(row, "fromQuery");
            
    };

    function getMatchedModelFail(result){
        //ShowMessage(result.get_message());
        alert(result.get_message());
    }
     
     
    
   function clkDelete()
   {
       if(confirm(msgDelete))
       {
           ShowInfo("");
           return true;
       }   
       return false;
        
   }


   function clkSave()
   {
       ShowInfo("");
       var model = document.getElementById("<%=txtModel.ClientID %>").value;   
       if(model.trim()=="")
       {
           //ShowMessage(msgAddSave);
           alert(msgAddSave);
           return false;
       }   
       
       var family = document.getElementById("<%=cmbFamily.ClientID%>").options[document.getElementById("<%=cmbFamily.ClientID%>").selectedIndex].value.trim();  
       if(family.trim()=="")
       {
           //ShowMessage(msgAddSave);
           alert("Please Select Family");
           return false;
       }   
       
       return true;
        
   }

    function inputNumberAndDot(con)
    {
	    if (getSelectionText() != "") 
	    {
		    document.selection.clear();
	    }
        var inputContent = con.value;
        var pattern = /^[0-9.]*$/;
        var content = inputContent + String.fromCharCode(event.keyCode);
        
        if (pattern.test(content)) {
            event.returnValue = true;
        } else {
            event.returnValue = false;
        }
    }


    function GetModelList(){
        document.getElementById("<%=querySecondModelName.ClientID%>").value = "";
    
        document.getElementById("<%=btnRefreshModelList.ClientID%>").click();
    }
    
    function GetFamilyTopList()
    {
        document.getElementById("<%=btnGetFamilyTopList.ClientID%>").click();
    }
    
    function GetFamilyList()
    {
        document.getElementById("<%=btnGetFamilyList.ClientID%>").click();
    }

    function doRefreshModelListComplete() {
        var strQueryFirstModelName = document.getElementById("<%=queryFirstModelName.ClientID%>").value;
        strQueryFirstModelName = strQueryFirstModelName.toUpperCase();
        if (strQueryFirstModelName.length != 0) {
            var gdModelListClientID = "<%=gdModelList.ClientID%>";


            row = eval("setScrollTopForGvExt_" + gdModelListClientID + "('" + strQueryFirstModelName + "',1)");
            //row = null;

            //selectedRowIndex = -1;
            clickTable(row, "fromQuery");
        }
    }
    
    function RefreshModelListComplete(){
        doRefreshModelListComplete();
    }

    function RefreshModelListForModelNameChangeComplete() {
        doRefreshModelListComplete();
    }

    function AutoFocusFamily(v) {
         
        document.getElementById("<%=hidFamily.ClientID%>").value = v;
        document.getElementById("<%=btnGetCustomer.ClientID%>").click();
    
    
        var f = document.getElementById("<%=cmbFamilyTop.ClientID%>"); 
        if(f!=null)
        for (var i = 0; i < f.options.length; i++) {
            if (f.options[i].value == v) {
                f.options[i].selected = true;
                break;
            }
        }
    }
    
    function changeselect(customer)
    {
        document.getElementById("<%=cmbCustomer.ClientID%>").value = customer;
        document.getElementById("<%=cmbFamily.ClientID%>").value = document.getElementById("<%=hidFamily.ClientID%>").value;
    }
    
    function clickTable(row, from)
    {
        if(row == null)
        {
            document.getElementById("<%=selStatus.ClientID%>").value = "1";//"first item:normal"
            document.getElementById("<%=txtModel.ClientID%>").value = "";
            document.getElementById("<%=txtCustPN.ClientID%>").value = "";
            document.getElementById("<%=cmbOSCode.ClientID%>").value = "";
            document.getElementById("<%=lblOSDescContent.ClientID%>").value = "";
            document.getElementById("<%=hidBOMApproved.ClientID%>").value = "";

            document.getElementById("<%=hidModelName.ClientID%>").value = "";
            document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
            if (''==GetCheckedModel())
                document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
            else
                document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            document.getElementById("<%=btnSaveAs.ClientID%>").disabled = true;

            if('fromQuery'!=from)
                AutoFocusFamily('');
            return;
        }
        
        if(from == "fromQuery")
        {
//            setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gdModelList.ClientID %>");                
        
            selectedRowIndex = row.rowIndex - 1;
        }else{
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(row.index, 10)))
            {
                if(selectedRowIndex > (row.parentNode.rows.length-1))
                    selectedRowIndex = row.parentNode.rows.length-2;
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gdModelList.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(row.index, true, "<%=gdModelList.ClientID %>");
            selectedRowIndex = parseInt(row.index, 10);
        }
        //selectedRowIndex = parseInt(con.index, 10);


        //SetSelectedProcess(row.cells[1].innerText.trim());
        document.getElementById("<%=txtModel.ClientID%>").value = row.cells[<%=indexField["Model"]%>].innerText.trim();
        document.getElementById("<%=txtCustPN.ClientID%>").value = row.cells[<%=indexField["CustPN"]%>].innerText.trim();
        document.getElementById("<%=cmbOSCode.ClientID%>").value = "For "+row.cells[<%=indexField["OsCode"]%>].innerText.trim();
        document.getElementById("<%=lblOSDescContent.ClientID%>").value = row.cells[<%=indexField["OSDesc"]%>].innerText.trim();
        document.getElementById("<%=hidBOMApproved.ClientID%>").value = row.cells[<%=indexField["BomApproveDate"]%>].innerText.trim();
        AutoFocusFamily(row.cells[<%=indexField["Family"]%>].innerText.trim());

        //model name
        if(row.cells[<%=indexField["Model"]%>].innerText.trim() == ""){
            document.getElementById("<%=selStatus.ClientID%>").value = "1";//"first item:normal"
            document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
            if (''==GetCheckedModel())
                document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
            else
                document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            document.getElementById("<%=btnSaveAs.ClientID%>").disabled = true;
            document.getElementById("<%=hidModelName.ClientID%>").value = "";
        }else{
            document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=btnDelete.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            document.getElementById("<%=btnSaveAs.ClientID%>").disabled = false;
            document.getElementById("<%=hidModelName.ClientID%>").value = row.cells[<%=indexField["Model"]%>].innerText.trim();
            document.getElementById("<%=selStatus.ClientID%>").value = row.cells[<%=indexField["statusId"]%>].innerText.trim();//"0"
            ObjShipType = document.getElementById("<%=CmbShipType.ClientID%>");
            ObjShipType.value = row.cells[<%=indexField["ShipType"]%>].innerText.trim();

            ObjRegion = document.getElementById("<%=CmbRegion.ClientID%>");
            ObjRegion.value = row.cells[<%=indexField["Region"]%>].innerText.trim();//"FRU"
        }
        
    }
    
    function dblClickTable(row) {
        onInfo(document.getElementById("<%=hidModelName.ClientID%>").value);
    }
    
    
    function onInfo(m){
        if(''==m){
            m=GetCheckedModel();
        }
        if(''==m){
            m=document.getElementById("<%=hidModelName.ClientID%>").value;
        }
        if(''==m){
            return;
        }
        var ret = window.showModalDialog("ModelInfoMaintain.aspx?modelname=" + m + "&editor=<%=Master.userInfo.UserId%>", 0, "dialogwidth:1000px; dialogheight:560px; status:no;help:no;");
    }

    function onInfoName(){
        var ret = window.showModalDialog("ModelInfoNameMaintain.aspx?editor=<%=Master.userInfo.UserId%>", 0, "dialogwidth:1000px; dialogheight:560px; status:no;help:no;");
    
    }
    

    function clearInputs(){
         
        document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
        document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
        document.getElementById("<%=btnSave.ClientID%>").disabled = true;
        document.getElementById("<%=btnSaveAs.ClientID%>").disabled = true;
    }
    
     function clkButton()
    {
        var dlgFeature = "dialogHeight:200px;dialogWidth:400px;center:yes;status:no;help:no";
        var oldPartNoValue=document.getElementById("<%=hidModelName.ClientID%>").value.trim().toUpperCase();
        if(oldPartNoValue!="")
        {
            oldPartNoValue=encodeURI(Encode_URL2(oldPartNoValue));
            var user="<%=Master.userInfo.UserId%>";
            user=encodeURI(Encode_URL2(user));
            var dlgReturn=window.showModalDialog("MaintainSaveAs.aspx?OldPartNo="+oldPartNoValue+"&userName="+user, window, dlgFeature);

        }
        else
        {
            alert(msg2);
        }
    }
    
    function SaveComplete(id){
        
        var gdModelListClientID="<%=gdModelList.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdModelListClientID+"('"+id+"',1,'','MUTISELECT')");
        
        if(row != null)
        {
        
            selectedRowIndex = row.rowIndex - 1;

            document.getElementById("<%=txtModel.ClientID%>").value = row.cells[<%=indexField["Model"]%>].innerText.trim();
            document.getElementById("<%=txtCustPN.ClientID%>").value = row.cells[<%=indexField["CustPN"]%>].innerText.trim();
            document.getElementById("<%=cmbOSCode.ClientID%>").value = row.cells[<%=indexField["OsCode"]%>].innerText.trim();
            document.getElementById("<%=lblOSDescContent.ClientID%>").value = row.cells[<%=indexField["OSDesc"]%>].innerText.trim();
            document.getElementById("<%=hidBOMApproved.ClientID%>").value = row.cells[<%=indexField["BomApproveDate"]%>].innerText.trim();


            document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=btnDelete.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            document.getElementById("<%=btnSaveAs.ClientID%>").disabled = false;
            document.getElementById("<%=hidModelName.ClientID%>").value = row.cells[<%=indexField["Model"]%>].innerText.trim();
            document.getElementById("<%=selStatus.ClientID%>").value = row.cells[<%=indexField["statusId"]%>].innerText.trim();//"0"
            ObjShipType = document.getElementById("<%=CmbShipType.ClientID%>");
            ObjShipType.value = row.cells[<%=indexField["ShipType"]%>].innerText.trim();

            ObjRegion = document.getElementById("<%=CmbRegion.ClientID%>");
            ObjRegion.value = row.cells[<%=indexField["Region"]%>].innerText.trim();//"FRU"
             
        }
        else{
            document.getElementById("<%=selStatus.ClientID%>").value = "1";//"first item:normal"
            document.getElementById("<%=txtModel.ClientID%>").value = "";
            document.getElementById("<%=txtCustPN.ClientID%>").value = "";
            document.getElementById("<%=cmbOSCode.ClientID%>").value = "";
            document.getElementById("<%=lblOSDescContent.ClientID%>").value = "";
            document.getElementById("<%=hidBOMApproved.ClientID%>").value = "";

            document.getElementById("<%=hidModelName.ClientID%>").value = "";
            document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
            if (''==GetCheckedModel())
                document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
            else
                document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            document.getElementById("<%=btnSaveAs.ClientID%>").disabled = true;
        }

    }

    function OnKeyPress(obj) {
        var key = event.keyCode;

        if (key == 13)//enter
        {
            if (event.srcElement.id == "<%=queryFirstModelName.ClientID %>") {
                var value = document.getElementById("<%=queryFirstModelName.ClientID %>").value.trim().toUpperCase();
                if (value != "") {
                    document.getElementById("<%=btnModelNameChange.ClientID %>").click();
                }

            }
        }
    }

    function GetMaxCheckboxCount(){
        var Parent = document.getElementById("<%=gdModelList.ClientID%>");
        var maxRow=-1;
        for (i = 0; i < Parent.rows.length; i++) {
            if(''==Parent.rows[i].cells[1].innerText.trim()){
                maxRow=i;
                break;
            }
        }
        if(maxRow==-1) maxRow=Parent.rows.length;
        return maxRow;
    }
    
    function GetCheckedModel(){
        var Parent = document.getElementById("<%=gdModelList.ClientID%>");
        var maxRow=GetMaxCheckboxCount();
        
        var m='';
        var items = Parent.getElementsByTagName('input');
        for (i = 1; i < maxRow; i++) { // items.length
            if (items[i].type == "checkbox" && items[i].checked) {
                if(m!='') m+=',';
                m+=Parent.rows[i].cells[1].innerText.trim();
            }
        }
        return m;
    }
    
    function SelectAllCheckboxesSpecific(spanChk) {
        var Parent = document.getElementById("<%=gdModelList.ClientID%>");
        var maxRow=GetMaxCheckboxCount();
        
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        var items = Parent.getElementsByTagName('input');
        for (i = 0; i < maxRow; i++) { // items.length
            if (items[i].id != Chk && items[i].type == "checkbox") {
                if (items[i].checked != IsChecked) {
                    items[i].click();
                }
            }
        }
    }

</script>

 
</asp:Content>

