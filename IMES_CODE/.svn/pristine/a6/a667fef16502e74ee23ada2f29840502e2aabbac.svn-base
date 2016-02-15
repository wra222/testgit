<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFamily.ascx.cs" Inherits="CommonControl_CmbMaintainFamily" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainFamily" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainFamilyCmbObj()
    {    
         return document.getElementById("<%=drpMaintainFamily.ClientID %>");   
    }
    
    function getMaintainFamilyCmbText()
    { 
        if (document.getElementById("<%=drpMaintainFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainFamily.ClientID %>")[document.getElementById("<%=drpMaintainFamily.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainFamilyCmbValue()
    {
         return document.getElementById("<%=drpMaintainFamily.ClientID %>").value;
    }
    
    function checkMaintainFamilyCmb()
    {
        if ( getMaintainFamilyCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainFamilyCmbFocus()
    {
        document.getElementById("<%=drpMaintainFamily.ClientID %>").focus();   
    }                
</script>
