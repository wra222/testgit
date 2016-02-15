<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PickCard page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-27  zhu lei               Create          
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PickCard.aspx.cs" Inherits="PAK_PickCard" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PickCardService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width:120px;"/>
                    <col />
                    <col style="width:80px;"/>
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                         <asp:Label ID="lblDateContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblForwarder" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                         <asp:Label ID="lblForwarderContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDriver" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                         <asp:Label ID="lblDriverContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
                    </td>
                </tr>                                
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>                                
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>  
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var g_truckid;
        var station;
        var pCode;
        var editor;
        var customer;
        var emptyPattern = /^\s*$/;
        var inputObj;
        var emptyString = "";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            inputObj = getCommonInputObject();
            station = '<%=Request["Station"] %>'; //"12";
            pCode = '<%=Request["PCode"] %>'; // "OPPA052";
            getAvailableData("processFun");

            window.setTimeout(function() {
                PickCardService.Init(null, function(result) {
                    document.getElementById("<%=lblDateContent.ClientID %>").innerText = result[0];
                    window.dateStr = result[0];
                });
            },100);
        };
        
        function processFun(backData)
        {
            ShowInfo("");
            try
            {
               var lstPrintItem = getPrintItemCollection();
               if (lstPrintItem == null)                 
               {
                  alert(msgPrintSettingPara);
                  getAvailableData("processFun");
                  inputObj.focus(); 
               } 
               else {
                   g_truckid = backData;
                    PickCardService.inputTruckID(emptyString, backData, dateStr, editor, station, customer, lstPrintItem, onSucc, onFail);
               }
            }
            catch(e)
            {
                alert(e.description);
                getAvailableData("processFun");
                inputObj.focus(); 
            }              
        }
        
        function onSucc(result)
        {
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + g_truckid + "] " + msgSuccess;
            ShowSuccessfulInfo(true, message);
            
            //document.getElementById("<%=lblDateContent.ClientID %>").innerText = result[0].Date;
            document.getElementById("<%=lblForwarderContent.ClientID %>").innerText = result[0].Forwarder;
            document.getElementById("<%=lblDriverContent.ClientID %>").innerText = result[0].Driver;
            var lstPrtItem = result[1];
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@pickid";
            valueCollection[0] = generateArray(result[2]);
    
            setPrintParam(lstPrtItem, "Pick Card", keyCollection, valueCollection);
            printLabels(result[1], false);
            
            getAvailableData("processFun");
            inputObj.focus();
        }
        
        function onFail(result)
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            //document.getElementById("<%=lblDateContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblForwarderContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblDriverContent.ClientID %>").innerText = "";
            getAvailableData("processFun"); 
            endWaitingCoverDiv();
            inputObj.focus();
        }
        
        function clkSetting()
        {
            showPrintSetting(station, pCode);
        }
    </script>
</asp:Content>

