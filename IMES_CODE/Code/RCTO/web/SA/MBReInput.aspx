<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:PCA ICT INPUT-04 MB ReInput
 * UI:CI-MES12-SPEC-SA-UI PCA ICT Input.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA ICT Input.docx         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-18   Chen Xu               Create
 * 2012-02-06   Chen Xu              Modify: ITC-1360-0255
 * Known issues:
 * TODO：
 * UC 具体业务：  重流板子
 *           
 */
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MBReInput.aspx.cs" Inherits="PAK_MBReInput"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path=  "~/SA/Service/WebServiceMBReInput.asmx" />
    </Services>
</asp:ScriptManager>   
<style type="text/css">
</style>

<div id="divECR" style="z-index: 0;">
       
       <br />
        <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
         <tr>
	        <td style="width:20%; height:30px" align="left" valign="bottom">
	           <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	        </td>
	        <td colspan="4" valign="middle"align="left">
                <iMES:CmbPdLine ID="CmbPdLine1" runat="server"  Width="99" IsPercentage="true" TabIndex="1"/>
            </td>       
         </tr>
        </table>
        
         <hr class="footer_line" style="width:95%"/>
        
        <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr style="height:40px">
                <td width="20%" align="left" >
                    <asp:Label id="lblMBSno" runat="server" class="iMes_label_13pt"> </asp:Label>    
                </td>
                 
                <td colspan="4" align="left" >
                    <input id="txtMBSno" style=" width:95%; height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Transparent" />    
                </td>
            </tr>
            
            <tr style="height:40px">
                 <td width="20%" align="left" >
                   
                   <asp:Label id="lblDCode" runat="server" class="iMes_label_13pt"> </asp:Label>
                   
                </td>
                <td width="25%" align="left">
                        
                        <input id="txtDCode" style="width:95%;  height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Transparent" /></td>
                
                <td width="20%" align="left" >
                   <asp:Label id="lblECR" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td width="25%" align="left">
                        <input id="txtECR" align="left" style="width:93%; height: 20; " type="text" readonly="readonly"
                            class="iMes_textbox_input_Transparent" />
               </td>
               <td></td>
                
            </tr>    
            
            <tr style="height:40px">
                 <td width="20%" align="left" >
                   
                   <asp:Label id="lblMAC" runat="server" class="iMes_label_13pt"> </asp:Label>
                   
                </td>
                <td width="25%" align="left">
                        
                        <input id="txtMAC" style="width:95%;  height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Transparent" /></td>
                
                <td width="20%" align="left" >
                   <asp:Label id="lblMBCT" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td width="25%" align="left">
                        <input id="txtMBCT" align="left" style="width:93%; height: 20; " type="text" readonly="readonly"
                            class="iMes_textbox_input_Transparent" />
               </td>
               <td></td>
                
            </tr>  
        </table>
        
        <br />
        <br />
        
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
             <tr style="height:30px">   
               <td width="20%" align="left">
                 <asp:Label id="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                 </td>
               <td  colspan="3" align="left">
                    <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                    TabIndex="2"/>
                 </td>
               <td width="20%" align="right" >
                    <input id="btnReInput" type="button"  runat="server" onclick="ReInput()" 
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    style="height:auto" class=" iMes_button"  tabindex="5"/>
                     &nbsp; 
               </td>
            </tr>
        </table>
        
        <table>
            <tr>
                <td> 
                 <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline" >
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

     var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString()%>';
     var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString() %>';
     var msgMBSnoBit = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoBit").ToString() %>';
     var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgECRLength = '<%=this.GetLocalResourceObject(Pre + "_msgECRLength").ToString()%>';
     var msgReInputSucc = '<%=this.GetLocalResourceObject(Pre + "_msgReInputSucc").ToString()%>';
     
     var line = "";
     var station = "";
     var MBSno = "";
     var inputSNControl;
     var printFlag =false;

     window.onload = function() {
    //     beginWaitingCoverDiv();
         inputSNControl = getCommonInputObject();
         station = document.getElementById("<%=hiddenStation.ClientID %>").value;
         setStart();
     };

     function setStart() {
         endWaitingCoverDiv();
         MBSno = "";

         //ITC-1360-0255 : Pdline自动选择第一项
//         try {
//            getPdLineCmbObj().selectedIndex = 1;
//            setInputFocus();
//         }
//         catch (e) {
//             setPdLineCmbFocus();
//         }
//         if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
//             // alert(msgPdLineNull);
//             getPdLineCmbObj().selectedIndex = 0;
//             setPdLineCmbFocus();
         //         }
         
         // ITC-1360-0256: 该bug取消
         getPdLineCmbObj().selectedIndex = 0;
         setPdLineCmbFocus();
         
         getAvailableData("processDataEntry");

     }

     function setInputFocus() {
         inputSNControl.value = "";
         inputSNControl.focus();
     }
    
     
     function processDataEntry(inputStr) {
         ShowInfo("");
         if (inputStr.trim() == "9999" ){
            if (printFlag)
            {
                ReInput();
            }
            else 
            {
                alert (msgMBSnoNull);
                setInputFocus();
                getAvailableData("processDataEntry");
            }
         }
         else if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
             alert(msgPdLineNull);
          //   ShowInfo(msgPdLineNull);
             setPdLineCmbFocus();
             getAvailableData("processDataEntry");
         }
         else  if (checkMBSno()) {
             beginWaitingCoverDiv();
             printFlag = false;
             WebServiceMBReInput.InputMBSno(getPdLineCmbValue(), MBSno, "<%=UserId%>", station, "<%=Customer%>", onInputSucceeded, onFailed);
         }
       
         else {
             setInputFocus();
             getAvailableData("processDataEntry");
         }
     }

     function ReInput() {
         if (printFlag) {
             beginWaitingCoverDiv();
             WebServiceMBReInput.reInput(getPdLineCmbValue(), MBSno, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFailed);
         }
         else {
             alert(msgMBSnoNull);
             setInputFocus();
             getAvailableData("processDataEntry");
         }
     }

     function checkMBSno() {
         MBSno = inputSNControl.value.trim();
         if (MBSno == "" || MBSno == null) {
             alert(msgMBSnoNull);
          //   ShowInfo(msgMBSnoNull);
             return false;
         }
         if (!(MBSno.length == 10 || MBSno.length == 11)) {
             alert(msgMBSnoLength);
         //    ShowInfo(msgMBSnoLength);
             return false;
         }
         //if (MBSno.substr(4, 1) != "M") {
         if ((MBSno.substr(4, 1) != "M") && (MBSno.substr(5, 1) != "M")) {
             alert(msgMBSnoBit);
         //    ShowInfo(msgMBSnoBit);
             return false;
         }
         //         MBSno = MBSno.substr(0, 10);
         MBSno = SubStringSN(MBSno, "MB");
         return true;
     }

    function onInputSucceeded(result)
    {
        try 
        {
            endWaitingCoverDiv();
            
            if(result==null)
            {
                //service方法没有返回
                endWaitingCoverDiv();
                alert(msgSystemError);  
                 
            }
            else if((result.length==5)&&(result[0]==SUCCESSRET))
            {
                document.getElementById("txtMBSno").value = MBSno;
                document.getElementById("txtDCode").value = result[1];
//                if (result[2].trim().length != 5) {
//                    ResetPage();
//                    ShowErrorMessage(msgECRLength);
//                    return;                  
//                }
                document.getElementById("txtECR").value = result[2];
                document.getElementById("txtMAC").value = result[3];
                document.getElementById("txtMBCT").value = result[4];
                printFlag = true;
            }
            else
            {
                endWaitingCoverDiv();    
                var content =result[0];
                ShowMessage(content);
                ShowInfo(content);
            }

            setInputFocus();
            getAvailableData("processDataEntry");

        }
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        }
        
    }  
     

     
     function onSucceed(result) {
         if (result == null) {
             endWaitingCoverDiv();
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == SUCCESSRET) {
         var msgmbsno = "[" + MBSno + "]"; 
             ResetPage();
             ShowSuccessfulInfo(true, msgmbsno + " " + msgReInputSucc);
         }
     }

     
     function onFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     function alertNoPdLine() {
         alert(msgPdLineNull);
     }        

     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
         MBSno = "";
         inputSNControl.value = "";
     //    inputSNControl.focus();
         printFlag = false;
         document.getElementById("txtMBSno").value = "";
         document.getElementById("txtDCode").value = "";
         document.getElementById("txtECR").value = "";
         document.getElementById("txtMAC").value = "";
         document.getElementById("txtMBCT").value = "";
         if (getPdLineCmbObj().selectedIndex == 0) {
            setPdLineCmbFocus();
         }
         else {
            inputSNControl.focus();
         }
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



