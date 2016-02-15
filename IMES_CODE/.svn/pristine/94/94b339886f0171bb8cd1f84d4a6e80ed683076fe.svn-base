<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for FRU Carton Label Print (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI FRU Carton Label for Docking
 * UC:CI-MES12-SPEC-PAK-UC FRU Carton Label for Docking      
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-25  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="FRUCartonLabel.aspx.cs" Inherits="PAK_FRUCartonLabel" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="~/CommonControl/JS/iMESCommonUse.js"></script>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
            </Services>
        </asp:ScriptManager>
    <div id="div1" style="z-index: 0;">
        <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td colspan="3" height="50"></td>
            </tr>
            <tr valign="middle">
                <td width="250" align="left">
                    <asp:Label ID="lblModel" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td width="350" align="left">
                    <asp:TextBox ID="txtModel" runat="server" Width="300" MaxLength="20" TabIndex="101"></asp:TextBox>
                </td>
                <td align="center">
                    <button type="button" style ="width:110px; height:24px;" id="btnPrint" onclick="clickPrint()" tabindex="104">
                        <%=this.GetLocalResourceObject(Pre + "_btnPrint").ToString()%>
                    </button>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="20"></td>
            </tr>
            <tr valign="middle">
                <td width="16%" align="left">
                    <asp:Label ID="lblPCS" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPCS" runat="server" Width="100" MaxLength="4" TabIndex="102"></asp:TextBox>
                </td>
                <td align="center">
                    <button type="button" style ="width:110px; height:24px;" id="btnPrintSetting" onclick="clickPrintSetting()" tabindex="105">
                        <%=this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString()%>
                    </button>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="20"></td>
            </tr>
            <tr valign="middle">
                <td width="16%" align="left">
                    <asp:Label ID="lblQty" runat="server" class="iMes_label_13pt"> </asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtQty" runat="server" Width="100" MaxLength="4" TabIndex="103"></asp:TextBox>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="20"></td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
 


var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError =  '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';  

var msgNoModel = '<%=this.GetLocalResourceObject(Pre + "_msgNoModel").ToString() %>';
var msgBadPCS = '<%=this.GetLocalResourceObject(Pre + "_msgBadPCS").ToString() %>';
var msgBadQty = '<%=this.GetLocalResourceObject(Pre + "_msgBadQty").ToString() %>';
var msgPrintDone = '<%=this.GetLocalResourceObject(Pre + "_msgPrintDone").ToString() %>';
var msgBadModel = '<%=this.GetLocalResourceObject(Pre + "_msgBadModel").ToString() %>';

var objModel = document.getElementById("<%=txtModel.ClientID %>");
var objPCS = document.getElementById("<%=txtPCS.ClientID %>");
var objQty = document.getElementById("<%=txtQty.ClientID %>");
var pattInt = /^[\d]+$/;
var printLst = null;
var stationId;
var pCode;

document.body.onload = function() {
    try {
        stationId = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';
        objModel.focus();
    }
    catch (e) {
        alert(e.description);
    }
};

function clickPrint() {
    valModel = objModel.value.trim();
    valPCS = objPCS.value.trim();
    valQty = objQty.value.trim();
    
    if (objModel.value.trim() == "") {
        alert(msgNoModel);
        objModel.select();
        return false;
    }
    objModel.value = valModel;

    if (!pattInt.exec(valPCS) || parseInt(valPCS) <= 0) {
        alert(msgBadPCS);
        objPCS.select();
        return false;
    }
    valPCS = parseInt(valPCS);
    objPCS.value = valPCS.toString();

    if (!pattInt.exec(valQty) || parseInt(valQty) <= 0) {
        alert(msgBadQty);
        objQty.select();
        return false;
    }
    valQty = parseInt(valQty);
    objQty.value = valQty.toString();

    try {
        PageMethods.CheckModelExist('<%=Customer %>', valModel, onCheckModelSuccess, onFail);
    }
    catch (e) {
        alert(e.description);
    }

    return;
}

function onCheckModelSuccess(result) {
    try {
        if (result == null) {
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
        }
        else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
            flag = result[1];
            if (flag) {
                if (printLst == null) {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null) {
                        alert(msgPrintSettingPara);
                        return;
                    }

                    PageMethods.GetPrintTemplate('<%=Customer %>', lstPrintItem, onGetPrintTemplateSuccess, onFail);
                }
                else {
                    doPrint();
                }
            }
            else {
                ShowErrorMessage(msgBadModel);
                objModel.select();
            }
        }
        else {
            ShowInfo("");
            var content1 = result[0];
            ShowMessage(content1);
            ShowInfo(content1);
        }
    } catch (e) {
        alert(e.description);
    }
}

function onGetPrintTemplateSuccess(result) {
    try {
        if (result == null) {
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
        }
        else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
            printLst = result[1];
            doPrint();
        }
        else {
            ShowInfo("");
            var content1 = result[0];
            ShowMessage(content1);
            ShowInfo(content1);
        }

    } catch (e) {
        alert(e.description);
    }
}

function doPrint() {
    valModel = objModel.value.trim();
    valPCS = parseInt(objPCS.value.trim());
    valQty = parseInt(objQty.value.trim());
    cartonCnt = parseInt(valQty / valPCS);
    pcsInLastCarton = valQty % valPCS;

    if (cartonCnt > 0) {
        setPrintItemListParam(printLst, valModel, valPCS);
        for (i = 0; i < cartonCnt; i++) {
            printLabels(printLst, false);
        }
    }

    if (pcsInLastCarton > 0) {
        setPrintItemListParam(printLst, valModel, pcsInLastCarton);
        printLabels(printLst, false);
    }

    ShowSuccessfulInfo(true, msgPrintDone);
    objModel.value = "";
    objPCS.value = "";
    objQty.value = "";
    objModel.focus();
}

function setPrintItemListParam(backPrintItemList, model, qty) {
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@Model";
    valueCollection[0] = generateArray(model);
    keyCollection[1] = "@Qty";
    valueCollection[1] = generateArray(qty);

    setPrintParam(lstPrtItem, "DK_Carton_FRU", keyCollection, valueCollection);
}

function onFail(error) {
    try {
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
    } catch (e) {
        alert(e.description);
    }
}

function clickPrintSetting() {
    showPrintSetting(stationId, pCode);
    return;
}

function ShowErrorMessage(msg)
{
    ShowMessage(msg);
    ShowInfo(msg);    
}

    </script>

</asp:Content>
