<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbGradeHPFamily.ascx.cs" Inherits="CommonControl_DataMaintain_CmbGradeHPFamily" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlFamily" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getGradeFamiyCmbObj() {
       
        return document.getElementById("<%=ddlFamily.ClientID %>");   
    }

    function getGradeFamiyCmbText()
    {
        if (document.getElementById("<%=ddlFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlFamily.ClientID %>")[document.getElementById("<%=ddlFamily.ClientID %>").selectedIndex].text;
        }
    }

    function getGradeFamiyCmbValue()
    {
        return document.getElementById("<%=ddlFamily.ClientID %>").value;
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
        document.getElementById("<%=ddlFamily.ClientID %>").focus();   
    }                
</script>