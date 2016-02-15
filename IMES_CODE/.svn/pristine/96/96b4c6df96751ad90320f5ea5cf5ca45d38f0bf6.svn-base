
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="TravelCardRePrint_CR.aspx.cs" Inherits="TravelCardReprint_CR" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceTravelCardRePrint_CR.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
     <TR>
	   <td></td>
    </TR
    <TR>
	    <TD style="width:12%" align="left"><asp:Label ID="lblProdID" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></TD>
	    <TD colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </ContentTemplate>
        </asp:UpdatePanel>                                        
       </TD>
    </TR>
    
   <tr>
	    <td style="width:12%" align="left" ><asp:Label ID="lbReason" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <textarea id="txtReason" style="width: 99%; height: 100px"></textarea>                
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
    </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>          
    </ContentTemplate>   
</asp:UpdatePanel> 


<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';



    var SUCCESSRET = "SUCCESSRET";
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var lstPrintItem;
    var inputObj;
    var inpuProdid;
    
    document.body.onload = function() {
        inputObj = getCommonInputObject();
        ShowInfo("");
        getAvailableData("ProcessInput");
        getCommonInputObject().focus();
    }

    function ProcessInput(inputData) {
        try{
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

    function print() {
        try {
            var errorFlag = false;
            var msg = "";
            var reason = document.getElementById("txtReason").value;
            inpuProdid = getCommonInputObject().value.trim();
            var station = document.getElementById("<%=stationHF.ClientID %>").value;
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            
            if (inpuProdid == "") {
                errorFlag = true;
                msg = mesNoProdId;
                alert(msg);
                getCommonInputObject().focus();                
            }
            else if (reason == "") {
                //ITC-1360-1612
            }
            else if (reason.length > 80) {
                errorFlag = true;
                msg = mesReasonOutRange;
                //ITC-1360-1264
                alert(msg);
                document.getElementById("txtReason").focus();                
            }
            lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
            {
                msg = msgPrintSettingPara;
                alert(msg);
                getCommonInputObject().focus();
                return;
            }
            if (inpuProdid.length == 9) {
                WebServiceTravelCardRePrint.PrintTravelCard(inpuProdid, reason, editor, station, customer, pCode, lstPrintItem, onSucceed, onFail);
            }
            else if (inpuProdid.length == 14 || inpuProdid.length == 18) {
                WebServiceTravelCardRePrint.GetProductID(inpuProdid, onSucceedGet, onFailGet)
            }
            
//            if (!errorFlag) {
//                var station = document.getElementById("<%=stationHF.ClientID %>").value;
//                try {
//                    lstPrintItem = getPrintItemCollection();
//                    if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
//                    {
//                        msg = msgPrintSettingPara;
//                        alert(msg);
//                        getCommonInputObject().focus();                        
//                        return;
//                    }
//                    beginWaitingCoverDiv();
//                    WebServiceTravelCardRePrint.PrintTravelCard(inpuProdid, reason, editor, station, customer, pCode, lstPrintItem, onSucceed, onFail);                    
//                }
//                catch (e1) {
//                    alertAndCallNext(e1.description);
//                } 
//            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onSucceedGet(result) {
        var pCode = document.getElementById("<%=pCode.ClientID%>").value;
        var reason = document.getElementById("txtReason").value;
        var station = document.getElementById("<%=stationHF.ClientID %>").value;
        try {
            
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            inpuProdid = result;
            beginWaitingCoverDiv();
            WebServiceTravelCardRePrint.PrintTravelCard(inpuProdid, reason, editor, station, customer, pCode, lstPrintItem, onSucceed, onFail);
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFailGet(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }

    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                ShowSuccessfulInfo(true, "[" + result[1][1] + "] " + msgSuccess);
                setPrintItemListParam1(result[1][2], result[1][0], result[1][1]);
                printLabels(result[1][2], true);
                //ITC-1360-1348
                //document.getElementById("txtReason").value = "";
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }
    
    function setPrintItemListParam1(backPrintItemList, pdline, beginProid) //Modify By Benson at 2011/03/30
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@productID1";
        keyCollection[1] = "@productID2";

        valueCollection[0] = generateArray(beginProid);
        valueCollection[1] = generateArray(beginProid);

        for (ii = 0; ii < backPrintItemList.length; ii++) {
            backPrintItemList[ii].ParameterKeys = keyCollection;
            backPrintItemList[ii].ParameterValues = valueCollection;
            //setPrintParam(lstPrtItem, lstPrtItem[ii].LabelType, keyCollection, valueCollection);
        }

        //setPrintParam(lstPrtItem, "Travel Card", keyCollection, valueCollection);
    }



    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        document.getElementById("txtReason").value = "";
        ShowInfo("");
        endWaitingCoverDiv();

    }

    function showPrintSettingDialog() {
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
</script>
</asp:Content>

