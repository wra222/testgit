
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OnlineProductIDHold.aspx.cs" Inherits="PAK_OnlineProductIDHold" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceReleaseProductIDHold.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                 <tr>
                        <td>
                            <asp:Label ID="lblCustomerSn" runat="server" CssClass="iMes_label_13pt">CustSN:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerSnContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductId" runat="server" CssClass="iMes_label_13pt">ProductID:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIdContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                   </tr>
            </table>
            <hr />
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td  style="width:120px">
                        <asp:Label ID="lblReleaseDefect" runat="server" CssClass="iMes_label_13pt">Defect:</asp:Label>
                    </td>
                    <td>
                        <select id="cmbReleaseDefect" style="width:98%" runat="server"></select>
                    </td>
                    
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblReleaseAction" runat="server" CssClass="iMes_label_13pt">Action:</asp:Label>
                    </td>
                    <td>
                        <select id="cmbReleaseAction" style="width:98%" runat="server" >
                            <option text="ReleaseHoldStation" value="ReleaseHoldStation"></option>
                        </select>
                    </td>
                   
                    
                </tr>
                
            </table>
                
            <asp:Panel ID="Panel3" runat="server" > 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td >
                          
                            
                        </td>
                    </tr> 
                   
                </table>
                 <table width="100%">
                 <colgroup>
                    <col style="width: 120px;"/>
                    <col />
                    <col style="width: 150px;"/>
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_DataEntryLabel">Data Entry: </asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                    
                </tr>
             </table>   
             </asp:Panel>              
        </div>
      
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input id="TypeValue" type="hidden" runat="server" />
                <input id="HoldStation" type="hidden" runat="server" />
                <input id="hidGuidCode" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidProList" runat="server" />
        <input id="hidQueryValue" type="hidden" runat="server" />
        <input id="hideditor" type="hidden" runat="server" />
        <input id="hidcustomer" type="hidden" runat="server" />
        <input id="hidstationId" type="hidden" runat="server" />
        <input id="hidIsCUSTSNValue" type="hidden" runat="server" />
        <input id="hidHoldStationList" type="hidden" runat="server" />

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
        var line="";
        var qcMethod;
        var showMsgList = "";
        //var HoldStationList;
        
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
        
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgToEPIA = '<%=this.GetLocalResourceObject(Pre + "_msgToEPIA").ToString()%>';
        var msgNQC = '<%=this.GetLocalResourceObject(Pre + "_msgNQC").ToString()%>';

        window.onload = function() {
          
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            document.getElementById("<%=hidHoldStationList.ClientID %>").value = "<%=HoldStationValue%>";
            getcmbReleaseDefect();
            inputObj = getCommonInputObject();
            inputObj.focus();
            getAvailableData("input");
        };
        function input(data) {
           
                var releaseCode = getcmbReleaseDefectObj().options[getcmbReleaseDefectObj().selectedIndex].value;
                if (releaseCode == "") {
                    alert("請選擇Defect...");
                    getAvailableData("input");
                    return;
                }
                var releaseAction = getcmbReleaseActionObj().options[getcmbReleaseActionObj().selectedIndex].value;
                if (releaseAction == "") {
                    alert("請選擇HoldStation...");
                    getAvailableData("input");
                    return;
                }
                if (isProdIDorCustSN(data.trim(),"")) {
                    save(data.trim());
                    inputObj.focus();
                   
                }
                else {
                    alert("Please Input CustSN or Prdid!!");
                    getAvailableData("input");

                }
            

            getAvailableData("input");
            inputObj.focus();
        }
        
        function getcmbReleaseDefectObj() {
            return document.getElementById("<%=cmbReleaseDefect.ClientID %>");
        }
        
        function getcmbReleaseDefect() {
            WebServiceReleaseProductIDHold.GetReleaseDefect(oncmbReleaseDefectSucceeded, oncmbReleaseDefectFailed);
        }

        function oncmbReleaseDefectSucceeded(result) {
            try {
                //DealHideWait();
                if (result == null) {
                    endWaitingCoverDiv();
                    alert(msgSystemError);
                }
                else if ((result.length == 2) && (result[0] == "SUCCESSRET")) {
                    var o = null;
                    var item = result[1];
                    getcmbReleaseDefectObj().length = 0;
                    getcmbReleaseDefectObj().options.add(firstoption());
                    for (var i in item) {
                        o = document.createElement('option');
                        o.text = item[i];
                        o.value = item[i].split('-')[0].trim();
                        getcmbReleaseDefectObj().add(o);
                    }
                }
                else {
                    //DealHideWait();
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                }
            }
            catch (e) {
                //DealHideWait();
                alert(e.description);
            }
        }

        function oncmbReleaseDefectFailed(error) {
            try {
                //DealHideWait();
                getcmbReleaseDefectObj().options.add(firstoption());
                //ClearData();
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());

                //getAvailableData("processDataEntry");
            }
            catch (e) {
                //DealHideWait();
                alert(e.description);
            }
        }

        function firstoption() {
            var o = document.createElement('option');
            o.text = '';
            o.value = "";
            return o;
        }


       

        function getcmbReleaseActionObj() {
            return document.getElementById("<%=cmbReleaseAction.ClientID %>");
        }

      

        function setInfo(prodId, info)
        {
            //set defectCache value
            defectCache = info[2];
            if (info[5] != "" && info[5] != null) {
                alert(info[5]);
            }
        }

        function save(prdid) {

            var releaseCode = getcmbReleaseDefectObj().options[getcmbReleaseDefectObj().selectedIndex].value;
            if (releaseCode == "") {
                alert("請選擇ReleaseDefect...");
                return;
            }
            var goToStation = "";
            gprodId = prdid;
             beginWaitingCoverDiv();
             WebServiceReleaseProductIDHold.OfflineHoldSave(prdid, stationId, editor, customer, releaseCode, goToStation, onSaveSucceeded, onSaveFailed);

        }
        function onSaveSucceeded(result) 
        {
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + gprodId + "] " + msgSuccess;
            ShowSuccessfulInfo(true, message, false);
            initPage();
            inputObj.focus();
        }

        function onSaveFailed(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            initPage();
        }
        
        function initPage()
        {
          
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            document.getElementById("<%=hidProList.ClientID %>").value = "";
            document.getElementById("<%=hidGuidCode.ClientID %>").value = "";
            gprodId = "";
           
        }

        window.onbeforeunload = function()
        {
            if (inputFlag)
            {
                OnCancel();
            }
        };
        function modelandcustsnList(modelList, CUSTSNList) {
            if (document.getElementById("<%=hidIsCUSTSNValue.ClientID %>").value == "Y") {
                showMsgList = CUSTSNList;
            }
            else {
                showMsgList = gprodId;
            }
        }
        function OnCancel()
        {
            WebServiceReleaseProductIDHold.cancel(document.getElementById("<%=hidGuidCode.ClientID %>").value);
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

