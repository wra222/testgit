
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WeightTypeControl.ascx.cs" Inherits="CommonControl_WeightTypeControl" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline"  UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="DropDownList1" runat="server" onchange="getobjMSComm()">
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>旧秤</asp:ListItem>
            <asp:ListItem>新秤</asp:ListItem>
        </asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>



<script type="text/javascript" language="javascript">


    function getWeightTypeValue()
    {
       return document.getElementById("<%=DropDownList1.ClientID %>").value;  
    }
    
    function setWeightTypeFocus()
    {
        document.getElementById("<%=DropDownList1.ClientID %>").focus();   
    } 
    
    function getobjMSComm()
    {
        if (getWeightTypeValue() == "旧秤") {
           setobjMSCommPara1();
           alert(getWeightTypeValue());
        }
        else if(getWeightTypeValue() == "新秤")
        {
            setobjMSCommPara2();
            alert(getWeightTypeValue());
        }
    }  
    
</script>