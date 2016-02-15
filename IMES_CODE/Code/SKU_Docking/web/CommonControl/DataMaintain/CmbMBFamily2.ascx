<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMBFamily2.ascx.cs" Inherits="CommonControl_cmbMBFamily2" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMBFamily" runat="server" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMBFamily2CmbObj()
    {
        return document.getElementById("<%=drpMBFamily.ClientID %>");   
    }

    function getMBFamily2CmbText()
    {
        if (document.getElementById("<%=drpMBFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpMBFamily.ClientID %>")[document.getElementById("<%=drpMBFamily.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMBFamily2CmbValue()
    {
        return document.getElementById("<%=drpMBFamily.ClientID %>").value;
    }
    
    function checkMBFamily2Cmb()
    {
        if (getMBFamilyCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMBFamily2CmbFocus()
    {
        document.getElementById("<%=drpMBFamily.ClientID %>").focus();   
    }                
</script>
