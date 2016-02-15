<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="CombineCarton_CR.aspx.cs" Inherits="CombineCarton_CR" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceCombineCarton_CR.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbpdLine" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbsubStation" runat="server" CssClass="iMes_label_13pt" Text="Qty" />
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="cmbqty" runat="server" Enabled="true" ></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table border="0" width="95%">   
                <tr>
                    <td style="width: 15%" align="left">
                    &nbsp;</td>
                    <td style="width: 30%" align="left">
                    &nbsp;</td>
                    <td>
                    </td>
                    <td style="width: 12%" align="left">
                    &nbsp;</td>
                    <td align="left">
                    &nbsp;</td>
                    <td>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="5">
                        <hr>
                    </td>
                </tr>

                <tr>
                    <td style="width: 12%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td align="left">
                     <asp:Label ID="txtModel" runat="server" CssClass="label_normal" />
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                        <ContentTemplate>
                           
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>                    
                    <td colspan="4">
                    </td>
                </tr>

                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lbCollection" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick"></asp:AsyncPostBackTrigger>
                        </Triggers>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
                        </Triggers>
                        <ContentTemplate>
                            <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" 
                            AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                            OnGvExtRowClick="" OnGvExtRowDblClick=""  Width="99.9%" Height="230px"
                            SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
                            onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                                <Columns >   
                                        
                                        <asp:BoundField DataField="PartNo" />
                                        
                                </Columns>
                            </iMES:GridViewExt>
                        </ContentTemplate> 
                        </asp:UpdatePanel> 
                    </td>
                </tr>
            </table>
            <table border="0" width="95%">   
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="5" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="500" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <button id="btnGridClear" runat="server" type="button" onclick="" style="display: none"
                            onserverclick="clearGrid">
                            </button>
                            <input type="hidden" runat="server" id="prodHidden" />
                            <input type="hidden" runat="server" id="sumCountHidden" />
                            <input type="hidden" runat="server" id="station" />
                            <input type="hidden" runat="server" id="useridHidden" />
                            <input type="hidden" runat="server" id="hidRowCnt" /> 
                            <input type="hidden" runat="server" id="hidWantData" />                                  
                            <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="5" align="right">
                        <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="showPrintSettingDialog()" />
                        <input id="btnUnderPrint" type="button"  runat="server"  class="iMes_button" onclick="UnderPrint()" value="未滿箱列印" />
                        <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
                        <input type="hidden" runat="server" id="pCode" />
                        <input type="hidden" runat="server" id="hidModel" />
                        <asp:HiddenField ID="stationHF" runat="server" />
                    </td>
                </tr>
            </table>
        </center>
    </div>


<script type="text/javascript">

var editor;
var customer;

var GridViewExt1ClientID = "<%=GridViewExt1.ClientID%>";

var index = 1;
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount,10) + 1;
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelSubStation = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectTestStation").ToString()%>';
var mesNoInputProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputProdId").ToString()%>';
var mesDupData = '<%=this.GetLocalResourceObject(Pre + "_mesDupData").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var custsn = "";
var proid="";
var IsBlankStation = false;
var fistSelStation = "";
var pcode;

var accountid = '<%=AccountId%>';
var username = '<%=UserName%>';
var login = '<%=Login%>';

document.body.onload = function() {
    try {
        pcode = document.getElementById("<%=pCode.ClientID%>").value;
        editor = "<%=userId%>";
        customer = "<%=customer%>";
        gvClientID = "<%=GridViewExt1.ClientID %>";
        //        setPdLineCmbFocus();
        getPdLineCmbObj().setAttribute("AutoPostBack", "True");
        document.getElementById("<%=hidWantData.ClientID%>").value = "0";   //ProdId wanted
        fistSelStation = document.getElementById("<%=stationHF.ClientID %>").value;
        getAvailableData("processDataEntry");
    } catch (e) {
        alert(e.description);
    }
}



function SetStationDefault(selStation)
{
 
     var obj = getTestStationCmbObj();
      for (var i = 0; i < obj.options.length; i++)
      {
         if (obj.options[i].value == selStation)
         { obj.selectedIndex = i; return; }
     }   
}

var lstPrintItem;
var selStation = "";
var FirstTime = true;
var FirstInput = "";
var inputcount = 0;
function processDataEntry(inputData) {
    try{
        var errorFlag = false;
        getCommonInputObject().focus();
        if (getPdLineCmbValue() == "") {
            alert(mesNoSelPdLine);
            errorFlag = true;
            getAvailableData("processDataEntry");
        }
        if (document.getElementById("<%=cmbqty.ClientID%>").value == "") {
            alert(mesNoSelPdLine);
            errorFlag = true;
            document.getElementById("<%=cmbqty.ClientID%>").focus();
        }

        if (!errorFlag) {
            lstPrintItem = getPrintItemCollection();
            if (lstPrintItem != null) {
                if (FirstTime) {
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    WebServiceCombineCarton_CR.inputProductFirst(getPdLineCmbValue(), inputData, fistSelStation, editor, customer, onFirstSuccess, onFirstFail);
                }
                else {
                    ShowInfo("");
                    var Checkinput = false;
                    gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
                    for (k = 1; k <= inputcount; k++) {
                        if(gvTable.rows[k].cells[0].innerText == inputData)
                        {
                            Checkinput = true;
                        }
                    }
                    if (!Checkinput) {
                        beginWaitingCoverDiv();
                        WebServiceCombineCarton_CR.inputProductOther(inputData, FirstInput, onOtherSuccess, onOtherFail);
                    }
                    else {
                        alert("ProductID:" + inputData + "已經輸入...")
                        callNextInput();
                    }
                }
            }
            else {
                alert(msgPrintSettingPara);
                callNextInput();
                endWaitingCoverDiv();
            }
        }
    } 
    catch(e) {
        alert(e.description);
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onFirstSuccess
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function onFirstSuccess(result) {
    try {
        if (result == null) {
            endWaitingCoverDiv();
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if (result[0] == SUCCESSRET) {
            
            gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
            inputcount += 1;
            FirstTime = false;
            PartNo = result[1];
            FirstInput = result[1];
            gvTable.rows[inputcount].cells[0].innerText = PartNo;
            gvTable.rows[inputcount].cells[0].title = PartNo;
            getPdLineCmbObj().setAttribute("disabled", "disabled");
            document.getElementById("<%=cmbqty.ClientID%>").setAttribute("disabled", "disabled");
            document.getElementById("<%=txtModel.ClientID%>").innerHTML = result[2];

            if (document.getElementById("<%=cmbqty.ClientID%>").value == inputcount) {
                WebServiceCombineCarton_CR.save(FirstInput, lstPrintItem, onSaveSucceed, onSaveFail);
            }
            callNextInput();
            endWaitingCoverDiv();
        }
        else {
            endWaitingCoverDiv();
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

function onFirstFail(error) {
    try {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        reset();
        callNextInput();
    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onOtherSuccess
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function onOtherSuccess(result) {
    try {
        if (result == null) {
            endWaitingCoverDiv();
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if (result[0] == SUCCESSRET) {
            gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
            inputcount += 1;
            FirstTime = false;
            PartNo = result[1];
            gvTable.rows[inputcount].cells[0].innerText = PartNo;
            gvTable.rows[inputcount].cells[0].title = PartNo;
            if (document.getElementById("<%=cmbqty.ClientID%>").value == inputcount) {
                WebServiceCombineCarton_CR.save(FirstInput, lstPrintItem, onSaveSucceed, onSaveFail);
            }
            callNextInput();
            endWaitingCoverDiv();
        }
        else {
            endWaitingCoverDiv();
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

function onOtherFail(error) {
    try {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        reset();
        callNextInput();
    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSaveSucceed
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

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
        else if (result[0] == SUCCESSRET) {
            var tmpid = result[2];
            reset();
            FirstInput = "";
            FirstTime = true;
            inputcount = 0;
            setPrintItemListParam(result[1], result[2]); // 1 : PrintItem   2 : Custsn
            printLabels(result[1], false);
            ShowSuccessfulInfo(true, "[CartonSN:" + tmpid + "] " + msgSuccess);
            callNextInput();
        }
        else {
            setStatus(true);
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
    } catch (e) {
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

function UnderPrint() {
    gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
    if (FirstInput != "") {
        WebServiceCombineCarton_CR.save(FirstInput, lstPrintItem, onSaveSucceed, onSaveFail);
    }
    else {
        alert("請先輸入至少一個ProductID...");
        callNextInput(); 
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onClearSucceeded
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

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
            //变更描述： 修改7777规则：刷入7777，清空除Station和Line之外的所有信息
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

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Print using~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function setPrintItemListParam(backPrintItemList, custsn) {
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();
    keyCollection[0] = "@CartonSN";
    valueCollection[0] = generateArray(custsn);
//    setPrintParam(lstPrtItem, "CR_CartonLabel", keyCollection, valueCollection);
//    keyCollection[0] = "@sn";
//    valueCollection[0] = generateArray(custsn);
    for (i = 0; i < lstPrintItem.length; i++) {
        setPrintParam(lstPrtItem, lstPrtItem[i].LabelType, keyCollection, valueCollection);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	ÍË³öÒ³ÃæÊ±µ÷ÓÃ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage() {
    WebServiceCombineCarton_CR.Cancel(FirstInput, fistSelStation, onClearSucceeded, onClearFailed);
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ResetPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	Ë¢ÐÂÒ³ÃæÊ±µ÷ÓÃ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ResetPage() {
    ExitPage();
    getCommonInputObject().value = "";
    reset();
}

function showPrintSettingDialog() {
    var Station = document.getElementById("<%=stationHF.ClientID %>").value;
    showPrintSetting(Station, document.getElementById("<%=pCode.ClientID%>").value);
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	clearTable
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	½«Defect List tableµÄÄÚÈÝÇå¿Õ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function clearTable() {
    try {
        ClearGvExtTable("<%=GridViewExt1.ClientID%>", initRowsCount);

    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	reset
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	Çå¿Õ½çÃæ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function reset() {
    try {
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";
        getPdLineCmbObj().removeAttribute("disabled");
        document.getElementById("<%=cmbqty.ClientID%>").removeAttribute("disabled");
        clearTable();
    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callNextInput
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	µÈ´ý¿ìËÙ¿Ø¼þ¼ÌÐøÊäÈë
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function callNextInput() {
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}

window.onbeforeunload = function() {
    ExitPage();
}

function reprint() {
    var fistSelStation = document.getElementById("<%=stationHF.ClientID %>").value;

    var url = "CombineCartonReprint_CR.aspx?Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
    var paramArray = new Array();
    paramArray[0] = getPdLineCmbValue();
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = fistSelStation;
    window.showModalDialog(url, paramArray, 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');     


}

//-->
    </script>

</asp:Content>

