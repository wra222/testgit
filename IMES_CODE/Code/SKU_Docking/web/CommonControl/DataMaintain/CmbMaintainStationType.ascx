<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainStationType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainStationType" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainStationType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainStationTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainStationType.ClientID %>");   
    }
    
    function getMaintainStationTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainStationType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainStationType.ClientID %>")[document.getElementById("<%=drpMaintainStationType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainStationTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainStationType.ClientID %>").value;
    }
    
    function checkMaintainStationTypeCmb()
    {
        if ( getMaintainStationTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainStationTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainStationType.ClientID %>").focus();   
    }                
</script>

