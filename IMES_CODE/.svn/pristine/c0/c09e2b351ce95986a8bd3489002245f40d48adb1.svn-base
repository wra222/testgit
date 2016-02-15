<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for CDSI PO Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="CDSIPO.aspx.cs" Inherits="DataMaintain_CDSIPO" Title="CDSI PO Maintain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
    
}

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>                        
                    </td> 
                    <td>                       
                    </td>                                   
                    <td width="32%" align="right">
                        <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                         
                    </td>    
                </tr>
             </table>                                                    
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />   
                <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />            
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="130%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="376px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        OnGvExtRowClick='selectRow(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" style="top: -365px; left: 24px"  
                        >
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>   
        </div>
        
        <div id="div3">        
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>
                    <td style="width: 80px;">
                        <asp:Label ID="lblProd" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="txtProd" SkinId="textBoxSkin" runat="server" MaxLength="14" Width="80%" style="text-transform:uppercase;"></asp:TextBox>
                    </td>
                </tr>
                <tr>                   
                    <td style="width: 80px;">
                        <asp:Label ID="lblPO" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="txtPO"   SkinId="textBoxSkin" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
                    </td>                        
                    <td style="width: 80px;">
                        <asp:Label ID="lblPOItem" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="txtPOItem" SkinId="textBoxSkin" runat="server" MaxLength="10"  Width="80%"></asp:TextBox>
                    </td>                                           
                    <td style="width: 80px;">
                        <asp:Label ID="lblDelivery" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="160px">
                        <asp:TextBox ID="txtDelivery" SkinId="textBoxSkin" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                    </td>           
                    <td align="right">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                    </td>           
                </tr>        
                <tr>                   
                    <td>
                        <asp:Label ID="lblPallet" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPallet"   SkinId="textBoxSkin" runat="server" MaxLength="12" Width="80%"></asp:TextBox>
                    </td>                        
                    <td>
                        <asp:Label ID="lblBoxID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBoxID" SkinId="textBoxSkin" runat="server" MaxLength="30" Width="80%"></asp:TextBox>
                    </td>                                           
                    <td>
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" SkinId="textBoxSkin" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                    </td>           
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                    </td>           
                </tr>                
            </table> 
        </div>  
        
        <input type="hidden" id="hidSelectedId" runat="server" />         
        <input type="hidden" id="hidTableHeight" runat="server" />
   
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/>
                </td>
                <td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

    var selectedRowIndex = -1;
    
    window.onload = function() {
        initPage();
        resetTableHeight();
    };

    function initPage() {
        showEmptyContent();
        document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        document.getElementById("<%=btnDel.ClientID %>").disabled = true;    
    }
    
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
        var tableHeigth=300;
        
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=hidTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    function clkDelete()
    {
        ShowInfo("");
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex >= recordCount)
        {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectRecord").ToString()%>');
            return false;
        }

        if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString()%>')) {
            return false;
        }
        
        ShowWait();
        return true;        
    }
   
    function clkSave() {
        ShowInfo("");
        if (checkInputValues()) {
            ShowWait();
            return true;
        }
        else {
            return false;
        }
    }

    function clkAdd() {
        ShowInfo("");
        if (checkInputValues()) {
            ShowWait();
            return true;
        }
        else {
            return false;
        }
    }

    function checkInputValues() {
        var valProdid = document.getElementById("<%=txtProd.ClientID %>").value.trim().toUpperCase();
        var valPO = document.getElementById("<%=txtPO.ClientID %>").value.trim();
        var valPOItem = document.getElementById("<%=txtPOItem.ClientID %>").value.trim();
        var valDelivery = document.getElementById("<%=txtDelivery.ClientID %>").value.trim();
        var valPallet = document.getElementById("<%=txtPallet.ClientID %>").value.trim();
        var valBoxID = document.getElementById("<%=txtBoxID.ClientID %>").value.trim();
        var valRemark = document.getElementById("<%=txtRemark.ClientID %>").value.trim();

        if (valProdid.length == 0 || valProdid.length > 14) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadProdID").ToString()%>');
            document.getElementById("<%=txtProd.ClientID %>").focus();
            document.getElementById("<%=txtProd.ClientID %>").select();
            return false;
        }
        
        if (valPO.length == 0 || valPO.length > 50) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadPO").ToString()%>');
            document.getElementById("<%=txtPO.ClientID %>").focus();
            document.getElementById("<%=txtPO.ClientID %>").select();
            return false;
        }
        
        if (valPOItem == 0 || valPOItem.length > 10) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadPOItem").ToString()%>');
            document.getElementById("<%=txtPOItem.ClientID %>").focus();
            document.getElementById("<%=txtPOItem.ClientID %>").select();
            return false;
        }
        
        if (valDelivery.length > 20) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadDelivery").ToString()%>');
            document.getElementById("<%=txtDelivery.ClientID %>").focus();
            document.getElementById("<%=txtDelivery.ClientID %>").select();
            return false;
        }
        
        if (valPallet.length > 12) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadPallet").ToString()%>');
            document.getElementById("<%=txtPallet.ClientID %>").focus();
            document.getElementById("<%=txtPallet.ClientID %>").select();
            return false;
        }
        
        if (valBoxID.length > 30) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadBoxID").ToString()%>');
            document.getElementById("<%=txtBoxID.ClientID %>").focus();
            document.getElementById("<%=txtBoxID.ClientID %>").select();
            return false;
        }

        if (valRemark.length > 20) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadRemark").ToString()%>');
            document.getElementById("<%=txtRemark.ClientID %>").focus();
            document.getElementById("<%=txtRemark.ClientID %>").select();
            return false;
        }
        
        return true;
    }

    function selectRow(row) {
        ShowInfo("");
        if (selectedRowIndex == row.index) {
            return;
        }
        setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gd.ClientID %>");
        selectedRowIndex = row.index;
        setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, "<%=gd.ClientID %>");

        if (parseInt(selectedRowIndex) < parseInt(document.getElementById("<%=hidRecordCount.ClientID %>").value)) {
            showRowContent(row);
        }
        else
        {
            showEmptyContent();
        }
    }

    function showRowContent(row) {
        document.getElementById("<%=hidSelectedId.ClientID %>").value = row.cells[0].innerText.trim();
        document.getElementById("<%=txtProd.ClientID %>").value = row.cells[0].innerText.trim();
        document.getElementById("<%=txtPO.ClientID %>").value = row.cells[2].innerText.trim();
        document.getElementById("<%=txtPOItem.ClientID %>").value = row.cells[3].innerText.trim();
        document.getElementById("<%=txtDelivery.ClientID %>").value = row.cells[4].innerText.trim();
        document.getElementById("<%=txtPallet.ClientID %>").value = row.cells[5].innerText.trim();
        document.getElementById("<%=txtBoxID.ClientID %>").value = row.cells[6].innerText.trim();
        document.getElementById("<%=txtRemark.ClientID %>").value = row.cells[7].innerText.trim();
        document.getElementById("<%=btnSave.ClientID %>").disabled = false;
        document.getElementById("<%=btnDel.ClientID %>").disabled = false;  
    }
    
    function showEmptyContent() {
        document.getElementById("<%=hidSelectedId.ClientID %>").value = "";
        document.getElementById("<%=txtProd.ClientID %>").value = "";
        document.getElementById("<%=txtPO.ClientID %>").value = "";
        document.getElementById("<%=txtPOItem.ClientID %>").value = "";
        document.getElementById("<%=txtDelivery.ClientID %>").value = "";
        document.getElementById("<%=txtPallet.ClientID %>").value = "";
        document.getElementById("<%=txtBoxID.ClientID %>").value = "";
        document.getElementById("<%=txtRemark.ClientID %>").value = "";
        document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        document.getElementById("<%=btnDel.ClientID %>").disabled = true;  
    }   
   
    function AddUpdateComplete(id)
    {        
        var gdObj = document.getElementById("<%=gd.ClientID %>");

        for (var i = 0; i < document.getElementById("<%=hidRecordCount.ClientID %>").value; i++) 
        {
           if(gdObj.rows[i + 1].cells[0].innerText == id)
           {
               selectedRowIndex = i;
               break;
           }
       }

       if (selectedRowIndex < document.getElementById("<%=hidRecordCount.ClientID %>").value) {
           setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
           showRowContent(gdObj.rows[selectedRowIndex + 1]);
       }
       else {
           showEmptyContent();
       }
    }
    /*
     function disposeTree(sender, args) {
            var elements = args.get_panelsUpdating();
            for (var i = elements.length - 1; i >= 0; i--) {
                var element = elements[i];
                var allnodes = element.getElementsByTagName('*'),
                length = allnodes.length;
                var nodes = new Array(length)
                for (var k = 0; k < length; k++) {
                    nodes[k] = allnodes[k];
                }
                for (var j = 0, l = nodes.length; j < l; j++) {
                    var node = nodes[j];
                    if (node.nodeType === 1) {
                        if (node.dispose && typeof (node.dispose) === "function") {
                            node.dispose();
                        }
                        else if (node.control && typeof (node.control.dispose)=== "function") {
                            node.control.dispose();
                        }

                        var behaviors = node._behaviors;
                        if (behaviors) {
                            behaviors = Array.apply(null, behaviors);
                            for (var k = behaviors.length - 1; k >= 0; k--) {
                                behaviors[k].dispose();
                            }
                        }
                    }
                }
                element.innerHTML = "";
            }

        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);
        */
    </script>
</asp:Content>

