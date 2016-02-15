<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:Reprint PCA ICT Input-02 ECR Label Reprint
 * UI:CI-MES12-SPEC-SA-UI PCA ICT Input.docx �C2011/11/03
 * UC:CI-MES12-SPEC-SA-UC PCA ICT Input.docx �C2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-17   Chen Xu              Create
 * 2012-02-06   Chen Xu              Modify: ITC-1360-0252,ITC-1360-0251
 * Known issues:
 * TODO��
 * UC ����ҵ��  ��ǩ��ʱ����ӡ ECR Label
 *           
 */
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReprintECRLabel.aspx.cs" Inherits="PAK_ReprintECRLabel"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path=  "~/SA/Service/WebServiceReprintECRLabel.asmx" />
    </Services>
</asp:ScriptManager>   
<style type="text/css">
</style>

<div id="divECR" style="z-index: 0;">
       
       <br />
        <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
         <tr>
	        <td style="width:15%; height:30px" align="left" valign="bottom">
	           <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	        </td>
	        <td width="75%" valign="middle"align="left">
                <iMES:CmbPdLine ID="CmbPdLine1" runat="server"  Width="99" IsPercentage="true" TabIndex="1"/>
            </td>       
         </tr>
        </table>
        
         <hr class="footer_line" style="width:95%"/>
        
        <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td width="15%" align="left" >
                 <asp:Label id="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                 
                <td colspan="5" align="left" >
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                    TabIndex="2"/>
                </td>
            </tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr>
                 <td width="15%" align="left" >
                   <asp:Label id="lblMBSno" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td  width="15%" align="left">
                        <input id="txtMBSno" style=" width:90%; height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
               </td>
                <td  width="15%" align="center" >
                   <asp:Label id="lblDCode" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td  width="15%" align="left">
                        <input id="txtDCode" style="width:90%;  height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
               </td>
                <td  width="15%" align="center" >
                   <asp:Label id="lblECR" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td  width="15%" align="left">
                        <input id="txtECR"  style="width:90%; height: 20; " type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
               </td>
                
            </tr>    
        </table>
        
        <br />
        
        <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr style="height: 30px" align="left" valign="middle">
                <td width="15%">
                     <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="75%" valign="middle">
                        <textarea id="txtReason" rows="5" style="width:98%;" 
                    runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                    onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="3"></textarea>
                </td>
             </tr>          
        </table>
        
        <br />
             
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
             <tr style="height:30px" >
                 <td width="15%" align="left"></td>
                 <td width="50%" align="left"></td>
                 <td width="25%" align="right">
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                            tabindex ="4"/> 
                            &nbsp; 
                </td>
             </tr>
        
             <tr style="height:30px">   
               <td width="15%" align="left"></td>
               <td width="50%" align="left"></td>
               <td width="25%" align="right" >
                    <input id="btnPrint" type="button"  runat="server" onclick="Reprint()" style="height:auto"
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    class=" iMes_button"  tabindex="5"/>
                     &nbsp; 
               </td>
            </tr>
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
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
     var msgmon = '<%=this.GetLocalResourceObject(Pre + "_msgmonsn").ToString()%>';
     var msgchild = '<%=this.GetLocalResourceObject(Pre + "_msgchildsn").ToString()%>';
     var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString()%>';
     var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString() %>';
     var msgMBSnoBit = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoBit").ToString() %>';
     var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgECRLength = '<%=this.GetLocalResourceObject(Pre + "_msgECRLength").ToString()%>';
     
     var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
     var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
     var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';

     var ecr = "";
     var station = "";
     var MBSno = "";
     var strReason = ""; 
     var inputSNControl;
     var printFlag = false;
     var rePrintButtonFlag = false;

     window.onload = function() {
         inputSNControl = getCommonInputObject();
         station = document.getElementById("<%=hiddenStation.ClientID %>").value;
         setStart();
     };

     function setStart() {
         MBSno = "";

         //ITC-1360-0252 : Pdline�Զ�ѡ���һ��
//         try {
//             getPdLineCmbObj().selectedIndex = 1;
//             setInputFocus();
//         }
//         catch (e) {
//             setPdLineCmbFocus();
//         }
//         if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
//             // alert(msgPdLineNull);
//             getPdLineCmbObj().selectedIndex = 0;
//             setPdLineCmbFocus();
         //         }

         // ITC-1360-0252: ��bugȡ��
         getPdLineCmbObj().selectedIndex = 0;
         setPdLineCmbFocus();
         
         getAvailableData("processDataEntry");

     }

     function setInputFocus() {
         inputSNControl.value = "";
         inputSNControl.focus();
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

     function processDataEntry(inputStr) {
         if (!printFlag && !rePrintButtonFlag) {
             document.getElementById("txtMBSno").value = "";
             document.getElementById("txtDCode").value = "";
             document.getElementById("txtECR").value = "";
         }
         
         Print(inputStr);
     }

     function Reprint() {
         rePrintButtonFlag = true;
         Print();
     }
     
     function Print(inputdata) {
         ShowInfo("");
         
         var getdata = getCommonInputObject().value;
         
         if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
             alert(msgPdLineNull);
             rePrintButtonFlag = false;
         }
         else if (getdata == "" || getdata == null) {
            alert(msgMBSnoNull);
            rePrintButtonFlag = false;
         }
         else if (getdata.length == 5 && !rePrintButtonFlag) {
            ecr = getdata;
            document.getElementById("txtECR").value = ecr;
         }
         else if (printFlag) {
             printExecute();
             return;
         }
         else if (checkMBSno()) {
             beginWaitingCoverDiv();
             WebServiceReprintECRLabel.InputMBSno(getPdLineCmbValue(), MBSno, "<%=UserId%>", station, "<%=Customer%>", onInputSucceeded, onFailed);
             return;
         }
         
         setInputFocus();
         getAvailableData("processDataEntry");
         
        
     }

     function printExecute() {
         if (checkReprintReason()) {
             var printItemlist = getPrintItemCollection();
             if (printItemlist == "" || printItemlist == null) {

                 alert(msgPrintSettingPara);
                 ResetPage();
                 return;
             }
             beginWaitingCoverDiv();
             WebServiceReprintECRLabel.reprint(MBSno, strReason, ecr,getPdLineCmbValue(), "<%=UserId%>", station, "<%=Customer%>", printItemlist, onSucceed, onFailed);
         }
         else {
             getAvailableData("processDataEntry");
         }
     }

    function printExecuteList(result) {
        if (checkReprintReason()) {
            var printItemlist = getPrintItemCollection();
            if (printItemlist == "" || printItemlist == null) {
                alert(msgPrintSettingPara);
                ResetPage();
                return;
            }
            beginWaitingCoverDiv();
            var mbsnlist = [];
            var ecrlist = [];

            mbsnlist.push() 


            for (var i = 0; i < result[1].length; i++) {
                mbsnlist.push(result[1][i].id);
                ecrlist.push(result[1][i].ecr);
            }
            WebServiceReprintECRLabel.reprints(mbsnlist, strReason, ecrlist, getPdLineCmbValue(), "<%=UserId%>", station, "<%=Customer%>", printItemlist, onListSucceed, onListFailed);                 
        }
        else {
            getAvailableData("processDataEntry");
        }
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
         //MBSno = MBSno.substr(0, 10);

         MBSno =SubStringSN(MBSno, "MB");
         return true;
     }

     function alertNoPdLine() {
         alert(msgPdLineNull);
     }
    
    function onInputSucceeded(result)
    {
        try 
        {
            endWaitingCoverDiv();
            
            if(result==null)
            {
                //service����û�з���
                endWaitingCoverDiv();
                alert(msgSystemError);  
                 
            }
            else if((result.length==2)&&(result[0]==SUCCESSRET)) {

                for (var i = 0; i < result[1].length; i++) {
                    document.getElementById("txtMBSno").value = result[1][i].id;
                    document.getElementById("txtDCode").value = result[1][i].dateCode;

                    if (ecr == "") {
                        document.getElementById("txtECR").value = result[1][i].ecr;
                    }
                    else {
                        document.getElementById("txtECR").value = ecr;
                    }
                }
                printFlag = true;
                if (result[1].length > 1) {
                    printExecuteList(result);
                }
                else {
                    printExecute();
                }
                
//                document.getElementById("txtMBSno").value = MBSno;
//                document.getElementById("txtDCode").value = result[1];

//                if (ecr == "") {
//                    document.getElementById("txtECR").value = result[2];
//                }
//                else {
//                    document.getElementById("txtECR").value = ecr;
//                }
//                printFlag = true;
//                printExecute();
            }
            else
            {
                endWaitingCoverDiv();    
                var content =result[0];
                ShowMessage(content);
                ShowInfo(content);
            }

            //ITC-1360-0251:���DataEntry
            inputSNControl.value = "";
           
           // reasonFocus();
            getAvailableData("processDataEntry");

        }
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        }
        
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
         endWaitingCoverDiv();
         if (result == null) {
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == SUCCESSRET) {

         //====================print=======================

             for (var i = 0; i < result[1].length; i++) {
                 setPrintItemListParam(result[1][i], result[1][i].LabelType);
             }
             printLabels(result[1], false);
             //====================print=======================
             var SuccessItem = "[" + MBSno + "]";
            // ResetPage(); //����Ҫ��������reasonֱ�Ӵ�ӡ���ǳɹ���ֱ�������ҳ��Ļ��������������ʾ��Ϣ�ͻῴ�����ˣ���Ϊ�´�����ʱ���
             initPage();
             ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
         }
     }

     
     function onFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

    function onListSucceed(result) {
        endWaitingCoverDiv();
        if (result == null) {
            ShowErrorMessage(msgSystemError);
        }
        else if (result[0] == SUCCESSRET) {
            //====================print=======================
            for (var ii = 0; ii < result[2].length; ii++) { 
                for (var i = 0; i < result[1].length; i++) {
                    setPrintItemListParamList(result[1][i], result[1][i].LabelType, result[2][ii]);
                }
                printLabels(result[1], false);
            }
            document.getElementById("txtMBSno").value = "";
            document.getElementById("txtDCode").value = "";
            document.getElementById("txtECR").value = "";
            //====================print=======================
            var SuccessItem = "[" + msgmon + " " + MBSno + ":" + msgchild + " " + result[3] + "]";
            // ResetPage(); //����Ҫ��������reasonֱ�Ӵ�ӡ���ǳɹ���ֱ�������ҳ��Ļ��������������ʾ��Ϣ�ͻῴ�����ˣ���Ϊ�´�����ʱ���
            initPage();
            ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
        }
    }


     function onListFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     function setPrintItemListParam(backPrintItemList,labeltype)
     {
         var lstPrtItem = new Array();
         var keyCollection = new Array();
         var valueCollection = new Array();

         lstPrtItem[0] = backPrintItemList;
         
         keyCollection[0] = "@MBSno";

         valueCollection[0] = generateArray(MBSno);

         setPrintParam(lstPrtItem, labeltype, keyCollection, valueCollection);

     }

     function setPrintItemListParamList(backPrintItemList, labeltype,mbsnlist) {
         var lstPrtItem = new Array();
         var keyCollection = new Array();
         var valueCollection = new Array();

         lstPrtItem[0] = backPrintItemList;

         keyCollection[0] = "@MBSno";

         valueCollection[0] = generateArray(mbsnlist);

         setPrintParam(lstPrtItem, labeltype, keyCollection, valueCollection);

     }

    function initPage() {
        endWaitingCoverDiv();
        ShowInfo("");
        MBSno = "";
        inputSNControl.value = "";
        //    inputSNControl.focus();
        printFlag = false;
        rePrintButtonFlag = false;

        ecr = "";
        
        if (getPdLineCmbObj().selectedIndex == 0) {
            setPdLineCmbFocus();
        }
        else {
            inputSNControl.focus();
        }
        getAvailableData("processDataEntry");
    }
               

     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
         MBSno = "";
         ecr = "";
         inputSNControl.value = "";
     //    inputSNControl.focus();
         printFlag = false;
         rePrintButtonFlag = false;
         document.getElementById("txtMBSno").value = "";
         document.getElementById("txtDCode").value = "";
         document.getElementById("txtECR").value = "";
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



