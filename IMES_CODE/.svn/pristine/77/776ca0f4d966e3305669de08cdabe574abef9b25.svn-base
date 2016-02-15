<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartTypeAttributeCust.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPartTypeAttributeCust" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:CheckBoxList ID="drpCustList" runat="server" Width="326px" AutoPostBack="true">
    </asp:CheckBoxList>
    <input type="hidden" id="checkItemCust" runat="server"/>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getCustListCmbObj() 
    {
        var count = document.getElementById("<%=checkItemCust.ClientID %>").value;
        if (count != null && count != "") {
            return document.getElementById("<%=drpCustList.ClientID %>");
        }
        return document.getElementById("<% =checkItemCust.ClientID%>");
    }

    function getCustListCmbObjLength() 
    {
        var count = document.getElementById("<%=checkItemCust.ClientID %>").value;
        if (count != null && count != "") {
            return document.getElementById("<%=drpCustList.ClientID %>").getElementsByTagName("input").length;
        }
        return 0;
    }

    function getCustListCmbObjChecked(i) {
        var count = document.getElementById("<%=checkItemCust.ClientID %>").value;
        if (count != null && count != "") {
            return document.getElementById("<%=drpCustList.ClientID %>_" + i);
        }
        return 0;
    }

    function getCustListCmbValue() {
        return document.getElementById("<%=drpCustList.ClientID %>").value;
    }

    function checkCustListCmb() {
        if (getSiteListCmbText() == "") {
            return false;
        }

        return true;
    }

    function setCustListCmbFocus() {
        document.getElementById("<%=drpCustList.ClientID %>").focus();
    }                
</script>