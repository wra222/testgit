<%--
 INVENTEC INVENTEC corporation (c)2011 all rights reserved. 
 Description: IEC Label Print
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-11-30  zhu lei              Create
 2012-03-01  Li.Ming-Jun          ITC-1360-0974
 2012-03-12  Li.Ming-Jun          ITC-1360-1382
 Known issues:
 TODO:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="IECLabelPrint.aspx.cs" Inherits="FA_IECLabelPrint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path="~/FA/Service/WebServiceIECLabelPrint.asmx" />
    </Services>
</asp:ScriptManager>

<div style="z-index: 0; width:95%" class="iMes_div_center">
    <table width="96%" border="0" align="center">
    <tr>
        <td width="20%" align="left"></td>
        <td align="left">
            <input id="chkPC" type="checkbox"  runat="server"  class="iMes_CheckBox" onclick="chkPC_OnCheckChanged()" />
            <asp:Label ID="lblchkPC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
    </tr>
    <tr>
	    <td align="left" >
	        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3">
	        <iMES:CmbFamily ID="cmbFamily" runat="server" Width="100" IsPercentage="true"/>
	    </td>   
    </tr>
    <tr>
	    <td align="left" >
	        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3">
	        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
        	        <iMES:CmbModelInput ID="cmbModel" runat="server"/>
                </ContentTemplate>
                <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnHidModel" EventName="ServerClick" />
                </Triggers>                                        
            </asp:UpdatePanel>
	    </td>   
    </tr>
    <tr>
	    <td align="left" >
	        <asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3">
	        <asp:DropDownList ID="drpPartType" runat="server" Width="100">
	            <asp:ListItem Selected>Memory</asp:ListItem>
            </asp:DropDownList>
	    </td>   
    </tr>
    <tr>
	    <td align="left">
	        <asp:Label ID="lblREV" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3" align="left">
	        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="txtREV" style="ime-mode:disabled;width:99%" class="iMes_textbox_input_Yellow" maxlength="5" onpaste="return false;" ondrop="return false;" onblur="UpperCase(this)" onkeypress="inputNumberAndEnglishCharDot(this)"
                                                       runat="server" />
                </ContentTemplate>
                <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                </Triggers>                                        
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
	    <td align="left" >
	        <asp:Label ID="lblDataCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3">
	        <iMES:CmbDCodeType ID="cmbDataCode" runat="server" Width="100" IsPercentage="true" IsKP="true" />
	    </td>   
    </tr>
    <tr>
	    <td align="left">
	        <asp:Label ID="lblConfig" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3" align="left">
	        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="txtConfig" style="ime-mode:disabled;width:99%" class="iMes_textbox_input_Yellow" maxlength="15" onpaste="return false;" ondrop="return false;" onblur="UpperCase(this)" onkeypress="inputNumberAndEnglishChar(this)"
                                                       runat="server" />
                </ContentTemplate>
                <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                </Triggers>                                        
            </asp:UpdatePanel>
        </td>
    </tr>   
	<tr>
	    <td align="left">
	        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="2" align="left" >
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <input type="text" id="txtQty" style="ime-mode:disabled;width:100%" class="iMes_textbox_input_Yellow"
                                                     onkeypress="inputNumberAndEnglishChar(this)"  runat="server" onpaste="return false;" ondrop="return false;" />
                </ContentTemplate>
                <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                </Triggers>                                        
            </asp:UpdatePanel>
        </td>
	    <td align="center">
	        <asp:Label ID="lblQtyTip" runat="server" iMes_textbox_input_Yellow></asp:Label>
	    </td>

    </tr>
	<tr>
	    <td></td>
        <td colspan="3" align="right">
		    <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="clkSetting()" />
		    <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
		    <input id="btnPrint" type="button"  runat="server" class="iMes_button" onclick="print()" />             
        </td>
    </tr>  
    </table>
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
        </button> 
        <button id="btnHidModel" runat="server" onserverclick="btnHidModel_Click" style="display: none" >
        </button> 
    </ContentTemplate>   
</asp:UpdatePanel> 
  


<script type="text/javascript">

var editor;
var customer;
var stationId;
var pCode;
var AccountId = '<%=Request["AccountId"] %>';
var Login = '<%=Request["Login"] %>';
var mesCheckVendorCT = '<%=this.GetLocalResourceObject(Pre + "_mesCheckVendorCT").ToString()%>';
var mesNoSelFamily = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectFamily").ToString()%>';
var mesNoSelModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
var mesNoSelPartType = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPartType").ToString()%>';
var mesNoSelDateCode = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectDateCode").ToString()%>';
var mesNoInputConfig = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputConfig").ToString()%>';
var mesNoInputREV = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputREV").ToString()%>';
var mesNoInputQty = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputQty").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

var config = '';

EnterTabJSCode = "document.getElementById('<%=btnHidModel.ClientID%>').click();";

document.body.onload = function() {
    try {
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        stationId = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';
        chkPC_OnCheckChanged()
    } catch (e) {
        alert(e.description);
    }
}   

function chkPC_OnCheckChanged() {
    if (document.getElementById("<%=chkPC.ClientID%>").checked) {
        getDecodeTypeCmbObj().disabled = true;
        getFamilyCmbObj().disabled = false;
        getModelCmbObj().disabled = false;
        setFamilyCmbFocus();
    }
    else {
        getDecodeTypeCmbObj().selectedIndex = 0;
        getDecodeTypeCmbObj().disabled = false;
        getFamilyCmbObj().selectedIndex = 0;
        getFamilyCmbObj().disabled = true;
        getModelCmbObj().value = "";
        getModelCmbObj().disabled = true;
        setDecodeTypeCmbFocus();
    }
}

function print() {
    try {
        var errorFlag = false;
        var checkPC = document.getElementById("<%=chkPC.ClientID%>").checked;
        var qty = document.getElementById("<%=txtQty.ClientID%>").value.trim();
        config = document.getElementById("<%=txtConfig.ClientID%>").value.trim();
        var rev = document.getElementById("<%=txtREV.ClientID%>").value.trim();
        var dataCode = getDecodeTypeValue();
        
        //在打印之前检查页面输入是否合法
        if (getFamilyCmbValue() == "" && checkPC) {
            errorFlag = true;
            alert(mesNoSelFamily);
            setFamilyCmbFocus();
        } else if (getModelCmbValue() == "" && checkPC) {
            errorFlag = true;
            alert(mesNoSelModel);
            setModelCmbFocus();
        } else if (dataCode == "") {
            errorFlag = true;
            alert(mesNoSelDateCode);
            setDecodeTypeCmbFocus();
        } else if (config == "") {
            errorFlag = true;
            alert(mesNoInputConfig);
            document.getElementById("<%=txtConfig.ClientID%>").focus();
        } else if (qty == "") {
            errorFlag = true;
            alert(mesNoInputQty);
            document.getElementById("<%=txtQty.ClientID%>").focus();
        } else if (rev == "") {
            errorFlag = true;
            alert(mesNoInputREV);
            document.getElementById("<%=txtREV.ClientID%>").focus();
        } 
        
        if (!errorFlag) {
           //调用web service提供的打印接口
            var lstPrintItem = getPrintItemCollection();
            //打印部分
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                OnCancel();
                return;
            }

            beginWaitingCoverDiv();
            WebServiceIECLabelPrint.print(dataCode, config, rev, qty, "", editor, stationId, customer, lstPrintItem, onSucceed, onFail);
        }
    } catch (e) {
        alert(e.description);
    }
}

function onSucceed(result) {
    try {
        endWaitingCoverDiv();

        if (result == null) {
            //service方法没有返回
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
        }
        else if (result[0] == SUCCESSRET) {
            //置成功信息
            var ctLst = new Array();
            ctLst = result[1][0];
            var dCode = result[1][1];
            var printLst = result[1][2];
            var template = printLst[0].TemplateName;
            var retREV = result[2];
            var ct = "";
            for (var i = 0; i < ctLst.length; i++) {
                 ct =ct+ ctLst[i]+";";
                 }
                var keyCollection = new Array();
                var valueCollection = new Array();

                keyCollection[0] = "@CT";
                valueCollection[0] = generateArray(ct);

                keyCollection[1] = "@DCode";
                valueCollection[1] = generateArray(dCode);

                keyCollection[2] = "@Rev";
                valueCollection[2] = generateArray(retREV);
				
				keyCollection[3] = "@Config";
				valueCollection[3] = generateArray(config);

                keyCollection[4] = "@TemplateName";
                valueCollection[4] = generateArray(template);

                setPrintParam(printLst, "KP Label", keyCollection, valueCollection);
                printLabels(printLst, false);
          

            //ShowInfo( "Successful");
            ShowSuccessfulInfo(true,msgSuccess);
            //清空页面选择
            document.getElementById("<%=btnHidden.ClientID%>").click();
        }
        else {
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
        }
    } catch (e) {
        endWaitingCoverDiv();
        alert(e.description);
    } finally {
        disabledElement();
    }
}

function onFail(error) {
   try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
    } catch(e) {
        endWaitingCoverDiv();
        alert(e.description);
    } finally {
        disabledElement();
    }
}

function disabledElement() {
    if (document.getElementById("<%=chkPC.ClientID%>").checked) {
        getDecodeTypeCmbObj().disabled = true;
    }
    else {
        getFamilyCmbObj().disabled = true;
        getModelCmbObj().disabled = true;
    }
}

function clkSetting() {
    showPrintSetting(stationId, pCode);
}

function reprint() {
    try {
        var url = "IECLabelReprint.aspx?Station=" + stationId + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + AccountId + "&Login=" + Login;
        window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
    }
    catch (e) {
        alert(e.description);
    }
}
</script>
</asp:Content>
