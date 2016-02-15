<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for KB CT Check Page
 * UI:CI-MES12-SPEC-FA-UI KB CT Check.docx –2012/6/12 
 * UC:CI-MES12-SPEC-FA-UC KB CT Check.docx –2012/6/12            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-6-12   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="KBCTCheckOffline.aspx.cs" Inherits="FA_KBCTCheckOffline" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgDataEntryNull = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryNull").ToString() %>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    //var inputData = "";
    var station = "";
    var prodid = "";
    var kbct = "";
    var SessionStartFlag = false;
    var newProcessFlag = true;
    var kbctFirstFlag = false;

    var waitDefect = false;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	加载接受输入数据事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    document.body.onload = function() {
        try {
            ShowInfo("");

            inputObj = getCommonInputObject();
            //inputData = inputObj.value;
            inputObj.focus();

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;
            newProcessFlag = true;

            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	CustomerSNEnterOrTab
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	CustomerSN中按enter或tab键的处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    /*
    function CustomerSNEnterOrTab() 
    {
        if (event.keyCode == 9 || event.keyCode == 13) 
        {
            getAvailableData("processDataEntry");
        }
    }
    */


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	processDataEntry
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputdata) 
    {    
        try {
            var uutInput = true;

            if (inputdata == "") 
            {
                alert(msgDataEntryNull);
                
                inputObj.focus();
                uutInput = false;
                
                getAvailableData("processDataEntry");
            }

            if (uutInput) 
            {
                if (checkInputValid(inputdata) == false)  
                {
                    alert(msgDataEntryField);
                    
                    inputObj.value = "";      
                    inputObj.focus();
                    
                    getAvailableData("processDataEntry");
                }
                else
                {
                    if (waitDefect == true) {
                        var currentProdid = SubStringSN(prodid, "ProdId");
                        WebServiceKBCTCheck.SaveDefect(currentProdid, inputdata, onSaveDefectSucceed, onFail);
                    }
                    else if (newProcessFlag == true)
                    {
                        //getAvailableData("processDataEntry");
                        document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
                        document.getElementById("<%=txtKBCT.ClientID%>").innerText = "";

                        if (kbctFirstFlag == true)
                        {
                            document.getElementById("<%=txtKBCT.ClientID%>").innerText = kbct;

                            inputObj.value = "";
                            callNextInput();
                        }
                        else 
                        {
                            beginWaitingCoverDiv();
                        
                            var line = ""; 
                            var currentProdid = SubStringSN(prodid, "ProdId");
                            SessionStartFlag = true;

                            WebServiceKBCTCheck.checkProdIdOffline(currentProdid, line, "<%=UserId%>", station, "<%=Customer%>", onCheckSucceed, onCheckFail);
                        }

                        newProcessFlag = false;
                    }
                    else
                    {
                        if (kbctFirstFlag == false) {
                            beginWaitingCoverDiv();

                            var line = "";
                            var currentProdid = SubStringSN(prodid, "ProdId");

                            WebServiceKBCTCheck.checkAndSaveOffline(currentProdid, kbct, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
                        }
                        else {
                            beginWaitingCoverDiv();

                            var line = "";
                            var currentProdid = SubStringSN(prodid, "ProdId");
                            SessionStartFlag = true;

                            WebServiceKBCTCheck.checkProdIdOffline(currentProdid, line, "<%=UserId%>", station, "<%=Customer%>", onCheckSucceed, onCheckFail);
                        }
                    }
                }    
            }
        } catch (e) {
            alert(e.description);
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkCustomerSNOnProductValid
    //| Author		:	Jessica Liu
    //| Create Date	:	10/21/2011
    //| Description	:	判断SN On Product是否合法
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkInputValid(inputdata) 
    {
        var strNoEmptyInput = inputdata.trim();
        var ret = true;
        var isProdid = false;
        var isKbct = false;
        
        if (newProcessFlag == true) {
            if ((prodid != "") || (kbct != "")) {
                ret = false;
            }
        }
        else
        {
            if ((prodid == "") && (kbct == "")) {
                ret = false;
            }
        }
        
        if ((strNoEmptyInput.length == 9) || (strNoEmptyInput.length == 10)) {
            isProdid = true;
        }
//        else if (strNoEmptyInput.length == 14) {
//            isKbct = true;
//        }
        else {
            //ret = false;
            isKbct = true;
        }

        if (ret == true) {
            if (((prodid != "") && (isProdid == true)) || ((kbct != "") && (isKbct == true)))
            {
                ret = false;
            }
        }

        if ((ret == true) && (isProdid == true)) {
            prodid = strNoEmptyInput;     //SubStringSN(strNoEmptyInput, "ProdId");

            if (newProcessFlag == true)
            {
                kbctFirstFlag = false;
            }
        }
        else if ((ret == true) && (isKbct == true)) {
            kbct = strNoEmptyInput;

            if (newProcessFlag == true) {
                kbctFirstFlag = true;
            }
        }

        if ((ret == false) && (waitDefect == true))
            ret = true;

        return ret;

    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onCheckSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	调用web service checkProdId成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onCheckSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();
                
        try {
            if (result == null) {
                //endWaitingCoverDiv();                
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
                document.getElementById("<%=txtProductID.ClientID%>").innerText = prodid;

                if (kbctFirstFlag == true) {
                    beginWaitingCoverDiv();

                    var line = "";
                    var currentProdid = SubStringSN(prodid, "ProdId");

                    WebServiceKBCTCheck.checkAndSaveOffline(currentProdid, kbct, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
                }
                else {
                    inputObj.value = "";
                    callNextInput();
                }
            }
            else {
                //ShowInfo("");
                //endWaitingCoverDiv();
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
    //| Create Date	:	6/12/2012
    //| Description	:	调用web service checkProdId失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onCheckFail(error) {
        SetInitStatus();
        SessionStartFlag = false;
        document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
        document.getElementById("<%=txtKBCT.ClientID%>").innerText = "";

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
    //| Create Date	:	6/12/2012
    //| Description	:	调用web service checkAndSave成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) 
    {
        //SetInitStatus();
        ShowInfo("");
        endWaitingCoverDiv();
        inputObj.value = "";
        inputObj.focus(); 
        /*newProcessFlag = true;
        kbctFirstFlag = false;
        SessionStartFlag = false;*/
               
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
            {
                if (kbctFirstFlag == false) {
                    document.getElementById("<%=txtKBCT.ClientID%>").innerText = kbct;
                }
                
                var successInfo = "";                
                successInfo += "[" + prodid + ", " + kbct + "] " + msgSuccess;
                ShowSuccessfulInfo(true, successInfo);
            }
            else 
            {
                /*
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
                */
                if (kbctFirstFlag == false) {
                    document.getElementById("<%=txtKBCT.ClientID%>").innerText = kbct;
                }

                waitDefect = true;
                ShowInfo("ProdId与 KB CT 匹配错误, 请刷 Defect Code!");
                callNextInput();
                return;
            }

            newProcessFlag = true;
            kbctFirstFlag = false;
            SessionStartFlag = false;

            waitDefect = false;
            
            prodid = "";
            kbct = "";
            callNextInput();
            
        } catch (e) {
            alert(e.description);
        }

    }

    function onSaveDefectSucceed(result) {
        //SetInitStatus();
        ShowInfo("");
        endWaitingCoverDiv();
        inputObj.value = "";
        inputObj.focus();
        newProcessFlag = true;
        kbctFirstFlag = false;
        SessionStartFlag = false;
        waitDefect = false;

        try {
            
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
                if (kbctFirstFlag == false) {
                    document.getElementById("<%=txtKBCT.ClientID%>").innerText = kbct;
                }

                var successInfo = "";
                successInfo += "[" + prodid + ", " + kbct + "] " + msgSuccess;
                ShowSuccessfulInfo(true, successInfo);
            }
            else {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }

            prodid = "";
            kbct = "";
            callNextInput();

        } catch (e) {
            alert(e.description);
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	调用web service checkAndSave失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) 
    {
        SetInitStatus();
        SessionStartFlag = false;
        document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
        document.getElementById("<%=txtKBCT.ClientID%>").innerText = "";
        waitDefect = false;
        
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
    //| Create Date	:	6/12/2012
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
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
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
    //| Name		:	onbeforeunload
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	onbeforeunload时调用
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    window.onbeforeunload = function() {
        ExitPage();
    } 

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        if (SessionStartFlag == true) {
            WebServiceKBCTCheck.Cancel(document.getElementById("<%=txtProductID.ClientID%>").innerText, station);
            sleep(waitTimeForClear);
            SessionStartFlag = false;
        }   
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
    //| Description	:	重置所有控件到初始状态
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        inputObj.value = "";
        //document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
        //document.getElementById("<%=txtKBCT.ClientID%>").innerText = "";

        newProcessFlag = true;
        kbctFirstFlag = false;

        prodid = "";
        kbct = "";
        
        inputObj.focus();   
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Jessica Liu
    //| Create Date	:	6/12/2012
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceKBCTCheck.asmx" />
            </Services>
        </asp:ScriptManager>

        <center>

        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">

            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>

            <tr>
                <td align="left" >
                    <asp:Label ID="lblProductID" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="txtProductID" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>     

            <tr><td>&nbsp;</td><td></td></tr>

            <tr>
                <td align="left" >
                    <asp:Label ID="lblKBCT" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="txtKBCT" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>     

            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
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
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" ></button>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none"></button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        </center>
    </div>    
</asp:Content>
