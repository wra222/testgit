
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbException.ascx.cs" Inherits="CommonControl_CmbException" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpException" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = "Input Exception!";
        
    function getExceptionCmbObj()
    {    
       return document.getElementById("<%=drpException.ClientID %>");   
    }
    
    function getExceptionCmbText()
    {
        if (document.getElementById("<%=drpException.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpException.ClientID %>")[document.getElementById("<%=drpException.ClientID %>").selectedIndex].text;
        }
    }
    
    function getExceptionCmbValue()
    {
       return document.getElementById("<%=drpException.ClientID %>").value;  
    }
    
    function checkExceptionCmb()
    {
        if (getExceptionCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }
        return true;
    }
    
    function setExceptionCmbFocus()
    {
        document.getElementById("<%=drpException.ClientID %>").focus();   
    }    
                
</script>

