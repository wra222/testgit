<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainMBCode.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainMBCode" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainMBCode" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainMBCodeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainMBCode.ClientID %>");   
    }
    
    function getMaintainMBCodeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainMBCode.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainMBCode.ClientID %>")[document.getElementById("<%=drpMaintainMBCode.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainMBCodeCmbValue()
    {
         return document.getElementById("<%=drpMaintainMBCode.ClientID %>").value;
    }
    
    function checkMaintainMBCodeCmb()
    {
        if ( getMaintainMBCodeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainModelCmbFocus()
    {
        document.getElementById("<%=drpMaintainMBCode.ClientID %>").focus();   
    }                
</script>
