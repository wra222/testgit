<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012-01-06  zhu lei              Create 
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="MBLabelPrint.aspx.cs" Inherits="SA_MBLabelPrint"  Title="无标题页"%>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceMBLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
       
    <TABLE border="0" width="95%" >
    <TR>
	    <TD style="width:15%" align="left" ><asp:Label ID="lbMBCode" runat="server"  CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6"><iMES:CmbMBCode ID="cmbMBCode" runat="server" Width="100" IsPercentage="true" /></TD>
	   
    </TR>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6"><iMES:Cmb111Level ID="cmbModel" runat="server" Width="100" IsPercentage="true"/></TD>
    </TR>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbMO" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="6"><iMES:CmbMO ID="cmbMO" runat="server" Width="100" IsPercentage="true"/></TD>
    </TR>
    <TR>
	    <TD></TD>
	    <TD width="13%" align="left">
	        <asp:Label ID="lbMoQty" runat="server" CssClass="iMes_label_13pt" ></asp:Label>                      
        </TD>
	    <TD width="10%" align="left">
	      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
	                <asp:Label ID="lbShowMoQty" runat="server" CssClass="iMes_label_11pt"></asp:Label>
	            </ContentTemplate>                                        
           </asp:UpdatePanel>
        </TD>
	    <TD width="5%" align="left">&nbsp;</TD>
	    <TD width="15%" align="left"> 
	             <asp:Label ID="lbReQty" runat="server" CssClass="iMes_label_13pt" ForeColor="Red" ></asp:Label>
        </TD>
	    <TD  align="left">
	        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
	            <asp:Label ID="lbShowReQty" runat="server" CssClass="iMes_label_11pt" ForeColor="Red" ></asp:Label>
	            </ContentTemplate>                                        
           </asp:UpdatePanel>
        </TD>
	    <TD width="8%"  align="left"></TD>
    </TR>
    
     <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbFactor" runat="server" CssClass="iMes_label_13pt" ></asp:Label></TD>
	    <TD colspan="6">
	        <asp:DropDownList ID="drpFactor" runat="server" Width="100%" IsPercentage="true">
	            <asp:ListItem Selected Value="">Local</asp:ListItem>
	            <asp:ListItem Value="G">G</asp:ListItem>
	        </asp:DropDownList>
	    </TD>
	   
    </TR>
    
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbPdline" runat="server" CssClass="iMes_label_13pt" ></asp:Label></TD>
	    <TD colspan="6"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" Stage="SA"/></TD>
	   
    </TR>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbPeriod" runat="server" CssClass="iMes_label_13pt" ></asp:Label></TD>
	    <TD align="left" colspan="5">
	     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <input type="radio" name="month" id="thisMonth" checked runat="server" /><asp:Label ID="lbThisMonth" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <input type="radio" name="month" id="nextMonth" runat="server" /><asp:Label ID="lbNextMonth" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                </ContentTemplate>   
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="ServerClick" />
                </Triggers>                                     
         </asp:UpdatePanel>
	    </TD>
	    <TD ></TD>
    </TR>
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lbQty" runat="server"  CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="4" align="left">
	     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="txtQty" style="width:100%" class="iMes_textbox_input_Yellow"
                                                 onkeypress="input1To100Number(this)"  runat="server"  />
                </ContentTemplate>   
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="ServerClick" />
                </Triggers>                                     
         </asp:UpdatePanel>
	   </TD>
	    
	    <TD width="10%"  align="left"><asp:Label ID="lbQtyTip" runat="server" CssClass="iMes_label_11pt_Pink" ></asp:Label></TD>
        
        <TD align="right">
	        <input id="btpPrintSet" type="button"  runat="server" class="iMes_button" onclick="showPrintSettingDialog()" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
	    </TD>
    </TR>

    
    <TR>
        <td></td>
	    <TD colspan="4">
            <input type="radio" name="label" id="largeLabel" checked runat="server" /><asp:Label ID="lblLargeLabel" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="radio" name="label" id="smallLabel" runat="server" /><asp:Label ID="lblSmallLabel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
	    </TD>
	    <TD  align="right">
            <input id="btnPrint" type="button"  runat="server" onclick="print()" 
                class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" />
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                <ContentTemplate>
                    <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                    </button> 
                    <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" >
                    </button> 
                    <input type="hidden" runat="server" id="station" />
                    <input type="hidden" runat="server" id="pCode" />
                </ContentTemplate>   
            </asp:UpdatePanel> 
         </TD>
         
         <TD  align="right">
            <input id="btnReprint" type="button"  runat="server" onclick="reprint()" 
                class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" />
         </TD>
    </TR>
    </TABLE>
    </center>
     
       
</div>

  

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<script type="text/javascript">

var mesNoSelMBCode = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectMBCode").ToString()%>';
var mesNoSelModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
var mesNoSelMO = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectMO").ToString()%>';
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelPrintTemp = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPrintTemplate").ToString()%>';
var mesNoInputQty = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputQty").ToString()%>';
//var mesNoInputDateCode = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputDateCode").ToString()%>';
var mesQtyOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesQtyOutRange").ToString()%>';
var mesQtyExReQty = '<%=this.GetLocalResourceObject(Pre + "_mesQtyExReQty").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var mesQtyIllegal = '<%=this.GetLocalResourceObject(Pre + "_mesQtyIllegal").ToString()%>';
var mesIsPrinted = '<%=this.GetLocalResourceObject(Pre + "_mesIsPrinted").ToString()%>';
var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var remainQty;
var qty;
var mbSno = "";
var labelType = "";
var AccountId = '<%=Request["AccountId"] %>';
var Login = '<%=Request["Login"] %>';

document.body.onload = function ()
{
    editor = "<%=userId%>";
    customer = "<%=customer%>";
    stationId = '<%=Request["Station"] %>';
    pCode = '<%=Request["PCode"] %>';
}

function print()
{
    try {
        var errorFlag = false;
         qty =  document.getElementById("<%=txtQty.ClientID%>").value;
         remainQty = document.getElementById("<%=lbShowReQty.ClientID%>").innerHTML;
        
        //var station = document.getElementById("<%=station.ClientID%>").value
    
        //在打印之前检查页面输入是否合法
        if (getMBCodeCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelMBCode);
            setMBCodeCmbFocus();
        } else if (get111LevelCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelModel);
            set111LevelCmbFocus();
        } else if (getMOCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelMO);
            setMOCmbFocus();
        } else if (getPdLineCmbValue() == "") {
            errorFlag = true;
            alert(mesNoSelPdLine);
            setPdLineCmbFocus();
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
      
        if (!errorFlag) {
            //调用web service提供的打印接口
            //打印方法
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                ResetPage();
                return;
            }

            var month;
            if (document.getElementById("<%=thisMonth.ClientID%>").checked) {
                month = "false";
            } else {
                month = "true";
            }

            var dateCode = " "
            var factor = document.getElementById("<%=drpFactor.ClientID %>").value;

            mbSno = SubStringSN(getMBCodeCmbValue(), "MB");

            if (document.getElementById("<%=largeLabel.ClientID%>").checked) {
                labelType = "MB Label";
            } else {
                labelType = "MB Label-1";
            }

            beginWaitingCoverDiv();
            WebServiceMBLabelPrint.print(getPdLineCmbValue(), month, mbSno, getMOCmbValue(), qty, dateCode, editor, stationId, customer, get111LevelCmbValue(), factor, lstPrintItem, labelType, onSucceed, onFail);
        }
    } catch(e) {
        alert(e);
       
    }
}

function onSucceed(result) {
    try {
        endWaitingCoverDiv();
        if (result == null) {
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
        }
        else if (result[0] == SUCCESSRET) // 0:Success,1:printitem lst 2 : pdline 3:_111   4:snlst
        {
            remainQty = remainQty - qty;
            document.getElementById("<%=lbShowReQty.ClientID%>").innerHTML = remainQty;
            if(remainQty == 0)
            {
                ResetPage();
            }

            if (result[1].length > 0) {
                var keyCollection = new Array();
                var valueCollection = new Array();

                keyCollection[0] = "@BegNo";
                valueCollection[0] = generateArray(result[2]);

                keyCollection[1] = "@EndNo";
                valueCollection[1] = generateArray(result[3]);

                result[1][0].LabelType = labelType;

                setPrintParam(result[1], labelType, keyCollection, valueCollection);
                printLabels(result[1], false);
            }

            ShowSuccessfulInfo(true, "MB (" + result[2] + ") ~ (" + result[3] + ") " + mesIsPrinted);
        }
        else {
            ShowInfo("");
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
        }
    }
    catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onFail
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	调用web service失败
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onFail(error)
{
   try {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
     
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showPrintSettingDialog
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:   弹出Print Setting对话框
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function showPrintSettingDialog()
{
    showPrintSetting(stationId, pCode);
}


function reprint() {
    var url = "../SA/MBLabelReprint.aspx" + "?Station=" + stationId + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + AccountId + "&Login=" + Login;
    window.showModalDialog(url, "", 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');  
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkNumber
//| Author		:	Lucy Liu
//| Create Date	:	01/07/2010
//| Description	:	check Qty的合法性
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

function OnCancel() {
    WebServiceMBLabelPrint.cancel(getMOCmbValue());
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	退出页面时调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage() {
    OnCancel();
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ResetPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	刷新页面时调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ResetPage()
{
    ExitPage();
    document.getElementById("<%=btnReset.ClientID%>").click();
}
</script>
</asp:Content>

