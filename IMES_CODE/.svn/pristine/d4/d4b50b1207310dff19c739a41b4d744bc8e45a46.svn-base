<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: TPCBType Common Control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-04-14  Chen Xu (EB1-4)      create
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbTPCBType.ascx.cs" Inherits="CommonControl_CmbTPCBType" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--

function getTPCBTypeCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
function getTPCBTypeText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}

function getTPCBTypeValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


function checkTPCBTypeCmb()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getTPCBTypeText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

function setTPCBTypeCmbFocus()
{
    
    try {
        getTPCBTypeCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}

//-->
</SCRIPT>