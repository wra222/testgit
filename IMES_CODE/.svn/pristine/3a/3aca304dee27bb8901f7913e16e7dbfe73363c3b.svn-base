<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainCheckItemType.ascx.cs" Inherits="CommonControl_CmbMaintainCheckItemType" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainCheckItemType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainCheckItemTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainCheckItemType.ClientID %>");   
    }
    
    function getMaintainCheckItemTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainCheckItemType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainCheckItemType.ClientID %>")[document.getElementById("<%=drpMaintainCheckItemType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainCheckItemTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainCheckItemType.ClientID %>").value;
    }
    
    function checkMaintainCheckItemTypeCmb()
    {
        if ( getMaintainCheckItemTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainCheckItemTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainCheckItemType.ClientID %>").focus();   
    }                
</script>
