<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainSection.ascx.cs" Inherits="CommonControl_cmbMaintainSection" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainSection" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainSectionCmbObj() {
        return document.getElementById("<%=drpMaintainSection.ClientID %>");
    }

    function getMaintainSectionCmbText() {
        if (document.getElementById("<%=drpMaintainSection.ClientID %>").selectedIndex == -1) {
            return "";
        } else {
        return document.getElementById("<%=drpMaintainSection.ClientID %>")[document.getElementById("<%=drpMaintainSection.ClientID %>").selectedIndex].text;
        }
    }

    function getMaintainSectionCmbValue() {
        return document.getElementById("<%=drpMaintainSection.ClientID %>").value;
    }

    function checkMaintainSectionCmb() {
        if (getMaintainSectionCmbText() == "") {
            return false;
        }

        return true;
    }

    function setMaintainSectionCmbFocus() {
        document.getElementById("<%=drpMaintainSection.ClientID %>").focus();
    }                
</script>
