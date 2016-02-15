<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: Japanese Label Print
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-15  Chen Xu(EB1-4)       Create
 Known issues:
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MVunpack.aspx.cs" Inherits="FA_MVunpack" Title="MV Unpack" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript">

    var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
    var strModelDisplay = "";
    var strCustSN = "";
    var station = "";
    var inputObj = "";
    var inputData = "";
    var uutInput = true;
    var flowflag = false;
    var accountid = '<%=AccountId%>';
    var username = '<%=UserName%>';
    var login = '<%=Login%>';
    var customer = '<%=Customer%>';
    var editor = '<%=UserId%>';
    var pCode = "";
    var emptyPattern = /^\s*$/;

    window.onload = function() {

        station = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';

        inputObj = getCommonInputObject();
        inputData = inputObj.value;
        getAvailableData("input");
        
        setReturnStationCmbFocus();

        ShowInfo("");
    }
    function  initFlag()
    {
        flowflag = false;
    }

    function checkInput(data) {
        if (data.length == 10) {
            //if (data.substring(0, 3) == "CNU")
			if (CheckCustomerSN(data))
                return data;
        }
        return '0';
    }
    
    function input(inputData) {

        if (uutInput) {
            if (flowflag) {
                ExitPage();
                getAvailableData("input");
            }
            else {
                var returnStation = getReturnStationCmbValue();

                if (emptyPattern.test(returnStation)) {
                    alert(mesNoSelPdLine);
                    setReturnStationCmbFocus();
                    getAvailableData("input");
                    return;
                }

                strCustSN = checkInput(inputData);
                if (strCustSN == '0') {
                    alert("wrong code");
                    setReturnStationCmbFocus();
                    getAvailableData("input");
                    return;
                }
                else {
                    flowflag = true;
                    beginWaitingCoverDiv();
                    WebServicePIAOutput.MVunpack(returnStation, strCustSN, "<%=UserId%>", station, "<%=Customer%>", onDisplaySuccess, onDisplayFail);

                }
            }
        }
        else {
            setInputFocus();
        }
    }

    
    
    function onDisplaySuccess(result) {
        ShowInfo("");
        endWaitingCoverDiv();
        initFlag();
       // flowflag = false;
        try {
            //setInfo( result);
            //printLabelFun(result)
            //var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            //ShowSuccessfulInfo(true, "[" + strCustSN + "]" + msgSuccess1);
            //ShowSuccessfulInfoFormat(true, "Product ID", strProdID); // Print 成功，带成功提示音！
            //CustSN：XXX下一站去”+ [Return STation].Text
            ShowSuccessfulInfo(true, "CustSN：" + strCustSN + "下一站去 [" + getReturnStationCmbText() + "]");
            
            
            //setnull();
            setReturnStationCmbFocus();
        }
        catch (e) {
            alert(e.description);
        }
        getAvailableData("input");

    }

   

    function onDisplayFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();
        initFlag();
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

        }
        catch (e) {

            alert(e.description);
        }

        OnInputClearClick();
        getAvailableData("input");

    }
 
    function ClearAndSetFocusCUSTSN() {
        getCommonInputObject().focus();
    }


    function OnClearAll() {
        getCommonInputObject().value = "";
        inputData = "";
        strProdID = "";
        strModelDisplay = "";
        strCustSN = "";

        setInputFocus();
    }

    function OnInputClearClick() {
        getCommonInputObject().value = "";
        inputData = "";
        strProdID = "";
        strModelDisplay = "";
        strCustSN = "";
        setInputFocus();
    }

    function setInputFocus() {
        getCommonInputObject().focus();
    }

    function showPrintSettingDialog() {
        showPrintSetting(station, pCode);
    }


    window.onbeforeunload = function() {
        ExitPage();
    }

    function ExitPage() {

    }

    function onClearSucceeded(result) {
        ShowInfo("");
        try {
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
            }
            else if (result == SUCCESSRET) {
            }
            else {
                ShowMessage(result);
                ShowInfo(result);
            }


        }
        catch (e) {
            alert(e.description);

        }
        OnClearAll();

    }

    function onClearFailed(error) {
        ShowInfo("");
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        }
        catch (e) {
            alert(e.description);
        }

        OnClearAll();
    }

    function ResetPage() {
        ExitPage();
        OnClearAll();
        ShowInfo("");
        
    }
   
</script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
             <asp:ServiceReference Path="~/FA/Service/WebServicePIAOutput.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <center>
           
      <table  width="95%" style="vertical-align:middle" cellpadding="0" cellspacing="0">
        <tr><td></td><td>&nbsp;</td></tr>
        <TR>
	    <TD style="width:25%" align="left" ><asp:Label ID="lbRetStation" runat="server" 
                CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD style="width:110px" align="left"><iMES:CmbReturnStation ID="CmbReturnStation" runat="server" /></TD>
	   
    </TR>
     <tr><td></td><td>&nbsp;</td></tr>
       
        <tr>
            <td style="width: 25%" align="left">
                        <asp:Label ID="lbCustSN" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 70%" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                            
                            <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none"
                                    >
                                </button>
                                <button id="btnGridClear" runat="server" type="button" onclick="" style="display: none"
                                    >
                                </button>
                                <input id="prodHidden" type="hidden" runat="server" />
                                <input id="sumCountHidden" type="hidden" runat="server" />
                                <input type="hidden" runat="server" id="Hidden1" />
                                <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
        </tr>
  
     
      </table>
    </center>
</div>
   
</asp:Content>