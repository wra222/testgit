<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartTypeByCustomer.ascx.cs" Inherits="CommonControl_CmbMaintainPartTypeByCustomer" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPartTypeByCustomer" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPartTypeByCustomerCmbObj()
    {    
         return document.getElementById("<%=drpMaintainPartTypeByCustomer.ClientID %>");   
    }
    
    function getMaintainPartTypeByCustomerCmbText()
    { 
        if (document.getElementById("<%=drpMaintainPartTypeByCustomer.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainPartTypeByCustomer.ClientID %>")[document.getElementById("<%=drpMaintainPartTypeByCustomer.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPartTypeByCustomerCmbValue()
    {
         return document.getElementById("<%=drpMaintainPartTypeByCustomer.ClientID %>").value;
    }
    
    function checkMaintainPartTypeByCustomerCmb()
    {
        if ( getMaintainPartTypeByCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPartTypeByCustomerCmbFocus()
    {
        document.getElementById("<%=drpMaintainPartTypeByCustomer.ClientID %>").focus();   
    }                
</script>
