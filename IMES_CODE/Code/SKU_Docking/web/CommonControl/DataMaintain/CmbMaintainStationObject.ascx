<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainStationObject.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainStationObject" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainStationObject" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainStationObjectCmbObj()
    {    
         return document.getElementById("<%=drpMaintainStationObject.ClientID %>");   
    }
    
    function getMaintainStationObjectCmbText()
    { 
        if (document.getElementById("<%=drpMaintainStationObject.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainStationObject.ClientID %>")[document.getElementById("<%=drpMaintainStationObject.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainStationObjectCmbValue()
    {
         return document.getElementById("<%=drpMaintainStationObject.ClientID %>").value;
    }
    
    function checkMaintainStationObjectCmb()
    {
        if ( getMaintainStationObjectCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainStationObjectCmbFocus()
    {
        document.getElementById("<%=drpMaintainStationObject.ClientID %>").focus();   
    }                
</script>

