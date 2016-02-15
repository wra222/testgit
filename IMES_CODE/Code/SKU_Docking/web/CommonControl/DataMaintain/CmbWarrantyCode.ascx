<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbWarrantyCode.ascx.cs" Inherits="CommonControl_CmbWarrantyCode" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpWarrantyCode" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    //Ship Type Code
    function getWarrantyCodeCmbObj()
    {    
         return document.getElementById("<%=drpWarrantyCode.ClientID %>");   
    }
    
    function getWarrantyCodeCmbText()
    { 
        if (document.getElementById("<%=drpWarrantyCode.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpWarrantyCode.ClientID %>")[document.getElementById("<%=drpWarrantyCode.ClientID %>").selectedIndex].text;
        }
    }
    
    function getWarrantyCodeCmbValue()
    {
         return document.getElementById("<%=drpWarrantyCode.ClientID %>").value;
    }
    
    function checkWarrantyCodeCmb()
    {
        if ( getWarrantyCodeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setWarrantyCodeCmbFocus()
    {
        document.getElementById("<%=drpWarrantyCode.ClientID %>").focus();   
    }                
</script>
