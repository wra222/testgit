<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Change Key Part type DropdownList
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-12-21   itc202017           create
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbChangeKPTypeControl.ascx.cs" Inherits="CommonControl_CmbChangeKPTypeControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--
function getChangeKPTypeCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getChangeKPTypeCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getChangeKPTypeCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function checkChangeKPTypeCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getChangeKPTypeCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }

}

function setChangeKPTypeCmbFocus()
{
    
    try {
        getChangeKPTypeCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//-->
</SCRIPT>