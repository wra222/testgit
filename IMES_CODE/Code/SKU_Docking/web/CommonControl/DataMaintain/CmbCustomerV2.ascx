<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbCustomerV2.ascx.cs" Inherits="CommonControl_CmbCustomerV2" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpCustomer" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getCustomerCmbObj()
    {    
         return document.getElementById("<%=drpCustomer.ClientID %>");   
    }
    
    function getCustomerCmbText()
    { 
        if (document.getElementById("<%=drpCustomer.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpCustomer.ClientID %>")[document.getElementById("<%=drpCustomer.ClientID %>").selectedIndex].text;
        }
    }
    
    function getCustomerCmbValue()
    {
         return document.getElementById("<%=drpCustomer.ClientID %>").value;
    }
    
    function checkCustomerCmb()
    {
        if ( getCustomerCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setCustomerCmbFocus()
    {
        document.getElementById("<%=drpCustomer.ClientID %>").focus();   
    }                
</script>
