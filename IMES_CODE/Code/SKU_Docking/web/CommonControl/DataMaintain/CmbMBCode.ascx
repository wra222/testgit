<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMBCode.ascx.cs" Inherits="CommonControl_cmbMBCode" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMBCode" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMBCodeCmbObj()
    {
        return document.getElementById("<%=drpMBCode.ClientID %>");   
    }

    function getMBCodeCmbText()
    {
        if (document.getElementById("<%=drpMBCode.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMBCode.ClientID %>")[document.getElementById("<%=drpMBCode.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMBCodeCmbValue()
    {
        return document.getElementById("<%=drpMBCode.ClientID %>").value;
    }
    
    function checkMBCodeCmb()
    {
        if (getMBCodeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMBCodeCmbFocus()
    {
        document.getElementById("<%=drpMBCode.ClientID %>").focus();   
    }                
</script>
