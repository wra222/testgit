<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbKittingCodeType.ascx.cs" Inherits="CommonControl_CmbKittingCodeType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpKittingCodeType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getKittingCodeTypeCmbObj()
    {    
         return document.getElementById("<%=drpKittingCodeType.ClientID %>");   
    }
    
    function getKittingCodeTypeCmbText()
    { 
        if (document.getElementById("<%=drpKittingCodeType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpKittingCodeType.ClientID %>")[document.getElementById("<%=drpKittingCodeType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getKittingCodeTypeCmbValue()
    {
         return document.getElementById("<%=drpKittingCodeType.ClientID %>").value;
    }
    
    function checkKittingCodeTypeCmb()
    {
        if ( getKittingCodeTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setKittingCodeTypeCmbFocus()
    {
        document.getElementById("<%=drpKittingCodeType.ClientID %>").focus();   
    }                
</script>
