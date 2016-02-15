<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLightNoStation.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLightNoStation" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainLightNoStation" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainLightNoStationCmbObj()
    {    
         return document.getElementById("<%=drpMaintainLightNoStation.ClientID %>");   
    }
    
    function getMaintainLightNoStationCmbText()
    { 
        if (document.getElementById("<%=drpMaintainLightNoStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainLightNoStation.ClientID %>")[document.getElementById("<%=drpMaintainLightNoStation.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainLightNoStationCmbValue()
    {
         return document.getElementById("<%=drpMaintainLightNoStation.ClientID %>").value;
    }
    
    function checkMaintainLightNoStationCmb()
    {
        if ( getMaintainLightNoStationCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainLightNoStationCmbFocus()
    {
        document.getElementById("<%=drpMaintainLightNoStation.ClientID %>").focus();   
    }                
</script>
