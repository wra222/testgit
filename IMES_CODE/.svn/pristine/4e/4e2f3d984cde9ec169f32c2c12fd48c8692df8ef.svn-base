<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFAFloatLocationFamily.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainFAFloatLocationFamily" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpFamilyList" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getFamilyListCmbObj()
    {
        return document.getElementById("<%=drpFamilyList.ClientID %>");   
    }

    function getFamilyListCmbText()
    {
        if (document.getElementById("<%=drpFamilyList.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpFamilyList.ClientID %>")[document.getElementById("<%=drpFamilyList.ClientID %>").selectedIndex].text;
        }
    }

    function getFamilyListCmbValue()
    {
        return document.getElementById("<%=drpFamilyList.ClientID %>").value;
    }

    function checkFamilyListCmb()
    {
        if (getFamilyListCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setFamilyListCmbFocus()
    {
        document.getElementById("<%=drpFamilyList.ClientID %>").focus();   
    }                
</script>