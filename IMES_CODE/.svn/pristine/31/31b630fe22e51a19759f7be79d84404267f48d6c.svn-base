<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
* Check Item
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ Import Namespace="com.inventec.iMESWEB" %> 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CommonLabelPrint.aspx.cs" Inherits="FA_CommonLabelPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceCommonLabelPrint.asmx" />
        </Services>
</asp:ScriptManager>

<div>
    <center>
    <table border="0" width="95%">
    <tr>   
        <td style="width:15%"> &nbsp; </td> 
        <td style="width:35%"> &nbsp;</td>     
        <td style="width:15%"> &nbsp; </td> 
        <td style="width:35%" align="center">&nbsp;</td>
    </tr> 
    <tr>
        <td align="left">
            <asp:Label ID="lbFilePath" runat="server"  CssClass="iMes_label_13pt" Text="File Path:"></asp:Label></td>
        <td colspan="3" align="left">
            <asp:TextBox ID="txtFilePath" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%" />
        </td>
    </tr>
    <tr>
        <td align="left"><asp:Label ID="lbLabelName" runat="server" CssClass="iMes_label_13pt" Text="Label Name:"></asp:Label></td> 
		<td colspan="3" align="left">
            <asp:DropDownList ID="cmbLabelName" runat="server" Width="90%" onchange="changeLabel(this.value)" ></asp:DropDownList>
        </td>
    </tr>
	<tr>
        <td align="left"><asp:Label ID="lbPrintMode" runat="server" CssClass="iMes_label_13pt" Text="Print Mode:"></asp:Label></td> 
		<td colspan="3" align="left">
            <asp:Label ID="lbPrintModeContent" runat="server" CssClass="iMes_label_13pt" />
        </td>
    </tr>
	<tr>
        <td align="left"><asp:Label ID="lbLabelSpec" runat="server" CssClass="iMes_label_13pt" Text="Label Spec:"></asp:Label></td> 
		<td colspan="3" align="left">
            <asp:Label ID="lbLabelSpecContent" runat="server" CssClass="iMes_label_13pt" />
        </td>
    </tr>
	<tr>
        <td align="left"><asp:Label ID="lbFileName" runat="server" CssClass="iMes_label_13pt" Text="File Name:"></asp:Label></td> 
		<td colspan="3" align="left">
            <asp:Label ID="lbFileNameContent" runat="server" CssClass="iMes_label_13pt" />
        </td>
    </tr>
	<tr>
        <td align="left"><asp:Label ID="lbSPName" runat="server" CssClass="iMes_label_13pt" Text="SP Name:"></asp:Label></td> 
		<td colspan="3" align="left">
            <asp:Label ID="lbSPNameContent" runat="server" CssClass="iMes_label_13pt" />
        </td>
    </tr>
    <tr>
        <td align="left"><asp:Label ID="lbPiece" runat="server" CssClass="iMes_label_13pt" Text="Piece:" />
		</td>
        <td align="left"><asp:TextBox ID="txtPiece" runat="server" CssClass="iMes_textbox_input_Normal" Width="30%" Text="1" />
		</td>
        <td align="left"><asp:Label ID="lbLayout" runat="server" CssClass="iMes_label_13pt" Text="Layout:" />
		</td>
        <td ><asp:DropDownList ID="cmbLayout" runat="server" Width="60%"></asp:DropDownList>
		</td>                
    </tr>
    
    <TR><TD colspan="6"><hr></TD></TR>
    <TR>
	    <TD colspan="6" align="left">
	        <asp:Label ID="lbList" runat="server" CssClass="iMes_label_13pt" Text="Parameter List:" /></TD>	   
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
                    <asp:BoundField DataField="Parameter" />
                    <asp:BoundField DataField="Value" />
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
             CanUseKeyboard="true" IsPaste="true"  MaxLength="17"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
        </TD>
    </tr>
    
    <tr>
    	   <td align="right" colspan="5"><input id="btpPrintSet" type="button"  runat="server" 
               class="iMes_button" onclick="showPrintSettingDialog()" 
               onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'" align="right"/></td>
               
                <td style="width:15%">
            <input id="btnPrint" type="button" style="width:100%" runat="server" class="iMes_button" 
               onclick="print()" onmouseover="this.className='iMes_button_onmouseover'" 
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

var DataEntryControl;
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesInput = '<%=this.GetLocalResourceObject(Pre + "_mesInput").ToString()%>';
var txtProdId = "";
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount, 10) + 1;
var index = 1;
var GridViewExt1ClientID = "<%=gridview.ClientID%>";

var station;
var globeProdid;
var pcode;
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var mesnoprint = '<%=this.GetLocalResourceObject(Pre + "_mesnoprint").ToString()%>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgInputField = '<%=this.GetLocalResourceObject(Pre + "_msgInputField").ToString()%>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var lblPrintModeBat = '<%=this.GetLocalResourceObject(Pre + "_lblPrintModeBat").ToString()%>';
var lblPrintModeTemplate = '<%=this.GetLocalResourceObject(Pre + "_lblPrintModeTemplate").ToString()%>';
var lblLayoutV = '<%=this.GetLocalResourceObject(Pre + "_lblLayoutV").ToString()%>';
var lblLayoutH = '<%=this.GetLocalResourceObject(Pre + "_lblLayoutH").ToString()%>';
var lblLabelName = "Label Name";
var lblLayout = "Layout";
var lblPiece = "Piece";
var lblFilePath = "File Path";

var accountid = '<%=AccountId%>';
var username = '<%=UserName%>';
var login = '<%=Login%>';
var lstPrintItem = null;
var gvTable = document.getElementById("<%=gridview.ClientID%>");

var cmbLabelName=document.getElementById("<%=cmbLabelName.ClientID %>");
var cmbLayout=document.getElementById("<%=cmbLayout.ClientID %>");

var idxParam = 0;
var totalParam = 0;

var printMode = -1;
var LabelType = "";
var SpName = "";
var TemplateName = "";
var Layout = 0;
var Piece = 0;

document.body.onload = function() {
    try {
        DataEntryControl = getCommonInputObject();
        
        station = document.getElementById("<%=station1.ClientID%>").value;
        editor = document.getElementById("<%=editor1.ClientID%>").value;
        customer = document.getElementById("<%=customer1.ClientID%>").value;
        pcode = document.getElementById("<%=pCode1.ClientID%>").value;
		
		cmbLayout.options.add(new Option(lblLayoutV, "0"));
		cmbLayout.options.add(new Option(lblLayoutH, "1"));
		WebServiceCommonLabelPrint.GetOfflineLabelSettingList(editor, customer, onGetOfflineLabelSettingListSucceed, onFail);
		
    } catch (e) {
        showErrorMessageAndCleanAndCallNext(e.description);
    }
}

function callNextInput() {
	getCommonInputObject().focus();
	getCommonInputObject().select();
	getAvailableData("processDataEntry");
}

function PreCheck(){
	if ("" != document.getElementById("<%=txtFilePath.ClientID %>").value.trim()){
		ClientBatFilePath = document.getElementById("<%=txtFilePath.ClientID %>").value;
	}
	
	lstPrintItem = getPrintItemCollection();
	if (lstPrintItem == null) {
		showErrorMessageAndCallNext(msgPrintSettingPara);
		return false;
	}
	
	if ("" == cmbLabelName.options[cmbLabelName.selectedIndex].value){
		showErrorMessageAndCallNext(msgInputField+lblLabelName);
		return false;
	}
	if ("" == cmbLayout.options[cmbLayout.selectedIndex].value){
		showErrorMessageAndCallNext(msgInputField+lblLayout);
		return false;
	}
	if ("" == document.getElementById("<%=txtPiece.ClientID %>").value){
		showErrorMessageAndCallNext(msgInputField+lblPiece);
		return false;
	}
	
	if (0 == printMode || 3 == printMode){
		if ("" == document.getElementById("<%=txtFilePath.ClientID %>").value.trim()){
			showErrorMessageAndCallNext(msgInputField+lblFilePath);
			return false;
		}
	}
	
	return true;
}

function processDataEntry(inputData) {
    try {
        ShowInfo("");
		
		if (totalParam > 0){
			if (!PreCheck())
				return;
			
			gvTable = document.getElementById("<%=gridview.ClientID%>");
			if (idxParam < totalParam){
				idxParam++;
				gvTable.rows[idxParam].cells[1].innerText = inputData;
			}
			
			if (idxParam == totalParam){
				print();
			}
		}
		
		callNextInput();
    } catch (e) {
        showErrorMessageAndCleanAndCallNext(e);
    }
}
function generateArray(val) {
    var ret = new Array();
    ret[0] = val;
    return ret;
}

function AddRowInfo(RowArray) {
    if (index < initRowsCount) {
        eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
    } else {
        eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
    }
    index++;
    setSrollByIndex(0, false, GridViewExt1ClientID);
}

function showErrorMessageAndCallNext(message) {
    endWaitingCoverDiv();
    ShowMessage(message);
    ShowInfo(message);
    //clearData();
    callNextInput();
}

function showErrorMessageAndCleanAndCallNext(message) {
    endWaitingCoverDiv();
    ShowMessage(message);
    ShowInfo(message);
    clearData();
    callNextInput();
}

function updateTable(items, len1) {
    for (var i = 0; i < len1; i++) {
        var rowInfo = new Array();
        rowInfo.push(items[i]);
        rowInfo.push("");
        AddRowInfo(rowInfo);
    }
}

function clearData(){
	setInputOrSpanValue(document.getElementById("<%=lbPrintModeContent.ClientID %>"), "");
	setInputOrSpanValue(document.getElementById("<%=lbLabelSpecContent.ClientID %>"), "");
	setInputOrSpanValue(document.getElementById("<%=lbFileNameContent.ClientID %>"), "");
	setInputOrSpanValue(document.getElementById("<%=lbSPNameContent.ClientID %>"), "");
	setInputOrSpanValue(document.getElementById("<%=txtPiece.ClientID %>"), "1");
	cmbLayout.selectedIndex=0;
	
	clearTable();
	
	idxParam = 0;
	totalParam = 0;
	printMode = -1;
	ShowInfo("");
}

function changeLabel(labelName){
	clearData();
	if ("" != labelName){
		beginWaitingCoverDiv();
		WebServiceCommonLabelPrint.GetOfflineLabelSetting(labelName, editor, customer, onGetOfflineLabelSettingSucceed, onFail);
	}
}

function onGetOfflineLabelSettingSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            showErrorMessageAndCleanAndCallNext(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
			var lst=result[1];
			
			setInputOrSpanValue(document.getElementById("<%=lbLabelSpecContent.ClientID %>"), lst.labelSpec);
			setInputOrSpanValue(document.getElementById("<%=lbFileNameContent.ClientID %>"), lst.fileName);
			setInputOrSpanValue(document.getElementById("<%=lbSPNameContent.ClientID %>"), lst.SPName);
			
			printMode = lst.PrintMode;
			switch(printMode){
				case 0:
					setInputOrSpanValue(document.getElementById("<%=lbPrintModeContent.ClientID %>"), lblPrintModeBat);
					break;
				case 1:
					setInputOrSpanValue(document.getElementById("<%=lbPrintModeContent.ClientID %>"), lblPrintModeTemplate);
					break;
				case 3:
					setInputOrSpanValue(document.getElementById("<%=lbPrintModeContent.ClientID %>"), "Bartender");
					break;
				case 4:
					setInputOrSpanValue(document.getElementById("<%=lbPrintModeContent.ClientID %>"), "Bartender Server");
					break;
			}
			
			updateTable(lst.ParamList, lst.ParamList.length);
			
			totalParam = lst.ParamList.length;
			
			callNextInput();
        }
        else {
            var content = result[0];
            showErrorMessageAndCleanAndCallNext(content);
        }
        
    } catch (e) {
        showErrorMessageAndCleanAndCallNext(e.description);
    }
}

function onGetOfflineLabelSettingListSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            showErrorMessageAndCleanAndCallNext(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            cmbLabelName.options.length=0;
			cmbLabelName.options.add(new Option(""));
			var lst=result[1];
			if(null!=lst && lst.length>0){
				for(var i=0;i<lst.length;i++){
					var lstLabel = lst[i];
					for(var z=0;z<lstLabel.length;z++){
						cmbLabelName.options.add(new Option(lstLabel[z], lstLabel[z]));
					}
				}
			}
        }
        else {
            var content = result[0];
            showErrorMessageAndCleanAndCallNext(content);
        }
        
    } catch (e) {
        showErrorMessageAndCleanAndCallNext(e.description);
    }
	callNextInput();
}

function onFail(error) {
    try {
        endWaitingCoverDiv();
        onSysError(error);
        DataEntryControl.focus();
        
    } catch (e) {
        showErrorMessageAndCleanAndCallNext(e.description);
    }
	callNextInput();
}

function clearTable() {
    ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
    //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
    index = 1;
    //清空客户端保存表格的结构数组
}

function onSysError(error) {
    endWaitingCoverDiv();
    ShowMessage(error.get_message());
    ShowInfo(error.get_message());
    clearData();
    callNextInput();
}

function showPrintSettingDialog() {
    showPrintSetting(document.getElementById("<%=station1.ClientID%>").value, document.getElementById("<%=pCode1.ClientID%>").value);
}

function print() {
    if (!PreCheck())
		return;
	
	if (idxParam < totalParam){
		showErrorMessageAndCallNext("Please input all Parameter.");
		return;
	}
	
	LabelType = "";
	SpName = "";
	TemplateName = "";
	Layout = 0;
	
	LabelType = cmbLabelName.options[cmbLabelName.selectedIndex].value;
	switch(printMode){
		case 0:
			SpName = document.getElementById("<%=lbSPNameContent.ClientID %>").innerHTML;
			break;
		case 1:
			Layout = parseInt(cmbLayout.options[cmbLayout.selectedIndex].value);
			break;
		case 3:
			SpName = document.getElementById("<%=lbSPNameContent.ClientID %>").innerHTML;
			break;
		case 4:
			Layout = parseInt(cmbLayout.options[cmbLayout.selectedIndex].value);
			SpName = document.getElementById("<%=lbSPNameContent.ClientID %>").innerHTML;
			break;
	}

	Piece = parseInt(document.getElementById("<%=txtPiece.ClientID %>").value);
	
	var keyCollection = new Array();
    var valueCollection = new Array();
	for (var z=0; z<totalParam; z++){
		keyCollection[z] = "@" + gvTable.rows[z+1].cells[0].innerText;
		valueCollection[z] = generateArray(gvTable.rows[z+1].cells[1].innerText);
	}
	
	var printList = new Array();
	if (lstPrintItem && lstPrintItem.length > 0) {
		for (var i = 0; i < lstPrintItem.length; i++) {
			if (lstPrintItem[i].PrintMode == printMode){
				TemplateName = document.getElementById("<%=lbFileNameContent.ClientID %>").innerHTML; //lstPrintItem[i].TemplateName;
				
				var labelCollection = [];
				printList[i] = new PrintItem(printMode, lstPrintItem[i].RuleMode, LabelType, TemplateName, Piece, 
					SpName, keyCollection, valueCollection, lstPrintItem[i].OffsetX, lstPrintItem[i].OffsetY, 
					lstPrintItem[i].PrinterPort, lstPrintItem[i].dpi, Piece, lstPrintItem[i].Rotate180, Layout);
				labelCollection.push(printList[i]);
				printLabels(labelCollection, false);
			}
		}
	}
	
	idxParam = 0;
	for (var z=0; z<totalParam; z++){
		gvTable.rows[z+1].cells[1].innerText = "";
	}
	
	ShowSuccessfulInfo(true, msgSuccess);
	callNextInput();
}

</script>
</asp:Content>