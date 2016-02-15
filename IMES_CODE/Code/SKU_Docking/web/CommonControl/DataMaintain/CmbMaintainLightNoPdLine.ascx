<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLightNoPdLine.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLightNoPdLine" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainLightNoPdLine" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainLightNoPdLineCmbObj()
    {    
         return document.getElementById("<%=drpMaintainLightNoPdLine.ClientID %>");   
    }
    
    function getMaintainLightNoPdLineCmbText()
    { 
        if (document.getElementById("<%=drpMaintainLightNoPdLine.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainLightNoPdLine.ClientID %>")[document.getElementById("<%=drpMaintainLightNoPdLine.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainLightNoPdLineCmbValue()
    {
         return document.getElementById("<%=drpMaintainLightNoPdLine.ClientID %>").value;
    }
    
    function checkMaintainLightNoPdLineCmb()
    {
        if ( getMaintainLightNoPdLineCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainLightNoPdLineCmbFocus()
    {
        document.getElementById("<%=drpMaintainLightNoPdLine.ClientID %>").focus();   
    }                
</script>
