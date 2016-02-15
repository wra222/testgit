<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbConstValueTypeForType.ascx.cs" Inherits="CommonControl_CmbConstValueTypeForType" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpConstValueTypeForType" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getConstValueTypeCmbObj()
    {
        return document.getElementById("<%=drpConstValueTypeForType.ClientID %>");   
    }

    function getConstValueTypeCmbText()
    {
        if (document.getElementById("<%=drpConstValueTypeForType.ClientID %>").selectedIndex == -1)
        {
            return "";
        } 
        else
        {
            return document.getElementById("<%=drpConstValueTypeForType.ClientID %>")[document.getElementById("<%=drpConstValueTypeForType.ClientID %>").selectedIndex].text;
        }
    }

    function getConstValueTypeCmbValue()
    {
        return document.getElementById("<%=drpConstValueTypeForType.ClientID %>").value;  
    }

    function checkConstValueTypeCmb()
    {
        if (getConstValueTypeCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setConstValueTypeCmbFocus()
    {
        document.getElementById("<%=drpConstValueTypeForType.ClientID %>").focus();   
    }    
                
</script>

