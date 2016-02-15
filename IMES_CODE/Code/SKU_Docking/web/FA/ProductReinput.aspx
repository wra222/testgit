<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
/*
 * Answer to: ITC-1413-0039
 * Description: [新需求]增加按照文件中提供的DN 批量删除的功能
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ProductReinput.aspx.cs" Inherits="FA_ProductReinput" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
     <asp:ScriptManager ID="ScriptManager2" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceProductReinput.asmx" />
        </Services>
    </asp:ScriptManager>
    <div>
        <center>
            <table width="100%">
                <colgroup>
                    <col style="width: 110px;" />
                    <col />
                    <col />
                </colgroup>
                <tr>  
                    <td style="width: 20%">
                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 60%">
                    <iMES:Input ID="Input1" runat="server" IsClear="true" ProcessQuickInput="true" Width="99%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                    <td align="right">
                        <button type="button" style="width: 110px; height: 24px;" id="btnPrintSet" onclick="clkSetting()">
                            <%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet").ToString()%>
                        </button>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblFile" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="70%">
                        <input type="file" id="bsrFile" style="width: 100%" />
                    </td>
                    <td align="right">
                        <button type="button" style="width: 110px; height: 24px;" id="btnUpload" onclick="clickUpload()">
                            <%=this.GetLocalResourceObject(Pre + "_btnUpload").ToString()%>
                        </button>
                    </td>
                </tr>
                <tr>
                     <td >
                        <asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="70%">
                        <iMES:CmbStationForFAReiput runat="server" ID="cmbStation" Width="80%">
                        </iMES:CmbStationForFAReiput >
                        <asp:CheckBox ID="PrintChk" runat="server" CssClass="iMes_CheckBox" />
                    </td>
                    <td align="right">
                        <button type="button" style="width: 110px; height: 24px;" id="btnReinput" onclick="clickReinput()">
                            <%=this.GetLocalResourceObject(Pre + "_btnReinput").ToString()%>
                        </button>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="48%">
                        <asp:Label ID="lblReinputList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="4%">
                    </td>
                    <td width="48%">
                        <asp:Label ID="lblFailList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gve2" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="200px" SetTemplateValueEnable="False" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">                                  
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gve3" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="200px" SetTemplateValueEnable="False" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="btnUploadData" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnQueryData" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnShowDetail" runat="server" type="button" onclick="" style="display: none" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <input type="hidden" runat="server" id="hidStation" />
            <input type="hidden" runat="server" id="hidProdId" />
            <input type="hidden" runat="server" id="hidDateFrom" />
            <input type="hidden" runat="server" id="hidDateTo" />
            <input type="hidden" runat="server" id="hidSelectedDN" />
            <input type="hidden" id="hidGUID" runat="server" />
        </center>
    </div>

    <script type="text/javascript">
        var rowsCount = "<%=initRowsCount%>";

        var editor;
        var customer;
        var stationId;
        var pCode;
        
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgSuccess ='<%=this.GetLocalResourceObject(Pre + "_msgSuccess").ToString()%>'
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var selectedRowIndex = -1;

        var inputObj;

        var readProList = [];　
        var passProList = [];
        var failProList = [];

        window.onload = function() {
        
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';

            initPage();
            callNextInput();

        }

        window.onbeforeunload = function() {

            //OnCancel();

        };

        function initPage() {
          
            failProList = [];
            document.getElementById("<%=PrintChk.ClientID%>").checked = true;
            clearTable();
            
        }

        function input(inputData) {

            if (inputData == "7777") {
                clearTable();
                callNextInput();
                return;
            }
            if (inputData.length == 10) {
                //if (inputData.substring(0, 3) == "CNU") {
				if (CheckCustomerSN(inputData)) {
                    inputData = inputData;
                }
                else {
                    inputData = inputData.substring(0, 9);
                }
            }
            else if (inputData.length == 9) {
                inputData = inputData;
            }
            else {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgErrorInput").ToString()%>');
                callNextInput();
                return;
            }

            clearTable();
            beginWaitingCoverDiv();
            readProList = [];
            readProList[0] = inputData;
            WebServiceProductReinput.checkProdList(readProList, editor, stationId, customer, onCheckReturn);   
            
        }
        
        function clickUpload() {
            ShowInfo("");
            fn = document.getElementById("bsrFile").value;

            //文件名合法性检查
            if (fn == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoFile").ToString()%>');
                callNextInput();
                return;
            }
            if (fn.substring(fn.length - 4).toUpperCase() != ".TXT") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadFileName").ToString()%>');
                callNextInput();
                return;
            }
            var fso = new ActiveXObject("Scripting.FileSystemObject");  
            
            //检查文件是否存在                  
            if (!fso.FileExists(fn)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgFileNotExist").ToString()%>');
                callNextInput();
                return;
            }

            readProList = [];
            var file = fso.OpenTextFile(fn, 1);
            var i =0;
            while (!file.AtEndOfLine) {
                readProList[i] = file.ReadLine();
                i++;
            }    
            file.Close();

            if (readProList.length == 0) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgFileNull").ToString()%>');
                callNextInput();
                return;
            }
            
            clearTable();
            beginWaitingCoverDiv();
            WebServiceProductReinput.checkProdList(readProList, editor, stationId, customer, onCheckReturn);   
            
            return;
        }

        function onCheckReturn(result) {
            endWaitingCoverDiv();
            if (result == null) {
                ShowInfo("");
                ResetPage()
                alert(msgSystemError);
                ShowInfo(msgSystemError);
                callNextInput();
            }
            else if (result[0] == SUCCESSRET) {
                passProList = result[1];
                failProList = result[2];
                setSuccTable(result[1]);
                setFailTable(result[2])
                callNextInput();
            }
            else {
                ShowInfo("");
                ResetPage();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
                callNextInput();
            }
        }

        function setSuccTable(info) {

            var tbl = "<%=gve2.ClientID %>";
            var tabList = info;
    
            for (var i = 0; i < tabList.length; i++) {

                var rowArray = new Array();
                var rw;

                rowArray.push(tabList[i]["id"]);
                rowArray.push(tabList[i]["customSN"]);
                rowArray.push(tabList[i]["modelId"]);

                if (i < rowsCount-1) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }                
            }
        }


        function setFailTable(info) {

            var tbl = "<%=gve3.ClientID %>";
            var tabList = info;

            for (var i = 0; i < tabList.length; i++) {

                var rowArray = new Array();
                var rw;

                rowArray.push(tabList[i]["id"]);
                rowArray.push(tabList[i]["familyId"]);

                if (i < rowsCount-1) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(i, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                setSrollByIndex(0, true, tbl);
            }
        }

        function clickReinput() {
        
            //若[Reinput List]为空，则报错：“请Load Product”                 
            if (passProList.length == 0) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgProdNull").ToString()%>');
                return;
            }    
            var restation = getStationForFAReiputCmbValue();
            if (restation == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgStationNull").ToString()%>');
                callNextInput();
                return;
            }

            //确认继续操作
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmReinput").ToString()%>')) {
                callNextInput();
                return;
            }
            beginWaitingCoverDiv();
            WebServiceProductReinput.checkProdPassList(passProList, failProList, restation, editor, stationId, customer, onCheckPassReturn);
        }

        function onCheckPassReturn(result) {
            endWaitingCoverDiv();
            if (result == null) {
                ShowInfo("");
                ResetPage()
                alert(msgSystemError);
                ShowInfo(msgSystemError);
                callNextInput();
            }
            else if (result[0] == SUCCESSRET) {
                
                clearTable();
                passProList = result[1];
                failProList = result[2];
                setSuccTable(result[1]);
                setFailTable(result[2])
                
                if (passProList.length > 0) {
                    saveProcess();      
                }
            }
            else {
                ShowInfo("");
                ResetPage();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
                callNextInput();
            }
        }

        function saveProcess() {
        
            var restation = getStationForFAReiputCmbValue();
            var printFlag = document.getElementById("<%=PrintChk.ClientID%>").checked;
            
            var printItemlist = getPrintItemCollection();
            if (printItemlist == null || printItemlist == "") {
                ShowMessage(msgPrintSettingPara);
                callNextInput();
                return;
            }
            WebServiceProductReinput.save(passProList, restation, printFlag, editor, stationId, customer, "", printItemlist, onSaveSucc, onSaveFail);
        }
        
        function setPrintItemListParam(backPrintItemList, proId) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = new Array();
            lstPrtItem[0] = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@ProductID";
            valueCollection[0] = generateArray(proId);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            setPrintParam(lstPrtItem, "ReInput_Label", keyCollection, valueCollection);
        }

        function onSaveSucc(result) {
            endWaitingCoverDiv();
            
            if (result == null) {
                ShowInfo("");
                ResetPage()
                alert(msgSystemError);
                ShowInfo(msgSystemError);
                callNextInput();
            }
            else if (result[0] == SUCCESSRET) {
                var printFlag = document.getElementById("<%=PrintChk.ClientID%>").checked;
                if (printFlag) {
                    var printlist = new Array();
                    for (var i = 0; i < passProList.length; i++) {
                        setPrintItemListParam(result[1][i], passProList[i]["id"]);
                        printlist[i] = result[1][i];
                    }
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    printLabels(printlist, true);
                }
                var msg = msgSuccess + " " + getStationForFAReiputCmbText();
                ShowSuccessfulInfo(true, msg);
                callNextInput();
            }
            else {
                ShowInfo("");
                ResetPage();
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
                callNextInput();
            }
        }

        function onSaveFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }
        
        function clearTable() {

            ClearGvExtTable("<%=gve2.ClientID %>", rowsCount);
            ClearGvExtTable("<%=gve3.ClientID %>", rowsCount);
            
            passProList = [];
            failProList =[];
        }

        function ResetPage() {
            clearTable();
        }
        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");

        }

        function clkSetting() {
           showPrintSetting(stationId, pCode);
        }           
    </script>

</asp:Content>
