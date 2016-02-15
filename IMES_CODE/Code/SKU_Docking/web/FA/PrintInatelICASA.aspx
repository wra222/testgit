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
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PrintInatelICASA.aspx.cs" Inherits="FA_PrintInatelICASA" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript">

    var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';

    var msgCustSNNull = '<%=this.GetLocalResourceObject(Pre + "_msgCustSNNull").ToString() %>';
    var msgInvalidCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidCustSN").ToString() %>';
    var msgInvalidProdID = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidProdID").ToString() %>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var msgModelNull = '<%=this.GetLocalResourceObject(Pre + "_msgModelNull").ToString() %>';
    var msgProdIDNull = '<%=this.GetLocalResourceObject(Pre + "_msgProdIDNull").ToString() %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var strProdID = "";
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
    
    
    window.onload = function() {
    //station = document.getElementById("<%=station.ClientID %>").value.trim();
    
        station = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';
        
        inputObj = getCommonInputObject();
        inputData = inputObj.value;
        getAvailableData("input");
        setPdLineCmbFocus();
        

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
                var line = getPdLineCmbValue();
                if (line == "") {
                    alert(mesNoSelPdLine);
                    setPdLineCmbFocus();
                    getAvailableData("input");
                    return;
                }
                //strProdID = SubStringSN(inputData, "ProdId");
                strCustSN = checkInput(inputData);
                if (strCustSN == '0') {
                    alert("wrong code");
                    setPdLineCmbFocus();
                    getAvailableData("input");
                    return;
                }
                else {
                    flowflag = true;
                    var printPara = setPrintPara();
                    if (printPara) {
                        beginWaitingCoverDiv();
                        PrintInatelICASAService.display(line, strCustSN, printPara, "<%=UserId%>", station, "<%=Customer%>", onDisplaySuccess, onDisplayFail);
                    }
                    else {
                        alert(msgPrintSettingPara);
                        getAvailableData("input");
                        flowflag = false;
                    }
                }
            }
        }
        else {
            setInputFocus();
        }
    }

    function setPrintPara() {

        try {
            var lstPrintItem = getPrintItemCollection();
            //打印部分
            if (lstPrintItem == null) {
                return null;
            }
        }
        catch (e) {
            alert(e.description);
            return null;
        }
     
       
       
        //lstPrintItem[0].PrintMode = 1; //template
        return lstPrintItem;
    }
    
    function onDisplaySuccess(result) {
        ShowInfo("");
        endWaitingCoverDiv();
        initFlag();
       // flowflag = false;
        try {
            setInfo( result);
            printLabelFun(result)
            var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            ShowSuccessfulInfo(true, "[" + strCustSN + "]" + msgSuccess1);
            //ShowSuccessfulInfoFormat(true, "Product ID", strProdID); // Print 成功，带成功提示音！
            setnull();
            getCommonInputObject().focus();
        }
        catch (e) {
            alert(e.description);
        }
        getAvailableData("input");

    }

    function printLabelFun(result) {
        var LabelType;
        if (result[2] == "Anatel label")
            LabelType = "Anatel Label";
        else
            LabelType = "ICASA Label";

        var labelCollection = [];
        for (var i in result[1]) {
            if (result[1][i].LabelType == LabelType) {
                labelCollection.push(result[1][i]);
                break;
            }
        }
        
        try {
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@productID";
            valueCollection[0] = generateArray(result[3]);

            setPrintParam(labelCollection, LabelType, keyCollection, valueCollection);
            printLabels(labelCollection, false);
        }

        catch (e) {
            alert(e.description);
            //lstPrintItem = null;
        }
    }
    
    function setInfo(info) {
        //set value to the label
        setInputOrSpanValue(document.getElementById("<%=lblInputProdID.ClientID %>"), info[3]); 
        setInputOrSpanValue(document.getElementById("<%=lblInputModel.ClientID %>"), info[0]["modelId"]);
    }

    function setnull() {
        //set value to the label
        setInputOrSpanValue(document.getElementById("<%=lblInputProdID.ClientID %>"),"");
        setInputOrSpanValue(document.getElementById("<%=lblInputModel.ClientID %>"), "");
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

    function EnterOrTab(cusnData) {
        if (flowflag) {
            var inputContent = cusnData.value;

            if (event.keyCode == 9 || event.keyCode == 13) {
                inputContent = inputContent.toUpperCase();

                var pattern = /[^0-9a-zA-Z]?$/;

                if (pattern.test(inputContent)) {
                    checkCustSn();
                    event.returnValue = false;
                }
                else {
                    alert(msgInvalidCustSN);
                    event.returnValue = false;
                }
            }
        }
        else {
            alert(msgProdIDNull);
            setInputFocus();
            event.returnValue = false;
        }
    }

    function checkCustSn() {
    }

    function onCheckCustSnSuccess(result) {
        ShowInfo("");
        endWaitingCoverDiv();
        try {
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                ClearAndSetFocusCUSTSN();
            }
            else if (result == SUCCESSRET) {
                printFun();
            }
            else {
                ShowMessage(result[0]);
                ShowInfo(result[0]);
                ClearAndSetFocusCUSTSN();
            }

        }
        catch (e) {
            alert(e.description);
        }
    }

    function onCheckCustSnFail(error) {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        }
        catch (e) {
            alert(e.description);
        }

        ClearAndSetFocusCUSTSN();

    }

    function printFun() {
        try {
            if (flowflag) {
                var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

                if (lstPrintItem == "" || lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    ClearAndSetFocusCUSTSN();
                    return;
                }

                beginWaitingCoverDiv();
            }
        }
        catch (e) {
            alert(e);
        }
    }

    function ClearAndSetFocusCUSTSN() {
        getCommonInputObject().focus();
    }

    function generateArray(val) {
        var ret = new Array();
        ret[0] = val;
        return ret;
    }

    function setPrintItemListParam(backPrintItemList) {

        // @prdid --Product ID
        //============================================generate PrintItem List==========================================
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@prdid";

        valueCollection[0] = generateArray(strProdID);


        /*
        * Function Name: setPrintParam
        * @param: printItemCollection
        * @param: labelType
        * @param: keyCollection(Client: Array of string.    Server: List<string>)
        * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        */

        setPrintParam(lstPrtItem, "Japanese Label", keyCollection, valueCollection);
    }


    function onPrintSuccess(result) {
        ShowInfo("");
        endWaitingCoverDiv();
        initFlag();
        try {
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
            }
            else if (result[0] == SUCCESSRET) {
                setPrintItemListParam(result[1]);

                printLabels(result[1], false);

                //==========================================end print process===================================

                ShowSuccessfulInfo(true);
            }
            else {
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }

        }
        catch (e) {
            alert(e.description);
        }

        OnClearAll();

    }

    function onPrintFail(error) {
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

        OnClearAll();
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

    //*********************************************************************************
    //| Function	:	Exit Page 
    //| Author		:	Chen Xu
    //| Create Date	:	01/20/2010
    //| Description	:	Clear the UI Session
    //| Input para.	:	MBSNo, station
    //| Ret value	:	
    //*********************************************************************************

    window.onbeforeunload = function() {
        ExitPage();
    }

    function ExitPage() {
        if (flowflag) {
            PrintInatelICASAService.Cancel(strCustSN, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
        }
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
        document.getElementById("<%=hiddenbtn.ClientID %>").click();
    }
    function showRepaint() {
        //var url = "PrintInatelICASARePrint.aspx?Station=" + station + "&PCode=" + pCode;
        var url = "PrintInatelICASARePrint.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;

       // alert(url);
        window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
    }
</script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/PrintInatelICASAService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <center>
           
      <table  width="95%" style="vertical-align:middle" cellpadding="0" cellspacing="0">
        <tr><td></td><td>&nbsp;</td></tr>
        <TR>
	    <TD style="width:25%" align="left" ><asp:Label ID="lbPdline" runat="server" 
                CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD style="width:70%" align="left"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"/></TD>
	   
    </TR>
     <tr><td></td><td>&nbsp;</td></tr>
       
        <tr>
            <td style="width:25%" align="left">
             <asp:Label ID="lblProdID" runat="server"  CssClass="iMes_label_13pt" ></asp:Label>
            </td>
            <td style="width:70%" align="left">
              <asp:Label ID="lblInputProdID" runat="server"  CssClass="iMes_label_13pt" ></asp:Label>         
            </td>
        </tr>
        <tr><td></td><td>&nbsp;</td></tr>
            
        <tr>
            <td style="width:25%" align="left" >
                <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt" ></asp:Label>
            </td>
            <td style="width:70%" align="left">
             
              <asp:Label ID="lblInputModel" runat="server"  CssClass="iMes_label_13pt" ></asp:Label>         

            </td>
        </tr>
         <tr><td></td><td>&nbsp;</td></tr> 
                  <tr><td></td><td>&nbsp;</td></tr> 
                    <tr><td></td><td>&nbsp;</td></tr> 
                      <tr><td></td><td>&nbsp;</td></tr> 
                        <tr><td></td><td>&nbsp;</td></tr> 
                               <tr><td></td><td>&nbsp;</td></tr> 
                    <tr><td></td><td>&nbsp;</td></tr> 
                      <tr><td></td><td>&nbsp;</td></tr> 
                        <tr><td></td><td>&nbsp;</td></tr> 
       <tr>
                    <td style="width: 25%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 70%" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanel2" runat="server" RenderMode="Inline">
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
        <tr>
	   
	    <td style="width:25%" align="left">&nbsp</td>
	   <td style="width:70%" align="right">&nbsp</td>
         <table border="0" width="95%">
         <td style="width:65%">&nbsp</td>
         
        <td style="width:25%" align="right"><input id="btnPrintSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
                
        <td style="width:75%"><input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="showRepaint()"
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                align="right"/></td>
	      </table>
    </tr>

        
        
 

       <tr>
        <td>&nbsp;</td>
        <td>
        <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                    <button id="hiddenbtn"  runat="server" onserverclick="hiddenbtn_Click" style="display: none" >
                    </button> 
                    <input type="hidden" runat="server" id="Hidden_ProdID"/>
                    <input type="hidden" runat="server" id="station" /> 
                    <input type="hidden" runat="server" id="pCode" /> 
            </ContentTemplate>   
        </asp:UpdatePanel>   
        </td>
        </tr>
     
     
     
      </table>
    </center>
</div>
   
</asp:Content>