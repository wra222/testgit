<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaitainGradeFamilyTop.ascx.cs" Inherits="CommonControl_DataMaintain_CblMaitainGradeFamilyTop" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlFamilyTop" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getGradeFamiyTopCmbObj()
    {
        return document.getElementById("<%=ddlFamilyTop.ClientID %>");   
    }

    function getGradeFamiyTopCmbText()
    {
        if (document.getElementById("<%=ddlFamilyTop.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlFamilyTop.ClientID %>")[document.getElementById("<%=ddlFamilyTop.ClientID %>").selectedIndex].text;
        }
    }

    function getGradeFamiyTopCmbValue()
    {
        return document.getElementById("<%=ddlFamilyTop.ClientID %>").value;
    }

    function checkGradeFamiyTopCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setGradeFamiyTopCmbFocus()
    {
        document.getElementById("<%=ddlFamilyTop.ClientID %>").focus();   
    }                
</script>