<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPrintInfoType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPrintInfoType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPrintInfoType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPrintInfoTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainPrintInfoType.ClientID %>");   
    }
    
    function getMaintainPrintInfoTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainPrintInfoType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainPrintInfoType.ClientID %>")[document.getElementById("<%=drpMaintainPrintInfoType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPrintInfoTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainPrintInfoType.ClientID %>").value;
    }
    
    function checkMaintainPrintInfoTypeCmb()
    {
        if ( getMaintainPrintInfoTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPrintInfoTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainPrintInfoType.ClientID %>").focus();   
    }                
</script>

