 <%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0725, Jessica Liu, 2012-2-24
* ITC-1360-1523, Jessica Liu, 2012-3-20
* ITC-1360-1687, Jessica Liu, 2012-4-11
*/ 
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="AssetTagLabelPrint.aspx.cs" Inherits="PAK_AssetTagLabelPrint" Title="Untitled Page" %>
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
    //    var msgNoPrintItem='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrintItem") %>';
    var inputObj = "";
    var inputData = "";
    var station = "";
    var assetSN = "";
    var flag = false;
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

    //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
    var msgMN2Error = '<%=this.GetLocalResourceObject(Pre + "_msgMN2Error").ToString() %>';
    var msgNoAST = '<%=this.GetLocalResourceObject(Pre + "_msgNoAST").ToString() %>';
    
    //2012-4-11
    var CustSN = "";
    
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;

            inputObj.focus();

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;

            //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
            document.getElementById("<%=ShowImage.ClientID %>").src = "";
            
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
        //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
        document.getElementById("<%=ShowImage.ClientID %>").src = "";
        
        //ITC-1360-1523, Jessica Liu, 2012-3-20
        try {
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
                getAvailableData("processDataEntry");
             
                if (checkCustomerSNValid(inputData) == false) 
                {
                    alert(msgDataEntryField);
                    inputObj.value = "";       
                    inputObj.focus();
                    
                    getAvailableData("processDataEntry");
                }
                else 
                {
                    var lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null) {
                        alert(msgPrintSettingPara);

                        inputObj.value = "";
                        document.getElementById("lblProductIDDisplay").value = "";
                        document.getElementById("lblCustomerSNDisplay").value = "";

                        inputObj.focus();
                        getAvailableData("processDataEntry");
                    }
                    else {
                        //2012-4-11
                        CustSN = inputObj.value;
                        
                        beginWaitingCoverDiv();

                        //WebServiceAssetTagLabelPrint.CheckCustomerSN(SubStringSN(inputData, "MB"), "", station, "<%=UserId%>", "<%=Customer%>", onSCNSucceed, onSCNFail);
                        WebServiceAssetTagLabelPrint.CheckCustomerSN(inputObj.value, "", station, "<%=UserId%>", "<%=Customer%>", onSCNSucceed, onSCNFail);
                    }
                }
            }
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }
    
    
    function checkCustomerSNValid(strCustomerSN) 
    {  
        var strNoEmptySN = strCustomerSN.trim();
        var ret = false;    
        
                
//        if (strNoEmptySN.length == 10)
//        {
//            //if (strNoEmptySN.substring(0, 2) == "CN")
//			if (CheckCustomerSN(strNoEmptySN))
//            {
//                ret = true;
//            }
//        }
//        else if (strNoEmptySN.length == 11)
//        {
//            //ITC-1360-0725, Jessica Liu, 2012-2-24
//            //if (strNoEmptySN.substring(0, 3) == "SCN")
//			if (CheckCustomerSN(strNoEmptySN))
//            {
//                ret = true;
//            }
//        }
        if (CheckCustomerSN(strNoEmptySN)) {
            ret = true;
        }
        return ret;
    
    }


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
				ExitPage(); 
                inputObj.value = "";
                
                inputObj.focus();
            }
            //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
            //else if ((result.length == 3) && (result[0] == SUCCESSRET))
            else if ((result.length == 5) && (result[0] == SUCCESSRET)) 
            {
                document.getElementById("lblCustomerSNDisplay").value = result[1];
                document.getElementById("lblProductIDDisplay").value = result[2];
                //assetSN = result[4];
                    
                var strProcuctID = document.getElementById("lblProductIDDisplay").value;
                var strCustomerSN = document.getElementById("lblCustomerSNDisplay").value;

                var lstPrintItem = getPrintItemCollection();
// * test，2011-10-18，需放开==========
                if (lstPrintItem == null) 
                {
                    alert(msgPrintSettingPara);

                    //2011-10-13去掉及新增                    
                    //等待快速控件继续输入
                    //callNextInput();
					ExitPage();
                    //2011-10-14，要清空控件
                    inputObj.value = "";
                    document.getElementById("lblProductIDDisplay").value = "";
                    document.getElementById("lblCustomerSNDisplay").value = "";

                    inputObj.focus(); 
                }
                else {
//test，2011-10-18，需放开==========* /                
                    //????下面内容均需按照编码重新修改
                    //setPrintItemListParam(lstPrintItem, strCustomerSN, strProcuctID);

                    //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
                    if (result[3] == "1") {
                        ShowInfo("");
                        ShowInfo(msgMN2Error);
                    }
                    else if (result[3] == "2") {
                        ShowInfo("");
                        ShowInfo(msgNoAST);
                    }
                    else if (result[3] == "0") {
                        var imageUrl = "";
                        var RDSServer = '<%=ConfigurationManager.AppSettings["RDS_Server_PAK"].Replace("\\", "\\\\")%>';
                        imageUrl = RDSServer + result[4] + ".JPG";
                        document.getElementById("<%=ShowImage.ClientID %>").src = imageUrl;
                    }       

                    beginWaitingCoverDiv();

                    WebServiceAssetTagLabelPrint.print(inputObj.value, strProcuctID, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
//test，2011-10-18，需放开==========
                }
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
				ExitPage(); 
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
                       
            endWaitingCoverDiv();
			ExitPage();
			resetAll();

            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();

            inputObj.focus();
        }

    }


    function setPrintItemListParam(backPrintItemList, customersn, astCodeList) 
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@sn";   //customer sn
        keyCollection[1] = "@AstCodeList";   //Ast code List
        valueCollection[0] = generateArray(customersn);
        valueCollection[1] = generateArray(astCodeList);

        /*
        * Function Name: setPrintParam
        * @param: printItemCollection
        * @param: labelType
        * @param: keyCollection(Client: Array of string.    Server: List<string>)
        * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        */
        //setPrintParam(lstPrtItem, "Asset Tage", keyCollection, valueCollection);
        setAllPrintParam(lstPrtItem,keyCollection, valueCollection);
    }


    function onSucceed(result) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
        
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                
                resetAll();
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) 
            {
                var strCustomerSN = document.getElementById("lblCustomerSNDisplay").value;
                setPrintItemListParam(result[1], strCustomerSN, result[2]);
                
                /*
                * Function Name: printLabels
                * @param: printItems
                * @param: isSerial
                */
                printLabels(result[1], false);

                //ITC-1360-1687, Jessica Liu, 2012-4-11 
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + CustSN + "] " + msgSuccess);

                //resetAll();
                inputObj.value = "";
                inputObj.focus();
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);

                resetAll();
            }

            callNextInput();
        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }


    function onFail(error) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
    
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            resetAll(); 
            callNextInput();

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }
    }


    function callNextInput() 
    {
        inputObj.focus();    

        getAvailableData("processDataEntry");
    }


    function showPrintSettingDialog() 
    {
        showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
    }
	
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onbeforeunload
    //| Author		:	Vincent
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
    //| Author		:	Vincent
    //| Create Date	:	11/22/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        if (inputObj.value !="" ) 
        {
            WebServiceAssetTagLabelPrint.Cancel(inputObj.value, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);           
        }
    }
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onClearSucceeded
    //| Author		:	Vincent
    //| Create Date	:	11/22/2011
    //| Description	:	Cancel WF成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onClearSucceeded(result) 
    {
       
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onClearFailed
    //| Author		:	Jessica Liu
    //| Create Date	:	Vincent
    //| Description	:	Cancel WF失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onClearFailed(error) 
    {
        
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
    
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceAssetTagLabelPrint.asmx" />
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
            
            <tr><td>&nbsp;</td><td></td></tr>
                       
            <tr>
                <td>&nbsp;</td>
                <td align="right">   
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
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
                <td colspan="4">
                    <asp:Image ID="ShowImage" runat="server" Width="550" Height="400"/>
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
        
        </table>
        
        </center>
    </div>
    
</asp:Content>
