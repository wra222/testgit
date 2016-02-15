<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 

*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="SNCheck_CQ.aspx.cs" Inherits="PAK_SNCheck_CQ" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    //ITC-1360-0542, Jessica Liu, 2012-2-18
    var msgScanAnotherSN = '<%=this.GetLocalResourceObject(Pre + "_msgScanAnotherSN").ToString()%>'; // '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgScanAnotherSN") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var pCode = "";
    var custsnOnProductValue = "";
    var firstInput = false;
    var flag = false;
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var SessionStartFlag = false;
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var IsChinaEnergyLabel = false;
    var isNeedPrint = false;
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
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
            station = document.getElementById("<%=hiddenStation.ClientID %>").value;
            pCode = document.getElementById("<%=pCode.ClientID %>").value;
            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);
            inputObj.focus();
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
        if (inputData == "") 
        {
            alert(msgNoInputCustomerSn);
            inputObj.focus();
            uutInput = false;
            getAvailableData("processDataEntry");
        }
        var lstPrintItem = getPrintItemCollection();
        if (lstPrintItem == null) {
            alert(msgPrintSettingPara);
            inputObj.focus();
            getAvailableData("processDataEntry");
            return;
        }
        if (uutInput) 
        {
            if (firstInput == false)    
            {
                getAvailableData("processDataEntry");
                if (CheckCustomerSNForFirstScan(inputData) == true)  //firstScan must not A S P sn 
                {
                    alert(msgDataEntryField);
                    inputObj.value = "";      
                    inputObj.focus();
                    getAvailableData("processDataEntry");
                }
                else 
                {
                    if (inputData.length == 11) {
                        inputData = inputData.substr(1, 10);
                    }
                    if (inputData.length > 11) {
                        inputData = inputData.substr(0, 10);
                    }// FOR Table SN 
                        
                    beginWaitingCoverDiv();
                    var line = "";
                    IsChinaEnergyLabel = false;
                    custsnOnProductValue = inputData;
                    SessionStartFlag = true;
                    isNeedPrint = document.getElementById("isPrintEnergy").checked;
                    WebServiceSNCheck.InputCustsnForCQ(inputData, line, "<%=UserId%>", station, "<%=Customer%>", isNeedPrint, onFSNSucceed, onFSNFail);
                }
                
            }
            else {
      
           
      
                if (checkCustomerSNOnPizzaValid(inputData) == false) 
                {
                    alert(msgDataEntryField);
                    inputObj.value = "";
                    inputObj.focus();   
                     getAvailableData("processDataEntry");
                }
                else 
                {
                    beginWaitingCoverDiv();
                    var line1 = ""; 
                    var custsnOnPizza = inputObj.value;
                    var custsnOnProduct = document.getElementById("lblCustomerSNDisplay").value;
                    //2012-2-21
                    //WebServiceSNCheck.inputCustSNOnPizzaReturn(custsnOnPizza, custsnOnProduct, line1, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
                    WebServiceSNCheck.checkTwoSNIdentical(custsnOnPizza, custsnOnProduct, line1, "<%=UserId%>", station, "<%=Customer%>", onCheckSucceed, onCheckFail);
                }
                
            }        
            
        }
       
    }
    function CheckCustomerSNForFirstScan(sn) {
        var regxCQPizza = /^[SAP]{1}5CG[0-9]{3}[A-Z0-9]{4}$/;
        return regxCQPizza.test(sn);
    }
    
    function checkCustomerSNOnPizzaValid(strCustomerSN) 
    {

        return CheckCustomerSNinPizzaForCQ(strCustomerSN);
    
    }


    function onFSNSucceed(result)  //第一次SN扫入成功，获取到Product ID
    {
        SetInitStatus();
        
        try {
             if (result == null) 
            {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) 
            {
                ShowInfo("");
                firstInput = true;
                document.getElementById("lblProductIDDisplay").value = result[1];
                document.getElementById("lblCustomerSNDisplay").value = custsnOnProductValue;
                ShowInfo(msgScanAnotherSN); 
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFSNFail
    //| Author		:	Jessica Liu
    //| Create Date	:	10/21/2011
    //| Description	:	第一次SN扫入处理失败，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFSNFail(error) 
    {
        SetInitStatus();
        firstInput = false;
        SessionStartFlag = false;
        
        try {
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onCheckSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	2/21/2012
    //| Description	:	调用web service checkTwoSNIdentical成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onCheckSucceed(result) {
        ShowInfo("");
                
        try {

            if (result == null) {
                endWaitingCoverDiv();
                
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
                var line1 = "";

                var custsnOnPizza = inputObj.value;
                var custsnOnProduct = document.getElementById("lblCustomerSNDisplay").value;
                var lstPrintItem = getPrintItemCollection();
                WebServiceSNCheck.InputCustsnOnPizzaForCQ(custsnOnPizza, custsnOnProduct, line1, "<%=UserId%>", station, "<%=Customer%>",lstPrintItem, onSucceed, onFail);
            }
            else {
                ShowInfo("");
                endWaitingCoverDiv();
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }

        } catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onCheckFail
    //| Author		:	Jessica Liu
    //| Create Date	:	2/21/2012
    //| Description	:	调用web service checkTwoSNIdentical失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onCheckFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        //ITC-1360-1514,Jessica Liu, 2012-3-20
        //SessionStartFlag = false;

        try {
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
    //| Author		:	Jessica Liu
    //| Create Date	:	10/21/2011
    //| Description	:	调用web service inputCustSNOnPizza成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) 
    {
        SetInitStatus();
        firstInput = false;
        SessionStartFlag = false;
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
            }
            else if ((result.length == 4) && (result[0] == SUCCESSRET)) 
            {


                if (result[2] != "" && isNeedPrint) {
                    setPrintItemListParam(result[3], custsnOnProductValue);
                    printLabels(result[3], true);
                }
                var successInfo = "";
                successInfo += "[" + custsnOnProductValue + "] " + msgSuccess + "  " + result[1];
                ShowSuccessfulInfo(true, successInfo);
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
            alert(e.description);
        }

    }
    function setPrintItemListParam(backPrintItemList,sn) {
        //============================================generate PrintItem List==========================================
        //var lstPrtItem = backPrintItemList;
        var lstPrtItem = new Array();
        lstPrtItem[0] = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(sn);

        setPrintParam(backPrintItemList, "EnergyLabel", keyCollection, valueCollection);
    }
    function onFail(error) 
    {
        SetInitStatus();
        firstInput = false;
        SessionStartFlag = false;
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
        }
    }
 
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callNextInput
    //| Author		:	Jessica Liu
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

    function SetInitStatus() 
    {
        ShowInfo("");
        endWaitingCoverDiv();
        resetAll();

    }

    window.onbeforeunload = function() {
        ExitPage();
    } 

    function ExitPage() 
    {
        //ITC-1360-0824, Jessica Liu, 2012-2-28
        if (SessionStartFlag == true) {
            WebServiceSNCheck.Cancel(document.getElementById("lblCustomerSNDisplay").value, station);
            sleep(waitTimeForClear);
            SessionStartFlag = false;
        }   
    }

    function resetAll() 
    {
        inputObj.value = "";
        document.getElementById("lblProductIDDisplay").value = "";
        document.getElementById("lblCustomerSNDisplay").value = "";

        inputObj.focus();   
    }


    function ResetPage() 
    {
        ExitPage();
        resetAll();
    }

    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    function btnPrintSetting_onclick() {
        showPrintSetting(station, pCode);
    }
    
  
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceSNCheck.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" >
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
                       <input type="checkbox" name="isPrint" id="isPrintEnergy" value="">Print Energy Label
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
                            <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                            </button>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            </button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
                <td>
      <input id="btnPrintSetting" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'"    onclick="return btnPrintSetting_onclick()" />
                </td>
            </tr>
        
        </table>
        
        </center>
    </div>
    
</asp:Content>
