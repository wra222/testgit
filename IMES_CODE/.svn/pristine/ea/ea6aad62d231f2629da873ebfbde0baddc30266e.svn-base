<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine Carton
 * CI-MES12-SPEC-PAK-UI Combine DN & Pallet for BSam.doc    
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2013-04-16 Benson              Create
 * Known issues:
 * TODO:
*/
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineCartonInDN.aspx.cs" Inherits="BSam_CombineCartonInDN" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/BSam/Service/WebServiceCombineCartonInDN.asmx" />
            <asp:ServiceReference Path="~/PAK/Service/WebServicePDPALabel02.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" language="javascript ">   
       
    </script>

    <script type="text/javascript" language="javascript">
        var grvProductClientID = "<%=grvProduct.ClientID%>";
        var grvDNClientID = "<%=grvDN.ClientID%>";
        var editor;
        var customer;
        var cartonSn;
        var station;
        var pCode;
        var username;
        var key;
        var scanQty = 0;
        var actQ = 0;
        var msgInputCOO = '<%=this.GetLocalResourceObject(Pre + "_msgInputCOO") %>';
        var msgInputPE = '<%=this.GetLocalResourceObject(Pre + "_msgInputPE") %>';

        var msgWrongCOO = '<%=this.GetLocalResourceObject(Pre + "_msgWrongCOO") %>';
        var msgWrongPE = '<%=this.GetLocalResourceObject(Pre + "_msgWrongPE") %>';
        //    var inputIdx = 0; msgWrongPE
        var printItemlist;
        var msgInputPDLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgFinishCarton = '<%=this.GetLocalResourceObject(Pre + "_msgFinishCarton") %>';
        var msgNoTemp = '<%=this.GetLocalResourceObject(Pre + "_msgNoTemp").ToString() %>';
        var msgException = '<%=this.GetLocalResourceObject(Pre + "_msgException").ToString() %>';
        var msgCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgCreatePDF").ToString() %>';
        var msgErrCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgErrCreatePDF").ToString() %>';
        var index = 1;
        var initPrdRowsCount;
        var initDnRowsCount;
        var isFirstIuputCoo = false;
        var firstInput = "";
        //var secondInput = "";
        var isLastCarton = false; //尾箱;
        var inputMode = 1;
        var imgAddr = "";
        var webEDITSaddr = "";
        var xmlFilePath = "";
        var pdfFilePath = "";
        var tmpFilePath = "";
        var fopFilePath = ""
        var templateName = "";
        var pdfPrintPath;
        var pdfFullPath;
        window.onload = function() {
            initPrdRowsCount = parseInt('<%=initProductTableRowsCount%>', 10) + 1;
            initDnRowsCount = parseInt('<%=initDnTableRowsCount%>', 10) + 1;
            key = "";
            inputMode = document.getElementById("<%=hidInputMode.ClientID%>").value;
            editor = "<%=userId%>";
            customer = "<%=customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            callNextInput();
            //Get Edites Params
            var nameCollection = new Array();
            nameCollection.push("PLEditsImage");
            nameCollection.push("PLEditsURL");
            nameCollection.push("PLEditsXML");
            nameCollection.push("PLEditsPDF");
            nameCollection.push("PLEditsTemplate");
            nameCollection.push("FOPFullFileName");
            nameCollection.push("PDFPrintPath");

            WebServicePDPALabel02.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);
        }
        window.onbeforeunload = function() {
            ExitPage();
        }
        function onGetSetting(result) {


            if (result == null) {
                // setobjMSCommParaForLights();
            }
            else if (result[0] == SUCCESSRET) {
                imgAddr = result[1][0];
                webEDITSaddr = result[1][1];
                xmlFilePath = result[1][2];
                pdfFilePath = result[1][3];
                tmpFilePath = result[1][4];
                fopFilePath = result[1][5];
                pdfPrintPath = result[1][6];

            } else {
                ShowInfo("");
                var content = result[0];
                alert(content);
                ShowInfo(content);
            }

        }

        function onGetSettingFailed(result) {
            //endWaitingCoverDiv();
            //ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }
        function ExitPage() {
            if (key != "")
            { WebServiceCombineCartonInDN.Cancel(key, cartonSn, editor); }

        }
        function IsNumber(src) {
            var regNum = /^[0-9]+[0-9]*]*$/;
            return regNum.test(src);
        }



        function AddRowInfo(RowArray) {
            try {
                if (index < initPrdRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + grvProductClientID + "(RowArray,false, index)");
                }
                else {
                    eval("AddCvExtRowToBottom_" + grvProductClientID + "(RowArray,false)");
                }
                setSrollByIndex(index, false);
                setRowSelectedOrNotSelectedByIndex(index - 2, false, grvProductClientID);
                //      inputIdx = index - 2;
                setRowSelectedOrNotSelectedByIndex(index - 1, true, grvProductClientID);     //设置当前高亮行
                index++;

            }
            catch (e) {
                ShowInfo(e.description);
                //  PlaySound();
            }
        }
        //****** Begin Input Function  *******
        function input(data) {


            scanQty = parseInt(document.getElementById("<%=txtScanQty.ClientID%>").value, 10);
            printItemlist = getPrintItemCollection();
            var line = getPdLineCmbValue();

            if (line == "") {
                alert(msgInputPDLine);
                ShowInfo(msgInputPDLine);
                getAvailableData("input");
                return;
            }
            if (data == "7777") {
              //  getPdLineCmbObj().selectedIndex = 0;
                ResetValue();
                ShowInfo("");
                clearTable();
                getAvailableData("input");
                return;
            }
            if (data == "9999") {
                actQ = document.getElementById("<%=txtActualCartonQty.ClientID%>").value;
                if (isLastCarton && scanQty == actQ) {
                    beginWaitingCoverDiv();
                    WebServiceCombineCartonInDN.Save(key, cartonSn, editor, printItemlist, onSaveSucc, onSaveFail);
                    getAvailableData("input");
                    return;
                }
                else {
                    ShowInfo("尾箱已滿,才可刷9999");
                    getAvailableData("input");
                    return;

                }
            }
            if (isLastCarton && scanQty == actQ && data != "9999") {
                ShowMessage("尾箱已滿,請刷9999");
                ShowInfo("尾箱已滿,請刷9999");
                getAvailableData("input");
                return;
            }

            if (checkCustomerSNOnProductValid(data) == false) {
                alert("Wrong Code!");
                getAvailableData("input");
                return;
            }

            if (printItemlist == null || printItemlist == "") {
                alert(msgPrintSettingPara);
                getAvailableData("input");
                return;
            }
            var tmpSn = data;
            if (inputMode != 1) {
                if (data.length == 11)
                { tmpSn = data.substr(1, 10); }
                if (firstInput == "") {
                    if (CheckExistData(tmpSn)) {
                        ShowMessage("重復刷入");
                        ShowInfo("重復刷入");
                        getAvailableData("input");
                        return;
                    }
                    firstInput = data;
                    if (data.length == 11) {

                        ShowInfo(msgInputCOO, "green");
                        getAvailableData("input");
                        return;
                    }
                    else {
                        ShowInfo(msgInputPE, "green");
                        getAvailableData("input");
                        return;
                    }
                }
                if (firstInput.length == 11) {
                    if (data.length != 10 || firstInput.substr(1, 10) != data) {
                        ShowInfo(msgWrongCOO);
                        getAvailableData("input");
                        return;
                    }

                }
                else if (data.length != 11 || firstInput != tmpSn) {
                ShowInfo(msgWrongPE);
                    getAvailableData("input");
                    return;
                }
            } //if (inputMode != 1) 
            else {
                if (data.length != 10) {
                    ShowMessage(msgInputPE);
                    ShowInfo(msgInputPE);
                    getAvailableData("input");
                    return;
                }
                if (checkCustomerSNOnProductValid(data) == false) {
                    alert("Wrong Code!");
                    getAvailableData("input");
                    return;
                }
            //    tmpSn = data.substr(1, 10);
                tmpSn = data;
                if (CheckExistData(tmpSn)) {
                    ShowMessage("重復刷入");
                    ShowInfo("重復刷入");
                    getAvailableData("input");
                    return;
                }
                //  

            }

            //  WebServiceCombineCartonInDN.InputFirstCustSn(data, line, editor, station, customer, onFSNSucceed, onFSNFail);

            document.getElementById("<%=hidInput.ClientID%>").value = tmpSn;
            document.getElementById("<%=hidLine.ClientID%>").value = line;
            firstInput = "";
            ShowInfo("");
            var txtActQ = document.getElementById("<%=txtActualCartonQty.ClientID%>").value;
            if (scanQty == 0 || scanQty == txtActQ)// ********* First SN input ********
            {
                beginWaitingCoverDiv();
                document.getElementById("<%=btnInputFirstSN.ClientID%>").click();
            }
            else {
                WebServiceCombineCartonInDN.InputCustSn(key, tmpSn, printItemlist, onFSNSucceed, onFSNFail);
            }

            getCommonInputObject().focus();
            getAvailableData("input");
        }
        //****** End Input Function  *******
        function onFSNSucceed(result) {


            try {
                if (result == null) {
                    //   ShowInfo("");
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                    // "Product ID,Custromer SN,Model,Location";
                    var n1 = result[1];
                    var rowInfo = new Array();
                    rowInfo.push(result[1][0].ProductID);
                    rowInfo.push(result[1][0].CustomerSN);
                    rowInfo.push(result[1][0].Model);
                    rowInfo.push(result[1][0].Location);
                    AddRowInfo(rowInfo);
                    scanQty++;
                    document.getElementById("<%=txtScanQty.ClientID%>").value = scanQty;
                    if (actQ == scanQty) {
                        if (isLastCarton) {
                            ShowInfo("尾箱已滿,請刷9999", "green");
                            return;
                        }
                        beginWaitingCoverDiv();
                        WebServiceCombineCartonInDN.Save(key, cartonSn, editor, printItemlist, onSaveSucc, onSaveFail);

                    }

                }
                else {
                    ShowInfo("");
                    var content1 = result[0];
                    ShowMessage(content1);
                    ShowInfo(content1);
                }
            } catch (e) {
                alert(e);
                callNextInput();
            }

        }

        function onFSNFail(error) {
            endWaitingCoverDiv();
            try {
                ShowInfo("");
                var msg = error.get_message();
                if (msg.indexOf("NOTSAMEMODEL") >= 0) {
                    ResetValue();
                    clearTable();
                }
                msg = msg.replace("NOTSAMEMODEL", "");
                ShowMessage(msg);
                ShowInfo(msg);
//                ShowMessage(error.get_message());
//                ShowInfo(error.get_message());
            } catch (e) {
                alert(e.description);
            }
            callNextInput();
        }

        function btnPrintSetting_onclick() {
            showPrintSetting(station, pCode);
        }
        function checkCustomerSNOnProductValid(data) {
            var sn = data.trim();
            var ret = false;
            if (sn.length == 10) {
                if (sn.substr(0, 3) == "5CG") {
                    ret = true;

                }
            }
            else if (sn.length == 11) {
            if ((sn.substr(0, 1) == "P" || sn.substr(0, 1) == "A" )&& sn.substr(1, 3) == "5CG") {
                    ret = true;

                }
            }
            return ret;

        }


        function callNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }
        function ReIniValue() {
            firstSN = "";
            secondSN = "";
            cartonSn = "";
            key = "";
            scanQty = 0;
            index = 1;
            isLastCarton = false;
            isFirstIuputCoo = false;
            firstInput = "";
            actQ = 0;

        }

        function ResetValue() {

            if (key != "")
            { ExitPage(); }
            ReIniValue();
            document.getElementById("<%=txtCartonSN.ClientID%>").value = "";
            document.getElementById("<%=txtPalletNo.ClientID%>").value = "";
            document.getElementById("<%=txtFullCartonQty.ClientID%>").value = "";
            document.getElementById("<%=txtActualCartonQty.ClientID%>").value = "";
            document.getElementById("<%=txtScanQty.ClientID%>").value = "0";
            document.getElementById("<%=lblCartonMsg.ClientID%>").innerHTML = "";
            callNextInput();

        }


        function SetInfo() {
            document.getElementById("<%=lblCartonMsg.ClientID%>").innerHTML = "";
            document.getElementById("<%=txtCartonSN.ClientID%>").value = document.getElementById("<%=hidCartonSN.ClientID%>").value;
            document.getElementById("<%=txtPalletNo.ClientID%>").value = document.getElementById("<%=hidPalletNo.ClientID%>").value;
            document.getElementById("<%=txtFullCartonQty.ClientID%>").value = document.getElementById("<%=hidFullQty.ClientID%>").value;
            document.getElementById("<%=txtActualCartonQty.ClientID%>").value = document.getElementById("<%=hidActualQty.ClientID%>").value;
            document.getElementById("<%=txtScanQty.ClientID%>").value = "1";
            scanQty = 1;
            index = 2;
            cartonSn = document.getElementById("<%=hidCartonSN.ClientID%>").value;
            ShowInfo("");
            getCommonInputObject().focus();
            key = document.getElementById("<%=hidInput.ClientID%>").value;
            actQ = parseInt(document.getElementById("<%=txtActualCartonQty.ClientID%>").value, 10);
            var fuQ = parseInt(document.getElementById("<%=hidFullQty.ClientID%>").value, 10);
            //lblCartonMsg
            var t = fuQ - actQ;
            if (t != 0) {
                document.getElementById("<%=lblCartonMsg.ClientID%>").innerHTML = "尾箱" + actQ + "台";
                isLastCarton = true;
            }

            if (actQ == 1) {
                if (isLastCarton)
                { ShowInfo("尾箱已滿,請刷9999", "green"); endWaitingCoverDiv(); }
                else
                { WebServiceCombineCartonInDN.Save(key, cartonSn, editor, printItemlist, onSaveSucc, onSaveFail); }

            }
            else { endWaitingCoverDiv(); }
        }
        function clearTable() {
            try {
                ClearGvExtTable(grvProductClientID, initPrdRowsCount); //grvDNClientID
                ClearGvExtTable(grvDNClientID, initDnRowsCount); //grvDNClientID
                index = 1;
                setSrollByIndex(0, false);

            }
            catch (e) {
                alert(e.description);
            }
        }

        function CheckExistData(data) {
            if (data.length == 11)
            { data = data.substr(1, 10); }
            var gdObj = document.getElementById(grvProductClientID);
            var result = false;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (data.trim() != "" && gdObj.rows[i].cells[1].innerText.toUpperCase() == data) {
                    result = true;
                    break;
                }
            }
            return result;
        }
		 function PrintCarton(printItem) {
            var printlist = new Array();
            setPrintItemListParam(printItem, key);
            var cartonLabel;
            for (var p in printItem) {
                if (printItem[p].LabelType == "Bsam Carton Label") {
                    cartonLabel = printItem[p];
                }
            }
            printlist[0] = cartonLabel;
            printLabels(printlist, false);

        }
        function PrintShipto(printItem) {
            var printlist = new Array();
            setPrintItemListParam2(printItem, key);
            var cartonLabel;
            for (var p in printItem) {
                if (printItem[p].LabelType == "BSAM SHIP TO LABEL") {
                    cartonLabel = printItem[p];
                }
            }
            printlist[0] = cartonLabel;
            printLabels(printlist, false);

        }
        function onSaveSucc(result) {

            ShowInfo("");
            var pdf_Result;
            try {
                if (result == null) {

                    var content = "System Error";
                    ShowMessage(content);
                    ShowInfo(content);
                }
                else if ((result.length == 6) && (result[0] == SUCCESSRET)) {
                    // //SUCCESSRET,dn,locmsg,createpdf,templatename,PrintItem
                    var printlist = new Array();
			       templateName = result[4];
		            if (result[3] != "") {
					
                    //   printLabels(printlist, false);
					 PrintCarton(result[5]);
                        if (templateName == "") {
                            ShowMessage(msgNoTemp);
                            ShowInfo(msgNoTemp);

                        }
                        else {
						// alert('11');
                            pdf_Result = StartCreatePDF(result[1]);
                        }
                        if (!pdf_Result[0]) {
                            endWaitingCoverDiv();
                            ShowMessage(pdf_Result[1]);
                            ShowInfo(pdf_Result[1]);
                        }
                        else {
                            ShowSuccessfulInfo(true, msgFinishCarton + " ;" + result[2]);
                            //ShowInfo(msgFinishCarton + " ;" + result[2]);
                        
                         }


                    }
                    else //  if (result[3] != "")
                    {
                  

                         PrintCarton(result[5]);
                        PrintShipto(result[5]);
                 //       ShowInfo(msgFinishCarton + " ;" + result[2]);
                        ShowSuccessfulInfo(true, msgFinishCarton + " ;" + result[2]);
                        
                    }
                }
                else {
                    ShowMessage(result[0]);
                }

            }
            catch (e) {
                //   endWaitingCoverDiv();
                ShowMessage(e.description);
                //   callNextInput();
            }
            endWaitingCoverDiv();

            ReIniValue();
            callNextInput();
        }
        function onSaveFail(error) {
            endWaitingCoverDiv();
            try {
                ShowInfo("");
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                clearTable();
                ResetValue();

            } catch (e) {
                alert(e.description);
            }
        }
        function StartCreatePDF(dn) {
            var r_Result = new Array();

            try {
                r_Result = CallEDITSFunc(dn);

            } catch (e) {
                r_Result[0] = false;
                r_Result[1] = e.description;
                //   alert(e.description);
            }
            return r_Result;

        }
        function CallEDITSFunc(dn) {

            var r_Result = new Array();
            r_Result[0] = true;
            var line = getPdLineCmbValue();
            var Paralist = new EDITSFuncParameters();
            var filepath = "";
            //var filename = dn + "-" + key + "-[BoxShipLabel].xml"
            var filename = dn + "-" + cartonSn + "-[BoxShipLabel].xml"
            if (xmlFilePath == "" || webEDITSaddr == "") {
                r_Result[0] = false;
                r_Result[1] = "Path error!";
                return r_Result;
            }
            filepath = xmlFilePath + "XML\\" + line.substring(0, 1) + "\\" + filename;
            CheckMakeDir(filename);


            //TEST
            //filepath = "\\\\10.99.183.68\\test\\yy11111.xml";
            //dn = "4110418594000010";
            // key = "CNU3109SFS";
            //TEST
            Paralist.add(1, "FilePH", filepath);
            Paralist.add(2, "Dn", dn);
            Paralist.add(3, "SerialNum", key);

            var result = invokeEDITSFuncAsync_BSam(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
            if (result[0]) {
                var result2 = CallPdfCreateFunc(dn);
                if (result2[0]) {
                    //     ShowMessage("pdf ok");
                    try
                      { PrintPDF(); }
                    catch (e) {

                        if (e.description == "") {
                            r_Result[0] = false;
                            r_Result[1] = "PrintPDF  error!";
                        }
                        else {
                            r_Result[0] = false;
                            r_Result[1] = e.description;
                        }
                        // alert(e.description);

                    }

                }
                else {
                    r_Result[0] = false;
                    r_Result[1] = msgErrCreatePDF + "\r\n" + result2[1];
                    return r_Result;
                    //   ShowMessage(msgErrCreatePDF + "\r\n" + result2[1]);
                }
            }
            else {
                r_Result[0] = false;
                r_Result[1] = msgErrCreatePDF + "\r\n" + result[1];
                return r_Result;
                // ShowMessage(msgErrCreatePDF + "\r\n" + result[1]);
            }
            return r_Result;
        }
        function CallPdfCreateFunc(dn) {
            var line = getPdLineCmbValue();
            var xmlfilename = dn + "-" + cartonSn + "-[BoxShipLabel].xml";
            var xslfilename = dn + "-" + cartonSn + "-[BoxShipLabel].xslt";
            var pdffilename = dn + "-" + cartonSn + "-[BoxShipLabel].pdf"

            if (xmlFilePath == "" || webEDITSaddr == "") {
                alert("Path error!");
                return false;
            }

            var xmlfullpath = xmlFilePath + "XML\\" + line.substring(0, 1) + "\\" + xmlfilename;
            var xslfullpath = tmpFilePath + templateName;
            pdfFullPath = pdfFilePath + "pdf\\" + line.substring(0, 1) + "\\" + pdffilename;
            var islocalCreate = false;
            var result = CreatePDFfileAsynGenPDF_BSam(webEDITSaddr, xmlfullpath, xslfullpath, pdfFullPath, islocalCreate);
            return result;
        }
        function PrintPDF(filePath) {

            var pdf = new ActiveXObject("Scripting.FileSystemObject");
            var exe_path = pdfPrintPath;
            CheckMakeDir(exe_path);
            if (pdf.FileExists(exe_path + "\\pdfprintlist.txt")) {
                pdf.DeleteFile(exe_path + "\\pdfprintlist.txt");
            }
            var file = pdf.CreateTextFile(exe_path + "\\pdfprintlist.txt", true);
            file.WriteLine(pdfFullPath);
            file.Close();
            printPDFBAT();
        }
        function printPDFBAT() {

            var WshSheel = new ActiveXObject("wscript.shell");
            var exe1 = pdfPrintPath;
            if (exe1.charAt(exe1.length - 1) != "\\")
                exe1 = exe1 + "\\";
            var ClientPDFBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
            if (ClientPDFBatFilePath.charAt(ClientPDFBatFilePath.length - 1) != "\\")
                ClientPDFBatFilePath = ClientPDFBatFilePath + "\\";
            var cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + exe1 + "pdfprintlist.txt 100";
            WshSheel.Run(cmdpdfprint, 2, false);

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
        //Print Function
        function setPrintItemListParam(backPrintItemList, custSN) {


            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
      
            keyCollection[0] = "@SN";
            valueCollection[0] = generateArray(custSN);
            setPrintParam(lstPrtItem, "Bsam Carton Label", keyCollection, valueCollection);
        }
        function setPrintItemListParam2(backPrintItemList, custSN) {


            //============================================generate PrintItem List==========================================
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
    
            keyCollection[0] = "@CUSTSN";
            valueCollection[0] = generateArray(custSN);
            setPrintParam(lstPrtItem, "BSAM SHIP TO LABEL", keyCollection, valueCollection);
        }
    </script>

    <div>
        <center>
            <table width="100%" border="0">
                <tr>
                    <td align="left">
                        <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="80" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lbFloor" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:Label ID="lbFloor2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label1" CssClass="iMes_label_13pt" runat="server" Text="Product List"></asp:Label></legend>
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="grvProduct" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="150px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" HighLightRowPosition="3"
                                        Style="top: 0px; left: 0px">
                                        <Columns>
                                            <asp:BoundField DataField="Product ID" />
                                            <asp:BoundField DataField="Custromer SN" />
                                            <asp:BoundField DataField="Model" />
                                            <asp:BoundField DataField="Location" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnInputFirstSN" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">
                        <fieldset>
                            <legend>
                                <asp:Label ID="lbTableTitle" CssClass="iMes_label_13pt" runat="server"></asp:Label></legend>
                            <asp:UpdatePanel runat="server" ID="UpdatePanelTable" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="grvDN" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="120px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" HighLightRowPosition="3"
                                        HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Delivery No" />
                                            <asp:BoundField DataField="Model" />
                                            <asp:BoundField DataField="Ship Date" />
                                            <asp:BoundField DataField="Qty" />
                                            <asp:BoundField DataField="Remain Qty" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnInputFirstSN" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <fieldset style="width: auto">
                            <legend>
                                <asp:Label ID="lblInfo" runat="server" Text="Information"></asp:Label></legend>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCartonSN" Width="10%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                        <asp:TextBox ID="txtCartonSN" runat="server" Style="width: 20%" CssClass="iMes_textbox_input_Disabled"
                                            IsClear="true" ReadOnly="True" Height="22px" />
                                        <asp:Label ID="lblPalletNo" Width="10%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                        <asp:TextBox ID="txtPalletNo" runat="server" Style="width: 20%" CssClass="iMes_textbox_input_Disabled"
                                            IsClear="true" ReadOnly="True" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblCartonMsg" Width="10%" runat="server" Font-Size="XX-Large" Font-Bold="true"
                                            ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFullCartonQty" Width="10%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                        <asp:TextBox ID="txtFullCartonQty" runat="server" Style="width: 20%" CssClass="iMes_textbox_input_Disabled"
                                            IsClear="true" ReadOnly="True" />
                                        <asp:Label ID="lblActualCartonQty" Width="10%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                        <asp:TextBox ID="txtActualCartonQty" runat="server" Style="width: 20%" CssClass="iMes_textbox_input_Disabled"
                                            IsClear="true" ReadOnly="True" />
                                        <asp:Label ID="lblScanQty" Width="10%" runat="server" CssClass="iMes_label_13pt"
                                            Text="Scan Qty:"></asp:Label>
                                        <asp:TextBox ID="txtScanQty" runat="server" Style="width: 20%" CssClass="iMes_textbox_input_Disabled"
                                            IsClear="true" ReadOnly="True">0</asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="1">
                        <asp:Label ID="lbDataEntry" Width="13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="45%" IsClear="true" IsPaste="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="btnPrintSetting" type="button" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onclick="return btnPrintSetting_onclick()" />
                        <input id="btnRePrint" type="button" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onclick="return btnRePrint_onclick()"
                            style="display: none" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnInputFirstSN" runat="server" Text="Button" OnClick="btnInputFirstSN_Click"
                                    Style="display: none" />
                                <input type="hidden" runat="server" id="hidInput" />
                                <input type="hidden" runat="server" id="hidLine" />
                                <input type="hidden" runat="server" id="index" />
                                <input type="hidden" runat="server" id="hidCartonSN" />
                                <input type="hidden" runat="server" id="hidFullQty" />
                                <input type="hidden" runat="server" id="hidActualQty" />
                                <input type="hidden" runat="server" id="hidPalletNo" />
                                <input type="hidden" runat="server" value="1" id="hidInputMode" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
