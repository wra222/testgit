
<%@ Import Namespace="com.inventec.iMESWEB" %> 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PCAOQCCosmetic.aspx.cs" Inherits="PCAOQCCosmetic" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 
<asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServicePCAOQCCosmetic.asmx" />
        </Services>
</asp:ScriptManager>

<div>

    <center>
    <table border="0" width="95%">
    <tr>
        <td style="width:15%" align="left"><asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:35%" align="left">
            <asp:UpdatePanel ID="upMBSno" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upMBSnoContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>
        <td style="width:15%"><asp:Label ID="lblLotNo" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:35%">
            <asp:UpdatePanel ID="upLotNo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upLotNoContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>
    </tr>
    <tr>
        <td style="width:15%" align="left"><asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:35%" align="left">
            <asp:UpdatePanel ID="upPdline" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upPdlineContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>         
        <td style="width:15%"><asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"/></td>
        <td style="width:15%">
            <asp:UpdatePanel ID="upStation" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="upStationContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
            </asp:UpdatePanel></td>
    </tr>
    
    
    <tr><td colspan="4"><hr></td></tr>
    <tr>
	    <td colspan="4" align="left">
	        <asp:Label ID="lbDefectList" runat="server" CssClass="iMes_label_13pt"/></td>	   
    </tr>

    <tr>
	    <td colspan="4">
	      <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
          <ContentTemplate>
	         
             
             <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" 
                                        GetTemplateValueEnable="False" GvExtHeight="200px" Height="190px" 
                                        GvExtWidth="100%" OnGvExtRowClick=""
                                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                                        HighLightRowPosition="1" HorizontalAlign="Left"
                                        onrowdatabound="gd_RowDataBound">                                     
                                        <Columns>
                                            <asp:BoundField DataField="Defect Code" SortExpression="DefectId" />
                                            <asp:BoundField DataField="Description" SortExpression="DefectId" />
                                        </Columns>
             </iMES:GridViewExt>
             
             
                  
          </ContentTemplate>   
          </asp:UpdatePanel> 
	    </td>   
    </tr>
    
    <tr>
    <td style="width:15%" align="left">
            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
    <td colspan ="3" align="left">
        <textarea id="txtReason" style="width:90%; height: 30px" rows="2" cols="100"></textarea>
    </td>
    </tr>

    <tr>
    <td style="width:15%" align="left">
            <asp:Label ID="lblChecked" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
    <td align="left"><asp:CheckBox ID="Chk" runat="server" onclick=""/></td>
    </tr>

    <tr>
	    <td style="width:15%" align="left">
	        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" /></td>
	    <TD colspan="2" align="left" >
	         <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true" CssClass="textbox"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="11"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" Width="99%"/>
        </TD>
        <td style="width:15%">
            <input id="btnQuery" type="button" onclick="showQueryDialog()" runat="server" 
                class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" 
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
                <input type="hidden" runat="server" id="hidRepairID" />
                <input type="hidden" runat="server" id="hidMBSN" />
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
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var strRowsCount = "<%=DEFAULT_ROWS%>";
var initRowsCount = parseInt(strRowsCount, 10) + 1;
var saveDefectArray = new Array();
var index = 1;
var GridViewExt1ClientID = "<%=gd.ClientID%>";
var station;
var glombsn = "";
var pcode;
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

var accountid = '<%=AccountId%>';
var username = '<%=UserName%>';
var login = '<%=Login%>';
var msgMBSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSN").ToString() %>';
var msgError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgError").ToString() %>';
var msgAlreadyExist = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAlreadyExist").ToString() %>';

var msgInfo1 = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInfo1").ToString() %>';
var msgInfo2 = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInfo2").ToString() %>';

var msgInputMBSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBSN").ToString() %>';
var msgInputRemark = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputRemark").ToString() %>';
var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
var msgRemark = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgRemark").ToString() %>';

document.body.onload = function() {
    try {
        DataEntryControl = getCommonInputObject();
        DataEntryControl.focus();
        getAvailableData("processDataEntry");
        document.getElementById("<%=Chk.ClientID%>").disabled = true;
        document.getElementById("<%=Chk.ClientID%>").checked = false;
        station = document.getElementById("<%=station1.ClientID%>").value;
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function processDataEntry(inputData) {
    try {
        ShowInfo(" ");
        var mbsn = document.getElementById("<%=upMBSnoContent.ClientID%>").innerHTML;
        if (mbsn == "") {
            var sn = checkMBSN(inputData);
            if (sn == "") {
                alert(msgInputMBSN);
                getCommonInputObject().focus();
                getAvailableData("processDataEntry");
                return;
            }
            
            beginWaitingCoverDiv();
            WebServicePCAOQCCosmetic.ProcessMBSN(sn, "", editor, station, customer, onMBSNSucceed, onMBSNFail);
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
                    var check = (document.getElementById("<%=Chk.ClientID%>").checked == true) ? "1" : "0";
                    var lotNo = document.getElementById("<%=upLotNoContent.ClientID%>").innerHTML;
                    var remark = document.getElementById("txtReason").value;

                    var existDefect = false;
                    var defectArray = new Array();
                    var gvTable = document.getElementById("<%=gd.ClientID%>");
                    for (var i = 0; i < gvTable.rows.length - 1; i++) {
                        if (gvTable.rows[i + 1].cells[0].innerHTML != "&nbsp;") {
                            defectArray.push(gvTable.rows[i + 1].cells[0].innerText);
                            existDefect = true;
                        }
                    }
                    if (existDefect == false) {
                        alert(msgInputDefect);
                        getCommonInputObject().focus();
                        getAvailableData("processDataEntry");
                        return;
                    }

                    if (remark.trim() == "") {
                        alert(msgInputRemark);
                        getCommonInputObject().focus();
                        getAvailableData("processDataEntry");
                        return;
                    }

                    if (remark.length > 200) {
                        alert(msgRemark);
                        getCommonInputObject().focus();
                        getAvailableData("processDataEntry");
                        return;
                    }

                    beginWaitingCoverDiv();
                    WebServicePCAOQCCosmetic.save(glombsn, glombsn, lotNo, remark, check, defectArray, on9999Succeed, on9999Fail);
                    getAvailableData("processDataEntry");
                }
                else {
                    var table = document.getElementById("<%=gd.ClientID%>");
                    for (var i = 0; i < table.rows.length; i++) {
                        if (table.rows[i].cells[0].innerText == inputData) {
                            alert(msgAlreadyExist);
                            getCommonInputObject().focus();
                            getAvailableData("processDataEntry");
                            return;
                        }
                    }
                    beginWaitingCoverDiv();
                    WebServicePCAOQCCosmetic.ProcessDefect(inputData, onDefectSucceed, onDefectFail);
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

///////////////9999/////////////////
function on9999Succeed(result) {
    try {
        endWaitingCoverDiv();
        var tmpmbsn = glombsn;
        
        glombsn = "";
        document.getElementById("<%=hidMBSN.ClientID%>").value = "";
        document.getElementById("<%=upMBSnoContent.ClientID%>").innerHTML = "";
        document.getElementById("<%=upStationContent.ClientID%>").innerHTML = "";
        document.getElementById("<%=upLotNoContent.ClientID%>").innerHTML = "";
        document.getElementById("<%=upPdlineContent.ClientID%>").innerHTML = "";
        document.getElementById("txtReason").value = "";
        document.getElementById("<%=hidRepairID.ClientID%>").value = "";
        document.getElementById("<%=Chk.ClientID%>").disabled = true;
        document.getElementById("<%=Chk.ClientID%>").checked = false;
        clearTable();
        
        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();
        }
        else if (result[0] == SUCCESSRET) {
            
            if (result[1] == "0") {
                ShowInfo("MBSN:" + tmpmbsn + msgInfo1);
            }
            else {
                ShowInfo("MBSN:" + tmpmbsn + msgInfo2);
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

function on9999Fail(error) {
    try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        DataEntryControl.focus();
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

//////////////////////////MBSN/////////////////
function onMBSNSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            alert(msgSystemError);
            DataEntryControl.focus();            
        }
        else if (result[0] == SUCCESSRET) {
            glombsn = result[1];
            document.getElementById("<%=hidMBSN.ClientID%>").value = result[1];
            document.getElementById("<%=upMBSnoContent.ClientID%>").innerHTML = glombsn;
            document.getElementById("<%=upStationContent.ClientID%>").innerHTML = result[2][0];
            document.getElementById("<%=upLotNoContent.ClientID%>").innerHTML = result[2][1];
            document.getElementById("<%=upPdlineContent.ClientID%>").innerHTML = result[2][2];
            var length = result[2][5].length;
            if (length > 0) {
                document.getElementById("txtReason").value = result[2][3];
                document.getElementById("<%=hidRepairID.ClientID%>").value = result[2][4];
                document.getElementById("<%=Chk.ClientID%>").disabled = false;
                for (var i = 0; i < length; i++) {
                    var rowInfo = new Array();
                    rowInfo.push(result[2][5][i]);
                    rowInfo.push(result[2][6][i]);
                    AddRowInfo(rowInfo);
                }
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

function onMBSNFail(error) {
    try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        DataEntryControl.focus();     
    } catch (e) {
        alertAndCallNext(e.description);
    }
}

function clearTable() {
    ClearGvExtTable("<%=gd.ClientID%>", initRowsCount);
    index = 1;
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

function alertAndCallNext(message) {
    endWaitingCoverDiv();
    alert(message);
    getAvailableData("processDataEntry");
}

function checkMBSN(sn) {
    var ret = "";
    if (sn.length == 10) {
        if (sn.charAt(4) == "M" || sn.charAt(4) == "B") {
            return sn;
        }
    }
    else if (sn.length == 11) {
    if (sn.charAt(4) == "M" || sn.charAt(4) == "B") {
            ret = sn.substring(0, 10);
            return ret;
        }
        else if (sn.charAt(5) == "M" || sn.charAt(5) == "B") {
            return sn;
        }
    }
    return ret;
}

var feature = "dialogHeight:350px;dialogWidth:950px;center:yes;status:no;help:no";
function showQueryDialog() {
    var paramArray = new Array();
    paramArray[0] = "";
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = station;
    //window.showModalDialog("./PCAOQCCosmetic_Query.aspx", paramArray, 'dialogWidth:900px;dialogHeight:400px;center:yes;status:no;help:no');


    window.showModalDialog("PCAOQCCosmetic_Query.aspx", paramArray, feature);
                                                                      
}


function clearSession() {
    var mbsn = document.getElementById("<%=hidMBSN.ClientID%>").value;
    if (mbsn != "") {
        WebServicePCAOQCCosmetic.wfcancel(mbsn);
    }
}

window.onbeforeunload = clearSession;

</script>
</asp:Content>