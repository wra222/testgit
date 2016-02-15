<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainVendor.ascx.cs" Inherits="CommonControl_CmbMaintainVendor" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpVendor" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getVendorCmbObj()
    {
        return document.getElementById("<%=drpVendor.ClientID %>");   
    }

    function getVendorCmbText()
    {
        if (document.getElementById("<%=drpVendor.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpVendor.ClientID %>")[document.getElementById("<%=drpVendor.ClientID %>").selectedIndex].text;
        }
    }

    function getVendorCmbValue()
    {
        return document.getElementById("<%=drpVendor.ClientID %>").value;
    }

    function checkVendorCmb()
    {
        if (getVendorCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setVendorCmbFocus()
    {
        document.getElementById("<%=drpVendor.ClientID %>").focus();   
    }                
</script>
