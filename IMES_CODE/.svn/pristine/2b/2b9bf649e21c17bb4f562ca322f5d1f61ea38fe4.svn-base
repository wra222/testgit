<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PickCard page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-27  zhu lei               Create          
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PickCard.aspx.cs" Inherits="PAK_PickCard" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
     <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.expressInputAny.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PickCardService.asmx" />
			<asp:ServiceReference Path="~/PAK/Service/WebServicePDPALabel02.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width:120px;"/>
                    <col />
                    <col style="width:80px;"/>
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                         <asp:Label ID="lblDateContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblForwarder" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                         <asp:Label ID="lblForwarderContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDriver" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                         <asp:Label ID="lblDriverContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
                    </td>
                </tr>                                
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtInput" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%" Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" />
                    </td>
                </tr>                                
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>  
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var g_truckid;
        var station;
        var pCode;
        var editor;
        var customer;
        var emptyPattern = /^\s*$/;
        var inputObj;
        var emptyString = "";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
		
		//var pickid = "";
		
		var msgCreatePDF = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCreatePDF") %>';
        var msgErrCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgErrCreatePDF").ToString() %>';
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
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
          
            station = '<%=Request["Station"] %>'; //"12";
            pCode = '<%=Request["PCode"] %>'; // "OPPA052";
   
            $('#<%= txtInput.ClientID %>').expressInput({ callback: input, filter: "\\s" });
            window.setTimeout(function() {
                PickCardService.Init(null, function(result) {
                    document.getElementById("<%=lblDateContent.ClientID %>").innerText = result[0];
                    window.dateStr = result[0];
                });
            },100);
			
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
        };
		
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
        
        function input(data)
        {
			ShowInfo("");
            try
            {
               var lstPrintItem = getPrintItemCollection();
               if (lstPrintItem == null)                 
               {
                  alert(msgPrintSettingPara);
                  callNextInput();
               } 
               else {
                   g_truckid = data;
                   PickCardService.inputTruckID(emptyString, data, dateStr, editor, station, customer, lstPrintItem, onSucc, onFail);
               }
            }
            catch(e)
            {
                alert(e.description);
                callNextInput();
            }              
        }
        
        function onSucc(result)
        {
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + g_truckid + "] " + msgSuccess;
			var editsmessage = "[" + g_truckid + "] Edits " + msgSuccess;
            //ShowSuccessfulInfo(true, message);
            
            //document.getElementById("<%=lblDateContent.ClientID %>").innerText = result[0].Date;
            document.getElementById("<%=lblForwarderContent.ClientID %>").innerText = result[0].Forwarder;
            document.getElementById("<%=lblDriverContent.ClientID %>").innerText = result[0].Driver;
			
			var pickid = result[2];
			if ("Y" == result[3]){
				templateName = result[4];
				var dateNow = dateStr.replace(/\//g,'');
				pdf_Result = StartCreatePDF(pickid, g_truckid, dateStr, dateNow);
				if (!pdf_Result[0]) {
					endWaitingCoverDiv();
					ShowMessage(pdf_Result[1]);
					ShowInfo(pdf_Result[1]);
				}
				else {
					ShowSuccessfulInfo(true, editsmessage);
				}
			}
			else{
			
				ShowSuccessfulInfo(true, message);
				
				var lstPrtItem = result[1];
				var keyCollection = new Array();
				var valueCollection = new Array();

				keyCollection[0] = "@pickid";
				valueCollection[0] = generateArray(result[2]);
		
				setPrintParam(lstPrtItem, "Pick Card", keyCollection, valueCollection);
				printLabels(result[1], false);
            }

            callNextInput();
        }
        
        function onFail(result)
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            //document.getElementById("<%=lblDateContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblForwarderContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblDriverContent.ClientID %>").innerText = "";
        //    getAvailableData("processFun"); 
            endWaitingCoverDiv();
            //  inputObj.focus();
            callNextInput();
        }
        
        function clkSetting()
        {
            showPrintSetting(station, pCode);
        }
		
		function StartCreatePDF(pickid, truckid, shipdate, dateNow) {
            var r_Result = new Array();

            try {
                r_Result = CallEDITSFunc(pickid, truckid, shipdate, dateNow);

            } catch (e) {
                r_Result[0] = false;
                r_Result[1] = e.description;
                //   alert(e.description);
            }
            return r_Result;

        }
        function CallEDITSFunc(pickid, truckid, shipdate, dateNow) {

            var r_Result = new Array();
            r_Result[0] = true;
            var Paralist = new EDITSFuncParameters();
            var filepath = "";
            var filename = pickid + "-" + dateNow + "-[PosLabel].xml"
            if (xmlFilePath == "" || webEDITSaddr == "") {
                r_Result[0] = false;
                r_Result[1] = "Path error!";
                return r_Result;
            }
            filepath = xmlFilePath + "POSXML\\" + filename;
            CheckMakeDir(filepath);


            //TEST
            //filepath = "\\\\10.99.183.68\\test\\yy11111.xml";
            //dn = "4110418594000010";
            // key = "CNU3109SFS";
            //TEST
            Paralist.add(1, "FilePH", filepath);
            Paralist.add(2, "truckId", truckid);
			Paralist.add(3, "shipDate", shipdate);

            var result = invokeEDITSFuncAsync_BSam(webEDITSaddr, "GenPosXML", Paralist);
            if (result[0]) {
				var result2 = CallPdfCreateFunc(pickid, dateNow);
                if (result2[0]) {
                    //     ShowMessage("pdf ok");
                    try
                    {
						PrintPDF();
					}
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
        function CallPdfCreateFunc(pickid, dateNow) {
            var xmlfilename = pickid + "-" + dateNow + "-[PosLabel].xml";
            var xslfilename = pickid + "-" + dateNow + "-[PosLabel].xslt";
            var pdffilename = pickid + "-" + dateNow + "-[PosLabel].pdf"

            if (xmlFilePath == "" || webEDITSaddr == "") {
                alert("Path error!");
                return false;
            }

            var xmlfullpath = xmlFilePath + "POSXML\\" + xmlfilename;
            var xslfullpath = tmpFilePath + templateName;
            pdfFullPath = pdfFilePath + "POSPDF\\" + pdffilename;
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
			WshSheel.Run(cmdpdfprint, 2, false);
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
        function callNextInput() {

       
            $('#<%= txtInput.ClientID %>').expressInput('setCallback', input);

            var txtBox = document.getElementById("<%= txtInput.ClientID %>");
            if (txtBox != null) txtBox.focus();

        }
    </script>
</asp:Content>

