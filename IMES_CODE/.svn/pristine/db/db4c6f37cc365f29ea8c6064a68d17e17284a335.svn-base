
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbCarrier.ascx.cs" Inherits="CommonControl_CmbCarrier" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpCarrier" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = "Input Carrier!";

    function getCarrierCmbObj()
    {
        return document.getElementById("<%=drpCarrier.ClientID %>");   
    }

    function getCarrierCmbText()
    {
        if (document.getElementById("<%=drpCarrier.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpCarrier.ClientID %>")[document.getElementById("<%=drpCarrier.ClientID %>").selectedIndex].text;
        }
    }

    function getCarrierCmbValue()
    {
        return document.getElementById("<%=drpCarrier.ClientID %>").value;  
    }

    function checkCarrierCmb()
    {
        if (getCarrierCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }
        return true;
    }

    function setCarrierCmbFocus()
    {
        document.getElementById("<%=drpCarrier.ClientID %>").focus();   
    }    
                
</script>

