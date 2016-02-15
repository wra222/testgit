
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReleaseProductIDHold.aspx.cs" Inherits="PAK_ReleaseProductIDHold" Title="无标题页" %>
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
                    <td style="width:120px">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt">Model:</asp:Label>
                    </td>
                    <td  style="width:150px">
                        <input type="text" id="txtModel" runat="server"  style="width:98%" />
                    </td>
                    <td style="width:120px">
                        <asp:Label ID="lblCUSTSN" runat="server" CssClass="iMes_label_13pt">CUSTSN:</asp:Label>
                    </td>
                    <td style="width:400px">
                        <asp:TextBox ID="txtCUSTSN" runat="server" Height="19px" Width="200px"></asp:TextBox>
                        <input id="BtnBrowse" type="button" value="Input"  onclick="UploadDNList()" 
                        class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                    <td>
                        <input type="button" id="btnQuery" runat="server" value="Query"  onclick="if(checkQuery())" onserverclick="btnQuery_ServerClick" 
                            class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td  style="width:120px">
                        <asp:Label ID="lblReleaseDefect" runat="server" CssClass="iMes_label_13pt">Release Defect:</asp:Label>
                    </td>
                    <td>
                        <select id="cmbReleaseDefect" style="width:98%" runat="server"></select>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblReleaseAction" runat="server" CssClass="iMes_label_13pt">Release Action:</asp:Label>
                    </td>
                    <td>
                        <select id="cmbReleaseAction" style="width:98%" runat="server" onchange="cmbReleaseActionChange();">
                            <option text="ReleaseHoldStation" value="ReleaseHoldStation"></option>
                            <option text="GoToStation" value="GoToStation"></option>
                        </select>
                    </td>
                    <td style="width:120px" align="right">
                        <asp:Label ID="lblGotoStation" runat="server" CssClass="iMes_label_13pt" >Go To Station:</asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="cmbGotoStation" runat="server" Enabled="false" Width="98%" onchange="cmbGoToStationChange();">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input type="button" id="btnRelease" runat="server" value="Release" onclick="save()"
                        class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="2"></td>
                    <td style="width:120px" align="right">
                        <asp:Label ID="lblUnpackStation" runat="server" CssClass="iMes_label_13pt" >UnPack Station:</asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblUnpackStationValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td></td>
                </tr>
            </table>
                
            <asp:Panel ID="Panel3" runat="server" > 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gd" runat="server" 
                                        AutoGenerateColumns="true" 
                                        GvExtWidth="100%" GvExtHeight="220px" 
                                        style="top: 0px; left: 0px" Width="99.9%" Height="210px" 
                                        SetTemplateValueEnable="False" 
                                        HighLightRowPosition="3" 
                                        AutoHighlightScrollByValue="True">
                                    </iMES:GridViewExt> 
                                </ContentTemplate>
                                <Triggers>
                                    
                                </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr> 
                </table>
             </asp:Panel>              
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
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
            tbl = "<%=gd.ClientID %>";
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            document.getElementById("<%=hidHoldStationList.ClientID %>").value = "<%=HoldStationValue%>";
            getcmbReleaseDefect();
        };

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

        function UploadDNList() {
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "InputProductlist.aspx?List=" + document.getElementById("<%=hidProList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {

                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hidProList.ClientID %>").value = RemoveBlank(dlgReturn);
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=hidProList.ClientID %>").value = ""; }
                return;
            }
        }

        function RemoveBlank(List) {
            var arr = List.split(",");
            var proList = "";
            if (List != "") {
                for (var m in arr) {
                    if (arr[m] != "") {
                        proList = proList + arr[m] + ",";
                    }
                }
                proList = proList.substring(0, proList.length - 1)
            }
            return proList;
        }

        function checkQuery() {
            if (document.getElementById("<%=txtModel.ClientID %>").value == "" && document.getElementById("<%=hidProList.ClientID %>").value == "" && document.getElementById("<%=txtCUSTSN.ClientID %>").value == "") {
                alert("請至少輸入Model 或 CustSN...");
                return false;
            }
            else if (document.getElementById("<%=txtModel.ClientID %>").value != "" && (document.getElementById("<%=hidProList.ClientID %>").value != "" || document.getElementById("<%=txtCUSTSN.ClientID %>").value != ""))
            {
                alert("Model 或 CustSN 只能選一項輸入");
                return false;
            }
            else if (document.getElementById("<%=txtModel.ClientID %>").value != "") {
                document.getElementById("<%=hidIsCUSTSNValue.ClientID %>").value = "N";
                document.getElementById("<%=hidQueryValue.ClientID %>").value = document.getElementById("<%=txtModel.ClientID %>").value;
            }
            else if (document.getElementById("<%=hidProList.ClientID %>").value != "") {
                document.getElementById("<%=hidIsCUSTSNValue.ClientID %>").value = "Y";
                document.getElementById("<%=hidQueryValue.ClientID %>").value = document.getElementById("<%=hidProList.ClientID %>").value;
            }
            else if (document.getElementById("<%=txtCUSTSN.ClientID %>").value != "") {
                document.getElementById("<%=hidIsCUSTSNValue.ClientID %>").value = "Y"; 
                document.getElementById("<%=hidQueryValue.ClientID %>").value = document.getElementById("<%=txtCUSTSN.ClientID %>").value;
            }
            document.getElementById("<%=hideditor.ClientID %>").value = editor;
            document.getElementById("<%=hidcustomer.ClientID %>").value = customer;
            document.getElementById("<%=hidstationId.ClientID %>").value = stationId;
            gprodId = document.getElementById("<%=hidQueryValue.ClientID %>").value;
            ShowInfo("");
            return true;
        }

        function getcmbReleaseActionObj() {
            return document.getElementById("<%=cmbReleaseAction.ClientID %>");
        }

        function getcmbGotoStationObj() {
            return document.getElementById("<%=cmbGotoStation.ClientID %>");
        }
        
        function cmbReleaseActionChange() {
            if (getcmbReleaseActionObj().options[getcmbReleaseActionObj().selectedIndex].value == "GoToStation") {
                getcmbGotoStationObj().disabled = false;
            }
            else {
                getcmbGotoStationObj().selectedIndex = 0;
                document.getElementById("<%=lblUnpackStationValue.ClientID %>").innerHTML = "";
                getcmbGotoStationObj().disabled = true;
            }
        }

        function cmbGoToStationChange() {
            var unPackStation = getcmbGotoStationObj().options[getcmbGotoStationObj().selectedIndex].value;
            //lblUnpackStationValue
            document.getElementById("<%=lblUnpackStationValue.ClientID %>").innerHTML = unPackStation;
        }
        
        function setInfo(prodId, info)
        {
            //set defectCache value
            defectCache = info[2];
            if (info[5] != "" && info[5] != null) {
                alert(info[5]);
            }
        }

        function save() {
            var guid = document.getElementById("<%=hidGuidCode.ClientID %>").value;
            if (guid == "") {
                alert("沒有Hold Info...");
                return;
            }
            var key = document.getElementById("<%=hidGuidCode.ClientID %>").value;
            var releaseCode = getcmbReleaseDefectObj().options[getcmbReleaseDefectObj().selectedIndex].value;
            if (releaseCode == "") {
                alert("請選擇ReleaseDefect...");
                return;
            }
            var releaseAction = getcmbReleaseActionObj().options[getcmbReleaseActionObj().selectedIndex].value;
            var goToStation = getcmbGotoStationObj().options[getcmbGotoStationObj().selectedIndex].text;
            if (releaseAction == "GoToStation") {
                //getcmbGotoStationObj
                //goToStation = getcmbGotoStationObj().options[getcmbGotoStationObj().selectedIndex].text;
                if (goToStation == "") {
                    alert("請選擇GoToStation...");
                    return;
                }
            }

            WebServiceReleaseProductIDHold.save(key, releaseCode, goToStation, onSaveSucceeded, onSaveFailed);

        }

        function modelandcustsnList(modelList,CUSTSNList) {
            if (document.getElementById("<%=hidIsCUSTSNValue.ClientID %>").value == "Y") {
                showMsgList = CUSTSNList;
            }
            else {
                showMsgList = gprodId;
            }
        }

        function onSaveSucceeded(result) 
        {
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            var message = "[" + showMsgList + "] " + msgSuccess;
            ShowSuccessfulInfo(true, message, false);
            initPage();
        }

        function onSaveFailed(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            initPage();
        }
        
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0 ,false);
            inputFlag = false;
            checkPicdFlag = false;
            checkWwanFlag = false;
            defectCount = 0;
            defectInTable = [];
            document.getElementById("<%=txtModel.ClientID %>").value = "";
            document.getElementById("<%=hidProList.ClientID %>").value = "";
            document.getElementById("<%=txtCUSTSN.ClientID %>").value = "";
            document.getElementById("<%=cmbReleaseDefect.ClientID %>").value = "";
            document.getElementById("<%=hidGuidCode.ClientID %>").value = "";
            var releaseAction = getcmbReleaseActionObj().options[getcmbReleaseActionObj().selectedIndex].value;
            if (releaseAction == "ReleaseHoldStation") {
                getcmbGotoStationObj().disabled = true;
            }
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

