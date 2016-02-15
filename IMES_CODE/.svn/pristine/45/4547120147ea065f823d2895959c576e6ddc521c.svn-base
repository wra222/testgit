<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainDefect.ascx.cs" Inherits="CommonControl_cmbMaintainDefect" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainDefect" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainDefectCmbObj()
    {
        return document.getElementById("<%=drpMaintainDefect.ClientID %>");   
    }
    
    function getMaintainDefectCmbText()
    {
        if (document.getElementById("<%=drpMaintainDefect.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMaintainDefect.ClientID %>")[document.getElementById("<%=drpMaintainDefect.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainDefectCmbValue()
    {
        return document.getElementById("<%=drpMaintainDefect.ClientID %>").value;
    }
    
    function checkMaintainDefectCmb()
    {
        if ( getMaintainDefectCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainDefectCmbFocus()
    {
        document.getElementById("<%=drpMaintainDefect.ClientID %>").focus();   
    }                
</script>
