 <%--
/*
*/
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CombineAndGenerateAST.aspx.cs" Inherits="FA_CombineAndGenerateAST" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgNoProductID = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoProductID") %>';
    var msgNoPrinted = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrinted") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var assetSN = "";
    var flowFlag = false;     
    var dataEntry = "";       
    var flag = false;
    var firstinput = '0Begin'; // 0Begin 1CheckNeedATSN9 2SaveWhenMatchATSN9
    var msgDataEntryNull = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryNull").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';   
    var msgStatusNull = '<%=this.GetLocalResourceObject(Pre + "_msgStatusNull").ToString() %>';
    var msgASTLengthNull = '<%=this.GetLocalResourceObject(Pre + "_msgASTLengthNull").ToString() %>';
    var msgASTLengthDifference = '<%=this.GetLocalResourceObject(Pre + "_msgASTLengthDifference").ToString() %>';//AST长度与输入的Ast Length不符
    var msgMN2Error = '<%=this.GetLocalResourceObject(Pre + "_msgMN2Error").ToString() %>';
    var msgNoAST = '<%=this.GetLocalResourceObject(Pre + "_msgNoAST").ToString() %>';
    
    var CustSNOrProdID = "";
    var astinfo = "";
	var stationGenerateAST = 'AT';
	var partSnATSN9 = '';
	var promptATSN9 = '';
    
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;

            document.getElementById("<%=ShowImage.ClientID %>").src = "";
			document.getElementById("<%=ShowImageGenAst.ClientID %>").src = "";

            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);

            setPdLineCmbFocus();
        }

    }

    function CustomerSNEnterOrTab() 
    {
        if (event.keyCode == 9 || event.keyCode == 13) 
        {
            getAvailableData("processDataEntry");
        }
    }

    function processDataEntry(inputData) 
    {
        var uutInput = true;
        ShowInfo("");

        if (getPrintItemCollection() == null) {
            alert(msgPrintSettingPara);
            inputObj.value = "";
            inputObj.focus();
            getAvailableData("processDataEntry");
            return;
        }
        if (getPdLineCmbValue() == "") 
        {
            alert(msgPdLineNull);

            if (document.getElementById("<%=txtCustomerSN.ClientID%>").innerText != "") {
                ExitPage();
                flowFlag = false;
            }

            setPdLineCmbFocus();
            getAvailableData("processDataEntry");
        }
        else {
            if (inputData == "" || inputData == null) {
                alert(msgDataEntryNull);
                ShowInfo(msgDataEntryNull);

                inputObj.focus();
                uutInput = false;

                getAvailableData("processDataEntry");
            }


            if (uutInput) {
                if (firstinput == '0Begin') {
                    
                    document.getElementById("<%=ShowImage.ClientID %>").src = "";
					document.getElementById("<%=ShowImageGenAst.ClientID %>").src = "";
                    
                    getAvailableData("processDataEntry");

                    if (!flowFlag) 
                    {
                        beginWaitingCoverDiv();

                        dataEntry = inputData;  
                        if (inputData.length == 10) 
                        {
							if (!CheckCustomerSN(inputData))
                            {
                                dataEntry = SubStringSN(inputData, "ProdId");
                            }
                        }

                        CustSNOrProdID = dataEntry;

                        WebServiceCombineAST.blockcheck_CombineAndGenerateAST(dataEntry, 0, document.getElementById("<%=line.ClientID %>").value, "", station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
//WebServiceOnlineGenerateAST.CheckCustomerSN_CombineAndGenerateAST(CustSNOrProdID, "", stationGenerateAST, "<%=UserId%>", "<%=Customer%>", OnlineGenerateASTonSCNSucceed, OnlineGenerateASTonSCNFail);
                    }
                }
                else if (firstinput == '1CheckNeedATSN9')
                {
                    document.getElementById("<%=txtAST.ClientID %>").innerText = inputData;
                    astinfo = inputData;
                    beginWaitingCoverDiv();
                    WebServiceCombineAST.dosaveBeforeCheckATSN9(dataEntry, inputData, station, "<%=UserId%>", "<%=Customer%>", onSucceedBeforeCheckATSN9, onFail);
                }
				else if (firstinput == '2SaveWhenMatchATSN9')
				{
					if (partSnATSN9 == inputData){
						WebServiceCombineAST.dosaveAfterCheckATSN9(dataEntry, station, "<%=UserId%>", "<%=Customer%>", onSucceed, onFail);
					}
					else {
						ShowInfo('刷入 '+promptATSN9+' 错误，请重新刷入!'); //胶水条码
						inputObj.value = "";
						inputObj.focus();
						getAvailableData("processDataEntry");
					}
				}
            }
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSCNSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	Block成功，根据返回的标志量进一步弹出对话框判断或者继续调用后续处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onBCSucceed(result) {
        try {
            endWaitingCoverDiv();

            if (result == null) {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);

                inputObj.value = "";
                inputObj.focus();
            }
            else if ((result.length == 4) && (result[0] == SUCCESSRET)) {
                var astreturn = result[1];

                var lstPrintItem = getPrintItemCollection();
                    if (result[2] == "1") {
                        ShowInfo("");
                        ShowInfo(msgMN2Error);
                    }
                    else if (result[2] == "2") {
                        ShowInfo("");
                        ShowInfo(msgNoAST);
                    }
                    else if (result[2] == "0") {
                        var imageUrl = "";
                        var RDSServer = '<%=ConfigurationManager.AppSettings["RDS_Server"].Replace("\\", "\\\\")%>';

                        imageUrl = RDSServer + result[3] + ".JPG";

                        document.getElementById("<%=ShowImage.ClientID %>").src = imageUrl;
						ShowInfo("<%=msgInputAST%>");
                    }
					else{ // WantFinishFlow_when_CombineAndGenerateAST
						var resultCode = result[2];
						if('PP'==resultCode) ShowInfo('需要打印PP类标签');
						else if('ATSN3'==resultCode) ShowInfo('需要打印ATSN3标签');
						else if('ATSN5'==resultCode) ShowInfo('需要打印ATSN5标签');
						
						beginWaitingCoverDiv();
						WebServiceOnlineGenerateAST.CheckCustomerSN_CombineAndGenerateAST(CustSNOrProdID, "", stationGenerateAST, "<%=UserId%>", "<%=Customer%>", OnlineGenerateASTonSCNSucceed, OnlineGenerateASTonSCNFail);
						callNextInput();
						return;
					}
                    
                    if (astreturn == "") {                      
                        beginWaitingCoverDiv();

                        WebServiceCombineAST.doprint(dataEntry, false, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSCNSucceed, onSCNFail);
                    }
                    else {
                        var message = "此机器已结合资产标签，是否删除此资产标签重新结合?";
                        
                        if (confirm(message)) {
                            beginWaitingCoverDiv();

                            WebServiceCombineAST.doprint(dataEntry, true, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSCNSucceed, onSCNFail);
                        }
                        else {
                            WebServiceCombineAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
                            sleep(waitTimeForClear);

                            inputObj.value = "";
                            firstinput = '0Begin';
                            callNextInput();
                        }
                    }
            }
            else {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);

                inputObj.value = "";
                inputObj.focus();
            }
        } catch (e) {
            alert(e);

            //ResetPage();
            WebServiceCombineAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
            firstinput = '0Begin';
            flowFlag = false;
            
            callNextInput();
        }
    }


    function onBCFail(error) {
        try {
            //resetAll();
            reset();
            endWaitingCoverDiv();

            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();

            inputObj.focus();
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSCNSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	DoPrint成功，获取到Product ID与Customer SN及Model
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSCNSucceed(result) 
    {
        try {
            endWaitingCoverDiv();

            if (result == null) 
            {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);

                inputObj.value = "";
                inputObj.focus();
            }
            else if ((result.length == 7) && (result[0] == SUCCESSRET)) 
            {
                document.getElementById("<%=txtProdId.ClientID %>").innerText = result[2];
                document.getElementById("<%=txtCustomerSN.ClientID %>").innerText = result[3];
                document.getElementById("<%=txtModel.ClientID %>").innerText = result[4];

                if (result[5].toUpperCase() == "TRUE") 
                {
					firstinput = '0Begin';
                    ShowInfo("");
                    reset();
                    ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess + " " + result[6]);                   
                    callNextInput();
                }
                else 
                {
                    inputObj.value = "";
                    firstinput = '1CheckNeedATSN9';
                    callNextInput();
                }
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);

                inputObj.value = "";
                inputObj.focus();
            }
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }


    function onSCNFail(error) 
    {
        try {
            //resetAll();
            reset();
            endWaitingCoverDiv();

            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();

            inputObj.focus();
        }

    }


    function onSucceedBeforeCheckATSN9(result) {
		ShowInfo("");
        endWaitingCoverDiv();
		try {
			if (result == null) {
                var content = msgSystemError;
                //resetAll();
                reset();
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
				if (result[1] == null || result[1] == ''){
					beginWaitingCoverDiv();
					WebServiceCombineAST.dosaveAfterCheckATSN9(dataEntry, station, "<%=UserId%>", "<%=Customer%>", onSucceed, onFail);
				}
				else{
					partSnATSN9 = result[1];
					firstinput = '2SaveWhenMatchATSN9';
					promptATSN9 = result[2];
					ShowInfo('请刷入 '+promptATSN9+' 条码');
				}
			}
            else {
                ShowInfo("");
                var content1 = result[0];
                //resetAll();
                reset();
                ShowMessage(content1);
                ShowInfo(content1);
            }

			callNextInput();
		} catch (e) {
            alert(e.description);
            inputObj.focus();
        }
	}
	
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	调用web service save成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();

        firstinput = '0Begin';

        try {
            if (result == null) {
                var content = msgSystemError;
                //resetAll();
                reset();
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
                /*
                //var astinfo = document.getElementById("<%=txtAST.ClientID %>").innerText;
                reset();
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess + " " + astinfo);
				*/
				
				beginWaitingCoverDiv();
				WebServiceOnlineGenerateAST.CheckCustomerSN_CombineAndGenerateAST(CustSNOrProdID, document.getElementById("<%=line.ClientID %>").value, stationGenerateAST, "<%=UserId%>", "<%=Customer%>", OnlineGenerateASTonSCNSucceed, OnlineGenerateASTonSCNFail);
            }
            else {
                ShowInfo("");
                var content1 = result[0];
                //resetAll();
                reset();
                ShowMessage(content1);
                ShowInfo(content1);
            }

            callNextInput();
        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }

    function onFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        firstinput = '0Begin';

        try {
            //resetAll(); //reset();
            reset();
            
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            
            callNextInput();

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }
    }
	
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSCNSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	SN扫入成功，获取到Product ID与Customer SN????
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function OnlineGenerateASTonSCNSucceed(result) 
    {
        try {
            endWaitingCoverDiv();

            if (result == null) 
            {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);

                inputObj.value = "";
                inputObj.focus();
            }
            else if ((result.length == 7) && (result[0] == SUCCESSRET)) //2012-4-10
            {
                ///document.getElementById("lblProductIDDisplay").value = result[1];
                document.getElementById("<%=txtProdId.ClientID %>").innerText = result[1];
                //ITC-1360-0091, Jessica Liu, 2012-1-21
                ///document.getElementById("txtCPQSNO).innerText = result[2];
                document.getElementById("<%=txtCustomerSN.ClientID %>").innerText = result[2];
                document.getElementById("<%=txtModel.ClientID %>").innerText = result[3];

                //2012-4-9
                if (result[4] == "TRUE") 
                {
                    ShowInfo("");
                    //ShowInfo(msgGoToCombineAST);
                }
                    
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) 
                {
                    alert(msgPrintSettingPara);

                    inputObj.value = "";
                    document.getElementById("<%=txtProdId.ClientID %>").innerText = "";
                    document.getElementById("<%=txtCustomerSN.ClientID %>").innerText = "";
                    document.getElementById("<%=txtModel.ClientID %>").innerText = "";
                    
                    inputObj.focus(); 
                }
                else {
                    //2012-4-12
                    if (result[5] == "1") {
                        ShowInfo("");
                        ShowInfo(msgMN2Error);
                    }
                    else if (result[5] == "2") {
                        ShowInfo("");
                        ShowInfo(msgNoAST);
                    }
                    else if (result[5] == "0") {
                        var imageUrl = "";
                        var RDSServer = '<%=ConfigurationManager.AppSettings["RDS_Server"].Replace("\\", "\\\\")%>';

                        imageUrl = RDSServer + result[6] + ".JPG";

                        //ITC-1360-1619, Jessica Liu, 2012-4-6
                        //ShowImage.ImageUrl = imageUrl;
                        document.getElementById("<%=ShowImageGenAst.ClientID %>").src = imageUrl;
                    }
					else if (result[5] == "WantFinishFlow"){
						ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess);
						callNextInput();
						return;
					}

                    //调用web service提供的打印接口????  
                    beginWaitingCoverDiv();
                    
                    //WebServiceOnlineGenerateAST.print(inputObj.value, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
                    WebServiceOnlineGenerateAST.print(dataEntry, document.getElementById("<%=line.ClientID %>").value, stationGenerateAST, "<%=UserId%>", "<%=Customer%>", lstPrintItem, OnlineGenerateASTonSucceed, OnlineGenerateASTonFail);
                }
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);

                inputObj.value = "";
                inputObj.focus();
            }
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }
	
	function OnlineGenerateASTonSCNFail(error) 
    {
        try {           
            endWaitingCoverDiv();

            /* ITC-1360-1161, Jessica Liu, 2012-3-6
            //ITC-1360-0088,ITC-1360-0089  Jessica Liu, 2012-1-19
            //reset();
            resetAll();
            //ResetPage();
            */

            //ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();

            inputObj.focus();
        }

    }
	
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	调用web service print成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function OnlineGenerateASTonSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);

                //resetAll();
                reset();
                //ResetPage();

                ShowInfo(content);
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
                
                //ITC-1360-1457, Jessica Liu, 2012-3-15
            ///setPrintItemListParam(result[1], document.getElementById("lblProductIDDisplay").value); //txtCPQSNO.innerText);//,result[2]);
            setPrintItemListParam(result[1], document.getElementById("<%=txtProdId.ClientID %>").innerText); //txtCPQSNO.innerText);//,result[2]);
               
                /*
                * Function Name: printLabels
                * @param: printItems
                * @param: isSerial
                */
                printLabels(result[1], false);
                
                //ITC-1360-1017, Jessica Liu, 2012-3-2
                //resetAll();
                //ResetPage();

                /* 2012-5-2
                //2012-4-11  
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess); 
                */
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess + " " + result[2]); 
            }
            else {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);

                //resetAll();
                reset();
                //ResetPage();

                ShowInfo(content1);
            }

            callNextInput();
        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	调用web service print失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function OnlineGenerateASTonFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            /* ITC-1360-1161, Jessica Liu, 2012-3-6
            //resetAll(); 
            reset();
            //ResetPage();
            */

            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            
            callNextInput();

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	setPrintItemListParam
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	产生打印信息????
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function setPrintItemListParam(backPrintItemList, prodid)//, ast) 
    {
        //============================================generate PrintItem List==========================================
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@productID";   //
        valueCollection[0] = generateArray(prodid);
        //keyCollection[1] = "@Ast";   //
        //valueCollection[1] = generateArray(ast);

        /*
        * Function Name: setPrintParam
        * @param: printItemCollection
        * @param: labelType
        * @param: keyCollection(Client: Array of string.    Server: List<string>)
        * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        */
        setPrintParam(lstPrtItem, "Asset Label", keyCollection, valueCollection);
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	reset
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	清空输入控件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function reset() 
    {
        //ITC-1360-1516, Jessica Liu, 2012-3-20
        //getPdLineCmbObj().selectedIndex = 0;
        
        document.getElementById("<%=txtProdId.ClientID %>").innerText = "";
        document.getElementById("<%=txtCustomerSN.ClientID %>").innerText = "";
        document.getElementById("<%=txtModel.ClientID %>").innerText = "";
        document.getElementById("<%=txtAST.ClientID %>").value = "";
        //ITC-1360-1516, Jessica Liu, 2012-3-20
        //document.getElementById("<%=txtASTLength.ClientID%>").value = "";
        
        inputObj.value = "";
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callNextInput
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	等待快速控件继续输入
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function callNextInput() 
    {
        //ITC-1360-1156, Jessica Liu, 2012-3-6
        getCommonInputObject().value = "";
        
        //ITC-1360-0317, Jessics Liu, 2012-2-11
        //inputObj.focus();
        getCommonInputObject().focus();

        getAvailableData("processDataEntry");
    }



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onbeforeunload
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	onbeforeunload时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    window.onbeforeunload = function() 
    {
        ExitPage();
    } 
  
  
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        if (document.getElementById("<%=txtCustomerSN.ClientID%>").innerText != "") 
        {
            WebServiceCombineAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
            flowFlag = false;
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onClearSucceeded
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	Cancel WF成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onClearSucceeded(result) 
    {
        /* ITC-1360-0452, Jessica Liu, 2012-3-7
        ShowInfo("");
        
        try {
        if (result == null) 
        {
            ShowMessage(msgSystemError);
            ShowInfo(msgSystemError);
        }
        else if (result == SUCCESSRET) {
        }
        else {
            ShowMessage(result);
            ShowInfo(result);
        }
        }
        catch (e) {
            alert(e.description);
        }

        reset();        
        */
     }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onClearFailed
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	Cancel WF失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onClearFailed(error) 
    {
        /* ITC-1360-0452, Jessica Liu, 2012-3-7
        ShowInfo("");
        try {
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        }
        catch (e) {
        alert(e.description);
        }

        reset();
        */
    } 


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        ShowInfo("");

        document.getElementById("<%=hiddenbtn.ClientID%>").click();
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	刷新页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ResetPage() 
    {
        ExitPage();
        resetAll();
        reset();
        flowFlag = false;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	printSetting
    //| Author		:	Jessica Liu
    //| Create Date	:	2/28/2012
    //| Description	:	鏄剧ず鎵撳嵃璁剧疆瀵硅瘽妗?
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function showPrintSettingDialog() {
        showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
    }

    
</script>  

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceCombineAST.asmx" />
				<asp:ServiceReference Path="Service/WebServiceOnlineGenerateAST.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <!--tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="5">
                    
                    <select name="CmbStatus" style="Width:99%">
                        <option value="1">有效资产标签</option>
                    </select>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr-->
            
            <tr>
                <td>
                    <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="5">
                    <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="FA"/>
                </td>
            </tr>     
            
            <tr><td>&nbsp;</td><td></td></tr>
                        
            <tr>
                    <td style="width: 150px;">
                       <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
                    </td>
                    <td>
                        <asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td style="width: 140px;">
                        <asp:Label ID="lblCustomerSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                    
                    <td>
                        <asp:Label ID="txtCustomerSN" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>  
                    <td style="width: 140px;">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                    
                    <td>
                        <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td> 
                </tr>      

                <tr><td>&nbsp;</td><td></td></tr>
                
                <tr>
                <td align="left" >
                    <asp:Label ID="lblAST" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="5">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <input type="text" ID="txtAST" class="iMes_textbox_input_Yellow" runat="server"
                             MaxLength="20" style="width:98%" readonly="readonly"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>   

            <tr><td>&nbsp;</td><td></td></tr>
                        
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" colspan="4">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50" CssClass="iMes_textbox_input_Yellow" 
             InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>   
                <td align="right">   
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                        
                            <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
                        </ContentTemplate>  
                    </asp:UpdatePanel>
                </td> 
            </tr>
            
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td colspan="3">
                    <asp:Image ID="ShowImage" runat="server" Width="550" Height="300"/>
                </td>
				<td colspan="3">
					<asp:Image ID="ShowImageGenAst" runat="server" Width="550" Height="300"/>
                </td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                            </button>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            
                            <input id="line" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            
                            <asp:Label ID="lblASTLength" runat="server"  CssClass="iMes_label_13pt" type="hidden"></asp:Label>
                            <input type="hidden" id="txtASTLength" runat="server" class="iMes_textbox_input_Normal"  maxlength="20" style="width:98%" onkeyup="value=value.replace(/[^\d]/g,'') "/>
                            </button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        
        </table>
        
        </center>
    </div>
    
</asp:Content>
