<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainAssetRuleCheckStation.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainAssetRuleCheckStation" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainAssetRuleCheckStation" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainAssetRuleCheckStationCmbObj()
    {    
         return document.getElementById("<%=drpMaintainAssetRuleCheckStation.ClientID %>");   
    }
    
    function getMaintainAssetRuleCheckStationCmbText()
    { 
        if (document.getElementById("<%=drpMaintainAssetRuleCheckStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainAssetRuleCheckStation.ClientID %>")[document.getElementById("<%=drpMaintainAssetRuleCheckStation.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainAssetRuleCheckStationCmbValue()
    {
         return document.getElementById("<%=drpMaintainAssetRuleCheckStation.ClientID %>").value;
    }
    
    function checkMaintainAssetRuleCheckStationCmb()
    {
        if ( getMaintainAssetRuleCheckStationCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainAssetRuleCheckStationCmbFocus()
    {
        document.getElementById("<%=drpMaintainAssetRuleCheckStation.ClientID %>").focus();   
    }                
</script>
