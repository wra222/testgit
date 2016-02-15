<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * 
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DismantlePilotRun.aspx.cs" Inherits="DismantlePilotRun" Title="Dismantle Pilot Run" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  <style type="text/css">
   .tdTxt {
    color: blue;
  }
  </style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0">
             
               
                <tr>
                  <td style="width:10%" align="left">
                          <asp:Label ID="lblDismantleType" runat="server" CssClass="iMes_label_13pt" Text="Dismantle Type:"></asp:Label>
                    </td> 
                    <td colspan="3">
                         <asp:DropDownList ID="dropStage" runat="server" Width="95%">
                         </asp:DropDownList>
                    </td>
                    <td   colspan="2">
                      <input id="btnDismantle" type="button" value="Dismantle" onclick="Dismantle()" disabled="disabled"/>
                    </td>
        
                </tr>
                <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt" Text="MBSno:"></asp:Label>
                   </td>
                   <td   align="left" id="tdMBSno" style="color:Blue" > </td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblProdId" runat="server" CssClass="iMes_label_13pt" Text="ProdId:"></asp:Label>
                   </td>
                   <td  align="left" id="tdProdId" ></td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblCustomerSN" runat="server" CssClass="iMes_label_13pt" Text="Customer SN:"></asp:Label>
                   </td>
                   <td  align="left" id="tdCustomerSN" ></td>
                </tr>  
             
                  <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblPilotMo" runat="server" CssClass="iMes_label_13pt" Text="Pilot Mo:"></asp:Label>
                   </td>
                   <td   align="left" id="tdPilotMo" > </td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblStage" runat="server" CssClass="iMes_label_13pt" Text="Stage:"></asp:Label>
                   </td>
                   <td  align="left" id="tdStage" ></td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblMoType" runat="server" CssClass="iMes_label_13pt" Text="Mo Type:"></asp:Label>
                   </td>
                   <td  align="left" id="tdMoType" ></td>
                </tr>  
          
                 <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                   </td>
                   <td   align="left" id="tdModel" > </td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt" Text="Qty:"></asp:Label>
                   </td>
                   <td  align="left" id="tdQty" ></td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblCombinedQty" runat="server" CssClass="iMes_label_13pt" Text="CombinedQty:"></asp:Label>
                   </td>
                   <td  align="left" id="tdCombinedQty" ></td>
                </tr>  
                
                
                 <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblState" runat="server" CssClass="iMes_label_13pt" Text="State:"></asp:Label>
                   </td>
                   <td   align="left" id="tdState" > </td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblStartPlanDate" runat="server" CssClass="iMes_label_13pt" Text="Start Plan Date:"></asp:Label>
                   </td>
                  <td   align="left" id="tdStartPlanDate" > </td>
                </tr>  
                
                <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt" Text="PartNo:"></asp:Label>
                   </td>
                   <td   align="left" id="tdPartNo" colspan="5" > </td>
                 </tr>  
                 <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt" Text="Vendor:"></asp:Label>
                   </td>
                   <td   align="left" id="tdVendor" colspan="5" > </td>
                 </tr> 
                 <tr>
                   <td style="width:10%" align="left">
                         <asp:Label ID="lblCauseDescr" runat="server" CssClass="iMes_label_13pt" Text="Cause Descr:"></asp:Label>
                   </td>
                   <td   align="left" id="tdCauseDescr" colspan="5" > </td>
                 </tr> 
             </table>
            
             
                        
        </div>
        <div id="div3">
             <table width="100%">
               
                <tr>
                    <td style="width:10%" align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel" Text="Data Entry:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                  
                </tr>
             </table>   
        </div> 
       
        
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
       
            <ContentTemplate>
                <input id="TypeValue" type="hidden" runat="server" />
                <input id="LimitSpeed" type="hidden" runat="server" />
                <input id="HoldStation" type="hidden" runat="server" />
                <input id="SpeedExpression" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
   
    <script language="javascript" type="text/javascript">
       
        var editor;
        var customer;
		var station;
		var sn = "";
		var custsn = "";
		var mbsn = "";
		var mono = "";
		var proId = "";
        window.onload = function() {
      
        $('td[id^="td"]').css("color", "blue");
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            inputObj.focus();
        };

        function input(data) 
        {
            ShowInfo("");
            var stage = $('#<%=dropStage.ClientID %> option:selected').val();
            if (stage == "") 
            {
                alert("Please select dismantle type");
                getAvailableData("input");
                return;
            }
            beginWaitingCoverDiv();
            if (sn != "") {
                OnCancel();
            }
            sn = data;
            PageMethods.GetPilotMoInfo(data, stage, customer, "", editor, onGetPilotMoInfoSucess, onGetPilotMoInfoError);
          
        }

        function onGetPilotMoInfoSucess(result) {
         
            ShowInfo("");
            endWaitingCoverDiv();
            //  tdMBSno
            var piloMoObj = result[0];
            $("#tdPilotMo").text(piloMoObj.mo);
            mono = piloMoObj.mo;
            $("#tdCauseDescr").text(piloMoObj.mo);
            $("#tdStage").text(piloMoObj.stage);
            $("#tdMoType").text(piloMoObj.moType);
            $("#tdModel").text(piloMoObj.model);
            $("#tdQty").text(piloMoObj.qty);
            $("#tdCombinedQty").text(piloMoObj.combinedQty);
            $("#tdState").text(piloMoObj.state);
            $("#tdState").text(piloMoObj.state);
            var d = piloMoObj.planStartTime.format("yyyy-MM-dd");
            $("#tdStartPlanDate").text(d);
            $("#tdPartNo").text(piloMoObj.partNo);
            $("#tdCauseDescr").text(piloMoObj.causeDescr);
            $("#tdVendor").text(piloMoObj.vendor);
            $("#tdMBSno").text(result[1]);
            $("#tdProdId").text(result[2]);
            $("#tdCustomerSN").text(result[3]);
            custsn = result[3];
            proId = result[2];
            mbsn = result[1];
            $("#btnDismantle").attr("disabled", false);
            CallNextInput();
        }

        function onGetPilotMoInfoError(result) {
            $('td[id^="td"]').text('');
            $("#btnDismantle").attr("disabled", true);    
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            CallNextInput();
        }
        function Dismantle() {
            if (confirm('Do you really want to do dismantle?')) {
                beginWaitingCoverDiv();
                PageMethods.Dismantle(sn, onDismantleSuccess, onDismantleFail);
            }
            else
            {CallNextInput(); }
        }
        function onDismantleSuccess(result) 
        {
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            ShowInfo("SUCCESS!! MBSno:"+mbsn+"/ProductID:"+proId+"/CUSTSN:"+custsn+" 已與 Pilot Mo:"+mono+" 的解除綁定", "green");
            $("#btnDismantle").attr("disabled", true);
            $('td[id^="td"]').text('');
            initPage();
            CallNextInput();
        }
        function onDismantleFail(result) {
            endWaitingCoverDiv();
            $("#btnDismantle").attr("disabled", true);
            $('td[id^="td"]').text('');
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        } 
        function initPage() {
            sn = "";
            custsn = "";
            mbsn = "";
            mbno = "";
            proId = "";
            $('td[id^="td"]').text('');
        }
        window.onbeforeunload = function()
        {
            if (sn != "") {
                OnCancel();
                initPage();
            }
        };   
       
        function OnCancel() {
             PageMethods.Cancel(sn);
        }
        function CallNextInput() {
            getAvailableData("input");
            inputObj.focus();
        }
    </script>
</asp:Content>

