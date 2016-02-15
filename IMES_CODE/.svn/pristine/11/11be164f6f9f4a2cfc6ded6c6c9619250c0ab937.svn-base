<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainDept.ascx.cs" Inherits="CommonControl_cmbMaintainDept" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMaintainDept" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getMaintainDeptCmbObj()
    {    
         return document.getElementById("<%=drpMaintainDept.ClientID %>");   
    }

    function getMaintainDeptCmbText()
    { 
        if (document.getElementById("<%=drpMaintainDept.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpMaintainDept.ClientID %>")[document.getElementById("<%=drpMaintainDept.ClientID %>").selectedIndex].text;
        }
    }
    
    function getMaintainDeptCmbValue()
    {
         return document.getElementById("<%=drpMaintainDept.ClientID %>").value;
    }
    
    function checkMaintainDeptCmb()
    {
        if (getMaintainDeptCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setMaintainDeptCmbFocus()
    {
        document.getElementById("<%=drpMaintainDept.ClientID %>").focus();   
    }                
</script>
