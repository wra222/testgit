<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PackingPizza page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-07  zhu lei               Create          
 * Known issues:
 * TODO:
 * getbom,partmatch等共通还未完成，尚未调试，需整合阶段再行调试
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PackingPizza.aspx.cs" Inherits="PAK_PackingPizza" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PackingPizzaWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 100px;"/>
                    <col />
                    <col style="width: 100px;"/>
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModelContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>  
                    <td>
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                            GvExtWidth="100%" GvExtHeight="220px" style="top: 0px; left: 0px" Width="99.9%" Height="210px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt> 
                    </td>
                </tr> 
             </table>
        </div>
        <div id="div3">
             <table width="100%">
                <tr>
                    <td style="width: 120px;">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
                    </td>
                </tr>
             </table>            
        </div> 
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>       
    </div>    
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var checkPicdFlag = false;
        var checkWwanFlag = false;
        var editor;
        var partCache;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var modelId = "";
        var customer;
        var stationId;
        var inputObj;
        var partmachQty = 0;
        var totalQty = 0;

        //error message


        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = "MP"; //'<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj.focus();
        };
        function ExitPage() {
            if (document.getElementById("<%=lblModelContent.ClientID%>").innerText != "") {

                PackingPizzaWebService.Cancel(document.getElementById("<%=lblModelContent.ClientID%>").innerText, onClearSucceeded, onClearFailed);
                sleep(waitTimeForClear);
            }
        }
        window.onbeforeunload = function() {
            ExitPage();

        }

        function input(data) {
            if (inputFlag) {
                if (data == "7777") {
                    ShowInfo("");
                    //clear table action
                    initPage();
                }
                else {
                    ShowInfo("");
                    //input part info
                    inputPartNo(data);
                }
            }
            else {
                if (data != "7777") {
                    ShowInfo("");
                    //beginWaitingCoverDiv();
                    modelId = SubStringSN(data, "Model");
                    PackingPizzaWebService.InputUUT(data, "", editor, stationId, customer, inputSucc, inputFail);
                }
                else {
                    getAvailableData("input");
                }
            }
        }

        function inputSucc(result) {
            setInfo(modelId, result);
            inputFlag = true;
            getAvailableData("input");
            inputObj.focus();
        }

        function inputFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function setInfo(modelId, info) {
            //init global variables
            partmachQty = totalQty = 0;
            for (var i in info)
                totalQty += info[i]['qty'];
            partCache = info;

            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), modelId);
            setInputOrSpanValue(document.getElementById("<%=lblQtyContent.ClientID %>"), partmachQty);

            //set value to the table
            fillTable(partCache);
        }

        function inputPartNo(data) {
            if (isExistInCache) {
                PackingPizzaWebService.CheckPartAndItem(modelId, data, checkSucc, checkFail);
            }
        }

        function checkSucc(result) {
            var matched = false;
            if (partCache != undefined && partCache != null) {
                for (var i = 0; i < partCache.length; i++) {
                    var bominfo = partCache[i];
                    for (var p in bominfo.parts) {
                        if (bominfo.parts[p].id != result.PNOrItemName)
                            continue;
                         
                        // Update PQty
                        document.getElementById("<%=gd.ClientID %>").rows[i + 1].cells[4].innerText = (++bominfo['scannedQty']).toString();

                        bominfo.collectionData.push({
                            sn: result.CollectionData,
                            pn: result.PNOrItemName,
                            valueType: result.ValueType
                        });

                        var coll = bominfo['collectionData'], cd = '';
                        for (var c in coll) {
                            if (cd) cd += ',';
                            cd += coll[c]['sn'];
                        }
                        // Update Collection data
                        document.getElementById("<%=gd.ClientID %>").rows[i + 1].cells[5].innerText = cd;

                        // Update Part Matched Qty
                        partmachQty++;
                        setInputOrSpanValue(document.getElementById("<%=lblQtyContent.ClientID %>"), partmachQty);
                        
                        matched = true;
                        break;
                    }

                    if (matched)
                        break;
                }
            }
            
            if (partmachQty == totalQty) {
                save();
            }
            getAvailableData("input");
            inputObj.focus();
        }

        function checkFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function isExistInCache(data) {
            if (partCache != undefined && partCache != null) {
                for (var i = 0; i < partCache.length; i++) {
                    if (partCache[i][0] == data && partCache[i][6] == false) {
                        return true;
                    }
                }
            }

            return false;
        }
        
        function save() {
            beginWaitingCoverDiv();
            //var model = document.getElementById("<%=lblModelContent.ClientID %>").innerText;
            var lstPrintItem = getPrintItemCollection();
            
            //打印方法
            if (lstPrintItem == null) {
                endWaitingCoverDiv();
                alert(msgPrintSettingPara);
                OnCancel();
                return;
            }

            PackingPizzaWebService.Save(modelId, lstPrintItem, saveSucc, saveFail);
        }

        function saveSucc(result) {
            //show success message
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@Kit ID";

            valueCollection[0] = generateArray(result[0]);

            setPrintParam(result[1], "Picking Pizza label", keyCollection, valueCollection);

            printLabels(result[1], false);
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + result[0] + "] " + msgSuccess;
            ShowSuccessfulInfo(true, message);
            
            //initPage
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }

        function saveFail(result) {
            //show error message
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
            inputObj.focus();
        }

        function initPage() {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblQtyContent.ClientID %>"), "0");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0, false);
            inputFlag = false;
            totalQty = 0;
            partmachQty = 0;
        }

        function fillTable(partCache) {
            if (partCache != undefined && partCache != null) {
                for (var i = 0; i < partCache.length; i++) {
                    var bominfo = partCache[i];
                    
                    var rowArray = new Array();
                    var rw;
                    rowArray.push(bominfo['PartNoItem']);
                    rowArray.push(bominfo['type']);
                    rowArray.push(bominfo['description']);
                    rowArray.push(bominfo['qty']);
                    rowArray.push(bominfo['scannedQty']);

                    var coll = bominfo['collectionData'], cd = '';
                    for (var c in coll) {
                        if (cd) cd += ',';
                        cd += coll[c]['sn'];
                    }
                    rowArray.push(cd);
                    
                    //add data to table
                    if (i < 6) {
                        eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i + 1);");
                    }
                    else {
                        eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                        rw.cells[1].style.whiteSpace = "nowrap";
                    }
                    setSrollByIndex(i, false);
                }
            }
        }

        window.onbeforeunload = function() {
            if (inputFlag) {
                OnCancel();
            }
        };

        function OnCancel() {
            PackingPizzaWebService.Cancel(modelId);
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
        }                  
        function clkSetting()
        {
            showPrintSetting(stationId, pCode);
        }
    </script>
</asp:Content>

