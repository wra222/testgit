
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="SKULabelPrint.aspx.cs" Inherits="SKULabelPrint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceSKULabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    <tr>
	   <td></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPID" runat="server" Text="ProductID:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="ProductID" runat="server"></asp:Label>    
        </td>
        <td>
            <asp:Label ID="lblMID" runat="server" Text="MBCT2:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="MBCT2ID" runat="server"></asp:Label>
        </td>
        <td colspan="3"></td>
    </tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lblProdID" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </ContentTemplate>
        </asp:UpdatePanel>                                        
       </td>
    </tr>
    
   <tr>
	    <td style="width:12%" align="left" ><asp:Label ID="lbReason" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <textarea id="txtReason" style="width: 99%; height: 100px"></textarea>                
                </textarea>
                </ContentTemplate>                                     
         </asp:UpdatePanel>
         </td>  
    </tr>
    
    <tr>
	    <td style="width:12%" align="left">&nbsp;</td>
	    <td colspan="5" align="left">&nbsp;</td>	   
	    <td align="right">
	        <table border="0" width="100%">
	            <tr><td style="width:80%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/>
					&nbsp;<input id="btnPrint" type="button"  runat="server" style="width:100px" onclick="print()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        align="right"/>
					&nbsp;<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
				</td>                
	            </tr>
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

<input type="hidden" runat="server" id= "hidPCBID" />
<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
	var msgError = 'Error Input!';


    var SUCCESSRET = "SUCCESSRET";
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var lstPrintItem;
    var inputObj;
    var inpuProdid;
    var mac = '';
	var stationId = '<%=Request["Station"] %>';
	var accountId = '<%=Request["AccountId"] %>';
	var login = '<%=Request["Login"] %>';
    
    document.body.onload = function() {
        inputObj = getCommonInputObject();
        ShowInfo("");
        getAvailableData("ProcessInput");
        getCommonInputObject().focus();
    }

    function ProcessInput(inputData) {
        try {
            ShowInfo("");
            var reason = document.getElementById("txtReason").value;
            inpuProdid = getCommonInputObject().value.trim();
            if (inpuProdid == "") {
                errorFlag = true;
                msg = mesNoProdId;
                alert(msg);
                getCommonInputObject().focus();
            }
            if (inputData.length == 9 || (inputData.length == 10 && inputData.charAt(4) != "M")) {
                //ProdID:
                beginWaitingCoverDiv();
                WebServiceSKULabelPrint.GetPCBID(inpuProdid, GetPCBIDSucceed, GetPCBIDFail);
                getAvailableData("ProcessInput");
            }
            else if ((inputData.length == 10 && inputData.charAt(4) == "M") || (inputData.length == 11 && inputData.charAt(5) == "M") || (inputData.length == 12)) {
                var prod = document.getElementById("<%=ProductID.ClientID %>").innerHTML;
                var PCBID = document.getElementById("<%=hidPCBID.ClientID %>").value;
                if (prod == "") {
                    alertAndCallNext("請先刷入Product...");
                    getCommonInputObject().value = "";
                    getCommonInputObject().focus();
                    getAvailableData("ProcessInput");
                    return;
                }
                else if (mac == inputData || PCBID == inputData) {
                    print();
                    getAvailableData("ProcessInput");
                }
                else {
                    alertAndCallNext("刷入PCBID 或 MAC 錯誤...");
                    getCommonInputObject().value = "";
                    getCommonInputObject().focus();
                    getAvailableData("ProcessInput");
                    return;
                }
            }
            else {
                alert(msgError);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
                getAvailableData("ProcessInput");
                return;
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function ProcessInput1(inputData) {
        try{
            print();
            getAvailableData("ProcessInput");
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
    
    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }

    function print() {
        try {
            var errorFlag = false;
            var msg = "";
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            var reason = document.getElementById("txtReason").value;
            inpuProdid = getCommonInputObject().value.trim();

            if (inpuProdid == "") {
                errorFlag = true;
                msg = mesNoProdId;
                alert(msg);
                getCommonInputObject().focus();
            }
            else if (reason == "") {
                //ITC-1360-1612
            }
            else if (reason.length > 80) {
                errorFlag = true;
                msg = mesReasonOutRange;
                //ITC-1360-1264
                alert(msg);
                document.getElementById("txtReason").focus();
            }
            if (!errorFlag) {
                var station = document.getElementById("<%=stationHF.ClientID %>").value;
                var prod = document.getElementById("<%=ProductID.ClientID %>").innerHTML;
                try {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
                    {
                        msg = msgPrintSettingPara;
                        alert(msg);
                        getCommonInputObject().focus();                        
                        return;
                    }
                    beginWaitingCoverDiv();
                    WebServiceSKULabelPrint.Print(prod, inpuProdid, reason, editor, station, customer, pCode, lstPrintItem, onSucceed, onFail);                    
                }
                catch (e1) {
                    alertAndCallNext(e1.description);
                }
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
    

    function GetPCBIDSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                setInputOrSpanValue(document.getElementById("<%=ProductID.ClientID %>"), result[1][0]);
                document.getElementById("<%=hidPCBID.ClientID %>").value = result[1][1];
                mac = result[1][2];
                ShowInfo("[請刷入PCBID 或 MAC]");
                
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

    function GetPCBIDFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }

    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                setInputOrSpanValue(document.getElementById("<%=ProductID.ClientID %>"), result[1][1]);
                setInputOrSpanValue(document.getElementById("<%=MBCT2ID.ClientID %>"), result[1][2]);
                ShowSuccessfulInfo(true, "[Product:" + result[1][1] + "] " + "[MBCT2:" + result[1][2] + "] " + msgSuccess);
                setPrintItemListParam1(result[1][0], result[1][1]);
                printLabels(result[1][0], false);
                //ITC-1360-1348
                //document.getElementById("txtReason").value = "";
                document.getElementById("<%=hidPCBID.ClientID %>").value = "";
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
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
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
	
	function reprint() {
		var url = "../FA/RePrintSKULabel.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
		var paramArray = new Array();
		paramArray[0] = '';
		paramArray[1] = editor;
		paramArray[2] = customer;
		paramArray[3] = stationId;
		window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
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
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
</script>
</asp:Content>

