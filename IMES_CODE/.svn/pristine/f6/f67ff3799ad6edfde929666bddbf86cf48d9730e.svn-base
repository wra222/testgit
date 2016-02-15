<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPAKitLocPartNo.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPAKitLocPartNo" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlPartNo" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getPAKitLocPartNoCmbObj() {
       
        return document.getElementById("<%=ddlPartNo.ClientID %>");   
    }

    function getPAKitLocPartNoCmbText()
    {
        if (document.getElementById("<%=ddlPartNo.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlPartNo.ClientID %>")[document.getElementById("<%=ddlPartNo.ClientID %>").selectedIndex].text;
        }
    }

    function getPAKitLocPartNoCmbValue()
    {
        return document.getElementById("<%=ddlPartNo.ClientID %>").value;
    }

    function checkPAKitLocPartNoCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setPAKitLocPartNoCmbFocus()
    {
        document.getElementById("<%=ddlPartNo.ClientID %>").focus();   
    }                
</script>