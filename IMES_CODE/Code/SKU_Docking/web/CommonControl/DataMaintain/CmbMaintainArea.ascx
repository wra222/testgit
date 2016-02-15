<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaintainArea.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaintainArea" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="drpMaintainArea" runat="server" Width="310px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
 function getAreaCmbObj()
 {
  return document.getElementById("<%=drpMaintainArea.ClientID %>");
 }
 
 function getAreaCmbText()
 {
  if(document.getElementById("<%=drpMaintainArea.ClientID %>").selecetdIndex==-1)
  {
   return "";
  }
  else
  {
   return document.getElementById("<%=drpMaintainArea.ClientID %>")[document.getElementById("<%=drpMaintainArea.ClientID %>").selectedIndex].text;
  }
 }
 
 function getAreaCmbValue()
 {
  return document.getElementById("<%=drpMaintainArea.ClientID %>").Value;
 }
 
 function checkAreaCmb()
 {
  if(getAreaCmbText()=="")
  {
   return false;
  }
  return true;
 }
 
 function setAreaCmbFocus()
 {
  document.getElementById("<%=drpMaintainArea.ClientID %>").focus();
 }
 
 function getMyAreaCmbObj()
{
 try
 {
  return document.getElementById("<%=drpMaintainArea.ClientID %>");
 }
 catch(e)
 {
  alert(e.description);
 }
}
</script>

