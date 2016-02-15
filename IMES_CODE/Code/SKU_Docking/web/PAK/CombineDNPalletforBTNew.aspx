<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine DN & Pallet for BT
 * CI-MES12-SPEC-PAK-Combine DN & Pallet for BT.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineDNPalletforBTNew.aspx.cs" Inherits="PAK_CombineDNPalletforBTNew"
    Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="Service/CombineDNBTWebService.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" language="javascript">

        var editor = '<%=userId%>';
        var customer = '<%=customer%>';
        var station = '<%=station%>';
        var currentNO = "";
        var lastId = 1;
        var table = "";
        var inputObj;
        var msgModelCheck = '<%=this.GetLocalResourceObject(Pre + "_msgModelCheck").ToString() %>';
        var msgModelNoFind = '<%=this.GetLocalResourceObject(Pre + "_msgModelNoFind").ToString() %>';
        var msgNoPath = '<%=this.GetLocalResourceObject(Pre + "_msgNoPath").ToString() %>';
        var msgModeChange = '<%=this.GetLocalResourceObject(Pre + "_msgModeChange").ToString() %>';
        var msgNoCDSI = '<%=this.GetLocalResourceObject(Pre + "_msgNoCDSI").ToString() %>';

        var PLEditsURL = '';
        var PLEditsTemplate = '';
        var PLEditsXML = '';
        var PLEditsPDF = '';
        var PLEditsImage = '';
        var FOPFullFileName = '';
        var PDFPrintPath = '';
        var successTemp = "";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCreatePDFFail = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCreatePDFFail") %>';
        var pdfCreateFailed = "";
        var isFocus = 0;

        window.onload = function() {
            GetFloor();
            getPdLineCmbObj().selectedIndex = 0;
            document.getElementById("<%=HSN.ClientID%>").value = "";
            //getFloorCmbObj().selectedIndex = 0;
            table = document.getElementById("<%=gridViewDN.ClientID%>");
            inputObj = getCommonInputObject();
            callNextInput();
            var nameCollection = new Array();
            nameCollection.push("PLEditsImage");
            nameCollection.push("PLEditsURL");
            nameCollection.push("PLEditsXML");
            nameCollection.push("PLEditsTemplate");
            nameCollection.push("PLEditsPDF");
            nameCollection.push("FOPFullFileName");
            beginWaitingCoverDiv();
            CombineDNBTWebService.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);

        }
        function GetFloor() {
            document.getElementById("CmbFloor").selectedIndex = 0;
            document.getElementById("CmbFloor").focus();
            getAvailableData("processDataEntry");

            var f = document.getElementById('CmbFloor');
            var allowFloors = '<%=Request["Floor"] %>'.split(",");
            for (var i = 0; i < allowFloors.length; i++) {
                f.options.add(new Option(allowFloors[i], allowFloors[i]));
            }
        }
        function onGetSetting(result) {

            endWaitingCoverDiv();
            callNextInput();
            if (result[0] == SUCCESSRET) {
                PLEditsImage = result[1][0];
                PLEditsURL = result[1][1];
                PLEditsXML = result[1][2];
                PLEditsTemplate = result[1][3];
                PLEditsPDF = result[1][4];
                FOPFullFileName = result[1][5];
                var path = PLEditsImage + "\*.jpg";


            } else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }
            callNextInput();
        }

        function onGetSettingFailed(result) {
            endWaitingCoverDiv();
            //ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }
        function IsNumber(src) {
            return src == parseInt(src);
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");

        }

        function DisplsyMsg(src) {
            ShowMessage(src);
            ShowInfo(src);
        }

        function input(data) {
            ShowInfo("");
            successTemp = "";
            pdfCreateFailed = "";
            var inputTextBox1 = data.trim();
            var line = "";
            var floor = "";
            line = getPdLineCmbValue();
            floor = document.getElementById("CmbFloor").selectedIndex;

            var pdLineObject = getPdLineCmbObj();
            if (line == "") {
                alert("Please input PdLine first!")
                callNextInput();
                return;
            }
            document.getElementById("<%=HLine.ClientID%>").value = line;
            if (floor == "") {
                alert("Please input Floor first!")
                callNextInput();
                return;
            }
            floor = document.getElementById("CmbFloor")[document.getElementById("CmbFloor").selectedIndex].text;
            document.getElementById("<%=HFloor.ClientID%>").value = floor;
            inputTextBox1 = inputTextBox1.toUpperCase();
            if (document.getElementById("<%=HSN.ClientID%>").value == "") {
                if (inputTextBox1.length == 10 || inputTextBox1.length == 11) {
                    pattCustSN1 = /^CN.{8}$/;
                    pattCustSN2 = /^SCN.{8}$/;
                    //if (pattCustSN1.exec(inputTextBox1) || pattCustSN2.exec(inputTextBox1)) {
					if (CheckCustomerSN(inputTextBox1)) {
                        if (inputTextBox1.length == 11) {
                            inputTextBox1 = inputTextBox1.substring(1, 11);
                        }
                    }
                    else {
                        resetLast();
                        DisplsyMsg("Wrong Code!");
                        return;
                    }
                }
                else {
                    resetLast();
                    DisplsyMsg("Wrong Code!");
                    return;
                }
                document.getElementById("<%=HSN.ClientID%>").value = inputTextBox1;
                beginWaitingCoverDiv();
                document.getElementById("<%=btnCheckProduct.ClientID%>").click();
                callNextInput();
            }
        }

        function getSuccess(name) {
            pdfCreateFailed = "";
            endWaitingCoverDiv();
            document.getElementById("txtPallet").value = document.getElementById("<%=HPalletNO.ClientID%>").value;
            document.getElementById("txtScanQty").value = document.getElementById("<%=HQQty.ClientID%>").value;
            document.getElementById("txtPalletQty").value = document.getElementById("<%=HPQty.ClientID%>").value;
            callNextInput();
            if (name != "") {
                getPDF(name);
                try {
                    getPDF(name);
                }
                catch (err) {

                    ShowMessage(err);
                    callNextInput();
                    return;
                }

            }

            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            successTemp = "";
            if (temp != "") {
                //successTemp = msgSuccess + " Customer SN: " + temp  ;
                successTemp = "[" + temp + "] " + msgSuccess;
            }
            if (pdfCreateFailed != "") {
                successTemp = successTemp + "\n" + pdfCreateFailed;
            }

            if (successTemp != "") {
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
            document.getElementById("<%=HSN.ClientID%>").value = "";

        }

        function resetLast() {
            document.getElementById("<%=HSN.ClientID%>").value = "";
            callNextInput();
        }
        function ResetAll() {

            document.getElementById("<%=HSN.ClientID%>").value = "";
            document.getElementById("txtScanQty").value = "";
            document.getElementById("txtPalletQty").value = "";
            document.getElementById("txtPallet").value = "";
            
            callNextInput();

        }


        function getEditURL(edit) {
            PLEditsURL = edit;
        }
        function getXML(xml) {
            PLEditsXML = xml;
        }
        function getEditsPDF(editsPDF) {
            PLEditsPDF = editsPDF;
        }



        function getImage(image) {
            PLEditsImage = image;
            var path = PLEditsImage + "\\*.jpg";
        }

        function getTemplate(template) {
            PLEditsTemplate = template;
        }

        function getFOPFullFileName(fop) {

            FOPFullFileName = fop;

        }
        function getAllSetting(ret) {

            PLEditsImage = ret[0];
            PLEditsURL = ret[1];
            PLEditsXML = ret[2];
            PLEditsTemplate = ret[3];
            PLEditsPDF = ret[4];
            FOPFullFileName = ret[5];
            var path = PLEditsImage + "*.jpg";

        }

        function getPDF(tempName) {
            var CmbPL = document.getElementById("<%=HLine.ClientID%>").value;
            var DeliveryNo = document.getElementById("<%=HDN.ClientID%>").value;
            if ("" == DeliveryNo || "------------" == DeliveryNo) {
                if ("" != document.getElementById("<%=HDNTemp.ClientID%>").value) {
                    DeliveryNo = document.getElementById("<%=HDNTemp.ClientID%>").value;
                }
            }
            var CPQ = document.getElementById("<%=HSN.ClientID%>").value;
            var cmbPLSub = CmbPL.substr(0, 1);
            var XmlFilename = cmbPLSub + "\\" + DeliveryNo + "-" + CPQ + "-[BoxShipLabel].xml";
            var PdfFilename = cmbPLSub + "\\" + DeliveryNo + "-" + CPQ + "-[BoxShipLabel].pdf";
            CallEDITSFunc(XmlFilename, DeliveryNo, CPQ);
            CallPdfCreateFunc(XmlFilename, tempName, PdfFilename);
        }

        function CallEDITSFunc(XmlFilename, DeliveryNo, CPQ) {
            var Paralist = new EDITSFuncParameters();
            var xmlpathfile = "";
            var webEDITSaddr = "";

            if (PLEditsXML == "" || PLEditsURL == "") {
                alert("EDIT Path error!");
                return false;
            }
            xmlpathfile = PLEditsXML + "XML\\" + XmlFilename;
            webEDITSaddr = PLEditsURL;
            Paralist.add(1, "FilePH", xmlpathfile);
            Paralist.add(2, "Dn", DeliveryNo);
            Paralist.add(3, "SerialNum", CPQ);
            var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
            return IsSuccess;
        }

        function CallPdfCreateFunc(XmlFilename, xsl, PdfFilename) {
            var xmlfilename, xslfilename, pdffilename;
            if (PLEditsXML == "" || PLEditsURL == "" || PLEditsTemplate == "") {
                alert("EDIT Path error!");
                return false;
            }

            //Run Mode Get Path from DB, set Full Path
            xmlfilename = PLEditsXML + "XML\\" + XmlFilename;
            xslfilename = PLEditsTemplate + xsl;
            pdffilename = PLEditsPDF + "pdf\\" + PdfFilename;

            var islocalCreate = false;
            //var islocalCreate = true;
            //================================================================
            //var IsSuccess = CreatePDFfileAsyn(FOPFullFileName, xmlfilename, xslfilename, pdffilename, islocalCreate);
            var IsSuccess = CreatePDFfileAsynGenPDF(PLEditsURL, xmlfilename, xslfilename, pdffilename, islocalCreate);
            if (!IsSuccess) {
                pdfCreateFailed = msgCreatePDFFail;
            }
            else {
                pdfCreateFailed = "";
            }
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
  
    </script>

    <div>
        <center>
            <table border="0" width="100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lbPdLine" Width="9%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="80" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lbFloor" Width="9%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        <select id="CmbFloor" name='CmbFloor' style="width: 80%">
                           <option value="1">&nbsp</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <asp:Label ID="lbTableDNTitle" runat="server"></asp:Label></legend>
                            <asp:UpdatePanel runat="server" ID="UpdatePanelTableDN" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gridViewDN" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="148px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        Width="97%" Height="148px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Delivery NO" />
                                            <asp:BoundField DataField="Model" />
                                            <asp:BoundField DataField="Customer P/N" />
                                            <asp:BoundField DataField="PoNo" />
                                            <asp:BoundField DataField="Date" />
                                            <asp:BoundField DataField="Qty" />
                                            <asp:BoundField DataField="Packed Qty" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lbPallet" Width="13%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                <input type="text" id="txtPallet" readonly="readonly" style="width: 35%" class="iMes_textbox_input_Disabled"
                                    value="" />
                                <asp:Label ID="lbScanQty" Width="8%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                <input type="text" id="txtScanQty" readonly="readonly" style="width: 8%" class="iMes_textbox_input_Disabled"
                                    value="" />
                                <asp:Label ID="lbPalletQty" Width="8%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                <input type="text" id="txtPalletQty" readonly="readonly" style="width: 8%" class="iMes_textbox_input_Disabled"
                                    value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="1">
                        <asp:Label ID="lbDataEntry" Width="13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="60%" IsClear="true" IsPaste="true" onfocus="DoFocus();" onblur="DoNotFocus();" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="btnPrintSetting" type="button" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" visible="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnCheckProduct" runat="server" type="button" style="display: none" onserverclick="btnCheckProduct_ServerClick" />
                                <button id="btnClear" runat="server" type="button" style="display: none" />
                                <button id="btnAssign" runat="server" type="button" style="display: none" onserverclick="btnAssign_ServerClick" />
                                <button id="btnDisplay" runat="server" type="button" style="display: none" />
                                <input type="hidden" runat="server" id="HDN" />
                                <input type="hidden" runat="server" id="HSN" />
                                <input type="hidden" runat="server" id="HQQty" />
                                <input type="hidden" runat="server" id="HPQty" />
                                <input type="hidden" runat="server" id="HPalletNO" />
                                <input type="hidden" runat="server" id="HLine" />
                                <input type="hidden" runat="server" id="HFloor" />
                                <input type="hidden" runat="server" id="HDisplay" />
                                <input type="hidden" runat="server" id="HDNTemp" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
