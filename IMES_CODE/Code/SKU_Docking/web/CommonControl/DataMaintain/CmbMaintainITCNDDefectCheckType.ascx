<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainITCNDDefectCheckType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainITCNDDefectCheckType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainITCNDDefectCheckType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainFamilyCmbObj()
    {    
         return document.getElementById("<%=drpMaintainITCNDDefectCheckType.ClientID %>");   
    }
    
    function getMaintainFamilyCmbText()
    { 
        if (document.getElementById("<%=drpMaintainITCNDDefectCheckType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainITCNDDefectCheckType.ClientID %>")[document.getElementById("<%=drpMaintainITCNDDefectCheckType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainFamilyCmbValue()
    {
         return document.getElementById("<%=drpMaintainITCNDDefectCheckType.ClientID %>").value;
    }
    
    function checkMaintainFamilyCmb()
    {
        if ( getMaintainFamilyCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainFamilyCmbFocus()
    {
        document.getElementById("<%=drpMaintainITCNDDefectCheckType.ClientID %>").focus();   
    }                
</script>