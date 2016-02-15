<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:IECLabelReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-09  zhu lei               Create
 * 2012-03-01  Li.Ming-Jun           ITC-1360-0974
 * Known issues:
 * TODO:
 * 
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IECLabelReprint.aspx.cs" Inherits="FA_IECLabelReprint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path=  "~/FA/Service/WebServiceIECLabelPrint.asmx" />
    </Services>
</asp:ScriptManager>   

<div id="divIECLabel" style="z-index: 0; width:95%" class="iMes_div_center">
    <br />

    <table width="90%" cellpadding="0" cellspacing="0" border="0" align="center">
         <tr style="height:30px">   
            <td width="20%" align="left" >
                &nbsp;<asp:Label id="lblVendorCT" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
            </td>

            <td width="70%" valign="middle" align="left" >
                <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                    TabIndex="1"/> 
            </td>
        </tr>

        <tr><td colspan="2">&nbsp;</td></tr>
        
        <tr align="left" valign="middle">
            <td width="20%" align="left" >
                &nbsp;<asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            </td>
            <td width="70%" align="left" valign="middle">
                <textarea id="txtReason" rows="5" style="width:98%;" 
                    runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                    onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="2"></textarea>
            </td>
         </tr>   
         
         <tr><td colspan="2">&nbsp;</td></tr>
          
         <tr>
             <td width="20%" align="left"></td>
             <td width="70%" align="right">
                &nbsp;<input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                        onclick="showPrintSettingDialog()" class="iMes_button" 
                        onmouseover="this.className='iMes_button_onmouseover'" 
                        onmouseout="this.className='iMes_button_onmouseout'"
                        tabindex ="3"/>&nbsp;
             </td>
         </tr>
           
         <tr><td colspan="2">&nbsp;</td></tr>
          
         <tr>
             <td width="20%" align="left"></td>
             <td width="70%" align="right">
                &nbsp;<input id="btnPrint" type="button"  runat="server" onclick="Reprint()" style="height:auto"
                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                class=" iMes_button"  tabindex="4"/>&nbsp;
            </td>
         </tr>       
    </table>
    
    <br />
    
    <asp:UpdatePanel ID="updatePanel" runat="server">
    </asp:UpdatePanel> 
</div>

<script language="javascript" type="text/javascript">
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
    var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
    var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';

    var msgVendorCT = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgVendorCT").ToString() %>';
    var msgVendorCTNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgVendorCTNull").ToString() %>';

    var inputObj;
    var vendorCT = "";
    var strReason = "";
    var station = '<%=Request["Station"] %>';
    var pcode = '<%=Request["PCode"] %>';
    var editor = "<%=UserId%>";
    var customer = "<%=Customer%>";
    var emptyPattern = /^\s*$/;

    window.onload = function() {
        inputObj = getCommonInputObject();
        DisplayInfoText();     //显示MessageTextArea框
        inputObj.focus();
        getAvailableData("processFun");
    };

    function checkReprintReason() {
        strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

        if ((strReason == "") || (strReason.length == 0) || (strReason == null)) {
            alert(msgReasonNull);
            reasonFocus();
            return false;
        }
        else return true;
    }

    function ismaxlength(obj) {
        var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
        if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");    
            alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
            obj.value = obj.value.substring(0, mlength);
            reasonFocus();
        }
    }

    function imposeMaxLength(obj) {
        var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
        return (obj.value.length < mlength);
    }

    function Tab(reasonPara) {
        if (event.keyCode == 9) {
            inputObj.focus();
            event.returnValue = false;
        }
    }

    function reasonFocus() {
        document.getElementById("<%=txtReason.ClientID %>").focus();
    }

    function showPrintSettingDialog() {
        showPrintSetting(station, pcode);
    }

    function processFun(inputStr) {
        reasonFocus()
    }

    function Reprint() {
        ShowInfo("");
        vendorCT = inputObj.value.trim();
        if (vendorCT != "") {
            if (checkReprintReason()) {
                if (vendorCT.length == 14) {
                    var printItemlist = getPrintItemCollection();

                    if (printItemlist == "" || printItemlist == null) {
                        alert(msgPrintSettingPara);
                        return;
                    }

                    beginWaitingCoverDiv();
                    WebServiceIECLabelPrint.Reprint(vendorCT, strReason, "", editor, station, customer, printItemlist, onSucceed, onFailed);
                }
                else {
                    alert(msgVendorCT);
                    initPage();
                    getAvailableData("processFun");
                }
            }
            else {
                getAvailableData("processFun");
            }
        }
        else {
            alert(msgVendorCTNull);
            initPage();
            getAvailableData("processFun");
        }
    }

    function onSucceed(result) {
        if (result == null) {
            endWaitingCoverDiv();
            ShowErrorMessage(msgSystemError);
        }
        else if (result[0] == SUCCESSRET) {
            var config = result[1][3];
			setPrintItemListParam(vendorCT, result[1], config);
            printLabels(result[1][2], false);

            ResetPage();
            ShowSuccessfulInfo(true, "[" + vendorCT + "] " + msgSuccess);
        }
        else {
            endWaitingCoverDiv();
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            inputObj.value = "";
            getAvailableData("processFun");
            inputObj.focus();
        }
    }

    function onFailed(result) {
        ResetPage();
        ShowErrorMessage(result.get_message());
    }

    function setPrintItemListParam(vendorCT, backPrintItemList, config) {
        var dCode = backPrintItemList[0];
        var rev = backPrintItemList[1];
        var lstPrtItem = backPrintItemList[2];
        var template = lstPrtItem[0].TemplateName;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@CT";
        valueCollection[0] = generateArray(vendorCT+";");

        keyCollection[1] = "@DCode";
        valueCollection[1] = generateArray(dCode);

        keyCollection[2] = "@Rev";
        valueCollection[2] = generateArray(rev);
		
		keyCollection[3] = "@Config";
		valueCollection[3] = generateArray(config);

		keyCollection[4] = "@TemplateName";
		valueCollection[4] = generateArray(template);


        setPrintParam(lstPrtItem, "KP Label", keyCollection, valueCollection);
    }

    function ExitPage() {
    }

    function ResetPage() {
        endWaitingCoverDiv();
        initPage();
    }

    function initPage() {
        ShowInfo("");
        inputObj.value = "";
        getAvailableData("processFun");
        inputObj.focus();
    }
</script>

</asp:Content>