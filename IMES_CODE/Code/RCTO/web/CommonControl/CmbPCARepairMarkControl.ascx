<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: PCA Repair mark DropdownList
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-12-21   itc202017           create
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPCARepairMarkControl.ascx.cs" Inherits="CommonControl_CmbPCARepairMarkControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--  
function getPCARepairMarkCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getPCARepairMarkCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getPCARepairMarkCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function checkPCARepairMarkCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getPCARepairMarkCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }

}

function setPCARepairMarkCmbFocus()
{
    
    try {
        getPCARepairMarkCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//-->
</SCRIPT>