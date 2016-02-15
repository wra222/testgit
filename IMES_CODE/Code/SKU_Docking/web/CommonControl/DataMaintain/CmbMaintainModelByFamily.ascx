<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainModelByFamily.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainModelByFamily" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainModelByFamily" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainModelByFamilyCmbObj()
    {    
         return document.getElementById("<%=drpMaintainModelByFamily.ClientID %>");   
    }
    
    function getMaintainModelByFamilyCmbText()
    { 
        if (document.getElementById("<%=drpMaintainModelByFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainModelByFamily.ClientID %>")[document.getElementById("<%=drpMaintainModelByFamily.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainModelByFamilyCmbValue()
    {
         return document.getElementById("<%=drpMaintainModelByFamily.ClientID %>").value;
    }
    
    function checkMaintainModelByFamilyCmb()
    {
        if ( getMaintainModelByFamilyCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainModelByFamilyCmbFocus()
    {
        document.getElementById("<%=drpMaintainModelByFamily.ClientID %>").focus();   
    }                
</script>

