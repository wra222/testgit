<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:Pallet Verify Data for Docking        
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 * TODO：
 *
 */
--%>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PalletDataVerifyForDocking.aspx.cs" Inherits="PalletVerifyDataForDocking" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path="~/Docking/Service/WebServicePalletVerifyDataForDocking.asmx" />
    </Services>
</asp:ScriptManager>   
<style type="text/css">
</style>

<div id="divPalletVerify" style="z-index: 0;">
       
        <br />
        
        <fieldset style="width:95%" align="center">
            <legend id="lblPalletInfo" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr style="height: 30px" align="left" valign="middle">
                    <td width="10%" align="left">
                        <asp:Label id="lblPalletNo" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="30%">
                        <input id="txtPalletNo" style="width: 80%; height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
                    </td>
                    <td width="10%"> </td>
                    <td width="30%"> </td>
                 </tr>          
                 
                 <tr style="height: 30px" align="left" valign="middle" >   
                   <td width="10%" align="left">
                        <asp:Label id="lblScanQty" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="30%">
                        <input id="txtScanQty" style="width: 80%; height: 20;" type="text" readonly="readonly"
                            class="iMes_textbox_input_Disabled" />
                    </td>
                    <td width="10%" align="left">
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
                    <td colspan="6" align="center" valign="middle" width="99%">
                        <div id="divGrid" style="z-index: 0">
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="218px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" 
                                        HighLightRowPosition="3" style="top: 151px; left: 10px">
                                        <Columns>
                                            <asp:BoundField DataField="ProdID"/>
                                            <asp:BoundField DataField="Carton" />
                                            <asp:BoundField DataField="CustSN" />
                                            <asp:BoundField DataField="" />
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
                <td width="45%" align="left" >
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
                </td>
                 <td  width="20%" align="left">
                        <asp:CheckBox id="gdCheckBox" runat="server"></asp:CheckBox>
                 </td>
                <td align="right">
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
                </td>
                <td align="right">
                    
                </td>
            </tr>
            
             <tr>
            <td>
               <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                    <ContentTemplate>
                      <button id="btnGetOP" runat="server" type="button" onclick="" style="display:none" onserverclick="GetOPGrid"></button>
                      <input id="HD_deliveryDN" type="hidden" runat="server" />
                      <input id="HD_PLT" type="hidden" runat="server" />
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>
        
       <br />
    </div>

 <script type="text/javascript" language="javascript">
    
     var GridViewExt1ClientID = "<%=GridViewExt1.ClientID%>";
     var msgCallEditsFail = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCallEditsFail") %>';
     var msgCreatePDFFail = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCreatePDFFail") %>';
     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
     var msgPAQC = '<%=this.GetLocalResourceObject(Pre + "_msgPAQC").ToString() %>';
     var msgPAQCFail = '<%=this.GetLocalResourceObject(Pre + "_msgPAQCFail").ToString() %>';
     var msgPAQCPD = '<%=this.GetLocalResourceObject(Pre + "_msgPAQCPD").ToString() %>';
     var msgDuplicateScan = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateScan").ToString() %>';
     var msgInvalidSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
     var msgSaveSucc = '<%=this.GetLocalResourceObject(Pre + "_msgSaveSucc").ToString() %>';
     var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
     var msgQtyFull = '<%=this.GetLocalResourceObject(Pre + "_msgQtyFull").ToString() %>';
     var msgScanEMEA = '<%=this.GetLocalResourceObject(Pre + "_msgScanEMEA").ToString() %>';
     var msgScan9999 = '<%=this.GetLocalResourceObject(Pre + "_msgScan9999").ToString() %>';
     var msgCount = '<%=this.GetLocalResourceObject(Pre + "_msgCount").ToString() %>';
     var msgBAPalletNo = '<%=this.GetLocalResourceObject(Pre + "_msgBAPalletNo").ToString() %>';
     var msgWrongCode = '<%=this.GetLocalResourceObject(Pre + "_msgWrongCode").ToString() %>';
     var msgInputCustSNFirst = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSNFirst").ToString() %>';
     var msgDupCarton = '<%=this.GetLocalResourceObject(Pre + "_msgDupCarton").ToString() %>';
     var msgDupProd = '<%=this.GetLocalResourceObject(Pre + "_msgDupProd").ToString() %>';
     var msgNewInput = '<%=this.GetLocalResourceObject(Pre + "_msgNewInput").ToString() %>';
     
     var editor = "<%=UserId%>";
     var customer = '<%=Customer%>';
     var station = '<%=Request["Station"] %>';
     var pcode = '<%=Request["PCode"] %>';
     var accountId = '<%=Request["AccountId"] %>';
     var login = '<%=Request["Login"] %>';
     var pdfClinetPath = "<%=PDFClinetPath%>";
     var PdfFilename = "";
     var XmlFilename = "";
     var Templatename = "";
     var arrTemp = new Array();
     var falseCustSn = "";
     var falsePalletNo = "";
         
     var line = "";

     var inputSNControl;
     var firstCustSn = "";
     var custSn = "";
//     var totalQty = 0;
     var scanQty = 0;
     var palletQty = 0;
     var palletNo = "";
     var infoValue = "";
     var index = 1;
     var strRowsCount = "<%=initRowsCount%>";
     var initRowsCount = parseInt(strRowsCount, 10) + 1;
     var ifFirst;
     var CustList = new Array();
     var labeltypeBranch = "";
     var DeliveryPerPalletList = new Array();
     var modelList = new Array();
     var PDFPLLst = new Array();
     var EndPDFFile = "";
     var secondSn;
     var isShow=false;
    	 
     window.onload = function() {
         inputSNControl = getCommonInputObject();
         inputSNControl.disabled = true;
         ifFirst = true;
         document.getElementById("txtScanQty").value = scanQty;
         document.getElementById("txtPalletQty").value = palletQty;
         
         setStart();
     };

     function setStart() {
         inputSNControl.disabled = "";
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
         if (inputStr.length == 9) {
             inputStr = inputStr;
             inputCustSN(inputStr);
         }
         //else if (inputStr.length == 10 && inputStr.substr(0, 2) != "CN") {
		 else if (inputStr.length == 10 && !CheckCustomerSN(inputStr)) {
             inputStr = inputStr.substr(0, 9);
             inputCustSN(inputStr);
         }
         //else if (inputStr.length == 10 && inputStr.substr(0, 2) == "CN") {
		 else if (inputStr.length == 10 && CheckCustomerSN(inputStr)) {
             inputStr = inputStr;
             inputCustSN(inputStr);
         }
         //else if (inputStr.length == 11 && inputStr.substr(0, 3) == "SCN") {
		 else if (inputStr.length == 11 && CheckCustomerSN(inputStr)) {
             inputStr = inputStr.substr(1, 10);
             inputCustSN(inputStr);
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
         if (isShow) {
             // isShow = false;
             //var SuccessItem = "CustSn:[" + falseCustSn + "], pallet:["+ falsePalletNo +"] ";

             ShowErrorMessage(falseCustSn);
             
         }
          
         if (ifFirst) {
             firstCustSn = inputStr;
             secondSn = "";
             beginWaitingCoverDiv();
             WebServicePalletVerify.inputFirstCustSN(firstCustSn, line, editor, station, customer, onFirstSnSucceeded, onFirstSnFailed);
         }
         else {
             // ITC-1360-1218: 首次刷入Sn计算ScanQty后，这里应该改回原来的判断方式：Scan Qty >= Pallet Qty
             if (scanQty >= palletQty) {
                 ShowInfo(msgQtyFull);
                 PlaySound();
				 inputSNControl.value = "";
				 setInputFocus();
                 getAvailableData("processDataEntry");
             }
             else if(CheckInTable(inputStr,0) || CheckInTable(inputStr,2)){
                ShowInfo(msgDupProd); //Duplicate Product
                PlaySound();
			    inputSNControl.value = "";
			    setInputFocus();
                getAvailableData("processDataEntry");                
             }
             else{
                 custSn = inputStr;
                 secondSn = custSn;
                 WebServicePalletVerify.inputCustSN(firstCustSn, custSn, onSucceeded, onFailed);
             }
         }
     }
   
     function processDataEntry(inputStr) {
//         PlaySound();

 //        PlaySoundClose();
         ShowInfo("");
         inputStr = inputStr.trim();

         if (!ifFirst && inputStr == "9999") {

             if (scanQty < palletQty) {
                 ShowInfo(msgCount);
                 inputSNControl.value = "";
                 setInputFocus();
                 PlaySound();
                 getAvailableData("processDataEntry");
             }
             else           
                 saveData();
         }
         else {
             checkCustSN(inputStr);
         }
        
     }

     function onFirstSnSucceeded(result) {
         if (isShow) {
             isShow = false;
             falseCustSn = "";
             falsePalletNo = "";
         }
         else
             ShowInfo("");
            
         if (result == null || result.length == 0) {
             ShowErrorMessage(msgSystemError);
         }
          else  {
             endWaitingCoverDiv();
             inputSNControl.value = "";
             ifFirst = false;
             //var firstCustSnIndex = 0;
             //firstCustSnIndex = result[1];
             labeltypeBranch = result[2];
             var HighlightFlag = false;
             
             /// result[0]: [0],    [1],      [2],     [3],       [4],   [5],     [6],      [7]  
             ///        palletNo,prodIdLst,custSnLst,carton no,palletQty
             document.getElementById("txtPalletNo").value = result[0][0];
             document.getElementById("<%=HD_PLT.ClientID %>").value = result[0][0];
             palletNo =result[0][0];
             document.getElementById("txtPalletQty").value = result[0][4];
             palletQty = result[0][4];
             //palletQty = 2;

             // SVN 3094: 如果Scan Qty > Pallet Qty，则报告错误：“数量超出范围!”。
             scanQty = 0; //初始值是"0"
             if (scanQty > palletQty) {
                 PlaySound();
                 ResetPage();
                 ShowInfo(msgQtyFull);
                 return;
             }

             PDFPLLst = result[0][9];

             //获取与当前Pallet 结合的Delivery
             DeliveryPerPalletList = result[3];
             
             CustList = result[0][2];
             var ProdList = result[0][1];
             var cartonNo = result[0][3];
             
           
             /// 添加到表格: prodIdLst,custSnLst,carton
            // for (var i = 0; i < CustList.length; i++) {
                 var rowInfo = new Array();
                 rowInfo.push(ProdList);
                 rowInfo.push(cartonNo);
                 rowInfo.push(CustList);   //SVN 2569: 每成功接收一个正确的Customer S/N，需要填写在Product List 中对应记录的Collection Data 栏位，包括前面刷入的第一个Customer S/N
                 rowInfo.push(""); 
                 HighlightFlag = true;

                 //成功接收一个正确的Customer S/N, scanQty+1;
                 scanQty++;
                 document.getElementById("txtScanQty").value = scanQty;                     
                 AddRowInfo(rowInfo);
            //   }

             if (HighlightFlag) {
                 var gdObj = document.getElementById("<%=GridViewExt1.ClientID %>");
                 var con = gdObj.rows[1]; //index从1开始
                 //高亮当前行
                 setGdHighLight(con);
             }
			
			modelList.length = 0;
			if (labeltypeBranch == "A") {
				//var dn = result[0][11];
				if(DeliveryPerPalletList[0] != null && DeliveryPerPalletList[0] != ''){
                    modelList = result[4];
				    WebServicePalletVerify.callTemplateCheckLaNew(DeliveryPerPalletList[0], "Pallet Ship Label- Pack ID Single", onCallOPSucceed, onSaveFailed);
                }
			}
			
            // if (scanQty == palletQty) {
             //    saveData();
             //} else {
                 setInputFocus();
                 getAvailableData("processDataEntry");
             //}
         }     
     }
     function CheckInTable(str,index) {
         var tbl = "<%=GridViewExt1.ClientID %>";
         var table = document.getElementById(tbl);
         var subFindFlag = false;
         for (var i = 1; i < table.rows.length; i++) {
            // alert(table.rows[i].cells[0].innerText.trim());
             if ((table.rows[i].cells[index].innerText.trim() == str)) {
                 subFindFlag = true;
                 break;
             }
         }
         return subFindFlag;
     }
     function onSucceeded(result) {
         ShowInfo("");
         if (result == null || result.length == 0) {
             ShowErrorMessage(msgSystemError);
         }
         else if ((result[0] == "PAK144") && result.length == 2) {  //再开启新的Pallet Verify 流程
             ResetPage();
             ShowErrorMessage(result[1]);
             ifFirst = true;
             isShow = true;
             falseCustSn = result[1];
             //falsePalletNo = palletNo;
             inputCustSN(secondSn);
         }
         else if ((result[0] == "CHK880" || result[0] == "SFC002" || result[0] == "CHK881") && result.length == 2) {
            ShowErrorMessage(result[1]);
         }
         else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
             // SVN 2569: 如果Scan Qty >= Pallet Qty，则报告错误：“数量超出范围!”。
             // SVN 3094: 如果Scan Qty > Pallet Qty，则报告错误：“数量超出范围!”。
             if (scanQty > palletQty) {
                 ShowErrorMessage(msgQtyFull);
             }
             else {
                 endWaitingCoverDiv();
                 inputSNControl.value = "";
                 ifFirst = false;

                 var newSelectedRowIndex = 1;
                 //newSelectedRowIndex = result[1];
                 /*if (newSelectedRowIndex >= CustList.length) {
                     ShowErrorMessage(msgInvalidSN);    //非法数据
                 }
                 else {
                */
                     /// result[1]: [0],    [1],      [2],     [3],            
                     ///             palletNo,prodIdLst,custSnLst,carton no
                if( CheckInTable(result[1][3],1)) {
                    ShowErrorMessage(msgDupCarton);    //Carton No 已经录入
                    PlaySound();
                    inputSNControl.value = "";
                    setInputFocus();
                    getAvailableData("processDataEntry");
                }
                else{
                     var rowInfo = new Array();
                     rowInfo.push(result[1][1]);
                     rowInfo.push(result[1][3]);
                     rowInfo.push(result[1][2]);
                     rowInfo.push("");
                     scanQty++;
                     document.getElementById("txtScanQty").value = scanQty;
                     AddRowInfo(rowInfo);

                     ///高亮当前行
                     var gdObj = document.getElementById("<%=GridViewExt1.ClientID %>");
                     var con = gdObj.rows[newSelectedRowIndex + 1]; //index从1开始
                     setGdHighLight(con);
                     if (scanQty == palletQty) {
                         if (document.getElementById("<%=gdCheckBox.ClientID%>").checked) {
                             saveData();
                         }
                         else {
                             inputSNControl.value = "";
                             setInputFocus();
                             getAvailableData("processDataEntry");
                         }
                     }
                     else {
                         inputSNControl.value = "";
                         setInputFocus();
                         getAvailableData("processDataEntry");
                     }
                 }
                
             }

         }
     }

     function onFirstSnFailed(result) {
         
         ExitPage();
         firstCustSn = "";
         ifFirst = true;
         inputSNControl.value = "";
         if (isShow)
             isShow = false;
         ShowErrorMessage(result.get_message());
     }
     
     function onFailed(result) {
         ResetPage(); // Maintis : 0000705 // UC更新：[2012-5-3] 只有刷入的Customer S/N 非当前栈板上的Product 的Customer S/N 时，才要Cancel 当前的Pallet Verify 流程，再开启新的Pallet Verify 流程
         ShowErrorMessage(result.get_message());
     }

     var iSelectedRowIndex = null;
     function setGdHighLight(con) {
         if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=GridViewExt1.ClientID %>");     //去掉过去高亮行           
        }
        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=GridViewExt1.ClientID %>");     //设置当前高亮行
        iSelectedRowIndex = parseInt(con.index, 10);    //记住当前高亮行
     }

         
     
     function saveData() {
         try {
             
             lstPrintItem = getPrintItemCollection();
             if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
             {
                 PlaySound();
                 ResetPage();               // 重刷
                 ShowInfo(msgPrintSettingPara);
                 return;
             }
             else {
                 // beginWaitingCoverDiv(); // 保存时统一修改为不启动waiting
                // lstPrintItem[0].LabelType = "DK_Travel_Card";
                WebServicePalletVerify.save(firstCustSn, lstPrintItem, onSaveSucceed, onSaveFailed);
             }
         }
         catch (e) {
             PlaySound();
             ResetPage();           
             ShowInfo(e);
             
         }

     }

     function onSaveSucceed(result) {
         endWaitingCoverDiv();
         ifFirst = true;
         if (result == null) {
             //service方法没有返回
             ShowErrorMessage(msgSystemError);
             ResetPage();
         }
         else if (result[0] == SUCCESSRET) {
             inputSNControl.focus();

             var LabelType;
             if (labeltypeBranch == "A") {
				//if (PDFPLLst != null) {
                    tryToFindPDFFile();
                //}
			 }
			 else if (result[1][3] == 'C') {
                 LabelType = "DK_Pallet_Label"; //packing label 
             }
             else {
                 LabelType = "DK_Pallet_Label_2"; //Consolidate Label
             }
             onPrintFun(LabelType, result)

             var SuccessItem = "[" + palletNo + "]";
             ResetPage();
             ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
         }
        // ResetPage();
     }

     function onPrintFun(LabelType, result) { 
         try {
                var labelCollection = [];
                for (var i in result[1][0]) {
                     if (result[1][0][i].LabelType == LabelType) {
                         labelCollection.push(result[1][0][i]);
                         break;
                     }
                 }
                 var keyCollection = new Array();
                 var valueCollection = new Array();

                 keyCollection[0] = "@Pallet";
                 valueCollection[0] = generateArray(palletNo);

                 setPrintParam(labelCollection, LabelType, keyCollection, valueCollection);
                 printLabels(labelCollection, false);
                 }

                 catch (e) {
                     alert(e.description);                
                 }
     }

     function onSaveFailed(result) {
         ifFirst = true;
         ResetPage();
         ShowErrorMessage(result.get_message());
     }

     function onCallOPSucceed(result) {
         if (result == null) {
             //service方法没有返回
             ShowErrorMessage(msgSystemError);
         }
         else if (result[0] == SUCCESSRET) {
                 //CP170000049
            var rowsCount = result[2];
            var colsCount = result[3];
            var begNo = 0;
            var endNo = result[1].length;
            //DEBUG Clear arrTemp
            arrTemp = new Array();
            for (var i = 0; i < rowsCount; i++) {
                 arrTemp[i] = new Array();
                 for (var j = 0; j < colsCount; j++) {
                     if (begNo < endNo) {
                         arrTemp[i][j] = result[1][begNo];
                         begNo++;
                     }
                 }
             }
           
           //  PDFPrint();
           //  ResetPage();
           //  ShowSuccessfulInfo(true, msgSaveSucc); 
           
           generatePDFinFirstScanCustSN();
           
         }
         else {
             var content = result[0];
             ShowErrorMessage(content);
         }
     }
    
    
    function setPrintItemListParam(backPrintItemList,dn,labeltype) {
        //============================================generate PrintItem List==========================================
        //        /*
        //         * Function Name: setPrintParam
        //         * @param: printItemCollection
        //         * @param: labelType
        //         * @param: keyCollection(Client: Array of string.    Server: List<string>)
        //         * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        //         */
        var lstPrtItem = new Array();  
        lstPrtItem[0]= backPrintItemList;

        var keyCollection = new Array();
        var valueCollection = new Array();

       // keyCollection[0] = "@PalletNo";
        keyCollection[0] = "@palletno";
        keyCollection[1] = "@deliveryno"

        valueCollection[0] = generateArray(palletNo);
        valueCollection[1] = generateArray(dn);

        setPrintParam(lstPrtItem, labeltype, keyCollection, valueCollection);
        printLabels(lstPrtItem, false);
    }

//    function para_transfer(pdfFilename, xmlFilename, templatename) {
//        PdfFilename = pdfFilename;
//        XmlFilename = xmlFilename;
//        Templatename = templatename;
//    }
    
   function generatePDFinFirstScanCustSN() {

        try
        {
           var plt = palletNo;
           var arrDelivery = new Array();
           arrDelivery = DeliveryPerPalletList;
           var edi_Delivery = DeliveryPerPalletList[0];


           if (arrTemp != null && arrTemp.length>0)
           {
              
               var rowsCount =  arrTemp.length;
              

               if (rowsCount > 1)
               {
                   for (var TpCount = 0; TpCount < rowsCount; TpCount++)
                   {
                       var Template = arrTemp[TpCount][0].trim();
                       var DOC_SET = arrTemp[TpCount][2].trim();
					   var XSD = '';
                       if (arrTemp[TpCount].length >= 4)
                           XSD = arrTemp[TpCount][3].trim();
                       var Sntp = "";
                       var Attp = "";
                       var plttp = "";

                       //if (DOC_SET.search("LA")!=-1 || DOC_SET.search("NA-00010")!=-1 || DOC_SET.search("EM")!=-1)
                       //{
                           if (Template.search("Serial")!=-1)
                           {
                               Sntp = "SN";
                           }
                           else if (Template.search("General")!=-1)
                           {
                               Sntp = "Ship";
                           }
                           else if (Template.search("BOX_ID")!=-1)
                           {
                               Sntp = "EMEA";
                           }
                           else Sntp = "";
                       //}

                       var TCount = 0;

                       if (Template.search("_ATT")!=-1)
                       {
                           Attp = "ATT";
                       }
                       else if (Template.search("Verizon2D")!=-1)
                       {
                           Attp = "2D";
                       }
                       else Attp = "";

                       if (Template.search("TypeB")!=-1)
                       {
                           plttp = "B";
                           CmdGeneratePdf(edi_Delivery, plt, "PLT", Template, Attp, TCount, Sntp, XSD);
                       }
                       else
                       {
                           plttp = "A";

                           var arrDNlength = arrDelivery.length;
                           for (var DNCount = 0; DNCount < arrDNlength; DNCount++)
                           {
                               CmdGeneratePdf(arrDelivery[DNCount], plt, "PLT", Template, Attp, TpCount, Sntp, XSD);
                           }
                       }

                   }
               }
               else
               {
                   var Template = arrTemp[0][0].trim();
				   var XSD = '';
                   if (arrTemp[0].length >= 4)
                       XSD = arrTemp[0][3].trim();
                   var Sntp = "";
                   var Attp = "";
                   var TpCount= 0;
                   var plttp = "";

                   if (Template.search("TypeB")!=-1)
                   {
                       plttp = "B";
                       CmdGeneratePdf(edi_Delivery, plt, "PLT", Template, Attp, TpCount, Sntp, XSD);
                   }
                   else
                   {
                       plttp = "A";

                       var arrDNlength = arrDelivery.length;
                       for (var DNCount = 0; DNCount < arrDNlength; DNCount++)
                       {
                           CmdGeneratePdf(arrDelivery[DNCount], plt, "PLT", Template, Attp, TpCount, Sntp, XSD);
                       }
                   }
               }
           }
        }
        catch (e) {
             PlaySound();
             ResetPage();           
             ShowInfo(e);
             
         }

    }

   function CmdGeneratePdf(DeliveryNo, Pallet, plttp, Template, Attp, TCount, Sntp, XSD)
    {
        try
        {
            var cmdGeneratePdf = false;
            
            Templatename = Template;
            
            if (Template.search("TypeB")!=-1)
            {
                plttp = "B";
            }
            else plttp = "A";
            
            
            XmlFilename = DeliveryNo + "-" + Pallet + "-[PalletShipLabel]" + plttp.trim() + Sntp.trim() + ".xml";

            

            if (Attp == "ATT")
            {
                PdfFilename = DeliveryNo + "-" + Pallet + "-[ATTPalletShipLabel]" + Sntp.trim() + ".pdf";
            }
            else if (Attp == "2D")
            {
                PdfFilename = DeliveryNo + "-" + Pallet + "-[2DPalletShipLabel]" + Sntp.trim() + ".pdf";
            }
            else 
            {
                PdfFilename = DeliveryNo + "-" + Pallet + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
            }

           // para_transfer();

            if (TCount == 0)
            {

                GenerateCaseSGTIN96(DeliveryNo, Pallet, plttp, XSD);

            }

            //generatePDF();
            cmdGeneratePdf = true;
 
        }
       catch (e) {
             PlaySound();
             ResetPage();           
             ShowInfo(e);
             
         }

    }

    function GeneratePDF() {
        var wsh = new ActiveXObject("wscript.shell");
        // var path = "C:\\Program" + " " + " " + "Files\\Altova\\FOP-0.93\\fop -xml \\hp-iis\\OUT\\PLTXML\\" + XmlFilename + "-xsl" + "\\hp-iis\\OUT\\" + Template + "-pdf" + "\\hp-iis\\OUT\\PLTPDF\\" + PdfFilename;
        var path = PDFPLLst[5] + "-xml" + PDFPLLst[2] + "PLTXML\\" + XmlFilename + "-xsl" + PDFPLLst[1] + Templatename + "-pdf" + PDFPLLst[3] + "PLTPDF\\" + PdfFilename;
        wsh.run("cmd /c " + path, 0, true);
        wsh = null;

    }

    function GenerateCaseSGTIN96(DeliveryNo, PLT, LType, XSD) {

        if (CallEDITSFunc(DeliveryNo, PLT, LType, XSD)) {

            CallPdfCreateFunc();
        }
    }


    function CallEDITSFunc(dn, plt, LType, XSD) {
        var Paralist = new EDITSFuncParameters();
        var xmlpathfile = "";
        var webEDITSaddr = "";
        if (GetDebugMode()) {
            //Debug Mode get Root path from Web.conf
            xmlpathfile = GetCreateXMLfileRootPath() + "PLTXML\\" + XmlFilename;
            CheckMakeDir(xmlpathfile);
            //webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
            webEDITSaddr = GetEDITSTempIP();   //Packing List for aaaa
        }
        else {
            //Run Mode Get Path from DB, set Full Path
            xmlpathfile = PDFPLLst[2]+"PLTXML\\"+ XmlFilename;
            webEDITSaddr = PDFPLLst[0];
            //webEDITSaddr = "http://10.190.40.68:8080/edits.asmx"; // Packing List for aaa
        }
        Paralist.add(1, "FilePH", xmlpathfile);
        Paralist.add(2, "Dn", dn);
        Paralist.add(3, "Pallet", plt);
        var IsSuccess = false;
		if (LType == "A") {
            IsSuccess = invokeEDITSFunc(webEDITSaddr, "PalletAShipmentLabel", Paralist);
        }
        else {
            if (XSD.toUpperCase().indexOf('BSAM') > 0)
                IsSuccess = invokeEDITSFunc(webEDITSaddr, "BsamPalletBShipmentLabel", Paralist);
            else
                IsSuccess = invokeEDITSFunc(webEDITSaddr, "PalletBShipmentLabel", Paralist);
        }
        if (!IsSuccess) {
            PlaySound();
            ShowInfo(palletNo + " " + msgCallEditsFail);
        }
        return IsSuccess;
    }

    function CallPdfCreateFunc() {
        var xmlfilename, xslfilename, pdffilename;

        if (GetDebugMode()) {
            //Debug Mode get Root path from Web.conf
            xmlfilename = "PLTXML\\" + XmlFilename;
            xslfilename = Templatename;
            pdffilename = "PLTPDF\\" + PdfFilename;
        }
        else {
            var xml_path = PDFPLLst[2];
            var temp_path = PDFPLLst[1];
            var pdf_path = PDFPLLst[3];
            //Run Mode Get Path from DB, set Full Path
            xmlfilename = xml_path + "PLTXML\\" + XmlFilename;
            xslfilename = temp_path + "\\" + Templatename;
            pdffilename = pdf_path + "PLTPDF\\" + PdfFilename; 

            EndPDFFile = pdffilename;
        }

        //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
        //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
        //---------------------------------------------------------------
        var islocalCreate = false;
        //var islocalCreate = true;
        //================================================================
        var exe_path = PDFPLLst[5];
        //var IsSuccess = CreatePDFfileAsyn(exe_path, xmlfilename, xslfilename, pdffilename, islocalCreate);
        
        var webEDITSaddr = PDFPLLst[0];
        var IsSuccess = CreatePDFfileAsynGenPDF(webEDITSaddr, xmlfilename, xslfilename, pdffilename, islocalCreate);
        
        if (!IsSuccess) {
            PlaySound();
            ShowInfo(palletNo + " " + msgCreatePDFFail);
        }
        return IsSuccess;
    }



    function GetEDITSIP() {
        var HPEDITSIP = '<%=ConfigurationManager.AppSettings["HPEDITSIP"].Replace("\\", "\\\\")%>';
        return HPEDITSIP;
    }

    function GetEDITSTempIP() {
        var HPEDITSTempIP = '<%=ConfigurationManager.AppSettings["HPEDITSTEMPIP"].Replace("\\", "\\\\")%>';
        return HPEDITSTempIP;
    }
    function GetFopCommandPathfile() {
        var FopCommandPathfile = '<%=ConfigurationManager.AppSettings["FopCommandPathfile"].Replace("\\", "\\\\")%>';
        return FopCommandPathfile;
    }

    function GetTEMPLATERootPath() {
        var TEMPLATERootPath = '<%=ConfigurationManager.AppSettings["TEMPLATERootPath"].Replace("\\", "\\\\")%>';
        return TEMPLATERootPath;
    }
    function GetCreateXMLfileRootPath() {
        var CreateXMLfileRootPath = '<%=ConfigurationManager.AppSettings["CreateXMLfileRootPath"].Replace("\\", "\\\\")%>';
        return CreateXMLfileRootPath;
    }
    function GetCreatePDFfileRootPath() {
        var CreatePDFfileRootPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileRootPath"].Replace("\\", "\\\\")%>';
        return CreatePDFfileRootPath;
    }
    function GetCreatePDFfileClientPath() {
        var CreatePDFfileClientPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileClientPath"].Replace("\\", "\\\\")%>';
        return CreatePDFfileClientPath;
    }
    function GetDebugMode() {
        var DEBUGmode = '<%=ConfigurationManager.AppSettings["DEBUGmode"]%>';
        if (DEBUGmode == "True")
            return true;
        else
            return false;
    }
   
    var tryTimes = 0;
    function tryToFindPDFFile() 
    {
      //  EndPDFFile = "D:\\itc208014\\IMES2012\\FIS\\3.pdf"; //testing
        try {
            var fso = new ActiveXObject("Scripting.FileSystemObject");
            if (!(fso.FileExists(EndPDFFile))) {
                sleep(5000);
                if (!(fso.FileExists(EndPDFFile))) {
                    var err = palletNo + " " + msgCreatePDFFail;
                    ResetPage();
                    PlaySound();
                    ShowInfo(err + " " + msgReprint);
                }
                else {
                    PDFPrint();
                }
            }
            else PDFPrint();
        }
        catch (e)
         { }
    }
    
   
    //PalletVerify 自动单列印 Pallet Label (PDF打印：Non-bulk  Pallet Label for EDITS order）
    function PDFPrint() {
       try {
        
            var Fs = new ActiveXObject("Scripting.FileSystemObject");
            var FsFile = "C:\\FIS\\pdfprintlist.txt";
            var Fsfolder = "C:\\FIS";
            var FsEnabled = true;
            var File;
            //var printerpath = PDFPLLst[6];
             var printerpath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
			if (printerpath==null || printerpath=="")
			{
				printerpath = Fsfolder;
			}

			if (printerpath.slice(-1) != "\\") {
			    printerpath = printerpath + "\\";
			}

            if (!FsEnabled) {
                ResetPage();
                return;
            }
            if (!Fs.FolderExists(Fsfolder)) {
                Fs.CreateFolder(Fsfolder);
            }

            if (Fs.FileExists(FsFile)) {
                Fs.DeleteFile(FsFile);
            }

           File= Fs.CreateTextFile(FsFile,FsEnabled);


           var arrDelivery = new Array();
           arrDelivery = DeliveryPerPalletList;
          

           // 调SP：HP_EDI.dbo.op_TemplateCheck_LANEW, ==> arrTemp
            
           var Template = "";  
           var DOC_SET ="";
           var Sntp = "";
           var plttp = "";
           var EditsFISAddr =PDFPLLst[3];
           var rowsCount = arrTemp.length;

           if (rowsCount > 1) 
           {

               for (var TmpCount = 0; TmpCount < rowsCount; TmpCount++) 
               {

                       Template = arrTemp[TmpCount][0].trim();
                       DOC_SET = arrTemp[TmpCount][2].trim();
                       Sntp = "";
                       plttp = "";

                       //if (DOC_SET.search("LA") != -1 || DOC_SET.search("NA-00010") != -1 || DOC_SET.search("EM") != -1) 
                       //{
                           if (Template.search("Serial") != -1)  //ITC-1360-1596
                           {
                               Sntp = "SN";
                           }
                           else if (Template.search("General") != -1) 
                           {
                               Sntp = "Ship";
                           }
                           else if (Template.search("BOX_ID") != -1) 
                           {
                               Sntp = "EMEA";
                           }
                           else Sntp = "";
                      //}

                      if (Template.search("_ATT") == -1 && Template.search("2D") == -1) 
                      {
                          if (Template.search("TypeB") != -1) 
                           {
                               plttp = "B";
                           }
                           else plttp = "A";
                       
                           if (Sntp != "SN" && Sntp != "EMEA") 
                           {
                               if (plttp == "A") 
                               {
                                   for (j = 0; j < arrDelivery.length; j++) 
                                   {
                                       if (!(((modelList[j].substr(9, 2) == "AA" || modelList[j].substr(9, 2) == "AM")) && Sntp=="Ship"))
                                       { 
                                           var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                           var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                           File.WriteLine(pdfPath);
                                           File.WriteLine(pdfPath);
                                       }
                                   }
                               }
                               else 
                               {
                                   if (!(((modelList[0].substr(9, 2) == "AA" || modelList[0].substr(9, 2) == "AM")) && Sntp == "Ship")) 
                                   {
                                       var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                       var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                       File.WriteLine(pdfPath);
                                       File.WriteLine(pdfPath);
                                   }
                               }
                           }
                           else 
                           {
                               if (plttp == "A") 
                               {
                                   for (j = 0; j < arrDelivery.length; j++) 
                                   {
                                       var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                       var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                       File.WriteLine(pdfPath);

                                   }
                               }
                               else 
                               {
                                   var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                                   var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                   File.WriteLine(pdfPath);

                               }
                           }
                       }
                       else 
                       {
                           if (Template.search("TypeA") != -1 && Template.search("2D") != -1)
                           {
                               plttp = "2DA";
                           }
                           else if (Template.search("TypeB") != -1 && Template.search("2D") != -1) 
                           {
                               plttp = "2DB";
                           }
                           else if (Template.search("TypeB") != -1 && Template.search("_ATT") != -1) 
                           {
                               plttp = "ATTB";
                           }
                           else plttp = "ATTA";

                           if (plttp == "ATTA") 
                           {
                               for (j = 0; j < arrDelivery.length; j++) 
                               {
                                   var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[ATTPalletShipLabel]" + Sntp.trim() + ".pdf";
                                   var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                   File.WriteLine(pdfPath);
                               }
                           }
                           else if (plttp == "ATTB") 
                           {
                                var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[ATTPalletShipLabel]" + Sntp.trim() + ".pdf";
                                var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                File.WriteLine(pdfPath);

                           }
                           else if (plttp == "2DA") 
                           {
                                for (j = 0; j < arrDelivery.length; j++) 
                                {
                                   var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[2DpalletShipLabel]" + Sntp.trim() + ".pdf";
                                   var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                                   File.WriteLine(pdfPath);
                               }
                           }
                           else 
                           {
                               var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[2DpalletShipLabel]" + Sntp.trim() + ".pdf";
                               var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                               File.WriteLine(pdfPath);

                           }
                       }
                   }
           }   
           else 
           {
               var Template = arrTemp[0][0].trim();
               var plttp = "";

               if (Template.search("TypeB") != -1) 
               {
                   plttp = "B";
               }
               else plttp = "A";

               if (plttp == "A") 
               {
                   for (j = 0; j < arrDelivery.length; j++) 
                   {
                       if (!(((modelList[j].substr(9, 2) == "AA" || modelList[j].substr(9, 2) == "AM")) && Sntp == "Ship")) 
                       {
                           var PdfFilenameForPrint = arrDelivery[j].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                           var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                           File.WriteLine(pdfPath);
                           File.WriteLine(pdfPath);
                       }
                   }
               }
               else 
               {
                   if (!(((modelList[0].substr(9, 2) == "AA" || modelList[0].substr(9, 2) == "AM")) && Sntp == "Ship")) 
                   {
                       var PdfFilenameForPrint = arrDelivery[0].trim() + "-" + palletNo.trim() + "-[PalletShipLabel]" + Sntp.trim() + ".pdf";
                       var pdfPath = EditsFISAddr + "PLTPDF\\" + PdfFilenameForPrint;
                       File.WriteLine(pdfPath);
                       File.WriteLine(pdfPath);
                   }
               }

           }

           File.Close();

           var wsh = new ActiveXObject("wscript.shell");
           //var cmdpdfprint = "PDFPrint.exe C:\\FIS\\pdfprintlist.txt 4000";
           var cmdpdfprint = "PrintPDF.bat C:\\FIS\\pdfprintlist.txt 4000";
           wsh.run("cmd /k " + getHomeDisk(printerpath) + "&cd " + printerpath + "&" + cmdpdfprint + "&exit", 2, false);  //打印步驟改爲異步操作
           wsh = null;

           var SuccessItem = "[" + palletNo + "]" + " " + msgSuccess; 
           ResetPage();
           ShowSuccessfulInfo(true, SuccessItem + " " + msgSaveSucc); 
            
    }
    
    catch (err)
    {
        ResetPage();
        PlaySound();
        ShowInfo(err.description);
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
         infoValue = "";
         falseCustSn = "";
         falsePalletNo = "";
         document.getElementById("txtPalletNo").value = "";
         document.getElementById("txtScanQty").value = 0;
         document.getElementById("txtPalletQty").value =0;
         inputSNControl.value = "";
         setInputFocus();
         clearData();
         CustList.length = 0;
         DeliveryPerPalletList.length = 0;
         modelList.length = 0;
         PDFPLLst.length = 0;
         index = 1;
         labeltypeBranch = "";
         clearTable();
         ifFirst = true;
         EndPDFFile = "";
         tryTimes = 0;
         getAvailableData("processDataEntry");

     }


     function ShowErrorMessage(result) {
         endWaitingCoverDiv();
         PlaySound();
         ShowInfo(result);
         inputSNControl.value = "";
         setInputFocus();
         getAvailableData("processDataEntry");
     }


     function showPrintSettingDialog() {
         showPrintSetting(station, pcode);
     }
     
     function rePrint() {
         var url = "ReprintPalletVerify.aspx?Station=" + station + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer +"&AccountId=" + accountId +  "&Login=" + login;
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
             WebServicePalletVerify.Cancel(firstCustSn);
             sleep(waitTimeForClear);
         }
     }

     function ResetPage() {
         ExitPage();
         ClearData();
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
     window.onunload = function() {
         ExitPage();
     };
     
     window.onbeforeunload = function() {
         ExitPage();

     };
     
 </script>
</asp:Content>



