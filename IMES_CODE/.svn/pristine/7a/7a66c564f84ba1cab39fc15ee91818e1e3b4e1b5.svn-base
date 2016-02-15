<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartTypeAttribute.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPartTypeAttribute" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPartTypeAttributeList" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getPartTypeAttributeListCmbObj()
    {
        return document.getElementById("<%=drpPartTypeAttributeList.ClientID %>");   
    }

    function getPartTypeAttributeListCmbText()
    {
        if (document.getElementById("<%=drpPartTypeAttributeList.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpPartTypeAttributeList.ClientID %>")[document.getElementById("<%=drpPartTypeAttributeList.ClientID %>").selectedIndex].text;
        }
    }

    function getPartTypeAttributeListCmbValue()
    {
        return document.getElementById("<%=drpPartTypeAttributeList.ClientID %>").value;
    }

    function checkPartTypeAttributeListCmb()
    {
        if (getPartTypeAttributeListCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setPartTypeAttributeListCmbFocus()
    {
        document.getElementById("<%=drpPartTypeAttributeList.ClientID %>").focus();   
    }                
</script>
