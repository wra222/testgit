<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbWarrantyType.ascx.cs" Inherits="CommonControl_CmbWarrantyType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpWarrantyType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getWarrantyTypeCmbObj()
    {    
         return document.getElementById("<%=drpWarrantyType.ClientID %>");   
    }
    
    function getWarrantyTypeCmbText()
    { 
        if (document.getElementById("<%=drpWarrantyType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpWarrantyType.ClientID %>")[document.getElementById("<%=drpWarrantyType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getWarrantyTypeCmbValue()
    {
         return document.getElementById("<%=drpWarrantyType.ClientID %>").value;
    }
    
    function checkWarrantyTypeCmb()
    {
        if ( getWarrantyTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setWarrantyTypeCmbFocus()
    {
        document.getElementById("<%=drpWarrantyType.ClientID %>").focus();   
    }                
</script>
