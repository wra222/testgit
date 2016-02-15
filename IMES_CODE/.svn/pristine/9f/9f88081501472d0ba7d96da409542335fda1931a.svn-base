 <%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Online Generate AST Reprint Page
 * UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/2/1 
 * UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/2/1             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-2-1   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0501, Jessica Liu, 2012-2-23
* ITC-1360-0503, Jessica Liu, 2012-2-28
* ITC-1360-0978, Jessica Liu, 2012-3-1
* ITC-1360-1613, Jessica Liu,2012-4-6
* ITC-1360-1775, Jessica Liu, 2012-5-2
*/ 
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="OnlineGenerateASTReprint.aspx.cs" Inherits="FA_OnlineGenerateASTReprint" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgNoProductID = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoProductID") %>';
    var msgNoPrinted = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrinted") %>';
    var SUCCESSRET ="<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var uutInput = true;
    var flag = false;
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgNoInputReason = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputReason").ToString()%>';
    var msgRemarkLength = '<%=this.GetLocalResourceObject(Pre + "_msgRemarkLength").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

    //2012-4-11
    var CustSNOrProdID = "";

    //ITC-1360-1775, Jessica Liu, 2012-5-2
    var msgMN2Error = '<%=this.GetLocalResourceObject(Pre + "_msgMN2Error").ToString() %>';
    var msgNoAST = '<%=this.GetLocalResourceObject(Pre + "_msgNoAST").ToString() %>';
    

    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;

            inputObj.focus();

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;

            //2012-5-2
            document.getElementById("<%=ShowImage.ClientID %>").src = "";

            getAvailableData("checkInfoAndPrint");

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }


    function generateArray(val) 
    {
        var ret = new Array();
        ret[0] = val;
        
        return ret;
    }


    function setPrintItemListParam(backPrintItemList, prodid)//ast)
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@productID";   //
        valueCollection[0] = generateArray(prodid);
        //keyCollection[0] = "@Ast";   //AST
        //valueCollection[0] = generateArray(ast);

        /*
        * Function Name: setPrintParam
        * @param: printItemCollection
        * @param: labelType
        * @param: keyCollection(Client: Array of string.    Server: List<string>)
        * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        */
        setPrintParam(lstPrtItem, "Asset Label", keyCollection, valueCollection);
    }


    function onSucceed(result) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
        
        try {
            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 5) && (result[0] == SUCCESSRET)) {
                setPrintItemListParam(result[1], result[2]);   
                             
                /*
                * Function Name: printLabels
                * @param: printItems
                * @param: isSerial
                */
                printLabels(result[1], false);

                reset();

                //ITC-1360-1775, Jessica Liu, 2012-5-2
                if (result[3] == "1") {
                    ShowInfo("");
                    ShowInfo(msgMN2Error);
                }
                else if (result[3] == "2") {
                    ShowInfo("");
                    ShowInfo(msgNoAST);
                }
                else if (result[3] == "0") {
                    var imageUrl = "";
                    var RDSServer = '<%=ConfigurationManager.AppSettings["RDS_Server"].Replace("\\", "\\\\")%>';
                    imageUrl = RDSServer + result[4] + ".JPG";
                    document.getElementById("<%=ShowImage.ClientID %>").src = imageUrl;
                }                    

                //2012-4-11
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + CustSNOrProdID + "] " + msgSuccess); 

                //ITC-1360-0978, Jessica Liu, 2012-3-1
                callNextInput();
                
            }
            else 
            {
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);

            }
            
        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

        flag = false;
    }

    
    function onFail(error) 
    {
        ShowInfo("");
        endWaitingCoverDiv();

        try {
            reset();
            
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            //inputObj.focus();

            callNextInput();

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

        flag = false;
    }


    function reset() 
    {
        inputObj.value = "";

        //ITC-1360-0503, Jessica Liu, 2012-2-28

        inputObj.focus();
    }


    function checkCustomerSNValid(strCustomerSN) {
        var strNoEmptySN = strCustomerSN.trim();
        var ret = false;   

        /*
        if (strNoEmptySN.length == 10) {
            if (strNoEmptySN.substring(0, 2) == "CN") {
                ret = true;
            }
        }
        else if (strNoEmptySN.length == 11) {
            if (strNoEmptySN.substring(0, 3) == "SCN") {
                ret = true;
            }
        }
        */
        ret = isProdIDorCustSN(strCustomerSN, "");

        return ret;

    }

    function CustomerSNEnterOrTab() 
    {
        if (event.keyCode == 9 || event.keyCode == 13) {
            getAvailableData("checkInfoAndPrint");
        }
    }

    function checkInfoAndPrint() 
    {
        //2012-5-2
        document.getElementById("<%=ShowImage.ClientID %>").src = "";
        
        try {
            var errorFlag = false;
            var reason = document.getElementById("lblReasonInfo").value;
            inputData = inputObj.value; 
            
            if (inputData == "") 
            {
                errorFlag = true;
                alert(msgNoInputCustomerSn);
                inputObj.focus();    
                
                getAvailableData("checkInfoAndPrint");
            }
            else if (checkCustomerSNValid(inputData) == false)  
            {
                errorFlag = true;
                alert(msgDataEntryField);
                inputObj.focus();    
                
                getAvailableData("checkInfoAndPrint");
            }
            /* ITC-1360-1613, Jessica Liu,2012-4-6
            else if (reason == "") 
            {
                errorFlag = true;
                alert(msgNoInputReason);
                document.getElementById("lblReasonInfo").select();

                getAvailableData("checkInfoAndPrint");
            }
            */
            else if (reason.length > 80) 
            {
                errorFlag = true;
                alert(msgRemarkLength);
                document.getElementById("lblReasonInfo").select();

                getAvailableData("checkInfoAndPrint");
            }

            if (!errorFlag) 
            {
                var lstPrintItem = getPrintItemCollection();

// * test，2011-10-18，需放开==========               
                if (lstPrintItem == null) 
                {
                    alert(msgPrintSettingPara);
                        
                    //2011-10-13去掉及新增                    
                    //等待快速控件继续输入
                    //callNextInput();

                    inputObj.focus();               
                }
                else 
                {
//test，2011-10-18，需放开==========* /
                    flag = true;

                    beginWaitingCoverDiv();
                    
                    //ITC-1360-0501, Jessica Liu, 2012-2-23
                    var dataEntry = inputObj.value;  
                    if (dataEntry.length == 10) 
                    {
                        if (dataEntry.substring(0, 2) != "CN") //prodid
                        {
                            dataEntry = SubStringSN(dataEntry, "ProdId");
                        }
                    }

                    CustSNOrProdID = dataEntry;

                    //调用web service提供的打印接口
                    WebServiceOnlineGenerateAST.reprint(dataEntry, "", "", "<%=UserId%>", "<%=Customer%>", reason, lstPrintItem, onSucceed, onFail);
//test，2011-10-18，需放开==========
                }
            }
        } catch (e) {
            endWaitingCoverDiv();
        
            alert(e.description);

            inputObj.focus();
        }
    }

    function callNextInput() {
        inputObj.focus();

        getAvailableData("checkInfoAndPrint");
    }

    function showPrintSettingDialog() 
    {     
        showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value); 
    }


    String.prototype.len = function() {
        return this.replace(/[^\x00-\xff]/g, "**").length;
    }

    function setMaxLength(object, length) {
        var result = true;
        var controlid = document.selection.createRange().parentElement().id;
        var controlValue = document.selection.createRange().text;
        if (controlid == object.id && controlValue != "") {
            result = true;
        }
        else if (object.value.len() >= length) {
            result = false;
        }
        if (window.event) {
            window.event.returnValue = result;
            return result;
        }
    }

    function limitPaste(object, length) {
        var tempLength = 0;
        if (document.selection) {
            if (document.selection.createRange().parentElement().id == object.id) {
                tempLength = document.selection.createRange().text.len();
            }
        }
        var tempValue = window.clipboardData.getData("Text");
        tempLength = object.value.len() + tempValue.len() - tempLength;
        if (tempLength > length) {
            tempLength -= length;

            var tt = "";
            for (var i = 0; i < tempValue.len() - tempLength; i++) {
                if (tt.len() < (tempValue.len() - tempLength))
                    tt = tempValue.substr(0, i + 1);
                else
                    break;
            }
            tempValue = tt;
            window.clipboardData.setData("Text", tempValue);
        }

        window.event.returnValue = true;
    } 

    
    function ExitPage() 
    {
        if (flag)
        {
            flag = false; 
        }
    }


    
    function ResetPage() 
    {
        ExitPage();

        //resetAll();
        reset();
        document.getElementById("lblReasonInfo").value = ""; 
    }

    
    window.onbeforeunload = function() 
    {
        ExitPage();
    }
    
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceOnlineGenerateAST.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
            <tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>  
            <tr>
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td colspan="9" align="left" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50" />
                        </ContentTemplate>                           
                    </asp:UpdatePanel>
                </td>     
            </tr>
            
            <tr><td colspan="10">&nbsp;</td></tr>
          
            <tr>
                <td align="left" >
                    <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan ="9" align="left">
                    <textarea id="lblReasonInfo" style="width: 98%; height: 100px" onkeypress="setMaxLength(this,80);" onpaste="limitPaste(this, 80)"></textarea>
                </td>
            </tr>
            
            <tr><td colspan="10">&nbsp;</td></tr>
                    
            <tr>
                <td colspan="9" rowspan="6">
                    <asp:Image ID="ShowImage" runat="server" Width="550" Height="400"/>
                </td>
                <td align="right">   
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                        
                            <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/>                            
                        </ContentTemplate>  
                    </asp:UpdatePanel>
                    <tr>
                        <td align="right">   
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                        
                                    <input id="btnReprint" style="height:auto" type="button"  runat="server" 
                                    onclick="checkInfoAndPrint()" class="iMes_button" 
                                    onmouseover="this.className='iMes_button_onmouseover'" 
                                    onmouseout="this.className='iMes_button_onmouseout'"/>                            
                                </ContentTemplate>  
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </td>
            </tr>
            <tr><td colspan="10">&nbsp;</td></tr>
            <tr><td colspan="10">&nbsp;</td></tr>
            
            
            
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <button id="btnHidden" runat="server" style="display: none" >
                            </button>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            </button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        
        </table>
        
    </center>
</div>
    
</asp:Content>
