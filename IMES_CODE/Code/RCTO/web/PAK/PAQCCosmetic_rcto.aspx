
<%@ Import Namespace="com.inventec.iMESWEB" %> 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PAQCCosmetic_rcto.aspx.cs" Inherits="PAQCCosmetic" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/PAQCCosmetic_rcto.asmx" />
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
    <tr>
        <td>&nbsp;</td>
        <td style="width:10%" align="left"><asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"/></td> 
        <td style="width:10%" align="left">
            <asp:UpdatePanel ID="upPassQty" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upPassQtyValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td> 
            <td>&nbsp;</td>
        <td style="width:10%" align="left"><asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:10%">
            <asp:UpdatePanel ID="upFailQty" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upFailQtyValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>
        <td style="width:15%" align="left">&nbsp;</td>  
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

    <tr>
    <td colspan="6">
    <fieldset id="Fieldset2" >
        <legend align ="left"  ><asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt" /></legend>
            <table border="0" width="99%">
            <tr>
                <td style="width:100%">
	            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
	            <ContentTemplate>
                    <iMES:GridViewExt ID="GridView1" runat="server" AutoGenerateColumns="false" 
                        AutoHighlightScrollByValue="true" GvExtWidth="100%" Width="99.9%" GvExtHeight="200px" 
                        OnGvExtRowClick="" OnGvExtRowDblClick=""  
                        SetTemplateValueEnable="false"  GetTemplateValueEnable="false"  
                        HighLightRowPosition="1" HorizontalAlign="Left" onrowdatabound="GridView1_RowDataBound" 
                        Height="190px">
                            <Columns >  
                               <asp:BoundField DataField="Defect Code" SortExpression="DefectId" />    
                               <asp:BoundField DataField="Description" SortExpression="DefectId" />
                            </Columns>
                    </iMES:GridViewExt>
                </ContentTemplate>                                    
                </asp:UpdatePanel>
                </td>
            </tr>
            </table>         
    </fieldset>
    </td>
    </tr>


    <tr>
	    <td style="width:12%" align="left">
	        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></TD>
	    <td style="width:50%" align="left">
	         <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="10"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
        </td>
        
        
        <td align="left"><asp:CheckBox ID="Chk" runat="server" onclick="" 
                Text="Don't scan '9999'"/></td>
    </tr>

    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" runat="server" id="station1" />
                <input type="hidden" runat="server" id="editor1" />
                <input type="hidden" runat="server" id="customer1" />  
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
var passQty = document.getElementById("<%=upPassQtyValue.ClientID%>");
var failQty = document.getElementById("<%=upFailQtyValue.ClientID%>");

var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount, 10) + 1;

var index = 1;
var GridViewExt1ClientID = "<%=GridView1.ClientID%>";
var passCount = 0;
var failCount = 0;
var station;
var globeProdid;

var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgAlreadyExist = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAlreadyExist").ToString() %>';
var msgError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgError").ToString() %>';

var mesNopaqc = '<%=this.GetLocalResourceObject(Pre + "_mesNopaqc").ToString()%>';
var mespaqc = '<%=this.GetLocalResourceObject(Pre + "_mespaqc").ToString()%>';

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
        document.getElementById("<%=Chk.ClientID%>").checked = false;
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
        ShowInfo("");
        var prod = txtProdId.innerHTML;

        if (prod == "") {
            if (getPdLineCmbValue() == "") {
                alertAndCallNext(mesNoSelPdLine);
                setPdLineCmbFocus();
                return;
            }
            if (inputData.length != 9 && inputData.length != 10 && inputData.substring(0, 2) != "CN") {
                alertAndCallNext(mesInput);
                getCommonInputObject().focus();
                return;
            }
            clearTable();
            clearData();
            updateLabel("", "", "");
            var checked = (document.getElementById("<%=Chk.ClientID%>").checked == true) ? "1" : "0";
            beginWaitingCoverDiv();
            WebServicePAQCCosmetic.ProcessInput(inputData, checked, getPdLineCmbValue(), station, editor, customer, onSucceed, onFail);
            getAvailableData("processDataEntry");
        }
        else {
            if (inputData.length == 4) {
                if (inputData == "7777") {
                    clearTable();
                    getCommonInputObject().focus();
                    getAvailableData("processDataEntry");
                }
                else if (inputData == "9999") {
                    var defectArray = new Array();
                    var gvTable = document.getElementById("<%=GridView1.ClientID%>");
                    for (var i = 0; i < gvTable.rows.length - 1; i++) {
                        if (gvTable.rows[i + 1].cells[0].innerHTML != "&nbsp;") {
                            defectArray.push(gvTable.rows[i + 1].cells[0].innerText);
                        }
                    }
                    var prod1 = txtProdId.innerHTML;            
                    beginWaitingCoverDiv();
                    WebServicePAQCCosmetic.save(prod1, defectArray, on9999Succeed, on9999Fail);
                    getAvailableData("processDataEntry");
                }
                else {
                    var table = document.getElementById("<%=GridView1.ClientID%>");
                    for (var i = 0; i < table.rows.length; i++) {
                        if (table.rows[i].cells[0].innerText == inputData) {
                            alert(msgAlreadyExist);
                            getCommonInputObject().focus();
                            getAvailableData("processDataEntry");
                            return;
                        }
                    }
                    beginWaitingCoverDiv();
                    WebServicePAQCCosmetic.ProcessDefect(inputData, onDefectSucceed, onDefectFail);
                    getAvailableData("processDataEntry");
                }
            }
            else {
                alert(msgError);
                getCommonInputObject().focus();
                getAvailableData("processDataEntry");
                return;
            }
        }
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

/////////////////////Defect//////////////////////
function onDefectSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            var rowInfo = new Array();
            rowInfo.push(result[1]);
            rowInfo.push(result[2][0]);
            AddRowInfo(rowInfo);
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

function onDefectFail(error) {
    try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        DataEntryControl.focus();
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

///////////////9999/////////////////
function on9999Succeed(result) {
    try {
        endWaitingCoverDiv();

        document.getElementById("<%=hidProdID.ClientID%>").value = "";
        txtCustSN.innerHTML = "";
        txtModel.innerHTML = "";
        txtProdId.innerHTML = "";
        document.getElementById("<%=Chk.ClientID%>").checked = false;
        clearTable();

        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            if (result[2][0] == "0") {
                if (result[2][1] == "1") {
                    ShowSuccessfulInfo(true, "Product:[" + result[1] + "] " + mespaqc);
                }
                else {
                    ShowSuccessfulInfo(true, "Product:[" + result[1] + "] " + mesNopaqc);
                }
                passCount++;
            }
            else {
                ShowSuccessfulInfo(true, "[" + result[1] + "] " + msgSuccess);
                failCount++;
            }
            updateQty(passCount, failCount);
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

function on9999Fail(error) {
    try {
        endWaitingCoverDiv();

        document.getElementById("<%=hidProdID.ClientID%>").value = "";
        txtCustSN.innerHTML = "";
        txtModel.innerHTML = "";
        txtProdId.innerHTML = "";
        document.getElementById("<%=Chk.ClientID%>").checked = false;
        clearTable();
        
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        DataEntryControl.focus();
    } catch (e) {
        alertAndCallNext(e.description);
    }
}
//////////////////////////ProdID/////////////////
function onSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            if (result[2] == "1") {
                if (result[1][4] == "1") {
                    ShowSuccessfulInfo(true, "Product:[" + result[1][2] + "] " + mespaqc);
                }
                else {
                    ShowSuccessfulInfo(true, "Product:[" + result[1][2] + "] " + mesNopaqc);
                }
                document.getElementById("<%=hidProdID.ClientID%>").value = "";
                txtCustSN.innerHTML = "";
                txtModel.innerHTML = "";
                txtProdId.innerHTML = "";
                
                clearTable();

                passCount++;
                updateQty(passCount, failCount);
            }
            else {
                document.getElementById("<%=hidProdID.ClientID%>").value = result[1][2];
                txtCustSN.innerHTML = result[1][0];
                txtModel.innerHTML = result[1][1];
                txtProdId.innerHTML = result[1][2];
            }   
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

function onFail(error) {
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

function AddRowInfo(RowArray) {
    if (index < initRowsCount) {
        eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
    } else {
        eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
    }
    index++;
    setSrollByIndex(index, false);
}

function updateLabel(customerSN, model, productId) {
    globeProdid = productId;
    txtProdId.innerHTML = productId;
    txtModel.innerHTML = customerSN;
    txtCustSN.innerHTML = model;
}

function clearTable() {
    ClearGvExtTable("<%=GridView1.ClientID%>", initRowsCount);
    //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
    index = 1;
    //清空客户端保存表格的结构数组
}

function onSysError(error) {
    endWaitingCoverDiv();
    ShowMessage(error.get_message());
    ShowInfo(error.get_message());
    clearData();
    getAvailableData("processDataEntry");
}

function clearSession() {
    var prodid = document.getElementById("<%=hidProdID.ClientID%>").value;
    if (prodid != "") {
        WebServicePAQCCosmetic.wfcancel(prodid);
    }
}

window.onbeforeunload = clearSession;

</script>
</asp:Content>