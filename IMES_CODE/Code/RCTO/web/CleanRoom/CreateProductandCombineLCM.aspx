﻿
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CreateProductandCombineLCM.aspx.cs" Inherits="CreateProductandCombineLCM" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceCreateProductandCombineLCM.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
        <table border="0" width="95%">
            <%--Cotrol PDLine--%>
            <tr>
                <td style="width:15%" align="left"><asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                <td colspan="7"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" /></td>	   
            </tr>
            <%--Control Family--%>
            <tr>
	            <td style="width:15%" align="left"><asp:Label ID="lbFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	            <td colspan="7"><iMES:CmbFamily ID="cmbFamily" runat="server" Width="100" IsPercentage="true"/></td>
            </tr>
            <%--Control MO--%>
            <tr>
	            <td style="width:15%" align="left"><asp:Label ID="lbMO" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	            <td colspan="7">
	                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbModelMO" runat="server" Width="100%" AutoPostBack="true"></asp:DropDownList>                             
                        </ContentTemplate>                                        
                   </asp:UpdatePanel>
	            </td>
            </tr>
            <%--MO Detail--%>
            <tr>
	            <td></td>
	            <td width="10%" align="left"><asp:Label ID="lbMoQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	            <td width="10%" align="left">&nbsp;</td>
	            <td width="15%" align="left">
	                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lbShowMoQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                        </ContentTemplate>                                        
                   </asp:UpdatePanel>
	            </td>
	            <td style="width:5%">&nbsp;</td>
	            <td width="15%" align="left"><asp:Label ID="lbReQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	            <td align="left"> 
	                 <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lbShowReQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                                           
                        </ContentTemplate>                                        
                   </asp:UpdatePanel>
                </td>
                <td width="8%"  align="left"></td>
            </tr>
        </table>
        <hr />
        <table border="0" width="95%">
            <tr>
                <td style="width: 15%" align="left">
                    <asp:Label ID="lblProId" runat="server" CssClass="iMes_label_13pt">ProductID:</asp:Label>
                </td>
                <td style="width: 20%" align="left">
                    <asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_13pt" />
                            
                    <%--<asp:UpdatePanel runat="server" ID="upProId" UpdateMode="Conditional">
                        <ContentTemplate>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td colspan="6" align="left">
                    <asp:Label ID="lblCollectionData" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
                
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
                            <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick"></asp:AsyncPostBackTrigger>
                        </Triggers>
                        <ContentTemplate>
                            <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                            OnGvExtRowClick="" OnGvExtRowDblClick="showCollection(this)"  Width="99.9%" Height="230px"
                            SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
                            onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                                <Columns >   
                                    <asp:BoundField DataField="Image" />
                                    <asp:BoundField DataField="SubstitutePartNo" />
                                    <asp:BoundField DataField="SubstituteDescr" />
                                    <asp:BoundField DataField="PartType"  />
                                    <asp:BoundField DataField="PartDescr"  />
                                    <asp:BoundField DataField="PartNo" />
                                    <asp:BoundField DataField="Qty" />
                                    <asp:BoundField DataField="PQty" />
                                    <asp:BoundField DataField="CollectionData" />
                                    <asp:BoundField DataField="HfCollectionData" />
                                    <asp:BoundField DataField="HfPartNo" />
                                    <asp:BoundField DataField="HfIndex" />
                                </Columns>
                            </iMES:GridViewExt>
                        </ContentTemplate> 
                    </asp:UpdatePanel> 
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" colspan="5">
                    <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    <input type="button" id="btnGridFresh" runat="server" onclick="" style="display: none" onserverclick="FreshGrid" />
                    <input type="button" id="btnGridClear" runat="server" onclick="" style="display: none" onserverclick="clearGrid" />
                    <input type="hidden" runat="server" id="hidRowCnt" />
                    <input type="hidden" runat="server" id="hidStation" />
                    <input type="hidden" runat="server" id="hidLine" />
                    <input type="hidden" runat="server" id="hidKpt" />
                    <input type="hidden" runat="server" id="hidWantData" />
                    <input type="hidden" runat="server" id="hidInput" />
                </td>
            </tr>
            <tr>
                <td align="right">&nbsp;
                    
                </td>
                <td align="right" colspan="4" >
                    <input id="btpPrintSet" type="button"  runat="server" 
                    class="iMes_button" onclick="showPrintSettingDialog()" 
                    onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" align="right"/>
                </td>
                <td align="right">&nbsp;
                    <input id="btnPrint" type="button"  runat="server" 
                    onclick="print()" class="iMes_button" 
                    onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="False"/>
                    <input id="btnReprint" type="button"  runat="server" 
                    onclick="reprint()" class="iMes_button" 
                    onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True"/>
                </td>                      	   	   	    
            </tr>
            <tr>
                <td colspan="5">
                    <asp:HiddenField ID="hidSKU" runat="server" />
                </td>
                <td align="right">&nbsp;
                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
                        <ContentTemplate>
                            <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" ></button> 
                            <input type="hidden" runat="server" id="station1" />
                            <input type="hidden" runat="server" id="editor1" />
                            <input type="hidden" runat="server" id="customer1" />
                            <input type="hidden" runat="server" id="pCode" /> 
                            <input type="hidden" runat="server" id="prodHidden" /> 
                            <input type="hidden" runat="server" id="firstInputCT" />
                            <input type="hidden" runat="server" id="hidMOPrefix" />
                            <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" ></button> 
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
    </center>
</div>

  


<script type="text/javascript">

var mesNoSelectFamily = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectFamily").ToString()%>';
var mesNoSelectModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelPrintTemp = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPrintTemplate").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var mesQtyIllegal = '<%=this.GetLocalResourceObject(Pre + "_mesQtyIllegal").ToString()%>';
var mesQtyExReQty = '<%=this.GetLocalResourceObject(Pre + "_mesQtyExReQty").ToString()%>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var firstCT;
var productID;
var startSn;
var endSn;
var station;
var editor;
var customer;
var code;
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount, 10) + 1;

var accountid = '<%=AccountId%>';
var username = '<%=UserName%>';
var login = '<%=Login%>';
//

document.body.onload = function() {
    station = document.getElementById("<%=station1.ClientID%>").value;
    editor = document.getElementById("<%=editor1.ClientID%>").value;
    customer = document.getElementById("<%=customer1.ClientID%>").value;
    code = document.getElementById("<%=pCode.ClientID%>").value;
    gvClientID = "<%=GridViewExt1.ClientID %>";
    setPdLineCmbFocus();
    callNextInput();
}

function reprint() {
    var url = "CreateProductandCombineLCMRePrint.aspx?Station=" + station + "&PCode=" + code + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
    var paramArray = new Array();
    paramArray[0] = station;
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = code;
    window.showModalDialog(url, paramArray, 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
}

function getModelMOCmbObj() {
    return document.getElementById("<%=cmbModelMO.ClientID %>");
}

function getModelMOCmbText() {
    if (getModelMOCmbObj().selectedIndex == -1) {
        return "";
    } else {
    return getModelMOCmbObj()[getModelMOCmbObj().selectedIndex].text;
    }
}

function getCmbModelMO_Model() {
    if (getModelMOCmbObj().selectedIndex == -1) {
        return "";
    } else {
    var temp = getModelMOCmbObj()[getModelMOCmbObj().selectedIndex].text;
        return temp.split("-")[1];
    }
}

function getCmbModelMO_MO() {
    if (getModelMOCmbObj().selectedIndex == -1) {
        return "";
    } else {
    var temp = getModelMOCmbObj()[getModelMOCmbObj().selectedIndex].text;
        return temp.split("-")[0];
    }
}

function getModelMOCmbValue() {
    return document.getElementById("<%=cmbModelMO.ClientID %>").value;
}

function processDataEntry(inputData) {
    try {
        var errorFlag = false;
        getCommonInputObject().focus();
        if (getPdLineCmbText() == "") {
            alert(mesNoSelPdLine);
            errorFlag = true;
            setPdLineCmbFocus();
            callNextInput();
        }
        else if (getFamilyCmbText() == "") {
            alert(mesNoSelectFamily);
            errorFlag = true;
            setFamilyCmbFocus();
            callNextInput();
        }
        else if (getModelMOCmbText() == "") {
            alert(mesNoSelectModel);
            errorFlag = true;
            getModelMOCmbObj().focus();
            callNextInput();
        }
        else if (document.getElementById("<%=lbShowReQty.ClientID %>").innerText == "0") {
            alert("該Model已經無法列印...");
            errorFlag = true;
            setFamilyCmbFocus();
            callNextInput();
        }
        
        if (!errorFlag) {
            if (document.getElementById("<%=txtProdId.ClientID%>").innerText == "") {
                if (inputData.length != 14 && inputData.length != 18 && inputData.length != 23) {
                    alert("輸入LCM CT 錯誤...");
                    errorFlag = true;
                    callNextInput();
                    return;
                }
                ShowInfo("");
               // beginWaitingCoverDiv();
                document.getElementById("<%=firstInputCT.ClientID%>").value = inputData;
                firstCT = inputData;
                WebServiceCreateProductandCombineLCM.CreateProductID(getPdLineCmbValue(), getCmbModelMO_Model(), getCmbModelMO_MO(),
                                                                    "<%=userId%>", station, "<%=customer%>", getFamilyCmbText(),
                                                                    document.getElementById("<%=hidMOPrefix.ClientID%>").value, onCreateProductSucceed, onCreateProductFail);
            } 
            else {
                InputLCMCT(inputData, document.getElementById("<%=hidRowCnt.ClientID%>").value);
            }
        }
    } 
    catch (e) {
        alert(e.description);
    }
}

function InputLCMCT(inputData,cnts) {
    var proid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
    document.getElementById("<%=hidRowCnt.ClientID%>").value = cnts;
    proid = SubStringSN(proid, "ProdId");
    var lstPrintItem = getPrintItemCollection();
    if (lstPrintItem == null) {
        alert(msgPrintSettingPara); //請先檢查設置列印頁面參數
        callNextInput();
    } else if (inputData == "getbomnull") {
        flag = false;
        flag_39 = false;
        var selStation = station;
        if (selStation == '3B') {
            flag = true;
        }
        else {
            flag = false;
        }
        if (selStation == '39') {
            flag_39 = true;
        }
        else {
            flag_39 = false;
        }
        WebServiceCreateProductandCombineLCM.save(proid, flag, flag_39, lstPrintItem, onSaveSucceed, onSaveFail);
    }
    else {
        if (inputData == "7777") {
            var tmpTable = document.getElementById("<%=GridViewExt1.ClientID%>");
            var tmprowCnt = document.getElementById("<%=hidRowCnt.ClientID%>").value;
            var tmp_count;
            for (tmp_count = 1; tmp_count <= tmprowCnt; tmp_count++) {
                tmpTable.rows[tmp_count].cells[7].innerText = 0;
                tmpTable.rows[tmp_count].cells[8].innerText = " ";
                tmpTable.rows[tmp_count].cells[8].title = "";
            }
            WebServiceCreateProductandCombineLCM.ClearPart(proid, onClearSucceeded, onClearFailed);
        }
        else {
            WebServiceCreateProductandCombineLCM.inputPPID(proid, inputData, onSucceed, onFail);
        }
    }
}

function setPrintItemListParam1(backPrintItemList,Proid) //Modify By Benson at 2011/03/30
{
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@ProductID";

    valueCollection[0] = generateArray(Proid);

    for (var jj = 0; jj < backPrintItemList.length; jj++) {
        backPrintItemList[jj].ParameterKeys = keyCollection;
        backPrintItemList[jj].ParameterValues = valueCollection;
    }
}

function onSucceed(result) {
    try {
        eval("setRowNonSelected_" + gvClientID + "()");
        var iLength = result.length;
        if (result == null) {
            endWaitingCoverDiv();
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
            //处理界面输出信息
            gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
            qtySum = 0;
            pqtySum = 0;
            needCheck = true;
            alreadyMatch = false;
            var flag = false;
            var flag_39 = false;
            var rowCnt = document.getElementById("<%=hidRowCnt.ClientID%>").value;
            repVC = result[1];
            for (k = 1; k <= rowCnt; k++) {
                qty = parseInt(gvTable.rows[k].cells[6].innerText);
                pqty = parseInt(gvTable.rows[k].cells[7].innerText);
                qtySum += qty;
                pqtySum += pqty;
                if (!alreadyMatch) {
                    if (gvTable.rows[k].cells[1].innerText.indexOf(repVC) != -1 && gvTable.rows[k].cells[6].innerText != gvTable.rows[k].cells[7].innerText) {
                        eval("setScrollTopForGvExt_" + gvClientID + "('" + gvTable.rows[k].cells[11].innerText + "',11,'','MUTISELECT')");
                        alreadyMatch = true;
                        pqty++;
                        pqtySum++;
                        gvTable.rows[k].cells[7].innerText = pqty;
                        collectionData = gvTable.rows[k].cells[8].innerText;
                        // }
                        if (collectionData != " ") {
                            collectionData += "," + result[2];
                        }
                        else {
                            collectionData = result[2];
                        }
                        gvTable.rows[k].cells[8].innerText = collectionData;
                        gvTable.rows[k].cells[8].title = collectionData;
                    }
                }
            }
            if (needCheck == true && qtySum <= pqtySum) {
                flag = false;
                flag_39 = false;
                var selStation = station;
                if (selStation == '3B') {
                    flag = true;
                }
                else {
                    flag = false;
                }
                if (selStation == '39') {
                    flag_39 = true;
                }
                else {
                    flag_39 = false;
                }
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) {
                    alert(msgPrintSettingPara); //請先檢查設置列印頁面參數
                    callNextInput();
                }
                WebServiceCreateProductandCombineLCM.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, flag, flag_39, lstPrintItem, onSaveSucceed, onSaveFail);
            }
            endWaitingCoverDiv();
            callNextInput();
        }
        else {
            endWaitingCoverDiv();
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
    } catch (e) {
        alert(e.description);
    }
}

function onFail(error) {
    try {
        setStatus(true);
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        clearData();
        callNextInput();
    } catch (e) {
        alert(e.description);
    }
}

function onSaveSucceed(result) {
    try {
        endWaitingCoverDiv();
        ShowInfo("Suceed!", "green");
        eval("setRowNonSelected_" + gvClientID + "()");
        if (result == null) {
            setStatus(true);
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if (result[0] == SUCCESSRET && result.length == 3) {
            var tmpid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
            reset();
            scanQty = 0;
            sumQty = 0;

            
            setStatus(false);
            callNextInput();
            setPrintItemListParam1(result[1], result[2]);
            printLabels(result[1], false);
            ShowSuccessfulInfo(true, "[" + result[2] + "] " + msgSuccess);
        }
        else {
            setStatus(true);
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
    } 
    catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function onSaveFail(error) {
    try {
        endWaitingCoverDiv();
        eval("setRowNonSelected_" + gvClientID + "()");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    } catch (e) {
        alert(e.description);
    }
}

function onCreateProductSucceed(result)
{
    try {
        if(result==null)
        {            
            endWaitingCoverDiv();
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);          
        }
        else if((result.length == 3)&&(result[0]==SUCCESSRET)) {
            ShowInfo("");
            endWaitingCoverDiv();
            productID = result[2][0];
            document.getElementById("<%=txtProdId.ClientID%>").innerText = productID;
            document.getElementById("<%=prodHidden.ClientID%>").value = SubStringSN(productID, "ProdId");
            document.getElementById("<%=btnGridFresh.ClientID%>").click();
            document.getElementById("<%=lbShowReQty.ClientID%>").innerText = result[2][1];
            
        }
        else 
        {
            endWaitingCoverDiv();
            ShowInfo("");
            var content =result[0];
            ShowMessage(content);
            ShowInfo(content);
         } 
    } 
    catch(e)
    {
        alert(e.description);
        endWaitingCoverDiv();
    }    
}

function onCreateProductFail(error)
{
    try
    {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    }
     catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function setStatus(status) {
}

function showCollection(row) {
    if (row != null) {
        var qty = row.cells[6].innerText.trim();
        var pqty = row.cells[7].innerText.trim();
        var PN = row.cells[5].innerText.trim();
        var Cldata = row.cells[9].innerText.trim();
        var ClPartNodata = row.cells[10].innerText.trim();

        var dataCllist = Cldata.trim().split(",");
        var dataPartNoCllist = ClPartNodata.trim().split(",");
        if (Cldata == "") {
            dataCllist.pop();
        }
        if (ClPartNodata == "") {
            dataPartNoCllist.pop();
        }
        var popParam = new dataInfo(PN, qty, pqty, dataCllist, dataPartNoCllist);
        ShowCollection(popParam);
    }
    else {
        alert("Please select a row");
    }
}

function showPrintSettingDialog() {    
    showPrintSetting(document.getElementById("<%=station1.ClientID%>").value,document.getElementById("<%=pCode.ClientID%>").value);
}

document.onkeydown = check;
function check() {
    //keyCode是event事件的属性,对应键盘上的按键,回车键是13,tab键是9,其它的如果不知道 ,查keyCode大全
    if(event.keyCode == 13)
        event.keyCode = 9;
}

function onClearSucceeded(result) {
    try {
        if (result == null) {
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
            ExitPage();
            reset();
            ShowInfo("");
            callNextInput();
        }
        else {
            ShowInfo("");
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
    } catch (e) {
        alert(e.description);
    }
}

function onClearFailed(error) {
    try {
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        reset();
        callNextInput();
    } catch (e) {
        alert(e.description);
    }
}

function ExitPage() {
    if (document.getElementById("<%=txtProdId.ClientID%>").innerText != "") {
        WebServiceCreateProductandCombineLCM.ClearPart(document.getElementById("<%=txtProdId.ClientID%>").innerText, onClearSucceeded, onClearFailed);
    } 
}

function ResetPage()
{
    ExitPage();
}

function clearTable() {
    try {
        ClearGvExtTable("<%=GridViewExt1.ClientID%>", initRowsCount);

    } catch (e) {
        alert(e.description);

    }

}

function reset() {
    try {
        document.getElementById("<%=txtProdId.ClientID%>").innerText = "";
        document.getElementById("<%=firstInputCT.ClientID%>").value = "";
        document.getElementById("<%=hidRowCnt.ClientID%>").value = "";
        clearTable();
    } catch (e) {
        alert(e.description);

    }
}

function alertNoQueryCondAndFocus() {
    alert(mesNoSelectFamily);
    setFamilyCmbFocus();
}

function alertNoInputModel() {
    alert(mesNoSelectModel);
    ModelFocus();
}

function callNextInput() {
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}   

window.onbeforeunload = function() {
    ExitPage();
}  

</script>

</asp:Content>


