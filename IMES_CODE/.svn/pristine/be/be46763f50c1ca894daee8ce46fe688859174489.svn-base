<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPAKitLocStation.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPAKitLocStation" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlStation" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getPAKitLocStationCmbObj()
    {
        return document.getElementById("<%=ddlStation.ClientID %>");   
    }

    function getPAKitLocStationCmbText()
    {
        if (document.getElementById("<%=ddlStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlStation.ClientID %>")[document.getElementById("<%=ddlStation.ClientID %>").selectedIndex].text;
        }
    }

    function getPAKitLocStationCmbValue()
    {
        return document.getElementById("<%=ddlStation.ClientID %>").value;
    }

    function checkPAKitLocStationCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setPAKitLocStationCmbFocus()
    {
        document.getElementById("<%=ddlStation.ClientID %>").focus();   
    }                
</script>