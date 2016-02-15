<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainCheckBit.ascx.cs" Inherits="CommonControl_CmbMaintainCheckBit" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainCheckBit" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainCheckBitCmbObj()
    {    
         return document.getElementById("<%=drpMaintainCheckBit.ClientID %>");   
    }
    
    function getMaintainCheckBitCmbText()
    { 
        if (document.getElementById("<%=drpMaintainCheckBit.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainCheckBit.ClientID %>")[document.getElementById("<%=drpMaintainCheckBit.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainCheckBitCmbValue()
    {
         return document.getElementById("<%=drpMaintainCheckBit.ClientID %>").value;
    }
    
    function checkMaintainCheckBitCmb()
    {
        if ( getMaintainCheckBitCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainCheckBitCmbFocus()
    {
        document.getElementById("<%=drpMaintainCheckBit.ClientID %>").focus();   
    }                
</script>
