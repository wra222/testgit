<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFamilyForSMT.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainFamilyForSMT" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpLine" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function <%=this.ClientID %>getFamilyCmbObj()
    {    
       return document.getElementById("<%=drpLine.ClientID %>");   
    }
    
    function <%=this.ClientID %>getFamilyCmbText()
    {
        if (document.getElementById("<%=drpLine.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpLine.ClientID %>")[document.getElementById("<%=drpLine.ClientID %>").selectedIndex].text;
        }
    }
    
    function <%=this.ClientID %>getFamilyCmbValue()
    {
       return document.getElementById("<%=drpLine.ClientID %>").value;  
    }
    
    function <%=this.ClientID %>setFamilyCmbFocus()
    {
        document.getElementById("<%=drpLine.ClientID %>").focus();   
    }    
                
</script>

