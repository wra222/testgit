<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainNeedCheck.ascx.cs" Inherits="CommonControl_CmbMaintainNeedCheck" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainNeedCheck" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainNeedCheckCmbObj()
    {    
         return document.getElementById("<%=drpMaintainNeedCheck.ClientID %>");   
    }
    
    function getMaintainNeedCheckCmbText()
    { 
        if (document.getElementById("<%=drpMaintainNeedCheck.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainNeedCheck.ClientID %>")[document.getElementById("<%=drpMaintainNeedCheck.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainNeedCheckCmbValue()
    {
         return document.getElementById("<%=drpMaintainNeedCheck.ClientID %>").value;
    }
    
    function checkMaintainNeedCheckCmb()
    {
        if ( getMaintainNeedCheckCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainNeedCheckCmbFocus()
    {
        document.getElementById("<%=drpMaintainNeedCheck.ClientID %>").focus();   
    }                
</script>
