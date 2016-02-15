<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: RCTO HP PN Label
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012-08-02  ITC000052             Create
 Known issues:
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HPPNLabelforRCTO.aspx.cs" Inherits="FA_HPPNLabelforRCTO" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceHPPNLabelforRCTO.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table width="95%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lblHPPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <asp:Label ID="lblInputHPPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <asp:Label ID="lblInputModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 75%" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanel2" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none">
                                </button>
                                <button id="btnGridClear" runat="server" type="button" onclick="" style="display: none">
                                </button>
                                <input id="prodHidden" type="hidden" runat="server" />
                                <input id="sumCountHidden" type="hidden" runat="server" />
                                <input type="hidden" runat="server" id="Hidden1" />
                                <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr style="height: 40px; vertical-align: middle">
                    <td align="left">
                    </td>
                    <td align="right">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnPrintSetting" runat="server" OnClientClick="clkSetting()"
                                    UseSubmitBehavior="false" TabIndex="2" onmouseover="this.className='iMes_button_onmouseover'"
                                    onmouseout="this.className='iMes_button_onmouseout'" />
                                &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="hiddenbtn" runat="server" onserverclick="hiddenbtn_Click" style="display: none">
                                </button>
                                <input type="hidden" runat="server" id="Hidden_ProdID" />
                                <input type="hidden" runat="server" id="station" />
                                <input type="hidden" runat="server" id="pCode" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
        <script type="text/javascript">


            var msgInvalidProdID = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidProdID").ToString() %>';
            var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
            var msgModelNull = '<%=this.GetLocalResourceObject(Pre + "_msgModelNull").ToString() %>';
            var msgProdIDNull = '<%=this.GetLocalResourceObject(Pre + "_msgProdIDNull").ToString() %>';
            var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
            var msgWrongCode = '<%=this.GetLocalResourceObject(Pre + "_mesWrongCode").ToString() %>';

            var strProdID = "";
            var strModelDisplay = "";
            var strCustSN = "";
            var station = "";
            var inputObj = "";
            var inputData = "";
            var uutInput = true;
            var flowflag = false;
            var stationId = "";
            var pCode = "";

            var type = "";

            window.onload = function() {
                stationId = '<%=Request["Station"] %>';
                pCode = '<%=Request["PCode"] %>';
                station = document.getElementById("<%=station.ClientID %>").value.trim();
                inputObj = getCommonInputObject();
                inputData = inputObj.value;
                getAvailableData("input");
                getCommonInputObject().focus();

                ShowInfo("");
            }

            function initFlag() {
                flowflag = false;
            }

            function clkSetting() {
                showPrintSetting(stationId, pCode);
            }

            function TransferData(data) {
                data = data.trim().toUpperCase();
                var datalength = parseInt(data.length);

                if (datalength == 14) {
                    type = "LCM CT";
                    return data;
                }
                else if (datalength == 9 || datalength == 10) {
                    type = "ProductID";
                    return data;
                }
                else {
                    type = "";
                    data = "Wrong Code";
                    return data;
                }
                
            }        
                  
            function input(inputData) {
                try{

                        flowflag = true;

                        var tmpdata = TransferData(inputData);
                        if (tmpdata == "Wrong Code") {
                            alert(msgWrongCode);
                            getAvailableData("input");

                        }
                        else 
                        {


                            var lstPrintItem = null;
                            //打印部分
                            
                            try {
                            lstPrintItem = getPrintItemCollection();
                            if (lstPrintItem == null) {
                            alert(msgPrintSettingPara);
                            getAvailableData("input");
                            flowflag = false;
                            // return null;
                            }
                            }
                            catch (e) {
                            alert(e);
                            getAvailableData("input");
                            flowflag = false;
                            }
                            try {
                                if (lstPrintItem != null) {
                                    WebServiceHPPNLabelforRCTO.GetData(tmpdata, "<%=UserId%>", station, "<%=Customer%>", onGetDataSuccess, onGetDataFail);
                               }
                            }
                            catch (e) {
                                alert(e);
                                getAvailableData("input");
                                flowflag = false;
                            }
                        }
                    }
                    catch (e) {
                alert(e.description);
            }
            
            }

            function setPrintPara() {
                var lstPrintItem = null;
                //打印部分
                
                try {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null) {
                        return null;
                    }
                    var keyCollection = new Array();
                    var valueCollection = new Array();

                    keyCollection[0] = "@ProductID";
                    valueCollection[0] = generateArray(strProdID);

                    setPrintParam(lstPrintItem, "HPPN_Label", keyCollection, valueCollection);
                }
                catch (e) {
                    alert(e.description);
                    return null;
                 }
               // lstPrintItem[0].PrintMode = 0; //bat
               // lstPrintItem[0].SpName = "FA_MasterLabe_Print.sql";
                return lstPrintItem;
            }

            function onDisplaySuccess(result) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag()
                try {
                    setInfo(result);

                   // setPrintItemListParam(result[1]);
                    printLabels(result[2], false);
                    //ShowInfo("print success!");
                    //ShowSuccessfulInfo(true);
                    var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
                    //ShowSuccessfulInfo(true, "'" + strProdID + "' " + msgSuccess1);
                    ShowSuccessfulInfoFormat(true, "Product ID", strProdID); // Print 成功，带成功提示音！

                   // setnull();
                   // OnInputClearClick();
                    
                }
                catch (e) {
                    alert(e.description);
                }
                getCommonInputObject().focus();

                getAvailableData("input");

            }
            function setInfo(info) {
                //set value to the label
                setInputOrSpanValue(document.getElementById("<%=lblInputHPPN.ClientID %>"), info[1]);
                setInputOrSpanValue(document.getElementById("<%=lblInputModel.ClientID %>"), info[0]);
            }
            
            function setnull() {
                //set value to the label
                setInputOrSpanValue(document.getElementById("<%=lblInputHPPN.ClientID %>"),"");
                setInputOrSpanValue(document.getElementById("<%=lblInputModel.ClientID %>"), "");
            }       

            function onDisplayFail(error) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag();
                try {
                    //alert(error.get_message());
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());

                }
                catch (e) {
                    alert(e.description);
                }

                OnInputClearClick();
                getAvailableData("input");
            }
            function onGetDataSuccess(result) {
                var lstPrintItem = getPrintItemCollection();

                strProdID = result[0];
                var keyCollection = new Array();
                var valueCollection = new Array();

                keyCollection[0] = "@ProductID";
                valueCollection[0] = generateArray(strProdID);

                setPrintParam(lstPrintItem, "HPPN_Label", keyCollection, valueCollection);
                beginWaitingCoverDiv();
                WebServiceHPPNLabelforRCTO.display(result[0], result[1], result[2], lstPrintItem, "<%=UserId%>", station, "<%=Customer%>", onDisplaySuccess, onDisplayFail);


            }
            
            function onGetDataFail(error) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag();
                try {
                    //alert(error.get_message());
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());

                }
                catch (e) {
                    alert(e.description);
                }

                OnInputClearClick();
                getAvailableData("input");
            }           

            function EnterOrTab(cusnData) {
                if (flowflag) {
                    var inputContent = cusnData.value;

                    if (event.keyCode == 9 || event.keyCode == 13) {
                        inputContent = inputContent.toUpperCase();

                        var pattern = /[^0-9a-zA-Z]?$/;

                        if (pattern.test(inputContent)) {
                            checkCustSn();
                            event.returnValue = false;
                        }
                        else {
                            alert(msgInvalidCustSN);
                            event.returnValue = false;
                        }
                    }
                }
                else {
                    alert(msgProdIDNull);
                    setInputFocus();
                    event.returnValue = false;
                }
            }

            function checkCustSn() {
            }

            function onCheckCustSnSuccess(result) {
                ShowInfo("");
                endWaitingCoverDiv();
                try {
                    if (result == null) {
                        ShowMessage(msgSystemError);
                        ShowInfo(msgSystemError);
                        ClearAndSetFocusCUSTSN();
                    }
                    else if (result == SUCCESSRET) {
                        printFun();
                    }
                    else {
                        ShowMessage(result[0]);
                        ShowInfo(result[0]);
                        ClearAndSetFocusCUSTSN();
                    }

                }
                catch (e) {
                    alert(e.description);
                }
            }

            function onCheckCustSnFail(error) {
                ShowInfo("");
                endWaitingCoverDiv();

                try {
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());
                }
                catch (e) {
                    alert(e.description);
                }

                ClearAndSetFocusCUSTSN();

            }

            function printFun() {
                try {
                    if (flowflag) {
                        var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

                        if (lstPrintItem == "" || lstPrintItem == null) {
                            alert(msgPrintSettingPara);
                            ClearAndSetFocusCUSTSN();
                            return;
                        }

                        beginWaitingCoverDiv();
                    }
                }
                catch (e) {
                    alert(e);
                }
            }

            function ClearAndSetFocusCUSTSN() {
            }

            function generateArray(val) {
                var ret = new Array();
                ret[0] = val;
                return ret;
            }

            function setPrintItemListParam(backPrintItemList) {

                // @prdid --Product ID
                //============================================generate PrintItem List==========================================
                var lstPrtItem = backPrintItemList;
                var keyCollection = new Array();
                var valueCollection = new Array();

                keyCollection[0] = "@ProductID";

                valueCollection[0] = generateArray(strProdID);


                /*
                * Function Name: setPrintParam
                * @param: printItemCollection
                * @param: labelType
                * @param: keyCollection(Client: Array of string.    Server: List<string>)
                * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
                */

                setPrintParam(lstPrtItem, "HPPN_Label", keyCollection, valueCollection);
            }


            function onPrintSuccess(result) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag();
                try {
                    if (result == null) {
                        ShowMessage(msgSystemError);
                        ShowInfo(msgSystemError);
                    }
                    else if (result[0] == SUCCESSRET) {
                        setPrintItemListParam(result[1]);

                        printLabels(result[1], false);

                        //==========================================end print process===================================

                        ShowSuccessfulInfo(true);
                    }
                    else {
                        ShowMessage(result[0]);
                        ShowInfo(result[0]);
                    }

                }
                catch (e) {
                    alert(e.description);
                }

                OnClearAll();

            }

            function onPrintFail(error) {
                ShowInfo("");
                endWaitingCoverDiv();
                initFlag();
                try {
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());
                }
                catch (e) {

                    alert(e.description);
                }

                OnClearAll();
            }


            function OnClearAll() {
                getCommonInputObject().value = "";
                inputData = "";
                strProdID = "";
                strModelDisplay = "";

                setInputFocus();
            }

            function OnInputClearClick() {
                getCommonInputObject().value = "";
                inputData = "";
                strProdID = "";
                strModelDisplay = "";
                setInputFocus();
            }

            function setInputFocus() {
                getCommonInputObject().focus();
            }



            function showPrintSettingDialog() {
                showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
            }

            //*********************************************************************************
            //| Function	:	Exit Page 
            //| Author		:	Chen Xu
            //| Create Date	:	01/20/2010
            //| Description	:	Clear the UI Session
            //| Input para.	:	MBSNo, station
            //| Ret value	:	
            //*********************************************************************************

            window.onbeforeunload = function() {
                ExitPage();
            }

            function ExitPage() {
                if (flowflag) {
                    WebServiceHPPNLabelforRCTO.Cancel(strProdID, onClearSucceeded, onClearFailed);
                    sleep(waitTimeForClear);
                }
            }

            function onClearSucceeded(result) {
                ShowInfo("");
                try {
                    if (result == null) {
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
                OnClearAll();

            }

            function onClearFailed(error) {
                ShowInfo("");
                try {
                    ShowMessage(error.get_message());
                    ShowInfo(error.get_message());
                }
                catch (e) {
                    alert(e.description);
                }

                OnClearAll();
            }

            function ResetPage() {
                ExitPage();
                OnClearAll();
                ShowInfo("");
                document.getElementById("<%=hiddenbtn.ClientID %>").click();
            }

        </script>
</asp:Content>
