<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:AoiOfflineKbCheck page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create          
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AoiOfflineKbCheck.aspx.cs" Inherits="PAK_AoiOfflineKbCheck" Title="AoiOfflineKbCheck" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceAoiOfflineKbCheck.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0" style="table-layout: fixed;">
                
                <tr>
                    <td style="width:10%">
                        <asp:Label ID="lblReason" runat="server" Text="Reason:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                   
                        <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server" Width="100" IsPercentage="true"  />
                    </td>
                </tr> 
            
                <tr>
                       <td style="width:10%">
                            <asp:Label ID="lblCustomerSn" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerSnContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td style="width:10%">
                            <asp:Label ID="lblProductId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIdContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                  
                </tr>  
             </table>
            
             <asp:Panel ID="Panel3" runat="server"> 
                <table width="100%" border="0" style="table-layout: fixed;">
                   
                    <tr>
                        <td>
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
                        &nbsp;</td>
                </tr>
             </table>   
        </div> 
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>
        
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
       
            <ContentTemplate>
                <input id="TypeValue" type="hidden" runat="server" />
                <input id="LimitSpeed" type="hidden" runat="server" />
                <input id="HoldStation" type="hidden" runat="server" />
                <input id="SpeedExpression" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
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
        var custsn = "";
        var customer;
        var stationId;
        var inputObj;
        var line="";
        var qcMethod;
        
        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
       var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
         
        

        
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
  
            if (inputFlag) 
            {
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
                //getConstValueTypeCmbText
                if (defectCount == 0 && getConstValueTypeCmbText() == "") {
                    alert("Please select reason!")
                    CallNextInput();
                    return;
                }
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
                    if (data.length == 11)
                    {
                        data = data.substr(1, 10);
                    }
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    custsn = SubStringSN(data, "CustSN");

                    WebServiceAoiOfflineKbCheck.input("", SubStringSN(data, "CustSN"), editor, stationId, customer, inputSucc, inputFail);
                }
                else
                {
                    getAvailableData("input");
                }
            }
        }
        
        function inputSucc(result) {
            endWaitingCoverDiv();
            setInfo(custsn, result);
            ShowInfo("Please scan 9999 or defect code!", "green");
            getAvailableData("input");
            inputFlag = true;
            CallNextInput();

        }

        function inputFail(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            CallNextInput();
        }
        function CallNextInput() {
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
        
        function setInfo(prodId, info)
        {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>"), prodId);
            setInputOrSpanValue(document.getElementById("<%=lblProductIdContent.ClientID %>"),info.ProductID);
       
            //set defectCache value
            defectCache = info.DefectList;

        }

        function save() {
            beginWaitingCoverDiv();
            WebServiceAoiOfflineKbCheck.save(custsn, defectInTable,getConstValueTypeCmbText() ,saveSucc, saveFail);
        }

        function saveSucc(result) 
        {
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + custsn + "] " + msgSuccess;
            ShowSuccessfulInfo(true, "[" + custsn + "] " + msgSuccess);
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIdContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0 ,false);
            inputFlag = false;
       
            defectCount = 0;
            defectInTable = [];
       //     getPdLineCmbObj().disabled = false;
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
            if (custsn!='')
            {
                OnCancel();
            }
        };   
        
        function OnCancel()
        {
            WebServiceAoiOfflineKbCheck.cancel(custsn);
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

