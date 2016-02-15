<%--
/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:UI for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-7-23   Jessica Liu           Create
* Known issues:
* TODO：
* ITC-1428-0003, Jessica Liu, 2012-9-7
* ITC-1428-0012, Jessica Liu, 2012-9-10
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RCTOMBChange.aspx.cs" Inherits="SA_RCTOMBChange" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var inputObj = "";
    var inputData = "";
    var station = "";
    var flowFlag = false;     
    var dataEntry = "";
    var mbSNo = "";
    var feature = "dialogHeight:560px;dialogWidth:860px;center:yes;status:no;help:no";     //the style of pop up window
    var url = "RCTOMBChangeReprint.aspx";

    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";    
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';
    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgDataEntryNull = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryNull").ToString() %>';
    var msgInputMBSno = '<%=this.GetLocalResourceObject(Pre + "_msgInputMBSno").ToString() %>';
    //ITC-1413-0147, Jessica Liu, 2012-8-30
    var AccountId = '<%=Request["AccountId"] %>';
    var Login = '<%=Request["Login"] %>';
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	加载接受输入数据事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;
            mbSNo = "";

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;

            //ITC-1413-0147, Jessica Liu, 2012-8-30
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';

            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);

            setPdLineCmbFocus();
        }

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkInputInfo
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	底层调用前的信息检测
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkInputInfo() 
    {
        if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) 
        {
            alert(msgPdLineNull);
            ShowInfo(msgPdLineNull);

            if (flowFlag) 
            {
                ExitPage();
                flowFlag = false;
            }

            setPdLineCmbFocus();

            return false;
        }

        if (mbSNo == "" || mbSNo == null) 
        {
            alert(msgInputMBSno);
            ShowInfo(msgInputMBSno);
            inputObj.focus();

            return false;
        }

        if (!isMBSno(mbSNo)) {
            alert(msgDataEntryField);
            ShowInfo(msgDataEntryField);
            inputObj.focus();
        
            return false;
        }

        mbSNo = SubStringSN(mbSNo, "MB");
        
        return true;
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	isMBSno
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	合法MB SNo检测
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function isMBSno(input) 
    {
        var ret = false;
        //ITC-1428-0012, Jessica Liu, 2012-9-10
        if ((input.length == 10 || input.length == 11) && (input.substr(4, 1) == "M")) {
            ret = true;
        }
        else if ((input.length == 11) && (input.substr(5, 1) == "M")) {
            ret = true;
        }

        return ret;
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	processDataEntry
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputData) 
    {
        ShowInfo("");

        document.getElementById("lblMBSNoDisplay").value = "";
        document.getElementById("txtModel").value = "";
        
        mbSNo = inputData;

        if (checkInputInfo())
        {
            if (!flowFlag) 
            {
                beginWaitingCoverDiv();

                //ITC-1428-0003, Jessica Liu, 2012-9-7
                flowFlag = true;
                dataEntry = mbSNo;

                WebServiceRCTOMBChange.checkMBSNo(mbSNo, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", onSCNSucceed, onSCNFail);              
            }
        }
        else {
            getAvailableData("processDataEntry");
        }
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSCNSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	checkMBSNo成功
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
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) 
            {
                mbSNo = result[1];
                document.getElementById("lblMBSNoDisplay").value = result[1];
                document.getElementById("txtModel").value = result[2];
                    
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) 
                {
                    alert(msgPrintSettingPara);

                    //Jessica Liu, 2012-9-7
                    document.getElementById("lblMBSNoDisplay").value = "";
                    document.getElementById("txtModel").value = "";                    
                    ExitPage();
                    flowFlag = false;
                    callNextInput();
                }
                else {
                    beginWaitingCoverDiv();

                    //ITC-1428-0003, Jessica Liu, 2012-9-7
                    //flowFlag = true;

                    WebServiceRCTOMBChange.saveAndPrint(mbSNo, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
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
    //| Create Date	:	7/25/2012
    //| Description	:	checkMBSNo失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSCNFail(error) 
    {
        ShowInfo("");

        //ITC-1428-0003, Jessica Liu, 2012-9-7
        flowFlag = false;
        
        try {           
            endWaitingCoverDiv();

            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            //Jessica Liu, 2012-9-7
            document.getElementById("lblMBSNoDisplay").value = "";
            document.getElementById("txtModel").value = "";
  
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
    //| Create Date	:	7/25/2012
    //| Description	:	调用web service saveAndPrint成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();

        flowFlag = false;

        try {
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                reset();
                ShowInfo(content);
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) 
            {
                //2012-9-7, Jessica Liu
                //setPrintItemListParam(result[1], mbSNo);
                setPrintItemListParam(result[1], result[2]);
               
                printLabels(result[1], false);

                ShowSuccessfulInfo(true, "[" + mbSNo + "] " + msgSuccess); 
            }
            else {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);

                reset();

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
    //| Create Date	:	7/25/2012
    //| Description	:	调用web service saveAndPrint失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        flowFlag = false;

        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            //Jessica Liu, 2012-9-7
            document.getElementById("lblMBSNoDisplay").value = "";
            document.getElementById("txtModel").value = "";
 
            callNextInput();

        } catch (e) {
            alert(e.description);
            inputObj.focus();
        }
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	setPrintItemListParam
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	产生打印信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function setPrintItemListParam(backPrintItemList, retMBSno) 
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@MBSN";
        valueCollection[0] = generateArray(retMBSno);

        setPrintParam(lstPrtItem, "RCTO_MB_Label", keyCollection, valueCollection);
    }
    

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	reprint
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	点击reprint按钮的处理函数
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function reprint() 
    {
        //ITC-1413-0147, Jessica Liu, 2012-8-30
        //window.showModalDialog(url + "?Customer=" + "<%=Customer%>", window, feature);
        window.showModalDialog(url + "?Station=" + stationId + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + AccountId + "&Login=" + Login, window, feature);
        //window.showModalDialog(url + "?Customer=" + "<%=Customer%>", "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	reset
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	清空输入控件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function reset() 
    {
        getPdLineCmbObj().selectedIndex = 0;
        document.getElementById("lblMBSNoDisplay").value = "";
        document.getElementById("txtModel").value = "";
        inputObj.value = "";
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callNextInput
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
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
    //| Create Date	:	7/25/2012
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
    //| Author		:	7/25/2012
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
    //| Create Date	:	7/25/2012
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        if (flowFlag) 
        {
            WebServiceRCTOMBChange.Cancel(dataEntry, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
            flowFlag = false;
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onClearSucceeded
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
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
    //| Create Date	:	7/25/2012
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
    //| Create Date	:	7/25/2012
    //| Description	:	
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        ShowInfo("");
        reset();

        document.getElementById("<%=btnHidden.ClientID%>").click(); 
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Jessica Liu
    //| Create Date	:	7/25/2012
    //| Description	:	刷新页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ResetPage() 
    {
        ExitPage();
        resetAll();
        flowFlag = false;
        //Jessica Liu, 2012-9-7
        callNextInput();
    }    
    
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceRCTOMBChange.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>                
                <td>
                    <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="6">
                    <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="SA"/>
                </td>
            </tr>     
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td align="left" >
                    <asp:Label ID="lblMBSNo" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="6">
                    <input type="text" ID="lblMBSNoDisplay" class="iMes_textbox_input_Disabled"
                             MaxLength="20" style="width:98%" readonly="readonly"/>
                </td>
            </tr>  
            
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
                <td align="left" >
                    <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="6">
                    <input type="text" ID="txtModel" class="iMes_textbox_input_Disabled"
                             MaxLength="20" style="width:98%" readonly="readonly"/>
                </td>
            </tr>         

            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td width="60%" align="left" colspan="4">
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

                <td align="left">
                    <input id="btnReprint" type="button"  runat="server" onclick="reprint()" 
                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                        style="width:100px"  class=" iMes_button" />
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
