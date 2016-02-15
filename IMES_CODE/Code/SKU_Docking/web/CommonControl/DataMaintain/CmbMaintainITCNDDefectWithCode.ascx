<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainITCNDDefectWithCode.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainITCNDDefectWithCode" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlCode" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getGradeFamiyCmbObj() {
       
        return document.getElementById("<%=ddlCode.ClientID %>");   
    }

    function getGradeFamiyCmbText()
    {
        if (document.getElementById("<%=ddlCode.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlCode.ClientID %>")[document.getElementById("<%=ddlCode.ClientID %>").selectedIndex].text;
        }
    }

    function getGradeFamiyCmbValue()
    {
        return document.getElementById("<%=ddlCode.ClientID %>").value;
    }

    function checkGradeFamiyCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setGradeFamiyCmbFocus()
    {
        document.getElementById("<%=ddlCode.ClientID %>").focus();   
    }                
</script>