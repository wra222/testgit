<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainStage.ascx.cs" Inherits="CommonControl_CmbMaintainStage" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainStage" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainStageCmbObj()
    {    
         return document.getElementById("<%=drpMaintainStage.ClientID %>");   
    }
    
    function getMaintainStageCmbText()
    { 
        if (document.getElementById("<%=drpMaintainStage.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainStage.ClientID %>")[document.getElementById("<%=drpMaintainStage.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainStageCmbValue()
    {
         return document.getElementById("<%=drpMaintainStage.ClientID %>").value;
    }
    
    function checkMaintainStageCmb()
    {
        if ( getMaintainStageCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainStageCmbFocus()
    {
        document.getElementById("<%=drpMaintainStage.ClientID %>").focus();   
    }                
</script>
