<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainFamilyForSA.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainFamilyForSA" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpFamily" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function <%=this.ClientID %>getFamilyCmbObj()
    {    
       return document.getElementById("<%=drpFamily.ClientID %>");   
    }
    
    function <%=this.ClientID %>getFamilyCmbText()
    {
        if (document.getElementById("<%=drpFamily.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpFamily.ClientID %>")[document.getElementById("<%=drpFamily.ClientID %>").selectedIndex].text;
        }
    }
    
    function <%=this.ClientID %>getFamilyCmbValue()
    {
       return document.getElementById("<%=drpFamily.ClientID %>").value;  
    }
    
    function <%=this.ClientID %>setFamilyCmbFocus()
    {
        document.getElementById("<%=drpFamily.ClientID %>").focus();   
    }    
                
</script>

