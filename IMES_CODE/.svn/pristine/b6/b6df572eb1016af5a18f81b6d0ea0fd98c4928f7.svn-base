<%--
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: CmbStagePdLine.ascx  user control
 *             
 * Update: 
 * Date         Name                  Reason 
 * ==========   ================      =============
 * 2011-10-21   Wang ShaoHua          Create                    
 * Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbStagePDLine.ascx.cs"
    Inherits="CommonControl_DataMaintain_CmbStagePDLine" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="drpMaintainPdLine" runat="server" Width="310px" AutoPostBack="true">
        </asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" language="javascript">
function getPdLineCmbObj()
{
 return document.getElementById("<%=drpMaintainPdLine.ClientID %>");
}
function getPdLineCmbText()
{
 if(document.getElementById("<%=drpMaintainPdLine %>").selectedIndex==-1)
 {
  return "";
 }
 else
 {
  return document.getElementById("<%=drpMaintainPdLine.ClientID %>")[document.getElementById("<%=drpMaintainPdLine.ClientID %>").selectedIndex].text;
 }
}

function getPdLineCmbValue()
{
 return document.getElementById("<%=drpMaintainPdLine.ClientID %>").Value;
}

function checkPdLineCmb()
{
 if(getPdLineCmbText()=="")
 {
  return false;
 }
 return true;
}

function setPdLineCmbFocus()
{
 document.getElementById("<%=drpMaintainPdLine.ClientID %>").focus();
}

function <%=this.ClientID %>getAPdLineCmbObj()
{
 return document.getElementById("<%=drpMaintainPdLine.ClientID %>");
}

function setPdLineFocus()
{
 document.getElementById("<%=drpMaintainPdLine.ClientID %>").focus();
}
</script>