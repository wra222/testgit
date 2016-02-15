
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ReprintCartonLabel_BIRCH.aspx.cs" Inherits="ReprintCartonLabel_BIRCH" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
           <Services>  
		     <asp:ServiceReference Path="~/PAK/Service/WebServiceCombineCartonInDN_BIRCH.asmx" />
			 <asp:ServiceReference Path="~/PAK/Service/WebServicePDPALabel02.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
   
    <tr>
	    <td style="width:25%" align="left"><asp:Label ID="lblCustomSN" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
	    <td  style="width:60%" align="left">
	  
        <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
                                         
       </td>
       <td   align="right">
              <asp:Label ID="Label3" Width="20%" runat="server" CssClass="iMes_label_13pt" Text="Label:"></asp:Label>
                        <asp:DropDownList ID="drpLabel" runat="server">
                            <asp:ListItem Value="ALL" Selected="True">ALL</asp:ListItem>
                            <asp:ListItem Value="Carton">Carton Label</asp:ListItem>
                            <asp:ListItem Value="Shipto">Shipto Label</asp:ListItem>
                        </asp:DropDownList>
       </td>
    </tr>
    
    <tr>
	    <td style="width:25%"  align="left"><asp:Label ID="lbReason" runat="server" 
                CssClass="iMes_DataEntryLabel" ForeColor="Blue"></asp:Label>
        </td>
	    <td align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <textarea id="txtReason" style="overflow:auto; width: 99%; height: 100px"></textarea>                
                </textarea>
                </ContentTemplate>                                     
         </asp:UpdatePanel>
         </td>  
         <td align="right">
         <input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="return btnPrintSetting_onclick()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/>
	                <br />
	                 <br />
	                     <input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="input()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        />
         </td>
    </tr>
     <tr>
        <td style="width:24%" align="left"></td>
        <td align="right" colspan="2">
	     </td>
     
     </tr>
    <tr>
        <td style="width:24%" align="left"></td>
        <td align="right" colspan="2">
       
        
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
var msgNoTemp = '<%=this.GetLocalResourceObject(Pre + "_msgNoTemp").ToString() %>';
var pCode;
var SUCCESSRET ="SUCCESSRET";
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var station;
var printItemlist;
var inpuProdid;
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var imgAddr = "";
var webEDITSaddr = "";
var xmlFilePath = "";
var pdfFilePath = "";
var tmpFilePath = "";
var fopFilePath = ""
var templateName = "";
var line = "";
var pdfPrintPath;
var pdfFullPath;
var key;
var label;
//var pdfFileName = "";
var templateName = "";
var cartonSn;
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgCreatePDF").ToString() %>';
var msgErrCreatePDF = '<%=this.GetLocalResourceObject(Pre + "_msgErrCreatePDF").ToString() %>';

window.onload = function() {
    station = '<%=Request["Station"] %>';
    pCode = '<%=Request["PCode"] %>';
    inpuProdid = "";
    ShowInfo("");
    callNextInput();
    var nameCollection = new Array();
    nameCollection.push("PLEditsImage");
    nameCollection.push("PLEditsURL");
    nameCollection.push("PLEditsXML");
    nameCollection.push("PLEditsPDF");
    nameCollection.push("PLEditsTemplate");
    nameCollection.push("FOPFullFileName");
    nameCollection.push("PDFPrintPath");
    GetLabel();
    WebServicePDPALabel02.GetSysSettingList(nameCollection, onGetSetting, onGetSettingFailed);
}
function GetLabel() {

        var id = "<%=drpLabel.ClientID %>";
        label = document.getElementById(id)[document.getElementById(id).selectedIndex].value;
   
  
}
function btnPrintSetting_onclick() {
    showPrintSetting(station, pCode);
}
function callNextInput() {
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("ProcessInput");
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
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
}


function alertAndCallNext(message) {
    endWaitingCoverDiv();
    alert(message);
    getAvailableData("ProcessInput");
}
function ProcessInput(inputData) {
    try {
        input();
        getAvailableData("ProcessInput");
    } catch (e) {
        alertAndCallNext(e.description);
    }
}


function input() {

    //   var errorFlag = false;
    var msg = "";
    var reason = document.getElementById("txtReason").value;
    inpuProdid = getCommonInputObject().value.trim();
    printItemlist = getPrintItemCollection();
    if (inpuProdid == "") {
        alert(mesNoCustomerSN);
        callNextInput();
        return;
    } 
    if (reason.length > 80) {
        ShowMessage(mesReasonOutRange);
        ShowInfo(mesReasonOutRange);
        callNextInput();
        return; ;
    }
    if (printItemlist == null || printItemlist == "") {
        alert(msgPrintSettingPara);
        callNextInput();
        return;
    }
    beginWaitingCoverDiv();
    // public ArrayList ReprintCartonLabel(string sn, string editor, string station, string customer, string reason, IList<PrintItem> printItems)
    WebServiceCombineCartonInDN_BIRCH.ReprintCartonLabel(inpuProdid, editor, station, customer,reason, printItemlist, onSucceed, onFail)
}
function PrintCarton(printItem)
{
        var printlist = new Array();
           setPrintItemListParam(printItem, key);
           var cartonLabel;
           for (var p in printItem) {
               if (printItem[p].LabelType == "Tablet Carton Label") {
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


function onSucceed(result)
{
   ShowInfo("");
   endWaitingCoverDiv();
   callNextInput();
    var pdf_Result;
    var pdfErr = "";
    var flag = "";
    var dn = "";
    GetLabel();
        if (result == null) {
            var content = "System Error";
            ShowMessage(content);
            ShowInfo(content);
        }
        else if ((result.length ==8) && (result[0] == SUCCESSRET)) {
        //SUCCESSRET,custsn,dn,CartonSN,templatename,line,flag,PrintItem
        key = result[1];
        dn = result[2];
        cartonSn = result[3];
           templateName = result[4];
           line = result[5];
           flag = result[6];
           if (label == "ALL" || label == "Carton") 
           {
               PrintCarton(result[7]);
           }
           if (label == "ALL" || label == "Shipto") {
                if (templateName == "" && flag == "N") {
                    ShowMessage(msgNoTemp);
                    ShowInfo(msgNoTemp);
                    callNextInput();
                    return;
                }
                if (templateName != "") {
                    // ********* Edits print ********* //
                       var pdf_Result = StartCreatePDF(dn);
                     
                        if (!pdf_Result[0]) {
                            ShowMessage(pdf_Result[1]);
                            ShowInfo(pdf_Result[1]);
                            callNextInput();
                            return;
                        }
                    // ********* Edits print ********* //
                }
                else {
                    PrintShipto(result[7]);
                }

            }
            callNextInput();
            ShowInfo(msgSuccess, "green");
          
        } //  else if ((result.length ==5) && (result[0] == SUCCESSRET))
         else {
            ShowMessage(result[0]);
        }
    
  }


function onFail(error)
{
   
   try {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    } catch(e) {
        alertAndCallNext(e.description);
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
            }

        }
        else {
            r_Result[0] = false;
            r_Result[1] = msgErrCreatePDF + "\r\n" + result2[1] + " Err1" +"file path :"+ filepath +";dn:"+dn +";key:"+key;
            return r_Result;
            //   ShowMessage(msgErrCreatePDF + "\r\n" + result2[1]);
        }
    }
    else {
        r_Result[0] = false;
        r_Result[1] = msgErrCreatePDF + "\r\n" + result[1] + " Err2" + "; file path :" + filepath + ";dn:" + dn + ";key:" + key;
        return r_Result;
        // ShowMessage(msgErrCreatePDF + "\r\n" + result[1]);
    }
    return r_Result;
}
function CallPdfCreateFunc(dn) {
   
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
    keyCollection[0] = "@CUSTSN";
    valueCollection[0] = generateArray(custSN);
    setPrintParam(lstPrtItem, "Tablet Carton Label", keyCollection, valueCollection);
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
</asp:Content>

