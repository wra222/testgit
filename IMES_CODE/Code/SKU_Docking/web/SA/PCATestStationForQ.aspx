<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: PCA Test Station(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012/05/24  Kaisheng             Create
 
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PCATestStationForQ.aspx.cs" Inherits="SA_PCATestStation" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
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
                    <HeaderTemplate>
                        <asp:CheckBox ID="HeaderChk"  runat="server" onclick="ChkAllClick(this)" Text="ALL" ForeColor="White" nowrap="noWrap" BackColor="Gray" BorderStyle="None" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="RowChk" runat="server" onclick="CheckClick(this)"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                    <asp:BoundField DataField="LotNo" SortExpression="LotNo" />
                    <asp:BoundField DataField="Type" SortExpression="Type" />
                    <asp:BoundField DataField="MBCode" SortExpression="MBCode" />
                    <asp:BoundField DataField="Qty" SortExpression="Qty" />
                </Columns>
             </iMES:GridViewExt>
            </ContentTemplate>   
          </asp:UpdatePanel> 
	    </td>
	   
    </tr>
    </table>
    
    <table border="0" cellspacing="0" cellpadding="0" width="98%">
    <tr>
        <td nowrap="noWrap" style="width:15%" align="left"><asp:Label ID="lbTotalPCS" runat="server" CssClass="iMes_label_13pt" />
        </td>
        <td style="width:85%" align="left">
        <asp:UpdatePanel ID="updatePanelPCS" runat="server" UpdateMode="Conditional" RenderMode="Inline">
         <ContentTemplate>
            <asp:Label ID="txtTotalPCS" runat="server"   CssClass="iMes_DataEntryLabel" Font-Underline="True" />
          </ContentTemplate>   
            </asp:UpdatePanel>   
        </td>
    </tr>
    <tr>
	    <td nowrap="noWrap" style="width:15%" align="left"><asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></td>
	    <td style="width:85%" align="left"> <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true"  Width="99%"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
             <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                 <input type="hidden" runat="server" id="station" />
                 <input type="hidden" runat="server" id="useridHidden" />
                 <input type="hidden" runat="server" id="customerHidden" />
                 <input type="hidden" runat="server" id="txtLotnolst" />
                 <input type="hidden" runat="server" id="TotalPCS" />
                  <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" ></button>
                  <button id="btnFru" runat="server" onserverclick="btnFru_Click" style="display: none" ></button>
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
var AllowPass = 1;

var SaveLotNoArray = new Array();

var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelTestStation = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectTestStation").ToString()%>';
var mesNoSelLotNo = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelLotNo").ToString()%>';
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
var meserrwrongcode = '<%=this.GetLocalResourceObject(Pre + "_mesWrongCode").ToString()%>';

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
        else if (inputID != "9999") 
        {
            //DEBUG ITC-1414-0002
            errorFlag = true;
            ShowMessage(meserrwrongcode);
            ShowInfo(meserrwrongcode);
            callNextInput();
        
        }
        
        else if (GetSelectedLotNoList()==false) {
            ShowMessage(mesNoSelLotNo);
            ShowInfo(mesNoSelLotNo);
            errorFlag = true;
            getAvailableData("processDataEntry");
        }
        
        if (!errorFlag) 
        {
            //The motherboard has scanned, successor should input the defect
            if (inputID == "9999") 
            {
                //beginWaitingCoverDiv();
                document.getElementById("<%=btnReset.ClientID%>").click();
                getAvailableData("processDataEntry");
                //WebServicePCATestStation.savePage(inputID, saveDefectArray, onSaveSucceed, onSaveFail);
            } 
            else 
            {
                //DEBUG ITC-1414-0002
                ShowMessage(meserrwrongcode);
                ShowInfo(meserrwrongcode);
                callNextInput();
                //getAvailableData("processDataEntry");
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
                      
            //Need to clear interface
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
        //saveDefectArray.length = 0;
    } 
    catch (e) 
    {
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
            document.getElementById(gvClientID + "_" + i + "_RowChk").checked = headChk.checked;
        }
        getCommonInputObject().value = "";
        //Set fast control focus
        getCommonInputObject().focus();
    } catch (e) {
        alert(e.description);
    }
}

function CheckClick(singleChk, id) {

    try {
        gvTable = document.getElementById(gvClientID);
        if (singleChk.checked) {
            for (var i = 0; i < gvTable.rows.length - 1; i++) {
                if (gvTable.rows[i + 1].cells[1].innerText != " ") {
                    if (!document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                        getCommonInputObject().value = "";
                        //Set fast control focus
                        getCommonInputObject().focus();
                        return;
                    }
                }
            }
            document.getElementById(gvHeaderID + "_HeaderChk").checked = true;
        }
        else {
            document.getElementById(gvHeaderID + "_HeaderChk").checked = false;
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

            if (document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                if (gvTable.rows[i + 1].cells[1].innerHTML != "&nbsp;") {
                    SaveLotNoArray.push(gvTable.rows[i + 1].cells[1].innerText);
                    strlist = strlist + gvTable.rows[i + 1].cells[1].innerText + "+";
                }
            }
        }
        cliLotnolstID.value = strlist;
        if (SaveLotNoArray.length == 0) {
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
<script language="javascript" type="text/javascript">
    //DEBUG ITC-144-0005--post Data error  2012/05/31  
    Sys.Application.add_load(
     function() {
         var form = Sys.WebForms.PageRequestManager.getInstance()._form;
         form._initialAction = form.action = window.location.href;
     }
 );
</script>

</asp:Content>
