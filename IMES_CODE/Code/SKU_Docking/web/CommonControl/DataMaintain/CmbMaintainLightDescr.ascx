<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLightDescr.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLightDescr"%>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlLightDescr" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getLightDescrCmbObj()
    {
        return document.getElementById("<%=ddlLightDescr.ClientID %>");   
    }

    function getLightDescrCmbText()
    {
        if (document.getElementById("<%=ddlLightDescr.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlLightDescr.ClientID %>")[document.getElementById("<%=ddlLightDescr.ClientID %>").selectedIndex].text;
        }
    }

    function getLightDescrCmbValue()
    {
        return document.getElementById("<%=ddlLightDescr.ClientID %>").value;
    }

    function checkLightDescrCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setLightDescrCmbFocus()
    {
        document.getElementById("<%=ddlLightDescr.ClientID %>").focus();   
    }                
</script>