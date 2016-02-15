<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLine.ascx.cs" Inherits="CommonControl_CmbMaintainLine" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainLine" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainLineCmbObj()
    {    
         return document.getElementById("<%=drpMaintainLine.ClientID %>");   
    }
    
    function getMaintainLineCmbText()
    { 
        if (document.getElementById("<%=drpMaintainLine.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainLine.ClientID %>")[document.getElementById("<%=drpMaintainLine.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainLineCmbValue()
    {
         return document.getElementById("<%=drpMaintainLine.ClientID %>").value;
    }
    
    function checkMaintainLineCmb()
    {
        if ( getMaintainLineCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainLineCmbFocus()
    {
        document.getElementById("<%=drpMaintainLine.ClientID %>").focus();   
    }                
</script>
