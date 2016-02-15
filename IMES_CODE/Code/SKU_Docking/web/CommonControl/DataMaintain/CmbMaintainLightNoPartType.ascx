<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLightNoPartType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLightNoPartType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainLightNoPartType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainLightNoPartTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainLightNoPartType.ClientID %>");   
    }
    
    function getMaintainLightNoPartTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainLightNoPartType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainLightNoPartType.ClientID %>")[document.getElementById("<%=drpMaintainLightNoPartType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainLightNoPartTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainLightNoPartType.ClientID %>").value;
    }
    
    function checkMaintainLightNoPartTypeCmb()
    {
        if ( getMaintainLightNoPartTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainLightNoPartTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainLightNoPartType.ClientID %>").focus();   
    }                
</script>

