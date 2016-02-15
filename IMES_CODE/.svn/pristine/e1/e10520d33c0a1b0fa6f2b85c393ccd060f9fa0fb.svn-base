<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-03   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. （散装）检查Pallet 上的所有SKU；
                  2.  散装是不上栈板的，但为了方便管控，FIS会在上传DN页面为每一台分配NA或者BA开头的栈板序号
                  3.  列印Pallet SN Label，Delivery Label
 * UC Revision: 2569: a.	对于Case NA Dummy Pallet 和Case BA Dummy Pallet，如果Scan Qty <> Pallet Qty ，则报告错误：“The count on this Pallet is wrong!”
 * UC Revision: 4387
 * UC Revision: 7411:Pallet Verify (FDE only) for NA Non Dummy Pallet and BA Non Dummy Pallet Scan Qty 初始值为1
 * UC Revision: 7357:明确第一个刷入的Customer S/N 也需要进行相关检查
 * UC Revision: 7438:取消下面的修改，原因是在业务流程中已经增加说明，第一台Pass 时，需要和其它Product 一样进行相关处理。:Pallet Verify (FDE only) for NA Non Dummy Pallet and BA Non Dummy Pallet Scan Qty 初始值为1
 */
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PalletVerifyFDEOnly.aspx.cs" Inherits="PAK_PalletVerifyFDEOnly" %>
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

<div id="divPalletVerify" style="z-index: 0; ">
       
       <br />
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr  valign="middle">
                <td style="height: 24px" align="left"  width="10%" nowrap="nowrap"  valign="middle">
                    <asp:Label id="lblTotalQty" runat="server" class="iMes_label_13pt" > </asp:Label>
                </td>
            
                <td style="height: 24px" align="left"  width="30%">
                     <asp:Label id="lblTotalQtyDisplay" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td width="10%"></td>
                <td width="30%"></td>
            </tr>
        </table>
        
        <hr class="footer_line" style="width:95%"/>
        
        <fieldset style="width:95%; " align="center">
            <legend id="lblPalletInfo" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
            <table width="100%"  cellpadding="0" cellspacing="0" border="0" align="center">
                <tr style="height: 30px" align="left" valign="middle">
                    <td width="10%">
                        <asp:Label id="lblPalletNo" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="30%">
                        <input id="txtPalletNo" style="width: 80%;  height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
                    </td>
                    <td width="10%"></td>
                    <td width="30%"></td>
                 </tr>          
           
                 <tr style="height: 30px" align="left" valign="middle" >   
                   <td width="10%">
                        <asp:Label id="lblScanQty" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="30%">
                        <input id="txtScanQty" style="width: 80%; height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
                    </td>
                    <td width="10%">
                        <asp:Label id="lblPalletQty" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="30%"> 
                        <input id="txtPalletQty" style="width: 80%; height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
                    </td>
                </tr>
            </table>
        </fieldset>  
             
        <fieldset style="width:95%" align="center">
            <legend id="lblProductLst" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr>
                    <td colspan="6" align="center" width="99%">
                        <div id="divGrid" style="z-index: 0">
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="218px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" HighLightRowPosition="3">
                                        <Columns>
                                            <asp:BoundField DataField="ProdID"/>
                                            <asp:BoundField DataField="CustSN" />
                                            <asp:BoundField DataField="PAQC" />
                                            <asp:BoundField DataField="POD" />
                                            <asp:BoundField DataField="WC" />
                                            <asp:BoundField DataField="CollectionData" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
         </fieldset>
         
        <br />
        <br />
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
             <tr>   
                <td width="15%" align="left">
                    <asp:Label id="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td width="55%" align="left" >
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
                </td>
                <td align="right">
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
                </td>
                <td align="right">
                    <input id="btnReprint" style="height:auto" type="button"  runat="server" 
                            onclick="rePrint()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
                </td>
            </tr>
        </table>
        
        <br />
    </div>

 <script type="text/javascript" language="javascript">
    
     var GridViewExt1ClientID = "<%=GridViewExt1.ClientID%>";

     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPAQC = '<%=this.GetLocalResourceObject(Pre + "_msgPAQC").ToString() %>';
     var msgPAQCFail = '<%=this.GetLocalResourceObject(Pre + "_msgPAQCFail").ToString() %>';
     var msgPAQCPD = '<%=this.GetLocalResourceObject(Pre + "_msgPAQCPD").ToString() %>';
     var msgDuplicateScan = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateScan").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgSaveSucc1 = '<%=this.GetLocalResourceObject(Pre + "_msgSaveSucc1").ToString() %>';
     var msgSaveSucc2 = '<%=this.GetLocalResourceObject(Pre + "_msgSaveSucc2").ToString() %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
     var msgQtyFull = '<%=this.GetLocalResourceObject(Pre + "_msgQtyFull").ToString() %>';
     var msgScanEMEA = '<%=this.GetLocalResourceObject(Pre + "_msgScanEMEA").ToString() %>';
     var msgScan9999 = '<%=this.GetLocalResourceObject(Pre + "_msgScan9999").ToString() %>';
     var msgCount = '<%=this.GetLocalResourceObject(Pre + "_msgCount").ToString() %>';
     var msgBAPalletNo = '<%=this.GetLocalResourceObject(Pre + "_msgBAPalletNo").ToString() %>';
     var msgWrongCode = '<%=this.GetLocalResourceObject(Pre + "_msgWrongCode").ToString() %>';
     var msgInputCustSNFirst = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSNFirst").ToString() %>';

     var editor = "<%=UserId%>";
     var customer = '<%=Customer%>';
     var station = '<%=Request["Station"] %>';
     var pcode = '<%=Request["PCode"] %>';
     var accountId = '<%=Request["AccountId"] %>';
     var login = '<%=Request["Login"] %>';
     
     var line = "";
     var inputSNControl;
     var hostName = "";
     var urlKey = "";
     var firstCustSn = "";
     var custSn = "";
     var scanQty = 0;
     var palletQty = 0;
     var palletNo = "";
     var DPC = "";
     var index = 1;
     var strRowsCount = "<%=initRowsCount%>";
     var initRowsCount = parseInt(strRowsCount, 10) + 1;
     var ifFirst;
     var CustList = new Array();
    //var iSelectedRowIndex = null;
     //Mantis Bug :http://10.99.183.26/Mantis/view.php?id=627 (改为前台页面收集scanProductNoList)

     var ScanProductNoList = new Array();
        
     window.onload = function() {
         inputSNControl = getCommonInputObject();
         inputSNControl.disabled = true;
         ifFirst = true;
        
         setStart();
     };

     function setStart() {
         inputSNControl.disabled = "";
         inputSNControl.value = "";
         setInputFocus();
         firstCustSn = "";
         getAvailableData("processDataEntry");

     }

     function setInputFocus() {
         if ((inputSNControl.disabled == false) || (inputSNControl.disabled == "")) {
             inputSNControl.focus();
         }
         else {
             inputSNControl.disabled = false;
             inputSNControl.focus();
         }
     }

     function checkCustSN(inputStr) {
         //if (inputStr.length == 10 && inputStr.substr(0, 2) == "CN") {
		 if (inputStr.length == 10 && CheckCustomerSN(inputStr)) {
             inputStr = inputStr;
             inputCustSN(inputStr);
         }
         //else if (inputStr.length == 11 && inputStr.substr(0, 3) == "SCN") {
		 else if (inputStr.length == 11 && CheckCustomerSN(inputStr)) {
             inputStr = inputStr.substr(1, 10);
             inputCustSN(inputStr);
         }
         else if (inputStr == "FDE") {
             PlaySound();
             ShowInfo(msgInputCustSNFirst);
			 inputSNControl.value = "";
			 setInputFocus();
             getAvailableData("processDataEntry");
         }
         else {
             PlaySound();
             ShowInfo(msgWrongCode);
			 inputSNControl.value = "";
			 setInputFocus();
             getAvailableData("processDataEntry");
         }

     }

     function inputCustSN(inputStr) {
         if (ifFirst) {
             firstCustSn = inputStr.trim();
             beginWaitingCoverDiv();
             WebServicePalletVerifyFDEOnly.inputFirstCustSN(firstCustSn, line, editor, station, customer, onFirstSnSucceeded, onFirstSnFailed);
         }
         else {
             custSn = inputStr.trim();
             WebServicePalletVerifyFDEOnly.inputCustSN(firstCustSn, custSn, onSucceeded, onFailed);
         }
     }
     
     function processDataEntry(inputStr) {
         PlaySoundClose();
         ShowInfo("");
     
         
         // 如果用户刷入'FDE'，则 Save Data
         if (!ifFirst && inputStr.trim() == "FDE") {
             if ((DPC =="NA" || DPC =="BA") && (scanQty != palletQty)) {
                 ShowInfo(msgCount);   //SVN 2569: a.	对于Case NA Dummy Pallet 和Case BA Dummy Pallet，如果Scan Qty <> Pallet Qty ，则报告错误：“The count on this Pallet is wrong!”
                 PlaySound();
                 inputSNControl.value = "";
                 setInputFocus();
                 getAvailableData("processDataEntry");
             }
            else saveData();
         }
         else {
             checkCustSN(inputStr);
         }
         
         
     }

     function onFirstSnSucceeded(result) {

         if (result == null || result.length == 0) {
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == "CHK020" && result.length == 2) {
             ExitPage();
             ShowErrorMessage(result[1]);
         }
         else if (result.length == 2) {
             endWaitingCoverDiv();
             ifFirst = false;
             var firstCustSnIndex = 0;
             firstCustSnIndex = result[1];
             var HighlightFlag = false;
             
             /// result[0]:  [0],        [1],      [2],     [3],  [4],   [5],     [6],      [7]       [8]     [9]    [10]
             ///        DummypalletNo,prodIdLst,custSnLst,PAQCLst,PODLst,WCLst,CollectData,palletQty,scanQty, DPC, totalQty
             document.getElementById("txtPalletNo").value = result[0][0];
             palletNo = result[0][0];
             document.getElementById("txtPalletQty").value = result[0][7];
             palletQty = result[0][7];
             
             DPC = result[0][9];
             document.getElementById("<%=lblTotalQtyDisplay.ClientID %>").innerText = result[0][10];

             /// 添加到表格: prodIdLst,custSnLst,PAQCLst,PODLst,WCLst,CollectData

             var ProdList = result[0][1];
             var PAQCList = result[0][3];
             var PODList = result[0][4];
             var WCList = result[0][5];
             CustList = result[0][2];
             
             for (var i = 0; i < CustList.length; i++) {
                 var rowInfo = new Array();
                 rowInfo.push(ProdList[i]);
                 rowInfo.push(CustList[i]);
                 rowInfo.push(PAQCList[i]);
                 rowInfo.push(PODList[i]);
                 rowInfo.push(WCList[i]);
                 
                 if ((i + 1) == firstCustSnIndex) {
                     // SVN 7357:明确第一个刷入的Customer S/N 也需要进行相关检查
                     if (PAQCList[i] == "PAQC") {
                          // “该机器尚未完成PAQC！”
                         PlaySound();
                         var currCustSN = CustList[i];
                         ResetPage();           // 重刷
                         ShowInfo(currCustSN + " " + msgPAQC);
                         
                         break;
                     }
                     else if (PAQCList[i] == "Fail") {
                         //“该机器PAQC Fail！”
                         PlaySound();
                         var currCustSN = CustList[i];

                         ResetPage();               // 重刷
                         ShowInfo(currCustSN + " " + msgPAQCFail);
                        
                         break;
                     }

                     if (PODList[i] == "PD") {
                          //“该机器尚未完成POD Label Check！”
                         PlaySound();
                         var currCustSN = CustList[i];

                         ResetPage();             // 重刷
                         ShowInfo(currCustSN + " " + msgPAQCPD);
                         
                         break;
                     }
                     else {
                         rowInfo.push(firstCustSn);   //SVN 2569: 每成功接收一个正确的Customer S/N，需要填写在Product List 中对应记录的Collection Data 栏位，包括前面刷入的第一个Customer S/N
                         HighlightFlag = true;

                         //UC Revision: 7411:Pallet Verify (FDE only) for NA Non Dummy Pallet and BA Non Dummy Pallet Scan Qty 初始值为1
                         //UC Revision: 7438: NA/BA 初始值是0，首次刷入+1；NAN/NBAN 初始值找到，首次刷入+1；找不到，初始值为0，首次刷入+1；
                         scanQty = result[0][8];
                         scanQty++;
                         document.getElementById("txtScanQty").value = scanQty;
                         
                         ScanProductNoList = new Array();
                         ScanProductNoList.push(ProdList[i]);
                     }
                     
                     
//                     rowInfo.push(firstCustSn);
//                     HighlightFlag = true;
                 }
                 else {
                     rowInfo.push(result[0][6]);
                 }
                 AddRowInfo(rowInfo);
             }

             if (HighlightFlag) {
                 var gdObj = document.getElementById("<%=GridViewExt1.ClientID %>");
                 var con = gdObj.rows[firstCustSnIndex]; //index从1开始
                 //高亮当前行
                 setGdHighLight(con);
             }
//             else {
//                 var gdObj = document.getElementById("<%=GridViewExt1.ClientID %>");
//                 var con = gdObj.rows[1]; //index从1开始
//                 //高亮第一行
//                 setGdHighLight(con);
//             }
             
             inputSNControl.value = "";
             setInputFocus();
             getAvailableData("processDataEntry");
         }
         else {
             var content = result[0];
             ShowErrorMessage(content);
         }
     }

     function onSucceeded(result) {

         if (result == null || result.length == 0) {
             ShowErrorMessage(msgSystemError);
         }
         else if ((result[0] == "CHK079" || result[0] == "CHK152" || result[0] == "PAK022") && result.length == 2) {
             // UC更新：[2012-5-3] 只有刷入的Customer S/N 非当前栈板上的Product 的Customer S/N 时，才要Cancel 当前的Pallet Verify 流程，再开启新的Pallet Verify 流程
             ResetPage();
             ShowErrorMessage(result[1]);
         }
         else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
             endWaitingCoverDiv();
             inputSNControl.value = "";
             ifFirst = false;

             var newSelectedRowIndex = 0;
             newSelectedRowIndex = result[1];
             if (newSelectedRowIndex >= CustList.length) {
                 ShowErrorMessage(msgInvalidSN);    //非法数据
             }
             else {

                 var gdObj = document.getElementById("<%=GridViewExt1.ClientID %>");
                 var con = gdObj.rows[newSelectedRowIndex + 1]; //index从1开始

                 //  rowinfo:   [0],      [1],     [2],    [3],  [4],     [5],    
                 //          prodIdLst,custSnLst,PAQCLst,PODLst,WCLst,CollectData
                 if (con.cells[5].innerText.trim() == "") { 
                     if (con.cells[2].innerText.trim() == "PAQC") {
                         // 如果该记录的Collection Data 为空，则报告错误：“该机器尚未完成PAQC！”
                         ShowInfo(con.cells[1].innerText.trim() + " " + msgPAQC);
                         PlaySound();
                         inputSNControl.value = "";
                         setInputFocus();
                         getAvailableData("processDataEntry");
                     }
                     else if (con.cells[2].innerText.trim() == "Fail") {
                         //“该机器PAQC Fail！”
                         ShowInfo(con.cells[1].innerText.trim() + " " + msgPAQCFail);
                         PlaySound();
                         inputSNControl.value = "";
                         setInputFocus();
                         getAvailableData("processDataEntry");
                     }

                     else if (con.cells[3].innerText.trim() == "PD") {
                         //“该机器尚未完成POD Label Check！”
                         ShowInfo(con.cells[1].innerText.trim() + " " + msgPAQCPD);
                         PlaySound();
                         inputSNControl.value = "";
                         setInputFocus();
                         getAvailableData("processDataEntry");
                     }
                     else {
                         con.cells[5].innerText = custSn;   //如果上述错误均未发生，则该记录的Collection Data 栏位显示刷入的Customer S/N，并且Scan Qty + 1
                         scanQty++;
                         document.getElementById("txtScanQty").value = scanQty;

                         ScanProductNoList.push(con.cells[0].innerText);
                     }

                 }
                 else {
                     // 如果该记录的Collection Data 不为空，则报告错误：“You had duplicate scan.”
                     ShowInfo(msgDuplicateScan);
                     PlaySound();
                     setInputFocus();
                     getAvailableData("processDataEntry");
                 }


                 setGdHighLight(con);   //高亮当前行
                 inputSNControl.value = "";
                 setInputFocus();
                 getAvailableData("processDataEntry");
             }
         }
         else {
             var content = result[0];
             ShowErrorMessage(content);
         }
     }

     function onFirstSnFailed(result) {
         ExitPage(); 
         firstCustSn = "";
         ifFirst = true;
         inputSNControl.value = "";
         
         ShowErrorMessage(result.get_message());
     }

     function onFailed(result) {
      //   ResetPage(); // Maintis : 0000705
         ShowErrorMessage(result.get_message());
     }

     var iSelectedRowIndex = null;
     function setGdHighLight(con) {
         if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=GridViewExt1.ClientID %>");     //去掉过去高亮行             
        }
        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=GridViewExt1.ClientID %>"); //设置当前高亮行
        iSelectedRowIndex = parseInt(con.index, 10);     //记住当前高亮行
     }
     
     function saveData() {
         try {
            // beginWaitingCoverDiv();
                
             lstPrintItem = getPrintItemCollection();
             if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
             {
                 PlaySound();
                 ResetPage();
                 ShowInfo(msgPrintSettingPara);
                 
                 
                 return;
             }
             // beginWaitingCoverDiv(); // 保存时统一修改为不启动waiting
             WebServicePalletVerifyFDEOnly.save(firstCustSn, lstPrintItem, ScanProductNoList, onSaveSucceed, onSaveFailed);

         }
         catch (e) {
             ResetPage();
             ShowInfo(e.description);
             PlaySound();
             
         }

     }

     function onSaveSucceed(result) {
         ifFirst = true;
         if (result == null) {
             //service方法没有返回
             endWaitingCoverDiv();
             ShowErrorMessage(msgSystemError);
         }
         else if (result.length == 1) {
             ShowErrorMessage(result[0]);
         }
         else if (result[0] == SUCCESSRET) {
             inputSNControl.focus();

////====================print=======================

             for (var i = 0; i < result[1].length; i++) {

                 setPrintItemListParam(result[1][i], result[2]);    
             }
             printLabels(result[1], false);
            
             
////====================print=======================           

             ResetPage();
             ShowSuccessfulInfo(true, msgSaveSucc1 + " "+ result[2]  + " " + msgSaveSucc2 );  ///显示信息：请放置到仓库' + @DummyPalletNo + '号库位'

         }
     }

     
     function onSaveFailed(result) {
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     

     function setPrintItemListParam(backPrintItemList, DummyPalletNo) {
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
         
         valueCollection[0] = generateArray(DummyPalletNo);


         if (backPrintItemList.LabelType == "Bulk Delivery Label") {

             setPrintParam(lstPrtItem, "Bulk Delivery Label", keyCollection, valueCollection);
         }

         else if (backPrintItemList.LabelType == "Bulk Pallet SN Label") {

             setPrintParam(lstPrtItem, "Bulk Pallet SN Label", keyCollection, valueCollection);
         }
        
     }

     function AddRowInfo(RowArray) {
         try {
             if (index < initRowsCount) {
                 eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
             }
             else {
                 eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
             }
             setSrollByIndex(index, false);
             index++;

         }
         catch (e) {
             ShowInfo(e.description);
             PlaySound();
         }
     }

     function ClearData() {
         endWaitingCoverDiv();
         ShowInfo("");
         firstCustSn = "";
         custSn = "";
         scanQty = 0;
         palletQty = 0;
         palletNo = "";
         DPC = "";
         document.getElementById("<%=lblTotalQtyDisplay.ClientID %>").innerText = "0";
         document.getElementById("txtPalletNo").value = "";
         document.getElementById("txtScanQty").value = "";
         document.getElementById("txtPalletQty").value = "";
         inputSNControl.value = "";
         setInputFocus();
         clearData();
         CustList.length = 0;
         ScanProductNoList.length = 0;
         index = 1;
         clearTable();
         ifFirst = true;
         getAvailableData("processDataEntry");

     }


     function ShowErrorMessage(result) {
         endWaitingCoverDiv();
         ShowInfo(result);
         PlaySound();
         inputSNControl.value = "";
         setInputFocus();
         getAvailableData("processDataEntry");
     }


     function showPrintSettingDialog() {
         showPrintSetting(station, pcode);
     }

     function rePrint() {
         var url = "ReprintPalletVerifyFDEOnly.aspx?Station=" + station + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login;
         window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');

     }
     
     function clearTable() {
         try {
             ClearGvExtTable(GridViewExt1ClientID, initRowsCount);
             //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加

             index = 1;
             setSrollByIndex(0, false);
             eval("setRowNonSelected_" + GridViewExt1ClientID + "()"); 
         }
         catch (e) {
             ShowInfo(e.description);
             PlaySound();
         }

     }

     function ExitPage() {
         if (firstCustSn != "") {
             WebServicePalletVerifyFDEOnly.Cancel(firstCustSn);
             sleep(waitTimeForClear);
         }
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
 
     function ResetPage() {
         ExitPage();
         ClearData();
     }

     window.onunload = function() {
         ExitPage();
     };
     
    </script>
</asp:Content>


