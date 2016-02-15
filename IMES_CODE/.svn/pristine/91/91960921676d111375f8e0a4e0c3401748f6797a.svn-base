<%--
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CheckCartonCTForRCTO.aspx.cs" Inherits="RCTO_CheckCartonCT" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceCheckCartonCTForRCTO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 100px;" />
                    <col />
                    <col style="width: 300px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        Carton SN:
                    </td>
                    <td>
                        <asp:Label ID="lblCartonSN" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                    </td>
                    <td>
                        MBCT2 Qty in Carton:
                    </td>
                    <td>
                        <asp:Label ID="lblCartonQtyContent" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>
            <hr />
            
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    
                </legend>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td>
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                            GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                            HighLightRowPosition="3" AutoHighlightScrollByValue="True">
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
                            <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        
        var mbctInTable = [];
        var mbctInCache = [];
        var editor;
        var customer;
        var stationId;
        var inputObj;
        var emptyPattern = /^\s*$/;
        
        var cartonSN = "";
        var cartonQty = 0;
        var successQty = 0;
        var stepNow = 1;


        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSN").ToString() %>';
        var msgInputPdLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>'
        //2012-7-17
        var msgWrongCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWrongCode").ToString() %>';
        //2012-7-20
        var msgBadProductID = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgBadProductID").ToString() %>';
        var msgInputProdIdFirst = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputProdIdFirst").ToString() %>';
        //2012-9-11
        var msgInputMBSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBSN").ToString() %>';
        //2012-9-17
        var msgInput9999 = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInput9999").ToString() %>';
        
        window.onload = function() {

            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            //getPdLineCmbObj().selectedIndex = 0;
            initPage();
            callNextInput();

        };

        window.onbeforeunload = function() {

            OnCancel();

        };
        function initPage() {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCartonSN.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblCartonQtyContent.ClientID %>"), "0");
            
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);

            mbctInTable = [];
            mbctInCache = [];

            cartonSN = "";
            cartonQty = 0;
            successQty = 0;

            stepNow = 1
        }

        function input(inputData) {
            if (stepNow == 1) {
                if (inputData.length == 9) {
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    WebServiceCheckCartonCTForRCTO.input(inputData, '', editor, stationId, customer, inputSucc, inputFail);
                }
                else
                    ShowInfo('请输入CartonSN, 需9码');
            }
            else if (stepNow == 2) {
                if (inputData.length == 14) {
                    if (isExistInTable(inputData)) {
                        ShowInfo('此MBCT2 曾經刷入，请继续刷入MBCT2');
                    }
                    else if (isExistInCache(inputData)) {
                        //successQty++;
                        inputMBCT2(inputData);
                        if (successQty == cartonQty) {
                            ShowInfo("");
                            beginWaitingCoverDiv();
                            WebServiceCheckCartonCTForRCTO.save(cartonSN, '', editor, stationId, saveSucc, saveFail);
                        }
                        else {
                            ShowInfo('请继续刷入MBCT2');
                        }
                    }
                    else {
                        ShowInfo('此MBCT2与CartonSN不匹配，请继续刷入MBCT2');
                    }
                }
                else
                    ShowInfo('请刷入MBCT2, 需14码');
            }
            callNextInput();
        }

        function inputSucc(result) {
            endWaitingCoverDiv();
            setInfo(result);
            stepNow = 2;
            ShowInfo('请刷入MBCT2');
          
            callNextInput();
        }

        function inputFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function setInfo(info) {
            cartonSN = info[0];
            cartonQty = info[1];
            successQty = 0;

            setInputOrSpanValue(document.getElementById("<%=lblCartonSN.ClientID %>"), cartonSN);
            setInputOrSpanValue(document.getElementById("<%=lblCartonQtyContent.ClientID %>"), cartonQty);
            mbctInCache = info[2];
        }

        function saveSucc(result) {
            endWaitingCoverDiv();
            var msg = cartonSN + " 匹配成功，请封箱！";
            ResetPage();
            ShowSuccessfulInfoFormat(true, "", msg);
            callNextInput();
        }

        function saveFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            var errmsg = '匹配成功，儲存有错 ! ' + result.get_message();
            ShowMessage(errmsg);
            ShowInfo(errmsg);
            callNextInput();
        }

        function inputMBCT2(data) {
            var rowArray = new Array();
            var rw;

            rowArray.push(data);

            //add data to table
            if (mbctInTable.length < 12) {
                eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, mbctInTable.length + 1);");
                setSrollByIndex(mbctInTable.length, true, tbl);
            }
            else {
                eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                setSrollByIndex(mbctInTable.length, true, tbl);
                rw.cells[1].style.whiteSpace = "nowrap";
            }

            mbctInTable[successQty++] = data;
            callNextInput();
        }

        function isExistInTable(data) {
            if (mbctInTable != undefined && mbctInTable != null) {
                for (var i = 0; i < mbctInTable.length; i++) {
                    if (mbctInTable[i] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function isExistInCache(data) {
            if (mbctInCache != undefined && mbctInCache != null) {
                for (var i = 0; i < mbctInCache.length; i++) {
                    if (mbctInCache[i] == data) {
                        mbctInCache[i] = '';
                        return true;
                    }
                }
            }

            return false;
        }

        function OnCancel() {
            if (cartonSN != "") {
                //WebServiceCheckCartonCTForRCTO.cancel(cartonSN);
            }
        }

        function callNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
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
    </script>
</asp:Content>
