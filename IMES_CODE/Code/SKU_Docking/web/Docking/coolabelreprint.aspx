<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:CombineCOAandDNReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-2-7     207003               Create          
 * Known issues:
 * TODO:
 */ --%>
 <%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="coolabelreprint.aspx.cs" Inherits="DOCK_CooLabelReprint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/CooLabelWebService.asmx" />
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
                        <asp:Label ID="lblPizzaID" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtPizzaID" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="80%" IsClear="true" IsPaste="true" CssClass="iMes_textbox_input_Yellow" />
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
                        <button id="btnReprint" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkReprint()" tabindex="2"></button>
                    </td> 
                     <td>
                        <button id="btnPrintSetting" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkSetting()" tabindex="3"></button>
                    </td> 
                    <td></td>            
                </tr>                                
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
	              <input type="hidden" runat="server" id="Hstation" /> 
	              <input type="hidden" runat="server" id="HpCode" />  
	              <input type="hidden" runat="server" id="HCOO" />  
	              <input type="hidden" runat="server" id="HMode" />
	              <button id="btnGetModel" runat="server" type="button" style="display: none" /> 
	            </ContentTemplate>   
            </asp:UpdatePanel> 
        </div>
    </div>      
    
    <script language="javascript" type="text/javascript">
        var editor = '<%=UserId%>';
        var customer = '<%=Customer%>';
        var station;
        var inputObj;
        var pCode;
        var kitID;
        var emptyPattern = /^\s*$/;
        
        var msgPrintSettingPara= '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
        var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
        var msgInputMaxLength1='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
        var msgInputMaxLength2='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
        var msgKitIDNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgKitIDNull").ToString()%>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj = getCommonInputObject();
            inputObj.focus();
            getAvailableData("processFun");
        }
        
        function clkSetting()
        {
            if (null == station || "" == station) {
                station = document.getElementById("<%=Hstation.ClientID %>").value;
            }
            if (null == pCode || "" == pCode) {
                pCode = document.getElementById("<%=HpCode.ClientID %>").value;
            }
            showPrintSetting(station, pCode);
        }
        function clkReprint() {
            ShowInfo("");
            if (null == kitID) {
                kitID = getCommonInputObject().value.trim();
            }
            if ("" == kitID) {
                kitID = getCommonInputObject().value.trim();
            }
            var strReason = "";
            strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

            /*if (emptyPattern.test(strReason)) {
                alert(msgReasonNull);
                getAvailableData("processFun");
                document.getElementById("<%=txtReason.ClientID %>").focus();
                return;
            }*/
            if (kitID.length == 9 || kitID.length == 10) {
                pattCustSN1 = /^CN.{7}$/;
                pattCustSN2 = /^CN.{8}$/;
                //if (pattCustSN1.exec(kitID) || pattCustSN2.exec(kitID)) {
				if (CheckCustomerSN(kitID)) {
                }
                else {
                    if (kitID.length == 10) {
                        kitID = kitID.substr(0, 9);
                    }
                    else {
                        kitID = kitID;
                    }
                }
            }
            else {

                alert("Wrong Code!");
                getAvailableData("processFun");
                getCommonInputObject().focus();
                return;
            }
            if (kitID.length == 10) {
                pattCustSN1 = /^CN.{8}$/;
                //if (pattCustSN1.exec(kitID)) {
				if (CheckCustomerSN(kitID)) {
                    kitID = kitID;
                }
            }
            
            if (kitID != "") {
                beginWaitingCoverDiv();
                try {
                    var printItemlist = getPrintItemCollection();
                    if (printItemlist == null) {
                        endWaitingCoverDiv();
                        alert(msgPrintSettingPara);
                        //getAvailableData("processFun");
                        //getCommonInputObject().focus();
                        initPage();
                        return;
                    }
                    if (null == station || "" == station) {
                        station = document.getElementById("<%=Hstation.ClientID %>").value;
                    }
                    if (null == pCode || "" == pCode) {
                        pCode = document.getElementById("<%=HpCode.ClientID %>").value;
                    }
                    CooLabelWebService.ReprintCOO(strReason, kitID, editor, station, customer,pCode,printItemlist, printSuccCOO, printFailCOO);
                }
                catch (e) {
                    getAvailableData("processFun");
                    endWaitingCoverDiv();
                    getCommonInputObject().focus();
                    alert(e);
                }
            }
            else {
                alert(msgKitIDNull);
                getAvailableData("processFun");
                endWaitingCoverDiv();
                getCommonInputObject().focus();
            }          
        }
        function processFun(backData) 
        {
            kitID = backData.trim();
            clkReprint();
            
        }
        function printSuccCOO(result) {
            var labelCollection = [];
            var labelname = "";
            if (null == pCode || "" == pCode) {
                pCode = document.getElementById("<%=HpCode.ClientID %>").value;
            }
            if (pCode == "OPDK024")
            {
                labelname = "DK_COO_Label";
            }
            else
            {
                labelname = "DK_JAN_Label";
            }
            for (var i in result[1]) {
                if (result[1][i].LabelType == labelname) {
                    labelCollection.push(result[1][i]);
                    setPrintItemListParamCOO(labelCollection, result[0],labelname);
                    printLabels(labelCollection, false);
                    break;
                }
            }
            endWaitingCoverDiv();
            var successTemp = "";
            if (kitID != "") {
                successTemp = "[" + kitID + "]" + msgSuccess;
            }
            if (successTemp != "") {
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
            //ShowSuccessfulInfo(true);
            initPage();
     
        }

        function setPrintItemListParamCOO(backPrintItemList, sn,labelname) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@ProductId";
            valueCollection[0] = generateArray(sn);
            setPrintParam(lstPrtItem, labelname, keyCollection, valueCollection);
        }
        function printFailCOO(result)
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            endWaitingCoverDiv();
            initPage();            
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
        
        function initPage()
        {
            clearData();
            kitID = "";
            getCommonInputObject().value = "";
            getAvailableData("processFun");
            getCommonInputObject().focus();
            document.getElementById("<%=txtReason.ClientID %>").value = "";
        }       
    </script>  
</asp:Content>

