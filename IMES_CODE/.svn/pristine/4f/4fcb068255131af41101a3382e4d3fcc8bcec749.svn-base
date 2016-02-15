<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainModelModelTolerance.ascx.cs" Inherits="CommonControl_CmbMaintainModel" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainModel" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainModelCmbObj()
    {    
         return document.getElementById("<%=drpMaintainModel.ClientID %>");   
    }
    
    function getMaintainModelCmbText()
    { 
        if (document.getElementById("<%=drpMaintainModel.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainModel.ClientID %>")[document.getElementById("<%=drpMaintainModel.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainModelCmbValue()
    {
         return document.getElementById("<%=drpMaintainModel.ClientID %>").value;
    }
    
    function checkMaintainModelCmb()
    {
        if ( getMaintainModelCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainModelCmbFocus()
    {
        document.getElementById("<%=drpMaintainModel.ClientID %>").focus();   
    }                
</script>
