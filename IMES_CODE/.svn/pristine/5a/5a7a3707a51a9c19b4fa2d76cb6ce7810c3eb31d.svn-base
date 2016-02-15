<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PrintInatelICASARePrint.aspx.cs" Inherits="PrintInatelICASARePrint" Title="Reprint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/PrintInatelICASAService.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
     <TR>
	   <td></td>
    </TR
    <TR>
	    <TD style="width:25%" align="left"><asp:Label ID="lblProdID" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></TD>
	    <TD colspan="6" align="left">
	    
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"  
	                     ProcessQuickInput="true" Width="99%" CanUseKeyboard="true"
                        IsPaste="true" IsClear="false"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                                           
       </TD>
    </TR>
    
   <tr>
	    <td style="width:25%" align="left" ><asp:Label ID="lbReason" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
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
	    <td style="width:25%" align="left"></td>
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
 <tr>
        <td>&nbsp;</td>
        <td>
        <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                    
                    <input type="hidden" runat="server" id="Hidden_ProdID"/>
                    <input type="hidden" runat="server" id="station" /> 
                    <input type="hidden" runat="server" id="Hidden1" /> 
            </ContentTemplate>   
        </asp:UpdatePanel>   
        </td>
        </tr>


<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';


    var stationid = '';
    var pCodeid;
    var SUCCESSRET = "SUCCESSRET";
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var lstPrintItem;
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Lucy Liu
    //| Create Date	:	10/27/2009
    //| Description	:	置页面焦点
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    var inputObj;
    var inpuProdid;
    var inputdata;
    var strCustSn;
    
    document.body.onload = function() {
        DisplayInfoText();     //显示MessageTextArea框
        stationId = '<%=Request["Station"] %>';
        pCodeid = '<%=Request["PCode"] %>';
        inputObj = getCommonInputObject();
        getAvailableData("input");
        ShowInfo("");                
        getCommonInputObject().focus();
    }
    function checkInput(data) {
        if (data.length == 10) {
            //if (data.substring(0, 3) == "CNU")
			if (CheckCustomerSN(data))
                return data;
        }
        return '0';
    }
    
    function input() {
        print();
    }
    function print() {
        try {
            var errorFlag = false;
            var msg = "";
            var pCode = document.getElementById("<%=pCode.ClientID%>").value;
            var reason = document.getElementById("txtReason").value;
            inputdata = getCommonInputObject().value.trim();
            strCustSn = checkInput(inputdata);

            if (strCustSn == '0') {
                errorFlag = true;
               
                alert("wrong code");
                getAvailableData("input");
                getCommonInputObject().focus();                
            }
            
            else if (reason.length > 80) {
                errorFlag = true;
                msg = mesReasonOutRange;
                document.getElementById("txtReason").focus();                
            }

            if (!errorFlag) {

                var station = document.getElementById("<%=stationHF.ClientID %>").value;
                try {
                    lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
                    {
                        msg = msgPrintSettingPara;
                        alert(msg);
                        getCommonInputObject().focus();
                        getAvailableData("input");                      
                        return;
                    }
                                        
                    beginWaitingCoverDiv();
                    PrintInatelICASAService.rePrint("", strCustSn, reason, lstPrintItem, editor, station, customer, onSucceed, onFail);                    
                }
                catch (e1) {
                    alert(e1.description);
                }
            }
        } catch (e) {
            alert(e.description);
        }
    }


    function printLabelFun(result) {
        var LabelType;
        if (result[2] == "Anatel label")
            LabelType = "Anatel Label";
        else
            LabelType = "ICASA Label";

        var labelCollection = [];
        for (var i in result[1]) {
            if (result[1][i].LabelType == LabelType) {
                labelCollection.push(result[1][i]);
                break;
            }
        }

        try {
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@productID";
            valueCollection[0] = generateArray(result[3]);

            setPrintParam(labelCollection, LabelType, keyCollection, valueCollection);
            printLabels(labelCollection, false);
        }

        catch (e) {
            alert(e.description);
            //lstPrintItem = null;
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
            getAvailableData("input");
            if (result == null) {
                //service方法没有返回
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 4) && (result[0] == SUCCESSRET)) {

            // setPrintItemListParam1(result[1][2], result[1][0], result[1][1]);
            //printLabels(result[1], false);
            printLabelFun(result);
                //setPrintItemListParam1(backPrintItemList, pdline, shipdate, qty, model, mo, beginProid, endProid)
                //置成功信息
            var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
                ShowSuccessfulInfo(true, "[" + strCustSn + "]" + msgSuccess1);
                //ShowSuccessfulInfoFormat(true, "Product ID", inpuProdid); // Print 成功，带成功提示音！
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
        endWaitingCoverDiv();
        getAvailableData("input");
        try {         
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
           // getCommonInputObject().value = "";
            //document.getElementById("txtReason").value = "";
            getCommonInputObject().focus();
        } catch (e) {
            alert(e.description);
        }

    }


    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        document.getElementById("txtReason").value = "";
        ShowInfo("");
        endWaitingCoverDiv();

    }
    function setProductFocus() {
        getCommonInputObject().focus();
    }
    function showPrintSettingDialog() {
        //     showPrintSetting(document.getElementById("<%=pCode.ClientID%>").value);
        showPrintSetting(stationid, pCodeid);  
    }
</script>
</asp:Content>

