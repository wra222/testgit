<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PCAShippingLabelPrint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create
 * 2012-02-27  Li.Ming-Jun           ITC-1360-0573
 * 2012-02-28  Li.Ming-Jun           ITC-1360-0536
 * 2012-02-28  Li.Ming-Jun           ITC-1360-0715
 * 2012-02-28  Li.Ming-Jun           ITC-1360-0864
 * 2012-03-01  Li.Ming-Jun           ITC-1360-0948
 * 2012-03-04  Li.Ming-Jun           ITC-1360-0882
 * 2012-03-04  Li.Ming-Jun           ITC-1360-1031
 * 2012-03-15  Li.Ming-Jun           ITC-1360-1467
 * 2012-04-11  Li.Ming-Jun           ITC-1360-1662
 * Known issues:
 * TODO:
 * checkitems 中的WWANcheck未完成，由于与BOM有关还没有最终确定所以暂时缺少
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCAShippingLabelPrint.aspx.cs" Inherits="FA_PCAShippingLabelPrint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServicePCAShippingLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 15%;"/>
                    <col />
                    <col style="width: 15%;"/>
                    <col />
                </colgroup>
                <tr>
                    <td></td>
                    <td align="center" colspan="2">
                        <asp:RadioButtonList ID="rblPCA" runat="server" RepeatDirection="Horizontal" Enabled="true">
                            <asp:ListItem Value="ForPC" onclick="rblPCA_Changed(this)" Enabled="false">For PC</asp:ListItem>
                            <asp:ListItem Selected　Value="ForRCTO" onclick="rblPCA_Changed(this)" >For RCTO</asp:ListItem>
                            <asp:ListItem Value="ForFRU" onclick="rblPCA_Changed(this)">For FRU</asp:ListItem>
                        </asp:RadioButtonList> 
                    </td>
                    <td align="center">
		                <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="clkSetting()" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="FA" />
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblDCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbDCodeType ID="CmbDCode" runat="server" Width="99%" />
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblRegion" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="drpRegion" runat="server" Width="99%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>  
                <tr>
                    <td>
                        <asp:Label ID="lblMBSNO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblMBSNOContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProductModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblProductModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblFamilyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVersion" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblVersionContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblECRContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                     
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMAC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblMACContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                     

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
             </table>                                       
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>       
    </div>    
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var DEFAULT_ROW_NUM = 7;
        var mbSNo = "";
        var dcode = "";
        var modelId = "";
        var checkPCMBRCTOMB = "";
        var customer;
        var stationId;
        var pCode;
        var inputObj;
        var val;
        var inputECRCode = "";
        var mbECRCode = "";
        var inputFRUFlag = true;
        //error message
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var mesNoSelDCodeType = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelDCodeType").ToString() %>';
        var msgDCodeTypeNull = '<%=this.GetLocalResourceObject(Pre + "_msgDCodeTypeNull").ToString() %>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString() %>';
        var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString()%>';
        var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString()%>';
        var msg173Mode = '<%=this.GetLocalResourceObject(Pre + "_msg173Mode").ToString()%>';
        window.onload = function() {
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            val = "ForRCTO";
            document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
            inputObj.focus();
        };

        function rblPCA_Changed(obj) {
            val = obj.value;
            if (val == "ForFRU") {
                document.getElementById("<%=drpRegion.ClientID%>").disabled = false;
                inputFRUFlag = false;
                getDecodeTypeCmbObj().disabled = false;
                getDecodeTypeCmbObj().options[0].selected = true;
//            } else if (val == "ForPC") {
//                document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
//                getDecodeTypeCmbObj().disabled = true;
//                getDecodeTypeCmbObj().options[0].selected = true;
            } else if (val == "ForRCTO"){
                document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
                setInputOrSpanValue(document.getElementById("<%=lblProductModelContent.ClientID %>"), "");
                inputFRUFlag = true;
                getDecodeTypeCmbObj().disabled = false;
                getDecodeTypeCmbObj().options[0].selected = true;
                modelId = "";
            }
        }

        function checkMBSno() {
            if (mbSNo == "" || mbSNo == null) {
                alert(msgMBSnoNull);
                return false;
            }
            if (!(mbSNo.length == 10 || mbSNo.length == 11)) {
                alert(msgMBSnoLength);
                return false;
            }
            mbSNo = SubStringSN(mbSNo, "MB");
            return true;
        }

        function input(data)
        {
            if (getPdLineCmbValue() == "") {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("input");
                return;
            }

            if (getDecodeTypeValue() == "" && (val == "ForRCTO" || val == "ForFRU")) {
                alert(mesNoSelDCodeType);
                setDecodeTypeCmbFocus();
                getAvailableData("input");
                return;
            }
            if (inputFlag) {
                ShowInfo("");
                if (checkECR(data) == "ECR") {
                    inputECRCode = data;
                }
                else {
                    alert("請輸入ECR");
                    getAvailableData("input");
                    inputObj.focus();
                    return;
                }
                if (inputECRCode != mbECRCode) {
                    alert("該Ecr沒有Maintain");
                    getAvailableData("input");
                    inputObj.focus();
                    return;
                }
                setInputOrSpanValue(document.getElementById("ctl00_iMESContent_lblECRContent"), inputECRCode);
                dcode = getDecodeTypeValue();
                save();
            }
            else {
                ShowInfo("");
                mbSNo = data.trim();
                
                if (inputFRUFlag && data.length != 12) {
                    if (checkMBSno()) {
                        if (val == "ForFRU") {
                            checkPCMBRCTOMB = "FRU";
                        }
                        else if (val == "ForRCTO") {
                            checkPCMBRCTOMB = "RCTO";
                        }
                        else {
                            checkPCMBRCTOMB = "";
                        }
                        WebServicePCAShippingLabelPrint.InputMB(getPdLineCmbValue(), mbSNo, checkPCMBRCTOMB, editor, stationId, customer, modelId, inputSucc, inputFail);
                    }
                    else {
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }
                else {
                    if (val == "ForFRU") {
                        var inputModel = checkModel(data);
                        if (inputModel == "Model") {
                            setInputOrSpanValue(document.getElementById("ctl00_iMESContent_lblProductModelContent"), data);
                            modelId = data;
                            inputFRUFlag = true;
                            getAvailableData("input");
                            inputObj.focus();
                            return;
                        }
                        else {
                            alert("輸入Model必須是PF開頭，長度必須為12...");
                            getAvailableData("input");
                            inputObj.focus();
                            return;
                        }
                    }
                    inputFRUFlag = true;
                }
            }
        }

        function checkECR(value) {
            var code = "ERROR";
            try {
                if (value.length == 5) {
                    var pattern = /^([A-Z|0-9][0]{2}[A-Z|0-9]{2})$/;
                    if (pattern.test(value)) {
                        code = "ECR";
                    }
                }
                return code;
            }
            catch (e) {
                alert(e.description);
            }
        }

        function checkModel(value) {
            var code = "ERROR";
            try {
                if (value.length == 12) {
                    // var pattern = /P{1}F{1}[A-Z | 0-9]{10}$/;
                    var pattern = /^(?:PF\S{10}|JF\S{10}|SF\S{9})/;
                    if (pattern.test(value)) {
                        code = "Model";
                    }
                }
                return code;
            }
            catch (e) {
                alert(e.description);
            }
        }
        
        function inputSucc(result) {
            setInfo(mbSNo, result);
            mbECRCode = result[4];
            inputFlag = true;
            getAvailableData("input");
            getPdLineCmbObj().disabled = true;
            inputObj.focus();
        }

        function inputFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function setInfo(mbSNo, info)
        {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblMBSNOContent.ClientID %>"), mbSNo);
            setInputOrSpanValue(document.getElementById("<%=lblFamilyContent.ClientID %>"), info[0]);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[1]);
            setInputOrSpanValue(document.getElementById("<%=lblMACContent.ClientID %>"), info[2]);
            setInputOrSpanValue(document.getElementById("<%=lblVersionContent.ClientID %>"), info[3]);
        }
        
        function save()
        {
            var lstPrintItem = getPrintItemCollection();
            //打印方法
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                ResetPage();
                return;
            }
            var region = document.getElementById("<%=drpRegion.ClientID%>").value;
            beginWaitingCoverDiv();
            WebServicePCAShippingLabelPrint.save(mbSNo, dcode, region, lstPrintItem, saveSucc, saveFail);
        }

        function saveSucc(result) 
        {
            endWaitingCoverDiv();
            //show success message
            var keyCollection = new Array();
            var valueCollection = new Array();

            var retDCode = result[0];
            var printLst = result[1];

            keyCollection[0] = "@MBSNO";
            valueCollection[0] = generateArray(mbSNo);

            keyCollection[1] = "@DCode";
            valueCollection[1] = generateArray(retDCode);

            var ModelTypeForPrint = "RCTO";
            if (val == "ForFRU") {
                ModelTypeForPrint = "FRU";
            } else if (val == "ForRCTO") {
                ModelTypeForPrint = "RCTO";
            }
            keyCollection[2] = "@ModelType";
            valueCollection[2] = generateArray(ModelTypeForPrint);
            
            keyCollection[3] = "@Model";
            valueCollection[3] = generateArray(modelId);

            for (var jj = 0; jj < printLst.length; jj++) {
                printLst[jj].ParameterKeys = keyCollection;
                printLst[jj].ParameterValues = valueCollection;
            }
            //setPrintParam(printLst, "MB CT Label", keyCollection, valueCollection);

            printLabels(printLst, false);

            ShowSuccessfulInfo(true, "[" + mbSNo + "] " + msgSuccess);
            
            //initPage
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            setInputOrSpanValue(document.getElementById("<%=lblMBSNOContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblECRContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblFamilyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblMACContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblVersionContent.ClientID %>"), "");
            
            inputFlag = false;            
            getPdLineCmbObj().disabled = false;
            if (val == "ForRCTO") {
                document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
                inputFRUFlag = true;
            }
            else if (val == "ForFRU") {
                if (modelId == "") {
                    inputFRUFlag = false;
                }
            }
            mbCode = "";
            mbECRCode = "";
        }
        
        function saveFail(result)
        {
            //show error message
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
            inputObj.focus();
        }
        
        function clkSetting() {
            showPrintSetting(stationId, pCode);
        }

        window.onbeforeunload = function()
        {
            if (inputFlag)
            {
                OnCancel();
            }
        };   
        
        function OnCancel() {
            if (mbSNo != "") {
                WebServicePCAShippingLabelPrint.cancel(mbSNo);
            }
        }
        
        function ExitPage(){
            OnCancel();
        }
        
        function ResetPage(){
            ExitPage();
            initPage();
            ShowInfo("");
        }                  
       
    </script>
</asp:Content>

