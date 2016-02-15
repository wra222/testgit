<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainITCNDCheckSettingCheckType.ascx.cs" Inherits="CommonControl_cmbMaintainITCNDCheckSettingCheckType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainITCNDCheckSettingCheckType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getITCNDCheckSettingCheckTypeCmbObj()
    {
        return document.getElementById("<%=drpMaintainITCNDCheckSettingCheckType.ClientID %>");   
    }
    
    function getITCNDCheckSettingCheckTypeCmbText()
    {
        if (document.getElementById("<%=drpMaintainITCNDCheckSettingCheckType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMaintainITCNDCheckSettingCheckType.ClientID %>")[document.getElementById("<%=drpMaintainITCNDCheckSettingCheckType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getITCNDCheckSettingCheckTypeCmbValue()
    {
        return document.getElementById("<%=drpMaintainITCNDCheckSettingCheckType.ClientID %>").value;
    }
    
    function checkITCNDCheckSettingCheckTypeCmb()
    {
        if (getITCNDCheckSettingCheckTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setITCNDCheckSettingCheckTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainITCNDCheckSettingCheckType.ClientID %>").focus();   
    }                
</script>
