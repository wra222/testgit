<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFaKittingFamily.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainFaKittingFamily" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainFaKittingFamily" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainFaKittingFamilyCmbObj()
    {    
         return document.getElementById("<%=drpMaintainFaKittingFamily.ClientID %>");   
    }
    
    function getMaintainFaKittingFamilyCmbText()
    { 
        if (document.getElementById("<%=drpMaintainFaKittingFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainFaKittingFamily.ClientID %>")[document.getElementById("<%=drpMaintainFaKittingFamily.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainFaKittingFamilyCmbValue()
    {
         return document.getElementById("<%=drpMaintainFaKittingFamily.ClientID %>").value;
    }
    
    function checkMaintainFaKittingFamilyCmb()
    {
        if ( getMaintainFaKittingFamilyCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainFaKittingFamilyCmbFocus()
    {
        document.getElementById("<%=drpMaintainFaKittingFamily.ClientID %>").focus();   
    }                
</script>
