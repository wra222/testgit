 <%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:Model DropdownList
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-10-25  Chen Xu (EB1-4)      Create      
 * Known issues:
 */
 --%> 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModelControl.ascx.cs" Inherits="CommonControl_ModelControl" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true" >
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>



<script type="text/javascript" language="javascript">

function getModelCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
 function getModelCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


 function getModelCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


 function checkModelCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getModelCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

function setModelCmbFocus()
{
    
    try {
        getModelCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}
function setModelCmbToDefault() {
    document.all("<%=DropDownList1.ClientID %>").options[0].selected = true;
}
</script>