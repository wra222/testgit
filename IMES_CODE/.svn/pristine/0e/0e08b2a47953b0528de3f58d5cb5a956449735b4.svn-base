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
    CodeFile="CartonWeight.aspx.cs" Inherits="BSam_CartonWeight" Title="Untitled Page" %>

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
            <asp:ServiceReference Path="~/BSam/Service/WebServiceCartonWeight.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="divUnitWeight" style="z-index: 0;">
        <fieldset style="width: 95%" align="center">
            <legend id="lblProductInfo" runat="server" style="color: Blue" class="iMes_label_13pt">
            <asp:Label ID="Label2" CssClass="iMes_label_13pt" runat="server" Text="Carton Information"></asp:Label>
            </legend>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblCartonNo" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblCartonNo_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                        <asp:Label ID="lblModel" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblModel_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblStdWeight" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblStdWeight_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                        &nbsp;</td>
                    <td width="24%">
                        &nbsp;</td>
                </tr>
           
                        </table>
             
        </fieldset>
        <fieldset style="width: 95%" align="center">
            <legend id="Legend1" runat="server" style="color: Blue" class="iMes_label_13pt">
            <asp:Label ID="Label1" CssClass="iMes_label_13pt" runat="server" Text="Custmer SN List"></asp:Label>
            </legend>
          <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 100px">
         <tr>
        <td colspan="4" align="left">
           
              <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="grvProduct" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="150px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" 
                                        HighLightRowPosition="3" style="top: 131px; left: 10px">
                                        <Columns>
                                            <asp:BoundField DataField="Product ID"/>
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
            <tr>
                  <td colspan="3" align="right">
		                    <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="showPrintSettingDialog()" />
		                    <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
                            <input type="hidden" runat="server" id="pCode" />
                    </td>
            </tr>
        </table>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 50px">
            <tr valign="middle">
                <td width="20%" align="left">
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td width="80%" align="left">
                    <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
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
        <asp:UpdatePanel ID="updatePanel1" runat="server"  RenderMode="Inline" UpdateMode="Conditional">
	            <ContentTemplate>
                    <asp:Button ID="btnInputFirstSN" runat="server" Text="Button"   onclick="btnInputFirstSN_Click"  style="display: none"  />
	             
	              <input type="hidden" runat="server" id="hidCustsn" /> 
	              <input type="hidden" runat="server" id="hidLine" /> 
	              <input type="hidden" runat="server" id="index" />
	              <input type="hidden" runat="server" id="hidCartonSN" />
	              <input type="hidden" runat="server" id="hidActWeight" />
	          
	              <input type="hidden" runat="server" id="hidBoxID" />
	              <input type="hidden" runat="server" id="hidModel" />
	              <input type="hidden" runat="server" id="hidStdWeight" />
	                   <input  type="hidden" runat="server" id="hidIsBsam" />
	            
	            </ContentTemplate>   
            </asp:UpdatePanel> 
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
var msgUnitWeightNull ='<%=this.GetLocalResourceObject(Pre + "_msgUnitWeightNull").ToString() %>';
var msgTolerance ='<%=this.GetLocalResourceObject(Pre + "_msgTolerance").ToString() %>';
var msgWrongBoxID='<%=this.GetLocalResourceObject(Pre + "_msgWrongBoxID").ToString() %>';
var msgScanBoxIDorUCC ='<%=this.GetLocalResourceObject(Pre + "_msgScanBoxIDorUCC").ToString() %>';
var msgNOMLabel = '<%=this.GetLocalResourceObject(Pre + "_msgNOMLabel").ToString()%>'; 
var msgCommonPrintHint= '<%=this.GetLocalResourceObject(Pre + "_msgCommonPrintHint").ToString()%>'; 
var inputControl =getCommonInputObject();
var editor = "<%=UserId%>";
var customer = '<%=Customer %>';
var station = '<%=Request["Station"] %>';
var pcode ='<%=Request["PCode"] %>';  
var accountId='<%=Request["AccountId"] %>';
var login ='<%=Request["Login"] %>';
var isBsam="";
 var grvProductClientID = "<%=grvProduct.ClientID%>";
       var pCode  ='<%=Request["PCode"] %>';
            var accountid= '<%=AccountId%>';
var model ="";
var productID = "";
var custSN = "";

var BoxID = "";

var actWeight;

var saveFlag = false;

var snInPizza="";
var hostname = getClientHostName();
var bufferLength="";
var splitPara="";
var subInputlength="";
var printerpath="";
var WeightFilePath ="<%=UnitWeightFilePath%>";
  var havePrintItem = false;
        var lstPrintItem;
 var needHineLabelArr = new Array("POSTEL Label", "Country Label", "CHINA LABEL","TAIWAN LABEL","TabletKC LABEL","TabletRussia LABEL");

 //String.Format function ***************
 String.format = function ()
{
    var s = arguments[0];
    if (s == null) return "";
    for (var i = 0; i < arguments.length - 1; i++)
    {
        var reg = getStringFormatPlaceHolderRegEx(i);
        s = s.replace(reg, (arguments[i + 1] == null ? "" : arguments[i + 1]));
    }
    return cleanStringFormatResult(s);
}
function getStringFormatPlaceHolderRegEx(placeHolderIndex)
{
    return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm')
}
function cleanStringFormatResult(txt)
{
    if (txt == null) return "";
    return txt.replace(getStringFormatPlaceHolderRegEx("\\d+"), "");
}
 //String.Format function ***************

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
        isBsam=document.getElementById("<%=hidIsBsam.ClientID%>").value;
        callNextInput();
        havePrintItem= document.getElementById("<%=btnPrintSet.ClientID %>"); //btnPrintSet

    
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

    var inputData=inputStr;
    if(isBsam=="Y")
    { inputData= inputStr.trim().toUpperCase();}
       lstPrintItem = getPrintItemCollection();
     if (lstPrintItem == null && havePrintItem) {
                alert(msgPrintSettingPara);
                setInputFocus();
                getAvailableData("processDataEntry", true);
                return;
           }
    actWeight=document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText; 
    if (inputData == "") 
    {
        alert(msgInputNull);
        setInputFocus();
        getAvailableData("processDataEntry", true);
        return;
    }

    if (actWeight==null || actWeight=="")
    {
        if (Boolean( <%=isTestWeight%> ))
        {
            alert("TEST!");
            actWeight = "60";
            document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText=actWeight;
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
  if(isBsam=="Y")
  {
  if(custSN=="")
    { 
       if (checkCustomerSNOnProductValid(inputData) == false) {
            alert("Wrong Code!");
              callNextInput();
            return;
        }
        document.getElementById("<%=hidCustsn.ClientID%>").value = inputData;
        document.getElementById("<%=hidActWeight.ClientID%>").value = actWeight;
        beginWaitingCoverDiv();
        document.getElementById("<%=btnInputFirstSN.ClientID%>").click();   
        callNextInput();
        ShowInfo("");
        return;
    }
    else
    {
       if(BoxID!="" && inputData!=BoxID)
       {
            ShowMessage(msgWrongBoxID);
            ShowInfo(msgWrongBoxID);
            callNextInput();
            return;
       }
       else
       {     
              getCommonInputObject().value = "";
              ShowInfo("");
              Save();
       }
    }
  }
  else // if(isBsam=="Y")
  {
    if(custSN=="")
    {
        // if(checkSnInBox(inputStr))
       if (checkCustomerSNOnProductValid(inputData))
         {
             document.getElementById("<%=hidCustsn.ClientID%>").value = inputStr.trim();
            document.getElementById("<%=hidActWeight.ClientID%>").value = actWeight;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnInputFirstSN.ClientID%>").click();   
            callNextInput();
            ShowInfo("");
            return;
         }
         else
         {
             alert("Wrong Code!");
                  callNextInput();
                return;
         }
    }//  if(custSN=="")
    else
    {
         if(snInPizza=="" )
         {
             if(!checkSnInPizza(inputStr.trim()))
             ShowErrorMessage('Wrong sn on carton');
             callNextInput();
            return;
         }
         else
         {
               if(inputStr.trim()!=BoxID)
                 {
                    ShowMessage(msgWrongBoxID);
                    ShowInfo(msgWrongBoxID);
                    callNextInput();
                    return;
                 }
                 else{
                      getCommonInputObject().value = "";
                      ShowInfo("");
                      Save();
                 }
         }
     
       
       
    }
    
  
  }  
    
       
}
  function clearTable() {
      var  initPrdRowsCount = parseInt('<%=initProductTableRowsCount%>', 10) + 1;
                 var grvProductClientID = "<%=grvProduct.ClientID%>";
             try {
                 ClearGvExtTable(grvProductClientID, initPrdRowsCount); //grvDNClientID
                  
                  index = 1;
                 setSrollByIndex(0, false);
               
             }
             catch (e) {
                 alert(e.description);
             }
         }
function checkSnInBox(data)
{
     if( (data.length!=11 || data.substr(10,11)!=" ") || !CheckCustomerSN(data.trim()))
     {
       return false;
     }

     return true;
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
function checkSnInPizza(data)
{
     if( data.length!=10 || !CheckCustomerSN(data.trim()) || !CheckExistData(data) )
     {
       return false;
     }
     snInPizza=data;
     if(BoxID=="")
     {  
          getCommonInputObject().value = "";
          ShowInfo("");
          Save()
     }
     else
     {
       ShowInfo(msgScanBoxIDorUCC,"green");
       callNextInput();
     }
     return true;
}    
         
 function checkCustomerSNOnProductValid(data) {
        var sn = data.trim();
        var ret = false;
        if (sn.length == 10) {
            //if (sn.substr(0, 3) == "CNU") {
			if (CheckCustomerSN(sn)) {
                ret = true;
            
            }
        }
        else if (sn.length == 11) {
        //if (sn.substr(0, 1) == "P" && sn.substr(1, 3) == "CNU") {
		if (sn.substr(0, 1) == "P" && CheckCustomerSN(sn.substr(1,10))) {
            ret = true;
        
           }
        }
        return ret;

    }
function SetInfo() {
             document.getElementById("<%=lblCartonNo_Display.ClientID%>").innerText =  document.getElementById("<%=hidCartonSN.ClientID%>").value;
             document.getElementById("<%=lblModel_Display.ClientID%>").innerText =  document.getElementById("<%=hidModel.ClientID%>").value;
             document.getElementById("<%=lblStdWeight_Display.ClientID%>").innerText =  document.getElementById("<%=hidStdWeight.ClientID%>").value;
             BoxID=  document.getElementById("<%=hidBoxID.ClientID%>").value;
            custSN= document.getElementById("<%=hidCustsn.ClientID%>").value;
            if(isBsam=="N")
            {
                endWaitingCoverDiv();
                ShowInfo('Please scna sn on carton',"green");
                callNextInput();
                return;
            }
           if(BoxID=="")
           {
              Save();
           }
           else
           {
               endWaitingCoverDiv();
                ShowInfo(msgScanBoxIDorUCC,"green");
           }
            
         }



 function inputCustSN(inputData)
 {
     //if(inputData.substr(0,2) == "CN" && inputData.length == 10) 
	 if (inputData.length == 10 && CheckCustomerSN(inputData))
     {
         custSN = inputData;
         inputCustomerSN();
     }
     //else if (inputData.substr(0,3) == "SCN" && inputData.length == 11) 
	 else if (inputData.length == 11 && CheckCustomerSN(inputData.substr(1,10))) 
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






function Save()
{
    try{
    
            beginWaitingCoverDiv();// 保存时统一修改为不启动waiting       var lstPrintItem;
            if(isBsam=="N")
            {
               WebServiceCartonWeight.SaveForTablet(custSN, lstPrintItem,onSaveTabletSuceed,onSaveFailed)
            }
            else
            {
              WebServiceCartonWeight.Save(custSN,onSaveSuceed,onSaveFailed);
            }
          
  
     }
     catch(e) 
     {
         alert(e);
     }
}
function GetLabelHint(labelName)
{
      var msg="";
      for (var i=0;i<needHineLabelArr.length;i++)
      {
         if(labelName.toUpperCase().indexOf(needHineLabelArr[i].toUpperCase())>=0)
         {
          msg=  String.format(msgCommonPrintHint,labelName);
         break;
         }
      }
      return msg;
}
function onSaveTabletSuceed(result)
{
      endWaitingCoverDiv();
      if (result == null) {
        ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
        ResetPage();
     }
     else if (result.success==SUCCESSRET)
     {
        var sn=custSN;
        clearInfo();
        var msgPrint="";
        var tmp="";
        if(result.printItem!=null && havePrintItem)
        {
    
                    for (var i = 0; i < result.printItem.length; i++) {
                        var labelCollection = [];
                       tmp= GetLabelHint(result.printItem[i].LabelType);
                       if(tmp!='')
                       { msgPrint = msgPrint + "," + tmp;}
                       // labelCollection.push(result[1][i]);//result.printItem
                       // setPrintItemListParam(labelCollection, result[1][i].LabelType, customerSN);
                        labelCollection.push(result.printItem[i]);//result.printItem
                        setPrintItemListParam(labelCollection, result.printItem[i].LabelType, sn);
                        printLabels(labelCollection, false);

                    }
        
        }
     
       if(msgPrint!="")
       {
         ShowSuccessfulInfoWithColor(true,msgSuccess+msgPrint, null, "red");
       }
       else
       {
          ShowSuccessfulInfoWithColor(true, msgSuccess, null, "green");
        }
     
    }
     else {
        ShowInfo("");
        var content = result;
        alert(content);
        ShowInfo(content);
        ResetPage();
    }
   clearTable();
   callNextInput();
}

function onSaveSuceed(result)
{
     endWaitingCoverDiv();
      ShowInfo("");
  
     
     if (result == null) {
      
        ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
        ResetPage();
     }
     else if (result==SUCCESSRET)
     {
         
        clearInfo();
        clearTable();
        callNextInput();
        ShowSuccessfulInfo(true,"Success");
 //       ShowInfo("Success","green");
     
      }
     else {
        ShowInfo("");
        var content = result;
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


// exit page
 function ExitPage()
 { 
    if(custSN!="")
    {
        WebServiceCartonWeight.Cancel(custSN);
    
    }  
 }
 
 
 //reset page
 function ResetPage()
 {
     ExitPage();
     clearInfo();
     clearTable();
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

function ShowErrorMessage(msg)
{
    ShowMessage(msg);
    ShowInfo(msg);    
}

function clearInfo()
{
     ShowInfo("");
     custSN = "";
     BoxID = "";
     actWeight ="";
    snInPizza="";
    document.getElementById("<%=lblCartonNo_Display.ClientID%>").innerText =  "";
    document.getElementById("<%=lblModel_Display.ClientID%>").innerText = "";
    document.getElementById("<%=lblStdWeight_Display.ClientID%>").innerText =""
    document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "";
    callNextInput();

}
   function setPrintItemListParam(backPrintItemList,labelType,sn) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(sn);
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        }
        function reprint() {
            //Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var url = "../FA/RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; 
            var paramArray = new Array();
            paramArray[0] = "";
            paramArray[1] = editor;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }

function callNextInput() 
{
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("processDataEntry", true);
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
