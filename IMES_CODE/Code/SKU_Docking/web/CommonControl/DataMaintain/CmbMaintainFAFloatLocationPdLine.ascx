<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFAFloatLocationPdLine.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainFAFloatLocationPdLine" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPdLineList" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getPdLineListCmbObj()
    {
        return document.getElementById("<%=drpPdLineList.ClientID %>");   
    }

    function getPdLineListCmbText()
    {
        if (document.getElementById("<%=drpPdLineList.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpPdLineList.ClientID %>")[document.getElementById("<%=drpPdLineList.ClientID %>").selectedIndex].text;
        }
    }

    function getPdLineListCmbValue()
    {
        return document.getElementById("<%=drpPdLineList.ClientID %>").value;
    }

    function checkPdLineListCmb()
    {
        if (getPdLineListCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setPdLineListCmbFocus()
    {
        document.getElementById("<%=drpPdLineList.ClientID %>").focus();   
    }                
</script>
