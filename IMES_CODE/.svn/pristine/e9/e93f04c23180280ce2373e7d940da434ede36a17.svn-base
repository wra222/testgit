<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPrintInfoBuild.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPrintInfoBuild" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPrintInfoBuild" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPrintInfoBuildCmbObj()
    {    
         return document.getElementById("<%=drpMaintainPrintInfoBuild.ClientID %>");   
    }
    
    function getMaintainPrintInfoBuildCmbText()
    { 
        if (document.getElementById("<%=drpMaintainPrintInfoBuild.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainPrintInfoBuild.ClientID %>")[document.getElementById("<%=drpMaintainPrintInfoBuild.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPrintInfoBuildCmbValue()
    {
         return document.getElementById("<%=drpMaintainPrintInfoBuild.ClientID %>").value;
    }
    
    function checkMaintainPrintInfoBuildCmb()
    {
        if ( getMaintainPrintInfoBuildCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPrintInfoBuildCmbFocus()
    {
        document.getElementById("<%=drpMaintainPrintInfoBuild.ClientID %>").focus();   
    }                
</script>


