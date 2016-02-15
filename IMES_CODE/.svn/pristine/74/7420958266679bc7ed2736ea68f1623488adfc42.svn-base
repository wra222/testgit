<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainCheckItem.ascx.cs" Inherits="CommonControl_cmbMaintainCheckItem" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainCheckItem" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainCheckItemCmbObj()
    {    
         return document.getElementById("<%=drpMaintainCheckItem.ClientID %>");   
    }
    
    function getMaintainCheckItemCmbText()
    { 
        if (document.getElementById("<%=drpMaintainCheckItem.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainCheckItem.ClientID %>")[document.getElementById("<%=drpMaintainCheckItem.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainCheckItemCmbValue()
    {
         return document.getElementById("<%=drpMaintainCheckItem.ClientID %>").value;
    }
    
    function checkMaintainCheckItemCmb()
    {
        if ( getMaintainCheckItemCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainCheckItemCmbFocus()
    {
        document.getElementById("<%=drpMaintainCheckItem.ClientID %>").focus();   
    }                
</script>
