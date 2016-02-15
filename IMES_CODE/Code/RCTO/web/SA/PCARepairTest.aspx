<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PCARepairTest page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create
 * 2012-04-11  Li.Ming-Jun           ITC-1360-1669
 * Known issues:
 * TODO:
 * checkitems 中的WWANcheck未完成，由于与BOM有关还没有最终确定所以暂时缺少
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCARepairTest.aspx.cs" Inherits="SA_PCARepairTest" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServicePCARepairTest.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 15%;"/>
                    <col />
                    <col />
                    <col style="width: 11%;"/>
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMBSnoContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <input type="checkbox" id="chkMusic" runat="server" checked /><asp:Label ID="lblMusicOn" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>   

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFamilyContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModelContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td colspan="4">
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                            GvExtWidth="100%" GvExtHeight="220px" style="top: 0px; left: 0px" Width="99.9%" Height="210px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt> 
                    </td>
                </tr>  
             </table>                        
        </div>
        <div id="div3">
             <table width="100%">
                 <colgroup>
                    <col style="width: 15%;"/>
                    <col />
                    <col style="width: 20%;"/>
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                    <td>
                        <asp:CheckBox id="chk9999" runat="server" Checked="false"></asp:CheckBox>
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
        var DEFAULT_ROW_NUM = 7;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var mbSno = "";
        var customer;
        var stationId = "";
        var inputObj;
        var inputCode = "";
        
        //error message
        var msgSucc = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var msgInformation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInformation").ToString() %>';
        var msgOutputRepair = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgOutputRepair").ToString() %>';
        var msgReturnRepair = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReturnRepair").ToString() %>';
        var msgError9999 = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgError9999").ToString() %>';
        var msgError6666 = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgError6666").ToString() %>';
        var msgInputMBSno = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBSno").ToString() %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";

        window.onload = function()
        {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            inputObj.focus();
        };

        function input(data)
        {
       
            if (inputFlag) {
                if (data == "7777")
                {
                    ShowInfo("");
                    //clear table action
//                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
//                    setSrollByIndex(0 ,false);
//                    defectCount = 0;
//                    defectInTable = [];
                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    setSrollByIndex(0, false);
                    getAvailableData("input");
                }
                else if (data == "9999" || data == "6666")
                {
                    ShowInfo("");
                    //save action
                    save(data);
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
                    //beginWaitingCoverDiv();
                    mbSno = SubStringSN(data, "MB");
                    //mbSno = data;
                    WebServicePCARepairTest.input("", mbSno, editor, stationId, customer, inputSucc, inputFail);
                }
                else {
                    alert(msgInputMBSno);
                    //ShowInfo(msgInputMBSno);
                    getAvailableData("input");
                }
            }
        }

        function inputSucc(result) {
            if (result[0] == SUCCESSRET) {
                setInfo(mbSno, result);
                inputFlag = true;
                ShowInfo(msgInformation);
                if (document.getElementById("<%=chk9999.ClientID%>").checked) {
                    save("9999");
                }
            }
            else {
                //show error message
                if (isMusicOn()) {
                    ShowMessage(result[0], true);
                }
                else {
                    ShowMessage(result[0], false);
                }
                ShowInfo(result[0]);
                initPage();
            }
            getAvailableData("input");
            inputObj.focus();
        }
        
        function inputFail(result)
        {
            //show error message
            if (isMusicOn()) {
                ShowMessage(result.get_message());
            }
            else {
                ShowMessage(result.get_message(), false);
            }
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }
        
        function isMusicOn() {
            return document.getElementById("<%=chkMusic.ClientID %>").checked;
        }

        function isPass()
        {
            if (defectCount == 0)
            {
                return true;
            }
            
            return false;
        }
        
        function setInfo(prodId, info)
        {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), info[1].id);
            setInputOrSpanValue(document.getElementById("<%=lblFamilyContent.ClientID %>"), info[1].family);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[1]._111LevelId);
            //set defectCache value
            defectCache = info[2];
        }
        
        function save(scanCode)
        {
//            if (isPass())
//            {
//                //must input defect
//                alert(msgInputDefect);
//                getAvailableData("input");
//                inputObj.focus();
//            }
//            else
//            {
                beginWaitingCoverDiv();
                inputCode = scanCode;
                WebServicePCARepairTest.save(mbSno, defectInTable, scanCode, saveSucc, saveFail);                
//            }
        }
        
        function saveSucc(result) 
        {
            //show success message
            if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                var infoMsg = ""
                if (inputCode == "9999") {
                    if (defectInTable.length > 0) {
                        infoMsg = msgReturnRepair;
                    }
                    else {
                        infoMsg = msgOutputRepair;
                    }
                    if (isMusicOn()) {
                        ShowSuccessfulInfo(true, infoMsg);
                    }
                    else {
                        ShowSuccessfulInfo(false, infoMsg);
                    }
                }
                else {
                    if (isMusicOn()) {
                        ShowSuccessfulInfo(true, "[" + mbSno + "] " + msgSucc);
                    }
                    else {
                        ShowSuccessfulInfo(false, "[" + mbSno + "] " + msgSucc);
                    }
                }
                //initPage            
                initPage();
            }
            else if (result[0] == "ERROR") {
                endWaitingCoverDiv();
                if (inputCode == "9999") {
                    alert(msgError9999);
//                    if (isMusicOn()) {
//                        ShowMessage(msgError9999, true);
//                    }
//                    else {
//                        ShowMessage(msgError9999, false);
//                    }
//                    ShowInfo(msgError9999);
                }
                else {
                    alert(msgError6666);
//                    if (isMusicOn()) {
//                        ShowMessage(msgError6666, true);
//                    }
//                    else {
//                        ShowMessage(msgError6666, false);
//                    }
//                    ShowInfo(msgError6666);
                }
            }
            else {
                //show error message
                endWaitingCoverDiv();
                if (isMusicOn()) {
                    ShowMessage(result[0], true);
                }
                else {
                    ShowMessage(result[0], false);
                };
                ShowInfo(result[0]);
                initPage();
            }
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblFamilyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0 ,false);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
        }
        
        function saveFail(result)
        {
            //show error message
            endWaitingCoverDiv();
            if (isMusicOn()) {
                ShowMessage(result.get_message());
            }
            else {
                ShowMessage(result.get_message(), false);
            };
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
                    
                rowArray.push(data);
                rowArray.push(desc);
            
                //add data to table
                if (defectInTable.length < 6)
                {   
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    //setSrollByIndex(defectInTable.length, true, tbl);
                }
                else
                {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    //setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }
                
                setSrollByIndex(defectInTable.length, false);
                
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
            WebServicePCARepairTest.cancel(mbSno);
        }
        
        function ExitPage(){
            OnCancel();
        }
        
        function ResetPage(){
            ExitPage();
            initPage();
            ShowInfo("");
        }                  
       
    </script>
</asp:Content>

