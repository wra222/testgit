<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbBomNodeType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbBomNodeType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpBomNodeType" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getBomNodeTypeObj()
    {    
         return document.getElementById("<%=drpBomNodeType.ClientID %>");   
    }

    function getBomNodeTypeText()
    { 
        if (document.getElementById("<%=drpBomNodeType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpBomNodeType.ClientID %>")[document.getElementById("<%=drpBomNodeType.ClientID %>").selectedIndex].text;
        }
    }

    function getBomNodeTypeValue()
    {
         return document.getElementById("<%=drpBomNodeType.ClientID %>").value;
    }

    function checkBomNodeType()
    {
        if ( getMaintainPartNodeTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setBomNodeTypeFocus()
    {
        document.getElementById("<%=drpBomNodeType.ClientID %>").focus();   
    }                
</script>