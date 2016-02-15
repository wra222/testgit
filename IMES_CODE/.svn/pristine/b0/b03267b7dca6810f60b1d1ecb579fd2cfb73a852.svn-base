<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  207006(EB2)          Create 
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="AssignModel.aspx.cs" Inherits="FA_AssignModel" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/AssignModel.asmx" />
			<asp:ServiceReference Path="~/Service/PrintSettingService.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <TABLE border="0" width="95%">
    <TR>
	    <TD style="width:15%" align="left" ><asp:Label ID="lbPdline" runat="server" 
                CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6" align="left"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"/></TD>
	   
    </TR>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbFamily" runat="server" 
                CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6" align="left">
            <iMES:CmbFamily ID="CmbFamily1" runat="server" Width="100" IsPercentage="true" />
        </TD>
    </TR>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" 
                CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6" align="left">
            <iMES:CmbModel ID="CmbModel1" runat="server" Width="100" IsPercentage="true" />
        </TD>
    </TR>
    
    <TR>
	    <TD></TD>
	    <TD width="28%" align="left" >
	        <asp:Label ID="lbMoQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>&nbsp;
	        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
	                <asp:Label ID="lbShowMoQty" runat="server" CssClass="iMes_label_11pt" ></asp:Label>
	            </ContentTemplate>
           </asp:UpdatePanel>
        </TD>
        <TD width="5%" align="left">&nbsp;</TD>
	    <TD width="28%" align="left" > 
           <asp:Label ID="lbReQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>&nbsp;
             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <asp:Label ID="lbShowReQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </ContentTemplate>  
           </asp:UpdatePanel>
         </TD>
	    <TD width="5%" align="left">&nbsp;</TD>
	    <TD align="left">
	       <asp:Label ID="lbShipDate" runat="server" CssClass="iMes_label_13pt"></asp:Label>&nbsp;
             <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <asp:Label ID="txtShipDate" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </ContentTemplate>  
           </asp:UpdatePanel>
        </TD>
	    <TD width="8%"  align="left"></TD>
    </TR>
    <tr>
		<td style="width: 15%" align="left">
			<asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
		</td>
		<td align="left" colspan="6">
			<iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
				CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
				ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
			<asp:UpdatePanel ID="updatePanel3" runat="server" RenderMode="Inline">
				<ContentTemplate>
					<input type="hidden" runat="server" id="station" />
					<input type="hidden" runat="server" id="pdLine" />
					<input type="hidden" runat="server" id="theLine" />
					<input type="hidden" runat="server" id="theFamily" />
					<input type="hidden" runat="server" id="theModel" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
	</tr>
	<tr>
		<td align="right" colspan="7">
			<input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="showPrintSettingDialog()" />
			<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
			<input type="hidden" runat="server" id="pCode" />
		</td>
	</tr>
    </TABLE>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>
        <button id="btnHidden" runat="server" onclick="" onserverclick="btnHidden_Click" style="display: none" >
        </button> 
    </ContentTemplate>   
</asp:UpdatePanel>

<script type="text/javascript">
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoInputQty = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputQty").ToString()%>';
var mesQtyOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesQtyOutRange").ToString()%>';
var mesQtyExReQty = '<%=this.GetLocalResourceObject(Pre + "_mesQtyExReQty").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var mesNoSelectFamily = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectFamily").ToString()%>';
var mesNoSelectModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
var msgNeedReQty = '<%=this.GetLocalResourceObject(Pre + "_msgNeedReQty").ToString()%>';
var msgPass = '<%=this.GetLocalResourceObject(Pre + "_msgPass").ToString()%>';

var SUCCESSRET ="SUCCESSRET";
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var station;
var accountid;
var login;

var custsn = '';

var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var lstPrintItem;
var havePrintItem = false;
var pCode = "";

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onload
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	置页面焦点
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
document.body.onload = function ()
{
    try {
        pCode = '<%=Request["PCode"] %>';
		station = '<%=Request["Station"] %>';
		accountid = '<%=AccountId%>';
		login = '<%=Login%>';
		customer = '<%=Request["Customer"]%>';
		editor = '<%=Request["UserId"]%>';
		PrintSettingService.GetPrintInfo(pCode, onGetPSucceed, onGetPFail);
		getAvailableData("processDataEntry");
//        setPdLineCmbFocus();
    } catch(e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onFail
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	调用web service失败
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onFail(error)
{
   try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
     
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function checkNumberAndEnglishChar(value)
{
	var errorFlag = false;
	try 
    {
	   var pattern = /^[0-9a-zA-Z]*$/;
	    if (pattern.test(value)) 
	    {
		    errorFlag = false;
	    }
	    else 
	    {
		    errorFlag = true;
	    } 
	    return errorFlag;
	}
    catch (e)
    {
	    alert(e.description);
    }   
}

function ExitPage()
{}

function ResetPage()
{
    ExitPage();
    ShowInfo("");
    getPdLineCmbObj().selectedIndex = 0;
    document.getElementById("<%=btnHidden.ClientID%>").click(); 
    endWaitingCoverDiv(); 
}

function processDataEntry(inputData) {
    try {
        inputData = SubStringSN(inputData, "CustSN");
        var errorFlag = false;
		var msg='';
        if (getPdLineCmbValue() == "") {
            errorFlag = true;
			msg=mesNoSelPdLine;
            setPdLineCmbFocus();
        }
		else if (getFamilyCmbValue() == "") {
            errorFlag = true;
            msg=mesNoSelectFamily;        
        }
		else if (getModelCmbValue() == "") {
            errorFlag = true;
            msg=mesNoSelectModel;
        }
		lstPrintItem = getPrintItemCollection();
		if (lstPrintItem == null && havePrintItem) {
            errorFlag = true;
            msg=msgPrintSettingPara;
		}
		if(errorFlag){
			alert(msg);
			getAvailableData("processDataEntry");
			return;
		}
		
		if (inputData == "") {
			alert('Please input CustSN!');
			getAvailableData("processDataEntry");
			return;
		}
        
		ShowInfo("");
		custsn = inputData;
		var reQty = document.getElementById("<%=lbShowReQty.ClientID%>").innerText;
		if(reQty=='' || isNaN(reQty) || (parseInt(reQty)<=0) ){
			alert(msgNeedReQty);
			getAvailableData("processDataEntry");
			return;
		}

		beginWaitingCoverDiv();
		//getCommonInputObject().focus();
		AssignModel.ToAssignModel(custsn, document.getElementById("<%=theLine.ClientID%>").value, document.getElementById("<%=theFamily.ClientID%>").value, document.getElementById("<%=theModel.ClientID%>").value,
			document.getElementById("<%=pdLine.ClientID%>").value, editor, document.getElementById("<%=station.ClientID%>").value, customer, lstPrintItem, onSucceed, onFail);
        
    } catch (e) {
        alert(e.description);
    }
}

function onSucceed(result) {
	endWaitingCoverDiv();
	var reQty = document.getElementById("<%=lbShowReQty.ClientID%>").innerText;
	document.getElementById("<%=lbShowReQty.ClientID%>").innerText = (parseInt(reQty) - 1);
	if (result[0] == SUCCESSRET) {
		if (havePrintItem) {
		    setPrintItemListParam(result[1], result[1][0].LabelType, custsn);
			printLabels(result[1], false);
		}
		if (''==result[2])
			ShowSuccessfulInfo(true, msgPass + ' [ ' + custsn + ' ]');
		else
			ShowInfo(msgPass + ' [ ' + custsn + ' ]. ' + result[2]);
	}
	else
	{
		ShowMessage(msgSystemError);
		ShowInfo(msgSystemError);
	}
	callNextInput();
}

function onFail(error) {
	try {
		endWaitingCoverDiv();
		ShowMessage(error.get_message());
		ShowInfo(error.get_message());
		callNextInput();
	} catch (e) {
		alert(e.description);
	}
}

function callNextInput() {
	getCommonInputObject().focus();
	getAvailableData("processDataEntry");
}

function setPrintItemListParam(backPrintItemList,labelType,sn) {
	var lstPrtItem = backPrintItemList;
	var keyCollection = new Array();
	var valueCollection = new Array();
	keyCollection[0] = "@sn";
	valueCollection[0] = generateArray(sn);
	setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
}
function reprint() {
	//Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
	var url = "AssignModelRePrint.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; ;
	var paramArray = new Array();
	paramArray[0] = getPdLineCmbValue();
	paramArray[1] = editor;
	paramArray[2] = customer;
	paramArray[3] = station;
	window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
}
function showPrintSettingDialog() {
	showPrintSetting(station, pCode);
}
function onGetPSucceed(result) {
	try {
		if (result == null) {
			var content = msgSystemError;
			alert(content);
		}
		else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
			if (result[1].length == 0) {
				havePrintItem = false;
			}
			else {
				havePrintItem = true;
			}
			SetPrintBtn();
		}
		else {
			alert(result[0]);
			var content = result[0];
		}
	} catch (e) {
		alert(e.description);
	}
}
function onGetPFail(error) {
	try {
		alert(error.get_message());
	} catch (e) {
		alert(e.description);
	}
}
function SetPrintBtn() {
	var id1 = "#" + "<%=btnPrintSet.ClientID %>";
	var id2 = "#" + "<%=btnReprint.ClientID %>";
	if (havePrintItem) {
		$(id1).show();
		$(id2).show();
	}
	else {
		$(id1).hide();
		$(id2).hide();
	}
}
</script>
</asp:Content>

