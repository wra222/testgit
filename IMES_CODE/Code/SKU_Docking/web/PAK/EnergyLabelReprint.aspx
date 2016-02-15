<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:EnergyLabelReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 * TODO:
 */ --%>
 <%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="EnergyLabelReprint.aspx.cs" Inherits="PAK_EnergyLabelReprint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <script language="javascript" type="text/javascript">
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var station = '<%=Station%>'
    var inputObj;
    var pCode = '<%=PCode%>';
    var custsn;
    var emptyPattern = /^\s*$/;

    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
    var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
    var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
    var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
    var msgCustSNNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgKitIDNull").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            inputObj.focus();
            getAvailableData("processFun");
        } catch (e) {
            alert(e.description);
            inputObj.focus();
        }
    }

    function clkSetting() {
        showPrintSetting(station, pCode);
    }
    
    function clkReprint() {
        ShowInfo("");
        if (null == custsn) {
            custsn = getCommonInputObject().value.trim();
        }
        if ("" == custsn) {
            custsn = getCommonInputObject().value.trim();
        }
        var strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

        if (emptyPattern.test(strReason)) {
            alert(msgReasonNull);
            getAvailableData("processFun");
            document.getElementById("<%=txtReason.ClientID %>").focus();
            return;
        }
        custsn = Get2DCodeCustSN(custsn);
        if ("" == custsn) {
            alert(msgCustSNNull);
            getAvailableData("processFun");
            endWaitingCoverDiv();
            getCommonInputObject().focus();
            return;
        }
        if (custsn.length == 10 || custsn.length == 11) {
            pattCustSN1 = /^CN.{8}$/;
            pattCustSN2 = /^SCN.{8}$/;
            //if (pattCustSN1.exec(custsn) || pattCustSN2.exec(custsn)) {
			if (CheckCustomerSN(custsn)) {
                if (custsn.length == 11) {
                    custsn = custsn.substring(1, 11);
                }
            }
            else {
                alert("Wrong Code!");
                getAvailableData("processFun");
                getCommonInputObject().focus();
                return;
            }
        }
        else {

            alert("Wrong Code!");
            getAvailableData("processFun");
            getCommonInputObject().focus();
            return;
        }
        
        try {
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null) {
                //endWaitingCoverDiv();
                alert(msgPrintSettingPara);
                //getAvailableData("processFun");
                //getCommonInputObject().focus();
                initPage();
                return;
            }
            beginWaitingCoverDiv();
            WebServiceEnergyLabelReprint.Reprint(custsn, strReason, "", editor, station, customer, printItemlist, onSucceedPrint, onFail);
        }
        catch (e) {
            getAvailableData("processFun");
            endWaitingCoverDiv();
            getCommonInputObject().focus();
            alert(e);
        }
    }

    function processFun(backData) {
        custsn = backData.trim();
        clkReprint();
    }

    function onSucceedPrint(result) {
        endWaitingCoverDiv();
        if (!((result.length == 2) && (result[0] == SUCCESSRET) && (result[1][0].LabelType == "EnergyLabel"))) {
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            return;
        }
        var successTemp = "[" + custsn + "]" + msgSuccess;
        ShowSuccessfulInfo(true, successTemp);

        setPrintItemListParam1(result[1], custsn);
        printLabels(result[1], true);
        initPage();
    }

    function onFail(result) {
        ShowMessage(result.get_message());
        ShowInfo(result.get_message());
        endWaitingCoverDiv();
        initPage();
    }

    function setPrintItemListParam1(backPrintItemList, custsn) {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@sn";
        valueCollection[0] = generateArray(custsn);
        setPrintParam(lstPrtItem, "EnergyLabel", keyCollection, valueCollection);
    }

    function imposeMaxLength(obj) {
        var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
        return (obj.value.length < mlength);
    }

    function ismaxlength(obj) {
        var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
        if (obj.getAttribute && obj.value.length > mlength) {
            alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
            obj.value = obj.value.substring(0, mlength);
            reasonFocus();
        }
    }

    function Tab(reasonPara) {
        if (event.keyCode == 9) {
            getCommonInputObject().focus();
            event.returnValue = false;
        }
    }

    function reasonFocus() {
        document.getElementById("<%=txtReason.ClientID %>").focus();
    }

    function initPage() {
        clearData();
        custsn = "";
        getCommonInputObject().value = "";
        getAvailableData("processFun");
        getCommonInputObject().focus();
        document.getElementById("<%=txtReason.ClientID %>").value = "";
    }       
    </script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceEnergyLabelReprint.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width:140px;"/>
                    <col />
                    <col style="width:150px;"/>
                </colgroup>
                 <tr>
                    <td>
                        <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <iMES:Input ID="txtCustSN" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="80%" IsClear="true" IsPaste="true" CssClass="iMes_textbox_input_Yellow" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>    
                    </td>
                    <td colspan="2">
                        <textarea id="txtReason" rows="5" style="width:98%;" 
                        runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                        onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="1"></textarea>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="center">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <button id="btnReprint" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkReprint()" tabindex="2"></button>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td> 
                     <td>
                        <input id="btnPrintSetting" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'"    onclick="return clkSetting()" />
                    </td> 
                    <td></td>
                </tr>                                
            </table>

        </div>
    </div>      
    
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel> 
      
</asp:Content>

