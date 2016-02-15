<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbWarrantyFormat.ascx.cs" Inherits="CommonControl_CmbWarrantyFormat" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpWarrantyFormat" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getWarrantyFormatCmbObj()
    {    
         return document.getElementById("<%=drpWarrantyFormat.ClientID %>");   
    }
    
    function getWarrantyFormatCmbText()
    { 
        if (document.getElementById("<%=drpWarrantyFormat.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpWarrantyFormat.ClientID %>")[document.getElementById("<%=drpWarrantyFormat.ClientID %>").selectedIndex].text;
        }
    }
    
    function getWarrantyFormatCmbValue()
    {
         return document.getElementById("<%=drpWarrantyFormat.ClientID %>").value;
    }
    
    function checkWarrantyFormatCmb()
    {
        if ( getWarrantyFormatCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setWarrantyFormatCmbFocus()
    {
        document.getElementById("<%=drpWarrantyFormat.ClientID %>").focus();   
    }                
</script>
