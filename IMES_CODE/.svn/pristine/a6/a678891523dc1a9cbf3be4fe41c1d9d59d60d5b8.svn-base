<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/FA/TravelCardPrint Page
 * UI:CI-MES12-SPEC-FA-UI Travel Card Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC Travel Card Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-15   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* Exception控件
* UC/UI变更
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="TravelCardPrint.aspx.cs" Inherits="FA_TravelCardPrint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceTravelCard.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <TABLE border="0" width="95%">
    
    <%--Cotrol PDLine--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" /></TD>	   
    </TR>
    <%--Control Family--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7"><iMES:CmbFamily ID="cmbFamily" runat="server" Width="100" IsPercentage="true"/></TD>
    </TR>
    <%--Control Model--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <td colspan="7">
	        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
	                <input type="text" id="txtModel"  class="iMes_textbox_input_Yellow"
                        maxlength="50" onkeypress="inputNumberAndEnglishChar(this)"            
                        runat="server"  onkeydown="CheckModel()" style="width: 30%" />
                </ContentTemplate>                                        
            </asp:UpdatePanel>
	    </td>
    </TR>
    <%--Control MO PO Radio--%>
    <%--Control MO--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbMO" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7"><iMES:CmbMO ID="cmbMO" runat="server" Width="100" IsPercentage="true" BelongPage="TravelCard" /></TD>
    </TR>
    <%--MO Detail--%>
    <TR>
	    <TD></TD>
	    <TD width="10%" align="left"><asp:Label ID="lbMoQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD width="10%" align="left">&nbsp;</TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="lbShowMoQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>
	    <TD style="width:5%">&nbsp;</TD>
	    <TD width="15%" align="left"><asp:Label ID="lbReQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD align="left"> 
	         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Label ID="lbShowReQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>                                           
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	     </TD>
	     <TD width="8%"  align="left"></TD>
    </TR>
    <%--Control ShipDate--%>
    <tr> 
        <td align="left"><asp:Label ID="lblShipdate" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td colspan="7" align="left"><input type="text" id="dCalShipdate" style="width:90px;" readonly="readonly" />
            <input id="btnCal" type="button" value=".." 
                onclick="showCalendar('dCalShipdate')" style="width: 17px" class="iMes_button"  />
        </td>
    </tr>


    <%--Control Period--%>    
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbPeriod" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD align="left" colspan="6">
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
	    </TD>
	    <TD></TD>
	</TR>
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
	<%--Control Product Id range--%>   
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbProductId" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD width="10%" align="left"><asp:Label ID="lbStart" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD width="10%" align="left">&nbsp;</TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label12" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>
	    <TD style="width:5%">&nbsp;</TD>
	    <TD width="15%" align="left"><asp:Label ID="lbEnd" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD align="left"> 
	         <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Label ID="Label14" runat="server" CssClass="iMes_label_11pt"></asp:Label>                                           
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	     </TD>
	     <TD width="8%"  align="left"></TD>
    </TR>

    <%--Control Remark--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7" align="left"> 
	     <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="Text2" style="width:99%" class="iMes_textbox_input_Yellow"
                         onkeydown="inputRemark()"  runat="server"  />
                </ContentTemplate>   
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="ServerClick" />
                </Triggers>                                     
         </asp:UpdatePanel>
	   </TD>    
    </TR>
    <%--Control Exception--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbException" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7"><iMES:CmbException ID="cmbexception" runat="server" Width="100" IsPercentage="true"/></TD>
    </TR>
    <%--Control Bom Remark--%>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbBomRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7" align="left"> 
	     <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="Text1" style="width:99%" class="iMes_textbox_input_Yellow"
                    onkeydown="inputBomRemark()"  runat="server"  />
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
                <button id="btnCheck" runat="server" onserverclick="btnCheck_Click" style="display: none" ></button> 
            </ContentTemplate>   
        </asp:UpdatePanel> 
	    </TD>
    </TR>
    
   
    </TABLE>
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
var toDate = document.getElementById('dCalShipdate');


document.body.onload = function() {
    //ITC-1360-1253
    toDate.value = "<%=today%>";
    station = document.getElementById("<%=station1.ClientID%>").value;
    editor = document.getElementById("<%=editor1.ClientID%>").value;
    customer = document.getElementById("<%=customer1.ClientID%>").value;
    code = document.getElementById("<%=pCode.ClientID%>").value;
    setPdLineCmbFocus();
    document.getElementById("<%=Text1.ClientID%>").disabled = true;
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
        var deliverydate = document.getElementById('dCalShipdate').value;
        var bomremark = document.getElementById("<%=Text1.ClientID%>").value;
        var remark = document.getElementById("<%=Text2.ClientID%>").value;
        var exception = getExceptionCmbValue();
        var model = document.getElementById("<%=txtModel.ClientID%>").value;

        ShowInfo("");
        if (!errorFlag) {
            if (getPdLineCmbValue() == "") {
                errorFlag = true;
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
            }else if (getFamilyCmbValue() == "") {
                errorFlag = true;
                alert(mesNoSelectFamily);
                setFamilyCmbFocus();
            } else if (model == "") {
                errorFlag = true;
                alert(mesNoSelectModel);
                document.getElementById("<%=txtModel.ClientID%>").focus();
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
            } else if (deliverydate=="") {
                errorFlag = true;
                alert(msgNoShipDate);  
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
                WebServiceTravelCard.PrintTCWithProductIDForBN(getPdLineCmbValue(), model, getMOCmbValue(),
                                        qty, IsNextMonth, "<%=userId%>", station, "<%=customer%>",
                                        lstPrintItem, deliverydate, bomremark, remark, exception, onSucceed, onFail);
            }           
        }        
    } catch(e) {
        alert(e);  
    }
}

function setPrintItemListParam1(backPrintItemList,pdline,shipdate,qty,model,mo,beginProid,endProid) //Modify By Benson at 2011/03/30
{
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();

    keyCollection[0] = "@productID1";
    keyCollection[1] = "@productID2";
    
    valueCollection[0] = generateArray(beginProid);
    valueCollection[1] = generateArray(endProid);
    
    setPrintParam(lstPrtItem, "Travel Card", keyCollection, valueCollection);
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
             document.getElementById("<%=Label12.ClientID%>").innerHTML = startSn;
             document.getElementById("<%=Label14.ClientID%>").innerHTML = endSn;
             document.getElementById("<%=btnHidden.ClientID%>").click();
               
             var _shipdate=document.getElementById('dCalShipdate').value;
             var _qty=document.getElementById("<%=txtQty.ClientID%>").value;
             var _line= getPdLineCmbValue();
             var _mo = getMOCmbText();
             var printNumber;
             var beginProid
             var endProid
             for (i = 0; i < result[3].length; i++) 
             {
                 printNumber = i % 2;
                 if (printNumber == 0 & i == result[3].length - 1)
                 {
                     beginProid = result[3][i];
                     endProid ='';
                     setPrintItemListParam1(result[1], _line, _shipdate, _qty, result[2], _mo, beginProid, endProid);
                     printLabels(result[1], false);
                     break;
                 }
                 else if (printNumber == 0) 
                 {
                     beginProid = result[3][i];
                 }
                 else
                 {
                     endProid = result[3][i];
                     setPrintItemListParam1(result[1], _line, _shipdate, _qty, result[2], _mo, beginProid, endProid);
                     printLabels(result[1], false);                             
                 }            
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

function inputBomRemark() {
    if (event.keyCode == 13)
        event.keyCode = 9;
}

function inputQty() {
    if (event.keyCode == 13)
        event.keyCode = 9;
}

function inputRemark() {
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



function CheckModel() {
    if (event.keyCode == 9 || event.keyCode == 13) {
        ShowInfo("");
        document.getElementById("<%=btnCheck.ClientID%>").click();
    }
}


function Modelclear() {
    document.getElementById("<%=txtModel.ClientID%>").value = "";
}

function ModelFocus() {
    document.getElementById("<%=txtModel.ClientID%>").focus();
}

function alertNoQueryCondAndFocus() {
    alert(mesNoSelectFamily);
    setFamilyCmbFocus();
}

function alertNoInputModel() {
    alert(mesNoSelectModel);
    ModelFocus();
}

</script>

</asp:Content>


