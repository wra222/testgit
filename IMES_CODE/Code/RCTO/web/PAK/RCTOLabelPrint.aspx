
<%@ Import Namespace="com.inventec.iMESWEB" %> 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RCTOLabelPrint.aspx.cs" Inherits="RCTOLabelPrint" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/RCTOLabelPrint.asmx" />
        </Services>
</asp:ScriptManager>

<div>
    <center>
    <table border="0" width="95%">
    <tr>   
        <td style="width:10%"> &nbsp; </td> 
        <td style="width:10%"> &nbsp; </td> 
        <td style="width:10%"> &nbsp;</td>     
        <td style="width:10%"> &nbsp; </td> 
        <td style="width:10%">&nbsp;</td>
        <td style="width:10%">&nbsp;</td>
    </tr> 
    <tr>
        <td style="width:10%" align="left">
            <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:30%" colspan="7" align="left">
            <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"/></td>
    </tr>
    
    </table>
    <hr class="footer_line" style="width:95%"/>
    
    <table border="0" width="95%">
    <tr>
    <td colspan="6">
    <fieldset id="Fieldset1">
        <legend align ="left"  ><asp:Label ID="lblProdInfo" runat="server" CssClass="iMes_label_13pt" /></legend>
            <table border="0" width="95%">
                <tr>
                <td style="width:10%" align="left"><asp:Label ID="lblCustomerSN" runat="server" CssClass="iMes_label_13pt"/></td>
                <td style="width:15%" align="left">
                    <asp:UpdatePanel ID="upCustomerSN" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="upCustomerSNValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                        </ContentTemplate>                                        
                    </asp:UpdatePanel></td>
                <td style="width:10%" align="left"><asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"/></td>
                <td style="width:15%" colspan="3">
                    <asp:UpdatePanel ID="upProductID" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="upProductIDValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                        </ContentTemplate>                                        
                    </asp:UpdatePanel></td>            
                </tr>
                <tr>
                <td style="width:10%" align="left"><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"/></td>
                <td style="width:15%" align="left">
                    <asp:UpdatePanel ID="upModel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="upModelValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                        </ContentTemplate>                                        
                    </asp:UpdatePanel></td>
                </tr>
            </table>         
    </fieldset>
    </td>
    </tr>

    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    </table>

    <table border="0" width="95%">
    <tr>
	    <td style="width:12%" align="left">
	        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></td>
	    <td style="width:75%" align="left">
	         <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="11"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
        </td>
        
        
        <td align="right"><input id="btnPrintSet" type="button"  runat="server" 
               class="iMes_button" onclick="showPrintSettingDialog()" 
               onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'" align="right"/>
			&nbsp;<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
		</td>
    </tr>

    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" runat="server" id="station1" />
                <input type="hidden" runat="server" id="editor1" />
                <input type="hidden" runat="server" id="customer1" />  
                <input type="hidden" runat="server" id="pCode1" /> 
                <input type="hidden" runat="server" id="hidProdID" />  
            </ContentTemplate>   
            </asp:UpdatePanel>  
            <asp:HiddenField ID="stationHF" runat="server" />
        </td>
    </tr>
    </table>
    </center> 
</div>

<script language="JavaScript">

var PdLineControl;
var DataEntryControl;
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesInput = '<%=this.GetLocalResourceObject(Pre + "_mesInput").ToString()%>';
var txtProdId = document.getElementById("<%=upProductIDValue.ClientID%>");
var txtCustSN = document.getElementById("<%=upCustomerSNValue.ClientID%>");
var txtModel = document.getElementById("<%=upModelValue.ClientID%>");

var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var pcode;
var station;
var stationId = '<%=Request["Station"] %>';
var accountId = '<%=Request["AccountId"] %>';
var login = '<%=Request["Login"] %>';

var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgError").ToString() %>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';


document.body.onload = function() {
    try {
        PdLineControl = getPdLineCmbObj();
        setPdLineCmbFocus();
        DataEntryControl = getCommonInputObject();
        getAvailableData("processDataEntry");
        
        station = document.getElementById("<%=station1.ClientID%>").value;
        editor = document.getElementById("<%=editor1.ClientID%>").value;
        customer = document.getElementById("<%=customer1.ClientID%>").value;
        pcode = document.getElementById("<%=pCode1.ClientID%>").value;
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function processDataEntry(inputData) {
    try {
        ShowInfo("");
        
        if (getPdLineCmbValue() == "") {
            alertAndCallNext(mesNoSelPdLine);
            setPdLineCmbFocus();
            return;
        }
        if (inputData.length == 9 || (inputData.length == 10 && inputData.charAt(4) != "M" && inputData.charAt(4) != "B")) {
            //ProdID:
            var existProd = txtProdId.innerHTML;
            beginWaitingCoverDiv();
            WebServiceRCTOLabelPrint.ProcessProd(inputData, existProd, getPdLineCmbValue(), station, editor, customer, onProdSucceed, onProdFail);
            getAvailableData("processDataEntry");
        }
        else if ((inputData.length == 10 && (inputData.charAt(4) == "M" || inputData.charAt(4) == "B")) ||
            (inputData.length == 11 && (inputData.charAt(5) == "M" || inputData.charAt(5) == "B"))) {
            //MBSN:
            var prod = txtProdId.innerHTML;
            if (prod == "") {
                alertAndCallNext(mesInput);
                getCommonInputObject().focus();
                getAvailableData("processDataEntry");
                return;
            }
            else {
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null)//判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
                {
                    msg = msgPrintSettingPara;
                    alert(msg);
                    getCommonInputObject().focus();
                    return;
                }
                beginWaitingCoverDiv();
                WebServiceRCTOLabelPrint.ProcessMB(prod, inputData, lstPrintItem, onMBSucceed, onMBFail);
                getAvailableData("processDataEntry");
            }
        }
        else {
            alert(msgError);
            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
            return;
        }
    } catch (e) {
        alertAndCallNext(e.description);
    }
}


///////////////MB/////////////////
function onMBSucceed(result) {
    try {
        endWaitingCoverDiv();

        var prodid = document.getElementById("<%=hidProdID.ClientID%>").value;
        
        document.getElementById("<%=hidProdID.ClientID%>").value = "";
        txtCustSN.innerHTML = "";
        txtModel.innerHTML = "";
        txtProdId.innerHTML = "";

        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            ShowSuccessfulInfo(true, "[" + prodid + "] " + msgSuccess);
            setPrintItemListParam1(result[2][0], result[2][1]);
            printLabels(result[2][0], false);
            DataEntryControl.focus();
        }
        else {
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            DataEntryControl.focus();
        }
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function setPrintItemListParam1(backPrintItemList, Proid)
{
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@ProductID";

    valueCollection[0] = generateArray(Proid);

    for (var jj = 0; jj < backPrintItemList.length; jj++) {
        backPrintItemList[jj].ParameterKeys = keyCollection;
        backPrintItemList[jj].ParameterValues = valueCollection;
    }
    //setPrintParam(lstPrtItem, "RCTO_Label", keyCollection, valueCollection);
}

function onMBFail(error) {
    try {
        endWaitingCoverDiv();

        document.getElementById("<%=hidProdID.ClientID%>").value = "";
        txtCustSN.innerHTML = "";
        txtModel.innerHTML = "";
        txtProdId.innerHTML = "";
        
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        DataEntryControl.focus();
    } catch (e) {
        alertAndCallNext(e.description);
    }
}
//////////////////////////ProdID/////////////////
function onProdSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            document.getElementById("<%=hidProdID.ClientID%>").value = result[1][2];
            txtCustSN.innerHTML = result[1][0];
            txtModel.innerHTML = result[1][1];
            txtProdId.innerHTML = result[1][2];
            DataEntryControl.focus();
        }
        else {
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            DataEntryControl.focus();
        }
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function onProdFail(error) {
    try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        DataEntryControl.focus();
    } catch (e) {
        alertAndCallNext(e.description);
    }
}


function alertAndCallNext(message) {
    endWaitingCoverDiv();
    alert(message);
    clearData();
    getAvailableData("processDataEntry");
}

function updateLabel(customerSN, model, productId) {
    txtProdId.innerHTML = productId;
    txtModel.innerHTML = customerSN;
    txtCustSN.innerHTML = model;
}

function onSysError(error) {
    endWaitingCoverDiv();
    ShowMessage(error.get_message());
    ShowInfo(error.get_message());
    clearData();
    getAvailableData("processDataEntry");
}

function showPrintSettingDialog() {
    showPrintSetting(document.getElementById("<%=station1.ClientID%>").value, document.getElementById("<%=pCode1.ClientID%>").value);
}

function clearSession() {
    var prodid = document.getElementById("<%=hidProdID.ClientID%>").value;
    if (prodid != "") {
        WebServiceRCTOLabelPrint.wfcancel(prodid);
    }
}

function reprint() {
	var url = "../PAK/RCTOLabelRePrint.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode1.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
	var paramArray = new Array();
	paramArray[0] = getPdLineCmbValue();
	paramArray[1] = editor;
	paramArray[2] = customer;
	paramArray[3] = stationId;
	window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
}

window.onbeforeunload = clearSession;

</script>
</asp:Content>