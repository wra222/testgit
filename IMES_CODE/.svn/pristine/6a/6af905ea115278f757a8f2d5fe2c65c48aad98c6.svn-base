<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainWC.ascx.cs" Inherits="CommonControl_cmbMaintainWC" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainWC" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainWCCmbObj()
    {
        return document.getElementById("<%=drpMaintainWC.ClientID %>");   
    }
    
    function getMaintainWCCmbText()
    {
        if (document.getElementById("<%=drpMaintainWC.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMaintainWC.ClientID %>")[document.getElementById("<%=drpMaintainWC.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainWCCmbValue()
    {
        return document.getElementById("<%=drpMaintainWC.ClientID %>").value;
    }
    
    function checkMaintainWCCmb()
    {
        if ( getMaintainWCCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainWCCmbFocus()
    {
        document.getElementById("<%=drpMaintainWC.ClientID %>").focus();   
    }                
</script>
