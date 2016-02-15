<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmdMaintainSMTDashboardRefreshTime.ascx.cs" Inherits="CommonControl_CmdMaintainSMTDashboardRefreshTime" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainSMTDashboardRefreshTime" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainSMTDashboardRefreshTimeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainSMTDashboardRefreshTime.ClientID %>");   
    }
    
    function getMaintainSMTDashboardRefreshTimeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainSMTDashboardRefreshTime.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainSMTDashboardRefreshTime.ClientID %>")[document.getElementById("<%=drpMaintainSMTDashboardRefreshTime.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainSMTDashboardRefreshTimeCmbValue()
    {
         return document.getElementById("<%=drpMaintainSMTDashboardRefreshTime.ClientID %>").value;
    }
    
    function checkMaintainSMTDashboardRefreshTimeCmb()
    {
        if ( getMaintainSMTDashboardRefreshTimeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainSMTDashboardRefreshTimeCmbFocus()
    {
        document.getElementById("<%=drpMaintainSMTDashboardRefreshTime.ClientID %>").focus();   
    }                
</script>