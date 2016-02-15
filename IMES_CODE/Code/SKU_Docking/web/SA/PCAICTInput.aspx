﻿<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PCAICTInput page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create          
 * 2012-03-19  Li.Ming-Jun           ITC-1360-1508
 * Known issues:
 * TODO:
 * checkitems 中的WWANcheck未完成，由于与BOM有关还没有最终确定所以暂时缺少
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PCAICTInput.aspx.cs" Inherits="SA_PCAICTInput" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServicePCAICTInput.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 15%;" />
                    <col />
                    <col />
                    <col />
                    <col style="width: 15%;" />
                </colgroup>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" Stage="SA" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <iMES:CmbDCodeType ID="cmbDataCode" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblECRContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblQtyContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMBSnoContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPCBVersion" runat="server" CssClass="iMes_label_13pt">PCB Version:</asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPCBVersionContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <input id="btnClearQty" type="button" runat="server" class="iMes_button" onclick="clkClearQty()" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAoiNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAoiNoContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <input type="checkbox" id="chkInputAoi" runat="server" /><asp:Label ID="lblInputAoi"
                            runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <input id="btnPrintSet" type="button" runat="server" class="iMes_button" onclick="clkSetting()" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBCCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBCCodeContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPilotMo" runat="server" CssClass="iMes_label_13pt" Text="Pilot Mo:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPilotMoContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <input type="checkbox" id="chkInputPilotMo" runat="server" onclick="CheckPilotCheck()" />
                        <asp:Label ID="lblInputPilotMo" runat="server" CssClass="iMes_label_13pt" Text="Combine Pilot Mo"></asp:Label>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCombinedQty" runat="server" CssClass="iMes_label_13pt" Text="Combined Qty:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCombinedQtyContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCauseDescr" runat="server" CssClass="iMes_label_13pt" Text="Cause Descr:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCauseDescrContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                            GvExtHeight="220px" Style="top: 0px; left: 0px" Width="99.9%" Height="210px"
                            SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div3">
            <table width="100%">
                <colgroup>
                    <col style="width: 15%;" />
                    <col />
                    <col style="width: 20%;" />
                </colgroup>
                <tr>
                    <td>
                        
                    </td>
                    <td>
                      
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkRCTO" runat="server" Checked="false"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk9999" runat="server" Checked="true"></asp:CheckBox>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var defectCount = 0;
        var defectInTable = [];
        var mbSno = "";
        var customer;
        var stationId;
        var pCode;
        var inputObj;
        var inputCode = "";
        var qty;
        var pilotMo = "";

        //error message
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSucc = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var msgInformation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInformation").ToString() %>';
        var msgOutputRepair = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgOutputRepair").ToString() %>';
        var msgReturnRepair = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReturnRepair").ToString() %>';
        var msgInputAQI = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputAQI").ToString() %>';
        var mesNoSelDateCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelectDateCode").ToString()%>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelPdLine").ToString()%>';
        var mesNoInputECR = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputECR").ToString()%>';
        var mesInputError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesInputError").ToString()%>';
        var mesNoInputMBSno = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputMBSno").ToString()%>';
        var mesNoInputAOINo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputAOINo").ToString()%>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
        var mesNoInputBC = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputB/C").ToString()%>';
        var mesBCCodeNoMatch = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesBCCodeNoMatch").ToString()%>';
        var msgInputPilotMo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPilotMo").ToString() %>';
        

        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj.focus();
            document.getElementById("<%=chk9999.ClientID %>").disabled = true;
        };

        function clkClearQty() {
            if (getPdLineCmbValue() == "") {
                checkMBFlg = true;
                alert(mesNoSelPdLine);
                getAvailableData("input");
                setPdLineCmbFocus();
            }
            else {
                qty = document.getElementById("<%=lblQtyContent.ClientID %>").innerText.trim();
                WebServicePCAICTInput.cleatQty(getPdLineCmbValue(), qty, clearSucc, clearFail);
            }
        }

        function clearSucc(result) {
            if (result[0] == SUCCESSRET) {
                setInputOrSpanValue(document.getElementById("<%=lblQtyContent.ClientID %>"), "0");
            }
            else {
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }
            getAvailableData("input");
            inputObj.focus();
        }

        function clearFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function input(data) {
            
            if (inputFlag) {
                if (data == "7777") {
                    ShowInfo("");
                    //clear table action
                    defectCount = 0;
                    defectInTable = [];

                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    setSrollByIndex(0, false);
                    getAvailableData("input");
                }
                else if (data == "9999") {
                    ShowInfo("");
                    //save action
                    beginWaitingCoverDiv();
                    save();
                }
                else {
                    ShowInfo("");
                    inputCode = checkInput(data);
                    if (inputCode == "Defect") {
                        //input defect info
                        inputDefect(data);
                    }
                    else if (inputCode == "AOINo") {
                        setInputOrSpanValue(document.getElementById("<%=lblAoiNoContent.ClientID %>"), data);
                        getAvailableData("input");
                    }
                    //PilotMO
                    else if (inputCode == "PilotMO") {
                        CheckPilotMo(data, document.getElementById("<%=lblMBSnoContent.ClientID %>").innerText.trim());
                    }
                    else {
                        alert(mesInputError);
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }

            }
            else {
                if (data != "9999" && data != "7777") {
                    ShowInfo("");
                    //beginWaitingCoverDiv();
                    //mbSno = SubStringSN(data, "MB");
                    inputCode = checkInput(data);
                    if (inputCode == "ECR") {
                        setInputOrSpanValue(document.getElementById("<%=lblECRContent.ClientID %>"), data);
                        getAvailableData("input");
                    }
                    else if (inputCode == "PCBVer") {
					    data=data.substr(3,3);
                        setInputOrSpanValue(document.getElementById("<%=lblPCBVersionContent.ClientID %>"), data);
                        getAvailableData("input");
                    } 
                    else if (inputCode == "AOINo") {
                        setInputOrSpanValue(document.getElementById("<%=lblAoiNoContent.ClientID %>"), data);
                        getAvailableData("input");
                    }
                    else if (inputCode == "PilotMO") {
                        CheckPilotMo(data, document.getElementById("<%=lblMBSnoContent.ClientID %>").innerText.trim());
                    }
                    else if (inputCode == "MBSno") {
                        var checkMBFlg = false;
                        if (getPdLineCmbValue() == "") {
                            checkMBFlg = true;
                            alert(mesNoSelPdLine);
                            getAvailableData("input");
                            setPdLineCmbFocus();
                            return;
                        }
                        if (getDecodeTypeValue() == "") {
                            checkMBFlg = true;
                            alert(mesNoSelDateCode);
                            getAvailableData("input");
                            setDecodeTypeCmbFocus();
                            return;
                        }
                        if (document.getElementById("<%=lblBCCodeContent.ClientID %>").innerText.trim() == "") {
                            checkMBFlg = true;
                            alert(mesNoInputBC);
                            getAvailableData("input");
                            return;
                        }
                        if (document.getElementById("<%=lblECRContent.ClientID %>").innerText.trim() == "") {
                            checkMBFlg = true;
                            alert(mesNoInputECR);
                            getAvailableData("input");
                            return;
                        }
                        var BCCode = document.getElementById("<%=lblBCCodeContent.ClientID %>").innerText.trim();
                        var MBSnoCode ="";
						if(BCCode.length==2)
						{
						MBSnoCode=data.substr(0, 2);
						}
						else
						{
						MBSnoCode=data.substr(0, 3);
						}
                        if (BCCode != MBSnoCode) {
                            checkMBFlg = true;
                            alert(mesBCCodeNoMatch);
                            getAvailableData("input");
                            return;
                        }
                        
                        if (document.getElementById("<%=lblPCBVersionContent.ClientID %>").innerText.trim() == "") {
                            mbSno = SubStringSN(data, "MB");
                            WebServicePCAICTInput.CheckPCBVer(mbSno, CheckPCBVerSucc, CheckPCBVerFail);
                        }
                        else if (document.getElementById("<%=chkInputPilotMo.ClientID%>").checked && document.getElementById("<%=lblPilotMoContent.ClientID %>").innerText.trim() == "") {
                            alert("Please Input PilotMO");
                            getAvailableData("input");
                            return;
                        }
                        else {
                            mbSno = SubStringSN(data, "MB");
                            var IsInputPilotMo = document.getElementById("<%=chkInputPilotMo.ClientID%>").checked;
                            WebServicePCAICTInput.input(mbSno, document.getElementById("<%=lblECRContent.ClientID %>").innerText.trim(), getDecodeTypeValue(), editor, getPdLineCmbValue(), stationId, customer, document.getElementById("<%=lblPCBVersionContent.ClientID %>").innerText.trim(), document.getElementById("<%=lblModelContent.ClientID %>").innerText.trim(), IsInputPilotMo, inputSucc, inputFail);
                        }


                    }
                    else if (inputCode == "B/CCode") {
                        setInputOrSpanValue(document.getElementById("<%=lblBCCodeContent.ClientID %>"), data);
                        getAvailableData("input");
                    }
                    else {
                        alert(mesInputError);
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }
                else {
                    alert(mesNoInputMBSno);
                    getAvailableData("input");
                    inputObj.focus();
                }
            }
        }

        function checkInput(value) {
            var code = "ERROR";
            try {
                if (value.length == 4) {
                    code = "Defect";
                }
                else if (value.length == 5) {
                    var pattern = /^([A-Z|0-9][0]{2}[A-Z|0-9]{2})$/;
                    if (pattern.test(value)) {
                        code = "ECR";
                    }
                }
                else if (value.length == 6) {
                    var pattern = /^[0-9A-Z]{6}$/;
                    if (pattern.test(value)) {
                        code = "PCBVer";
                    }
                }
                else if (value.length == 10) {
                    var val = value.substr(4, 1);
                    if (val == "M" || val == "B") {
                        code = "MBSno";
                    }
                    else if (val == "A") {
                        if (document.getElementById("<%=lblAoiNoContent.ClientID %>").innerText.trim() == "") {
                            if (document.getElementById("<%=chkInputAoi.ClientID%>").checked) {
                                code = "AOINo";
                            }
                            else {
                                var ret = confirm(msgInputAQI);
                                if (ret) {
                                    document.getElementById("<%=chkInputAoi.ClientID%>").checked = true;
                                    code = "AOINo";
                                }
                            }
                        }
                    }
                }
                else if (value.length == 11) {
                    var checkCode = value.substr(5, 1);
                    if (checkCode == "M" || checkCode == "B") {
                        code = "MBSno";
                       
                    } else {
                        checkCode = value.substr(4, 1);
                        if (checkCode == "M" || checkCode == "B") {
                            code = "MBSno";
                            value = value.substr(0, 10);
                        }
                    }
                }
                else if (value.length == 2||value.length == 3) {
                    code = "B/CCode";
                }
                else if (value.length == 13) {
                    if (document.getElementById("<%=chkInputPilotMo.ClientID%>").checked) {
                        code = "PilotMO";
                    }
                    else {
                        var ret = confirm(msgInputPilotMo);
                        if (ret) {
                            document.getElementById("<%=chkInputPilotMo.ClientID%>").checked = true;
                            code = "PilotMO";
                        }
                    }

                }
                return code;
            }
            catch (e) {
                alert(e.description);
            }
        }

        function CheckPilotCheck() {
            if (!document.getElementById("<%=chkInputPilotMo.ClientID%>").checked) {
                setInputOrSpanValue(document.getElementById("<%=lblPilotMoContent.ClientID %>"), "");
                setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
                setInputOrSpanValue(document.getElementById("<%=lblCombinedQtyContent.ClientID %>"), "");
                setInputOrSpanValue(document.getElementById("<%=lblCauseDescrContent.ClientID %>"), "");
            }
        }

        function CheckPilotMo(pilotMo, mbsn) {
            WebServicePCAICTInput.GetPilotMoInfo(pilotMo, mbsn, checkPilotSucc, checkPilotFail);
        }

        function checkPilotSucc(result) {
            if (result == null) {
                setInputOrSpanValue(document.getElementById("<%=lblPilotMoContent.ClientID %>"), "");
                setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
                setInputOrSpanValue(document.getElementById("<%=lblCombinedQtyContent.ClientID %>"), "");
                setInputOrSpanValue(document.getElementById("<%=lblCauseDescrContent.ClientID %>"), "");
            }
            else {
                setInputOrSpanValue(document.getElementById("<%=lblPilotMoContent.ClientID %>"), result[0]);
                setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), result[1]);
                setInputOrSpanValue(document.getElementById("<%=lblCombinedQtyContent.ClientID %>"), result[2]);
                setInputOrSpanValue(document.getElementById("<%=lblCauseDescrContent.ClientID %>"), result[3]);
            }

            getAvailableData("input");
            inputObj.focus();
        }

        function checkPilotFail(result) {
            //show error message
            ShowMessage(result._message);
            ShowInfo(result._message);
            setInputOrSpanValue(document.getElementById("<%=lblPilotMoContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblCombinedQtyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblCauseDescrContent.ClientID %>"), "");
            getAvailableData("input");
            inputObj.focus();
        }

        function inputSucc(result) {
            if (result[0] == SUCCESSRET) {
                setInfo(mbSno, result);
                inputFlag = true;
                ShowInfo("");
                if (document.getElementById("<%=chk9999.ClientID%>").checked) {
                    beginWaitingCoverDiv();
                    save();
                    return;
                }
            }
            else {
                ShowMessage(result[0], true);
                ShowInfo(result[0]);
            }
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

        function CheckPCBVerSucc(result) {

            if (result[0]=="true") {
                checkMBFlg = true;
                alert("请输入PCB Version！"); //noinputPCBVer
                getAvailableData("input");
                return;
            }
            else {
                if (result[0] == "false") {
                    if (document.getElementById("<%=chkInputPilotMo.ClientID%>").checked && document.getElementById("<%=lblPilotMoContent.ClientID %>").innerText.trim() == "") {
                        alert("Please Input PilotMO");
                        getAvailableData("input");
                        return;
                    }
                    mbSno = SubStringSN(result[1], "MB");
                    var IsInputPilotMo = document.getElementById("<%=chkInputPilotMo.ClientID%>").checked;
                    WebServicePCAICTInput.input(mbSno, document.getElementById("<%=lblECRContent.ClientID %>").innerText.trim(), getDecodeTypeValue(), editor, getPdLineCmbValue(), stationId, customer, document.getElementById("<%=lblPCBVersionContent.ClientID %>").innerText.trim(), document.getElementById("<%=lblModelContent.ClientID %>").innerText.trim(), IsInputPilotMo, inputSucc, inputFail);
                }
            }
           
            getAvailableData("input");
            inputObj.focus();
        }

        function CheckPCBVerFail(result) {
            //show error message
            ShowMessage("刷入的PCB Version 有誤");
            ShowInfo("刷入的PCB Version 有誤");
            getAvailableData("input");
            inputObj.focus();
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }

            return false;
        }

        function setInfo(mb, info) {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), mb);

            //set defectCache value
            defectCache = info[1];
        }

        function save() {
            var checkSaveFlg = false;
            if (getDecodeTypeValue() == "") {
                checkSaveFlg = true;
                alert(mesNoSelDateCode);
                getAvailableData("input");
                setDecodeTypeCmbFocus();
            }
            if (getPdLineCmbValue() == "") {
                checkSaveFlg = true;
                alert(mesNoSelPdLine);
                getAvailableData("input");
                setPdLineCmbFocus();
            }
            if (document.getElementById("<%=lblECRContent.ClientID %>").innerText.trim() == "") {
                checkSaveFlg = true;
                alert(mesNoInputECR);
                getAvailableData("input");
            }
            if (document.getElementById("<%=lblMBSnoContent.ClientID %>").innerText.trim() == "") {
                checkSaveFlg = true;
                alert(mesNoInputMBSno);
                getAvailableData("input");
            }
            if (document.getElementById("<%=lblAoiNoContent.ClientID %>").innerText.trim() == "" && document.getElementById("<%=chkInputAoi.ClientID%>").checked == true) {
                checkSaveFlg = true;
                alert(mesNoInputAOINo);
                getAvailableData("input");
            }
            //            if (isPass())
            //            {
            //                //must input defect
            //                alert(msgInputDefect);
            //                getAvailableData("input");
            //                inputObj.focus();
            //            }
            //            else
            //            {
            if (!checkSaveFlg) {
                //调用web service提供的打印接口
                //打印方法
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    return;
                }

                //                var keyCollection = new Array();
                //                var valueCollection = new Array();

                //                keyCollection[0] = "@MBSno";

                //                valueCollection[0] = generateArray(mbSno);

                //                setPrintParam(lstPrintItem, "ECR Label", keyCollection, valueCollection);
                //inputCode = scanCode;
                //beginWaitingCoverDiv();
                var IsRCTO = document.getElementById("<%=ChkRCTO.ClientID%>").checked;
                var IsInputPilotMo = document.getElementById("<%=chkInputPilotMo.ClientID%>").checked;
                if (IsInputPilotMo) {
                    pilotMo = document.getElementById("<%=lblPilotMoContent.ClientID %>").innerText.trim();
                }
                WebServicePCAICTInput.save(IsRCTO, mbSno, document.getElementById("<%=lblAoiNoContent.ClientID %>").innerText.trim(), defectInTable, lstPrintItem, IsInputPilotMo, pilotMo, 1, saveSucc, saveFail);
                
            }
            //            }
        }

        function saveSucc(result) {
            endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                //show success message
                setInputOrSpanValue(document.getElementById("<%=lblQtyContent.ClientID %>"), result[1]);
                var printList = new Array();

                if (result[2] && result[2].length > 0) {
                    var k = 0;
                    for (var i = 0; i < result[3].length; i++) {
                        for (var j = 0; j < result[2].length; j++) {
                            
                            printList[k] = new PrintItem(result[2][j].PrintMode,
                                                    result[2][j].RuleMode,
                                                    result[2][j].LabelType,
                                                    result[2][j].TemplateName,
                                                    result[2][j].Piece,
                                                    result[2][j].SpName, null, null,
                                                    result[2][j].OffsetX,
                                                    result[2][j].OffsetY,
                                                    result[2][j].PrinterPort,
                                                    result[2][j].dpi,
                                                    result[2][j].BatPiece,
                                                    result[2][j].Rotate180,
                                                    result[2][j].Layout);
                            printList[k].ParameterKeys = new Array("@MBSno");
                            printList[k].ParameterValues = new Array(new Array(result[3][i]));
                            k++;
                        }
                    }
                    printLabels(printList, true);
                }

                ShowSuccessfulInfo(true, "[" + mbSno + "] " + msgSucc);
            }
            else {
                //show error message
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }
            if (document.getElementById("<%=chkInputPilotMo.ClientID%>").checked) {
                CheckPilotMo(document.getElementById("<%=lblPilotMoContent.ClientID %>").innerText.trim(), document.getElementById("<%=lblMBSnoContent.ClientID %>").innerText.trim());
            }
            //initPage            
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }

        function initPage() {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), "");
            //setInputOrSpanValue(document.getElementById("<%=lblECRContent.ClientID %>"), ""); //ICT Input完成Save以后，ECR栏位不需要清空。批量生产时，机型一样时，ECR不需要变动。
            setInputOrSpanValue(document.getElementById("<%=lblAoiNoContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0, false);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
        }

        function saveFail(result) {
            //show error message
            //endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
            inputObj.focus();
        }

        function inputDefect(data) {
            if (isExistInTable(data)) {
                //error message
                alert(msgDuplicateData);
                getAvailableData("input");
                inputObj.focus();
                return;
            }

            if (isExistInCache(data)) {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;

                rowArray.push(data);
                rowArray.push(desc);

                //add data to table
                if (defectInTable.length < 6) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    //setSrollByIndex(defectInTable.length, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    //setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }

                setSrollByIndex(defectInTable.length, false);

                defectInTable[defectCount++] = data;
                getAvailableData("input");
            }
            else {
                alert(msgInputValidDefect);
                getAvailableData("input");
                inputObj.focus();
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

        function clkSetting() {
            showPrintSetting(stationId, pCode);
        }

        window.onbeforeunload = function() {
            if (inputFlag) {
                OnCancel();
            }
        };

        function OnCancel() {
            WebServicePCAICTInput.cancel(mbSno);
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
        }                  
       
    </script>

</asp:Content>
