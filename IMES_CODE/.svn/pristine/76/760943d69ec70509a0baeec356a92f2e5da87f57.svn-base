<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMBType.ascx.cs" Inherits="CommonControl_cmbMBType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMBType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMBTypeCmbObj()
    {
        return document.getElementById("<%=drpMBType.ClientID %>");   
    }
    
    function getMBTypeCmbText()
    {
        if (document.getElementById("<%=drpMBType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMBType.ClientID %>")[document.getElementById("<%=drpMBType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMBTypeCmbValue()
    {
        return document.getElementById("<%=drpMBType.ClientID %>").value;
    }
    
    function checkMBTypeCmb()
    {
        if (getMBTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMBTypeCmbFocus()
    {
        document.getElementById("<%=drpMBType.ClientID %>").focus();   
    }                
</script>
