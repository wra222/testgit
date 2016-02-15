<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Reprint Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx 
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-12   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  Reprint Config Label and POD Label
 * UC Revision：  4078          
 */
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReprintUnitWeight.aspx.cs" Inherits="PAK_ReprintUnitWeight"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path= "~/PAK/Service/WebServiceUnitWeight.asmx" />
    </Services>
</asp:ScriptManager>   
<style type="text/css">
</style>

<div id="divPalletVerify" style="z-index: 0;">
       
        <br />
        
        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>   
                <td width="20%" align="left" >
                   &nbsp;  <asp:Label id="lblCustsn" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                 
                <td align="left" >
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                            InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/>
                </td>
            </tr>
        </table>
        
        <br />
        
        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
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
        </table>
        
        <br />
             
        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
             <tr style="height: 30px">
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
           
             <tr>
               <td width="20%" align="left"></td>
               <td align="right" >
                    <input id="btnPrint" type="button"  runat="server" onclick="Reprint()" 
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    style="width:100px"  class=" iMes_button"  tabindex="4"/>
                     &nbsp; 
               </td>
            </tr>
        </table>
        
        <table>
            <tr>
                <td>
                <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline">
                   <ContentTemplate>
                    <input id="hiddenStation" type="hidden" runat="server" />
                    <input id="pCode" type="hidden" runat="server" /> 
                    </ContentTemplate>
                 </asp:UpdatePanel> 
                    
                </td>
            </tr>
        </table>                       
        
        
    </div>

 <script type="text/javascript" language="javascript">

     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
     var msgNoPODLabelFile = '<%=this.GetLocalResourceObject(Pre + "_msgNoPODLabelFile").ToString() %>';

     var msgCustsnNull = '<%=this.GetLocalResourceObject(Pre + "_msgCustsnNull").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
     var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
     var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
     var msgPDFFileNull1 = '<%=this.GetLocalResourceObject(Pre + "_msgPDFFileNull1").ToString() %>';
     var msgPDFFileNull2 = '<%=this.GetLocalResourceObject(Pre + "_msgPDFFileNull2").ToString() %>';
     var msgPrintLabelName = '<%=this.GetLocalResourceObject(Pre + "_msgPrintLabelName").ToString() %>';
     var site = "";
     var line = "";
     var station = "";
     var custSN = "";
     var strReason = ""; 
     var inputSNControl;
     var pdfClinetPath = ""; 
     var model = "";
	 var printerpath = "";
	 var pdfFlag = false;
	 var color = "";
	 var blackPrinter = "";
	 var whitePrinter = "";
	 var podLabelPath = "";
	 window.onload = function() {
         inputSNControl = getCommonInputObject();
         station = document.getElementById("<%=hiddenStation.ClientID %>").value;
         DisplayInfoText();
         pdfClinetPath = '<%=System.Configuration.ConfigurationManager.AppSettings["UnitWeightPDFPath"] %>';
         WebServiceUnitWeight.GetPODLabelPathAndSite(onGetPathSucceed, onFailed);
         
         setStart();
     };

     function setStart() {
         custSN = "";
         inputSNControl.focus();
         getAvailableData("processDataEntry");

     }


     function checkCustSN() {
	 
	      var inputData = inputSNControl.value.trim();
		  custSN = inputData;
		  var b=CheckCustomerSN(inputData);
		  if (b) {
		      custSN = inputData;
		      return b;
		  }
		  else {
		      alert(msgInvalidSN);
		      //    ShowInfo(msgInvalidSN);
		      inputSNControl.value = "";
		      inputSNControl.focus();
		      return false;
		  }
   
     }

     function checkReprintReason()      
     {
         strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();
         return true;
     }
     function checkPODLabelPdfExist() {
         var pdfFileName = pdfClinetPath;
         if (site == "ICC") {
             pdfFileName = podLabelPath;
         }
         if (pdfClinetPath.slice(-1) != "\\") {
             pdfFileName += "\\";
         }
         pdfFileName += model + ".Pdf";

         var Fs = new ActiveXObject("Scripting.FileSystemObject");

         if (!Fs.FileExists(pdfFileName)) {
             ResetPage();
             ShowMessage(msgNoPODLabelFile);
             ShowInfo(msgNoPODLabelFile);
             return false;
         }
         return true;
     }
     function Reprint() {

         ShowInfo("");
         color = "";
         model = "";
         if (inputSNControl.value != "") {
             if (checkCustSN()) {
                 if (checkReprintReason()) {
                     var printItemlist = getPrintItemCollection();

                     if (printItemlist == "" || printItemlist == null) {
                         alert(msgPrintSettingPara);
                         getAvailableData("processDataEntry");
                         return;
                     }
                     if (site == "ICC") {
                         GetPrinterName(printItemlist);
                         var newPrintItemArr = new Array();
                         for (var i = 0; i < printItemlist.length; i++) {

                             if (printItemlist[i].LabelType.toUpperCase().indexOf("WHITE") == -1 && printItemlist[i].LabelType.toUpperCase().indexOf("BLACK") == -1) {
                                 newPrintItemArr.push(printItemlist[i]);
                             }
                         }
                         printItemlist = newPrintItemArr;
                      }
                               
                     if (podLabelPath == "" && site=="ICC") {
                         alert("Please config the PODLabelPath in SysSetting");
                         getAvailableData("processDataEntry");
                         return;
                     }
                     beginWaitingCoverDiv();
                     WebServiceUnitWeight.reprint(custSN, strReason, line, "<%=UserId%>", station, "<%=Customer%>", printItemlist, onSucceed, onFailed);
                 
                 }
                 else {
                     getAvailableData("processDataEntry");
                 }
             }
             else {
                 getAvailableData("processDataEntry");
             }
         }
         else {
             alert(msgCustsnNull);
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

     var isend = true;
     function onSucceed(result) {
         endWaitingCoverDiv();
         if (result == null) {
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == SUCCESSRET) {
         pdfFlag = false;

             //====================print=======================
         //printerpath = result[4];
         //Modify:printPDF.bat Path: Bat file path
         printerpath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
         if (printerpath.slice(-1) != "\\") {
             printerpath = printerpath + "\\";
         }

         model = result[3];
         color = result[5];
         if(site!="ICC"){color = "";}
			if (result[2] == "C") {
                 // 模板打印："Config Label"

                 for (i = 0; i < result[1].length; i++) {
                     //                     if (result[1][i].LabelType == "Config Label")    //"Config Label"
                     if (result[1][i].LabelType == "config label")    //"Config Label"
                     {
                         setPrintItemListParam(result[1][i]);
                        
                     }
                 }
                 printLabels(result[1], false);
                
             }
             else if (result[2] == "P") {
             // PDF打印: "POD Label"
             if (checkPODLabelPdfExist())
             { PDFPrint(); }
             else
             {setStart(); return; }
              
             }
             else if (result[2] == "CP") {

                 for (i = 0; i < result[1].length; i++) {
//                     if (result[1][i].LabelType == "Config Label")    //"Config Label"
                     if (result[1][i].LabelType == "config label")    //"Config Label"
                     {
                         setPrintItemListParam(result[1][i]);
                     }
                     printLabels(result[1], false);

                 }
                 if (checkPODLabelPdfExist())
                 { PDFPrint(); }
                 else
                 {setStart();  return; }
                
             }
             //====================print=======================
             var SuccessItem = "[" + custSN + "]" ;
             ResetPage();

             var msgShow = msgSuccess;
            
                 switch (result[2]) {
                     case "C":
                         msgShow = msgSuccess + " " + msgPrintLabelName + " " + "config label";
                         break;
                     case "P":
                         if (pdfFlag) {
                             msgShow = msgSuccess + " " + msgPrintLabelName + " " + "POD Label";
                         }
                         break;
                     case "CP":
                         if (pdfFlag) {
                             msgShow = msgSuccess + " " + msgPrintLabelName + " " + "config label" + ", " + "POD Label";
                         }
                         else msgShow = msgSuccess + " " + msgPrintLabelName + " " + "config label";
                         break;
                     default: break;
                 }
         
           

             ShowSuccessfulInfo(true, SuccessItem + " " + msgShow);
         }
         else {
             var content = result[0];
             ShowErrorMessage(content);
         }
         setStart();
     }

     function onGetPathSucceed(result) {
         site = result[0];
         podLabelPath = result[1];
     }
     function onFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

    
    function setPrintItemListParam(backPrintItemList) {

        var lstPrtItem = new Array();
        var keyCollection = new Array();
        var valueCollection = new Array();
        lstPrtItem[0] = backPrintItemList;
        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(custSN);
        setPrintParam(lstPrtItem, "config label", keyCollection, valueCollection);
    }

        function PDFPrint() {
           // if (site == "ICC") {pdfClinetPath = podLabelPath;}
             
           
            var pdfFileName = model + ".Pdf";
            var FsFile = "";
            var Fs = new ActiveXObject("Scripting.FileSystemObject");
            if (pdfClinetPath.slice(-1) == "\\") 
            {
                FsFile = pdfClinetPath + "unitweightprintlist.txt";
            }
            else {
                pdfClinetPath = pdfClinetPath + "\\";
                FsFile = pdfClinetPath + "unitweightprintlist.txt";
            }

            if (!Fs.FolderExists(pdfClinetPath)) {
                Fs.CreateFolder(pdfClinetPath);
            }

            if (Fs.FileExists(FsFile)) {
                Fs.DeleteFile(FsFile);
            }
            File = Fs.CreateTextFile(FsFile, true);
            //    var pdfPath = podLabelPath + pdfFileName;
            var pdfPath;
            if (site == "ICC")
            { pdfPath = podLabelPath + pdfFileName; }
            else { pdfPath = pdfClinetPath + pdfFileName; }
            File.WriteLine(pdfPath);
            File.Close();
            var wsh = new ActiveXObject("wscript.shell");
            //var cmdpdfprint = "PDFPrint.exe " + FsFile + " 4000";
            //Modify: PDF Print :EXE->Bat
            var cmdpdfprint;
            if (color != ""  && site=="ICC") {
                if (color == "White") {
                cmdpdfprint = "PrintPDF.bat " + FsFile +' "' + whitePrinter +'"' ;
                 }
                else {
                    cmdpdfprint = "PrintPDF.bat " + FsFile + ' "'  + blackPrinter + '"';
                
                }
            }
            else {
                 cmdpdfprint = "PrintPDF.bat " + FsFile + " 4000";
            }
         
            //    wsh.run("cmd /k " + getHomeDisk(pdfClinetPath) + "&copy /Y " + webserverPath +" " + pdfClinetPath + "&exit");

            if (!Fs.FileExists(FsFile)) {
                alert(msgPDFFileNull1 + " " + pdfFileName + " " + msgPDFFileNull2);
            }
            else {
                pdfFlag = true;
                wsh.run("cmd /k " + getHomeDisk(printerpath) + "&cd " + printerpath + "&" + cmdpdfprint + "&exit", 2, false);
            }
            wsh = null;
            isend = false;
        }

        function GetPrinterName(printItem) {
           
            for (i = 0; i < printItem.length; i++) {
                if (printItem[i].LabelType.toUpperCase().indexOf("WHITE") >= 0) {
                    whitePrinter = printItem[i].PrinterPort;
                    continue;
                }
                if (printItem[i].LabelType.toUpperCase().indexOf("BLACK") >= 0) {
                    blackPrinter = printItem[i].PrinterPort;
                    continue;
                }
            }
          
        }
        function GetPodColor() {

           WebServiceUnitWeight.GetCqPodLabelColor(model, onGetColorSucceed, onGetColorFailed);
      
        }
        function onGetColorSucceed(result) {

            color = result;
            PDFPrint();
        
        }
        function onGetColorFailed(result) {
            ResetPage();
            ShowErrorMessage(result.get_message());
        }
        
     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
        // model = "";
         custSN = "";
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
         showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
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



