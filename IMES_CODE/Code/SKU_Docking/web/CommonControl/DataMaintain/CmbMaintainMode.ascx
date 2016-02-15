<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainMode.ascx.cs" Inherits="CommonControl_CmbMaintainMode" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainMode" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainModeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainMode.ClientID %>");   
    }
    
    function getMaintainModeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainMode.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainMode.ClientID %>")[document.getElementById("<%=drpMaintainMode.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainModeCmbValue()
    {
         return document.getElementById("<%=drpMaintainMode.ClientID %>").value;
    }
    
    function checkMaintainModeCmb()
    {
        if ( getMaintainModeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainModeCmbFocus()
    {
        document.getElementById("<%=drpMaintainMode.ClientID %>").focus();   
    }                
</script>
