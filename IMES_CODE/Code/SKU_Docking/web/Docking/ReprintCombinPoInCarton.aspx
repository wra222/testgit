<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/10/13 Warrant
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/10/13            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-28   Du.Xuan               Create   

* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ReprintCombinPoInCarton.aspx.cs" Inherits="SA_ReprintCombinPoInCarton"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/WebServiceCombinePoInCarton.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 150px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txt" runat="server"  ProcessQuickInput="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" IsClear="false" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox id="txtReason" rows="5" style="width:100%;" 
                            runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                            onblur="ismaxlength(this)" cols="20" name="S1"/>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                 <tr style="height: 30px">
                    <td colspan="4" align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                   </tr>
                   <tr>
                    <td colspan="4" align="right">
                        <button id="btnReprint" runat="server" onclick="clkReprint()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server">
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="hiddenbtn" runat="server" onserverclick="hiddenbtn_Click" style="display: none">
                    </button>
                    <input type="hidden" runat="server" id="station" />
                    <input type="hidden" runat="server" id="pCode" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript" language="javascript">

        var editor = "";
        var customer = "";
        var station = "";
        var pcode = "";

        var customerSN = "";

        var inputObj = "";
        var inputData = "";

        var snInput = true;

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara")%>';
        var msgInputReason = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgReasonNull") %>';
        var msgNoPrintItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrintItem")%>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';

        <%-- //var msgNotSame = '<%=this.GetLocalResourceObject(Pre + "_msgNotSame").ToString() %>'; --%>
        var msgNoPath = '<%=this.GetLocalResourceObject(Pre + "_msgNoPath").ToString() %>';
        <%-- //var msgNoTemp = '<%=this.GetLocalResourceObject(Pre + "_msgNoTemp").ToString() %>';--%>
        <%-- //var msgErrCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgErrCreatePDF").ToString() %>';--%>

        var imgAddr = "";
        var webEDITSaddr = "";
        var xmlFilePath = "";
        var pdfFilePath = "";
        var tmpFilePath = "";
        var fopFilePath = "";
        
        document.body.onload = function() {

            try {
                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                station = '<%=Request["Station"]%>';
                pcode = '<%=Request["PCode"]%>';

                inputObj = getCommonInputObject();
                inputData = inputObj.value;
                DisplayInfoText();
                initPage();

                var nameCollection = new Array();
                nameCollection.push("PLEditsImage");
                nameCollection.push("PLEditsURL");
                nameCollection.push("PLEditsXML");
                nameCollection.push("PLEditsPDF");
                nameCollection.push("PLEditsTemplate");
                nameCollection.push("FOPFullFileName");

                WebServiceCombinePoInCarton.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);
                //callNextInput();

            } catch (e) {
                alert(e.description);
            }
        }

        function onGetSetting(result) {

            //endWaitingCoverDiv();
            if (result[0] == SUCCESSRET) {
                imgAddr = result[1][0];
                webEDITSaddr = result[1][1];
                xmlFilePath = result[1][2];
                pdfFilePath = result[1][3];
                tmpFilePath = result[1][4];
                fopFilePath = result[1][5];
                 /* mark for Mantis 557
                var path = imgAddr + "\\*.jpg";
                var fso = new ActiveXObject("Scripting.FileSystemObject");
                var fileflag = fso.FolderExists(imgAddr);
                if (fileflag) {
                fso.CopyFile(path, "C:\\");
                }
                else {
                alert(msgNoPath);
                }
                 */
            } else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onGetSettingFailed(result) {

            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

        
        function initPage() {
            document.getElementById("<%=txtReason.ClientID %>").value = "";
            getCommonInputObject().value = "";
            ShowInfo("");
            customerSN = "";
        }

        function checkInput(inputData) {

            /*if ((inputData.length == 11) && (inputData.substring(0, 3) == "SCN")) {
                customerSN = inputData.substring(1, 11);
            }
            else if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {

                customerSN = inputData;
            }
            else {
                alert(msgInputCustSN);
                callNextInput();
                return;
            }
            */
            //document.getElementById("<%=txtReason.ClientID %>").focus();
            clkReprint();
            return;
        }

        function clkReprint() {

            var reason = document.getElementById("<%=txtReason.ClientID %>").value;
            var inputData = getCommonInputObject().value;
            
            var checkflag = false;
            //if ((inputData.length == 11) && (inputData.substring(0, 3) == "SCN")) {
			if ((inputData.length == 11) && CheckCustomerSN(inputData)) {
                inputData = inputData.substring(1, 11);
                checkflag = true;
            }
            //else if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {
			else if ((inputData.length == 10) && CheckCustomerSN(inputData)) {
                inputData = inputData;
                checkflag = true;
            }
            else if ((inputData.length == 9) || (inputData.length == 10)) {

                inputData = SubStringSN(inputData, "ProdId");
                checkflag = true;
            }
         
            var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            WebServiceCombinePoInCarton.ReprintCartonLabel(inputData, reason, "", editor, station, customer, lstPrintItem, onPrintSucceed, onPrintFail);       
            
        }

        function setPrintItemListParam(backPrintItemList, cartonNo) {
            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            //@sn, @warranty
            keyCollection[0] = "@CartonNo";
            valueCollection[0] = generateArray(cartonNo);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            //setPrintParam(lstPrtItem, "Customer SN Label", keyCollection, valueCollection);
            setPrintParam(lstPrtItem, "DK_Carton_Label", keyCollection, valueCollection);
        }

        function onPrintSucceed(result) {

            ShowInfo("");
            endWaitingCoverDiv();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                    var printflag = result[9];
                    if (printflag == "Y")
                    {
                        setPrintItemListParam(result[1], result[4]); // 使用Reprint的存储过程
                        //==========================================print process=======================================

                        /*
                        * Function Name: printLabels
                        * @param: printItems
                        * @param: isSerial
                        */
                        printLabels(result[1], false);
                    }
                    if (result[2] = "N") {
                        var ret = CallEDITSFunc(result);
                        if (ret) {
                            ret = CallPdfCreateFunc(result);
                        }
                    }

                    //==========================================end print process===================================
                    //ResetPage();
                    customerSN = result[6];
                    var cartonSn = result[4];
                    ShowSuccessfulInfoFormat(true, "Customer SN", customerSN); // Print 成功，带成功提示音！
                }
                else {
                    ResetPage();
                    ShowMessage(result[0]);
                    ShowInfo(result[0]);
                }
            }
            catch (e) {
                alert(e.description);
            }
            getCommonInputObject().value = "";
            callNextInput();
        }

        function onPrintFail(error) {

            endWaitingCoverDiv();
            try {
                ResetPage();
                getCommonInputObject().value = "";
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
                
               
            }
        }
        
        function clkSetting() {

            showPrintSetting(station, pcode);
            callNextInput();

        }

        function ResetPage() {
            initPage();
            callNextInput();
        }

        function callNextInput() {
            //getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }

        function imposeMaxLength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            return (obj.value.length < mlength);
        }

        function ismaxlength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");    
                alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
                obj.value = obj.value.substring(0, mlength);
                reasonFocus();
            }
        }
        
        function CallEDITSFunc(result) {

            var Paralist = new EDITSFuncParameters();
            var filepath = "";
            var devno = result[5];
            var cust = result[6];
            var cartno = result[4];
            //XmlFilename = DN & "-" & CN & "-[BoxShipLabel].xml"
            var filename = devno + "-" + cartno + "-[BoxShipLabel].xml"

            if (GetDebugMode()) {
                //Debug Mode get Root path from Web.conf
                xmlFilePath = GetCreateXMLfileRootPath();
                webEDITSaddr = GetEDITSIP();     //Packing List for Product Line
            }
            if (xmlFilePath == "" || webEDITSaddr == "") {
                alert("Path error!");
                return false;
            }
            filepath = xmlFilePath + "DOCKXML\\" + filename;
            CheckMakeDir(filename);

            Paralist.add(1, "FilePH", filepath);
            Paralist.add(2, "Dn", devno);
            Paralist.add(3, "SerialNum", cust);

            var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
            return IsSuccess;
        }

        function CallPdfCreateFunc(result) {
            var devno = result[5];
            var cust = result[6];
            var cartno = result[4];
            var tempName = result[7];

            var xmlfilename = devno + "-" + cartno + "-[BoxShipLabel].xml";
            var xslfilename = devno + "-" + cartno + "-[BoxShipLabel].xslt";
            var pdffilename = devno + "-" + cartno + "-[BoxShipLabel].pdf"

            if (xmlFilePath == "" || webEDITSaddr == "") {
                alert("Path error!");
                return false;
            }

            var xmlfullpath = xmlFilePath + "DOCKXML\\" + xmlfilename;
            var xslfullpath = tmpFilePath + tempName;
            var pdffullpath = pdfFilePath + "DOCKPDF\\" + pdffilename;

            //DEBUG Mode :非Client端生成PDF -false --\10.99.183.58\out\
            //            Client端生成PDF -True --c:\fis\  --這部分似乎只涉及到Packing List for Product Line 
            //--------------------------------------------------------------
            var islocalCreate = false;
            //var islocalCreate = true;
            //================================================================
            
            //marked for Mantis 557
            //var IsSuccess = CreatePDFfileAsyn(fopFilePath, xmlfullpath, xslfullpath, pdffullpath, islocalCreate);
            var IsSuccess = CreatePDFfileAsynGenPDF(webEDITSaddr, xmlfullpath, xslfullpath, pdffullpath, islocalCreate); 

            return IsSuccess;
        }

        function GetEDITSIP() {
            var HPEDITSIP = '<%=ConfigurationManager.AppSettings["HPEDITSIP"].Replace("\\", "\\\\")%>';
            return HPEDITSIP;
        }

        function GetEDITSTempIP() {
            var HPEDITSTempIP = '<%=ConfigurationManager.AppSettings["HPEDITSTEMPIP"].Replace("\\", "\\\\")%>';
            return HPEDITSTempIP;
        }
        function GetFopCommandPathfile() {
            var FopCommandPathfile = '<%=ConfigurationManager.AppSettings["FopCommandPathfile"].Replace("\\", "\\\\")%>';
            return FopCommandPathfile;
        }

        function GetTEMPLATERootPath() {
            var TEMPLATERootPath = '<%=ConfigurationManager.AppSettings["TEMPLATERootPath"].Replace("\\", "\\\\")%>';
            return TEMPLATERootPath;
        }
        function GetCreateXMLfileRootPath() {
            var CreateXMLfileRootPath = '<%=ConfigurationManager.AppSettings["CreateXMLfileRootPath"].Replace("\\", "\\\\")%>';
            return CreateXMLfileRootPath;
        }
        function GetCreatePDFfileRootPath() {
            var CreatePDFfileRootPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileRootPath"].Replace("\\", "\\\\")%>';
            return CreatePDFfileRootPath;
        }
        function GetCreatePDFfileClientPath() {
            var CreatePDFfileClientPath = '<%=ConfigurationManager.AppSettings["CreatePDFfileClientPath"].Replace("\\", "\\\\")%>';
            return CreatePDFfileClientPath;
        }
        function GetDebugMode() {
            var DEBUGmode = '<%=ConfigurationManager.AppSettings["DEBUGmode"]%>';
            if (DEBUGmode == "True")
                return true;
            else
                return false;
        }

        function PlaySound() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["DuplicateAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            obj.src = sUrl;
        }

        function PlaySoundClose() {

            var obj = document.getElementById("bsoundInModal");
            obj.src = "";
        }

    </script>

</asp:Content>
