<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainValueType.ascx.cs" Inherits="CommonControl_CmbMaintainValueType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainValueType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainValueTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainValueType.ClientID %>");   
    }
    
    function getMaintainValueTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainValueType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainValueType.ClientID %>")[document.getElementById("<%=drpMaintainValueType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainValueTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainValueType.ClientID %>").value;
    }
    
    function checkMaintainValueTypeCmb()
    {
        if ( getMaintainValueTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainValueTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainValueType.ClientID %>").focus();   
    }                
</script>
