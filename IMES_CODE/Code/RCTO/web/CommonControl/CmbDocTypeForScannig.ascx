
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbDocTypeForScannig.ascx.cs" Inherits="CommonControl_CmbDocTypeForScaning" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpDocType" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = "Input Doc_Type!";
        
    function getDocTypeCmbObj()
    {    
       return document.getElementById("<%=drpDocType.ClientID %>");   
    }
    
    function getDocTypeCmbText()
    {
        if (document.getElementById("<%=drpDocType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpDocType.ClientID %>")[document.getElementById("<%=drpDocType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getDocTypeCmbValue()
    {
       return document.getElementById("<%=drpDocType.ClientID %>").value;  
    }
    
    function checkDocTypeCmb()
    {
        if (getDocTypeCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }
        return true;
    }
    
    function setDocTypeCmbFocus()
    {
        document.getElementById("<%=drpDocType.ClientID %>").focus();   
    }    
                
</script>

