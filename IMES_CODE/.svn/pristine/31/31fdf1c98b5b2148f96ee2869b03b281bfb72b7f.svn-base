<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainAssetRuleCheckType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainAssetRuleCheckType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainAssetRuleCheckType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainAssetRuleCheckTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainAssetRuleCheckType.ClientID %>");   
    }
    
    function getMaintainAssetRuleCheckTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainAssetRuleCheckType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainAssetRuleCheckType.ClientID %>")[document.getElementById("<%=drpMaintainAssetRuleCheckType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainAssetRuleCheckTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainAssetRuleCheckType.ClientID %>").value;
    }
    
    function checkMaintainAssetRuleCheckTypeCmb()
    {
        if ( getMaintainAssetRuleCheckTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainAssetRuleCheckTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainAssetRuleCheckType.ClientID %>").focus();   
    }                
</script>
