<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="MaterialReturn.aspx.cs" Inherits="FA_MaterialReturn" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceMaterialReturn.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 100px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblMaterialType" Text="MaterialType:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="cmbMaterialType" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                </tr>
            </table>
           
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lbCollection" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td nowrap="noWrap" style="width: 150px;">
                            <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var inputFlag = false;
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 10;
        var passQty = 0;
        var totalQty = 0;
        var customerSN = "";
        var bomList;
        var customer;
        var stationId = "FAReturn";
        var inputObj;
        var ReturnType = "";
        var MaterialType = "";
        var sessionkey = "";
        var viewinputcount = 0;

        //error message
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
        var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var gvTable = document.getElementById("<%=GridViewExt1.ClientID %>");
        var msgNoCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgNoCustSN").ToString()%>';
        var msgNoVendorCT =  '<%=this.GetLocalResourceObject(Pre + "_msgNoVendorCT").ToString()%>';

        window.onload = function() {
            tbl = "<%=GridViewExt1.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["ReturnType"] %>';
            MaterialType = '<%=Request["MaterialType"] %>';
            
//            if (ReturnType == "FAReturn") {
//                stationId = "FAReturn";
//            }
        }

        window.onbeforeunload = function() 
        {
            OnCancel();
        }
        
        function initPage() 
        {
            ShowInfo("");
            tbl = "<%=GridViewExt1.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            getPdLineCmbObj().disabled = false;
            getMaterialTypeCmbObj().disabled = false;
            inputFlag = false;
            viewinputcount = 0;
            callNextInput(); 
        }
        function initPage_Success() {
            //ShowInfo("");
            tbl = "<%=GridViewExt1.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            getPdLineCmbObj().disabled = false;
            getMaterialTypeCmbObj().disabled = false;
            inputFlag = false;
            viewinputcount = 0;
            callNextInput();
        }
        function callNextInput() 
        {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function getMaterialTypeCmbValue() {
            return document.getElementById("<%=cmbMaterialType.ClientID %>").options[document.getElementById("<%=cmbMaterialType.ClientID %>").selectedIndex].value;
        }

        function getMaterialTypeCmbObj() {
            return document.getElementById("<%=cmbMaterialType.ClientID %>");
        }

        function input(data) {
            ShowInfo("");
            
            if (data == "") {
                ShowInfo("Please Input MaterialCT");
                callNextInput();
                return;
            }
            
            if ((data.length < 12 || data.length > 14) && data.length != 4) {
                if (data.length >= 90) {
                    data = data.substr(76, 13);
                }
                else {
                    ShowInfo("Input Error");
                    callNextInput();
                    return;
                }
            }
        
            var line = getPdLineCmbValue();
            if (line == "") 
            {
                ShowInfo(mesNoSelPdLine);
                callNextInput();
                return;
            }
            
            var materialType = getMaterialTypeCmbValue();
            if (materialType == "") {
                ShowInfo("Please select MaterialType");
                callNextInput();
                return;
            }
            
            if (data == "7777") 
            {
                //执行Reset
                ResetPage();
                callNextInput();
                return;
            }

            if (data == "9999") 
            {
                WebServiceMaterialReturn.save(sessionkey, onSaveSuccess, onSaveFail);
//                ResetPage();
//                callNextInput();
            }
            else {
                
                if (inputFlag) {
//                    setTable(data);
//                    callNextInput();

                    WebServiceMaterialReturn.InputMaterialCT(data, sessionkey, onMCTSuccess, onMCTFail);
                }
                else {
//                    setTable(data);
//                    callNextInput();
                    
                    WebServiceMaterialReturn.InputMaterialCTFirst(data,"",line,editor,stationId,customer, onMCTFSuccess, onMCTFFail);
                }
                
            }
        }

        function onMCTFSuccess(result) {
            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                setInfo(result);
                getPdLineCmbObj().disabled = true;
                getMaterialTypeCmbObj().disabled = true;
                inputFlag = true;
                //ShowSuccessfulInfo(true, msgSuccess);
            }
            else if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                getPdLineCmbObj().disabled = false;
                getMaterialTypeCmbObj().disabled = false;
                inputFlag = false;
                endWaitingCoverDiv();
            }
            else {
                ShowMessage(result._message);
                ShowInfo(result._message);
                getPdLineCmbObj().disabled = false;
                getMaterialTypeCmbObj().disabled = false;
                inputFlag = false;
                endWaitingCoverDiv();
            }
            //initPage_Success();
            callNextInput();
        }

        function onMCTFFail(result) {
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else {
                ShowMessage(result._message);
                ShowInfo(result._message);
                endWaitingCoverDiv();
            }
            getPdLineCmbObj().disabled = false;
            getMaterialTypeCmbObj().disabled = false;
            inputFlag = false;
            initPage_Success();
            callNextInput();
        }

        function onMCTSuccess(result) {
            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                setInfo(result);
                getPdLineCmbObj().disabled = true;
                getMaterialTypeCmbObj().disabled = true;
                inputFlag = true;
                //ShowSuccessfulInfo(true, msgSuccess);
            }
            else if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else {
                ShowMessage(result._message);
                ShowInfo(result._message);
                endWaitingCoverDiv();
            }
            //initPage_Success();
            callNextInput();
        }

        function onMCTFail(result) {
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else {
                ShowMessage(result._message);
                ShowInfo(result._message);
                endWaitingCoverDiv();
            }
            //getPdLineCmbObj().disabled = false;
            //getMaterialTypeCmbObj().disabled = false;
            //inputFlag = false;
            //initPage_Success();
            callNextInput();
        }     
              
        function setInfo(info) {
            sessionkey = info[2];
            setTable(info[1])
        }

        function setTable(info) 
        {
            var materialCT = info;

            var rowArray = new Array();
            var rw;
            if ((materialCT == null) || (materialCT  == "")) {
                rowArray.push("Null");
            }
            else {
                rowArray.push(materialCT);                     //0
            }

            //add data to table
            if (viewinputcount > 8) {
                eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                setSrollByIndex(viewinputcount, true, tbl);
            }
            else {
                eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, viewinputcount+1);");
                setSrollByIndex(viewinputcount, true, tbl);
            }
            viewinputcount = viewinputcount + 1;
        }
        
        function onSaveSuccess(result) {
            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                ResetPage(); 
                ShowSuccessfulInfo(true, "Success");
                //ShowInfo("Success");
            }
            else if(result == null)
            {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else 
            {
                ShowMessage(result._message);
                ShowInfo(result._message);
                endWaitingCoverDiv();
            }
            
            callNextInput();
        }
        
        function onSaveFail(result) {
            if (result == null) {
                ShowMessage(msgSystemError);
                ShowInfo(msgSystemError);
                endWaitingCoverDiv();
            }
            else {
                ShowMessage(result._message);
                ShowInfo(result._message);
                endWaitingCoverDiv();
            }
            ResetPage();
            callNextInput();
        }     
        
        function OnCancel() {
            if (sessionkey != "") {
                WebServiceMaterialReturn.Cancel(sessionkey);
            }
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
        }
        window.onunload = function() {
            OnCancel();
        }                                                                                 

    </script>

</asp:Content>
