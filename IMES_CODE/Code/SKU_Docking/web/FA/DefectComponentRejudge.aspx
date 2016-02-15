<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * 
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DefectComponentRejudge.aspx.cs" Inherits="DefectComponentRejudge" Title="Defect Component Rejudge" %>
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
                    <td colspan="4">
                        <input type="radio" id="Y" name="IsNeedUpload" value="10" checked="checked" />良品 
                        <input type="radio" id="N" name="IsNeedUpload" value="11" />不良品
                    </td>
                </tr>
                <tr>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblPartSerialNo" runat="server" CssClass="iMes_label_13pt" Text="Part Serial No:"></asp:Label>
                    </td>
                    <td align="left" id="txtPartSerialNo" style="width:20%"></td>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt" Text="Status:"></asp:Label>
                    </td>
                    <td align="left" id="txtStatus"></td>
                </tr>
                <tr>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblPartSN" runat="server" CssClass="iMes_label_13pt" Text="Part SN:"></asp:Label>
                    </td>
                    <td align="left" id="txtPartSN"></td>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblRecycleCount" runat="server" CssClass="iMes_label_13pt" Text="Recycle Count:"></asp:Label>
                    </td>
                    <td align="left" id="txtRecycleCount"></td>
                </tr>  
                <tr>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt" Text="PartNo:"></asp:Label>
                    </td>
                    <td align="left" id="txtPartNo"></td>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt" Text="Part Type:"></asp:Label>
                    </td>
                    <td align="left" id="txtPartType"></td>
                </tr>  
                <tr>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt" Text="Vendor:"></asp:Label>
                    </td>
                    <td align="left" id="txtVendor"></td>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblIECPn" runat="server" CssClass="iMes_label_13pt" Text="IECPn:"></asp:Label>
                    </td>
                    <td align="left" id="txtIECPn"></td>
                </tr>  
                <tr>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblDefcet" runat="server" CssClass="iMes_label_13pt" Text="Defcet:"></asp:Label>
                    </td>
                    <td align="left" id="txtDefcet"></td>
                    <td style="width:10%" align="right"></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td style="width:10%" align="right">
                         <asp:Label ID="lblComment" runat="server" CssClass="iMes_label_13pt" Text="Comment:"></asp:Label>
                    </td>
                    <td align="left" colspan="3">
                        <input type="text" id="txtComment" style="width:98%" />
                    </td>
                    
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
                <tr>
                    <td colspan="3"></td>
                    <td align="right">
                        <input id="btquery" type="button" runat="server" class="iMes_button" value="Log Query"
                           onclick="showQueryDialog()" onmouseover="this.className='iMes_button_onmouseover'" 
                           onmouseout="this.className='iMes_button_onmouseout'"/></td>
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
			var method ="";
			var comment="";
			
            ShowInfo("");
            beginWaitingCoverDiv();
            if (data == "9999") {
                if (sn == "") {
                    alert("請先刷入sn...");
                    CallNextInput();
                    inputObj.focus();
                    return;
                }
                method= $("input[name='IsNeedUpload']:checked").val();
                comment = $('#txtComment').val();
                PageMethods.Save(sn, method, comment, onSaveSucess, onSaveError);
            }
            else {
                if (sn != "") {
                    OnCancel();
                }
                sn = data;
				method = $("input[name='IsNeedUpload']:checked").val();
                PageMethods.GetDefectComponentInfo(sn, customer, method, editor, onGetDefectComponentInfoSucess, onGetDefectComponentInfoError);
            }
        }

        function onGetDefectComponentInfoSucess(result) {
         
            ShowInfo("");
            endWaitingCoverDiv();
            //  tdMBSno
            var defectInfoObj = result[0];
            $("#txtPartSerialNo").text(sn);
            $("#txtStatus").text(defectInfoObj.Status + "-" + result[2]);
            $("#txtPartSN").text(defectInfoObj.PartSn);
            $("#txtRecycleCount").text(result[1]);
            $("#txtPartNo").text(defectInfoObj.PartNo);
            $("#txtPartType").text(defectInfoObj.PartType);
            $("#txtVendor").text(defectInfoObj.Vendor);
            $("#txtIECPn").text(defectInfoObj.IECPn);
            $("#txtDefcet").text(defectInfoObj.DefectCode + "-" + defectInfoObj.DefectDescr);
            CallNextInput();
        }

        function onGetDefectComponentInfoError(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            CallNextInput();
        }

        function onSaveSucess(result) {

            ShowInfo("");
            endWaitingCoverDiv();
            //  tdMBSno
            $("#txtPartSerialNo").text("");
            $("#txtStatus").text("");
            $("#txtPartSN").text("");
            $("#txtRecycleCount").text("");
            $("#txtPartNo").text("");
            $("#txtPartType").text("");
            $("#txtVendor").text("");
            $("#txtIECPn").text("");
            $("#txtDefcet").text("");
            document.getElementById('txtComment').value = "";
            CallNextInput();
            ShowInfo("SUCCESS!", "green");
        }

        function onSaveError(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            CallNextInput();
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

        function showQueryDialog() {
            var paramArray = new Array();
            paramArray[0] = customer;
            window.showModalDialog("./DefectComponentRejudge_Query.aspx", paramArray, 'dialogWidth:1200px;dialogHeight:500px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
    </script>
</asp:Content>

