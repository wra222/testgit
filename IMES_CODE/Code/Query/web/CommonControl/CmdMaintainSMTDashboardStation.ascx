<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmdMaintainSMTDashboardStation.ascx.cs" Inherits="CommonControl_CmdMaintainSMTDashboardStation" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainSMTDashboardStation" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainSMTDashboardStationCmbObj()
    {    
         return document.getElementById("<%=drpMaintainSMTDashboardStation.ClientID %>");   
    }
    
    function getMaintainSMTDashboardStationCmbText()
    { 
        if (document.getElementById("<%=drpMaintainSMTDashboardStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainSMTDashboardStation.ClientID %>")[document.getElementById("<%=drpMaintainSMTDashboardStation.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainSMTDashboardStationCmbValue()
    {
         return document.getElementById("<%=drpMaintainSMTDashboardStation.ClientID %>").value;
    }
    
    function checkMaintainSMTDashboardStationCmb()
    {
        if ( getMaintainLineCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainSMTDashboardStationCmbFocus()
    {
        document.getElementById("<%=drpMaintainSMTDashboardStation.ClientID %>").focus();   
    }                
</script>