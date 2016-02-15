<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainMBTestCode.ascx.cs"
    Inherits="CommonControl_DataMaintain_CmbMaintainMBTestCode" %>
<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:DropDownList ID="drpMBTestCode" runat="server" Width="326px">
        </asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function getObjMBTestCode() {
        return document.getElementById("<%=drpMBTestCode.ClientID %>");
    }

    function getObjMBTestCodeValue() {
        return document.getElementById("<%=drpMBTestCode.ClientID %>").selectedValue;
    }

    function setMBTestCodeCmbFocus() {
        document.getElementById("<%=drpMBTestCode.ClientID %>").focus();
    }      
</script>

