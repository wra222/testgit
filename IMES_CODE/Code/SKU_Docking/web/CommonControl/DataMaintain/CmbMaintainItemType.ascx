<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainItemType.ascx.cs" Inherits="CommonControl_CmbMaintainItemType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainItemType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainItemTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainItemType.ClientID %>");   
    }
    
    function getMaintainItemTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainItemType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainItemType.ClientID %>")[document.getElementById("<%=drpMaintainItemType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainItemTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainItemType.ClientID %>").value;
    }
    
    function checkMaintainItemTypeCmb()
    {
        if ( getMaintainItemTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainItemTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainItemType.ClientID %>").focus();   
    }                
</script>
