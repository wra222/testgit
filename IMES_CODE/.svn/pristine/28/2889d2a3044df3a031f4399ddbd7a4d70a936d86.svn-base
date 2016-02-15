<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainStationByItemCheck.ascx.cs" Inherits="CommonControl_CmbMaintainStationByItemCheck" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainStationByItemCheck" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainStationByItemCheckCmbObj()
    {    
         return document.getElementById("<%=drpMaintainStationByItemCheck.ClientID %>");   
    }
    
    function getMaintainStationByItemCheckCmbText()
    { 
        if (document.getElementById("<%=drpMaintainStationByItemCheck.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainStationByItemCheck.ClientID %>")[document.getElementById("<%=drpMaintainStationByItemCheck.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainStationByItemCheckCmbValue()
    {
         return document.getElementById("<%=drpMaintainStationByItemCheck.ClientID %>").value;
    }
    
    function checkMaintainStationByItemCheckCmb()
    {
        if ( getMaintainStationByItemCheckCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainStationByItemCheckCmbFocus()
    {
        document.getElementById("<%=drpMaintainStationByItemCheck.ClientID %>").focus();   
    }                
</script>
