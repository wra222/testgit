<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PackingPizzaLabelReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-14  zhu lei               Create          
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */ --%>
 <%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PackingPizzaLabelReprint.aspx.cs" Inherits="PAK_PackingPizzaLabelReprint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PackingPizzaWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width:110px;"/>
                    <col />
                    <col style="width:120px;"/>
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>    
                    </td>
                    <td colspan="2">
                        <textarea id="txtReason" rows="5" style="width:98%;" 
                        runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                        onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="1"></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPizzaID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtPizzaID" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="95%" IsClear="true" IsPaste="true" CssClass="iMes_textbox_input_Yellow" />
                    </td>
                    <td>
                        <button id="btnPrintSetting" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkSetting()" tabindex="3"></button>
                    </td>  
                </tr>

                <tr>
                    <td></td>
                    <td align="center">
                        <button id="btnReprint" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkReprint()" tabindex="2"></button>
                    </td> 
                    <td></td>            
                </tr>                                
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel> 
        </div>
    </div>      
    
    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var station;
        var inputObj;
        var pCode;
        var kitID;
        var emptyPattern = /^\s*$/;
        
        var msgPrintSettingPara= '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>'; 
        var msgReasonNull='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
        var msgInputMaxLength1='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
        var msgInputMaxLength2='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
        var msgKitIDNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgKitIDNull").ToString()%>';
        
        window.onload = function()
        {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj = getCommonInputObject();
            getAvailableData("processFun");
        }
        
        function clkSetting()
        {
            showPrintSetting(station, pCode);
        }

        function processFun(backData) 
        {
            clkReprint();
        }

        function clkReprint() 
        {
            ShowInfo("");
            kitID = inputObj.value.trim();
            if (kitID != "") {
                var strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

                if (emptyPattern.test(strReason)) {
                    alert(msgReasonNull);
                    getAvailableData("processFun");
                    document.getElementById("<%=txtReason.ClientID %>").focus();
                    return;
                }

                beginWaitingCoverDiv();

                try {
                    var printItemlist = getPrintItemCollection();

                    if (printItemlist == null) {
                        alert(msgPrintSettingPara);
                        getAvailableData("processFun");
                        endWaitingCoverDiv();
                        getCommonInputObject().focus();
                        return;
                    }

                    PackingPizzaWebService.reprintPizzaLabel(kitID, editor, customer, station, strReason, printItemlist, printSucc, printFail);
                }
                catch (e) {
                    getAvailableData("processFun");
                    endWaitingCoverDiv();
                    getCommonInputObject().focus();
                    alert(e);
                }
            }
            else {
                alert(msgKitIDNull);
                getAvailableData("processFun");
                endWaitingCoverDiv();
                getCommonInputObject().focus();
            }          
        }
        
        function printSucc(result)
        {
            setPrintItemListParam(result[1], result[0]); 
            printLabels(result[1], false);
            endWaitingCoverDiv();
            ShowSuccessfulInfo(true);
            initPage();
        }

        function setPrintItemListParam(backPrintItemList, pizzaID)
        {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@Kit ID";

            valueCollection[0] = generateArray(pizzaID);

            setPrintParam(lstPrtItem, "Packing Pizza Label Reprint", keyCollection, valueCollection);
        } 
        
        function printFail(result)
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            endWaitingCoverDiv();
            initPage();            
        }
        
        function imposeMaxLength(obj)
        {  
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            return (obj.value.length <mlength);
        }        
        
        function ismaxlength(obj)
        {	
            var mlength=obj.getAttribute?parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength)
        	{  
        	    alert (msgInputMaxLength1 + mlength + msgInputMaxLength2); 
        	    obj.value = obj.value.substring(0, mlength);
        	    reasonFocus();	
        	}
        }
        

        function Tab(reasonPara)
        {
            if (event.keyCode == 9)
            {
               getCommonInputObject().focus();
               event.returnValue=false; 
            }
        }                
        
        function reasonFocus()
        {
            document.getElementById("<%=txtReason.ClientID %>").focus();
        }        
        
        function initPage()
        {
            clearData();
            kitID = "";
            getCommonInputObject().value = "";
            getAvailableData("processFun"); 
            getCommonInputObject().focus();
        }       
    </script>  
</asp:Content>

