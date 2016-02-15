<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLightStation.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLightStation" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlLightStation" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getLightStationCmbObj()
    {
        return document.getElementById("<%=ddlLightStation.ClientID %>");   
    }

    function getLightStationCmbText()
    {
        if (document.getElementById("<%=ddlLightStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlLightStation.ClientID %>")[document.getElementById("<%=ddlLightStation.ClientID %>").selectedIndex].text;
        }
    }

    function getLightStationCmbValue()
    {
        return document.getElementById("<%=ddlLightStation.ClientID %>").value;
    }

    function checkLightStationCmb()
    {
        if ( getLightStationCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setLightStationCmbFocus()
    {
        document.getElementById("<%=ddlLightStation.ClientID %>").focus();   
    }                
</script>