﻿
<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TouchGlassTimeCheck.aspx.cs" Inherits="CleanRoom_TouchGlassTimeCheck"
    Title="TouchGlassTimeCheck" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="20%" align="left">
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPdLine ID="cmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="Label7" runat="server" CssClass="iMes_label_13pt" Text="Part Type:"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server" Enabled="true" Width="100"
                            IsPercentage="true" />
                    </td>
               
            </table>
          
            <hr />
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 12%" align="left">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td style="width: 75%">
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" IsCanInputChinese="true" />
                        </td>
                        <td align="right">
                            <input id="btnPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                                align="right" />
                            &nbsp;<input id="btnReprint" type="button" runat="server" class="iMes_button" 
                                onclick="reprint()"  />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
               
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">


        var inputObj;
        var line = "";
        //cn_NotInVendorCode cn_msgInputModel cn_msgNeed9999 message cn_msgDuplicateData msgNotInVendorCode cn_msgInputDev cn_msgInput9999 cn_msgValidModel
      
        var msgInput9999 = '<%=this.GetLocalResourceObject(Pre + "_msgInput9999").ToString() %>';
        var msgValidModel = '<%=this.GetLocalResourceObject(Pre + "_msgValidModel").ToString() %>';
        var msgNeed9999 = '<%=this.GetLocalResourceObject(Pre + "_msgNeed9999").ToString() %>';
        var msgNotInVendorCode = '<%=this.GetLocalResourceObject(Pre + "_msgNotInVendorCode").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';
        var msgInputModel = '<%=this.GetLocalResourceObject(Pre + "_msgInputModel").ToString() %>';
        var msgInputPdLine = '<%=this.GetLocalResourceObject(Pre + "_msgInputPdLine").ToString() %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var inputSn = "";
        var paramArr;
        window.onload = function() {

            inputObj = getCommonInputObject();
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            paramArr = getUrlVars();
            getAvailableData("input");
            callNextInput();
        };

        window.onbeforeunload = function() {

            //OnCancel();
        };
        function input(inputData) {
            ShowInfo("");
            line = getPdLineCmbValue();
            if (line == "") {
                ShowMessage(msgInputPdLine);
                callNextInput();
                return;
            }
            if (getConstValueTypeCmbText() == "") {
                ShowMessage("Please select part type!");
                callNextInput();
                return;
            }
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null)//判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
            {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }


            if (inputData.length == 14) {
                inputSn = inputData;
                PageMethods.CheckSn(inputData, line, paramArr["UserId"], paramArr["Customer"], getConstValueTypeCmbValue(), paramArr["Station"], onCheckSnSucess, onCheckSnError);
                callNextInput();
                return;
            }
            else {
                alert("Wrong CT");
            }
            callNextInput();
        }
       
        function onCheckSnSucess() {
                 Save();   
            }

        
        function onCheckSnError(error) {
            endWaitingCoverDiv();
            try {
                //  initPage();
                //clearTable();
                ShowMessage(error.get_message());
                ShowInfo(error.get_message()); 
            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();
        }
      
        function Save() {
            beginWaitingCoverDiv();
            PageMethods.Save(inputSn, getPdLineCmbValue(), paramArr["UserId"], paramArr["Station"], paramArr["Customer"],
                                           getConstValueTypeCmbText(), getPrintItemCollection(), onSaveSuccess, onSaveFail);
        }
        function onSaveSuccess(result) {
            callNextInput();
            endWaitingCoverDiv();
            setPrintItemListParam(result[1], result[0]);
            printLabels(result[1], false);
            ShowSuccessfulInfo(true, "Success,CT: " + result[0]);
            callNextInput();
        }
        function onSaveFail(error) {
            endWaitingCoverDiv();
            try {
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();
        }
       
        function OnCancel() {
            if (inputSn != "") {
                PageMethods.Cancel(inputSn, onCancelSucess, onError);
            }
        }
        function onCancelSucess() {
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
        function ShowAllMsg(msg) {
            ShowMessage(msg);
            ShowInfo(msg);
        }
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function showPrintSettingDialog() {
            showPrintSetting(paramArr["Station"], paramArr["PCode"]);
        }
        function reprint() {
            var url = "ReprintTouchGlass.aspx?Station=" + stationId + "&PCode=" + paramArr["PCode"] + "&UserId=" + paramArr["UserId"] + "&Customer=" + paramArr["Customer"] + "&AccountId=" + paramArr["AccountId"] + "&Login=" + paramArr["UserId"];
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = paramArr["UserId"];
            paramArray[2] = paramArr["Customer"];
            paramArray[3] = paramArr["Station"]; ;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
        function setPrintItemListParam(backPrintItemList, sn) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@CT";
            valueCollection[0] = generateArray(sn);
            for (var i = 0; i < backPrintItemList.length; i++) {
                backPrintItemList[i].ParameterKeys = keyCollection;
                backPrintItemList[i].ParameterValues = valueCollection;
            }
            //setPrintParam(lstPrtItem, "RCTO_Label", keyCollection, valueCollection);
        }
                                   
    </script>

</asp:Content>


