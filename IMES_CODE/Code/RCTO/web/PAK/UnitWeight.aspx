<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Unit Weight
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx –2011/11/25
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx –2011/11/25         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-25   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  本站站号：85
 *                1. Unit 称重；
 *                2. 列印Unit Weight Label；
 *                3. 上传数据至SAP
 *
 * UC Revision：  4078
 */
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UnitWeight.aspx.cs" Inherits="PAK_UnitWeight" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/CommonControl/WeightTypeControl.ascx" TagName="WeightTypeControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>

    <asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceUnitWeight.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="divUnitWeight" style="z-index: 0;">
        <fieldset style="width: 95%" align="center">
            <legend id="lblProductInfo" runat="server" style="color: Blue" class="iMes_label_13pt">
            </legend>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblCustSN" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblCustSN_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                        <asp:Label ID="lblProdId" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblProdId_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblModel" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblModel_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                        <asp:Label ID="lblStdWeight" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblStdWeight_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 50px">
                    <td colspan="4" valign="middle">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <%--<td width="33%" align="center">
                                    <asp:Label ID="lblPAQC" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                                </td>--%>
                                <td width="33%" align="center">
                                    <asp:Label ID="lblVista" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                                </td>
                                <td width="33%" align="center">
                                    <asp:Label ID="lblAdaptor" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset style="width: 95%" align="center">
            <legend id="lblCheckItem" runat="server" style="color: Blue" class="iMes_label_13pt">
            </legend>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblBoxID" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblBoxID_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                    </td>
                    <td width="24%">
                    </td>
                </tr>
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblConfigID" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblConfigID_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                    </td>
                    <td width="24%">
                    </td>
                </tr>
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblAssetTagItem" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblAssetTagItem_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                    </td>
                    <td width="24%">
                    </td>
                </tr>
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblRMN" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblRMN_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <%--UI Update: SVN 10635: Mantis: 0000855: Unit Weight页面 PAQC抽检的字样放在重量的上面并字体放大--%>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblPAQC" runat="server" class="iMes_label_30pt_Red"> </asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 100px">
            <tr>
                <td width="60%" align="left" style="text-align: center">
                    <asp:Label ID="lblUnitWeight" runat="server" class="iMes_label_30pt_Black"> </asp:Label>
                </td>
                <td width="5%">
                </td>
                <td width="35%" align="left" style="text-align: left">
                    <asp:Label ID="lblUnitWeightValue" runat="server" class="iMes_label_30pt_Red_Underline"
                        Font-Bold="True" ForeColor="Red"> </asp:Label>
                </td>
            </tr>
        </table>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 50px">
            <tr valign="middle">
                <td width="16%" align="left">
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td width="50%" align="left">
                    <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                </td>
                <td align="right">
                    <input id="btnPrintSetting" style="height: auto" type="button" runat="server" onclick="showPrintSettingDialog()"
                        class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    &nbsp;
                </td>
                <td align="right">
                    <input id="btnReprint" style="height: auto" type="button" runat="server" onclick="rePrint()"
                        class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    &nbsp;
                </td>
            </tr>
        </table>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td>
                </td>
                <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </tr>
        </table>
    </div>

    <script language="javascript" for="objMSComm" event="OnComm">  

// MSComm1控件每遇到 OnComm 事件就调用 MSComm1_OnComm()函数
objMSComm_OnComm();
  
    </script>

    <script language="javascript" type="text/javascript">
 


var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError =  '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';  
var msgInput='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "msgInputSN") %>';  

var msgNoCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgNoCustSN").ToString() %>';
var msgInputNull = '<%=this.GetLocalResourceObject(Pre + "_msgInputNull").ToString() %>';
var msgInvalidSN =  '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
var msgCustSNErr ='<%=this.GetLocalResourceObject(Pre + "_msgCustSNErr").ToString() %>';
var msgScanRMNFail = '<%=this.GetLocalResourceObject(Pre + "_msgScanRMNFail").ToString() %>';
var msgASTFail = '<%=this.GetLocalResourceObject(Pre + "_msgASTFail").ToString() %>';
var msgScanRMN = '<%=this.GetLocalResourceObject(Pre + "_msgScanRMN").ToString() %>';
var msgScanYAB = '<%=this.GetLocalResourceObject(Pre + "_msgScanYAB").ToString() %>';
var msgScanYB = '<%=this.GetLocalResourceObject(Pre + "_msgScanYB").ToString() %>';
var msgScanYJ = '<%=this.GetLocalResourceObject(Pre + "_msgScanYJ").ToString() %>';
var msgScanYAJ = '<%=this.GetLocalResourceObject(Pre + "_msgScanYAJ").ToString() %>';
var msgScanYBJ = '<%=this.GetLocalResourceObject(Pre + "_msgScanYBJ").ToString() %>';
var msgScanYA = '<%=this.GetLocalResourceObject(Pre + "_msgScanYA").ToString() %>';
var msgScanYABJ = '<%=this.GetLocalResourceObject(Pre + "_msgScanYABJ").ToString() %>';
var msgScanBoxIDorUCC ='<%=this.GetLocalResourceObject(Pre + "_msgScanBoxIDorUCC").ToString() %>';
var msgUnitWeightNull ='<%=this.GetLocalResourceObject(Pre + "_msgUnitWeightNull").ToString() %>';
var msgTolerance ='<%=this.GetLocalResourceObject(Pre + "_msgTolerance").ToString() %>';
var PAQCDisplay = '<%=this.GetLocalResourceObject(Pre + "_lblPAQC").ToString() %>';
var AdaptorDisplay =  '<%=this.GetLocalResourceObject(Pre + "_lblAdaptor").ToString() %>';
var VistaDisplay = '<%=this.GetLocalResourceObject(Pre + "_lblVista").ToString() %>';
var IndiaDisplay = '<%=this.GetLocalResourceObject(Pre + "_lblIndia").ToString() %>';

var msgPDFFileNull1 = '<%=this.GetLocalResourceObject(Pre + "_msgPDFFileNull1").ToString() %>';
var msgPDFFileNull2 = '<%=this.GetLocalResourceObject(Pre + "_msgPDFFileNull2").ToString() %>';
var msgNoPODLabelFile = '<%=this.GetLocalResourceObject(Pre + "_msgNoPODLabelFile").ToString() %>';
var msgPDFPrinterNull ='<%=this.GetLocalResourceObject(Pre + "_msgPDFPrinterNull").ToString() %>';
var msgPAQCDispaly='<%=this.GetLocalResourceObject(Pre + "_msgPAQCDispaly").ToString() %>';
var msgPrintLabelName ='<%=this.GetLocalResourceObject(Pre + "_msgPrintLabelName").ToString() %>';

var inputControl =getCommonInputObject();
var editor = "<%=UserId%>";
var customer = '<%=Customer %>';
var station = '<%=Request["Station"] %>';
var pcode ='<%=Request["PCode"] %>';  
var accountId='<%=Request["AccountId"] %>';
var login ='<%=Request["Login"] %>';
var pdfClinetPath ="<%=PDFClinetPath%>";
var WeightFilePath ="<%=UnitWeightFilePath%>";

var model ="";
var productID = "";
var custSN = "";
var rmnflag = "";
var RMN ="";
var UCCID = "";
var BoxID = "";
var ConfigID = "";
var AssetTagItemValue = "";
var AST =""; // AssetTagItem
var InputType = ""; // B: BoxID: C: ConfigID; A: Asset Tag Item; R: RMN
var partType ="";  // Part Info: YBJ, YB, YAB, YABJ, YJ, YAJ, YA
var printlabeltype1 =""; // print Config Label
var printlabeltype2 =""; // print POD Label
var EditsFISAddr ="";
var lstPrintItem;
var actWeight;
var pdfFlag=false;
//var UnitStandardWeight = -1;
//var Tolerance =0;

var RMNInput =false;
var custInput = false;
var BoxInput = false;
var saveFlag = false;
var ConfigInput =false;
var AssetInput =false;
var hostname = getClientHostName();
var ConfigLabelValueCollection = new Array();

var bufferLength="";
var splitPara="";
var subInputlength="";
var printerpath="";
var displayPQAC=false;
document.body.onload = function() {
    try {

		$.exclusionInOut({ acquire: function() {
			return setCommPara("U",WeightFilePath);
		},
		release: function() {
			var objMSComm =document.getElementById("objMSComm");
			if (objMSComm && objMSComm.PortOpen) {
				objMSComm.PortOpen = false;
			}
			return true;
		}
		});
        //setCommPara("U",WeightFilePath);
        //var objMSComm =document.getElementById("objMSComm");
        //ShowInfo("CommPort,Settings,RThreshold,SThreshold,Handshaking,PortOpen  is: "+objMSComm.CommPort+" , "+objMSComm.Settings +" , "+objMSComm.RThreshold+" , "+objMSComm.SThreshold+" , "+objMSComm.Handshaking+" , "+objMSComm.PortOpen);     
   
        callNextInput();
        

 //   PDFPrint();   
    
    }
    catch (e) {
        alert(e.description);
    }
};



function onFailed(result) {
     endWaitingCoverDiv();
     ResetPage();
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
 }
 
var weightBuffer="";
function objMSComm_OnComm()
{
    var objMSComm = document.getElementById("objMSComm");
   if(objMSComm.CommEvent != 2)
   { 
        //如果不是接收事件,就返回   
		 return;
	}
    else if(objMSComm.CommEvent==2)//如果是接收事件  
    {
        weightBuffer= objMSComm.Input;
        //ShowInfo(weightBuffer);
        var result;
        var idx;
  	    if(weighttype=="")
	    {      
	        idx = weightBuffer.indexOf("\r\n");
            result = weightBuffer.substring(0, idx);
        }
		else
		{
		    idx = weightBuffer.indexOf(weighttype);
		    result = weightBuffer.substring(idx);
		    idx = result.indexOf("\r\n");
            result = result.substring(0, idx);
		}

       var weight = getNumber(result);
       if (weight === false) {
         document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "";
       } else {
         document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = weight;
       } 

    }
    
}  

function getNumber(str) 
{
	var result = false;
	if (typeof (str) == 'string')
	{
		    result = str.replace(/[^\d\.]/g, '');
		    result = isNaN(result) ? false : parseFloat(result);

	}

	return result;
}


function processDataEntry(inputStr)
{
    var inputData = inputStr.trim();
    actWeight=document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText; 
    if (inputData == "") 
    {
        alert(msgInputNull);
        setInputFocus();
        getAvailableData("processDataEntry");
        return;
    }
    //欠缺IMEI处理
    if (actWeight==null || actWeight=="")
    {
        if (Boolean( <%=isTestWeight%> ))
        {
            alert("TEST!");
            actWeight = "100.00";
            document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText="100.00";
        }
    }
    if (actWeight=="")
    {
        alert(msgUnitWeightNull);
        callNextInput();
        return ;
    }
    if (inputData == "7777") {
        ResetPage();
        return;
    }
                 
    if(!custInput)
    {
         inputCustSN(inputData);
    }
//    else if (!saveFlag && custInput && ConfigInput && AssetInput && BoxInput &&RMNinput)
//    {
//        checkData();
//    }
    else if (!saveFlag)
    {
        inputOptional(inputData);
    }
//    else 
//    {
//        Save();    
//    }
       
}

 function inputCustSN(inputData)
 {
     if(inputData.substr(0,2) == "CN" && inputData.length == 10) 
     {
         custSN = inputData;
         inputCustomerSN();
     }
     else if (inputData.substr(0,3) == "SCN" && inputData.length == 11) 
     {
         custSN = inputData.substr(1,10);   //'SCN' 开头的11位数据，后10位为真正的Customer S/N :(CN********)
         inputCustomerSN();
     }
     else
     {
        alert(msgNoCustSN);
     //   ShowInfo(msgNoCustSN);
        callNextInput();
     }
 }

 function inputCustomerSN() 
 {
      beginWaitingCoverDiv();
      var line = "";
      WebServiceUnitWeight.inputCustSN(custSN, actWeight,line, editor, station, customer,onCustSucceed,onCustFailed);        
 }


 function onCustSucceed(result)
{
     endWaitingCoverDiv();
     
     if (result == null) {
        ShowInfo("");
        alert(msgSystemError);
        ShowInfo(msgSystemError);
     }
     else if (result[0] == "CHK020") {
             ShowErrorMessage(result[1]);
     }
     else if (result[0]==SUCCESSRET){
        
        custInput = true;  
        
        document.getElementById("<%=lblCustSN_Display.ClientID %>").innerText = custSN;
        productID = result[1][0]; 
        document.getElementById("<%=lblProdId_Display.ClientID %>").innerText = result[1][0];     
        document.getElementById("<%=lblModel_Display.ClientID %>").innerText = result[1][1];
        model = result[1][1];
        
        //	Standard Weight – IMES_GetData..ModelWeight.UnitWeight（需转换为decimal(10,2)） – 如果没有取到，则标准重量为NULL (这台机器后面就不需要进行标准重量的比较了)
        if (result[1][2] ==-1)
        {
            document.getElementById("<%=lblStdWeight_Display.ClientID %>").innerText = "NULL";
        }
        else
        {
            document.getElementById("<%=lblStdWeight_Display.ClientID %>").innerText = result[1][2];
        }
        
        labelDisplay(result[1][3]);
        labelDisplay(result[1][4]);
        
        AST = result[1][5];
      // ITC-1360-1186 
      // document.getElementById("<%=lblAssetTagItem_Display.ClientID %>").innerText = result[1][5];
      
      // ITC-1360-1243: Asset Tag Item Value，Get后不需要直接显示到UI的Asset Tag Item Value文本框中，而是靠User刷入值检查通过后才显示到这个文本框中。
      // document.getElementById("<%=lblAssetTagItem_Display.ClientID %>").innerText = result[1][8];
         
         document.getElementById("<%=lblAssetTagItem.ClientID %>").innerText = result[1][5] + ":";
          
        rmnflag = result[1][6];
        ConfigID = result[1][7];      
        AssetTagItemValue = result[1][8];
        
        partType = result[1][9];
//        UnitStandardWeight = result[1][2];
//        Tolerance = (parseInt(result[1][10],10))/100;
       
        printlabeltype1 = result[1][11];
        printlabeltype2 = result[1][12];
        
//        if (printlabeltype1=="ConfigLabel")
//        {
//            ConfigLabelValueCollection[0] = generateArray(result[2][0]); 
//            ConfigLabelValueCollection[1] = generateArray(result[2][1]);
//            ConfigLabelValueCollection[2] = generateArray(result[2][2]); 
//            ConfigLabelValueCollection[3] = generateArray(result[2][3]);
//            ConfigLabelValueCollection[4] = generateArray(parseInt(result[2][4],10));
//            ConfigLabelValueCollection[5] = generateArray(parseInt(result[2][5],10)); 
//            ConfigLabelValueCollection[6] = generateArray(parseInt(result[2][6],10));
//        }
        EditsFISAddr = result[1][13];
        //PDF Print:.exe ->.bat  path = bat file path
        //printerpath=result[1][14];
        printerpath= '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"] %>';
        if (printerpath.charAt(printerpath.length - 1) != "\\")
            printerpath = printerpath + "\\";

        DisPlayTip();
        //Mantis 957
        if (partType == "N" && rmnflag == "1")
        {
            checkData();
            return;
        }
        
        if (printlabeltype2 == "PODLabel")
        {
            checkPODLabelPdfExist();
        }
               
     }
     else {
        ShowInfo("");
        var content = result[0];
        alert(content);
        ShowInfo(content);
    }
    
     callNextInput(); 
}

 function onCustFailed(result) {
     endWaitingCoverDiv();
     ExitPage();
     clearInfo();
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
     custInput = false;
     callNextInput();
 }
 
 function checkPODLabelPdfExist()
 {
    var pdfFileName = pdfClinetPath;
    if (pdfClinetPath.slice(-1)!="\\")
    {
        pdfFileName += "\\";
    }
    pdfFileName += model+".Pdf";

    var Fs = new ActiveXObject("Scripting.FileSystemObject");

    if (!Fs.FileExists(pdfFileName))
    {
        ResetPage();
        ShowMessage(msgNoPODLabelFile);
        ShowInfo(msgNoPODLabelFile);
    }
 }
 
 function labelDisplay(labelType) {
     if (labelType == "P") {
            document.getElementById("<%=lblPAQC.ClientID %>").innerText = PAQCDisplay;
            displayPQAC=true;
            return;
        }
     if(labelType == "A") {
            document.getElementById("<%=lblAdaptor.ClientID %>").innerText = AdaptorDisplay;
            return;
        }
     if(labelType == "I") {
            document.getElementById("<%=lblAdaptor.ClientID %>").innerText = IndiaDisplay;
            return;
        }
 }

 function DisPlayTip()
 {
    if (rmnflag == "0")
    {
        ShowInfo(msgScanRMN);
    }
    else if (rmnflag == "1")
    {
        switch (partType)
        {
            case("N"):break;
            case("YAB"):
                     ShowInfo(msgScanYAB + " " + AST);
                     break;
            case("YB"):
                     ShowInfo(msgScanYB);
                     break;    
            case("YJ"):
                     ShowInfo(msgScanYJ);
                     break;
            case("YAJ"):
                     ShowInfo(msgScanYAJ + " " + AST);
                     break;
            case("YBJ"):
                     ShowInfo(msgScanYBJ);
                     break;     
            case("YA"):
                     ShowInfo(msgScanYA + " " + AST);
                     break;
            case("YABJ"):
                     ShowInfo(msgScanYABJ + " " + AST);
                     break;
            default: break;
        }
        
    }
 }

//function inputOptional(inputData)
// {
//    if (inputData.substr(5,1) == "-" && inputData != AssetTagItemValue && !RMNInput) {
//        InputType = "R";
//        checkRMN(inputData);       
//        return;
//    }

//    if (inputData.substr(5,1) == "-" && inputData != AssetTagItemValue && RMNInput) {
//        alert(msgScanRMNFail);
//        ShowInfo(msgScanRMNFail);
//        callNextInput();
//        return;
//    }
//    
//    if ((!BoxInput) && (((partType == "YBJ" && inputData != ConfigID) || (partType == "YB") || (partType == "YAB" && inputData != AssetTagItemValue) || (partType == "YABJ" && inputData != ConfigID && inputData != AssetTagItemValue))))  {
//        if (inputData.length == 12 && inputData.substr(0,2) != "PC") {
//            BoxID = inputData.substr(2, 10);
//        }
//        else if (inputData.length == 10 || inputData.length == 20) {
//            BoxID = inputData;
//        }
//        else {
//            alert(msgScanBoxIDorUCC);
//            ShowInfo(msgScanBoxIDorUCC);
//            callNextInput();
//            return;
//        }
//        InputType = "B";
//        checkBox(inputData);
//        return;
//    }
//    
//    if(ConfigInput)
//    {
//        if (inputData == ConfigID.toUpperCase()) {
//             InputType = "C";
//             document.getElementById("<%=lblConfigID_Display.ClientID %>").innerText = ConfigID;
//             ConfigInput =false;
//             checkData();
//           //  callNextInput();
//             return;
//        }
//        else
//        {
//            alert(msgScanYJ);
//            ShowInfo(msgScanYJ);
//            callNextInput();
//            return;
//        }
//    }
//   
//    if(AssetInput) 
//    {
//        if (inputData.substr(0,5) == AssetTagItemValue.substr(0,5).toUpperCase()) {
//            InputType = "A";
//            AssetInput=false;
//            checkData();
//         //   callNextInput();
//            return;
//        }
//        
//         else {
//            alert(msgASTFail);
//            ShowInfo(msgASTFail);
//            callNextInput();
//            return;
//        }
//    }
//    
//    else {
//        alert(msgCustSNErr);    // Wrong Code!
//        ShowInfo(msgCustSNErr);
//        callNextInput();
//     }
//}

//ITC-1360-1247
function inputOptional(inputData)
 {
    ShowInfo("");
    
    if (inputData.substr(5,1) == "-" && inputData != AssetTagItemValue && !RMNInput) {
        InputType = "R";
        checkRMN(inputData);       
        return;
    }

    if (inputData.substr(5,1) == "-" && inputData != AssetTagItemValue && RMNInput) {
        alert(msgScanRMNFail);
        ShowInfo(msgScanRMNFail);
        callNextInput();
        return;
    }
    // ITC-1360-1451
    // if ((!BoxInput) && (((partType == "YBJ" && inputData != ConfigID) || (partType == "YB") || (partType == "YAB" && inputData != AssetTagItemValue) || (partType == "YABJ" && inputData != ConfigID && inputData != AssetTagItemValue))))  {
     if ((!BoxInput) && ((inputData.length == 12 && inputData.substr(0,2) != "PC") ||(inputData.length == 10 || inputData.length == 20)) && (((partType == "YBJ" && inputData != ConfigID) || (partType == "YB") || (partType == "YAB" && inputData != AssetTagItemValue) || (partType == "YABJ" && inputData != ConfigID && inputData != AssetTagItemValue))))  {
        if (inputData.length == 12 && inputData.substr(0,2) != "PC") {
            BoxID = inputData.substr(2, 10);
        }
        else if (inputData.length == 10 || inputData.length == 20) {
            BoxID = inputData;
        }
        else {
            alert(msgScanBoxIDorUCC);
            ShowInfo(msgScanBoxIDorUCC);
            callNextInput();
            return;
        }
        InputType = "B";
       // checkBox(inputData); //ITC-1360-1267
        checkBox(BoxID);
        return;
    }
    

    if (inputData == ConfigID.toUpperCase()) {
         InputType = "C";
         document.getElementById("<%=lblConfigID_Display.ClientID %>").innerText = ConfigID;
         ConfigInput = true;
         checkData();
         callNextInput();
         return;
    }

    
   if (inputData.substr(0,5) == AssetTagItemValue.substr(0,5).toUpperCase()) {
           if (inputData!=AssetTagItemValue.toUpperCase()){
            alert(msgASTFail);
            ShowInfo(msgASTFail);
            callNextInput();
            return;
        }
            InputType = "A";
            AssetInput=true;
            
            // ITC-1360-1243 : Asset Tag Item Value，Get后不需要直接显示到UI的Asset Tag Item Value文本框中，而是靠User刷入值检查通过后才显示到这个文本框中。
            document.getElementById("<%=lblAssetTagItem_Display.ClientID %>").innerText = AssetTagItemValue;
            
            checkData();
            callNextInput();
            return;
         
    }
  
    else {
        alert(msgCustSNErr);    // Wrong Code!
        ShowInfo(msgCustSNErr);
        callNextInput();
     }
}


function checkRMN(inputData)
{
    WebServiceUnitWeight.checkRMN(productID, inputData,onCheckRMNSucceed,onCheckRMNFailed);        
    RMN =inputData;
}

function onCheckRMNSucceed(result)
{
     endWaitingCoverDiv();
     
     if (result == null) {
        ShowInfo("");
        alert(msgSystemError);
        ShowInfo(msgSystemError);
        RMN = "";
        callNextInput(); 
     }
     else if (result[0] == "PAK033" ||result[0] == "PAK032" ||result[0] == "PAK073")  {
             ShowErrorMessage(result[1]);
             RMN = "";
             callNextInput();
     }
     else if (result[0]==SUCCESSRET){
     // saveFlag =true;  
      RMNInput = true;
      document.getElementById("<%=lblRMN_Display.ClientID %>").innerText = RMN;
      checkData();
     }
     else {
        ShowInfo("");
        var content = result[0];
        alert(content);
        ShowInfo(content);
        RMN = "";
        callNextInput(); 
    }
}

 function onCheckRMNFailed(result) {
     endWaitingCoverDiv();
  //   ResetPage(); 改为不重置页面
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
     RMNInput=false;
	 RMN = "";
     callNextInput();
 }

function checkBox(inputData)
{
    WebServiceUnitWeight.checkBoxIDorUCC(productID, inputData,onCheckBoxSucceed,onCheckBoxFailed);        
 //   BoxID =inputData;
}


function onCheckBoxSucceed(result)
{
     endWaitingCoverDiv();
     
     if (result == null) {
        ShowInfo("");
        alert(msgSystemError);
        ShowInfo(msgSystemError);
        BoxID = "";
        callNextInput();
     }
     else if (result[0] == "PAK035" ||result[0] == "PAK034" )  {
             ShowMessage(result[1]);
             ShowInfo(result[1]);
             BoxID = "";
             callNextInput();
     }
     else if (result[0]==SUCCESSRET){
     // saveFlag =true; 
      BoxInput = true;
      document.getElementById("<%=lblBoxID_Display.ClientID %>").innerText = BoxID;
      checkData();
     }
     else {
        ShowInfo("");
        var content = result[0];
        alert(content);
        ShowInfo(content);
        BoxID = "";
        callNextInput();
    }
 }

 function onCheckBoxFailed(result) {
     endWaitingCoverDiv();
   //  ITC-1360-1250 : 改为不重置页面
  //   ResetPage();
      
     ShowMessage(result.get_message()); 
     ShowInfo(result.get_message());
     BoxInput = false;
	 BoxID = "";
     callNextInput();
 }

function checkData()
{
    if(rmnflag =="0" && document.getElementById("<%=lblRMN_Display.ClientID %>").innerText == "")
    {
    //    alert(msgScanRMN);    //ITC-1360-1231: 取消Alert提示
        ShowInfo(msgScanRMN);
        callNextInput();
        return;
    }
    
    if( partType.search("B")!=-1 && document.getElementById("<%=lblBoxID_Display.ClientID %>").innerText == "" )
    {
      //  alert(msgScanBoxIDorUCC);
        ShowInfo(msgScanBoxIDorUCC);
        callNextInput();
        return;
    }
    
    if( partType.search("J")!=-1 && document.getElementById("<%=lblConfigID_Display.ClientID %>").innerText == "")
    {
     //   ConfigInput = true;
     //   alert(msgScanYJ);
        ShowInfo(msgScanYJ);
        callNextInput();
        return;
    }
        
    if( partType.search("A")!=-1 && AssetTagItemValue == "") 
    {
    //    AssetInput = true;
    //    alert(msgScanYA +" "+ AST);
        ShowInfo(msgScanYA + "" + AST);
        callNextInput();
        return;
    }
    if( partType.search("A")!=-1 && document.getElementById("<%=lblAssetTagItem_Display.ClientID %>").innerText  == "") 
    {
    //    AssetInput = true;
    //    alert(msgScanYA +" "+ AST);
        ShowInfo(msgScanYA + "" + AST);
        callNextInput();
        return;
    }
    if (actWeight == "" || actWeight== null )
    {
        alert(msgUnitWeightNull);
        ShowInfo(msgUnitWeightNull);
        callNextInput();
        return;
    }
    
    // 6.	如果前文取得的Unit Standard Weight 不为空，但Product 重量与Unit Standard Weight 的误差超过2 %，则报告错误：“误差超过2%！ 标准重量为”+ @UnitStandardWeight；报告错误后，Reset UI
//    if(UnitStandardWeight!=-1 && Tolerance > 0.02)
//    {   
//        ShowInfo(msgTolerance + " " + UnitStandardWeight);
//        alert(msgTolerance + " " + UnitStandardWeight);
//        ResetPage();
//    }
//    if(UnitStandardWeight!=-1 )
//    {   
//        if (toleranceAbs < (UnitStandardWeight - parseInt(actWeight,10)))
//        {
//            ShowInfo(msgTolerance + " " + UnitStandardWeight);
//            alert(msgTolerance + " " + UnitStandardWeight);
//            ResetPage();
//        }
//    }
// Activity做: GetModelWeight+CheckWeight

    else 
    {
        Save();
    }
}


function Save()
{
    try{
        lstPrintItem = getPrintItemCollection();
                    
        if (lstPrintItem == null) //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
        {                     
            alert(msgPrintSettingPara);  
           // callNextInput();                          
            ResetPage();
        } 
        else 
        {   
           // beginWaitingCoverDiv();// 保存时统一修改为不启动waiting
            WebServiceUnitWeight.save(productID, lstPrintItem,onSaveSuceed,onSaveFailed);
             
        }
     }
     catch(e) 
     {
         alert(e);
     }
}

function onSaveSuceed(result)
{
     endWaitingCoverDiv();
     
     var printBranch = null;
     
     if (result == null) {
        ShowInfo("");
        alert(msgSystemError);
        ShowInfo(msgSystemError);
        ResetPage();
     }
     else if (result[0]==SUCCESSRET){
         
         saveFlag =true;
         pdfFlag=false;
////==================================Print=======================================         
        
        if (printlabeltype1 == "ConfigLabel"){
            printBranch = "C";
        }
        if (printlabeltype2 == "PODLabel"){
            printBranch= "P";
            if(printlabeltype1 == "ConfigLabel"){
                printBranch ="CP";
            }
        }
        
        
        if (printBranch =="C") 
        {
            // 模板打印："Config Label"
            
            for (i=0;i<result[1].length;i++)
             {
                // ITC-1360-1447： // 按HP Label list-final里定义的写，用小写
                // if (result[1][i].LabelType=="Config Label")    //"Config Label"
                if (result[1][i].LabelType=="config label")    //"Config Label"
                { 
                    setPrintItemListParam(result[1][i]);
                  
                }
             } 
            printLabels(result[1], false);                
        }
        else if(printBranch=="P")
        {
            // PDF打印: "POD Label"
            PDFPrint();            
        }
        else if (printBranch =="CP")
        {
                  
             for (i=0;i<result[1].length;i++)
             {
                // ITC-1360-1447： // 按HP Label list-final里定义的写，用小写
                // if (result[1][i].LabelType=="Config Label")    //"Config Label"
                 if (result[1][i].LabelType=="config label")  
                { 
                    setPrintItemListParam(result[1][i]);
                   // printLabels(result[1][i], false);
                }
                printLabels(result[1], false);     
                
//                if (result[1][i].LabelType=="POD Label")  //"POD Label"
//                {
//                    // PDF打印: "POD Label"
//                    PDFPrint();
//                } 
               PDFPrint();
             }    
          
                           
        }
       

////==================================Print=======================================         
        var SuccessItem ="[" + custSN + "]";
        
        
        ResetPage();
   
      //  ShowSuccessfulInfo(true, msgSuccess); //Use for test, need delete
        
        var msgShow = msgSuccess;
        switch(printBranch)
        {
            case "C":
                    msgShow = msgSuccess +" " + msgPrintLabelName + " " + "config label";
                    break;
            case "P":
                    if(pdfFlag)
                    {
                         msgShow = msgSuccess +" " + msgPrintLabelName + " " + "POD Label";
                    }
                    break;
            case "CP":
                    if(pdfFlag)
                    {
                        msgShow = msgSuccess +" " + msgPrintLabelName + " " +"config label" + ", " + "POD Label";
                    }
                    else  msgShow = msgSuccess +" " + msgPrintLabelName + " " + "config label";
                    break;    
            default:break;    
        }
        
        if (displayPQAC)
        {
            displayPQAC =false;
            ShowSuccessfulInfo(true, SuccessItem + " " + msgShow + "\r\n" + msgPAQCDispaly, true);
        }
        else 
        {
           ShowSuccessfulInfo(true, SuccessItem + " " + msgShow);
        }
     }
     else {
        ShowInfo("");
        var content = result[0];
        alert(content);
        ShowInfo(content);
        ResetPage();
    }
}

function onSaveFailed(result) {
     endWaitingCoverDiv();
     ResetPage();
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
     
 }
 
function PDFPrint(){

      
       var pdfFileName = model+".Pdf";
       var FsFile ="";
       var Fs = new ActiveXObject("Scripting.FileSystemObject");
       if (pdfClinetPath.slice(-1)=="\\") //ITC-1360-1521
       {
         //FsFile = pdfClinetPath + pdfFileName; 
         FsFile=pdfClinetPath + "unitweightprintlist.txt";
       }
       else 
       {
         //FsFile = pdfClinetPath + "\\" + pdfFileName; 
          pdfClinetPath = pdfClinetPath + "\\";
          FsFile = pdfClinetPath + "unitweightprintlist.txt";
       }
       
       if (!Fs.FolderExists(pdfClinetPath)) 
       {
          Fs.CreateFolder(pdfClinetPath);
       }

        if (Fs.FileExists(FsFile))
        {
            Fs.DeleteFile(FsFile);
        }
        
        File= Fs.CreateTextFile(FsFile,true);
        
        var pdfPath = pdfClinetPath + pdfFileName;
        
        File.WriteLine(pdfPath);
        
        File.Close();
        
       var wsh = new ActiveXObject("wscript.shell");
       //var cmdpdfprint = "PDFPrint.exe " + FsFile + " 4000"; 
       
       var cmdpdfprint = "PrintPDF.bat " + FsFile + " 4000"; 
       
   //    wsh.run("cmd /k " + getHomeDisk(pdfClinetPath) + "&copy /Y " + webserverPath +" " + pdfClinetPath + "&exit");
       
       if (!Fs.FileExists(FsFile)) {
           alert(msgPDFFileNull1 +" "+ pdfFileName +" "+ msgPDFFileNull2);          
       }
       else 
       {       
            pdfFlag=true;
            wsh.run("cmd /k " + getHomeDisk(printerpath) + "&cd " + printerpath +"&" + cmdpdfprint + "&exit", 2, false);
       }
       wsh = null;
    
}


// exit page
 function ExitPage()
 {
    if(custSN!="")
    {
        WebServiceUnitWeight.Cancel(custSN);
        sleep(waitTimeForClear);   
    }  
 }
 
 
 //reset page
 function ResetPage()
 {
     ExitPage();
     clearInfo();
     callNextInput();
 }
 
 window.onbeforeunload= function() 
{
   ExitPage();
     
};

 function showPrintSettingDialog()
{
    showPrintSetting(station,pcode);
}

 function rePrint() {
         var url = "ReprintUnitWeight.aspx?Station=" + station + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer +"&AccountId=" + accountId +  "&Login=" + login + "&PDFClinetPath =" + pdfClinetPath;
         window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');

 }
     
function ShowErrorMessage(msg)
{
    ShowMessage(msg);
    ShowInfo(msg);    
}

function clearInfo()
{
     ShowInfo("");
     model="";
     productID = "";
     custSN = "";
     rmnflag = "";
     RMN = "";
     UCCID = "";
     BoxID = "";
     ConfigID = "";
     AssetTagItemValue = "";
     AST = "";
     InputType = ""; 
     partType =""; 
     printlabeltype1 ="";
     printlabeltype2 ="";
     EditsFISAddr = "";
     RMNInput =false;
     custInput = false;
     BoxInput = false;
     saveFlag = false;
     ConfigInput =false;
     AssetInput =false;
//     displayPQAC =false;
     
     actWeight ="";
//     UnitStandardWeight = -1;
//     Tolerance = 0;
     ConfigLabelValueCollection = new Array();
     
    document.getElementById("<%=lblCustSN_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblProdId_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblModel_Display.ClientID %>").innerText = "";
    document.getElementById("<%=lblStdWeight_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblBoxID_Display.ClientID %>").innerText = "";  
    document.getElementById("<%=lblConfigID_Display.ClientID %>").innerText =""; 
    document.getElementById("<%=lblAssetTagItem_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblRMN_Display.ClientID %>").innerText ="";
    
    document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "";
    document.getElementById("<%=lblPAQC.ClientID %>").innerText ="";
    document.getElementById("<%=lblAdaptor.ClientID %>").innerText="";
}

function setPrintItemListParam(backPrintItemList)
{
   //Batch File: config.bat,调用config.bat列印Config Label 
    var lstPrtItem =new Array();
    
    var keyCollection = new Array();
    var valueCollection = new Array();
    
    lstPrtItem[0]= backPrintItemList;
    keyCollection[0] = "@sn";

    valueCollection[0] = generateArray(custSN);
    
//    setPrintParam(lstPrtItem, "Config Label", keyCollection, valueCollection);
    setPrintParam(lstPrtItem, "config label", keyCollection, valueCollection);
}



function callNextInput() 
{
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}



function OnClearInput()
{
    getCommonInputObject().value = "";
   
}
   
function setInputFocus()
{
     getCommonInputObject().focus();

}

    </script>

</asp:Content>
