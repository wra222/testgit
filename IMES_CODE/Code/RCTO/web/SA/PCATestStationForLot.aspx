<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: PCA Test Station(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012/01/10  Kaisheng             Create
 
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PCATestStationForLot.aspx.cs" Inherits="SA_PCATestStation" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
   

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServicePCATestStation.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="98%">
     
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lbtestStation" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td colspan="6"><iMES:CmbTestStation ID="cmbTestStation" runat="server" Width="100" IsPercentage="true"/></td>
    </tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lbpdLine" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td colspan="6"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"/></td>
    </tr>
    <tr>
	    <td colspan="7">
	    <hr>
	    </td>
    </tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lbMBSn" runat="server"  CssClass="iMes_label_13pt" /></td>
	    <td style="width:30%" align="left"><asp:Label ID="txtMBSn" runat="server"   CssClass="iMes_label_11pt" /></td>
	    <td></td>
	    <td style="width:12%"  align="left"><asp:Label ID="lb111" runat="server"  CssClass="iMes_label_13pt" /></td>
	    <td align="left" colspan="2"><asp:Label ID="txt111" runat="server"  CssClass="iMes_label_11pt" /></td>
	    <td></td>
    </tr>
    </table>
    <table border="0" width="98%">
    <tr>
        <td style="width:12%" align="left"><asp:Label ID="lblotlist" runat="server" CssClass="iMes_label_13pt" /></td>
        <td style="width:12%"></td>
        <td style="width:12%"></td>
        <td style="width:14%"></td>
        <td style="width:12%" align="left"><asp:Label ID="lbPassQty" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:12%" align="left"><asp:Label ID="txtPassQty" runat="server"  Text="0" CssClass="iMes_label_11pt"/></td>
	    <td style="width:12%" align="left"><asp:Label ID="lbFailQty" runat="server" CssClass="iMes_label_13pt" /></td>
	    <td style="width:12%" align="left"><asp:Label ID="txtFailQty" runat="server" Text="0" CssClass="iMes_label_11pt"/></td>
	 </tr>   
    </table>
    <table border="0" cellspacing="0" cellpadding="0" width="98%">
       <tr>
         <td rowspan="3" style="width:48%">
            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                  <ContentTemplate>
	                 <iMES:GridViewExt ID="GridViewLot" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                        GetTemplateValueEnable="False" GvExtHeight="251px" GvExtWidth="100%" OnGvExtRowClick="" Height="245px" 
                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                        onrowdatabound="GridViewLot_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="LotNo" SortExpression="LotNo" />
                            <asp:BoundField DataField="Type" SortExpression="Type" />
                            <asp:BoundField DataField="MBCode" SortExpression="MBCode" />
                            <asp:BoundField DataField="Qty" SortExpression="Qty" ItemStyle-Font-Size="15pt" ItemStyle-ForeColor="blue" ItemStyle-Font-Bold="True" />
                        </Columns>
                     </iMES:GridViewExt>
                    </ContentTemplate>   
               </asp:UpdatePanel> 
         </td>
            
            <td style="width:2%">
            </td>
            
            <td style="width:48%">
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" >
                  <ContentTemplate>
	                 <iMES:GridViewExt ID="gridviewMB" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                        GetTemplateValueEnable="False" GvExtHeight="110px" GvExtWidth="100%" OnGvExtRowClick="" Height="105px" 
                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                        onrowdatabound="GridViewExtMB_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="MBCode" SortExpression="MBCode" />
                            <asp:BoundField DataField="PassQty" SortExpression="PassQty" />
                        </Columns>
                     </iMES:GridViewExt>
                    </ContentTemplate>   
               </asp:UpdatePanel> 
            </td>
       </tr>
       <tr>
         <td style="width:2%">
            </td>
		 <td style="width:48%"><asp:Label ID="lbDefectList" runat="server" CssClass="iMes_label_13pt" /></td>
            
       </tr>

       <tr>
            <td style="width:2%">
            </td>
            
            <td style="width:48%">
                <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                  <ContentTemplate>
	                 <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                        GetTemplateValueEnable="False" GvExtHeight="110px" GvExtWidth="100%" OnGvExtRowClick="" Height="105px" 
                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                        onrowdatabound="GridViewExt1_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="DefectId" SortExpression="DefectId" />
                            <asp:BoundField DataField="DefectDescr" SortExpression="DefectDescr" />
                        </Columns>
                     </iMES:GridViewExt>
                    </ContentTemplate>   
                  </asp:UpdatePanel> 
            </td>
       </tr>
    </table>
    <table border="0" cellspacing="0" cellpadding="0" width="98%">
    <tr>
        <td nowrap="noWrap" style="width:14%" align="left"><asp:Label ID="lbTotalPCS" runat="server" CssClass="iMes_label_13pt" />
        </td>
        <td style="width:70%" align="left">
            <asp:UpdatePanel ID="updatePanelPCS" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Label ID="txtTotalPCS" runat="server"   CssClass="iMes_DataEntryLabel" Font-Underline="True" />
                </ContentTemplate>  
            </asp:UpdatePanel>         
        </td>
        <td  align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="FruCheck"  AutoPostBack="false" 
                            CssClass="iMes_label_11pt" BackColor="Transparent" BorderStyle="None" /> 
        </td>
    </tr>
    <tr>
	    <td nowrap="noWrap" style="width:14%" align="left"><asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></td>
	    <td style="width:70%" align="left"> <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true"  Width="99%"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
             <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                 <input type="hidden" runat="server" id="station" />
                 <input type="hidden" runat="server" id="useridHidden" />
                 <input type="hidden" runat="server" id="customerHidden" />
                  <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" ></button>
                  <button id="btnFru" runat="server" onserverclick="btnFru_Click" style="display: none" ></button>
                  <button id="btnRefresh" runat="server" onserverclick="btnRefresh_Click" style="display: none" ></button>
                </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
	     <td  align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="nineSelect"  AutoPostBack="false" 
                            CssClass="iMes_label_11pt" BackColor="Transparent" BorderStyle="None" 
                            oncheckedchanged="nineSelect_CheckedChanged"></asp:CheckBox>
                       <input type="hidden" runat="server" id="pCode" />
                    </td>
    </tr>
       
    </table>
    </center> 
    
</div>

 

<script type="text/javascript">
<!--
var GridViewExt1ClientID = "<%=gridview.ClientID%>";
var GridViewExtMBClientID = "<%=gridviewMB.ClientID%>";
var index = 1;
var indexMB = 1;
var strRowsCount = "<%=initRowsCount%>";
var strRowsCountForLot = "<%=initRowsCountForLot%>";
var initRowsCount = parseInt(strRowsCount,10) + 1;
var initRowsCountForLot = parseInt(strRowsCountForLot, 10) + 1;
var defectArray = new Array();
var saveDefectArray = new Array();
var passQty = 0;
var failQty = 0;
var AllowPass = 1;
var scanNine4Flag = false;
var FruCheckFlag = false;

var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelTestStation = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectTestStation").ToString()%>';
var mesNoInputMB = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputMB").ToString()%>';
var mesDupData = '<%=this.GetLocalResourceObject(Pre + "_mesDupData").ToString()%>';
var mesIllegalDefect = '<%=this.GetLocalResourceObject(Pre + "_mesIllegalDefect").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var mesNotAllowPass = '<%=this.GetLocalResourceObject(Pre + "_mesNotAllowPass").ToString()%>';
var beSureSelectsation = '<%=this.GetLocalResourceObject(Pre + "_beSureSelectsation").ToString()%>';
var invalidMBSN = '<%=this.GetLocalResourceObject(Pre + "_invalidMBSN").ToString()%>';
var meserrStation19 = '<%=this.GetLocalResourceObject(Pre + "_meserrStation19").ToString()%>';
var meserrStation1A = '<%=this.GetLocalResourceObject(Pre + "_meserrStation1A").ToString()%>';

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
        getAvailableData("processDataEntry");
    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	processDataEntry
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	After inputting the data, processing
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function processDataEntry(inputData) {
    PlaySoundClose();
    var inputID = inputData.trim();
    try {
        var errorFlag = false;
        //To conduct a preliminary judgment, then submit the background service,pdline,teststation
        if (getTestStationCmbValue() == "") 
        {
            //alert(mesNoSelTestStation);
            ShowMessage(mesNoSelTestStation);
            ShowInfo(mesNoSelTestStation);
            errorFlag = true;
            setTestStationCmbFocus();
            getAvailableData("processDataEntry");
        } 
        else if (getPdLineCmbValue() == "") 
        {
            //alert(mesNoSelPdLine);
            ShowMessage(mesNoSelPdLine);
            ShowInfo(mesNoSelPdLine);
            errorFlag = true;
            setPdLineCmbFocus();
            getAvailableData("processDataEntry");
        }
        if (!errorFlag) 
        {
            if (inputID == "7777") 
            {
                ////Enter 7777 for the empty table
                //clearTable();
                ////getAvailableData("processDataEntry");
                //callNextInput();
                //2012/08/23 modify --guanwei:7777->clear session,and mbsn->Null
                ExitPage();
                reset();
                callNextInput();
            }
            else if (document.getElementById("<%=txtMBSn.ClientID%>").innerText == "") 
            {
                //Motherboard not scan, and then input 9999, report error
                if (inputID == "9999") 
                {
                    //alert(mesNoInputMB);
                    ShowMessage(mesNoInputMB);
                    ShowInfo(mesNoInputMB);
                    getAvailableData("processDataEntry");
                } 
                else 
                {
                    //f (((inputID.length == 10) || (inputID.length == 11)) && (inputID.substring(4, 5) == "M"))
                    //DEBUG ITC-1414-0018 MB input Check
                    if ((inputID.length == 10) || (inputID.length == 11)) //MBSN
                    {
                        var sFirstInput = "";
                        if ((inputID.length == 11) && (inputID.substr(4, 1) == "M")) {
                            sFirstInput = inputID.substr(0, 10);
                        }
                        else //
                        {
                            sFirstInput = inputID;
                        }
                        
                        //The motherboard is empty, that is not scan motherboard, this input data is considered to be the motherboard.
                        //beginWaitingCoverDiv();
                        //Clear client holds the defect array
                        defectArray.length = 0;
                        //            <bug>
                        //                BUG NO:ITC-1103-0340
                        //                REASON:By hiding the control to save the editor value
                        //            </bug>
                        //WebServicePCATestStation.inputMBCodeForLot(getPdLineCmbValue(), getTestStationCmbValue(), SubStringSN(inputID, "MB"), document.getElementById("<%=useridHidden.ClientID%>").value, document.getElementById("<%=customerHidden.ClientID%>").value, onMBSucceed, onMBFail);
                        WebServicePCATestStation.inputMBCodeForLot(getPdLineCmbValue(), getTestStationCmbValue(), sFirstInput, document.getElementById("<%=useridHidden.ClientID%>").value, document.getElementById("<%=customerHidden.ClientID%>").value, onMBSucceed, onMBFail);
                     }
                     else
                     {
                        //invalidMBSN
                        ShowMessage(invalidMBSN);
                        ShowInfo(invalidMBSN);
                        callNextInput();
                     }
                }
            }
            else 
            {
                //The motherboard has scanned, successor should input the defect
                if (inputID == "9999") 
                {
                    if (AllowPass == 'N' && saveDefectArray.length == 0) 
                    {
                        //alert(mesNotAllowPass);
                        ShowMessage(mesNotAllowPass);
                        ShowInfo(mesNotAllowPass);
                        //getAvailableData("processDataEntry");
                        callNextInput();
                        return;
                    }
                    var sCheckStation  ="";
                    sCheckStation = CheckStation191A();
                    if (sCheckStation !="")
                    {
                        ShowMessage(sCheckStation);
                        ShowInfo(sCheckStation);
                        //getAvailableData("processDataEntry");
                        callNextInput();
                        return;
                    }
                    beginWaitingCoverDiv();
                    FruCheckFlag = document.getElementById("<%=FruCheck.ClientID%>").checked;
                    WebServicePCATestStation.savePageForLot(document.getElementById("<%=txtMBSn.ClientID%>").innerText, saveDefectArray, FruCheckFlag,onSaveSucceed, onSaveFail);
                } 
                else 
                {
                    //The input information is considered to be defect
                    //Test defect legitimacy
                    checkDefect(inputID);
                    //Continue to receive input data
                    getAvailableData("processDataEntry");
                }
            }
        }
    } catch (e) {
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
        getTestStationCmbObj().disabled = status;
        //disabled all with ProdId scan related controls
    } catch (e) {
        alert(e.description);
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onMBSucceed
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	Motherboard, scanning, acquisition of Model
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onMBSucceed(result) {
    try {
        //endWaitingCoverDiv();
        if (result == null) {
            ShowInfo("");
            //service method not return
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 7) && (result[0] == SUCCESSRET)) {

            document.getElementById("<%=txtMBSn.ClientID%>").innerText = result[1];
            document.getElementById("<%=txt111.ClientID%>").innerText = result[2];
            //Receiving incoming defectLst
            for (i = 0; i < result[3].length; i++) {
                defectArray.push(result[3][i]);
            }
            AllowPass = result[4];
            //disabled all with ProdId scan related controls
            setStatus(true);
            if (result[6] == "NO!") {
                callNextInput();
                ShowInfo("");
            }
            else {
                ShowInfo("");
                ShowMessage(result[6]);
                ShowInfo(result[6]);
                callNextInput();
            }
            scanNine4Flag = document.getElementById("<%=nineSelect.ClientID%>").checked;
            FruCheckFlag = document.getElementById("<%=FruCheck.ClientID%>").checked;
            if (scanNine4Flag) {
                 var sCheckStation = CheckStation191A();
                 if (sCheckStation !="")
                 {
                    ShowMessage(sCheckStation);
                    ShowInfo(sCheckStation);
                        //getAvailableData("processDataEntry");
                    callNextInput();
                  }
                  else
                  {
                    beginWaitingCoverDiv();
                    WebServicePCATestStation.savePageForLot(document.getElementById("<%=txtMBSn.ClientID%>").innerText, saveDefectArray, FruCheckFlag, onSaveSucceed, onSaveFail);
                  }
            }
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
        //        endWaitingCoverDiv();
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
function onMBFail(error) {
    try {
        //        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    } catch (e) {
        alert(e.description);
        //        endWaitingCoverDiv();
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkDefect
//| Author		:	Lucy liu
//| Create Date	:	10/27/2009
//| Description	:	Test defect legitimacy
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkDefect(defectCode) 
{
    try 
    {
        var errorFlag = false;
        var bFind = false;
        var description = "";
        //To determine defect legitimate
        for (i = 0; i < defectArray.length; i++) 
        {

            if (defectCode == defectArray[i].id.toUpperCase()) 
            {
                bFind = true;
                description = defectArray[i].description;
                break;
            }
        }
        if (bFind) 
        {
            //Judge to add Defect exists in the table? if already have ,show error
            var table = document.getElementById(GridViewExt1ClientID);
            for (var i = 1; i < table.rows.length; i++) {
                if (table.rows[i].cells[0].innerText == defectCode) {
                    alert(mesDupData);
                    errorFlag = true;
                    break;
                }
            }
        }
        else 
        {
            alert(mesIllegalDefect);
            errorFlag = true;
        }
        if (!errorFlag) {
            //Added to the table
            var rowInfo = new Array();
            rowInfo.push(defectCode);
            rowInfo.push(description);
            AddRowInfo(rowInfo);
            //Add to the client structure array
            saveDefectArray.push(defectCode);
        }
    } catch (e) {
        alert(e.description);
    }
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
        else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
        {
            //According to the size of the number of Qty defectArray
            if (saveDefectArray.length == 0) 
            {
                passQty++;
                document.getElementById("<%=txtPassQty.ClientID%>").innerText = passQty + "";
                //ADD by Kaisheng, UC update: Add table show MBcode & pass Qty
                CheckandShowMBCodeTable();
                ShowSuccessfulInfo(true, "[" + document.getElementById("<%=txtMBSn.ClientID%>").innerText + "] " + msgSuccess);
            } else 
            {
                failQty++;
                document.getElementById("<%=txtFailQty.ClientID%>").innerText = failQty + "";
                ShowInfo("[" + document.getElementById("<%=txtMBSn.ClientID%>").innerText + "] " + msgSuccess);
                PlaySound();                
            }

            document.getElementById("<%=btnRefresh.ClientID%>").click();
            
            //Need to clear interfaced
            reset();
            //Make the page for all ProdId scanning are related to the control of enabled
            setStatus(false);
            callNextInput();
            //ShowInfo(msgSuccess);
            //ShowSuccessfulInfo(true);
        }
        else 
        {
            ShowInfo("");
            //Need to clear interface
            reset();
            //Make the page for all ProdId scanning are related to the control of enabled
            setStatus(false);
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

function CheckandShowMBCodeTable() 
{
    var mbcodeAll = document.getElementById("<%=txtMBSn.ClientID%>").innerText;
    var mbcode = "";
    //mbcode = mbcode.substr(0, 2);
    if (mbcodeAll.substr(4, 1) == "M")
        mbcode = mbcodeAll.substr(0, 2);
    else if (mbcodeAll.substr(5, 1) == "M")
        mbcode = mbcodeAll.substr(0, 3);
    else
        mbcode = mbcodeAll.substr(0, 2);
    var tblMB = "<%=gridviewMB.ClientID %>";
    var tableMB = document.getElementById(tblMB);
    var subFindFlag = false;
    var subScanQty;
    var strpassQty = "1";

    for (var i = 1; i < tableMB.rows.length; i++) 
    {
        if (tableMB.rows[i].cells[0].innerText == mbcode) 
        {
            subFindFlag = true;
            subScanQty = parseInt(tableMB.rows[i].cells[1].innerText, 10);
            subScanQty = subScanQty + 1;
            tableMB.rows[i].cells[1].innerText = subScanQty.toString();
            break;
        }
    }
    if (subFindFlag ==false) 
    {
        var rowMBInfo = new Array();
        rowMBInfo.push(mbcode);
        rowMBInfo.push(strpassQty);
        AddMBRowInfo(rowMBInfo); 
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
        reset();
        //Make the page and all MB scan associated control enable
        setStatus(false);
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
        ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
        //The header is a zeroth line, data line is 1, and index = 1, said from the first row is added
        index = 1;
        setSrollByIndex(0, false, "<%=gridview.ClientID%>");
        //Clear client save table structure array
        saveDefectArray.length = 0;
    } 
    catch (e) 
    {
        alert(e.description);
    }
}

function clearTableOther() {
    try {
        ClearGvExtTable("<%=gridviewMB.ClientID%>", initRowsCount);
        //The header is a zeroth line, data line is 1, and index = 1, said from the first row is added
        indexMB = 1;
        setSrollByIndex(0, false, "<%=gridviewMB.ClientID%>");
        //
        
        //ClearGvExtTable("<%=GridViewLot.ClientID%>", initRowsCountForLot);
        //index = 1;
        //setSrollByIndex(0, false, "<%=GridViewLot.ClientID%>");
        
    }
    catch (e) {
        alert(e.description);
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
function AddMBRowInfo(RowArray) 
{
    try 
    {
        if (indexMB < initRowsCount) 
        {
            eval("ChangeCvExtRowByIndex_" + GridViewExtMBClientID + "(RowArray,false, indexMB)");
        }
        else 
        {
            eval("AddCvExtRowToBottom_" + GridViewExtMBClientID + "(RowArray,false)");
        }
        setSrollByIndex(indexMB, false, GridViewExtMBClientID);
        indexMB++;
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
        document.getElementById("<%=txtMBSn.ClientID%>").innerText = "";
        document.getElementById("<%=txt111.ClientID%>").innerText = "";
        clearTable();
        //clearTableOther();
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

window.onbeforeunload = function() 
{
    ExitPage();
}

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
function ExitPage() 
{
    if (document.getElementById("<%=txtMBSn.ClientID%>").innerText != "") {
        //            <bug>
        //                BUG NO:ITC-1103-0136
        //                REASON:station==teststation
        //            </bug>
        WebServicePCATestStation.Cancel(document.getElementById("<%=txtMBSn.ClientID%>").innerText, getTestStationCmbValue(), onClearSucceeded, onClearFailed);
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
    document.getElementById("<%=txtFailQty.ClientID%>").innerText = "0";
    document.getElementById("<%=txtPassQty.ClientID%>").innerText = "0";
    //DEBUG ITC1414-0099
    passQty = 0;
    failQty = 0;
    getCommonInputObject().value = "";
    reset();
    document.getElementById("<%=btnReset.ClientID%>").click();
}
//DEBUG ITC-1360-0277 Add Function to check  conditions for station =19 or station =1A
function CheckStation191A() 
{
    var sCkeckresult = ""; //false do not block
    var curStation = getTestStationCmbValue();
    curStation = curStation.trim();
    if ((curStation =='19') && (saveDefectArray.length == 0))    
    {
        sCkeckresult = meserrStation19;
    }
    else if ((curStation =='1A') && (saveDefectArray.length != 0))
    {
         sCkeckresult = meserrStation1A;
    }
    else
    {
      sCkeckresult = "";  
    }
    return sCkeckresult;
}

function FruRuninTest() 
{
    if (confirm(beSureSelectsation)) {
        document.getElementById("<%=btnFru.ClientID%>").click(); 
        callNextInput();
    }
    else {
        //getTestStationCmbObj().setSelected(0);
        document.getElementById("<%=cmbTestStation.ClientID%>" + "_DropDownList1").selectedIndex = 0;
        //alert( document.getElementById("<%=cmbTestStation.ClientID%>" + "_DropDownList1").SelectedValue);
        //document.getElementById("<%=cmbTestStation.ClientID%>" + "_DropDownList1").selectchange(null, null);
        document.getElementById("<%=btnReset.ClientID%>").click();  
    }
}
//DEBUG ITC-1414-0035 2012/05/31 
function stationchange() {
    document.getElementById("<%=txtFailQty.ClientID%>").innerText = "0";
    document.getElementById("<%=txtPassQty.ClientID%>").innerText = "0";
    //DEBUG ITC1414-0099
    passQty = 0;
    failQty = 0;
    clearTableOther();
    ShowInfo("");
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    
}

function ChangeFocus() {
    getCommonInputObject().focus();
}
 

function PlaySound() {
    var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
    var obj = document.getElementById("bsoundInModal");
    obj.src = sUrl;
}
function PlaySoundClose() {

    var obj = document.getElementById("bsoundInModal");
    obj.src = "";
}
//-->
</script>
</asp:Content>
