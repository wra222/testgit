 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="OfflineCommonPartsLabelPrint.aspx.cs" Inherits="OfflineCommonPartsLabelPrint" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<script type="text/javascript">

    var msgNoPrinted = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrinted") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    //    var msgNoPrintItem='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrintItem") %>';
    var inputObj = "";
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var editor = "";
    var customer = "";
    var station = "";
    var pcode = "";
    var model = "";
    var qty="";
    document.body.onload = function() {
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        station = '<%=Request["Station"]%>';
        pcode = '<%=Request["PCode"]%>';
        inputObj = getCommonInputObject();
        ShowInfo("");
        inputObj.focus();
        getAvailableData("processDataEntry");
    }
    function callNextInput() {
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
    }

    function processDataEntry(inputData) {
        ShowInfo("");
        var lstPrintItem = getPrintItemCollection();
        if (lstPrintItem == null)//
        {
            alert(msgPrintSettingPara);
            callNextInput();
            return;
        }
        if (model == "") {
            model = inputData;
            document.getElementById("txtModel").value = inputData;
            ShowInfo("Please input qty", "green");
        }
        else {
            qty=inputData;
            beginWaitingCoverDiv();
            PageMethods.Print(model, lstPrintItem, "", editor, "", customer, onSucess, onError);
        }
        callNextInput();
    }
    function onSucess(result) {
        endWaitingCoverDiv();
        var SPS = result[0]; //＠Model,@SPS,@Qty
        setPrintItemListParam(result[1], model,SPS,qty);
        printLabels(result[1], false);
        ShowSuccessfulInfo(true, "[" + model + "] " + msgSuccess);
        ResetValue();
        callNextInput();
    }
    function onError(result) {
        endWaitingCoverDiv();
        ResetValue();
        ShowMessage(result._message);
        ShowInfo(result._message);
        callNextInput();
    }
    function ResetValue() {
        model = "";
        qty = "";
        document.getElementById("txtModel").value = "";
    }
    function setPrintItemListParam(backPrintItemList,m,s,q) 
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        keyCollection[0] = "@Model";   ////＠Model,@SPS,@Qty
        valueCollection[0] = generateArray(m);
        keyCollection[1] = "@SPS";   ////＠Model,@SPS,@Qty
        valueCollection[1] = generateArray(s);
        keyCollection[2] = "@Qty";   ////＠Model,@SPS,@Qty
        valueCollection[2] = generateArray(q);
        setPrintParam(lstPrtItem, "CommonParts_Label", keyCollection, valueCollection);
    }
    function callNextInput() 
    {
        inputObj.focus();    
        getAvailableData("processDataEntry");
    }

    function ExitPage() 
    {

    }
    function clkSetting() {
        showPrintSetting(station, pcode);
    }
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:100px; vertical-align:middle" cellpadding="0" cellspacing="0">
          
            <tr>
            <td style="width:10%" >
              <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel" Text="Data Entry"></asp:Label>
            </td>
                <td align="left">
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                       CanUseKeyboard="true" IsPaste="true" MaxLength="50" IsClear="true"
                       InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
                </td>
               
            </tr>     
  
            <tr>
                <td align="left" style="width:10%" >
                    <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt" Text="Model"> </asp:Label>
                </td>
                <td align="left" >
                               <input type="text" ID="txtModel" class="iMes_textbox_input_Disabled"
                             MaxLength="20" style="width:98%" readonly="readonly"/>
                </td>
            </tr>     
                    
           

            <tr>
            <td>
            </td>
       
              <td  align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button" value="Print Setting"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
           
            </tr>
          
        
        </table>
        
        </center>
    </div>
    
</asp:Content>
