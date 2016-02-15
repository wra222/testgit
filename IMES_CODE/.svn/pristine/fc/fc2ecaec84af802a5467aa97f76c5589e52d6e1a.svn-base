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
* ITC-1360-0173 给CmbStation添加默认空项
* ITC-1360-0174 给msgInputSno添加正确文字资源
* ITC-1360-0175 Rework Station选择项错误
* ITC-1360-0200 调整成功过站提示信息显示顺序
* ITC-1360-0219 输入9999后，如果Station & List均为空，增加提示
* ITC-1360-0220 输入9999后增加对已选station的处理
* ITC-1360-0221 去掉Message提示
* ITC-1360-0350 恢复部分message信息
* ITC-1360-0351 增加提示保护条件
* ITC-1360-0352 页面初始化时设置焦点
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BGAOutPut.aspx.cs" Inherits="FA_BGAOutput" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServiceBGAOutput.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%">
                <tr>
                    <td td width="25%">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                   
                </tr>
                <tr>
                    <td td width="15%">
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMBSnoContent" runat="server" CssClass="iMes_label_11pt" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="15%" >
                        <asp:Label runat="server" ID="lblStation" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <iMES:CmbPalletType ID="CmbStation" runat="server" Width="100" IsPercentage="true" />
                    </td>
                    <td>
                        <button id="btnAddNew" runat="server" onclick="clkAddNew()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                </tr>               
            </table>
            <hr />
            
            
            <asp:Label ID="lblRepairList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                
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
           
            
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var saveFlag = false;
        
        var reworkCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var newReworkCount = 0;
        var newReworkList = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var editor = "";
        var customer = "";
        var station = "";
        var inputObj;
        var emptyPattern = /^\s*$/;

        var MBSno = "";
        

        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';

        var msgInputPdLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine")%>'
        var msgInputStation = '<%=this.GetLocalResourceObject(Pre + "_msgInputStation").ToString() %>'
        var msgInputSno = '<%=this.GetLocalResourceObject(Pre + "_msgInputSno").ToString()%>'
        
        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            station = '<%=Request["Station"] %>';
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            initPage();
            callNextInput();
        };

        window.onbeforeunload = function() {

            OnCancel();

        };
        function initPage() {
            tbl = "<%=gd.ClientID %>";
            getPalletTypeCmbObj().selectedIndex = 0;
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            saveFlag = false;
            newReworkCount = 0;
            newReworkList = [];
            MBSno = "";            
                                  
        }

        function input(inputData) {

            if (inputData == "") {
                alert(msgInputSno);
                //ShowInfo(msgInputSno);
                callNextInput();
                return;
            }
            
            if (MBSno == "") {
               ShowInfo("");
               beginWaitingCoverDiv();
               //inputData = inputData.substring(0, 10);
               inputData = SubStringSN(inputData.toString(), "MB");
               WebServiceBGAOutput.inputSno(inputData, editor, station, customer, inputSnoSucc, inputFail);
               return;
            }
            if (inputData == "9999") {
                var reworkstation = getPalletTypeCmbValue();
                if (reworkstation == "") {
                    if (reworkCache.length <= 0) {
                        //ITC-1360-0219
                        alert(msgInputStation);
                        callNextInput();
                    }
                    else {
                        inputSave();
                    }
                }
                else {
                    saveFlag = true;
                    clkAddNew();
                }
            }
        }

        function inputSnoSucc(result) {

            endWaitingCoverDiv();
            setInfo(result);
            callNextInput();
            
        }

        function inputFail(result) {

            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ResetPage();
            //ITC-1360-0350
            ShowInfo(result.get_message());
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }

            return false;
        }

        function setInfo(info) {
            //set value to the label
            MBSno = info[1];
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), MBSno);
            setTable(info)
        }
        
        function setTable(info) {

            var repairList = info[2];
            reworkCache = info[2]
            
            for (var i = 0; i < repairList.length; i++) {
                var rowArray = new Array();
                var rw;

                rowArray.push(repairList[i]["ReworkCode"]);
                rowArray.push(repairList[i]["Cdt"]);
                rowArray.push(repairList[i]["Udt"]);

                //add data to tablef
                if (i < 12) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
            }
            
        }
        
        function isExistInCache(data) {
            if (reworkCache != undefined && reworkCache != null) {
                for (var i = 0; i < reworkCache.length; i++) {
                    if (reworkCache[i]["ReworkCode"] == data) {
                        return true;
                    }
                }
            }
            
            return false;
        }

        function clkAddNew() {

            if (MBSno == "") {
                alert(msgInputSno);
                //ITC-1360-0221
                //ShowInfo(msgInputSno);
                callNextInput();
                return;
            }
            
            var reworkstation = getPalletTypeCmbValue();
            if (reworkstation == "") {
                alert(msgInputStation);
                //ShowInfo(msgInputStation);
                callNextInput();
                return;
            }

            if (isExistInCache(reworkstation)) {
                if (!saveFlag) {
                    alert(msgDuplicateData);
                    //DEBUG ITC-1360-0221
                    //ShowInfo(msgDuplicateData);
                }
                else {
                    inputSave();
                    return;
                }
            }
            else {
                beginWaitingCoverDiv();
                WebServiceBGAOutput.addNew(MBSno, reworkstation,onAddSucc, onAddFail);
            }
            callNextInput();
        }

        function onAddSucc(result) {

            endWaitingCoverDiv();
            
            var rowArray = new Array();
            var rw;

            rowArray.push(result[1]["ReworkCode"]);
            rowArray.push(result[1]["Cdt"]);
            rowArray.push(result[1]["Udt"]);

            //add data to table
            if (reworkCache.length < 12) {
                eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, reworkCache.length + 1);");
                setSrollByIndex(reworkCache.length, true, tbl);
            }
            else {
                eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                setSrollByIndex(reworkCache.length, true, tbl);
                rw.cells[1].style.whiteSpace = "nowrap";
            }
            
                       
            //newReworkCount++;
            //newReworkList.push(newitem); //reworkstation);
            reworkCache.push(result[1]);
            if (!saveFlag) {
                callNextInput();
            }
            else {
                inputSave();
            }
        }

        function onAddFail(result) {

            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }
        
        function inputSave() {
            beginWaitingCoverDiv();
            WebServiceBGAOutput.save(MBSno, inputSaveSucc, inputSaveFail);
        }

        function inputSaveSucc(result) {
            endWaitingCoverDiv();
            var tmpinfo = MBSno;
            ResetPage();
            ShowSuccessfulInfoFormat(true, "MB Sno", tmpinfo);
        }


        function inputSaveFail(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ResetPage();
            ShowInfo(result.get_message());
        }

        function OnCancel() {
            if (MBSno != "") {
                WebServiceBGAOutput.cancel(MBSno);
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
