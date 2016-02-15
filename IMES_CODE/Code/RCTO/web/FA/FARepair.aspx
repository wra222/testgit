
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FARepair.aspx.cs" Inherits="FA_FARepair" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/FARepairService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%">
                <tr>
                    <td style="width: 5%;"><asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width: 45%;" colspan="3"><iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%"/></td>
                </tr>
                <tr>
                    <td style="width: 5%;"><asp:Label ID="lblProdid" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
                    <td style="width: 45%;">
                        <iMES:Input ID="txtInput" runat="server" ProcessQuickInput="true" Width="98%" CanUseKeyboard="true" IsPaste="true" IsClear="true" />
                    </td>
                    <td style="width: 5%;">
                        <asp:Label ID="lblTestStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                    
                    <td style="width: 45%;">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>                    
                                <asp:Label ID="lblTestStationContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%;"><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width: 45%;">
                        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>                    
                                <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>                        
                    </td>
                    <td style="width:15%;"><asp:Label ID="lblReturnStation" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>                    
                    <td style="width: 35%;">
                        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>                                                    
                                <iMES:CmbReturnStation ID="CmbReturnStation" runat="server" Width="99%"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div style="padding-left: 2px;">
                <asp:Label ID="lblRepairLog" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            </div>
        </div>

        <div id="div2">
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="hidRefresh" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="hidCheck" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <input type="hidden" id="hidIsEOQC" runat="server" />  
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        GvExtWidth="100%" GvExtHeight="200px" OnRowDataBound="gd_RowDataBound" 
                        Height="190px" OnGvExtRowClick="clickTable(this)" 
                        OnGvExtRowDblClick="dblclickTable(this)" style="top: 0px; left: 29px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="btnArea">
            <table width="100%">
                <tr>                    
                    <td style="width: 140px; display: none;">&nbsp;</td>
                    <td style="width: 30%; display: none;">&nbsp;</td>                        
                    <td style="text-align: right;">
                        
                        <input id="btnAdd" type="button" style="width:15%" runat="server" class="iMes_button" 
                            onclick="clickAdd()" onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled" />
            
                        
                        
                        <button id="btnEdit" runat="server" onclick="clickEdit()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled"></button>
                        
                        <input id="btnSave" type="button" style="width:15%" runat="server" class="iMes_button" 
                            onclick="clkSave()" onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled" />
                        
                        
                        <button id="hidbtn" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_ServerClick"></button>
                        <button id="hidRefresh" style="width: 0; display: none;" runat="server" onserverclick="hidRefresh_ServerClick"></button>
                        <button id="hidCheck" style="width: 0; display: none;" runat="server" onserverclick="hidCheck_ServerClick"></button>
                        <input type="hidden" id="hidProdId" runat="server" />
                        <input type="hidden" id="hidPdLine" runat="server" />
                        <input type="hidden" id="hidMac" runat="server" />
                    </td>
                </tr>
            </table>         
        </div>        
    </div>
    
    <script language="javascript" type="text/javascript">
        var feature = "dialogHeight:392px;dialogWidth:800px;center:yes;status:no;help:no";     //the style of pop up window
        var url = "FARepairAddEdit.aspx?Customer=HP";
        var status = "";      //add or edit
        var inputObj;
        var emptyPattern = /^\s*$/;
        var editObj;
        var inputFlag = false;
        var pdLineObj;
        var globalProID = "";
        var inputProductID = "";
        var tblObj;
        var globalDefectID;
        var selectedRowIndex = -1;
        var cause = "";
        var editor;
        var customer;
        var stationId;
        var initRowsCount = 6;
        var globalNewPartSno = "";
        var returnStationObj;
        var mesInput = '<%=this.GetLocalResourceObject(Pre + "_mesInput").ToString()%>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var mac_value = "";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgNext = '<%=this.GetLocalResourceObject(Pre + "_msgNext").ToString()%>';

        
        
        //confirm message
        var msgConfirmDelete = '';
        
        var msgHasNotFinishRecord = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgHasNotFinishRecord").ToString() %>';
        var msgSelectReturnStation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSelectReturnStation").ToString() %>';
        var CauseCustomerID = "";

        window.onload = function() {
            inputObj = getCommonInputObject();
            inputObj.focus();
            pdLineObj = getPdLineCmbObj();
            tblObj = document.getElementById("<%=gd.ClientID %>");
            getAvailableData("processData");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"]%>';
            CauseCustomerID = '<%=Request["CauseCustomerID"]%>';
            
        }; 
        
        function processData(productID) {
            if (getPdLineCmbValue() == "") {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("processData");
                return;
            }
            if (!isProdIDorCustSN(productID, getPdLineCmbValue())) {
                alert(mesInput);
                getCommonInputObject().focus();
                getAvailableData("processData");
                return;
            }
            inputProductID = productID;
            globalProID = SubStringSN(productID, "ProdId");
            ShowInfo("");
            beginWaitingCoverDiv();
            document.getElementById("<%=hidProdId.ClientID %>").value = globalProID;
            document.getElementById("<%=hidPdLine.ClientID %>").value = getPdLineCmbValue();
            inputFlag = true;
            selectedRowIndex = -1;
            document.getElementById("<%=hidbtn.ClientID %>").click();
        }

        function inputFinish() {
            endWaitingCoverDiv();
            getPdLineCmbObj().disabled = true;
            inputObj.focus();
            inputObj.select();
            inputObj.disabled = true;
            inputObj.value = inputProductID;
            document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
            //document.getElementById("<%=btnEdit.ClientID %>").disabled = false;
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            //getAvailableData("processData");  
        }
        
        function clickAdd()
        {
            if (inputFlag)
            {
                status = "A";
                window.showModalDialog("FARepairAddEdit.aspx?Customer=" + customer, window, feature);
                refreshTable();  
            }
        }
        
        function refreshTable()
        {
            selectedRowIndex = -1;
            enableOrDisableEdit();        
            document.getElementById("<%=hidRefresh.ClientID %>").click();
        }        
        
        function clickEdit()
        {
            if (hasEditData() && !isMBChangePartRecord()) {
                getRepairLogInfo();
                status = "E";
             //   window.showModalDialog("FARepairAddEdit.aspx?Customer=" + customer, window, feature);
                window.showModalDialog("FARepairAddEdit.aspx?Customer=" + customer + "&CauseCustomerID=" + CauseCustomerID, window, feature);

                refreshTable();
            }
        }

        //ITC-1122-0020 Tong.Zhi-Yong 2010-02-02
        function dblclickTable(con)
        {
            clickEdit();   
        }
        
        function hasEditData()
        {
            tblObj = document.getElementById("<%=gd.ClientID %>");

            if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText)) {
                return false;
            }            
            return true;
        }       
        
        function clkSave()
        {
            if (inputFlag)
            {
                ShowInfo("");
                           
                //if there is no defect, show error message
                if (isFinishAllRepair())
                {                    
                    var returnStation = getReturnStationCmbValue();
                    
                    if (!emptyPattern.test(returnStation)) {
                        beginWaitingCoverDiv();
                        FARepairService.save(globalProID, returnStation, "0", saveSucc, saveFail);
                    }
                    else
                    {
                        ShowMessage(msgSelectReturnStation);
                        ShowInfo(msgSelectReturnStation);
                        getReturnStationCmbObj().focus();
                    }
                }
                else
                {
                    ShowMessage(msgHasNotFinishRecord);
                    ShowInfo(msgHasNotFinishRecord);                   
                }
            }
        }

        function checkFinish(par) {
            var returnStation = getReturnStationCmbValue();
            if (par == 1) {
                beginWaitingCoverDiv();
                FARepairService.save(globalProID, returnStation, "1", saveSucc, saveFail);
            }
            else {
                
                if (!emptyPattern.test(returnStation)) {
                    beginWaitingCoverDiv();
                    FARepairService.save(globalProID, returnStation, "0", saveSucc, saveFail);
                }
                else
                {
                        ShowMessage(msgSelectReturnStation);
                        ShowInfo(msgSelectReturnStation);
                        getReturnStationCmbObj().focus();
                }
            }
        }
        
        function saveSucc(result) {
            try {

                endWaitingCoverDiv();
                initPage();
                if (result == null) {
                    alert(msgSystemError);
                    getAvailableData("processData"); 
                }
                else if (result[0] == SUCCESSRET) {
                    
                    if (result[1][1] == "1") {
                        ShowInfo("CustSN:" + result[1][0] + " " + msgNext + " " + result[1][2] + " " + result[1][3]);
                    }
                    else {
                        var tmp = document.getElementById("<%=hidProdId.ClientID %>").value;
                        ShowSuccessfulInfo(true, "[" + tmp + "] " + msgSuccess);
                        setCommonFocus();
                    }
                }
                else {
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    inputObj.focus();
                }
            } catch (e) {
                initPage();
                alert(e.description);
            }     
        }
        
        function saveFail(result)
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            initPage();
            endWaitingCoverDiv();
            getPdLineCmbObj().disabled = true; //Dean 20110418
            setCommonFocus();
        }        
        
        function getRepairLogInfo()
        {   
            editObj = new DefectInfo();
            tblObj = document.getElementById("<%=gd.ClientID %>");
            
            var valueArray = tblObj.rows[selectedRowIndex + 1].cells[8].innerText.split("\u0003");
            
            editObj.id = valueArray[0];
            editObj.repairID = valueArray[1];
            editObj.type = valueArray[2];
            editObj.obligation = valueArray[3];
            editObj.component = valueArray[4];
            editObj.site = valueArray[5];
            editObj.majorPart = valueArray[6];
            editObj.remark = valueArray[7];
            editObj.vendorCT = valueArray[8];
            editObj.partType = valueArray[9];
            editObj.oldPart = valueArray[10];
            editObj.oldPartSno = valueArray[11];
            editObj.newPart = valueArray[12];
            editObj.newPartSno = valueArray[13];
            editObj.manufacture = valueArray[14];
            editObj.versionA = valueArray[15];
            editObj.versionB = valueArray[16];
            editObj.returnSign = valueArray[17];
            editObj.mark = valueArray[18];
            editObj.subDefect = valueArray[19];
            editObj.piaStation = valueArray[20];
            editObj.distribution = valueArray[21];
            editObj._4M = valueArray[22];
            editObj.responsibility = valueArray[23];
            editObj.action = valueArray[24];
            editObj.cover = valueArray[25];
            editObj.uncover = valueArray[26];
            editObj.trackingStatus = valueArray[27];
            editObj.isManual = valueArray[28];
            editObj.editor = valueArray[29]; 
            editObj.newPartDateCode = valueArray[30];   
            
//            editObj.pdLine = tblObj.rows[selectedRowIndex].cells[1].innerText;
            editObj.testStation = tblObj.rows[selectedRowIndex + 1].cells[2].innerText;
//            editObj.defectCodeID = tblObj.rows[selectedRowIndex].cells[3].innerText;
//            editObj.cause = tblObj.rows[selectedRowIndex].cells[4].innerText;   
            editObj.defectCodeID = valueArray[31];
            editObj.cause = valueArray[32];
            editObj.returnStation = valueArray[33];

            mac_value = valueArray[34];
            //editObj.mac = valueArray[34];

            editObj.location = valueArray[35];
            editObj.mtaID = valueArray[36];                  
            globalNewPartSno = valueArray[13];
        }                               
        
        function initPage()
        {
            inputFlag = false;
            inputObj.value = "";
            globalProID = "";
            
            //return station
            document.getElementById("<%=lblTestStationContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblModelContent.ClientID %>").innerText = "";
            //product iD  Dean 20110428
            getReturnStationCmbObj().value = "";
            //ITC-1360-1025
            clearData();
            //clear Table
            ClearGvExtTable("<%=gd.ClientID%>",initRowsCount);
            enableControls();
            disableButtons();
            getAvailableData("processData"); 
        } 
        
        function isFinishAllRepair()
        {
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            
            tblObj = document.getElementById("<%=gd.ClientID %>");
            
            for (var i = 0; i < recordCount; i++)
            {
                if (emptyPattern.test(tblObj.rows[i + 1].cells[4].innerText)) {
                    var cause = tblObj.rows[i + 1].cells[8].innerText.split("\u0003")[32];
                    if ('CN' != cause)
                        return false;
                }
            }
            
            return true;
        }        
        
        function isExistDefectInTable(defect, objId)
        {
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            var id;
            
            tblObj = document.getElementById("<%=gd.ClientID %>");
            
            for (var i = 0; i < recordCount; i++)
            {
                if (tblObj.rows[i + 1].cells[3].innerText.indexOf(defect) == 0)
                {
                    if (status == "E")
                    {
                        id = tblObj.rows[i + 1].cells[8].innerText.split("\u0003")[0];
                    
                        if (id != objId)
                        {
                            return true;
                        }
                    }
                    else 
                    {
                        return true;
                    }
                    
                }
            }
            
            return false;            
        }        
               
        function setCommonFocus()
        {
            endWaitingCoverDiv();
            inputObj.focus();
            inputObj.select();
        } 
        
        function clickTable(con)
        {            
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gd.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            selectedRowIndex=parseInt(con.index, 10);
            
            enableOrDisableEdit();
        }
        
        function enableControls()
        {
            pdLineObj.disabled = false;
            inputObj.disabled = false;
            inputObj.focus();        
        }
        
        function enableButtons()
        {
            if (inputFlag)
            {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;               
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;                
            }
        }
        
        function disableButtons()
        {
            document.getElementById("<%=btnAdd.ClientID %>").disabled = true;
            document.getElementById("<%=btnEdit.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        
        function enableOrDisableEdit()
        {
            if (hasEditData() && !isMBChangePartRecord())
            {
                document.getElementById("<%=btnEdit.ClientID %>").disabled = false;                
            }
            else 
            {
                document.getElementById("<%=btnEdit.ClientID %>").disabled = true;                
            }
        }
        
        function isMBChangePartRecord()
        {
            tblObj = document.getElementById("<%=gd.ClientID %>");
        
            var rowArray = tblObj.rows[selectedRowIndex + 1].cells[8].innerText.split("\u0003");

            if (rowArray[11] != "")
            {
                //return true;
            }
            
            return false;            
        }
        
        function hasMBRecord(objId)
        {
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            var id;
            
            tblObj = document.getElementById("<%=gd.ClientID %>");
            
            for (var i = 0; i < recordCount; i++)
            {
                if (tblObj.rows[i + 1].cells[8].innerText.split("\u0003")[9] == "MB")
                {
                    if (status == "E")
                    {
                        id = tblObj.rows[i + 1].cells[8].innerText.split("\u0003")[0];
                    
                        if (id != objId)
                        {
                            return true;
                        }
                    }
                    else 
                    {
                        return true;
                    }                    
                }
            }
            
            return false;             
        }

        function window.onbeforeunload() {
                FARepairService.wfcancel(globalProID);            
        }

                                                               
        
    </script>
</asp:Content>

