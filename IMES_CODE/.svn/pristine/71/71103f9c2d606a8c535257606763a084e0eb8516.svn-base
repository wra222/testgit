<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainRepairInfoType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainRepairInfoType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainRepairInfoType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainRepairInfoTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainRepairInfoType.ClientID %>");   
    }
    
    function getMaintainRepairInfoTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainRepairInfoType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainRepairInfoType.ClientID %>")[document.getElementById("<%=drpMaintainRepairInfoType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainRepairInfoTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainRepairInfoType.ClientID %>").value;
    }
    
    function checkMaintainRepairInfoTypeCmb()
    {
        if ( getMaintainRepairInfoTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainRepairInfoTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainRepairInfoType.ClientID %>").focus();   
    }                
</script>