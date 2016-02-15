<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainAssetRuleCheckItem.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainAssetRuleCheckItem" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainAssetRuleCheckItem" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainAssetRuleCheckItemCmbObj()
    {    
         return document.getElementById("<%=drpMaintainAssetRuleCheckItem.ClientID %>");   
    }
    
    function getMaintainAssetRuleCheckItemCmbText()
    { 
        if (document.getElementById("<%=drpMaintainAssetRuleCheckItem.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainAssetRuleCheckItem.ClientID %>")[document.getElementById("<%=drpMaintainAssetRuleCheckItem.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainAssetRuleCheckItemCmbValue()
    {
         return document.getElementById("<%=drpMaintainAssetRuleCheckItem.ClientID %>").value;
    }
    
    function checkMaintainAssetRuleCheckItemCmb()
    {
        if ( getMaintainAssetRuleCheckItemCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainAssetRuleCheckItemCmbFocus()
    {
        document.getElementById("<%=drpMaintainAssetRuleCheckItem.ClientID %>").focus();   
    }   
                 
</script>

