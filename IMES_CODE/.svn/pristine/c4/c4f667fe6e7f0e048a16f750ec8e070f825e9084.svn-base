<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFAFloatLocationFamilyTop.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainFAFloatLocationFamilyTop" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpFamilyTopList" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getFamilyTopListCmbObj()
    {
        return document.getElementById("<%=drpFamilyTopList.ClientID %>");   
    }

    function getFamilyTopListCmbText()
    {
        if (document.getElementById("<%=drpFamilyTopList.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpFamilyTopList.ClientID %>")[document.getElementById("<%=drpFamilyTopList.ClientID %>").selectedIndex].text;
        }
    }

    function getFamilyTopListCmbValue()
    {
        return document.getElementById("<%=drpFamilyTopList.ClientID %>").value;
    }

    function checkFamilyTopListCmb()
    {
        if (getFamilyTopListCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setFamilyTopListCmbFocus()
    {
        document.getElementById("<%=drpFamilyTopList.ClientID %>").focus();   
    }                
</script>
