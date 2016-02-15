<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainNeedSave.ascx.cs" Inherits="CommonControl_CmbMaintainNeedSave" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainNeedSave" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">


    function getMaintainNeedSaveCmbObj()
    {    
         return document.getElementById("<%=drpMaintainNeedSave.ClientID %>");   
    }
    
    function getMaintainNeedSaveCmbText()
    { 
        if (document.getElementById("<%=drpMaintainNeedSave.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainNeedSave.ClientID %>")[document.getElementById("<%=drpMaintainNeedSave.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainNeedSaveCmbValue()
    {
         return document.getElementById("<%=drpMaintainNeedSave.ClientID %>").value;
    }
    
    function checkMaintainNeedSaveCmb()
    {
        if ( getMaintainNeedSaveCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainNeedSaveCmbFocus()
    {
        document.getElementById("<%=drpMaintainNeedSave.ClientID %>").focus();   
    }                
</script>
