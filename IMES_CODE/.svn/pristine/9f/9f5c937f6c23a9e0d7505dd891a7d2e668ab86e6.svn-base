<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainPartTypeAll.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainPartNodeType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainPartNodeType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainPartTypeCmbObj()
    {    
         return document.getElementById("<%=drpMaintainPartNodeType.ClientID %>");   
    }
    
    function getMaintainPartTypeCmbText()
    { 
        if (document.getElementById("<%=drpMaintainPartNodeType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainPartNodeType.ClientID %>")[document.getElementById("<%=drpMaintainPartNodeType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainPartNodeTypeCmbValue()
    {
         return document.getElementById("<%=drpMaintainPartNodeType.ClientID %>").value;
    }
    
    function checkMaintainPartNodeTypeCmb()
    {
        if ( getMaintainPartNodeTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainPartNodeTypeCmbFocus()
    {
        document.getElementById("<%=drpMaintainPartNodeType.ClientID %>").focus();   
    }                
</script>