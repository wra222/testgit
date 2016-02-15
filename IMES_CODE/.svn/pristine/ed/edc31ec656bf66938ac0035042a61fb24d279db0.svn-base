<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: OQC INPUT(Docking)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012/05/29  Kaisheng             Create
 
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PCAOQCInput.aspx.cs" Inherits="DOCKING_PCAOQCInput" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
   

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServicePCAOQCInputForDocking.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="98%">
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lbpdLine" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:52%" align="left"><asp:Label ID="pdLineContext" runat="server" CssClass="iMes_label_11pt" /></td>
	    <td style="width:12%" align="left"><asp:Label ID="lblLotNo" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:22%" align="left"><asp:Label ID="lblLotNoContext" runat="server" CssClass="iMes_label_11pt" /></td>
    </tr>
   
    </table>
    <table border="0" width="98%">
        <tr>
            <td style="width:12%" align="left"><asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt" /></td>
            <td style="width:20%" align="left"><asp:Label ID="lblTypeContext" runat="server" CssClass="iMes_label_11pt" /></td>
            <td style="width:12%" align="left"><asp:Label ID="lblPCS" runat="server" CssClass="iMes_label_13pt" /></td>
            <td style="width:20%" align="left"><asp:Label ID="lblPCSContext" runat="server" CssClass="iMes_DataEntryLabel" Font-Underline="True" /></td>
            <td style="width:12%" align="left"><asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt" /></td>
            <td style="width:22%" align="left"><asp:Label ID="lblStatusContext" runat="server" CssClass="iMes_DataEntryLabel" Font-Underline="True" /></td>
        </tr>
        <tr>
            <td colspan="6">
                <hr />
            </td>
        </tr>
        <tr>
            <td style="width:12%" align="left"><asp:Label ID="lbltblmbsnList" runat="server" CssClass="iMes_label_13pt" /></td>
            <td style="width:20%" align="left"><asp:Label ID="Labelnull" runat="server" CssClass="iMes_label_11pt" /></td>
            <td style="width:12%" align="left"><asp:Label ID="lblDefineQty" runat="server" CssClass="iMes_label_13pt" /></td>
            <td style="width:20%" align="left"><asp:Label ID="lblDefineQtyContext" runat="server" CssClass="iMes_DataEntryLabel" Font-Underline="True" /></td>
            <td style="width:12%" align="left"><asp:Label ID="lblCheckQty" runat="server" CssClass="iMes_label_13pt" /></td>
            <td style="width:22%" align="left"><asp:Label ID="lblCheckQtyContext" runat="server" CssClass="iMes_DataEntryLabel" Font-Underline="True" /></td>
        </tr>
	 </table>
	 <table border="0" width="98%">
	 <tr>
	    <td style="width:100%">
	      <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline"  >
          <ContentTemplate>
	         <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                GetTemplateValueEnable="False" GvExtHeight="230px" GvExtWidth="100%" OnGvExtRowClick="" Height="220px" 
                OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                onrowdatabound="GridView_RowDataBound" >
                <Columns>
                    <asp:BoundField DataField="MBSN" SortExpression="MBSN" />
                    <asp:BoundField DataField="Checked" SortExpression="Checked" />                    
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
             <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                 <input type="hidden" runat="server" id="station" />
                 <input type="hidden" runat="server" id="lineHidden" />
                 <input type="hidden" runat="server" id="useridHidden" />
                 <input type="hidden" runat="server" id="customerHidden" />
                 <input type="hidden" runat="server" id="txtLotnolst" />
                 <input type="hidden" runat="server" id="TotalPCS" />
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
    
var index = 1;
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount, 10);  //+ 1;

var sFirstInput = "";
var strLine = "";
var InputType ="";
var sKeyCode = "";

var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var invalidMBSN = '<%=this.GetLocalResourceObject(Pre + "_invalidMBSN").ToString()%>';
var mesNoInputLotorMB = '<%=this.GetLocalResourceObject(Pre + "_NoInputlotormb").ToString()%>';
var mesWrongCode = '<%=this.GetLocalResourceObject(Pre + "_mesWrongCode").ToString()%>';

var msgMBSNNotFond = '<%=this.GetLocalResourceObject(Pre + "_NotFound").ToString()%>';
var msgMBSNIsChecked = '<%=this.GetLocalResourceObject(Pre + "_IsChecked").ToString()%>';
var editor;
var customer;
var station;

var intputMBSNforlot;
var statusPrompt = "";

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onload
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	Loading accept input data events apposition focus
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
document.body.onload = function() {
    try {
        //setPdLineCmbFocus();
        sFirstInput = "";
        strLine = "";
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        station = '<%=Request["Station"] %>'
        getAvailableData("processDataEntry");
        //DEBUG ITC1414-0046
        //DEBUG ITC1414-0047 2012/06/04
        getCommonInputObject().focus();
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
function processDataEntry(inputData) 
{
    //1°¢	Length=12-LotNo
    //2°¢	Length=10/11-MBSN
    //3°¢	MBSN-11£¨(5)=°ØM°Ø£¨remove last CRC
    //4°¢	Length=4/6---KeyCode
    //      KeyCode£∫LOCK/UNDO/PASS/UNLOCK/7777
    //5°¢	°Ø7777°Ø£¨‘ÚClear UI£¨include PdLine
    var inputID = inputData.trim();
    try 
    {
        var errorFlag = false;
        //To conduct a preliminary judgment, then submit the background service,pdline
        //DEBUG ITC-1414-0048
        ShowInfo("");
        if (!errorFlag) 
        {
            if (inputID == "7777") 
            {
                //Enter 7777 for the empty table
                //clearTable();
                //getAvailableData("processDataEntry");
                //callNextInput();
                // DEBUG ITC-1414-0045 2012/06/04
                //ShowMessage(mesWrongCode);
                //ShowInfo(mesWrongCode);
                reset();
                callNextInput();
            }
            else 
            {
                if (sFirstInput == "") // Input LotNo or MBSN
                {
                    if (inputID.length == 12)  //LotNo
                    {
                        sFirstInput = inputID;
                        InputType = "LOTNO";
                    }
                    else if ((inputID.length == 10) || (inputID.length == 11)) //MBSN
                    {
                        if ((inputID.length == 11) && (inputID.substr(4, 1) == "M" || inputID.substr(4, 1) == "B")) {
                            sFirstInput = inputID.substr(0, 10);
                        }
                        else //
                        {
                            sFirstInput = inputID;
                        }
                        InputType = "MBSN";
                    }
                    else  //Please LotNo or MBSN
                    {
                        ShowMessage(mesNoInputLotorMB);
                        ShowInfo(mesNoInputLotorMB);
                        callNextInput();
                        return;
                    }
                    //invoke Service
                    WebServicePCAOQCInputForDocking.InputMBorLot(sFirstInput, InputType, editor, station, customer, onInputSucceeded, onInputFailed);
                }
                else  //
                {
                    var StrCMD = inputID.toUpperCase().trim();
                    if ((StrCMD == "LOCK") || (StrCMD == "UNDO") || (StrCMD == "PASS") || (StrCMD == "UNLOCK"))  //KeyCode£∫LOCK/UNDO/PASS
                    {
                        sKeyCode = StrCMD;
                        //invoke Service-save
                        sFirstInput = document.getElementById("<%=lblLotNoContext.ClientID%>").innerText;
                        var line = document.getElementById("<%=lineHidden.ClientID%>").innerText;

                        WebServicePCAOQCInputForDocking.save(sFirstInput, sKeyCode, editor, strLine, customer, onSaveSucc, onSaveFail);
                    }
                    else if ((StrCMD.length == 10) || (StrCMD.length == 11)) //MBSN
                    {
                        if ((StrCMD.length == 11) && (StrCMD.substr(4, 1) == "M" || StrCMD.substr(4, 1) == "B")) {
                            intputMBSNforlot = StrCMD.substr(0, 10);
                        }
                        else //
                        {
                            intputMBSNforlot = StrCMD;
                        }
                        if (statusPrompt != "") {
                            ShowMessage(statusPrompt);
                            ShowInfo(statusPrompt);
                            callNextInput();
                            return;
                        }
                        var getCheckValue = CheckMBSNfromTable(intputMBSNforlot);
                        if (getCheckValue == "NotFound") {
                            ShowMessage( "[" + intputMBSNforlot + "] " + msgMBSNNotFond);
                            ShowInfo("[" + intputMBSNforlot + "] " + msgMBSNNotFond);
                            callNextInput();
                            return;  
                        }
                        else if (getCheckValue == "IsChecked") {
                            ShowMessage("[" + intputMBSNforlot + "] " + msgMBSNIsChecked);
                            ShowInfo("[" + intputMBSNforlot + "] " + msgMBSNIsChecked);
                            callNextInput();
                            return;  
                        }
                        
                        //call webservice ->Checked;
                        //string lotNo, string mbSno, int icheckQty,string line, string editor,string customer
                        WebServicePCAOQCInputForDocking.InsertmbsntoPcbLotCheck(document.getElementById("<%=lblLotNoContext.ClientID%>").innerText,
                                                                                       intputMBSNforlot,
                                                                                       parseInt(document.getElementById("<%=lblDefineQtyContext.ClientID%>").innerText),
                                                                                       strLine,editor,customer,
                                                                                       onInsertSucc,onInsertFail);
                    } 
                    else {
                        //Wrong Code;
                        ShowMessage(mesWrongCode);
                        ShowInfo(mesWrongCode);
                        callNextInput();
                    }
                }    
            }
        }
    } 
    catch (e) 
    {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setStatus
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	Set page all MB scanning are related to the state of the control
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setStatus(status) {
    try {
        getPdLineCmbObj().disabled = status;
        //disabled all with ProdId scan related controls
    } catch (e) {
        alert(e.description);
    }
}

function setInformation(strLotNo, strLineinfo, strType, strQty, strStatus,strLineHidden,strDefineQty,strCheckedQty)
{
    document.getElementById("<%=lblLotNoContext.ClientID%>").innerText = strLotNo;
    document.getElementById("<%=pdLineContext.ClientID%>").innerText = strLineinfo;
    document.getElementById("<%=lblTypeContext.ClientID%>").innerText = strType;
    document.getElementById("<%=lblPCSContext.ClientID%>").innerText = strQty;
    document.getElementById("<%=lblStatusContext.ClientID%>").innerText = strStatus;
    document.getElementById("<%=lineHidden.ClientID%>").innerText = strLineHidden;
    document.getElementById("<%=lblDefineQtyContext.ClientID%>").innerText = strDefineQty;
    document.getElementById("<%=lblCheckQtyContext.ClientID%>").innerText = strCheckedQty;
    strLine = strLineHidden;
}

function setTable(mBlist,checkedList) 
{
    //DEBUG ITC-1414-0049 2012/06/04
     //var rowArray = new Array();
     var rw;
     for (var i = 0; i < mBlist.length; i++)
     {
        //DEBUG ITC-1414-0049 2012/06/04
        var rowArray = new Array();
        rowArray.push(mBlist[i], checkedList[i]);
        //add data to table
        if (i < initRowsCount) 
        {
            eval("ChangeCvExtRowByIndex_" + gvClientID + "(rowArray, false, i+1);");
            //eval("ChangeCvExtRowByIndex_" + gvClientID + "(rowArray, false, i);");
            //setSrollByIndex(i, true, gvClientID);
        }
        else 
        {
            eval("rw = AddCvExtRowToBottom_" + gvClientID + "(rowArray, false);");
            //setSrollByIndex(i, true, gvClientID);
        }
        index++;
     }
}

function FindMBSNfromTable(strmbsn) {
    var subFindFlag = false;
    var table = document.getElementById(gvClientID);
    var row;
    var isExist = false;
    eval("setRowNonSelected_" + gvClientID + "()");

    for (var i = 1; i < table.rows.length; i++) {
         if (table.rows[i].cells[0].innerText.trim() == strmbsn) {
             row = eval("setScrollTopForGvExt_" + gvClientID + "('" + table.rows[i].cells[0].innerText + "',0,'','MUTISELECT')");
             isExist = true;
             break;
         }
     }
    return isExist;
}
function CheckMBSNfromTable(strmbsn) {
    var returnvalue = "NotFound";
    var table = document.getElementById(gvClientID);
    var isExist = false;
    for (var i = 1; i < table.rows.length; i++) {
        if (table.rows[i].cells[0].innerText.trim() == strmbsn) {
            isExist = true;
            if (table.rows[i].cells[1].innerText.trim() == "1") {
                returnvalue = "IsChecked";
                eval("setRowNonSelected_" + gvClientID + "()");
                var row = eval("setScrollTopForGvExt_" + gvClientID + "('" + table.rows[i].cells[0].innerText + "',0,'','MUTISELECT')");
            } 
            else {
                returnvalue = "IsGoOnOK";
            }
            break;
        }
    }
    return returnvalue;
}
function onInputSucceeded(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            ShowInfo("");
            //service method not return
            //Need to clear interface
            reset();
            //Make the page for all ProdId scanning are related to the control of enabled
            ////setStatus(false);
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 12) && (result[0] == SUCCESSRET)) //OK
        {
            setInformation(result[1], result[2], result[3], result[4], result[5], result[7], result[9].toString(), result[10].toString());

            setTable(result[6], result[8]);
            statusPrompt = result[11];
            if (statusPrompt !="")
            {
                ShowMessage(statusPrompt);
                ShowInfo(statusPrompt);
                callNextInput();
                return;
            }
            if (sFirstInput.length == 12) //LOT
                callNextInput();
            else {

               
                if (statusPrompt != "") {
                    ShowMessage(statusPrompt);
                    ShowInfo(statusPrompt);
                    callNextInput();
                    return;
                }
                intputMBSNforlot = sFirstInput;
                var getCheckValue = CheckMBSNfromTable(intputMBSNforlot);
                if (getCheckValue == "NotFound") {
                    ShowMessage("[" + intputMBSNforlot + "] " + msgMBSNNotFond);
                    ShowInfo("[" + intputMBSNforlot + "] " + msgMBSNNotFond);
                    callNextInput();
                    return;
                }
                else if (getCheckValue == "IsChecked") {
                    ShowMessage("[" + intputMBSNforlot + "] " + msgMBSNIsChecked);
                    ShowInfo("[" + intputMBSNforlot + "] " + msgMBSNIsChecked);
                    callNextInput();
                    return;
                }
                WebServicePCAOQCInputForDocking.InsertmbsntoPcbLotCheck(document.getElementById("<%=lblLotNoContext.ClientID%>").innerText,
                                                                                       sFirstInput,
                                                                                       parseInt(document.getElementById("<%=lblDefineQtyContext.ClientID%>").innerText),
                                                                                       strLine, editor, customer,
                                                                                       onInsertSucc, onInsertFail);
            }

            //Test Table
            //for (var i < index; i  > 0; i--) {
            //    gvTable.rows[i].cells[0].swapNode(gvTable.rows[i - 1].cells[0]);
            //}
            callNextInput();
        }
        else {
            callNextInput();
        }
    }
    catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }    
}

function onInputFailed(result)
{
    endWaitingCoverDiv();
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
    sFirstInput = "";
    intputMBSNforlot = "";
    setInformation("", "", "", "", "", "","","");
    
    callNextInput();
}
//onInsertSucc,onInsertFail
function onInsertSucc(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            ShowInfo("");
            reset();
            ////setStatus(false);
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 5) && (result[0] == SUCCESSRET)) //OK
        {
            //setInformation(result[1], result[2], result[3], result[4], result[5], result[7], result[9].toString(), result[10].toString());
            clearTable();
            setTable(result[1], result[2]);
            document.getElementById("<%=lblCheckQtyContext.ClientID%>").innerText = result[3].toString();
            FindMBSNfromTable(intputMBSNforlot);            
             //Test Table
            //for (var i < index; i  > 0; i--) {
            //    gvTable.rows[i].cells[0].swapNode(gvTable.rows[i - 1].cells[0]);
            //}
            if (result[4] == "GotoPass") {
                //intputMBSNforlot -PASS
                WebServicePCAOQCInputForDocking.save(document.getElementById("<%=lblLotNoContext.ClientID%>").innerText, 
                                                            "PASS", editor, strLine, customer, onSaveSucc, onSaveFail);
            }
            else {
                callNextInput(); 
            }
            
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
function onInsertFail(result) {
    endWaitingCoverDiv();
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
    
    intputMBSNforlot = "";
    callNextInput();
}

function ClearData() {
    sFirstInput = "";
    setInformation("", "", "", "", "", "","","");
    
    clearTable();
    
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSaveSucceed
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	A complete preservation of success
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSaveSucc(result) 
{
     try  
    {
        endWaitingCoverDiv();
        if  (result == SUCCESSRET)
        {
            var SuccessItem = "[" + sFirstInput + "]"; 
            ClearData(); 
            ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
            callNextInput();
        }       
        else
        {
            ClearData(); 
            ShowMessage(result[0]);
            ShowInfo(result[0]);
            callNextInput();
        }
    }
    catch (e) 
    {
            endWaitingCoverDiv();
            alert(e.description);
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onMBFail
//| Author		:	Lucy liu
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
        //DEBUG ITC-1414-0076 Kaisheng 2012/06/14
        //----------------------
        //reset();
        ClearData(); 
        //----------------------
        callNextInput();

    } catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
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
    try 
    {
        eval("setRowNonSelected_" + gvClientID + "()");
        ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount+1);
        //ClearGvExtTable("<%=gridview.ClientID%>", DEFAULT_ROW_NUM);
        //The header is a zeroth line, data line is 1, and index = 1, said from the first row is added
        index = 1;
        setSrollByIndex(0, false, "<%=gridview.ClientID%>");
        //Clear client save table structure array
        //saveDefectArray.length = 0;
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
         //DEBUG ITC-1414-0057 2012/06/04
        ClearData();
        //setInformation("","","","","","","","");
        ShowInfo("");
        callNextInput();
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

function onClearSucceeded(result) 
{
    try 
    {
        if (result == null) 
        {
            ShowInfo("");
            //service -The method does not return
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 2) && (result[0] == SUCCESSRET)) 
        {
            //            reset();
            //           callNextInput();
            ShowInfo("");
        }
        else 
        {
            ShowInfo("");
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
    }
    catch (e) 
    {
        alert(e.description);
    }
}

function onClearFailed(error) 
{
    try 
    {
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        //Need to clear interface
        reset();
        callNextInput();
    }
    catch (e) 
    {
        alert(e.description);
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Lucy liu
//| Create Date	:	01/24/2010
//| Description	:	Exit page called
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage() {
    reset();
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
}


//-->
</script>

</asp:Content>
