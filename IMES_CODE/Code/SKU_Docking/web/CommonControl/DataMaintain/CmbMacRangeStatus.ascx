<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMacRangeStatus.ascx.cs" Inherits="CommonControl_CmbMacRangeStatus" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMacRangeStatus" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMacRangeStatusCmbObj()
    {    
         return document.getElementById("<%=drpMacRangeStatus.ClientID %>");   
    }
    
    function getMacRangeStatusCmbText()
    { 
        if (document.getElementById("<%=drpMacRangeStatus.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMacRangeStatus.ClientID %>")[document.getElementById("<%=drpMacRangeStatus.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMacRangeStatusCmbValue()
    {
         return document.getElementById("<%=drpMacRangeStatus.ClientID %>").value;
    }
    
    function checkMacRangeStatusCmb()
    {
        if ( getMacRangeStatusCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMacRangeStatusCmbFocus()
    {
        document.getElementById("<%=drpMacRangeStatus.ClientID %>").focus();   
    }                
</script>
