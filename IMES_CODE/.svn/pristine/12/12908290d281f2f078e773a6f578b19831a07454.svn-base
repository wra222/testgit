<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartTypeAttributeSite.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPartTypeAttributeSite" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:CheckBoxList ID="drpSiteList" runat="server" Width="326px" AutoPostBack="true">
    </asp:CheckBoxList>
    <input type="hidden" runat="server" id="checkItem"/>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getSiteListCmbObj() 
    {
        var count = document.getElementById("<%=checkItem.ClientID %>").value;
        if (count != null && count != "") {
            return document.getElementById("<%=drpSiteList.ClientID %>");
        }
        return document.getElementById("<%=checkItem.ClientID %>");
    }

    function getSiteListCmbObjLength() {
        var count = document.getElementById("<%=checkItem.ClientID %>").value;
        if (count!=null && count!="") {
           return document.getElementById("<%=drpSiteList.ClientID %>").getElementsByTagName("input").length;
       }
       return 0;
    }

    function getSiteListCmbObjChecked(i) {
        var count = document.getElementById("<%=checkItem.ClientID %>").value;
        if (count != null && count != "") {
            return document.getElementById("<%=drpSiteList.ClientID %>_" + i);
        }
        return 0;
    }

    function getSiteListCmbValue()
    {
        return document.getElementById("<%=drpSiteList.ClientID %>").value;
    }

    function checkSiteListCmb()
    {
        if (getSiteListCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setSiteListCmbFocus()
    {
        document.getElementById("<%=drpSiteList.ClientID %>").focus();   
    }                
</script>