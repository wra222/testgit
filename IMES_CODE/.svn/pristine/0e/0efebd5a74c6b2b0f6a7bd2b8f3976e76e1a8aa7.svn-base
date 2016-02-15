 <%--
 /*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Combine AST Page
 * UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2012/2/28 
 * UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2012/2/28           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0331, Jessica Liu, 2012-2-11
* ITC-1360-0453, Jessica Liu, 2012-2-16
* ITC-1360-0452, Jessica Liu, 2012-2-16
* ITC-1360-0818, Jessica Liu, 2012-2-28
* ITC-1360-1156, Jessica Liu, 2012-3-6
* ITC-1360-1192, Jessica Liu, 2012-3-10
* ITC-1360-1460, Jessica Liu, 2012-3-15
* ITC-1360-1516, Jessica Liu, 2012-3-20
* ITC-1360-1649, Jessica Liu, 2012-4-10
* ITC-1360-1691, Jessica Liu, 2012-4-11
* ITC-1360-1786, Jessica Liu, 2012-5-7
*/
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CombineAST.aspx.cs" Inherits="FA_CombineAST" Title="Untitled Page" %>
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
    var firstinput = false;   
    var msgDataEntryNull = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryNull").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';   
    var msgStatusNull = '<%=this.GetLocalResourceObject(Pre + "_msgStatusNull").ToString() %>';
    var msgASTLengthNull = '<%=this.GetLocalResourceObject(Pre + "_msgASTLengthNull").ToString() %>';
    //ITC-1360-0818, Jessica Liu, 2012-2-28
    var msgASTLengthDifference = '<%=this.GetLocalResourceObject(Pre + "_msgASTLengthDifference").ToString() %>';//AST长度与输入的Ast Length不符
    //2012-4-10
    var msgMN2Error = '<%=this.GetLocalResourceObject(Pre + "_msgMN2Error").ToString() %>';
    var msgNoAST = '<%=this.GetLocalResourceObject(Pre + "_msgNoAST").ToString() %>';
    //2012-4-11
    var CustSNOrProdID = "";
    //ITC-1360-1786, Jessica Liu, 2012-5-7
    var astinfo = "";
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	加载接受输入数据事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;

            //2012-4-11
            document.getElementById("<%=ShowImage.ClientID %>").src = "";            

            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);

            setPdLineCmbFocus();
        }

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	CustomerSNEnterOrTab
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
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
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputData) 
    {
        var uutInput = true;
        ShowInfo("");

        //if ((getStatusCmbValue() == "") || (getStatusCmbValue() == null)) {
        /*
        if (document.getElementById("CmbStatus").value == ""){
            alert(msgStatusNull);

            if (document.getElementById("<%=txtCustomerSN.ClientID%>").innerText != "") {
                ExitPage();
                flowFlag = false;
            }

            setPdLineCmbFocus();
            getAvailableData("processDataEntry");
        }
        else */if (getPdLineCmbValue() == "") 
        {
            alert(msgPdLineNull);

            if (document.getElementById("<%=txtCustomerSN.ClientID%>").innerText != "") {
                ExitPage();
                flowFlag = false;
            }

            setPdLineCmbFocus();
            getAvailableData("processDataEntry");
        }
        /* 2012-7-14, Jessica Liu, for mantis 
        else if (document.getElementById("<%=txtASTLength.ClientID%>").value == "")  //2011-12-26，添加AST Length判断
        {
            alert(msgASTLengthNull);

            if (document.getElementById("<%=txtCustomerSN.ClientID%>").innerText != "") {
                ExitPage();
                flowFlag = false;
            }

            //ITC-1360-0453, Jessica Liu, 2012-2-16
            //setPdLineCmbFocus();
            document.getElementById("<%=txtASTLength.ClientID%>").focus();
            
            getAvailableData("processDataEntry");
        }
        */
        else {
            if (inputData == "" || inputData == null) {
                alert(msgDataEntryNull);
                ShowInfo(msgDataEntryNull);

                inputObj.focus();
                uutInput = false;

                getAvailableData("processDataEntry");
            }

            /* UC确认不改
            //ITC-1360-0316, Jessica Liu, 2012-2-9
            if (isProdIDorCustSN(inputData, "") == false) {
                alert(msgDataEntryField);
                ShowInfo(msgDataEntryField);

                inputObj.focus();
                uutInput = false;

                getAvailableData("processDataEntry");
            }
            */

            if (uutInput) {
                if (firstinput == false) {
                    //2012-4-11
                    document.getElementById("<%=ShowImage.ClientID %>").src = "";
                    
                    getAvailableData("processDataEntry");

                    if (!flowFlag) 
                    {
                        beginWaitingCoverDiv();

                        dataEntry = inputData;  
                        if (inputData.length == 10) 
                        {
                            //if (inputData.substring(0, 2) != "CN") //prodid
							if (!CheckCustomerSN(inputData))
                            {
                                dataEntry = SubStringSN(inputData, "ProdId");
                            }
                        }

                        //2012-4-11
                        CustSNOrProdID = dataEntry;

			            //WebServiceCombineAST.blockcheck(inputData, document.getElementById("<%=txtASTLength.ClientID%>").value, line, status, station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
			            //WebServiceCombineAST.blockcheck(inputData, document.getElementById("<%=txtASTLength.ClientID%>").value, document.getElementById("<%=line.ClientID %>").value, window.CmbStatus.DropDown[window.CmbStatus.DropDown.selectedIndex].value, station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
			            //WebServiceCombineAST.blockcheck(inputData, document.getElementById("<%=txtASTLength.ClientID%>").value, document.getElementById("<%=line.ClientID %>").value, "", station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
                        //2012-7-14, Jessica Liu, for mantis
                        ////WebServiceCombineAST.blockcheck(dataEntry, document.getElementById("<%=txtASTLength.ClientID%>").value, document.getElementById("<%=line.ClientID %>").value, "", station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
                        //WebServiceCombineAST.blockcheck(dataEntry, document.getElementById("<%=txtASTLength.ClientID%>").value, document.getElementById("<%=line.ClientID %>").value, "", station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
                        WebServiceCombineAST.blockcheck(dataEntry, 0, document.getElementById("<%=line.ClientID %>").value, "", station, "<%=UserId%>", "<%=Customer%>", onBCSucceed, onBCFail);
                    }
                }
                else    
                {
                    /* 2012-7-14, Jessica Liu, for mantis
                    //ITC-1360-0818, Jessica Liu, 2012-2-28
                    if (inputData.length == parseInt(document.getElementById("<%=txtASTLength.ClientID%>").value, 10))
                    {
                        document.getElementById("<%=txtAST.ClientID %>").innerText = inputData;

                        //ITC-1360-1786, Jessica Liu, 2012-5-7
                        astinfo = inputData;

                        beginWaitingCoverDiv();

                        WebServiceCombineAST.dosave(dataEntry, inputData, station, "<%=UserId%>", "<%=Customer%>", onSucceed, onFail);
                    }
                    else
                    {
                        alert(msgASTLengthDifference);
                        
                        //ITC-1360-1156, Jessica Liu, 2012-3-6
                        callNextInput();
                    }
                    */
                    document.getElementById("<%=txtAST.ClientID %>").innerText = inputData;

                    //ITC-1360-1786, Jessica Liu, 2012-5-7
                    astinfo = inputData;

                    beginWaitingCoverDiv();

                    WebServiceCombineAST.dosave(dataEntry, inputData, station, "<%=UserId%>", "<%=Customer%>", onSucceed, onFail);
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
                /* 2012-4-28
                if (lstPrintItem == null) {
                    alert(msgPrintSettingPara);

                    WebServiceCombineAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
                    sleep(waitTimeForClear);
                    firstinput = false;
                                        
                    inputObj.value = "";        
                    inputObj.focus();
                    getAvailableData("processDataEntry");
                }
                else {
                */
                    //2012-4-12
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

                        //ITC-1360-1649, Jessica Liu, 2012-4-10
                        document.getElementById("<%=ShowImage.ClientID %>").src = imageUrl;
                    }
                    
                    if (astreturn == "") {                      
                        beginWaitingCoverDiv();

                        //ITC-1360-0331, Jessica Liu, 2012-2-11
                        WebServiceCombineAST.doprint(dataEntry, false, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSCNSucceed, onSCNFail);
                    }
                    else {
                        //2012-9-4, Jessica Liu, UC需求变更
                        //var message = "此机器已结合资产标签(" + astreturn + ")，是否删除此资产标签重新结合?";
                        var message = "此机器已结合资产标签，是否删除此资产标签重新结合?";
                        
                        if (confirm(message)) {
                            beginWaitingCoverDiv();

                            //ITC-1360-0331, Jessica Liu, 2012-2-11
                            WebServiceCombineAST.doprint(dataEntry, true, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSCNSucceed, onSCNFail);
                        }
                        else {
                            //ITC-1360-0452, Jessica Liu, 2012-2-16
                            //WebServiceCombineAST.Cancel(dataEntry);
                            WebServiceCombineAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
                            sleep(waitTimeForClear);

                            inputObj.value = "";
                            firstinput = false;
                            callNextInput();
                        }
                    }
               //} //2012-4-28
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

            //ITC-1360-1192, Jessica Liu, 2012-3-10
            //ResetPage();
            WebServiceCombineAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
            firstinput = false;
            flowFlag = false;
            
            callNextInput();
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	setPrintItemListParam
    //| Author		:	Jessica Liu
    //| Create Date	:	2/28/2012
    //| Description	:	浜х敓鎵撳嵃淇℃伅
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function setPrintItemListParam(backPrintItemList, prodid) {
        //============================================generate PrintItem List==========================================
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@productID";   
        valueCollection[0] = generateArray(prodid);

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
    //| Name		:	onBCFail
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	SN扫入失败，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
            //2012-5-2
            //else if ((result.length == 6) && (result[0] == SUCCESSRET))
            else if ((result.length == 7) && (result[0] == SUCCESSRET)) 
            {
                document.getElementById("<%=txtProdId.ClientID %>").innerText = result[2];
                document.getElementById("<%=txtCustomerSN.ClientID %>").innerText = result[3];
                document.getElementById("<%=txtModel.ClientID %>").innerText = result[4];

                if (result[5].toUpperCase() == "TRUE") 
                {
                    firstinput = false;
                    ShowInfo("");
                    
                    /* 2012-4-28
                    //ITC-1360-0818, Jessica Liu, 2012-2-28
                    setPrintItemListParam(result[1], document.getElementById("<%=txtProdId.ClientID %>").innerText);
                    
                    //Function Name: printLabels
                    //@param: printItems
                    //@param: isSerial
                    printLabels(result[1], false);
                    */
                    
                    //ITC-1360-1460, Jessica Liu, 2012-3-15
                    //resetAll();
                    reset();

                    /* 2012-5-2
                    //2012-4-11
                    //ShowSuccessfulInfo(true);
                    ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess);  
                    */
                    ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess + " " + result[6]);                   
                    
                    callNextInput();
                    
                }
                else 
                {
                    inputObj.value = "";
                    firstinput = true;
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


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSCNFail
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	DoPrint失败，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

        firstinput = false;

        try {
            if (result == null) {
                var content = msgSystemError;
                //resetAll();
                reset();
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
                //2012-5-2, 2012-5-7
                //var astinfo = document.getElementById("<%=txtAST.ClientID %>").innerText;
                
                //resetAll();
                reset();

                /* 2012-5-2
                //ITC-1360-1691, Jessica Liu, 2012-4-11
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess); 
                */
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess + " " + astinfo);   
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
    //| Name		:	onFail
    //| Author		:	Jessica Liu
    //| Create Date	:	12/2/2011
    //| Description	:	调用web service save失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        firstinput = false;

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
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="5">
                    
                    <select name="CmbStatus" style="Width:99%">
                        <option value="1">有效资产标签</option>
                    </select>
                </td>
            </tr>
            
            <tr><td>&nbsp;</td><td></td></tr>
            
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
                <%--2012-4-28
                <td align="left" colspan="4">
                --%>
                <td align="left" colspan="5">
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
                <%--2012-4-28   
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
                --%>
            </tr>
            
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td colspan="6">
                    <%-- 2012-7-14, Jessica Liu, for mantis --%>
                    <asp:Image ID="ShowImage" runat="server" Width="550" Height="300"/>
                </td>
            </tr>
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                            </button>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <%-- 2011-12-26
                            <input id="status" type="hidden" runat="server" />
                            --%>
                            <input id="line" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            <%-- 2012-7-14, Jessica Liu, for mantis --%>
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
