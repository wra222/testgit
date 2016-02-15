<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: FA Combine Key Parts(FA)
 Update : 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-10-19  Song Hai-Yan           Create 
 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="FACombineKeyParts.aspx.cs" Inherits="FA_FACombineKeyParts" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceFACombineKeyParts.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbpdLine" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbsubStation" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td colspan="5">
                        <iMES:CmbReturnStation ID="cmbsubStation" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                  </table>
         <table border="0" width="95%">   
       
                <tr>
                    <td style="width: 15%" align="left">
                        &nbsp;</td>
                    <td style="width: 30%" align="left">
                        &nbsp;</td>
                    <td>
                    </td>
                    <td style="width: 12%" align="left">
                        &nbsp;</td>
                    <td align="left">
                        &nbsp;</td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr>
                    </td>
                </tr>
                              
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                     <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtProdId" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                   <td>
                    </td>
                    <td style="width: 12%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                   <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>                    
                     <td>
                    </td>
                </tr>
                 
                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lbCollection" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
   <TR>
	    <TD colspan="5">
	      <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
            <Triggers>
              　<asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick"></asp:AsyncPostBackTrigger>
            </Triggers>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                OnGvExtRowClick="" OnGvExtRowDblClick="showCollection(this)"  Width="99.9%" Height="230px"
                SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
               onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                    <Columns >   
           <asp:BoundField DataField="Image" />
                                        <asp:BoundField DataField="SubstitutePartNo" />
                                        <asp:BoundField DataField="SubstituteDescr" />
                                        <asp:BoundField DataField="PartType"  />
                                        <asp:BoundField DataField="PartDescr"  />
                                        <asp:BoundField DataField="PartNo" />
                                        <%--
                                        <asp:BoundField DataField="ValueType" />
                                         --%>
                                        <asp:BoundField DataField="Qty" />
                                        <asp:BoundField DataField="PQty" />
                                        <asp:BoundField DataField="CollectionData" />
                                        <asp:BoundField DataField="HfCollectionData" />
                                        <asp:BoundField DataField="HfPartNo" />
                                        <asp:BoundField DataField="HfIndex" />
                  </Columns>
                </iMES:GridViewExt>
            </ContentTemplate> 
        </asp:UpdatePanel> 
	    </TD>
	</TR>
         </table>
         <table border="0" width="95%">   
	
	
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="5" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="500" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none"
                                    onserverclick="FreshGrid">
                                </button>
                                <button id="btnGridClear" runat="server" type="button" onclick="" style="display: none"
                                    onserverclick="clearGrid">
                                </button>
                                <input id="prodHidden" type="hidden" runat="server" />
                                <input id="sumCountHidden" type="hidden" runat="server" />
                                <input type="hidden" runat="server" id="station" />
                                <input type="hidden" runat="server" id="useridHidden" />
                                <input type="hidden" runat="server" id="hidRowCnt" /> 
                                <input type="hidden" runat="server" id="hidWantData" />                                  
                                <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
             <tr>
                    <td></td>
                    <td colspan="5" align="right">
		                    <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="showPrintSettingDialog()" />
		                    <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
                            <input type="hidden" runat="server" id="pCode" />
                    </td>
                </tr>
            </table>
        </center>
    </div>

<script language="javascript" for="objMSComm" event="OnComm">
</script>

<script type="text/javascript">

var editor;
var customer;

var GridViewExt1ClientID = "<%=GridViewExt1.ClientID%>";

var index = 1;
var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount,10) + 1;
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesNoSelSubStation = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectTestStation").ToString()%>';
var mesNoInputProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoInputProdId").ToString()%>';
var mesDupData = '<%=this.GetLocalResourceObject(Pre + "_mesDupData").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var custsn = "";
var proid="";
var IsBlankStation = false;
var fistSelStation = "";
var pcode;

var accountid = '<%=AccountId%>';
var username = '<%=UserName%>';
var login = '<%=Login%>';

var enableComm = '<%=Request["SendPass"] %>'=='1' ? true : false;
var objMSComm = document.getElementById("objMSComm");
var signalPass = '';
var signalFail = '';
function CommSendPass() {
	if(enableComm)
		objMSComm.Output = signalPass;
}
function CommSendFail() {
	if(enableComm)
		objMSComm.Output = signalFail;
}

document.body.onload = function() {

    try {
        pcode = document.getElementById("<%=pCode.ClientID%>").value;
        editor = "<%=userId%>";
        customer = "<%=customer%>";
        gvClientID = "<%=GridViewExt1.ClientID %>";
        //        setPdLineCmbFocus();
        getPdLineCmbObj().setAttribute("AutoPostBack", "True");
        document.getElementById("<%=hidWantData.ClientID%>").value = "0";   //ProdId wanted

        if (enableComm) {
            var ret = setCommPara_PassOrFail('<%=ConfigurationSettings.AppSettings["CommIniPath"]%>', "PassOrFail");
            if (ret[0]) {
                signalPass = ret[1];
                signalFail = ret[2];
            }

            /*$.exclusionInOut({
            acquire: function() {
	            var ret=setCommPara_PassOrFail('<%=ConfigurationSettings.AppSettings["CommIniPath"]%>', "PassOrFail");
	            if(ret[0]){
		            signalPass = ret[1];
		            signalFail = ret[2];
	            }
	            return ret[0];
            },
            release: function() {
	            objMSComm =document.getElementById("objMSComm");
	            if (objMSComm && objMSComm.PortOpen) {
	            	objMSComm.PortOpen = false;
	            }
	            return true;
            }
            });*/
        }
        
        getAvailableData("processDataEntry");
    } catch (e) {
        alert(e.description);
    }
}



function SetStationDefault(selStation)
{
 
     var obj = getTestStationCmbObj();
      for (var i = 0; i < obj.options.length; i++)
      {
         if (obj.options[i].value == selStation)
         { obj.selectedIndex = i; return; }
     }   
}

var lstPrintItem;
 var selStation = "";
function processDataEntry(inputData) {

 // var obj=getPdLineCmbObj();
 // obj.disabled = true;
    // var selStation = "";
   try{
       var errorFlag = false;
        getCommonInputObject().focus();
       if (getPdLineCmbValue() == "")
       {
           alert(mesNoSelPdLine);
           errorFlag = true;
           //setPdLineCmbFocus();
           getAvailableData("processDataEntry");
       }

       if (getReturnStationCmbValue() != "")
       {
           selStation = getReturnStationCmbValue();
           IsBlankStation = false;
       }
       else {
           alert(mesNoSelSubStation);
           errorFlag = true;
           //setReturnStationCmbFocus();
           getAvailableData("processDataEntry");

       }
       if (!errorFlag) {
           if (document.getElementById("<%=txtProdId.ClientID%>").innerText == "") {
               ShowInfo("");

               lstPrintItem = getPrintItemCollection();
               var SeltmpStation = getReturnStationCmbValue();
               if (SeltmpStation == '3B' || SeltmpStation == '3FB') {
                   if (lstPrintItem != null) {
                       //For BN  Custs SN lenth =16 so not need  SubStringSN(...) Modify by Benson
                       document.getElementById("<%=prodHidden.ClientID%>").value = SubStringSN(inputData, "ProdId");
                       beginWaitingCoverDiv();
                       document.getElementById("<%=btnGridFresh.ClientID%>").click(); //Refesh DataGrid..
                       getCommonInputObject().focus();
                       //getAvailableData("processDataEntry");
                   }
                   else {
                       alert(msgPrintSettingPara);
					   CommSendFail();
                       callNextInput();
                       endWaitingCoverDiv();
                   }
               }
               else {


                   //For BN  Custs SN lenth =16 so not need  SubStringSN(...) Modify by Benson
                   document.getElementById("<%=prodHidden.ClientID%>").value = SubStringSN(inputData, "ProdId");
                   beginWaitingCoverDiv();
                   document.getElementById("<%=btnGridFresh.ClientID%>").click(); //Refesh DataGrid..
                   getCommonInputObject().focus();
                   //getAvailableData("processDataEntry");
               }

           }
           else {
                    //lstPrintItem = getPrintItemCollection();


                    var proid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
                   fistSelStation = getReturnStationCmbText();
                   proid = SubStringSN(proid, "ProdId");
                   if (inputData == "getbomnull") {
                       flag = false;
                       flag_39 = false;
                       var SelStation = getReturnStationCmbValue();
                       if (selStation == '3B' || selStation == '3FB') {
                           flag = true;
                       }
                       else {
                           flag = false;
                       }
                       if (selStation == '39') {
                           flag_39 = true;
                       }
                       else {
                           flag_39 = false;
                       }

                       WebServiceFACombineKeyParts.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, flag, flag_39, lstPrintItem, onSaveSucceed, onSaveFail);

                   }
                   else {

                       if (inputData == "7777") {

                           var tmpTable = document.getElementById("<%=GridViewExt1.ClientID%>");
                           var tmprowCnt = document.getElementById("<%=hidRowCnt.ClientID%>").value;
                           var tmp_count;
                           for (tmp_count = 1; tmp_count <= tmprowCnt; tmp_count++) {
                               tmpTable.rows[tmp_count].cells[7].innerText = 0;
                               tmpTable.rows[tmp_count].cells[8].innerText = " ";
                               tmpTable.rows[tmp_count].cells[8].title = "";

                           }
                           WebServiceFACombineKeyParts.ClearPart(document.getElementById("<%=txtProdId.ClientID%>").innerText, onClearSucceeded, onClearFailed);

                       }
                       else {
                           WebServiceFACombineKeyParts.inputPPID(document.getElementById("<%=txtProdId.ClientID%>").innerText, inputData, onSucceed, onFail);
                       }
                   }

           }

                    }

            } catch(e) 
      
      {
        alert(e.description);
		CommSendFail();
      }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setStatus
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	ÉèÖÃÒ³ÃæËùÓÐÓëProdIdÉ¨ÈëÏà¹Ø¿Ø¼þµÄ×´Ì¬
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setStatus(status)
{
   
    try {
        getPdLineCmbObj().disabled = status;
        //getTestStationCmbObj().disabled = status;
     
      

    }catch (e) {
        alert(e.description);
        
    }
}

function onSucceed(result) {
    // endWaitingCoverDiv();
    //ShowInfo("Succeed!");
    //ShowMessage("Succeed!");
   // WebServiceFACombineKeyParts.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, onSaveSucceed, onSaveFail);
    try {
        // endWaitingCoverDiv();
        eval("setRowNonSelected_" + gvClientID + "()");
       
        var iLength = result.length;
        if (result == null) {
            endWaitingCoverDiv();
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
			CommSendFail();
            callNextInput();
        }
        else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
            //处理界面输出信息
            gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
            qtySum = 0;
            pqtySum = 0;
            needCheck = true;
            alreadyMatch = false;

            var flag = false;
            var flag_39 = false;

            var rowCnt = document.getElementById("<%=hidRowCnt.ClientID%>").value;
            repVC = result[1];
            for (k = 1; k <= rowCnt; k++) {
                qty = parseInt(gvTable.rows[k].cells[6].innerText);
                pqty = parseInt(gvTable.rows[k].cells[7].innerText);
                qtySum += qty;
                pqtySum += pqty;
                if (!alreadyMatch) {
                    //oldVC = "," + gvTable.rows[k].cells[10].innerText + ",";
                    //if (oldVC.indexOf("," + repVC.substring(0, 5) + ",") >= 0) {
                    //    if (qty <= pqty) {
                    //        needCheck = false;
                    //        ShowInfo("Part \"" + repVC + "\" already changed!\nPlease scan/input another part!");
                    //        break;
                    //    }
                    //modify ITC-1360-1651 BUG
                    //if (gvTable.rows[k].cells[1].innerText == repVC && gvTable.rows[k].cells[6].innerText != gvTable.rows[k].cells[7].innerText) {
                    if (gvTable.rows[k].cells[1].innerText.indexOf(repVC) != -1 && gvTable.rows[k].cells[6].innerText != gvTable.rows[k].cells[7].innerText) {
                            eval("setScrollTopForGvExt_" + gvClientID + "('" + gvTable.rows[k].cells[11].innerText + "',11,'','MUTISELECT')");
                        
                            alreadyMatch = true;
                            pqty++;
                            pqtySum++;
                            gvTable.rows[k].cells[7].innerText = pqty;
                            collectionData = gvTable.rows[k].cells[8].innerText;
                       // }
                            if (collectionData != " ") {
                            collectionData += "," + result[2];
                        }
                        else {
                            collectionData = result[2];
                        }
                        gvTable.rows[k].cells[8].innerText = collectionData;
                        gvTable.rows[k].cells[8].title = collectionData;
                       
                    }
                }
            }
            //ShowInfo("qtySum" + qtySum + "pqtySum" + pqtySum);
             if (needCheck == true && qtySum <= pqtySum) {
            //if (needCheck == true && 1 == pqtySum) {
                 //ResetPage();
                 flag = false;
                 flag_39 = false;
                 var SelStation = getReturnStationCmbValue();
                 if (selStation == '3B' || selStation == '3FB') {
                     flag = true;
                 }
                 else {
                     flag = false;
                 }
                 if (selStation == '39') {
                     flag_39 = true;
                 }
                 else {
                     flag_39 = false;
                 }

                 WebServiceFACombineKeyParts.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, flag, flag_39, lstPrintItem, onSaveSucceed, onSaveFail);

            }
            endWaitingCoverDiv();
            callNextInput();
        }
        else {
            endWaitingCoverDiv();
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
			CommSendFail();
            callNextInput();
        }
    } catch (e) {
        alert(e.description);
    }
    /*
    // endWaitingCoverDiv();
    try {
    if (result == null) {
    setStatus(true);
    var content = msgSystemError;
    ShowMessage(content);
    ShowInfo(content);
    callNextInput();

        }
    else if ((result.length == 2) && (result[0] == SUCCESSRET)) {

            setStatus(true);
    setScrollCycle(result[1]);
    callNextInput();
    ShowInfo("");

        }
    else {
    setStatus(true);
    var content = result[0];
    ShowMessage(content);
    ShowInfo(content);
    clearData();
    callNextInput();
    }
    } catch (e) {
    alert(e.description);
    }*/
}

function setScrollCycle(matchDataArray) {
    eval("setRowNonSelected_" + gvClientID + "()");


    for (var i = 0; i < matchDataArray.length; i++) {

        setScroll(matchDataArray[i].ValueType, matchDataArray[i].PNOrItemName, matchDataArray[i].CollectionData)
    }
}
function setScroll(valueType, partno, inputData) {
    try {
        var oldQty;
        var oldScanedQty;
        var oldCldata;
        var oldhfCldata;
        var oldhfPartno;
        var newCldata;
        var newhfCldata;
        var newhfPartno;
        var newScanedQty;
        var rowQty;
        var row;
        var subArray;
        var subFindFlag = false;
        var table = document.getElementById(gvClientID);
        for (var i = 1; i < table.rows.length; i++) {
            if (table.rows[i].cells[6].innerText.trim() == valueType) {
                if (table.rows[i].cells[3].innerText == partno) {
                    row = eval("setScrollTopForGvExt_" + gvClientID + "('" + partno + "',3,'','MUTISELECT')");
                    break;
                }
                subArray = table.rows[i].cells[1].innerText.split(";");
                for (var j = 0; j < subArray.length; j++) {
                    if (partno == subArray[j]) {
                        subFindFlag = true;
                    }
                }

                if (subFindFlag) {
                    row = eval("setScrollTopForGvExt_" + gvClientID + "('" + table.rows[i].cells[3].innerText + "',3,'','MUTISELECT')");
                    break;
                }
            }
        }
        if (row != null) {
            rowQty = row.cells[7].innerText.trim();
            oldScanedQty = row.cells[8].innerText.trim();
            oldCldata = row.cells[9].innerText.trim();
            oldhfCldata = row.cells[10].innerText.trim();
            oldhfPartno = row.cells[11].innerText.trim();
            newScanedQty = parseInt(oldScanedQty, 10) + 1;
            if (newScanedQty > parseInt(rowQty, 10)) {
                alert(msgCollectExceedCount);
                eval("setRowNonSelected_" + gvClientID + "()");
                return;
            }
            if (oldhfPartno.length > 0) {
                newhfPartno = oldhfPartno + "," + partno;
            }
            else {
                newhfPartno = oldhfPartno + partno;
            }

            if (oldhfCldata.length > 0) {
                newhfCldata = oldhfCldata + "," + inputData;
            }
            else {
                newhfCldata = oldhfCldata + inputData;
            }
            var newCldata = newhfCldata;
            if (newhfCldata.length > 20) {
                newCldata = newhfCldata.substring(0, 20) + "...";
            }
            row.cells[8].innerText = newScanedQty;
            row.cells[9].innerText = newCldata;
            row.cells[10].innerText = newhfCldata;
            row.cells[11].innerText = newhfPartno;
            scanQty++;
            if (scanQty == parseInt(qty, 10)) {
                beginWaitingCoverDiv();

                ResetPage();
                WebServiceFACombineKeyParts.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, onSaveSucceed, onSaveFail);
            }

        }
        else {
            alert(msgCollectNoItem);
            eval("setRowNonSelected_" + gvClientID + "()");
        }
    } catch (e) {
        alert(e.description);
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	clearTable
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	½«Defect List tableµÄÄÚÈÝÇå¿Õ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function clearTable() {
    try {
        ClearGvExtTable("<%=GridViewExt1.ClientID%>", initRowsCount);

    } catch (e) {
        alert(e.description);

    }

}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	reset
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	Çå¿Õ½çÃæ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function reset()
{
    try {
        document.getElementById("<%=txtProdId.ClientID%>").innerText = "";
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";
        selStation = "";
        clearTable();
    }catch (e) {
        alert(e.description);
       
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callNextInput
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	µÈ´ý¿ìËÙ¿Ø¼þ¼ÌÐøÊäÈë
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function callNextInput()
{
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}       


window.onbeforeunload= function() 
{
      
    ExitPage(); 
}  


function onClearSucceeded(result)
{
    try {
        if(result==null)
        {
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
			CommSendFail();
            callNextInput();
        }
        else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
            //变更描述： 修改7777规则：刷入7777，清空除Station和Line之外的所有信息
            ExitPage();
            reset();             
            ShowInfo("");
			CommSendFail();
            callNextInput();               
        }
        else 
        {
            ShowInfo("");
            var content =result[0];
            ShowMessage(content);
            ShowInfo(content);
			CommSendFail();
            callNextInput();
        } 
    }catch (e) {
        alert(e.description);
       
    }
}

function onClearFailed(error)
{
   try {
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
		CommSendFail();
        reset();
        callNextInput();
     }catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showCollection
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	ÏÔÊ¾ÏêÏ¸ÐÅÏ¢
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function showCollection(row) {
    if (row != null) {
        var qty = row.cells[6].innerText.trim();
        var pqty = row.cells[7].innerText.trim();
        var PN = row.cells[5].innerText.trim();
        var Cldata = row.cells[9].innerText.trim();
        var ClPartNodata = row.cells[10].innerText.trim();

        var dataCllist = Cldata.trim().split(",");
        var dataPartNoCllist = ClPartNodata.trim().split(",");
        if (Cldata == "") {
            dataCllist.pop();
        }
        if (ClPartNodata == "") {
            dataPartNoCllist.pop();
        }
        var popParam = new dataInfo(PN, qty, pqty, dataCllist, dataPartNoCllist);
        ShowCollection(popParam);
    }
    else {
        alert("Please select a row");
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showSubstitute
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	ÏÔÊ¾Ìæ´úÁÏ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function showSubstitute(partno, substitutePartNo, substituteDescr) {
    try {
        var dataPartNolist = substitutePartNo.trim().split(";");
        var dataDescrlist = substituteDescr.trim().split("|");
        var popParam = new substituteInfo(partno, dataPartNolist, dataDescrlist);

        ShowSubstitute(popParam);
    } catch (e) {
        alert(e.description);
    }
}


function onFail(error) {
    //ShowMessage("Fail!");
    //ShowInfo("Fail!");

    // endWaitingCoverDiv();
    try {
        //       endWaitingCoverDiv();
        setStatus(true);
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        clearData();
		CommSendFail();
        callNextInput();
    } catch (e) {
        alert(e.description);
        //        endWaitingCoverDiv();
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSaveSucceed
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	±£´æ³É¹¦
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSaveSucceed(result) {
    //ShowMessage("onSaveSucceed!");

    try {
        endWaitingCoverDiv();
        ShowInfo("Suceed!", "green");
        //ShowMessage("Suceed0!");
        eval("setRowNonSelected_" + gvClientID + "()");
        //ShowInfo("Suceed1");        
        if (result == null) {
            setStatus(true);
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
			CommSendFail();
            callNextInput();
        }
        else if (result[0] == SUCCESSRET && result.length == 3) {

        var tmpid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
        reset();
            scanQty = 0;
            sumQty = 0;
            setStatus(false);
            callNextInput();
            //lstPrintItem = getPrintItemCollection();
            var stationtmp = getReturnStationCmbText();
            if (stationtmp.indexOf('3B') != -1 || stationtmp.indexOf('3FB') != -1) {
                setPrintItemListParam(result[1], result[2]); // 1 : PrintItem   2 : Custsn
                printLabels(result[1], false);
           
            }

            ShowSuccessfulInfo(true, "[" + tmpid + "] " + msgSuccess);
			CommSendPass();
        }
        else {
            setStatus(true);
            var content = result[0];
            ShowMessage(content);
            ShowInfo(content);
			CommSendFail();
            callNextInput();
        }


    } catch (e) {
        alert(e.description);
        endWaitingCoverDiv();
    }

}

function onSaveFail(error) {
    //ShowMessage("onSaveFail!");

    try {
        endWaitingCoverDiv();
        eval("setRowNonSelected_" + gvClientID + "()");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    } catch (e) {
        alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Print using~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function setPrintItemListParam(backPrintItemList, custsn) {
    var lstPrtItem = backPrintItemList;
    var keyCollection = new Array();
    var valueCollection = new Array();
    keyCollection[0] = "@productID";
    valueCollection[0] = generateArray(custsn);
    //setPrintParam(lstPrtItem, "CT Label", keyCollection, valueCollection);
    setAllPrintParam(lstPrtItem, keyCollection, valueCollection);

}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	ÍË³öÒ³ÃæÊ±µ÷ÓÃ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage()
{
    if(document.getElementById("<%=txtProdId.ClientID%>").innerText != "")
    {
         WebServiceFACombineKeyParts.Cancel(document.getElementById("<%=txtProdId.ClientID%>").innerText, getReturnStationCmbValue(), onClearSucceeded,onClearFailed);
    } 
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ResetPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	Ë¢ÐÂÒ³ÃæÊ±µ÷ÓÃ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ResetPage()
{
    ExitPage();
    getCommonInputObject().value = "";
    reset();
}

function showPrintSettingDialog() 
{
    var fistSelStation = getReturnStationCmbValue();
    showPrintSetting(fistSelStation, document.getElementById("<%=pCode.ClientID%>").value);
}

function reprint() {
    var fistSelStation = getReturnStationCmbValue();

    var url = "FACombineKeyPartsReprint.aspx?Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
    var paramArray = new Array();
    paramArray[0] = getPdLineCmbValue();
    paramArray[1] = editor;
    paramArray[2] = customer;
    paramArray[3] = fistSelStation;
    window.showModalDialog(url, paramArray, 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');     


}

//-->
    </script>

</asp:Content>

