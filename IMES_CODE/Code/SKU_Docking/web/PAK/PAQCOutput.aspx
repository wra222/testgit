<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* ITC-1360-0066 设置PdLine默认选中噢乖你第一个item
* ITC-1360-0067 设置check9999初始值为未选择
* ITC-1360-0072 ResetPage时错误重置FailQty,修改之
* ITC-1360-0074 ResetPage时错误重置PassQty,修改之
* ITC-1360-0073 ResetPage是pdline初始状态
* ITC-1360-0532 ResetPage时不初始化scan
* ITC-1360-0525 默认初始值为第一行
* ITC-1360-0526 调整布局
* ITC-1360-0531 去掉reset中对于scan pdline的操作
* ITC-1360-1185 增加流程完毕后成功提示
* ITC-1360-1202 初始化页面设置焦点
* ITC-1360-1519 替换cmbPdline为只读txtbox显示
* ITC-1360-1520 同1519
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PAQCOutput.aspx.cs" Inherits="PAK_PAQCOutput" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PAQCOutputService.asmx" />
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
                        <asp:TextBox ID="txtPdLine" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="95%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblOKQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOKQtyContent" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNGQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNGQtyContent" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>
            <hr />
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 110px;" />
                        <col />
                        <col style="width: 110px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblCustSNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIDContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
             <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                        <td style="width: 150px;">
                            <asp:CheckBox ID="ScanChk" runat="server" CssClass="iMes_CheckBox" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var editor;
        var customer;
        var stationId;
        var inputObj;
        var emptyPattern = /^\s*$/;

        var productID = "";
        var customerSN = "";

        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSN").ToString() %>';
        var msgInputPdLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>'
        
        window.onload = function() {

            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            document.getElementById("<%=ScanChk.ClientID%>").checked = false;
            //getPdLineCmbObj().selectedIndex = 0;
            initPage();
            callNextInput();

        };

        window.onbeforeunload = function() {

            OnCancel();

        };
        function initPage() {
            tbl = "<%=gd.ClientID %>";           
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblOKQtyContent.ClientID %>"), passQty);
            setInputOrSpanValue(document.getElementById("<%=lblNGQtyContent.ClientID %>"), failQty);
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            productID = "";
            customerSN = "";
        }


        function input(inputData) {

            scanFlag = !document.getElementById("<%=ScanChk.ClientID%>").checked;
            inputData = Get2DCodeCustSN(inputData);
            if (inputData == "7777") {
                ResetPage();
            }
            else if (inputData == "9999" && scanFlag && productID != "") {
            
                ShowInfo("");
                save();
            }
           
            else if (productID == "") {

                var line = document.getElementById("<%=txtPdLine.ClientID%>").innerText; //getPdLineCmbValue();
                /*if (line == "") {
                    alert(msgInputPdLine);
                    //ShowInfo(msgInputPdLine);
                    callNextInput();
                    return;
                }*/
                if (inputData.length == 10) {
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    customerSN = inputData;
                    //customerSN = "1010000010000001"; //"1034000034";
                    PAQCOutputService.input(line, customerSN, editor, stationId, customer, inputSucc, inputFail);
                }
                else {
                    alert(msgInputSN);
                    //ShowInfo(msgInputSN);
                    callNextInput();
                }

            }
            else {
                inputDefect(inputData);
            }
        }

        function inputSucc(result) {

            endWaitingCoverDiv();
            setInfo(result);
           
            if (!scanFlag) {
                ShowInfo("");
                save();
            }
            else {
                callNextInput();
            }
        }

        function inputFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();            
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }

            return false;
        }

        function setInfo(info) {
            //set value to the label
            productID = info[0]["ProductID"];
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), info[0]["CustSN"]);
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[0]["Model"]);
            setInputOrSpanValue(document.getElementById("<%=txtPdLine.ClientID %>"), info[2]);
            //set defectCache value
            defectCache = info[1];
        }

        function save() {
            beginWaitingCoverDiv();
            var line = document.getElementById("<%=txtPdLine.ClientID%>").value; //getPdLineCmbValue();
            var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;

            PAQCOutputService.save(prodId,line,editor, defectInTable, saveSucc, saveFail);
        }

        function saveSucc(result) {

            endWaitingCoverDiv();
            //ShowSuccessfulInfo(true);

            if (isPass()) {
                passQty++;
                setInputOrSpanValue(document.getElementById("<%=lblOKQtyContent.ClientID %>"), passQty);
            }
            else {
                failQty++;
                setInputOrSpanValue(document.getElementById("<%=lblNGQtyContent.ClientID %>"), failQty);
            }

            var tmpinfo = customerSN;
            ResetPage();
            ShowSuccessfulInfoFormat(true, "Customer SN", tmpinfo);
            callNextInput();
        }


        function saveFail(result) {
            endWaitingCoverDiv();            
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function inputDefect(data) {

            if (isExistInTable(data)) {
                //error message
                alert(msgDuplicateData);
                callNextInput();
                return;
            }

            if (isExistInCache(data)) {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;

                rowArray.push(data);
                rowArray.push(desc);

                //add data to table
                if (defectInTable.length < 12) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }

                defectInTable[defectCount++] = data;
                callNextInput();
            }
            else {
                alert(msgInputValidDefect);
                callNextInput();
            }
        }

        function isExistInTable(data) {
            if (defectInTable != undefined && defectInTable != null) {
                for (var i = 0; i < defectInTable.length; i++) {
                    if (defectInTable[i] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function isExistInCache(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function getDesc(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return defectCache[i]["description"];
                    }
                }
            }

            return "";
        }

        function OnCancel() {
            if (productID != "") {
                PAQCOutputService.cancel(productID);
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
