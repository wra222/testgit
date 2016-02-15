<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainCheckType.ascx.cs" Inherits="CommonControl_cmbMaintainCheckType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainCheckType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainCheckTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainCheckType.ClientID %>");   
    }
    
    function getMaintainCheckTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainCheckType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainCheckType.ClientID %>")[document.getElementById("<%=drpMaintainCheckType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainCheckTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainCheckType.ClientID %>").value;
    }
    
    function checkMaintainCheckTypeCmb()
    {
        if ( getMaintainCheckTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainCheckTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainCheckType.ClientID %>").focus();   
    }                
</script>
