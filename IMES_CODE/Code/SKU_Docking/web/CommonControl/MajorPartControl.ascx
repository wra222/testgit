
<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: MajorPart common control
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-10-22  Chen Xu (EB1-4)      Create      
 * Known issues:
 */
 --%> 


<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MajorPartControl.ascx.cs" Inherits="CommonControl_MajorPartControl" %>


<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server" >
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>



<script type="text/javascript" language="javascript">

function getMajorPartCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 

function getMajorPartCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


function getMajorPartCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


function checkMajorPartCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getMajorPartCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

function setMajorPartCmbFocus()
{
    
    try {
        getMajorPartCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}

</script>