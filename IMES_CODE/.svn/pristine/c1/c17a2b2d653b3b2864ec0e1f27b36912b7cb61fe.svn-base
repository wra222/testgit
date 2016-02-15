<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelBOMRelation.aspx.cs" Inherits="DataMaintain_ModelBOMRelation" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

<style type="text/css">
body
{
    resizable: no;
    overflow-x: hidden;
    overflow-y: hidden;
}
.ColorB
{
    background-color: #99CDFF;
}
.iMes_button_MainTainModelBOM2
{
	cursor:hand; 
	height: 23px;
	width:78px;
}
</style>
<link href="CSS/dataMaintain.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>


<div id="container" style="width: 99%; border: solid 0px red; margin: 0 auto;">
  <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ><ContentTemplate>
  <table width="100%">        
  <tr>
  <td width="20%">
      <%--  left table  --%>      
      <asp:Label ID="lblTree" runat="server" CssClass="iMes_label_13pt"></asp:Label><hr />
      <asp:Panel ID="Panel1" runat="server" scrollbars=Both style="width: 99%;height: 470px;">
      <asp:TreeView ID="dTree" runat="server" 
          onselectednodechanged="dTree_SelectedNodeChanged" ShowLines="True">
      </asp:TreeView>
      </asp:Panel>
      <%--  left table end --%>
  </td>
  <td width="80%" valign="top">
  <%--  right table  --%>
      <table class="ColorB" border=0>
        <tr>
          <td width="190px"><asp:Label ID="lblParentBOMNodeType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
          <td width="320px">
          <asp:DropDownList id="selBOMNodeType"  runat="server" Width="200px" TabIndex="1"></asp:DropDownList>
          </td>
          <td width="90px"><button id="btnAdd" runat="server" type="button" onclick="if(clkAdd())" class="iMes_button_MainTainModelBOM2"    onserverclick="btnAdd_ServerClick" ></button></td>
        </tr>
        <tr>
          <td ><asp:Label ID="lblBOMNodeType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
          <td >
          <asp:DropDownList id="selChildBOMNodeType"  runat="server" Width="200px" TabIndex="2"></asp:DropDownList>
          </td>
          <td ><button id="btnSave" runat="server" type="button" onclick="if(clkSave())" class="iMes_button_MainTainModelBOM2"    onserverclick="btnSave_ServerClick"></button></td>
        </tr>
        <tr>
          <td ><asp:Label ID="lblBOMNodeDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
          <td ><asp:TextBox ID="BOMNodeDescr" runat="server"   MaxLength="255" Width="290px" TabIndex="3" SkinId="textBoxSkin" ></asp:TextBox></td>
          <td ><button id="btnDelete" runat="server" type="button" onclick="if(clkDelete())" class="iMes_button_MainTainModelBOM2"    onserverclick="btnDelete_ServerClick"></button></td>
        </tr>
      </table>
  </td>
  </tr>
  </table>
<input type="hidden" id="hidOldBOMNodeType" runat="server" />
<input type="hidden" id="hidOldChildBOMNodeType" runat="server" />
  </ContentTemplate>
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
    <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
    <asp:AsyncPostBackTrigger ControlID="dTree" EventName="SelectedNodeChanged" />
  </Triggers>
  </asp:UpdatePanel>
  <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
  </asp:UpdatePanel>

  <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
    <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
        <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
    </table>
</div>

<script type="text/javascript">
    window.onload = function() {
        msgDel = "<%=pmtDel%>";
        msgEmpty = "<%=pmtEmpty%>";
    };
    function clickButton() {
        switch (event.srcElement.id) {
            case "btnClose.ClientID ":

                if (clickClose() == false) {
                    return false;
                }
                break;
        }
        ShowWait();
        return true;
    }

    function DataChkEmpty() {
        var ddlNode = document.getElementById("<%=selBOMNodeType.ClientID %>");
        var ddlChildNode = document.getElementById("<%=selChildBOMNodeType.ClientID %>");
        var valNode = ddlNode.options[ddlNode.selectedIndex].value;
        var valChildNode = ddlChildNode.options[ddlChildNode.selectedIndex].value;
        if ('' == valNode || '' == valChildNode) {
            alert(msgEmpty);
            return false;
        }
        return true;
    }
    function clkAdd() {
        if (!DataChkEmpty()) return false;
        return true;
    }
    function clkSave() {
        if (!DataChkEmpty()) return false;
        return true;
    }
    function clkDelete() {
        if (!DataChkEmpty()) return false;
        var ddlNode = document.getElementById("<%=selBOMNodeType.ClientID %>");
        var ddlChildNode = document.getElementById("<%=selChildBOMNodeType.ClientID %>");
        var valNode = ddlNode.options[ddlNode.selectedIndex].value;
        var valChildNode = ddlChildNode.options[ddlChildNode.selectedIndex].value;
        var ret = confirm(msgDel + '\r\nBOMNodeType= ' + valNode +
        '\r\nChildBOMNodeType= ' + valChildNode);
        if (!ret) {
            return false;
        }
        //ShowWait();
        return true;
    }

    function DealHideWait() {
        HideWait();
    }
    
</script>

</asp:Content>
