<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: Combine PCB In Lot (PC)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012/07/09  Kaisheng             Create
 
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CombinePCBinLot.aspx.cs" Inherits="SA_CombinePCBinLot" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceCombinePCBinLot.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="98%">
     
    <tr>
	    <td style="width:15%" align="left"><asp:Label ID="lblMBSN" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:30%" align="left"><asp:Label ID="lblMBSNContext" runat="server" CssClass="iMes_label_11pt" /></td>
	    <td style="width:15%" align="left"><asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:40%" align="left"><asp:Label ID="lblPdlineContext" runat="server" CssClass="iMes_label_11pt" /></td>
   </tr>
    <tr>
	    <td style="width:15%; visibility: hidden;" align="left"><asp:Label ID="lblLotQty" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:30%; visibility: hidden;" align="left"><asp:Label ID="lblLotQtyContext" runat="server" CssClass="iMes_label_11pt" /></td>
	    <td style="width:15%" align="left"></td>
	    <td style="width:40%" align="left"></td>
    </tr>
    <tr>
	    <td colspan="7">
	    <hr />
	    </td>
    </tr>
     <tr>
	    <td colspan="3" align="left"><asp:Label ID="lblotlist" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td align="left">
        </td>
	    <td align="left">
        </td>
	    <td colspan="2" align="left">&nbsp;</td>
	 </tr>
	 <tr>
	    <td colspan="7">
	      <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline"  >
          <ContentTemplate>
	         <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                GetTemplateValueEnable="False" GvExtHeight="230px" GvExtWidth="100%" OnGvExtRowClick="" Height="220px" 
                OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                onrowdatabound="GridViewLot_RowDataBound" >
                <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="RowRadioChk" runat="server" onclick="RadioCheck(this)"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                    <asp:BoundField DataField="LotNo" SortExpression="LotNo" />
                    <asp:BoundField DataField="Type" SortExpression="Type" />
                    <asp:BoundField DataField="Qty" SortExpression="Qty" />
                    <asp:BoundField DataField="Status" SortExpression="Status" />
                    <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" />       
                </Columns>
             </iMES:GridViewExt>
            </ContentTemplate>   
          </asp:UpdatePanel> 
	    </td>
	   
    </tr>
    </table>
    
    <table border="0" cellspacing="0" cellpadding="0" width="98%">
    <tr>
	    <td nowrap="noWrap" style="width:15%" align="left"><asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></td>
	    <td style="width:85%" align="left"> <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true"  Width="99%"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
             <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                <ContentTemplate>
                 <input type="hidden" runat="server" id="station" />
                 <input type="hidden" runat="server" id="useridHidden" />
                 <input type="hidden" runat="server" id="customerHidden" />
                 <input type="hidden" runat="server" id="txtLotnolst" />
                  <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" ></button>
                </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
	</tr>
       
    </table>
    </center> 
    
</div>

<script type="text/javascript">
<!--
var GridViewExt1ClientID = "<%=gridview.ClientID%>";
var gvClientID = GridViewExt1ClientID;
var gvTable = document.getElementById(gvClientID);
var gvHeaderID = "<%=gridview.HeaderRow.ClientID%>";
    
var index = 1;
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount,10) + 1;

var SaveLotNoArray = new Array();

var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';

var mesWrongCode = '<%=this.GetLocalResourceObject(Pre + "_msgWrongCode").ToString()%>';
var msgInputMBSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputMBSN").ToString()%>';

var sFirstInput = "";
var cliLotnolstID;
var iSelectRadioIndex = -1;

var editor = "";
var customer = "";
var station = "";
var pCode = "";
var tbl;
var currentlstcount = 0;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onload
//| Create Date	:	10/27/2009
//| Description	:	Loading accept input data events apposition focus
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
document.body.onload = function() {
    try {
        getAvailableData("processDataEntry");
        editor = "<%=userId%>";
        customer = "<%=customer%>";
        station = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';
        tbl = "<%=gridview.ClientID %>";
        currentlstcount = 0;
    } catch(e) {
        alert(e.description);
    }
};

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	processDataEntry
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	After inputting the data, processing
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function processDataEntry(inputData) {
    //1、	刷入数据的长度为10/11，则视为MBSN
    //2、	若MBSN的长度为11，第5码为’M’，则去掉最后一位校验码
    //3、	若刷入的数据为’7777’，则清空页面
    //4、	若刷入的数据为’9999’，则保存数据
    //5、	其他，则报错：“Wrong Code”
   
    var inputID = inputData.trim();
    try {
        var errorFlag = false;
        //To conduct a preliminary judgment, then submit the background service,pdline
        //DEBUG ITC-1414-0048
        ShowInfo("");
        if (!errorFlag) {
            if (inputID == "7777") {
                //Enter 7777 for the empty table
                // DEBUG ITC-1414-0045 2012/06/04
                ResetPage();
                callNextInput();
            }
            else {
                if (sFirstInput == "") // Input MBSN
                {
                    if ((inputID.length == 10) || (inputID.length == 11)) //MBSN
                    {
                        if ((inputID.length == 11) && (inputID.substr(4, 1) == "M" || inputID.substr(4, 1) == "B")) {
                            sFirstInput = inputID.substr(0, 10);
                        }
                        else //
                        {
                            sFirstInput = inputID;
                        }
                    }
                    else if (inputID == "9999") //Please MBSN
                    {
                        ShowMessage(msgInputMBSN);
                        ShowInfo(msgInputMBSN);
                        callNextInput();
                        return;
                    } 
                    else {
                        ShowMessage(mesWrongCode);
                        ShowInfo(mesWrongCode);
                        callNextInput();
                        return;
                    }
                    //invoke Service
                    WebServiceCombinePCBinLot.inputMBCode(sFirstInput, station, editor, customer, onInputSucceeded, onInputFailed);
                    
                }
                else  //
                {
                    var StrCMD = inputID.trim();
                    if (StrCMD =="9999") //KeyCode：LOCK/UNDO/PASS
                    {
                        //Save
                        GetSelectedLotNoList();
                        WebServiceCombinePCBinLot.save(sFirstInput, document.getElementById("<%=txtLotnolst.ClientID%>").value, onSaveSucceed, onSaveFail);
                    }
                    else 
                    {
                        //Wrong Code;
                        ShowMessage(mesWrongCode);
                        ShowInfo(mesWrongCode);
                        callNextInput();
                    }
                }
            }
        }
    }
    catch (e) {
        alert(e.description);
    }
}
function setInformation(strMbsn, strLineinfo, strQty) {
    document.getElementById("<%=lblMBSNContext.ClientID%>").innerText = strMbsn;
    document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = strLineinfo;
    document.getElementById("<%=lblLotQtyContext.ClientID%>").innerText = strQty;
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onInputSucceeded
//| Create Date	:	10/27/2009
//| Description	:	A complete preservation of success
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onInputSucceeded(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            ShowInfo("");
            //service method not return
            //Need to clear interface
            reset();
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 6) && (result[0] == SUCCESSRET)) //OK
        {
            setInformation(result[1], result[2], result[3].toString());
            setTable(result[4], result[5]);
            //Test Table
            //for (var i < index; i  > 0; i--) {
            //    gvTable.rows[i].cells[0].swapNode(gvTable.rows[i - 1].cells[0]);
            //}
            callNextInput();
        }
        else 
        {
            callNextInput();
        }
    }
    catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onInputFailed
//| Create Date	:	10/27/2009
//| Description	:	Board number scanning fails, show error message
//| Input para.	:	
//| Ret value	:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onInputFailed(result) {
    endWaitingCoverDiv();
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
    sFirstInput = "";
    
    setInformation("", "", "");

    callNextInput();
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSaveSucceed
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	A complete preservation of success
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSaveSucceed(result) 
{
    try {
        endWaitingCoverDiv();
        if (result == null) 
        {
            ShowInfo("");
            //service method not return
            //Need to clear interface
            reset();
            //Make the page for all ProdId scanning are related to the control of enabled
            setStatus(false);
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if (result[0] == SUCCESSRET)
        {
            //Need to clear interface
            //Make the page for all ProdId scanning are related to the control of enabled
            //setStatus(false);
            
            //ShowInfo(msgSuccess);
            //ShowSuccessfulInfo(true);
            if (result[1] == "OK") {
                ShowSuccessfulInfo(true, "[" + sFirstInput + "] " + msgSuccess);
                reset();
                callNextInput();
            }
            else if (result[1] == "ReloadLotList") {
                ShowMessage(result[2]);
                ShowInfo(result[2]);
                clearTable();
                setTable(result[3], result[4]);
                callNextInput();
            }
        }
        else 
        {
            ShowInfo("");
            //Need to clear interface
            reset();
            //Make the page for all ProdId scanning are related to the control of enabled
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
    } 
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSaveFail
//| Create Date	:	10/27/2009
//| Description	:	Board number scanning fails, show error message
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSaveFail(error) {
    try {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        //Need to clear interface
        reset();
        //Make the page and all MB scan associated control enable
        setStatus(false);
        callNextInput();

    } catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}


function ClearData() {
    currentlstcount = 0;
    sFirstInput = "";
    setInformation("", "", "");
    clearTable();
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	clearTable
//| Create Date	:	10/27/2009
//| Description	:	clear Defect List table
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function clearTable() 
{

    for (var i = 0; i <  initRowsCount - 1; i++) {

        var rowArray = gvTable.rows[i+1]; //new Array();ce
        var rw;
        document.getElementById(gvClientID + "_" + parseInt(i) + "_RowRadioChk").style.visibility = "hidden";
        document.getElementById(gvClientID + "_" + parseInt(i) + "_RowRadioChk").checked = false;
        rowArray.cells[1].innerText = " ";
        rowArray.cells[2].innerText = " ";
        rowArray.cells[3].innerText = " ";
        rowArray.cells[4].innerText = " ";
        rowArray.cells[5].innerText = " ";
    }


}

function setTable(Lotinfo,LotStatus) {

    var vclist = initRowsCount;
    if (initRowsCount < Lotinfo.length) 
        vclist = initRowsCount;
    else 
        vclist = Lotinfo.length;

    currentlstcount = vclist;
    for (var i = 0; i <= vclist - 1; i++) {

        var rowArray = gvTable.rows[i + 1]; //new Array();ce
        var rw;
        document.getElementById(gvClientID + "_" + parseInt(i) + "_RowRadioChk").style.visibility = "visible";
        
        if ((Lotinfo[i].lotNo == null) || (Lotinfo[i].lotNo == "")) {
            //rowArray.push("Null");
            rowArray.cells[1].innerText = "Null";
        }
        else {
            //rowArray.push(Lotinfo[i].lotNo);                     //0
            rowArray.cells[1].innerText = Lotinfo[i].lotNo;        }
        if ((Lotinfo[i].type == null) || (Lotinfo[i].type == "")) {
            //rowArray.push("Null");
            rowArray.cells[2].innerText = "Null";
        }
        else {
            //rowArray.push(Lotinfo[i].type);
            rowArray.cells[2].innerText = Lotinfo[i].type;
        }

        if ((Lotinfo[i].qty == null) || (Lotinfo[i].qty.toString() == "")) {
            //rowArray.push("0");
            rowArray.cells[3].innerText = "0";
        }
        else {
            //rowArray.push(Lotinfo[i].qty.toString());           //3
            rowArray.cells[3].innerText = Lotinfo[i].qty.toString();
        }
        if ((LotStatus[i] == null) || (LotStatus[i] == "")) {
            //rowArray.push("0");
            rowArray.cells[4].innerText = "unkown";
        }
        else {
            //rowArray.push(LotStatus[i]);    //0?//4
            rowArray.cells[4].innerText = LotStatus[i];
        }
        if ((Lotinfo[i].cdt == null) || (Lotinfo[i].cdt  == "")) {
            //rowArray.push("null");
            rowArray.cells[5].innerText = "null";
        }
        else {
            //rowArray.push(Lotinfo[i].cdt);    //0?//4
            rowArray.cells[5].innerText = Lotinfo[i].editor;
        }
        document.getElementById(gvClientID + "_" + parseInt(0) + "_RowRadioChk").checked = "checked";
        //add data to table
        //if (i < 12) {
        //    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
        //    setSrollByIndex(i, true, tbl);
        //}
        //else {
        //    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
        //    setSrollByIndex(i, true, tbl);
            //rw.cells[1].style.whiteSpace = "nowrap";
        //}
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	AddRowInfo
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	To add a row to table
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function AddRowInfo(RowArray) {
    try {
        if (index < initRowsCount) 
        {
           eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
        }
       else 
        {
            eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
        }
        setSrollByIndex(index, false, GridViewExt1ClientID);
        index++;
    }
    catch (e) 
    {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	reset
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	Clear interface
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function reset() 
{
    try {
        ClearData();
        clearTable();
    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callNextInput
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	Wait for the rapid control input
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function callNextInput() 
{
    getCommonInputObject().value="";
    //Set fast control focus
    getCommonInputObject().focus();
    //Continue to receive input data
    getAvailableData("processDataEntry");
}

    window.onbeforeunload = function() {
        ExitPage();
    };


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Lucy liu
//| Create Date	:	01/24/2010
//| Description	:	Exit page called
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage() {
    if (sFirstInput != "") {
        WebServiceCombinePCBinLot.Cancel(sFirstInput);
        sleep(waitTimeForClear);
    }

}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ResetPage
//| Author		:	Lucy liu
//| Create Date	:	01/24/2010
//| Description	:	Refresh the page when calling
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ResetPage() 
{
    ExitPage();
    getCommonInputObject().value = "";
    reset();
    document.getElementById("<%=btnReset.ClientID%>").click();
}

function ChkAllClick(headChk) {
    try {
        gvTable = document.getElementById(gvClientID);
        for (var i = 0; i < gvTable.rows.length - 1; i++) {
            document.getElementById(gvClientID + "_" + i + "_RowRadioChk").checked = headChk.checked;
            //document.getElementById("<%=gridview.ClientID%>_" + parseInt(i + 1) + "_RowRadioChk").style.display = "";

        }
        getCommonInputObject().value = "";
        //Set fast control focus
        getCommonInputObject().focus();
    } catch (e) {
        alert(e.description);
    }
}


function RadioCheck(singleChk) {

    try {
        
        singleChk.checked = true;
        gvTable = document.getElementById(gvClientID);
        for (var i = 0; i < gvTable.rows.length - 1; i++) {
            document.getElementById(gvClientID + "_" + (i) + "_RowRadioChk").checked = false;
        }
        //document.getElementById(gvClientID + "_" + (4) + "_RowRadioChk").style.display = "none";
        singleChk.checked = "checked";
        getCommonInputObject().focus();
        return;
        if (iSelectRadioIndex != -1) {
            document.getElementById(gvClientID + "_" + iSelectRadioIndex + "_RowRadioChk").checked = false;
        }
        if (singleChk.checked) {
            for (var i = 0; i < gvTable.rows.length - 1; i++) {
                if (gvTable.rows[i + 1].cells[1].innerText != " ") {
                    
                    if (!document.getElementById(gvClientID + "_" + (i+1) + "_RowRadioChk").checked) {
                        iSelectRadioIndex = i+1;
                        getCommonInputObject().value = "";
                        //Set fast control focus
                        getCommonInputObject().focus();
                        return;
                    }
                }
            }
            //document.getElementById(gvHeaderID + "_HeaderChk").checked = true;
        }
        else {
            //document.getElementById(gvHeaderID + "_HeaderChk").checked = false;
        }
        getCommonInputObject().value = "";
        //Set fast control focus
        getCommonInputObject().focus();
    } catch (e) {
        alert(e.description);
    }
}

function GetSelectedLotNoList() {
    try {
        SaveLotNoArray.length = 0;
        gvClientID = "<%=gridview.ClientID%>";
        cliLotnolstID = document.getElementById("<%=txtLotnolst.ClientID%>");
        var strlist = "";
        gvTable = document.getElementById(gvClientID);
        for (var i = 0; i < gvTable.rows.length - 1; i++) {

            if (document.getElementById(gvClientID + "_" + i + "_RowRadioChk").checked) {
                if (gvTable.rows[i + 1].cells[1].innerHTML != "&nbsp;") {
                    strlist = gvTable.rows[i + 1].cells[1].innerText.trim();
                    break;
                }
            }
        }
        cliLotnolstID.value = strlist;
        if (strlist == "") {
            //alert(mesNoSeRecord);
            return false;
        }
        return true;
    } catch (e) {
        alert(e.description);
    }
}

//-->
</script>


</asp:Content>
