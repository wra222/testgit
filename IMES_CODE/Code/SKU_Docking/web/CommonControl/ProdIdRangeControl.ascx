<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:ProdIdRange DropdownList
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-05  Chen Xu (EB1-4)      Create      
 * Known issues:
 */
 --%> 

 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdIdRangeControl.ascx.cs" Inherits="CommonControl_ProdIdRangeControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>



<script type="text/javascript" language="javascript">
  
function getProdIdRangeCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
function getProdIdRangeCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getProdIdRangeCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


function checkProdIdRangeCmbText()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getProdIdRangeCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

function setProdIDRangeCmbFocus()
{
    
    try {
        getProdIdRangeCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}

</script>