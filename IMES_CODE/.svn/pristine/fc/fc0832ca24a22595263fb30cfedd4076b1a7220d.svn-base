<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPAKitLocPdLine.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPAKitLocPdLine" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlPdLine" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getPAKitLocPdLineCmbObj() {
       
        return document.getElementById("<%=ddlPdLine.ClientID %>");   
    }

    function getPAKitLocPdLineCmbText()
    {
        if (document.getElementById("<%=ddlPdLine.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlPdLine.ClientID %>")[document.getElementById("<%=ddlPdLine.ClientID %>").selectedIndex].text;
        }
    }

    function getPAKitLocPdLineCmbValue()
    {
        return document.getElementById("<%=ddlPdLine.ClientID %>").value;
    }

    function checkPAKitLocPdLineCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setPAKitLocPdLineCmbFocus()
    {
        document.getElementById("<%=ddlPdLine.ClientID %>").focus();   
    }                
</script>