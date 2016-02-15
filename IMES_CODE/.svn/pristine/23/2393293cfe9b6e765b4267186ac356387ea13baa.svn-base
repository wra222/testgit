 <%--   
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: FA Test Station(FA)
 * UI:CI-MES12-SPEC-FA-UI FA Test Station.docx --2011/10/20 
 * UC:CI-MES12-SPEC-FA-UC FA Test Station.docx --2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * TODO:
 * Known issues:
 */
 --%>
 
<%@ MasterType VirtualPath="~/MasterPage.master"%>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="FATestStation.aspx.cs" Inherits="FA_FATestStation" Title="拸梓枙珜" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceFATestStation.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                     <td style="width: 15%" align="left">
                        <asp:Label ID="lbtestStation" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td colspan="5">
                        <iMES:CmbTestStation ID="cmbTestStation" runat="server" Width="98" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbpdLine" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="98" IsPercentage="true" />
                    </td>

                </tr>
                
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbPassQty" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td style="width: 30%" align="left">
                        <asp:Label ID="txtPassQty" runat="server" Text="0" CssClass="iMes_label_11pt" />
                    </td>
                    <td>
                    </td>
                    <td style="width: 12%" align="left">
                        <asp:Label ID="lbFailQty" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td align="left">
                        <asp:Label ID="txtFailQty" runat="server" Text="0" CssClass="iMes_label_11pt" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                      <hr>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td style="width: 30%" align="left">
                        <asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_11pt" />
                    </td>
                    <td>
                    </td>
                    <td style="width: 12%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td align="left">
                        <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_11pt" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr align="left">
                    <td colspan="6">
                        <asp:Label ID="lbDefectList" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="True"
                                    GetTemplateValueEnable="False" GvExtHeight="220px" GvExtWidth="100%" OnGvExtRowClick=""
                                    Width="99%" Height="210px" OnGvExtRowDblClick="" SetTemplateValueEnable="False"
                                    HighLightRowPosition="3" HorizontalAlign="Left" 
                                    OnRowDataBound="GridViewExt1_RowDataBound" HiddenColCount="0" 
                                    style="top: -149px; left: -408px">
                                    <Columns>
                                        <asp:BoundField DataField="DefectId" SortExpression="DefectId" />
                                        <asp:BoundField DataField="DefectDescr" SortExpression="DefectDescr" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td nowrap="noWrap" style="width: 15%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" />
                    </td>
                    <td colspan="5" align="left">
                        <iMES:Input ID="txt" runat="server" IsClear="true" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <input type="hidden" runat="server" id="station" />
                                <input type="hidden" runat="server" id="useridHidden" />
                                <input type="hidden" runat="server" id="customerHidden" />
                                <input type="hidden" runat="server" id="Print2DLabelStation" />
                                <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none">
                                </button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="left">
                        <asp:CheckBox runat="server" ID="nineSelect"  AutoPostBack="false" 
                            CssClass="iMes_label_11pt" BackColor="Transparent" BorderStyle="None" 
                            oncheckedchanged="nineSelect_CheckedChanged"></asp:CheckBox>
                       <input type="hidden" runat="server" id="pCode" />
                    </td>
                    
                    
                </tr>
            </table>
        </center>
    </div>

<script type="text/javascript">
var scanNine4Flag = false;
var GridViewExt1ClientID = "<%=gridview.ClientID%>";//Grid control Client ID
var index = 1;                                      //Table Count count=index-1 
var strRowsCount = "<%=initRowsCount%>";            //define Table Lines-string
var initRowsCount = parseInt(strRowsCount, 10) + 1; //define Table Lines-int
var defectArray = new Array();
var saveDefectArray = new Array();
var passQty = 0;
var failQty = 0;
var IsNeedPrint = false;      
///Get local resource Info
var mesNoSelPdLine      = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelTestStation = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectTestStation").ToString()%>';
var mesNoInputProdId    = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputProdId").ToString()%>';
var mesDupData          = '<%=this.GetLocalResourceObject(Pre + "_mesDupData").ToString()%>';
var mesIllegalDefect    = '<%=this.GetLocalResourceObject(Pre + "_mesIllegalDefect").ToString()%>';
var mesMustHaveDefect   = '<%=this.GetLocalResourceObject(Pre + "_mesMustHaveDefect").ToString()%>';
var msgSystemError      = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgToEPIA = '<%=this.GetLocalResourceObject(Pre + "_msgToEPIA").ToString()%>';
var msgNQC = '<%=this.GetLocalResourceObject(Pre + "_msgNQC").ToString()%>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var SUCCESSRET          = "<%=WebConstant.SUCCESSRET%>";
var mgsSpecialRunin = '<%=this.GetLocalResourceObject(Pre + "_mgsSpecialRunin").ToString()%>';
var msgScanCustsn      = '<%=this.GetLocalResourceObject(Pre + "_msgScanCustsn").ToString()%>';
var custsn = "";
var proid  = "";
var firstSelStation = "";
var selStation = "";
var selPdLine = "";
var IsAllowPass    = true;
var IsBlankStation = false;
var IsBlankPdLine = false;
var specialRunin = false;
var JustKeyDefectCode = false;

document.body.onload = function() {
    try {
        specialRunin = false;
        //setPdLineCmbFocus();
        setTestStationCmbFocus();
        getAvailableData("processDataEntry");
    }
    catch (e) {
        alert(e.description);
    }
}
///---------------------------------------------------
///| Name		    :	onGetLineSucceeded
///| Description	:	callback return Succeeded for GetLine 
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function onGetLineSucceeded(result)
{
    if (result == "") 
    {
        return;
    }
    setCmbPdlineValue(result);
}
///---------------------------------------------------
///| Name		    :	onGetLineFailed
///| Description	:	callback return Failed for GetLine 
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function onGetLineFailed(error)
{ 
    try 
    {
    
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        reset();
        //setStatus(false);
        callNextInput();
    }
    catch (e)
    {
        alert(e.description);
    }
}
///---------------------------------------------------
///| Name		    :	removePdlineItem
///| Description	:	Clear pdline combox list
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function removePdlineItem()
{
    var obj=getPdLineCmbObj();
    //obj.options.remove(0);
    //obj.selectedIndex =0;
    //obj.disabled=true
}
///---------------------------------------------------
///| Name		    :	setCmbPdlineValue
///| Description	:	Set pdline combox selected item
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function setCmbPdlineValue(line)
{
   var obj = getPdLineCmbObj();
   obj.options.remove(0);
   var option = null;
   option = document.createElement("option");
   option.text = line;
   option.value = line;
   obj.setAttribute("selected","selected");
   obj.options.add(option);
   obj.selectedIndex  = 0;
   //obj.disabled = true
}
///---------------------------------------------------
///| Name		    :	showPrintSettingDialog
///| Description	:	Show Print Setting Dialog
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function showPrintSettingDialog()
{
    showPrintSetting(document.getElementById("<%=station.ClientID%>").value,document.getElementById("<%=pCode.ClientID%>").value);
}

function SetStationDefault(selStation)
{
     var obj = getTestStationCmbObj();
      for (var i = 0; i < obj.options.length; i++)
      {
        if (obj.options[i].value == selStation) 
        {
            obj.selectedIndex = i;
            return; 
        }
     }   
}
///---------------------------------------------------
///| Name		    :	processDataEntry
///| Description	:	handle Input Data (Data Entry)
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function processDataEntry(inputData) 
{
    /*
    var Print2DLabelStation = document.getElementById("<%=Print2DLabelStation.ClientID%>").value;
    if (getTestStationCmbText() == Print2DLabelStation)
    {
        IsNeedPrint = true;
    }
    else 
    {
        IsNeedPrint = false;
    }
    */
    PlaySoundClose();
    
    var content = "";
    var obj = getPdLineCmbObj();
    
    //scanNine4Flag = document.getElementById("<%=nineSelect.ClientID%>").checked;
    //obj.disabled = true;
    try
    {
        var errorFlag = false;
        selPdLine = getPdLineCmbValue();
        selStation = getTestStationCmbValue();

        ///if selStation is NULL,Message
        if (selStation != "") {

            IsBlankStation = false;
        }
        else {
            IsBlankStation = true;
            ShowInfo("");
            content = mesNoSelTestStation;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
            setTestStationCmbFocus(); 
            errorFlag = true;
            return;
        }
        
        
        ///if selPdLine is NULL, Message 
        if (selPdLine != "") 
        {
            IsBlankPdLine = false;
        }
        else {
            IsBlankPdLine = true;
            ShowInfo("");
            content = mesNoSelPdLine;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
            setPdLineCmbFocus();
            errorFlag = true;
            return;
        }
       
        if (!errorFlag) 
        {
            ///reset UI
            if (inputData == "7777") 
            {
                ShowInfo("");
				ExitPage();
				reset();
                getAvailableData("processDataEntry");
            }
            else if (document.getElementById("<%=txtProdId.ClientID%>").innerText == "") 
            {
                if (inputData == "9999")
                {
                    alert(mesNoInputProdId);
                    getAvailableData("processDataEntry");
                }
                else 
                {
                    //Get Prod ID,,,,,,
                    //begin call Service Agent
                    firstSelStation = getTestStationCmbText();
                    beginWaitingCoverDiv();
					custsn = "";
                    proid = inputData;
                    defectArray.length = 0; 
					// SubStringSN(inputData,"ProdId")
                    WebServiceFATestStation.inputProdId(getPdLineCmbValue(),getTestStationCmbValue(),inputData,document.getElementById("<%=useridHidden.ClientID%>").value, document.getElementById("<%=customerHidden.ClientID%>").value,onProdIdSucceed,onProdIdFail);
                    //WebServiceFATestStation.inputCustsn(getPdLineCmbValue(), selStation, inputData, document.getElementById("<%=useridHidden.ClientID%>").value, document.getElementById("<%=customerHidden.ClientID%>").value, onProdIdSucceed, onProdIdFail);
                }
            }
            else 
            {
                //getElementById(=txtProdId.ClientID> is not NULL, 眒冪怀徹Customer SN甜get善ProdId
                if (custsn != "") {
                    if (custsn != inputData) {
                        ShowInfo("Wrong CustSN");
                        getAvailableData("processDataEntry");
                        return;
                    }
                    else {
                        custsn = "";
                        After_ProdIdSucceed_Step2_AFterCheckCustsn();
                        getAvailableData("processDataEntry");
                        return;
                    }
                }
                else if (inputData == "9999") 
                {
                    if (!IsAllowPass & saveDefectArray.length == 0) 
                    {
                        alert(mesMustHaveDefect);
                        getAvailableData("processDataEntry");
                        return;
                    }
                    //湔紫
                    if (JustKeyDefectCode && saveDefectArray.length == 0) {
                        alert("RunIn时间不够，只能刷不良...");
                        //ShowInfo("RunIn时间不够，只能刷不良...");
                        getAvailableData("processDataEntry");
                        return;
                    }
                    if (saveDefectArray.length != 0) 
                    {
                        IsNeedPrint = false;  
                    }
                    var lstPrintItem = null ;
                    if(IsNeedPrint) // need print 2d
                    {   
                        try
                        {
                           lstPrintItem = getPrintItemCollection();
                        }
                        catch(e)
                        {
                            alert(e);
                            getAvailableData("processDataEntry");
                            return;
                        }
                        if (lstPrintItem == null)                 
                        {
                            alert(msgPrintSettingPara);
                            getAvailableData("processDataEntry");
                        } 
                        else 
                        {
                            beginWaitingCoverDiv();
                            WebServiceFATestStation.savePage(document.getElementById("<%=txtProdId.ClientID%>").innerText, saveDefectArray, IsNeedPrint,lstPrintItem, onSaveSucceed, onSaveFail);
                        }
                    }
                    else {   
                        beginWaitingCoverDiv();
                        WebServiceFATestStation.savePage(document.getElementById("<%=txtProdId.ClientID%>").innerText, saveDefectArray, IsNeedPrint,lstPrintItem, onSaveSucceed, onSaveFail);
                    }
                } 
				else 
                {
                    //Input Defect 
                    checkDefect(inputData);
                    getAvailableData("processDataEntry");
                }
            }
        }
       
    }
    catch(e) 
    {
        alert(e.description);
    }
}

///---------------------------------------------------
///| Name		    :	setStatus
///| Description	:	扢离珜醱垀衄迵ProdId禸眈壽諷璃腔袨怓
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function setStatus(status)
{
    try 
    {
        getPdLineCmbObj().disabled = status;
        getTestStationCmbObj().disabled = status;
    }
    catch (e) 
    {
        alert(e.description);
    }
}
var ProdIdSucceed_defectList;
var ProdIdSucceed_strRuninrtn;
var ProdIdSucceed_JustKeyDefectCode;
var ProdIdSucceed_RunInTimes;
function After_ProdIdSucceed_Step1_SaveVars(_defectList, _strRuninrtn, _JustKeyDefectCode, _RunInTimes, _custsn){
	ProdIdSucceed_defectList = _defectList;
	ProdIdSucceed_strRuninrtn = _strRuninrtn;
	ProdIdSucceed_JustKeyDefectCode = _JustKeyDefectCode;
	ProdIdSucceed_RunInTimes = _RunInTimes;
	custsn = _custsn;
}
function After_ProdIdSucceed_Step2_AFterCheckCustsn(){
	/*
	// kshzh document.getElementById("<=txtDefectStation.ClientID>").innerText = ProdIdSucceed_strRuninrtn;
	*/
    var lstPrintItem = null;
	if (ProdIdSucceed_strRuninrtn != "NO!") {
		specialRunin = true;
		setStatus(true);
		clearTable(); //saveDefectArray.length = 0;
		beginWaitingCoverDiv();
		WebServiceFATestStation.savePage(document.getElementById("<%=txtProdId.ClientID%>").innerText, saveDefectArray, IsNeedPrint, lstPrintItem, onSaveSucceed, onSaveFail);                
	}
	else {
		for (i = 0; i < ProdIdSucceed_defectList.length; i++) {
			defectArray.push(ProdIdSucceed_defectList[i]);
		}
		//IsAllowPass = ProdIdSucceed_JustKeyDefectCode;
		//onGetLineSucceeded(ProdIdSucceed_RunInTimes);
		//   var station = result[7];
		//if (IsBlankStation) 
		//{
		//    SetStationDefault(result[7]); //result[7] : station 
		//}
		setStatus(true);
		if (scanNine4Flag && !ProdIdSucceed_JustKeyDefectCode) {
		    clearTable(); //saveDefectArray.length = 0;
			beginWaitingCoverDiv();
			WebServiceFATestStation.savePage(document.getElementById("<%=txtProdId.ClientID%>").innerText, saveDefectArray, IsNeedPrint, lstPrintItem, onSaveSucceed, onSaveFail);
		}
		else {
			callNextInput();
			if (ProdIdSucceed_JustKeyDefectCode) {
				JustKeyDefectCode = true;
				ShowInfo("RunIn时间不够，还差" + ProdIdSucceed_RunInTimes + "分钟，只能刷不良...");
			}
			else {
				ShowInfo("");
			}
		}
	}
}
///---------------------------------------------------
///| Name		:	onProdIdSucceed
///| Description	:	ProdId禸傖髡ㄛ鳳善Model
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function onProdIdSucceed(result)
{
    try 
    {
        endWaitingCoverDiv();
        var lstPrintItem = null;
        JustKeyDefectCode = false;
        scanNine4Flag = document.getElementById("<%=nineSelect.ClientID%>").checked;
        if(result == null)
        {
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if ((result.length == 8) && (result[0] == SUCCESSRET))  //old: result.length==8
        {
            document.getElementById("<%=txtProdId.ClientID%>").innerText = result[1];
			proid = result[1];
            document.getElementById("<%=txtModel.ClientID%>").innerText = result[2];
            After_ProdIdSucceed_Step1_SaveVars(result[3], result[4], result[5], result[6], result[7]);
			ShowInfo(msgScanCustsn);
			callNextInput();
        }
        else // return 峈 Error message
        {       
            //removePdlineItem();
            ShowInfo("");
            var content =result[0];
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
///---------------------------------------------------
///| Name		:	onProdIdFail
///| Author		:	Lucy Liu
///| Create Date	:	10/27/2009
///| Description	:	ProdId禸囮啖ㄛ离渣昫陓洘
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function onProdIdFail(error) 
{
    var obj = getPdLineCmbObj();
    obj.disabled = true;
    getTestStationCmbObj().disabled = true;
    setStatus(false);
    try 
    {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    }
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
    //var obj=getPdLineCmbObj();
    //obj.disabled=true
}

///---------------------------------------------------
///| Name		    :	checkDefect
///| Description	:	潰桄defect腔磁楊俶
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function checkDefect(defectCode)
{
    try 
    {
        var errorFlag = false;
        var bFind = false;
        var description = "";
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
            var table = document.getElementById(GridViewExt1ClientID); 
            for(var i = 1; i < table.rows.length; i++)
            {
                if (table.rows[i].cells[0].innerText == defectCode) 
                {
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
        if (!errorFlag) 
        {
            var rowInfo = new Array();
            rowInfo.push(defectCode); 
            rowInfo.push(description); 
            AddRowInfo(rowInfo);
            saveDefectArray.push(defectCode);
        }
    }
    catch (e) 
    {
        alert(e.description);
    }
}


function setPrintItemListParam(printItemList)
{
    ///=======generate PrintItem List=================
    var lstPrtItem = printItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();
    var a= document.getElementById("<%=txtProdId.ClientID%>").innerText
    keyCollection[0] = "@sn"; 
    valueCollection[0] = generateArray(proid);
    setPrintParam(lstPrtItem, "2D Door Label", keyCollection, valueCollection);
}
///---------------------------------------------------
///| Name		    :	onSaveSucceed
///| Description	:	珨棒俇淕悵湔傖髡
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function onSaveSucceed(result)
{
    try 
    {
         endWaitingCoverDiv();
         //removePdlineItem();
         if(result==null)
        {
            ShowInfo("");
            reset();
            //setStatus(false);
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if((result.length == 4) && (result[0] == SUCCESSRET))
        {
            if (specialRunin) {
                specialRunin = false;
                failQty++;
                document.getElementById("<%=txtFailQty.ClientID%>").innerText = failQty + "";
                ShowMessage(mgsSpecialRunin);
                ShowInfo(mgsSpecialRunin);
            }
            else {
                if (saveDefectArray.length == 0) 
                {
                    passQty++;
                    document.getElementById("<%=txtPassQty.ClientID%>").innerText = passQty + "";
                    
                    if (result[1] == "isPIA") {
                        if (result[3] != "" && result[3] != null) {
                            var setmsg = result[3];
                            var messageToSetMsg = msgToEPIA + setmsg;
                            ShowInfo("[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + messageToSetMsg, "red");
                        }
                        else {
                            ShowInfo("[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgToEPIA, "green");
                        }
                        PlaySoundPIA();
                    }
                    else if (result[1] == "noPIA") {
                        if (result[3] != "" && result[3] != null) {
                            var setmsg = result[3];
                            //msgNQC += setmsg;
                            var messageToSetMsg = msgNQC + setmsg;
                            ShowSuccessfulInfoWithColor(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + messageToSetMsg, null, "red");
                        }
                        else {
                            ShowSuccessfulInfo(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgNQC);
                        }
                    }
                    else {
                        if (result[3] != "" && result[3] != null) {
                            var setmsg = result[3];
                            //msgSuccess += setmsg;
                            var messageToSetMsg = msgSuccess + setmsg;
                            ShowSuccessfulInfoWithColor(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + messageToSetMsg, null, "red");
                        }
                        else {
                            ShowSuccessfulInfo(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgSuccess);
                        }
                    }
                }
                else 
                {
                    failQty++;
                    document.getElementById("<%=txtFailQty.ClientID%>").innerText = failQty + "";
                    if (result[3] != "" && result[3] != null) {
                        var setmsg = result[3];
                        //msgSuccess += setmsg;
                        var messageToSetMsg = msgSuccess + setmsg;
                        ShowInfo("[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + messageToSetMsg, "red");
                    }
                    else {
                        ShowInfo("[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgSuccess);
                    }
                    PlaySound();
                }
                if (IsNeedPrint) // if select  "PreTest" ~~~~~~~~~~~
                {
                    setPrintItemListParam(result[1]);
                    printLabels(result[1], false);
                }
                //ShowSuccessfulInfo(true, msgSuccess);
            }
            SetStationDefault(firstSelStation);
            reset();
            callNextInput();
        }
        else 
        {
            ShowInfo("");
            reset();
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

///---------------------------------------------------
///| Name		    :	onSaveFail
///| Description	:	悵湔囮啖ㄛ离渣昫陓洘
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function onSaveFail(error)
{
    try 
    {
        endWaitingCoverDiv();
        //removePdlineItem();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        reset();
        callNextInput();
    }
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

///---------------------------------------------------
///| Name		    :	clearTable
///| Description	:	蔚Defect List table腔囀諾
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function clearTable()
{
    try 
    {
        ClearGvExtTable("<%=gridview.ClientID%>",initRowsCount);
        index = 1;
        setSrollByIndex(0 ,false);
        saveDefectArray.length = 0;
    }
    catch (e) 
    {
        alert(e.description);
    }
}

///---------------------------------------------------
///| Name		:	AddRowInfo
///| Author		:	Lucy Liu
///| Create Date	:	10/27/2009
///| Description	:	砃桶跡笢氝樓珨俴
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function AddRowInfo(RowArray) 
{
    try 
    {
        if (index < initRowsCount) 
        {
            eval("ChangeCvExtRowByIndex_" +GridViewExt1ClientID+"(RowArray,false, index)");
        } 
        else 
        {
            eval("AddCvExtRowToBottom_"+GridViewExt1ClientID+"(RowArray,false)");
        }
        setSrollByIndex(index ,false);
        index++;
    }
    catch (e) 
    {
        alert(e.description);
    }
}

///---------------------------------------------------
///| Name		    :	reset
///| Author		    :	Lucy Liu
///| Create Date	:	10/27/2009
///| Description	:	諾賜醱
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function reset()
{
    try 
    {
        document.getElementById("<%=txtProdId.ClientID%>").innerText = "";
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";
		document.getElementById("<%=txtFailQty.ClientID%>").innerText = "0";
		document.getElementById("<%=txtPassQty.ClientID%>").innerText = "0";
		getCommonInputObject().value = "";
        /*
        // kshzh document.getElementById("<=txtDefectStation.ClientID>").innerText = "";
        */
        specialRunin = false;
        selStation = "";
        selPdLine = "";
        clearTable();
        setStatus(false);
    }
    catch (e) 
    {
        alert(e.description);
    }
}

///---------------------------------------------------
///| Name		    :	callNextInput
///| Author		    :	Lucy Liu
///| Create Date	:	10/27/2009
///| Description	:	脹渾辦厒諷璃樟哿怀
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function callNextInput()
{
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}       

window.onbeforeunload= function() 
{
    ExitPage(); 
}  

function onClearSucceeded(result)
{
    try 
    {
        if(result == null)
        {
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            callNextInput();
        }
        else if((result.length==2)&&(result[0]==SUCCESSRET))
        {
            ShowInfo("");
        }
        else 
        {
            ShowInfo("");
            var content =result[0];
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
        reset();
        callNextInput();
    }
    catch (e) 
    {
        alert(e.description);
    }
}

///---------------------------------------------------
///| Name		    :	ExitPage
///| Description	:	豖堤珜醱奀覃蚚
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function ExitPage()
{
    if(document.getElementById("<%=txtProdId.ClientID%>").innerText != "")
    {
         WebServiceFATestStation.Cancel(document.getElementById("<%=txtProdId.ClientID%>").innerText, getTestStationCmbValue(), onClearSucceeded,onClearFailed);
    } 
}

///---------------------------------------------------
///| Name		    :	ResetPage
///| Description	:	芃陔珜醱奀覃蚚
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function ResetPage()
{
    ExitPage();
    reset();
    document.getElementById("<%=btnReset.ClientID%>").click();
}
function PlaySoundPIA() {
    var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["DuplicateAudioFile"] %>';
    var obj = document.getElementById("bsoundInModal");
    //obj.loop =-1;
    obj.loop = 1;
    obj.src = sUrl;
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

       
</script>

</asp:Content>
