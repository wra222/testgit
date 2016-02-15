<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:POD Label Check
 * UI:CI-MES12-SPEC-PAK-UI POD Label Check.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-UC POD Label Check.docx –2011/10/28           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28   Chen Xu               Create
 * 2012-03-01   Chen Xu               Modify : ITC-1360-0855: 对刷入的字串作以下转换：将字串中”/C”转为”#”
 * 2012-03-02   Chen Xu               Modify : ITC-1360-1008: 忘记将PN_13Length变量置回初始值了
 * Known issues:
 * TODO：
 * UC 具体业务：
 * UC Revision: 7294: CustPN 修改为Model
 */
--%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PODLabelCheck.aspx.cs" Inherits="PAK_PODLabelCheck" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path=  "~/PAK/Service/WebServicePODLabelCheck.asmx"  />
            </Services>
</asp:ScriptManager>   
 
<script type="text/javascript">

    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgScanModel = '<%=this.GetLocalResourceObject(Pre + "_msgScanModel").ToString() %>';
    var msgWrongModel = '<%=this.GetLocalResourceObject(Pre + "_msgWrongModel").ToString() %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var custSnOnCooValue = "";
    var model = "";
    var InputModel = ""; 
    var firstInput = false;
    var flag = false;
    var PN_13Length=false;
    var line = "";
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgConfiglabelSnWrong = '<%=this.GetLocalResourceObject(Pre + "_msgConfiglabelSnWrong").ToString()%>';
    var msgConfiglabelSnInput = '<%=this.GetLocalResourceObject(Pre + "_msgConfiglabelSnInput").ToString()%>';
    var msgConfiglabelMacWrong = '<%=this.GetLocalResourceObject(Pre + "_msgConfiglabelMacWrong").ToString()%>';
    var configLabelSN = "";
    var configLabelMACOrTag = "";
    
    var POD = "";
    var MRPCheck = "";
    var BoxID = "";
    var MrpCode = "60MRPLABEL01";
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Chen Xu
    //| Create Date	:	10/11/2011
    //| Description	:	加载接受输入数据事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    document.body.onload = function() {
        try {

            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;
            inputObj.focus();
            line = "";
            station = document.getElementById("<%=hiddenStation.ClientID %>").value;

            getAvailableData("processDataEntry");

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    };

    window.onbeforeunload = function() {
        ExitPage(); //ITC-1360-0843

    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	CustomerSNEnterOrTab
    //| Author		:	Chen Xu
    //| Create Date	:	10/12/2011
    //| Description	:	CustomerSN中按enter或tab键的处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function CustomerSNEnterOrTab() 
    {
        if (event.keyCode == 9 || event.keyCode == 13) 
        {
            getAvailableData("processDataEntry");
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	processDataEntry
    //| Author		:	Chen Xu
    //| Create Date	:	10/12/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputData) {
        ShowInfo("");
        var uutInput = true;

        if (inputData == "") 
        {
            alert(msgNoInputCustomerSn);
            inputObj.focus();
            uutInput = false;
            
            getAvailableData("processDataEntry");
        }

        if (uutInput) 
        {
            inputData = inputData.replace("/C", "#");   //ITC-1360-0855:对刷入的字串作以下转换：将字串中”/C”转为”#”
            
            if (firstInput == false)    
            {              
                getAvailableData("processDataEntry");
      
                if (checkCustomerSNOnCOOLabel(inputData) == false)  
                {
                    alert(msgDataEntryField);
                    inputObj.value = "";      
                    inputObj.focus();
                    
                    getAvailableData("processDataEntry");
                }
                else 
                {                  
                    beginWaitingCoverDiv();
                    WebServicePODLabelCheck.inputCustSNOnCooLabel(custSnOnCooValue, line, "<%=UserId%>", station, "<%=Customer%>", onSNSucceed, onSNFail);
                }
                
            }
            else 
            {
                beginWaitingCoverDiv();
                if (POD == "PODLabel") {
                    if (checkInputMRPCheck(inputData) == false) {
                        inputObj.value = "";
                        inputObj.focus();
                        getAvailableData("processDataEntry");
                    }
                    else {
                        if (PN_13Length == false) {
                            if (checkInputModel(inputData) == false) {
                                inputObj.value = "";
                                inputObj.focus();
                                getAvailableData("processDataEntry");
                            }
                            else {
                                InputModel = inputData;
                                save();
                            }
                        }
                    }
                }
                else if (POD == "ConfigLabel") {
                
                    if (checkInputMRPCheck(inputData) == false) {
                        inputObj.value = "";
                        inputObj.focus();
                        getAvailableData("processDataEntry");
                    }
                    else{
                        if (configLabelSN == "") {
                            if (checkCustomerSNOnConfigLabel(inputData) == false) {
                                ShowMessage(msgConfiglabelSnWrong);
                                ShowInfo(msgConfiglabelSnWrong);
                                configLabelSN = "";
                                inputObj.value = "";
                                inputObj.focus();
                                getAvailableData("processDataEntry");
                            }
                            else {
                                configLabelSN = inputData;
                                inputObj.value = "";
                                inputObj.focus();
                                getAvailableData("processDataEntry");
                                ShowInfo(msgConfiglabelSnInput);
                            }
                        }
                        else if (configLabelMACOrTag == "") {
                            configLabelMACOrTag = inputData;
                            saveConfigLabel();
                        }
                        else {
                            ResetPage();
                            callNextInput();
                        }
                    }
                }
                else {
                    if (checkInputMRPCheck(inputData) == false) {
                        inputObj.value = "";
                        inputObj.focus();
                        getAvailableData("processDataEntry");
                    }
                    else {
                        saveMRPLabel();
                    }
                }
            }        
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	save
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	save
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function save() {
        beginWaitingCoverDiv();
        var productIdValue = document.getElementById("lblProductIDDisplay").value;

        //WebServicePODLabelCheck.inputCustPNOnPODLabel(productIdValue, custSnOnCooValue, InputModel, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
        WebServicePODLabelCheck.inputCustPNOnPODLabel(productIdValue, "", InputModel, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	save
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	save
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function saveConfigLabel() {
        beginWaitingCoverDiv();
        var productIdValue = document.getElementById("lblProductIDDisplay").value;
        WebServicePODLabelCheck.inputCustPNOnPODLabel(productIdValue, configLabelMACOrTag, InputModel, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	save
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	save
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function saveMRPLabel() {
        beginWaitingCoverDiv();
        var productIdValue = document.getElementById("lblProductIDDisplay").value;
        WebServicePODLabelCheck.inputCustPNOnPODLabel(productIdValue, configLabelMACOrTag, "MRPCheck", line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkCustomerSNOnCOOLabel
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	判断SN On COO Label是否合法
    //| Input para.	:	1. 10位前3位等于CNU 
    //|                 2. 11位前4位等于SCNU (对于11位的sn去掉第一位S后作后续操作)
    //| Ret value	:	当不是时提示”Please scan S CPQSNo.!”
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkCustomerSNOnCOOLabel(customerSn) 
    {
        var strCustomerSN = customerSn.trim();
        
        var ret = false;

        //if (strCustomerSN.length == 10 && strCustomerSN.substr(0, 3) == "CNU")
		if (CheckCustomerSN(strCustomerSN))
        {
            custSnOnCooValue = strCustomerSN;
			if (strCustomerSN.length == 11) {
				custSnOnCooValue = strCustomerSN.substr(1, 10);
			}
            ret = true;
        }
        //else if (strCustomerSN.length == 11 && strCustomerSN.substr(0, 4) == "SCNU")
//		else if (strCustomerSN.length == 11 && CheckCustomerSN(strCustomerSN.substr(1, 3)) && strCustomerSN.substr(0, 1) == "S")
//        {
//            custSnOnCooValue = strCustomerSN.substr(1,10);
//            ret = true;
//        }    
        
        return ret;
    
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkCustomerSNOnConfigLabel
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	这里要刷入的Config Label 上的Customer S/N，要求是11位以KCNU 开头的数据，后10位为真正的Customer S/N
    //| Input para.	:	1. 11位以KCNU 开头的数的数据
    //|                 2. 11位前4位等于SCNU (对于11位的sn去掉第一位S后作后续操作)
    //| Ret value	:	当不是时提示”Please scan S CPQSNo.!”
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkCustomerSNOnConfigLabel(customerSn) {
        var strCustomerSN = customerSn.trim();

        var ret = false;

        //if (strCustomerSN.length == 11 && strCustomerSN.substr(0, 4) == "KCNU") {
		if (strCustomerSN.length == 11 && CheckCustomerSN(strCustomerSN.substr(1,10)) && strCustomerSN.substr(0, 1) == "K") {
            strCustomerSN =  strCustomerSN.substr(1, 10);
            if (custSnOnCooValue == strCustomerSN) {
                ret = true;
            }
        }
        return ret;
    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkInputModel 
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	判断刷入的IEC P/N是否与带出的Customer P/N相同
    //| Input para. :	若刷入的IEC P/N与带出的Customer P/N不同时，则提示信息”Wrong Model”，焦点置于Data Entry，等待User再次刷入IEC P/N
    //|                 若刷入的IEC P/N第10，11位等于12时，需要user再次刷13位机型码，并在Message区显示” 请再输入一遍十叁位的机型码！”
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkInputModel(inputmodel) 
    {
        var strInputModel = inputmodel.trim();
        var ret = false;    
        PN_13Length=false;

        if (strInputModel != model) {
            PN_13Length = false;  //等待User再次刷入Customer P/N :回到 checkInputModel
            alert(msgWrongModel);
            //  ShowInfo(msgWrongModel);  //ITC-1360-0825
        }

        else if (strInputModel.substr(9, 2) == "12") {
            PN_13Length = true;   //请再输入一遍十叁位的机型码 (13位机型码): 转到 checkInputModel_13
            ShowInfo(msgScanModel);
        }

        else {
            ret = true;    
            
        }  
        return ret;
    
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkInputModel_13( 13位机型码)
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	Check 13位机型码 (13位机型码)
    //| Input para  :   若刷入的机型码最后一位不等于D，或前12位与带出的Customer P/N不等时，则提示信息”Wrong Model”，焦点置于Data Entry，等待User再次刷入13位机型码
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkInputModel_13(inputmodel) 
    {
        var strInputModel = inputmodel.trim();
        var ret = false;    
        ShowInfo("");

        if ((strInputModel.length != 13) || (strInputModel.slice(-1) != "D") || (strInputModel.substr(0, 12) != model))
        {
            PN_13Length=true;  //请再输入一遍十叁位的机型码 (13位机型码): 回到 checkInputModel_13
            alert(msgWrongModel);
        }
        
        else ret = true;    //直接Save
        return ret;
    
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkInputMRPCheck
    //| Author		:	Vic
    //| Create Date	:	09/06/2013
    //| Description	:	Check MRPCheck
    //| Input para  :   
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkInputMRPCheck(inputMRP) {
        var strInputMRP = inputMRP.trim();
        var ret = false;
        ShowInfo("");
        if (MRPCheck == "MrpCode") {
            if (MrpCode == strInputMRP) {
                document.getElementById("lblMrp_PartNoDisplay").value = inputMRP;
                MRPCheck = "";
                if (POD == "") {
                    ret = true;
                }
                else {
                    ShowInfo("請Check " + POD);
                }
            }
            else {
                alert("Wrong Code，MRP  Check失败，请再次刷入...");
                ShowInfo("請Check MrpCode");
            }
        }
        else if (MRPCheck == "BoxID") {
            if (BoxID == strInputMRP) {
                document.getElementById("lblBoxidDisplay").value = inputMRP;
                MRPCheck = "";
                if (POD == "") {
                    ret = true;
                }
                else {
                    ShowInfo("請Check " + POD);
                }
            }
            else {
                alert("刷入MRP错误，请再刷...");
                ShowInfo("請刷入MRP Label");
            }
        }
        else {
            ShowInfo("請Check " + POD + "!!!");
            ret = true;
        }
        endWaitingCoverDiv();
        return ret;

    }
    //
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSNSucceed
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	第一次SN扫入成功，根据sn带出来的Product ID、Customer P/N填入UI对应控件中
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSNSucceed(result) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
        
        try {
             if (result == null) 
            {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == "CHK020") {
                ShowMessage(result[1]);
                ShowInfo(result[1]);
            }
            else if ((result.length == 7) && (result[0] == SUCCESSRET)) 
            {
                firstInput = true;
                inputObj.value = "";
                document.getElementById("lblProductIDDisplay").value = result[1];
                document.getElementById("lblCustomerSNDisplay").value = custSnOnCooValue;
                document.getElementById("lblModelDisplay").value = result[2];
                model = result[2];
                BoxID = result[6];
                //result[4] INDIA Price
                //result[5] INDIA PriceID
                if (result[3] == "ConfigLabel") {
                    POD = "ConfigLabel";
                    if (result[4] != "" && result[5] != "") {
                        MRPCheck = "BoxID";
                        ShowInfo("请刷入MRP Label");
                        //alert("请Check MRP");
                    }
                    else if (result[4] == "" && result[5] != "") {
                        MRPCheck = "MrpCode";
                        ShowInfo("請Check MrpCode");
                        //alert("请Check MRP");
                    }
                    else {
                        ShowInfo("請Check" + POD);
                    }
                }
                else if (result[3] == "PODLabel") {
                    POD = "PODLabel";
                    if (result[4] != "" && result[5] != "") {
                        MRPCheck = "BoxID";
                        ShowInfo("请刷入MRP Label");
                        //alert("请Check MRP");
                    }
                    else if (result[4] == "" && result[5] != "") {
                        MRPCheck = "MrpCode";
                        ShowInfo("請Check MrpCode");
                        //alert("请Check MRP");
                    }
                    else {
                        ShowInfo("請Check" + POD);
                    }
                }
                else {

                    if (result[4] != "" && result[5] != "") {
                        MRPCheck = "BoxID";
                        ShowInfo("请刷入MRP Label");
                        //alert("请Check MRP");
                    }
                    else if (result[4] == "" && result[5] != "") {
                        MRPCheck = "MrpCode";
                        ShowInfo("請Check MrpCode");
                        //alert("请Check MRP");
                    }
                    else {
                        ShowInfo("請Check" + POD);
                    }
                }
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
            callNextInput();
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSNFail
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	第一次SN扫入处理失败，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSNFail(error) 
    {
        endWaitingCoverDiv();
        ExitPage();
       
        firstInput = false;
        
        try {
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            inputObj.value = "";
            callNextInput();

        } catch (e) {
            alert(e.description);
        }
        
       
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	调用web service InputCustPNOnPODLabel成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) 
    {
        endWaitingCoverDiv();
    //    SetInitStatus();
        
        firstInput = false;
               
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
            }
            else if (result[0] == SUCCESSRET)
            {
                if (result[1] == "checkwrong") {
                    firstInput = true;
                    inputObj.value = "";
                    inputObj.focus();
                    getAvailableData("processDataEntry");
                    var temp = configLabelMACOrTag + " Wrong!\n" + msgConfiglabelMacWrong;
                    ShowInfo(temp);
                    ShowMessage(temp);
                    configLabelMACOrTag = "";
                    return;
                }
                
                
                //  var SuccessItem = document.getElementById("lblProductIDDisplay").value;
                var SuccessItem = "[" + custSnOnCooValue + "]"; 
                inputObj.value = "";
                ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
                
                if (result[1] == "tag") {
                    document.getElementById("lblMacDisplay").value = "";
                    document.getElementById("lblTagDisplay").value = configLabelMACOrTag;
                }
                else {
                    document.getElementById("lblMacDisplay").value = "";
                    document.getElementById("lblTagDisplay").value = configLabelMACOrTag;
                }
             //   ShowInfo(msgSuccess);                 
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
            configLabelMACOrTag = "";
            configLabelSN = "";
            
        } catch (e) {
            alert(e.description);
        }
        ResetPage();
        callNextInput();
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Chen Xu
    //| Create Date	:	10/11/2011
    //| Description	:	调用web service InputCustPNOnPODLabel失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) 
    {
        endWaitingCoverDiv();
        ExitPage();
        SetInitStatus();
        configLabelMACOrTag = "";
        configLabelSN = "";
        firstInput = false;
            
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            inputObj.value = "";
           

        } catch (e) {
            alert(e.description);
        }
        
        callNextInput();
    }

 
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callNextInput
    //| Author		:	Chen Xu
    //| Create Date	:	10/11/2011
    //| Description	:	等待快速控件继续输入
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function callNextInput() 
    {
        inputObj.focus();    

        getAvailableData("processDataEntry");
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	SetInitStatus
    //| Author		:	Chen Xu
    //| Create Date	:	10/21/2011
    //| Description	:	处理底层调用返回时，做的控件清空、取消hold界面等初始处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function SetInitStatus() 
    {
        ShowInfo("");
        
        endWaitingCoverDiv();
        
        resetAll();

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Chen Xu
    //| Create Date	:	10/11/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
//        if(document.getElementById("lblProductIDDisplay").value!="")
//        {
//          WebServicePODLabelCheck.Cancel(document.getElementById("lblProductIDDisplay").value);
//          sleep(waitTimeForClear);
        //        }

        if (document.getElementById("lblCustomerSNDisplay").value != "")
        {
            WebServicePODLabelCheck.Cancel(document.getElementById("lblCustomerSNDisplay").value);
            sleep(waitTimeForClear);
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	Chen Xu
    //| Create Date	:	10/11/2011
    //| Description	:	重置所有控件到初始状态
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        inputObj.value = "";
        document.getElementById("lblProductIDDisplay").value = "";
        document.getElementById("lblCustomerSNDisplay").value = "";
        document.getElementById("lblModelDisplay").value = "";
        document.getElementById("lblMacDisplay").value = "";
        document.getElementById("lblTagDisplay").value = "";
        document.getElementById("lblMrp_PartNoDisplay").value = "";
        document.getElementById("lblBoxidDisplay").value = "";
        model = "";
        InputModel = "";
        custSnOnCooValue = "";
        configLabelMACOrTag = "";
        configLabelSN = "";
        PN_13Length = false;    //ITC-1360-1008

        POD = "";
        MRPCheck = "";
        BoxID = "";
        inputObj.focus();   
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Chen Xu
    //| Create Date	:	10/11/2011
    //| Description	:	刷新页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ResetPage() 
    {
        ExitPage();
        resetAll();
    }


    
</script>
    
        
    <div>
         
        <center>
            
        <table width="95%" style="height:250px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
             InputRegularExpression="^[-0-9a-zA-Z\+\s\*/]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*/]"/>
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>     
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblProductID" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <input type="text" ID="lblProductIDDisplay" class="iMes_textbox_input_Disabled"
                             MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>     
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblCustomerSN" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <Input type="text" ID="lblCustomerSNDisplay" class="iMes_textbox_input_Disabled" 
                            MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            </button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>     
            <tr>
                <td align="left" >
                    <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <Input type="text" ID="lblModelDisplay" class="iMes_textbox_input_Disabled" 
                            MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr> 
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblBoxid" runat="server"  CssClass="iMes_label_13pt">BoxID:</asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <Input type="text" ID="lblBoxidDisplay" class="iMes_textbox_input_Disabled" 
                            MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblMrp_PartNo" runat="server"  CssClass="iMes_label_13pt">Mrp PartNo:</asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <Input type="text" ID="lblMrp_PartNoDisplay" class="iMes_textbox_input_Disabled" 
                            MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblMAC" runat="server"  CssClass="iMes_label_13pt">MAC:</asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <Input type="text" ID="lblMacDisplay" class="iMes_textbox_input_Disabled" 
                            MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblTag" runat="server"  CssClass="iMes_label_13pt">Asset Tag No:</asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <Input type="text" ID="lblTagDisplay" class="iMes_textbox_input_Disabled" 
                            MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            
        </table>
        
        </center>
    </div>

</asp:Content>

