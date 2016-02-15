<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:NoneEPIAtoEPIA page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-09  zhu lei               Create          
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NoneEPIAtoEPIA.aspx.cs" Inherits="PAK_NoneEPIAtoEPIA" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >  
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0">
                <tr>
                    <td style="width: 100px;">
                        <asp:Label ID="lblProdID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" ReplaceRegularExpression="" Width="99%" IsClear="true" IsPaste="true"/>
                    </td>
                </tr>
            </table>
            <div style="padding-left: 2px;">                
                <input type="hidden" id="hidProdID" runat="server" /><input type="hidden" id="hidPdLine" runat="server" />
                <button id="hidbtn" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_ServerClick"></button> 
            </div>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                
                </ContentTemplate>
            </asp:UpdatePanel>
        </div> 
    </div>
      <script language="javascript" type="text/javascript">
          var inputObj;
          var emptyPattern = /^\s*$/;

          window.onload = function() {
              inputObj = getCommonInputObject();
              getAvailableData("processFun");
          };
          
          function processFun(backData) {
              ShowInfo("");
              beginWaitingCoverDiv();
              document.getElementById("<%=hidProdID.ClientID %>").value = SubStringSN(backData, "CustSN");

              document.getElementById("<%=hidbtn.ClientID %>").click();
              
          }

          function initPage() {
              clearData();
              inputObj.value = "";
              getAvailableData("processFun");
              inputObj.focus();
          }

          function setCommonFocus() {
              endWaitingCoverDiv();
              inputObj.focus();
              inputObj.select();
          }

          function ExitPage() {
          }

          function ResetPage() {
              initPage();
              ShowInfo("");
          }                
    </script>    

</asp:Content>