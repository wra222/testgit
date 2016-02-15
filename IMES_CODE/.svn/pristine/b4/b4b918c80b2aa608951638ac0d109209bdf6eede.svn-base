<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainAssetRuleAstType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainAssetRuleAstType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainAssetRuleAstType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainAssetRuleAstTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainAssetRuleAstType.ClientID %>");   
    }
    
    function getMaintainAssetRuleAstTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainAssetRuleAstType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainAssetRuleAstType.ClientID %>")[document.getElementById("<%=drpMaintainAssetRuleAstType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainAssetRuleAstTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainAssetRuleAstType.ClientID %>").value;
    }
    
    function checkMaintainAssetRuleAstTypeCmb()
    {
        if ( getMaintainAssetRuleAstTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainAssetRuleAstTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainAssetRuleAstType.ClientID %>").focus();   
    }                
</script>
