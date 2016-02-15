<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/Docking/ProdId Print Page
 * UI:CI-MES12-SPEC-FA-UI ProdId Print For Docking.docx –2012/5/22 
 * UC:CI-MES12-SPEC-FA-UC ProdId Print For Docking.docx –2012/5/22            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-22   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ProdIdSinglePrint.aspx.cs" Inherits="ProdIdSinglePrint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceProdIdPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    
    <%--Cotrol PDLine--%>
    <tr>
	    <td style="width:15%" align="left"><asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="7"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" /></td>	   
    </tr>
    <%--Control Family--%>
    <tr>
	    <td style="width:15%" align="left"><asp:Label ID="lbFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="7"><iMES:CmbFamilyForDocking ID="cmbFamily" runat="server" Width="100" IsPercentage="true"/></td>
    </tr>
    <%--Control Model--%>
    <tr>
	    <td style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="7"><iMES:CmbModelForDocking ID="cmbModel" runat="server" Width="100" IsPercentage="true" /></td>
    </tr>
    <%--Control MO--%>
    <tr>
	    <td style="width:15%" align="left"><asp:Label ID="lbMO" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="7"><iMES:CmbMOForDocking ID="cmbMO" runat="server" Width="100" IsPercentage="true" BelongPage="TravelCard" /></td>
    </tr>
    <%--MO Detail--%>
    <tr>
	    <td></td>
	    <td width="10%" align="left"><asp:Label ID="lbMoQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td width="10%" align="left">&nbsp;</td>
	    <td width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="lbShowMoQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </td>
	    <td style="width:5%">&nbsp;</td>
	    <td width="15%" align="left"><asp:Label ID="lbReQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left"> 
	         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Label ID="lbShowReQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                                           
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	     </td>
	     <td width="8%"  align="left"></td>
    </tr>
    <%--ECR--%>
    <tr style="display: none;">
        <td style="width:15%" align="left"><asp:Label ID="lbECR" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <td colspan="7" align="left"> 
	        <input type="text" id="txtECR" style="width:99%;text-transform:uppercase" class="iMes_textbox_input_Yellow"
                         onkeydown="inputECR()"  runat="server"  />
	   </td> 
    </tr>
    <%--Control Period--%>    
    <tr>
	    <td style="width:15%" align="left"><asp:Label ID="lbPeriod" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" colspan="6">
	     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <input type="radio" NAME="month" id="thisMonth" checked runat="server" /><asp:Label ID="lbThisMonth" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <input type="radio" NAME="month" id="nextMonth" runat="server" /><asp:Label ID="lbNextMonth" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </ContentTemplate>   
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="ServerClick" />
                </Triggers>                                     
         </asp:UpdatePanel>
	    </td>
	    <td></td>
	</tr>
    <%-- Control print qty--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7" align="left"> 
	     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="txtQty" style="width:99%" class="iMes_textbox_input_Yellow"
                                                 onkeypress="input1To100Number(this)" onkeydown="inputQty()"  runat="server"  />
                </ContentTemplate>   
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="ServerClick" />
                </Triggers>                                     
         </asp:UpdatePanel>
	   </TD>    
    </TR>
  
    <TR>
    	   <TD align="right">&nbsp;<input id="btnReprint" type="button"  runat="server" 
                onclick="reprint()" class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="False"/></TD>
	   <td align="right" colspan="6" ><input id="btpPrintSet" type="button"  runat="server" 
               class="iMes_button" onclick="showPrintSettingDialog()" 
               onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'" align="right"/></td>
               
         <TD align="right">&nbsp;<input id="btnPrint" type="button"  runat="server" 
                onclick="print()" class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True"/></TD>                      	   	   	    
    </TR>
    <TR>
	   
	    <td colspan="7">
            <asp:HiddenField ID="hidSKU" runat="server" />
        </td>
	    <TD align="right">&nbsp;<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                </button> 
                <input type="hidden" runat="server" id="station1" />
                <input type="hidden" runat="server" id="editor1" />
                <input type="hidden" runat="server" id="customer1" />
                <input type="hidden" runat="server" id="pCode" /> 
                <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" >                 
                </button> 
            </ContentTemplate>   
        </asp:UpdatePanel> 
	    </TD>
    </TR>
    
   
    </table>
    </center>
     
</div>

  


<script type="text/javascript">

var mesNoSelectFamily = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectFamily").ToString()%>';
var mesNoSelectModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
var mesNoSelMO = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectMO").ToString()%>';
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelPrintTemp = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPrintTemplate").ToString()%>';
var mesNoInputQty = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputQty").ToString()%>';
var msgNoShipDate= '<%=this.GetLocalResourceObject(Pre + "_msgNoShipDate").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var mesQtyIllegal = '<%=this.GetLocalResourceObject(Pre + "_mesQtyIllegal").ToString()%>';
var mesQtyExReQty = '<%=this.GetLocalResourceObject(Pre + "_mesQtyExReQty").ToString()%>';
var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var startSn;
var endSn;
var station;
var editor;
var customer;
var code;



document.body.onload = function() {
    station = document.getElementById("<%=station1.ClientID%>").value;
    editor = document.getElementById("<%=editor1.ClientID%>").value;
    customer = document.getElementById("<%=customer1.ClientID%>").value;
    code = document.getElementById("<%=pCode.ClientID%>").value;
    setPdLineCmbFocus();
}


function reprint() {
    var paramArray = new Array();
    paramArray[0] = station;
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = code;
    
    window.showModalDialog("./TravelCardRePrint.aspx", paramArray, 'dialogWidth:500px;dialogHeight:250px;status:no;help:no;menubar:no;toolbar:no;resize:no');
}

function print()
{   
  try {
        var errorFlag = false;
        var qty = document.getElementById("<%=txtQty.ClientID%>").value;
        var remainQty = document.getElementById("<%=lbShowReQty.ClientID%>").innerHTML;
        var pdline = getPdLineCmbValue();
        var ecr = document.getElementById("<%=txtECR.ClientID%>").value;

        if (!errorFlag) {
            if (getPdLineCmbValue() == "") {
                errorFlag = true;
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
            }else if (getFamilyCmbValue() == "") {
                errorFlag = true;
                alert(mesNoSelectFamily);
                setFamilyCmbFocus();
            } else if (getModelCmbValue() == "") {
                errorFlag = true;
                alert(mesNoSelectModel);
                setModelCmbFocus();
            } else if (getMOCmbValue() == "") {
                errorFlag = true;
                alert(mesNoSelMO);
                setMOCmbFocus();
            } else if (qty == "") {
                errorFlag = true;
                alert(mesNoInputQty);
                document.getElementById("<%=txtQty.ClientID%>").focus();
            } else if (checkNumber(qty)) {
                errorFlag = true;
                alert(mesQtyIllegal);
                document.getElementById("<%=txtQty.ClientID%>").focus();
            } else if ((remainQty != "") && (parseInt(qty) > parseInt(remainQty))) {
                errorFlag = true;
                alert(mesQtyExReQty);
                document.getElementById("<%=txtQty.ClientID%>").focus();
            }
        }
        if (!errorFlag)
        {
            var IsNextMonth = true;
            if (document.getElementById("<%=thisMonth.ClientID%>").checked) {
                IsNextMonth = false;
            } else {
                IsNextMonth = true;
            }
              
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara); //請先檢查設置列印頁面參數
            } else {
                beginWaitingCoverDiv();
                WebServiceProdIdPrint.PrintProdIdForDocking(getPdLineCmbValue(), getModelCmbValue(), getMOCmbValue(),
                                        qty, IsNextMonth, "<%=userId%>", station, "<%=customer%>", ecr,
                                        lstPrintItem, onSucceed, onFail);
            }           
        }        
    } catch(e) {
        alert(e);  
    }
}

function setPrintItemListParam1(backPrintItemList,beginProid,endProid)
{
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@BegNo";
    keyCollection[1] = "@EndNo";
    
    valueCollection[0] = generateArray(beginProid);
    valueCollection[1] = generateArray(endProid);
    
    setPrintParam(lstPrtItem, "DK_Travel_Card_1", keyCollection, valueCollection);
}


function onSucceed(result)
{
    try {
        if(result==null)
        {            
            endWaitingCoverDiv();
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);          
        }
        else if((result.length == 6)&&(result[0]==SUCCESSRET)) {
            ShowInfo("");
             endWaitingCoverDiv();
             startSn = result[3][0];
             endSn = result[3][result[3].length - 1];

             document.getElementById("<%=btnHidden.ClientID%>").click();

             var printNumber;
             var beginProid;
             var endProid;
             for (i = 0; i < result[3].length; i++) 
             {
                 //Modify 单张打印
                     beginProid = result[3][i];
                     endProid ='';
                     setPrintItemListParam1(result[1], beginProid, endProid);
                     printLabels(result[1], false);
                 //---------------------------------------------------------
                 //printNumber = i % 2;
                 //if (printNumber == 0 & i == result[3].length - 1)
                 //{
                 //    beginProid = result[3][i];
                 //    endProid ='';
                 //    setPrintItemListParam1(result[1], beginProid, endProid);
                 //    printLabels(result[1], false);
                 //    break;
                 //}
                 //else if (printNumber == 0) 
                 //{
                 //    beginProid = result[3][i];
                 //}
                 //else
                 //{
                 //    endProid = result[3][i];
                 //    setPrintItemListParam1(result[1], beginProid, endProid);
                 //    printLabels(result[1], false);                             
                 //}            
             }
        }
        else 
        {
            endWaitingCoverDiv();
            ShowInfo("");
            var content =result[0];
            ShowMessage(content);
            ShowInfo(content);
         } 
    } 
    catch(e)
    {
        alert(e.description);
        endWaitingCoverDiv();
    }    
}

function onFail(error)
{
    try
    {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
     
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function showPrintSettingDialog() {    
    showPrintSetting(document.getElementById("<%=station1.ClientID%>").value,document.getElementById("<%=pCode.ClientID%>").value);
}

function checkNumber(value)
{
	var errorFlag = false;
	try 
    {
	    var pattern = /^([1-9][0-9]?|100)$/;

    	
	    if (pattern.test(value)) 
	    {
		    errorFlag = false;
	    }
	    else 
	    {
		    errorFlag = true;
	    } 
	    return errorFlag;
	}
    catch (e)
    {
	    alert(e.description);
    }   
}

function checkNumberAndEnglishChar(value)
{
	var errorFlag = false;
	try 
    {
	   var pattern = /^[0-9a-zA-Z]*$/;

    	
	    if (pattern.test(value)) 
	    {
		    errorFlag = false;
	    }
	    else 
	    {
		    errorFlag = true;
	    } 
	    return errorFlag;
	}
    catch (e)
    {
	    alert(e.description);
    }

}

document.onkeydown = check;
function check() {
    //keyCode是event事件的属性,对应键盘上的按键,回车键是13,tab键是9,其它的如果不知道 ,查keyCode大全
    if(event.keyCode == 13)
        event.keyCode = 9;

}

function inputECR() {
    if (event.keyCode == 13)
        event.keyCode = 9;
}

function inputQty() {
    if (event.keyCode == 13)
        event.keyCode = 9;
}




function ExitPage()
{   
}

function ResetPage()
{
    ExitPage();
    document.getElementById("<%=btnReset.ClientID%>").click();  
}

function test()
{
    ShowSuccessfulInfo(true, "ProductID: (" + startSn + ") - (" + endSn + ") is printed!");
}

function test1()
{
    ShowSuccessfulInfo(true);
}

</script>

</asp:Content>


