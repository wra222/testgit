<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMBFamily.ascx.cs" Inherits="CommonControl_cmbMBFamily" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMBFamily" runat="server" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMBFamilyCmbObj()
    {
        return document.getElementById("<%=drpMBFamily.ClientID %>");   
    }

    function getMBFamilyCmbText()
    {
        if (document.getElementById("<%=drpMBFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMBFamily.ClientID %>")[document.getElementById("<%=drpMBFamily.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMBFamilyCmbValue()
    {
        return document.getElementById("<%=drpMBFamily.ClientID %>").value;
    }
    
    function checkMBFamilyCmb()
    {
        if (getMBFamilyCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMBFamilyCmbFocus()
    {
        document.getElementById("<%=drpMBFamily.ClientID %>").focus();   
    }                
</script>
