<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/11/04 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/11/04            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-04   Du.Xuan               Create   
* Known issues:
* ITC-1360-0663 打印参数传入错误，修改
* ITC-1360-0645 alert后设置焦点
* ITC-1360-0648 更正PalletNo 12位长检查
* ITC-1360-0651 substring截取长度错误
* ITC-1360-0652 增加长度为6的判断
* ITC-1360-0654 称重为0错误提示
* ITC-1360-0655 增加刷入2D提示
* ITC-1360-0657 调整table显示比例
* ITC-1360-0660 增加IMEI 大于14位检查
* ITC-1360-0665 增加pallet type选择保护
* ITC-1360-1608 提示内容和重量字体调大
* ITC-1360-1622 选择[Pallet Type] 步骤调整到输入[Customer S/N] 前
* ITC-1360-1623 Save 成功，页面上的[Pallet Type]下拉框，默认复原到空白栏
* ITC-1360-1624 只有EMEA地区的才会需要扣栈板重量(pallet Type选择)
* ITC-1360-1673 success文字资源使用错误
* ITC-1360-1745 如果标准重量为0 的话,不需要进行误差检查,直接放行
* ITC-1360-1748 增加手动输入部分
* ITC-1360-1749 增大pallet Type字体
* TODO：
*/
 --%>
<%@ Register Src="../CommonControl/WeightTypeControl.ascx" TagName="WeightTypeControl"
    TagPrefix="uc1" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PalletWeight.aspx.cs" Inherits="PAK_PalletWeight" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>

    <asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="Service/PalletWeightWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="divl">
        <table style="width: 100%">
            <tr>
                <td style="width: 15%">
                    <asp:Label runat="server" ID="lblTotalQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblTotalQtyValue" Text="" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="div2">
        <fieldset style="width: 98%">
            <legend align="left" style="height: 20px">
                <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            </legend>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <table>
                            <colgroup>
                                <col style="width: 110px;" />
                                <col />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblPalletNo" Text="" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPalletNoValue" Text="" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblCustSN" Text="" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td colspan="3" style="width: 80%">
                                    <asp:Label runat="server" ID="lblCustSnValue" Text="" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblPalletType" Text="" CssClass="iMes_label_30pt_Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%">
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                            GvExtHeight="128px" Style="top: 0px; left: 0px; width: 98%;" Height="120px" SetTemplateValueEnable="False"
                            HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="div3">
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <asp:Label runat="server" ID="lblUnitWeight" Text="" CssClass="iMes_label_30pt_Black"></asp:Label>
                </td>
                <td colspan="4" style="width: 50%">
                    <asp:Label runat="server" ID="lblUnitWeightValue" Style="width: 99%;" readonly="readonly"
                        class="iMes_label_72pt_Red_Underline" Font-Bold="True" ForeColor="Red" Text="" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div4">
        <table style="width: 100%">
            <colgroup>
                <col style="width: 110px;" />
                <col />
                <col />
            </colgroup>
            <tr>
                <td style="width: 20%">
                    <asp:Label runat="server" ID="lblPalletKind" Text="" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <iMES:CmbPalletType ID="cmbPalletKind" runat="server" Width="50" IsPercentage="true"
                        CssClass="iMes_DropDownList" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="lblManual" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </td>
                <td style="width: 60%">
                    <asp:TextBox ID="txtManual" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%"
                        onkeypress='OnKeyPress(this)' />
                </td>
                <td style="width: 20%">
                    <asp:Label ID="lblKG" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td colspan="3" style="width: 60%">
                    <iMES:Input ID="Input1" runat="server" IsClear="true" ProcessQuickInput="true" Width="99%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td align="right" style="width: 20%">
                    <input id="btpPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    <button id="btnReprint" runat="server" onclick="clkReprint()" class="iMes_button"
                        width="200px" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" for="objMSComm" event="OnComm" type="text/javascript">  

        // MSComm1控件每遇到 OnComm 事件就调用 MSComm1_OnComm()函数
        objMSComm_OnComm();
  
    </script>

    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">

        var weightBuffer = "";
        var lastWeigth = 0.000;

        //重写 mscomm 控件的唯一事件处理代码  
        function objMSComm_OnComm() {
            var objMSComm = document.getElementById("objMSComm");
            if (objMSComm.CommEvent != 2) {
                //如果不是接收事件,就返回   
                return;
            }
            else if (objMSComm.CommEvent == 2)//如果是接收事件  
            {

                weightBuffer += objMSComm.Input;
                if (weightBuffer.length < 48)
                    return;
                var data = weightBuffer.substring(16);
                weightBuffer = "";
                //ST,GS,+00098.5kg
                //var index = data.indexOf("ST,GS,");
                //var index = data.indexOf("ST,");
                var index = data.indexOf("+");
                if (index >= 0) {
                    var result = data.substring(index + 1, index + 1 + 9);
                    //ShowInfo(weightBuffer);
                    var weight = getNumber(result);
                    if (weight == false) {
                        document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "0.000";
                    } else {
                        document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = weight;
                        lastWeigth = weight;
                    }
                }
                else {
                    document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "0.000";
                }
            }
        }

        function getNumber(str) {
            var result = false;
            if (typeof (str) == 'string') {
                result = str.replace(/[^\d\.]/g, '');
                result = isNaN(result) ? false : parseFloat(result);
            }

            return result;
        }
  
    </script>

    <script language="javascript" type="text/javascript">
 
        var DEFAULT_ROW_NUM = 3;
        var inputsn = "";
        var inputControl =getCommonInputObject();
        var editor;
        var customer;
        var station;
        var pcode;
        var login;
        var accountId;
        var vendorsnLen=0;
        var mbsn="";
        var hostname = getClientHostName();
        var WeightFilePath ='<%=System.Configuration.ConfigurationManager.AppSettings["UnitWeightPath"]%>'

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSystemError") %>';
        var mesNoSelWeightType = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "mesNoSelWeightType") %>';
        var msgNoPalletNo = '<%=this.GetLocalResourceObject(languagePre + "msgNoPalletNo").ToString() %>';
        var msgNoCustSN = '<%=this.GetLocalResourceObject(languagePre + "msgNoCustSN").ToString() %>';
        var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgPrintSettingPara") %>';  
        var msgInputSN='<%=this.GetLocalResourceObject(languagePre + "msgInputSN").ToString() %>'; 
        var msgInputCarton='<%=this.GetLocalResourceObject(languagePre + "msgInputCarton").ToString() %>'; 
        var msgSave='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSave") %>';      
        var msgWeightNull='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgWeightNull") %>';
        var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgPrintSettingPara") %>';  
        var msgPalletNoErr = '<%=this.GetLocalResourceObject(languagePre + "msgPalletNoErr").ToString() %>';
        var msgCodeErr = '<%=this.GetLocalResourceObject(languagePre + "msgCodeErr").ToString() %>';
        var msgUnitWeightNull ='<%=this.GetLocalResourceObject(languagePre + "msgUnitWeightNull").ToString() %>';
        var msgPalletType = '<%=this.GetLocalResourceObject(languagePre + "msgPalletType").ToString() %>';
        var msgScan2D='<%=this.GetLocalResourceObject(languagePre + "msgScan2D").ToString() %>';
        var msgConfirm1='<%=this.GetLocalResourceObject(languagePre + "msgConfirm1").ToString() %>';
        var msgConfirm2='<%=this.GetLocalResourceObject(languagePre + "msgConfirm2").ToString() %>';
        var msgManualFormat='<%=this.GetLocalResourceObject(languagePre + "msgManualFormat").ToString() %>';
        
        var lblCartonSN='<%=this.GetLocalResourceObject(languagePre + "lblCartonSN").ToString() %>';
        var lblCustSN='<%=this.GetLocalResourceObject(languagePre + "CustSN").ToString() %>';
 
        var productID="";
        var palletNo="";
        var custSN="";
        var cartonSN ="";

        var palletObj=document.getElementById("<%=lblPalletNoValue.ClientID %>"); 
        var palletWeight="";
        var palletType ="";

        var palletCheck = false;
        var palletTypeCheck = false;
        var cartonCheck = false;
        var command ="";
        var imes ="";

        var standardWeight;
        var tolerance;

        var lstPrintItem;
        var actWeight;

        var tbl;

        var isPC = false;

        document.body.onload = function ()
        {
             try {

                /*editor = '<%=UserId%>';
                customer = '<%=Customer %>';
                station = '<%=Station%>';
                pcode = '<%=Pcode%>';
                */
                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                station = '<%=Request["Station"] %>';
                pcode = '<%=Request["PCode"] %>';
                accountId = '<%=Request["AccountId"] %>';
                login = '<%=Request["Login"] %>';

                tbl = "<%=gd.ClientID %>";
                
				$.exclusionInOut({ acquire: function() {
					return setCommPara("P",WeightFilePath);
				},
				release: function() {
					var objMSComm =document.getElementById("objMSComm");
					if (objMSComm && objMSComm.PortOpen) {
						objMSComm.PortOpen = false;
					}
					return true;
				}
				});
                //setCommPara("P",WeightFilePath);
                initPage();

                //var objMSComm =document.getElementById("objMSComm");
                //ShowInfo("CommPort,Settings,RThreshold,SThreshold,Handshaking,PortOpen  is: "+objMSComm.CommPort+" , "+objMSComm.Settings +" , "+objMSComm.RThreshold+" , "+objMSComm.SThreshold+" , "+objMSComm.Handshaking+" , "+objMSComm.PortOpen);     
                //callNextInput();
                
            }
            catch (e) {
                alert(e.description);
            }
        }

        window.onbeforeunload= function() 
        {
           ExitPage();
        }
         
         function initPage() {
                    
                    //setInputOrSpanValue(document.getElementById("<%=lblTotalQtyValue.ClientID %>"), 0);
                    setInputOrSpanValue(document.getElementById("<%=lblCustSN.ClientID %>"), lblCustSN);
                    setInputOrSpanValue(document.getElementById("<%=lblPalletNoValue.ClientID %>"), "");
                    setInputOrSpanValue(document.getElementById("<%=lblCustSnValue.ClientID %>"), "");
                    setInputOrSpanValue(document.getElementById("<%=lblPalletType.ClientID %>"), "");
                    setInputOrSpanValue(document.getElementById("<%=lblUnitWeightValue.ClientID %>"), "0.000");
                    setInputOrSpanValue(document.getElementById("<%=txtManual.ClientID %>"), "");
                     
                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    
                    imes="";
                    productID="";
                    palletNo="";
                    custSN="";
                    cartonSN ="";
                    palletCheck = false;
                    palletTypeCheck = false;
                    cartonCheck = false;
                    command="";
                    
                    standardWeight =0.00;
                    tolerance =0.00;
                    
                    lastWeigth =0.00;
                    
                    getPalletTypeCmbObj().selectedIndex = 0;
                                        
                    callNextInput();
                          
       }

        function processDataEntry(inputData)
        {
            actWeight=document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText; 
            if (actWeight==null || actWeight=="" || parseFloat(actWeight) == 0)
            {
                actWeight = lastWeigth;
            }
            var manualWeight = document.getElementById("<%=txtManual.ClientID%>").value;
            if (manualWeight != null && manualWeight!= "")
            {
                actWeight = manualWeight;
            }
            var fweight =  parseFloat(actWeight);
            if (actWeight==null || actWeight=="" || fweight == 0)
            {
                if (Boolean( <%=isTestWeight%> ))
                {
                    alert("TEST!");
                    actWeight = "100";
                    document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText="100.000";
                }
                else
                {
                    alert(msgUnitWeightNull);
                    callNextInput();
                    return;
                }
            }
            
            if (actWeight=="")
            {
                alert(msgWeightNull);
                //ShowInfo(msgWeightNull);
                callNextInput();
                return ;
            }
            
            if( palletNo == "")
            {
                inputPallet(inputData);
                
            }
            else if (custSN == "" && cartonSN == "" && (isPC || cartonCheck))
            {
                 inputCustomSN(inputData);
            }
            else
            {
                
                if (palletCheck == true && imes==""){
                    inputIMEI(inputData);
                }
                else{
                    inputCode(inputData);
                }        
            }

        }

         function inputPallet(inputData)
         {
            if (!((inputData.length == 10) || (inputData.length == 11)
                ||(inputData.length == 12) || (inputData.length == 20))) 
            {
                alert(msgPalletNoErr);
                //ShowInfo(msgPalletNoErr);
                callNextInput();
            }
            else{
                beginWaitingCoverDiv();
                //station = "69";
                //var line = "";
                //palletNo = "0085477195";
                PalletWeightWebService.InputPallet(inputData, actWeight,"","", editor, station, customer,onPalletReturn);        
            }
        }


        function onPalletReturn(result)
        {
             endWaitingCoverDiv();
             document.getElementById("<%=lblPalletNoValue.ClientID %>").innerText = "";
             var msgall="";
             
             if (result == null) {
                ShowInfo("");
                ResetPage()
                alert(msgSystemError);
                ShowInfo(msgSystemError);
                callNextInput();
                return;
             }
             else if (result[0] == SUCCESSRET){

                setInfo(result);
                setTable(result);
                //if (palletCheck == true)
                if (result[8]!="")
                {
                    //alert(msgScan2D);
                    //alert(result[8]);
                    msgall=result[8];
                }
                if (isPC)
                { 
                    ShowInfo(msgall+" "+msgInputSN);
                    callNextInput();
                }
                else 
                {
                    if(cartonCheck)
                    {
                        ShowInfo(msgall+" "+msgInputCarton);
                        callNextInput();
                    }
                    else
                    {
                        if(palletCheck==true){
                            ShowInfo(msgall);
                            callNextInput();
                        }
                        else
                        {
                            if (command == ""){
                                palletSave();
                            }
                            else{      
                                callNextInput(); 
                           }
                        }   
                    }                   
                }
             }
             else {
                ShowInfo("");
                var content = result[0];
                ResetPage();
                alert(content);
                ShowInfo(content);
                callNextInput();
            }
        }
        function setInfo(result) {
        
            palletCheck = result[1];
            palletNo = result[2];
            palletType = result[3];
            command = result[4];
            standardWeight = result[6];
            tolerance = result[7];
            palletTypeCheck = result[9];
            cartonCheck = result[10];
            
            setInputOrSpanValue(document.getElementById("<%=lblPalletNoValue.ClientID %>"), palletNo);
           
            setInputOrSpanValue(document.getElementById("<%=lblPalletType.ClientID %>"), palletType);
            
        }

        function setTable(result)
        {
                var modleList = result[5];
               // isPC = true; 
                for (var i = 0; i < modleList.length; i++) {
                    
                    if (modleList[i]["Model"].substring(0,2) != "PC"){
                        isPC = false;
                    }
                    var rowArray = new Array();
                    var rw;

                    rowArray.push(modleList[i]["Model"]);
                    rowArray.push(modleList[i]["Qty"]);

                    //add data to table
                    if (i < DEFAULT_ROW_NUM-1) {
                        eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                        setSrollByIndex(i, true, tbl);
                    }
                    else {
                        eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                        setSrollByIndex(i, true, tbl);
                        rw.cells[1].style.whiteSpace = "nowrap";
                    }
                }
            
        }

     function inputCustomSN(inputData)
     {
            //请选择Pallet Type
         var type = getPalletTypeCmbValue();
           if (type == "" && palletTypeCheck && isPC==true) {
                alert(msgPalletType);
                callNextInput();
                return;
             }
			 var modeltype ="";
             customerSN = inputData;
              if (isPC == true){
                    customerSN = inputData.substring(inputData.length-10,inputData.length);  
                    modeltype ="PC"; 
                }

                beginWaitingCoverDiv();
                PalletWeightWebService.InputCustSN(customerSN, palletNo, type, modeltype,onCustomReturn);
         /*
         if (!(inputData.substring(0,3) == "SCN" || inputData.substring(0,2) == "CN")&& isPC == true)
                   
		   {
                alert(msgNoCustSN);
                //ShowInfo(msgNoCustSN);
                callNextInput();
            }
            else
            {   
                var modeltype ="";
                customerSN = inputData;
                if (isPC == true){
                    customerSN = inputData.substring(inputData.length-10,inputData.length);  
                    modeltype ="PC"; 
                }

                beginWaitingCoverDiv();
                PalletWeightWebService.InputCustSN(customerSN, palletNo, type, modeltype,onCustomReturn);
             }
			 */
    }

    function onCustomReturn(result)
    {
         endWaitingCoverDiv();
         if (result == null) {
            custSN="";
            cartonSN="";
            ShowInfo("");
            alert(msgSystemError);
            ShowInfo(msgSystemError);
            callNextInput();
         }
         else if (result[0] == SUCCESSRET){
            ShowInfo("");   

            custSN = result[1];
            cartonSN = result[2]; 
            if (isPC)
            {         
                setInputOrSpanValue(document.getElementById("<%=lblCustSN.ClientID %>"), lblCustSN);
                setInputOrSpanValue(document.getElementById("<%=lblCustSnValue.ClientID %>"), custSN);
            }
            else
            {
                setInputOrSpanValue(document.getElementById("<%=lblCustSN.ClientID %>"), lblCartonSN);
                setInputOrSpanValue(document.getElementById("<%=lblCustSnValue.ClientID %>"), cartonSN);
            }
            
            if(palletCheck==true){
                callNextInput();
            }
            else
            {
                if (command == ""){
                    palletSave();
                }
                else{      
                    callNextInput(); 
                }
            }
         }
         else{
            custSN="";
            ShowInfo("");
            var content = result[0];
            alert(content);
            ShowInfo(content);   
            callNextInput();
        }
    }

    function inputIMEI(inputData)
    {
        if (inputData.length<=14)
        {
            alert(msgCodeErr);
            callNextInput();
            return;
        }
        beginWaitingCoverDiv();
        PalletWeightWebService.inputIMEI(palletNo, inputData, onIMEIReturn);        
    }
    
    function onIMEIReturn(result)
    {
        endWaitingCoverDiv();
        if (result == null) {
            imes ="";
            ShowInfo("");
            //ResetPage();
            alert(msgSystemError);
            ShowInfo(msgSystemError);
            callNextInput();
        }
        else if (result[0] == SUCCESSRET){
            ShowInfo("");   
            imes = result[1];

            if (command ==""){
                palletSave();
            }
            else{      
                callNextInput(); 
            }
        }
        else{
            imes ="";
            ShowInfo("");
            //ResetPage();
            var content = result[0];
            alert(content);
            ShowInfo(content);   
            callNextInput();
        }
    }
    
    function inputCode(inputData)
    {
        /*if (inputData.length != 6)
        {
            alert(msgCodeErr);
            callNextInput();
            return;   
        }
        */        
        if (inputData == command) 
        {
                palletSave();
        }
        else
        {
            alert(msgCodeErr);
            //ShowInfo(msgCodeErr);
            callNextInput();
        }
        
    }
    
    function checkWeight()
    {   
        var alertFlag = false;
        var fweight =  parseFloat(actWeight);
        if (standardWeight >0)
        {
            if (fweight > (standardWeight+tolerance) || fweight < (standardWeight-tolerance))
            {
                alertFlag =true;
            }
        }
        return alertFlag;
        
    }


    function palletSave()
    {
        var alertFlag = checkWeight();
        
        if (alertFlag)
        {   
            var tmp = actWeight - standardWeight;
            var message = msgConfirm1+" " + tmp +" "+ msgConfirm2;
            if (!confirm(message)) {
             ResetPage();
             callNextInput();
             return;   
            }
        }
        
        try{
            lstPrintItem = getPrintItemCollection();
                    
            if (lstPrintItem == null){                     
                alert(msgPrintSettingPara);  
                return ;                                  
            } 
            else 
            {   
                //setPrintItemListParam(lstPrintItem, palletNo,actWeight);
                beginWaitingCoverDiv();
                PalletWeightWebService.Save(palletNo,custSN, actWeight, lstPrintItem,onSaveReturn,onSaveFail);
            }
         }
        catch(e) 
        {
            alert(e);
        }
    }

    function onSaveReturn(result)
    {
        endWaitingCoverDiv();
        if(result==null)
        {
            ResetPage();
            alert(msgSystemError);
            ShowInfo(msgSystemError);
            
        }
        if( result[0]==SUCCESSRET )     //length =5 result[0]:SUCCEED, [1]:printItem [2]:Pallet No [3]:Standard Weight [4]:Customer SN [5]:actual weight
        {
                 
            setPrintItemListParam( result[1],palletNo, actWeight);           
            printLabels(result[1], false);
            //OKQty +1
            var totalqty=parseInt(document.getElementById ("<%=lblTotalQtyValue.ClientID%>").innerText)+1;
            
            var tmpinfo= custSN;
            var tmpPalletNo = palletNo;
            ResetPage();
            if  (tmpinfo.trim() !="")
                ShowSuccessfulInfoFormat(true,"Customer SN",tmpPalletNo);
            else {
                 ShowSuccessfulInfoFormat(true,"Pallet No","Pallet No:" + tmpPalletNo);
            }
            document.getElementById ("<%=lblTotalQtyValue.ClientID%>").innerText=totalqty.toString();  
            
        }
        else    //has error
        {
            ResetPage();
            ShowMessage(result[0]); 
            ShowInfo(result[0]); 
        }
 
    }
            function onSaveFail(result) {

            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function onCancelClear(result)
        {
            if(result=="")
            {
                 //ShowInfo(""); 
            }
            else
            {
                 ShowMessage(result);
                 ShowInfo(result);
            }
            ClearPalletInfo();
        }

        function ClearPalletInfo()
        {
            document.getElementById("<%=lblPalletNoValue.ClientID %>").innerText="";
            palletcheck=false;
            inputControl.value="";
            palletNo="";
            palletObj.value="";  
            palletObj.focus();
        }

        function showPrintSettingDialog()
        {
            //station="99";
            //pcode="OPPA018";
            showPrintSetting(station,pcode);
        }

        function showError(msg)
        {
            ShowMessage(msg);
            ShowInfo(msg);    
            clearInfo();
        }

        function clearInfo()
        {
             document.getElementById("<%=lblPalletNoValue.ClientID %>").innerText="";
             palletcheck=false;
             inputControl.value="";
             palletNo="";
             palletObj.value="";
             palletObj.focus(); 
        }

        function setPrintItemListParam(backPrintItemList, plno,pltactWeight)
        {
         
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            
            keyCollection[0] = "@palletno";
            keyCollection[1] = "@weight";
            
            valueCollection[0] = generateArray(plno); 
            valueCollection[1] = generateArray(pltactWeight);
           
            setPrintParam(lstPrtItem, "Weight Label", keyCollection, valueCollection);
        }


        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }

        function ExitPage() {
            if(palletNo!=""){
                PalletWeightWebService.Cancel(palletNo);
                sleep(waitTimeForClear);   
            }  
        }
        
        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }       
             
         function clkReprint() {

            var url = "../PAK/ReprintPalletWeight.aspx" + "?Station=" + station + "&PCode=" + pcode+ "&UserId=" + editor + "&Customer=" + customer +"&AccountId=" + accountId +  "&Login=" + login;
            window.showModalDialog(url, "", 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }   
        
        function OnKeyPress(obj)
        {
            var key = event.keyCode;
        
            if (key == 13 || key == 9)//enter || tab
            {
                if(event.srcElement.id=="<%=txtManual.ClientID %>")
                {
                    if(check()==false)
                    {
                        //Input data format is not correct
                        alert(msgManualFormat);
                        return;
                    }
                    callNextInput();           
                }
            }       
        }
    
    function check()
    {
       var valueWeight=document.getElementById("<%=txtManual.ClientID %>").value;
        //则检查本框输入内容是否满足格式要求
        var reExp = /^[0-9]+(\.[0-9]?[0-9]?[0-9]?)?$/;
	    if (reExp.exec(valueWeight)==null||reExp.exec(valueWeight)==false){
		    return false;
	    }
	    
	    var fweight = parseFloat(valueWeight);
	    if( fweight>=1000 || fweight <= 0)
	    {	    
	        return false;
	    }
	    
        return true;
    }
    

    </script>

</asp:Content>
