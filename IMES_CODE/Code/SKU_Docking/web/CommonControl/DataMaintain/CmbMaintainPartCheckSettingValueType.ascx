<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartCheckSettingValueType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPartCheckSettingValueType" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPartCheckSettingValueType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPartCheckSettingValueTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainPartCheckSettingValueType.ClientID %>");   
    }
    
    function getMaintainPartCheckSettingValueTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainPartCheckSettingValueType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainPartCheckSettingValueType.ClientID %>")[document.getElementById("<%=drpMaintainPartCheckSettingValueType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPartCheckSettingValueTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainPartCheckSettingValueType.ClientID %>").value;
    }
    
    function checkMaintainPartCheckSettingValueTypeCmb()
    {
        if ( getMaintainPartCheckSettingValueTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPartCheckSettingValueTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainPartCheckSettingValueType.ClientID %>").focus();   
    }                
</script>
