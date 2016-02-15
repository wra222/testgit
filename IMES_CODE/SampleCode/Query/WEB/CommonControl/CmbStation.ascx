
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbStation.ascx.cs" Inherits="CommonControl_CmbStation" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpStation" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputStation") %>';;
        
    function getStationCmbObj()
    {    
       return document.getElementById("<%=drpStation.ClientID %>");   
    }
    
    function getStationCmbText()
    {
        if (document.getElementById("<%=drpStation.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpStation.ClientID %>")[document.getElementById("<%=drpStation.ClientID %>").selectedIndex].text;
        }
    }
    function getStationCmbValue() {
        if (document.getElementById("<%=drpStation.ClientID %>").selectedIndex == -1) {
            return "";
        } else {
            return document.getElementById("<%=drpStation.ClientID %>")[document.getElementById("<%=drpStation.ClientID %>").selectedIndex].value;
        }
    }
    function getStationCmbValue()
    {
       return document.getElementById("<%=drpStation.ClientID %>").value;  
    }
    
    function checkStationCmb()
    {
        if (getStationCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }
    
    function setStationCmbFocus()
    {
        document.getElementById("<%=drpStation.ClientID %>").focus();   
    }    
                
</script>

