<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for MB CT Check Page
* Known issues:
* TODO：
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="MBCTCheck.aspx.cs" Inherits="PAK_MBCTCheck" Title="Untitled Page" %>
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
    var station = "";
    var SessionStartFlag = false;
    var newProcessFlag = true;
    var mbct1 = '';
    var mbct2 = '';

    var inputData = "";
    var product = '';
    var mbct = '';
    var lcm = '';
    var sn = '';
    var callMBCT = '請輸入MBCT';
    var callLCM = '請輸入LCM';
    var callSN = '請輸入SN';    
    var checkLCM = false;
    var checkMBCT = false;
    var checkSN = false;
    var line = '';


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	
    //| Create Date	:	
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
    //| Name		:	processDataEntry
    //| Author		:	
    //| Create Date	:	
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputdata) 
    {    
        try {
            var content = '';

            if (inputdata == "") 
            {
                alert(msgDataEntryNull);
                inputObj.focus();
                getAvailableData("processDataEntry");
                return;
            }
            if (inputdata == "7777") {
                ResetPage();
                ShowInfo("");
                inputObj.focus();
                getAvailableData("processDataEntry");
                return;
            }

//            if (inputdata.length != 14) {
//                content = '输入错误';
//                ShowMessage(content);
//                ShowInfo(content);
//                inputObj.value = "";
//                callNextInput();
//                return;
//            }

            if (newProcessFlag) {
                if (!isProdID(SubStringSN(inputdata, "ProdId"), "")) {
                    content = '输入错误ProdId';
                    ShowMessage(content);
                    ShowInfo(content);
                    inputObj.value = "";
                    callNextInput();
                    return;
                }
                product = inputdata;
                beginWaitingCoverDiv(); //
                WebServiceMBCTCheck.InputProduct(product, line, document.getElementById("<%=hideditor.ClientID%>").value, document.getElementById("<%=hiddenStation.ClientID%>").value, document.getElementById("<%=hidCustomer.ClientID%>").value, onInputSucceed, onInputFail);
                return;
            }
            else {
                var msg = '';
                if (document.getElementById("<%=lblLCM.ClientID%>").innerText == inputdata) {//lcm
                    //msg = callMBCT;
                    document.getElementById("<%=txtLCM.ClientID%>").innerText = inputdata;
                    checkLCM = true;
                }
                else if (document.getElementById("<%=lblMBCT.ClientID%>").innerText == inputdata) {//mbct
                    //msg = callSN;
                    document.getElementById("<%=txtMBCT.ClientID%>").innerText = inputdata;
                    checkMBCT = true;
                }
                else if (document.getElementById("<%=lblSN.ClientID%>").innerText == inputdata) {//sn
                    //msg = callSN;
                    document.getElementById("<%=txtSN.ClientID%>").innerText = inputdata;
                    checkSN = true;
                }
                else {
                    //Check error 沒有符合的partno
                    ShowInfo("Check error 沒有符合的partno");
                    inputObj.value = "";
                    callNextInput();
                }
                
                if (checkLCM && checkMBCT && checkSN) {
                    WebServiceMBCTCheck.Save(product, line, document.getElementById("<%=hideditor.ClientID%>").value, document.getElementById("<%=hiddenStation.ClientID%>").value, document.getElementById("<%=hidCustomer.ClientID%>").value, onSucceed, onFail);
                }//string pcbno1, string mbct1, string mbct2, string pdLine, string editor, string stationId, string customerId)
                else {
                    ShowInfo(msg);
                    inputObj.value = "";
                    callNextInput();
                }
            }
        } catch (e) {
            alert(e.description);
        }
    }

    
    function onInputSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                lcm = result[1][0];
                mbct = result[1][1];
                sn = result[1][2];
                if (sn == "" || sn == null) {
                    checkSN = true;
                }
                document.getElementById("<%=lblLCM.ClientID%>").innerText = lcm;
                document.getElementById("<%=lblMBCT.ClientID%>").innerText = mbct;
                document.getElementById("<%=lblSN.ClientID%>").innerText = sn;
                document.getElementById("<%=lblProduct.ClientID%>").innerText = product;
                newProcessFlag = false;
                SessionStartFlag = true;
                ShowInfo('請刷入LCM & MBCT & SN');
                inputObj.value = "";
                callNextInput();
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

    function onInputFail(error) {
        SetInitStatus();
        SessionStartFlag = true;
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
    //| Author		:	
    //| Create Date	:	
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
        newProcessFlag = true;
        SessionStartFlag = false;
               
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
            {
                var successInfo = "检查成功 " + " [" + product + "] ";
                successInfo += " [" + lcm + ", " + mbct + ", " + sn + "] " + msgSuccess;
                ShowSuccessfulInfo(true, successInfo);
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }

            resetAll();
            callNextInput();
            
        } catch (e) {
            alert(e.description);
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	
    //| Create Date	:	
    //| Description	:	调用web service checkAndSave失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) 
    {
        SetInitStatus();
        SessionStartFlag = true;
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
    //| Author		:	
    //| Create Date	:	
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
    //| Author		:	
    //| Create Date	:	
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
    //| Author		:	
    //| Create Date	:	
    //| Description	:	onbeforeunload时调用
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    window.onbeforeunload = function() {
        ExitPage();
    } 

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	
    //| Create Date	:	
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        if (SessionStartFlag == true) {
            WebServiceMBCTCheck.Cancel(product, station);
            sleep(waitTimeForClear);
            SessionStartFlag = false;
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	
    //| Create Date	:	
    //| Description	:	重置所有控件到初始状态
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        inputObj.value = "";
        document.getElementById("<%=txtLCM.ClientID%>").innerText = "";
        document.getElementById("<%=txtMBCT.ClientID%>").innerText = "";
        document.getElementById("<%=txtSN.ClientID%>").innerText = "";
        document.getElementById("<%=lblLCM.ClientID%>").innerText = "";
        document.getElementById("<%=lblMBCT.ClientID%>").innerText = "";
        document.getElementById("<%=lblProduct.ClientID%>").innerText = "";
        document.getElementById("<%=lblSN.ClientID%>").innerText = "";
        newProcessFlag = true;
		product = '';
		mbct = '';
		lcm = '';
		sn = '';
		checkLCM = false;
		checkMBCT = false;
		checkSN = false;
		line = '';
        inputObj.focus();   
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	
    //| Create Date	:	
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
                <asp:ServiceReference Path="Service/WebServiceMBCTCheck.asmx" />
            </Services>
        </asp:ScriptManager>

        <center>

        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">


            
            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="Label3" runat="server"  CssClass="iMes_label_13pt">ProductID:</asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="lblProduct" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>    
            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="Label1" runat="server"  CssClass="iMes_label_13pt">PartType</asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="Label2" runat="server"  CssClass="iMes_label_11pt">Value</asp:Label>
                </td>
            </tr>    
            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td align="left" >
                    <asp:Label ID="lblLCM" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="txtLCM" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>     

            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td align="left" >
                    <asp:Label ID="lblMBCT" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="txtMBCT" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>     

            <tr>
                <td>&nbsp;</td><td></td>
            </tr>
            <tr>
                <td align="left" >
                    <asp:Label ID="lblSN" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="txtSN" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>

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

            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <input id="hideditor" type="hidden" runat="server" />
                            <input id="hidCustomer" type="hidden" runat="server" />
                            <button id="btnReset" runat="server" type="button" onserverclick="btnReset_Click" style="display: none" />
                            <button id="hiddenbtn" runat="server" style="display: none"></button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        </center>
    </div>    
</asp:Content>
