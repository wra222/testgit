﻿<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PIAOutput page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PIAOutput.aspx.cs" Inherits="FA_PIAOutput" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServicePIAOutput.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="95%" border="0">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                     <td style="width: 10%;">
                        <asp:Label ID="lblPdLineContent" runat="server"  CssClass="iMes_label_11pt" ></asp:Label>
                    </td>
                  
                     <td style="width: 20%;">
                    </td>  
              
                     <td colspan="4" align ="left" style="width: 40%;">
                         <input id="btnOK" type="button" runat="server" class="iMes_button" onclick="showChangePia()" style="width:150px"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        <input id="btnMVunpack" type="button" runat="server" class="iMes_button" onclick="showMVunpack()" style="width:150px"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        <input type="hidden" runat="server" id="pCode" />
                    </td>                    
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblModelContent" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                    </td> 
                    <td style="width: 15%;">
                        <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td style="width: 15%;">
                        <asp:Label ID="lblPassQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>  
                     <td style="width: 15%;">
                        <asp:Label ID="lblPassQty1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td style="width: 15%;">
                        <asp:Label ID="lblPassQtyContent1" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                                      
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblProdIdContent" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                    </td> 
                    <td style="width: 15%;">
                        <asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td style="width: 15%;">
                        <asp:Label ID="lblFailQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>    
                    <td style="width: 15%;">
                        <asp:Label ID="lblFailQty1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td style="width: 15%;">
                        <asp:Label ID="lblFailQtyContent1" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                                  
                </tr>                
             </table>
        </div>
        <hr />
        <table width="100%">
            <tr>
                <td style="width: 55%;">
                    <asp:Label ID="lblInputDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width: 45%;">
                    <asp:Label ID="lblSupportDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        GvExtWidth="96%" GvExtHeight="198px" style="top: 0px; left: 0px" Width="96%" Height="190px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                    </iMES:GridViewExt> 
                </td>
                <td>
                    <asp:ListBox ID="lbDefectList" runat="server" Width="100%" Height="200px"></asp:ListBox>
                </td>
            </tr>
        </table>


        <div id="div3">
             <table width="100%">
                <tr>
                    <td style="width: 110px;">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
             </table>            
        </div> 
        <div id="div4">
             <table width="95%">
                <tr>
                    <td style="width: 300px;">
                        <asp:CheckBox id="gdCheckBox" runat="server"></asp:CheckBox>
                    </td>
                    
                </tr>
             </table>            
        </div> 
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>       
    </div>    
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var passQty1 = 0;
        var failQty1 = 0;
        var gprodId = "";
        var customer;
        var stationId;
        var inputObj;
        var preStation = "";
        var qcStatus = "";
        var custsn = "";
        var scanFlag = false;
        var isEPia = false;
        var isInputDefet = false;
        
        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        
        window.onload = function()
        {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            document.getElementById("<%=btnOK.ClientID %>").disabled = true;
            isEPia = false;
            //stationId = "79"; 
        };
        
        function input(data) {
            if (inputFlag)
            {
                if (data == "7777")
                {
                    ShowInfo("");
                    //clear table action
                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    defectCount = 0;
                    isInputDefet = false;
                    defectInTable = [];
                    getAvailableData("input");
                }
                else if (data == "9999")
                {
                    ShowInfo("");
                    //save action
                    save();
                }
                else
                {
                    ShowInfo("");
                    //input defect info
                    inputDefect(data);
                }               
            }
            else
            {
                if (data != "9999" && data != "7777")
                {
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    //custsn = SubStringSN(data, "ProdId");
                    data=Get2DCodeCustSN(data);
					custsn = data;
                    WebServicePIAOutput.input("", data, editor, stationId, customer, inputSucc, inputFail);
                    //setInfo1(data);
                }
                else
                {
                    getAvailableData("input");
                }
            }
        }
        
        function inputSucc(result)
        {
            setInfo(custsn, result); 
            endWaitingCoverDiv();

            scanFlag = document.getElementById("<%=gdCheckBox.ClientID%>").checked;
            if (scanFlag) {
                save();
                inputFlag = false;
            }
            else {
                inputFlag = true;
            }
            getAvailableData("input");
            inputObj.focus();
        }
        
        function inputFail(result)
        {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            endWaitingCoverDiv();
            inputFlag = false;
            getAvailableData("input");
            inputObj.focus();
        }
        
        function isPass()
        {
            if (defectCount == 0)
            {
                return true;
            }
            
            return false;
        }
       
        function setInfo(custsn, info)
        {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblProdIdContent.ClientID %>"), info[1]["productId"]); //Dean 20110312
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[0]["modelId"]);
            setInputOrSpanValue(document.getElementById("<%=lblPdLineContent.ClientID %>"), info[1]["pdLine"]);            
            
            //set defectCache value
            defectCache = info[2];
            preStation = info[3];
            qcStatus = info[4];
            
            //document.getElementById("<%=btnOK.ClientID %>").disabled = false;
            if (qcStatus == "2") {  //epia =2
                document.getElementById("<%=btnOK.ClientID %>").disabled = false;
                isEPia = true;
            }
            else {
                isEPia = false;
            }
          
        }
        
        function save()
        {
            if (isEqualPreStation() && isPass())
            {
                //must input defect
                ShowMessage(msgInputDefect);
                ShowInfo(msgInputDefect);
                getAvailableData("input");
                inputObj.focus();
            }
            else if (!checkPrestation())
            {
                //ShowMessage(msgPrestationError);
                ShowInfo(msgPrestationError);
                OnCancel();
                initPage();
                getAvailableData("input");
                inputObj.focus();
            }
            else
            {
                beginWaitingCoverDiv();
                //var prodId = document.getElementById("<%=lblProdIdContent.ClientID %>").innerText;

                WebServicePIAOutput.save(custsn, defectInTable,isInputDefet,qcStatus, saveSucc, saveFail);                
            }
        }
        
        function saveSucc(result) 
        {
            //show success message
            endWaitingCoverDiv();
            //ShowSuccessfulInfo(true);

            //ShowSuccessfulInfo(true, "'" + custsn + "' " + msgSuccess1);
            ShowSuccessfulInfoFormat(true, "Product ID", custsn); // Print 成功，带成功提示音！
            
            //initPage
            if (qcStatus != "1") {
               
                    if (isPass()) {
                        if (isEPia) {
                            passQty1++;
                            setInputOrSpanValue(document.getElementById("<%=lblPassQtyContent1.ClientID %>"), passQty1);
                        }
                        else {
                            passQty++;
                            setInputOrSpanValue(document.getElementById("<%=lblPassQtyContent.ClientID %>"), passQty);
                        }
                    }
                    else {
                        if (isEPia) {
                            failQty1++;
                            setInputOrSpanValue(document.getElementById("<%=lblFailQtyContent1.ClientID %>"), failQty1);
                        }
                        else {
                            failQty++;
                            setInputOrSpanValue(document.getElementById("<%=lblFailQtyContent.ClientID %>"), failQty);
                        }
                    }
             
            }
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblProdIdContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblPdLineContent.ClientID %>"), "");
            
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            isInputDefet = false;
            defectCount = 0;
            defectInTable = [];
            preStation = "";
            qcStatus = "";
            isEPia = false;
        }
        
        function saveFail(result)
        {
            //show error message
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
            inputObj.focus();
        }
        
        function inputDefect(data)
        {
            if (isExistInTable(data))
            {
                //error message
                alert(msgDuplicateData);
                getAvailableData("input");
                inputObj.focus();
                return;
            }
            
            if (isExistInCache(data))
            {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;
                isInputDefet = true;    
                rowArray.push(data);
                rowArray.push(desc);
            
                //add data to table
                if (defectInTable.length < 12)
                {   
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                }
                else
                {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                
                defectInTable[defectCount++] = data;
                getAvailableData("input");  
            }
            else 
            {
                alert(msgInputValidDefect);
                getAvailableData("input");
                inputObj.focus();
            }
        }
        
        function isEqualPreStation() {
            return ((qcStatus == "3" || qcStatus == "6"));
            //return ((preStation == stationId) && (qcStatus == "3" || qcStatus == "6"));
        }
        
        function checkPrestation()
        {
           /* if (preStation != "72" && preStation != "73")// Dean 20110420  OQC input/免檢站
            {
                return false;
            }*/
            
            return true;
        }
        
        function isExistInTable(data)
        {
            if (defectInTable != undefined && defectInTable != null)
            {
                for (var i = 0; i < defectInTable.length; i++)
                {
                    if (defectInTable[i] == data)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        function isExistInCache(data)
        {
            if (defectCache != undefined && defectCache != null)
            {
                for (var i = 0; i < defectCache.length; i++)
                {
                    if (defectCache[i]["id"] == data)
                    {
                        return true;
                    }
                }
            }
            
            return false;               
        }
        
        function getDesc(data)
        {
            if (defectCache != undefined && defectCache != null)
            {
                for (var i = 0; i < defectCache.length; i++)
                {
                    if (defectCache[i]["id"] == data)
                    {
                        return defectCache[i]["description"];
                    }
                }
            }
            
            return "";
        }
        
        window.onbeforeunload = function()
        {
            if (inputFlag)
            {
                OnCancel();
            }
        };   
        
        function OnCancel()
        {
            WebServicePIAOutput.cancel(custsn); // 由gprodId改成custsn
        }
        
        function ExitPage(){
            OnCancel();
        }
        
        function ResetPage(){
            ExitPage();
            initPage();
            ShowInfo("");
        }
        function showChangePia() {

            var url = "ChangeToPIAEPIA.aspx?Station=7A&PCode=OPFA024&UserId=admin&Customer=HP&UserName=admin&AccountId=8406&Login=admin";
            window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
            //showModalDialog
        }

        function showMVunpack() {

            var url = "MVunpack.aspx?Station=MV&PCode=OPFA024&Login=admin&AccountId=8406&UserName=admin&UserId=" + editor + "&Customer=" +customer;
            window.showModalDialog(url, "", 'dialogWidth:550px;dialogHeight:300px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }
    </script>
</asp:Content>
