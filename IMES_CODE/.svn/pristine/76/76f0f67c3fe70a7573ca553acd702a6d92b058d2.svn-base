<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Light Code DropdownList
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-12-21   itc202017           create
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbLightCodeControl.ascx.cs" Inherits="CommonControl_CmbLightCodeControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--  
function getLightCodeCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getLightCodeCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getLightCodeCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function checkLightCodeCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getLightCodeCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }

}

function setLightCodeCmbFocus()
{
    
    try {
        getLightCodeCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//-->
</SCRIPT>