<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for QC Repair Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0432, Jessica Liu, 2012-2-15
* ITC-1360-1657, Jessica Liu, 2012-4-11
*/
 --%>
 <%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PAQCRepair.aspx.cs" Inherits="FA_PAQCRepair" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/PAQCRepairService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="FA"/>
                        <%--
                        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>                    
                                <asp:Label ID="lblPdLineName" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
                            </ContentTemplate>
                        </asp:UpdatePanel>
                         --%>
                    </td>
                </tr>            
                <tr>
                    <td style="width: 160px;">
                        <%--ITC-1360-1657, Jessica Liu, 2012-4-11--%>
                       <asp:Label ID="lblProdIDCustSN" runat="server" class="iMes_DataEntryLabel"></asp:Label> 
                    </td>
                    <td style="width: 35%;">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="90%" IsPaste="true" IsClear="true"/>
                    </td>
                    <td style="width: 130px;">
                        <asp:Label ID="lblTestStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                    
                    <td>
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
                <%-- 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件 --%>
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
                </Triggers>
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />                
                    <%-- Jessica Liu, 2012-4-17, old(GvExtHeight="320px" Height="310px")--%>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%" GvExtHeight="200px" Height="190px" OnRowDataBound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" OnGvExtRowDblClick="dblclickTable(this)">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="btnArea" style="text-align: right; margin-bottom: 5px;">
            <button id="btnAdd" runat="server" onclick="clkAdd()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled"></button>
            <button id="btnEdit" runat="server" onclick="clkEdit()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled"></button>
            <button id="btnDelete" runat="server" onclick="clkDelete()" style="display: none;"></button>
            <%--<button id="btnSave" runat="server" onclick="clkSave()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled"></button>--%>
            <input id="btnSave" type="button" value="Finish" onclick="clkSave()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="disabled" />
            <%-- 2012-7-6--%>
            <input id="Hidden1" type="hidden" />
            <button id="hidbtn" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_ServerClick"></button>
            <button id="hidRefresh" style="width: 0; display: none;" runat="server" onserverclick="hidRefresh_ServerClick"></button>
            <input type="hidden" id="hidProdId" runat="server" />
            <input type="hidden" id="hidPdLine" runat="server" />
            <%-- Jessica Liu, 2012-5-7, 新需求--%>
            <button id="hidbtnforexcel" style="width: 0; display: none;" runat="server" onserverclick="hidbtnforexcel_ServerClick"></button>
            <input id="hidParam0" type="hidden" runat="server" />
            <input id="hidParam1" type="hidden" runat="server" />
            <input id="hidParam2" type="hidden" runat="server" />
            <input id="hidParam3" type="hidden" runat="server" />
            <input id="hidParam4" type="hidden" runat="server" />
            <input id="hidParam5" type="hidden" runat="server" />
            <input id="hidParam6" type="hidden" runat="server" />
            <input id="hidParam7" type="hidden" runat="server" />
        </div>                
    </div> 
    
    <script language="javascript" type="text/javascript">
        var globalForSubPage;
        var feature = "dialogHeight:472px;dialogWidth:800px;center:yes;status:no;help:no";     //the style of pop up window
        var url = "PAQCRepairAddEdit.aspx";
        var status = "";      //add or edit
        var inputObj;
        var emptyPattern = /^\s*$/;
        var editObj;
        var inputFlag = false;
        var pdLineObj;
        var globalProID = "";
        var tblObj;
        var globalDefectID;
        var selectedRowIndex = -1;
        var cause = "";
        var editor;
        var customer;
        var stationId;
        //Jessica Liu, 2012-4-17
        var initRowsCount = 6;     //12;       
        var globalNewPartSno = "";

        var msgHasNotFinishRecord = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgHasNotFinishRecord").ToString() %>';

        //ITC-1360-0432, Jessica Liu, 2012-2-15
        var msgPdLineNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPdLineNull").ToString() %>';

        //2012-4-11
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        var msgSelectReturnStation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSelectReturnStation").ToString() %>';
        var returnStationValue = "";
        
        
        window.onload = function() {
            inputObj = getCommonInputObject();
            inputObj.focus(); //Dean 20110418
            pdLineObj = getPdLineCmbObj();
            tblObj = document.getElementById("<%=gd.ClientID %>");
            getAvailableData("processFun");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            //getPdLineCmbObj().disabled = true; //Dean 20110418
        }; 
        
        function processFun(backData)
        {
            //show wait?
            ShowInfo("");
            beginWaitingCoverDiv();

            //ITC-1360-0432, Jessica Liu, 2012-2-15
            if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
                endWaitingCoverDiv();

                alert(msgPdLineNull);

                //pdLineObj.focus();
                inputObj.value = "";
                getAvailableData("processFun");
                return;
            }
        
            document.getElementById("<%=hidProdId.ClientID %>").value = backData;
            //document.getElementById("<%=hidPdLine.ClientID %>").value = getPdLineCmbValue(); //Dean 20110314
            
            
            /*if (!checkPdLineCmb()) //Dean 20110314
            {
                endWaitingCoverDiv();
                pdLineObj.focus();
                inputObj.value = "";
                getAvailableData("processFun"); 
                return;
            }*/
            
            globalProID = backData;
            inputFlag = true;
            selectedRowIndex = -1;                        
            
            document.getElementById("<%=hidbtn.ClientID %>").click();
        }
        
        function clkAdd()
        {
            /* 2012-8-2
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            var retStation = getReturnStationCmbValue();
            if (!emptyPattern.test(retStation)) {
            returnStationValue = retStation;
            }
            else {
            ShowMessage(msgSelectReturnStation);
            ShowInfo(msgSelectReturnStation);
            getReturnStationCmbObj().focus();
            return;
            }
            */
            
            if (inputFlag)
            {
                status = "A";
                window.showModalDialog(url+"?Customer=" + "<%=Customer%>", window, feature);
                refreshTable();  
            }
        }
        
       function refreshTable()
        {
            selectedRowIndex = -1;
            enableOrDisableEdit();        
            document.getElementById("<%=hidRefresh.ClientID %>").click();
        }         
        
        function clkEdit()
        {
            /* 2012-8-2
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            var retStation = getReturnStationCmbValue();
            if (!emptyPattern.test(retStation))
            {
                returnStationValue = retStation;
            }
            else
            {
                ShowMessage(msgSelectReturnStation);
                ShowInfo(msgSelectReturnStation);
                getReturnStationCmbObj().focus();
                return;
            }
            */
            
            if (hasEditData() && !isMBChangePartRecord())
            {
                getRepairLogInfo();
            
                status = "E";
                window.showModalDialog(url + "?Customer=" + "<%=Customer%>", window, feature);
                refreshTable();
            }
        }
        
        function dblclickTable(con)
        {
            clkEdit();   
        }         
        
        function hasEditData()
        {
            tblObj = document.getElementById("<%=gd.ClientID %>");
        
            if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText))
            {
                return false;
            }
            
            return true;
        }          
        
        function clkDelete()
        {
        }
        
        function clkSave()
        {
            //2012-7-6
            document.getElementById("Hidden1").value = "1";
            
            if (inputFlag)
            {
                ShowInfo("");
                            
                //if there is no defect, show error message
                if (isFinishAllRepair())
                {
                    /* 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                    beginWaitingCoverDiv();
                    PAQCRepairService.save(globalProID, saveSucc, saveFail);
                    */
                    var returnStation = getReturnStationCmbValue();
                    if (!emptyPattern.test(returnStation)) 
                    {
                        beginWaitingCoverDiv();
                        var returnStationText = getReturnStationCmbText();
                        PAQCRepairService.save(globalProID, returnStation, returnStationText, saveSucc, saveFail);
                    }
                    else {
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
        
        /* 2012-7-5，新需求
        function saveSucc(result)
        {
            //show success message
            initPage();
            endWaitingCoverDiv();
            //getPdLineCmbObj().disabled = true; //Dean 20110418    
            
            //2012-4-11
            //ShowSuccessfulInfo(true);
            ShowSuccessfulInfo(true, "[" + document.getElementById("<%=hidProdId.ClientID %>").value + "] " + msgSuccess); 
            
            setCommonFocus();            
        }
        */
        function saveSucc(result) {
            try {
                //2012-7-6
                document.getElementById("Hidden1").value = "";
                
                //show success message
                initPage();
                endWaitingCoverDiv();

                if (result == null) {
                    ShowInfo("");
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                }
                //2012-7-18
                //else if ((result.length == 9) && (result[0] == SUCCESSRET))
                //2012-7-19
                //else if ((result.length == 12) && (result[0] == SUCCESSRET))
                else if ((result.length == 13) && (result[0] == SUCCESSRET)) 
                {
                    document.getElementById("<%=hidParam0.ClientID %>").value = result[1];
                    document.getElementById("<%=hidParam1.ClientID %>").value = result[2];
                    document.getElementById("<%=hidParam2.ClientID %>").value = result[3];
                    document.getElementById("<%=hidParam3.ClientID %>").value = result[4];
                    document.getElementById("<%=hidParam4.ClientID %>").value = result[5];
                    document.getElementById("<%=hidParam5.ClientID %>").value = result[6];
                    document.getElementById("<%=hidParam6.ClientID %>").value = result[7];
                    document.getElementById("<%=hidParam7.ClientID %>").value = result[8];

                    //2012-8-13，先去掉excel导出处理
                    //document.getElementById("<%=hidbtnforexcel.ClientID %>").click();


                    //getPdLineCmbObj().disabled = true; //Dean 20110418    

                    //2012-4-11
                    //ShowSuccessfulInfo(true);
                    //2012-7-18
                    //ShowSuccessfulInfo(true, "[" + document.getElementById("<%=hidProdId.ClientID %>").value + "] " + msgSuccess);
                    //2012-7-19
                    if (result[12] == "TRUE") {
                        var stationInfo = "CustSN：" + result[9] + " 下一站去 " + result[10] + "  " + result[11] + "!";
                        ShowSuccessfulInfo(true, stationInfo + "  [" + document.getElementById("<%=hidProdId.ClientID %>").value + "] " + msgSuccess);
                    }
                    else {
                        ShowSuccessfulInfo(true, "[" + document.getElementById("<%=hidProdId.ClientID %>").value + "] " + msgSuccess);
                    }
                    
                    setCommonFocus();
                    
                }
                else {
                    ShowInfo("");
                    var content1 = result[0];
                    ShowMessage(content1);
                    ShowInfo(content1);
                }
            } catch (e) {
                alert(e);
            }
        }
        
        
        function saveFail(result)
        {
            //2012-7-6
            document.getElementById("Hidden1").value = "";
            
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            initPage();
            endWaitingCoverDiv();
            //getPdLineCmbObj().disabled = true; //Dean 20110418            
            setCommonFocus();
        }                
        
        function getRepairLogInfo()
        {   
            editObj = new DefectInfo();
            tblObj = document.getElementById("<%=gd.ClientID %>");
            
            var valueArray = tblObj.rows[selectedRowIndex + 1].cells[5].innerText.split("\u0003");
            
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
//            editObj.testStation = tblObj.rows[selectedRowIndex].cells[2].innerText;
//            editObj.defectCodeID = tblObj.rows[selectedRowIndex].cells[3].innerText;
//            editObj.cause = tblObj.rows[selectedRowIndex].cells[4].innerText;   
            editObj.defectCodeID = valueArray[31];
            editObj.cause = valueArray[32];

            //2012-5-4
            editObj.location = valueArray[33];
            editObj.mtaID = valueArray[34];                  
  
            
            globalNewPartSno = valueArray[13];          
        }     
        
        function initPage()
        {
            inputFlag = false;
            inputObj.value = "";           
            globalProID = "";
            //pdLineObj.selectedIndex = -1;
            
            //return station
            document.getElementById("<%=lblTestStationContent.ClientID %>").innerText = "";

            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            document.getElementById("<%=lblModelContent.ClientID %>").innerText = "";
            
            clearData();
            //clear Table
            ClearGvExtTable("<%=gd.ClientID%>",initRowsCount);
            enableControls();
            disableButtons();            
            getAvailableData("processFun"); 
        }        
        
        function isFinishAllRepair()
        {
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            
            tblObj = document.getElementById("<%=gd.ClientID %>");
            
            for (var i = 0; i < recordCount; i++)
            {
                if (emptyPattern.test(tblObj.rows[i + 1].cells[2].innerText))
                {
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
                if (tblObj.rows[i + 1].cells[1].innerText.indexOf(defect) == 0)
                {
                    if (status == "E")
                    {
                        id = tblObj.rows[i + 1].cells[5].innerText.split("\u0003")[0];
                    
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
            //getPdLineCmbObj().disabled = true; //Dean 20110418            
            inputObj.focus();
            inputObj.select();
        }
        
        function clickTable(con)
        {
            //selectedRowIndex = parseInt(con.index, 10);
            
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gd.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            selectedRowIndex=parseInt(con.index, 10);
            
            enableOrDisableEdit();
        }           
        
        function disableControls(para)
        {
            //getPdLineCmbObj().disabled = true; //Dean 20110314
            inputObj.disabled = true;
            inputObj.value = para;
            enableButtons();
        }
        
        function enableControls()
        {
           // pdLineObj.disabled = false;//Dean 20110314
            inputObj.disabled = false;   
            inputObj.focus();         
        }
        
        function enableButtons()
        {
            if (inputFlag)
            {
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById('btnSave').disabled = false;                
            }
        }
        
        function disableButtons()
        {
            document.getElementById("<%=btnAdd.ClientID %>").disabled = true;
            document.getElementById("<%=btnEdit.ClientID %>").disabled = true;
            document.getElementById('btnSave').disabled = true;
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
        
            var rowArray = tblObj.rows[selectedRowIndex + 1].cells[5].innerText.split("\u0003");
            
            if ((rowArray[9] == "MB" || rowArray[9] == "KP" || rowArray[9] == "ME") && !emptyPattern.test(rowArray[13]))
            {
                return true;
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
                if (tblObj.rows[i + 1].cells[5].innerText.split("\u0003")[9] == "MB")
                {
                    if (status == "E")
                    {
                        id = tblObj.rows[i + 1].cells[5].innerText.split("\u0003")[0];
                    
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
        
        window.onbeforeunload = function ()
        {
            //2012-7-6
            if (document.getElementById("Hidden1").value == "") {
                OnCancel();
            }
        };   
        
        function OnCancel()
        {
            PAQCRepairService.cancel(globalProID);
        }
        
        function ExitPage(){
            OnCancel();
        }
        
        function ResetPage(){
            ExitPage();
            //pdLineObj.selectedIndex = -1;//Dean 20110314
            initPage();
            ShowInfo("");
        }           
        
                                                      
    </script>    
</asp:Content>

