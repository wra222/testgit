<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartType.ascx.cs" Inherits="CommonControl_CmbMaintainPartType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPartType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPartTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainPartType.ClientID %>");   
    }
    
    function getMaintainPartTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainPartType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainPartType.ClientID %>")[document.getElementById("<%=drpMaintainPartType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPartTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainPartType.ClientID %>").value;
    }
    
    function checkMaintainPartTypeCmb()
    {
        if ( getMaintainPartTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPartTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainPartType.ClientID %>").focus();   
    }                
</script>
