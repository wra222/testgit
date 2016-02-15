<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0542, Jessica Liu, 2012-2-18
* ITC-1360-0824, Jessica Liu, 2012-2-28
* ITC-1360-0824, Jessica Liu, 2012-3-7
* ITC-1360-1514, Jessica Liu, 2012-3-20
* ITC-1360-1678, Jessica Liu, 2012-4-11
* ITC-1360-1686, Jessica Liu, 2012-4-12
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="SNCheck.aspx.cs" Inherits="PAK_SNCheck" Title="Untitled Page" %>
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
    var custsnOnProductValue = "";
    var firstInput = false;
    var flag = false;
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var SessionStartFlag = false;

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

            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	CustomerSNEnterOrTab
    //| Author		:	Jessica Liu
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
    //| Author		:	Jessica Liu
    //| Create Date	:	10/12/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

        if (uutInput) 
        {
            if (firstInput == false)    
            {              
                getAvailableData("processDataEntry");
      
                if (checkCustomerSNOnProductValid(inputData) == false)  
                {
                    alert(msgDataEntryField);
                    inputObj.value = "";      
                    inputObj.focus();
                    
                    getAvailableData("processDataEntry");
                }
                else 
                {                  
                    beginWaitingCoverDiv();
                    
                    var line = ""; 

                    custsnOnProductValue = inputObj.value;

                    SessionStartFlag = true;
                    WebServiceSNCheck.inputCustSNOnProduct(inputObj.value, line, "<%=UserId%>", station, "<%=Customer%>", onFSNSucceed, onFSNFail);
                }
                
            }
            else    
            {
                getAvailableData("processDataEntry");
      
                if (checkCustomerSNOnPizzaValid(inputData) == false) 
                {
                    alert(msgDataEntryField);

                    /* 2012-2-21                 
                    firstInput = false;
                    
                    resetAll();
                    WebServiceSNCheck.Cancel(document.getElementById("lblCustomerSNDisplay").value, station);
                    */
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
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkCustomerSNOnProductValid
    //| Author		:	Jessica Liu
    //| Create Date	:	10/21/2011
    //| Description	:	判断SN On Product是否合法
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkCustomerSNOnProductValid(strCustomerSN) 
    {  
        var strNoEmptySN = strCustomerSN.trim();
        var ret = false;   
        
        if (strNoEmptySN.length == 10)
        {
            if (strNoEmptySN.substr(0, 3) == "CNU")     
            {
                ret = true;
            }
        }       
        
        return ret;
    
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkCustomerSNOnPizzaValid
    //| Author		:	Jessica Liu
    //| Create Date	:	10/21/2011
    //| Description	:	判断SN On Pizza是否合法
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkCustomerSNOnPizzaValid(strCustomerSN) 
    {  
        var strNoEmptySN = strCustomerSN.trim();
        var ret = false;    
        
        if (strNoEmptySN.length == 11)
        {
            if ((strNoEmptySN.substr(0, 1) == "P") || (strNoEmptySN.substr(0, 1) == "A"))
            {
                ret = true;
            }
        }
        
        return ret;
    
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFSNSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	10/21/2011
    //| Description	:	第一次SN扫入成功，获取到Product ID
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFSNSucceed(result) 
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
                
                WebServiceSNCheck.inputCustSNOnPizzaReturn(custsnOnPizza, custsnOnProduct, line1, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
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
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) 
            {
                //ITC-1360-1686, Jessica Liu, 2012-4-12
                var successInfo = "";
                /* ITC-1360-1678, Jessica Liu, 2012-4-11
                successInfo += "  " + result[1];
                ShowInfo(successInfo); 
                */
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

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	调用web service inputCustSNOnPizza失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	SetInitStatus
    //| Author		:	Jessica Liu
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
    //| Name		:	onbeforeunload
    //| Author		:	Jessica Liu
    //| Create Date	:	7/3/2012
    //| Description	:	onbeforeunload时调用
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //ITC-1360-0824, Jessica Liu, 2012-3-7
    window.onbeforeunload = function() {
        ExitPage();
    } 

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        //ITC-1360-0824, Jessica Liu, 2012-2-28
        if (SessionStartFlag == true) {
            WebServiceSNCheck.Cancel(document.getElementById("lblCustomerSNDisplay").value, station);
            sleep(waitTimeForClear);
            SessionStartFlag = false;
        }   
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	Jessica Liu
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

        inputObj.focus();   
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Jessica Liu
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
        
        </table>
        
        </center>
    </div>
    
</asp:Content>
