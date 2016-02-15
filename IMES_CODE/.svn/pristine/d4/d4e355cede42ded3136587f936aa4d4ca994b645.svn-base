
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RePrintRULabel.aspx.cs" Inherits="PAK_RePrintRULabel" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceRULabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
     <TR>
	   <td></td>
    </TR
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lblProdID" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </ContentTemplate>
        </asp:UpdatePanel>                                        
       </TD>
    </TR>
    <tr>
	    <td style="width:9%" align="left"></td>
	    <td colspan="5" align="left"></td>
	   
	    <td align="right">
	        <table border="0" width="95%">
	            <td style="width:80%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
	            <td><input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="checkRePrint()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        align="right"/></td>
	        </table>
        </td>
    </tr>
    </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>          
    </ContentTemplate>   
</asp:UpdatePanel> 


<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoPalletNo = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

    var SUCCESSRET = "SUCCESSRET";
    var lstPrintItem;
    var inputObj;
    var inpuPalletNo;
    var parentParams;
    var line;
    var editor;
    var customer;
    var station;
    
    
    document.body.onload = function() {
        parentParams = window.dialogArguments;
        line = parentParams[0];
        editor = parentParams[1];
        customer = parentParams[2];
        station = parentParams[3];
        inputObj = getCommonInputObject();
        ShowInfo("");
        getCommonInputObject().focus();
        inputData = inputObj.value;

        getAvailableData("ProcessInput");
    }

    function Check(inputData) {
        ShowInfo("");
        lstPrintItem = getPrintItemCollection();
        if (lstPrintItem == null) {
            alert(msgPrintSettingPara);
            getAvailableData("ProcessInput");
            inputObj.focus();
            return false;
        }
        inpuPalletNo = inputData
        if (inpuPalletNo == "") {
            errorFlag = true;
            msg = mesNoPalletNo;
            alert(msg);
            getCommonInputObject().focus();
            return false;
        }
        if (inpuPalletNo.length != 10) {
            alert("Input Error");
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("ProcessInput");
            return false;
        }
        return true;
    }
    
    
    
    
    function ProcessInput(inputData) {
        try {
            if (Check(inputData)) {
                WebServiceRULabelPrint.CheckRePrintPalletNo(inpuPalletNo, CheckPalletNoSucceed, CheckPalletNoFail);
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }

    function checkRePrint() {
        try {
            inpuPalletNo=inputObj.value;
            if (Check(inpuPalletNo)) {
                WebServiceRULabelPrint.CheckRePrintPalletNo(inpuPalletNo, CheckPalletNoSucceed, CheckPalletNoFail);
            }
        } 
        catch (e) {
            alert(e.description);
        }
    }

    function print() {
        try {
            var errorFlag = false;
            var msg = "";
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            if (inpuPalletNo == "") {
                errorFlag = true;
                msg = mesNoPalletNo;
                alert(msg);
                getCommonInputObject().focus();
                return;
            }

            if (!errorFlag) {
                var station = document.getElementById("<%=stationHF.ClientID %>").value;
                try {
                    beginWaitingCoverDiv();
                    WebServiceRULabelPrint.RePrint(inpuPalletNo, line, editor, station, customer, lstPrintItem, onSucceed, onFail);
                }
                catch (e1) {
                    alert(e1.description);
                }
            }
        } catch (e) {
            alert(e.description);
        }
    }

    function CheckPalletNoSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                print();
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
                getAvailableData("ProcessInput");
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function CheckPalletNoFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Lucy Liu
    //| Create Date	:	10/27/2009
    //| Description	:	调用web service成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                //service方法没有返回
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
                result[1][0].BatPiece = 0;
                setPrintItemListParam(result[1], result[2]);
                printLabels(result[1], false);
                ShowSuccessfulInfo(true, "[" + result[2] + "] " + msgSuccess);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Lucy Liu
    //| Create Date	:	10/27/2009
    //| Description	:	调用web service失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) {

        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }

    }
    function setPrintItemListParam(backPrintItemList, palletNo) //Modify By Benson at 2011/03/30
    {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            var templateName = lstPrtItem[0].TemplateName
            
            keyCollection[0] = "@PalletNo";
            valueCollection[0] = generateArray(palletNo);
            keyCollection[1] = "@TemplateName";
            valueCollection[1] = generateArray(templateName);
            for (var jj = 0; jj < backPrintItemList.length; jj++) {
                backPrintItemList[jj].ParameterKeys = keyCollection;
                backPrintItemList[jj].ParameterValues = valueCollection;
            }
            //setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
    }



    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        document.getElementById("txtReason").value = "";
        ShowInfo("");
        endWaitingCoverDiv();

    }

    function showPrintSettingDialog() {
        //     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
</script>
</asp:Content>

