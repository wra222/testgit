﻿<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx –2011/11/15 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx –2011/11/15            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011/11/15    Du.Xuan               Create   
* Known issues:

*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PDPALabel02_CQ.aspx.cs" Inherits="PAK_PDPALabel02_CQ" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePDPALabel02.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCode" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbLabelKittingCode ID="CmbCode" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblFloor" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbLocFloor id="CmbFloor" name='CmbFloor' runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
            </table>
            <hr />
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                 
                    <tr >
                        <td style="width:10%">
                            <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:Label ID="lblCustSNContent" runat="server" Font-Size="11pt" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                           <td style="width:10%">
                            <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIDContent" runat="server" Font-Size="11pt" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    </table>
                    <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                          <td style="width:10%">
                            <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td align="left" >
                            <asp:Label ID="lblModelContent" runat="server" Font-Size="11pt" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                         <td style="width:10%">
                            <asp:Label ID="lblNeedMenu" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
          
                        </td>
            </tr>
                </table>
            </fieldset>
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                    
                                
                                  <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="100px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" HighLightRowPosition="3"
                                        Style="top: 0px; left: 0px">
                                
                                   <Columns>
                            <asp:BoundField DataField="Tp"/>
                            <asp:BoundField DataField="PartNo" />
                            <asp:BoundField DataField="Scan"  />
                          
                        </Columns>
                             
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                         <td style="width: 120px;">
                            <asp:CheckBox ID="QueryChk" runat="server" CssClass="iMes_CheckBox" />
                        </td>
                        <td  align="right" style="width: 110px;">
                            <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                            </button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <input id="modelHidden" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 4;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var customer="";
        var stationId="";
        var pCode = "";
        var hostname = getClientHostName();
        
        var inputObj;
        var emptyPattern = /^\s*$/;
        
        var customerSN = "";
        var productID = "";
        var model = "";

        var wwanCheck = false;
        var hitachiCheck = false;
        var cardSNCheck = false;

        var QueryFlag = false;
                
        var msgNeedMenu = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_lblNeedMenu") %>';
        //error message
        var msgInputPDLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
        var msgInputCode = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPcodeIsNull") %>'
        var msgInputFloor = '<%=this.GetLocalResourceObject(Pre + "_msgInputFloor").ToString() %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgNeedWWAN = '<%=this.GetLocalResourceObject(Pre + "_msgNeedWWAN").ToString() %>';
        var msgNeedCardSN = '<%=this.GetLocalResourceObject(Pre + "_msgNeedCardSN").ToString() %>';
        var msgNeedHitachi = '<%=this.GetLocalResourceObject(Pre + "_msgNeedHitachi").ToString() %>';
        var msgHitachiError = '<%=this.GetLocalResourceObject(Pre + "_msgHitachiError").ToString() %>';
        
        var msgNotSame = '<%=this.GetLocalResourceObject(Pre + "_msgNotSame").ToString() %>';
        var msgNoPath = '<%=this.GetLocalResourceObject(Pre + "_msgNoPath").ToString() %>';
        var msgNoTemp = '<%=this.GetLocalResourceObject(Pre + "_msgNoTemp").ToString() %>';
        var msgErrCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgErrCreatePDF").ToString() %>';
        var msgCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgCreatePDF").ToString() %>';
        var msgInputCpuLabel = '<%=this.GetLocalResourceObject(Pre + "_msgInputCpuLabel").ToString() %>';
		var msgNoNeedCpuLabel = '<%=this.GetLocalResourceObject(Pre + "_msgNoNeedCpuLabel").ToString() %>';
        //msgInputCpuLabel
        var imgAddr = "";
        var webEDITSaddr = "";
        var xmlFilePath = "";
        var pdfFilePath = "";
        var tmpFilePath = "";
        var fopFilePath = ""
        var templateName = "";

        var needReset = false;
        var endInputPn = false;
        //var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        //var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
		var noNeedCpuLabel = false;

        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();

            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            
            getPdLineCmbObj().selectedIndex = 0;
            getLabelKittingCodeCmbObj().selectedIndex = 0;
            getLocFloorCmbObj().selectedIndex = 0;
            document.getElementById("<%=QueryChk.ClientID%>").checked = false;
            
            initPage();
            //callNextInput();
            beginWaitingCoverDiv();

            var nameCollection = new Array();
            nameCollection.push("PLEditsImage");
            nameCollection.push("PLEditsURL");
            nameCollection.push("PLEditsXML");
            nameCollection.push("PLEditsPDF");
            nameCollection.push("PLEditsTemplate");
            nameCollection.push("FOPFullFileName");
            WebServicePDPALabel02.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);
            endWaitingCoverDiv();
            callNextInput();
            /*  For Light using  
           
            WebServicePDPALabel02.GetCommSetting(hostname,editor,onGetCommSucceed, onGetCommFailed);
           */
        };

        window.onbeforeunload = function() {

            OnCancel();
        };

        function onGetSetting(result) {
            //endWaitingCoverDiv();

            if (result == null) {
                setobjMSCommParaForLights();
            }
            else if (result[0] == SUCCESSRET) {
                imgAddr = result[1][0];
                webEDITSaddr = result[1][1];
                xmlFilePath = result[1][2];
                pdfFilePath = result[1][3];
                tmpFilePath = result[1][4];
                fopFilePath = result[1][5];
                var path = imgAddr + "\\*.jpg";
                //Change to EDITS GenPDF, Do not need copy jpg to c:\
                //var fso = new ActiveXObject("Scripting.FileSystemObject");
                //var fileflag = fso.FolderExists(imgAddr);
                //if (fileflag) {
                //    fso.CopyFile(path, "C:\\");
                //}
                //else {
                //    alert(msgNoPath);
                //}

            } else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }

        }
        
        function onGetSettingFailed(result) {
            //endWaitingCoverDiv();
            //ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }
        
        function onGetCommSucceed(result) {
            endWaitingCoverDiv();

            if (result == null) {
                setobjMSCommParaForLights();
            }
            else if (result[0] == SUCCESSRET) {

                var objMSCommSet = document.getElementById("objMSComm");

                try {

                    objMSCommSet.CommPort = result[1];
                    objMSCommSet.Settings = result[2];
                    objMSCommSet.RThreshold = result[3];
                    objMSCommSet.SThreshold = result[4];
                    objMSCommSet.Handshaking = result[5];
                    objMSCommSet.PortOpen = true;

                } catch (e) {
                    alert(e.description);
                }

            }
            else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }
            //initPage();
            //var objMSComm = document.getElementById("objMSComm");
            //ShowInfo("CommPort,Settings,RThreshold,SThreshold,Handshaking,PortOpen  is: " + objMSComm.CommPort + " , " + objMSComm.Settings + " , " + objMSComm.RThreshold + " , " + objMSComm.SThreshold + " , " + objMSComm.Handshaking + " , " + objMSComm.PortOpen);     
   
            callNextInput();

        }

        function onGetCommFailed(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

        function initPage() {

            tbl = "<%=gd.ClientID %>";
            
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblNeedMenu.ClientID %>"), "");
            document.getElementById("<%=QueryChk.ClientID%>").checked = false;
            showNeedMenu(false);
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setRowSelectedOrNotSelectedByIndex(0, false, tbl);   
            customerSN = "";
            productID = "";
            model = "";
            wwanCheck = false;
            hitachiCheck = false;
            cardSNCheck = false;
            endInputPn = false;
            needReset = false;
        }
        function updateTable(data) {
            var gdObj = document.getElementById(tbl);
            var correct = false;
             for (var i = 1; i < gdObj.rows.length; i++) {
                if (data.trim() !== "" && gdObj.rows[i].cells[1].innerText.toUpperCase() == data) {
                    gdObj.rows[i].cells[2].innerText = data;
                    setRowSelectedOrNotSelectedByIndex(i - 1, true, tbl);
                    correct = true;
                    break;
                }
            }
            return correct;
        }
        function checkFinished() {
            var finish = true;
            var gdObj = document.getElementById(tbl);
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[1].innerText != "" && gdObj.rows[i].cells[2].innerText == "")
                { finish = false; break; }
            }
            return finish;
        }
        
        
        function bindTableCheck(data) {
            endInputPn = true;
            var gdObj = document.getElementById(tbl);
            var result = false;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (data.trim() !== "" && gdObj.rows[i].cells[1].innerText.toUpperCase() == data) {
                    gdObj.rows[i].cells[2].innerText = data;
                    setRowSelectedOrNotSelectedByIndex(i - 1, true, tbl);
                  //  gdObj.rows[i].style.backgroundColor = "#A9D956";  
                    break;
                 }
             }
             for (var i = 1; i < gdObj.rows.length; i++) {
                 if (gdObj.rows[i].cells[1].innerText != "" && gdObj.rows[i].cells[2].innerText=="")
                 {endInputPn = false;break;}
             }
             if (endInputPn) {
                 if (wwanCheck) {
                     ShowInfo(msgNeedWWAN);
                     callNextInput();
                 }
                 else if (cardSNCheck) {
                     //ITC-1360-0120
                     ShowInfo(msgNeedCardSN);
                     callNextInput();
                 }
                 else if (hitachiCheck) {
                     ShowInfo(msgNeedHitachi);
                     callNextInput();
                 }
                 else {
                
                     beginWaitingCoverDiv();
                     WebServicePDPALabel02.checkCOA(productID, customerSN, onInputCOASucc, onInputCOAFail);
                 }

             }
             else {
                 ShowMessage("Wrong part no!!");
                 ShowInfo("Wrong part no!!");
             }
        }
        function checkOther(data) {
            if (wwanCheck) {
                beginWaitingCoverDiv();
                WebServicePDPALabel02.InputWWANID(productID, data, onInputWWANSucc, onFail);
                return;
            }

            if (cardSNCheck) {
                if (data.toString().length == 13 && data.toString().substring(0, 3) == "SN*" && data.substring(3, 13) == customerSN) {
                    cardSNCheck = false;
                    if (!hitachiCheck) {
                        beginWaitingCoverDiv();
                        WebServicePDPALabel02.checkCOA(productID, customerSN, onInputCOASucc, onInputCOAFail);
                    }
                    else {
                        ShowInfo(msgNeedHitachi); //请输入HITACHI Check No！
                        callNextInput();
                    }
                }
                else {
                    alert(msgNotSame);
                    callNextInput();
                }
                return;
            }

            if (hitachiCheck) {
                if (data != "460346-292") {
                    ShowInfo(msgHitachiError); //请输入正确的HITACHI Check No！
                    callNextInput();
                }
                else {
                    hitachiCheck = false;
                    beginWaitingCoverDiv();
                    WebServicePDPALabel02.checkCOA(productID, customerSN, onInputCOASucc, onInputCOAFail);
                }
            }
        }
        function ShowOtherCheckInfo() { 
              if (wwanCheck) {
                            ShowInfo(msgNeedWWAN);
                            callNextInput();
                        }
                        else if (cardSNCheck) {
                            //ITC-1360-0120
                            ShowInfo(msgNeedCardSN);
                            callNextInput();
                        }
                        else if (hitachiCheck) {
                            ShowInfo(msgNeedHitachi);
                            callNextInput();
                        }
         }
        function input(data) {

            ShowInfo("");
            var line = getPdLineCmbValue();
            var code = getLabelKittingCodeCmbValue();
            var floor = getLocFloorCmbValue();
                   
            QueryFlag = document.getElementById("<%=QueryChk.ClientID%>").checked;

            if (line == "") {

                alert(msgInputPDLine);
                //ShowInfo(msgInputPDLine);
                callNextInput();
                return;
            }
            
            if (code == "") {
                alert(msgInputCode);
                //ShowInfo(msgInputCode);
                callNextInput();
                return;
            }

            if (floor == "") {
                alert(msgInputFloor);
                //ShowInfo(msgInputFloor);
                callNextInput();
                return;
            }

            if (needReset == true) {
                ShowInfo("");
                ResetPage();
            }
            
            if (customerSN == "") {
                inputCustomerSN(data);
                return;
            }
            if (QueryFlag) {
                callNextInput();
                return;
            }
            if (!noNeedCpuLabel && !checkFinished()) {
                var bCorrect = updateTable(data);
                if (bCorrect) {
                    if (checkFinished()) {
                        if (!wwanCheck && !cardSNCheck && !hitachiCheck) {
                            beginWaitingCoverDiv();
                            WebServicePDPALabel02.checkCOA(productID, customerSN, onInputCOASucc, onInputCOAFail);
                        }
                        else {
                            ShowOtherCheckInfo();
                        }
                    }
                    else {
                        ShowInfo("");
                        callNextInput();
                        return;
                    }
                }
                else {
                    ShowMessage("Wrong part no!!");
                    ShowInfo("Wrong part no!!");
                    callNextInput();
                }
            }
            else {
                checkOther(data);
            }
   
        }

        function inputCustomerSN(inputData) {

            //var regxSN = /^5CG[0-9]{3}[A-Z0-9]{4}$/;
//            var regxSN = /^(5CG[0-9]{3}[A-Z0-9]{4}|8CN[0-9]{3}[A-Z0-9]{4})$/;
//            if (!regxSN.test(inputData)) {
//            alert(msgInputCustSN);
//            ShowInfo(msgInputCustSN);
//            callNextInput();
//            return;
            //            }
            if (!CheckCustomerSN(inputData)) {
                         alert(msgInputCustSN);
                         ShowInfo(msgInputCustSN);
                         callNextInput();
                         return;
            }
            var line = getPdLineCmbValue();
            var code = getLabelKittingCodeCmbValue();
            var floor = getLocFloorCmbValue();

            ShowInfo("");
            beginWaitingCoverDiv();
            WebServicePDPALabel02.InputSNforCQ(QueryFlag, line, code, floor, inputData, editor, stationId, customer, onInputSNSucc, onFail);
            return;
        }

        function onInputSNSucc(result) {

            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                setInfo(result);
                inputFlag = true;
                var plst = result[5];
                setTableForCQ(plst);
                if (QueryFlag) {
                    beginWaitingCoverDiv();
                    WebServicePDPALabel02.checkCOAQuery(productID, onInputCOAQuerySucc, onInputCOAFail);
                    return;
                }
                ShowInfo("");
                wwanCheck = result[2];
                hitachiCheck = result[3];
                cardSNCheck = result[4];

                noNeedCpuLabel = false;
                if (plst.length == 0) {
                    noNeedCpuLabel = true;
                }
                else {
                    ShowInfo("Please scan part no!");
                    callNextInput();
                    return;
                }
                if (wwanCheck) {
                    ShowInfo(msgNeedWWAN);
                    callNextInput();
                }
                else if (cardSNCheck) {
                    //ITC-1360-0120
                    ShowInfo(msgNeedCardSN);
                    callNextInput();
                }
                else if (hitachiCheck) {
                    ShowInfo(msgNeedHitachi);
                    callNextInput();
                }
                else {
                    beginWaitingCoverDiv();
                    WebServicePDPALabel02.checkCOA(productID, customerSN, onInputCOASucc, onInputCOAFail);
                }
            }
            else {
                ShowInfo("");
                var content = result[0];
                ResetPage(); 
                ShowMessage(content);
                ShowInfo(content);
                callNextInput();
            }
        }

        function onFail(result) {
            endWaitingCoverDiv();
            //ResetPage();        
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());           
            callNextInput();
        }
        function setTableForCQ(info) {


            for (var i = 0; i < info.length; i++) {

                var rowArray = new Array();
                var rw;

                rowArray.push(info[i].Tp);
                rowArray.push(info[i].PartNo);
                rowArray.push("");

                //add data to table
                if (i < 4) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
              //      setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
            }
          //  setSrollByIndex(0, true, tbl);
        }
        function onInputWWANSucc(result) {
            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                inputFlag = true;
                wwanCheck = false;
                
                if (cardSNCheck) {
                    //ITC-1360-0120
                    ShowInfo(msgNeedCardSN);
                    callNextInput();
                }
                else if(hitachiCheck){
                    ShowInfo(msgNeedHitachi);
                    callNextInput();
                }
                else {

                    beginWaitingCoverDiv();
                    WebServicePDPALabel02.checkCOA(productID, customerSN, onInputCOASucc, onInputCOAFail);         
                }
            }
            else {
                ShowInfo("");
                var content = result[0];
                ResetPage();
                ShowMessage(content);
                ShowInfo(content);
                callNextInput();              
            }
        }
        
        function onInputCOAFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());           
            callNextInput();
        }
        
        function onInputCOAQuerySucc(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            if (result[0] == SUCCESSRET) {
             //   setTable(result);
                // For Light using              
                //  turnOnLight(result);
                ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
                ExitPage();
                needReset = true;
                document.getElementById("<%=QueryChk.ClientID%>").checked = false;
                callNextInput();
            }
        }
        
        function onInputCOASucc(result) {
            endWaitingCoverDiv();
            ShowInfo("");
            var ret;
            if (result[0] == SUCCESSRET) {
                var pdffalg = result[5];
                templateName = result[6];
                var needprintastmessage = result[7];
                if (needprintastmessage != "") {
                    ShowMessage("Need Print " + needprintastmessage,false);
                }
                
              //  setTable(result);
          //      turnOnLight(result);
                if (pdffalg != "") {
                    if (templateName == "") {
                        alert(msgNoTemp);
                        ResetPage();
                        callNextInput();
                        return;
                    }
                    ShowInfo(msgCreatePDF);
                    window.setTimeout(function() { startCreatePDF(result); }, 50);
                    return;
                    
                }
                else {
                    //ResetPage();
                    needReset = true;
					if (noNeedCpuLabel)
						ShowSuccessfulInfoFormatDetail(true, "", "Customer SN", customerSN, msgNoNeedCpuLabel);
					else
						ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
                }
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
            callNextInput();
        }
        
        function startCreatePDF(result) {
            var ret;
            ret = CallEDITSFunc(result);
            if (ret) {
                ret = CallPdfCreateFunc(result);
            }

            if (ret) {
                //ResetPage();
                needReset = true;
                if (noNeedCpuLabel)
					ShowSuccessfulInfoFormatDetail(true, "", "Customer SN", customerSN, msgNoNeedCpuLabel);
				else
					ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
            }
            else {
                alert(msgErrCreatePDF);
                ResetPage();
                ShowInfo(msgErrCreatePDF);
            }
            callNextInput();
        }
        
         function setInfo(info) {
            //set value to the label
            productID = info[1]["ProductID"];
            customerSN = info[1]["CustSN"];
            model = info[1]["Model"];
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), customerSN);
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), model);
           
        }

        function setTable(info) {

            var wipbufferList = info[1];
            if (wipbufferList.length > 0) {
                PlaySound();
            }
            for (var i = 0; i < wipbufferList.length; i++) {

                var rowArray = new Array();
                var rw;

                rowArray.push(wipbufferList[i]["PartNo"]);
                rowArray.push(wipbufferList[i]["Tp"]);
                rowArray.push(wipbufferList[i]["Qty"]);

                //add data to table
                if (i < 8) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
            }
            setSrollByIndex(0, true, tbl);  
        }
        
        function inputSucc(result) {
            setInfo(result);
            inputFlag = true;
            endWaitingCoverDiv();
           

            getAvailableData("input");
            inputObj.focus();

        }

        function showNeedMenu(showFlag) {
            if (!showFlag){
                setInputOrSpanValue(document.getElementById("<%=lblNeedMenu.ClientID %>"), "");     
            }
            else {
                setInputOrSpanValue(document.getElementById("<%=lblNeedMenu.ClientID %>"), msgNeedMenu);  
            }
            
        }
        function OnCancel() {
            if (!(productID == "")) {
                WebServicePDPALabel02.cancel(productID);
            }
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        //set commport For Led
        function setobjMSCommParaForLights() {
            var objMSComm = document.getElementById("objMSComm");

            if (objMSComm.CommPort != 1) {
                if (objMSComm.PortOpen) {
                    objMSComm.PortOpen = false;
                }
                objMSComm.CommPort = 1;
            }

            objMSComm.Settings = "9600,n,8,1";
            objMSComm.RThreshold = 1;
            objMSComm.SThreshold = 1;
            objMSComm.Handshaking = 0;

            try {
                if (!objMSComm.PortOpen)
                    objMSComm.PortOpen = true;
            } catch (e) {
                alert(e.description);
            }
        }
        
        function turnOnLight(info) {
            var labelList = info[1];
            var objMSComm = document.getElementById("objMSComm");
            var lightArray = new Array();


            for (var i = 0; i < 192; i++) {
                lightArray.push("0");
            }

            for (var i = 0; i < labelList.length; i++) {
                //alert("LightNo is " + labelList[i]["LightNo"]);
                var lightno = labelList[i]["LightNo"] - 1;
                lightArray[lightno] = "1";
            }

            var lightChar = "";
            for (var i = 0; i < 24; i++) {
                var temp = "";
                for (var j = 7; j > -1; j--) {

                    temp = temp + lightArray[i * 8 + j];
                }
                //alert("temp is" + temp + " parseInt(temp, 2) is " + parseInt(temp, 2));
                lightChar += String.fromCharCode(parseInt(temp, 2));
            }

            objMSComm.Output = lightChar;

        }

        function clkSetting() {
            //var code = getLabelKittingCodeCmbValue();
            //stationId="92";
            //PCode="OPPA006"
            showPrintSetting(stationId, pCode);
        }

       
        function CallEDITSFunc(result) {

            var line = getPdLineCmbValue();
            var Paralist = new EDITSFuncParameters();
            var filepath ="";
            var devno = result[2];
            //XmlFilename =  DeliveryNo&"-"&CPQ&"-[BoxShipLabel].xml"
            var filename = devno + "-" + customerSN + "-[BoxShipLabel].xml"
            
            if (GetDebugMode()) {
                //Debug Mode get Root path from Web.conf
                xmlFilePath = GetCreateXMLfileRootPath();
                webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
                //webEDITSaddr = GetEDITTempSIP(); //Packing List for 出口单位
            }
            if (xmlFilePath =="" || webEDITSaddr =="")
            {
                alert("Path error!");
                return false;
            } 
            filepath = xmlFilePath + "XML\\"+line.substring(0,1)+"\\"+filename;
            CheckMakeDir(filename);

            Paralist.add(1, "FilePH", filepath);
            Paralist.add(2, "Dn", devno);
            Paralist.add(3, "SerialNum", customerSN);

            var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
            return IsSuccess;
        }
        
        function CallPdfCreateFunc(result) {
            var line = getPdLineCmbValue();
            var devno = result[2];
           
            var xmlfilename = devno + "-" + customerSN + "-[BoxShipLabel].xml";
            var xslfilename = devno + "-" + customerSN + "-[BoxShipLabel].xslt";
            var pdffilename = devno + "-" + customerSN + "-[BoxShipLabel].pdf"

            if (xmlFilePath =="" || webEDITSaddr =="")
            {
                alert("Path error!");
                return false;
            }

            var xmlfullpath = xmlFilePath + "XML\\" + line.substring(0, 1) + "\\" + xmlfilename;
            var xslfullpath = tmpFilePath + templateName;
            var pdffullpath = pdfFilePath + "pdf\\" + line.substring(0, 1) + "\\" + pdffilename;
            
            //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
            //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
            //--------------------------------------------------------------
            var islocalCreate = false;
            //var islocalCreate = true;
            //================================================================
            //var IsSuccess = CreatePDFfileAsyn(fopFilePath, xmlfullpath, xslfullpath, pdffullpath, islocalCreate);
            var IsSuccess = CreatePDFfileAsynGenPDF(webEDITSaddr, xmlfullpath, xslfullpath, pdffullpath, islocalCreate);
            return IsSuccess;
        }
        
        function GetEDITSIP() {
            var HPEDITSIP = '<%=ConfigurationManager.AppSettings["HPEDITSIP"].Replace("\\", "\\\\")%>';
            return HPEDITSIP;
        }

        function GetEDITSTempIP() {
            var HPEDITSTempIP = '<%=ConfigurationManager.AppSettings["HPEDITSTEMPIP"].Replace("\\", "\\\\")%>';
            return HPEDITSTempIP;
        }
        function GetFopCommandPathfile() {
            var FopCommandPathfile = '<%=ConfigurationManager.AppSettings["FopCommandPathfile"].Replace("\\", "\\\\")%>';
            return FopCommandPathfile;
        }

        function GetTEMPLATERootPath() {
            var TEMPLATERootPath = '<%=ConfigurationManager.AppSettings["TEMPLATERootPath"].Replace("\\", "\\\\")%>';
            return TEMPLATERootPath;
        }
        function GetCreateXMLfileRootPath() {
            var CreateXMLfileRootPath = '<%=ConfigurationManager.AppSettings["CreateXMLfileRootPath"].Replace("\\", "\\\\")%>';
            return CreateXMLfileRootPath;
        }
        function GetCreatePDFfileRootPath() {
            var CreatePDFfileRootPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileRootPath"].Replace("\\", "\\\\")%>';
            return CreatePDFfileRootPath;
        }
        function GetCreatePDFfileClientPath() {
            var CreatePDFfileClientPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileClientPath"].Replace("\\", "\\\\")%>';
            return CreatePDFfileClientPath;
        }
        function GetDebugMode() {
            var DEBUGmode = '<%=ConfigurationManager.AppSettings["DEBUGmode"]%>';
            if (DEBUGmode == "True")
                return true;
            else
                return false;
        }
        
        function PlaySound() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["DuplicateAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            obj.src = sUrl;
        }

        function PlaySoundClose() {

            var obj = document.getElementById("bsoundInModal");
            obj.src = "";
        }                                                                              
    </script>

</asp:Content>
