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

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="SpecialModelForITCND.aspx.cs" Inherits="FA_SpecialModelForITCND" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/SpecialModelForITCNDService.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5" align="left">
                        <iMES:CmbFamily ID="CmbFamily1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                    <td style="width: 10%" align="right">
                        <input id="btnQuery" type="button" runat="server" onclick="special_query()" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5" align="left">
                        <iMES:CmbModel ID="CmbModel1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                    <td style="width: 10%" align="right">
                        <input id="btnInsert" type="button" runat="server" onclick="special_insert()" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbSpecialType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5" align="left">
                        <asp:DropDownList ID="SpecialType" Width="100%" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%" align="right">
                        <input id="btnDelete" type="button" runat="server" onclick="special_delete()" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" />
                    </td>
                </tr>
            </table>
            <hr />
            <table width="95%">
                <tr>
                    <td style="width: 95%;">
                        <asp:Label ID="lblSpecialModelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" OnGvExtRowClick="showDetailInfo(this)"
                            GvExtWidth="96%" GvExtHeight="228px" Style="top: 0px; left: 0px" Width="96%"
                            Height="220px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <button id="btnHidden" runat="server" onclick="" onserverclick="btnHidden_Click"
                style="display: none">
            </button>
            <input type="hidden" runat="server" id="hidModel" />
            <input type="hidden" runat="server" id="hidType" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="stationHF" runat="server" />
    <input type="hidden" runat="server" id="pCode" />

    <script type="text/javascript">


var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var mesNoSelectFamily = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectFamily").ToString()%>';
var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';    //增加Alert信息

var mesNoSelectModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
var mesNoSelectSpecial = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectSpecial").ToString()%>';
var msgConfirmDelete = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString()%>';
var msgDeletefalse = '<%=this.GetLocalResourceObject(Pre + "_msgDeletefalse").ToString()%>';
var msgQueryfalse = '<%=this.GetLocalResourceObject(Pre + "_msgQueryfalse").ToString()%>';

var SUCCESSRET ="SUCCESSRET";
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var tbl = "<%=gd.ClientID%>";
var index = 1;
var initRowsCount = 12;
var lstPrintItem;
var type;
var selectedRowIndex = -1;
var selectDelModle;
var selectDelType;
var querytableModel;
var querytableMark;
var queryFamily = '';
var user = '';

 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onload
//| Author		:	
//| Create Date	:	
//| Description	:	置页面焦点
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
document.body.onload = function() {
    try {
        tbl = "<%=gd.ClientID %>";
        ClearGvExtTable(tbl, initRowsCount);
        user = '<%=Request["UserName"] %>';
        index = 1;
        //inputDefect("data1");
        //inputDefect("data2");
        //inputDefect("data3");

    } catch (e) {
        alert(e.description);
    }

}

function getSpecialType() {

    if (document.getElementById("<%=SpecialType.ClientID %>").selectedIndex == 0) {
        type = '';
    }
    else {
        if (document.getElementById("<%=SpecialType.ClientID %>").selectedIndex == 1) {
            type = '0';
        } else {
            type = '1';
        }
    }

}

function special_delete() {
    try {
        ShowInfo("");
        if (confirm(msgConfirmDelete)) {

            SpecialModelForITCNDService.Delete(getFamilyCmbValue(), selectDelModle, selectDelType, onSucceedDel, onFail);
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;

        }

    } catch (e) {
        alert(e.description);

    }

}

function onSucceedDel() {
    ClearGvExtTable(tbl, initRowsCount);
    index = 1;
    //update gb table
}

function special_query() {
    try {
        ShowInfo("");

        if (getFamilyCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelectFamily);
            setFamilyCmbFocus();
        }
        else   if (getModelCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelectModel);
            setModelCmbFocus();
        }
        else {
            getSpecialType();
            if (type == '') {
                errorFlag = true;
                alert(mesNoSelectSpecial);
                setSpecialTypeFocus();
            }
            else {
                ClearGvExtTable(tbl, initRowsCount);
                index = 1;
                queryFamily = getFamilyCmbValue();
                SpecialModelForITCNDService.Query(getFamilyCmbValue(), getModelCmbValue(), type, onSucceed, onFail);
            }
        } 
    } catch (e) {
        alert(e.description);

    }

}
function setSpecialTypeFocus() {
    document.getElementById("<%=SpecialType.ClientID %>").focus();
}
function special_insert() {
    try {
        ShowInfo("");

        if (getModelCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelectModel);
            setModelCmbFocus();
        }
        else {
            getSpecialType();
            if (type == '') {
                errorFlag = true;
                alert(mesNoSelectSpecial);
                setSpecialTypeFocus();
            }
            else {
                SpecialModelForITCNDService.Insert(getFamilyCmbValue(), getModelCmbValue(), type, editor, onInsertSucceed, onFail);
            }
        }
    } catch (e) {
        alert(e.description);

    }

}

function AddRow() {
    if (querytableModel != undefined && querytableModel != null) {
        for (var i = 0; i < querytableModel.length; i++) {
            
                var rowArray = new Array();
                rowArray.push(getFamilyCmbValue());
                //rowArray.push(querytable[0]);
                rowArray.push(querytableModel[i]);
                rowArray.push(querytableMark[i]);
                rowArray.push(" ");
                
                AddRowInfo(rowArray);

            }
           
    }

}
function AddRowInfo(RowArray) {
    //alert(index);
    if (index < initRowsCount-1) {
        eval("ChangeCvExtRowByIndex_" + tbl + "(RowArray,false, index)");
    } else {
    eval("AddCvExtRowToBottom_" + tbl + "(RowArray,false)");
    }
    index++;

    setSrollByIndex(0, true, tbl);
    // setSrollByIndex(index, false);
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSucceed
//| Author		:	
//| Create Date	:	
//| Description	:	调用web service成功
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSucceed(result) {

    try {
        // inputDefect("data1");
       
        querytableModel = result[1];
        querytableMark = result[2];
        if (querytableMark[0] == undefined) {
            ShowMessage(msgQueryfalse);
            ShowInfo(msgQueryfalse)
        }
        else {
            AddRow();
            ShowSuccessfulInfo(true);
        }
    } catch (e) {
        alert(e.description);
    }    

}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onInsetSucceed
//| Author		:	
//| Create Date	:	
//| Description	:	调用web service成功
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onInsertSucceed(result) {

    try {
        // inputDefect("data1");
        if (result[3] == 'false') {
          /*  if (index == 1) {
                querytableModel = result[1];
                querytableMark = result[2];

                AddRow();
                ShowSuccessfulInfo(true);
            }*/
            ShowMessage(msgDeletefalse);
            ShowInfo(msgDeletefalse);
            
        }
        else {
            ClearGvExtTable(tbl, initRowsCount);
            index = 1;
            querytableModel = result[1];
            querytableMark = result[2];

            AddRow();
            ShowSuccessfulInfo(true);
 
        }
        
    } catch (e) {
        alert(e.description);
    }

}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onFail
//| Author		:	
//| Create Date	:	
//| Description	:	调用web service失败
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onFail(error) {

    try {
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
     
  } catch(e) {
        alert(e.description);
    }

}

function showDetailInfo(row) {
    CalDisappear();
    if (selectedRowIndex == row.index) {
        return;
    }
    setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, tbl);
    selectedRowIndex = row.index;
    setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, tbl);


    selectDelModle = row.cells[1].innerText.trim();
    selectDelType = row.cells[2].innerText.trim();
    if (selectDelModle!='')
        document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
    else
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;    

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



    </script>

</asp:Content>
