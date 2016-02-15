<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/FA/ITCNDCheck Page
 * UI:CI-MES12-SPEC-FA-UI ITCNDCheck.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCNDCheck.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* Check Item
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ Import Namespace="com.inventec.iMESWEB" %> 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ITCNDCheck.aspx.cs" Inherits="FA_ITCNDCheck" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceITCNDCheck.asmx" />
        </Services>
</asp:ScriptManager>

<div>

    <center>
    <table border="0" width="95%">
    <tr>   
        <td style="width:10%"> &nbsp; </td> 
        <td style="width:30%"> &nbsp;</td>     
        <td style="width:15%"> &nbsp; </td> 
        <td style="width:15%" align="center">&nbsp;</td>
    </tr> 
    <tr>
        <td style="width:10%" align="left">
            <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:30%" colspan="2" align="left">
            <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"/></td>         
        <td style="width:15%">&nbsp;</td>
        <td style="width:15%">
            <input id="btquery" type="button" style="width:100%" runat="server" class="iMes_button" 
               onclick="showQueryDialog()" onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/></td>               

    </tr>
    <tr>
        <td style="width:10%" align="left"><asp:Label ID="lbProductId" runat="server" CssClass="iMes_label_13pt"/></td> 
        <td style="width:30%" align="left">
            <asp:UpdatePanel ID="UpdatePanelProdId" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upProductId" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td> 
        <td style="width:15%" align="left"><asp:Label ID="lbPassQty" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:15%">
            <asp:UpdatePanel ID="UpdatePanelPassQty" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upPassQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>
        <td style="width:15%" align="left"><asp:Label ID="lbFailQty" runat="server" CssClass="iMes_label_13pt"/></td>  
        <td style="width:15%">
            <asp:UpdatePanel ID="UpdatePanelFailQty" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upFailQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>       
    </tr>
    <tr>
        <td style="width:10%" align="left"><asp:Label ID="lbCustomerSN" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:30%" align="left">
            <asp:UpdatePanel ID="UpdatePanelCustomerSN" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upCustomerSN" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>
        <td style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:15%" colspan="3">
            <asp:UpdatePanel ID="UpdatePanelModel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>                
    </tr>
    
    <TR><TD colspan="6"><hr></TD></TR>
    <TR>
	    <TD colspan="6" align="left">
	        <asp:Label ID="lbDefectList" runat="server" CssClass="iMes_label_13pt"/></TD>	   
    </TR>

    <TR>
	    <TD colspan="6">
	      <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
          <ContentTemplate>
	         <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" 
                GetTemplateValueEnable="False" GvExtHeight="160px" Height="150px" GvExtWidth="100%" OnGvExtRowClick=""
                OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="1"
                HorizontalAlign="Left" onrowdatabound="GridViewExt1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Item" SortExpression="DefectId" />
                    <asp:BoundField DataField="Value" SortExpression="DefectDescr" />
                </Columns>
             </iMES:GridViewExt>
               
          </ContentTemplate>   
          </asp:UpdatePanel> 
	    </TD>   
    </TR>


    <tr>
	    <TD style="width:12%" align="left">
	        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></TD>
	    <TD colspan="5" align="left" >
	         <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="10"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
        </TD>
        
    </tr>
    
    <tr>
    
    	   <td align="right" colspan="5"><input id="btpPrintSet" type="button"  runat="server" 
               class="iMes_button" onclick="showPrintSettingDialog()" 
               onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'" align="right"/></td>
               
                <td style="width:15%">
            <input id="btreprint" type="button" style="width:100%" runat="server" class="iMes_button" 
               onclick="reprint()" onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/></td>
    </tr>
    
    
    
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" runat="server" id="station1" />
                <input type="hidden" runat="server" id="editor1" />
                <input type="hidden" runat="server" id="customer1" />  
                <input type="hidden" runat="server" id="pCode1" />   
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
var txtProdId = document.getElementById("<%=upProductId.ClientID%>");
var passQty = document.getElementById("<%=upPassQty.ClientID%>");
var failQty = document.getElementById("<%=upFailQty.ClientID%>");
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount, 10) + 1;
var saveDefectArray = new Array();
var index = 1;
var GridViewExt1ClientID = "<%=gridview.ClientID%>";
var passCount;
var failCount;
var station;
var globeProdid;
var pcode;
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var mesnoprint = '<%=this.GetLocalResourceObject(Pre + "_mesnoprint").ToString()%>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        
var accountid = '<%=AccountId%>';
var username = '<%=UserName%>';
var login = '<%=Login%>';


document.body.onload = function() {
    try {
        PdLineControl = getPdLineCmbObj();
        setPdLineCmbFocus();
        DataEntryControl = getCommonInputObject();
        getAvailableData("processDataEntry");
        passCount = 0;
        failCount = 0;
        updateQty(passCount, failCount);
        station = document.getElementById("<%=station1.ClientID%>").value;
        editor = document.getElementById("<%=editor1.ClientID%>").value;
        customer = document.getElementById("<%=customer1.ClientID%>").value;
        pcode = document.getElementById("<%=pCode1.ClientID%>").value;
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function updateQty(pass, fail) {
    passQty.innerHTML = pass.toString();
    failQty.innerHTML = fail.toString();
}

function processDataEntry(inputData) {
    try {
        ShowInfo(" ");
        var errorFlag = false;
        
        if (getPdLineCmbValue() == "") {
            alertAndCallNext(mesNoSelPdLine);
            errorFlag = true;
            setPdLineCmbFocus();
            return;
        }
        if (inputData.length != 9 && inputData.length != 10) {
            alertAndCallNext(mesInput);
            errorFlag = true;
            getCommonInputObject().focus();
            return;
        }
        if (!isProdIDorCustSN(inputData, getPdLineCmbValue())) {
            alertAndCallNext(mesInput);
            errorFlag = true;
            getCommonInputObject().focus();
            return;
        }
        if (!errorFlag) {     
            clearTable();
            clearData();
            //ITC-1360-1515
            
            updateLabel("", "", "");

            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem != null) {
                beginWaitingCoverDiv();
                WebServiceITCNDCheck.CheckImageDL(lstPrintItem, getPdLineCmbValue(), inputData, station, editor, customer, onSucceed, onFail);
            }
            else {
                alert(msgPrintSettingPara);                
            }
            getAvailableData("processDataEntry");
        }
    } catch (e) {
        //alertAndCallNext(e.description);
        alertAndCallNext(e);
    }
}
function generateArray(val) {
    var ret = new Array();
    ret[0] = val;
    return ret;
}

function alertAndCallNext(message) {
    endWaitingCoverDiv();
    alert(message);
    clearData();
    getAvailableData("processDataEntry");
}

function AddRowInfo(RowArray) {
    if (index < initRowsCount) {
        eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
    } else {
        eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
    }
    index++;
    setSrollByIndex(index, false);
}

function showErrorMessageAndCallNext(message) {
    endWaitingCoverDiv();
    ShowMessage(message);
    ShowInfo(message);
    clearData();
    getAvailableData("processDataEntry");
}

function updateLabel(customerSN, model, productId) {
    globeProdid = productId;
    document.getElementById("<%=upProductId.ClientID%>").innerHTML = productId;
    document.getElementById("<%=upCustomerSN.ClientID%>").innerHTML = customerSN;
    document.getElementById("<%=upModel.ClientID%>").innerHTML = model;
}

function updateTable(items, len1, values, len2) {
    for (var i = 0; i < len1; i++) {
        var rowInfo = new Array();
        rowInfo.push(items[i]);
        rowInfo.push(values[i]);
        AddRowInfo(rowInfo);
    }
}

function setPrintItemListParam1(backPrintItemList)
{
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@productID";
    valueCollection[0] = generateArray(globeProdid);
    
    setPrintParam(lstPrtItem, "WWAN Label", keyCollection, valueCollection);
}

function onSucceed(result) {
    try {
        endWaitingCoverDiv();

        if (result == null) {
            alertAndCallNext(msgSystemError);
            DataEntryControl.focus();
            
        }
        else if (result[0] == SUCCESSRET) {
            
            updateLabel(result[1][0], result[1][1], result[1][9]);
            updateTable(result[1][2], result[1][3], result[1][4], result[1][5]);
            if (result[1][8] == "1") {
                ShowSuccessfulInfo(true, "[" + result[1][9] + "] " + msgSuccess);
                setPrintItemListParam1(result[1][7]);
                printLabels(result[1][7], false);
            }
            else {
                ShowSuccessfulInfo(true, "[" + result[1][9] + "] " + mesnoprint);
            }
            var setMsg = result[1][10];
            if (setMsg != "" && setMsg != null) {
                ShowSuccessfulInfoWithColor(false, setMsg, null, "red");
            }
            DataEntryControl.focus();
            getAvailableData("processDataEntry");           
            passCount++;
        }
        else {
            var content = result[0];
            showErrorMessageAndCallNext(content);
            DataEntryControl.focus();
            if (result.length == 2 && result[1] == "CheckFail") {
                failCount++;
            }
        }
        updateQty(passCount, failCount);
    } catch (e) {
        alertAndCallNext(e.description);
    }
} 

function onFail(error) {
    try {
        endWaitingCoverDiv();
        onSysError(error);
        DataEntryControl.focus();
        
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function clearTable() {
    ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
    //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
    index = 1;
    //清空客户端保存表格的结构数组
    saveDefectArray.length = 0;
}

function onSysError(error) {
    endWaitingCoverDiv();
    ShowMessage(error.get_message());
    ShowInfo(error.get_message());
    clearData();
    getAvailableData("processDataEntry");
}

function showQueryDialog() {
    if (getPdLineCmbValue() == "") {
        alertAndCallNext(mesNoSelPdLine);        
        setPdLineCmbFocus();
        return;
    }
    var paramArray = new Array();
    paramArray[0] = getPdLineCmbValue();
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = station;
    window.showModalDialog("./ITCNDCheck_Query.aspx", paramArray, 'dialogWidth:700px;dialogHeight:300px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');  
}

function reprint() {
    var url = "ITCNDCheckReprint.aspx?Station=" + station + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
    var paramArray = new Array();
    paramArray[0] = getPdLineCmbValue();
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = station;
    window.showModalDialog(url, paramArray, 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');     
}



function showPrintSettingDialog() {
    showPrintSetting(document.getElementById("<%=station1.ClientID%>").value, document.getElementById("<%=pCode1.ClientID%>").value);
}


</script>
</asp:Content>