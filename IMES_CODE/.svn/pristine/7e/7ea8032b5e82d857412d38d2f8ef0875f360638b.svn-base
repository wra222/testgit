<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainProcessForModelProcess.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainProcessForModelProcess" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainProcess" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainProcessCmbObj()
    {    
         return document.getElementById("<%=drpMaintainProcess.ClientID %>");   
    }
    
    function getMaintainProcessCmbText()
    { 
        if (document.getElementById("<%=drpMaintainProcess.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainProcess.ClientID %>")[document.getElementById("<%=drpMaintainProcess.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainProcessCmbValue()
    {
         return document.getElementById("<%=drpMaintainProcess.ClientID %>").value;
    }
    
    function checkMaintainProcessCmb()
    {
        if ( getMaintainProcessCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainProcessCmbFocus()
    {
        document.getElementById("<%=drpMaintainProcess.ClientID %>").focus();   
    }                
</script>
