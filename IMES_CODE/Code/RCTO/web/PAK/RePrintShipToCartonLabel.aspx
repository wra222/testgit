
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RePrintShipToCartonLabel.aspx.cs" Inherits="RePrintShipToCartonLabel" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/ShipToCartonLabel.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    <tr>
	   <td></td>
    </tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lblCustomSN" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                <ContentTemplate>	    
        <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </ContentTemplate>
   
        </asp:UpdatePanel>                                        
       </td>
    </tr>
    
    <tr>
	    <td style="width:12%" align="left" ><asp:Label ID="lbReason" runat="server" 
                CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <textarea id="txtReason" style="overflow:auto; width: 99%; height: 100px"></textarea>                
                </textarea>
                </ContentTemplate>                                     
         </asp:UpdatePanel>
         </td>  
    </tr>
           
                
              
    <tr>
	    <td style="width:12%" align="left">&nbsp;</td>
	    <td colspan="5" align="left">&nbsp;</td>	   
	    <td align="right">
	        <table border="0" width="100%">
	            <tr><td style="width:80%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>                
	            </tr>
	        </table>
        </td>
    </tr>    
    <tr>
	    <td style="width:12%" align="left">&nbsp;</td>
	    <td colspan="5" align="left">&nbsp;</td>	   
	    <td align="right">
	        <table border="0" width="100%">	            
	            <tr><td style="width:80%" align="right"><input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="print()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        align="right"/></td>	                    
                </tr>
	        </table>
	        
        </td>
    </tr>
    <tr>
        <td>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
        <ContentTemplate>
        <asp:HiddenField ID="stationHF" runat="server" />
        <input type="hidden" runat="server" id="pCode" />
        <input type="hidden" runat="server" id="addr" /> 
        <input type="hidden" runat="server" id="exepath" />
        <input type="hidden" runat="server" id="pdfpath" />
        <input type="hidden" runat="server" id="editsURL" />
        <input type="hidden" runat="server" id="editsXML" />
        <input type="hidden" runat="server" id="editsTEMP" />
        <input type="hidden" runat="server" id="editsFOP" />
        <input type="hidden" runat="server" id="editsPDF" />
        </ContentTemplate>
        </asp:UpdatePanel> 

        </td>
    </tr>
    </table>
    </center>
       
</div>


<script type="text/javascript">

var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var mesReasonOutRange= '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
var mesNoCustomerSN = '<%=this.GetLocalResourceObject(Pre + "_mesNoCustomerSN").ToString()%>';

var SUCCESSRET ="SUCCESSRET";
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var lstPrintItem;
var inpuProdid;
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var preType = 0;
var path2 = "c:\\FIS\\";
var path1 = "c:\\EFIS-Workdir\\itc202007-Zhanghe\\test\\pdfprintlist.txt";
var mesPrintTip = '<%=this.GetLocalResourceObject(Pre + "_mesPrintTip").ToString()%>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';


document.body.onload = function() {
    inpuProdid = "";
    ShowInfo("");
    getAvailableData("ProcessInput");
    getCommonInputObject().focus();
    preType = 0;
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


function print()
{  
    try {
        var errorFlag = false;
        var msg="";
	    var pCode = document.getElementById("<%=pCode.ClientID%>").value;
        var reason=document.getElementById("txtReason").value;
        inpuProdid = getCommonInputObject().value.trim();


        if (inpuProdid == "")
        {
            errorFlag = true;
            msg = mesNoCustomerSN;
            alert(msg);
            getCommonInputObject().focus();
        } 
        else if (reason =="")
        {           
        }   
        else if (reason.length>80)
        {
            errorFlag = true;
            msg = mesReasonOutRange;
            //ITC-1360-680
            alert(msg);
            document.getElementById("txtReason").focus();            
        }

        if (!errorFlag)
        {               
              var station =document.getElementById("<%=stationHF.ClientID %>").value;
              try {                  
                  lstPrintItem = getPrintItemCollection();
                  if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
                  {
                    alert(msgPrintSettingPara);
                    getCommonInputObject().focus();
                    return;
                  }
                    
                  beginWaitingCoverDiv();
                  ShipToCartonLabel.Reprint(inpuProdid, preType, editor, station, customer, pCode, reason, lstPrintItem, onSucceed, onFail);                  
              }
              catch(e1)
              {
                  alertAndCallNext(e1.description);
              }             
        }           
    } catch(e) {
        alertAndCallNext(e.description);     
    }
}

function printPDF() {
    try {
        var WshSheel = new ActiveXObject("wscript.shell");
        var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
        var cmdpdfprint = exe1 + "\\PDFPrint.exe" + " " + exe1 + "\\pdfprintlist.txt 100";
        WshSheel.Run(cmdpdfprint, 2, false);
    }
    catch (e) {
        endWaitingCoverDiv();
        if (e.description == "")
            alert(exe1 + "PDFPrint exec error\n");
        else
            alert(e.description);
    }
}

function printPDFBAT() {
    try {
        var WshSheel = new ActiveXObject("wscript.shell");
        var exe1 = document.getElementById("<%=exepath.ClientID %>").value;
        //var cmdpdfprint = exe1 + "\\PDFPrint.exe" + " " + exe1 + "\\pdfprintlist.txt 100";
        var ClientPDFBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
        if (exe1.charAt(exe1.length - 1) != "\\")
            exe1 = exe1 + "\\";
        if (ClientPDFBatFilePath.charAt(ClientPDFBatFilePath.length - 1) != "\\")
            ClientPDFBatFilePath = ClientPDFBatFilePath + "\\";
        var cmdpdfprint = ClientPDFBatFilePath + "PrintPDF.bat" + " " + exe1 + "pdfprintlist.txt 100";
        WshSheel.Run(cmdpdfprint, 2, false);
    }
    catch (e) {
        endWaitingCoverDiv();
        if (e.description == "")
            alert(ClientPDFBatFilePath + "PrintPDF.bat exec error\n");
        else
            alert(e.description);
    }
}

function onSucceed(result)
{
    try {        
        if(result==null) {
            endWaitingCoverDiv();
            //service方法没有返回
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
            getCommonInputObject().value = "";
        }
        else if((result[0] == SUCCESSRET)) {
            if (result[1][0] == "1") {
                ChangeLabel("出拉美的机器,请更换标签纸", result[1][1]);
            }
            else {
                endWaitingCoverDiv();
                if (result[1][3] == "0") {
                    if (result[1][4] != "") {
                        //972
                        if (result[1][8] == "1" || result[1][8] == "0") {
                
                  
                            recreatePDF(result[1][4], result[1][6], result[1][5], result[1][7], result[1][3]);

                        }
                        else {
                            var pdf = new ActiveXObject("Scripting.FileSystemObject");
                            var pdf_path = document.getElementById("<%=pdfpath.ClientID%>").value;
                            var exe_path = document.getElementById("<%=exepath.ClientID%>").value;
                            var file_path = pdf_path + "\\pdf" + result[1][4];
                            CheckMakeDir(exe_path);
                            if (pdf.FileExists(exe_path + "\\pdfprintlist.txt")) {
                                pdf.DeleteFile(exe_path + "\\pdfprintlist.txt");
                            }
                            var file = pdf.CreateTextFile(exe_path + "\\pdfprintlist.txt", true);
                            file.WriteLine(file_path);
                            file.Close();
                            printPDFBAT(); //printPDF();
                        }
                    }
                    else
                        alert("pdf name is null");
                }
                else {
                    setPrintItemListParam1(result[1][1]);
                    printLabels(result[1][1], false);
                }
                if (result[1][8] != "1") {
                    if (result[1][2] == "1") {
                        preType = 1;
                    }
                    else {
                        preType = 0;
                    }
                    ShowSuccessfulInfo(true, "[" + inpuProdid + "] " + msgSuccess);
                    //ITC-1360-1433

                    getCommonInputObject().value = "";
                    getCommonInputObject().focus();
                }
                
            }
        }
        else {
            endWaitingCoverDiv();
            var content =result[0];
            ShowMessage(content);
            ShowInfo(content);
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
        }        
    } catch(e) {
        alertAndCallNext(e.description);
    }    
}

function onFail(error)
{
   
   try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
       
    } catch(e) {
        alertAndCallNext(e.description);
    }

}

///////////

function recreatePDF(pdf, dn, sn, temp, change) {
    var XmlFileName = pdf.substring(0, pdf.length - 4) + ".xml";
    var PdfFileName = pdf;


    CallEDITSFunc(XmlFileName, dn, sn);
    CallPdfCreateFunc(XmlFileName, temp, PdfFileName);

    var pdf_x = new ActiveXObject("Scripting.FileSystemObject");
    var pdf_path = document.getElementById("<%=pdfpath.ClientID%>").value;
    var exe_path = document.getElementById("<%=exepath.ClientID%>").value;
    var file_path = pdf_path + "\\pdf" + pdf;
    CheckMakeDir(exe_path);
    if (pdf_x.FileExists(exe_path + "\\pdfprintlist.txt")) {
        pdf_x.DeleteFile(exe_path + "\\pdfprintlist.txt");
    }
    var file = pdf_x.CreateTextFile(exe_path + "\\pdfprintlist.txt", true);
    file.WriteLine(file_path);
    file.Close();
    printPDFBAT();//printPDF();

    if (change == "1") {
        preType = 1;
    }
    else {
        preType = 0;
    }


    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    
    ShowSuccessfulInfo(true, "[" + inpuProdid + "] " + msgSuccess);
}


function CallEDITSFunc(XmlFilename, dn, sn) {
    var Paralist = new EDITSFuncParameters();
    var xmlpathfile = "";
    var webEDITSaddr = "";

    var PLEditsXML = document.getElementById("<%=editsXML.ClientID%>").value;
    var PLEditsURL = document.getElementById("<%=editsURL.ClientID%>").value;
    if (PLEditsXML == "" || PLEditsURL == "") {
        alert("EDIT Path error!");
        return false;
    }
    xmlpathfile = PLEditsXML + "XML" + XmlFilename;
    webEDITSaddr = PLEditsURL;

    Paralist.add(1, "FilePH", xmlpathfile);
    Paralist.add(2, "Dn", dn);
    Paralist.add(3, "SerialNum", sn);
    var IsSuccess = invokeEDITSFunc(webEDITSaddr, "BoxShipToShipmentLabel", Paralist);
    return IsSuccess;
}

function CallPdfCreateFunc(XmlFilename, xsl, PdfFilename) {
    var xmlfilename, xslfilename, pdffilename;

    var PLEditsXML = document.getElementById("<%=editsXML.ClientID%>").value;
    var PLEditsURL = document.getElementById("<%=editsURL.ClientID%>").value;
    var PLEditsTemplate = document.getElementById("<%=editsTEMP.ClientID%>").value;
    
    if (PLEditsXML == "" || PLEditsURL == "" || PLEditsTemplate == "") {
        alert("EDIT Path error!");
        return false;
    }

    var PLEditsPDF = document.getElementById("<%=editsPDF.ClientID%>").value;
    //Run Mode Get Path from DB, set Full Path
    xmlfilename = PLEditsXML + "XML" + XmlFilename;
    xslfilename = PLEditsTemplate + xsl;
    pdffilename = PLEditsPDF + "pdf" + PdfFilename;

    var FOPFullFileName = document.getElementById("<%=editsFOP.ClientID%>").value;

    var islocalCreate = false;
    //var islocalCreate = true;
    //================================================================
    //var IsSuccess = CreatePDFfileAsyn(FOPFullFileName, xmlfilename, xslfilename, pdffilename, islocalCreate);
    var PLEditsURL = document.getElementById("<%=editsURL.ClientID%>").value;
    var IsSuccess = CreatePDFfileAsynGenPDF(PLEditsURL, xmlfilename, xslfilename, pdffilename, islocalCreate);
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
///////////


function ChangeLabel(message, key) {
    var url = "./ShipToCartonLabel_ChangeLabel.aspx";
    var paramArray = new Array();
    paramArray[0] = message;
    paramArray[1] = key;
    //ITC-1360-1415
    //window.showModalDialog(url, paramArray, 'dialogWidth:600px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;');
    alert(message);
    ShipToCartonLabel.ServiceReprintChangeLabel(key, onSucceed, onFail);
} 

function setPrintItemListParam1(backPrintItemList) {
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@sn";
    valueCollection[0] = generateArray(inpuProdid);

    setPrintParam(lstPrtItem, "Shipto Label", keyCollection, valueCollection);
}


function ExitPage()
{}


function ResetPage()
{
    ExitPage();
    document.getElementById("txtReason").value = "";
    ShowInfo("");         
    endWaitingCoverDiv();

}

function showPrintSettingDialog()
{
//     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
       showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value,document.getElementById("<%=pCode.ClientID%>").value);
}
</script>
</asp:Content>

