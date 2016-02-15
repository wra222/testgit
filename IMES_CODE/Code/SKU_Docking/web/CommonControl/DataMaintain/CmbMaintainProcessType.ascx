<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainProcessType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainProcessType" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainProcessType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainProcessTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainProcessType.ClientID %>");   
    }
    
    function getMaintainProcessTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainProcessType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainProcessType.ClientID %>")[document.getElementById("<%=drpMaintainProcessType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainProcessTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainProcessType.ClientID %>").value;
    }
    
    function checkMaintainProcessTypeCmb()
    {
        if ( getMaintainProcessTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainProcessTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainProcessType.ClientID %>").focus();   
    }                
</script>
