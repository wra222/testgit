<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PACosmetic page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create          
 * Known issues:
 * TODO:
 * checkitems 中的WWANcheck未完成，由于与BOM有关还没有最终确定所以暂时缺少
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PACosmetic.aspx.cs" Inherits="PAK_PACosmetic" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePACosmetic.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 100px;"/>
                    <col />
                    <col style="width: 100px;"/>
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" />
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblPassQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblFailQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                     
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>  
             </table>
             <asp:Panel ID="Panel1" runat="server">
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;"/>
                        <col />
                        <col style="width: 100px;"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblCustomerSn" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerSnContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIdContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
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
                </table>
             </asp:Panel>
             <asp:Panel ID="Panel2" runat="server"> 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;"/>
                        <col />
                        <col style="width: 100px;"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblWWAN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblWWANContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPCID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPCIDContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
             </asp:Panel>
             <asp:Panel ID="Panel3" runat="server"> 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;"/>
                        <col />
                        <col style="width: 100px;"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                                GvExtWidth="100%" GvExtHeight="220px" style="top: 0px; left: 0px" Width="99.9%" Height="210px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt> 
                        </td>
                    </tr> 
                </table>
             </asp:Panel>                             
        </div>
        <div id="div3">
             <table width="100%">
                 <colgroup>
                    <col style="width: 120px;"/>
                    <col />
                    <col style="width: 150px;"/>
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
        var checkPicdFlag = false;
        var checkWwanFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var customer;
        var stationId;
        var inputObj;
        
        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelectPdLine").ToString() %>';
        var msgPcidCheck = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPcidCheck").ToString() %>'
        var msgWwanCheck = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWwanCheck").ToString() %>'
        var msgPcidError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPcidError").ToString() %>'
        var msgWwanError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWwanError").ToString() %>'
        var msgNeedCheck = 'Please check PCID / WWAN.';
        
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
            if (getPdLineCmbValue() == "") 
            {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("input");
                return;
            } 
        
            if (inputFlag) {
                    if (data == "7777")
                    {
                        ShowInfo("");
                        //clear table action
                        initPage();
                        OnCancel();                        
                        getAvailableData("input");
                    }
                    else if (data == "9999") {
                        // FIX BUG: ITC-1360-0643
                        if (checkPicdFlag || checkWwanFlag) {
                            alert(msgNeedCheck);
                            getAvailableData("input");
                            return;
                        }
                        
                        ShowInfo("");
                        //save action
                        save();
                    }
                    else if (checkPicdFlag || checkWwanFlag)
                    {
                        checkitems(data);
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
                    gprodId = SubStringSN(data, "CustSN");
                    WebServicePACosmetic.input(getPdLineCmbValue(), SubStringSN(data, "CustSN"), editor, stationId, customer, inputSucc, inputFail);
                }
                else
                {
                    getAvailableData("input");
                }
            }
        }
        
        function inputSucc(result)
        {
            setInfo(gprodId, result);
            inputFlag = true;
            if (result[3]) {
                checkPicdFlag = true;
                ShowInfo(msgPcidCheck);
            }
            else if (result[4]) {
                checkWwanFlag = true;
                ShowInfo(msgWwanCheck);
            }
            else {
                if (document.getElementById("<%=chk9999.ClientID%>").checked) {
                    save();
                }
            }
            if (result.length > 6)
                ShowMessage(result[6]);
                
            getAvailableData("input");
            getPdLineCmbObj().disabled = true;
            inputObj.focus();
        }
        
        function inputFail(result)
        {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function checkitems(data) 
        {
            WebServicePACosmetic.checkItem(getInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>")), data, checkPicdFlag, checkWwanFlag, checkItemSucc, checkItemFail);
        }

        function checkItemSucc(result) 
        {
            if (checkPicdFlag) {
                if (result[1]) {
                    alert(msgPcidError);
                }
                else {
                    ShowInfo("");
                    setInputOrSpanValue(document.getElementById("<%=lblPCIDContent.ClientID %>"), result[0]);
                    checkPicdFlag = false;
                    if (document.getElementById("<%=chk9999.ClientID%>").checked) {
                        save();
                    }
                }
            }
            else if (checkWwanFlag) {
                if (result[2]) {
                    alert(msgWwanError);
                }
                else {
                    ShowInfo("");
                    setInputOrSpanValue(document.getElementById("<%=lblWWANContent.ClientID %>"), result[0]);
                    checkWwanFlag = false;
                    if (document.getElementById("<%=chk9999.ClientID%>").checked) {
                        save();
                    }
                }
            }
            getAvailableData("input");
            inputObj.focus();
        }
        
        function checkItemFail(result)
        {
            //show error message
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
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
        
        function setInfo(prodId, info)
        {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>"),info[0]["customSN"]);
            setInputOrSpanValue(document.getElementById("<%=lblProductIdContent.ClientID %>"),info[0]["id"]);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"),info[0]["modelId"]);
            //set defectCache value
            defectCache = info[2];
            if (info[5] != "" && info[5] != null) {
                alert(info[5]);
            }
        }
        
        function save()
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
                //var prodId = document.getElementById("<%=lblProductIdContent.ClientID %>").innerText;

                WebServicePACosmetic.save(gprodId, defectInTable, saveSucc, saveFail);                
//            }
        }
        
        function saveSucc(result) 
        {
            //show success message
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + gprodId + "] " + msgSuccess;
            ShowSuccessfulInfo(true, message);
            
            //initPage
            if (isPass())
            {
                passQty++;
                setInputOrSpanValue(document.getElementById("<%=lblPassQtyContent.ClientID %>"), passQty);
            }
            else
            {
                failQty++;
                setInputOrSpanValue(document.getElementById("<%=lblFailQtyContent.ClientID %>"), failQty);
            }
            
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIdContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblPCIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblWWANContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0 ,false);
            inputFlag = false;
            checkPicdFlag = false;
            checkWwanFlag = false;
            defectCount = 0;
            defectInTable = [];
            getPdLineCmbObj().disabled = false;
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
            WebServicePACosmetic.cancel(gprodId);
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

