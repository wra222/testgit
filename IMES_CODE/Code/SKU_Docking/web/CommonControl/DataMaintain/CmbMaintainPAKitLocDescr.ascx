<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPAKitLocDescr.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPAKitLocDescr" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlTypeDescr" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getPAKitLocDescrCmbObj()
    {
        return document.getElementById("<%=ddlTypeDescr.ClientID %>");   
    }

    function getPAKitLocDescrCmbText()
    {
        if (document.getElementById("<%=ddlTypeDescr.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlTypeDescr.ClientID %>")[document.getElementById("<%=ddlTypeDescr.ClientID %>").selectedIndex].text;
        }
    }

    function getPAKitLocDescrCmbValue()
    {
        return document.getElementById("<%=ddlTypeDescr.ClientID %>").value;
    }

    function checkPAKitLocDescrCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setPAKitLocDescrCmbFocus()
    {
        document.getElementById("<%=ddlTypeDescr.ClientID %>").focus();   
    }                
</script>