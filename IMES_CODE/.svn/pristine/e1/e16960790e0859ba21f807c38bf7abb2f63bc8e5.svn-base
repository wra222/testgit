<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainGrade.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainGrade" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="ddlGrade" runat="server" Width="380px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">

    function getGradeCmbObj()
    {
        return document.getElementById("<%=ddlGrade.ClientID %>");   
    }

    function getGradeCmbText()
    {
        if (document.getElementById("<%=ddlGrade.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=ddlGrade.ClientID %>")[document.getElementById("<%=ddlGrade.ClientID %>").selectedIndex].text;
        }
    }

    function getGradeCmbValue()
    {
        return document.getElementById("<%=ddlGrade.ClientID %>").value;
    }

    function checkGradeCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setGradeCmbFocus()
    {
        document.getElementById("<%=ddlGrade.ClientID %>").focus();   
    }                
</script>