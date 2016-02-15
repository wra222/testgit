<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainCause.ascx.cs" Inherits="CommonControl_cmbMaintainCause" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainCause" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainCauseCmbObj()
    {
        return document.getElementById("<%=drpMaintainCause.ClientID %>");   
    }
    
    function getMaintainCauseCmbText()
    {
        if (document.getElementById("<%=drpMaintainCause.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMaintainCause.ClientID %>")[document.getElementById("<%=drpMaintainCause.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainCauseCmbValue()
    {
        return document.getElementById("<%=drpMaintainCause.ClientID %>").value;
    }
    
    function checkMaintainCauseCmb()
    {
        if ( getMaintainCauseCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainCauseCmbFocus()
    {
        document.getElementById("<%=drpMaintainCause.ClientID %>").focus();   
    }                
</script>
