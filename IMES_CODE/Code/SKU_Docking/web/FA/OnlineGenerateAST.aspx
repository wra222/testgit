﻿<%--
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
        <script type="text/javascript" src="../CommonControl/JS/BomTool.js"></script>
         <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
<style type="text/css">
    .trHeight
    {
        height: 41px;
    }
    .tdTitle
    {
         width: 150px;
    }
     .tdValue
    {
        font-size:larger;
   }
</style>
 
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceOnlineGenerateAST.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
           
               <table border="0" width="100%" >
               <tr>
               <td>
        <table width="95%" style="vertical-align:middle" cellpadding="0" cellspacing="0">
  
         
            
             <tr class="trHeight">          
                <td style="width: 150px;">
                    <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="5">
                    <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="FA"/>
               
                </td>
            </tr>     
            
      
            
            <tr class="trHeight">
                <td align="left" class="tdTitle">
                    <asp:Label ID="lblProductID" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
             <td id="tdProId" align="left" style="width:25%;" class="tdValue">
                 &nbsp;</td>
                   <td align="left" class="tdTitle">
                       <asp:Label ID="lblCPQSNO" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
                    </td>
                       <td id="tdCUSTSN" align="left" style="width:25%;" class="tdValue">
                           &nbsp;</td>
                      <td class="tdTitle">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                    
                          <td id="tdModel" align="left" style="width:25%;" class="tdValue">
                              &nbsp;</td>  
            </tr>  
            
    
    
           <tr class="tdHeight">
                <td  style="width: 150px;" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" colspan="4">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
             InputRegularExpression="^[-0-9a-zA-Z\+\s\*\,]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
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
            
 
            
         
        
        </table>
 
        
         <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td colspan="2">
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="160px" Style="top: 0px; left: 0px" Width="98%" 
                                Height="160px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                    <tr><td colspan="2"></td></tr>
                     <tr>
                
            
            </tr>
            
               <tr>
                
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
            </fieldset>
            </td>
            <td>
            <td >
                    <asp:Image ID="ShowImage" runat="server" Width="550" Height="400"/>
                </td>
                <td valign="top">
                <asp:Label ID="lbMsg1" runat="server"  CssClass="iMes_label_13pt" 
                        Font-Size="Larger"></asp:Label>
                <br />
                <asp:Label ID="lbMsg2" runat="server"  Font-Size="Larger" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            
            </td>
            </tr>
            </table>        
           
        </center>
    </div>
<script type="text/javascript">
    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgNoProductID = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoProductID") %>';
    var msgNoPrinted = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrinted") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var assetSN = "";
    var dataEntry = "";
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';
    var msgInputProIDOrCustsn = '<%=this.GetLocalResourceObject(Pre + "_msgInputProIDOrCustsn").ToString() %>';
    var msgGoToCombineAST = '<%=this.GetLocalResourceObject(Pre + "_msgGoToCombineAST").ToString() %>';
    var msgMN2Error = '<%=this.GetLocalResourceObject(Pre + "_msgMN2Error").ToString() %>';
    var msgNoAST = '<%=this.GetLocalResourceObject(Pre + "_msgNoAST").ToString() %>'; //cn_msgNeedAS
    var msgNeedAST = '<%=this.GetLocalResourceObject(Pre + "_msgNeedAST").ToString() %>'; //cn_msgNeedAS
    var CustSNOrProdID = "";
    var defaultRowNum = '<%=DEFAULT_ROWS %>';
    var iSelectedRowIndex = -1;
    var bomTool;
    var tbl;
    var _sessionKey = "";
    var lstPrintItem;
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;
            station = document.getElementById("<%=hiddenStation.ClientID %>").value;
            document.getElementById("<%=ShowImage.ClientID %>").src = "";
            tbl = "<%=gd.ClientID %>";
            getAvailableData("processDataEntry")
            IniPage();

        } catch (e) {
            alert(e.description);

            setPdLineCmbFocus();
        }

    }

    function IniPage() {
        iSelectedRowIndex = -1;
        bomTool = null;
        ClearGvExtTable(tbl, defaultRowNum);
        CustSNOrProdID = "";
        _sessionKey = "";
        $("[id^='td']").text("");
    }

    function CustomerSNEnterOrTab() {
        if (event.keyCode == 9 || event.keyCode == 13) {
            getAvailableData("processDataEntry");
        }
    }


    function processDataEntry(inputData) {
        ShowInfo("");
        document.getElementById("<%=lbMsg1.ClientID %>").innerText = "";
        document.getElementById("<%=lbMsg2.ClientID %>").innerText = "";
        //document.getElementById("<%=ShowImage.ClientID %>").src = "";
        lstPrintItem = getPrintItemCollection();
        if (lstPrintItem == null) {
            alert(msgPrintSettingPara);
            callNextInput();
            return;
        }
        if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
            alert(msgPdLineNull);
            callNextInput();
            return;
        }


        if (_sessionKey == "") {
            inputData = Get2DCodeCustSN(inputData);//2维码SN
            if (isProdIDorCustSN(inputData, "") == false) {
                alert(msgDataEntryField);
                ShowInfo(msgDataEntryField);
                callNextInput();
                return;
            } else {
                beginWaitingCoverDiv();
                if (inputData.length == 10) {
                    if (!CheckCustomerSN(inputData)) {
                        inputData = SubStringSN(inputData, "ProdId");
                    }
                }
                _sessionKey = inputData;
                WebServiceOnlineGenerateAST.CheckCustomerSN_New(_sessionKey, getPdLineCmbValue(), station, "<%=UserId%>", "<%=Customer%>", false, onSCNSucceed, onSCNFail);
            }

        } else {
            checkPart(inputData);
        }
        callNextInput();
    }


    function onSCNSucceed(result) {
        try {
            endWaitingCoverDiv();
            document.getElementById("<%=ShowImage.ClientID %>").src = "";
            if (result == null) {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                ExitPage();
                callNextInput();
            } else if ((result.length == 8) && (result[0] == SUCCESSRET)) //2012-4-10
            {

              
                $("#tdProId").text(result[1]);
                $("#tdCUSTSN").text(result[2]);
                $("#tdModel").text(result[3]);
                 if (result[4] == "TRUE") {
                    document.getElementById("<%=lbMsg1.ClientID %>").innerText = msgNeedAST;
                }
                if (result[5] == "1") {
                    document.getElementById("<%=lbMsg2.ClientID %>").innerText = msgMN2Error;
                } else if (result[5] == "2") {
                    document.getElementById("<%=lbMsg2.ClientID %>").innerText = msgNoAST;
                } else if (result[5] == "0") {
                    var imageUrl = "";
                    var RDSServer = '<%=ConfigurationManager.AppSettings["RDS_Server"].Replace("\\", "\\\\")%>';
                    imageUrl = RDSServer + result[6] + ".JPG";
                    document.getElementById("<%=ShowImage.ClientID %>").src = imageUrl;
                }

                if (result[7].length == 0) {
                    beginWaitingCoverDiv();
                    //WebServiceOnlineGenerateAST.print(inputObj.value, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
                    WebServiceOnlineGenerateAST.print(_sessionKey, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
                } else {
                    bomTool = new BomTool(result[7], tbl, defaultRowNum);
                    // setInfo(result);
                    bomTool.BindBomGrid();
                }

            } else {
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


    function onSCNFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            IniPage();
            callNextInput();

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();

            inputObj.focus();
        }

    }


    function onSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                reset();
                ShowInfo(content);
            } else if ((result.length == 5) && (result[0] == SUCCESSRET) && (result[4] == "")) {
                var _prodId=$("#tdProId").text();
                setPrintItemListParam(result[1], _prodId, result[3]);
                printLabels(result[1], false);
                ShowSuccessfulInfo(true, "[" + _sessionKey + "] " + msgSuccess + " " + result[2]);
            } else if (result[4] != "") {
                ShowSuccessfulInfo(true, "[" + _sessionKey + "] " + msgSuccess + " " + result[2] + " ," + result[4]);
            } else {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                reset();
                ShowInfo(content1);
            }

        } catch (e) {
            alert(e.description);
            inputObj.focus();
        } finally {
            IniPage();
            callNextInput();
        }

    }

    function onFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();
        reset();
        try {

            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            callNextInput();

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }
    }

    function setPrintItemListParam(backPrintItemList, prodid, astCodeList) //, ast) 
    {
        //============================================generate PrintItem List==========================================
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@productID"; //
        keyCollection[1] = "@AstCodeList"; //Ast code List
        valueCollection[0] = generateArray(prodid);
        valueCollection[1] = generateArray(astCodeList);
        setAllPrintParam(lstPrtItem, keyCollection, valueCollection);
    }


    function reprint() {
        var str = prompt(msgInputProIDOrCustsn, "");
        if (str != null) {
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);

                inputObj.value = "";
                $("[id^='td']").text("");

                inputObj.focus();
            } else {
                beginWaitingCoverDiv();
                WebServiceOnlineGenerateAST.reprint(str, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", "", lstPrintItem, onRPSucceed, onRPFail);
            }

        }
    }

    function onRPSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            if (result == null) {
                var content = msgSystemError;
                reset();
                ShowMessage(content);
                ShowInfo(content);

            } else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
                setPrintItemListParam(result[1], result[2]);
                printLabels(result[1], false);
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess);
            } else {
                ShowInfo("");
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

    function onRPFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            callNextInput();

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }
    }


    function setGdHighLight(index) {
        if ((iSelectedRowIndex != -1) && (iSelectedRowIndex != index)) {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, tbl); //去掉过去高亮行           
        }
        setRowSelectedOrNotSelectedByIndex(index, true, tbl); //设置当前高亮行
        iSelectedRowIndex = index; //记住当前高亮行
    }

    function checkPart(data) {
        WebServiceOnlineGenerateAST.checkPart(_sessionKey, data, onCheckSuccess, onCheckFail);
    }

    function onCheckSuccess(result) {
        var matchObj = result[1];
        var index = bomTool.GetMatchGridIdxAndUpdateQty(matchObj);
        if (index < 0) {
            ShowInfo("System error!");
            callNextInput();
            return;
        }
        bomTool.UpdateMatchPartDataInGrid(index, matchObj);
        setGdHighLight(index);
        var bFinished = bomTool.CheckFinishScan();
        if (bFinished == true) {
            beginWaitingCoverDiv();
            //WebServiceOnlineGenerateAST.print(inputObj.value, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
            WebServiceOnlineGenerateAST.print(_sessionKey, document.getElementById("<%=line.ClientID %>").value, station, "<%=UserId%>", "<%=Customer%>", lstPrintItem, onSucceed, onFail);
        }
        callNextInput();
    }

    function onCheckFail(result) {
        ShowInfo(result.get_message());
        callNextInput();
    }

    function reset() {
        Cancel();
        getPdLineCmbObj().selectedIndex = 0;
        $("[id^='td']").text("");
        inputObj.value = "";
        ClearGvExtTable(tbl, defaultRowNum);
        iSelectedRowIndex = -1;
        bomTool = null;
        _sessionKey = "";
    }

    function callNextInput() {
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
    }

    function showPrintSettingDialog() {
        showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
    }
    window.onbeforeunload = function() {
        Cancel();
    }

    function Cancel() {
        if (_sessionKey != "") {
            WebServiceOnlineGenerateAST.Cancel(_sessionKey, station);
         //   WebServiceOnlineGenerateAST.Cancel(_sessionKey, station, onClearSucceeded, onClearFailed);
            
        }
    }

    function onClearSucceeded(result) { }
    function onClearFailed(error) { }

</script>
</asp:Content>