<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainStation.ascx.cs" Inherits="CommonControl_cmbMaintainStation" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainStation" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainStationCmbObj()
    {    
         return document.getElementById("<%=drpMaintainStation.ClientID %>");   
    }
    
    function getMaintainStationCmbText()
    { 
        if (document.getElementById("<%=drpMaintainStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainStation.ClientID %>")[document.getElementById("<%=drpMaintainStation.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainStationCmbValue()
    {
         return document.getElementById("<%=drpMaintainStation.ClientID %>").value;
    }
    
    function checkMaintainStationCmb()
    {
        if ( getMaintainStationCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainStationCmbFocus()
    {
        document.getElementById("<%=drpMaintainStation.ClientID %>").focus();   
    }                
</script>
