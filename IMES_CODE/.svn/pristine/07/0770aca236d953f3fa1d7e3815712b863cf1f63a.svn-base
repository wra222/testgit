<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Reprint Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-10   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. Reprint Pallet Verify 列印所有Label；
 *           
 */
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReprintMBSplit.aspx.cs" Inherits="PAK_ReprintMBSplit"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path=  "~/SA/Service/WebServiceMBSplit.asmx" />
    </Services>
</asp:ScriptManager>   


<div id="divMBSplit" style="z-index: 0; width:95%" class="iMes_div_center">
       
       
        <br />
             
        <table width="90%" cellpadding="0" cellspacing="0" border="0" align="center">
             <tr style="height:30px">   
                <td width="20%" align="left" >
                   &nbsp;  <asp:Label id="lblMBSno" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                 
                <td width="70%" valign="middle" align="left" >
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                            InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/> 
                           
                </td>
               
              
            </tr>

            <tr><td colspan="2">&nbsp;</td></tr>
            
            <tr align="left" valign="middle">
                <td width="20%" align="left" >
                   &nbsp;   <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="70%" align="left" valign="middle">
                        <textarea id="txtReason" rows="5" style="width:98%;" 
                    runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                    onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="2"></textarea>
                </td>
             </tr>   
             
             <tr><td colspan="2">&nbsp;</td></tr>
              
              <tr>
                 <td width="20%" align="left"></td>
                 <td width="70%" align="right">
                  &nbsp;  <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                            tabindex ="3"/>&nbsp;
                 </td>
               </tr>
               
               <tr><td colspan="2">&nbsp;</td></tr>
              
              <tr>
                 <td width="20%" align="left"></td>
                 <td width="70%" align="right">
                    &nbsp;<input id="btnPrint" type="button"  runat="server" onclick="Reprint()" style="height:auto"
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    class=" iMes_button"  tabindex="4"/>&nbsp;
                </td>
             </tr>       
        </table>
        
        <br />
        
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel> 
    </div>

 <script type="text/javascript" language="javascript">

     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

     var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString() %>';
     var msgMBSnoBit = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoBit").ToString() %>';
     var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
     var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
     var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';

     var line = "";
     var station = "";
     var MBSno = "";
     var strReason = "";
     var inputSNControl;
     var station = '<%=Request["Station"] %>';
     var pcode = '<%=Request["PCode"] %>';
     var editor = "<%=UserId%>";
     var customer = "<%=Customer%>";
     
     window.onload = function() {
        inputSNControl = getCommonInputObject();
        DisplayInfoText();
        setStart();
     };

     function setStart() {
          MBSno = "";
          inputSNControl.focus();
          getAvailableData("processDataEntry");

     }

     function checkReprintReason()      
     {
         strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

//         if ((strReason == "") || (strReason.length == 0) || (strReason == null)) {
//             alert(msgReasonNull);
//             reasonFocus();
//             return false;
//         }
//         else return true;
         return true;
     }
     
     function Reprint() {
         ShowInfo("");
         if (inputSNControl.value != "") {
             if (checkReprintReason()) {
                 if (checkMBSno()) {
                     var printItemlist = getPrintItemCollection();

                     if (printItemlist == "" || printItemlist == null) {
                         alert(msgPrintSettingPara);
                         return;
                     }

                     beginWaitingCoverDiv();
                     
                     WebServiceMBSplit.reprint(MBSno, strReason, line, editor, station, customer, printItemlist, onSucceed, onFailed);
                 }
                 else {
                     inputSNControl.value = "";
                     inputSNControl.focus();
                     getAvailableData("processDataEntry");
                 }
             }
             else {
                 getAvailableData("processDataEntry");
             }
         }
         else {
             alert(msgMBSnoNull);
             inputSNControl.value = "";
             inputSNControl.focus();
             getAvailableData("processDataEntry");
         } 
     }

     function processDataEntry(inputStr) {
         Reprint();
     }

     function checkMBSno() {
         MBSno = inputSNControl.value.trim();
         if (MBSno == "" || MBSno == null) {
             alert(msgMBSnoNull);
//             ShowInfo(msgMBSnoNull);
             return false;
         }
         if (!(MBSno.length == 10 || MBSno.length == 11)) {
             alert(msgMBSnoLength);
//             ShowInfo(msgMBSnoLength);
             return false;
         }
         //if (MBSno.substr(4, 1) != "M") {
         if ((MBSno.substr(4, 1) != "M") && (MBSno.substr(5, 1) != "M")) {
             if ((MBSno.substr(4, 1) != "B") && (MBSno.substr(5, 1) != "B")) {
                 alert(msgMBSnoBit);
                 //             ShowInfo(msgMBSnoBit);
                 return false;
             }
         }
         //         MBSno = MBSno.substr(0, 10);
         MBSno = SubStringSN(MBSno, "MB");
         return true;
     }
     
     function ismaxlength(obj) {
         var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
         if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");    
             alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
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
//           
             setPrintItemListParam(result[1]);
             printLabels(result[1], false);
//====================print=======================
             var SuccessItem = "[" + MBSno + "]";
             ResetPage();
             ShowSuccessfulInfo(true,SuccessItem + " " + msgSuccess);
         }
     }

     
     function onFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     function setPrintItemListParam(backPrintItemList)
     {
         var lstPrtItem = backPrintItemList;
         var keyCollection = new Array();
         var valueCollection = new Array();
         
         keyCollection[0] = "@MBSno";
 
         valueCollection[0] = generateArray(MBSno);


         setPrintParam(lstPrtItem, "Unit MB Label", keyCollection, valueCollection);
    }
    
   
               

     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
         MBSno = "";
         inputSNControl.value = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");

     }


     function ShowErrorMessage(result) {
         endWaitingCoverDiv();
         ShowMessage(result);
         ShowInfo(result);
         inputSNControl.value = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");
     }


     function showPrintSettingDialog() {
         showPrintSetting(station, pcode);
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



