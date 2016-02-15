<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Reprint Pallet Verify FDE Only
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-11   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. Reprint Pallet Verify FDE Only  列印所有Label；
 *           
 */
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReprintPalletVerifyFDEOnly.aspx.cs" Inherits="PAK_ReprintPalletVerifyFDEOnly"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path=  "~/PAK/Service/WebServicePalletVerifyFDEOnly.asmx" />
    </Services>
</asp:ScriptManager>   
<style type="text/css">
</style>

<div id="divPalletVerify" style="z-index: 0; width:95%" class="iMes_div_center">
       
        <br />
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">

              <tr>   
                <td width="30%" align="left" >
                   &nbsp;  <asp:Label id="lblPalletNo" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                 
                <td width="70%" align="left" >
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                            InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/>
                </td>               
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr style="height: 30px" align="left" valign="middle">
                <td width="20%">
                   &nbsp;   <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="80%">
                        <textarea id="txtReason" rows="5" style="width:98%;" 
                    runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                    onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="2"></textarea>
                </td>
             </tr>          
        
            <tr><td colspan="2">&nbsp;</td></tr>
            
             <tr style="height:30px">
                 <td width="20%" align="left"></td>
                 <td align="right">
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                            tabindex ="3"/> 
                            &nbsp; 
                </td>
                
             </tr>
             <tr><td colspan="2">&nbsp;</td></tr>
             <tr>
                 <td width="20%" align="left"></td>
                 <td align="right" >
                    <input id="btnPrint" type="button"  runat="server" onclick="Reprint()" style="height:auto"
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    class=" iMes_button"  tabindex="4"/>
                     &nbsp; 
               </td>
             </tr>
            <asp:UpdatePanel ID="updatePanel" runat="server">
            </asp:UpdatePanel> 
        </table>
        
        <br />                           
    </div>

 <script type="text/javascript" language="javascript">

     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

     var msgPalletNoNull = '<%=this.GetLocalResourceObject(Pre + "_msgPalletNoNull").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
     var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
     var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';

     var editor = "<%=UserId%>";
     var customer = '<%=Customer %>';
     var station = '<%=Request["Station"] %>';
     var pCode = '<%=Request["PCode"] %>';
     var accountId = '<%=Request["AccountId"] %>';
     var login = '<%=Request["Login"] %>';
     
     var line = "";
    
     var palletNo = "";
     var strReason = ""; 
     var inputSNControl;

     window.onload = function() {
        inputSNControl = getCommonInputObject();
        DisplayInfoText();
        setStart();
     };

     function setStart() {
         palletNo = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");

     }

     function PlaySound() {
         var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
         var obj = document.getElementById("bsoundInModal");
         obj.src = sUrl;
     }

     function PlaySoundClose() {

         var obj = document.getElementById("bsoundInModal");
         obj.src = "";
     }
     
     function checkReprintReason()      
     {
         strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

//         if ((strReason == "") || (strReason.length == 0) || (strReason == null)) {
//             ShowInfo(msgReasonNull);
//             PlaySound();
//             reasonFocus();
//             return false;
//         }
//         else return true;
         return true;
     }
     
     function Reprint() {
         PlaySoundClose();
         ShowInfo("");
         if (inputSNControl.value != "") {
             if (checkReprintReason()) {
                 var printItemlist = getPrintItemCollection();

                 if (printItemlist == "" || printItemlist == null) {
                     ShowInfo(msgPrintSettingPara);
                     PlaySound();
                     return;
                 }

                 beginWaitingCoverDiv();
                 palletNo = inputSNControl.value.trim();
                 WebServicePalletVerifyFDEOnly.reprint(palletNo, strReason, line, editor, station, customer, printItemlist, onSucceed, onFailed);
             }
             else {
                 getAvailableData("processDataEntry");
             }
         }
         else {
             ShowInfo(msgPalletNoNull);
             PlaySound();
             inputSNControl.value = "";
             inputSNControl.focus();
             getAvailableData("processDataEntry");
         } 
     }

     function processDataEntry(inputStr) {
         Reprint();
     }

     function ismaxlength(obj) {
         var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
         if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");
             ShowInfo(msgInputMaxLength1 + mlength + msgInputMaxLength2);
             PlaySound();
             obj.value = obj.value.substring(0, mlength);
             reasonFocus();
         }
     }

     function imposeMaxLength(obj) {
         var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
         return (obj.value.length < mlength);
     }

     function reasonFocus() {
         document.getElementById("<%=txtReason.ClientID %>").focus();
     }

     function clearReason() {
         document.getElementById("<%=txtReason.ClientID %>").value = "";
         strReason = "";
     }

    
     function onSucceed(result) {
         if (result == null) {
             endWaitingCoverDiv();
             ShowErrorMessage(msgSystemError);
         }
         else if (result.length == 2 && result[0] == SUCCESSRET) {

//====================print=======================           
             for (var i = 0; i < result[1].length; i++) {

                 setPrintItemListParam(result[1][i]);    
             }
             printLabels(result[1], false);
//====================print=======================
             var SuccessItem ="[" + palletNo + "]";
             ResetPage();
             ShowSuccessfulInfo(true,SuccessItem + " " + msgSuccess);
         }
     }

     
     function onFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     function setPrintItemListParam(backPrintItemList) {
         //============================================generate PrintItem List========================================== 
         /*
         * Function Name: setPrintParam
         * @param: printItemCollection
         * @param: labelType
         * @param: keyCollection(Client: Array of string.    Server: List<string>)
         * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
         */
         //============================================generate PrintItem List==========================================

         var lstPrtItem = new Array();
         var keyCollection = new Array();
         var valueCollection = new Array();

         lstPrtItem[0] = backPrintItemList;

         keyCollection[0] = "@palletno";

         valueCollection[0] = generateArray(palletNo);


         if (backPrintItemList.LabelType == "Bulk Delivery Label") {

             setPrintParam(lstPrtItem, "Bulk Delivery Label", keyCollection, valueCollection);
         }

         else if (backPrintItemList.LabelType == "Bulk Pallet SN Label") {

             setPrintParam(lstPrtItem, "Bulk Pallet SN Label", keyCollection, valueCollection);
         }

     }
               

     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
         palletNo = "";
         inputSNControl.value = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");

     }


     function ShowErrorMessage(result) {
         endWaitingCoverDiv();

         ShowInfo(result);
         PlaySound();
         inputSNControl.value = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");
     }


     function showPrintSettingDialog() {
         showPrintSetting(station, pCode);
     }


     function Tab(reasonPara) {
         if (event.keyCode == 9) {
             inputSNControl.focus();
             event.returnValue = false;
         }
     }

     function ExitPage() {
     }

     function ResetPage() {
         ExitPage();
         ClearData();
     }

     window.onunload = function() {
         ExitPage();
     };
     
    </script>
</asp:Content>



