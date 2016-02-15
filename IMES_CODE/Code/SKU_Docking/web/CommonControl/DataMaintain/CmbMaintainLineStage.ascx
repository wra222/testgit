<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainLineStage.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainLineStage" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainLineStage" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainLineStageCmbObj()
    {    
         return document.getElementById("<%=drpMaintainLineStage.ClientID %>");   
    }
    
    function getMaintainLineStageCmbText()
    { 
        if (document.getElementById("<%=drpMaintainLineStage.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainLineStage.ClientID %>")[document.getElementById("<%=drpMaintainLineStage.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainLineStageCmbValue()
    {
         return document.getElementById("<%=drpMaintainLineStage.ClientID %>").value;
    }
    
    function checkMaintainLineStageCmb()
    {
        if ( getMaintainLineStageCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainLineStageCmbFocus()
    {
        document.getElementById("<%=drpMaintainLineStage.ClientID %>").focus();   
    }                
</script>

