<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLightNoKittingType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLightNoKittingType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainLightNoKittingType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainLightNoKittingTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainLightNoKittingType.ClientID %>");   
    }
    
    function getMaintainLightNoKittingTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainLightNoKittingType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainLightNoKittingType.ClientID %>")[document.getElementById("<%=drpMaintainLightNoKittingType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainLightNoKittingTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainLightNoKittingType.ClientID %>").value;
    }
    
    function checkMaintainLightNoKittingTypeCmb()
    {
        if ( getMaintainLightNoKittingTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainLightNoKittingTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainLightNoKittingType.ClientID %>").focus();   
    }                
</script>
