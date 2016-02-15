<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPalletSpec.ascx.cs" Inherits="CommonControl_CmbMaintainPalletSpec" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPalletSpec" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPalletSpecCmbObj()
    {
        return document.getElementById("<%=drpMaintainPalletSpec.ClientID %>");   
    }
    
    function getMaintainPalletSpecCmbText()
    {
        if (document.getElementById("<%=drpMaintainPalletSpec.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMaintainPalletSpec.ClientID %>")[document.getElementById("<%=drpMaintainPalletSpec.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPalletSpecCmbValue()
    {
        return document.getElementById("<%=drpMaintainPalletSpec.ClientID %>").value;
    }
    
    function checkMaintainPalletSpecCmb()
    {
        if ( getMaintainPalletSpecCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPalletSpecCmbFocus()
    {
        document.getElementById("<%=drpMaintainPalletSpec.ClientID %>").focus();   
    }                
</script>
