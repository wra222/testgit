<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainAllTypeFamily.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainAllTypeFamily" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
 <asp:DropDownList ID="drpMaintainFamily" runat="server" Width="326px" AutoPostBack="true">
 </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">
function getFamilyCmbObj()
{
 return document.getElementById("<%=drpMaintainFamily.ClientID %>");
}
function <%=this.ClientID %>getAFamilyCmbObj()
{
 return document.getElementById("<%=drpMaintainFamily.ClientID %>");
}
function getFamilyCmbText()
{
 if(document.getElementById("<%=drpMaintainFamily.ClientID %>").selectedIndex == -1)
 {
  return "";
 }
 else
 {
  return document.getElementById("<%=drpMaintainFamily.ClientID %>")[document.getElementById("<%=drpMaintainFamily.ClientID %>").selectEdIndex].text;
 }
}

function getFamilyCmbValue()
{
 return document.getElementById("<%=drpMaintainFamily.ClientID %>").value;
}

function checkFamilyCmb()
{
 if(getFamilyCmbText() == "")
 {
  return false;
 }
 
  return true;
}
function getMyFamilyCmbObj()
{
 try
 {
  return document.getElementById("<%=drpMaintainFamily.ClientID %>");
 }
 catch(e)
 {
  alert(e.description);
 }
}
</script>
