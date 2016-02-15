<%--
 /*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Online Generate AST Page
 * UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/3/6 
 * UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/3/6            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0088,ITC-1360-0089  Jessica Liu, 2012-1-19
* ITC-1360-0091  Jessica Liu, 2012-1-21
* ITC-1360-0090  Jessica Liu, 2012-1-21
* ITC-1360-1017, Jessica Liu, 2012-3-2
* ITC-1360-1161, Jessica Liu, 2012-3-6
* ITC-1360-1457, Jessica Liu, 2012-3-15
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="OnlineGenerateAST.aspx.cs" Inherits="FA_OnlineGenerateAST" Title="Untitled Page" %>
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
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';
    var msgInputProIDOrCustsn = '<%=this.GetLocalResourceObject(Pre + "_msgInputProIDOrCustsn").ToString() %>';
    //2012-4-9
    var msgGoToCombineAST = '<%=this.GetLocalResourceObject(Pre + "_msgGoToCombineAST").ToString() %>';
    //2012-4-10
    var msgMN2Error = '<%=this.GetLocalResourceObject(Pre + "_msgMN2Error").ToString() %>';
    var msgNoAST = '<%=this.GetLocalResourceObject(Pre + "_msgNoAST").ToString() %>';
    //2012-4-11
    var CustSNOrProdID = "";
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
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
    //| Create Date	:	11/22/2011
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
    //| Create Date	:	11/22/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputData) 
    {
        var uutInput = true;
        ShowInfo("");

        //2012-4-11
        document.getElementById("<%=ShowImage.ClientID %>").src = "";

        if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
            alert(msgPdLineNull);

            if (document.getElementById("<%=txtCPQSNO.ClientID%>").innerText != "") {
                ExitPage();
                flowFlag = false;
            }

            setPdLineCmbFocus();
            getAvailableData("processDataEntry");
        }
        else {
            if (inputData == "" || inputData == null) {
                alert(msgNoInputCustomerSn);
                ShowInfo(msgNoInputCustomerSn);

                inputObj.focus();
                uutInput = false;

                getAvailableData("processDataEntry");
            }

            if (isProdIDorCustSN(inputData, "") == false) {
                alert(msgDataEntryField);
                ShowInfo(msgDataEntryField);

                inputObj.focus();
                uutInput = false;

                getAvailableData("processDataEntry");
            }

            if (uutInput) {
                getAvailableData("processDataEntry");

                if (!flowFlag) 
                {
                    beginWaitingCoverDiv();

                    //ITC-1360-0090  Jessica Liu, 2012-1-21
                    dataEntry = inputData;    //SubStringSN(inputData, "MB");
                    if (dataEntry.length == 10) {
                        if (dataEntry.substring(0, 2) != "CN") //prodid
                        {
                            dataEntry = SubStringSN(dataEntry, "ProdId");
                        }
                    }

                    //2012-4-11
                    CustSNOrProdID = dataEntry;
                    
                    WebServiceOnlineGenerateAST.CheckCustomerSN(dataEntry, "", station, "<%=UserId%>", "<%=Customer%>", onSCNSucceed, onSCNFail);
                    
                }
            }
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
            else if ((result.length == 7) && (result[0] == SUCCESSRET)) //2012-4-10
            {
                document.getElementById("lblProductIDDisplay").value = result[1];
                //ITC-1360-0091, Jessica Liu, 2012-1-21
                document.getElementById("<%=txtCPQSNO.ClientID%>").innerText = result[2];
                document.getElementById("<%=txtModel.ClientID %>").innerText = result[3];

                //2012-4-9
                if (result[4] == "TRUE") 
                {
                    ShowInfo("");
                    ShowInfo(msgGoToCombineAST);
                }
                    
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) 
                {
                    alert(msgPrintSettingPara);

                    inputObj.value = "";
                    document.getElementById("lblProductIDDisplay").value = "";
                    document.getElementById("<%=txtCPQSNO.ClientID %>").innerText = "";
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
                        document.getElementById("<%=ShowImage.ClientID %>").src = imageUrl;
                    }           

                    //调用web service提供的打印接口????  
                    beginWaitingCoverDiv();
                    
                    //WebServiceOnlineGenerateAST.print(inputObj.value, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
                    WebServiceOnlineGenerateAST.print(dataEntry, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
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
    //| Create Date	:	11/22/2011
    //| Description	:	SN扫入失败，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSCNFail(error) 
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
    function onSucceed(result) {
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
                setPrintItemListParam(result[1], document.getElementById("lblProductIDDisplay").value); //document.getElementById("<%=txtCPQSNO.ClientID%>").innerText);//,result[2]);
               
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
    function onFail(error) {
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
    //| Name		:	reprint
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	点击reprint按钮的处理函数
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function reprint() 
    {
        var str = prompt(msgInputProIDOrCustsn, "");
        
        if (str != null) 
        {
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);

                inputObj.value = "";
                document.getElementById("lblProductIDDisplay").value = "";
                document.getElementById("<%=txtCPQSNO.ClientID %>").innerText = "";
                document.getElementById("<%=txtModel.ClientID %>").innerText = "";

                inputObj.focus();
            }
            else {

                //reset();

                beginWaitingCoverDiv();

                //调用service目录里对应asmx文件处理里的函数？？？？修改，去后台获得信息
                WebServiceOnlineGenerateAST.reprint(str, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", "", lstPrintItem, onRPSucceed, onRPFail);
            }
            
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onRPSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	调用web service reprint成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onRPSucceed(result) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
        
        try {
            if (result == null) 
            {
                var content = msgSystemError;

                //resetAll();
                reset();
                
                ShowMessage(content);
                ShowInfo(content);
                
                
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) 
            {
                setPrintItemListParam(result[1], result[2]); //document.getElementById("<%=txtCPQSNO.ClientID%>").innerText);//, result[2]);
                
                /*
                * Function Name: printLabels
                * @param: printItems
                * @param: isSerial
                */
                printLabels(result[1], false);

                //ITC-1360-1017, Jessica Liu, 2012-3-2
                //resetAll();

                //2012-4-11  
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess); 
            }
            else 
            {
                ShowInfo("");

                //resetAll();
                reset();
                
                var content1 = result[0];
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
    //| Name		:	onRPFail
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	调用web service reprint失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onRPFail(error) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
    
        try {
            ShowMessage(error.get_message());

            /* ITC-1360-1161, Jessica Liu, 2012-3-6
            //resetAll(); 
            reset();
            //ResetPage();
            */
            
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
    //| Create Date	:	11/22/2011
    //| Description	:	清空输入控件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function reset() 
    {
        getPdLineCmbObj().selectedIndex = 0;
        document.getElementById("lblProductIDDisplay").value = "";
        document.getElementById("<%=txtCPQSNO.ClientID %>").innerText = "";
        document.getElementById("<%=txtModel.ClientID %>").innerText = "";
        inputObj.value = "";
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callNextInput
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	等待快速控件继续输入
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function callNextInput() 
    {
        getCommonInputObject().value = "";
        getCommonInputObject().focus();    

        getAvailableData("processDataEntry");
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	printSetting
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	显示打印设置对话框
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function showPrintSettingDialog() 
    {
        showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onbeforeunload
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
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
    //| Create Date	:	11/22/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        if (document.getElementById("<%=txtCPQSNO.ClientID%>").innerText != "") 
        {
            WebServiceOnlineGenerateAST.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
            flowFlag = false;
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onClearSucceeded
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
    //| Description	:	Cancel WF成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onClearSucceeded(result) 
    {
        /*
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
    //| Create Date	:	11/22/2011
    //| Description	:	Cancel WF失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onClearFailed(error) 
    {
        /*
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
    //| Create Date	:	11/22/2011
    //| Description	:	
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        ShowInfo("");

        document.getElementById("<%=btnHidden.ClientID%>").click(); 
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Jessica Liu
    //| Create Date	:	11/22/2011
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
    
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceOnlineGenerateAST.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>                
                <td>
                    <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="3">
                    <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="FA"/>
                    <%--
                    <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                    </Triggers>
                    <ContentTemplate>                    
                    <asp:Label ID="lblPdLineName" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    --%>
                </td>
            </tr>     
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td align="left" >
                    <asp:Label ID="lblProductID" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="3">
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
                    <td style="width: 150px;">
                       <asp:Label ID="lblCPQSNO" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
                    </td>
                    <td style="width: 35%;">
                        <asp:Label ID="txtCPQSNO" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td style="width: 140px;">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                    
                    <td>
                        <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>  
                </tr>         

            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
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
            
                <%--
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
                <%--
                <td align="left">
                    <input id="btnReprint" type="button"  runat="server" onclick="reprint()" 
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                        style="width:100px"  class=" iMes_button" />
                </td>
                --%>
            </tr>
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td colspan="4">
                    <asp:Image ID="ShowImage" runat="server" Width="550" Height="400"/>
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
                            <input id="line" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            </button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        
        </table>
        
        </center>
    </div>
    
</asp:Content>
