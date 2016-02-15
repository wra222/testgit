<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainDescType.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainDescType" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpDescTypeList" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getDescTypeListCmbObj()
    {
        return document.getElementById("<%=drpDescTypeList.ClientID %>");   
    }

    function getDescTypeListCmbText()
    {
        if (document.getElementById("<%=drpDescTypeList.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpDescTypeList.ClientID %>")[document.getElementById("<%=drpDescTypeList.ClientID %>").selectedIndex].text;
        }
    }

    function getDescTypeListCmbSelectedIndex() {
        return document.getElementById("<%=drpDescTypeList.ClientID %>").selectedIndex;                  
    }

    function getDescTypeListCmbValue()
    {
        return document.getElementById("<%=drpDescTypeList.ClientID %>").value;
    }

    function checkDescTypeListCmb()
    {
        if (getDescTypeListCmbText() == "")
        {
            return false;
        }

        return true;
    }

    function setDescTypeListCmbFocus()
    {
        document.getElementById("<%=drpDescTypeList.ClientID %>").focus();   
    }                
</script>
