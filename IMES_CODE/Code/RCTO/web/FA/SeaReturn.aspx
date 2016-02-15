<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SeaReturn.aspx.cs" Inherits="FA_SeaReturn" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceSeaReturn.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
     <TR>
	   <td><asp:Label ID="lblModelType" runat="server" Text="机型类别:" CssClass="iMes_label_13pt"></asp:Label></td>
	   <td Width="20%"><iMES:CmbConstValueType ID="cmbSEAReturnCategory" runat="server" Width="50" IsPercentage="true" /></td>
    </TR
    <TR>
	    <TD style="width:15%" align="left"><asp:Label ID="lblProdID" runat="server" 
                Text="MBCT or LCDCT:" CssClass="iMes_label_13pt"></asp:Label></TD>
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
	    <td style="width:9%" align="left"></td>
	    <td colspan="5" align="left"></td>
	   
	    <td align="right">
	        <table border="0" width="95%">
	             <td style="width:80%" align="right"><input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
	            <td><input id="btnRePrint" type="button"  runat="server" style="width:100px" onclick="print()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                        align="right"/></td>
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

    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
   
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

    var SUCCESSRET = "SUCCESSRET";
    var lstPrintItem;
    var inputObj;
    var inpuProdid;
    var line;
    var editor;
    var customer;
    var station;
    document.body.onload = function() {
        line = '';
        editor = "<%=UserId%>";
        customer = '<%=Customer %>';
        station = '<%=Request["Station"] %>';
        inputObj = getCommonInputObject();
        ShowInfo("");
        getCommonInputObject().focus();
        inputData = inputObj.value;

        getAvailableData("ProcessInput");
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
	
	function GetCmbValue(o) {
		if (o == null) return '';
		return o.options[o.selectedIndex].text;
	}

    function print() {
        try {
            var msg = "";
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            inpuProdid = getCommonInputObject().value.trim();

            if (inpuProdid == "") {
                msg = "please input CT";
                alert(msg);
                getCommonInputObject().focus();
				return;
            }
			
			lstPrintItem = getPrintItemCollection();
			if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
			{
				msg = msgPrintSettingPara;
				alert(msg);
				getCommonInputObject().focus();
				return;
			}
            
			var returnType = GetCmbValue( document.getElementById("<%=cmbSEAReturnCategory.ClientID%>" + "_drpConstValueType") );
			if (''==returnType){
				msg = "please choice ReturnType";
                alert(msg);
                getCommonInputObject().focus();
				return;
			}
			beginWaitingCoverDiv();
			//     RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
			//PageMethods.RePrint(inpuProdid'', editor, station, customer, lstPrintItem, onSucceed, onFail);
			WebServiceSeaReturn.InputCT(inpuProdid, returnType, '', editor, station, customer, lstPrintItem, onSucceed, onFail);
        } catch (e) {
            alert(e.description);
        }
    }



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Lucy Liu
    //| Create Date	:	10/27/2009
    //| Description	:	调用web service成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                //service方法没有返回
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if (result.Success == SUCCESSRET && result.PrintItem != null && result.PrintItem.length > 0) {
                for (var i = 0; i < result.PrintItem.length; i++) {
					var labelCollection = [];
					labelCollection.push(result.PrintItem[i]); //result.PrintItem
					setPrintItemListParam(labelCollection, result.PrintItem[i].LabelType, result.ProId);
					printLabels(labelCollection, false);
				}
                    
                ShowInfo("print success!", "green");
                ShowSuccessfulInfo(true, "[" + getCommonInputObject().value + "] " + msgSuccess);

                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else {
                var content = result;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
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
    function onFail(error) {

        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());

        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }

    }
    function setPrintItemListParam(backPrintItemList, labelType, ProId) {
        var lstPrtItem = backPrintItemList;
		var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@ProductID";
        valueCollection[0] = generateArray(ProId);
        setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
    }

    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        ShowInfo("");
        endWaitingCoverDiv();

    }

    function showPrintSettingDialog() {
        //     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
        showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
    }
function btnSetting_onclick() {

}

function btnRePrint_onclick() {

}

</script>
</asp:Content>

