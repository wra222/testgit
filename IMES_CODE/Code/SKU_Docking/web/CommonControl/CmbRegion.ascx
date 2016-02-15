
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbRegion.ascx.cs" Inherits="CommonControl_CmbRegion" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpRegion" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = "Input Region!";

    function getRegionCmbObj()
    {
        return document.getElementById("<%=drpRegion.ClientID %>");   
    }

    function getRegionCmbText()
    {
        if (document.getElementById("<%=drpRegion.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpRegion.ClientID %>")[document.getElementById("<%=drpRegion.ClientID %>").selectedIndex].text;
        }
    }

    function getRegionCmbValue()
    {
        return document.getElementById("<%=drpRegion.ClientID %>").value;  
    }

    function checkRegionCmb()
    {
        if (getRegionCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }
        return true;
    }

    function setRegionCmbFocus()
    {
        document.getElementById("<%=drpRegion.ClientID %>").focus();   
    }    
                
</script>

