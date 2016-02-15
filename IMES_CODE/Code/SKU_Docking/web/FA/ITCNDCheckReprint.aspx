
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ITCNDCheckReprint.aspx.cs" Inherits="ITCNDCheckReprint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceITCNDCheck.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
     <TR>
	   <td></td>
    </TR
    <TR>
	    <TD style="width:12%" align="left"><asp:Label ID="lblProdID" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></TD>
	    <TD colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </ContentTemplate>
        </asp:UpdatePanel>                                        
       </TD>
    </TR>
    
   <tr>
	    <td style="width:12%" align="left" ><asp:Label ID="lbReason" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <textarea id="txtReason" style="width: 99%; height: 100px"></textarea>                
                </textarea>
                </ContentTemplate>                                     
         </asp:UpdatePanel>
         </td>  
    </tr>
                                         
    <tr>
	    <td style="width:12%" align="left"></td>
	    <td colspan="5" align="left"></td>
	   
	    <td align="right">
	        <table border="0" width="95%">
	            <td style="width:80%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" visible="False"/></td>
	            <td><input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="print()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        align="right"/></td>
	        </table>
        </td>
    </tr>
    </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>          
    </ContentTemplate>   
</asp:UpdatePanel> 


<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var mesnoprint = '<%=this.GetLocalResourceObject(Pre + "_mesnoprint").ToString()%>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';


    var SUCCESSRET = "SUCCESSRET";
    var lstPrintItem;
    var inputObj;
    var inpuProdid;
    var parentParams;
    var line;
    var editor;
    var customer;
    var station;
    document.body.onload = function() {
        parentParams = window.dialogArguments;
        line = parentParams[0];
        editor = parentParams[1];
        customer = parentParams[2];
        station = parentParams[3];
        inputObj = getCommonInputObject();
        ShowInfo("");
        getAvailableData("ProcessInput");
        getCommonInputObject().focus();
        //ITC-1360-1142
        DisplayInfoText();
    }

    function ProcessInput(inputData) {
        try {
            print();
            getAvailableData("ProcessInput");
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }

    function print() {
        try {
            var errorFlag = false;
            var msg = "";
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            var reason = document.getElementById("txtReason").value;
            inpuProdid = getCommonInputObject().value.trim();


            if (inpuProdid == "") {
                errorFlag = true;
                msg = mesNoProdId;
                alert(msg);
                getCommonInputObject().focus();
            }
                //ITC-1360-1620
            else if (reason.length > 80) {
                errorFlag = true;
                msg = mesReasonOutRange;
                alert(msg);
                document.getElementById("txtReason").focus();
            }

            if (!errorFlag) {
                var station = document.getElementById("<%=stationHF.ClientID %>").value;                
                try {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
                    {
                        msg = msgPrintSettingPara;
                        alert(msg);
                        getCommonInputObject().focus();
                        return;
                    }
                    else {
                        beginWaitingCoverDiv();
                        WebServiceITCNDCheck.RePrint(inpuProdid, reason, "", editor, station, customer, pCode, lstPrintItem, onSucceed, onFail);
                    }
                }
                catch (e1) {
                    alertAndCallNext(e1.description);
                }
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }


    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {

                if (result[1][0] == "0") {
                    ShowSuccessfulInfo(true, "[" + result[1][1] + "] " + msgSuccess);
                    setPrintItemListParam1(result[1][3], result[1][2], result[1][1]);
                    printLabels(result[1][3], true);
                }
                else {
                    ShowSuccessfulInfo(true, "[" + result[1][1] + "] " + mesnoprint);
                }
                                
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());

        } catch (e) {
            alertAndCallNext(e.description);
        }

    }
    function setPrintItemListParam1(backPrintItemList, pdline, beginProid) //Modify By Benson at 2011/03/30
    {        
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@productID";
        valueCollection[0] = generateArray(beginProid);        
        
        setPrintParam(lstPrtItem, "WWAN Label", keyCollection, valueCollection);
    }



    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        document.getElementById("txtReason").value = "";
        ShowInfo("");
        endWaitingCoverDiv();

    }

    function showPrintSettingDialog() {
        //     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
</script>
</asp:Content>

